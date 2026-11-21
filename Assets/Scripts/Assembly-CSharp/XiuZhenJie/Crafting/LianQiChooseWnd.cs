using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiChooseWnd : Window
	{
		public LianQiChoosePanel Panel;

		private int m_CurrentChoosedStage;

		private GameObject m_LianQiItemObj;

		private GameObject m_LianQiTogGoodItemObj;

		private LianQiChooseItemEntity m_ChooseData;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiChoosePanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.KaiShiHuiZhiBtn, KaiShiLianZhiBtnClick);
			AddButtonClickListener(Panel.ChaKanXiangQingBtn, ChaKanXiangQing);
			AddButtonClickListener(Panel.LingKuiLianZhiBtn, LingKuiLianZhiBtn);
			m_CurrentChoosedStage = 1;
			m_LianQiItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_TogItem.prefab");
			m_LianQiTogGoodItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			int ticket = ++showTicket;
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ENTER_EQUIPMENT_ASSOCIATION, null, bytes =>
			{
				if (ticket != showTicket || GameObject == null || !GameObject.activeInHierarchy)
					return;
				if (bytes != null && ForgeWire.WorkshopEnabled(bytes))
					ForgeWorkshop.OpenIn(Panel.transform, Panel.GetComponentInChildren<Text>(true).font, CloseWindow);
				else
					InitStage();
			}))
				InitStage();
		}

		private int showTicket;
        public override void OnDisable()
        {
            base.OnDisable();
            showTicket++;
            var sheet = Panel.transform.Find("ForgeWorkshop");
            if (sheet != null) Object.Destroy(sheet.gameObject);
        }

        private void InitStage()
		{
			GridLayoutGroup component = Panel.Level_Content.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.Level_Content.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 9; i++)
			{
				SetListContentParent(Panel.Level_Content, m_LianQiItemObj).GetComponent<Common_TogItem>().SetValue(null, WorldSetting.GetStageName(i + 1), i + 1, component2, ChooseStageItem, i == 0);
			}
			SetGrticalListContentHight(9, Panel.Level_Content, component);
		}

		private void ChooseStageItem(string name, int index, object dataSource)
		{
			m_CurrentChoosedStage = index;
			Panel.ChaKanXiangQingBtn.interactable = false;
			Panel.KaiShiHuiZhiBtn.interactable = false;
			GetLianQiData();
		}

		private void GetLianQiData()
		{
			SearchUnderstandEquipmentListReq_1005051 data = new SearchUnderstandEquipmentListReq_1005051
			{
				Stage = m_CurrentChoosedStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_UNDERSTAND_EQUIPMENT_LIST, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchUnderstandEquipmentListResp_1005051 searchUnderstandEquipmentListResp_ = new SearchUnderstandEquipmentListResp_1005051();
					searchUnderstandEquipmentListResp_.MergeFrom(byteString);
					if (searchUnderstandEquipmentListResp_.IsSuccess)
					{
						List<LianQiChooseItemEntity> list = new List<LianQiChooseItemEntity>();
						foreach (SearchUnderstandEquipmentListResp_1005051.Types.UnderstandEquipmentResp_1005052 item in searchUnderstandEquipmentListResp_.UnderstandEquipment)
						{
							list.Add(new LianQiChooseItemEntity
							{
								Id = item.Id,
								Name = item.Name,
								Rank = item.Rank,
								FiveElement = item.FiveElement,
								Level = item.Level,
								Type = item.Type,
								Icon = item.Icon
							});
						}
						CreateLianQiContent(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchUnderstandEquipmentListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateLianQiContent(List<LianQiChooseItemEntity> data)
		{
			foreach (Transform item in Panel.LianQi_ScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			GridLayoutGroup component = Panel.LianQi_ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.LianQi_ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			foreach (LianQiChooseItemEntity datum in data)
			{
				UI_GoodsTogItem component3 = SetListContentParent(Panel.LianQi_ScrollViewContent, m_LianQiTogGoodItemObj).GetComponent<UI_GoodsTogItem>();
				Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(4, out var value);
				Singleton<FacadeManager>.Instance.SelectGoodsRankBg().TryGetValue(WorldSetting.GetLevelByRank(datum.Rank), out var value2);
				component3.SetValue(datum, datum.Name, datum.FiveElement, datum.Level, value2, value, component2, ChooseQiTuItem);
			}
			SetGrticalListContentHight(data.Count, Panel.LianQi_ScrollViewContent, component);
		}

		private void ChooseQiTuItem(bool isOn, object dataSource, GameObject gameObj)
		{
			if (!isOn)
			{
				return;
			}
			m_ChooseData = dataSource as LianQiChooseItemEntity;
			if (m_ChooseData != null)
			{
				Panel.KaiShiHuiZhiBtn.interactable = true;
				Panel.LingKuiLianZhiBtn.interactable = true;
				if (m_ChooseData.Name == "练习用器图")
				{
					Panel.ChaKanXiangQingBtn.interactable = false;
					Panel.LingKuiLianZhiBtn.interactable = false;
				}
				else
				{
					Panel.ChaKanXiangQingBtn.interactable = true;
					Panel.LingKuiLianZhiBtn.interactable = true;
				}
			}
		}

		private void KaiShiLianZhiBtnClick()
		{
			if (m_ChooseData == null)
			{
				return;
			}
			if (m_ChooseData.Name == "练习用器图")
			{
				GetQiTuInfo();
				return;
			}
			SearchEquipmentRefiningStepListReq_1005053 data = new SearchEquipmentRefiningStepListReq_1005053
			{
				MethodId = m_ChooseData.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_EQUIPMENT_REFINING_STEPLIST, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchEquipmentRefiningStepListResp_1005053 searchEquipmentRefiningStepListResp_ = new SearchEquipmentRefiningStepListResp_1005053();
					searchEquipmentRefiningStepListResp_.MergeFrom(byteString);
					if (searchEquipmentRefiningStepListResp_.IsSuccess)
					{
						GetQiTuInfo();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchEquipmentRefiningStepListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetQiTuInfo()
		{
			SearchEquipmentRefiningStepListReq_1005053 data = new SearchEquipmentRefiningStepListReq_1005053
			{
				MethodId = m_ChooseData.Id
			};
			if (m_ChooseData.Name == "练习用器图")
			{
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_EQUIPMENT_REFINING_STEPLIST, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchEquipmentRefiningStepListResp_1005053 searchEquipmentRefiningStepListResp_ = new SearchEquipmentRefiningStepListResp_1005053();
					searchEquipmentRefiningStepListResp_.MergeFrom(byteString);
					if (searchEquipmentRefiningStepListResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("LianQiCaiLiaoShaiXuanWnd.prefab", true, FuZhi(searchEquipmentRefiningStepListResp_));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchEquipmentRefiningStepListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChaKanXiangQing()
		{
			if (m_ChooseData != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateQiTuDetail(m_ChooseData.Id, m_ChooseData.Level, m_ChooseData.Rank, m_ChooseData.FiveElement, (List<string> list) =>
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

		private QiTuEntity FuZhi(SearchEquipmentRefiningStepListResp_1005053 data)
		{
			QiTuEntity qiTuEntity = new QiTuEntity
			{
				Id = m_ChooseData.Id,
				Name = m_ChooseData.Name,
				Stage = m_CurrentChoosedStage,
				Rank = data.Rank,
				Type = m_ChooseData.Type,
				Points = new List<QiTuStepModel>(),
				CaiLiaoList = new List<QiTuCaiLiaoEntity>(),
				Icon = m_ChooseData.Icon,
				MaxChuiLianTimes = 6
			};
			foreach (SearchEquipmentRefiningStepListResp_1005053.Types.EquipmentRefiningStepResp_1005054 item3 in data.Step)
			{
				QiTuStepModel item = new QiTuStepModel
				{
					ActionType = item3.ActionType,
					TargetType = item3.TargetType,
					Order = item3.Order,
					ActionNum = item3.ActionNum,
					BestVal = item3.BestVal,
					MINVal = item3.MinVal,
					MAXVal = item3.MaxVal,
					ValType = item3.ValType,
					StepId = item3.StepId,
					TargetName = item3.TargetName,
					MapId = item3.MapId,
					MapName = item3.MapName,
					TargetId = item3.TargetId
				};
				qiTuEntity.Points.Add(item);
			}
			foreach (SearchEquipmentRefiningStepListResp_1005053.Types.EquipmentRefiningStepResp_1005054 item4 in data.Step)
			{
				QiTuCaiLiaoEntity item2 = new QiTuCaiLiaoEntity
				{
					TargetType = item4.TargetType,
					TargetName = item4.TargetName,
					MapId = item4.MapId,
					MapName = item4.MapName,
					TargetId = item4.TargetId
				};
				qiTuEntity.CaiLiaoList.Add(item2);
			}
			return qiTuEntity;
		}

		private void LingKuiLianZhiBtn()
		{
			SearchEquipmentRefiningStepListReq_1005053 data = new SearchEquipmentRefiningStepListReq_1005053
			{
				MethodId = m_ChooseData.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_EQUIPMENT_REFINING_STEPLIST, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchEquipmentRefiningStepListResp_1005053 searchEquipmentRefiningStepListResp_ = new SearchEquipmentRefiningStepListResp_1005053();
					searchEquipmentRefiningStepListResp_.MergeFrom(byteString);
					if (searchEquipmentRefiningStepListResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("LianQiCaiLiaoShaiXuan_LK_Wnd.prefab", true, FuZhi(searchEquipmentRefiningStepListResp_));
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchEquipmentRefiningStepListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
