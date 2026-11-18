using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DJSD_GongFaDuiHuanTipWnd : Window
	{
		public DJSD_GongFaDuiHuanTipPanel Panel;

		private GameObject m_ItemObje;

		private DJSD_GongFaEntity m_DataSource;

		private DaoJingShengDianWnd m_DaoJingShengDianWnd;

		private DJSD_GongFaDuiHuanWnd m_DJSD_DuiHuanWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DJSD_GongFaDuiHuanTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as DJSD_GongFaEntity;
			}
			m_DaoJingShengDianWnd = Singleton<UIManager>.Instance.FindWndByName<DaoJingShengDianWnd>("DaoJingShengDianWnd.prefab");
			m_DJSD_DuiHuanWnd = Singleton<UIManager>.Instance.FindWndByName<DJSD_GongFaDuiHuanWnd>("DJSD_GongFaDuiHuanWnd.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.DuiHuanBtn, DuiHuanBtnClick);
			m_ItemObje = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DST_DanFangDuiHuanTipItem.prefab");
			InitXiangQing();
		}

		private void InitXiangQing()
		{
			Panel.TitleName.text = m_DataSource.Name;
			double num = Singleton<RoleManager>.Instance.Role_Entity.GongXianDian - m_DataSource.ExchangeContribution;
			Panel.ShengYuDuiHuanDianText.text = string.Format("兑换后贡献点还剩余{0}点", ((num < 0.0) ? 0.0 : num).ToString("f0"));
			GridLayoutGroup layoutGroup = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> _list) =>
			{
				foreach (string item in _list)
				{
					DanFangDuiHuanTipItem component = SetListContentParent(Panel.ScrollViewContent, m_ItemObje).GetComponent<DanFangDuiHuanTipItem>();
					string[] array = item.Split(',');
					if (array.Length == 2)
					{
						if (!(array[0] == "材料"))
						{
							component.SetValue(array[0], array[1], "");
						}
					}
					else if (array.Length == 3 && !(array[0] == "材料"))
					{
						component.SetValue(array[0], array[1], array[2]);
					}
				}
				SetGrticalListContentHight(_list.Count, Panel.ScrollViewContent, layoutGroup);
			});
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void DuiHuanBtnClick()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.GongXianDian < m_DataSource.ExchangeContribution)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("贡献点不足!");
				return;
			}
			ExchangeMethodAchievementReq_1008032 data = new ExchangeMethodAchievementReq_1008032
			{
				MethodAchievementId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DJSD_GONGFADUIHUAN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ExchangeMethodAchievementResp_1008032 exchangeMethodAchievementResp_ = new ExchangeMethodAchievementResp_1008032();
					exchangeMethodAchievementResp_.MergeFrom(byteString);
					if (exchangeMethodAchievementResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = exchangeMethodAchievementResp_.Contribution;
						m_DaoJingShengDianWnd.ReFlashData();
						m_DJSD_DuiHuanWnd.ReFlashData();
						CloseBtnClick();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(exchangeMethodAchievementResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
