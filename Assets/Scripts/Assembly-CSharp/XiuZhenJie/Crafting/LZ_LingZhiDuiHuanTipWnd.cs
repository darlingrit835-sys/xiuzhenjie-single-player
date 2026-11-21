using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_LingZhiDuiHuanTipWnd : Window
	{
		public LZ_LingZhiDuiHuanTipPanel Panel;

		private GameObject m_ItemObje;

		private LZ_LingZhiEntity m_DataSource;

		private LZ_LingZhiDuiHuanWnd m_LingZhiDuiHuanWnd;

		private LZ_MianWnd m_ShenFuHuiWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LZ_LingZhiDuiHuanTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as LZ_LingZhiEntity;
			}
			m_LingZhiDuiHuanWnd = Singleton<UIManager>.Instance.FindWndByName<LZ_LingZhiDuiHuanWnd>("LZ_LingZhiDuiHuanWnd.prefab");
			m_ShenFuHuiWnd = Singleton<UIManager>.Instance.FindWndByName<LZ_MianWnd>("LZ_MianWnd.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.DuiHuanBtn, DuiHuanBtnClick);
			m_ItemObje = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DST_DanFangDuiHuanTipItem.prefab");
			InitXiangQing();
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void InitXiangQing()
		{
			Panel.TitleName.text = m_DataSource.Name;
			double num = Singleton<RoleManager>.Instance.Role_Entity.GongXianDian - m_DataSource.ExchangeContribution;
			Panel.ShengYuDuiHuanDianText.text = string.Format("兑换后贡献点还剩余{0}点", ((num < 0.0) ? 0.0 : num).ToString("f0"));
			GridLayoutGroup layoutGroup = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(m_DataSource.Id, (List<string> _list) =>
			{
				foreach (string item in _list)
				{
					string[] array = item.Split(',');
					DanFangDuiHuanTipItem component = SetListContentParent(Panel.ScrollViewContent, m_ItemObje).GetComponent<DanFangDuiHuanTipItem>();
					if (array[0] != "产地")
					{
						if (array != null && array.Length == 2)
						{
							component?.SetValue(array[0] + "：", array[1], "");
						}
						else if (array != null && array.Length == 3)
						{
							component?.SetValue(array[0] + "：", array[1], array[2]);
						}
					}
					else if (array != null && array.Length == 2)
					{
						component?.SetValue(array[0] + "：", "虚神界", "");
					}
					else if (array != null && array.Length == 3)
					{
						component?.SetValue(array[0] + "：", "虚神界", array[2]);
					}
				}
				SetGrticalListContentHight(_list.Count, Panel.ScrollViewContent, layoutGroup);
			});
		}

		private void DuiHuanBtnClick()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.GongXianDian < m_DataSource.ExchangeContribution)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("贡献点不足!");
				return;
			}
			ExchangeTemplatePlantReq_1008057 data = new ExchangeTemplatePlantReq_1008057
			{
				TemplatePlantId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.EXCHANGE_TEMPLATE_PLANT_RESP, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ExchangeTemplatePlantResp_1008057 exchangeTemplatePlantResp_ = new ExchangeTemplatePlantResp_1008057();
					exchangeTemplatePlantResp_.MergeFrom(byteString);
					if (exchangeTemplatePlantResp_.IsSuccess)
					{
						m_LingZhiDuiHuanWnd.ReFlashData();
						m_ShenFuHuiWnd.ReFlashData();
						CloseBtnClick();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(exchangeTemplatePlantResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
