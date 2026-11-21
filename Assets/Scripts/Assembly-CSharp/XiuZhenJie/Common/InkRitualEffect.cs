using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // The same brush silhouettes mark an inscription in the practice room and on the caster.
    internal sealed class InkRitualEffect : MaskableGraphic
    {
        internal enum Mark { Hammer, Mold, Pour, Practice, Flare, Swift, Harmony, Resonance }
        Mark mark;
        Color pigment;
        bool reveal;
        float start, lifetime;

        internal static void Play(Transform anchor, Mark mark, Color pigment, bool reveal = false)
        {
            if (anchor == null) return;
            var rect = anchor as RectTransform;
            var obj = new GameObject("流光", typeof(RectTransform), typeof(CanvasRenderer), typeof(InkRitualEffect));
            obj.layer = anchor.gameObject.layer;
            var effect = obj.GetComponent<InkRitualEffect>();
            effect.rectTransform.SetParent(anchor, false);
            float size = rect == null ? 120 : Mathf.Clamp(Mathf.Min(rect.rect.width, rect.rect.height), 76, 160);
            effect.rectTransform.sizeDelta = Vector2.one * size;
            effect.raycastTarget = false;
            effect.mark = mark;
            effect.pigment = pigment;
            effect.reveal = reveal;
            effect.start = Time.unscaledTime;
            effect.lifetime = mark == Mark.Hammer ? .40f : reveal ? 1.05f : .70f;
        }

        void Update()
        {
            if (Time.unscaledTime - start >= lifetime) Destroy(gameObject);
            else SetVerticesDirty();
        }

        static Vector2 Ray(float radians) => new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        static void Stroke(VertexHelper mesh, Vector2 from, Vector2 to, float width, float endWidth, Color color)
        {
            Vector2 axis = (to - from).normalized;
            Vector2 normal = new Vector2(-axis.y, axis.x);
            int first = mesh.currentVertCount;
            mesh.AddVert(from - normal * width, color, Vector2.zero);
            mesh.AddVert(from + normal * width, color, Vector2.zero);
            mesh.AddVert(to + normal * endWidth, color, Vector2.zero);
            mesh.AddVert(to - normal * endWidth, color, Vector2.zero);
            mesh.AddTriangle(first, first + 1, first + 2);
            mesh.AddTriangle(first, first + 2, first + 3);
        }

        static void Arc(VertexHelper mesh, float radius, float angle, float sweep, float width, Color color)
        {
            Vector2 last = Ray(angle) * radius;
            const int segments = 20;
            for (int i = 1; i <= segments; i++)
            {
                float part = (float)i / segments;
                Vector2 point = Ray(angle + sweep * part) * radius;
                Stroke(mesh, last, point, width * (1.04f - part + 1f / segments), width * (1.04f - part), color);
                last = point;
            }
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (lifetime <= 0) return;
            float t = Mathf.Clamp01((Time.unscaledTime - start) / lifetime);
            float spread = 1 - (1 - t) * (1 - t);
            float radius = rectTransform.rect.width * .43f;
            Color ink = pigment;
            ink.a = Mathf.SmoothStep(1, 0, Mathf.InverseLerp(.44f, 1, t));
            Color glint = Color.Lerp(ink, new Color(1, .85f, .55f), .53f);
            glint.a = ink.a;

            if (mark == Mark.Hammer || mark == Mark.Flare)
            {
                int count = mark == Mark.Hammer ? 13 : 7;
                for (int i = 0; i < count; i++)
                {
                    Vector2 spoke = Ray(i * 2 * Mathf.PI / count + (i % 2) * .11f);
                    float reach = radius * (.55f + (i % 4) * .13f);
                    Vector2 tip = spoke * reach * (.26f + .74f * spread);
                    if (mark == Mark.Hammer) tip.y -= t * t * radius * .28f;
                    Stroke(mesh, tip - spoke * reach * (1 - t) * .38f, tip, 1.7f, .2f, (i % 2 == 0) ? ink : glint);
                }
            }
            else if (mark == Mark.Resonance)
            {
                // The crossing sword seals surround the caster; trained spell marks stay inside them.
                float ring = radius * (.73f + .2f * spread);
                for (int i = 0; i < 5; i++)
                {
                    float angle = i * Mathf.PI * .4f + spread * .28f;
                    Vector2 axis = Ray(angle);
                    Vector2 side = new Vector2(-axis.y, axis.x);
                    Vector2 hilt = axis * ring;
                    Stroke(mesh, hilt, hilt + axis * 9, 2f, 0, glint);
                    Stroke(mesh, hilt - side * 4, hilt + side * 4, 1f, .6f, ink);
                    Arc(mesh, ring - 1.7f, angle + .12f, .84f, 1.4f, ink);
                }
            }
            else if (mark == Mark.Mold)
            {
                float ring = radius * (.38f + spread * .39f);
                for (int i = 0; i < 4; i++)
                {
                    float angle = Mathf.PI * (.25f + i * .5f);
                    Vector2 corner = Ray(angle) * ring;
                    Stroke(mesh, corner, Ray(angle + Mathf.PI / 2) * ring, 2.3f, .6f, ink);
                    Stroke(mesh, corner * 1.22f, corner * 1.40f, 1.5f, .65f, glint);
                }
                // Tapered blade and quillons impressed into the square workshop seal.
                Stroke(mesh, Vector2.down * ring * .47f, Vector2.down * ring * .18f, 1.7f, 1.25f, ink);
                Stroke(mesh, new Vector2(-.38f, -.12f) * ring, new Vector2(0, -.2f) * ring, .85f, 1.8f, ink);
                Stroke(mesh, new Vector2(0, -.2f) * ring, new Vector2(.38f, -.12f) * ring, 1.8f, .85f, ink);
                Stroke(mesh, Vector2.down * ring * .15f, Vector2.up * ring * .50f, ring * .10f, 0, ink);
            }
            else
            {
                int count = mark == Mark.Swift ? 3 : 2;
                for (int i = 0; i < count; i++)
                {
                    float angle = (float)i * Mathf.PI * 2 / count + spread * (mark == Mark.Swift ? 3.1f : 1.2f);
                    float ring = mark == Mark.Practice ? 1 - .67f * spread : .34f + spread * .61f;
                    Arc(mesh, radius * ring, angle, mark == Mark.Harmony ? 2.8f : 1.76f, 3f, ink);
                    if (mark == Mark.Harmony || mark == Mark.Pour)
                        Arc(mesh, radius * ring * .66f, -angle, 2.4f, 2.1f, glint);
                }
            }
            if (reveal)
                for (int i = 0; i < 8; i++)
                {
                    Vector2 axis = Ray(i * Mathf.PI / 4);
                    float r = radius * (.65f + .40f * spread);
                    Stroke(mesh, axis * (r - 3f), axis * (r + 3f), 1.65f, 0, glint);
                }
        }
    }

    internal static class ForgeEffects
    {
        internal static void Play(Transform parent, string phase)
        {
            switch (phase)
            {
                case "hammer": InkRitualEffect.Play(parent, InkRitualEffect.Mark.Hammer, new Color32(211, 89, 36, 255)); break;
                case "mold": InkRitualEffect.Play(parent, InkRitualEffect.Mark.Mold, new Color32(45, 130, 99, 255), true); break;
                case "cast": InkRitualEffect.Play(parent, InkRitualEffect.Mark.Pour, new Color32(194, 115, 34, 255), true); break;
            }
        }
    }

    internal static class SpellPracticeEffects
    {
        static readonly Color32[] Elements =
        {
            new Color32(101, 99, 135, 255), new Color32(198, 147, 50, 255),
            new Color32(58, 148, 104, 255), new Color32(47, 145, 182, 255),
            new Color32(209, 83, 54, 255), new Color32(161, 117, 72, 255)
        };

        internal static void Reveal(RectTransform anchor, int element, int affix, bool breakthrough)
        {
            var mark = affix switch
            {
                1 => InkRitualEffect.Mark.Flare,
                2 => InkRitualEffect.Mark.Swift,
                3 => InkRitualEffect.Mark.Harmony,
                _ => InkRitualEffect.Mark.Practice
            };
            InkRitualEffect.Play(anchor, mark, Elements[Mathf.Clamp(element, 0, 5)], breakthrough);
        }

        internal static void OnCast(PlayerEntity caster, BaseSkillModel spell)
        {
            if ((spell.PracticeAffix <= 0 && spell.ForgeResonancePercent <= 0) || caster.BaseController == null) return;
            var actor = caster.BaseController.transform;
            var field = actor.Find("术纹") as RectTransform;
            if (field == null)
            {
                var obj = new GameObject("术纹", typeof(RectTransform), typeof(Canvas));
                obj.layer = actor.gameObject.layer;
                var canvas = obj.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.sortingOrder = 33;
                field = (RectTransform)obj.transform;
                field.SetParent(actor, false);
                field.sizeDelta = Vector2.one * 96;
                field.localScale = Vector3.one * .026f;
                field.localPosition = new Vector3(0, .50f, -2);
            }
            if (spell.PracticeAffix > 0) Reveal(field, spell.fiveElement, spell.PracticeAffix, false);
            if (spell.ForgeResonancePercent > 0)
                InkRitualEffect.Play(field, InkRitualEffect.Mark.Resonance, Elements[Mathf.Clamp(spell.fiveElement, 0, 5)]);
        }
    }
}
