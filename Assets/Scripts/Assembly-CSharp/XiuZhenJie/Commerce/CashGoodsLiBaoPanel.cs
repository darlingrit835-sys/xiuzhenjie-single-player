using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CashGoodsLiBaoPanel : MonoBehaviour
	{
		private Dictionary<int, CashLiBaoGoodsEntity> m_DataSource;

		private Dictionary<int, XuanTianLouEntity> m_XuanTianLouDic;

		public List<CashGoodsLiBaoItem> m_UIDataSource;

		private CashGoodsWnd m_CashGoodsWnd;

		public RectTransform Content;

		public RectTransform XinChunLiBaoItem;

		public Button XinChunLiBao_1Btn;

		public Text XinChunLiBao_1BtnText;

		public Button XinChunLiBao_2Btn;

		public Text XinChunLiBao_2BtnText;

		public Button XinChunLiBao_3Btn;

		public Text XinChunLiBao_3BtnText;

		public Button XinChunLiBao_4Btn;

		public Text XinChunLiBao_4BtnText;

		public void Init(CashGoodsWnd cashGoodsWnd, List<CashGoodsBuyStatus> LiBaoStatus, List<CashGoodsBuyStatus> xinChunLiBaoStatus)
		{
			m_CashGoodsWnd = cashGoodsWnd;
			m_DataSource = Singleton<FacadeManager>.Instance.SelectCashGoodsLiBaoInfo();
			if (m_DataSource != null)
			{
				m_XuanTianLouDic = Singleton<FacadeManager>.Instance.SelectXuanTianLouGoodsInfo();
			}
			int num = 0;
			if (m_DataSource.Count != m_UIDataSource.Count)
			{
				return;
			}
			bool isShowEveryLiBao = false;
			foreach (KeyValuePair<int, CashLiBaoGoodsEntity> item in m_DataSource)
			{
				List<XuanTianLouEntity> list = new List<XuanTianLouEntity>();
				ShowOrHideMeiRiLiBao(num, ref isShowEveryLiBao, LiBaoStatus);
				foreach (int goodsId in item.Value.GoodsIds)
				{
					XuanTianLouEntity value = null;
					if (goodsId == -10)
					{
						value = new XuanTianLouEntity
						{
							GoodsType = 99,
							Name = "灵玉",
							Level = 1,
							Rank = 16,
							FiveElement = 0
						};
						list.Add(value);
					}
					else if (m_XuanTianLouDic.TryGetValue(goodsId, out value))
					{
						list.Add(value);
					}
				}
				m_UIDataSource[num].SetGoodsData(item.Key, LiBaoStatus[num], item.Value.TitleStr, list, item.Value.GoodsNums, LiBaoGouMai);
				num++;
			}
			ShowEventItem(xinChunLiBaoStatus);
		}

		private void ShowEventItem(List<CashGoodsBuyStatus> xinChunLiBaoStatus)
		{
			HuoDongEntity huoDongEntity = null;
			for (int i = 0; i < Singleton<RoleManager>.Instance.EventInfoList.Count; i++)
			{
				if (Singleton<RoleManager>.Instance.EventInfoList[i].type == 1)
				{
					huoDongEntity = Singleton<RoleManager>.Instance.EventInfoList[i];
					break;
				}
			}
			if (huoDongEntity != null)
			{
				long createtime = CommonMethod.GetCreatetime();
				if (createtime >= huoDongEntity.startAt && createtime < huoDongEntity.endAt)
				{
					XinChunLiBaoItem.gameObject.SetActive(value: true);
					AddBtnListener();
					ReflashXinChunLiBaoStatus(xinChunLiBaoStatus);
					Content.sizeDelta = new Vector2(Content.sizeDelta.x, 1600f);
				}
				else
				{
					XinChunLiBaoItem.gameObject.SetActive(value: false);
					Content.sizeDelta = new Vector2(Content.sizeDelta.x, 1036.5f);
				}
			}
		}

		private void AddBtnListener()
		{
			XinChunLiBao_1Btn.onClick.RemoveAllListeners();
			XinChunLiBao_2Btn.onClick.RemoveAllListeners();
			XinChunLiBao_3Btn.onClick.RemoveAllListeners();
			XinChunLiBao_4Btn.onClick.RemoveAllListeners();
			XinChunLiBao_1Btn.onClick.AddListener(XinChunLiBao_1BtnClick);
			XinChunLiBao_2Btn.onClick.AddListener(XinChunLiBao_2BtnClick);
			XinChunLiBao_3Btn.onClick.AddListener(XinChunLiBao_3BtnClick);
			XinChunLiBao_4Btn.onClick.AddListener(XinChunLiBao_4BtnClick);
		}

		private void ReflashXinChunLiBaoStatus(List<CashGoodsBuyStatus> xinChunLiBaoStatus)
		{
			for (int i = 0; i < xinChunLiBaoStatus.Count; i++)
			{
				switch (xinChunLiBaoStatus[i])
				{
				case CashGoodsBuyStatus.UNFINISHED:
				case CashGoodsBuyStatus.UNGET:
					SetXinChunLiBaoStatus(i);
					break;
				case CashGoodsBuyStatus.GET:
					SetYiLingQuXinChunLiBaoStatus(i);
					break;
				}
			}
		}

		private void SetXinChunLiBaoStatus(int LiBaoIndex)
		{
			switch (LiBaoIndex)
			{
			case 0:
				XinChunLiBao_1Btn.interactable = true;
				XinChunLiBao_1BtnText.text = "领取";
				break;
			case 1:
				XinChunLiBao_2Btn.interactable = true;
				XinChunLiBao_2BtnText.text = "¥ 12";
				break;
			case 2:
				XinChunLiBao_3Btn.interactable = true;
				XinChunLiBao_3BtnText.text = "¥ 30";
				break;
			case 3:
				XinChunLiBao_4Btn.interactable = true;
				XinChunLiBao_4BtnText.text = "¥ 68";
				break;
			}
		}

		private void SetYiLingQuXinChunLiBaoStatus(int LiBaoIndex)
		{
			switch (LiBaoIndex)
			{
			case 0:
				XinChunLiBao_1Btn.interactable = false;
				XinChunLiBao_1BtnText.text = "已领取";
				break;
			case 1:
				XinChunLiBao_2Btn.interactable = false;
				XinChunLiBao_2BtnText.text = "已购买";
				break;
			case 2:
				XinChunLiBao_3Btn.interactable = false;
				XinChunLiBao_3BtnText.text = "已购买";
				break;
			case 3:
				XinChunLiBao_4Btn.interactable = false;
				XinChunLiBao_4BtnText.text = "已购买";
				break;
			}
		}

		private void ShowOrHideMeiRiLiBao(int i, ref bool isShowEveryLiBao, List<CashGoodsBuyStatus> LiBaoStatus)
		{
			if (LiBaoStatus[2] == CashGoodsBuyStatus.GET && LiBaoStatus[3] == CashGoodsBuyStatus.GET && LiBaoStatus[4] == CashGoodsBuyStatus.GET && LiBaoStatus[5] == CashGoodsBuyStatus.GET)
			{
				m_UIDataSource[2].gameObject.SetActive(value: false);
				m_UIDataSource[3].gameObject.SetActive(value: false);
				m_UIDataSource[4].gameObject.SetActive(value: false);
				m_UIDataSource[5].gameObject.SetActive(value: true);
			}
			else if (i == 2 && LiBaoStatus[i] != CashGoodsBuyStatus.GET)
			{
				isShowEveryLiBao = true;
				m_UIDataSource[2].gameObject.SetActive(value: true);
			}
			else if (i == 3 && LiBaoStatus[i] != CashGoodsBuyStatus.GET)
			{
				if (!isShowEveryLiBao)
				{
					isShowEveryLiBao = true;
					m_UIDataSource[2].gameObject.SetActive(value: false);
					m_UIDataSource[3].gameObject.SetActive(value: true);
				}
			}
			else if (i == 4 && LiBaoStatus[i] != CashGoodsBuyStatus.GET)
			{
				if (!isShowEveryLiBao)
				{
					isShowEveryLiBao = true;
					m_UIDataSource[3].gameObject.SetActive(value: false);
					m_UIDataSource[4].gameObject.SetActive(value: true);
				}
			}
			else if (i == 5 && LiBaoStatus[i] != CashGoodsBuyStatus.GET && !isShowEveryLiBao)
			{
				isShowEveryLiBao = true;
				m_UIDataSource[4].gameObject.SetActive(value: false);
				m_UIDataSource[5].gameObject.SetActive(value: true);
			}
		}

		private void LiBaoGouMai(int id, CashGoodsBuyStatus status)
		{
		}

		private void LingQuShouChong()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ShouChongLiBaoGet, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ShouChongLiBaoGetResp_1002023 shouChongLiBaoGetResp_ = new ShouChongLiBaoGetResp_1002023();
					shouChongLiBaoGetResp_.MergeFrom(byteString);
					if (shouChongLiBaoGetResp_.IsSuccess)
					{
						m_CashGoodsWnd.OnShow();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(shouChongLiBaoGetResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void GouMaiLingYuLiBao(int type)
		{
			BuyLiBaoReq_1001018 data = new BuyLiBaoReq_1001018
			{
				Type = type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BuyLiBao, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuyLiBaoResp_1001018 buyLiBaoResp_ = new BuyLiBaoResp_1001018();
					buyLiBaoResp_.MergeFrom(byteString);
					if (buyLiBaoResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = buyLiBaoResp_.CurrentJade;
						PlayerPrefs.SetInt("EveryDayLiBao", DateTime.Now.Day);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						m_CashGoodsWnd.OnShow();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buyLiBaoResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void XinChunLiBao_1BtnClick()
		{
			BuyLiBaoReq_1001018 data = new BuyLiBaoReq_1001018
			{
				Type = 25
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BuyLiBao, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuyLiBaoResp_1001018 buyLiBaoResp_ = new BuyLiBaoResp_1001018();
					buyLiBaoResp_.MergeFrom(byteString);
					if (buyLiBaoResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.QiYun += 100;
						XinChunLiBao_1Btn.interactable = false;
						XinChunLiBao_1BtnText.text = "已领取";
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buyLiBaoResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void XinChunLiBao_2BtnClick()
		{
		}

		private void XinChunLiBao_3BtnClick()
		{
		}

		private void XinChunLiBao_4BtnClick()
		{
		}

		private void OnDestroy()
		{
			XinChunLiBao_1Btn.onClick.RemoveAllListeners();
			XinChunLiBao_2Btn.onClick.RemoveAllListeners();
			XinChunLiBao_3Btn.onClick.RemoveAllListeners();
			XinChunLiBao_4Btn.onClick.RemoveAllListeners();
		}
	}
}
