using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZF_CreateChooseWnd : Window
	{
		private ZF_CreateChoosePanel m_Panel;

		private int m_CurrentChoosedStage;

		private GameObject m_FuLuItemObj;

		private GameObject m_LianQiTogGoodItemObj;

		private ZhenPanChooseItemEntity m_ChooseData;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZF_CreateChoosePanel>();
			AddButtonClickListener(m_Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(m_Panel.KaiShiHuiZhiBtn, KaiShiHuiZhiBtnClick);
			AddButtonClickListener(m_Panel.ChaKanXiangQingBtn, ChaKanXiangQing);
			AddButtonClickListener(m_Panel.LingKuiLianZhiBtn, LingKuiLianZhiBtn);
			m_CurrentChoosedStage = 1;
			m_ChooseData = null;
			m_FuLuItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_TogItem.prefab");
			m_LianQiTogGoodItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			InitStage();
			GetFaShuData();
		}

		private void InitStage()
		{
			GridLayoutGroup component = m_Panel.Level_Content.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = m_Panel.Level_Content.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 9; i++)
			{
				SetListContentParent(m_Panel.Level_Content, m_FuLuItemObj).GetComponent<Common_TogItem>().SetValue(null, WorldSetting.GetStageName(i + 1), i + 1, component2, ChooseStageItem, i == 0);
			}
			SetGrticalListContentHight(9, m_Panel.Level_Content, component);
		}

		private void GetFaShuData()
		{
			SearchUnderstandMatrixListReq_1005058 data = new SearchUnderstandMatrixListReq_1005058
			{
				Stage = m_CurrentChoosedStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCHUNDERSTANDMATRIXLIST, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchUnderstandMatrixListResp_1005058 searchUnderstandMatrixListResp_ = new SearchUnderstandMatrixListResp_1005058();
					searchUnderstandMatrixListResp_.MergeFrom(byteString);
					if (searchUnderstandMatrixListResp_.IsSuccess)
					{
						List<ZhenPanChooseItemEntity> list = new List<ZhenPanChooseItemEntity>();
						foreach (SearchUnderstandMatrixListResp_1005058.Types.UnderstandMatrixResp_1005059 item in searchUnderstandMatrixListResp_.UnderstandMatrix)
						{
							list.Add(new ZhenPanChooseItemEntity
							{
								Id = item.Id,
								Name = item.Name,
								Rank = item.Rank,
								FiveElement = item.FiveElement,
								Level = item.Level
							});
						}
						CreateFuLuContent(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchUnderstandMatrixListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateFuLuContent(List<ZhenPanChooseItemEntity> data)
		{
			m_ChooseData = null;
			foreach (Transform item in m_Panel.FuLu_ScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			GridLayoutGroup component = m_Panel.FuLu_ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = m_Panel.FuLu_ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			foreach (ZhenPanChooseItemEntity datum in data)
			{
				UI_GoodsTogItem component3 = SetListContentParent(m_Panel.FuLu_ScrollViewContent, m_LianQiTogGoodItemObj).GetComponent<UI_GoodsTogItem>();
				Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(4, out var value);
				Singleton<FacadeManager>.Instance.SelectGoodsRankBg().TryGetValue(WorldSetting.GetLevelByRank(datum.Rank), out var value2);
				component3.SetValue(datum, datum.Name, datum.FiveElement, datum.Level, value2, value, component2, ChooseQiTuItem);
			}
			SetGrticalListContentHight(data.Count, m_Panel.FuLu_ScrollViewContent, component);
		}

		private void ChooseQiTuItem(bool isOn, object dataSource, GameObject gameObj)
		{
			if (isOn)
			{
				m_ChooseData = dataSource as ZhenPanChooseItemEntity;
				if (m_ChooseData != null)
				{
					m_Panel.KaiShiHuiZhiBtn.interactable = true;
					m_Panel.LingKuiLianZhiBtn.interactable = true;
					m_Panel.ChaKanXiangQingBtn.interactable = true;
				}
			}
		}

		private void ChooseFuLuItem(string name, int index, object dataSource)
		{
			m_ChooseData = dataSource as ZhenPanChooseItemEntity;
			m_Panel.ChaKanXiangQingBtn.interactable = true;
			m_Panel.KaiShiHuiZhiBtn.interactable = true;
		}

		private void ChooseStageItem(string name, int index, object dataSource)
		{
			m_CurrentChoosedStage = index;
			m_Panel.LingKuiLianZhiBtn.interactable = false;
			m_Panel.ChaKanXiangQingBtn.interactable = false;
			m_Panel.LingKuiLianZhiBtn.interactable = false;
			m_Panel.KaiShiHuiZhiBtn.interactable = false;
			GetFaShuData();
		}

		private void KaiShiHuiZhiBtnClick()
		{
			if (m_ChooseData == null || string.IsNullOrEmpty(m_ChooseData.Id))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先选择阵盘!");
				return;
			}
			VerifyIsCanRefiningMatrixReq_1005056 data = new VerifyIsCanRefiningMatrixReq_1005056
			{
				MethodMatrixId = m_ChooseData.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFYISCANREFININGMATRIX, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					VerifyIsCanRefiningMatrixResp_1005056 verifyIsCanRefiningMatrixResp_ = new VerifyIsCanRefiningMatrixResp_1005056();
					verifyIsCanRefiningMatrixResp_.MergeFrom(byteString);
					if (verifyIsCanRefiningMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZF_LianZhiWnd.prefab", true, m_ChooseData);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyIsCanRefiningMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChaKanXiangQing()
		{
			if (m_ChooseData != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(m_ChooseData.Id, m_ChooseData.Level, m_ChooseData.Rank, m_ChooseData.FiveElement, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						TitleName = m_ChooseData.Name,
						ContentList = list
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
		}

		private void LingKuiLianZhiBtn()
		{
			VerifyIsCanRefiningMatrixReq_1005056 data = new VerifyIsCanRefiningMatrixReq_1005056
			{
				MethodMatrixId = m_ChooseData.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFYISCANREFININGMATRIX, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					VerifyIsCanRefiningMatrixResp_1005056 verifyIsCanRefiningMatrixResp_ = new VerifyIsCanRefiningMatrixResp_1005056();
					verifyIsCanRefiningMatrixResp_.MergeFrom(byteString);
					if (verifyIsCanRefiningMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZF_Choose_LK_Wnd.prefab", true, m_ChooseData);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyIsCanRefiningMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
