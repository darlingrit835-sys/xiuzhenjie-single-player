using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CreateRolePanel : MonoBehaviour
	{
		internal void StyleGenderCaptions()
		{
			PlaceGenderCaption(ManBtn, ManChooseImg, ManUnChooseImg, "ManText");
			PlaceGenderCaption(WomenBtn, WomenChooseImg, WomenUnChooseImg, "WomenText");
			var hint = Part1Panel.Find("XingBieToggleGroup/Text")?.GetComponent<Text>();
			if (hint != null)
			{
				hint.text = "请选择角色性别，再挑选对应的头像。";
				hint.color = InkInterfaceArt.Ink;
			}
		}

		private static void PlaceGenderCaption(Button button, Image selected, Image unselected, string captionName)
		{
			var rect = (RectTransform)button.transform;
			var caption = rect.Find(captionName) ?? rect.parent.Find(captionName);
			if (caption == null) return;
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(46f, rect.rect.height));
			InkInterfaceArt.Set(button.image, "IvoryPlaque", false);
			foreach (var icon in new[] { selected, unselected })
			{
				icon.rectTransform.anchorMin = icon.rectTransform.anchorMax = new Vector2(.32f, .5f);
				icon.rectTransform.anchoredPosition = Vector2.zero;
				icon.raycastTarget = false;
			}
			caption.SetParent(rect, false);
			var label = caption.GetComponent<Text>();
			label.rectTransform.anchorMin = new Vector2(.46f, 0f);
			label.rectTransform.anchorMax = new Vector2(.93f, 1f);
			label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero;
			label.alignment = TextAnchor.MiddleLeft;
			label.color = InkInterfaceArt.Ink;
			label.raycastTarget = false;
		}

		public RectTransform Part1Panel;

		public InputField Name_Ipt;

		public ScrollRect TouXiang_Img;

		public Text TouXiangTitle_text;

		public Button Panel_1_BackBtn;

		public Button ManBtn;

		public Image ManChooseImg;

		public Image ManUnChooseImg;

		public Button WomenBtn;

		public Image WomenChooseImg;

		public Image WomenUnChooseImg;

		public Toggle TouXiangYi_toggle;

		public Toggle TouXiangEr_toggle;

		public Toggle TouXiangSan_toggle;

		public Toggle TouXiangSi_toggle;

		public Toggle TouXiangWu_toggle;

		public Toggle TouXiangLiu_toggle;

		public RectTransform TouXiangSpriteContent;

		public Button NextBtn;

		public RectTransform Part2Panel;

		public Button Part2BackBtn;

		public Button WuXingLingGenWHAT;

		public Button HunLiJiaoRuoWHAT;

		public Button Part2NextBtn;

		public Button Jin_Btn;

		public Button JinChoose_Btn;

		public Button Mu_Btn;

		public Button MuChoose_Btn;

		public Button Shui_Btn;

		public Button ShuiChoose_Btn;

		public Button Huo_Btn;

		public Button HuoChoose_Btn;

		public Button Tu_Btn;

		public Button TuChoose_Btn;

		public Button XiaoWenHao;

		public RectTransform Part3Pane;

		public Image TouXiang;

		public Image WuXingLingGenJin;

		public Image WuXingLingGenMu;

		public Image WuXingLingGenShui;

		public Image WuXingLingGenHuo;

		public Image WuXingLingGenTu;

		public Button Part3BackBtn;

		public Button TiaoZhenBtn;

		public Button QueRenBtn;

		public Text RoleNameText;

		public Text ChuShengDiText;

		public RectTransform MapPartRect;

		public Button Createrolerbtn;

		public Button Part4BackBtn;
	}
}
