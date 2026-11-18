using System;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class AH_PaiPinXiangQingWnd : Window
	{
		private AH_PaiPinXiangQingPanel m_Panel;

		private BaseNaJieGoodEntity m_TargetEntity;

		private int m_LingYuShu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<AH_PaiPinXiangQingPanel>();
			m_TargetEntity = paralist[0] as BaseNaJieGoodEntity;
			if (m_TargetEntity != null)
			{
				m_Panel.Name.text = m_TargetEntity.Name;
			}
			AddButtonClickListener(m_Panel.Exit, CloseWndClick);
			AddButtonClickListener(m_Panel.JiaHao, JiaHaoXiaoGuo);
			AddButtonClickListener(m_Panel.JianHao, JianHaoXiaoGuo);
			AddButtonClickListener(m_Panel.QiReng, DuiHuan);
			m_LingYuShu = 10000;
			m_Panel.LingShiDanJia.onEndEdit.AddListener(LingYuShu);
			Time();
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void Time()
		{
			m_Panel.Time.text = DateTime.Now.ToString("yyyy-MM-dd");
			PanDu();
		}

		private void JiaHaoXiaoGuo()
		{
			m_LingYuShu += 10000;
			PanDu();
		}

		private void JianHaoXiaoGuo()
		{
			m_LingYuShu -= 10000;
			PanDu();
		}

		private void LingYuShu(string count)
		{
			string text = m_Panel.LingShiDanJia.text;
			m_LingYuShu = int.Parse(text);
			PanDu();
		}

		private void PanDu()
		{
			if (m_LingYuShu < 9999)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最低价为一万灵石");
				m_LingYuShu = 10000;
			}
			m_Panel.LingShiDanJia.text = m_LingYuShu.ToString();
		}

		private void DuiHuan()
		{
			OnShelfTreasureReq_1006013 data = new OnShelfTreasureReq_1006013
			{
				StartPrice = m_LingYuShu,
				GoodsId = m_TargetEntity.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ON_SHELF_TREASURE_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					OnShelfTreasureResp_1006013 onShelfTreasureResp_ = new OnShelfTreasureResp_1006013();
					onShelfTreasureResp_.MergeFrom(byteString);
					if (onShelfTreasureResp_.IsSuccess)
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(onShelfTreasureResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<RoleManager>.Instance.RoleBoxDic[10].RemoveGoods(m_TargetEntity.GoodsId);
						Singleton<UIManager>.Instance.PopUpWnd("AH_WoDePaiMaoWnd.prefab", true);
						CloseWndClick();
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(onShelfTreasureResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
					}
				}
			});
		}
	}
}
