using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // A single noninteractive UI mesh per painting keeps the lighting in the artwork's coordinates.
    internal abstract class CaveLightMesh : MaskableGraphic
    {
        protected const float Cycle = 2f * Mathf.PI;
        protected float SceneTime { get; private set; }
        private float nextFrame;

        protected override void OnEnable()
        {
            base.OnEnable();
            raycastTarget = false;
            SceneTime = 0;
            nextFrame = 0;
        }

        protected virtual void Update()
        {
            SceneTime += Time.unscaledDeltaTime;
            if (SceneTime < nextFrame) return;
            nextFrame = SceneTime + 1f / 25f;
            SetVerticesDirty();
        }

        protected static Vector2 Orbit(float theta, float x, float y)
        {
            return new Vector2(Mathf.Cos(theta) * x, Mathf.Sin(theta) * y);
        }

        protected static Color Ink(Color color, float alpha)
        {
            color.a = Mathf.Clamp01(alpha);
            return color;
        }

        protected static void Quad(VertexHelper mesh, Vector2 a, Vector2 b, Vector2 c, Vector2 d,
            Color first, Color last)
        {
            int i = mesh.currentVertCount;
            mesh.AddVert(a, first, Vector2.zero);
            mesh.AddVert(b, first, Vector2.zero);
            mesh.AddVert(c, last, Vector2.zero);
            mesh.AddVert(d, last, Vector2.zero);
            mesh.AddTriangle(i, i + 1, i + 2);
            mesh.AddTriangle(i, i + 2, i + 3);
        }

        protected static void Stroke(VertexHelper mesh, Vector2 a, Vector2 b, float width, Color color)
        {
            Vector2 axis = b - a;
            Vector2 normal = new Vector2(-axis.y, axis.x).normalized * (width * .5f);
            Quad(mesh, a - normal, a + normal, b + normal, b - normal, color, color);
        }

        protected static void Glow(VertexHelper mesh, Vector2 center, float rx, float ry, Color color, int sides = 16)
        {
            int first = mesh.currentVertCount;
            mesh.AddVert(center, color, Vector2.zero);
            for (int i = 0; i < sides; ++i)
                mesh.AddVert(center + Orbit(Cycle * i / sides, rx, ry), Ink(color, 0), Vector2.zero);
            for (int i = 0; i < sides; ++i)
                mesh.AddTriangle(first, first + 1 + i, first + 1 + (i + 1) % sides);
        }

        protected static void Spark(VertexHelper mesh, Vector2 p, float r, Color color)
        {
            Glow(mesh, p, r * 2.4f, r * 2.4f, Ink(color, color.a * .4f), 10);
            Glow(mesh, p, r * 1.25f, r * .26f, color, 4);
            Glow(mesh, p, r * .27f, r * 1.6f, color, 4);
        }

        protected static void Arc(VertexHelper mesh, Vector2 center, float rx, float ry,
            float start, float sweep, float width, Color color, float tilt = 0)
        {
            float cos = Mathf.Cos(tilt), sin = Mathf.Sin(tilt);
            int steps = Mathf.Clamp(Mathf.CeilToInt(Mathf.Abs(sweep) * 9), 3, 64);
            Vector2 previous = default;
            for (int i = 0; i <= steps; ++i)
            {
                Vector2 point = Orbit(start + sweep * i / steps, rx, ry);
                point = center + new Vector2(cos * point.x - sin * point.y, sin * point.x + cos * point.y);
                if (i > 0) Stroke(mesh, previous, point, width, color);
                previous = point;
            }
        }
    }
}
