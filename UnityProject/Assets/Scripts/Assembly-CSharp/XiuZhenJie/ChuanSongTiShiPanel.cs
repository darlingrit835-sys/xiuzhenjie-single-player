using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuanSongTiShiPanel : MonoBehaviour
	{
		public Text title_text;

		public Text JuLi_text;

		public Text LingShi_text;

		public Text ShenYu_text;

		public Button ChuanSong_btn;

		public Button Clsoe_btn;

		public void SetValue(string target, int amount, long shengYu, UnityAction ChuanSongCallBack)
		{
			title_text.text = $"传送到:{target}";
			LingShi_text.text = CommonMethod.HuoBiLongToString(amount, 4);
			if (shengYu < 0)
			{
				shengYu = 0L;
			}
			ShenYu_text.text = $"缴纳后还剩余{shengYu}灵石";
			ChuanSong_btn.onClick.AddListener(ChuanSongCallBack);
			Clsoe_btn.onClick.AddListener(ClosePanel);
			base.gameObject.SetActive(value: true);
			base.gameObject.transform.SetAsLastSibling();
		}

		public void ClosePanel()
		{
			title_text.text = string.Empty;
			LingShi_text.text = string.Empty;
			ShenYu_text.text = string.Empty;
			ChuanSong_btn.onClick.RemoveAllListeners();
			Clsoe_btn.onClick.RemoveAllListeners();
			base.gameObject.SetActive(value: false);
		}
	}
}
