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

		internal bool PracticeRoomLoaded { get; private set; }
		internal bool PracticeBookEquipped => m_DataSource?.SuperAirName == "启灵经";
		internal RuningGongFaZuHeType PracticeRunning => m_CurrentYunXingGongFa;

		private GameObject m_BuffInsObj;

		private Dictionary<DongFuBuffType, DongFuBuffItem> m_BuffDic;

		private bool m_IsTuPoing;

		private bool m_BreakRequestPending;
		private bool m_RouteChangePending;

		private readonly (string Name, WuXing Element)[] m_QiTechniques = new (string, WuXing)[4];
		private readonly int[] m_QiLookupVersions = new int[4];
		private int m_ExerciseRoomVersion;
		private string m_CaveRoleId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuPanel>();
			InkCultivationInterface.StylePractice(Panel);
			CultivationRouteSheet.Attach(this);
			m_CaveRoleId = DongFuResidenceState.CurrentRoleId;
			DongFuResidenceState.LevelChanged += OnCaveGradeChanged;
			InkCaveThemeSelector.Attach(Panel, m_CaveRoleId, theme =>
			{
				DongFuSceneManager scene = MonoSingleton<DongFuSceneManager>.Instance;
				if (scene != null)
				{
					scene.SelectCaveTheme(theme);
				}
			});
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiDongFuBtnClick);
			AddButtonClickListener(Panel.ShengJiBtn, ShengJiBtnClick);
			AddButtonClickListener(Panel.KongLingFaZhenXuanZeBtn, KongLingFaZhenBtnClick);
			AddButtonClickListener(Panel.JuLingFaZhenXuanZeBtn, JuLingFaZhenBtnClick);
			AddToggleClickListener(Panel.LianShenBtn, LianShenBtnClick);
			AddToggleClickListener(Panel.DuanTiBtn, DuanTiBtnClick);
			AddToggleClickListener(Panel.NaQiBtn, NaQiBtnClick);
			AddButtonClickListener(Panel.BiGuanTuPoBtn, BiGuanToPoBtnClick);
			InkThunderTrial.AttachEntry(Panel, () =>
			{
				if (!m_BreakRequestPending && !m_IsTuPoing)
					InkThunderTrial.Open(Panel, null);
			});
			AddButtonClickListener(Panel.LianShenGengHuanBtn, GengHuanLianShenBtnClick);
			AddButtonClickListener(Panel.DuanTiGengHuanBtn, GengHuanDuanTiBtnClick);
			AddButtonClickListener(Panel.NaQiGengHuanBtn, GengHuanNaQiBtnClick);
			AddButtonClickListener(Panel.EnterNeiShiBtn, EnterNeiShi);
			ShowMeridianEntry();
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
			InkCultivationPages.RefreshRolePortrait(Panel.transform.Find("CultivatorIdentity/Portrait"));
			OnCaveGradeChanged(m_CaveRoleId, DongFuResidenceState.GetLevel(m_CaveRoleId));
			PracticeRoomLoaded = false;
			Singleton<GuideManager>.Instance.ShowPractice();
			int roomVersion = ++m_ExerciseRoomVersion;
			DongFuSceneManager exerciseScene = MonoSingleton<DongFuSceneManager>.Instance;
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_EXERCISE_ROOM, null, (object _obj) =>
			{
				if (Panel != null && exerciseScene != null && roomVersion == m_ExerciseRoomVersion)
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
							RefreshCultivationQi(loadMetadata: true);
							Panel.DoubleXiuLianProgressRect.SetRunningGongFaType(m_CurrentYunXingGongFa);
							Init();
							Animator animator = exerciseScene.XiuXingAnimator;
							if (animator != null)
								animator.gameObject.SetActive(value: true);
							ReFlashData();
							ReflashTitleValue();
							reflash();
							ShowMeridianEntry();
							PracticeRoomLoaded = true;
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

		private void OnCaveGradeChanged(string roleId, int level)
		{
			if (roleId != m_CaveRoleId || Panel == null)
			{
				return;
			}
			DongFuSceneManager scene = MonoSingleton<DongFuSceneManager>.Instance;
			if (scene != null)
			{
				scene.SetCaveGrade(level);
			}
			string grade = level switch
			{
				1 => "黄阶",
				2 => "玄阶",
				3 => "地阶",
				4 => "天阶",
				_ => "普通"
			};
			Panel.DongFuNameText.text = grade + " · 练功房";
		}

		internal void RecordCultivationTechnique(GongFaType type, GongFaEntity technique)
		{
			int slot = (int)type;
			m_QiLookupVersions[slot]++;
			m_QiTechniques[slot] = (technique.Name, ToQiElement(technique.FiveElement));
			RefreshCultivationQi(loadMetadata: false);
		}

		private static WuXing ToQiElement(int element)
		{
			return element >= (int)WuXing.Jin && element <= (int)WuXing.Tu ? (WuXing)element : WuXing.All;
		}

		private string TechniqueName(GongFaType type)
		{
			return type switch
			{
				GongFaType.LianShen => m_DataSource.PerceptionName,
				GongFaType.DuanTi => m_DataSource.PhysiqueName,
				GongFaType.NaQi => m_DataSource.SuperAirName,
				_ => null
			};
		}

		private WuXing TechniqueElement(GongFaType type)
		{
			string name = TechniqueName(type);
			var known = m_QiTechniques[(int)type];
			return !string.IsNullOrEmpty(name) && name == known.Name ? known.Element : WuXing.All;
		}

		private void RefreshCultivationQi(bool loadMetadata)
		{
			DongFuSceneManager scene = MonoSingleton<DongFuSceneManager>.Instance;
			if (scene == null)
			{
				return;
			}
			var (primary, secondary) = RunningTechniques();
			InkCultivationPages.TechniqueMote(Panel.LianShenRunningImg, TechniqueElement(GongFaType.LianShen));
			InkCultivationPages.TechniqueMote(Panel.DuanTiRunningImg, TechniqueElement(GongFaType.DuanTi));
			InkCultivationPages.TechniqueMote(Panel.NaQiRunningImg, TechniqueElement(GongFaType.NaQi));
			scene.ShowCultivationQi(TechniqueElement(primary), TechniqueElement(secondary));
			if (loadMetadata)
			{
				LoadTechniqueElement(primary);
				LoadTechniqueElement(secondary);
			}
		}

		private void LoadTechniqueElement(GongFaType type)
		{
			int slot = (int)type;
			string name = TechniqueName(type);
			if (string.IsNullOrEmpty(name) || m_QiTechniques[slot].Name == name)
			{
				return;
			}
			int lookupVersion = ++m_QiLookupVersions[slot];
			int roomVersion = m_ExerciseRoomVersion;
			var request = new ChangeAchievementPanelReq_1005009 { AchievementType = slot };
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHANGE_ACHIEVEMENT_PANEL, request, response =>
			{
				if (roomVersion != m_ExerciseRoomVersion || lookupVersion != m_QiLookupVersions[slot] || Panel == null ||
					!(response is ByteString payload))
				{
					return;
				}
				var details = new ChangeAchievementPanelResp_1005009();
				details.MergeFrom(payload);
				var technique = details.CurrentAchievement;
				if (details.IsSuccess && technique != null && technique.Name == name && TechniqueName(type) == name)
				{
					m_QiTechniques[slot] = (name, ToQiElement(technique.FiveElement));
					RefreshCultivationQi(loadMetadata: false);
				}
			}, false, ServerType.GamesNormalServer, showTimeout: false);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_DataSource != null && !(Panel == null))
			{
				if (m_DataSource.ControllerMartixLessTime > 0.0)
				{
					m_DataSource.ControllerMartixLessTime -= Time.deltaTime;
					Panel.KongLingFaZhenShengYuTimeText.text = string.Format("剩余 <b>{0}</b> 小时", (m_DataSource.ControllerMartixLessTime / 60.0 / 60.0).ToString("0.0"));
				}
				if (m_DataSource.GatherMatrixLessTime > 0.0)
				{
					m_DataSource.GatherMatrixLessTime -= Time.deltaTime;
					Panel.JuLingFaZhenShengYuTimeText.text = string.Format("剩余 <b>{0}</b> 小时", (m_DataSource.GatherMatrixLessTime / 60.0 / 60.0).ToString("0.0"));
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
			if (!ClientHotUpdate.MeridianEntryEnabled || Singleton<GuideManager>.Instance.Practice?.Active == true)
				return;
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "NeiShiScene");
		}

		private void ShowMeridianEntry()
		{
			Panel.EnterNeiShiBtn.gameObject.SetActive(ClientHotUpdate.MeridianEntryEnabled &&
				Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 2);
		}

		private void WenHaoTogClick()
		{
			string text = "未装备功法时可基础行功，更换为已参悟的功法可获得对应加成。纳气积修为，炼神、锻体各可练到本层底值的140%。元婴起可兼修两项，各七成效率，相生一路再乘1.2。离线按所选路线八成积累，每次至多12小时。周天推演可估算各项练满时间。\n" + CultivationRouteSheet.BreakthroughTrainingNote();
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "练功房");
		}

		private (GongFaType primary, GongFaType secondary) RunningTechniques()
		{
			return m_CurrentYunXingGongFa switch
			{
				RuningGongFaZuHeType.LianShen => (GongFaType.LianShen, GongFaType.None),
				RuningGongFaZuHeType.DuanTi => (GongFaType.DuanTi, GongFaType.None),
				RuningGongFaZuHeType.NaQi => (GongFaType.NaQi, GongFaType.None),
				RuningGongFaZuHeType.DuanTiNaQi => (GongFaType.NaQi, GongFaType.DuanTi),
				RuningGongFaZuHeType.DuanTiLianShen => (GongFaType.DuanTi, GongFaType.LianShen),
				RuningGongFaZuHeType.LianShenNaQi => (GongFaType.NaQi, GongFaType.LianShen),
				_ => (GongFaType.None, GongFaType.None)
			};
		}

		private void SyncPracticeToggles()
		{
			var (first, second) = RunningTechniques();
			Panel.NaQiBtn.SetIsOnWithoutNotify(first == GongFaType.NaQi || second == GongFaType.NaQi);
			Panel.DuanTiBtn.SetIsOnWithoutNotify(first == GongFaType.DuanTi || second == GongFaType.DuanTi);
			Panel.LianShenBtn.SetIsOnWithoutNotify(first == GongFaType.LianShen || second == GongFaType.LianShen);
		}

		private void Init()
		{
			SyncPracticeToggles();
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
			Panel.LianShenGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PerceptionName) ? "基础行功" : m_DataSource.PerceptionName);
			Panel.LianShenGongFaXiaoLv.text = string.Format("{0}/周天", m_DataSource.PerceptionCircle.ToString("f2"));
			Panel.DuanTiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PhysiqueName) ? "基础行功" : m_DataSource.PhysiqueName);
			Panel.DuanTiGongFaXiaoLv.text = string.Format("{0}/周天", m_DataSource.PhysiqueCircle.ToString("f2"));
			Panel.NaQiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.SuperAirName) ? "基础行功" : m_DataSource.SuperAirName);
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
			Panel.NaQiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.SuperAirName) ? "基础行功" : m_DataSource.SuperAirName);
		}

		public void ResetDuanTiName(string name)
		{
			m_DataSource.PhysiqueName = name;
			Panel.DuanTiGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PhysiqueName) ? "基础行功" : m_DataSource.PhysiqueName);
		}

		public void ResetLianShenName(string name)
		{
			m_DataSource.PerceptionName = name;
			Panel.LianShenGongFaName.text = (string.IsNullOrEmpty(m_DataSource.PerceptionName) ? "基础行功" : m_DataSource.PerceptionName);
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
			if (m_IsTuPoing || m_BreakRequestPending || InkThunderTrial.IsOpen(Panel))
				return;
			SendBreakRequest(MsgType.ROLE_BREAK_REALM, byteString =>
			{
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
			});
		}

		private void SendBreakRequest(MsgType type, Action<ByteString> complete)
		{
			if (m_BreakRequestPending || Panel == null) return;
			m_BreakRequestPending = true;
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(type, null, payload =>
			{
				m_BreakRequestPending = false;
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (Panel == null || role != Singleton<RoleManager>.Instance.Role_Entity) return;
				if (payload != null) complete(payload);
				else MonoSingleton<GameStart>.Instance.PopUpAttention("境界回讯超时，请重新进入角色查看修为。");
			});
			if (!sent)
			{
				m_BreakRequestPending = false;
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				MonoSingleton<GameStart>.Instance.PopUpAttention("未连上游戏服，请稍后再试。");
			}
		}

		private void NaQiTuPo(int LeiDianCount)
		{
			if (LeiDianCount == 0)
			{
				int level = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
				// The single-player servers use the ordinary breakthrough response for major realms too.
				if (level >= 20 && level % 10 == 0)
				{
					InkThunderTrial.Open(Panel, TuPoConnect);
					return;
				}
				if (level == 10)
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
			SendBreakRequest(MsgType.ROLE_BREAK, byteString =>
			{
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
						role_Entity.Vigor = Math.Max(role_Entity.Vigor, 100 + role_Entity.RoleLevel * 5);
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
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			MonoSingleton<GameStart>.Instance.StartCoroutine(ProgressRunningIE(() =>
			{
				MonoSingleton<DongFuSceneManager>.Instance.EndTuPo();
				Timer.Register(1f, () =>
				{
					Singleton<UIManager>.Instance.ShowWnd(this, false);
					m_IsTuPoing = false;
					Panel.BottomBg.gameObject.SetActive(value: true);
					ShowMeridianEntry();
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
				ShowMeridianEntry();
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

		private void ChangePracticeRoute(GongFaType type, bool enable)
		{
			// Display the confirmed route until the server accepts a player's change.
			SyncPracticeToggles();
			if (!PracticeRoomLoaded || m_RouteChangePending)
				return;
			var (first, second) = RunningTechniques();
			bool alreadyRunning = first == type || second == type;
			if (enable == alreadyRunning || (!enable && second == GongFaType.None))
				return;
			int location;
			if (!enable)
				location = (int)(first == type ? second : first);
			else if (first == GongFaType.None || Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				location = (int)type;
			else if (second != GongFaType.None)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最多同时运行两种类型功法！");
				return;
			}
			else if (first != GongFaType.LianShen && type != GongFaType.LianShen)
				location = (int)RuningGongFaZuHeType.DuanTiNaQi;
			else if (first != GongFaType.NaQi && type != GongFaType.NaQi)
				location = (int)RuningGongFaZuHeType.DuanTiLianShen;
			else
				location = (int)RuningGongFaZuHeType.LianShenNaQi;

			m_RouteChangePending = true;
			var request = new ChangeRunLocationReq_1001007 { NewLocation = location };
			bool queued = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ROLE_CHANGE_LOCATION, request, bytes =>
			{
				m_RouteChangePending = false;
				if (GameObject == null || !GameObject.activeInHierarchy || m_CaveRoleId != DongFuResidenceState.CurrentRoleId)
					return;
				var reply = bytes == null ? null : ActionBoolResp_1001001.Parser.ParseFrom(bytes);
				if (reply != null && !reply.IsSuccess)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(reply.Code));
					return;
				}
				// A lost acknowledgement can follow a committed write. Read the server's current route.
				OnShow();
			});
			if (!queued)
			{
				m_RouteChangePending = false;
				MonoSingleton<GameStart>.Instance.PopUpAttention("未连接游戏服，请重连后切换功法。");
			}
		}

		private void NaQiBtnClick(bool isOn) => ChangePracticeRoute(GongFaType.NaQi, isOn);

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
						if (gongFaEntity != null && !string.IsNullOrEmpty(gongFaEntity.Id) && !list.Exists(book => book.Id == gongFaEntity.Id))
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

		private void DuanTiBtnClick(bool isOn) => ChangePracticeRoute(GongFaType.DuanTi, isOn);

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
						var current = changeAchievementPanelResp_.CurrentAchievement;
						GongFaEntity gongFaEntity = current == null ? null : new GongFaEntity(current);
						List<GongFaEntity> list = new List<GongFaEntity>();
						if (changeAchievementPanelResp_.Achievements != null)
						{
							foreach (ChangeAchievementPanelResp_1005009.Types.AchievementObjectResp_1005010 achievement in changeAchievementPanelResp_.Achievements)
							{
								list.Add(new GongFaEntity(achievement));
							}
						}
						if (gongFaEntity != null && !string.IsNullOrEmpty(gongFaEntity.Id) && !list.Exists(book => book.Id == gongFaEntity.Id))
						{
							list.Add(gongFaEntity);
						}
						if (list.Count == 0)
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

		private void LianShenBtnClick(bool isOn) => ChangePracticeRoute(GongFaType.LianShen, isOn);

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
						var current = changeAchievementPanelResp_.CurrentAchievement;
						GongFaEntity gongFaEntity = current == null ? null : new GongFaEntity(current);
						List<GongFaEntity> list = new List<GongFaEntity>();
						if (changeAchievementPanelResp_.Achievements != null)
						{
							foreach (ChangeAchievementPanelResp_1005009.Types.AchievementObjectResp_1005010 achievement in changeAchievementPanelResp_.Achievements)
							{
								list.Add(new GongFaEntity(achievement));
							}
						}
						if (gongFaEntity != null && !string.IsNullOrEmpty(gongFaEntity.Id) && !list.Exists(book => book.Id == gongFaEntity.Id))
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

		[Serializable] private class MatrixOrder { public string kind; public int target; public long price; }
		[Serializable] private class MatrixQuote { public string name, description, error; public int target; public long price, stones; public bool canBuy; }

		public void KongLingFaZhenBtnClick() => RequestMatrix(new MatrixOrder {kind = "control"});
		public void JuLingFaZhenBtnClick() => RequestMatrix(new MatrixOrder {kind = "gather"});

		private void RequestMatrix(MatrixOrder order)
		{
			ForgeWire.Send(MsgType.CaveMatrix, JsonUtility.ToJson(order), (ok, json) =>
			{
				var quote = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<MatrixQuote>(json);
				if (!ok || quote == null)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(quote?.error ?? "阵图未能读取，请重试。");
					return;
				}
				if (order.target > 0)
				{
					Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = quote.stones;
					OnShow();
					MonoSingleton<GameStart>.Instance.PopUpAttention(quote.description, null, null, quote.name);
					return;
				}
				Action buy = null;
				if (quote.canBuy) buy = () => RequestMatrix(new MatrixOrder { kind=order.kind, target=quote.target, price=quote.price });
				MonoSingleton<GameStart>.Instance.PopUpAttention(quote.description, buy, null, quote.name);
				if (buy != null)
					Singleton<UIManager>.Instance.FindWndByName<AttentionWnd>("AttentionWnd.prefab")?.SetChoices("布阵", "稍后", () => {});
			});
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
			m_ExerciseRoomVersion++;
			DongFuResidenceState.LevelChanged -= OnCaveGradeChanged;
			DongFuSceneManager scene = MonoSingleton<DongFuSceneManager>.Instance;
			if (scene != null)
			{
				scene.HideCultivationQi();
			}
			RoleManager instance = Singleton<RoleManager>.Instance;
			instance.KeTuPoActionEvent = (UnityAction)Delegate.Remove(instance.KeTuPoActionEvent, new UnityAction(TuPoTiShi));
			base.OnClose();
			ClearEvent();
		}

		private void JingMaiYinDaoPanDuan()
		{
			if (!ClientHotUpdate.MeridianEntryEnabled || Singleton<GuideManager>.Instance.Practice?.Active == true)
				return;
			if ((Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 2) & (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal == 1))
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 6;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
				Singleton<GuideManager>.Instance.ShowGuideNew(6);
			}
		}
	}
}
