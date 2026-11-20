using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CanWuWnd : Window
	{
		public CanWuPanel Panel;

		private GameObject CanWuObj;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<string, ShaiXuanParamsModel> m_ContentShaiXuanResult;

		private List<ShaiXuanItemType> UseShaiXuanType;

		private GameObject m_YiCanWuObj;

		private GameObject m_CanWuScrollItem;

		private List<CanWuEntity> m_GongFaDataSource;

		private List<CanWuEntity> m_FaShuDataSource;

		private List<CanWuEntity> m_DanFangDataSource;

		private List<CanWuEntity> m_FuShuDataSource;

		private List<CanWuEntity> m_QituDataSource;

		private List<CanWuEntity> m_ZhenTuDataSource;

		public CanWuTitleType m_CurrentType;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private Dictionary<CanWuTitleType, CanWuScrollItem> m_CanWuScrollItemDic;

		private int m_HaveLearnBooks;

		private int m_CanLearnBooks;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			ResourceLoad();
			m_CurrentType = CanWuTitleType.FaShu;
			Panel = base.GameObject.GetComponent<CanWuPanel>();
			UseShaiXuanType = new List<ShaiXuanItemType>();
			UseShaiXuanType.Add(ShaiXuanItemType.CanWuZhuangTai);
			UseShaiXuanType.Add(ShaiXuanItemType.WuXing);
			UseShaiXuanType.Add(ShaiXuanItemType.PinJie);
			UseShaiXuanType.Add(ShaiXuanItemType.DengJie);
			m_GongFaDataSource = new List<CanWuEntity>();
			m_FaShuDataSource = new List<CanWuEntity>();
			m_DanFangDataSource = new List<CanWuEntity>();
			m_FuShuDataSource = new List<CanWuEntity>();
			m_QituDataSource = new List<CanWuEntity>();
			m_ZhenTuDataSource = new List<CanWuEntity>();
			m_CanWuScrollItemDic = new Dictionary<CanWuTitleType, CanWuScrollItem>();
			AddButtonClickListener(Panel.ShaiXuanBtn, PopShaiXuan);
			AddButtonClickListener(Panel.TiShiLiKaiBtn, () =>
			{
				Panel.TiShiPanel.gameObject.SetActive(value: false);
			});
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, UseShaiXuanType, true) as Common_ShaiXuanWnd;
			m_CanWuScrollItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/CanWuScrollItem.prefab");
			if (m_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
				shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(RelfashData));
				Common_ShaiXuanWnd shaiXuanWnd2 = m_ShaiXuanWnd;
				shaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(shaiXuanWnd2.ReChooseAction, new UnityAction(RelfashData));
			}
		}

		private void PopShaiXuan()
		{
			m_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			EnterCanWu();
		}

		private void ShowGuide(GuideType _type, int _index)
		{
			MonoSingleton<GameStart>.Instance.StartCoroutine(delayShowGuide(_type, _index));
		}

		private IEnumerator delayShowGuide(GuideType _type, int _index)
		{
			yield return new WaitForSeconds(0.3f);
			if (Singleton<GuideManager>.Instance.IsCanShowGuide(_type, _index))
			{
				Singleton<GuideManager>.Instance.ShowGuide(_type, _index);
			}
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			CanWuObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_CanJuan.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuShu.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanShu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FaSuShu.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_QiTu.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenTu.png");
			m_GoodsIconSprite.Add(2, value);
			m_GoodsIconSprite.Add(3, value3);
			m_GoodsIconSprite.Add(6, value2);
			m_GoodsIconSprite.Add(1, value4);
			m_GoodsIconSprite.Add(4, value5);
			m_GoodsIconSprite.Add(5, value6);
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value7);
			m_RankBgSprite.Add(2, value8);
			m_RankBgSprite.Add(3, value9);
			m_RankBgSprite.Add(4, value10);
		}

		public void EnterCanWu()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CANWU_LIST, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					OpenUnderstandPanelResp_1001011 openUnderstandPanelResp_ = new OpenUnderstandPanelResp_1001011();
					openUnderstandPanelResp_.MergeFrom(byteString);
					if (openUnderstandPanelResp_.IsSuccess)
					{
						m_FaShuDataSource.Clear();
						m_GongFaDataSource.Clear();
						m_DanFangDataSource.Clear();
						m_FuShuDataSource.Clear();
						m_QituDataSource.Clear();
						m_CanWuScrollItemDic.Clear();
						m_ZhenTuDataSource.Clear();
						ClearData();
						m_HaveLearnBooks = openUnderstandPanelResp_.Object.Count;
						m_CanLearnBooks = Singleton<RoleManager>.Instance.Role_Entity.HunLiVal / 5 - m_HaveLearnBooks;
						foreach (OpenUnderstandPanelResp_1001011.Types.UnderstandMethodObjectResp_1001012 item2 in openUnderstandPanelResp_.Object)
						{
							CanWuEntity item = new CanWuEntity
							{
								Type = item2.Type,
								Name = item2.Name,
								FiveElement = item2.FiveElement,
								Id = item2.Id,
								IsCanWu = item2.IsUnderstand,
								Level = item2.Level,
								Rank = item2.Rank
							};
							switch ((CanWuTitleType)item2.Type)
							{
							case CanWuTitleType.FaShu:
								m_FaShuDataSource.Add(item);
								CreateScrollItem("法术", CanWuTitleType.FaShu);
								break;
							case CanWuTitleType.GongFa:
								m_GongFaDataSource.Add(item);
								CreateScrollItem("功法", CanWuTitleType.GongFa);
								break;
							case CanWuTitleType.DanFang:
								m_DanFangDataSource.Add(item);
								CreateScrollItem("丹方", CanWuTitleType.DanFang);
								break;
							case CanWuTitleType.QiTu:
								m_QituDataSource.Add(item);
								CreateScrollItem("器图", CanWuTitleType.QiTu);
								break;
							case CanWuTitleType.ZhenFa:
								m_ZhenTuDataSource.Add(item);
								CreateScrollItem("阵图", CanWuTitleType.ZhenFa);
								break;
							case CanWuTitleType.FuShu:
								m_FuShuDataSource.Add(item);
								CreateScrollItem("符书", CanWuTitleType.FuShu);
								break;
							}
						}
						Panel.SmallTitleText.text = $"已学习书籍{m_HaveLearnBooks}本，可学习书籍{m_CanLearnBooks}本";
						CreateData();
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateScrollItem(string titleName, CanWuTitleType canWuTitleType)
		{
			string helpContent = string.Empty;
			if (!m_CanWuScrollItemDic.ContainsKey(canWuTitleType))
			{
				if (canWuTitleType == CanWuTitleType.GongFa)
				{
					helpContent = "纳气、锻体、炼神升重后将提升基础周天效率，每一阶功法均可以升至第三重。每次升重可以使用同名功法残卷或道经残卷。";
				}
				CanWuScrollItem component = SetListContentParent(Panel.ScrollContent, m_CanWuScrollItem).gameObject.GetComponent<CanWuScrollItem>();
				component.SetValue(titleName, this, helpContent);
				m_CanWuScrollItemDic.Add(canWuTitleType, component);
			}
		}

		public void ClearData()
		{
			foreach (Transform item in Panel.ScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		public void KeCanWuItemsInIt(CanWuEntity canWuEntity, CanWuActionModel canWuActionModel, List<string> xiangQingData)
		{
			CanWuTitleType type = (CanWuTitleType)canWuEntity.Type;
			CanWuScrollItem value = null;
			if (!m_CanWuScrollItemDic.TryGetValue(type, out value))
			{
				return;
			}
			RectTransform component = value.gameObject.GetComponent<RectTransform>();
			UI_GoodsItem component2 = SetListContentParent(component, CanWuObj).GetComponent<UI_GoodsItem>();
			Sprite value2 = null;
			m_GoodsIconSprite.TryGetValue(canWuActionModel.type, out value2);
			int levelByRank = WorldSetting.GetLevelByRank(canWuEntity.Rank);
			Sprite value3 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value3);
			component2.SetItem(null, canWuEntity.Name, canWuEntity.FiveElement, canWuEntity.Level, value3, MethodBookIcon.Resolve(canWuEntity, value2), (object _obj, GameObject gameObj) =>
			{
				UnityAction<object> unityAction = (object obj) =>
				{
					CanWuAction(canWuActionModel.id, canWuActionModel.type);
				};
				switch ((CanWuTitleType)canWuActionModel.type)
				{
				case CanWuTitleType.FaShu:
					Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "参悟", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.GongFa:
					Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "参悟", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.DanFang:
					Singleton<RoleManager>.Instance.SelectTemplateElixirDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "参悟", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.QiTu:
					Singleton<RoleManager>.Instance.SelectTemplateQiTuDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "参悟", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.ZhenFa:
					Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "参悟", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.FuShu:
					Singleton<RoleManager>.Instance.SelectTemplateSignDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "参悟", canWuActionModel, unityAction, list);
					});
					break;
				}
			});
			value.AddItem(component2);
		}

		public void YiCanWuItemListInit(CanWuEntity canWuEntity, CanWuActionModel canWuActionModel, List<string> xiangQingData)
		{
			CanWuTitleType type = (CanWuTitleType)canWuEntity.Type;
			CanWuScrollItem value = null;
			if (!m_CanWuScrollItemDic.TryGetValue(type, out value))
			{
				return;
			}
			RectTransform component = value.gameObject.GetComponent<RectTransform>();
			UI_GoodsItem component2 = SetListContentParent(component, CanWuObj).GetComponent<UI_GoodsItem>();
			Sprite value2 = null;
			m_GoodsIconSprite.TryGetValue(canWuEntity.Type, out value2);
			int levelByRank = WorldSetting.GetLevelByRank(canWuEntity.Rank);
			Sprite value3 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value3);
			component2.SetItem(null, canWuEntity.Name, canWuEntity.FiveElement, canWuEntity.Level, value3, MethodBookIcon.Resolve(canWuEntity, value2), (object _obj, GameObject gameObj) =>
			{
				UnityAction<object> unityAction = (object obj) =>
				{
					YiWangYiCanWu(canWuActionModel.id, canWuEntity.Name, canWuActionModel.type);
				};
				switch ((CanWuTitleType)canWuActionModel.type)
				{
				case CanWuTitleType.FaShu:
					Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "法术", canWuActionModel, unityAction, list, canWuEntity);
					});
					break;
				case CanWuTitleType.GongFa:
					Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingGongFaPanel(canWuEntity.Name, "遗忘", canWuActionModel, unityAction, list, canWuEntity);
					});
					break;
				case CanWuTitleType.DanFang:
					Singleton<RoleManager>.Instance.SelectTemplateElixirDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "遗忘", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.QiTu:
					Singleton<RoleManager>.Instance.SelectTemplateQiTuDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "遗忘", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.ZhenFa:
					Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "遗忘", canWuActionModel, unityAction, list);
					});
					break;
				case CanWuTitleType.FuShu:
					Singleton<RoleManager>.Instance.SelectTemplateSignDetail(canWuActionModel.id, canWuEntity.Level, canWuEntity.Rank, canWuEntity.FiveElement, (List<string> list) =>
					{
						PopUpXiangQingPanel(canWuEntity.Name, "遗忘", canWuActionModel, unityAction, list);
					});
					break;
				}
			}, isUsing: true);
			value.AddItem(component2);
		}

		private void PopUpXiangQingPanel(string name, string btnName, CanWuActionModel canWuActionModel, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void PopUpXiangQingPanel(string name, string btnName, CanWuActionModel canWuActionModel, UnityAction<object> sureBtnCallBack, List<string> xiangQingData, CanWuEntity canWuEntity)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = canWuEntity,
				TitleName = name,
				SureBtnClick = sureBtnCallBack,
				ID = canWuEntity.Id,
				Rank = canWuEntity.Rank
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void PopUpXiangQingGongFaPanel(string name, string btnName, CanWuActionModel canWuActionModel, UnityAction<object> sureBtnCallBack, List<string> xiangQingData, CanWuEntity canWuEntity)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = canWuEntity,
				TitleName = name,
				SureBtnClick = sureBtnCallBack,
				ID = canWuEntity.Id,
				Rank = canWuEntity.Rank
			};
			Singleton<UIManager>.Instance.PopUpWnd("CanWuGongFaWnd.prefab", true, common_TipWndParamEntity, canWuEntity);
		}

		private void YiWangYiCanWu(string id, string name, int type)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确定遗忘" + name + "?", () =>
			{
				UnderstandMethodForgetReq_1001013 data = new UnderstandMethodForgetReq_1001013
				{
					Id = id,
					Type = type
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_YIWANG_CANWU, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						ActionBoolResp_1001001 actionBoolResp_ = new ActionBoolResp_1001001();
						actionBoolResp_.MergeFrom(byteString);
						if (actionBoolResp_.IsSuccess)
						{
							OnShow();
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}, isShowWaitUI: true);
			});
		}

		private void computeContentHightAndWith(int count)
		{
			GridLayoutGroup component = Panel.ScrollContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(count, Panel.ScrollContent, component);
		}

		private void RelfashData()
		{
			EnterCanWu();
		}

		private void CreateData()
		{
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			if (dictionary != null)
			{
				CreateGongFa(dictionary);
				CreateFaShu(dictionary);
				CreateDanFang(dictionary);
				CreateFuShu(dictionary);
				CreateQiTu(dictionary);
				CreateZhenTu(dictionary);
			}
			float num = 0f;
			foreach (CanWuScrollItem value in m_CanWuScrollItemDic.Values)
			{
				num += value.GetContentHight();
			}
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, num);
		}

		private int CreateGongFa(Dictionary<string, ShaiXuanParamsModel> result)
		{
			List<CanWuEntity> list = m_GongFaDataSource;
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel value = null;
			if (result.TryGetValue("状态", out value))
			{
				bool chooseState = !(value.ItemName == "可参悟");
				if (value.ItemTag != null)
				{
					list = list.Where((CanWuEntity x) => x.IsCanWu == chooseState).ToList();
				}
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			if (list.Count > 0)
			{
				GongFaBtnClick(list);
			}
			else if (m_CanWuScrollItemDic.ContainsKey(CanWuTitleType.GongFa))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[CanWuTitleType.GongFa];
				m_CanWuScrollItemDic.Remove(CanWuTitleType.GongFa);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
			return list.Count;
		}

		private int CreateFaShu(Dictionary<string, ShaiXuanParamsModel> result)
		{
			List<CanWuEntity> list = m_FaShuDataSource;
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel value = null;
			if (result.TryGetValue("状态", out value))
			{
				bool chooseState = !(value.ItemName == "可参悟");
				if (value.ItemTag != null)
				{
					list = list.Where((CanWuEntity x) => x.IsCanWu == chooseState).ToList();
				}
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			if (list.Count > 0)
			{
				FaShuBtnClick(list);
			}
			else if (m_CanWuScrollItemDic.ContainsKey(CanWuTitleType.FaShu))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[CanWuTitleType.FaShu];
				m_CanWuScrollItemDic.Remove(CanWuTitleType.FaShu);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
			return list.Count;
		}

		private int CreateDanFang(Dictionary<string, ShaiXuanParamsModel> result)
		{
			List<CanWuEntity> list = m_DanFangDataSource;
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel value = null;
			if (result.TryGetValue("状态", out value))
			{
				bool chooseState = !(value.ItemName == "可参悟");
				if (value.ItemTag != null)
				{
					list = list.Where((CanWuEntity x) => x.IsCanWu == chooseState).ToList();
				}
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			if (list.Count > 0)
			{
				DanFangBtnClick(list);
			}
			else if (m_CanWuScrollItemDic.ContainsKey(CanWuTitleType.DanFang))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[CanWuTitleType.DanFang];
				m_CanWuScrollItemDic.Remove(CanWuTitleType.DanFang);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
			return list.Count;
		}

		private int CreateFuShu(Dictionary<string, ShaiXuanParamsModel> result)
		{
			List<CanWuEntity> list = m_FuShuDataSource;
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel value = null;
			if (result.TryGetValue("状态", out value))
			{
				bool chooseState = !(value.ItemName == "可参悟");
				if (value.ItemTag != null)
				{
					list = list.Where((CanWuEntity x) => x.IsCanWu == chooseState).ToList();
				}
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			if (list.Count > 0)
			{
				FuShuBtnClick(list);
			}
			else if (m_CanWuScrollItemDic.ContainsKey(CanWuTitleType.FuShu))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[CanWuTitleType.FuShu];
				m_CanWuScrollItemDic.Remove(CanWuTitleType.FuShu);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
			return list.Count;
		}

		private int CreateQiTu(Dictionary<string, ShaiXuanParamsModel> result)
		{
			List<CanWuEntity> list = m_QituDataSource;
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel value = null;
			if (result.TryGetValue("状态", out value))
			{
				bool chooseState = !(value.ItemName == "可参悟");
				if (value.ItemTag != null)
				{
					list = list.Where((CanWuEntity x) => x.IsCanWu == chooseState).ToList();
				}
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			if (list.Count > 0)
			{
				QiTuBtnClick(list);
			}
			else if (m_CanWuScrollItemDic.ContainsKey(CanWuTitleType.QiTu))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[CanWuTitleType.QiTu];
				m_CanWuScrollItemDic.Remove(CanWuTitleType.QiTu);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
			return list.Count;
		}

		private int CreateZhenTu(Dictionary<string, ShaiXuanParamsModel> result)
		{
			List<CanWuEntity> list = m_ZhenTuDataSource;
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel value = null;
			if (result.TryGetValue("状态", out value))
			{
				bool chooseState = !(value.ItemName == "可参悟");
				if (value.ItemTag != null)
				{
					list = list.Where((CanWuEntity x) => x.IsCanWu == chooseState).ToList();
				}
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((CanWuEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			if (list.Count > 0)
			{
				ZhenTuBtnClick(list);
			}
			else if (m_CanWuScrollItemDic.ContainsKey(CanWuTitleType.ZhenFa))
			{
				CanWuScrollItem canWuScrollItem = m_CanWuScrollItemDic[CanWuTitleType.ZhenFa];
				m_CanWuScrollItemDic.Remove(CanWuTitleType.ZhenFa);
				UnityEngine.Object.Destroy(canWuScrollItem.gameObject);
			}
			return list.Count;
		}

		public void DanFangBtnClick(List<CanWuEntity> data)
		{
			m_CurrentType = CanWuTitleType.DanFang;
			int num = 0;
			int num2 = 0;
			if (data == null)
			{
				return;
			}
			foreach (CanWuEntity datum in data)
			{
				CanWuActionModel canWuActionModel = new CanWuActionModel
				{
					id = datum.Id,
					roleId = Singleton<RoleManager>.Instance.currentRoleId,
					type = 3
				};
				WorldSetting.GetLevelByRank(datum.Rank);
				if (datum.IsCanWu)
				{
					YiCanWuItemListInit(datum, canWuActionModel, null);
					num++;
				}
				else
				{
					KeCanWuItemsInIt(datum, canWuActionModel, null);
					num2++;
				}
			}
		}

		public void FaShuBtnClick(List<CanWuEntity> data)
		{
			m_CurrentType = CanWuTitleType.FaShu;
			int num = 0;
			int num2 = 0;
			if (data == null)
			{
				return;
			}
			foreach (CanWuEntity datum in data)
			{
				CanWuActionModel canWuActionModel = new CanWuActionModel
				{
					id = datum.Id,
					roleId = Singleton<RoleManager>.Instance.currentRoleId,
					type = Convert.ToInt16(CanWuTitleType.FaShu)
				};
				WorldSetting.GetLevelByRank(datum.Rank);
				if (datum.IsCanWu)
				{
					YiCanWuItemListInit(datum, canWuActionModel, null);
					num++;
				}
				else
				{
					KeCanWuItemsInIt(datum, canWuActionModel, null);
					num2++;
				}
			}
		}

		public void GongFaBtnClick(List<CanWuEntity> dataSource)
		{
			m_CurrentType = CanWuTitleType.GongFa;
			int num = 0;
			int num2 = 0;
			if (dataSource == null)
			{
				return;
			}
			foreach (CanWuEntity item in dataSource)
			{
				CanWuActionModel canWuActionModel = new CanWuActionModel
				{
					id = item.Id,
					roleId = Singleton<RoleManager>.Instance.currentRoleId,
					type = Convert.ToInt16(CanWuTitleType.GongFa)
				};
				WorldSetting.GetLevelByRank(item.Rank);
				if (item.IsCanWu)
				{
					YiCanWuItemListInit(item, canWuActionModel, null);
					num++;
				}
				else
				{
					KeCanWuItemsInIt(item, canWuActionModel, null);
					num2++;
				}
			}
		}

		public void FuShuBtnClick(List<CanWuEntity> dataSource)
		{
			m_CurrentType = CanWuTitleType.FuShu;
			int num = 0;
			int num2 = 0;
			if (dataSource == null)
			{
				return;
			}
			foreach (CanWuEntity item in dataSource)
			{
				CanWuActionModel canWuActionModel = new CanWuActionModel
				{
					id = item.Id,
					roleId = Singleton<RoleManager>.Instance.currentRoleId,
					type = Convert.ToInt16(CanWuTitleType.FuShu)
				};
				WorldSetting.GetLevelByRank(item.Rank);
				if (item.IsCanWu)
				{
					YiCanWuItemListInit(item, canWuActionModel, null);
					num++;
				}
				else
				{
					KeCanWuItemsInIt(item, canWuActionModel, null);
					num2++;
				}
			}
		}

		public void QiTuBtnClick(List<CanWuEntity> dataSource)
		{
			m_CurrentType = CanWuTitleType.QiTu;
			int num = 0;
			int num2 = 0;
			if (dataSource == null)
			{
				return;
			}
			foreach (CanWuEntity item in dataSource)
			{
				CanWuActionModel canWuActionModel = new CanWuActionModel
				{
					id = item.Id,
					roleId = Singleton<RoleManager>.Instance.currentRoleId,
					type = Convert.ToInt16(CanWuTitleType.QiTu)
				};
				WorldSetting.GetLevelByRank(item.Rank);
				if (item.IsCanWu)
				{
					YiCanWuItemListInit(item, canWuActionModel, null);
					num++;
				}
				else
				{
					KeCanWuItemsInIt(item, canWuActionModel, null);
					num2++;
				}
			}
		}

		public void ZhenTuBtnClick(List<CanWuEntity> dataSource)
		{
			m_CurrentType = CanWuTitleType.ZhenFa;
			int num = 0;
			int num2 = 0;
			if (dataSource == null)
			{
				return;
			}
			foreach (CanWuEntity item in dataSource)
			{
				CanWuActionModel canWuActionModel = new CanWuActionModel
				{
					id = item.Id,
					roleId = Singleton<RoleManager>.Instance.currentRoleId,
					type = Convert.ToInt16(CanWuTitleType.ZhenFa)
				};
				WorldSetting.GetLevelByRank(item.Rank);
				if (item.IsCanWu)
				{
					YiCanWuItemListInit(item, canWuActionModel, null);
					num++;
				}
				else
				{
					KeCanWuItemsInIt(item, canWuActionModel, null);
					num2++;
				}
			}
		}

		public override void OnClose()
		{
			base.OnClose();
			Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
			shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(RelfashData));
			Singleton<UIManager>.Instance.CloseWnd("Common_ShaiXuanWnd.prefab");
		}

		private void CanWuAction(string id, int type)
		{
			UnderstandMethodReq_1001005 data = new UnderstandMethodReq_1001005
			{
				Id = id,
				Type = type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_UNDERSTAND, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ActionBoolResp_1001001 actionBoolResp_ = new ActionBoolResp_1001001();
					actionBoolResp_.MergeFrom(byteString);
					if (actionBoolResp_.IsSuccess)
					{
						OnShow();
						TiShi(type, actionBoolResp_.Code);
						if (Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 9;
							Singleton<GuideManager>.Instance.ShowGuideNew(1);
							ShangChuan(1);
						}
					}
					else
					{
						if (Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 7;
							Singleton<GuideManager>.Instance.ShowGuideNew(1);
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShangChuan(int Guide)
		{
			GuideInfoUpdateReq_1002013 guideInfoUpdateReq_ = new GuideInfoUpdateReq_1002013();
			guideInfoUpdateReq_.Type = Guide;
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_GUIDE, guideInfoUpdateReq_, (object _obj) =>
			{
			});
			Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal++;
		}

		private void TiShi(int type, int code)
		{
			Panel.TiShiPanel.gameObject.SetActive(value: true);
			string text = "去设置";
			UnityAction call = null;
			switch ((CanWuTitleType)type)
			{
			case CanWuTitleType.ZhenFa:
				text = "去刻制";
				call = ToKeZhi;
				break;
			case CanWuTitleType.FaShu:
				text = "去设置";
				call = ToSettingFaShu;
				break;
			case CanWuTitleType.DanFang:
				text = "去炼丹";
				call = ToLianDan;
				break;
			case CanWuTitleType.QiTu:
				text = "去炼器";
				call = ToLianQi;
				break;
			case CanWuTitleType.FuShu:
				text = "去画符";
				call = ToHuaFu;
				break;
			case CanWuTitleType.GongFa:
				text = "去设置";
				call = GoToSettingGongFa;
				break;
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code);
			Panel.TiShiGoSettingBtnText.text = text;
			Panel.TiShiContentText.text = hintMsgByCode;
			Panel.TiShiGoSettingBtn.onClick.RemoveAllListeners();
			Panel.TiShiGoSettingBtn.onClick.AddListener(call);
		}

		private void ToHuaFu()
		{
			if (!Singleton<RoleManager>.Instance.IsFuShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟符室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd?.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(3)?.Invoke();
		}

		private void ToLianQi()
		{
			if (!Singleton<RoleManager>.Instance.IsLianQiShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟炼器室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd?.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(2)?.Invoke();
		}

		private void ToSettingFaShu()
		{
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab").ZhanDouBtnClick();
			Singleton<UIManager>.Instance.FindWndByName<RoleZhuangBeiWnd>("RoleZhuangBeiWnd.prefab").Panel.JiNengTog.isOn = true;
		}

		private void GoToSettingGongFa()
		{
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				if (Singleton<GameMapManager>.Instance.CurrentMapName.Equals("DongFuScene"))
				{
					mainTitleWnd.DongFuBtnClickNoChangeScene();
				}
				else
				{
					mainTitleWnd.DongFuBtnClick();
				}
				Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab")?.LianGongFangBtn();
			}
			else
			{
				dongFuRoomWnd?.LianGongFangBtn();
			}
		}

		private void ToLianDan()
		{
			if (!Singleton<RoleManager>.Instance.IsDanShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟炼丹室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(1)?.Invoke();
		}

		private void ToKeZhi()
		{
			if (!Singleton<RoleManager>.Instance.IsZhenFaShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟阵法室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(5)?.Invoke();
		}

		public void CanWuBtnClick()
		{
			Singleton<UIManager>.Instance.HideWnd("CanWuWnd.prefab");
			Singleton<UIManager>.Instance.HideWnd("DongFuWnd.prefab");
			Singleton<UIManager>.Instance.PopUpWnd("CanWuZhongWnd.prefab", true);
		}
	}
}
