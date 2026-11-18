using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class MapZongMenItem : MonoBehaviour
	{
		public RectTransform ZongMenPanel;

		public RectTransform FuDiPanel;

		public Button BaiRuShanMenBtn;

		public Button QingChuYaoSouBtn;

		public Text QingChuYaoSouBtnText;

		public Text BaiRuShanMenBtnText;

		public Text ZongMenName;

		public Text QingChuProgress;

		private string m_ZongMenId;

		private string m_ZongMenName;

		public void SetZongMenValue(string zongMenId, string zongMenName, bool isYour, UnityAction<string, string> baiRuShanMenAction, UnityAction<string, string> backZongMen)
		{
			m_ZongMenId = zongMenId;
			m_ZongMenName = zongMenName;
			ZongMenPanel.gameObject.SetActive(value: true);
			FuDiPanel.gameObject.SetActive(value: false);
			ZongMenName.text = zongMenName;
			BaiRuShanMenBtn.onClick.RemoveAllListeners();
			if (isYour)
			{
				BaiRuShanMenBtnText.text = "返回宗门";
				if (backZongMen != null)
				{
					BaiRuShanMenBtn.onClick.AddListener(() =>
					{
						backZongMen(zongMenId, zongMenName);
					});
				}
				return;
			}
			BaiRuShanMenBtnText.text = "拜入宗门";
			if (baiRuShanMenAction != null)
			{
				BaiRuShanMenBtn.onClick.AddListener(() =>
				{
					baiRuShanMenAction(zongMenId, zongMenName);
				});
			}
		}

		public void ReflashZongValue(string zongMenId, string zongMenName, UnityAction<string, string> backZongMen)
		{
			m_ZongMenId = zongMenId;
			m_ZongMenName = zongMenName;
			ZongMenName.text = zongMenName;
			ZongMenPanel.gameObject.SetActive(value: true);
			FuDiPanel.gameObject.SetActive(value: false);
			BaiRuShanMenBtn.onClick.RemoveAllListeners();
			BaiRuShanMenBtnText.text = "返回宗门";
			if (backZongMen != null)
			{
				BaiRuShanMenBtn.onClick.AddListener(() =>
				{
					backZongMen(m_ZongMenId, zongMenName);
				});
			}
		}

		public void SetDongTianFuDiValue(string mapId, string goodsId, int currentProgress, int maxProgress, UnityAction<string, string> qingLiYaoShou, UnityAction<string, string> kaiZongLiPaiAction)
		{
			ZongMenPanel.gameObject.SetActive(value: false);
			FuDiPanel.gameObject.SetActive(value: true);
			QingChuProgress.text = currentProgress + "/" + maxProgress;
			QingChuYaoSouBtn.onClick.RemoveAllListeners();
			if (currentProgress <= 0)
			{
				QingChuYaoSouBtnText.text = "开宗立派";
				if (kaiZongLiPaiAction != null)
				{
					QingChuYaoSouBtn.onClick.AddListener(() =>
					{
						kaiZongLiPaiAction(mapId, goodsId);
					});
				}
				return;
			}
			QingChuYaoSouBtnText.text = "清理妖兽";
			if (qingLiYaoShou != null)
			{
				QingChuYaoSouBtn.onClick.AddListener(() =>
				{
					qingLiYaoShou(mapId, goodsId);
				});
			}
		}
	}
}
