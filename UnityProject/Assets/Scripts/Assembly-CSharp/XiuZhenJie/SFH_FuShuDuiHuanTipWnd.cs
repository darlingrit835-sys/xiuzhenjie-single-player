using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SFH_FuShuDuiHuanTipWnd : Window
	{
		public SFH_FuShuDuiHuanTipPanel Panel;

		private GameObject m_ItemObje;

		private SFH_FuShuEntity m_DataSource;

		private SFH_FuShuDuiHuanWnd m_FuShuDuiHuanWnd;

		private SFH_ShenFuHuiWnd m_ShenFuHuiWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<SFH_FuShuDuiHuanTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as SFH_FuShuEntity;
			}
			m_FuShuDuiHuanWnd = Singleton<UIManager>.Instance.FindWndByName<SFH_FuShuDuiHuanWnd>("SFH_FuShuDuiHuanWnd.prefab");
			m_ShenFuHuiWnd = Singleton<UIManager>.Instance.FindWndByName<SFH_ShenFuHuiWnd>("SFH_ShenFuHuiWnd.prefab");
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
			Singleton<RoleManager>.Instance.SelectTemplateSignDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> _list) =>
			{
				foreach (string item in _list)
				{
					DanFangDuiHuanTipItem component = SetListContentParent(Panel.ScrollViewContent, m_ItemObje).GetComponent<DanFangDuiHuanTipItem>();
					string[] array = item.Split(',');
					if (array.Length == 2)
					{
						component.SetValue(array[0], array[1], "");
					}
					else if (array.Length == 3)
					{
						component.SetValue(array[0], array[1], array[2]);
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
			ExchangeMethodSignReq_1008046 data = new ExchangeMethodSignReq_1008046
			{
				MethodSignId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.FUSHU_DUIHUAN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ExchangeMethodSignResp_1008046 exchangeMethodSignResp_ = new ExchangeMethodSignResp_1008046();
					exchangeMethodSignResp_.MergeFrom(byteString);
					if (exchangeMethodSignResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = exchangeMethodSignResp_.Contribution;
						m_FuShuDuiHuanWnd.ReFlashData();
						m_ShenFuHuiWnd.ReFlashData();
						CloseBtnClick();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(exchangeMethodSignResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
