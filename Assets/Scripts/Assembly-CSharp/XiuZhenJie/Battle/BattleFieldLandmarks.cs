using UnityEngine;

namespace XiuZhenJie
{
    public sealed partial class BattleFieldView
    {
        // Kind is part of the battle wire format, starting at one.
        static readonly string[] LandmarkNames = { "听风古松", "流萤灵泉", "观星遗台", "寒玉晶窟", "落霞剑冢", "云阶山门" };
        static readonly Color32[] LandmarkColors = {
            new Color32(209,179,72,255), new Color32(84,220,204,255), new Color32(139,158,246,255),
            new Color32(194,157,245,255), new Color32(241,133,93,255), new Color32(246,193,109,255)
        };
        static readonly float[] LandmarkWidths = { 6.5f, 5.8f, 5.6f, 4.9f, 5.6f, 6.1f };

        sealed class Vista
        {
            public BattleLandmark Site;
            public Vector3 Position;
            public SpriteRenderer Art;
            public float RevealTime = -10f;
            public bool Discovered;
        }

        Vista[] vistas;
        byte[] landmarkCells;
        float nextAmbient, discoveryTime = -10f;
        string discovery;
        BattleFlourish landmarkEffects;

        internal string Discovery => Time.time - discoveryTime < 2.8f ? discovery : null;
        internal float DiscoveryAlpha => Mathf.Min(Mathf.Clamp01((Time.time - discoveryTime) * 4f),
            Mathf.Clamp01((discoveryTime + 2.8f - Time.time) * 2.5f));
        public int DiscoveredLandmarks { get; private set; }

        void BuildLandmarks()
        {
            landmarkCells = new byte[layout.Tiles.Length];
            vistas = new Vista[layout.Landmarks.Length];
            if (vistas.Length == 0) return;
            var atlas = ReleasedResources.Load<Texture2D>("BattleField/Landmarks");
            int width = atlas.width / 3, height = atlas.height / 2;
            var pigment = Own(new Material(ReleasedResources.Load<Shader>("shaders/BattleLandmark")));
            var art = new Sprite[6];
            for (int i = 0; i < art.Length; i++)
            {
                var rect = new Rect((i % 3) * width + 1, (1 - i / 3) * height + 1, width - 2, height - 2);
                art[i] = Own(Sprite.Create(atlas, rect, new Vector2(.5f,.28f), width, 0, SpriteMeshType.FullRect));
            }
            for (int i = 0; i < vistas.Length; i++)
            {
                var site = layout.Landmarks[i];
                int kind = site.Kind - 1;
                var actor = new GameObject(LandmarkNames[kind]).AddComponent<SpriteRenderer>();
                actor.transform.SetParent(transform, false);
                Vector3 position = layout.Center(site.Cell);
                position.z = .23f;
                actor.transform.position = position;
                actor.transform.localScale = Vector3.one * LandmarkWidths[kind];
                actor.sprite = art[kind];
                actor.sharedMaterial = pigment;
                actor.sortingOrder = -48;
                actor.enabled = false;
                landmarkCells[site.Cell] = (byte)site.Kind;
                vistas[i] = new Vista { Site = site, Position = position, Art = actor };
            }
        }

        void RevealLandmarks()
        {
            if (vistas == null) return;
            foreach (var vista in vistas)
            {
                int cell = vista.Site.Cell;
                if (vista.Discovered || explored[cell] == 0) continue;
                vista.Discovered = true;
                DiscoveredLandmarks++;
                vista.Art.enabled = true;
                TouchMini(cell);
                if (!visible[cell]) continue;
                vista.RevealTime = discoveryTime = Time.time;
                discovery = LandmarkNames[vista.Site.Kind - 1];
                if (landmarkEffects) landmarkEffects.VistaMotes(vista.Position, vista.Site.Kind, true);
            }
        }

        void AnimateLandmarks()
        {
            if (vistas == null || Time.time < nextAmbient) return;
            nextAmbient = Time.time + .12f;
            foreach (var vista in vistas)
            {
                if (!vista.Discovered || !visible[vista.Site.Cell]) continue;
                float t = Mathf.Clamp01((Time.time - vista.RevealTime) / .48f);
                float distance = Vector2.Distance(observer.position, vista.Position);
                float opacity = Mathf.Lerp(.76f, 1f, Mathf.SmoothStep(0f, 1f, distance / 2.8f));
                vista.Art.color = new Color(1, 1, 1, opacity * (.38f + .62f * t));
                if (landmarkEffects) landmarkEffects.VistaMotes(vista.Position, vista.Site.Kind, false);
            }
        }
    }
}
