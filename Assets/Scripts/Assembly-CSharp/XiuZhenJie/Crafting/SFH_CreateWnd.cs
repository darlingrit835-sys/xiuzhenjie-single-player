using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class SFH_CreateWnd : Window
	{
		public SFH_CreatePanel Panel;

		private List<HuaFuLineItem> m_LineItemList;

		private int m_CurrentChooseStage;

		private FuLuEntity m_CurrentChooseModel;

		private GameObject m_LineItem;

		private bool m_ActiveInput = true;

		private Vector3 m_MousePos;

		private InputDirection m_InputDirection;

		private bool m_CanReceiveInput;

		private int m_CurrentLineIndex;

		private PatingStatus m_PatingStatus;

		private bool m_isRunning;

		private double m_RecatingTime;

		private bool CanCreate;

		private List<Transform> m_Points;

		private int CreateIsSuccess;

		private string m_CreateSuccessFuLuId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<SFH_CreatePanel>();
			m_isRunning = false;
			m_CanReceiveInput = false;
			m_CurrentLineIndex = 0;
			m_PatingStatus = PatingStatus.None;
			m_LineItemList = new List<HuaFuLineItem>();
			m_Points = new List<Transform>();
			m_CurrentChooseModel = paralist[0] as FuLuEntity;
			m_LineItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/HuaFuLineItem.prefab");
			AddButtonClickListener(Panel.LiKaiBtn, LiKai);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			AddButtonClickListener(Panel.Success_CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("SFH_CreateWnd.prefab");
			});
			AddButtonClickListener(Panel.Success_QueDingBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("SFH_CreateWnd.prefab");
			});
			AddButtonClickListener(Panel.Fail_AgainBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("SFH_CreateWnd.prefab");
			});
			AddButtonClickListener(Panel.Fail_CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd("SFH_CreateWnd.prefab");
			});
			m_CurrentChooseStage = m_CurrentChooseModel.Stage;
			m_RecatingTime = JiSuanShakeSpeed() * 2.0;
		}

		private double JiSuanShakeSpeed()
		{
			if (m_CurrentChooseModel == null)
			{
				return 0.05000000074505806;
			}
			double num = (double)(500f * Mathf.Pow(4f, m_CurrentChooseModel.Stage) / (float)((m_CurrentChooseModel.Stage - 4 < 1) ? 1 : (m_CurrentChooseModel.Stage - 4))) / (double)Singleton<RoleManager>.Instance.Role_Entity.CurrentPerception;
			CoefficientNumEntity coefficientNumEntity = Singleton<FacadeManager>.Instance.SelectHuaFuCoefficientNum(m_CurrentChooseModel.Rank, m_CurrentChooseModel.Stage);
			return num + coefficientNumEntity.CoefficientNum;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			CanCreate = true;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_PatingStatus == PatingStatus.CanContiue || m_PatingStatus == PatingStatus.Running)
			{
				GetInputDirection();
			}
			if (!CanCreate || !Input.GetMouseButtonDown(0))
			{
				return;
			}
			Vector2 localPoint = Input.mousePosition;
			Canvas component = Singleton<UIManager>.Instance.m_UiRoot.GetComponent<Canvas>();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(Singleton<UIManager>.Instance.m_UiRoot, Input.mousePosition, component.worldCamera, out localPoint);
			if (localPoint.x > 167f || localPoint.x < -167f || localPoint.y > 230f || localPoint.y < -334f)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("有效创造范围之外！");
				return;
			}
			if (m_CurrentChooseModel.Points == null)
			{
				m_CurrentChooseModel.Points = new List<FuShuPoint>();
			}
			m_CurrentChooseModel.Points.Add(new FuShuPoint
			{
				x = localPoint.x,
				y = localPoint.y
			});
			RectTransform component2 = Panel.TempleItem.gameObject.GetComponent<RectTransform>();
			component2.anchoredPosition = new Vector2(localPoint.x, localPoint.y + 40f);
			GameObject gameObject = Object.Instantiate(m_LineItem);
			gameObject.transform.SetParent(Panel.HuaBuRect);
			HuaFuLineItem component3 = gameObject.GetComponent<HuaFuLineItem>();
			Vector3 selfPos = new Vector3(component2.transform.position.x, component2.transform.position.y, -0.8f);
			m_Points.Add(gameObject.transform);
			component3.SetPos(selfPos, m_Points.Count, m_CurrentChooseModel.Points.Count == m_CurrentChooseStage + 4);
			if (m_CurrentChooseModel.Points.Count == m_CurrentChooseStage + 4)
			{
				CanCreate = false;
				GetCreateResult();
			}
		}

		public void OnceAgain()
		{
			ClearPointImg();
			CanCreate = true;
		}

		private void InitPoint()
		{
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("请开始画符!");
			m_LineItemList.Clear();
			for (int i = 0; i < m_Points.Count; i++)
			{
				Transform transform = m_Points[i];
				HuaFuLineItem component = transform.GetComponent<HuaFuLineItem>();
				if (m_CurrentChooseModel.Points.Count > i + 1)
				{
					Vector3 selfPos = new Vector3(transform.position.x, transform.position.y, -0.8f);
					Vector3 targetPoint = new Vector3(m_Points[i + 1].transform.position.x, m_Points[i + 1].transform.position.y, -0.8f);
					component.SetLine(i, selfPos, targetPoint, LineCompelete);
				}
				else
				{
					Vector3 vector = new Vector3(transform.position.x, transform.position.y, -0.8f);
					component.SetLine(i, vector, vector, LineCompelete, isEnd: true);
				}
				m_LineItemList.Add(component);
			}
			Timer.Register(0.2f, StartPating);
		}

		public void HideOrShowAllPoint(bool isHide)
		{
			foreach (Transform point in m_Points)
			{
				point.gameObject.SetActive(!isHide);
			}
		}

		private void GetCreateResult()
		{
			VerifyCreateMethodSignReq_1008047 verifyCreateMethodSignReq_ = new VerifyCreateMethodSignReq_1008047
			{
				MethodSkillId = m_CurrentChooseModel.Id
			};
			foreach (FuShuPoint point in m_CurrentChooseModel.Points)
			{
				verifyCreateMethodSignReq_.Points.Add(new VerifyCreateMethodSignReq_1008047.Types.SignPointReq_1008051
				{
					X = point.x,
					Y = point.y
				});
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.FUSHU_CHUANGZHAO_CHECK, verifyCreateMethodSignReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCreateMethodSignResp_1008047 verifyCreateMethodSignResp_ = new VerifyCreateMethodSignResp_1008047();
					verifyCreateMethodSignResp_.MergeFrom(byteString);
					if (verifyCreateMethodSignResp_.IsSuccess)
					{
						if (verifyCreateMethodSignResp_.CreatedIsSuccess)
						{
							m_CreateSuccessFuLuId = verifyCreateMethodSignResp_.MethodSignId;
							Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodSignResp_.CurrentClever;
							CreateIsSuccess = 1;
							InitPoint();
						}
						else
						{
							HideOrShowAllPoint(isHide: true);
							CreateIsSuccess = 2;
							Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodSignResp_.CurrentClever;
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodSignResp_.Code);
							Singleton<UIManager>.Instance.PopUpWnd("SFH_CreateResultWnd.prefab", false, true, false, m_CurrentChooseStage + 4, hintMsgByCode);
						}
					}
					else
					{
						HideOrShowAllPoint(isHide: true);
						CreateIsSuccess = 2;
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodSignResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
					}
				}
			});
		}

		private void LineCompelete(int index)
		{
			if (m_LineItemList.Count > index + 2)
			{
				m_PatingStatus = PatingStatus.CanContiue;
				m_CanReceiveInput = true;
				Timer.Register((float)m_RecatingTime, () =>
				{
					m_CanReceiveInput = false;
				});
			}
			else
			{
				m_PatingStatus = PatingStatus.Compelete;
				ClearPointImg();
				m_CanReceiveInput = false;
				LineCompeleteSuccess();
			}
		}

		private void GetInputDirection()
		{
			if (Input.GetMouseButtonDown(0))
			{
				m_ActiveInput = true;
				m_MousePos = Input.mousePosition;
			}
			if (Input.GetMouseButton(0) && m_ActiveInput && Vector3.Distance(Input.mousePosition, m_MousePos) > 40f)
			{
				float angle = GetAngle(Input.mousePosition, m_MousePos);
				if (m_CanReceiveInput && m_PatingStatus == PatingStatus.CanContiue)
				{
					Vector3 position = m_LineItemList[m_CurrentLineIndex + 1].transform.position;
					Vector3 position2 = m_LineItemList[m_CurrentLineIndex].transform.position;
					float angle2 = GetAngle(position, position2);
					if (angle <= angle2 + 30f && angle >= angle2 - 30f)
					{
						m_PatingStatus = PatingStatus.Compelete;
						m_LineItemList[m_CurrentLineIndex].StartLine();
						HuaFuLineSendToMsg();
						m_CurrentLineIndex++;
						m_PatingStatus = PatingStatus.Running;
					}
					else if (angle > angle2 + 30f || angle < angle2 - 30f)
					{
						m_PatingStatus = PatingStatus.Fail;
						ClearPointImg();
						LineCompeleteFail();
					}
					m_ActiveInput = false;
				}
				else
				{
					m_PatingStatus = PatingStatus.Fail;
					ClearPointImg();
					LineCompeleteFail();
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				m_ActiveInput = false;
			}
		}

		private void HuaFuLineSendToMsg()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.HUAFU_CHECK, null, null);
		}

		private float GetAngle(Vector2 OnePoint, Vector2 TwoPoint)
		{
			Vector3 vector = OnePoint - TwoPoint;
			if (vector.magnitude > 0f)
			{
				return Mathf.Acos(Vector3.Dot(Vector3.right, vector.normalized)) * 57.29578f;
			}
			return 0f;
		}

		public void StartPating()
		{
			if (!m_isRunning)
			{
				m_PatingStatus = PatingStatus.CanContiue;
				m_CanReceiveInput = true;
				m_isRunning = true;
			}
		}

		private void LiKai()
		{
			ClearPointImg();
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public void ClearPanelBtnClick()
		{
			if (!m_isRunning)
			{
				ClearPointImg();
			}
		}

		public void PopFailBg(string jingLiXiaoHao)
		{
			Panel.FailJingLiText.text = $"你消耗了{jingLiXiaoHao}点悟性";
			Panel.FailBg.gameObject.SetActive(value: true);
		}

		public void ClearPointImg()
		{
			CreateIsSuccess = 0;
			m_CurrentLineIndex = 0;
			foreach (HuaFuLineItem lineItem in m_LineItemList)
			{
				Object.Destroy(lineItem.gameObject);
			}
			m_LineItemList?.Clear();
			m_Points?.Clear();
			m_CurrentChooseModel.Points?.Clear();
		}

		private void WenHaoTogClick()
		{
			string text = $"符师封印法术入符纸，画符笔画长短方向内蕴天地法则。依照笔画次序，沿对应方向划过，即可控制运笔，注意转折时机，如此一气呵成，方可成符。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "画符静室");
		}

		private void LineCompeleteSuccess()
		{
			m_PatingStatus = PatingStatus.Compelete;
			m_isRunning = false;
			CreateIsSuccess = 1;
			CreateMethodSignRefiningResultReq_1008052 data = new CreateMethodSignRefiningResultReq_1008052
			{
				IsSuccess = true,
				MethodSignId = m_CreateSuccessFuLuId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.FUSHU_CREATE_RESULT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					CreateMethodSignRefiningResultResp_1008052 createMethodSignRefiningResultResp_ = new CreateMethodSignRefiningResultResp_1008052();
					createMethodSignRefiningResultResp_.MergeFrom(byteString);
					if (createMethodSignRefiningResultResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = createMethodSignRefiningResultResp_.CurrentClever;
						XSJ_FaShuEntity xSJ_FaShuEntity = new XSJ_FaShuEntity
						{
							Rank = createMethodSignRefiningResultResp_.Rank,
							DetailList = GetSuccessDataDetailList(createMethodSignRefiningResultResp_)
						};
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(createMethodSignRefiningResultResp_.LiveLevel, createMethodSignRefiningResultResp_.LiveProficiency, RoleFuZhiType.ZhiFu);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						Singleton<UIManager>.Instance.PopUpWnd("SFH_CreateResultWnd.prefab", false, true, true, m_CurrentChooseStage + 4, xSJ_FaShuEntity);
					}
					else
					{
						HideOrShowAllPoint(isHide: true);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createMethodSignRefiningResultResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private List<string> GetSuccessDataDetailList(CreateMethodSignRefiningResultResp_1008052 data)
		{
			List<string> list = new List<string>();
			switch ((FaShuType)data.SkillType)
			{
			case FaShuType.gongji_suoding:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-锁定攻击"));
				break;
			case FaShuType.gongji_dingxiang:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-定向攻击"));
				break;
			case FaShuType.gongji_chixu:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-持续攻击"));
				list.Add(string.Format("{0},{1}", "持续时间", data.ContinueTime / 1000 + "息"));
				break;
			case FaShuType.gongji_fanwei:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-范围攻击"));
				break;
			case FaShuType.gongji_fanwei_chixu:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-范围持续攻击"));
				list.Add(string.Format("{0},{1}", "持续时间", data.ContinueTime / 1000 + "息"));
				break;
			case FaShuType.fangyu_fashu_hudun:
				list.Add(string.Format("{0},{1}", "技能类型", "防御-法术护盾"));
				break;
			case FaShuType.zengyi_shanghai:
				list.Add(string.Format("{0},{1}", "技能类型", "增益-伤害增益"));
				break;
			case FaShuType.zengyi_fangyu:
				list.Add(string.Format("{0},{1}", "技能类型", "增益-防御增益"));
				break;
			case FaShuType.kongzhi_yisu:
				list.Add(string.Format("{0},{1}", "技能类型", "控制-减低移速"));
				break;
			case FaShuType.kongzhi_xingdong:
				list.Add(string.Format("{0},{1}", "技能类型", "控制-限制行动"));
				break;
			case FaShuType.dunshu_wuxing:
				list.Add(string.Format("{0},{1}", "技能类型", "遁术-五行遁术"));
				break;
			case FaShuType.dunshu_yujian:
				list.Add(string.Format("{0},{1}", "技能类型", "遁术-御剑术"));
				break;
			}
			data.Stage = ((data.Stage <= 0) ? 1 : data.Stage);
			list.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(data.Stage)));
			list.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetLevelByRankName(data.Rank)));
			list.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)data.FiveElement)));
			list.Add(string.Format("{0},{1}", "施法消耗", data.SpiritualityValue));
			list.Add(string.Format("{0},{1}", "施法时间", data.CastTime / 1000 + "息"));
			list.Add(string.Format("{0},{1}", "灵衍值", data.ConversionRate.ToString("0.00")));
			list.Add(string.Format("{0},{1}", "创造人", Singleton<RoleManager>.Instance.Role_Entity.RoleName));
			return list;
		}

		private void LineCompeleteFail()
		{
			m_isRunning = false;
			CreateIsSuccess = 2;
			CreateMethodSignRefiningResultReq_1008052 data = new CreateMethodSignRefiningResultReq_1008052
			{
				IsSuccess = false,
				MethodSignId = m_CreateSuccessFuLuId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.FUSHU_CREATE_RESULT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					CreateMethodSignRefiningResultResp_1008052 createMethodSignRefiningResultResp_ = new CreateMethodSignRefiningResultResp_1008052();
					createMethodSignRefiningResultResp_.MergeFrom(byteString);
					if (createMethodSignRefiningResultResp_.IsSuccess)
					{
						HideOrShowAllPoint(isHide: true);
						CreateIsSuccess = 2;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = createMethodSignRefiningResultResp_.CurrentClever;
						string text = "这个笔画似乎没什么问题，只是绘制过程中出现了些许纰漏，再试一次吧";
						Singleton<UIManager>.Instance.PopUpWnd("SFH_CreateResultWnd.prefab", false, true, false, m_CurrentChooseStage + 4, text);
					}
					else
					{
						HideOrShowAllPoint(isHide: true);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createMethodSignRefiningResultResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
