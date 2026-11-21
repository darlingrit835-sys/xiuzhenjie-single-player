using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class CaveResidenceLights : CaveLightMesh
    {
        private Vector2[] landmarks;
        private static readonly Color Gold = new Color32(255, 222, 132, 255);
        private static readonly Color Jade = new Color32(167, 245, 203, 255);
        private static readonly Color Fire = new Color32(255, 159, 57, 255);

        internal void SetLandmarks(Vector2[] locations)
        {
            landmarks = locations;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (landmarks == null) return;
            Rect rect = rectTransform.rect;
            float w = rect.width;
            for (int i = 0; i < landmarks.Length; ++i)
            {
                Vector2 uv = landmarks[i];
                Vector2 p = new Vector2(rect.xMin + w * uv.x, rect.yMax - rect.height * uv.y);
                float breath = .5f + .5f * Mathf.Sin(SceneTime * 1.6f + i);
                switch (i)
                {
                    case 0: // Golden tracery in the circular bronze vault door.
                        Glow(mesh, p, w * .061f, w * .065f, Ink(Gold, .23f + .14f * breath));
                        Arc(mesh, p, w * .063f, w * .066f, SceneTime * .35f, 1.5f, w * .0017f, Ink(Gold,.77f));
                        Spark(mesh, p, w * (.006f + .004f * breath), Gold);
                        break;
                    case 2: // Light follows the meditation terrace in perspective.
                        float wave = Mathf.Repeat(SceneTime * .36f, 1);
                        Arc(mesh, p, w * (.022f + wave * .068f), w * (.004f + wave * .012f),
                            0, Cycle, w * .002f, Ink(Jade, .72f * (1 - wave)));
                        break;
                    case 3: // Furnace flames and rising sparks.
                        Glow(mesh, p, w * .032f, w * (.024f + .012f * breath), Ink(Fire, .77f));
                        for (int k = 0; k < 7; ++k)
                        {
                            float age = Mathf.Repeat(SceneTime * .32f + k * .382f, 1);
                            Vector2 drift = new Vector2(Mathf.Sin(k * 12.2f + age * 5) * (.006f + age * .016f), age * .085f);
                            Spark(mesh, p + drift * w, w * .003f, Ink(Gold, Mathf.Sin(age * Mathf.PI)));
                        }
                        break;
                    case 5: // Fireflies along the irrigation terraces.
                        for (int k = 0; k < 8; ++k)
                        {
                            float phase = SceneTime * .65f + k * 2.4f;
                            Vector2 drift = new Vector2(Mathf.Sin(k * 1.9f) * .125f + Mathf.Sin(phase) * .014f,
                                Mathf.Cos(k * 3.4f) * .039f + Mathf.Cos(phase) * .012f);
                            Spark(mesh, p + w * drift, w * .004f, Ink(Jade, .52f + .42f * Mathf.Sin(phase)));
                        }
                        break;
                    default:
                        Glow(mesh, p, w * .049f, w * .025f, Ink(Gold, .14f + .14f * breath));
                        break;
                }
            }
        }
    }
}
