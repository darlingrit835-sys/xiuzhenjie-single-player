using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
    public sealed partial class BattleFieldView
    {
        const int WalkColumns = 8, WalkRows = 3;
        const int Front = 0, Back = 1, Side = 2;
        static readonly string[] WalkRowNames = { "front", "back", "side" };
        // Reloads can briefly overlap the old field. Only the last field may release a shared texture.
        sealed class WalkAtlas
        {
            public Texture2D texture;
            public Sprite[] frames;
            public int owners;
        }
        static readonly Dictionary<string, WalkAtlas> sharedWalkAtlases = new Dictionary<string, WalkAtlas>(3);
        readonly Dictionary<string, WalkAtlas> walkAtlases = new Dictionary<string, WalkAtlas>(2);
        public bool AnimatesPlayerSteps { get; private set; }

        sealed class WalkingFigure
        {
            public Sprite[] frames;
            public int row;
            public float frame = 2f;
            public float stillTime;
        }

        Sprite[] WalkFrames(string family)
        {
            if (family == null) return null;
            if (walkAtlases.TryGetValue(family, out var local)) return local?.frames;
            if (!sharedWalkAtlases.TryGetValue(family, out var sheet))
            {
                sheet = LoadWalkingAtlas(family);
                if (sheet != null) sharedWalkAtlases.Add(family, sheet);
            }
            walkAtlases.Add(family, sheet);
            if (sheet != null) sheet.owners++;
            return sheet?.frames;
        }

        static WalkAtlas LoadWalkingAtlas(string family)
        {
            string name = "Walk" + family;
            var atlas = ReleasedResources.Load<Texture2D>("BattleField/" + name);
            if (!atlas) return null;
            if (atlas.width < WalkColumns || atlas.height < WalkRows ||
                atlas.width % WalkColumns != 0 || atlas.height % WalkRows != 0)
            {
                Resources.UnloadAsset(atlas);
                return null;
            }
            int width = atlas.width / WalkColumns, height = atlas.height / WalkRows;
            var frames = new Sprite[WalkColumns * WalkRows];
            var feet = new Vector2(.5f, .08f);
            for (int row = 0; row < WalkRows; row++)
                for (int col = 0; col < WalkColumns; col++)
                {
                    var rect = new Rect(col * width, (WalkRows - 1 - row) * height, width, height);
                    var frame = Sprite.Create(atlas, rect, feet, height, 0, SpriteMeshType.FullRect);
                    frame.name = name + "_" + WalkRowNames[row] + "_" + col;
                    frames[row * WalkColumns + col] = frame;
                }
            return new WalkAtlas { texture = atlas, frames = frames };
        }

        void ReleaseWalkFrames()
        {
            foreach (var entry in walkAtlases)
            {
                var sheet = entry.Value;
                if (sheet == null || --sheet.owners > 0) continue;
                sharedWalkAtlases.Remove(entry.Key);
                foreach (var sprite in sheet.frames) Destroy(sprite);
                if (sheet.texture) Resources.UnloadAsset(sheet.texture);
            }
            walkAtlases.Clear();
        }

        static void Face(WalkingFigure gait, SpriteRenderer painter, Vector2 direction)
        {
            if (direction.sqrMagnitude < .000001f) return;
            direction.Normalize();
            // Separate enter/leave thresholds prevent alternating rows while sliding along an uneven bank.
            float sideThreshold = gait.row == Side ? .49f : .32f;
            gait.row = Mathf.Abs(direction.y) < sideThreshold ? Side : direction.y > 0 ? Back : Front;
            if (Mathf.Abs(direction.x) > .19f) painter.flipX = direction.x > 0;
        }

        static float RestingFrame(float frame)
        {
            return frame < 4f ? 2f : 6f;
        }

        void DrawWalk(Marker mark, Vector2 shift, float dt)
        {
            WalkingFigure gait = mark.walk;
            EntityBase unit = mark.controller.EntityDataSource;
            AniState state = unit != null ? unit.CurrentAniState : AniState.Idle;
            float distance = shift.magnitude;
            bool action = mark.controller.IsDie || state == AniState.Attack || state == AniState.Hit
                || state == AniState.DingShen || state == AniState.Die;
            // A reconnect/snap can move the transform by an arbitrary distance in one frame.
            bool moved = !action && dt > 0f && distance > .00015f && distance < Mathf.Max(2f, 24f * dt);
            if (moved)
            {
                gait.stillTime = 0;
                Face(gait, mark.bodyRenderer, shift);
                float scaledStride = 2.7f * Mathf.Max(.5f, mark.root.lossyScale.y);
                float advance = Mathf.Min(distance / scaledStride, 2.8f * dt) * WalkColumns;
                float next = gait.frame + advance;
                if (mark.kind == MarkerKind.Player && landmarkEffects && unit != null &&
                    Mathf.FloorToInt(next / 4f) != Mathf.FloorToInt(gait.frame / 4f))
                {
                    float shoe = Mathf.FloorToInt(next / 4f) % 2 == 0 ? -.12f : .12f;
                    landmarkEffects.Footfall(unit, shift / distance, shoe);
                }
                gait.frame = Mathf.Repeat(next, WalkColumns);
            }
            else
            {
                gait.stillTime += dt;
                if (gait.stillTime >= .065f || action)
                {
                    gait.frame = Mathf.MoveTowards(gait.frame, RestingFrame(gait.frame), 19f * dt);
                    var target = unit != null ? unit.Target : null;
                    if (!mark.controller.IsDie && target != null && target.BaseController &&
                        !target.BaseController.IsDie && target.VisibleToPlayer())
                        Face(gait, mark.bodyRenderer, target.GetPos() - mark.root.position);
                }
            }
            int column = Mathf.FloorToInt(gait.frame);
            Sprite nextFrame = gait.frames[gait.row * WalkColumns + column];
            if (mark.bodyRenderer.sprite != nextFrame) mark.bodyRenderer.sprite = nextFrame;
        }
    }
}
