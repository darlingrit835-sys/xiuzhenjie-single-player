using System.Collections.Generic;
using DG.Tweening;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class BattleWnd : Window
	{
		public BattlePanel Panel;

		private EntityBase m_Target;

		private BattleLifeModel m_Player;

		private SkillGroupType m_CurrentSkillGroupType;

		private GameObject m_SkillGroupItemObj;

		private GameObject m_BuffItem;

		private Dictionary<int, Battle_SkillItem> m_SkillDic;

		private Dictionary<string, Battle_BuffItem> m_BuffIds;

		private Dictionary<int, Sprite> m_SkillIconSprite;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<string, GameObject> m_GoodsDic;

		private bool m_IsMoving;

		private Vector2 m_JoyAngel;

		private XzjBoxEntity m_NaJieBoxEntity;

		private Dictionary<int, int> FuLuCiShu;

		private float verPercent = 1f;

		private float horPercent = 1f;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<BattlePanel>();
			m_NaJieBoxEntity = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			Panel.EtcJoyImg.onMove.AddListener(DrogMove);
			Panel.EtcJoyImg.onMoveStart.AddListener(DrogMoveStart);
			Panel.EtcJoyImg.onMoveEnd.AddListener(DrogMoveEnd);
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.LiKaiKillBossBtn, LiKaiBtnClickBossBtn);
			AddButtonClickListener(Panel.ChangeSkillGroupBtn, ChangeSkillGroup);
			AddButtonClickListener(Panel.ChuangTaSuccessNextFloorBtn, ChuangTaSuccessNextFloorBtnClick);
			AddButtonClickListener(Panel.ChuangTaSuccessNextFloorLiKaiBtn, ChuangTaSuccessNextFloorLiKaiBtnClick);
			AddButtonClickListener(Panel.ChuangTaFailBtn, ChuangTaFailBtnClick);
			AddButtonClickListener(Panel.OpenNaJieBtn, OpenNaJieClick);
			AddButtonClickListener(Panel.CloseNaJieBtn, CloseNaJieClick);
			m_BuffItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UI/Battle_BuffItem.prefab");
			m_SkillGroupItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UI/Battle_SkillItem.prefab");
			m_CurrentSkillGroupType = SkillGroupType.None;
			m_BuffIds = new Dictionary<string, Battle_BuffItem>();
			m_SkillDic = new Dictionary<int, Battle_SkillItem>();
			Panel.MapNameText.text = Singleton<BattleManager>.Instance.m_BattleSceneMgr.GetMapName();
			ResourceLoad();
			FuLuCiShu = new Dictionary<int, int>();
			if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu <= 11)
			{
				Time.timeScale = 0f;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 11;
				Singleton<GuideManager>.Instance.ShowGuideNew(2);
			}
		}

		public void SetPlayerInfo(BattleLifeModel _model)
		{
			m_Player = _model;
			Sprite sprite = null;
			Sprite sprite2 = null;
			Sprite sprite3 = null;
			sprite = (WuXing)_model.AttackFiveElement switch
			{
				WuXing.All => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/WuXing_Zhen.png"), 
				WuXing.Jin => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Jin_Zhen.png"), 
				WuXing.Mu => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Mu_Zhen.png"), 
				WuXing.Shui => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Shui_Zhen.png"), 
				WuXing.Huo => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Huo_Zhen.png"), 
				WuXing.Tu => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Tu_Zhen.png"), 
				_ => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/WuXing_Zhen.png"), 
			};
			switch ((WuXing)_model.AttackFiveElement)
			{
			case WuXing.All:
				sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/WuXing_Ti.png");
				break;
			case WuXing.Jin:
				sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Jin_Ti.png");
				break;
			case WuXing.Mu:
				sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Mu_Ti.png");
				break;
			case WuXing.Shui:
				sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Shui_Ti.png");
				break;
			case WuXing.Huo:
				sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Huo_Ti.png");
				break;
			case WuXing.Tu:
				sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Tu_Ti.png");
				break;
			default:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/WuXing_Zhen.png");
				break;
			}
			sprite3 = (WuXing)_model.AttackFiveElement switch
			{
				WuXing.All => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/WuXing_Shen.png"), 
				WuXing.Jin => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Jin_Shen.png"), 
				WuXing.Mu => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Mu_Shen.png"), 
				WuXing.Shui => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Shui_Shen.png"), 
				WuXing.Huo => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Huo_Shen.png"), 
				WuXing.Tu => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/Tu_Shen.png"), 
				_ => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Battle/WuXing/WuXing_Shen.png"), 
			};
			Panel.ZhenFiveElementIcon.sprite = sprite;
			Panel.TiFiveElementIcon.sprite = sprite2;
			Panel.ShenFiveElementIcon.sprite = sprite3;
			Panel.PlayerTouXiangImg.sprite = Singleton<RoleManager>.Instance.GetCurrentRoleSprite();
			ChangeSkillGroup();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_Player != null)
			{
				UpdatePlayerInfo();
			}
			horPercent = ((Singleton<BattleManager>.Instance.m_BattleSceneMgr == null) ? 0f : Singleton<BattleManager>.Instance.m_BattleSceneMgr.GetPlayerHorizontalPosPercent());
			verPercent = ((Singleton<BattleManager>.Instance.m_BattleSceneMgr == null) ? 0f : Singleton<BattleManager>.Instance.m_BattleSceneMgr.GetPlayerVerticalPercent());
			float x = Panel.SmallMapTran.sizeDelta.x;
			float y = Panel.SmallMapTran.sizeDelta.y;
			Panel.SmallPlayerItem.anchoredPosition = new Vector2(x * horPercent, y * verPercent);
			m_Target = ((Singleton<BattleManager>.Instance.m_BattleSceneMgr == null) ? null : Singleton<BattleManager>.Instance.m_BattleSceneMgr.SelfPlayerEntity.Target);
			if (Singleton<BattleManager>.Instance.m_BattleSceneMgr != null)
			{
				string text = CommonMethod.SecondToMinute((int)Singleton<BattleManager>.Instance.m_BattleSceneMgr.OverTime);
				Panel.OverTimeText.text = text;
				Panel.OverTimeText_AfterKillBoss.text = text;
			}
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_IsMoving)
			{
				Singleton<BattleManager>.Instance.m_BattleSceneMgr?.PlayerMove(m_JoyAngel);
			}
		}

		private void ResourceLoad()
		{
			m_SkillIconSprite = new Dictionary<int, Sprite>();
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			m_GoodsDic = new Dictionary<string, GameObject>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_AreaDamage.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_ContinueDamage.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DefenceSkill.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DownStatus.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_NoMove.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_OneDamage.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_UpStatus.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			m_SkillIconSprite.Add(14, value);
			m_SkillIconSprite.Add(12, value2);
			m_SkillIconSprite.Add(20, value3);
			m_SkillIconSprite.Add(40, value4);
			m_SkillIconSprite.Add(41, value5);
			m_SkillIconSprite.Add(10, value6);
			m_SkillIconSprite.Add(30, value7);
			m_SkillIconSprite.Add(31, value7);
			m_SkillIconSprite.Add(50, value8);
			m_SkillIconSprite.Add(51, value8);
			m_SkillIconSprite.Add(60, value9);
		}

		private void OpenNaJieClick()
		{
			m_NaJieBoxEntity.GetAllGoods(() =>
			{
				foreach (Transform item in Panel.NaJieScrollViewContent)
				{
					Object.Destroy(item.gameObject);
				}
				GetAllNaJieGood();
				DOTween.To(() => Panel.NaJiePanel.anchoredPosition, (Vector2 v) =>
				{
					Panel.NaJiePanel.anchoredPosition = v;
				}, new Vector2(0f, -1.5f), 0.5f);
			});
		}

		private void CloseNaJieClick()
		{
			DOTween.To(() => Panel.NaJiePanel.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJiePanel.anchoredPosition = v;
			}, new Vector2(0f, -522.22f), 0.5f);
		}

		private void GetAllNaJieGood()
		{
			m_GoodsDic.Clear();
			GridLayoutGroup component = Panel.NaJieScrollViewContent.GetComponent<GridLayoutGroup>();
			GameObject obj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			foreach (BaseNaJieGoodEntity goods in m_NaJieBoxEntity.GoodsList)
			{
				if (!goods.IsUsing)
				{
					CreateObjItem(obj, goods);
				}
			}
			SetGrticalListContentHight(m_NaJieBoxEntity.GoodsList.Count, Panel.NaJieScrollViewContent, component);
		}

		private void CreateObjItem(GameObject obj, BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(Panel.NaJieScrollViewContent, obj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(_data.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(_data.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, value2, value, (object _obj, GameObject _gameObj) =>
			{
				GoodsClick(_obj, DiuQiGoods);
			}, isUsing: false, _data.GoodsNum);
			if (!m_GoodsDic.ContainsKey(_data.GoodsId))
			{
				m_GoodsDic.Add(_data.GoodsId, gameObject);
			}
		}

		private void GoodsClick(object dataSource, UnityAction<int, object> XiaoHuiBtnClick)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			NaJieTipModel naJieTipModel = new NaJieTipModel();
			naJieTipModel.JiaGe = baseNaJieGoodEntity.RecyclePrice;
			naJieTipModel.LeftBtnText = "丢弃";
			naJieTipModel.LeftBtnClickEvent = XiaoHuiBtnClick;
			naJieTipModel.MiddleBtnClickEvent = null;
			naJieTipModel.RightBtnClickEvent = LockOrUnLockGoods;
			naJieTipModel.Name = baseNaJieGoodEntity.Name;
			naJieTipModel.ContentList = baseNaJieGoodEntity.ContentList;
			naJieTipModel.type = baseNaJieGoodEntity.Type;
			naJieTipModel.dataSource = baseNaJieGoodEntity;
			naJieTipModel.IsLock = baseNaJieGoodEntity.IsLock;
			Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, naJieTipModel);
		}

		private void DiuQiGoods(int index, object dataSource)
		{
			BaseNaJieGoodEntity _data = dataSource as BaseNaJieGoodEntity;
			if (_data.IsUsing)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("该物品在战斗设置中已设置，无法丢弃");
			}
			else if (_data.IsLock)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("物品已被锁定，无法丢弃");
			}
			else if (_data.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = _data,
					QueRenCallBack = (int num) =>
					{
						DestroyGoods(index, _data, num);
					},
					TitleContnet = "丢弃"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				DestroyGoods(index, _data, 1);
			}
		}

		private void DestroyGoods(int index, BaseNaJieGoodEntity _data, int num)
		{
			DestroyGoodsReq_1004027 destroyGoodsReq_ = new DestroyGoodsReq_1004027();
			destroyGoodsReq_.GoodsId = _data.GoodsId;
			destroyGoodsReq_.Count = num;
			Singleton<NetManager>.Instance.SendMsg(MsgType.DESTROY_GOODS, destroyGoodsReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					DestroyGoodsResp_1004027 destroyGoodsResp_ = new DestroyGoodsResp_1004027();
					destroyGoodsResp_.MergeFrom(byteString);
					if (destroyGoodsResp_.IsSuccess)
					{
						string goodsId = _data.GoodsId;
						if (_data.GoodsNum <= num)
						{
							m_NaJieBoxEntity.RemoveGoods(goodsId);
							if (m_GoodsDic.ContainsKey(goodsId))
							{
								GameObject obj = m_GoodsDic[goodsId];
								m_GoodsDic.Remove(goodsId);
								Object.Destroy(obj);
							}
						}
						else
						{
							m_NaJieBoxEntity.RemoveGoods(goodsId, num);
							m_GoodsDic[goodsId].GetComponent<UI_GoodsItem>()?.ResetNum(num);
						}
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = destroyGoodsResp_.NowCapacity;
					}
					else
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(destroyGoodsResp_.Code));
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
				}
			}, isShowWaitUI: true);
		}

		public void RemoveWuPin(string id)
		{
			m_NaJieBoxEntity.RemoveGoods(id);
			if (m_GoodsDic.ContainsKey(id))
			{
				GameObject obj = m_GoodsDic[id];
				m_GoodsDic.Remove(id);
				Object.Destroy(obj);
			}
		}

		private void LockOrUnLockGoods(int index, object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity.IsLock)
			{
				Singleton<RoleManager>.Instance.RoleBoxDic[10].UnLockOneGoods(baseNaJieGoodEntity.GoodsId, () =>
				{
					NaJieTipWnd naJieTipWnd = Singleton<UIManager>.Instance.FindWndByName<NaJieTipWnd>("NaJieTipWnd.prefab");
					if (naJieTipWnd != null)
					{
						naJieTipWnd.NaJieTipPanel.LeftBtn.interactable = false;
						naJieTipWnd.NaJieTipPanel.RightBtnText.text = "锁定";
					}
				});
				return;
			}
			Singleton<RoleManager>.Instance.RoleBoxDic[10].LockOneGoods(baseNaJieGoodEntity.GoodsId, () =>
			{
				NaJieTipWnd naJieTipWnd = Singleton<UIManager>.Instance.FindWndByName<NaJieTipWnd>("NaJieTipWnd.prefab");
				if (naJieTipWnd != null)
				{
					naJieTipWnd.NaJieTipPanel.LeftBtn.interactable = true;
					naJieTipWnd.NaJieTipPanel.RightBtnText.text = "解锁";
				}
			});
		}

		private void LiKaiBtnClick()
		{
			Time.timeScale = 0f;
			if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu < 16)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("刘老六说地图进度需要打到35%后击杀领主，还是先别离开此地了吧", () =>
				{
					Time.timeScale = 1f;
				}, () =>
				{
					Time.timeScale = 1f;
				});
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否离开战斗?", () =>
			{
				Time.timeScale = 1f;
				Singleton<BattleManager>.Instance.m_BattleSceneMgr.LeaveBattle();
			}, () =>
			{
				Time.timeScale = 1f;
			});
		}

		private void LiKaiBtnClickBossBtn()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否离开战斗?", () =>
			{
				Singleton<BattleManager>.Instance.m_BattleSceneMgr.LeaveBattle();
			});
		}

		private void DrogMoveStart()
		{
			m_IsMoving = true;
		}

		private void DrogMove(Vector2 _rotate)
		{
			m_JoyAngel = _rotate;
		}

		private void DrogMoveEnd()
		{
			m_IsMoving = false;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Panel.EtcJoyImg.OnDownDown.RemoveAllListeners();
			Panel.EtcJoyImg.OnDownUp.RemoveAllListeners();
			Panel.EtcJoyImg.OnDownLeft.RemoveAllListeners();
			Panel.EtcJoyImg.OnDownRight.RemoveAllListeners();
		}

		private float CalculaAngle(float _joyPositionX, float _joyPositionY)
		{
			float num = _joyPositionX * 90f + 90f;
			float num2 = _joyPositionY * 90f + 90f;
			if (num2 < 90f)
			{
				if (num < 90f)
				{
					return 270f + num2;
				}
				if (num > 90f)
				{
					return 180f + (90f - num2);
				}
			}
			return num;
		}

		private void UpdatePlayerInfo()
		{
			if (Singleton<BattleManager>.Instance.m_BattleSceneMgr != null && Singleton<BattleManager>.Instance.m_BattleSceneMgr.SelfPlayerEntity != null)
			{
				Vector2 vector = Singleton<BattleManager>.Instance.m_BattleSceneMgr.SelfPlayerEntity.GetPos();
				Panel.PosText.text = string.Format("{0},{1}", vector.x.ToString("0.0"), vector.y.ToString("0.0"));
			}
			Panel.FangHuValueImg.fillAmount = (float)((double)m_Player.CurrentJianRen / (double)m_Player.Max_JianRen);
			Panel.BloodValueImg.fillAmount = (float)((double)m_Player.CurrentBlood / (double)m_Player.Max_Blood);
			Panel.ZhenYuanValueImg.fillAmount = (float)((double)m_Player.CurrentSuperAir / (double)m_Player.MaxSuperAir);
			Panel.ShenShiValueImg.fillAmount = (float)((double)m_Player.CurrentPerception / (double)m_Player.MaxPerception);
			Panel.FangHuValueText.text = WorldSetting.simplifyNumber((ulong)((double)m_Player.CurrentJianRen + (double)m_Player.CurrentBlood));
			if (m_Player.Level > 0 && m_Player.Level <= 30)
			{
				Panel.ZhenYuanTitleText.text = "灵气:";
				Panel.ZhenYuanValueText.text = WorldSetting.simplifyNumber((ulong)(double)m_Player.CurrentSuperAir);
			}
			else if (m_Player.Level >= 31 && m_Player.Level <= 60)
			{
				Panel.ZhenYuanTitleText.text = "真气:";
				Panel.ZhenYuanValueText.text = WorldSetting.simplifyNumber((ulong)(double)m_Player.CurrentSuperAir / 100);
			}
			else if (m_Player.Level >= 61 && m_Player.Level <= 90)
			{
				Panel.ZhenYuanTitleText.text = "真元:";
				Panel.ZhenYuanValueText.text = WorldSetting.simplifyNumber((ulong)(double)m_Player.CurrentSuperAir / 100 / 100);
			}
			Panel.ShenShiValueText.text = WorldSetting.simplifyNumber((ulong)(double)m_Player.CurrentPerception);
		}

		private void ChangeSkillGroup()
		{
			switch (m_CurrentSkillGroupType)
			{
			case SkillGroupType.None:
				SetFirstSetting();
				m_CurrentSkillGroupType = SkillGroupType.FaShu;
				break;
			case SkillGroupType.FaShu:
				SetSecondSetting();
				m_CurrentSkillGroupType = SkillGroupType.FaQi;
				break;
			case SkillGroupType.FaQi:
				SetThirdSetting();
				m_CurrentSkillGroupType = SkillGroupType.FuLu;
				break;
			case SkillGroupType.FuLu:
				SetFirstSetting();
				m_CurrentSkillGroupType = SkillGroupType.FaShu;
				break;
			}
		}

		private void ReflashSkillGroup()
		{
			switch (m_CurrentSkillGroupType)
			{
			case SkillGroupType.FaShu:
				SetFirstSetting();
				break;
			case SkillGroupType.FaQi:
				SetSecondSetting();
				break;
			case SkillGroupType.FuLu:
				SetThirdSetting();
				break;
			}
		}

		private void ClearSkillGroup()
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform item in Panel.SkillGroupContnet)
			{
				list.Add(item);
			}
			foreach (Transform item2 in list)
			{
				item2.SetParent(Panel.SkillGroupWatingContnet);
			}
		}

		private void SetFaShu(BaseSkillModel BaseSkillModel, int settingId)
		{
			if (BaseSkillModel == null)
			{
				return;
			}
			GameObject gameObject = null;
			if (!m_SkillDic.ContainsKey(settingId))
			{
				gameObject = Object.Instantiate(m_SkillGroupItemObj, Panel.SkillGroupContnet);
				Battle_SkillItem battle_SkillItem = gameObject.GetComponent<Battle_SkillItem>();
				battle_SkillItem.SetItem(BaseSkillModel, m_SkillIconSprite, SkillGroupType.FaShu, (object d, SkillGroupType s) =>
				{
					FaShuClickCallBack(battle_SkillItem, d, s);
				});
				m_SkillDic.Add(settingId, battle_SkillItem);
			}
			else
			{
				gameObject = m_SkillDic[settingId].gameObject;
				gameObject.transform.SetParent(Panel.SkillGroupContnet);
			}
		}

		private void SetFirstSetting()
		{
			ClearSkillGroup();
			for (int i = 0; i < 3; i++)
			{
				if (m_Player.LifeSkills != null && m_Player.LifeSkills.Count >= i + 1)
				{
					SetSettingItem(m_Player.LifeSkills[i], i + 2);
				}
			}
		}

		private void SetSecondSetting()
		{
			ClearSkillGroup();
			for (int i = 3; i < 6; i++)
			{
				if (m_Player.LifeSkills != null && m_Player.LifeSkills.Count >= i + 1)
				{
					SetSettingItem(m_Player.LifeSkills[i], i + 2);
				}
			}
		}

		private void SetThirdSetting()
		{
			ClearSkillGroup();
			for (int i = 6; i < 9; i++)
			{
				if (m_Player.LifeSkills != null && m_Player.LifeSkills.Count >= i + 1)
				{
					SetSettingItem(m_Player.LifeSkills[i], i + 2);
				}
			}
		}

		private void SetSettingItem(BaseSkillModel baseSkillModel, int settingId)
		{
			switch (baseSkillModel.type)
			{
			case 1:
				SetFaShu(baseSkillModel, settingId);
				break;
			case 2:
				SetFaQi(baseSkillModel, settingId);
				break;
			case 3:
				SetFuLu(baseSkillModel, settingId);
				break;
			case 4:
				SetZhenFa(baseSkillModel, settingId);
				break;
			}
		}

		private BaseSkillModel GetFaShu(string id)
		{
			if (m_Player == null)
			{
				return null;
			}
			for (int i = 0; i < m_Player.LifeSkills.Count; i++)
			{
				if (id == m_Player.LifeSkills[i].id && m_Player.LifeSkills[i].type == 1)
				{
					return m_Player.LifeSkills[i];
				}
			}
			return null;
		}

		private BaseSkillModel GetFaQi(string id)
		{
			if (m_Player == null)
			{
				return null;
			}
			for (int i = 0; i < m_Player.LifeSkills.Count; i++)
			{
				if (id == m_Player.LifeSkills[i].id && m_Player.LifeSkills[i].type == 2)
				{
					return m_Player.LifeSkills[i];
				}
			}
			return null;
		}

		private BaseSkillModel GetFuLu(string id)
		{
			if (m_Player == null)
			{
				return null;
			}
			for (int i = 0; i < m_Player.LifeSkills.Count; i++)
			{
				if (id == m_Player.LifeSkills[i].id && m_Player.LifeSkills[i].type == 3)
				{
					return m_Player.LifeSkills[i];
				}
			}
			return null;
		}

		private void FaShuClickCallBack(Battle_SkillItem battle_SkillItem, object _dataSource, SkillGroupType _skillGroupType)
		{
			BaseSkillModel BaseSkillModel = _dataSource as BaseSkillModel;
			if (BaseSkillModel == null)
			{
				return;
			}
			BattleRoleUseSkillReq_8001005 data = new BattleRoleUseSkillReq_8001005
			{
				RoleIndex = m_Player.ObjectIndex,
				SkillIndex = BaseSkillModel.SkillIndex,
				TargetObjectIndex = ((m_Target != null) ? m_Target.BattleModel.ObjectIndex : 0)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_USESKILL, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BattleRoleUseSkillResp_8001005 battleRoleUseSkillResp_ = new BattleRoleUseSkillResp_8001005();
					battleRoleUseSkillResp_.MergeFrom(byteString);
					if (battleRoleUseSkillResp_.IsSuccess)
					{
						Singleton<BattleManager>.Instance.m_BattleSceneMgr?.PlayerFaShuAttack(BaseSkillModel, () =>
						{
							if ((double)BaseSkillModel.coolDown > 0.0)
							{
								battle_SkillItem.SetCD(BaseSkillModel.coolDown);
							}
						});
					}
				}
			}, isShowWaitUI: false, ServerType.BattleServer);
		}

		private void SetFaQi(BaseSkillModel BaseSkillModel, int settingId)
		{
			if (BaseSkillModel == null)
			{
				return;
			}
			GameObject gameObject = null;
			if (!m_SkillDic.ContainsKey(settingId))
			{
				gameObject = Object.Instantiate(m_SkillGroupItemObj, Panel.SkillGroupContnet);
				Battle_SkillItem battle_FaQiItem = gameObject.GetComponent<Battle_SkillItem>();
				battle_FaQiItem.SetItem(BaseSkillModel, m_GoodsIconSprite, SkillGroupType.FaQi, (object d, SkillGroupType s) =>
				{
					FaQiClickCallBack(battle_FaQiItem, d, s);
				});
				if (Singleton<BattleManager>.Instance.m_BattleSceneMgr.FaQiIsFlying(BaseSkillModel.id))
				{
					battle_FaQiItem.SetFaQiRunningAni(isShowAni: true);
				}
				m_SkillDic.Add(settingId, battle_FaQiItem);
			}
			else
			{
				gameObject = m_SkillDic[settingId].gameObject;
				gameObject.transform.SetParent(Panel.SkillGroupContnet);
			}
		}

		private void FaQiClickCallBack(Battle_SkillItem battle_FaQiItem, object _dataSource, SkillGroupType _skillGroupType)
		{
			BaseSkillModel BaseSkillModel = _dataSource as BaseSkillModel;
			if (BaseSkillModel == null)
			{
				return;
			}
			BattleRoleUseSkillReq_8001005 data = new BattleRoleUseSkillReq_8001005
			{
				RoleIndex = m_Player.ObjectIndex,
				SkillIndex = BaseSkillModel.SkillIndex,
				TargetObjectIndex = ((m_Target != null) ? m_Target.BattleModel.ObjectIndex : 0)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_USESKILL, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BattleRoleUseSkillResp_8001005 battleRoleUseSkillResp_ = new BattleRoleUseSkillResp_8001005();
					battleRoleUseSkillResp_.MergeFrom(byteString);
					if (battleRoleUseSkillResp_.IsSuccess)
					{
						Singleton<BattleManager>.Instance.m_BattleSceneMgr.PlayerFaQiAttack(BaseSkillModel, battle_FaQiItem);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(battleRoleUseSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
				}
			}, isShowWaitUI: false, ServerType.BattleServer);
		}

		private void SetFuLu(BaseSkillModel BaseSkillModel, int settingId)
		{
			if (BaseSkillModel != null)
			{
				if (!m_SkillDic.ContainsKey(settingId))
				{
					Battle_SkillItem component = Object.Instantiate(m_SkillGroupItemObj, Panel.SkillGroupContnet).GetComponent<Battle_SkillItem>();
					component.SetItem(BaseSkillModel, m_GoodsIconSprite, SkillGroupType.FuLu, FuLuClickCallBack);
					m_SkillDic.Add(settingId, component);
				}
				else
				{
					m_SkillDic[settingId].gameObject.transform.SetParent(Panel.SkillGroupContnet);
				}
			}
		}

		private void FuLuClickCallBack(object _dataSource, SkillGroupType _skillGroupType)
		{
			BaseSkillModel BaseSkillModel = _dataSource as BaseSkillModel;
			if (BaseSkillModel == null)
			{
				return;
			}
			if (FuLuCiShu.ContainsKey(BaseSkillModel.SkillIndex))
			{
				if (FuLuCiShu[BaseSkillModel.SkillIndex] <= 0)
				{
					string msg = "当前符箓次数已用尽！";
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(msg);
					return;
				}
			}
			else
			{
				FuLuCiShu.Add(BaseSkillModel.SkillIndex, BaseSkillModel.canUseCount);
			}
			BattleRoleUseSkillReq_8001005 data = new BattleRoleUseSkillReq_8001005
			{
				RoleIndex = m_Player.ObjectIndex,
				SkillIndex = BaseSkillModel.SkillIndex,
				TargetObjectIndex = ((m_Target != null) ? m_Target.BattleModel.ObjectIndex : 0)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_USESKILL, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BattleRoleUseSkillResp_8001005 battleRoleUseSkillResp_ = new BattleRoleUseSkillResp_8001005();
					battleRoleUseSkillResp_.MergeFrom(byteString);
					if (battleRoleUseSkillResp_.IsSuccess)
					{
						Singleton<BattleManager>.Instance.m_BattleSceneMgr.PlayerFuLuAttack(BaseSkillModel);
						FuLuCiShu[BaseSkillModel.SkillIndex]--;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(battleRoleUseSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
				}
			}, isShowWaitUI: false, ServerType.BattleServer);
		}

		private void SetZhenFa(BaseSkillModel BaseSkillModel, int settingId)
		{
			if (BaseSkillModel != null)
			{
				if (!m_SkillDic.ContainsKey(settingId))
				{
					Battle_SkillItem component = Object.Instantiate(m_SkillGroupItemObj, Panel.SkillGroupContnet).GetComponent<Battle_SkillItem>();
					component.SetItem(BaseSkillModel, m_GoodsIconSprite, SkillGroupType.ZhenFa, ZhenFaClickCallBack);
					m_SkillDic.Add(settingId, component);
				}
				else
				{
					m_SkillDic[settingId].gameObject.transform.SetParent(Panel.SkillGroupContnet);
				}
			}
		}

		private void ZhenFaClickCallBack(object _dataSource, SkillGroupType _skillGroupType)
		{
			if (!(_dataSource is BaseSkillModel baseSkillModel))
			{
				return;
			}
			if (Singleton<BattleManager>.Instance.m_BattleSceneMgr.SelfPlayerEntity.UsingZhenFa == null)
			{
				if ((int)baseSkillModel.canUseCount <= 0)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("阵法可使用次数耗尽!");
				}
				else
				{
					CheckZhenFaCastShinyStone(baseSkillModel, _skillGroupType);
				}
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("不能运行更多的阵法！");
			}
		}

		private void CheckZhenFaCastShinyStone(BaseSkillModel _dataSource, SkillGroupType _skillGroupType)
		{
			GoodsMatrixUseReq_1004045 data = new GoodsMatrixUseReq_1004045
			{
				SkillIndex = _dataSource.SkillIndex + 1
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_MATRIX_USE, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsMatrixUseResp_1004045 goodsMatrixUseResp_ = new GoodsMatrixUseResp_1004045();
					goodsMatrixUseResp_.MergeFrom(byteString);
					if (goodsMatrixUseResp_.IsSuccess)
					{
						UseZhenFa(_dataSource, _skillGroupType);
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = goodsMatrixUseResp_.ShinyStone;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsMatrixUseResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
				}
			});
		}

		private void UseZhenFa(BaseSkillModel _dataSource, SkillGroupType _skillGroupType)
		{
			BattleRoleUseSkillReq_8001005 data = new BattleRoleUseSkillReq_8001005
			{
				RoleIndex = m_Player.ObjectIndex,
				SkillIndex = _dataSource.SkillIndex,
				TargetObjectIndex = ((m_Target != null) ? m_Target.BattleModel.ObjectIndex : 0)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLE_ROLE_USESKILL, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BattleRoleUseSkillResp_8001005 battleRoleUseSkillResp_ = new BattleRoleUseSkillResp_8001005();
					battleRoleUseSkillResp_.MergeFrom(byteString);
					if (battleRoleUseSkillResp_.IsSuccess)
					{
						Singleton<BattleManager>.Instance.m_BattleSceneMgr.PlayerRunningMatrix(_dataSource);
						_dataSource.canUseCount = 0;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(battleRoleUseSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
				}
			}, isShowWaitUI: false, ServerType.BattleServer);
		}

		public void AddPlayerBuff(string id, int fiveElement, int icon, float time, int skillType)
		{
			Sprite value = null;
			if (!m_SkillIconSprite.TryGetValue(skillType, out value))
			{
				return;
			}
			if (skillType == 20)
			{
				foreach (KeyValuePair<string, Battle_BuffItem> buffId in m_BuffIds)
				{
					if (buffId.Value.skillType == 20)
					{
						CanclePlayerBuff(buffId.Value);
						break;
					}
				}
			}
			if (!m_BuffIds.ContainsKey(id))
			{
				Battle_BuffItem component = SetListContentParent(Panel.Buff_Content, m_BuffItem).GetComponent<Battle_BuffItem>();
				m_BuffIds.Add(id, component);
				component.SetItem(id, skillType, fiveElement, value, time, CanclePlayerBuff);
			}
			else
			{
				m_BuffIds[id].ReflashItem();
			}
		}

		private void CanclePlayerBuff(Battle_BuffItem item)
		{
			Battle_BuffItem battle_BuffItem = null;
			if (m_BuffIds.ContainsKey(item.SkillId))
			{
				battle_BuffItem = m_BuffIds[item.SkillId];
				m_BuffIds.Remove(item.SkillId);
			}
			if (battle_BuffItem != null)
			{
				Object.Destroy(battle_BuffItem.gameObject);
			}
			else
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void CanclePlayerBuff(string id)
		{
			Battle_BuffItem battle_BuffItem = null;
			if (m_BuffIds.ContainsKey(id))
			{
				battle_BuffItem = m_BuffIds[id];
				m_BuffIds.Remove(id);
			}
			if (battle_BuffItem != null)
			{
				Object.Destroy(battle_BuffItem.gameObject);
			}
		}

		public void ClearAllBuff()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Battle_BuffItem> buffId in m_BuffIds)
			{
				list.Add(buffId.Key);
			}
			foreach (string item in list)
			{
				CanclePlayerBuff(item);
			}
		}

		public void CancleFaShuHuDun()
		{
		}

		public void DiaoLuoShow(int rank)
		{
		}

		public void ReflashDeadMonsterNum(float progress)
		{
			Panel.MonsterDieProgressText.text = string.Format("{0}%", (progress * 100f).ToString("0"));
			Panel.MonsterDieProgressBossText.text = string.Format("{0}%", (progress * 100f).ToString("0"));
		}

		public void ChuangTaResult(bool isSuccess)
		{
			if (!(Panel == null) && !(Panel.ChuangTaSuccess == null) && !(Panel.ChuangTaFail == null))
			{
				if (isSuccess)
				{
					Panel.ChuangTaSuccess.gameObject.SetActive(value: true);
				}
				else
				{
					Panel.ChuangTaFail.gameObject.SetActive(value: true);
				}
			}
		}

		public void ChuangTaSuccessNextFloorBtnClick()
		{
			Singleton<BattleManager>.Instance.m_BattleSceneMgr.TongTianTaNextFloor();
			Panel.ChuangTaSuccess.gameObject.SetActive(value: false);
			Panel.MapNameText.text = Singleton<BattleManager>.Instance.m_BattleSceneMgr.GetMapName();
		}

		public void ChuangTaSuccessNextFloorLiKaiBtnClick()
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "FightingScene")
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			}
		}

		public void ChuangTaFailBtnClick()
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "FightingScene")
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			}
		}

		public void QingLiYaoShouCompelete()
		{
			Panel.ContinueClearYaoSou.gameObject.SetActive(value: true);
		}

		private void LiKaiClearYaoSou()
		{
			Singleton<BattleManager>.Instance.m_BattleSceneMgr.LeaveBattle();
		}

		public void ShowBossWarning()
		{
			Panel.BossWarningBg.gameObject.SetActive(value: true);
			Panel.WarningBg.gameObject.SetActive(value: true);
			Animation bossWarningAni = Panel.BossWarningBg.gameObject.GetComponent<Animation>();
			bossWarningAni.Play();
			Timer.Register(3f, () =>
			{
				bossWarningAni.Stop();
				Panel.BossWarningBg.gameObject.SetActive(value: false);
				Panel.WarningBg.gameObject.SetActive(value: false);
			});
		}

		public void KilledBoss()
		{
			Panel.LiKaiKillBossBtn.gameObject.SetActive(value: true);
			Panel.LiKaiBtn.gameObject.SetActive(value: false);
			ZhanDoYinDai();
		}

		private void ZhanDoYinDai()
		{
			if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 15)
			{
				Singleton<GuideManager>.Instance.ShowGuideNew(2);
			}
		}
	}
}
