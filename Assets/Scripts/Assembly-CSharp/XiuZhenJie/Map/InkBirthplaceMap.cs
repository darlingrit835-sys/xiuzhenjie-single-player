using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	[DisallowMultipleComponent]
	internal sealed class InkBirthplaceMap : MonoBehaviour
	{
		private const string ViewName = "InkBirthplaceLocations";

		private static readonly Color LockedInk = new Color32(103, 102, 88, 255);
		private static readonly Color SecondaryInk = new Color32(100, 86, 61, 255);

		private sealed class LocationCard
		{
			internal RectTransform Root;
			internal RectTransform Arrival;
			internal CanvasGroup Fade;
			internal Button Button;
			internal Image Illustration;
			internal Image Portrait;
			internal Text Name;
			internal Text Range;
			internal InkSelectionMotion Motion;
			internal CSZ_CityItemPanel Source;
			internal bool Selected;
		}

		private readonly List<LocationCard> cards = new List<LocationCard>();
		private readonly List<(CanvasGroup Group, float Alpha, bool Raycasts)> legacyGraphics =
			new List<(CanvasGroup, float, bool)>();
		private RectTransform view;
		private Text emptyMessage;
		private Image background;
		private Sequence reveal;

		internal static void Show(CSZ_MapPanel panel, int count)
		{
			if (!panel.TryGetComponent(out InkBirthplaceMap map))
			{
				map = panel.gameObject.AddComponent<InkBirthplaceMap>();
			}
			map.Present(panel, count);
		}

		internal static void Hide(CSZ_MapPanel panel)
		{
			if (panel.TryGetComponent(out InkBirthplaceMap map))
			{
				map.StopReveal();
				foreach (var legacy in map.legacyGraphics)
				{
					if (legacy.Group != null)
					{
						legacy.Group.alpha = legacy.Alpha;
						legacy.Group.blocksRaycasts = legacy.Raycasts;
					}
				}
				map.legacyGraphics.Clear();
				if (map.view != null)
				{
					map.view.gameObject.SetActive(false);
				}
			}
		}

		internal static void SetSelected(CSZ_CityItemPanel source, bool selected)
		{
			InkBirthplaceMap map = source.GetComponentInParent<InkBirthplaceMap>();
			if (map == null || map.view == null || !map.view.gameObject.activeSelf)
			{
				return;
			}
			foreach (LocationCard card in map.cards)
			{
				if (card.Source == source)
				{
					map.UpdateSelection(card, selected, animate: true);
					return;
				}
			}
		}

		private void Present(CSZ_MapPanel panel, int count)
		{
			StopReveal();
			if (view == null)
			{
				Transform copiedView = transform.Find(ViewName);
				if (copiedView != null)
				{
					copiedView.name = ViewName + " Retired";
					copiedView.gameObject.SetActive(false);
					Destroy(copiedView.gameObject);
				}
				view = Rect(transform, ViewName, Vector2.zero, Vector2.one);
				// Some legacy map templates lay out their direct children as city cells.
				view.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
				emptyMessage = Label(view, "Empty", new Vector2(0.1f, 0.46f), new Vector2(0.9f, 0.56f), 17);
			}
			view.gameObject.SetActive(true);
			view.SetAsLastSibling();
			HideLegacyGraphics(panel);
			background = panel.GetComponent<Image>();

			count = Mathf.Clamp(count, 0, panel.BtnDataList.Count);
			Font font = panel.GetComponentInParent<WorldMapPanel>()?.TitleText?.font;
			emptyMessage.font = font;
			emptyMessage.text = "此处暂无可选地点";
			emptyMessage.color = InkInterfaceArt.Ink;
			emptyMessage.gameObject.SetActive(count == 0);

			while (cards.Count < count)
			{
				cards.Add(CreateCard());
			}
			reveal = DOTween.Sequence().SetUpdate(true);
			if (background != null)
			{
				background.color = new Color(1f, 1f, 1f, 0.53f);
				reveal.Join(background.DOFade(1f, 0.34f).SetEase(Ease.OutQuad));
			}

			for (int i = 0; i < cards.Count; i++)
			{
				LocationCard card = cards[i];
				if (i >= count)
				{
					card.Source = null;
					card.Root.gameObject.SetActive(false);
					continue;
				}
				card.Root.gameObject.SetActive(true);
				Layout(card.Root, i, count);
				Bind(card, panel.BtnDataList[i], font);
				card.Fade.alpha = 0f;
				card.Fade.blocksRaycasts = false;
				card.Arrival.anchoredPosition = new Vector2(0f, -18f);

				float delay = 0.07f + i * 0.034f;
				float opacity = card.Button.interactable ? 1f : 0.67f;
				reveal.Insert(delay, card.Fade.DOFade(opacity, 0.25f).SetEase(Ease.OutQuad));
				reveal.Insert(delay, card.Arrival.DOAnchorPosY(0f, 0.37f).SetEase(Ease.OutCubic));
				reveal.InsertCallback(delay + 0.21f, () => card.Fade.blocksRaycasts = true);
			}
		}

		private void HideLegacyGraphics(CSZ_MapPanel panel)
		{
			if (legacyGraphics.Count > 0)
			{
				return;
			}
			foreach (CSZ_CityItemPanel place in panel.BtnDataList)
			{
				if (!place.TryGetComponent(out CanvasGroup group))
				{
					group = place.gameObject.AddComponent<CanvasGroup>();
				}
				legacyGraphics.Add((group, group.alpha, group.blocksRaycasts));
				group.alpha = 0f;
				group.blocksRaycasts = false;
			}
		}

		private LocationCard CreateCard()
		{
			RectTransform root = Rect(view, "Location", Vector2.zero, Vector2.one);
			var fade = root.gameObject.AddComponent<CanvasGroup>();
			var area = root.gameObject.AddComponent<Image>();
			area.color = Color.clear;
			var button = root.gameObject.AddComponent<Button>();
			button.targetGraphic = area;
			button.transition = Selectable.Transition.None;
			RectTransform arrival = Rect(root, "Arrival", Vector2.zero, Vector2.one);
			RectTransform art = Rect(arrival, "Art", Vector2.zero, Vector2.one);
			Image brush = Picture(art, "Plaque", new Vector2(0.04f, 0.025f), new Vector2(0.96f, 0.42f));
			InkInterfaceArt.Set(brush, "IvoryPlaque", false);
			Image illustration = Picture(art, "Illustration", new Vector2(0.18f, 0.36f), new Vector2(0.82f, 0.99f));
			Image portrait = Picture(art, "Portrait", new Vector2(0.69f, 0.66f), new Vector2(0.90f, 0.91f));
			portrait.preserveAspect = true;
			var card = new LocationCard
			{
				Root = root,
				Arrival = arrival,
				Fade = fade,
				Button = button,
				Illustration = illustration,
				Portrait = portrait,
				Name = Label(art, "Name", new Vector2(0.095f, 0.18f), new Vector2(0.905f, 0.39f), 17),
				Range = Label(art, "Level", new Vector2(0.10f, 0.04f), new Vector2(0.9f, 0.19f), 12),
				Motion = InkSelectionMotion.Attach(button, art)
			};
			button.onClick.AddListener(() => Choose(card));
			return card;
		}

		private void Bind(LocationCard card, CSZ_CityItemPanel source, Font font)
		{
			MapEntity map = source.dataSource;
			card.Source = source;
			card.Root.name = "Location " + map.Id;
			card.Button.interactable = source.GetComponent<Button>().interactable;
			card.Name.font = card.Range.font = font ?? source.City_text?.font ?? source.City1_text?.font;
			card.Name.text = map.MapName;
			int startingLevel = Mathf.Max(1, map.MinLevel);
			card.Range.text = map.MaxLevel >= 999 ? $"{startingLevel} 级起" : $"{startingLevel}–{Mathf.Max(startingLevel, map.MaxLevel)} 级";
			InkInterfaceArt.Set(card.Illustration, IllustrationFor(map));
			UpdateSelection(card, source.BirthplaceSelected, animate: false);
		}

		private void Choose(LocationCard card)
		{
			if (card.Source == null || !card.Button.interactable)
			{
				return;
			}
			if (card.Source.dataSource.Depth < 4)
			{
				foreach (LocationCard other in cards)
				{
					if (other.Source != null)
					{
						UpdateSelection(other, other == card, animate: true);
					}
				}
			}
			card.Source.GetComponent<Button>().onClick.Invoke();
		}

		private void UpdateSelection(LocationCard card, bool selected, bool animate)
		{
			card.Selected = selected;
			card.Motion?.SetSelected(selected, animate);
			card.Name.color = selected ? InkInterfaceArt.Seal : card.Button.interactable ? InkInterfaceArt.Ink : LockedInk;
			card.Range.color = SecondaryInk;
			Sprite portrait = selected && card.Source?.dataSource?.Depth == 4
				? Singleton<UIManager>.Instance.FindWndByName<CreateRoleWnd>("CreateRoleWnd.prefab")?.currentTouXiangSprite
				: null;
			card.Portrait.sprite = portrait;
			card.Portrait.gameObject.SetActive(portrait != null);
		}

		private void LateUpdate()
		{
			if (view == null || !view.gameObject.activeSelf)
			{
				return;
			}
			for (int i = 0; i < cards.Count; i++)
			{
				LocationCard card = cards[i];
				if (card.Source != null && card.Button.interactable)
				{
					float lift = Mathf.Sin(Time.unscaledTime * 1.65f + i * 0.71f) * (card.Selected ? 3f : 1.8f);
					card.Illustration.rectTransform.anchoredPosition = new Vector2(0f, lift);
				}
			}
		}

		private void OnDisable()
		{
			StopReveal();
		}

		private void StopReveal()
		{
			reveal?.Kill();
			reveal = null;
			if (background != null)
			{
				background.color = Color.white;
			}
		}

		private static string IllustrationFor(MapEntity map)
		{
			switch (map.Depth)
			{
			case 1: return "FoldedMap";
			case 2: return "Pavilion";
			case 3: return "Town";
			}
			return (MapLevelType)map.MapType switch
			{
				MapLevelType.MASSIF => "Swords",
				MapLevelType.CITY_SUPER => "CityGate",
				MapLevelType.CITY_STATE => "Town",
				MapLevelType.CITY_PREFECTURE => "Pavilion",
				_ => "Village"
			};
		}

		private static void Layout(RectTransform rect, int index, int count)
		{
			int columns = count > 6 ? 3 : count > 1 ? 2 : 1;
			int rows = (count + columns - 1) / columns;
			int row = index / columns;
			int rowCount = Mathf.Min(columns, count - row * columns);
			float horizontal = columns == 3 ? 0.315f : 0.43f;
			float vertical = rows >= 4 ? 0.215f : 0.24f;
			float x = 0.5f + (index % columns - (rowCount - 1) * 0.5f) * horizontal;
			float y = 0.52f + ((rows - 1) * 0.5f - row) * vertical;
			float width = columns == 1 ? 0.50f : columns == 2 ? 0.395f : 0.297f;
			float height = rows == 1 ? 0.27f : rows >= 4 ? 0.201f : 0.22f;
			rect.anchorMin = new Vector2(x - width * 0.5f, y - height * 0.5f);
			rect.anchorMax = new Vector2(x + width * 0.5f, y + height * 0.5f);
			rect.offsetMin = rect.offsetMax = Vector2.zero;
		}

		private static RectTransform Rect(Transform parent, string name, Vector2 min, Vector2 max)
		{
			var node = new GameObject(name, typeof(RectTransform));
			node.layer = parent.gameObject.layer;
			var rect = (RectTransform)node.transform;
			rect.SetParent(parent, false);
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = rect.offsetMax = Vector2.zero;
			return rect;
		}

		private static Image Picture(Transform parent, string name, Vector2 min, Vector2 max)
		{
			Image image = Rect(parent, name, min, max).gameObject.AddComponent<Image>();
			image.raycastTarget = false;
			return image;
		}

		private static Text Label(Transform parent, string name, Vector2 min, Vector2 max, int size)
		{
			Text text = Rect(parent, name, min, max).gameObject.AddComponent<Text>();
			text.fontSize = size;
			text.alignment = TextAnchor.MiddleCenter;
			text.raycastTarget = false;
			UIManager.FitTextToRect(text);
			return text;
		}
	}
}
