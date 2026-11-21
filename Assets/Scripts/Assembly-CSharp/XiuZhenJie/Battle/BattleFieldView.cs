using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public sealed partial class BattleFieldView : MonoBehaviour
    {
        public enum MarkerKind
        {
            Player,
            Beast,
            Cultivator,
            Herb,
            Ore,
            Portal,
            Loot
        }

        const int Sight = 4;
        const float FadeSeconds = 1f / 4.5f;
        const int MiniScale = 3;
        static readonly string[] TerrainNames = {"山壁", "山径", "草木林", "矿岩地", "水域", "营地", "遗迹"};
        static readonly Color32[] TerrainColors = {new Color32(96, 108, 109, 128), new Color32(210, 193, 151, 0), new Color32(152, 178, 137, 0), new Color32(183, 166, 142, 0), new Color32(102, 162, 160, 255), new Color32(201, 181, 149, 0), new Color32(169, 172, 165, 0)};
        static readonly Color32 Uncharted = new Color32(24, 39, 43, 255);
        static readonly Color32[] MarkerColors = {new Color32(254, 247, 202, 255), new Color32(237, 121, 79, 255), new Color32(229, 136, 172, 255), new Color32(109, 240, 151, 255), new Color32(121, 228, 255, 255), new Color32(199, 161, 252, 255), new Color32(239, 196, 99, 255)};
        BattleFieldLayout layout;
        Transform observer;
        bool[] visible;
        byte[] explored;
        int exploredCount, walkableCount, lastCell = -1;
        readonly List<int> previousSight = new List<int>(64);
        Color32[] fogPixels;
        bool[] fogPending, miniPending;
        readonly List<int> dirtyFog = new List<int>(128);
        readonly List<int> transitions = new List<int>(128);
        readonly List<int> dirtyMini = new List<int>(128);
        byte[] miniSymbols;
        Texture2D minimap, fog;
        RawImage miniImage;
        Material fogMaterial;
        float fadeStarted;
        readonly List<Object> owned = new List<Object>();
        readonly List<Marker> markers = new List<Marker>();
        readonly List<Marker> activeEnemies = new List<Marker>();
        readonly Dictionary<int, List<Marker>> resources = new Dictionary<int, List<Marker>>();
        readonly List<Marker> nearResources = new List<Marker>();
        sealed class Marker
        {
            public Transform root;
            public Renderer[] renderers;
            public Canvas[] canvases;
            public MarkerKind kind;
            public bool shown;
            public int cell = -1;
            public Controller controller;
            public Transform body, halo;
            public SpriteRenderer bodyRenderer;
            public WalkingFigure walk;
            public Vector3 lastPosition;
            public float stride;
            public float deathStarted = -1f;
        }

        public int ExploredPercent => walkableCount == 0 ? 0 : exploredCount * 100 / walkableCount;
        public void Initialize(BattleFieldLayout field, Controller player, BattleFlourish effects = null)
        {
            layout = field;
            landmarkEffects = effects;
            observer = player.transform;
            int cells = layout.Tiles.Length;
            visible = new bool[cells];
            explored = new byte[cells];
            fogPixels = new Color32[cells];
            fogPending = new bool[cells];
            miniPending = new bool[cells];
            miniSymbols = new byte[cells];
            for (int i = 0; i < cells; i++)
            {
                fogPixels[i] = Color.white;
                if (WalkableCell(i))
                    walkableCount++;
            }

            fog = Texture(layout.Width, layout.Height, "神识迷雾", FilterMode.Bilinear, true);
            minimap = Texture(layout.Width * MiniScale, layout.Height * MiniScale, "战场舆图", FilterMode.Point);
            fog.SetPixels32(fogPixels);
            fog.Apply(false);
            var blankMap = minimap.GetRawTextureData<Color32>();
            for (int i = 0; i < blankMap.Length; i++)
                blankMap[i] = Uncharted;
            minimap.Apply(false);
            BuildLandscape();
            BuildLandmarks();
            var cloudInk = ReleasedResources.Load<Texture2D>("BattleField/CloudInk");
            BuildOuterMist(cloudInk);
            fogMaterial = Plane("神识迷雾", fog, "BattleMist", 300);
            fogMaterial.SetTexture("_CloudInk", cloudInk);
            RestoreExplored(layout.Explored);
            Camera eye = player.GetComponentInChildren<Camera>();
            if (eye != null)
            {
                eye.clearFlags = CameraClearFlags.SolidColor;
                eye.backgroundColor = new Color(.084f, .143f, .157f);
                eye.orthographicSize = Mathf.Clamp(8.4f / Mathf.Max(.65f, eye.aspect), 7.8f, 12f);
                if (!eye.TryGetComponent<BattleFieldWorldScale>(out _)) eye.gameObject.AddComponent<BattleFieldWorldScale>();
            }
        }

        T Own<T>(T item)
            where T : Object
        {
            owned.Add(item);
            return item;
        }

        Texture2D Texture(int width, int height, string label, FilterMode filter, bool linear = false)
        {
            var tex = Own(new Texture2D(width, height, TextureFormat.RGBA32, false, linear));
            tex.name = label;
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = filter;
            return tex;
        }

        Material Plane(string label, Texture2D tex, string shaderName, int order)
        {
            var image = new GameObject(label).AddComponent<SpriteRenderer>();
            image.transform.SetParent(transform, false);
            var centre = layout.Origin + new Vector2(layout.Width, layout.Height) * (BattleFieldLayout.CellSize * .5f);
            image.transform.position = new Vector3(centre.x, centre.y, order < 0 ? 1 : -.5f);
            image.sprite = Own(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), 1f / BattleFieldLayout.CellSize, 0, SpriteMeshType.FullRect));
            image.sortingOrder = order;
            var material = Own(new Material(ReleasedResources.Load<Shader>("shaders/" + shaderName)));
            image.sharedMaterial = material;
            return material;
        }

        // The distant panorama is visible outside the chart only. A hollow mesh avoids painting it under every ground pixel.
        void BuildOuterMist(Texture2D clouds)
        {
            const float border = 64f;
            Vector2 min = layout.Origin, max = min + new Vector2(layout.Width, layout.Height) * BattleFieldLayout.CellSize;
            var vertices = new Vector3[8];
            for (int i = 0; i < 4; i++)
            {
                bool right = i == 1 || i == 2, top = i >= 2;
                vertices[i] = new Vector3(right ? max.x : min.x, top ? max.y : min.y, 1);
                vertices[i+4] = vertices[i] + new Vector3(right ? border : -border, top ? border : -border, 0);
            }
            var mesh = Own(new Mesh { name = "舆图外沿云海" });
            mesh.vertices = vertices;
            mesh.uv = new Vector2[vertices.Length];
            mesh.triangles = new[]{0,4,5, 0,5,1, 1,5,6, 1,6,2, 2,6,7, 2,7,3, 3,7,4, 3,4,0};
            mesh.UploadMeshData(true);
            var go = new GameObject("远山雾海");
            go.transform.SetParent(transform, false);
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            var renderer = go.AddComponent<MeshRenderer>();
            var mist = Own(new Material(ReleasedResources.Load<Shader>("shaders/BattleMist")));
            mist.mainTexture = Texture2D.whiteTexture;
            mist.SetTexture("_CloudInk", clouds);
            renderer.sharedMaterial = mist;
            renderer.sortingOrder = -180;
        }

        public void RestoreExplored(byte[] bits)
        {
            if (bits != null)
                for (int i = 0; i < explored.Length && (i >> 3) < bits.Length; i++)
                    if ((bits[i >> 3] & (1 << (i & 7))) != 0)
                        Explore(i);
            RefreshSight(layout.Index(observer.position), true);
        }

        void Explore(int index)
        {
            if (explored[index] != 0)
                return;
            explored[index] = 1;
            if (WalkableCell(index))
                exploredCount++;
            TouchFog(index);
        }

        bool WalkableCell(int cell)
        {
            byte terrain = layout.Tiles[cell];
            return terrain != 0 && terrain != 4;
        }

        void TouchMini(int cell)
        {
            if (cell < 0 || miniPending[cell])
                return;
            miniPending[cell] = true;
            dirtyMini.Add(cell);
        }

        void TouchFog(int cell)
        {
            if (fogPending[cell])
                return;
            fogPending[cell] = true;
            dirtyFog.Add(cell);
        }

        public bool IsVisible(Vector2 world)
        {
            int cell = layout.Index(world);
            return cell >= 0 && visible[cell];
        }

        public string TerrainNameAt(Vector2 world)
        {
            int cell = layout.Index(world);
            if (cell < 0) return "";
            foreach (var vista in vistas)
                if (vista.Discovered && (layout.Center(vista.Site.Cell) - world).sqrMagnitude < 5f)
                    return LandmarkNames[vista.Site.Kind-1];
            return TerrainNames[layout.Tiles[cell]];
        }

        // Sweep long steps and slide along open axes. Two touching blocked corners cannot be cut.
        public bool TryMove(Vector2 from, Vector2 desired, out Vector2 reached)
        {
            reached = from;
            var distance = desired - from;
            float length = distance.magnitude;
            if (length < .00001f)
                return true;
            if (!layout.IsWalkable(from) || float.IsNaN(length) || float.IsInfinity(length))
                return false;
            int steps = Mathf.CeilToInt(Mathf.Min(length, (layout.Width + layout.Height) * BattleFieldLayout.CellSize) / (BattleFieldLayout.CellSize * .3f));
            Vector2 step = distance / length * Mathf.Min(length / steps, BattleFieldLayout.CellSize * .3f);
            for (int i = 0; i < steps; i++)
            {
                Vector2 next = reached + step;
                bool passX = layout.IsWalkable(new Vector2(next.x, reached.y));
                bool passY = layout.IsWalkable(new Vector2(reached.x, next.y));
                if (passX && passY && layout.IsWalkable(next))
                    reached = next;
                else if (passX && (!passY || Mathf.Abs(step.x) >= Mathf.Abs(step.y)))
                    reached.x = next.x;
                else if (passY)
                    reached.y = next.y;
                else
                    break;
            }

            return (reached - from).sqrMagnitude > .0000001f;
        }

        void RefreshSight(int cell, bool reset = false)
        {
            if (cell == lastCell && !reset)
                return;
            TouchMini(lastCell);
            lastCell = cell;
            TouchMini(cell);
            foreach (int index in previousSight)
            {
                visible[index] = false;
                TouchFog(index);
            }

            previousSight.Clear();
            if (cell >= 0)
            {
                int cx = cell % layout.Width, cy = cell / layout.Width;
                for (int y = Mathf.Max(0, cy - Sight); y <= Mathf.Min(layout.Height - 1, cy + Sight); y++)
                    for (int x = Mathf.Max(0, cx - Sight); x <= Mathf.Min(layout.Width - 1, cx + Sight); x++)
                    {
                        int dx = x - cx, dy = y - cy;
                        if (dx * dx + dy * dy > Sight * Sight || !LineOfSight(cx, cy, x, y))
                            continue;
                        int index = y * layout.Width + x;
                        visible[index] = true;
                        previousSight.Add(index);
                        TouchFog(index);
                        Explore(index);
                    }
            }

            // Complete the previous blend before reusing the shader clock. Only the two
            // sight discs, newly restored tiles, and unfinished fades can change.
            foreach (int index in transitions)
                TouchFog(index);
            transitions.Clear();
            float fraction = Mathf.Clamp01((Time.time - fadeStarted) / FadeSeconds);
            var texturePixels = fog.GetRawTextureData<Color32>();
            foreach (int index in dirtyFog)
            {
                fogPending[index] = false;
                byte target = visible[index] ? (byte)0 : explored[index] != 0 ? (byte)155 : (byte)255;
                var old = fogPixels[index];
                if (target != old.g)
                {
                    TouchMini(index);
                    if (resources.TryGetValue(index, out var nodes))
                    {
                        for (int i = nodes.Count - 1; i >= 0; i--)
                        {
                            var node = nodes[i];
                            if (!node.root)
                            {
                                nodes.RemoveAt(i);
                                nearResources.Remove(node);
                            }
                            else
                                UpdateMarker(node);
                        }
                    }
                }

                byte current = reset ? target : (byte)Mathf.Lerp(old.r, old.g, fraction);
                if (old.r == current && old.g == target && current == target)
                    continue;
                var pixel = new Color32(current, target, 0, 255);
                fogPixels[index] = pixel;
                texturePixels[index] = pixel;
                if (current != target)
                    transitions.Add(index);
            }

            RevealLandmarks();
            dirtyFog.Clear();
            fadeStarted = Time.time;
            fogMaterial.SetFloat("_BlendStart", fadeStarted);
            fog.Apply(false);
        }

        bool LineOfSight(int x, int y, int targetX, int targetY)
        {
            int dx = Mathf.Abs(targetX - x), dy = -Mathf.Abs(targetY - y);
            int sx = x < targetX ? 1 : -1, sy = y < targetY ? 1 : -1, err = dx + dy;
            while (x != targetX || y != targetY)
            {
                int twice = err * 2;
                if (twice >= dy)
                {
                    err += dy;
                    x += sx;
                }

                if (twice <= dx)
                {
                    err += dx;
                    y += sy;
                }

                if (x == targetX && y == targetY)
                    break;
                byte terrain = layout.Tiles[y * layout.Width + x];
                if (terrain == 0 || terrain == 4)
                    return false;
            }

            return true;
        }

        public void BindMinimap(RectTransform container)
        {
            if (!container)
                return;
            if (miniImage == null || miniImage.transform.parent != container)
            {
                if (miniImage)
                    Destroy(miniImage.gameObject);
                miniImage = new GameObject("已探舆图", typeof(RectTransform), typeof(CanvasRenderer), typeof(RawImage)).GetComponent<RawImage>();
                var rect = miniImage.rectTransform;
                rect.SetParent(container, false);
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = rect.offsetMax = Vector2.zero;
                var ratio = miniImage.gameObject.AddComponent<AspectRatioFitter>();
                ratio.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                ratio.aspectRatio = (float)layout.Width / layout.Height;
                miniImage.raycastTarget = false;
                miniImage.texture = minimap;
                EnableAtlas(container);
            }

            if (dirtyMini.Count > 0)
                DrawMinimap();
        }

        void DrawMinimap()
        {
            // The CPU touches only changed cells. Moving an icon costs its old and new
            // 3x3 swatches, independently of the number of rooms in the dungeon.
            foreach (var mark in markers)
            {
                if (mark.shown && mark.deathStarted < 0 && mark.cell >= 0 && miniPending[mark.cell])
                    miniSymbols[mark.cell] = (byte)((int)mark.kind + 1);
            }

            if (lastCell >= 0 && miniPending[lastCell])
                miniSymbols[lastCell] = 1;
            var pixels = minimap.GetRawTextureData<Color32>();
            foreach (int cell in dirtyMini)
            {
                var baseColor = TerrainColors[layout.Tiles[cell]];
                baseColor.a = 255;
                Color32 pigment = Uncharted;
                if (explored[cell] != 0)
                    pigment = visible[cell] ? baseColor : Color32.Lerp(Uncharted, baseColor, .41f);
                int left = (cell % layout.Width) * MiniScale;
                int firstRow = (cell / layout.Width) * MiniScale * minimap.width + left;
                for (int y = 0; y < MiniScale; y++)
                    for (int x = 0; x < MiniScale; x++)
                        pixels[firstRow + y * minimap.width + x] = pigment;
                if (explored[cell] != 0 && landmarkCells[cell] != 0)
                {
                    Color32 ink = LandmarkColors[landmarkCells[cell]-1];
                    for (int k = 0; k < 3; k++)
                    {
                        pixels[firstRow + k * minimap.width + k] = ink;
                        pixels[firstRow + k * minimap.width + 2 - k] = ink;
                    }
                }
                int symbol = miniSymbols[cell];
                if (symbol == 0 && resources.TryGetValue(cell, out var nodes))
                {
                    foreach (var node in nodes)
                        if (node.shown && node.root)
                            symbol = (int)node.kind + 1;
                }

                if (symbol > 0)
                {
                    int center = firstRow + minimap.width + 1;
                    Color32 color = MarkerColors[symbol - 1];
                    pixels[center] = pixels[center + 1] = pixels[center + minimap.width] = color;
                    if (symbol == 1)
                        pixels[center - 1] = pixels[center - minimap.width] = color;
                }

                miniPending[cell] = false;
                miniSymbols[cell] = 0;
            }

            UpdateSurvey();
            dirtyMini.Clear();
            minimap.Apply(false);
            if (atlasVeil && atlasVeil.activeSelf) DrawAtlas();
        }

        public void TickVisibleMonsters(bool physics)
        {
            foreach (var marker in activeEnemies)
            {
                if (!marker.controller || marker.controller.IsDie)
                    continue;
                var enemy = marker.controller.EntityDataSource;
                if (physics)
                    enemy.FixUpdate();
                else
                    enemy.TickAILogic();
            }
        }

        void LateUpdate()
        {
            if (!observer || layout == null)
                return;
            RefreshSight(layout.Index(observer.position));
            AnimateLandmarks();
            for (int i = markers.Count - 1; i >= 0; i--)
            {
                Marker marker = markers[i];
                if (!marker.root)
                {
                    TouchMini(marker.cell);
                    if (!ReferenceEquals(marker.controller, null))
                    {
                        activeEnemies.Remove(marker);
                        visualRoots.Remove(marker.controller);
                    }

                    markers.RemoveAt(i);
                    continue;
                }

                UpdateMarker(marker);
            }

            for (int i = nearResources.Count - 1; i >= 0; i--)
            {
                Marker node = nearResources[i];
                if (node.root)
                    UpdateMarker(node);
                else
                {
                    nearResources.RemoveAt(i);
                    if (resources.TryGetValue(node.cell, out var nodes))
                        nodes.Remove(node);
                    TouchMini(node.cell);
                }
            }

            if (dirtyMini.Count > 0 && miniImage)
                DrawMinimap();
        }

        void OnDestroy()
        {
            ReleaseWalkFrames();
            if (miniImage)
                Destroy(miniImage.gameObject);
            if (atlasVeil) Destroy(atlasVeil);
            foreach (var asset in owned)
                if (asset)
                    Destroy(asset);
        }
    }
}
