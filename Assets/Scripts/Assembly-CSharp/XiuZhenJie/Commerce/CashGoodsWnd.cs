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
		internal int PageRevision { get; private set; }

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<CashGoodsPanel>();
			m_LiBaoRect = Panel.LiBaoPanel.GetComponent<RectTransform>();
			m_LingYuRect = Panel.LingYuPanel.GetComponent<RectTransform>();
			m_ChangShengLuRect = Panel.ChangShengLuPanel.GetComponent<RectTransform>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddToggleClickListener(Panel.YueKaTog, YueKaTogClick);
			AddToggleClickListener(Panel.LiBaoTog, LiBaoTogClick);
			AddToggleClickListener(Panel.LingYuTog, LingYuTogClick);
			AddToggleClickListener(Panel.ChangShengLuTog, ChangShengLu);
			m_CurrentPanelType = CashGoodsPanelType.YueKa;
			InkCurrencyArt.StyleStore(Panel);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length > 0 && paralist[0] is CashGoodsPanelType page && page == CashGoodsPanelType.LingYu &&
				Panel.LingYuTog.isActiveAndEnabled && Panel.LingYuTog.IsInteractable() && !Panel.LingYuTog.isOn)
			{
				Panel.LingYuTog.isOn = true;
				return;
			}
			LoadPage(m_CurrentPanelType);
		}

		private void LoadPage(CashGoodsPanelType page)
		{
			m_CurrentPanelType = page;
			PageRevision++;
			switch (page)
			{
			case CashGoodsPanelType.YueKa:
				Panel.YueKaPanel.Init(this);
				break;
			case CashGoodsPanelType.LiBao:
				GetLiBaoDataStatus(PageRevision);
				break;
			case CashGoodsPanelType.LingYu:
				MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(GetLingYuStatus(PageRevision));
				break;
			case CashGoodsPanelType.ChangShengLu:
				Panel.ChangShengLuPanel.Init(this);
				break;
			}
		}

		internal bool IsCurrentPage(int revision)
		{
			return revision == PageRevision && GameObject != null && GameObject.activeInHierarchy;
		}

		private void GetLiBaoDataStatus(int revision)
		{
			List<CashGoodsBuyStatus> liBaoStatus = new List<CashGoodsBuyStatus>();
			List<CashGoodsBuyStatus> xinChunLiBaoStatus = new List<CashGoodsBuyStatus>();
			Singleton<NetManager>.Instance.SendMsg(MsgType.LiBaoBuyInfo, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (!IsCurrentPage(revision)) return;
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

		private IEnumerator GetLingYuStatus(int revision)
		{
			Panel.LingYuPanel.Init(this);
			if (string.IsNullOrWhiteSpace(Constants.PayIp) || Constants.PayPort <= 0 ||
				!System.Uri.TryCreate($"https://{Constants.PayIp}:{Constants.PayPort}", System.UriKind.Absolute, out _))
				yield break;

			var query = new Dictionary<string, string>
			{
				{ "roleId", Singleton<RoleManager>.Instance.Role_Entity.Id },
				{ "channelId", "1" }
			};
			using var www = HttpsRequest.UnityGetRequest(Constants.PayIp, Constants.PayPort, NetInterfaceConstants.HTTP_PAY_SELECT_LINGYU_STATUS, query);
			www.timeout = 8;
			yield return www.SendWebRequest();
			if (!IsCurrentPage(revision) || www.result != UnityWebRequest.Result.Success || string.IsNullOrEmpty(www.downloadHandler.text))
				yield break;

			HttpsRequest.DealwithShellModel(www.downloadHandler.text, (status, response, message) =>
			{
				var result = response?.msgBody?.bizInfo;
				if (status == HttpsRequestStatus.SuccessFul && !string.IsNullOrEmpty(result?.dataString))
				{
					Panel.LingYuPanel.ApplyFirstRecharge(result.dataString.Split(','));
				}
				else if (status == HttpsRequestStatus.ServerError && !string.IsNullOrEmpty(result?.msg))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(result.msg);
				}
			});
		}

		private void YueKaTogClick(bool isOn)
		{
			if (isOn)
			{
				LoadPage(CashGoodsPanelType.YueKa);
			}
		}

		private void LiBaoTogClick(bool isOn)
		{
			if (isOn)
			{
				m_LiBaoRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, 0f);
				LoadPage(CashGoodsPanelType.LiBao);
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
				m_LingYuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, 0f);
				LoadPage(CashGoodsPanelType.LingYu);
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
				m_ChangShengLuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, 0f);
				LoadPage(CashGoodsPanelType.ChangShengLu);
			}
			else
			{
				m_ChangShengLuRect.anchoredPosition = new Vector2(m_LiBaoRect.anchoredPosition.x, -1000f);
			}
		}
	}
}
