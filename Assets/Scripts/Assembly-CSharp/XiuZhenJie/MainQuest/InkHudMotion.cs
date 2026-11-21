using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkHudMotion : MonoBehaviour
	{
		private const float SlotDuration = 1.9f;
		private const float SweepDuration = .85f;
		private const float NudgeDuration = .46f;
		private static readonly Color32 WarmLight = new Color32(255, 226, 172, 98);
		private static readonly Color32 JadeLight = new Color32(209, 255, 230, 116);

		private readonly HashSet<Image> openIcons = new HashSet<Image>();
		private Image[] icons;
		private Image[] sectEmblems;
		private Image schoolEmblem, jadeArt, friendArt, friendBadge;
		private InkArtGlint glint;
		private ChatManager chat;
		private float cycleStart, nudgeStart;
		private bool nudging;

		private void Awake()
		{
			var panel = GetComponent<MainTitlePanel>();
			friendArt = panel.DaoYou_btn.image;
			schoolEmblem = panel.MenPaiIcon.GetComponent<Image>();
			jadeArt = panel.DuiHuanLingYu_btn.transform.Find("CurrencyArt").GetComponent<Image>();
			var stoneArt = panel.DuiHuanLingShi_btn.transform.Find("CurrencyArt").GetComponent<Image>();
			sectEmblems = new[] {
				panel.ZongMenIconBtn_1.image, panel.ZongMenIconBtn_2.image,
				panel.ZongMenIconBtn_3.image, panel.ZongMenIconBtn_4.image
			};
			icons = new[] {
				stoneArt, panel.DongFu_btn.image, panel.TouXiang_btn.image, panel.NaJie_btn.image,
				schoolEmblem, panel.LiLian_btn.image, jadeArt, panel.ZhanDou_btn.image, panel.CanWu_btn.image, friendArt
			};

			glint = InkArtGlint.Create(transform, WarmLight);

			var original = panel.AddRedPointImg;
			original.color = new Color32(220, 196, 165, 255);
			original.raycastTarget = false;
			friendBadge = MakeImage("FriendUnread", panel.DaoYou_btn.transform);
			friendBadge.sprite = original.sprite;
			friendBadge.color = original.color;
			var badge = friendBadge.rectTransform;
			badge.anchorMin = badge.anchorMax = new Vector2(.5f, .65f);
			badge.anchoredPosition = new Vector2(13f, 12f);
			badge.sizeDelta = new Vector2(10f, 10f);
			chat = Singleton<ChatManager>.Instance;
		}

		private static Image MakeImage(string name, Transform parent)
		{
			var node = new GameObject(name, typeof(RectTransform), typeof(Image));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			Image image = node.GetComponent<Image>();
			image.raycastTarget = false;
			return image;
		}

		private void OnEnable()
		{
			cycleStart = Time.unscaledTime;
			chat.FriendUnreadChanged += OnUnreadChanged;
			RefreshBadge();
		}

		private void OnDisable()
		{
			chat.FriendUnreadChanged -= OnUnreadChanged;
			if (glint != null) glint.Hide();
			StopNudge();
		}

		internal void SetOpen(Image icon, bool open)
		{
			if (open) openIcons.Add(icon);
			else openIcons.Remove(icon);
			if (open && glint != null && glint.transform.parent == icon.transform)
				glint.Hide();
			if (icon == friendArt)
			{
				if (open) StopNudge();
				RefreshBadge();
			}
		}

		private void RefreshBadge()
		{
			friendBadge.gameObject.SetActive(chat.HasUnreadFriendMessages && !openIcons.Contains(friendArt));
		}

		private void OnUnreadChanged()
		{
			RefreshBadge();
			if (friendBadge.isActiveAndEnabled)
			{
				nudgeStart = Time.unscaledTime;
				nudging = true;
			}
			else StopNudge();
		}

		private void StopNudge()
		{
			if (!nudging) return;
			friendArt.rectTransform.localRotation = Quaternion.identity;
			nudging = false;
		}

		private void Update()
		{
			float now = Time.unscaledTime;
			if (glint != null) SampleGlint(now - cycleStart);
			if (!nudging) return;
			float t = (now - nudgeStart) / NudgeDuration;
			if (t >= 1f || !friendBadge.isActiveAndEnabled)
			{
				StopNudge();
				return;
			}
			float angle = 4f * Mathf.Sin(2f * Mathf.PI * t) * Mathf.Sin(Mathf.PI * t);
			friendArt.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
		}

		private void SampleGlint(float elapsed)
		{
			float cycle = Mathf.Repeat(elapsed, icons.Length * SlotDuration);
			int slot = Mathf.Min((int)(cycle / SlotDuration), icons.Length - 1);
			float phase = (cycle - slot * SlotDuration) / SweepDuration;
			Image art = icons[slot];
			if (art == schoolEmblem && !art.isActiveAndEnabled)
			{
				foreach (Image emblem in sectEmblems)
				{
					if (!emblem.isActiveAndEnabled) continue;
					art = emblem;
					break;
				}
			}
			if (phase >= 1f || art == null || !art.isActiveAndEnabled || openIcons.Contains(art) || art.overrideSprite == null)
			{
				glint.Hide();
				return;
			}

			glint.Show(art, phase, art == jadeArt ? JadeLight : WarmLight);
		}
	}
}
