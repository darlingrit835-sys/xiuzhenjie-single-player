using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeWnd : Window
	{
		public DiHuoGePanel Panel;

		private int m_CurrentStage;

		private Text m_RecipeStatus;

		private bool m_LegacyRecipes;

		private int m_RecipeRequest;

		private LianZhiType m_CurrentLianZhiType;

		private object m_ChooseData;

		private DiHuoGeEnterType m_CurrentEnterType;

		private DST_KaoShiItemModel m_KaoHeDanFangModel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DiHuoGePanel>();
			InkCraftHalls.StyleSelection(Panel.transform, "ContentBg", "ForgeHall", Panel.LianZhiButton, Panel.ChaKanXiangQing, Panel.LingKuiLianZhiBtn);
			m_RecipeStatus = InkCraftHalls.AddListStatus(Panel.ToggleGroup_4_ScrollContent, Panel.LianZhiButton.GetComponentInChildren<Text>().font);
			AddButtonClickListener(Panel.LiKaiButton, LiKaiClick);
			AddButtonClickListener(Panel.LianZhiButton, LianZhiClick);
			AddButtonClickListener(Panel.ChaKanXiangQing, ChaKanXiangQing);
			AddButtonClickListener(Panel.LingKuiLianZhiBtn, LingKuiLianZhi);
			m_CurrentLianZhiType = LianZhiType.DanFang;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: false);
			if (paralist.Length != 0)
			{
				m_CurrentEnterType = (DiHuoGeEnterType)paralist[0];
				switch (m_CurrentEnterType)
				{
				case DiHuoGeEnterType.ChuangZao:
					FromChuangZao();
					return;
				case DiHuoGeEnterType.KaoShi:
					m_KaoHeDanFangModel = paralist[1] as DST_KaoShiItemModel;
					FromKaoShi();
					return;
				}
			}
			if (!m_LegacyRecipes)
			{
				AlchemyWorkshop.Open((RectTransform)Panel.transform, Panel.LianZhiButton.GetComponentInChildren<Text>().font,
					LiKaiClick, OpenLegacyRecipes);
				return;
			}
			ShowLegacyRecipes();
		}

		private void OpenLegacyRecipes()
		{
			m_LegacyRecipes = true;
			ShowLegacyRecipes();
		}

		private void ShowLegacyRecipes()
		{
			m_CurrentStage = 1;
			m_CurrentLianZhiType = LianZhiType.DanFang;
			Panel.LianZhiButton.interactable = false;
			Panel.ChaKanXiangQing.interactable = false;
			InitUI_Data();
		}

		private void FromKaoShi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianZhiWnd.prefab", true, null, null, DiHuoGeEnterType.KaoShi, m_KaoHeDanFangModel);
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void FromChuangZao()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChuangZhao_LianZhiDengJieWnd.prefab", true);
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public void InitUI_Data()
		{
			InitGroup_2();
		}

		public override void OnDisable()
		{
			base.OnDisable();
			AlchemyWorkshop.CloseIn(Panel.transform);
			m_RecipeRequest++;
			ClearGroup_2();
			ClearGourp_4();
		}

		private void InitGroup_1()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeChooseSmallItem.prefab");
			ToggleGroup component = Panel.ToggleGroup_1.gameObject.GetComponent<ToggleGroup>();
			GridLayoutGroup component2 = Panel.ToggleGroup_1.gameObject.GetComponent<GridLayoutGroup>();
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = SetListContentParent(Panel.ToggleGroup_1, insObj);
				gameObject.GetComponent<Toggle>().group = component;
				DiHuoGeChooseSmallItem component3 = gameObject.GetComponent<DiHuoGeChooseSmallItem>();
				switch (i)
				{
				case 0:
					component3.SetValue(null, "丹方", i, component, Group_1_Click, isChoose: true);
					break;
				case 1:
					component3.SetValue(null, "器图", i, component, Group_1_Click);
					break;
				case 2:
					component3.SetValue(null, "阵盘", i, component, Group_1_Click);
					break;
				}
			}
			SetGrticalListContentHight(3, Panel.ToggleGroup_1, component2);
		}

		private void ClearGroup_1()
		{
			foreach (Transform item in Panel.ToggleGroup_1)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void InitGroup_2()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeChooseSmallItem.prefab");
			GridLayoutGroup component = Panel.ToggleGroup_2.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.ToggleGroup_2.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 9; i++)
			{
				GameObject gameObject = SetListContentParent(Panel.ToggleGroup_2, insObj);
				gameObject.GetComponent<Toggle>().group = component2;
				DiHuoGeChooseSmallItem component3 = gameObject.GetComponent<DiHuoGeChooseSmallItem>();
				switch (i)
				{
				case 0:
					component3.SetValue(null, "练气", i, component2, Group_2_Click, isChoose: true);
					break;
				case 1:
					component3.SetValue(null, "筑基", i, component2, Group_2_Click);
					break;
				case 2:
					component3.SetValue(null, "结丹", i, component2, Group_2_Click);
					break;
				case 3:
					component3.SetValue(null, "元婴", i, component2, Group_2_Click);
					break;
				case 4:
					component3.SetValue(null, "分神", i, component2, Group_2_Click);
					break;
				case 5:
					component3.SetValue(null, "炼虚", i, component2, Group_2_Click);
					break;
				case 6:
					component3.SetValue(null, "合体", i, component2, Group_2_Click);
					break;
				case 7:
					component3.SetValue(null, "渡劫", i, component2, Group_2_Click);
					break;
				case 8:
					component3.SetValue(null, "大乘", i, component2, Group_2_Click);
					break;
				}
			}
			SetGrticalListContentHight(9, Panel.ToggleGroup_2, component);
		}

		private void ClearGroup_2()
		{
			foreach (Transform item in Panel.ToggleGroup_2)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void InitGroup_3()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeChooseBigItem.prefab");
			GridLayoutGroup component = Panel.ToggleGroup_3.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.ToggleGroup_3.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = SetListContentParent(Panel.ToggleGroup_3, insObj);
				Toggle component3 = gameObject.GetComponent<Toggle>();
				component3.interactable = false;
				component3.group = component2;
				DiHuoGeChooseBigItem component4 = gameObject.GetComponent<DiHuoGeChooseBigItem>();
				switch (i)
				{
				case 0:
					component4.SetValue(null, isChoose: true, "地表之火", 0);
					break;
				case 1:
					component4.SetValue(null, isChoose: false, "地肺之火", 0);
					break;
				case 2:
					component4.SetValue(null, isChoose: false, "地脉之火", 0);
					break;
				}
			}
			SetGrticalListContentHight(3, Panel.ToggleGroup_3, component);
		}

		private void ClearGroup_3()
		{
			foreach (Transform item in Panel.ToggleGroup_3)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void InitGroup_4()
		{
			ClearGourp_4();
			m_ChooseData = null;
			Panel.LianZhiButton.interactable = Panel.ChaKanXiangQing.interactable = Panel.LingKuiLianZhiBtn.interactable = false;
			m_RecipeStatus.text = "正在读取丹方…";
			int request = ++m_RecipeRequest;
			SearchUnderstandElixirListReq_1005019 data = new SearchUnderstandElixirListReq_1005019
			{
				Stage = m_CurrentStage
			};
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.SEARCH_UNDERSTAND_LIST, data, byteString =>
			{
				if (request != m_RecipeRequest || GameObject == null) return;
				m_RecipeStatus.text = "未读到丹方，可重新选择境界。";
				if (!(byteString == null))
				{
					SearchUnderstandElixirListResp_1005019 searchUnderstandElixirListResp_ = new SearchUnderstandElixirListResp_1005019();
					searchUnderstandElixirListResp_.MergeFrom(byteString);
					if (searchUnderstandElixirListResp_.IsSuccess)
					{
						List<DanFangModel> list = new List<DanFangModel>();
						if (m_CurrentStage == 1)
						{
							DanFangModel item = new DanFangModel
							{
								ei = "LianXiDanFang",
								na = "练习用丹方",
								c = m_CurrentStage,
								r = 1,
								l = 1,
								v = 0
							};
							list.Add(item);
						}
						if (searchUnderstandElixirListResp_.UnderstandElixir != null)
						{
							foreach (SearchUnderstandElixirListResp_1005019.Types.UnderstandElixirResp_1005029 item3 in searchUnderstandElixirListResp_.UnderstandElixir)
							{
								DanFangModel item2 = new DanFangModel
								{
									ei = item3.Id,
									na = item3.Name,
									c = m_CurrentStage,
									r = item3.Rank,
									l = item3.Level,
									v = item3.FiveElement
								};
								list.Add(item2);
							}
						}
						m_RecipeStatus.text = list.Count == 0 ? "此境界暂无已参悟的丹方" : "";
						InitDanFang(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchUnderstandElixirListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
			if (!sent) m_RecipeStatus.text = "连接中断，请重连后选择境界。";
		}

		private void InitDanFang(List<DanFangModel> resout)
		{
			if (resout == null)
			{
				return;
			}
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			GridLayoutGroup component = Panel.ToggleGroup_4_ScrollContent.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.ToggleGroup_4_ScrollContent.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < resout.Count; i++)
			{
				if (resout[i].c == m_CurrentStage)
				{
					UI_GoodsTogItem component3 = SetListContentParent(Panel.ToggleGroup_4_ScrollContent, insObj).GetComponent<UI_GoodsTogItem>();
					Sprite value = null;
					Sprite value2 = null;
					Singleton<FacadeManager>.Instance.SelectGoodsRankBg()?.TryGetValue(WorldSetting.GetLevelByRank(resout[i].r), out value2);
					Singleton<FacadeManager>.Instance.SelectGoodsSprite()?.TryGetValue(2, out value);
					component3.SetValue(resout[i], resout[i].na, resout[i].v, resout[i].l, value2, value, component2, Group_4_Click);
				}
			}
			SetGrticalListContentHight(resout.Count, Panel.ToggleGroup_4_ScrollContent, component);
		}

		private void ClearGourp_4()
		{
			foreach (Transform item in Panel.ToggleGroup_4_ScrollContent)
			{
				item.gameObject.SetActive(false);
				Object.Destroy(item.gameObject);
			}
		}

		private void Group_1_Click(string name, int index, object data)
		{
			switch (name)
			{
			case "丹方":
				m_CurrentLianZhiType = LianZhiType.DanFang;
				break;
			case "器图":
				m_CurrentLianZhiType = LianZhiType.QiTu;
				break;
			case "阵盘":
				m_CurrentLianZhiType = LianZhiType.ZhenPan;
				break;
			}
			InitGroup_4();
			Panel.LianZhiButton.interactable = false;
			Panel.ChaKanXiangQing.interactable = false;
		}

		private void Group_2_Click(string name, int index, object data)
		{
			switch (name)
			{
			case "练气":
				m_CurrentStage = 1;
				break;
			case "筑基":
				m_CurrentStage = 2;
				break;
			case "结丹":
				m_CurrentStage = 3;
				break;
			case "元婴":
				m_CurrentStage = 4;
				break;
			case "分神":
				m_CurrentStage = 5;
				break;
			case "炼虚":
				m_CurrentStage = 6;
				break;
			case "合体":
				m_CurrentStage = 7;
				break;
			case "渡劫":
				m_CurrentStage = 8;
				break;
			case "大乘":
				m_CurrentStage = 9;
				break;
			}
			InitGroup_4();
			Panel.LianZhiButton.interactable = false;
			Panel.ChaKanXiangQing.interactable = false;
		}

		private void Group_4_Click(bool isOn, object data, GameObject gameObj)
		{
			if (data is DanFangModel danFangModel && isOn)
			{
				m_ChooseData = danFangModel;
				Panel.LianZhiButton.interactable = true;
				if (danFangModel.ei != "LianXiDanFang")
				{
					Panel.ChaKanXiangQing.interactable = true;
					Panel.LingKuiLianZhiBtn.interactable = true;
				}
				else
				{
					Panel.ChaKanXiangQing.interactable = false;
					Panel.LingKuiLianZhiBtn.interactable = false;
				}
			}
		}

		private void ChaKanXiangQing()
		{
			if (m_ChooseData != null)
			{
				DanFangModel data = m_ChooseData as DanFangModel;
				Singleton<RoleManager>.Instance.SelectTemplateElixirDetail(data.ei, data.l, data.r, data.v, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						TitleName = data.na,
						ContentList = list
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
		}

		private void LianZhiClick()
		{
			if (m_ChooseData != null)
			{
				DanFangModel data = m_ChooseData as DanFangModel;
				if (data.ei != "LianXiDanFang")
				{
					SearchElixirRefiningStepListReq_1005030 data2 = new SearchElixirRefiningStepListReq_1005030
					{
						MethodId = data.ei
					};
					Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_METHOD_REFINING_STEPLIST, data2, (object _obj) =>
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						ByteString byteString = _obj as ByteString;
						if (!(byteString == null))
						{
							SearchElixirRefiningStepListResp_1005030 searchElixirRefiningStepListResp_ = new SearchElixirRefiningStepListResp_1005030();
							searchElixirRefiningStepListResp_.MergeFrom(byteString);
							if (searchElixirRefiningStepListResp_.IsSuccess)
							{
								data.bb = new List<DanFangStepModel>();
								foreach (SearchElixirRefiningStepListResp_1005030.Types.ElixirRefiningStepResp_1005031 item2 in searchElixirRefiningStepListResp_.Step)
								{
									DanFangStepModel item = new DanFangStepModel
									{
										actionType = item2.ActionType,
										bestVal = item2.BestVal,
										continuetime = item2.ContinueTime,
										id = item2.StepId,
										maxVal = item2.MaxVal,
										minVal = item2.MinVal,
										order = item2.Order,
										targetType = item2.TargetType,
										valType = item2.ValType,
										targetName = item2.TargetName,
										mapId = item2.MapId,
										mapName = item2.MapName,
										targetId = item2.TargetId
									};
									data.bb.Add(item);
								}
								data.f = searchElixirRefiningStepListResp_.Rank;
								Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoShaiXuanWnd.prefab", true, data);
							}
							else
							{
								string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchElixirRefiningStepListResp_.Code);
								MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
							}
						}
					}, isShowWaitUI: true);
				}
				else
				{
					List<DanFangStepModel> bb = CreateDanFangStep();
					data.bb = bb;
					data.f = 1;
					Singleton<UIManager>.Instance.CloseWnd(this);
					Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianZhiWnd.prefab", true, data, new Dictionary<string, BaseNaJieGoodEntity>(), DiHuoGeEnterType.DongFu);
				}
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请至少选择一个丹方！");
			}
		}

		private List<DanFangStepModel> CreateDanFangStep()
		{
			List<DanFangStepModel> list = new List<DanFangStepModel>();
			DanFangStepModel item = new DanFangStepModel
			{
				actionType = 1,
				bestVal = 903.0,
				continuetime = 3000,
				id = "setp_1",
				maxVal = 1153.0,
				minVal = 653.0,
				order = 1,
				targetType = 0,
				valType = 3
			};
			DanFangStepModel item2 = new DanFangStepModel
			{
				actionType = 6,
				bestVal = 903.0,
				continuetime = 5000,
				id = "setp_2",
				maxVal = 1153.0,
				minVal = 653.0,
				order = 2,
				targetType = 0,
				valType = 3
			};
			DanFangStepModel item3 = new DanFangStepModel
			{
				actionType = 7,
				bestVal = 800.0,
				continuetime = 1500,
				id = "setp_3",
				maxVal = 1500.0,
				minVal = 0.0,
				order = 3,
				targetType = 0,
				valType = 1
			};
			DanFangStepModel item4 = new DanFangStepModel
			{
				actionType = 2,
				bestVal = 1128.0,
				continuetime = 2000,
				id = "setp_4",
				maxVal = 1878.0,
				minVal = 378.0,
				order = 4,
				targetId = "caiLiao",
				targetType = 12,
				valType = 1
			};
			DanFangStepModel item5 = new DanFangStepModel
			{
				actionType = 5,
				bestVal = 1500.0,
				continuetime = 3000,
				id = "setp_5",
				maxVal = 3000.0,
				minVal = 0.0,
				order = 5,
				valType = 1
			};
			list.Add(item);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			list.Add(item5);
			return list;
		}

		private void LingKuiLianZhi()
		{
			if (m_ChooseData != null)
			{
				DanFangModel data = m_ChooseData as DanFangModel;
				if (!(data.ei != "LianXiDanFang"))
				{
					return;
				}
				SearchElixirRefiningStepListReq_1005030 data2 = new SearchElixirRefiningStepListReq_1005030
				{
					MethodId = data.ei
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_METHOD_REFINING_STEPLIST, data2, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						SearchElixirRefiningStepListResp_1005030 searchElixirRefiningStepListResp_ = new SearchElixirRefiningStepListResp_1005030();
						searchElixirRefiningStepListResp_.MergeFrom(byteString);
						if (searchElixirRefiningStepListResp_.IsSuccess)
						{
							data.bb = new List<DanFangStepModel>();
							foreach (SearchElixirRefiningStepListResp_1005030.Types.ElixirRefiningStepResp_1005031 item2 in searchElixirRefiningStepListResp_.Step)
							{
								DanFangStepModel item = new DanFangStepModel
								{
									actionType = item2.ActionType,
									bestVal = item2.BestVal,
									continuetime = item2.ContinueTime,
									id = item2.StepId,
									maxVal = item2.MaxVal,
									minVal = item2.MinVal,
									order = item2.Order,
									targetType = item2.TargetType,
									valType = item2.ValType,
									targetName = item2.TargetName,
									mapId = item2.MapId,
									mapName = item2.MapName,
									targetId = item2.TargetId
								};
								data.bb.Add(item);
							}
							data.f = searchElixirRefiningStepListResp_.Rank;
							Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoShaiXuan_LK_Wnd.prefab", true, data);
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchElixirRefiningStepListResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请至少选择一个丹方！");
			}
		}

		private void LiKaiClick()
		{
			m_LegacyRecipes = false;
			Singleton<UIManager>.Instance.CloseWnd("DiHuoGeWnd.prefab");
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: true);
			Singleton<UIManager>.Instance.PopUpWnd("DongFuRoomWnd.prefab", true);
		}
	}
}
