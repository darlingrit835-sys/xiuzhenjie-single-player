using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class HuaFuChooseWnd : Window
	{
		public HuaFuChoosePanel Panel;

		private bool legacy;

		private int m_CurrentChoosedStage;

		private Text m_FuLuStatus;

		private int m_FuLuRequest;

		private GameObject m_FuLuItemObj;

		private GameObject m_FuLuTogGoodItemObj;

		private FuLuChooseItemEntity m_ChooseData;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<HuaFuChoosePanel>();
			InkCraftHalls.StyleSelection(Panel.transform, "ContentBg ", "MountainHall", Panel.KaiShiHuiZhiBtn, Panel.ChaKanXiangQingBtn, Panel.LingKuiLianZhiBtn);
			m_FuLuStatus = InkCraftHalls.AddListStatus(Panel.FuLu_ScrollViewContent, Panel.KaiShiHuiZhiBtn.GetComponentInChildren<Text>().font);
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.KaiShiHuiZhiBtn, KaiShiHuiZhiBtnClick);
			AddButtonClickListener(Panel.ChaKanXiangQingBtn, ChaKanXiangQing);
			AddButtonClickListener(Panel.LingKuiLianZhiBtn, LingKuiLianZhiBtn);
			m_CurrentChoosedStage = 1;
			m_FuLuItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_TogItem.prefab");
			m_FuLuTogGoodItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (legacy) { OpenLegacy(); return; }
			TalismanStudio.Open(Panel.transform, Panel.KaiShiHuiZhiBtn.GetComponentInChildren<Text>().font,
				LiKaiBtnClick, OpenLegacy, "旧符书");
		}

		public override void OnDisable()
		{
			base.OnDisable();
			TalismanStudio.CloseIn(Panel.transform);
		}

		private void OpenLegacy()
		{
			legacy = true;
			InitStage();
		}

		private void InitStage()
		{
			GridLayoutGroup component = Panel.Level_Content.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.Level_Content.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 9; i++)
			{
				SetListContentParent(Panel.Level_Content, m_FuLuItemObj).GetComponent<Common_TogItem>().SetValue(null, WorldSetting.GetStageName(i + 1), i + 1, component2, ChooseStageItem, i == 0);
			}
			SetGrticalListContentHight(9, Panel.Level_Content, component);
		}

		private void GetFuLuData()
		{
			m_ChooseData = null;
			Panel.LingKuiLianZhiBtn.interactable = false;
			foreach (Transform previous in Panel.FuLu_ScrollViewContent)
				previous.gameObject.SetActive(false);
			m_FuLuStatus.text = "正在读取符书…";
			int request = ++m_FuLuRequest;
			SearchUnderstandSignListReq_1005032 data = new SearchUnderstandSignListReq_1005032
			{
				Stage = m_CurrentChoosedStage
			};
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.SEARCH_UNDERSTAND_SIGNLIST, data, byteString =>
			{
				if (request != m_FuLuRequest || GameObject == null || !GameObject.activeInHierarchy) return;
				m_FuLuStatus.text = "未读到符书，可重新选择境界。";
				if (!(byteString == null))
				{
					SearchUnderstandSignListResp_1005032 searchUnderstandSignListResp_ = new SearchUnderstandSignListResp_1005032();
					searchUnderstandSignListResp_.MergeFrom(byteString);
					if (searchUnderstandSignListResp_.IsSuccess)
					{
						List<FuLuChooseItemEntity> list = new List<FuLuChooseItemEntity>();
						if (m_CurrentChoosedStage <= 1)
						{
							list.Add(new FuLuChooseItemEntity
							{
								Id = "1",
								Name = "练习用符书",
								Rank = 1,
								FiveElement = 0,
								Level = 1
							});
						}
						foreach (SearchUnderstandSignListResp_1005032.Types.UnderstandSignResp_1005033 item in searchUnderstandSignListResp_.UnderstandSign)
						{
							list.Add(new FuLuChooseItemEntity
							{
								Id = item.Id,
								Name = item.Name,
								Rank = item.Rank,
								FiveElement = item.FiveElement,
								Level = item.Level
							});
						}
						m_FuLuStatus.text = list.Count == 0 ? "此境界暂无已参悟的符书" : "";
						CreateFuLuContent(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchUnderstandSignListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
			if (!sent) m_FuLuStatus.text = "连接中断，请重连后选择境界。";
		}

		private void CreateFuLuContent(List<FuLuChooseItemEntity> data)
		{
			foreach (Transform item in Panel.FuLu_ScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			GridLayoutGroup component = Panel.FuLu_ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.FuLu_ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			int num = 0;
			foreach (FuLuChooseItemEntity datum in data)
			{
				UI_GoodsTogItem component3 = SetListContentParent(Panel.FuLu_ScrollViewContent, m_FuLuTogGoodItemObj).GetComponent<UI_GoodsTogItem>();
				Sprite value = null;
				Sprite value2 = null;
				Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(5, out value2);
				Singleton<FacadeManager>.Instance.SelectGoodsRankBg().TryGetValue(WorldSetting.GetLevelByRank(datum.Rank), out value);
				component3.SetValue(datum, datum.Name, datum.FiveElement, datum.Level, value, value2, component2, ChooseFuLuItem);
				num++;
			}
			SetGrticalListContentHight(data.Count, Panel.FuLu_ScrollViewContent, component);
		}

		private void ChooseFuLuItem(bool isOn, object dataSource, GameObject gameObj)
		{
			if (!isOn)
			{
				return;
			}
			m_ChooseData = dataSource as FuLuChooseItemEntity;
			if (m_ChooseData != null)
			{
				Panel.KaiShiHuiZhiBtn.interactable = true;
				Panel.LingKuiLianZhiBtn.interactable = true;
				if (m_ChooseData.Name == "练习用符书")
				{
					HuaFu();
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

		private void ChooseStageItem(string name, int index, object dataSource)
		{
			m_CurrentChoosedStage = index;
			Panel.ChaKanXiangQingBtn.interactable = false;
			Panel.KaiShiHuiZhiBtn.interactable = false;
			GetFuLuData();
		}

		private void KaiShiHuiZhiBtnClick()
		{
			if (m_ChooseData == null)
			{
				return;
			}
			if (m_ChooseData.Name == "练习用符书")
			{
				GetFuLuInfo();
				return;
			}
			VerifyIsCanRefiningSignReq_1005037 data = new VerifyIsCanRefiningSignReq_1005037
			{
				MethodSignId = m_ChooseData.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFY_ISCAN_REFINING_SIGN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyIsCanRefiningSignResp_1005037 verifyIsCanRefiningSignResp_ = new VerifyIsCanRefiningSignResp_1005037();
					verifyIsCanRefiningSignResp_.MergeFrom(byteString);
					if (verifyIsCanRefiningSignResp_.IsSuccess)
					{
						GetFuLuInfo();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyIsCanRefiningSignResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetFuLuInfo()
		{
			SearchSignRefiningStepListReq_1005034 data = new SearchSignRefiningStepListReq_1005034
			{
				MethodId = m_ChooseData.Id
			};
			if (m_ChooseData.Name == "练习用符书")
			{
				FuLuEntity fuLuEntity = new FuLuEntity
				{
					Id = "1",
					Name = "练习用符书",
					Stage = 2,
					Rank = 1
				};
				fuLuEntity.Points = new List<FuShuPoint>();
				fuLuEntity.Points.Add(new FuShuPoint
				{
					x = -90.0,
					y = 90.0
				});
				fuLuEntity.Points.Add(new FuShuPoint
				{
					x = -90.0,
					y = -90.0
				});
				fuLuEntity.Points.Add(new FuShuPoint
				{
					x = 90.0,
					y = -90.0
				});
				fuLuEntity.Points.Add(new FuShuPoint
				{
					x = 90.0,
					y = 90.0
				});
				fuLuEntity.Points.Add(new FuShuPoint
				{
					x = 90.0,
					y = 180.0
				});
				Singleton<UIManager>.Instance.PopUpWnd("HuaFuWnd.prefab", true, fuLuEntity);
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_SIGN_REFINING_STEPLIST, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchSignRefiningStepListResp_1005034 searchSignRefiningStepListResp_ = new SearchSignRefiningStepListResp_1005034();
					searchSignRefiningStepListResp_.MergeFrom(byteString);
					if (searchSignRefiningStepListResp_.IsSuccess)
					{
						FuLuEntity fuLuEntity2 = new FuLuEntity
						{
							Id = m_ChooseData.Id,
							Name = m_ChooseData.Name,
							Stage = m_CurrentChoosedStage,
							Rank = ((searchSignRefiningStepListResp_.Rank == 0) ? 1 : searchSignRefiningStepListResp_.Rank)
						};
						fuLuEntity2.Points = new List<FuShuPoint>();
						foreach (SearchSignRefiningStepListResp_1005034.Types.PointsResp_1005035 item in searchSignRefiningStepListResp_.Point)
						{
							fuLuEntity2.Points.Add(new FuShuPoint
							{
								x = item.X,
								y = item.Y
							});
						}
						Singleton<UIManager>.Instance.PopUpWnd("HuaFuWnd.prefab", true, fuLuEntity2);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchSignRefiningStepListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChaKanXiangQing()
		{
			if (m_ChooseData != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateSignDetail(m_ChooseData.Id, m_ChooseData.Level, m_ChooseData.Rank, m_ChooseData.FiveElement, (List<string> list) =>
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
			Singleton<UIManager>.Instance.PopUpWnd("HuaFuChoose_LK_Wnd.prefab", true, m_ChooseData);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void HuaFu()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal != 100)
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 7;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
				Singleton<GuideManager>.Instance.ShowGuideNew(7);
			}
		}
	}
}
