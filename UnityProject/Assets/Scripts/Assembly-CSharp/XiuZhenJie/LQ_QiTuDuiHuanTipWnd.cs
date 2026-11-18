using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LQ_QiTuDuiHuanTipWnd : Window
	{
		public LQ_QiTuDuiHuanTipPanel Panel;

		private GameObject m_ItemObje;

		private LQ_LianQiEntity m_DataSource;

		private LianQi_DuiHuanWnd m_LingZhiDuiHuanWnd;

		private LianQi_ChuangZaoMianWnd m_ShenFuHuiWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LQ_QiTuDuiHuanTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as LQ_LianQiEntity;
			}
			m_LingZhiDuiHuanWnd = Singleton<UIManager>.Instance.FindWndByName<LianQi_DuiHuanWnd>("LianQi_DuiHuanWnd.prefab");
			m_ShenFuHuiWnd = Singleton<UIManager>.Instance.FindWndByName<LianQi_ChuangZaoMianWnd>("LianQi_ChuangZaoMianWnd.prefab");
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
			num = ((num < 0.0) ? 0.0 : num);
			Panel.ShengYuDuiHuanDianText.text = $"兑换后贡献点还剩余{num:f0}点";
			GridLayoutGroup layoutGroup = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			Singleton<RoleManager>.Instance.SelectTemplateQiTuDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> list) =>
			{
				foreach (string item in list)
				{
					string[] array = item.Split(',');
					DanFangDuiHuanTipItem component = SetListContentParent(Panel.ScrollViewContent, m_ItemObje).GetComponent<DanFangDuiHuanTipItem>();
					if (array.Length == 2)
					{
						if (!array[0].Contains("材料"))
						{
							component?.SetValue(array[0], array[1], "");
						}
					}
					else if (array != null && array.Length == 3 && !array[0].Contains("材料"))
					{
						component?.SetValue(array[0], array[1], array[2]);
					}
				}
				SetGrticalListContentHight(list.Count, Panel.ScrollViewContent, layoutGroup);
			});
		}

		private void DuiHuanBtnClick()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.GongXianDian < m_DataSource.ExchangeContribution)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("贡献点不足!");
				return;
			}
			ExchangeMethodEquipmentReq_1008069 data = new ExchangeMethodEquipmentReq_1008069
			{
				MethodEquipmentId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.EXCHANGE_METHOD_EQUIPMENT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ExchangeMethodEquipmentResp_1008069 exchangeMethodEquipmentResp_ = new ExchangeMethodEquipmentResp_1008069();
					exchangeMethodEquipmentResp_.MergeFrom(byteString);
					if (exchangeMethodEquipmentResp_.IsSuccess)
					{
						m_LingZhiDuiHuanWnd.ReFlashData();
						m_ShenFuHuiWnd.ReFlashData();
						CloseBtnClick();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(exchangeMethodEquipmentResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
