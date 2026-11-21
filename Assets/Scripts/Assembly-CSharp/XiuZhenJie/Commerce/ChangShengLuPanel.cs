using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChangShengLuPanel : MonoBehaviour
	{
		public RectTransform ListRectTransform;

		public ScrollViewInfiniteVertical ListVerticalContent;

		public Image JieShoImage;

		public Button GuanBiButton;

		public Button GouMaiButton;

		private Dictionary<int, ChangShengLuItem> m_BaiShiDiZisDic;

		private GameObject m_BangDanItemObj;

		private List<ChangShengLuItem> m_DataSourceList;

		private bool m_IsBuyCsl;

		private List<int> m_GETLevelList;

		public void Init(CashGoodsWnd cashGoodsWnd)
		{
			m_BangDanItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChangShengLuItem.prefab");
			m_DataSourceList = new List<ChangShengLuItem>();
			DaKai(cashGoodsWnd, cashGoodsWnd.PageRevision);
		}

		private void DaKai(CashGoodsWnd store, int revision)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTERCSL, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (!store.IsCurrentPage(revision)) return;
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterCSLResp_1002027 enterCSLResp_ = new EnterCSLResp_1002027();
					enterCSLResp_.MergeFrom(byteString);
					if (enterCSLResp_.IsSuccess)
					{
						m_IsBuyCsl = enterCSLResp_.IsBuyCSL;
						m_GETLevelList = new List<int>(enterCSLResp_.GetLevels);
						ShengChang();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterCSLResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void Shanchu()
		{
			if (ListRectTransform == null)
			{
				return;
			}
			foreach (RectTransform item in ListRectTransform.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void LingYu(bool beiJin, bool panDuan, bool lingQu, int i, int j, string wuPingname, int level, int shu)
		{
			ChangShengLuItem component = SetListContentParent(ListRectTransform, m_BangDanItemObj).GetComponent<ChangShengLuItem>();
			component.PanDuanXianShi(beiJin, panDuan, lingQu, WorldSetting.GetStageChineseName(i) + WorldSetting.GetChineseNumber(j) + "阶", wuPingname, j, level, JieSho, shu);
			m_BaiShiDiZisDic.Add(level, component);
		}

		private void ShengChang()
		{
			m_BaiShiDiZisDic = new Dictionary<int, ChangShengLuItem>();
			Shanchu();
			int num = 0;
			bool flag = true;
			int num2 = 0;
			for (int i = 1; i < 10; i++)
			{
				for (int j = 0; j < 11; j += 3)
				{
					flag = !flag;
					switch (j)
					{
					case 0:
					{
						string text = "灵玉";
						num++;
						j++;
						bool lingQu3 = m_GETLevelList.Contains(num);
						LingYu(flag, panDuan: true, lingQu3, i, j, text + "*" + 5 * (num / 10 + 1), num, num2);
						break;
					}
					case 4:
					{
						string text = "道经残卷";
						num += 3;
						bool lingQu2 = m_GETLevelList.Contains(num);
						LingYu(flag, m_IsBuyCsl, lingQu2, i, j, text + "*" + 5, num, num2);
						break;
					}
					case 7:
					{
						num += 3;
						string text = (((num - 1) / 10 + 1 < 4) ? "地阶经脉丹" : "天阶经脉丹");
						bool lingQu4 = m_GETLevelList.Contains(num);
						LingYu(flag, m_IsBuyCsl, lingQu4, i, j, text + "*" + 1, num, num2);
						break;
					}
					case 10:
					{
						num += 3;
						string text = (((num - 1) / 10 + 1 < 4) ? "四阶德经" : (WorldSetting.GetChineseNumber((num - 1) / 10 + 1) + "阶德经"));
						bool lingQu = m_GETLevelList.Contains(num);
						LingYu(flag, m_IsBuyCsl, lingQu, i, j, text + "*" + 1, num, num2);
						break;
					}
					}
					num2++;
				}
				ListRectTransform.sizeDelta = new Vector2(ListRectTransform.sizeDelta.x, (float)m_BaiShiDiZisDic.Count * 49f);
			}
		}

		private GameObject SetListContentParent(RectTransform parentContent, GameObject insObj)
		{
			GameObject obj = Object.Instantiate(insObj);
			RectTransform component = obj.GetComponent<RectTransform>();
			component.SetParent(parentContent);
			component.localScale = Vector3.one;
			component.eulerAngles = Vector3.zero;
			return obj;
		}

		private void JieSho(bool panDuan)
		{
			JieShoImage.gameObject.SetActive(value: true);
			GouMaiButton.onClick.RemoveListener(GouMai);
			GouMaiButton.onClick.AddListener(GouMai);
			GuanBiButton.onClick.RemoveListener(GuanBi);
			GuanBiButton.onClick.AddListener(GuanBi);
		}

		private void GuanBi()
		{
			JieShoImage.gameObject.SetActive(value: false);
		}

		private void GouMai()
		{
			GuanBi();
		}
	}
}
