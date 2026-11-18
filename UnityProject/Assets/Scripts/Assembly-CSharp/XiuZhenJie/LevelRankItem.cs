using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LevelRankItem : MonoBehaviour
	{
		public Text NameText;

		public Text LevelText;

		public Text HotText;

		public Text ExtendText;

		private string m_RoleId;

		public string RoleId
		{
			get
			{
				return m_RoleId;
			}
			private set
			{
				m_RoleId = value;
			}
		}

		public void SetItem(string roleId, string name, string level, string hotValue, string extendValue = "")
		{
			RoleId = roleId;
			NameText.text = name;
			LevelText.text = level;
			HotText.text = hotValue;
			if (ExtendText != null)
			{
				ExtendText.text = extendValue;
			}
		}

		public void AddHotValue()
		{
			int num = int.Parse(HotText.text);
			HotText.text = (num + 1).ToString();
		}

		public void ClearItem()
		{
			RoleId = string.Empty;
			NameText.text = string.Empty;
			LevelText.text = string.Empty;
			HotText.text = string.Empty;
			if (ExtendText != null)
			{
				ExtendText.text = string.Empty;
			}
		}
	}
}
