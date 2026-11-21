using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeLianDanControlPanel : MonoBehaviour
	{
		public UnityAction<List<DanFangStepModel>, Dictionary<string, double>> FailerAction;

		public UnityAction<List<DanFangStepModel>, Dictionary<string, double>> SuccessAction;

		public RectTransform ZhiZhenImg;

		public Text ButtonText;

		public RectTransform CtrlBg;

		public RectTransform AreaBg;

		public CheckInput CheckInputPanel;

		private DanFangModel m_DataSource;

		private DiHuoGeKaiShiLianZhiWnd m_Window;

		private float m_HuaDongDistance;

		private Dictionary<string, double> m_StepBestTimeDic;

		private Queue<DanFangStepModel> m_NoCompeleteList;

		private List<DanFangStepModel> m_CompeleteList;

		private DanFangStepModel m_CurrentDataSource;

		private LianZhiStatus m_GlobalStatus;

		private LianZhiStatus m_StepStatus;

		private TouZhiType m_TouZhiType;

		private int m_ReduceTime;

		private float m_MaxWenDu = 2000f;

		private float m_MinWenDu;

		private float m_FlrameFirstAddValue;

		private float m_CurrentWendu;

		private float m_CurrentargetBestWenDu;

		private float m_TargetBestWenDu;

		private StepCompeleteStatus m_StepCompeleteStatus;

		private float m_PerfectMinWenDu;

		private float m_CurrentPerfectMinWenDu;

		private float m_PerfectMaxWenDu;

		private float m_CurrentPerfectMaxWenDu;

		private float m_GeneralMinWenDu;

		private float m_CurrentGeneralMinWenDu;

		private float m_GeneralMaxWenDu;

		private float m_CurrentGeneralMaxWenDu;

		private float m_StepBestTime;

		private Vector2 m_InputPos;

		private Vector2 m_UpInputPos;

		private IEnumerator m_FirstStep;

		private bool m_IsCanTouZhi;

		private bool m_IsInput;

		private bool m_IsFirstStepEnd;

		private float m_FlrameAddValue;

		private void Awake()
		{
			m_StepBestTimeDic = new Dictionary<string, double>();
			m_NoCompeleteList = new Queue<DanFangStepModel>();
			m_CompeleteList = new List<DanFangStepModel>();
			m_HuaDongDistance = 1f;
			m_ReduceTime = 2000;
			m_MaxWenDu = 2000f;
			m_MinWenDu = 0f;
			m_FlrameFirstAddValue = 30f;
			m_FlrameAddValue = 13f;
			m_CurrentWendu = 0f;
			m_StepBestTime = 0f;
			m_CurrentargetBestWenDu = 0f;
			m_TargetBestWenDu = 0f;
			m_PerfectMinWenDu = 0f;
			m_CurrentPerfectMinWenDu = 0f;
			m_PerfectMaxWenDu = 0f;
			m_CurrentPerfectMaxWenDu = 0f;
			m_GeneralMinWenDu = 0f;
			m_CurrentGeneralMinWenDu = 0f;
			m_GeneralMaxWenDu = 0f;
			m_CurrentGeneralMaxWenDu = 0f;
			m_IsInput = false;
			m_IsFirstStepEnd = false;
			m_GlobalStatus = LianZhiStatus.End;
			m_StepStatus = LianZhiStatus.End;
			m_StepCompeleteStatus = StepCompeleteStatus.None;
			m_TouZhiType = TouZhiType.None;
		}

		private void OnEnable()
		{
			CheckInput checkInputPanel = CheckInputPanel;
			checkInputPanel.PointDownHandler = (UnityAction<PointerEventData>)Delegate.Remove(checkInputPanel.PointDownHandler, new UnityAction<PointerEventData>(OnPointerDown));
			CheckInput checkInputPanel2 = CheckInputPanel;
			checkInputPanel2.PointDownHandler = (UnityAction<PointerEventData>)Delegate.Combine(checkInputPanel2.PointDownHandler, new UnityAction<PointerEventData>(OnPointerDown));
			CheckInput checkInputPanel3 = CheckInputPanel;
			checkInputPanel3.PointUpHandler = (UnityAction<PointerEventData>)Delegate.Remove(checkInputPanel3.PointUpHandler, new UnityAction<PointerEventData>(OnPointerUp));
			CheckInput checkInputPanel4 = CheckInputPanel;
			checkInputPanel4.PointUpHandler = (UnityAction<PointerEventData>)Delegate.Combine(checkInputPanel4.PointUpHandler, new UnityAction<PointerEventData>(OnPointerUp));
			CheckInput checkInputPanel5 = CheckInputPanel;
			checkInputPanel5.PointClickHandler = (UnityAction<PointerEventData>)Delegate.Remove(checkInputPanel5.PointClickHandler, new UnityAction<PointerEventData>(OnPointClick));
			CheckInput checkInputPanel6 = CheckInputPanel;
			checkInputPanel6.PointClickHandler = (UnityAction<PointerEventData>)Delegate.Combine(checkInputPanel6.PointClickHandler, new UnityAction<PointerEventData>(OnPointClick));
			CheckInput checkInputPanel7 = CheckInputPanel;
			checkInputPanel7.PointMoveHandler = (UnityAction<TouZhiType>)Delegate.Remove(checkInputPanel7.PointMoveHandler, new UnityAction<TouZhiType>(OnPointMove));
			CheckInput checkInputPanel8 = CheckInputPanel;
			checkInputPanel8.PointMoveHandler = (UnityAction<TouZhiType>)Delegate.Combine(checkInputPanel8.PointMoveHandler, new UnityAction<TouZhiType>(OnPointMove));
		}

		private void Update()
		{
			if (Time.timeScale == 0f || m_GlobalStatus == LianZhiStatus.End)
			{
				return;
			}
			PanDuanShiFouShiBai();
			RotateZhuanPan();
			if (!m_IsFirstStepEnd || Time.timeScale == 0f)
			{
				return;
			}
			if (m_IsInput && Input.GetMouseButtonUp(0))
			{
				m_IsInput = false;
			}
			if (m_IsInput)
			{
				m_CurrentWendu += m_FlrameAddValue;
			}
			else
			{
				if (m_CurrentWendu <= 0f)
				{
					m_CurrentWendu = 0f;
					return;
				}
				m_CurrentWendu -= m_FlrameAddValue;
			}
			if (m_StepStatus == LianZhiStatus.Start && m_CurrentDataSource.actionType != 6)
			{
				m_StepBestTime += Time.deltaTime * 1000f;
			}
			else if (m_StepStatus == LianZhiStatus.Start && m_CurrentDataSource.actionType == 6 && m_CurrentWendu >= m_CurrentPerfectMinWenDu && m_CurrentWendu <= m_CurrentPerfectMaxWenDu)
			{
				m_StepBestTime += Time.deltaTime * 1000f;
			}
		}

		private void FixedUpdate()
		{
		}

		private void StepFailed()
		{
			if (m_GlobalStatus != LianZhiStatus.End)
			{
				m_StepStatus = LianZhiStatus.End;
				m_GlobalStatus = LianZhiStatus.End;
				RecodeTime();
				m_StepBestTime = 0f;
				FailerAction?.Invoke(m_CompeleteList, m_StepBestTimeDic);
			}
		}

		private void PanDuanShiFouShiBai()
		{
			if (m_GlobalStatus == LianZhiStatus.End || !m_IsFirstStepEnd || (!(m_CurrentWendu < m_CurrentGeneralMinWenDu) && !(m_CurrentWendu > m_CurrentGeneralMaxWenDu)))
			{
				return;
			}
			if (m_DataSource.ei == "LianXiDanFang" && Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
			{
				if (m_CurrentWendu < m_CurrentGeneralMinWenDu)
				{
					m_CurrentWendu = m_CurrentGeneralMinWenDu;
				}
				else if (m_CurrentWendu > m_CurrentGeneralMaxWenDu)
				{
					m_CurrentWendu = m_CurrentGeneralMaxWenDu;
				}
			}
			else
			{
				StepFailed();
			}
		}

		public void InitDataSource(DanFangModel dataSource, DiHuoGeKaiShiLianZhiWnd window)
		{
			m_DataSource = dataSource;
			m_Window = window;
			foreach (DanFangStepModel item in dataSource.bb)
			{
				m_NoCompeleteList.Enqueue(item);
			}
			m_CurrentDataSource = m_NoCompeleteList.Dequeue();
			SetCurrentStep(m_CurrentDataSource);
			double num = JiSuanShakeSpeed();
			m_FlrameFirstAddValue = (float)num - 10f;
			m_FlrameAddValue = (float)num - 10f;
			if (m_FlrameAddValue >= -1f && m_FlrameAddValue < 0f)
			{
				m_FlrameAddValue = 1f;
			}
			else if (m_FlrameAddValue < 1f && m_FlrameAddValue > 0f)
			{
				m_FlrameAddValue = 1f;
			}
			if (m_FlrameAddValue < 0f)
			{
				m_FlrameAddValue = 0f - m_FlrameAddValue;
			}
		}

		public void SetCurrentStep(DanFangStepModel currentStep)
		{
			m_CurrentDataSource = currentStep;
			if (m_DataSource.ei == "LianXiDanFang" && Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
			{
				Time.timeScale = 0f;
			}
			switch ((LianZhiActionStepType)currentStep.actionType)
			{
			case LianZhiActionStepType.ACTION_FIRE:
				m_FirstStep = FirstStepShake();
				LianDanYinDaoPanDuan(1);
				FirstStep(currentStep);
				break;
			case LianZhiActionStepType.ACTION_THROW:
			{
				int touZhiType = UnityEngine.Random.Range(1, 3);
				m_TouZhiType = (TouZhiType)touZhiType;
				if (m_DataSource.ei == "LianXiDanFang")
				{
					LianDanYinDaoPanDuan(3);
				}
				m_TouZhiType = TouZhiType.Left;
				TouZhiCaiLiao();
				break;
			}
			case LianZhiActionStepType.ACTION_COLLECT:
				m_TouZhiType = TouZhiType.Up;
				LianDanYinDaoPanDuan(5);
				TouZhiCaiLiao();
				break;
			case LianZhiActionStepType.ACTION_CONTROLL_FIRE:
				SetBestWenDu(currentStep);
				MovePerfectArea(currentStep.continuetime);
				LianDanYinDaoPanDuan(2);
				TouZhiCaiLiao();
				break;
			case LianZhiActionStepType.ACTION_GATHER:
				m_TouZhiType = TouZhiType.Up;
				LianDanYinDaoPanDuan(4);
				TouZhiCaiLiao();
				break;
			}
			StartCoroutine(StepJiShi((float)currentStep.continuetime / 1000f, currentStep));
		}

		private void LianDanYinDaoPanDuan(int price)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100 && m_DataSource.ei == "LianXiDanFang")
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 8;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = price;
				Singleton<GuideManager>.Instance.ShowGuideNew(8);
			}
		}

		private void TouZhiCaiLiao()
		{
			if (m_GlobalStatus != LianZhiStatus.End)
			{
				m_Window.ShowTiShi(m_TouZhiType);
			}
		}

		private IEnumerator StepJiShi(float seconds, DanFangStepModel currentStep)
		{
			if (m_GlobalStatus != LianZhiStatus.End && currentStep.actionType != 1)
			{
				float jiShiTime = 0f;
				while (!(jiShiTime >= seconds + 0.5f))
				{
					jiShiTime += Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}
				if (!m_CompeleteList.Contains(m_CurrentDataSource))
				{
					StepFailed();
				}
				else
				{
					MoveCompelete();
				}
			}
		}

		private void FirstStep(DanFangStepModel currentStep)
		{
			SetBestWenDu(currentStep);
			SetCurrentWenDu(currentStep);
			float num = m_TargetBestWenDu / m_MaxWenDu;
			float num2 = 180f * num;
			AreaBg.eulerAngles = new Vector3(0f, 0f, 0f - num2);
			StartCoroutine(m_FirstStep);
			m_GlobalStatus = LianZhiStatus.Start;
		}

		private IEnumerator FirstStepShake()
		{
			bool isAdd = true;
			m_CurrentWendu = 0f;
			while (true)
			{
				if (Time.timeScale != 0f)
				{
					if (isAdd)
					{
						m_CurrentWendu += m_FlrameFirstAddValue;
					}
					else
					{
						m_CurrentWendu -= m_FlrameFirstAddValue;
					}
					if (m_CurrentWendu >= 2000f)
					{
						m_CurrentWendu = 2000f;
						isAdd = false;
					}
					else if (m_CurrentWendu <= 0f)
					{
						m_CurrentWendu = 0f;
						isAdd = true;
					}
				}
				yield return new WaitForEndOfFrame();
			}
		}

		public void SetBestWenDu(DanFangStepModel currentStep)
		{
			m_TargetBestWenDu = (float)currentStep.bestVal;
			if (m_TargetBestWenDu > 1750f)
			{
				Debug.Log("最佳温度出现异常温度:" + m_TargetBestWenDu);
				m_TargetBestWenDu = 1750f;
			}
			else if (m_TargetBestWenDu < 250f)
			{
				Debug.Log("最佳温度出现异常温度:" + m_TargetBestWenDu);
				m_TargetBestWenDu = 250f;
			}
			m_PerfectMaxWenDu = m_TargetBestWenDu + 45f;
			m_PerfectMinWenDu = m_TargetBestWenDu - 45f;
			m_GeneralMaxWenDu = m_TargetBestWenDu + 205f;
			m_GeneralMinWenDu = m_TargetBestWenDu - 205f;
		}

		private void SetCurrentWenDu(DanFangStepModel currentStep)
		{
			m_CurrentargetBestWenDu = (float)currentStep.bestVal;
			m_CurrentPerfectMaxWenDu = m_TargetBestWenDu + 45f;
			m_CurrentPerfectMinWenDu = m_TargetBestWenDu - 45f;
			m_CurrentGeneralMaxWenDu = m_TargetBestWenDu + 205f;
			m_CurrentGeneralMinWenDu = m_TargetBestWenDu - 205f;
		}

		private void MovePerfectArea(int time)
		{
			float num = (m_CurrentargetBestWenDu - m_TargetBestWenDu) / m_MaxWenDu;
			float jiaoDu = 180f * num;
			StartCoroutine(MovePerfect(time, jiaoDu));
		}

		private IEnumerator MovePerfect(float totalTime, float jiaoDu)
		{
			totalTime -= (float)m_ReduceTime;
			if (totalTime < 1000f)
			{
				totalTime += 2000f;
			}
			float num = m_TargetBestWenDu - m_CurrentargetBestWenDu;
			float speed = num / totalTime;
			float times = 0f;
			while (times < totalTime)
			{
				times += Time.deltaTime * 1000f;
				float num2 = speed * (Time.deltaTime * 1000f);
				m_CurrentargetBestWenDu += num2;
				m_CurrentGeneralMinWenDu += num2;
				m_CurrentGeneralMaxWenDu += num2;
				m_CurrentPerfectMaxWenDu += num2;
				m_CurrentPerfectMinWenDu += num2;
				float z = (0f - m_CurrentargetBestWenDu / m_MaxWenDu) * 180f;
				AreaBg.eulerAngles = new Vector3(0f, 0f, z);
				yield return new WaitForEndOfFrame();
			}
			m_CurrentargetBestWenDu = m_TargetBestWenDu;
			m_CurrentGeneralMinWenDu = m_GeneralMinWenDu;
			m_CurrentGeneralMaxWenDu = m_GeneralMaxWenDu;
			m_CurrentPerfectMaxWenDu = m_PerfectMaxWenDu;
			m_CurrentPerfectMinWenDu = m_PerfectMinWenDu;
			float z2 = (0f - m_TargetBestWenDu / m_MaxWenDu) * 180f;
			AreaBg.eulerAngles = new Vector3(0f, 0f, z2);
			StepCompelete();
		}

		private IEnumerator NextStep()
		{
			m_StepStatus = LianZhiStatus.End;
			yield return null;
			m_Window.NextStep();
			m_StepBestTime = 0f;
			m_StepStatus = LianZhiStatus.Start;
			m_CurrentDataSource = m_NoCompeleteList.Dequeue();
			SetCurrentStep(m_CurrentDataSource);
		}

		private void StepCompelete()
		{
			m_StepStatus = LianZhiStatus.End;
			DanFangStepModel currentDataSource = m_CurrentDataSource;
			m_CompeleteList.Add(currentDataSource);
		}

		private void RecodeTime()
		{
			if (!m_StepBestTimeDic.ContainsKey(m_CurrentDataSource.id))
			{
				m_StepBestTimeDic.Add(m_CurrentDataSource.id, m_StepBestTime);
			}
			else
			{
				m_StepBestTimeDic[m_CurrentDataSource.id] = m_StepBestTime;
			}
		}

		private void MoveCompelete()
		{
			if (m_GlobalStatus != LianZhiStatus.End)
			{
				switch ((LianZhiActionStepType)m_CurrentDataSource.actionType)
				{
				case LianZhiActionStepType.ACTION_FIRE:
					StartCoroutine(NextStep());
					m_IsFirstStepEnd = true;
					break;
				case LianZhiActionStepType.ACTION_THROW:
					StartCoroutine(NextStep());
					RecodeTime();
					break;
				case LianZhiActionStepType.ACTION_COLLECT:
					RecodeTime();
					CompeleteAllStep();
					break;
				case LianZhiActionStepType.ACTION_CONTROLL_FIRE:
					RecodeTime();
					StartCoroutine(NextStep());
					break;
				case LianZhiActionStepType.ACTION_GATHER:
					RecodeTime();
					StartCoroutine(NextStep());
					break;
				case LianZhiActionStepType.ACTION_SMELT:
				case LianZhiActionStepType.ACTION_BEAT:
					break;
				}
			}
		}

		private void CompeleteAllStep()
		{
			m_GlobalStatus = LianZhiStatus.End;
			SuccessAction?.Invoke(m_CompeleteList, m_StepBestTimeDic);
		}

		private void RotateZhuanPan()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			float num = m_CurrentWendu / m_MaxWenDu;
			float num2 = 180f * num;
			CtrlBg.eulerAngles = new Vector3(0f, 0f, 0f - num2);
			ZhiZhenImg.eulerAngles = new Vector3(0f, 0f, 0f - num2);
			if (m_IsFirstStepEnd)
			{
				if (m_NoCompeleteList.Count <= 3)
				{
					MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.ChengDan);
				}
				else if (num2 <= 0f)
				{
					MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
				}
				else if (num2 > 0f && num2 <= 60f)
				{
					MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.Small);
				}
				else if (num2 > 60f && num2 <= 120f)
				{
					MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.Middle);
				}
				else if (num2 > 120f && num2 <= 180f)
				{
					MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.Big);
				}
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			m_IsInput = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (m_GlobalStatus != LianZhiStatus.End)
			{
				m_IsInput = false;
			}
		}

		public void OnPointClick(PointerEventData eventData)
		{
			if (m_GlobalStatus == LianZhiStatus.End)
			{
				return;
			}
			if (m_CurrentDataSource.actionType == 1)
			{
				if (m_CurrentWendu < m_CurrentGeneralMinWenDu || m_CurrentWendu > m_CurrentGeneralMaxWenDu)
				{
					if (m_DataSource.ei == "LianXiDanFang" && Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
					{
						if (Time.timeScale == 0f)
						{
							Time.timeScale = 1f;
						}
					}
					else
					{
						StepFailed();
					}
				}
				else
				{
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanFire, isLoop: true);
					m_IsFirstStepEnd = true;
					StopCoroutine(m_FirstStep);
					StepCompelete();
					MoveCompelete();
				}
			}
			else if (Time.timeScale == 0f)
			{
				Time.timeScale = 1f;
			}
		}

		private double JiSuanShakeSpeed()
		{
			if (m_DataSource == null)
			{
				return 0.0;
			}
			double num = (double)(500f * Mathf.Pow(4f, m_DataSource.c) / (float)((m_DataSource.c - 4 < 1) ? 1 : (m_DataSource.c - 4))) / (double)Singleton<RoleManager>.Instance.Role_Entity.CurrentPerception;
			CoefficientNumEntity coefficientNumEntity = Singleton<FacadeManager>.Instance.SelectLianDanCoefficientNum(m_DataSource.r, m_DataSource.c);
			return num + coefficientNumEntity.CoefficientNum;
		}

		public void OnPointMove(TouZhiType direction)
		{
			if (m_StepStatus == LianZhiStatus.End || m_GlobalStatus == LianZhiStatus.End || !m_IsFirstStepEnd)
			{
				return;
			}
			if (m_TouZhiType != TouZhiType.None && m_TouZhiType == direction)
			{
				if (m_DataSource.ei == "LianXiDanFang" && Time.timeScale == 0f && Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
				{
					Time.timeScale = 1f;
				}
				if (m_TouZhiType == TouZhiType.Left)
				{
					m_Window.RightTouZhi();
				}
				else if (m_TouZhiType == TouZhiType.Right)
				{
					m_Window.LeftTouZhi();
				}
				m_TouZhiType = TouZhiType.None;
				TouZhiCaiLiao();
				StepCompelete();
			}
			else if ((!(m_DataSource.ei == "LianXiDanFang") || Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal == 100) && m_CurrentDataSource.actionType != 6)
			{
				StepFailed();
			}
		}
	}
}
