using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkCaveThemeSelector
	{
		private const string PreferencePrefix = "xzj.cultivation.caveTheme.";
		private static readonly string[] ThemeNames = { "随功法", "金", "木", "水", "火", "土" };

		internal static WuXing GetTheme(string roleId)
		{
			if (string.IsNullOrEmpty(roleId))
			{
				return WuXing.All;
			}
			int saved = PlayerPrefs.GetInt(PreferencePrefix + roleId, 0);
			return saved >= 0 && saved < ThemeNames.Length ? (WuXing)saved : WuXing.All;
		}

		internal static void Attach(DongFuPanel panel, string roleId, Action<WuXing> selectTheme)
		{
			WuXing selected = GetTheme(roleId);
			Font font = panel.DongFuNameText.font;
			RectTransform dock = FindOrCreate(panel.transform, "InkCaveThemeSelector");
			dock.anchorMin = dock.anchorMax = dock.pivot = new Vector2(0f, 1f);
			dock.anchoredPosition = new Vector2(13f, -34f);
			dock.sizeDelta = new Vector2(252f, 29f);
			dock.SetAsLastSibling();

			RectTransform openerRect = FindOrCreate(dock, "ThemeButton");
			openerRect.anchorMin = openerRect.anchorMax = openerRect.pivot = new Vector2(0f, 1f);
			openerRect.anchoredPosition = Vector2.zero;
			openerRect.sizeDelta = new Vector2(108f, 29f);
			Button opener = GetButton(openerRect, font);
			SetButtonStyle(opener, false, "洞府 · " + ThemeNames[(int)selected]);

			RectTransform options = FindOrCreate(dock, "ThemeOptions");
			options.anchorMin = options.anchorMax = options.pivot = new Vector2(0f, 1f);
			options.anchoredPosition = new Vector2(0f, -32f);
			options.sizeDelta = new Vector2(252f, 32f);
			Image backing = options.GetComponent<Image>() ?? options.gameObject.AddComponent<Image>();
			InkInterfaceArt.Set(backing, "Paper", false);
			backing.raycastTarget = false;

			var chips = new Button[ThemeNames.Length];
			for (int i = 0; i < chips.Length; i++)
			{
				WuXing choice = (WuXing)i;
				RectTransform chipRect = FindOrCreate(options, "Theme" + i);
				float left = i == 0 ? 0f : 0.245f + (i - 1) * 0.151f;
				float right = 0.245f + i * 0.151f;
				chipRect.anchorMin = new Vector2(left, 0f);
				chipRect.anchorMax = new Vector2(right, 1f);
				chipRect.offsetMin = new Vector2(2f, 3f);
				chipRect.offsetMax = new Vector2(-2f, -3f);
				Button chip = GetButton(chipRect, font);
				chips[i] = chip;
				SetButtonStyle(chip, choice == selected, ThemeNames[i]);
				chip.onClick.RemoveAllListeners();
				chip.onClick.AddListener(() =>
				{
					if (!string.IsNullOrEmpty(roleId))
					{
						PlayerPrefs.SetInt(PreferencePrefix + roleId, (int)choice);
						PlayerPrefs.Save();
					}
					SetButtonStyle(opener, false, "洞府 · " + ThemeNames[(int)choice]);
					for (int label = 0; label < chips.Length; label++)
					{
						SetButtonStyle(chips[label], (int)choice == label, ThemeNames[label]);
					}
					options.gameObject.SetActive(false);
					selectTheme?.Invoke(choice);
				});
			}

			options.gameObject.SetActive(false);
			opener.onClick.RemoveAllListeners();
			opener.onClick.AddListener(() => options.gameObject.SetActive(!options.gameObject.activeSelf));
			selectTheme?.Invoke(selected);
		}

		private static Button GetButton(RectTransform rect, Font font)
		{
			Image graphic = rect.GetComponent<Image>() ?? rect.gameObject.AddComponent<Image>();
			Button button = rect.GetComponent<Button>() ?? rect.gameObject.AddComponent<Button>();
			button.targetGraphic = graphic;
			RectTransform labelRect = FindOrCreate(rect, "Label");
			labelRect.anchorMin = Vector2.zero;
			labelRect.anchorMax = Vector2.one;
			labelRect.offsetMin = new Vector2(4f, 0f);
			labelRect.offsetMax = new Vector2(-4f, 0f);
			Text label = labelRect.GetComponent<Text>() ?? labelRect.gameObject.AddComponent<Text>();
			label.font = font;
			label.fontSize = 12;
			label.alignment = TextAnchor.MiddleCenter;
			label.resizeTextForBestFit = true;
			label.resizeTextMinSize = 9;
			label.resizeTextMaxSize = 12;
			label.raycastTarget = false;
			return button;
		}

		private static void SetButtonStyle(Button button, bool selected, string text)
		{
			InkInterfaceArt.Set(button.image, "LightPlaque", false);
			button.image.color = selected ? new Color32(193, 229, 213, 255) : Color.white;
			Text label = button.transform.Find("Label").GetComponent<Text>();
			label.text = text;
			label.color = InkInterfaceArt.Ink;
			label.fontStyle = selected ? FontStyle.Bold : FontStyle.Normal;
		}

		private static RectTransform FindOrCreate(Transform parent, string name)
		{
			if (parent.Find(name) is RectTransform existing)
			{
				return existing;
			}
			var node = new GameObject(name, typeof(RectTransform));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			return (RectTransform)node.transform;
		}
	}
}
