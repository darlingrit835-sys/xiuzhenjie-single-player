using UnityEngine;
using UnityEngine.Rendering;

namespace XiuZhenJie
{
    // The gameplay camera retains its full-size screen projection. A child renders just the world
    // when frame pacing needs relief; the existing UI camera and overlay canvases still draw afterwards.
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(900)]
    public sealed class BattleFieldWorldScale : MonoBehaviour
    {
        const int MaxWorldPixels = 1250000;
        Camera eye, scenePass;
        CommandBuffer present;
        RenderTexture target;
        int screenWidth, screenHeight, frames, slowWindows, fastWindows, savedMask;
        float seconds, holdUntil;
        bool duringPresent, triedRecovery;

        public bool Reduced => target != null;
        public int WorldWidth => target ? target.width : eye ? eye.pixelWidth : 0;
        public int WorldHeight => target ? target.height : eye ? eye.pixelHeight : 0;
        public RenderTexture WorldTexture => target;

        void Awake() => eye = GetComponent<Camera>();

        void OnEnable()
        {
            triedRecovery = false;
            ResetSampling(1f);
        }

        void ResetSampling(float delay)
        {
            seconds = 0;
            frames = slowWindows = fastWindows = 0;
            holdUntil = Time.unscaledTime + delay;
        }

        void LateUpdate()
        {
            if (!Application.isMobilePlatform) return;
            if (!eye.enabled || eye.targetTexture || eye.rect != new Rect(0, 0, 1, 1))
            {
                if (Reduced) RestoreNative();
                ResetSampling(1f);
                return;
            }

            int width = eye.pixelWidth, height = eye.pixelHeight;
            if (width != screenWidth || height != screenHeight)
            {
                screenWidth = width;
                screenHeight = height;
                // Keep the full frame visible throughout a rotation and wait for the new viewport to settle.
                if (Reduced) RestoreNative();
                ResetSampling(.7f);
            }
            if (scenePass)
            {
                if (!target.IsCreated() && !target.Create())
                {
                    RestoreNative();
                    ResetSampling(8f);
                    return;
                }
                if (scenePass.cullingMask != eye.cullingMask) scenePass.cullingMask = eye.cullingMask;
                if (scenePass.orthographicSize != eye.orthographicSize) scenePass.orthographicSize = eye.orthographicSize;
                if (scenePass.aspect != eye.aspect) scenePass.aspect = eye.aspect;
                if (scenePass.backgroundColor != eye.backgroundColor) scenePass.backgroundColor = eye.backgroundColor;
            }
            // Cap a single stall's influence, while still recognising consistently very slow frames.
            float dt = Mathf.Min(Time.unscaledDeltaTime, .125f);
            if (Time.unscaledTime < holdUntil || dt <= 0f || width < 2 || height < 2) return;
            seconds += dt;
            frames++;
            if (seconds < 1f) return;
            float average = seconds / frames;
            seconds = 0;
            frames = 0;
            float budget = 1f / Mathf.Clamp(Application.targetFrameRate > 0 ? Application.targetFrameRate : 30, 15, 60);
            if (!Reduced)
            {
                slowWindows = average > Mathf.Max(.0235f, budget * 1.035f) ? slowWindows + 1 : 0;
                if (slowWindows >= 2)
                {
                    if (TryReduce(width, height)) ResetSampling(4f);
                    else ResetSampling(10f);
                }
            }
            else if (!triedRecovery)
            {
                // A vsync-capped frame cannot prove spare GPU time. Probe native only once after
                // a long steady interval; if it slows again, keep the reduced setting for this field.
                fastWindows = average <= budget * 1.035f ? fastWindows + 1 : 0;
                if (fastWindows >= 40)
                {
                    triedRecovery = true;
                    RestoreNative();
                    ResetSampling(2f);
                }
            }
        }

        bool TryReduce(int width, int height)
        {
            float scale = Mathf.Min(.75f, Mathf.Sqrt(MaxWorldPixels / ((float)width * height)));
            scale = Mathf.Min(scale, (float)SystemInfo.maxTextureSize / Mathf.Max(width, height));
            int w = Mathf.Max(2, Mathf.FloorToInt(width * scale * .5f) * 2);
            int h = Mathf.Max(2, Mathf.FloorToInt(height * scale * .5f) * 2);
            target = new RenderTexture(w, h, 16, RenderTextureFormat.ARGB32)
            {
                name = "战场内部分辨率", filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp,
                antiAliasing = 1, useMipMap = false, autoGenerateMips = false, memorylessMode = RenderTextureMemoryless.Depth
            };
            if (!target.Create())
            {
                RestoreNative();
                return false;
            }

            var source = new GameObject("战场世界渲染");
            source.transform.SetParent(eye.transform, false);
            scenePass = source.AddComponent<Camera>();
            scenePass.CopyFrom(eye);
            source.transform.localPosition = Vector3.zero;
            source.transform.localRotation = Quaternion.identity;
            scenePass.targetTexture = target;
            scenePass.rect = new Rect(0, 0, 1, 1);
            scenePass.aspect = eye.aspect;
            scenePass.depth = eye.depth - 1f;
            scenePass.allowHDR = false;
            scenePass.allowMSAA = false;
            scenePass.eventMask = 0;
            scenePass.enabled = true;
            present = new CommandBuffer { name = "战场画面合成" };
            present.Blit(target, BuiltinRenderTextureType.CameraTarget);
            eye.AddCommandBuffer(CameraEvent.AfterEverything, present);
            return true;
        }

        void OnPreCull()
        {
            if (!Reduced || duringPresent) return;
            savedMask = eye.cullingMask;
            eye.cullingMask = 0;
            duringPresent = true;
        }

        void OnPostRender() => RestoreMask();

        void RestoreMask()
        {
            if (duringPresent && eye) eye.cullingMask = savedMask;
            duringPresent = false;
        }

        void RestoreNative()
        {
            RestoreMask();
            if (present != null)
            {
                if (eye) eye.RemoveCommandBuffer(CameraEvent.AfterEverything, present);
                present.Release();
                present = null;
            }
            if (scenePass)
            {
                scenePass.enabled = false;
                scenePass.targetTexture = null;
                Destroy(scenePass.gameObject);
                scenePass = null;
            }
            if (target)
            {
                target.Release();
                Destroy(target);
                target = null;
            }
        }

        void OnApplicationPause(bool paused)
        {
            if (paused) RestoreNative();
            ResetSampling(1f);
        }

        void OnDisable() => RestoreNative();
    }
}
