using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_YuanPanButItem : MonoBehaviour
	{
		public int Shu;

		public Button YuanPanButton;

		public Text YuanPanText;

		public Image YiYuanPanButton;

		public Text YiYuanPanText;

		public void BinDing()
		{
			YuanPanText.text = " ";
			YuanPanButton.onClick.RemoveListener(DianJi);
			YuanPanButton.onClick.AddListener(DianJi);
		}

		private void DianJi()
		{
			string text = YuanPanText.text;
			Singleton<UIManager>.Instance.FindWndByName<ZS_ChuangZhaoWnd>("ZS_ChuangZhaoWnd.prefab")?.LunPanDianJi(Shu, text);
		}

		public void ButImage()
		{
			YuanPanButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
		}

		public void BianSe(string namestr)
		{
			if (namestr != null && namestr == YuanPanText.text)
			{
				YiYuanPanButton.gameObject.SetActive(value: true);
			}
			else if (namestr != null && namestr != YuanPanText.text)
			{
				YiYuanPanButton.gameObject.SetActive(value: false);
			}
		}

		public void Direction(double direction)
		{
			Transform obj = YuanPanButton.transform;
			Vector3 localEulerAngles = obj.localEulerAngles;
			localEulerAngles.z += (float)direction;
			obj.localEulerAngles = localEulerAngles;
		}

		public void DireTionNew(double direction)
		{
			Transform obj = YuanPanButton.transform;
			Vector3 localEulerAngles = obj.localEulerAngles;
			obj.DOLocalRotate(new Vector3(localEulerAngles.x, localEulerAngles.y, localEulerAngles.z + (float)direction), 0.5f);
		}
	}
}
