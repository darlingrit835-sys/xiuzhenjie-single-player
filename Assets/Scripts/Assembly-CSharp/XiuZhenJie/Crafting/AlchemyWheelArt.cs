using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // All dial angles use turns measured clockwise from noon, including the displayed hit window.
    internal sealed class AlchemyWheelArt : AlchemyDialMesh
    {
        private enum Mode { Idle, Wheel, Success, Failure }
        private static readonly Color[] StageTints = {
            new Color32(255, 158, 83, 255), new Color32(111, 232, 185, 255),
            new Color32(119, 203, 245, 255), new Color32(255, 213, 117, 255)
        };

        private Mode mode;
        private int stage;
        private float pointer, target, halfWindow, score, hitAngle, hitTime = -10, resultTime, onset, nextTick;
        private int lastFire = -1, lastFlare = -1;
        private Image furnace;
        private RawImage fire, flare;

        internal static AlchemyWheelArt Attach(RectTransform area)
        {
            RectTransform group = Layer(area, "丹炉星盘");
            Fill(group);
            var plate = Layer(group, "玄铜刻盘").gameObject.AddComponent<AlchemyDialPlate>();
            Fill(plate.rectTransform);
            plate.raycastTarget = false;

            var cauldron = Layer(group, "双耳青铜炉").gameObject.AddComponent<Image>();
            cauldron.sprite = ReleasedResources.Load<Sprite>("InkInterface/Professions/LianDan");
            cauldron.preserveAspect = true;
            cauldron.raycastTarget = false;
            var atlas = ReleasedResources.Load<Texture2D>("InkInterface/AlchemyFireFrames");
            RawImage hearth = FireLayer(group, "地火逐帧", atlas);
            RawImage spiral = FireLayer(group, "聚火逐帧", atlas);
            var art = Layer(group, "火候与丹光").gameObject.AddComponent<AlchemyWheelArt>();
            Fill(art.rectTransform);
            art.furnace = cauldron;
            art.fire = hearth;
            art.flare = spiral;
            art.raycastTarget = false;
            art.onset = Time.unscaledTime;
            art.SetIdle();
            art.Layout();
            return art;
        }

        private static RectTransform Layer(RectTransform parent, string label)
        {
            var obj = new GameObject(label, typeof(RectTransform), typeof(CanvasRenderer));
            obj.layer = parent.gameObject.layer;
            var rect = (RectTransform)obj.transform;
            rect.SetParent(parent, false);
            return rect;
        }

        private static void Fill(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
        }

        private static RawImage FireLayer(RectTransform parent, string label, Texture2D texture)
        {
            var image = Layer(parent, label).gameObject.AddComponent<RawImage>();
            image.texture = texture;
            image.raycastTarget = false;
            image.enabled = texture != null;
            return image;
        }

        private void Layout()
        {
            if (furnace == null) return;
            float radius = Radius;
            furnace.rectTransform.sizeDelta = Vector2.one * (1.05f * radius);
            furnace.rectTransform.anchoredPosition = Vector2.down * (.057f * radius);
            fire.rectTransform.sizeDelta = new Vector2(.90f, .54f) * radius;
            fire.rectTransform.anchoredPosition = Vector2.down * (.40f * radius);
            flare.rectTransform.sizeDelta = Vector2.one * (1.15f * radius);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Layout();
        }

        internal void SetIdle()
        {
            mode = Mode.Idle;
            stage = 0;
            hitTime = -10;
            nextTick = 0;
            SetVerticesDirty();
        }

        internal void SetWheel(int stage, float pointerTurns, float targetTurns, float halfWindowTurns)
        {
            mode = Mode.Wheel;
            this.stage = Mathf.Clamp(stage, 0, StageTints.Length - 1);
            pointer = Mathf.Repeat(pointerTurns, 1);
            target = Mathf.Repeat(targetTurns, 1);
            halfWindow = Mathf.Clamp(halfWindowTurns, 0, .5f);
            SetVerticesDirty();
        }

        internal void MarkHit(float accuracy01)
        {
            score = Mathf.Clamp01(accuracy01);
            hitTime = Time.unscaledTime;
            hitAngle = pointer;
            nextTick = 0;
            SetVerticesDirty();
        }

        internal void SetResult(bool success)
        {
            mode = success ? Mode.Success : Mode.Failure;
            resultTime = Time.unscaledTime;
            hitTime = -10;
            nextTick = 0;
            SetVerticesDirty();
        }

        private void Update()
        {
            float now = Time.unscaledTime;
            if (now < nextTick || fire == null) return;
            nextTick = now + 1f / 30f;
            float t = now - onset;
            int frame = Mathf.FloorToInt(t * (mode == Mode.Wheel ? 13 + stage : 10)) % 16;
            float heat = mode == Mode.Failure ? .18f : mode == Mode.Idle ? .70f : 1f;
            SetFrame(fire, frame, ref lastFire);
            fire.color = new Color(1, .92f, .84f, heat);
            float flareAge = now - (mode == Mode.Success ? resultTime : hitTime);
            bool show = flare.texture != null && flareAge < .86f && (mode == Mode.Success || score > .25f);
            flare.enabled = show;
            if (show)
            {
                SetFrame(flare, 16 + Mathf.Clamp((int)(flareAge * 16 / .86f), 0, 15), ref lastFlare);
                flare.color = new Color(1, .92f, .74f, Mathf.Sin(Mathf.PI * flareAge / .86f) * .85f);
            }
            var bronze = mode == Mode.Failure ? new Color(.64f, .67f, .66f) : Color.white;
            furnace.color = Color.Lerp(bronze, new Color(1, .86f, .66f), heat * (.16f + .13f * Mathf.Sin(t * 5.2f)));
            SetVerticesDirty();
        }

        private static void SetFrame(RawImage image, int frame, ref int previous)
        {
            if (previous == frame) return;
            previous = frame;
            // A half texel inset keeps neighbouring cels out of bilinear sampling.
            image.uvRect = new Rect(((frame % 4) * 128 + .5f) / 512,
                ((7 - frame / 4) * 128 + .5f) / 1024, 127f / 512, 127f / 1024);
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (!Begin(mesh)) return;
            float now = Time.unscaledTime;
            float time = now - onset;
            Color tint = mode == Mode.Failure ? new Color32(155, 139, 157, 255) : StageTints[stage];
            DrawVapour(time, tint);
            if (mode == Mode.Wheel) DrawWheel();
            float age = now - hitTime;
            if (age >= 0 && age < .72f)
            {
                float pulse = age / .72f;
                Color gleam = score >= .60f ? Gold : new Color32(230, 107, 85, 255);
                Band(.84f + .12f * pulse, .009f * (1 - pulse), hitAngle - .10f * pulse, .20f * pulse, Alpha(gleam, 1 - pulse));
                Glow(Ray(hitAngle) * .865f, .055f + .11f * pulse, Alpha(gleam, .8f * (1 - pulse)));
            }
            if (mode == Mode.Success || mode == Mode.Failure) DrawResult(now - resultTime);
        }

        private void DrawWheel()
        {
            // The two bright gate posts coincide with the exact edge used by the timing calculation.
            Band(.853f, .083f, target - halfWindow, 2 * halfWindow, new Color32(66, 185, 149, 255));
            Band(.885f, .013f, target - halfWindow, 2 * halfWindow, new Color32(191, 255, 215, 255));
            for (int side = -1; side <= 1; side += 2)
            {
                Vector2 axis = Ray(target + side * halfWindow);
                Line(axis * .794f, axis * .914f, .013f, Gold);
            }
            Vector2 lockAxis = Ray(target);
            Diamond(lockAxis * .765f, lockAxis, .024f, .014f, Gold);
            Vector2 tip = Ray(pointer);
            Color lamp = new Color32(255, 250, 224, 255);
            Glow(tip * .916f, .085f, Alpha(Gold, .62f));
            Vector2 tangent = new Vector2(tip.y, -tip.x);
            Triangle(tip * .792f, tip * .966f - tangent * .050f, tip * .966f + tangent * .050f, new Color32(67, 41, 30, 255));
            Triangle(tip * .805f, tip * .955f - tangent * .034f, tip * .955f + tangent * .034f, lamp);
        }

        private void DrawVapour(float time, Color tint)
        {
            // Continuous curling trails rise from the lid, with distinct coiling, drifting and gathering motion.
            for (int ribbon = 0; ribbon < 2; ribbon++)
            {
                Vector2 previous = new Vector2(0, .37f);
                for (int i = 1; i <= 14; i++)
                {
                    float y = i / 14f;
                    float wave = Mathf.Sin(y * (6.4f + stage * .5f) - time * (1.7f + stage * .2f) + ribbon * 2.8f);
                    Vector2 point = new Vector2(wave * (.012f + y * .13f), .37f + .34f * y);
                    Line(previous, point, .006f + .012f * y, Alpha(tint, .34f * Mathf.Sin(Mathf.PI * y)));
                    previous = point;
                }
            }

            for (int i = 0; i < 14; i++)
            {
                float seed = Mathf.Repeat((i + 1) * .618034f, 1);
                float part = Mathf.Repeat(time * (.21f + seed * .09f) + i * .382f, 1);
                Vector2 point;
                if (mode == Mode.Wheel && stage == 1)
                    point = Ray(part * .8f + seed) * (.65f - part * .57f) + Vector2.up * (.30f * part);
                else if (mode == Mode.Wheel && stage >= 2)
                    point = new Vector2(Mathf.Cos((part + seed) * 6.28f) * (.58f - .29f * part), -.42f + .99f * part);
                else
                    point = new Vector2((seed - .5f) * 1.16f + .05f * Mathf.Sin(part * 8 + i), -.54f + .99f * part);
                float intensity = Mathf.Sin(Mathf.PI * part);
                Diamond(point, Vector2.up, .012f + .012f * seed, .008f, Alpha(tint, .86f * intensity));
            }
        }

        private void DrawResult(float age)
        {
            float rise = 1 - Mathf.Exp(-age * 3.4f);
            if (mode == Mode.Success)
            {
                Vector2 pill = Vector2.up * (.28f + .29f * rise + .012f * Mathf.Sin(age * 2.3f));
                Glow(pill, .255f, Alpha(Gold, .78f));
                Glow(pill, .085f, new Color32(255, 169, 77, 255), new Color32(163, 67, 46, 255));
                Glow(pill + new Vector2(-.024f, .027f), .050f, new Color32(255, 253, 205, 255));
                for (int i = 0; i < 6; i++)
                {
                    Vector2 axis = Ray(i / 6f + age * .06f);
                    Diamond(pill + axis * .127f, axis, .013f, .006f, Alpha(Gold, .75f));
                }
            }
            else if (age < 1.8f)
            {
                float part = age / 1.8f;
                for (int i = 0; i < 9; i++)
                {
                    Vector2 dust = new Vector2((i - 4) * .067f * rise, .29f + (.11f + .07f * (i % 3)) * rise - part * part * .40f);
                    Glow(dust, .042f + .031f * part, new Color(.44f, .43f, .45f, (1 - part) * .75f));
                }
            }
        }
    }

    internal sealed class AlchemyDialPlate : AlchemyDialMesh
    {
        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (!Begin(mesh)) return;
            Color brass = new Color32(163, 123, 72, 255);
            Color brassShade = new Color32(83, 75, 57, 255);
            Glow(Vector2.zero, .985f, new Color32(50, 72, 66, 255), new Color32(10, 26, 32, 255));
            Glow(Vector2.down * .33f, .46f, new Color32(167, 90, 36, 180));
            Band(.970f, .013f, 0, 1, brass);
            Band(.937f, .005f, 0, 1, brassShade);
            Band(.853f, .09f, 0, 1, new Color32(51, 60, 58, 255));
            Band(.899f, .011f, 0, 1, brass);
            Band(.797f, .009f, 0, 1, brass);
            Band(.726f, .007f, 0, 1, brassShade);
            for (int i = 0; i < 60; i++)
            {
                Vector2 direction = Ray(i / 60f);
                bool major = i % 5 == 0;
                Line(direction * (major ? .906f : .914f), direction * .934f, major ? .010f : .006f, major ? Gold : brass);
            }
            // Paired constellation arcs form the inner engraving on a bronze divination instrument.
            for (int i = 0; i < 8; i++)
            {
                float start = i / 8f;
                Band(.692f, .009f, start + .009f, .103f, brassShade);
                Band(.670f, .006f, start + .026f, .069f, brassShade);
                var axis = Ray(start);
                Diamond(axis * .703f, axis, .036f, .018f, brass);
            }
        }
    }

    // Normalised geometry keeps the cauldron, target posts and pointer registered at any aspect ratio.
    internal abstract class AlchemyDialMesh : MaskableGraphic
    {
        protected static readonly Color Gold = new Color32(251, 218, 148, 255);
        protected float Radius => Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) * .5f;
        private Vector2 origin;
        private float scale;
        private VertexHelper vertices;

        protected bool Begin(VertexHelper mesh)
        {
            origin = rectTransform.rect.center;
            scale = Radius;
            vertices = mesh;
            return scale > 0;
        }

        protected static Vector2 Ray(float turns)
        {
            float radians = turns * (2 * Mathf.PI);
            return new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
        }

        protected static Color Alpha(Color color, float alpha) { color.a = alpha; return color; }

        private void Vert(Vector2 p, Color tint) => vertices.AddVert(origin + p * scale, tint, Vector2.zero);

        protected void Triangle(Vector2 a, Vector2 b, Vector2 c, Color tint)
        {
            int v = vertices.currentVertCount;
            Vert(a, tint); Vert(b, tint); Vert(c, tint);
            vertices.AddTriangle(v, v + 1, v + 2);
        }

        protected void Diamond(Vector2 at, Vector2 axis, float length, float width, Color tint)
        {
            Vector2 side = new Vector2(axis.y, -axis.x) * width;
            int v = vertices.currentVertCount;
            Vert(at + axis * length, tint); Vert(at + side, tint); Vert(at - axis * length, tint); Vert(at - side, tint);
            vertices.AddTriangle(v, v + 1, v + 2);
            vertices.AddTriangle(v, v + 2, v + 3);
        }

        protected void Line(Vector2 a, Vector2 b, float width, Color tint)
        {
            Vector2 delta = (b - a).normalized;
            Vector2 side = new Vector2(delta.y, -delta.x) * (width * .5f);
            int v = vertices.currentVertCount;
            Vert(a + side, tint); Vert(b + side, tint); Vert(b - side, tint); Vert(a - side, tint);
            vertices.AddTriangle(v, v + 1, v + 2);
            vertices.AddTriangle(v, v + 2, v + 3);
        }

        protected void Band(float radius, float width, float start, float span, Color tint)
        {
            int parts = Mathf.Max(1, Mathf.CeilToInt(Mathf.Abs(span) * 104));
            int first = vertices.currentVertCount;
            for (int i = 0; i <= parts; i++)
            {
                Vector2 direction = Ray(start + span * i / parts);
                Vert(direction * (radius - width * .5f), tint);
                Vert(direction * (radius + width * .5f), tint);
                if (i == 0) continue;
                int v = first + i * 2;
                vertices.AddTriangle(v - 2, v - 1, v + 1);
                vertices.AddTriangle(v - 2, v + 1, v);
            }
        }

        protected void Glow(Vector2 at, float radius, Color center) => Glow(at, radius, center, Alpha(center, 0));

        protected void Glow(Vector2 at, float radius, Color center, Color edge)
        {
            int first = vertices.currentVertCount;
            Vert(at, center);
            int parts = radius >= .4f ? 72 : 24;
            for (int i = 0; i < parts; i++)
            {
                Vert(at + Ray((float)i / parts) * radius, edge);
                vertices.AddTriangle(first, first + 1 + i, first + 1 + (i + 1) % parts);
            }
        }
    }
}
