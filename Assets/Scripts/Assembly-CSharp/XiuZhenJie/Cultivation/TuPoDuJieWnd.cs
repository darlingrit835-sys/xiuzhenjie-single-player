using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class TuPoDuJieWnd : Window
	{
		public TuPoDuJiePanel Panel;

		private List<BaseSkillModel> m_PlayerFaQiModel;

		private GameObject m_ShanDianObj;

		private ObscuredInt m_ShanDianTimes;

		private ObscuredInt m_CurrentShanDianTimes;

		private ObscuredDouble m_ShanDianDamage;

		private bool m_IsCanShanDuo;

		private ObscuredDouble m_HuDunZhiValue;

		private ObscuredDouble m_MaxSuperAir;

		private ObscuredDouble m_CurrentSuperAir;

		private bool m_IsInputFangYu;

		private bool m_IsShanDuoSuccess;

		private BaseSkillModel m_CurrentUsedFaQi;

		private GameObject m_CurrentUsedFaQi_UI;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Queue<ObscuredDouble> m_DamageQue;

		private Dictionary<string, TuPoDuJieZhuangBeiRecord> m_CurrentZhuangBeiJianRenList;

		private Dictionary<string, RoleLevelModel> m_RoleInfo;

		private ObscuredDouble m_CurrentToughTenacity;

		private ObscuredDouble m_CurrentPureBlood;

		private bool m_DuJieEnd;

		private ObscuredDouble m_NextLuoLeiTime;

		private ObscuredDouble m_CurrentLuoLeiTimes;

		private ObscuredDouble m_ShanBiCDTime;

		private int YinDaoShu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<TuPoDuJiePanel>();
			AddButtonClickListener(Panel.XiaoWenHao, WenHaoTogClick);
			AddButtonClickListener(Panel.ShanDuoBtn, ShanDuo);
			ResourceLoad();
			InitData();
			m_NextLuoLeiTime = 6.0;
			m_ShanBiCDTime = 0.0;
			m_DuJieEnd = false;
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				Panel.ZhenYuanTitleText.text = "灵气";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				Panel.ZhenYuanTitleText.text = "真气";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				Panel.ZhenYuanTitleText.text = "真元";
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_DuJieEnd)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = Panel.LeiYunAnimator.GetCurrentAnimatorStateInfo(0);
			if (Panel.LeiYunAnimator.GetBool("IsLuoLei") && currentAnimatorStateInfo.normalizedTime > 0.01f && currentAnimatorStateInfo.IsName("YunLeiDian"))
			{
				Panel.LeiYunAnimator.SetBool("IsLuoLei", value: false);
				m_IsCanShanDuo = false;
				if (!m_IsShanDuoSuccess)
				{
					FaBaoJianShang(ref m_ShanDianDamage);
					if (!ChengShouShangHai(m_ShanDianDamage))
					{
						m_DuJieEnd = true;
						DuJieFail();
						JiSuanFangYu();
						return;
					}
				}
				else
				{
					m_IsShanDuoSuccess = false;
				}
				if ((int)m_CurrentShanDianTimes >= (int)m_ShanDianTimes)
				{
					if ((double)m_CurrentPureBlood <= 0.0)
					{
						DuJieFail();
						m_DuJieEnd = true;
						JiSuanFangYu();
						return;
					}
					m_DuJieEnd = true;
					DuJieSuccess();
					JiSuanFangYu();
				}
			}
			JiSuanFangYu();
			m_CurrentLuoLeiTimes = (double)m_CurrentLuoLeiTimes + (double)Time.deltaTime;
			if ((double)m_CurrentLuoLeiTimes >= (double)m_NextLuoLeiTime)
			{
				m_CurrentLuoLeiTimes = 0.0;
				++m_CurrentShanDianTimes;
				LuoLeiJiSuan();
				m_NextLuoLeiTime = 4.0;
			}
			if ((double)m_ShanBiCDTime > 0.0)
			{
				m_ShanBiCDTime = (double)m_ShanBiCDTime - (double)Time.deltaTime;
				Panel.ShanDuoImg.fillAmount = (float)(double)m_ShanBiCDTime / 5f;
			}
			else if (Panel.ShanDuoImg.gameObject.activeSelf)
			{
				Panel.ShanDuoImg.gameObject.SetActive(value: false);
				Panel.ShanDuoBtn.interactable = true;
			}
		}

		private void ResourceLoad()
		{
			m_RankBgSprite = new Dictionary<int, Sprite>();
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value);
			m_RankBgSprite.Add(2, value2);
			m_RankBgSprite.Add(3, value3);
			m_RankBgSprite.Add(4, value4);
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			m_GoodsIconSprite.Add(15, value5);
			m_GoodsIconSprite.Add(26, value6);
		}

		private void WenHaoTogClick()
		{
			string text = $"修士逆天而行，被天道所不容，强大的修士在突破境界是会遭受天道反噬，天道降下雷劫用以灭杀修士。越是强大的修士，降下的雷劫越多，若渡劫失败，轻则重伤难愈，重则损毁道基，亦可能自此消失于此界。渡劫前做好万全准备是重中之重，如服用破雷丹提升抗雷属性、穿上更强大的防具抵挡雷劫伤害、使用更强大的法宝硬撼雷劫抵消伤害。相传，高阶修士瞬移时触碰到空间法则，在瞬间会处于不同空间，也有胆大的修士利用这一点来完全规避雷劫伤害，但此法凶险，使用时应慎之又慎。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "渡劫");
		}

		private void InitData()
		{
			m_IsCanShanDuo = false;
			m_IsShanDuoSuccess = false;
			m_HuDunZhiValue = 0.0;
			m_ShanDianDamage = 100.0;
			m_DamageQue = new Queue<ObscuredDouble>();
			m_CurrentZhuangBeiJianRenList = new Dictionary<string, TuPoDuJieZhuangBeiRecord>();
			m_RoleInfo = Singleton<FacadeManager>.Instance.SelectRoleLevelInfo();
			m_CurrentSuperAir = Singleton<RoleManager>.Instance.Role_Entity.CurrentSuperAir;
			m_MaxSuperAir = Singleton<RoleManager>.Instance.Role_Entity.SuperAir;
			m_ShanDianObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/TuPo_ShanDian.prefab");
			m_CurrentShanDianTimes = 0;
			InitWeapon();
		}

		private void InitWeapon()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_THUNDER_CALAMITY, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterThunderCalamityResp_1002009 enterThunderCalamityResp_ = new EnterThunderCalamityResp_1002009();
					enterThunderCalamityResp_.MergeFrom(byteString);
					if (enterThunderCalamityResp_.IsSuccess)
					{
						List<BaseSkillModel> list = new List<BaseSkillModel>();
						if (enterThunderCalamityResp_.Skills != null)
						{
							foreach (EnterThunderCalamityResp_1002009.Types.BattleSkillResp_1002010 skill in enterThunderCalamityResp_.Skills)
							{
								if (skill.Type == 2)
								{
									list.Add(new BaseSkillModel(skill));
								}
							}
						}
						if (enterThunderCalamityResp_.ThunderDamage != null)
						{
							foreach (EnterThunderCalamityResp_1002009.Types.ThunderDamageResp_1002011 item in enterThunderCalamityResp_.ThunderDamage)
							{
								m_DamageQue.Enqueue(item.Damage);
							}
						}
						m_PlayerFaQiModel = list;
						m_ShanDianTimes = ((enterThunderCalamityResp_.ThunderDamage != null) ? enterThunderCalamityResp_.ThunderDamage.Count : 0);
						m_CurrentToughTenacity = enterThunderCalamityResp_.CurrentPhysiqueDefense;
						m_CurrentPureBlood = enterThunderCalamityResp_.CurrentPureBloodDefense;
						m_MaxSuperAir = enterThunderCalamityResp_.CurrentSuperAir;
						m_CurrentSuperAir = enterThunderCalamityResp_.CurrentSuperAir;
						m_ShanBiCDTime = enterThunderCalamityResp_.DodgeCoolDown;
						SetFaQi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterThunderCalamityResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					TuPoYinDao(1);
				}
			}, isShowWaitUI: true);
		}

		private int GetLeiJieTimes()
		{
			int nextLevelRoleStage = Singleton<RoleManager>.Instance.GetNextLevelRoleStage();
			int num = (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel + 1) % 10;
			return nextLevelRoleStage * 3 + num / 3;
		}

		private double GetLeiJieDamage()
		{
			double num = 1.0;
			RoleLevelModel value = null;
			int num2 = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel + 1;
			if (m_RoleInfo.TryGetValue(num2.ToString(), out value) && value != null)
			{
				num = value.topLimit * 0.03500000014901161;
			}
			int roleLingGenNum = Singleton<RoleManager>.Instance.GetRoleLingGenNum();
			roleLingGenNum = ((roleLingGenNum == 0) ? 1 : roleLingGenNum);
			float num3 = (float)roleLingGenNum * 0.05f + 0.75f;
			return num * 2.5 * (double)num3 * 10.0 / (double)Mathf.Pow(num2, 1.02f + (float)num2 / (100f + 0.25f * (float)num2));
		}

		private void JiSuanFangYu()
		{
			m_HuDunZhiValue = (double)m_CurrentPureBlood + (double)m_CurrentToughTenacity;
			Panel.FangYuZhiText.text = "防御值:" + m_HuDunZhiValue.ToString("f0");
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				Panel.ZhenYuanValue.text = m_CurrentSuperAir.ToString("f0");
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				Panel.ZhenYuanValue.text = ((double)m_CurrentSuperAir / 100.0).ToString("f0");
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				Panel.ZhenYuanValue.text = ((double)m_CurrentSuperAir / 100.0 / 100.0).ToString("f0");
			}
			Panel.ZhenYuanProgress.fillAmount = (float)((double)m_CurrentSuperAir * 1.0 / (double)m_MaxSuperAir);
		}

		private void SetFaQi()
		{
			if (m_PlayerFaQiModel == null || m_PlayerFaQiModel.Count == 0)
			{
				return;
			}
			for (int i = 0; i < m_PlayerFaQiModel.Count && i <= 3; i++)
			{
				if (m_PlayerFaQiModel[i].type == 2)
				{
					Sprite value = null;
					Sprite value2 = null;
					m_GoodsIconSprite.TryGetValue(m_PlayerFaQiModel[i].skillType, out value2);
					m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(m_PlayerFaQiModel[i].rank), out value);
					UI_GoodsItem uI_GoodsItem = null;
					switch (i)
					{
					case 0:
						uI_GoodsItem = Panel.ZhanDouJiNengItem_1;
						break;
					case 1:
						uI_GoodsItem = Panel.ZhanDouJiNengItem_2;
						break;
					case 2:
						uI_GoodsItem = Panel.ZhanDouJiNengItem_3;
						break;
					case 3:
						uI_GoodsItem = Panel.ZhanDouJiNengItem_4;
						break;
					}
					if (uI_GoodsItem != null)
					{
						uI_GoodsItem.SetItem(m_PlayerFaQiModel[i], m_PlayerFaQiModel[i].name, m_PlayerFaQiModel[i].fiveElement, m_PlayerFaQiModel[i].level, value, value2, UsedWeapon);
					}
				}
			}
		}

		private void MouseUp(object _dataSource, ZhanDouJiNengItem item)
		{
			m_IsInputFangYu = false;
			m_CurrentUsedFaQi = null;
			Panel.XuLiPanel.gameObject.SetActive(value: false);
		}

		private void MouseDown(object _dataSource, ZhanDouJiNengItem item)
		{
		}

		private void UsedWeapon(object data, GameObject gameObj)
		{
			if (!(data is BaseSkillModel baseSkillModel))
			{
				return;
			}
			if (m_CurrentUsedFaQi != null && m_CurrentUsedFaQi_UI != null)
			{
				if (m_CurrentUsedFaQi.id == baseSkillModel.id && m_CurrentUsedFaQi_UI == gameObj)
				{
					return;
				}
				ClearUsing(m_CurrentUsedFaQi_UI);
			}
			if ((double)m_CurrentSuperAir < (double)(long)baseSkillModel.usedSuperAir)
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("灵气不足");
				}
				else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("真气不足");
				}
				else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("真元不足");
				}
				return;
			}
			m_CurrentUsedFaQi = baseSkillModel;
			m_CurrentUsedFaQi_UI = gameObj;
			UI_GoodsItem component = gameObj.GetComponent<UI_GoodsItem>();
			if (component == Panel.ZhanDouJiNengItem_1)
			{
				Panel.LineCDItem_1.gameObject.SetActive(value: true);
			}
			else if (component == Panel.ZhanDouJiNengItem_2)
			{
				Panel.LineCDItem_2.gameObject.SetActive(value: true);
			}
			else if (component == Panel.ZhanDouJiNengItem_3)
			{
				Panel.LineCDItem_3.gameObject.SetActive(value: true);
			}
			else if (component == Panel.ZhanDouJiNengItem_4)
			{
				Panel.LineCDItem_4.gameObject.SetActive(value: true);
			}
		}

		private void ClearUsing(GameObject gameObj)
		{
			m_CurrentUsedFaQi = null;
			m_CurrentUsedFaQi_UI = null;
			Transform transform = gameObj.transform.parent.transform.Find("LineAni");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
		}

		private void LuoLeiJiSuan()
		{
			if (m_ShanDianObj != null)
			{
				m_ShanDianObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/TuPo_ShanDian.prefab");
			}
			m_IsCanShanDuo = true;
			m_ShanDianDamage = m_DamageQue.Dequeue();
			Panel.LeiYunAnimator.SetBool("IsLuoLei", value: true);
			Object.Instantiate(m_ShanDianObj, Panel.transform);
			TuPoYinDao(2);
			Timer.Register((float)GetTime(), () =>
			{
				m_IsCanShanDuo = false;
			});
		}

		private double GetTime()
		{
			int roleStage = Singleton<RoleManager>.Instance.GetRoleStage();
			int num = ((roleStage - 4 <= 0) ? 1 : (roleStage - 4));
			double num2 = 500f * Mathf.Pow(4f, roleStage) / (float)num;
			num2 /= (double)Singleton<RoleManager>.Instance.Role_Entity.CurrentPerception;
			num2 = 4.0 - num2;
			return (num2 < -2.0) ? (-2.0) : (0.20000000298023224 + num2 / 10.0);
		}

		private void FaBaoJianShang(ref ObscuredDouble ShanDianDamage)
		{
			if (m_CurrentUsedFaQi == null)
			{
				return;
			}
			double num = (long)m_CurrentUsedFaQi.usedSuperAir;
			m_CurrentSuperAir = (double)m_CurrentSuperAir - num;
			if (num != 0.0 && m_CurrentUsedFaQi != null)
			{
				ShanDianDamage = (double)ShanDianDamage - (double)m_CurrentUsedFaQi.baseDamage;
				if ((double)ShanDianDamage < 0.0)
				{
					ShanDianDamage = 0.0;
				}
			}
		}

		private bool ChengShouShangHai(double damage)
		{
			double num = damage;
			double num2 = 0.0;
			double num3 = 0.0;
			num -= num2;
			if (m_CurrentZhuangBeiJianRenList != null && m_CurrentZhuangBeiJianRenList.Count != 0)
			{
				foreach (KeyValuePair<string, TuPoDuJieZhuangBeiRecord> currentZhuangBeiJianRen in m_CurrentZhuangBeiJianRenList)
				{
					if (num <= 0.0)
					{
						break;
					}
					if (num >= currentZhuangBeiJianRen.Value.ZhuangBeiFangYu)
					{
						num -= currentZhuangBeiJianRen.Value.ZhuangBeiFangYu;
						num3 += currentZhuangBeiJianRen.Value.ZhuangBeiFangYu;
						currentZhuangBeiJianRen.Value.ZhuangBeiFangYu = 0.0;
					}
					else
					{
						currentZhuangBeiJianRen.Value.ZhuangBeiFangYu -= num;
						num3 += num;
						num = 0.0;
					}
				}
			}
			if (num > 0.0)
			{
				if (num >= (double)m_CurrentToughTenacity)
				{
					num -= (double)m_CurrentToughTenacity;
					num3 += (double)m_CurrentToughTenacity;
					m_CurrentToughTenacity = 0.0;
				}
				else
				{
					m_CurrentToughTenacity = (double)m_CurrentToughTenacity - num;
					num3 += num;
					num = 0.0;
				}
			}
			if (num > 0.0)
			{
				if (num >= (double)m_CurrentPureBlood)
				{
					num -= (double)m_CurrentPureBlood;
					num3 += (double)m_CurrentPureBlood;
					m_CurrentPureBlood = 0.0;
				}
				else
				{
					m_CurrentPureBlood = (double)m_CurrentPureBlood - num;
					num3 += num;
					num = 0.0;
				}
			}
			MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("承受{0}点雷霆伤害", num3.ToString("f0")));
			if (num > 0.0)
			{
				return false;
			}
			return true;
		}

		private void ShanDuo()
		{
			if (m_IsCanShanDuo)
			{
				m_IsShanDuoSuccess = true;
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("成功规避了雷霆伤害！");
			}
			else
			{
				m_IsShanDuoSuccess = false;
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("规避雷霆伤害失败！");
			}
			m_ShanBiCDTime = 5.0;
			Panel.ShanDuoImg.gameObject.SetActive(value: true);
			Panel.ShanDuoBtn.interactable = false;
			Panel.ShanDuoImg.fillAmount = 1f;
		}

		private void DuJieSuccess()
		{
			DuJieParamsModel duJieParamsModel = new DuJieParamsModel();
			duJieParamsModel.a = true;
			duJieParamsModel.b = 0;
			duJieParamsModel.c = new List<RoleDuJieEquipmentData>();
			foreach (KeyValuePair<string, TuPoDuJieZhuangBeiRecord> currentZhuangBeiJianRen in m_CurrentZhuangBeiJianRenList)
			{
				if (currentZhuangBeiJianRen.Value.ZhuangBeiFangYu <= 0.0)
				{
					duJieParamsModel.c.Add(new RoleDuJieEquipmentData
					{
						a = currentZhuangBeiJianRen.Key,
						b = 0.0
					});
				}
			}
			RoleBreakRealmReq_1001010 data = new RoleBreakRealmReq_1001010
			{
				IsThunderSuccess = true
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_BREAK, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleBreakRealmResp_1001010 roleBreakRealmResp_ = new RoleBreakRealmResp_1001010();
					roleBreakRealmResp_.MergeFrom(byteString);
					if (roleBreakRealmResp_.IsSuccess)
					{
						RoleInfoEntity roleInfoEntity = new RoleInfoEntity
						{
							MaxAge = Singleton<RoleManager>.Instance.Role_Entity.MaxAge,
							SuperAir = Singleton<RoleManager>.Instance.Role_Entity.SuperAir,
							Physique = Singleton<RoleManager>.Instance.Role_Entity.Physique,
							Perception = Singleton<RoleManager>.Instance.Role_Entity.Perception,
							PureBlood = Singleton<RoleManager>.Instance.Role_Entity.PureBlood
						};
						Singleton<RoleManager>.Instance.Role_Entity.RoleLevel++;
						Singleton<RoleManager>.Instance.Role_Entity.PureBlood = roleBreakRealmResp_.PureBlood;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPureBlood = roleBreakRealmResp_.CurrentPureBlood;
						Singleton<RoleManager>.Instance.Role_Entity.FlexibleRoot = roleBreakRealmResp_.FlexibleRoot;
						Singleton<RoleManager>.Instance.Role_Entity.SuperAir = roleBreakRealmResp_.SuperAir;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentSuperAir = roleBreakRealmResp_.CurrentSuperAir;
						Singleton<RoleManager>.Instance.Role_Entity.Perception = roleBreakRealmResp_.Perception;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPerception = roleBreakRealmResp_.CurrentPerception;
						Singleton<RoleManager>.Instance.Role_Entity.Physique = roleBreakRealmResp_.Physique;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPhysique = roleBreakRealmResp_.CurrentPhysique;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = roleBreakRealmResp_.CurrentVigor;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = roleBreakRealmResp_.CurrentClever;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPoisonVal = roleBreakRealmResp_.CurrentPoisonVal;
						Singleton<RoleManager>.Instance.Role_Entity.MaxAge = roleBreakRealmResp_.Age;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						Singleton<UIManager>.Instance.PopUpWnd("TuPoDuJieSuccessWnd.prefab", true, roleInfoEntity);
					}
				}
			});
		}

		private void DuJieFail()
		{
			DuJieParamsModel duJieParamsModel = new DuJieParamsModel();
			duJieParamsModel.a = false;
			duJieParamsModel.b = m_CurrentShanDianTimes;
			duJieParamsModel.c = new List<RoleDuJieEquipmentData>();
			foreach (KeyValuePair<string, TuPoDuJieZhuangBeiRecord> currentZhuangBeiJianRen in m_CurrentZhuangBeiJianRenList)
			{
				duJieParamsModel.c.Add(new RoleDuJieEquipmentData
				{
					a = currentZhuangBeiJianRen.Key,
					b = 0.0
				});
			}
			RoleBreakRealmReq_1001010 data = new RoleBreakRealmReq_1001010
			{
				IsThunderSuccess = false,
				FailThunderNum = m_CurrentShanDianTimes
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_BREAK, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleBreakRealmResp_1001010 roleBreakRealmResp_ = new RoleBreakRealmResp_1001010();
					roleBreakRealmResp_.MergeFrom(byteString);
					if (roleBreakRealmResp_.IsSuccess)
					{
						RoleInfoEntity roleInfoEntity = new RoleInfoEntity
						{
							MaxAge = Singleton<RoleManager>.Instance.Role_Entity.MaxAge,
							SuperAir = Singleton<RoleManager>.Instance.Role_Entity.SuperAir,
							Physique = Singleton<RoleManager>.Instance.Role_Entity.Physique,
							Perception = Singleton<RoleManager>.Instance.Role_Entity.Perception,
							PureBlood = Singleton<RoleManager>.Instance.Role_Entity.PureBlood
						};
						Singleton<RoleManager>.Instance.Role_Entity.PureBlood = roleBreakRealmResp_.PureBlood;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPureBlood = roleBreakRealmResp_.CurrentPureBlood;
						Singleton<RoleManager>.Instance.Role_Entity.FlexibleRoot = roleBreakRealmResp_.FlexibleRoot;
						Singleton<RoleManager>.Instance.Role_Entity.SuperAir = roleBreakRealmResp_.SuperAir;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentSuperAir = roleBreakRealmResp_.CurrentSuperAir;
						Singleton<RoleManager>.Instance.Role_Entity.Perception = roleBreakRealmResp_.Perception;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPerception = roleBreakRealmResp_.CurrentPerception;
						Singleton<RoleManager>.Instance.Role_Entity.Physique = roleBreakRealmResp_.Physique;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPhysique = roleBreakRealmResp_.CurrentPhysique;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = roleBreakRealmResp_.CurrentVigor;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = roleBreakRealmResp_.CurrentClever;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPoisonVal = roleBreakRealmResp_.CurrentPoisonVal;
						Singleton<RoleManager>.Instance.Role_Entity.MaxAge = roleBreakRealmResp_.Age;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						Singleton<UIManager>.Instance.PopUpWnd("TuPoDuJieFailWnd.prefab", true, roleInfoEntity);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleBreakRealmResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		public override void OnClose()
		{
			base.OnClose();
		}

		private void TuPoYinDao(int Shu)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 20 && YinDaoShu <= 1)
			{
				if (Shu <= 2)
				{
					Time.timeScale = 0f;
					m_ShanBiCDTime = 0.0;
				}
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = Shu;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 9;
				Singleton<GuideManager>.Instance.ShowGuideNew(9);
				YinDaoShu++;
			}
		}
	}
}
