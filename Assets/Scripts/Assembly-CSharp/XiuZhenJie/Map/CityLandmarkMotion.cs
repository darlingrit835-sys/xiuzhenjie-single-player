using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Every animated detail is registered against the same painting as its building's hit area.
    internal sealed class CityLandmarkMotion : MonoBehaviour, IPointerDownHandler
    {
        [Serializable]
        private class Clip
        {
            public string name;
            public int left, top, right, bottom;
            public float period;
        }

        [Serializable]
        private class Sheet
        {
            public int width, height, frames, cell;
            public Clip[] rooms, sunlight;
        }

        private static Sheet sheet;
        private static Sprite[,] sprites;
        private int room;
        private Button button;
        private Image landmark, sunlight, ripple;
        private float pressedAt = float.NegativeInfinity;
        private int lastAmbient = -1, lastSun = -1, lastRipple = -1;
        private const float RippleDuration = .38f;

        internal static void Attach(RectTransform map, MainCityPanel panel, Button board)
        {
            if (sprites == null)
            {
                var manifest = ReleasedResources.Load<TextAsset>("InkInterface/CityLandmarkClips");
                var texture = ReleasedResources.Load<Texture2D>("InkInterface/CityLandmarkFrames");
                if (manifest == null || texture == null) return;
                sheet = JsonUtility.FromJson<Sheet>(manifest.text);
                int rows = sheet.rooms.Length + 1 + sheet.sunlight.Length;
                sprites = new Sprite[rows, sheet.frames];
                // Use imported dimensions so lower texture quality settings keep the cells aligned.
                float w = (float)texture.width / sheet.frames, h = (float)texture.height / rows;
                for (int row = 0; row < rows; row++)
                {
                    string name = row < sheet.rooms.Length ? sheet.rooms[row].name :
                        row == sheet.rooms.Length ? "TouchRipple" : sheet.sunlight[row - sheet.rooms.Length - 1].name;
                    for (int col = 0; col < sheet.frames; col++)
                    {
                        var bounds = new Rect(col*w, (rows-1-row)*h, w, h);
                        var sprite = Sprite.Create(texture, bounds, new Vector2(.5f,.5f), 100f, 0, SpriteMeshType.FullRect);
                        sprite.name = name + "_" + col.ToString("00");
                        sprites[row, col] = sprite;
                    }
                }
            }

            Button[] buildings = {panel.ChengZhuFuBtn, panel.XuanTianLouBtn, panel.ZhongJieFangShiBtn,
                panel.LingQuanJiuSiBtn, panel.XuShenJieBtn, panel.ChuanSongDaZhenBtn, board};
            for (int i = 0; i < buildings.Length; i++)
            {
                var target = buildings[i];
                var motion = target.gameObject.AddComponent<CityLandmarkMotion>();
                motion.room = i;
                motion.button = target;
                motion.sunlight = Place(target.transform, "BuildingReflection", sheet.sunlight[i], true);
                motion.landmark = Place(target.transform, "LandmarkFrames", sheet.rooms[i], true);
                motion.ripple = MakeImage(target.transform, "TouchRipple");
                var contact = motion.ripple.rectTransform;
                contact.anchorMin = contact.anchorMax = new Vector2(.5f,.5f);
                contact.sizeDelta = new Vector2(49f,49f);
                motion.ripple.enabled = false;
                motion.Sample(Time.unscaledTime);
            }

            int river = sheet.sunlight.Length - 1;
            Image shine = Place(map, "RiverReflection", sheet.sunlight[river], false);
            var riverMotion = shine.gameObject.AddComponent<CityLandmarkMotion>();
            riverMotion.room = river;
            riverMotion.sunlight = shine;
            riverMotion.Sample(Time.unscaledTime);
            AddReminder(panel.ChengZhuFuHintImg, false);
            AddReminder(panel.NewXinJianTagImg, true);
        }

        private static Image MakeImage(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.layer = parent.gameObject.layer;
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.raycastTarget = false;
            return image;
        }

        private static Image Place(Transform parent, string name, Clip region, bool insideButton)
        {
            var image = MakeImage(parent, name);
            var rect = image.rectTransform;
            rect.SetAsFirstSibling();
            RectTransform anchor = (RectTransform)parent;
            Vector2 min = insideButton ? anchor.anchorMin : Vector2.zero;
            Vector2 max = insideButton ? anchor.anchorMax : Vector2.one;
            rect.anchorMin = new Vector2((region.left / (float)sheet.width-min.x)/(max.x-min.x),
                (1f-region.bottom / (float)sheet.height-min.y)/(max.y-min.y));
            rect.anchorMax = new Vector2((region.right / (float)sheet.width-min.x)/(max.x-min.x),
                (1f-region.top / (float)sheet.height-min.y)/(max.y-min.y));
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            return image;
        }

        private static void AddReminder(Image marker, bool mail)
        {
            marker.raycastTarget = false;
            RectTransform rect = marker.rectTransform;
            rect.SetAsLastSibling();
            // Keep the original marker GameObject: its active state is owned by the server-backed UI.
            rect.anchorMin = rect.anchorMax = mail ? new Vector2(.79f,.78f) : new Vector2(.78f,.85f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(9f, 9f);
            var halo = MakeImage(rect, "AttentionHalo");
            halo.sprite = sprites[sheet.rooms.Length, 4];
            halo.rectTransform.sizeDelta = new Vector2(26f, 26f);
            var pulse = halo.gameObject.AddComponent<CityAttentionPulse>();
            pulse.Bind(halo, mail ? 1.65f : 0f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (button == null || !button.IsInteractable() || eventData.button != PointerEventData.InputButton.Left) return;
            var area = (RectTransform)transform;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(area, eventData.position,
                eventData.pressEventCamera, out Vector2 local)) return;
            ripple.rectTransform.anchoredPosition = local - area.rect.center;
            pressedAt = Time.unscaledTime;
            lastRipple = 0;
            ripple.sprite = sprites[sheet.rooms.Length, 0];
            ripple.enabled = true;
            Sample(pressedAt);
        }

        private void Update()
        {
            Sample(Time.unscaledTime);
        }

        private void Sample(float time)
        {
            if (sunlight == null) return;
            if (landmark != null)
            {
                int f = Mathf.FloorToInt((time / sheet.rooms[room].period + room * .31f) * sheet.frames) % sheet.frames;
                if (f != lastAmbient)
                {
                    landmark.sprite = sprites[room, f];
                    lastAmbient = f;
                }
            }

            // A roof catches daylight briefly. Most of the scene stays still between passes.
            var glint = sheet.sunlight[room];
            float cycle = landmark == null ? 5.9f : 9.7f;
            float elapsed = Mathf.Repeat(time - 1.36f * room, cycle);
            float sinceTouch = time - pressedAt;
            // A press picks out this specific roof or prop while the fingertip ripple expands.
            if (sinceTouch >= 0f && sinceTouch < RippleDuration)
                elapsed = (.19f + .81f * sinceTouch / RippleDuration) * glint.period;
            int sun = elapsed >= glint.period ? -1 : Mathf.FloorToInt(elapsed / glint.period * sheet.frames);
            if (sun >= 0 && sun != lastSun)
                sunlight.sprite = sprites[sheet.rooms.Length + 1 + room, sun];
            sunlight.enabled = sun >= 0;
            lastSun = sun;

            if (ripple == null || !ripple.enabled) return;
            int touch = Mathf.FloorToInt((time - pressedAt) * sheet.frames / RippleDuration);
            if (touch >= sheet.frames) ripple.enabled = false;
            else if (touch != lastRipple && touch >= 0)
            {
                ripple.sprite = sprites[sheet.rooms.Length, touch];
                lastRipple = touch;
            }
        }

        private void OnDisable()
        {
            pressedAt = float.NegativeInfinity;
            if (ripple != null) ripple.enabled = false;
        }
    }

    internal sealed class CityAttentionPulse : MonoBehaviour
    {
        private Image halo;
        private float offset;

        internal void Bind(Image image, float stagger)
        {
            halo = image;
            offset = stagger;
            Sample();
        }

        private void OnEnable() { if (halo != null) Sample(); }
        private void Update() { Sample(); }

        private void Sample()
        {
            if (halo == null) return;
            float t = Mathf.Repeat(Time.unscaledTime + offset, 3.6f) / 1.05f;
            halo.enabled = t < 1f;
            if (!halo.enabled) return;
            halo.rectTransform.localScale = Vector3.one * (.77f + t * .89f);
            halo.color = new Color(1f, .83f, .59f, (1f-t)*.85f);
        }
    }
}
