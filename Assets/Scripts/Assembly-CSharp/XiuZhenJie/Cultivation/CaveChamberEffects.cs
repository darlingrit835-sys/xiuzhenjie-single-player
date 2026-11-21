using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class CaveChamberEffects : CaveLightMesh
    {
        private static readonly Color[] Tints = {
            new Color32(255,208,144,255), new Color32(255,189,74,255), new Color32(126,221,247,255),
            new Color32(139,245,177,255), new Color32(222,203,255,255)
        };
        private static readonly Color Ivory = new Color32(255,244,202,255);
        private static readonly Vector2[] Dipper = {
            new Vector2(-.98f,.83f), new Vector2(-.71f,.92f), new Vector2(-.52f,.74f),
            new Vector2(-.31f,.68f), new Vector2(-.12f,.82f), new Vector2(.12f,.67f), new Vector2(-.08f,.52f)
        };
        private int grade;
        private float lastPulse = -10;

        internal void SetGrade(int value)
        {
            value = Mathf.Clamp(value, 0, 4);
            if (grade == value) return;
            grade = value;
            PlayPulse();
        }

        internal void PlayPulse()
        {
            lastPulse = Time.unscaledTime;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            Rect rect = rectTransform.rect;
            if (rect.width < 2 || rect.height < 2) return;
            Vector2 dais = new Vector2(rect.xMin + rect.width * .611f, rect.yMin + rect.height * .391f);
            float radius = Mathf.Min(rect.width * .172f, rect.height * .37f);
            Color tint = Tints[grade];
            float breath = .5f + .5f * Mathf.Sin(SceneTime * 1.7f);
            if (grade > 0) LightPainting(mesh, rect, dais, radius, tint, breath);
            Glow(mesh, dais, radius * 1.18f, radius * .36f, Ink(tint, .17f + breath * .14f));
            switch (grade)
            {
                case 0:
                    Glow(mesh, dais, radius * .17f, radius * .12f, Ink(Ivory,.33f + breath * .14f));
                    break;
                case 1: Lanterns(mesh, dais, radius, tint); break;
                case 2: Spring(mesh, dais, radius, tint); break;
                case 3: CrystalVeins(mesh, dais, radius, tint); break;
                case 4: Heaven(mesh, dais, radius, tint); break;
            }
            float age = (Time.unscaledTime - lastPulse) / 1.35f;
            if (age >= 0 && age < 1)
            {
                float ease = 1 - (1 - age) * (1 - age);
                Arc(mesh, dais, radius * (.12f + ease * 1.05f), radius * (.045f + ease * .29f),
                    0, Cycle, radius * (.023f - .012f * age), Ink(Ivory, (1 - age) * .94f));
                Glow(mesh, dais, radius * .74f, radius * .40f, Ink(tint, .45f * (1 - age)));
            }
        }

        private void LightPainting(VertexHelper mesh, Rect rect, Vector2 dais, float r, Color tint, float breath)
        {
            // Reflected light shares the painted bronze door boss, gilded step and the opening over the dais.
            Vector2 door = new Vector2(rect.xMin + rect.width * .365f, rect.yMin + rect.height * .67f);
            Glow(mesh, door, r * .33f, r * .37f, Ink(tint, .28f + breath * .20f));
            Glow(mesh, dais - Vector2.up * r * .27f, r * 1.33f, r * .25f, Ink(tint, .28f));
            if (grade == 1) return;

            Vector2 aperture = new Vector2(rect.xMin + rect.width * .855f, rect.yMax);
            int count = grade == 4 ? 5 : 3;
            for (int i = 0; i < count; i++)
            {
                float drift = .5f + .5f * Mathf.Sin(SceneTime * .47f + i * 1.8f);
                Vector2 top = aperture + Vector2.right * r * ((i - 2) * .14f);
                Vector2 floor = dais + r * new Vector2((i - 2) * .21f + .3f * drift, -.09f);
                Color light = Ink(tint, .09f + .12f * drift);
                Quad(mesh, top, top + Vector2.right * (r * .09f), floor + Vector2.right * (r * .16f),
                    floor - Vector2.right * (r * .06f), light, Ink(tint, 0));
            }
        }

        // Each wick swells from the dais and lifts gold ash in the lamplight.
        private void Lanterns(VertexHelper mesh, Vector2 dais, float r, Color tint)
        {
            for (int i = 0; i < 5; ++i)
            {
                Vector2 foot = dais + Orbit(Cycle * i / 5 + .31f, r * .72f, r * .19f);
                float flicker = .5f + .5f * Mathf.Sin(SceneTime * 3.8f + i * 1.9f);
                Vector2 flame = foot + Vector2.up * r * (.11f + .035f * flicker);
                Glow(mesh, flame, r * .19f, r * (.29f + .04f * flicker), Ink(tint,.69f));
                Glow(mesh, flame, r * .052f, r * (.14f + .035f * flicker), Ivory, 8);
                // Bronze lotus cups form a still silhouette beneath the moving flame.
                for (int petal = -1; petal <= 1; ++petal)
                {
                    Vector2 lip = foot + r * new Vector2(petal * .10f, .047f + .032f * (1 - Mathf.Abs(petal)));
                    Vector2 rim = foot + r * new Vector2(.028f,.012f);
                    Quad(mesh, foot - Vector2.right * r * .024f, lip - Vector2.right * r * .026f,
                        lip, rim, new Color32(146,84,34,221), Ink(Ivory,.94f));
                }
                Arc(mesh, foot, r * .084f, r * .021f, 0, Cycle, r * .013f, Ink(tint,.85f));
            }
            for (int i = 0; i < 15; ++i)
            {
                float age = Mathf.Repeat(i * .382f + SceneTime * .24f, 1);
                Vector2 p = dais + r * new Vector2(.69f * Mathf.Sin(i * 5.1f) + age * .11f * Mathf.Sin(i + age * 6),
                    age * (.62f + .35f * Mathf.Abs(Mathf.Cos(i))) + .09f * Mathf.Sin(i));
                Spark(mesh, p, r * .024f, Ink(Ivory, Mathf.Sin(Mathf.PI * age)));
            }
        }

        private void Spring(VertexHelper mesh, Vector2 dais, float r, Color tint)
        {
            for (int i = 0; i < 3; ++i)
            {
                float wave = Mathf.Repeat(SceneTime * .31f + i / 3f, 1);
                Arc(mesh, dais, (.11f + wave) * r, (.035f + wave * .28f) * r,
                    0, Cycle, r * .017f, Ink(tint, (1 - wave) * .86f));
            }
            for (int strand = 0; strand < 2; ++strand)
            {
                Vector2 previous = dais;
                for (int i = 0; i <= 36; ++i)
                {
                    float u = i / 36f;
                    float a = u * Cycle * 1.35f + SceneTime * 1.28f + strand * Mathf.PI;
                    Vector2 p = dais + r * new Vector2(Mathf.Cos(a) * (.22f + .37f * Mathf.Sin(u * Mathf.PI)),
                        u * 1.09f + Mathf.Sin(a) * .11f);
                    if (i > 0)
                    {
                        float alpha = Mathf.Sin(u * Mathf.PI) * (.49f + .28f * Mathf.Sin(a));
                        Stroke(mesh, previous, p, r * .083f, Ink(tint, alpha * .30f));
                        Stroke(mesh, previous, p, r * .024f, Ink(tint, alpha));
                    }
                    previous = p;
                }
            }
            for (int i = 0; i < 9; ++i)
            {
                float u = Mathf.Repeat(SceneTime * .28f + i * .382f, 1);
                float angle = u * Cycle * 1.35f + SceneTime * 1.28f + (i % 2) * Mathf.PI;
                Vector2 p = dais + r * new Vector2(Mathf.Cos(angle) * (.22f + .37f * Mathf.Sin(u * Mathf.PI)), u * 1.09f + Mathf.Sin(angle) * .11f);
                Glow(mesh, p, r * .038f, r * .05f, Ink(Color.white, Mathf.Sin(u * Mathf.PI)));
            }
        }

        private void CrystalVeins(VertexHelper mesh, Vector2 dais, float r, Color tint)
        {
            Color shadow = new Color32(38,143,123,175);
            for (int i = 0; i < 6; ++i)
            {
                float theta = Cycle * i / 6 + .26f;
                float swell = .5f + .5f * Mathf.Sin(SceneTime * 1.65f - i * .7f);
                Vector2 mid = dais + Orbit(theta - .22f, r * .40f, r * .12f);
                Vector2 basePoint = dais + Orbit(theta, r * .78f, r * .20f);
                Stroke(mesh, dais, mid, r * .021f, Ink(tint, .45f + .53f * swell));
                Stroke(mesh, mid, basePoint, r * .025f, Ink(tint, .45f + .53f * swell));
                Vector2 tip = basePoint + r * new Vector2(Mathf.Cos(theta) * .13f, .27f + .24f * swell + (i % 2) * .17f);
                Vector2 left = basePoint + r * new Vector2(-.095f,.13f);
                Vector2 right = basePoint + r * new Vector2(.081f,.10f);
                Vector2 ridge = Vector2.Lerp(basePoint,tip,.39f) + Vector2.right * r * .022f;
                Glow(mesh, basePoint, r * .26f, r * .13f, Ink(tint,.60f));
                Quad(mesh, basePoint,left,tip,ridge,shadow,Ink(tint,.86f));
                Quad(mesh, basePoint,ridge,tip,right,Ink(tint,.55f),Ink(Ivory,.82f));
                Stroke(mesh, ridge,tip,r * .013f,Ink(Ivory,.80f));
                Spark(mesh,tip,r * (.013f + .025f * swell),Ink(tint,.95f));
            }
        }

        private void Heaven(VertexHelper mesh, Vector2 dais, float r, Color tint)
        {
            Vector2 sphere = dais + Vector2.up * r * .59f;
            float shimmer = .5f + .5f * Mathf.Sin(SceneTime * 1.8f);
            Color violet = new Color32(155,134,246,255);
            Glow(mesh, sphere, r * .79f, r * .79f, Ink(violet,.49f));
            Glow(mesh, dais, r * 1.13f, r * .32f, Ink(violet,.52f));
            // The bronze dial lights up in sections like an old Chinese planisphere.
            for (int i = 0; i < 12; ++i)
            {
                float angle = Cycle * i / 12 + SceneTime * .12f;
                Arc(mesh, dais, r * .91f, r * .23f, angle, Cycle * .056f, r * .018f, Ink(Ivory,.85f));
                Vector2 tick = dais + Orbit(angle,r * .83f,r * .21f);
                Stroke(mesh,tick,dais + Orbit(angle,r * .94f,r * .24f),r * .013f,Ink(tint,.82f));
            }
            Arc(mesh,sphere,r * .55f,r * .55f,0,Cycle,r * .013f,Ink(tint,.86f));
            for (int i = 0; i < 8; ++i)
            {
                float a = i * Cycle / 8 - SceneTime * .18f;
                Arc(mesh,sphere,r * .59f,r * .59f,a,.36f,r * .012f,Ink(Ivory,.80f));
                Spark(mesh,sphere + Orbit(a,r * .59f,r * .59f),r * .025f,tint);
            }
            Glow(mesh,sphere,r * .23f,r * .23f,Ink(tint,.92f));
            Glow(mesh,sphere,r * .091f,r * .091f,Ivory);
            Spark(mesh,sphere,r * (.12f + .034f * shimmer),Ivory);
            for (int i = 0; i < 3; ++i)
            {
                float tilt = SceneTime * (.14f + i * .017f) + i * Mathf.PI / 3;
                Arc(mesh,sphere,r * .52f,r * .21f,0,Cycle,r * .013f,Ink(i == 1 ? Ivory : tint,.73f),tilt);
                float phase = SceneTime * (.62f + i * .15f) + i;
                Vector2 orbit = Orbit(phase, r * .52f, r * .21f);
                Vector2 star = sphere + new Vector2(orbit.x * Mathf.Cos(tilt) - orbit.y * Mathf.Sin(tilt),
                    orbit.x * Mathf.Sin(tilt) + orbit.y * Mathf.Cos(tilt));
                Spark(mesh,star,r * .051f,Ivory);
            }
            // The Dipper's handle and bowl are drawn in the mountain air over the astrolabe.
            Vector2 prev = default, bowl = default;
            for (int i = 0; i < Dipper.Length; ++i)
            {
                Vector2 p = dais + r * (Dipper[i] + Vector2.up * .27f);
                if (i > 0) Stroke(mesh,prev,p,r * .009f,Ink(tint,.58f));
                if (i == 3) bowl = p;
                if (i == 6) Stroke(mesh,p,bowl,r * .009f,Ink(tint,.58f));
                Spark(mesh,p,r * (.032f + .018f * Mathf.Sin(SceneTime * 2 + i)),Ivory);
                prev = p;
            }
            for (int i = 0; i < 4; ++i)
            {
                float u = Mathf.Repeat(SceneTime * .24f + i * .30f,1);
                Vector2 top = dais + r * new Vector2(-.66f + i * .44f,1.44f);
                Vector2 foot = dais + r * new Vector2(-.28f + i * .19f,0);
                Vector2 p = Vector2.Lerp(top,foot,u);
                Quad(mesh,p - Vector2.right * r * .009f,p + Vector2.right * r * .009f,
                    p + r * new Vector2(.044f,.21f),p + r * new Vector2(.031f,.21f),
                    Ink(tint,Mathf.Sin(u * Mathf.PI)),Ink(tint,0));
            }
        }
    }
}
