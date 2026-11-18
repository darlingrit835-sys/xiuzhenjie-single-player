using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class HuaFuChoose_LK_Wnd : Window
	{
		public HuaFuChoose_LK_Panel Panel;

		private FuLuChooseItemEntity m_DataSource;

		private BaseNaJieGoodEntity m_LingKuiGoods;

		private bool m_IsHaveLingKui;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<HuaFuChoose_LK_Panel>();
			m_DataSource = paralist[0] as FuLuChooseItemEntity;
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
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
				Panel.TitleContent.text = $"灵傀炼制需消耗与符书同品阶的灵傀，不消耗精力，不增长炼制经验。当前选择符书品阶为：<color=red>{levelByRankName}</color> ";
				Panel.Title.text = m_DataSource.Name;
				Sprite value = null;
				Singleton<FacadeManager>.Instance.SelectGoodsRankBg().TryGetValue(WorldSetting.GetLevelByRank(m_DataSource.Rank), out value);
				Sprite value2 = null;
				Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(36, out value2);
				Panel.XiaoHaoPinGoodsItem.SetItem(null, "灵傀", 0, 1, value, value2, null);
			}
		}

		private void QueRenBtnClick()
		{
			PuppetRefiningSignReq_1005046 data = new PuppetRefiningSignReq_1005046
			{
				MethodSignId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PUPPET_REFINING_SIGN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PuppetRefiningSignResp_1005046 puppetRefiningSignResp_ = new PuppetRefiningSignResp_1005046();
					puppetRefiningSignResp_.MergeFrom(byteString);
					if (puppetRefiningSignResp_.IsSuccess)
					{
						Panel.ChoosePanel.gameObject.SetActive(value: false);
						AddButtonClickListener(Panel.SuccessPanelCloseBtn, CloseWindow);
						AddButtonClickListener(Panel.SuccessPanelSureBtn, CloseWindow);
						string levelByRankName = WorldSetting.GetLevelByRankName(puppetRefiningSignResp_.Rank);
						string name = m_DataSource.Name;
						PopSuccessBg(levelByRankName, name);
						if (m_LingKuiGoods != null)
						{
							m_LingKuiGoods.GoodsNum = puppetRefiningSignResp_.PuppetCount;
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(puppetRefiningSignResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void PopSuccessBg(string pinJie, string FuName)
		{
			Panel.SuccessPanelTitleText.text = $"恭喜！成功获取“{pinJie}”{FuName}";
			Panel.SuccessPanel.gameObject.SetActive(value: true);
		}
	}
}
