using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenShanFengItem : MonoBehaviour
	{
		public Text NameText;

		public Button SelfBtn;

		public Text StageText;

		private UnityAction<int, string> enter;
		private int duty;
		private string hillName;

		private void Awake()
		{
			// Some recovered cards serialize the stage text in both slots.
			if (NameText == StageText)
				NameText = SelfBtn.GetComponentInChildren<Text>(true);
			var sign = SelfBtn.transform;
			NameText.rectTransform.SetParent(sign, false);
			StageText.rectTransform.SetParent(sign, false);
			foreach (Transform child in transform)
			{
				if (child != sign)
					child.gameObject.SetActive(false);
			}
			GetComponent<Image>().enabled = false;
			InkDialogArt.Inset((RectTransform)sign, 0, 0, 0, 0);
			SelfBtn.targetGraphic = SelfBtn.GetComponent<Image>();
			InkDialogArt.StyleButton(SelfBtn, "ParchmentHeading", InkInterfaceArt.Ink);
			ZongMenWnd.StyleTwoLines(NameText, StageText, NameText.text, InkInterfaceArt.Ink, InkInterfaceArt.SoftInk);
			SelfBtn.onClick.AddListener(Enter);
		}

		private void Enter() => enter?.Invoke(duty, hillName);

		public void SetValue(int index, int stage, string name, UnityAction<int, string> clickAction)
		{
			gameObject.SetActive(true);
			duty = index;
			hillName = name;
			enter = clickAction;
			string function = ((ZongMenShanFengDuty)index).ToString();
			bool unnamed = string.IsNullOrEmpty(name) || name == "无名峰";
			NameText.text = unnamed ? function + "峰" : name;
			string level = WorldSetting.GetStageName(stage);
			StageText.text = unnamed ? level : function + " · " + level;
		}
	}
}
