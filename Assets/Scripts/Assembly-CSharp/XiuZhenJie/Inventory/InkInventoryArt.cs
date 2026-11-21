using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal static class InkInventoryArt
	{
		internal static readonly Color Selected = new Color32(208, 232, 217, 255);

		internal static void StyleTitle(Transform root)
		{
			Transform strip = root.Find("Title_image");
			SetSurface(strip, "LightPlaque");
			Text title = strip?.Find("TitleName")?.GetComponent<Text>();
			if (title == null)
				return;

			StyleLabel(title, 18);
			UIManager.FitTextToRect(title);
			title.resizeTextMinSize = 16;
		}

		internal static void SetSurface(Transform area, string art = "LightPaper")
		{
			InkInterfaceArt.Set(area?.GetComponent<Image>(), art, false);
		}

		internal static void StyleRank(Image frame, Sprite rank)
		{
			switch (rank?.name)
			{
				case "Rank_Huang":
				case "Rank_Xuan":
				case "Rank_Di":
				case "Rank_Tian":
					InkInterfaceArt.Set(frame, rank.name, false);
					break;
				default:
					if (rank == null) InkInterfaceArt.Set(frame, "LightPlaque", false);
					else
					{
						frame.type = Image.Type.Simple;
						frame.sprite = rank;
					}
					break;
			}
		}

		internal static void StyleItem(Image icon, Sprite artwork, Text name, Text element, Image countBadge, Text count)
		{
			icon.sprite = artwork != null ? artwork : InkInterfaceArt.Get("Satchel");
			icon.color = Color.white;
			InkInterfaceArt.Set(name.transform.parent.GetComponent<Image>(), "JadeButton", false);
			InkInterfaceArt.Set(element.transform.parent.GetComponent<Image>(), "LightPlaque", false);
			InkInterfaceArt.Set(countBadge, "JadeButton", false);
			name.color = count.color = InkDialogArt.Ivory;
			element.color = InkInterfaceArt.Ink;
		}

		// Keep the painting on the fixed scroll surface while item rows move above it.
		internal static void StyleLedger(Transform area)
		{
			if (area == null) return;
			SetSurface(area, "InventoryScroll");
			var viewport = area.GetComponent<ScrollRect>()?.viewport;
			if (viewport == null || viewport == area) return;
			if (viewport.TryGetComponent<Mask>(out var mask))
				mask.showMaskGraphic = false;
			else if (viewport.TryGetComponent<Image>(out var backing))
				backing.color = Color.clear;
		}

		internal static void StyleLabel(Text label, int minSize)
		{
			if (label == null)
				return;

			label.color = InkInterfaceArt.Ink;
			label.fontSize = Mathf.Max(label.fontSize, minSize);
		}
	}
}
