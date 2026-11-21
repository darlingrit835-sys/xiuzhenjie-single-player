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
			title_text.text = $"传送至{target}";
			title_text.color = InkInterfaceArt.Ink;
			LingShi_text.text = $"{amount:N0} 灵石";
			LingShi_text.color = InkInterfaceArt.Ink;
			ShenYu_text.text = shengYu >= 0 ? $"传送后剩余 {shengYu:N0} 灵石" : $"还缺 {-shengYu:N0} 灵石";
			ShenYu_text.color = shengYu >= 0 ? InkInterfaceArt.SoftInk : InkInterfaceArt.Seal;
			if (transform.Find("传送函") == null)
				InkDialogArt.StyleButton(ChuanSong_btn, "JadeButton", InkDialogArt.Ivory);
			ChuanSong_btn.interactable = shengYu >= 0;
			ChuanSong_btn.onClick.RemoveAllListeners();
			Clsoe_btn.onClick.RemoveAllListeners();
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
