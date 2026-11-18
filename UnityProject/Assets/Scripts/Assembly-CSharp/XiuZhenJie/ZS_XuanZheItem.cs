using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_XuanZheItem : MonoBehaviour
	{
		public string Name;

		public Button XuanZheButton;

		public Text XuanZhongText;

		public Text WeiXuanZhongText;

		public void XianshiNew(bool xianshi, string namestr = null)
		{
			XuanZheButton.gameObject.isStatic = xianshi;
			if (xianshi)
			{
				XuanZhongText.gameObject.SetActive(value: true);
				WeiXuanZhongText.gameObject.SetActive(value: false);
				if (namestr != null && namestr == Name)
				{
					XuanZheButton.Select();
					return;
				}
				XuanZheButton.onClick.RemoveListener(GenXing);
				XuanZheButton.onClick.AddListener(GenXing);
			}
			else
			{
				XuanZhongText.gameObject.SetActive(value: false);
				WeiXuanZhongText.gameObject.SetActive(value: true);
			}
		}

		public void ImgGaiBian(bool panDuan)
		{
			Sprite sprite = null;
			if (panDuan)
			{
				XuanZheButton.image.gameObject.GetComponent<Image>().color = Color.white;
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/XuShenDian/ZhenFaXieHui/ZhenTu_lvJuXing.png");
			}
			else
			{
				XuanZheButton.image.gameObject.GetComponent<Image>().color = Color.black;
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Common/Mask.png");
			}
			XuanZheButton.image.gameObject.GetComponent<Image>().sprite = sprite;
		}

		private void GenXing()
		{
			Singleton<UIManager>.Instance.FindWndByName<ZS_ChuangZhaoWnd>("ZS_ChuangZhaoWnd.prefab")?.ButDianJi(Name);
		}
	}
}
