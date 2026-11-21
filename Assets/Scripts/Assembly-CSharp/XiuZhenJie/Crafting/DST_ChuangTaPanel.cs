using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_ChuangTaPanel : MonoBehaviour
	{
		public Button CloseBtn;

		public Text CengShu;

		public Button DaoJingBtn;

		public Button SuiYueBtn;

		public Button ChuangTaBtn;

		public void SetValue(int level)
		{
			CengShu.text = level.ToString();
		}
	}
}
