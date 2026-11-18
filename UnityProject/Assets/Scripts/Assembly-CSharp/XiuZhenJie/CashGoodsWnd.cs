using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class CashGoodsWnd : Window
	{
		public CashGoodsPanel Panel;

		private RectTransform m_LiBaoRect;

		private RectTransform m_LingYuRect;

		private RectTransform m_ChangShengLuRect;

		private CashGoodsPanelType m_CurrentPanelType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<CashGoodsPanel>();
			m_LiBaoRect = Panel.LiBaoPanel.GetComponent<RectTransform>();
			m_LingYuRect = Panel.LingYuPanel.GetComponent<RectTransform>();
			m_ChangShengLuRect = Panel.ChangShengLuPanel.GetComponent<RectTransform>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddToggleClickListener(Panel.LiBaoTog, LiBaoTogClick);
			AddToggleClickListener(Panel.LingYuTog, LingYuTogClick);
			AddToggleClickListener(Panel.ChangShengLuTog, ChangShengLu);
			m_CurrentPanelType = CashGoodsPanelType.YueKa;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			GetData();
		}

		private void GetData()
		{
			Panel.YueKaPanel.Init(this);
			GetLiBaoDataStatus();
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(GetLingYuStatus());
			Panel.ChangShengLuPanel.Init(this);
		}

		private void GetLiBaoDataStatus()
		{
			List<CashGoodsBuyStatus> liBaoStatus = new List<CashGoodsBuyStatus>();
			List<CashGoodsBuyStatus> xinChunLiBaoStatus = new List<CashGoodsBuyStatus>();
			Singleton<NetManager>.Instance.SendMsg(MsgType.LiBaoBuyInfo, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					LiBaoBuyInfoResp_1002022 liBaoBuyInfoResp_ = new LiBaoBuyInfoResp_1002022();
					liBaoBuyInfoResp_.MergeFrom(byteString);
					if (liBaoBuyInfoResp_.IsSuccess)
					{
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsGetShouChong);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyXiuZhen);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyMeiRi);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyMeiRiJinJie);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyMeiRiChaoZhi);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyMeiRiHaoHua);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyShouChongTeHui);
						liBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyJingJieTeHui);
						xinChunLiBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyNew0);
						xinChunLiBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyNew1);
						xinChunLiBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyNew2);
						xinChunLiBaoStatus.Add((CashGoodsBuyStatus)liBaoBuyInfoResp_.IsBuyNew3);
						Panel.LiBaoPanel.Init(this, liBaoStatus, xinChunLiBaoStatus);
					}
				}
			}, isShowWaitUI: true);
		}

		private IEnumerator GetLingYuStatus()
		{
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			List<bool> lingYuStatus = new List<bool>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("roleId", Singleton<RoleManager>.Instance.Role_Entity.Id);
			dictionary.Add("channelId", "1");
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.PayIp, Constants.PayPort, "/pay/firstRechargeList", dictionary);
			yield return www.SendWebRequest();
			string text = www.downloadHandler.text;
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				switch (status)
				{
				case HttpsRequestStatus.ServerError:
					MonoSingleton<GameStart>.Instance.PopUpAttention(content);
					break;
				case HttpsRequestStatus.SuccessFul:
				{
					string[] array = sm.msgBody.bizInfo.dataString.Split(',');
					for (int i = 0; i < array.Length; i++)
					{
						if (!string.IsNullOrEmpty(array[i]))
						{
							lingYuStatus.Add(array[i] == "1");
						}
					}
					Panel.LingYuPanel.Init(this, lingYuStatus);
					break;
				}
				}
			});
		}

		private void YueKaTogClick(bool isOn)
		{
			if (isOn)
			{
				m_CurrentPanelType = CashGoodsPanelType.YueKa;
			}
		}

		private void LiBaoTogClick(bool isOn)
		{
			if (isOn)
			{
				m_CurrentPanelType = CashGoodsPanelType.LiBao;
				m_LiBaoRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, 0f);
			}
			else
			{
				m_LiBaoRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, -2000f);
			}
		}

		private void LingYuTogClick(bool isOn)
		{
			if (isOn)
			{
				m_CurrentPanelType = CashGoodsPanelType.LingYu;
				m_LingYuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, 0f);
			}
			else
			{
				Panel.LingYuPanel.ClosePanel();
				m_LingYuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, -1000f);
			}
		}

		private void ChangShengLu(bool isOn)
		{
			if (isOn)
			{
				m_CurrentPanelType = CashGoodsPanelType.ChangShengLu;
				m_ChangShengLuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, 0f);
			}
			else
			{
				m_ChangShengLuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, -1000f);
			}
		}
	}
}
