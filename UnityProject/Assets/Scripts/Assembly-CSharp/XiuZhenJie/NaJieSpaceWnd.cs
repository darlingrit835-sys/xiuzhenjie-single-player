using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NaJieSpaceWnd : Window
	{
		public NaJieSpacePanel Panel;

		private XzjBoxEntity m_DataSource;

		private Common_ShaiXuanWnd m_Common_ShaiXuanWnd;

		private List<BaseNaJieGoodEntity> m_BaseNaJieGoods;

		private List<BaseNaJieGoodEntity> m_CurrentNaJieGoods;

		private GameObject m_NaJieObj;

		private Dictionary<string, GameObject> m_GoodsDic;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private ScrollViewInfiniteContent m_ScrollViewInfiniteContent;

		private const int EnterMiJingCost_Stage_1 = 800;

		private const int EnterMiJingCost_Stage_2 = 4000;

		private const int EnterMiJingCost_Stage_3 = 20000;

		private const int EnterMiJingCost_Stage_4 = 100000;

		private const int EnterMiJingCost_Stage_5 = 500000;

		private const int EnterMiJingCost_Stage_6 = 1000000;

		private const int EnterMiJingCost_Stage_7 = 5000000;

		private const int EnterMiJingCost_Stage_8 = 10000000;

		private const int EnterMiJingCost_Stage_9 = 30000000;

		private int m_MinRow = 1;

		private int m_MaxRow = 9;

		private int m_Between = 8;

		private int m_RowCount;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<NaJieSpacePanel>();
			m_GoodsDic = new Dictionary<string, GameObject>();
			m_CurrentNaJieGoods = new List<BaseNaJieGoodEntity>();
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.ShaiXuanBtn, ShaiXuanWnd);
			AddButtonClickListener(Panel.YiJianChuShouBtn, OneKeySole);
			AddButtonClickListener(Panel.ShengJiBtn, ShengJiBeiBao);
			m_ScrollViewInfiniteContent = Panel.NaJieListConstant.gameObject.GetComponent<ScrollViewInfiniteContent>();
			ResourceLoad();
			if (m_NaJieObj == null)
			{
				m_NaJieObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			}
			List<ShaiXuanItemType> list = new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.AllGoodsType,
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.PinJie,
				ShaiXuanItemType.DengJie
			};
			m_Common_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, list, true) as Common_ShaiXuanWnd;
			if (m_Common_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd common_ShaiXuanWnd = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(common_ShaiXuanWnd.ReChooseAction, new UnityAction(FlashData));
				Common_ShaiXuanWnd common_ShaiXuanWnd2 = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(common_ShaiXuanWnd2.ReChooseAction, new UnityAction(FlashData));
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_DataSource = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_DataSource.GetAllGoods(InitNaJie);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void InitNaJie()
		{
			FlashData();
		}

		public void FlashData()
		{
			if (!(Panel == null) && !(Panel.NaJieListConstant == null) && m_CurrentNaJieGoods != null)
			{
				RongLiangReflash();
				m_ScrollViewInfiniteContent?.SetMinAndMax(m_MinRow, m_MaxRow);
				Dictionary<string, ShaiXuanParamsModel> result = m_Common_ShaiXuanWnd?.GetResult();
				m_CurrentNaJieGoods.Clear();
				m_CurrentNaJieGoods = ShaiXuanGoods(result, m_DataSource.GoodsList);
				ClearNaJie();
				CreatNaJieGoods(m_CurrentNaJieGoods);
			}
		}

		private void ChangeScrollContent(int min, int max)
		{
		}

		public void CreatNaJieGoods(List<BaseNaJieGoodEntity> naJieData)
		{
			if (!(Panel == null) && !(Panel.NaJieListConstant == null))
			{
				GridLayoutGroup component = Panel.NaJieListConstant.GetComponent<GridLayoutGroup>();
				GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
				_ = gameObject.GetComponent<RectTransform>().sizeDelta;
				_ = gameObject.GetComponent<RectTransform>().sizeDelta;
				component.enabled = true;
				SetNaJieKongJianList(component, gameObject, naJieData);
				SetGrticalListContentHight(naJieData.Count, Panel.NaJieListConstant, component);
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 1 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 4)
				{
					Singleton<GuideManager>.Instance.ShowGuideNew(1);
				}
			}
		}

		private List<BaseNaJieGoodEntity> ShaiXuanGoods(Dictionary<string, ShaiXuanParamsModel> result, List<BaseNaJieGoodEntity> data)
		{
			List<BaseNaJieGoodEntity> list = null;
			list = ((data != null) ? new List<BaseNaJieGoodEntity>(data) : new List<BaseNaJieGoodEntity>());
			ShaiXuanParamsModel leiBie = null;
			if (result.TryGetValue("类别", out leiBie))
			{
				if (leiBie.ItemTag != null)
				{
					list = list.Where((BaseNaJieGoodEntity x) => x.Type == int.Parse(leiBie.ItemTag)).ToList();
				}
				else if (leiBie.ItemName == "法器")
				{
					list = list.Where((BaseNaJieGoodEntity x) => x.Type == 14 || x.Type == 15 || x.Type == 16 || x.Type == 26).ToList();
				}
				else if (leiBie.ItemName == "动物")
				{
					list = list.Where((BaseNaJieGoodEntity x) => x.Type == 7 || x.Type == 8 || x.Type == 9 || x.Type == 10 || x.Type == 11).ToList();
				}
			}
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			return list;
		}

		private void SetNaJieKongJianList(GridLayoutGroup Glgroup, GameObject obj, List<BaseNaJieGoodEntity> dataSource)
		{
			m_RowCount = GetOneRowNum(Panel.NaJieListConstant, Glgroup);
			int num = m_RowCount * (m_MaxRow - m_MinRow);
			if (dataSource.Count <= num)
			{
				_ = dataSource.Count;
			}
			for (int i = 0; i < dataSource.Count; i++)
			{
				CreateObjItem(obj, dataSource[i]);
			}
		}

		private void CreateObjItem(GameObject obj, BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(Panel.NaJieListConstant, obj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(_data.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(_data.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, value2, value, (object _obj, GameObject _gameObj) =>
			{
				GoodsClick(_obj, _gameObj, UseGoods, SoleOneWuPin);
			}, _data.IsUsing, _data.GoodsNum);
			if (!m_GoodsDic.ContainsKey(_data.GoodsId))
			{
				m_GoodsDic.Add(_data.GoodsId, gameObject);
			}
		}

		private void GoodsClick(object dataSource, GameObject gameObj, UnityAction<int, object, GameObject> DyBtnClick, UnityAction<int, object> XiaoHuiBtnClick)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			NaJieTipModel naJieTipModel = new NaJieTipModel();
			string wuPinIsCanUse = WorldSetting.GetWuPinIsCanUse(baseNaJieGoodEntity.Type);
			if (wuPinIsCanUse == "法器")
			{
				naJieTipModel.MiddleBtnText = "穿戴";
				DyBtnClick = (int _type, object _obj, GameObject _gameObj) =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请于装备中进行设置");
				};
			}
			else if (wuPinIsCanUse == "合成")
			{
				if (baseNaJieGoodEntity.Level < 81)
				{
					naJieTipModel.MiddleBtnText = "合成";
					DyBtnClick = (int _type, object _obj, GameObject _gameObj) =>
					{
						Singleton<UIManager>.Instance.PopUpWnd("HeChengWnd.prefab", true, baseNaJieGoodEntity);
					};
				}
				else
				{
					DyBtnClick = (int _type, object _obj, GameObject _gameObj) =>
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("已达到最大等级，无法向上合成！");
					};
				}
			}
			else
			{
				naJieTipModel.MiddleBtnText = wuPinIsCanUse;
			}
			if ((wuPinIsCanUse == "法器" || wuPinIsCanUse == "穿戴") && baseNaJieGoodEntity != null && !baseNaJieGoodEntity.IsBinding)
			{
				naJieTipModel.MiddleBtnText = "滴血认主";
				DyBtnClick = (int _type, object obj, GameObject _gameObj) =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("滴血认主后,将无法进行转售,是否确认滴血认主？", () =>
					{
						GoodsBindingReq_1004023 data = new GoodsBindingReq_1004023
						{
							GoodsId = baseNaJieGoodEntity.GoodsId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.BINDING_GOODS, data, (object _obj) =>
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							ByteString byteString = _obj as ByteString;
							if (!(byteString == null))
							{
								GoodsBindingResp_1004023 goodsBindingResp_ = new GoodsBindingResp_1004023();
								goodsBindingResp_.MergeFrom(byteString);
								if (goodsBindingResp_ != null && goodsBindingResp_.IsSuccess)
								{
									Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab").FlashData();
									baseNaJieGoodEntity.IsBinding = true;
								}
								MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBindingResp_.Code));
							}
						}, isShowWaitUI: true);
					});
				};
			}
			naJieTipModel.JiaGe = baseNaJieGoodEntity.RecyclePrice;
			naJieTipModel.LeftBtnText = "贩卖";
			naJieTipModel.LeftBtnClickEvent = XiaoHuiBtnClick;
			naJieTipModel.MiddleBtnClickEvent = (int type, object dataObj) =>
			{
				if (DyBtnClick != null)
				{
					DyBtnClick(type, dataObj, gameObj);
				}
			};
			naJieTipModel.RightBtnClickEvent = LockOrUnLockGoods;
			naJieTipModel.Name = baseNaJieGoodEntity.Name;
			naJieTipModel.ContentList = baseNaJieGoodEntity.ContentList;
			naJieTipModel.type = baseNaJieGoodEntity.Type;
			naJieTipModel.dataSource = baseNaJieGoodEntity;
			naJieTipModel.IsLock = baseNaJieGoodEntity.IsLock;
			Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, naJieTipModel);
			if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1 && baseNaJieGoodEntity.Name == "启灵经" && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 5)
			{
				Singleton<GuideManager>.Instance.ShowGuideNew(1);
			}
		}

		private void LockOrUnLockGoods(int index, object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity.IsLock)
			{
				Singleton<RoleManager>.Instance.RoleBoxDic[10].UnLockOneGoods(baseNaJieGoodEntity.GoodsId, () =>
				{
				});
			}
			else
			{
				Singleton<RoleManager>.Instance.RoleBoxDic[10].LockOneGoods(baseNaJieGoodEntity.GoodsId, () =>
				{
				});
			}
		}

		private void ClearNaJie()
		{
			if (Panel == null || Panel.NaJieListConstant == null)
			{
				return;
			}
			foreach (RectTransform item in Panel.NaJieListConstant)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			m_GoodsDic?.Clear();
		}

		public void CloseWnd()
		{
			ClearNaJie();
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void ShaiXuanWnd()
		{
			m_Common_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void OneKeySole()
		{
			Singleton<UIManager>.Instance.PopUpWnd("NaJieOneKeySoleWnd.prefab", true, m_BaseNaJieGoods);
		}

		private void SoleOneWuPin(int type, object dataSource)
		{
			BaseNaJieGoodEntity _data = dataSource as BaseNaJieGoodEntity;
			if (_data.IsUsing)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("该物品在战斗设置中已设置，无法贩卖");
			}
			else if (_data.Rank > 8)
			{
				string rankAllName = WorldSetting.GetRankAllName(_data.Rank);
				MonoSingleton<GameStart>.Instance.PopUpAttention($"该物品品阶为{rankAllName},是否确认出售", () =>
				{
					if (_data.GoodsNum > 1)
					{
						Common_ChooseNumEntity common_ChooseNumEntity2 = new Common_ChooseNumEntity
						{
							Goods = _data,
							QueRenCallBack = (int num) =>
							{
								SoleGoods(type, _data, num);
							},
							TitleContnet = "出售"
						};
						Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity2);
					}
					else
					{
						SoleGoods(type, _data, 1);
					}
				});
			}
			else if (_data.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = _data,
					QueRenCallBack = (int num) =>
					{
						SoleGoods(type, _data, num);
					},
					TitleContnet = "出售"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				SoleGoods(type, _data, 1);
			}
		}

		private void SoleGoods(int type, BaseNaJieGoodEntity _data, int num)
		{
			GoodsSellReq_1004022 goodsSellReq_ = new GoodsSellReq_1004022();
			goodsSellReq_.GoodsInfos.Add(new GoodsSellReq_1004022.Types.GoodsInfoReq_1004010
			{
				Count = num,
				GoodsId = _data.GoodsId
			});
			Singleton<NetManager>.Instance.SendMsg(MsgType.SELL_GOODS, goodsSellReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsSellResp_1004022 goodsSellResp_ = new GoodsSellResp_1004022();
					goodsSellResp_.MergeFrom(byteString);
					if (goodsSellResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = goodsSellResp_.ShinyStone;
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = goodsSellResp_.NowCapacity;
						if (goodsSellResp_.FailedCount <= 0)
						{
							RemoveWuPin(_data.GoodsId, num);
						}
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsSellResp_.Code), goodsSellResp_.SuccessCount, goodsSellResp_.FailedCount, goodsSellResp_.AddShinyStone);
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
						RongLiangReflash();
					}
					else
					{
						string textValue2 = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsSellResp_.Code));
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue2);
					}
				}
			}, isShowWaitUI: true);
		}

		public void RemoveWuPin(string id, int num)
		{
			BaseNaJieGoodEntity goodsInfo = m_DataSource.GetGoodsInfo(id);
			if (goodsInfo.GoodsNum > num)
			{
				goodsInfo.GoodsNum -= num;
				if (m_GoodsDic.ContainsKey(id))
				{
					m_GoodsDic[id].GetComponent<UI_GoodsItem>().ResetNum(num);
				}
				return;
			}
			m_DataSource.RemoveGoods(id);
			if (m_GoodsDic.ContainsKey(id))
			{
				GameObject obj = m_GoodsDic[id];
				m_GoodsDic.Remove(id);
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				FlashData();
			}
			ReBuildContentHight();
		}

		public void ReBuildContentHight()
		{
			GridLayoutGroup component = Panel.NaJieListConstant.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(Panel.NaJieListConstant.childCount, Panel.NaJieListConstant, component);
		}

		private void UseGoods(int type, object dataSource, GameObject gameObject)
		{
			BaseNaJieGoodEntity data = dataSource as BaseNaJieGoodEntity;
			switch ((GoodsTypeEnum)type)
			{
			case GoodsTypeEnum.BOOK_ACHIEVEMENT:
				UseGongFa(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_SKILL:
				UseJiNeng(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_EQUIPMENT:
				UseQiTu(data, gameObject);
				break;
			case GoodsTypeEnum.EQUIPMENT:
			case GoodsTypeEnum.EQUIPMENT_ATTACK:
			case GoodsTypeEnum.EQUIPMENT_DEFENSE:
			case GoodsTypeEnum.MATRIX:
			case GoodsTypeEnum.EQUIPMENT_DEFENSE_APPAREL:
				UseZhuangBei(data);
				break;
			case GoodsTypeEnum.ELIXIR:
				UseDanYao(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_ELIXIR:
				UseDanFang(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_SIGN:
				UseFuShu(data, gameObject);
				break;
			case GoodsTypeEnum.SIGN:
				UseFuLu(data, gameObject);
				break;
			case GoodsTypeEnum.SECREAT_AREA:
			case GoodsTypeEnum.GOODS_SECRET:
			case GoodsTypeEnum.INHERIT_SECRET:
				UseMiJingKey(data, gameObject);
				break;
			case GoodsTypeEnum.GUILD_SECRET:
				UseDongTianFuDi(data, gameObject);
				break;
			case GoodsTypeEnum.MATERIAL_PLANT:
				UseZhiWu(data, gameObject);
				break;
			case GoodsTypeEnum.HaoYunFu:
				UseHaoYunFu(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_MATRIX:
				UseZhenTu(data, gameObject);
				break;
			}
		}

		private void UseJiNeng(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseGongFa(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
						if (Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 6;
							Singleton<GuideManager>.Instance.ShowGuideNew(1);
							ShangChuan(1);
						}
					}
					else if (Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1)
					{
						Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 4;
						Singleton<GuideManager>.Instance.ShowGuideNew(1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseQiTu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
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

		private void UseZhuangBei(BaseNaJieGoodEntity _data)
		{
			if (_data.Type == 16 || _data.Type == 15 || _data.Type == 26 || _data.Type == 20)
			{
				Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab").ZhanDouBtnClick();
				Singleton<UIManager>.Instance.CloseWnd(this);
			}
		}

		private void UseDanFang(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseFuShu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseZhenTu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseZhenPan(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
		}

		private void UseFuLu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("请在战斗设置中设置符箓");
		}

		private void UseDanYao(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			bool flag = false;
			for (int i = 0; i < _data.ContentList.Count; i++)
			{
				if (_data.ContentList[i].Contains("增长类-体魄") || _data.ContentList[i].Contains("增长类-神识") || _data.ContentList[i].Contains("增长类-灵气") || _data.ContentList[i].Contains("增长类-真气") || _data.ContentList[i].Contains("增长类-真元"))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("如果当前存在未炼化完全的丹药，将直接覆盖丹药效果！", () =>
				{
					UseElixir(_data, itemObj);
				});
			}
			else
			{
				UseElixir(_data, itemObj);
			}
		}

		private void UseElixir(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			UsedElixirReq_1004032 data = new UsedElixirReq_1004032
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USED_ELIXIR, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UsedElixirResp_1004032 usedElixirResp_ = new UsedElixirResp_1004032();
					usedElixirResp_.MergeFrom(byteString);
					if (usedElixirResp_ != null && usedElixirResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
						RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
						role_Entity.PureBlood = usedElixirResp_.PureBlood;
						role_Entity.CurrentPureBlood = usedElixirResp_.CurrentPureBlood;
						role_Entity.FlexibleRoot = usedElixirResp_.FlexibleRoot;
						role_Entity.SuperAir = usedElixirResp_.SuperAir;
						role_Entity.CurrentSuperAir = usedElixirResp_.CurrentSuperAir;
						role_Entity.Perception = usedElixirResp_.Perception;
						role_Entity.CurrentPerception = usedElixirResp_.CurrentPerception;
						role_Entity.Physique = usedElixirResp_.Physique;
						role_Entity.CurrentPhysique = usedElixirResp_.CurrentPhysique;
						role_Entity.CurrentVigor = usedElixirResp_.CurrentVigor;
						role_Entity.CurrentClever = usedElixirResp_.CurrentClever;
						role_Entity.CurrentPoisonVal = usedElixirResp_.CurrentPoisonVal;
						role_Entity.JinLingGen = usedElixirResp_.HasGold;
						role_Entity.MuLingGen = usedElixirResp_.HasWood;
						role_Entity.ShuiLingGen = usedElixirResp_.HasWater;
						role_Entity.TuLingGen = usedElixirResp_.HasEarth;
						role_Entity.HuoLingGen = usedElixirResp_.HasFire;
						role_Entity.HunLiVal = usedElixirResp_.HunLiVal;
						role_Entity.CurrentPower = usedElixirResp_.CurrentPower;
						role_Entity.CurrentAge = usedElixirResp_.Age;
						role_Entity.MaxAge = usedElixirResp_.MaxAge;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
						{
							Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab")?.OnShow();
						}
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(usedElixirResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseHaoYunFu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ExtendQiYun = 2000;
						RemoveWuPin(_data.GoodsId, 1);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseMiJingKey(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentPower < 20)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前体力不足20，无法打开秘境");
				return;
			}
			UsedSecretReq_1004031 data = new UsedSecretReq_1004031
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USED_SECRET, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UsedSecretResp_1004031 usedSecretResp_ = new UsedSecretResp_1004031();
					usedSecretResp_.MergeFrom(byteString);
					if (usedSecretResp_ != null && usedSecretResp_.IsSuccess)
					{
						PrepareEnterMiJing(usedSecretResp_.MapId, usedSecretResp_.MapType, usedSecretResp_.FiveElement, _data, itemObj);
					}
					else
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(usedSecretResp_.Code));
					}
				}
			}, isShowWaitUI: true);
		}

		private void UseZhiWu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GatherSeedReq_1013021 data = new GatherSeedReq_1013021
			{
				SeedId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GATHER_SEED_RESP, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GatherSeedResp_1013021 gatherSeedResp_ = new GatherSeedResp_1013021();
					gatherSeedResp_.MergeFrom(byteString);
					if (gatherSeedResp_.IsSuccess)
					{
						string textValue = "采集灵种成功！";
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(gatherSeedResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void PrepareEnterMiJing(string mapId, int mapType, int fiveElement, BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			BuildDungeonReq_1010001 data = new BuildDungeonReq_1010001
			{
				MapType = mapType,
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUILDDUNGEON, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuildDungeonResp_1010001 buildDungeonResp_ = new BuildDungeonResp_1010001();
					buildDungeonResp_.MergeFrom(byteString);
					string text = "";
					string text2 = "";
					if (buildDungeonResp_.IsSuccess)
					{
						text = buildDungeonResp_.MapId;
						text2 = buildDungeonResp_.DungeonId;
						MapEntity mapModel = new MapEntity
						{
							Id = text,
							MapName = _data.Name,
							FiveElement = fiveElement
						};
						RoleEnterDungeon(text, text2, mapModel, buildDungeonResp_.Address, buildDungeonResp_.Port);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Singleton<RoleManager>.Instance.Role_Entity.CurrentPower = buildDungeonResp_.CurrentPower;
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				}
			}, isShowWaitUI: true);
		}

		private void RoleEnterDungeon(string mapId, string dungeonId, MapEntity mapModel, string ipAddress, int port)
		{
			RoleJoinDungeonReq_1010002 data = new RoleJoinDungeonReq_1010002
			{
				DungeonId = dungeonId,
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLEJOINDUNGEON, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleJoinDungeonResp_1010002 roleJoinDungeonResp_ = new RoleJoinDungeonResp_1010002();
					roleJoinDungeonResp_.MergeFrom(byteString);
					if (roleJoinDungeonResp_.IsSuccess)
					{
						BattleLifeModel roleInfo = new BattleLifeModel(roleJoinDungeonResp_);
						Singleton<BattleManager>.Instance.StartBattle(mapModel, BattleType.MiJingBattle, roleInfo, dungeonId, ipAddress, port);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleJoinDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void UseDongTianFuDi(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void RongLiangReflash()
		{
			if (!(Panel == null) && !(Panel.RongLiangText == null) && !(Panel.RongLiangValueProgress == null) && m_DataSource != null)
			{
				float fillAmount = ((m_DataSource.MaxCapacity == 0) ? 0f : (1f * (float)m_DataSource.NowCapacity / (float)m_DataSource.MaxCapacity));
				Panel.RongLiangText.text = $"{m_DataSource.NowCapacity}/{m_DataSource.MaxCapacity}（立方尺）";
				Panel.RongLiangValueProgress.fillAmount = fillAmount;
			}
		}

		private void ShengJiBeiBao()
		{
			double num = ((Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 11) ? (m_DataSource.MaxCapacity - 50) : m_DataSource.MaxCapacity);
			double price = 1.0;
			switch ((int)num)
			{
			case 50:
				price = 10.0;
				break;
			case 60:
				price = 30.0;
				break;
			case 70:
				price = 60.0;
				break;
			case 80:
				price = 100.0;
				break;
			case 90:
				price = 150.0;
				break;
			case 100:
				price = 200.0;
				break;
			case 110:
				price = 300.0;
				break;
			case 120:
				price = 400.0;
				break;
			case 130:
				price = 500.0;
				break;
			case 140:
				price = 600.0;
				break;
			case 150:
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前纳戒已达到最大等级！");
				return;
			}
			Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, price, (UnityAction<int, int>)((int p, int t) =>
			{
				if (Singleton<RoleManager>.Instance.ShengYuLingYu(price) < 0.0)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("灵玉不足！");
				}
				else
				{
					SendUpdateRequest(price);
				}
			}), 2);
		}

		private void SendUpdateRequest(double price)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_NAJIE, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GradeRingResp_1004025 gradeRingResp_ = new GradeRingResp_1004025();
					gradeRingResp_.MergeFrom(byteString);
					if (gradeRingResp_.IsSuccess)
					{
						m_DataSource.MaxCapacity = gradeRingResp_.MaxCapacity;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = gradeRingResp_.ShinyJade;
						RongLiangReflash();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(gradeRingResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Singleton<UIManager>.Instance.CloseWnd("Common_ShaiXuanWnd.prefab");
			if (m_Common_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd common_ShaiXuanWnd = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(common_ShaiXuanWnd.ReChooseAction, new UnityAction(FlashData));
			}
			if (m_ScrollViewInfiniteContent != null)
			{
				ScrollViewInfiniteContent scrollViewInfiniteContent = m_ScrollViewInfiniteContent;
				scrollViewInfiniteContent.ChangeRowAction = (UnityAction<int, int>)Delegate.Remove(scrollViewInfiniteContent.ChangeRowAction, new UnityAction<int, int>(ChangeScrollContent));
			}
		}
	}
}
