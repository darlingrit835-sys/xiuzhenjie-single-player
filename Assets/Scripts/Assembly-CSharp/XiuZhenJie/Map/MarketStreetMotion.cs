using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // A single UI mesh registers animated details and particles to the painted street.
    [DisallowMultipleComponent]
    internal sealed class MarketStreetMotion : MaskableGraphic
    {
        private enum Effect { Cloth, Forge, Lamp, Crystal, Compass, Ember, Glimmer, Smoke, Petal }

        private const float ArtWidth = 1024f;
        private const float ArtHeight = 1536f;
        private static readonly int ClockId = Shader.PropertyToID("_MarketStreetClock");
        private static readonly int ScaleId = Shader.PropertyToID("_DesignToLocal");
        private Sprite painting;
        private Material paintMaterial;

        internal static void Attach(Image street)
        {
            if (street == null || street.sprite == null || street.transform.Find("MarketStreetMotion") != null) return;
            var shader = ReleasedResources.Load<Shader>("shaders/MarketStreetMotion");
            if (shader == null || !shader.isSupported) return;

            var node = new GameObject("MarketStreetMotion", typeof(RectTransform), typeof(CanvasRenderer));
            node.layer = street.gameObject.layer;
            var frame = (RectTransform)node.transform;
            frame.SetParent(street.transform, false);
            frame.SetAsFirstSibling();
            frame.anchorMin = Vector2.zero;
            frame.anchorMax = Vector2.one;
            frame.offsetMin = frame.offsetMax = Vector2.zero;
            var motion = node.AddComponent<MarketStreetMotion>();
            motion.raycastTarget = false;
            motion.painting = street.sprite;
            motion.paintMaterial = new Material(shader);
            motion.paintMaterial.SetVector("_SpriteUV", DataUtility.GetOuterUV(street.sprite));
            motion.material = motion.paintMaterial;
            motion.useLegacyMeshGeneration = false;
            motion.SetAllDirty();
            Shader.SetGlobalFloat(ClockId, Time.unscaledTime);
        }

        public override Texture mainTexture => painting != null ? painting.texture : Texture2D.whiteTexture;

        private void Update() => Shader.SetGlobalFloat(ClockId, Time.unscaledTime);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (paintMaterial != null) Destroy(paintMaterial);
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (painting == null) return;
            var rect = GetPixelAdjustedRect();
            Vector4 scale = new Vector4(rect.width / ArtWidth, rect.height / ArtHeight, 0, 0);
            paintMaterial.SetVector(ScaleId, scale);
            if (materialForRendering != paintMaterial) materialForRendering.SetVector(ScaleId, scale);

            // Pixel coordinates on MarketStreet.jpg, measured from its top left.
            Patch(mesh, rect, Effect.Cloth, 146, 149, 181, 215, .09f);   // Suspended paper talismans.
            Patch(mesh, rect, Effect.Cloth, 224, 164, 257, 232, .36f);
            Patch(mesh, rect, Effect.Cloth, 895, 154, 928, 217, .65f);   // Apothecary gourd.
            Patch(mesh, rect, Effect.Cloth, 650, 489, 717, 610, .24f);   // Scroll shop drapery.
            Patch(mesh, rect, Effect.Cloth, 131, 705, 402, 800, .87f);   // Rose canvas awning.
            Patch(mesh, rect, Effect.Forge, 180, 528, 280, 590, .15f);
            Patch(mesh, rect, Effect.Crystal, 675, 752, 738, 817, .27f);
            Patch(mesh, rect, Effect.Crystal, 916, 737, 976, 806, .82f);
            Patch(mesh, rect, Effect.Crystal, 832, 794, 903, 856, .59f);
            Patch(mesh, rect, Effect.Lamp, 527, 1040, 623, 1134, .11f);
            Patch(mesh, rect, Effect.Lamp, 900, 1016, 998, 1109, .71f);
            Patch(mesh, rect, Effect.Compass, 172, 1305, 380, 1391, .27f);

            Particles(mesh, rect, Effect.Smoke, 350, 254, 3, 26, 42);
            Particles(mesh, rect, Effect.Smoke, 231, 556, 3, 32, 53);
            Particles(mesh, rect, Effect.Ember, 231, 558, 10, 6, 10);
            Particles(mesh, rect, Effect.Glimmer, 864, 832, 7, 11, 13);
            Particles(mesh, rect, Effect.Petal, 459, 699, 5, 12, 6);
            Particles(mesh, rect, Effect.Petal, 472, 1217, 4, 12, 7);
        }

        private static void Particles(VertexHelper mesh, Rect rect, Effect effect, float x, float y,
            int count, float width, float height)
        {
            for (int i = 0; i < count; i++)
            {
                float seed = Mathf.Repeat(i * .618034f + x * .0013f, 1f);
                float spread = effect == Effect.Petal ? 58f : 14f;
                float cx = x + Mathf.Sin(i * 2.4f) * spread;
                float cy = y + Mathf.Cos(i * 1.7f) * 6f;
                float size = .72f + .5f * seed;
                Patch(mesh, rect, effect, cx - width * size * .5f, cy - height * size * .5f,
                    cx + width * size * .5f, cy + height * size * .5f, seed);
            }
        }

        private static void Patch(VertexHelper mesh, Rect rect, Effect effect,
            float left, float top, float right, float bottom, float seed)
        {
            int first = mesh.currentVertCount;
            int variant = Mathf.RoundToInt(seed * 31f);
            Vertex(mesh, rect, effect, left, top, 0, variant);
            Vertex(mesh, rect, effect, right, top, 1, variant);
            Vertex(mesh, rect, effect, right, bottom, 3, variant);
            Vertex(mesh, rect, effect, left, bottom, 2, variant);
            mesh.AddTriangle(first, first + 1, first + 2);
            mesh.AddTriangle(first, first + 2, first + 3);
        }

        private static void Vertex(VertexHelper mesh, Rect rect, Effect effect, float x, float y,
            int corner, int variant)
        {
            float sx = x / ArtWidth;
            float sy = 1f - y / ArtHeight;
            // UV bands carry the effect, corner and phase independently of UI vertex tint.
            mesh.AddVert(new Vector3(rect.xMin + rect.width * sx, rect.yMin + rect.height * sy),
                Color.white, new Vector2(8f * (int)effect + 2f * corner + sx, 2f * variant + sy));
        }
    }
}
