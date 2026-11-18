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

		private int m_Index;

		public void SetValue(int index, int stage, string name, UnityAction<int, string> clickAction)
		{
			base.gameObject.SetActive(value: true);
			NameText.text = name;
			m_Index = index;
			SelfBtn.onClick.RemoveAllListeners();
			SelfBtn.onClick.AddListener(() =>
			{
				if (clickAction != null)
				{
					clickAction(m_Index, name);
				}
			});
			string stageName = WorldSetting.GetStageName(stage);
			StageText.text = stageName;
		}
	}
}
