using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // One masked UI mesh per selected row. Only a freshly checked item animates.
    internal sealed class RingJadeFrame : Image
    {
        private static readonly Color32 Jade = new Color32(52, 95, 79, 255);
        private static readonly Color32 Gold = new Color32(164, 121, 52, 255);
        private static readonly Color32 FineGold = new Color32(192, 151, 75, 165);
        private static readonly Vector2[] CloudCorner = {
            new Vector2(1.8f, 8.5f), new Vector2(1.8f, 2.0f),
            new Vector2(13.2f, 2.0f), new Vector2(13.2f, 4.8f),
            new Vector2(8.7f, 4.8f), new Vector2(8.7f, 3.7f)
        };

        private Coroutine glint;
        private float travel = -1f;

        internal void Flash()
        {
            if (!isActiveAndEnabled) return;
            StopFlash();
            glint = StartCoroutine(Glint());
        }

        internal void StopFlash()
        {
            if (glint != null) StopCoroutine(glint);
            glint = null;
            if (travel < 0f) return;
            travel = -1f;
            SetVerticesDirty();
        }

        protected override void OnDisable()
        {
            StopFlash();
            base.OnDisable();
        }

        private IEnumerator Glint()
        {
            const float duration = .42f;
            float start = Time.unscaledTime;
            while (Time.unscaledTime - start < duration)
            {
                travel = Mathf.Clamp01((Time.unscaledTime - start) / duration);
                SetVerticesDirty();
                yield return null;
            }
            travel = -1f;
            glint = null;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            Rect rect = GetPixelAdjustedRect();
            if (rect.width < 34f || rect.height < 20f) return;
            Vector2 low = rect.min + Vector2.one * .65f;
            Vector2 high = rect.max - Vector2.one * .65f;
            Vector2 topLeft = new Vector2(low.x, high.y);
            Vector2 bottomRight = new Vector2(high.x, low.y);
            Line(mesh, low, topLeft, 1.3f, Jade);
            Line(mesh, topLeft, high, 1.3f, Jade);
            Line(mesh, high, bottomRight, 1.3f, Jade);
            Line(mesh, bottomRight, low, 1.3f, Jade);

            for (int side = 0; side < 4; side++)
            {
                bool left = (side & 1) == 0, bottom = (side & 2) == 0;
                Vector2 origin = new Vector2(left ? rect.xMin : rect.xMax, bottom ? rect.yMin : rect.yMax);
                Vector2 direction = new Vector2(left ? 1 : -1, bottom ? 1 : -1);
                Vector2 from = origin + Vector2.Scale(CloudCorner[0], direction);
                for (int i = 1; i < CloudCorner.Length; i++)
                {
                    Vector2 to = origin + Vector2.Scale(CloudCorner[i], direction);
                    Line(mesh, from, to, 1.3f, Gold);
                    from = to;
                }
            }
            for (int edge = 0; edge < 2; edge++)
            {
                float y = edge == 0 ? rect.yMin + 2f : rect.yMax - 2f;
                float left = rect.xMin + 14f, right = rect.xMax - 14f;
                Line(mesh, new Vector2(left, y), new Vector2(right, y), .7f, FineGold);
                if (travel >= 0f) Shine(mesh, left, right, y);
            }
        }

        // The glint follows the gold edging. Its soft tail and bright tip stay off the text.
        private void Shine(VertexHelper mesh, float left, float right, float y)
        {
            float length = Mathf.Min(38f, (right - left) * .26f);
            float tip = Mathf.Lerp(left, right + length, travel);
            float fade = Mathf.Clamp01(Mathf.Min(travel, 1f - travel) * 8f);
            float tail = Mathf.Max(left, tip - length);
            float head = Mathf.Min(right, tip);
            if (head <= tail) return;
            var light = new Color(1f, .88f, .54f, fade);
            var shadow = new Color(1f, .73f, .29f, 0f);
            float start = Mathf.InverseLerp(tip - length, tip, tail);
            float end = Mathf.InverseLerp(tip - length, tip, head);
            Quad(mesh, new Vector2(tail, y), new Vector2(head, y), new Vector2(0f, 1.05f),
                Color.Lerp(shadow, light, start), Color.Lerp(shadow, light, end));
        }

        private static void Line(VertexHelper mesh, Vector2 a, Vector2 b, float width, Color32 color)
        {
            Vector2 delta = (b - a).normalized * (width * .5f);
            Quad(mesh, a, b, new Vector2(-delta.y, delta.x), color, color);
        }

        private static void Quad(VertexHelper mesh, Vector2 a, Vector2 b, Vector2 normal, Color32 start, Color32 end)
        {
            int index = mesh.currentVertCount;
            mesh.AddVert(a - normal, start, Vector2.zero);
            mesh.AddVert(a + normal, start, Vector2.zero);
            mesh.AddVert(b + normal, end, Vector2.zero);
            mesh.AddVert(b - normal, end, Vector2.zero);
            mesh.AddTriangle(index, index + 1, index + 2);
            mesh.AddTriangle(index, index + 2, index + 3);
        }
    }
}
