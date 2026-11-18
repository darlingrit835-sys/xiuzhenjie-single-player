using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZF_Choose_LK_Wnd : Window
	{
		private ZF_Choose_LK_Panel m_Panel;

		private ZhenPanChooseItemEntity m_DataSource;

		private BaseNaJieGoodEntity m_LingKuiGoods;

		private bool m_IsHaveLingKui;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZF_Choose_LK_Panel>();
			m_DataSource = paralist[0] as ZhenPanChooseItemEntity;
			AddButtonClickListener(m_Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(m_Panel.QueRenBtn, QueRenBtnClick);
			InitGoods();
		}

		private void InitGoods()
		{
			List<BaseNaJieGoodEntity> list = Singleton<RoleManager>.Instance.RoleBoxDic[10].SelectGoods(36, m_DataSource.Rank);
			if (list == null || list.Count == 0)
			{
				m_IsHaveLingKui = false;
			}
			else
			{
				m_IsHaveLingKui = true;
				m_LingKuiGoods = list[0];
			}
			if (m_DataSource != null)
			{
				string levelByRankName = WorldSetting.GetLevelByRankName(m_DataSource.Rank);
				m_Panel.TitleContent.text = "灵傀炼制需消耗与阵盘同品阶的灵傀，不消耗精力，不增长炼制经验。当前选择阵盘品阶为：<color=red>" + levelByRankName + "</color> ";
				m_Panel.Title.text = m_DataSource.Name;
				Singleton<FacadeManager>.Instance.SelectGoodsRankBg().TryGetValue(WorldSetting.GetLevelByRank(m_DataSource.Rank), out var value);
				Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(36, out var value2);
				m_Panel.XiaoHaoPinGoodsItem.SetItem(null, "灵傀", 0, 1, value, value2, null);
			}
		}

		private void QueRenBtnClick()
		{
			PuppetRefiningMatrixReq_1005061 data = new PuppetRefiningMatrixReq_1005061
			{
				MethodMatrixId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PUPPETREFININGMATRIX, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					PuppetRefiningMatrixResp_1005061 puppetRefiningMatrixResp_ = new PuppetRefiningMatrixResp_1005061();
					puppetRefiningMatrixResp_.MergeFrom(byteString);
					if (puppetRefiningMatrixResp_.IsSuccess)
					{
						m_Panel.ChoosePanel.gameObject.SetActive(value: false);
						AddButtonClickListener(m_Panel.SuccessPanelCloseBtn, CloseWindow);
						AddButtonClickListener(m_Panel.SuccessPanelSureBtn, CloseWindow);
						string levelByRankName = WorldSetting.GetLevelByRankName(puppetRefiningMatrixResp_.Rank);
						string name = m_DataSource.Name;
						PopSuccessBg(levelByRankName, name);
						if (m_LingKuiGoods != null)
						{
							m_LingKuiGoods.GoodsNum = puppetRefiningMatrixResp_.PuppetCount;
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(puppetRefiningMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void PopSuccessBg(string pinJie, string fuName)
		{
			m_Panel.SuccessPanelTitleText.text = "恭喜！成功获取“" + pinJie + "”" + fuName;
			m_Panel.SuccessPanel.gameObject.SetActive(value: true);
		}
	}
}
