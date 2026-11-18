using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DongFuWnd : Window
	{
		public DongFuPanel Panel;

		private RuningGongFaZuHeType m_CurrentYunXingGongFa;

		private DongFuEntity m_DataSource;

		private GameObject m_BuffInsObj;

		private Dictionary<DongFuBuffType, DongFuBuffItem> m_BuffDic;

		private bool m_IsTuPoing;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiDongFuBtnClick);
			AddButtonClickListener(Panel.ShengJiBtn, ShengJiBtnClick);
			AddButtonClickListener(Panel.KongLingFaZhenXuanZeBtn, KongLingFaZhenBtnClick);
			AddButtonClickListener(Panel.JuLingFaZhenXuanZeBtn, JuLingFaZhenBtnClick);
			AddToggleClickListener(Panel.LianShenBtn, LianShenBtnClick);
			AddToggleClickListener(Panel.DuanTiBtn, DuanTiBtnClick);
			AddToggleClickListener(Panel.NaQiBtn, NaQiBtnClick);
			AddButtonClickListener(Panel.BiGuanTuPoBtn, BiGuanToPoBtnClick);
			AddButtonClickListener(Panel.LianShenGengHuanBtn, GengHuanLianShenBtnClick);
			AddButtonClickListener(Panel.DuanTiGengHuanBtn, GengHuanDuanTiBtnClick);
			AddButtonClickListener(Panel.NaQiGengHuanBtn, GengHuanNaQiBtnClick);
			AddButtonClickListener(Panel.EnterNeiShiBtn, EnterNeiShi);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			RoleManager instance = Singleton<RoleManager>.Instance;
			instance.KeTuPoActionEvent = (UnityAction)Delegate.Remove(instance.KeTuPoActionEvent, new UnityAction(TuPoTiShi));
			RoleManager instance2 = Singleton<RoleManager>.Instance;
			instance2.KeTuPoActionEvent = (UnityAction)Delegate.Combine(instance2.KeTuPoActionEvent, new UnityAction(TuPoTiShi));
			m_DataSource = new DongFuEntity();
			m_BuffDic = new Dictionary<DongFuBuffType, DongFuBuffItem>();
			m_BuffInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DongFuBuffItem.prefab");
			JingMaiYinDaoPanDuan();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_EXERCISE_ROOM, null, (object _obj) =>
			{
				if (!(Panel == null))
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						EnterExerciseRoomResp_1005008 enterExerciseRoomResp_ = new EnterExerciseRoomResp_1005008();
						enterExerciseRoomResp_.MergeFrom(byteString);
						if (enterExerciseRoomResp_.IsSuccess)
						{
							m_DataSource.SetValue(enterExerciseRoomResp_);
							m_CurrentYunXingGongFa = (RuningGongFaZuHeType)m_DataSource.LocationType;
							Panel.DoubleXiuLianProgressRect.SetRunningGongFaType(m_CurrentYunXingGongFa);
							Init();
							MonoSingleton<DongFuSceneManager>.Instance.XiuXingAnimator.gameObject.SetActive(value: true);
							ReFlashData();
							ReflashTitleValue();
							reflash();
							if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 2 && !Panel.EnterNeiShiBtn.gameObject.activeSelf)
							{
								Panel.EnterNeiShiBtn.gameObject.SetActive(value: true);
							}
							if (Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 10)
							{
								Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
								Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
							}
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterExerciseRoomResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_DataSource != null && !(Panel == null))
			{
				if (m_DataSource.ControllerMartixLessTime > 0.0)
				{
					m_DataSource.ControllerMartixLessTime -= Time.deltaTime;
					Panel.KongLingFaZhenShengYuTimeText.text = string.Format("剩余 <color=white><size=12>{0}</size></color> 小时", (m_DataSource.ControllerMartixLessTime / 60.0 / 60.0).ToString("0.0"));
				}
				if (m_DataSource.GatherMatrixLessTime > 0.0)
				{
					m_DataSource.GatherMatrixLessTime -= Time.deltaTime;
					Panel.JuLingFaZhenShengYuTimeText.text = string.Format("剩余 <color=white><size=12>{0}</size></color> 小时", (m_DataSource.GatherMatrixLessTime / 60.0 / 60.0).ToString("0.0"));
				}
				UpdateLianShenProgress(Time.deltaTime);
				UpdateDuanTiProgress(Time.deltaTime);
				UpdateNaQiProgress(Time.deltaTime);
				reflashTitleData();
			}
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			if (!(Panel == null))
			{
				ShowDanYaoLianHua();
				ReflashBuffIcon();
			}
		}

		private void reflashTitleData()
		{
			if (Panel == null || Panel.OneXiuLianValueText == null || Panel.OneXiuLianProgressText == null || Panel.OneXiuLianProgress == null)
			{
				return;
			}
			switch (m_CurrentYunXingGongFa)
			{
			case RuningGongFaZuHeType.LianShen:
				Panel.OneXiuLianValueText.text = Singleton<RoleManager>.Instance.Role_Entity.Perception.ToString();
				break;
			case RuningGongFaZuHeType.DuanTi:
				Panel.OneXiuLianValueText.text = Singleton<RoleManager>.Instance.Role_Entity.Physique.ToString();
				break;
			case RuningGongFaZuHeType.NaQi:
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					Panel.OneXiuLianValueText.text = Singleton<RoleManager>.Instance.Role_Entity.SuperAir.ToString();
				}
				else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					Panel.OneXiuLianValueText.text = (Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100).ToString();
				}
				else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
				{
					Panel.OneXiuLianValueText.text = (Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100 / 100).ToString();
				}
				double num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir;
				double upgradeByLevelId = Singleton<RoleManager>.Instance.GetUpgradeByLevelId(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel.ToString());
				double num2 = num / upgradeByLevelId;
				Panel.OneXiuLianProgressText.text = string.Format("{0}%", (num2 * 100.0).ToString("0.00"));
				Panel.OneXiuLianProgress.fillAmount = (float)num2;
				break;
			}
			case RuningGongFaZuHeType.None:
				break;
			}
		}

		private void EnterNeiShi()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "NeiShiScene");
		}

		private void WenHaoTogClick()
		{
			string text = $"元婴期之前，纳气、锻体、炼神仅可选择一处修行，当修士铸就元婴，便可开启两处修行。虽可以一心二用，但开启两处修行后修行效率降低至原效率的70%。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "练功房");
		}

		private void Init()
		{
			if (m_CurrentYunXingGongFa != RuningGongFaZuHeType.None)
			{
				switch (m_CurrentYunXingGongFa)
				{
				case RuningGongFaZuHeType.DuanTiNaQi:
					Panel.NaQiBtn.isOn = true;
					Panel.DuanTiBtn.isOn = true;
					Panel.LianShenBtn.isOn = false;
					break;
				case RuningGongFaZuHeType.DuanTiLianShen:
					Panel.NaQiBtn.isOn = false;
					Panel.DuanTiBtn.isOn = true;
					Panel.LianShenBtn.isOn = true;
					break;
				case RuningGongFaZuHeType.LianShenNaQi:
					Panel.NaQiBtn.isOn = true;
					Panel.DuanTiBtn.isOn = false;
					Panel.LianShenBtn.isOn = true;
					break;
				case RuningGongFaZuHeType.NaQi:
					Panel.NaQiBtn.isOn = true;
					Panel.LianShenBtn.isOn = false;
					Panel.DuanTiBtn.isOn = false;
					break;
				case RuningGongFaZuHeType.LianShen:
					Panel.LianShenBtn.isOn = true;
					Panel.NaQiBtn.isOn = false;
					Panel.DuanTiBtn.isOn = false;
					break;
				case RuningGongFaZuHeType.DuanTi:
					Panel.DuanTiBtn.isOn = true;
					Panel.LianShenBtn.isOn = false;
					Panel.NaQiBtn.isOn = false;
					break;
				}
			}
			if (m_DataSource.isCanBreak)
			{
				TuPoTiShi();
			}
		}

		private IEnumerator ShowGuide(GuideType _type, int _index)
		{
			yield return new WaitForEndOfFrame();
			if (Singleton<GuideManager>.Instance.IsCanShowGuide(_type, _index))
			{
				Singleton<GuideManager>.Instance.ShowGuide(_type, _index);
			}
		}

		private void UpdateLianShenProgress(double value)
		{
			float num = Panel.LianShenRunningImg.rectTransform.localEulerAngles.z - (float)value * 10f;
			if (num < -360f)
			{
				num += 360f;
			}
			Panel.LianShenRunningImg.rectTransform.localEulerAngles = new Vector3(0f, 0f, num);
		}

		private void UpdateDuanTiProgress(double value)
		{
			float num = Panel.DuanTiRunningImg.rectTransform.localEulerAngles.z - (float)value * 10f;
			if (num < -360f)
			{
				num += 360f;
			}
			Panel.DuanTiRunningImg.rectTransform.localEulerAngles = new Vector3(0f, 0f, num);
		}

		private void UpdateNaQiProgress(double value)
		{
			float num = Panel.NaQiRunningImg.rectTransform.localEulerAngles.z - (float)value * 10f;
			if (num < -360f)
			{
				num += 360f;
			}
			Panel.NaQiRunningImg.rectTransform.localEulerAngles = new Vector3(0f, 0f, num);
		}

		public void reflash()
		{
			string arg = m_DataSource?.GetGatherMatrixName();
			string arg2 = m_DataSource?.GetControllerMatrixName();
			Panel.JuLingFaZhenXuanZeText.text = $"{arg}";
			Panel.KongLingFaZhenXuanZeText.text = $"{arg2}";
			Panel.LianShenGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PerceptionName) ? "无" : m_DataSource.PerceptionName);
			Panel.LianShenGongFaXiaoLv.text = string.Format("{0}/周天", m_DataSource.PerceptionCircle.ToString("f2"));
			Panel.DuanTiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PhysiqueName) ? "无" : m_DataSource.PhysiqueName);
			Panel.DuanTiGongFaXiaoLv.text = string.Format("{0}/周天", m_DataSource.PhysiqueCircle.ToString("f2"));
			Panel.NaQiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.SuperAirName) ? "无" : m_DataSource.SuperAirName);
			double superAirCircle = m_DataSource.SuperAirCircle;
			string arg3 = "";
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				arg3 = ((!(superAirCircle < 10.0)) ? superAirCircle.ToString("f1") : superAirCircle.ToString("f2"));
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				superAirCircle /= 100.0;
				arg3 = ((!(superAirCircle < 10.0)) ? superAirCircle.ToString("f1") : superAirCircle.ToString("f2"));
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				superAirCircle = superAirCircle / 100.0 / 100.0;
				arg3 = ((!(superAirCircle < 10.0)) ? superAirCircle.ToString("f1") : superAirCircle.ToString("f2"));
			}
			Panel.NaQiGongFaXiaoLv.text = $"{arg3}/周天";
			ReflashBuffIcon();
			ReflashTitleValue();
			ShowDanYaoLianHua();
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31)
			{
				Panel.YuanYinIconImg.gameObject.SetActive(value: true);
			}
		}

		public void AddGatherMatrixTime(long addTimeHour)
		{
			if (m_DataSource != null)
			{
				m_DataSource.GatherMatrixLessTime += addTimeHour * 60 * 60;
				OnShow();
			}
		}

		public void ResetGatherRank(int newRank)
		{
			m_DataSource.GatherMatrixRank = newRank;
			m_DataSource?.GetGatherMatrixName();
		}

		public void ResetGatherMatrixTime(long timeSecond)
		{
			m_DataSource.GatherMatrixLessTime = timeSecond;
		}

		public void AddControllerMatrixTime(long addTimeHour)
		{
			if (m_DataSource != null)
			{
				m_DataSource.ControllerMartixLessTime += addTimeHour * 60 * 60;
				OnShow();
			}
		}

		public void ResetControllerMatrixTime(long timeSecond)
		{
			m_DataSource.GatherMatrixLessTime = timeSecond;
		}

		public void ResetControllerRank(int newRank)
		{
			m_DataSource.ControllerMatrixRank = newRank;
			m_DataSource?.GetControllerMatrixName();
		}

		public void ResetNaQiName(string name)
		{
			m_DataSource.SuperAirName = name;
			Panel.NaQiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.SuperAirName) ? "无" : m_DataSource.SuperAirName);
		}

		public void ResetDuanTiName(string name)
		{
			m_DataSource.PhysiqueName = name;
			Panel.DuanTiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PhysiqueName) ? "无" : m_DataSource.PhysiqueName);
		}

		public void ResetLianShenName(string name)
		{
			m_DataSource.PerceptionName = name;
			Panel.LianShenGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PerceptionName) ? "无" : m_DataSource.PerceptionName);
		}

		public void ResetNaQiXiaoLv(double xiaoLv)
		{
			m_DataSource.SuperAirCircle = xiaoLv;
			double superAirCircle = m_DataSource.SuperAirCircle;
			string arg = "";
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				arg = ((!(superAirCircle < 10.0)) ? superAirCircle.ToString("f1") : superAirCircle.ToString("f2"));
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				superAirCircle /= 100.0;
				arg = ((!(superAirCircle < 10.0)) ? superAirCircle.ToString("f1") : superAirCircle.ToString("f2"));
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				superAirCircle = superAirCircle / 100.0 / 100.0;
				arg = ((!(superAirCircle < 10.0)) ? superAirCircle.ToString("f1") : superAirCircle.ToString("f2"));
			}
			Panel.NaQiGongFaXiaoLv.text = $"{arg}/周天";
		}

		public void ResetDuanTiXiaoLv(double xiaoLv)
		{
			m_DataSource.PhysiqueCircle = xiaoLv;
			Panel.DuanTiGongFaXiaoLv.text = string.Format("{0}/周天", m_DataSource.PhysiqueCircle.ToString("f2"));
		}

		public void ResetLianShenXiaoLv(double xiaoLv)
		{
			m_DataSource.PerceptionCircle = xiaoLv;
			Panel.LianShenGongFaXiaoLv.text = string.Format("{0}/周天", m_DataSource.PerceptionCircle.ToString("f2"));
		}

		private void ReflashBuffIcon()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentPoisonVal > 0)
			{
				if (!m_BuffDic.ContainsKey(DongFuBuffType.DanDu))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(m_BuffInsObj);
					gameObject.transform.SetParent(Panel.BuffContent);
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					DongFuBuffItem component = gameObject.GetComponent<DongFuBuffItem>();
					component.SetItem(DongFuBuffType.DanDu, () =>
					{
						float num = Singleton<RoleManager>.Instance.Role_Entity.CurrentPoisonVal;
						float num2 = 0.0033333334f * (num * num) + 0.01f * num;
						if (num2 < 0f)
						{
							num2 = 0f;
						}
						string arg = $"{Mathf.CeilToInt(num2)}%";
						string textValue = string.Format("当前丹毒为:{0},降低纳气功法效率:{1}", num.ToString("0"), arg);
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					});
					m_BuffDic.Add(DongFuBuffType.DanDu, component);
				}
			}
			else if (m_BuffDic.ContainsKey(DongFuBuffType.DanDu))
			{
				DongFuBuffItem dongFuBuffItem = m_BuffDic[DongFuBuffType.DanDu];
				m_BuffDic.Remove(DongFuBuffType.DanDu);
				UnityEngine.Object.Destroy(dongFuBuffItem.gameObject);
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 10)
			{
				if (!m_BuffDic.ContainsKey(DongFuBuffType.ZhuJi))
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(m_BuffInsObj);
					gameObject2.transform.SetParent(Panel.BuffContent);
					gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
					DongFuBuffItem component2 = gameObject2.GetComponent<DongFuBuffItem>();
					component2.SetItem(DongFuBuffType.ZhuJi, () =>
					{
						int roleLingGenNum = Singleton<RoleManager>.Instance.GetRoleLingGenNum();
						roleLingGenNum = ((roleLingGenNum == 0) ? 1 : roleLingGenNum);
						string textValue = string.Format("当前筑基基础成功率:{0}%,筑基丹提高成功率:{1}%", ((6 - roleLingGenNum) * 20).ToString("0"), m_DataSource.BrokenRate.ToString());
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					});
					m_BuffDic.Add(DongFuBuffType.ZhuJi, component2);
				}
			}
			else if (m_BuffDic.ContainsKey(DongFuBuffType.ZhuJi))
			{
				DongFuBuffItem dongFuBuffItem2 = m_BuffDic[DongFuBuffType.ZhuJi];
				m_BuffDic.Remove(DongFuBuffType.ZhuJi);
				UnityEngine.Object.Destroy(dongFuBuffItem2.gameObject);
			}
			if (m_DataSource.IsBrokenAcupoint)
			{
				if (!m_BuffDic.ContainsKey(DongFuBuffType.JingMai))
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(m_BuffInsObj);
					gameObject3.transform.SetParent(Panel.BuffContent);
					gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
					DongFuBuffItem component3 = gameObject3.GetComponent<DongFuBuffItem>();
					component3.SetItem(DongFuBuffType.JingMai, () =>
					{
						string textValue = "经脉受损，无法获得经脉加成！";
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					});
					m_BuffDic.Add(DongFuBuffType.JingMai, component3);
				}
			}
			else if (m_BuffDic.ContainsKey(DongFuBuffType.JingMai))
			{
				DongFuBuffItem dongFuBuffItem3 = m_BuffDic[DongFuBuffType.JingMai];
				m_BuffDic.Remove(DongFuBuffType.JingMai);
				UnityEngine.Object.Destroy(dongFuBuffItem3.gameObject);
			}
			if (m_DataSource.BrokenRate > 0 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 20)
			{
				if (!m_BuffDic.ContainsKey(DongFuBuffType.DuJie))
				{
					GameObject gameObject4 = UnityEngine.Object.Instantiate(m_BuffInsObj);
					gameObject4.transform.SetParent(Panel.BuffContent);
					gameObject4.transform.localScale = new Vector3(1f, 1f, 1f);
					DongFuBuffItem component4 = gameObject4.GetComponent<DongFuBuffItem>();
					component4.SetItem(DongFuBuffType.DuJie, () =>
					{
						string textValue = $"当前渡劫减少雷劫伤害:{m_DataSource.BrokenRate.ToString()}%";
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					});
					m_BuffDic.Add(DongFuBuffType.DuJie, component4);
				}
			}
			else if (m_BuffDic.ContainsKey(DongFuBuffType.DuJie))
			{
				DongFuBuffItem dongFuBuffItem4 = m_BuffDic[DongFuBuffType.DuJie];
				m_BuffDic.Remove(DongFuBuffType.DuJie);
				UnityEngine.Object.Destroy(dongFuBuffItem4.gameObject);
			}
			if (m_DataSource.IsBrokenBody)
			{
				if (!m_BuffDic.ContainsKey(DongFuBuffType.DuJieShiBai))
				{
					GameObject gameObject5 = UnityEngine.Object.Instantiate(m_BuffInsObj);
					gameObject5.transform.SetParent(Panel.BuffContent);
					gameObject5.transform.localScale = new Vector3(1f, 1f, 1f);
					DongFuBuffItem component5 = gameObject5.GetComponent<DongFuBuffItem>();
					component5.SetItem(DongFuBuffType.DuJieShiBai, () =>
					{
						LeiJieXiangQingShow();
					});
					m_BuffDic.Add(DongFuBuffType.DuJieShiBai, component5);
				}
			}
			else if (m_BuffDic.ContainsKey(DongFuBuffType.DuJieShiBai))
			{
				DongFuBuffItem dongFuBuffItem5 = m_BuffDic[DongFuBuffType.DuJieShiBai];
				m_BuffDic.Remove(DongFuBuffType.DuJieShiBai);
				UnityEngine.Object.Destroy(dongFuBuffItem5.gameObject);
			}
		}

		private void LeiJieXiangQingShow()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.BROKEN_STATE, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BrokenStateResp_1005012 brokenStateResp_ = new BrokenStateResp_1005012();
					brokenStateResp_.MergeFrom(byteString);
					if (brokenStateResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZhongShangShowWnd.prefab", true, brokenStateResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(brokenStateResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void TuPoTiShi()
		{
			Panel.BiGuanTuPoBtn.interactable = true;
		}

		private void ReflashTitleValue()
		{
			if (!m_IsTuPoing)
			{
				_ = Singleton<RoleManager>.Instance.Role_Entity.SuperAir;
				Singleton<RoleManager>.Instance.GetUpgradeByLevelId(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel.ToString());
				switch (m_CurrentYunXingGongFa)
				{
				case RuningGongFaZuHeType.LianShen:
					Panel.OneXiuLianTitleText.text = "炼神";
					Panel.OneXiuLianProgressText.gameObject.SetActive(value: false);
					Panel.OneXiuLianProgress.gameObject.SetActive(value: false);
					Panel.LianShenRunningImg.gameObject.SetActive(value: true);
					Panel.NaQiRunningImg.gameObject.SetActive(value: false);
					Panel.DuanTiRunningImg.gameObject.SetActive(value: false);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: false);
					ShowOrHideDoubleProgressPanel(ShowDouble: false);
					break;
				case RuningGongFaZuHeType.DuanTi:
					Panel.OneXiuLianTitleText.text = "锻体";
					Panel.OneXiuLianProgressText.gameObject.SetActive(value: false);
					Panel.OneXiuLianProgress.gameObject.SetActive(value: false);
					Panel.LianShenRunningImg.gameObject.SetActive(value: false);
					Panel.NaQiRunningImg.gameObject.SetActive(value: false);
					Panel.DuanTiRunningImg.gameObject.SetActive(value: true);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: false);
					ShowOrHideDoubleProgressPanel(ShowDouble: false);
					break;
				case RuningGongFaZuHeType.NaQi:
					Panel.OneXiuLianTitleText.text = "纳气";
					Panel.OneXiuLianProgress.gameObject.SetActive(value: true);
					Panel.OneXiuLianProgressText.gameObject.SetActive(value: true);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: true);
					ShowOrHideDoubleProgressPanel(ShowDouble: false);
					Panel.LianShenRunningImg.gameObject.SetActive(value: false);
					Panel.NaQiRunningImg.gameObject.SetActive(value: true);
					Panel.DuanTiRunningImg.gameObject.SetActive(value: false);
					break;
				case RuningGongFaZuHeType.DuanTiLianShen:
					ShowOrHideDoubleProgressPanel(ShowDouble: true);
					Panel.LianShenRunningImg.gameObject.SetActive(value: true);
					Panel.NaQiRunningImg.gameObject.SetActive(value: false);
					Panel.DuanTiRunningImg.gameObject.SetActive(value: true);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: false);
					break;
				case RuningGongFaZuHeType.DuanTiNaQi:
					ShowOrHideDoubleProgressPanel(ShowDouble: true);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: true);
					Panel.LianShenRunningImg.gameObject.SetActive(value: false);
					Panel.NaQiRunningImg.gameObject.SetActive(value: true);
					Panel.DuanTiRunningImg.gameObject.SetActive(value: true);
					break;
				case RuningGongFaZuHeType.LianShenNaQi:
					ShowOrHideDoubleProgressPanel(ShowDouble: true);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: true);
					Panel.LianShenRunningImg.gameObject.SetActive(value: true);
					Panel.NaQiRunningImg.gameObject.SetActive(value: true);
					Panel.DuanTiRunningImg.gameObject.SetActive(value: false);
					break;
				case RuningGongFaZuHeType.None:
					break;
				}
			}
		}

		private void ShowDanYaoLianHua()
		{
			if (!(Panel == null) && !(Panel.LianHuanImg == null) && !(Panel.LianShenImg == null) && !(Panel.NaQiImg == null) && !(Panel.DuanTiImg == null))
			{
				switch ((GongFaType)m_DataSource.RefiningType)
				{
				case GongFaType.None:
					Panel.LianHuanImg.gameObject.SetActive(value: false);
					Panel.LianShenImg.gameObject.SetActive(value: false);
					Panel.NaQiImg.gameObject.SetActive(value: false);
					Panel.DuanTiImg.gameObject.SetActive(value: false);
					break;
				case GongFaType.LianShen:
					Panel.LianHuanImg.gameObject.SetActive(value: true);
					Panel.NaQiImg.gameObject.SetActive(value: false);
					Panel.DuanTiImg.gameObject.SetActive(value: false);
					Panel.LianShenImg.gameObject.SetActive(value: true);
					Panel.ShengYuTimeText.text = m_DataSource.RefiningValue.ToString();
					break;
				case GongFaType.DuanTi:
					Panel.LianHuanImg.gameObject.SetActive(value: true);
					Panel.NaQiImg.gameObject.SetActive(value: false);
					Panel.DuanTiImg.gameObject.SetActive(value: true);
					Panel.LianShenImg.gameObject.SetActive(value: false);
					Panel.ShengYuTimeText.text = m_DataSource.RefiningValue.ToString();
					break;
				case GongFaType.NaQi:
					Panel.LianHuanImg.gameObject.SetActive(value: true);
					Panel.NaQiImg.gameObject.SetActive(value: true);
					Panel.DuanTiImg.gameObject.SetActive(value: false);
					Panel.LianShenImg.gameObject.SetActive(value: false);
					Panel.ShengYuTimeText.text = m_DataSource.RefiningValue.ToString();
					break;
				}
			}
		}

		public void ReflashDanYaoLianHua(GongFaType gongFaType, long value)
		{
			switch (gongFaType)
			{
			case GongFaType.LianShen:
				if (Panel.LianShenImg.gameObject.activeSelf)
				{
					Panel.ShengYuTimeText.text = value.ToString();
					m_DataSource.RefiningValue = value;
				}
				break;
			case GongFaType.DuanTi:
				if (Panel.DuanTiImg.gameObject.activeSelf)
				{
					Panel.ShengYuTimeText.text = value.ToString();
					m_DataSource.RefiningValue = value;
				}
				break;
			case GongFaType.NaQi:
				if (Panel.NaQiImg.gameObject.activeSelf)
				{
					Panel.ShengYuTimeText.text = value.ToString();
					m_DataSource.RefiningValue = value;
				}
				break;
			}
		}

		public void ReflashGongFaValue(GongFaType gongFaType, double value)
		{
			switch (gongFaType)
			{
			case GongFaType.LianShen:
				Panel.LianShenGongFaXiaoLv.text = string.Format("{0}/周天", value.ToString("f2"));
				break;
			case GongFaType.DuanTi:
				Panel.DuanTiGongFaXiaoLv.text = string.Format("{0}/周天", value.ToString("f2"));
				break;
			case GongFaType.NaQi:
			{
				int roleLevel = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
				string text = "";
				if (roleLevel < 31)
				{
					text = ((!(value < 10.0)) ? value.ToString("f1") : value.ToString("f2"));
					Panel.NaQiGongFaXiaoLv.text = $"{text}/周天";
				}
				else if (roleLevel < 61)
				{
					value /= 100.0;
					text = ((!(value < 10.0)) ? value.ToString("f1") : value.ToString("f2"));
					Panel.NaQiGongFaXiaoLv.text = $"{text}/周天";
				}
				else if (roleLevel < 91)
				{
					value = value / 100.0 / 100.0;
					if (value < 10.0)
					{
						text = value.ToString("f2");
					}
					else
					{
						text = value.ToString("f1");
					}
					Panel.NaQiGongFaXiaoLv.text = string.Format("{0}/周天", (value / 100.0 / 100.0).ToString("f2"));
				}
				break;
			}
			}
		}

		private void ShowOrHideDoubleProgressPanel(bool ShowDouble)
		{
			if (Panel.DoubleXiuLianProgressRect.gameObject.activeSelf != ShowDouble)
			{
				Panel.DoubleXiuLianProgressRect.gameObject.SetActive(ShowDouble);
				Panel.OneXiuLianProgressPanel.gameObject.SetActive(!ShowDouble);
			}
		}

		private void DiHuoShiBtnClick()
		{
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DiHuoGe_LianDan", DiHuoGeEnterType.DongFu);
		}

		private void JingShiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("JingShiWnd.prefab", true);
		}

		public void BiGuanToPoBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_BREAK_REALM, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleBreakAskResp_1001008 roleBreakAskResp_ = new RoleBreakAskResp_1001008();
					roleBreakAskResp_.MergeFrom(byteString);
					if (roleBreakAskResp_.IsSuccess)
					{
						NaQiTuPo(roleBreakAskResp_.HasThunder);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleBreakAskResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void NaQiTuPo(int LeiDianCount)
		{
			if (LeiDianCount == 0)
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 10)
				{
					int roleLingGenNum = Singleton<RoleManager>.Instance.GetRoleLingGenNum();
					double num = 0.0;
					num = (6 - roleLingGenNum) * 20 + m_DataSource.BrokenRate;
					num = ((num > 100.0) ? 100.0 : num);
					if (num < 100.0)
					{
						string textValue = string.Format("当前筑基成功率为:{0}%,如不服用筑基丹(玄天楼有售),可能突破失败,是否强行突破?", num.ToString("0"));
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue, () =>
						{
							TuPoConnect();
						});
					}
					else
					{
						string textValue2 = string.Format("当前筑基成功率为:{0}%,是否突破?", num.ToString("0"));
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue2, () =>
						{
							TuPoConnect();
						});
					}
				}
				else
				{
					TuPoConnect();
				}
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel == 20)
			{
				FirstDuJie(LeiDianCount);
			}
			else
			{
				string textValue3 = $"本次突破为雷劫突破,中途不可退出,突破失败会遭受重创,严重时可能损毁道基。若无把握，可去玄天楼购买渡劫丹。本次雷劫预计为{LeiDianCount}道雷霆,请慎重考虑！";
				MonoSingleton<GameStart>.Instance.PopUpAttention(textValue3, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "TuPoScene");
				});
			}
		}

		private void FirstDuJie(int LeiDianCount)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.Search_IsSetting_BattleEquipment, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchIsSettingBattleEquipmentResp_1002015 searchIsSettingBattleEquipmentResp_ = new SearchIsSettingBattleEquipmentResp_1002015();
					searchIsSettingBattleEquipmentResp_.MergeFrom(byteString);
					if (searchIsSettingBattleEquipmentResp_.IsSuccess)
					{
						if (searchIsSettingBattleEquipmentResp_.IsSettingEquipmentBattle)
						{
							DuJIeYinDao(LeiDianCount);
						}
						else
						{
							string textValue = $"天劫威力强大，为避免渡劫失败，请先装备好法器！";
							MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchIsSettingBattleEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DuJIeYinDao(int LeiDianCount)
		{
			if (m_DataSource.BrokenRate == 0)
			{
				string textValue = $"天劫威力强大，为避免渡劫失败，请先至玄天楼购买二阶渡劫丹并服用";
				MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
				return;
			}
			string textValue2 = $"本次雷劫预计为{LeiDianCount}道雷霆,等级越高受到的雷劫次数越多，且雷劫伤害将一次比一次更强，定要小心";
			MonoSingleton<GameStart>.Instance.PopUpAttention(textValue2, () =>
			{
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "TuPoScene");
			});
		}

		private void TuPoConnect()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_BREAK, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleBreakRealmResp_1001010 roleBreakRealmResp_ = new RoleBreakRealmResp_1001010();
					roleBreakRealmResp_.MergeFrom(byteString);
					RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
					role_Entity.PureBlood = roleBreakRealmResp_.PureBlood;
					role_Entity.CurrentPureBlood = roleBreakRealmResp_.CurrentPureBlood;
					role_Entity.FlexibleRoot = roleBreakRealmResp_.FlexibleRoot;
					role_Entity.SuperAir = roleBreakRealmResp_.SuperAir;
					role_Entity.CurrentSuperAir = roleBreakRealmResp_.CurrentSuperAir;
					role_Entity.Perception = roleBreakRealmResp_.Perception;
					role_Entity.CurrentPerception = roleBreakRealmResp_.CurrentPerception;
					role_Entity.Physique = roleBreakRealmResp_.Physique;
					role_Entity.CurrentPhysique = roleBreakRealmResp_.CurrentPhysique;
					role_Entity.CurrentVigor = roleBreakRealmResp_.CurrentVigor;
					role_Entity.CurrentClever = roleBreakRealmResp_.CurrentClever;
					role_Entity.CurrentPoisonVal = roleBreakRealmResp_.CurrentPoisonVal;
					role_Entity.MaxAge = roleBreakRealmResp_.Age;
					role_Entity.CurrentPower = roleBreakRealmResp_.CurrentPower;
					if (roleBreakRealmResp_.IsSuccess)
					{
						role_Entity.RoleLevel++;
						TuPoSuccess();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleBreakRealmResp_.Code);
						TuPoFail(hintMsgByCode);
					}
				}
			});
		}

		private void TuPoSuccess()
		{
			m_IsTuPoing = true;
			Panel.BiGuanTuPoBtn.interactable = false;
			Panel.BottomBg.gameObject.SetActive(value: false);
			Panel.BiGuanTuPoBtn.gameObject.SetActive(value: false);
			Panel.EnterNeiShiBtn.gameObject.SetActive(value: false);
			Singleton<UIManager>.Instance.HideWnd(this);
			if (!Panel.EnterNeiShiBtn.gameObject.activeSelf)
			{
				Panel.EnterNeiShiBtn.gameObject.SetActive(value: true);
			}
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			MonoSingleton<GameStart>.Instance.StartCoroutine(ProgressRunningIE(() =>
			{
				MonoSingleton<DongFuSceneManager>.Instance.EndTuPo();
				Timer.Register(1f, () =>
				{
					Singleton<UIManager>.Instance.ShowWnd(this, false);
					m_IsTuPoing = false;
					Panel.BottomBg.gameObject.SetActive(value: true);
					Panel.EnterNeiShiBtn.gameObject.SetActive(value: true);
					Panel.BiGuanTuPoBtn.gameObject.SetActive(value: true);
					JingMaiYinDaoPanDuan();
					OnShow();
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				});
			}));
		}

		private void TuPoFail(string content)
		{
			m_IsTuPoing = true;
			Panel.BiGuanTuPoBtn.interactable = false;
			Panel.BottomBg.gameObject.SetActive(value: false);
			Panel.EnterNeiShiBtn.gameObject.SetActive(value: false);
			Singleton<UIManager>.Instance.HideWnd(this);
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			MonoSingleton<GameStart>.Instance.StartCoroutine(ProgressRunningIE(() =>
			{
				MonoSingleton<DongFuSceneManager>.Instance.EndTuPo();
				Singleton<UIManager>.Instance.ShowWnd(this, false);
				m_IsTuPoing = false;
				Panel.BottomBg.gameObject.SetActive(value: true);
				Panel.EnterNeiShiBtn.gameObject.SetActive(value: true);
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				ReFlashData();
				MonoSingleton<GameStart>.Instance.PopUpAttention(content);
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			}));
		}

		private IEnumerator ProgressRunningIE(UnityAction callBackOne)
		{
			MonoSingleton<DongFuSceneManager>.Instance.StartTuPo();
			float progress = 0f;
			while (true)
			{
				progress += 0.01f;
				if (progress >= 1f)
				{
					break;
				}
				yield return new WaitForSeconds(0.01f);
			}
			callBackOne?.Invoke();
		}

		private void NaQiBtnClick(bool isOn)
		{
			if (isOn)
			{
				if (string.IsNullOrEmpty(m_DataSource.SuperAirName))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请先选择纳气功法");
					Panel.NaQiBtn.isOn = false;
					return;
				}
				int newLocation = 3;
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31)
				{
					switch (m_CurrentYunXingGongFa)
					{
					case RuningGongFaZuHeType.DuanTi:
						newLocation = 4;
						break;
					case RuningGongFaZuHeType.LianShen:
						newLocation = 6;
						break;
					case RuningGongFaZuHeType.NaQi:
					case RuningGongFaZuHeType.DuanTiNaQi:
					case RuningGongFaZuHeType.LianShenNaQi:
						return;
					case RuningGongFaZuHeType.DuanTiLianShen:
						MonoSingleton<GameStart>.Instance.PopUpAttention("最多同时运行两种类型功法！");
						Panel.NaQiBtn.isOn = false;
						return;
					}
				}
				else if (m_CurrentYunXingGongFa == RuningGongFaZuHeType.NaQi)
				{
					Panel.NaQiBtn.isOn = true;
					return;
				}
				Panel.BiGuanTuPoBtn.gameObject.SetActive(value: true);
				ChangeRunLocationReq_1001007 data = new ChangeRunLocationReq_1001007
				{
					NewLocation = newLocation
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				return;
			}
			switch (m_CurrentYunXingGongFa)
			{
			case RuningGongFaZuHeType.NaQi:
				Panel.NaQiBtn.isOn = true;
				break;
			case RuningGongFaZuHeType.DuanTiNaQi:
			{
				ChangeRunLocationReq_1001007 data3 = new ChangeRunLocationReq_1001007
				{
					NewLocation = 2
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data3, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				break;
			}
			case RuningGongFaZuHeType.LianShenNaQi:
			{
				ChangeRunLocationReq_1001007 data2 = new ChangeRunLocationReq_1001007
				{
					NewLocation = 1
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data2, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				break;
			}
			case RuningGongFaZuHeType.DuanTiLianShen:
				break;
			}
		}

		private void GengHuanNaQiBtnClick()
		{
			ChangeAchievementPanelReq_1005009 data = new ChangeAchievementPanelReq_1005009
			{
				AchievementType = 3
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHANGE_ACHIEVEMENT_PANEL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChangeAchievementPanelResp_1005009 changeAchievementPanelResp_ = new ChangeAchievementPanelResp_1005009();
					changeAchievementPanelResp_.MergeFrom(byteString);
					if (changeAchievementPanelResp_.IsSuccess)
					{
						GongFaEntity gongFaEntity = null;
						if (changeAchievementPanelResp_.CurrentAchievement != null)
						{
							gongFaEntity = new GongFaEntity(changeAchievementPanelResp_.CurrentAchievement);
						}
						List<GongFaEntity> list = new List<GongFaEntity>();
						if (changeAchievementPanelResp_.Achievements != null)
						{
							foreach (ChangeAchievementPanelResp_1005009.Types.AchievementObjectResp_1005010 achievement in changeAchievementPanelResp_.Achievements)
							{
								list.Add(new GongFaEntity(achievement));
							}
						}
						if (gongFaEntity != null && !string.IsNullOrEmpty(gongFaEntity.Id))
						{
							list.Add(gongFaEntity);
						}
						if (list.Count == 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无已参悟的纳气功法，请先参悟功法");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("GengHuanGongFaWnd.prefab", true, gongFaEntity, list, GongFaType.NaQi, m_CurrentYunXingGongFa);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(changeAchievementPanelResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DuanTiBtnClick(bool isOn)
		{
			if (isOn)
			{
				if (string.IsNullOrEmpty(m_DataSource.PhysiqueName))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请先选择锻体功法");
					Panel.DuanTiBtn.isOn = false;
					return;
				}
				int newLocation = 2;
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31)
				{
					switch (m_CurrentYunXingGongFa)
					{
					case RuningGongFaZuHeType.NaQi:
						newLocation = 4;
						break;
					case RuningGongFaZuHeType.LianShen:
						newLocation = 5;
						break;
					case RuningGongFaZuHeType.DuanTi:
					case RuningGongFaZuHeType.DuanTiNaQi:
					case RuningGongFaZuHeType.DuanTiLianShen:
						return;
					case RuningGongFaZuHeType.LianShenNaQi:
						MonoSingleton<GameStart>.Instance.PopUpAttention("最多同时运行两种类型功法！");
						Panel.DuanTiBtn.isOn = false;
						return;
					}
				}
				else if (m_CurrentYunXingGongFa == RuningGongFaZuHeType.DuanTi)
				{
					Panel.DuanTiBtn.isOn = true;
					return;
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					Panel.BiGuanTuPoBtn.interactable = false;
				}
				ChangeRunLocationReq_1001007 data = new ChangeRunLocationReq_1001007
				{
					NewLocation = newLocation
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				return;
			}
			switch (m_CurrentYunXingGongFa)
			{
			case RuningGongFaZuHeType.DuanTi:
				Panel.DuanTiBtn.isOn = true;
				break;
			case RuningGongFaZuHeType.DuanTiNaQi:
			{
				ChangeRunLocationReq_1001007 data3 = new ChangeRunLocationReq_1001007
				{
					NewLocation = 3
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data3, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				break;
			}
			case RuningGongFaZuHeType.DuanTiLianShen:
			{
				ChangeRunLocationReq_1001007 data2 = new ChangeRunLocationReq_1001007
				{
					NewLocation = 1
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data2, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				break;
			}
			case RuningGongFaZuHeType.NaQi:
				break;
			}
		}

		private void GengHuanDuanTiBtnClick()
		{
			ChangeAchievementPanelReq_1005009 data = new ChangeAchievementPanelReq_1005009
			{
				AchievementType = 2
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHANGE_ACHIEVEMENT_PANEL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChangeAchievementPanelResp_1005009 changeAchievementPanelResp_ = new ChangeAchievementPanelResp_1005009();
					changeAchievementPanelResp_.MergeFrom(byteString);
					if (changeAchievementPanelResp_.IsSuccess)
					{
						GongFaEntity gongFaEntity = new GongFaEntity(changeAchievementPanelResp_.CurrentAchievement);
						List<GongFaEntity> list = new List<GongFaEntity>();
						if (changeAchievementPanelResp_.Achievements != null)
						{
							foreach (ChangeAchievementPanelResp_1005009.Types.AchievementObjectResp_1005010 achievement in changeAchievementPanelResp_.Achievements)
							{
								list.Add(new GongFaEntity(achievement));
							}
						}
						if (gongFaEntity != null && !string.IsNullOrEmpty(gongFaEntity.Id))
						{
							list.Add(gongFaEntity);
						}
						if (list.Count == 0 && string.IsNullOrEmpty(gongFaEntity.Id))
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无已参悟的锻体功法，请先参悟功法");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("GengHuanGongFaWnd.prefab", true, gongFaEntity, list, GongFaType.DuanTi, m_CurrentYunXingGongFa);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(changeAchievementPanelResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LianShenBtnClick(bool isOn)
		{
			if (isOn)
			{
				if (string.IsNullOrEmpty(m_DataSource.PerceptionName))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请先选择炼神功法");
					Panel.LianShenBtn.isOn = false;
					return;
				}
				int newLocation = 1;
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31)
				{
					switch (m_CurrentYunXingGongFa)
					{
					case RuningGongFaZuHeType.NaQi:
						newLocation = 6;
						break;
					case RuningGongFaZuHeType.DuanTi:
						newLocation = 5;
						break;
					case RuningGongFaZuHeType.LianShen:
					case RuningGongFaZuHeType.DuanTiLianShen:
					case RuningGongFaZuHeType.LianShenNaQi:
						return;
					case RuningGongFaZuHeType.DuanTiNaQi:
						MonoSingleton<GameStart>.Instance.PopUpAttention("最多同时运行两种类型功法！");
						Panel.LianShenBtn.isOn = false;
						return;
					}
				}
				else
				{
					if (m_CurrentYunXingGongFa == RuningGongFaZuHeType.LianShen)
					{
						Panel.LianShenBtn.isOn = true;
						return;
					}
					Panel.BiGuanTuPoBtn.interactable = false;
				}
				ChangeRunLocationReq_1001007 data = new ChangeRunLocationReq_1001007
				{
					NewLocation = newLocation
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				return;
			}
			switch (m_CurrentYunXingGongFa)
			{
			case RuningGongFaZuHeType.LianShen:
				Panel.LianShenBtn.isOn = true;
				break;
			case RuningGongFaZuHeType.LianShenNaQi:
			{
				ChangeRunLocationReq_1001007 data3 = new ChangeRunLocationReq_1001007
				{
					NewLocation = 3
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data3, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				break;
			}
			case RuningGongFaZuHeType.DuanTiLianShen:
			{
				ChangeRunLocationReq_1001007 data2 = new ChangeRunLocationReq_1001007
				{
					NewLocation = 2
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_LOCATION, data2, (object _obj) =>
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
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				break;
			}
			}
		}

		private void GengHuanLianShenBtnClick()
		{
			ChangeAchievementPanelReq_1005009 data = new ChangeAchievementPanelReq_1005009
			{
				AchievementType = 1
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHANGE_ACHIEVEMENT_PANEL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChangeAchievementPanelResp_1005009 changeAchievementPanelResp_ = new ChangeAchievementPanelResp_1005009();
					changeAchievementPanelResp_.MergeFrom(byteString);
					if (changeAchievementPanelResp_.IsSuccess)
					{
						GongFaEntity gongFaEntity = new GongFaEntity(changeAchievementPanelResp_.CurrentAchievement);
						List<GongFaEntity> list = new List<GongFaEntity>();
						if (changeAchievementPanelResp_.Achievements != null)
						{
							foreach (ChangeAchievementPanelResp_1005009.Types.AchievementObjectResp_1005010 achievement in changeAchievementPanelResp_.Achievements)
							{
								list.Add(new GongFaEntity(achievement));
							}
						}
						if (gongFaEntity != null && !string.IsNullOrEmpty(gongFaEntity.Id))
						{
							list.Add(gongFaEntity);
						}
						if (list.Count == 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无已参悟的炼神功法，请先参悟功法");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("GengHuanGongFaWnd.prefab", true, gongFaEntity, list, GongFaType.LianShen, m_CurrentYunXingGongFa);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(changeAchievementPanelResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void KongLingFaZhenBtnClick()
		{
			KongLingZhenModel kongLingZhenModel = new KongLingZhenModel
			{
				a = (m_DataSource.ControllerMatrixRank / 4).ToString(),
				i = m_DataSource.GetControllerMatrixName(),
				j = m_DataSource.ControllerMatrixRank / 4
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.OPEN_CONTROLLER_MATRIX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					OpenControllerMatrixResp_1005014 openControllerMatrixResp_ = new OpenControllerMatrixResp_1005014();
					openControllerMatrixResp_.MergeFrom(byteString);
					if (openControllerMatrixResp_.IsSuccess)
					{
						kongLingZhenModel.c = openControllerMatrixResp_.IsOpenGold;
						kongLingZhenModel.d = openControllerMatrixResp_.IsOpenWood;
						kongLingZhenModel.e = openControllerMatrixResp_.IsOpenWater;
						kongLingZhenModel.f = openControllerMatrixResp_.IsOpenFire;
						kongLingZhenModel.g = openControllerMatrixResp_.IsOpenEarth;
						Singleton<UIManager>.Instance.PopUpWnd("KongLingFaZhenWnd.prefab", true, kongLingZhenModel);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(openControllerMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void JuLingFaZhenBtnClick()
		{
			JuLingFaZhenModel juLingFaZhenModel = new JuLingFaZhenModel
			{
				a = (m_DataSource.GatherMatrixRank / 4).ToString(),
				b = m_DataSource.GetGatherMatrixName(),
				d = m_DataSource.GatherMatrixRank / 4
			};
			Singleton<UIManager>.Instance.PopUpWnd("JuLingFaZhenWnd.prefab", true, juLingFaZhenModel, m_DataSource.GatherMatrixLessTime);
		}

		private void LiKaiDongFuBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.PopUpWnd("DongFuRoomWnd.prefab", true);
		}

		private void ShengJiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DongFuChooseWnd.prefab", true);
		}

		private void ClearEvent()
		{
			m_BuffDic.Clear();
			foreach (Transform item in Panel.BuffContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		public override void OnClose()
		{
			RoleManager instance = Singleton<RoleManager>.Instance;
			instance.KeTuPoActionEvent = (UnityAction)Delegate.Remove(instance.KeTuPoActionEvent, new UnityAction(TuPoTiShi));
			base.OnClose();
			ClearEvent();
		}

		private void JingMaiYinDaoPanDuan()
		{
			if ((Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 2) & (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal == 1))
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 6;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
				Singleton<GuideManager>.Instance.ShowGuideNew(6);
			}
		}
	}
}
