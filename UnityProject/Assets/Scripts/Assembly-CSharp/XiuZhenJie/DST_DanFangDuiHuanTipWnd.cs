using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_DanFangDuiHuanTipWnd : Window
	{
		public DST_DanFangDuiHuanTipPanel Panel;

		private GameObject m_ItemObje;

		private DST_DanFangEntity m_DataSource;

		private DST_DanFangDuiHuanWnd m_DanFangDuiHuanWnd;

		private DanShiTaWnd m_DanShiTaWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DST_DanFangDuiHuanTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as DST_DanFangEntity;
			}
			m_DanFangDuiHuanWnd = Singleton<UIManager>.Instance.FindWndByName<DST_DanFangDuiHuanWnd>("DST_DanFangDuiHuanWnd.prefab");
			m_DanShiTaWnd = Singleton<UIManager>.Instance.FindWndByName<DanShiTaWnd>("DanShiTaWnd.prefab");
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
			Singleton<RoleManager>.Instance.SelectTemplateElixirDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> _list) =>
			{
				foreach (string item in _list)
				{
					DanFangDuiHuanTipItem component = SetListContentParent(Panel.ScrollViewContent, m_ItemObje).GetComponent<DanFangDuiHuanTipItem>();
					string[] array = item.Split(',');
					if (array.Length == 2)
					{
						if (!array[0].Contains("材料"))
						{
							component.SetValue(array[0], array[1], "");
						}
					}
					else if (array.Length == 3 && !array[0].Contains("材料"))
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
			ExchangeMethodElixirReq_1008014 data = new ExchangeMethodElixirReq_1008014
			{
				MethodElixirId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_DANFANGDUIHUAN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ExchangeMethodElixirResp_1008014 exchangeMethodElixirResp_ = new ExchangeMethodElixirResp_1008014();
					exchangeMethodElixirResp_.MergeFrom(byteString);
					if (exchangeMethodElixirResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = exchangeMethodElixirResp_.Contribution;
						m_DanFangDuiHuanWnd.ReFlashData();
						m_DanShiTaWnd.ReFlashData();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(exchangeMethodElixirResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
