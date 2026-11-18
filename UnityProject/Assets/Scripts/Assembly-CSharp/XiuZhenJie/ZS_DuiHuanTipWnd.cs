using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_DuiHuanTipWnd : Window
	{
		private ZS_DuiHuanTipPanel m_Panel;

		private GameObject m_ItemObje;

		private ZS_ZhenTuEntity m_DataSource;

		private ZS_DuiHuanWnd m_LingZhiDuiHuanWnd;

		private ZS_MianWnd m_ShenFuHuiWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZS_DuiHuanTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as ZS_ZhenTuEntity;
			}
			m_LingZhiDuiHuanWnd = Singleton<UIManager>.Instance.FindWndByName<ZS_DuiHuanWnd>("ZS_DuiHuanWnd.prefab");
			m_ShenFuHuiWnd = Singleton<UIManager>.Instance.FindWndByName<ZS_MianWnd>("ZS_MianWnd.prefab");
			AddButtonClickListener(m_Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(m_Panel.DuiHuanBtn, DuiHuanBtnClick);
			m_ItemObje = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DST_DanFangDuiHuanTipItem.prefab");
			InitXiangQing();
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void InitXiangQing()
		{
			m_Panel.TitleName.text = m_DataSource.Name;
			double num = Singleton<RoleManager>.Instance.Role_Entity.GongXianDian - m_DataSource.ExchangeContribution;
			num = ((num < 0.0) ? 0.0 : num);
			m_Panel.ShengYuDuiHuanDianText.text = $"兑换后贡献点还剩余{num:f0}点";
			GridLayoutGroup layoutGroup = m_Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> list) =>
			{
				foreach (string item in list)
				{
					string[] array = item.Split(',');
					DanFangDuiHuanTipItem component = SetListContentParent(m_Panel.ScrollViewContent, m_ItemObje).GetComponent<DanFangDuiHuanTipItem>();
					if (array[0] != "产地")
					{
						switch (array.Length)
						{
						case 2:
							component?.SetValue(array[0] + "：", array[1], "");
							break;
						case 3:
							component?.SetValue(array[0] + "：", array[1], array[2]);
							break;
						}
					}
					else if (array.Length == 2)
					{
						component?.SetValue(array[0] + "：", "虚神界", "");
					}
					else if (array != null && array.Length == 3)
					{
						component?.SetValue(array[0] + "：", "虚神界", array[2]);
					}
				}
				SetGrticalListContentHight(list.Count, m_Panel.ScrollViewContent, layoutGroup);
			});
		}

		private void DuiHuanBtnClick()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.GongXianDian < m_DataSource.ExchangeContribution)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("贡献点不足!");
				return;
			}
			ExchangeMethodMatrixReq_1008087 data = new ExchangeMethodMatrixReq_1008087
			{
				MethodMatrixId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.EXCHANGEMETHODMATRIX, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ExchangeMethodMatrixResp_1008087 exchangeMethodMatrixResp_ = new ExchangeMethodMatrixResp_1008087();
					exchangeMethodMatrixResp_.MergeFrom(byteString);
					if (exchangeMethodMatrixResp_.IsSuccess)
					{
						m_LingZhiDuiHuanWnd.ReFlashData();
						m_ShenFuHuiWnd.ReFlashData();
						CloseBtnClick();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(exchangeMethodMatrixResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
