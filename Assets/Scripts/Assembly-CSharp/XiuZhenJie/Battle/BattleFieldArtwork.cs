using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
    public sealed partial class BattleFieldView
    {
        readonly Dictionary<Controller, Transform> visualRoots = new Dictionary<Controller, Transform>();
        Sprite spiritHalo, lockRing;
        Material figureInk;
        Sprite[] beasts, forage;
        // Existing battle node packets identify these regional materials by display name.
        static readonly Dictionary<string, int> forageArt = new Dictionary<string, int>
        {
            { "凝露草", 0 }, { "灵草", 1 }, { "梦泽莲", 2 }, { "赤绒苔", 3 }, { "锁砂棘", 4 }, { "霜节芦", 5 },
            { "听风青石", 6 }, { "玄铁矿石", 7 }, { "雾镜云母", 8 }, { "阳燧晶", 9 }, { "赤烈砂", 10 }, { "寒渊银砂", 11 }
        };
        static readonly int[] herbElements = { 0, 5, 1, 2, 3, 4 };
        static readonly Color[] lootGrades =
        {
            new Color(.59f,.82f,.55f,.74f), new Color(.42f,.78f,.94f,.79f),
            new Color(.77f,.57f,.94f,.82f), new Color(1f,.73f,.33f,.87f)
        };
        static readonly int[] oreElements = { 7, 7, 6, 11, 9, 10 };
        // Two bilinear control maps preserve the server topology while the brush atlas supplies near-field detail.
        void BuildLandscape()
        {
            var relief = Texture(layout.Width, layout.Height, "崖岸林径", FilterMode.Bilinear, true);
            var sites = Texture(layout.Width, layout.Height, "石场遗址", FilterMode.Bilinear, true);
            var reliefPixels = relief.GetRawTextureData<Color32>();
            var sitePixels = sites.GetRawTextureData<Color32>();
            for (int i = 0; i < layout.Tiles.Length; i++)
            {
                byte tile = layout.Tiles[i];
                reliefPixels[i] = new Color32(Channel(tile == 0), Channel(tile == 4), Channel(tile == 2), Channel(tile == 1));
                sitePixels[i] = new Color32(Channel(tile == 3), Channel(tile == 5), Channel(tile == 6), 255);
            }

            relief.Apply(false, true);
            sites.Apply(false, true);
            var material = Plane("青绿山川", relief, "BattleTerrain", -100);
            material.SetTexture("_Sites", sites);
            material.SetTexture("_Pigments", ReleasedResources.Load<Texture2D>("BattleField/TerrainPigments"));
            material.SetColor("_Sunlight", ProvinceLight(layout.RegionId));
            PaintGroves(LoadSheet("Landscape"));
            figureInk = Own(new Material(ReleasedResources.Load<Shader>("shaders/BattleFigure")));
            beasts = LoadSheet("SpiritBeasts");
            forage = LoadSheet("Forage", 4);
            spiritHalo = MakeHalo(false);
            lockRing = MakeHalo(true);
        }

        static byte Channel(bool present) => present ? (byte)255 : (byte)0;

        static Color ProvinceLight(string id)
        {
            switch (id)
            {
                case "1112": return new Color(.98f, 1.05f, 1.08f); // 朝露，晨光
                case "1121": return new Color(.92f, .99f, 1.13f); // 云梦，水青
                case "1211": return new Color(1.14f, .95f, .97f); // 紫霞，晚照
                case "1311": return new Color(1.16f, 1.02f, .84f); // 沙海，黄沙
                case "1411": return new Color(.88f, 1.02f, 1.18f); // 寒渊，雪光
                default: return new Color(1.035f, 1.025f, .97f);
            }
        }

        void PaintGroves(Sprite[] art)
        {
            var ink = Own(new Material(Shader.Find("Sprites/Default")) { mainTexture = art[0].texture });
            var occupied = new bool[layout.Tiles.Length];
            const int sectorSize = 12;
            for (int sy = 0; sy < layout.Height; sy += sectorSize)
                for (int sx = 0; sx < layout.Width; sx += sectorSize)
                {
                    var batch = new MeshInk();
                    for (int y = Mathf.Min(sy + sectorSize, layout.Height) - 1; y >= sy; y--)
                        for (int x = sx; x < Mathf.Min(sx + sectorSize, layout.Width); x++)
                        {
                            int cell = y * layout.Width + x;
                            if (occupied[cell] || layout.NearLandmark(cell, 1)) continue;
                            byte tile = layout.Tiles[cell];
                            uint seed = Hash((uint)cell);
                            int motif = SelectGrove(x, y, tile, seed);
                            if (motif < 0) continue;

                            Vector2 centre = layout.Center(cell);
                            float scatterX = ((seed >> 9) % 101) * .012f - .6f;
                            float scatterY = ((seed >> 20) % 101) * .008f - .4f;
                            centre += new Vector2(scatterX, scatterY);
                            bool tall = motif <= 4;
                            float scale = tall ? 4.35f : motif == 6 ? 3.05f : 2.65f;
                            scale *= .86f + (seed % 53) * .01f;
                            if (Vector2.SqrMagnitude(centre - (Vector2)observer.position) < 3.5f) continue;
                            if (tall)
                            {
                                // Overlapping crowns read as forest at the edge; leave a channel through each pocket.
                                for (int yy = Mathf.Max(0, y - 1); yy <= Mathf.Min(layout.Height - 1, y + 1); yy++)
                                    for (int xx = Mathf.Max(0, x - 1); xx <= Mathf.Min(layout.Width - 1, x + 1); xx++)
                                        occupied[yy * layout.Width + xx] = true;
                            }
                            batch.Stamp(art[motif], centre, scale, (seed & 256) != 0);
                        }
                    if (batch.Empty) continue;

                    var go = new GameObject("山岩林相");
                    go.transform.SetParent(transform, false);
                    go.AddComponent<MeshFilter>().sharedMesh = Own(batch.Build());
                    var renderer = go.AddComponent<MeshRenderer>();
                    renderer.sharedMaterial = ink;
                    renderer.sortingOrder = -75;
                }
        }

        bool Borders(int x, int y, byte tile)
        {
            return (x > 0 && layout.Tiles[y * layout.Width + x - 1] == tile)
                || (x + 1 < layout.Width && layout.Tiles[y * layout.Width + x + 1] == tile)
                || (y > 0 && layout.Tiles[(y - 1) * layout.Width + x] == tile)
                || (y + 1 < layout.Height && layout.Tiles[(y + 1) * layout.Width + x] == tile);
        }

        int SelectGrove(int x, int y, byte tile, uint seed)
        {
            int chance = (int)((seed >> 12) % 100);
            float pocket = Mathf.PerlinNoise(x * .19f + 11.3f, y * .19f + 17.6f);
            switch (tile)
            {
                case 0:
                    if (chance >= (Borders(x, y, 1) || Borders(x, y, 2) ? 48 : 25)) return -1;
                    return pocket > .54f ? 3 : 4;
                case 2:
                    if (chance >= 51) return -1;
                    if (Borders(x, y, 4)) return chance < 18 ? 6 : 1;
                    if (pocket < .30f || (layout.RegionId == "1211" && pocket < .52f)) return 2;
                    if (pocket > .58f || layout.RegionId == "1112") return 1;
                    return chance < 10 ? 5 : 0;
                case 3: return chance < 14 ? 4 : chance < 23 ? 5 : -1;
                case 4: return chance < 29 && (Borders(x, y, 2) || Borders(x, y, 1)) ? 6 : -1;
                case 5: return chance < 9 ? 7 : chance < 18 ? 5 : -1;
                case 6: return chance < 10 ? 8 : chance < 17 ? 4 : -1;
                default: return chance < 7 && Borders(x, y, 2) ? 5 : -1;
            }
        }

        Sprite[] LoadSheet(string name, int columns = 3)
        {
            var texture = ReleasedResources.Load<Texture2D>("BattleField/" + name);
            int width = texture.width / columns, height = texture.height / 3;
            var pieces = new Sprite[columns * 3];
            for (int i = 0; i < pieces.Length; i++)
            {
                var rect = new Rect(i % columns * width, (2 - i / columns) * height, width, height);
                pieces[i] = Own(Sprite.Create(texture, rect, new Vector2(.5f, .5f), height, 0, SpriteMeshType.FullRect));
                pieces[i].name = name + (i + 1);
            }

            return pieces;
        }

        sealed class MeshInk
        {
            readonly List<Vector3> positions = new List<Vector3>();
            readonly List<Vector2> uv = new List<Vector2>();
            readonly List<Color32> colors = new List<Color32>();
            readonly List<int> triangles = new List<int>();
            public bool Empty => positions.Count == 0;
            public void Stamp(Sprite art, Vector2 center, float size, bool flip)
            {
                int start = positions.Count;
                Rect rect = art.rect;
                float x0 = rect.xMin / art.texture.width, x1 = rect.xMax / art.texture.width;
                float y0 = rect.yMin / art.texture.height, y1 = rect.yMax / art.texture.height;
                if (flip)
                {
                    float tmp = x0;
                    x0 = x1;
                    x1 = tmp;
                }

                float h = size * .5f;
                positions.Add(new Vector3(center.x - h, center.y - h, .6f));
                positions.Add(new Vector3(center.x + h, center.y - h, .6f));
                positions.Add(new Vector3(center.x + h, center.y + h, .6f));
                positions.Add(new Vector3(center.x - h, center.y + h, .6f));
                uv.Add(new Vector2(x0, y0));
                uv.Add(new Vector2(x1, y0));
                uv.Add(new Vector2(x1, y1));
                uv.Add(new Vector2(x0, y1));
                for (int i = 0; i < 4; i++)
                    colors.Add(new Color32(226, 235, 229, 255));
                triangles.Add(start);
                triangles.Add(start + 2);
                triangles.Add(start + 1);
                triangles.Add(start);
                triangles.Add(start + 3);
                triangles.Add(start + 2);
            }

            public Mesh Build()
            {
                var mesh = new Mesh{name = "水墨地景"};
                mesh.SetVertices(positions);
                mesh.SetUVs(0, uv);
                mesh.SetColors(colors);
                mesh.SetTriangles(triangles, 0);
                mesh.UploadMeshData(true);
                return mesh;
            }
        }

        static uint Hash(uint n)
        {
            unchecked
            {
                n += 1231;
                n = (n ^ (n >> 16)) * 0x45d9f3bu;
                n = (n ^ (n >> 16)) * 0x45d9f3bu;
                return n ^ (n >> 16);
            }
        }

        Sprite MakeHalo(bool targeting)
        {
            const int size = 64;
            var texture = Texture(size, size, "灵光", FilterMode.Bilinear);
            var pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float radius = new Vector2((x + .5f) / size * 2 - 1, (y + .5f) / size * 2 - 1).magnitude;
                    float inner = Mathf.Clamp01(1 - radius), rim = Mathf.Clamp01(1 - Mathf.Abs(radius - .68f) * 20f);
                    float edge = Mathf.Clamp01(1 - Mathf.Abs(radius - .87f) * 35);
                    float ticks = Mathf.Abs(x - size * .5f) < 2 || Mathf.Abs(y - size * .5f) < 2 ? 1 : 0;
                    float lockAlpha = edge * .78f + ticks * Mathf.Clamp01(1 - Mathf.Abs(radius - .79f) * 9);
                    pixels[y * size + x] = new Color(1, 1, 1, targeting ? lockAlpha : inner * inner * .64f + rim * .32f);
                }

            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            return Own(Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(.5f, .5f), size, 0, SpriteMeshType.FullRect));
        }

        public Transform GetVisual(Controller actor)
        {
            return actor && visualRoots.TryGetValue(actor, out Transform root) ? root : null;
        }

        static SpriteRenderer ChildArt(Transform parent, string name, Sprite art, float height, int order)
        {
            var r = new GameObject(name).AddComponent<SpriteRenderer>();
            r.transform.SetParent(parent, false);
            r.sprite = art;
            r.sortingOrder = order;
            r.transform.localScale = Vector3.one * (height / Mathf.Max(.01f, art.bounds.size.y));
            return r;
        }

        Transform Aura(Transform parent, MarkerKind kind)
        {
            var aura = ChildArt(parent, "灵息", spiritHalo, 1, -2);
            aura.color = MarkerColors[(int)kind];
            aura.transform.localScale = new Vector3(2.1f, .82f, 1);
            return aura.transform;
        }

        public Sprite PortraitFor(BattleLifeModel data)
        {
            if (data == null)
                return null;
            if (data.ObjectType % 1000 == 1)
            {
                var walk = WalkFrames("Wanderer");
                return walk != null ? walk[2] : ReleasedResources.Load<Sprite>("BattleField/Wanderer");
            }
            if (beasts != null && data.Icon >= 1 && data.Icon <= beasts.Length)
                return beasts[data.Icon - 1];
            return Singleton<FacadeManager>.Instance.SelectImgSprite(0, 1, 5);
        }

        public void RegisterActor(Controller controller, BattleLifeModel data, bool self)
        {
            MarkerKind kind = self ? MarkerKind.Player : data.ObjectType % 1000 == 1 ? MarkerKind.Cultivator : MarkerKind.Beast;
            string family = kind == MarkerKind.Cultivator ? "Wanderer" : null;
            if (self)
            {
                var role = Singleton<RoleManager>.Instance.Role_Entity;
                family = role == null || role.Sex == 1 ? "Swordsman" : "Swordswoman";
            }
            var frames = WalkFrames(family);
            Sprite art = frames != null ? frames[2] : family != null
                ? ReleasedResources.Load<Sprite>("BattleField/" + family) : PortraitFor(data);
            if (!art) art = controller.TouXiangSprite.sprite;
            var motion = new GameObject("施法动作").transform;
            motion.SetParent(controller.transform, false);
            visualRoots[controller] = motion;
            var body = new GameObject("步态").transform;
            body.SetParent(motion, false);
            var halo = Aura(controller.transform, kind);
            SpriteRenderer painter = null;
            if (art)
            {
                float height = self ? 2.45f : kind == MarkerKind.Cultivator ? 2.35f : Mathf.Lerp(1.95f, 3.25f, (data.Type - 1) / 4f);
                painter = ChildArt(body, "战场立绘", art, height, 5);
                painter.sharedMaterial = figureInk;
                if (frames == null) painter.transform.localPosition = new Vector3(0, height * .36f, 0);
                // Portrait colliders and perception triggers remain active as we replace their old paintings.
                foreach (var r in controller.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    string n = r.gameObject.name;
                    if (n == "Bg" || n == "Mask" || n == "PlayerIcon" || n == "PlayerIconBg" || n == "JianTou" || n == "ShenShiArea")
                        r.enabled = false;
                }
            }

            if (controller.MonsterCanvasRect)
            {
                var c = controller.MonsterCanvasRect;
                c.localPosition = new Vector3(0, kind == MarkerKind.Beast ? 1.65f : 2.12f, -.15f);
                c.localScale = Vector3.one * .31f;
                var canvas = c.GetComponent<Canvas>();
                if (canvas)
                {
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = 8;
                }
            }

            if (controller is NormalMonsterController enemy)
            {
                int separator = data.Name.IndexOf('·');
                enemy.NameText.text = separator >= 0 ? data.Name.Substring(separator + 1) : data.Name;
                enemy.NameText.color = new Color(.98f, .96f, .82f);
                var outline = enemy.NameText.gameObject.GetComponent<UnityEngine.UI.Outline>() ?? enemy.NameText.gameObject.AddComponent<UnityEngine.UI.Outline>();
                outline.effectColor = new Color(.15f, .20f, .19f, .9f);
                outline.effectDistance = new Vector2(.05f, -.05f);
                enemy.NameText.gameObject.SetActive(false);
                var lockArt = enemy.LockedSprite.GetComponent<SpriteRenderer>();
                lockArt.sprite = lockRing;
                lockArt.sortingOrder = 1;
                lockArt.color = new Color(1f, .76f, .36f, .90f);
                lockArt.transform.localPosition = new Vector3(0, -.04f, -.10f);
                lockArt.transform.localRotation = Quaternion.identity;
                lockArt.transform.localScale = new Vector3(1.68f, .59f, 1);
            }

            var mark = AddMarker(controller.gameObject, kind);
            mark.body = body;
            mark.halo = halo;
            mark.controller = controller;
            mark.bodyRenderer = painter;
            if (frames != null) mark.walk = new WalkingFigure { frames = frames };
            if (self) AnimatesPlayerSteps = mark.walk != null;
            if (!self && mark.shown)
                activeEnemies.Add(mark);
        }

        public void RegisterResource(GameObject node, BattleNoLifeModel data, MarkerKind kind)
        {
            if (data.Name == null || !forageArt.TryGetValue(data.Name, out int variant))
            {
                int element = Mathf.Clamp(data.FiveElement, 0, 5);
                variant = kind == MarkerKind.Herb ? herbElements[element] : oreElements[element];
            }
            var icon = forage[variant];
            var original = node.GetComponentInChildren<SpriteRenderer>();
            if (icon && original)
            {
                original.sprite = icon;
                original.sortingOrder = 3;
                original.transform.localScale = Vector3.one * (1.50f / Mathf.Max(.01f, icon.bounds.size.y * node.transform.lossyScale.y));
            }

            var halo = Aura(node.transform, kind);
            var mark = AddMarker(node, kind);
            mark.halo = halo;
        }

        public void RegisterLoot(GameObject node, int rank, bool spiritStone)
        {
            // The legacy reward animator paints a full-height beacon on its collider.
            // Keep the collider at the server position and give the drop an independent scale.
            var beacon = node.GetComponent<Animator>();
            if (beacon) beacon.enabled = false;
            foreach (var original in node.GetComponentsInChildren<SpriteRenderer>(true))
                original.enabled = false;

            float scale = Mathf.Max(.01f, node.transform.lossyScale.y);
            Sprite icon = spiritStone ? forage[11] : ReleasedResources.Load<Sprite>("InkInterface/Satchel");
            var objectArt = ChildArt(node.transform, spiritStone ? "灵石晶簇" : "战利品袋", icon,
                (spiritStone ? .72f : .91f) / scale, 3);
            var aura = Aura(node.transform, MarkerKind.Loot);
            aura.GetComponent<SpriteRenderer>().color = spiritStone ? new Color(.62f,.88f,1f,.77f)
                : lootGrades[Mathf.Clamp((rank - 1) / 4, 0, lootGrades.Length - 1)];
            var mark = AddMarker(node, MarkerKind.Loot);
            mark.body = objectArt.transform;
            mark.halo = aura;
        }

        public void RegisterMarker(GameObject go, MarkerKind kind)
        {
            AddMarker(go, kind);
        }

        Marker AddMarker(GameObject go, MarkerKind kind)
        {
            var mark = new Marker
            {
                root = go.transform,
                kind = kind,
                shown = true,
                renderers = go.GetComponentsInChildren<Renderer>(true),
                canvases = go.GetComponentsInChildren<Canvas>(true),
                lastPosition = go.transform.position,
                stride = go.transform.position.sqrMagnitude % 4f
            };
            bool anchored = kind == MarkerKind.Herb || kind == MarkerKind.Ore || kind == MarkerKind.Portal || kind == MarkerKind.Loot;
            if (anchored)
            {
                int cell = layout.Index(go.transform.position);
                if (!resources.TryGetValue(cell, out var nodes))
                    resources.Add(cell, nodes = new List<Marker>(1));
                nodes.Add(mark);
            }
            else
                markers.Add(mark);
            UpdateMarker(mark);
            if (anchored && mark.shown)
                nearResources.Add(mark);
            return mark;
        }

        // The old death clip animates the portrait prefab. Animate the painted body on
        // its own pivot while the existing combat and reward callbacks finish normally.
        void DrawDefeat(Marker mark, NormalMonsterController actor)
        {
            if (mark.deathStarted < 0)
            {
                mark.deathStarted = Time.time;
                activeEnemies.Remove(mark);
                TouchMini(mark.cell);
                actor.LockedSprite.SetActive(false);
                actor.NameText.gameObject.SetActive(false);
                actor.BloodImg.transform.parent.gameObject.SetActive(false);
                actor.JianRenImg.transform.parent.gameObject.SetActive(false);
                if (mark.halo) mark.halo.gameObject.SetActive(false);
            }
            if (!mark.bodyRenderer || !mark.bodyRenderer.enabled) return;
            const float duration = .54f;
            float progress = Mathf.Clamp01((Time.time - mark.deathStarted) / duration);
            float fall = Mathf.SmoothStep(0, 1, Mathf.Min(1, progress / .62f));
            float direction = mark.bodyRenderer.flipX ? 1f : -1f;
            mark.body.localPosition = new Vector3(direction * .22f * fall, -.09f * fall, 0);
            mark.body.localRotation = Quaternion.Euler(0, 0, direction * 76f * fall);
            mark.body.localScale = new Vector3(1, 1 - .18f * fall, 1);
            float haze = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(.25f, 1f, progress));
            mark.bodyRenderer.color = Color.Lerp(Color.white, new Color(.50f,.65f,.64f,0), haze);
            if (progress >= 1) mark.bodyRenderer.enabled = false;
        }

        void UpdateMarker(Marker mark)
        {
            Vector3 pos = mark.root.position;
            int cell = layout.Index(pos);
            bool shown = mark.root.gameObject.activeInHierarchy && (mark.kind == MarkerKind.Player || (cell >= 0 && visible[cell]));
            if (shown != mark.shown)
            {
                foreach (var r in mark.renderers)
                    if (r)
                        r.forceRenderingOff = !shown;
                foreach (var c in mark.canvases)
                    if (c)
                        c.enabled = shown;
                TouchMini(mark.cell);
                TouchMini(cell);
                mark.shown = shown;
                bool anchored = mark.kind == MarkerKind.Herb || mark.kind == MarkerKind.Ore || mark.kind == MarkerKind.Portal || mark.kind == MarkerKind.Loot;
                if (anchored)
                {
                    if (shown)
                        nearResources.Add(mark);
                    else
                        nearResources.Remove(mark);
                }

                if (mark.controller is NormalMonsterController)
                {
                    if (shown && !mark.controller.IsDie)
                        activeEnemies.Add(mark);
                    else
                        activeEnemies.Remove(mark);
                }
            }

            if (cell != mark.cell && shown)
            {
                TouchMini(mark.cell);
                TouchMini(cell);
            }

            mark.cell = cell;
            var shift = pos - mark.lastPosition;
            mark.lastPosition = pos;
            if (mark.controller is NormalMonsterController fallen && fallen.IsDie)
            {
                DrawDefeat(mark, fallen);
                return;
            }
            if (!shown)
                return;
            if (mark.controller is NormalMonsterController monster)
            {
                bool locked = monster.LockedSprite.activeSelf;
                if (monster.NameText.gameObject.activeSelf != locked)
                    monster.NameText.gameObject.SetActive(locked);
            }

            float t = Time.time;
            if (mark.walk != null) DrawWalk(mark, shift, Time.deltaTime);
            else if (mark.body && mark.bodyRenderer)
            {
                float distance = shift.magnitude;
                mark.stride += Mathf.Min(.4f, distance) * 5.1f;
                float sway = distance > .0001f ? Mathf.Sin(mark.stride) : Mathf.Sin(t * 2.2f + mark.stride) * .29f;
                mark.body.localPosition = new Vector3(0, Mathf.Abs(sway) * .085f, 0);
                mark.body.localRotation = Quaternion.Euler(0, 0, sway * 2.5f);
                if (Mathf.Abs(shift.x) > .003f)
                    mark.bodyRenderer.flipX = shift.x > 0;
            }

            if (mark.kind == MarkerKind.Loot && mark.body)
            {
                float lift = .13f + Mathf.Sin(t * 2.7f + mark.stride) * .045f;
                mark.body.localPosition = Vector3.up * (lift / mark.root.lossyScale.y);
            }
            if (mark.halo)
            {
                float pulse = Mathf.Sin(t * 2.55f + mark.stride) * .12f;
                float parentScale = Mathf.Max(.01f, mark.root.lossyScale.x);
                float width = mark.kind == MarkerKind.Loot ? 1.12f : 2.0f;
                mark.halo.localScale = new Vector3(width + pulse, (width + pulse) * .39f, 1) / parentScale;
            }
        }
    }
}
