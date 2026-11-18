using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DongFuRoomOtherItem : MonoBehaviour
	{
		public Image LockImg;

		public Text TitleText;

		public Button KaiPiBtn;

		public Button YiKaiPiImg;

		public Button EnterBtn;

		private RoleFuZhiType m_FuZhiType;

		private int locationId;

		public void InitClosed(int id, UnityAction TiaoZhuanAction)
		{
			locationId = id;
			KaiPiBtn.onClick.RemoveAllListeners();
			KaiPiBtn.onClick.AddListener(() =>
			{
				if (TiaoZhuanAction != null)
				{
					TiaoZhuanAction();
				}
			});
		}

		public void OpenLock(string name, int id, RoleFuZhiType fuZhiType, UnityAction TiaoZhuanAction, UnityAction<int, RoleFuZhiType> ReBuildAction)
		{
			if (this == null || LockImg == null || TitleText == null || KaiPiBtn == null || YiKaiPiImg == null || EnterBtn == null || YiKaiPiImg == null)
			{
				return;
			}
			locationId = id;
			m_FuZhiType = fuZhiType;
			LockImg.gameObject.SetActive(value: false);
			TitleText.text = name;
			KaiPiBtn.gameObject.SetActive(value: false);
			YiKaiPiImg.gameObject.SetActive(value: true);
			EnterBtn.onClick.RemoveAllListeners();
			EnterBtn.onClick.AddListener(() =>
			{
				if (TiaoZhuanAction != null)
				{
					TiaoZhuanAction();
				}
			});
			YiKaiPiImg.onClick.RemoveAllListeners();
			YiKaiPiImg.onClick.AddListener(() =>
			{
				if (ReBuildAction != null)
				{
					ReBuildAction(locationId, m_FuZhiType);
				}
			});
		}
	}
}
