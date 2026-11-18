using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiWnd : Window
	{
		public RoleZhuangBeiPanel Panel;

		private GameObject m_InstanceObj;

		private GameObject m_XiangQingInstanceObj;

		private int m_CurrentStage = 1;

		private PeiZhiType m_CurrentPeiZhiType;

		private RoleZhuangBeiType m_RoleZhuangBeiType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, Sprite> m_SkillIconSprite;

		private List<UI_GoodsItem> m_PeiZhiList;

		private Dictionary<string, UI_GoodsItem> m_CanPeiZhiList;

		private List<UI_GoodsItem> m_ZhuangBeiRaidList;

		private Dictionary<string, UI_GoodsItem> m_ZhuangBeiList;

		private XzjBoxEntity m_XzjBoxEntity;

		private List<BaseNaJieGoodEntity> m_SkillList;

		private Dictionary<XiangQingType, string> SuperAirZongLanXiangQing;

		private Dictionary<XiangQingType, string> TiPoZongLanXiangQing;

		private Dictionary<XiangQingType, string> ShenShiZongLanXiangQing;

		private Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem> ZhuangBeiXiangQingDic;

		private Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem> JiNengXiangQingDic;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<RoleZhuangBeiPanel>();
			SuperAirZongLanXiangQing = new Dictionary<XiangQingType, string>();
			TiPoZongLanXiangQing = new Dictionary<XiangQingType, string>();
			ShenShiZongLanXiangQing = new Dictionary<XiangQingType, string>();
			m_ZhuangBeiRaidList = new List<UI_GoodsItem>();
			ZhuangBeiXiangQingDic = new Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem>();
			JiNengXiangQingDic = new Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem>();
			m_SkillList = new List<BaseNaJieGoodEntity>();
			m_PeiZhiList = new List<UI_GoodsItem>();
			m_ZhuangBeiList = new Dictionary<string, UI_GoodsItem>();
			m_CanPeiZhiList = new Dictionary<string, UI_GoodsItem>();
			m_XzjBoxEntity = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			AddButtonClickListener(Panel.XiaoWenHao, WenHaoTogClick);
			AddToggleClickListener(Panel.ZhuangBeiTog, ZhuangBeiShow);
			AddToggleClickListener(Panel.JiNengTog, JiNengShow);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_1, Stage_1_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_2, Stage_2_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_3, Stage_3_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_4, Stage_4_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_5, Stage_5_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_6, Stage_6_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_7, Stage_7_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_8, Stage_8_Click);
			AddToggleClickListener(Panel.ZhuangBeiRect.Tog_Level_9, Stage_9_Click);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_1, FaShuTogClick);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_2, FaQiTogClick);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_3, FuLuTogClick);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_4, ZhenPanTogClick);
			m_RoleZhuangBeiType = RoleZhuangBeiType.DefencePanel;
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_XiangQingInstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiShuXingContentItem.prefab");
			ResourceLoad();
			m_CurrentStage = 1;
			m_CurrentPeiZhiType = PeiZhiType.FaShu;
			if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu < 2)
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 2;
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ZhuangBeiInit();
			JiNengInit();
			GetRoleChuanDaiZhuangBei();
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			m_SkillIconSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_Xie.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FangJu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuLu.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenPan.png");
			m_GoodsIconSprite.Add(29, value);
			m_GoodsIconSprite.Add(15, value2);
			m_GoodsIconSprite.Add(16, value3);
			m_GoodsIconSprite.Add(26, value4);
			m_GoodsIconSprite.Add(21, value5);
			m_GoodsIconSprite.Add(20, value6);
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value7);
			m_RankBgSprite.Add(2, value8);
			m_RankBgSprite.Add(3, value9);
			m_RankBgSprite.Add(4, value10);
			Sprite value11 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_AreaDamage.png");
			Sprite value12 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_ContinueDamage.png");
			Sprite value13 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DefenceSkill.png");
			Sprite value14 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DownStatus.png");
			Sprite value15 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_NoMove.png");
			Sprite value16 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_OneDamage.png");
			Sprite value17 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_UpStatus.png");
			Sprite value18 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			Sprite value19 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			m_SkillIconSprite.Add(14, value11);
			m_SkillIconSprite.Add(12, value12);
			m_SkillIconSprite.Add(20, value13);
			m_SkillIconSprite.Add(40, value14);
			m_SkillIconSprite.Add(41, value15);
			m_SkillIconSprite.Add(10, value16);
			m_SkillIconSprite.Add(30, value17);
			m_SkillIconSprite.Add(31, value17);
			m_SkillIconSprite.Add(50, value18);
			m_SkillIconSprite.Add(51, value18);
			m_SkillIconSprite.Add(60, value19);
		}

		private void ZhuangBeiInit()
		{
			foreach (UI_GoodsItem chuanDai in Panel.ZhuangBeiRect.ChuanDaiList)
			{
				m_ZhuangBeiRaidList.Add(chuanDai);
			}
		}

		private void JiNengInit()
		{
			foreach (UI_GoodsItem peiZhi in Panel.JiNengRect.PeiZhiList)
			{
				m_PeiZhiList.Add(peiZhi);
			}
		}

		private void InitZongLan(EnterEquipmentResp_1009005 enterEquipmentResp_1009005)
		{
			SuperAirZongLanXiangQing.Clear();
			TiPoZongLanXiangQing.Clear();
			ShenShiZongLanXiangQing.Clear();
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir;
				num2 = enterEquipmentResp_1009005.SuperAirPower;
				num3 = enterEquipmentResp_1009005.SuperAirRecoverValueOnBattle;
				num4 = enterEquipmentResp_1009005.SuperAirRecoverValue;
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100;
				num2 = enterEquipmentResp_1009005.SuperAirPower / 100.0;
				num3 = enterEquipmentResp_1009005.SuperAirRecoverValueOnBattle / 100.0;
				num4 = enterEquipmentResp_1009005.SuperAirRecoverValue / 100.0;
			}
			else
			{
				num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100 / 100;
				num2 = enterEquipmentResp_1009005.SuperAirPower / 100.0 / 100.0;
				num3 = enterEquipmentResp_1009005.SuperAirRecoverValueOnBattle / 100.0 / 100.0;
				num4 = enterEquipmentResp_1009005.SuperAirRecoverValue / 100.0 / 100.0;
			}
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAir, num.ToString());
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAirPower, num2.ToString());
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAirRecoverOnBattle, num3.ToString());
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAirRecover, num4.ToString());
			long num5 = ((enterEquipmentResp_1009005.CanUsePhysique > Singleton<RoleManager>.Instance.Role_Entity.Physique) ? Singleton<RoleManager>.Instance.Role_Entity.Physique : enterEquipmentResp_1009005.CanUsePhysique);
			TiPoZongLanXiangQing.Add(XiangQingType.Physique, Singleton<RoleManager>.Instance.Role_Entity.Physique.ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.CanUsePhysique, num5.ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.DefenceTotalValue, (enterEquipmentResp_1009005.CurrentPhysiqueDefense + enterEquipmentResp_1009005.CurrentPureBloodDefense).ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.PhysiqueRecoverOnBattle, enterEquipmentResp_1009005.DefenseRecoverValueOnBattle.ToString("0"));
			TiPoZongLanXiangQing.Add(XiangQingType.PhysiqueRecover, enterEquipmentResp_1009005.DefenseRecoverValue.ToString("0"));
			ShenShiZongLanXiangQing.Add(XiangQingType.Perception, Singleton<RoleManager>.Instance.Role_Entity.Perception.ToString());
			ShenShiZongLanXiangQing.Add(XiangQingType.FullLoadPerception, enterEquipmentResp_1009005.FullLoadPerception.ToString());
			ShenShiZongLanXiangQing.Add(XiangQingType.CurrentMoveSpeed, enterEquipmentResp_1009005.CurrentMoveSpeed.ToString("0"));
		}

		private void ReSetZongLan(XiangQingType title, string newValue)
		{
			string value = "";
			if (SuperAirZongLanXiangQing.TryGetValue(title, out value))
			{
				SuperAirZongLanXiangQing[title] = newValue;
			}
			else if (TiPoZongLanXiangQing.TryGetValue(title, out value))
			{
				TiPoZongLanXiangQing[title] = newValue;
			}
			else if (ShenShiZongLanXiangQing.TryGetValue(title, out value))
			{
				ShenShiZongLanXiangQing[title] = newValue;
			}
		}

		private void InitZongLanXiangQing()
		{
			if (Panel == null || !Panel.isActiveAndEnabled)
			{
				return;
			}
			ZhuangBeiXiangQingDic.Clear();
			JiNengXiangQingDic.Clear();
			foreach (Transform item in Panel.ZhuangBeiRect.XiangQingScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in Panel.JiNengRect.XiangQingScrollViewContent)
			{
				Object.Destroy(item2.gameObject);
			}
			InitShuXingContent(GongFaType.LianShen, XiangQingType.Perception, ShenShiZongLanXiangQing, Panel.ZhuangBeiRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.DuanTi, XiangQingType.Physique, TiPoZongLanXiangQing, Panel.ZhuangBeiRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.NaQi, XiangQingType.SuperAir, SuperAirZongLanXiangQing, Panel.ZhuangBeiRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.LianShen, XiangQingType.Perception, ShenShiZongLanXiangQing, Panel.JiNengRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.DuanTi, XiangQingType.Physique, TiPoZongLanXiangQing, Panel.JiNengRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.NaQi, XiangQingType.SuperAir, SuperAirZongLanXiangQing, Panel.JiNengRect.XiangQingScrollViewContent);
		}

		private void InitShuXingContent(GongFaType gongFaType, XiangQingType xiangQingType, Dictionary<XiangQingType, string> data, RectTransform rectTransform)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string value = "";
			foreach (KeyValuePair<XiangQingType, string> datum in data)
			{
				if (datum.Key == xiangQingType)
				{
					value = datum.Value;
					continue;
				}
				string xiangQingTitle = GetXiangQingTitle(datum.Key);
				if (!dictionary.ContainsKey(xiangQingTitle))
				{
					dictionary.Add(xiangQingTitle, datum.Value);
				}
			}
			Object.Instantiate(m_XiangQingInstanceObj, rectTransform).GetComponent<RoleZhuangBeiShuXingContentItem>().SetValue(gongFaType, value, dictionary);
		}

		private string GetXiangQingTitle(XiangQingType titleType)
		{
			return titleType switch
			{
				XiangQingType.SuperAir => "真元总值", 
				XiangQingType.SuperAirPower => "每息输出", 
				XiangQingType.SuperAirRecoverOnBattle => "战斗恢复", 
				XiangQingType.SuperAirRecover => "脱战恢复", 
				XiangQingType.Physique => "体魄总值", 
				XiangQingType.CanUsePhysique => "可用体魄", 
				XiangQingType.DefenceTotalValue => "防御总值", 
				XiangQingType.PhysiqueRecoverOnBattle => "战斗恢复", 
				XiangQingType.PhysiqueRecover => "脱战恢复", 
				XiangQingType.Perception => "神识总值", 
				XiangQingType.FullLoadPerception => "满载神识", 
				XiangQingType.CurrentMoveSpeed => "移动速度", 
				_ => "", 
			};
		}

		private void GetRoleChuanDaiZhuangBei()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_EQUIPMENT, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterEquipmentResp_1009005 enterEquipmentResp_ = new EnterEquipmentResp_1009005();
					enterEquipmentResp_.MergeFrom(byteString);
					if (enterEquipmentResp_.IsSuccess)
					{
						InitZongLan(enterEquipmentResp_);
						ChuanDaiZhuangBeiClear();
						for (int i = 0; i < enterEquipmentResp_.DressedEquipments.Count; i++)
						{
							EnterEquipmentResp_1009005.Types.EquipmentInfoResp_1009006 equipmentInfoResp_ = enterEquipmentResp_.DressedEquipments[i];
							BaseNaJieGoodEntity dataSource = new BaseNaJieGoodEntity
							{
								FiveElement = equipmentInfoResp_.FiveElement,
								GoodsId = equipmentInfoResp_.GoodsId,
								IsBinding = true,
								IsUsing = true,
								Level = equipmentInfoResp_.Level,
								Name = equipmentInfoResp_.Name,
								Rank = equipmentInfoResp_.Rank,
								Type = equipmentInfoResp_.Type
							};
							ChuanDaiZhuangBei(dataSource);
						}
						m_XzjBoxEntity.GetAllGoods(GetGoodsCompelete);
						InitZongLanXiangQing();
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
			}, isShowWaitUI: true);
		}

		private void ZhuangBeiReflashData()
		{
			m_ZhuangBeiList.Clear();
			m_XzjBoxEntity.GetAllGoods(GetGoodsCompelete);
		}

		private void GetGoodsCompelete()
		{
			if (Panel == null || !Panel.isActiveAndEnabled)
			{
				return;
			}
			GridLayoutGroup component = Panel.ZhuangBeiRect.GoodsScrollViewContent.GetComponent<GridLayoutGroup>();
			ClearZhuangBeiObjs();
			int num = 0;
			foreach (BaseNaJieGoodEntity goods in m_XzjBoxEntity.GoodsList)
			{
				if (m_CurrentStage == goods.GetGoodsStage() && (goods.Type == 16 || goods.Type == 29))
				{
					CreateZhuangBeiItem(goods);
					num++;
				}
			}
			SetGrticalListContentHight(num, Panel.ZhuangBeiRect.GoodsScrollViewContent, component);
		}

		private void GetGoodsIcon(int type, int rank, ref Sprite GoodsIcon, ref Sprite RankImg)
		{
			m_GoodsIconSprite.TryGetValue(type, out GoodsIcon);
			m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(rank), out RankImg);
		}

		private void CreateZhuangBeiItem(BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(Panel.ZhuangBeiRect.GoodsScrollViewContent, m_InstanceObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite RankImg = null;
			Sprite GoodsIcon = null;
			GetGoodsIcon(_data.Type, _data.Rank, ref GoodsIcon, ref RankImg);
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, RankImg, GoodsIcon, (object _obj, GameObject _gameObj) =>
			{
				GoodsClick(_obj);
			});
			if (!m_ZhuangBeiList.ContainsKey(_data.GoodsId))
			{
				m_ZhuangBeiList.Add(_data.GoodsId, component);
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
			naJieTipModel.LeftBtnClickEvent = null;
			naJieTipModel.MiddleBtnClickEvent = (int type, object dataObj) =>
			{
				if (DyBtnClick != null)
				{
					DyBtnClick(type, dataObj, gameObj);
				}
			};
			naJieTipModel.Name = baseNaJieGoodEntity.Name;
			naJieTipModel.ContentList = baseNaJieGoodEntity.ContentList;
			naJieTipModel.type = baseNaJieGoodEntity.Type;
			naJieTipModel.dataSource = baseNaJieGoodEntity;
			naJieTipModel.IsLock = baseNaJieGoodEntity.IsLock;
			Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, naJieTipModel);
		}

		private void GoodsClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			new List<string>();
			string btnName = "穿戴";
			if (!baseNaJieGoodEntity.IsBinding)
			{
				btnName = "滴血认主";
			}
			m_XzjBoxEntity.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> list) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, list, baseNaJieGoodEntity.Name, btnName, baseNaJieGoodEntity.Type, new UnityAction<object>(GoodsTipClick), dataSource);
			});
		}

		private void GoodsTipClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (!baseNaJieGoodEntity.IsBinding)
			{
				DiXueRenZhu(baseNaJieGoodEntity);
				return;
			}
			DressEquipmentReq_1009001 data = new DressEquipmentReq_1009001
			{
				EquipmentId = baseNaJieGoodEntity.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.DRESS_EQUIPMENT, data, (object _obj) =>
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 4;
					Singleton<GuideManager>.Instance.ShowGuideNew(2);
					ShangChuan(2);
				}
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					DressEquipmentResp_1009001 dressEquipmentResp_ = new DressEquipmentResp_1009001();
					dressEquipmentResp_.MergeFrom(byteString);
					if (dressEquipmentResp_.IsSuccess)
					{
						ReSetZongLan(XiangQingType.CurrentMoveSpeed, dressEquipmentResp_.CurrentMoveSpeed.ToString("0"));
						double num = 0.0;
						double num2 = 0.0;
						if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
						{
							num = dressEquipmentResp_.SuperAirRecoverValueOnBattle;
							num2 = dressEquipmentResp_.SuperAirRecoverValue;
						}
						else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
						{
							num = dressEquipmentResp_.SuperAirRecoverValueOnBattle / 100.0;
							num2 = dressEquipmentResp_.SuperAirRecoverValue / 100.0;
						}
						else
						{
							num = dressEquipmentResp_.SuperAirRecoverValueOnBattle / 100.0 / 100.0;
							num2 = dressEquipmentResp_.SuperAirRecoverValue / 100.0 / 100.0;
						}
						ReSetZongLan(XiangQingType.SuperAirRecover, num2.ToString("0"));
						ReSetZongLan(XiangQingType.SuperAirRecoverOnBattle, num.ToString("0"));
						ReSetZongLan(XiangQingType.PhysiqueRecover, dressEquipmentResp_.DefenseRecoverValue.ToString("0"));
						ReSetZongLan(XiangQingType.PhysiqueRecoverOnBattle, dressEquipmentResp_.DefenseRecoverValueOnBattle.ToString("0"));
						ReSetZongLan(XiangQingType.DefenceTotalValue, (dressEquipmentResp_.CurrentPhysiqueDefense + dressEquipmentResp_.CurrentPureBloodDefense).ToString());
						ReSetZongLan(XiangQingType.CanUsePhysique, dressEquipmentResp_.CanUsedPhysique.ToString("0"));
						ChuanDaiZhuangBei(baseNaJieGoodEntity);
						InitZongLanXiangQing();
						if (m_ZhuangBeiList.ContainsKey(baseNaJieGoodEntity.GoodsId))
						{
							UI_GoodsItem uI_GoodsItem = m_ZhuangBeiList[baseNaJieGoodEntity.GoodsId];
							m_ZhuangBeiList.Remove(baseNaJieGoodEntity.GoodsId);
							Object.Destroy(uI_GoodsItem.gameObject);
						}
						m_XzjBoxEntity.RemoveGoods(baseNaJieGoodEntity.GoodsId);
					}
					else
					{
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 2;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(dressEquipmentResp_.Code);
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
			Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal++;
		}

		private void DiXueRenZhu(BaseNaJieGoodEntity dataSource)
		{
			GoodsBindingReq_1004023 data = new GoodsBindingReq_1004023
			{
				GoodsId = dataSource.GoodsId
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
						dataSource.IsBinding = true;
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBindingResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void ChuanDaiZhuangBei(BaseNaJieGoodEntity dataSource)
		{
			UI_GoodsItem uI_GoodsItem = null;
			foreach (UI_GoodsItem zhuangBeiRaid in m_ZhuangBeiRaidList)
			{
				if (zhuangBeiRaid.IsEmpty)
				{
					uI_GoodsItem = zhuangBeiRaid;
					break;
				}
			}
			if (uI_GoodsItem != null)
			{
				Sprite RankImg = null;
				Sprite GoodsIcon = null;
				GetGoodsIcon(dataSource.Type, dataSource.Rank, ref GoodsIcon, ref RankImg);
				uI_GoodsItem.SetItem(dataSource, dataSource.Name, dataSource.FiveElement, dataSource.Level, RankImg, GoodsIcon, ChuanDaiZhuangBeiClick);
				ZhuangBeiReflashData();
			}
		}

		private void ChuanDaiZhuangBeiClear()
		{
			foreach (UI_GoodsItem zhuangBeiRaid in m_ZhuangBeiRaidList)
			{
				if (!zhuangBeiRaid.IsEmpty)
				{
					zhuangBeiRaid.ClearItem();
				}
			}
		}

		private void ChuanDaiZhuangBeiClick(object data, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			m_XzjBoxEntity.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> list) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, list, baseNaJieGoodEntity.Name, "卸下", baseNaJieGoodEntity.Type, (UnityAction<object>)((object obj) =>
				{
					TuoXiaZhuangBei(obj, gameObj);
				}), baseNaJieGoodEntity);
			});
		}

		private void TuoXiaZhuangBei(object data, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			UndressEquipmentReq_1009002 data2 = new UndressEquipmentReq_1009002
			{
				EquipmentId = baseNaJieGoodEntity.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UNDRESS_EQUIPMENT, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UndressEquipmentResp_1009002 undressEquipmentResp_ = new UndressEquipmentResp_1009002();
					undressEquipmentResp_.MergeFrom(byteString);
					if (undressEquipmentResp_.IsSuccess)
					{
						JieChuZhuangBei(baseNaJieGoodEntity.GoodsId);
						if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
						{
							_ = undressEquipmentResp_.SuperAirRecoverValueOnBattle;
							_ = undressEquipmentResp_.SuperAirRecoverValue;
						}
						else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
						{
							_ = undressEquipmentResp_.SuperAirRecoverValueOnBattle / 100.0;
							_ = undressEquipmentResp_.SuperAirRecoverValue / 100.0;
						}
						else
						{
							_ = undressEquipmentResp_.SuperAirRecoverValueOnBattle / 100.0 / 100.0;
							_ = undressEquipmentResp_.SuperAirRecoverValue / 100.0 / 100.0;
						}
						ReSetZongLan(XiangQingType.CurrentMoveSpeed, undressEquipmentResp_.CurrentMoveSpeed.ToString("0"));
						ReSetZongLan(XiangQingType.SuperAirRecover, undressEquipmentResp_.SuperAirRecoverValue.ToString("0"));
						ReSetZongLan(XiangQingType.SuperAirRecoverOnBattle, undressEquipmentResp_.SuperAirRecoverValueOnBattle.ToString("0"));
						ReSetZongLan(XiangQingType.PhysiqueRecover, undressEquipmentResp_.DefenseRecoverValue.ToString("0"));
						ReSetZongLan(XiangQingType.PhysiqueRecoverOnBattle, undressEquipmentResp_.DefenseRecoverValueOnBattle.ToString("0"));
						ReSetZongLan(XiangQingType.PhysiqueRecoverOnBattle, undressEquipmentResp_.DefenseRecoverValueOnBattle.ToString("0"));
						ReSetZongLan(XiangQingType.DefenceTotalValue, (undressEquipmentResp_.CurrentPhysiqueDefense + undressEquipmentResp_.CurrentPureBloodDefense).ToString());
						ReSetZongLan(XiangQingType.CanUsePhysique, undressEquipmentResp_.CanUsedPhysique.ToString("0"));
						InitZongLanXiangQing();
						gameObj.GetComponent<UI_GoodsItem>().ClearItem();
						switch (m_CurrentStage)
						{
						case 1:
							Panel.ZhuangBeiRect.Tog_Level_1.isOn = true;
							break;
						case 2:
							Panel.ZhuangBeiRect.Tog_Level_2.isOn = true;
							break;
						case 3:
							Panel.ZhuangBeiRect.Tog_Level_3.isOn = true;
							break;
						case 4:
							Panel.ZhuangBeiRect.Tog_Level_4.isOn = true;
							break;
						case 5:
							Panel.ZhuangBeiRect.Tog_Level_5.isOn = true;
							break;
						case 6:
							Panel.ZhuangBeiRect.Tog_Level_6.isOn = true;
							break;
						case 7:
							Panel.ZhuangBeiRect.Tog_Level_7.isOn = true;
							break;
						case 8:
							Panel.ZhuangBeiRect.Tog_Level_8.isOn = true;
							break;
						case 9:
							Panel.ZhuangBeiRect.Tog_Level_9.isOn = true;
							break;
						}
						ZhuangBeiReflashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(undressEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void Stage_1_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(1);
			}
		}

		public void Stage_2_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(2);
			}
		}

		public void Stage_3_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(3);
			}
		}

		public void Stage_4_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(4);
			}
		}

		public void Stage_5_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(5);
			}
		}

		public void Stage_6_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(6);
			}
		}

		public void Stage_7_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(7);
			}
		}

		public void Stage_8_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(8);
			}
		}

		public void Stage_9_Click(bool isOn)
		{
			if (isOn)
			{
				StageTogClick(9);
			}
		}

		private void StageTogClick(int stage)
		{
			m_CurrentStage = stage;
			ZhuangBeiReflashData();
		}

		public void ClearZhuangBeiObjs()
		{
			foreach (Transform item in Panel.ZhuangBeiRect.GoodsScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void GetRoleJiNeng()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_SKILL, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterSkillResp_1009007 enterSkillResp_ = new EnterSkillResp_1009007();
					enterSkillResp_.MergeFrom(byteString);
					if (enterSkillResp_.IsSuccess)
					{
						ClearChuanDaiZhuangBeiAndFuLu();
						for (int i = 0; i < enterSkillResp_.SetableSkills.Count; i++)
						{
							EnterSkillResp_1009007.Types.SkillInfoResp_1009008 skillInfoResp_ = enterSkillResp_.SetableSkills[i];
							if (skillInfoResp_ != null && !string.IsNullOrEmpty(skillInfoResp_.GoodsId))
							{
								BaseNaJieGoodEntity dataSource = new BaseNaJieGoodEntity
								{
									FiveElement = skillInfoResp_.FiveElement,
									GoodsId = skillInfoResp_.GoodsId,
									IsBinding = true,
									IsUsing = true,
									Level = skillInfoResp_.Level,
									Name = skillInfoResp_.Name,
									Rank = skillInfoResp_.Rank,
									Type = skillInfoResp_.Type,
									SecondType = skillInfoResp_.SettingType
								};
								ChuanDaiZhuangBeiAndFulu(dataSource, skillInfoResp_.CombatSettingId);
							}
						}
						m_SkillList.Clear();
						for (int j = 0; j < enterSkillResp_.CanuseSkills.Count; j++)
						{
							EnterSkillResp_1009007.Types.SkillInfoResp_1009008 skillInfoResp_2 = enterSkillResp_.CanuseSkills[j];
							if (skillInfoResp_2 != null && !string.IsNullOrEmpty(skillInfoResp_2.GoodsId))
							{
								BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
								{
									FiveElement = skillInfoResp_2.FiveElement,
									GoodsId = skillInfoResp_2.GoodsId,
									IsBinding = true,
									IsUsing = false,
									Level = skillInfoResp_2.Level,
									Name = skillInfoResp_2.Name,
									Rank = skillInfoResp_2.Rank,
									Type = skillInfoResp_2.Type,
									SecondType = skillInfoResp_2.SettingType
								};
								m_SkillList.Add(item);
							}
						}
						Panel.JiNengRect.Tog_Level_1.isOn = true;
						FaShuTogClick(isOn: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
				}
			});
		}

		private void GetFaShuIcon(int effect, ref Sprite FaShuIcon)
		{
			m_SkillIconSprite.TryGetValue(effect, out FaShuIcon);
		}

		private void FaShuTogClick(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			m_CurrentPeiZhiType = PeiZhiType.FaShu;
			ClearJiNengObjs();
			GridLayoutGroup component = Panel.JiNengRect.GoodsScrollViewContent.GetComponent<GridLayoutGroup>();
			int num = 0;
			m_CanPeiZhiList.Clear();
			foreach (BaseNaJieGoodEntity skill in m_SkillList)
			{
				if (!skill.IsUsing)
				{
					skill.SecondType = 1;
					CreateZhuangBeiAndFuLuItem(skill);
					num++;
				}
			}
			SetGrticalListContentHight(num, Panel.JiNengRect.GoodsScrollViewContent, component);
		}

		private void FaQiTogClick(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			m_CurrentPeiZhiType = PeiZhiType.ZhuangBei;
			ClearJiNengObjs();
			GridLayoutGroup component = Panel.JiNengRect.GoodsScrollViewContent.GetComponent<GridLayoutGroup>();
			int num = 0;
			m_CanPeiZhiList.Clear();
			foreach (BaseNaJieGoodEntity goods in m_XzjBoxEntity.GoodsList)
			{
				if (!goods.IsUsing)
				{
					goods.SecondType = 2;
					if (goods.Type == 15 || goods.Type == 26)
					{
						CreateZhuangBeiAndFuLuItem(goods);
						num++;
					}
				}
			}
			SetGrticalListContentHight(num, Panel.JiNengRect.GoodsScrollViewContent, component);
		}

		private void FuLuTogClick(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			m_CurrentPeiZhiType = PeiZhiType.ZhuangBei;
			ClearJiNengObjs();
			GridLayoutGroup component = Panel.JiNengRect.GoodsScrollViewContent.GetComponent<GridLayoutGroup>();
			int num = 0;
			m_CanPeiZhiList.Clear();
			foreach (BaseNaJieGoodEntity goods in m_XzjBoxEntity.GoodsList)
			{
				if (!goods.IsUsing)
				{
					goods.SecondType = 3;
					if (goods.Type == 21)
					{
						CreateZhuangBeiAndFuLuItem(goods);
						num++;
					}
				}
			}
			SetGrticalListContentHight(num, Panel.JiNengRect.GoodsScrollViewContent, component);
		}

		private void ZhenPanTogClick(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			m_CurrentPeiZhiType = PeiZhiType.ZhenPan;
			ClearJiNengObjs();
			GridLayoutGroup component = Panel.JiNengRect.GoodsScrollViewContent.GetComponent<GridLayoutGroup>();
			int num = 0;
			m_CanPeiZhiList.Clear();
			foreach (BaseNaJieGoodEntity goods in m_XzjBoxEntity.GoodsList)
			{
				if (!goods.IsUsing)
				{
					goods.SecondType = 4;
					if (goods.Type == 20)
					{
						CreateZhuangBeiAndFuLuItem(goods);
						num++;
					}
				}
			}
			SetGrticalListContentHight(num, Panel.JiNengRect.GoodsScrollViewContent, component);
		}

		private void CreateZhuangBeiAndFuLuItem(BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(Panel.JiNengRect.GoodsScrollViewContent, m_InstanceObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite RankImg = null;
			Sprite GoodsIcon = null;
			GetGoodsIcon(_data.Type, _data.Rank, ref GoodsIcon, ref RankImg);
			if (_data.SecondType == 1)
			{
				GetFaShuIcon(_data.Type, ref GoodsIcon);
			}
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, RankImg, GoodsIcon, (object _obj, GameObject _gameObj) =>
			{
				FuluAndWeaponClick(_obj);
			});
			if (!m_CanPeiZhiList.ContainsKey(_data.GoodsId))
			{
				m_CanPeiZhiList.Add(_data.GoodsId, component);
			}
		}

		private void FuluAndWeaponClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			new List<string>();
			string btnName = "配置";
			if (!baseNaJieGoodEntity.IsBinding && (baseNaJieGoodEntity.Type == 26 || baseNaJieGoodEntity.Type == 15))
			{
				btnName = "滴血认主";
			}
			if (baseNaJieGoodEntity.SecondType != 1)
			{
				m_XzjBoxEntity.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						BtnName = btnName,
						CloseBtnClick = null,
						ContentList = list,
						DataSource = dataSource,
						SureBtnClick = JiNengTipClick,
						TitleName = baseNaJieGoodEntity.Name
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
			else
			{
				Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity.Level, baseNaJieGoodEntity.Rank, baseNaJieGoodEntity.FiveElement, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						BtnName = btnName,
						CloseBtnClick = null,
						ContentList = list,
						DataSource = dataSource,
						SureBtnClick = JiNengTipClick,
						TitleName = baseNaJieGoodEntity.Name
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
		}

		private void JiNengTipClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (!baseNaJieGoodEntity.IsBinding && (baseNaJieGoodEntity.Type == 15 || baseNaJieGoodEntity.Type == 26))
			{
				DiXueRenZhu(baseNaJieGoodEntity);
				return;
			}
			int settingId = -1;
			for (int i = 0; i < m_PeiZhiList.Count; i++)
			{
				if (m_PeiZhiList[i].IsEmpty)
				{
					settingId = i;
					break;
				}
			}
			if (settingId == -1)
			{
				return;
			}
			SetSkillReq_1009003 data = new SetSkillReq_1009003
			{
				SkillId = baseNaJieGoodEntity.GoodsId,
				SkillType = baseNaJieGoodEntity.SecondType,
				CombatSettingId = settingId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SET_SKILL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SetSkillResp_1009003 setSkillResp_ = new SetSkillResp_1009003();
					setSkillResp_.MergeFrom(byteString);
					if (setSkillResp_.IsSuccess)
					{
						if (baseNaJieGoodEntity.Type == 15 || baseNaJieGoodEntity.Type == 26)
						{
							ReSetZongLan(XiangQingType.FullLoadPerception, setSkillResp_.FullLoadPerception.ToString());
						}
						InitZongLanXiangQing();
						ChuanDaiZhuangBeiAndFulu(baseNaJieGoodEntity, settingId);
						if (m_CanPeiZhiList.ContainsKey(baseNaJieGoodEntity.GoodsId))
						{
							UI_GoodsItem uI_GoodsItem = m_CanPeiZhiList[baseNaJieGoodEntity.GoodsId];
							m_CanPeiZhiList.Remove(baseNaJieGoodEntity.GoodsId);
							Object.Destroy(uI_GoodsItem.gameObject);
						}
						if (baseNaJieGoodEntity.Type == 1)
						{
							BaseNaJieGoodEntity baseNaJieGoodEntity2 = null;
							foreach (BaseNaJieGoodEntity skill in m_SkillList)
							{
								if (skill.GoodsId == baseNaJieGoodEntity.GoodsId)
								{
									baseNaJieGoodEntity2 = skill;
									break;
								}
							}
							if (baseNaJieGoodEntity2 != null)
							{
								m_SkillList.Remove(baseNaJieGoodEntity2);
							}
						}
						else
						{
							m_XzjBoxEntity.RemoveGoods(baseNaJieGoodEntity.GoodsId);
						}
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 8;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
							ShangChuan(2);
						}
					}
					else
					{
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 6;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(setSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChuanDaiZhuangBeiAndFulu(BaseNaJieGoodEntity dataSource, int index)
		{
			UI_GoodsItem uI_GoodsItem = null;
			for (int i = 0; i < m_PeiZhiList.Count; i++)
			{
				if (i == index && m_PeiZhiList[i].IsEmpty)
				{
					uI_GoodsItem = m_PeiZhiList[i];
					break;
				}
			}
			if (uI_GoodsItem != null)
			{
				Sprite RankImg = null;
				Sprite GoodsIcon = null;
				GetGoodsIcon(dataSource.Type, dataSource.Rank, ref GoodsIcon, ref RankImg);
				if (dataSource.SecondType == 1)
				{
					GetFaShuIcon(dataSource.Type, ref GoodsIcon);
				}
				uI_GoodsItem.SetItem(dataSource, dataSource.Name, dataSource.FiveElement, dataSource.Level, RankImg, GoodsIcon, PeiZhiClick);
				dataSource.IsUsing = true;
				ZhuangBeiReflashData();
			}
		}

		private void PeiZhiClick(object data, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity.SecondType != 1)
			{
				m_XzjBoxEntity.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> list) =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, list, baseNaJieGoodEntity.Name, "卸下", baseNaJieGoodEntity.Type, (UnityAction<object>)((object obj) =>
					{
						JieChuPeiZhi(obj, gameObj);
					}), baseNaJieGoodEntity);
				});
				return;
			}
			Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity.Level, baseNaJieGoodEntity.Rank, baseNaJieGoodEntity.FiveElement, (List<string> list) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, list, baseNaJieGoodEntity.Name, "卸下", baseNaJieGoodEntity.Type, (UnityAction<object>)((object obj) =>
				{
					JieChuPeiZhi(obj, gameObj);
				}), baseNaJieGoodEntity);
			});
		}

		private void JieChuPeiZhi(object data, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			RemoveSkillReq_1009004 data2 = new RemoveSkillReq_1009004
			{
				SkillId = baseNaJieGoodEntity.GoodsId,
				SkillType = baseNaJieGoodEntity.SecondType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REMOVE_SKILL, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RemoveSkillResp_1009004 removeSkillResp_ = new RemoveSkillResp_1009004();
					removeSkillResp_.MergeFrom(byteString);
					if (removeSkillResp_.IsSuccess)
					{
						JieChuChengGong(removeSkillResp_, baseNaJieGoodEntity, gameObj);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(removeSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JieChuChengGong(RemoveSkillResp_1009004 removeSkillResp_1009004, BaseNaJieGoodEntity baseNaJieGoodEntity, GameObject gameObj)
		{
			baseNaJieGoodEntity.IsUsing = false;
			BaseNaJieGoodEntity goodsInfo = m_XzjBoxEntity.GetGoodsInfo(baseNaJieGoodEntity.GoodsId);
			if (goodsInfo != null)
			{
				goodsInfo.IsUsing = false;
			}
			if (baseNaJieGoodEntity.Type == 15 || baseNaJieGoodEntity.Type == 26)
			{
				ReSetZongLan(XiangQingType.FullLoadPerception, removeSkillResp_1009004.FullLoadPerception.ToString());
				InitZongLanXiangQing();
			}
			UI_GoodsItem component = gameObj.GetComponent<UI_GoodsItem>();
			object dataSource = component.DataSource;
			component.ClearItem();
			switch (baseNaJieGoodEntity.SecondType)
			{
			case 1:
				if (dataSource is BaseNaJieGoodEntity item && !m_SkillList.Contains(item))
				{
					m_SkillList.Add(item);
				}
				if (!Panel.JiNengRect.Tog_Level_1.isOn)
				{
					Panel.JiNengRect.Tog_Level_1.isOn = true;
				}
				else
				{
					FaShuTogClick(isOn: true);
				}
				break;
			case 2:
				if (!Panel.JiNengRect.Tog_Level_2.isOn)
				{
					Panel.JiNengRect.Tog_Level_2.isOn = true;
				}
				else
				{
					FaQiTogClick(isOn: true);
				}
				break;
			case 3:
				if (!Panel.JiNengRect.Tog_Level_3.isOn)
				{
					Panel.JiNengRect.Tog_Level_3.isOn = true;
				}
				else
				{
					FuLuTogClick(isOn: true);
				}
				break;
			case 4:
				if (!Panel.JiNengRect.Tog_Level_4.isOn)
				{
					Panel.JiNengRect.Tog_Level_4.isOn = true;
				}
				else
				{
					ZhenPanTogClick(isOn: true);
				}
				break;
			}
		}

		private void ClearChuanDaiZhuangBeiAndFuLu()
		{
			foreach (UI_GoodsItem peiZhi in m_PeiZhiList)
			{
				if (!peiZhi.IsEmpty)
				{
					peiZhi.ClearItem();
				}
			}
		}

		public void ClearJiNengObjs()
		{
			if (Panel.JiNengRect.GoodsScrollViewContent == null)
			{
				return;
			}
			foreach (Transform item in Panel.JiNengRect.GoodsScrollViewContent)
			{
				if (item.gameObject != null)
				{
					Object.Destroy(item.gameObject);
				}
			}
		}

		private void JieChuZhuangBei(string id)
		{
			foreach (BaseNaJieGoodEntity goods in m_XzjBoxEntity.GoodsList)
			{
				if (goods.GoodsId == id)
				{
					goods.IsUsing = false;
					break;
				}
			}
		}

		private void ZhuangBeiShow(bool isOn)
		{
			Text component = Panel.ZhuangBeiTog.transform.Find("Text").GetComponent<Text>();
			if (isOn)
			{
				if (m_RoleZhuangBeiType != RoleZhuangBeiType.DefencePanel)
				{
					component.color = Color.white;
					GetRoleChuanDaiZhuangBei();
					Panel.ZhuangBeiRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 33.885f);
					ZhuangBeiReflashData();
					m_RoleZhuangBeiType = RoleZhuangBeiType.DefencePanel;
				}
			}
			else
			{
				Panel.ZhuangBeiRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
				component.color = Color.black;
			}
		}

		public void JiNengShow(bool isOn)
		{
			Text component = Panel.JiNengTog.transform.Find("Text").GetComponent<Text>();
			if (isOn)
			{
				if (m_RoleZhuangBeiType != RoleZhuangBeiType.SkillPanel)
				{
					component.color = Color.white;
					Panel.JiNengRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 33.885f);
					GetRoleJiNeng();
					m_RoleZhuangBeiType = RoleZhuangBeiType.SkillPanel;
				}
			}
			else
			{
				component.color = Color.black;
				Panel.JiNengRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}

		private void WenHaoTogClick()
		{
			string text = $"装备分为可穿戴的防具，如衣服、靴子等，还有可配置的法器，包括攻击法器、防御法器。所有的装备需要滴血认主后方可使用，滴血认主后无法再用于交易。穿戴防具需要消耗体魄值，体魄越强大，可以穿戴的防具越多。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "装备");
		}
	}
}
