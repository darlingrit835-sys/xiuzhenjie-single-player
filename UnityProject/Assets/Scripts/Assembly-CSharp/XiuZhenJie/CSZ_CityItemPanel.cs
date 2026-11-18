using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CSZ_CityItemPanel : MonoBehaviour
	{
		public GameObject NotChose;

		public Text City_text;

		public Text Position_text;

		public GameObject Chosed;

		public Text City1_text;

		public Text Position1_text;

		public MapEntity dataSource;

		public int index;

		public Image ChengChi;

		public Image TouXiangSprite;

		private MapEnterType curretnEnterType;

		public Button EnterMapBtn;

		public Button ShaoDangBtn;

		private void Awake()
		{
			ClearData();
		}

		public void SetValue(int _index, MapEntity _dataSource, Action<CSZ_CityItemPanel> _acton, Action<CSZ_CityItemPanel> _shaoDangActon, bool isHear, MapEnterType _enterType)
		{
			index = _index;
			if (isHear)
			{
				if (NotChose != null)
				{
					NotChose.SetActive(value: false);
				}
				if (Chosed != null)
				{
					Sprite currentRoleSprite = Singleton<RoleManager>.Instance.GetCurrentRoleSprite();
					if (TouXiangSprite != null)
					{
						TouXiangSprite.sprite = currentRoleSprite;
					}
					Chosed.SetActive(value: true);
				}
			}
			curretnEnterType = _enterType;
			dataSource = _dataSource;
			if (City1_text != null)
			{
				City1_text.text = _dataSource.MapName;
			}
			if (City_text != null)
			{
				City_text.text = _dataSource.MapName;
			}
			if (Position_text != null)
			{
				Position_text.text = $"[{_dataSource.MinLevel}-{_dataSource.MaxLevel}]";
			}
			if (Position1_text != null)
			{
				Position1_text.text = $"[{_dataSource.MinLevel}-{_dataSource.MaxLevel}]";
			}
			ButtonClick(_acton, _shaoDangActon);
			XinaShi(_dataSource.IsCleanMap);
			MapLevelType level = (MapLevelType)dataSource.MapType;
			bool locked = curretnEnterType == MapEnterType.CreateRole &&
				(level == MapLevelType.STATE || level == MapLevelType.PREFECTURE ||
				 level == MapLevelType.STATE_COMMON || level == MapLevelType.STATE_SUPER) &&
				(_dataSource.MaxLevel > 40 || _dataSource.MinLevel > 1);
			Button button = GetComponent<Button>();
			if (locked)
			{
				button.interactable = false;
			}

			StyleCaption(City_text, false);
			StyleCaption(City1_text, false);
			StyleCaption(Position_text, true);
			StyleCaption(Position1_text, true);
			Image marker = transform.Find("YuanDian")?.GetComponent<Image>();
			InkInterfaceArt.Set(marker, "Compass");
			if (marker != null && locked)
			{
				marker.color = new Color32(141, 136, 119, 165);
			}

			if (ChengChi == null)
			{
				if (button.image != null && button.image.color.a > 0.5f)
				{
					InkInterfaceArt.Set(button.image, "IvoryPlaque", false);
				}
				return;
			}

			if (level == MapLevelType.MASSIF || level == MapLevelType.CITY ||
				level == MapLevelType.CITY_LOW || level == MapLevelType.CITY_PREFECTURE ||
				level == MapLevelType.CITY_STATE || level == MapLevelType.CITY_SUPER)
			{
				string name = $"{WorldSetting.GetWuXingName((WuXing)_dataSource.FiveElement)} · {_dataSource.MapName}";
				if (City_text != null) City_text.text = name;
				if (City1_text != null) City1_text.text = name;
			}

			string icon = level switch
			{
				MapLevelType.MASSIF => "Swords",
				MapLevelType.CITY_SUPER => "CityGate",
				MapLevelType.CITY_STATE => "Town",
				MapLevelType.CITY_PREFECTURE => "Pavilion",
				_ => "Village"
			};
			InkInterfaceArt.Set(ChengChi, icon);
			if (curretnEnterType == MapEnterType.CreateRole && level == MapLevelType.MASSIF)
			{
				ChengChi.color = new Color32(153, 148, 131, 140);
			}

			// City tiles use a separate illustrated marker and a legible two-line caption.
			Image caption = transform.Find("Image")?.GetComponent<Image>();
			if (ChengChi.name == "CityIcon" && caption != null)
			{
				RectTransform iconRect = ChengChi.rectTransform;
				iconRect.anchorMin = new Vector2(0.16f, 0.40f);
				iconRect.anchorMax = new Vector2(0.84f, 0.99f);
				iconRect.offsetMin = Vector2.zero;
				iconRect.offsetMax = Vector2.zero;
				InkInterfaceArt.Set(caption, "IvoryPlaque", false);
				RectTransform captionRect = caption.rectTransform;
				captionRect.anchorMin = new Vector2(0.03f, 0.01f);
				captionRect.anchorMax = new Vector2(0.97f, 0.43f);
				captionRect.offsetMin = Vector2.zero;
				captionRect.offsetMax = Vector2.zero;
				captionRect.SetAsFirstSibling();
			}
		}

		private static void StyleCaption(Text label, bool secondary)
		{
			if (label != null)
			{
				label.color = secondary ? InkInterfaceArt.SoftInk : InkInterfaceArt.Ink;
				UIManager.FitTextToRect(label);
			}
		}

		private void XinaShi(bool isCleanMap)
		{
			if (EnterMapBtn != null && ShaoDangBtn != null)
			{
				if (isCleanMap)
				{
					ShaoDangBtn.gameObject.SetActive(value: true);
				}
				else
				{
					ShaoDangBtn.gameObject.SetActive(value: false);
				}
			}
		}

		public void ChoosedItem(bool isChoosed)
		{
			if (NotChose != null)
			{
				NotChose.SetActive(!isChoosed);
			}
			if (Chosed != null)
			{
				if (isChoosed && curretnEnterType == MapEnterType.CreateRole && TouXiangSprite != null)
				{
					CreateRoleWnd creation = Singleton<UIManager>.Instance.FindWndByName<CreateRoleWnd>("CreateRoleWnd.prefab");
					if (creation?.currentTouXiangSprite != null)
					{
						TouXiangSprite.sprite = creation.currentTouXiangSprite;
					}
				}
				Chosed.SetActive(isChoosed);
			}
		}

		public void ClearData()
		{
			if (Chosed != null)
			{
				Chosed.SetActive(value: false);
			}
			if (NotChose != null)
			{
				NotChose.SetActive(value: true);
			}
			index = 0;
			dataSource = null;
			base.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
			if (City1_text != null)
			{
				City1_text.text = string.Empty;
			}
			if (City_text != null)
			{
				City_text.text = string.Empty;
			}
			if (Position_text != null)
			{
				Position_text.text = string.Empty;
			}
			if (Position1_text != null)
			{
				Position1_text.text = string.Empty;
			}
		}

		private void ButtonClick(Action<CSZ_CityItemPanel> acton, Action<CSZ_CityItemPanel> shaoDangActon)
		{
			if (EnterMapBtn != null && ShaoDangBtn != null)
			{
				EnterMapBtn.onClick.RemoveAllListeners();
				EnterMapBtn.onClick.AddListener(() =>
				{
					if (acton != null)
					{
						acton(this);
					}
				});
				ShaoDangBtn.onClick.RemoveAllListeners();
				ShaoDangBtn.onClick.AddListener(() =>
				{
					if (shaoDangActon != null)
					{
						shaoDangActon(this);
					}
				});
			}
			else
			{
				if (!(EnterMapBtn == null) || !(ShaoDangBtn == null))
				{
					return;
				}
				base.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
				base.gameObject.GetComponent<Button>().onClick.AddListener(() =>
				{
					if (acton != null)
					{
						acton(this);
					}
				});
			}
		}
	}
}
