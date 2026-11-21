using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class HuaFuWnd : Window
	{
		public HuaFuPanel Panel;

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

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			if (paralist.Length != 0)
			{
				m_CurrentChooseModel = paralist[0] as FuLuEntity;
			}
			m_isRunning = false;
			m_CanReceiveInput = false;
			m_CurrentLineIndex = 0;
			m_PatingStatus = PatingStatus.CanContiue;
			Panel = base.GameObject.GetComponent<HuaFuPanel>();
			m_LineItemList = new List<HuaFuLineItem>();
			m_LineItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/HuaFuLineItem.prefab");
			AddButtonClickListener(Panel.LiKaiBtn, LiKai);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			AddButtonClickListener(Panel.Success_CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			});
			AddButtonClickListener(Panel.Success_QueDingBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			});
			AddButtonClickListener(Panel.Fail_AgainBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			});
			AddButtonClickListener(Panel.Fail_CloseBtn, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
			});
			m_CurrentChooseStage = 1;
			m_RecatingTime = JiSuanShakeSpeed();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			InitPoint();
			WanChengBtnClick();
		}

		private void InitPoint()
		{
			List<RectTransform> list = new List<RectTransform>();
			foreach (FuShuPoint point in m_CurrentChooseModel.Points)
			{
				GameObject gameObject = Object.Instantiate(Panel.TempleItem, Panel.HuaBuRect);
				gameObject.transform.SetParent(Panel.HuaBuRect);
				RectTransform component = gameObject.gameObject.GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2((float)point.x, (float)point.y);
				list.Add(component);
			}
			m_LineItemList.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				HuaFuLineItem component2 = Object.Instantiate(m_LineItem).GetComponent<HuaFuLineItem>();
				if (m_CurrentChooseModel.Points.Count > i + 1)
				{
					Vector3 selfPos = new Vector3(list[i].transform.position.x, list[i].transform.position.y, -0.8f);
					Vector3 targetPoint = new Vector3(list[i + 1].transform.position.x, list[i + 1].transform.position.y, -0.8f);
					component2.SetLine(i, selfPos, targetPoint, LineCompelete);
				}
				else
				{
					Vector3 vector = new Vector3(list[i].transform.position.x, list[i].transform.position.y, -0.8f);
					component2.SetLine(i, vector, vector, LineCompelete, isEnd: true);
				}
				m_LineItemList.Add(component2);
			}
			foreach (RectTransform item in list)
			{
				Object.Destroy(item.gameObject);
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_PatingStatus == PatingStatus.CanContiue || m_PatingStatus == PatingStatus.Running)
			{
				GetInputDirection();
			}
		}

		private void GetInputDirection()
		{
			if (Input.GetMouseButtonDown(0))
			{
				m_ActiveInput = true;
				m_MousePos = Input.mousePosition;
			}
			if (Input.GetMouseButton(0) && m_ActiveInput && Vector3.Distance(Input.mousePosition, m_MousePos) > 5f)
			{
				Vector3 moveNormalized = Input.mousePosition - m_MousePos;
				float angle = GetAngle(m_MousePos, Input.mousePosition, moveNormalized);
				if (m_CanReceiveInput && m_PatingStatus == PatingStatus.CanContiue)
				{
					Vector3 position = m_LineItemList[m_CurrentLineIndex + 1].transform.position;
					Vector3 position2 = m_LineItemList[m_CurrentLineIndex].transform.position;
					Vector3 moveNormalized2 = position - position2;
					float angle2 = GetAngle(position, position2, moveNormalized2);
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

		private InputDirection PointChangeEnum(Vector2 OnePoint, Vector2 TwoPoint)
		{
			InputDirection result = InputDirection.NULL;
			Vector3 vector = OnePoint - TwoPoint;
			if (vector.magnitude > 0f)
			{
				float num = Mathf.Acos(Vector3.Dot(Vector3.up, vector.normalized)) * 57.29578f;
				float num2 = Mathf.Acos(Vector3.Dot(Vector3.right, vector.normalized)) * 57.29578f;
				if (num <= 22.5f)
				{
					result = InputDirection.Up;
				}
				else if (num >= 157.5f)
				{
					result = InputDirection.Down;
				}
				else if (num2 <= 22.5f)
				{
					result = InputDirection.Right;
				}
				else if (num2 >= 157.5f)
				{
					result = InputDirection.Left;
				}
				else if (num2 > 22.5f && num2 < 90f && num > 22.5f && num < 67.5f)
				{
					result = InputDirection.UpRght;
				}
				else if (num2 > 22.5f && num2 < 90f && num >= 112.5f && num <= 157.5f)
				{
					result = InputDirection.DownRight;
				}
				else if (num2 > 112.5f && num2 < 157.5f && num > 22.5f && num < 67.5f)
				{
					result = InputDirection.UpLeft;
				}
				else if (num2 > 112.5f && num2 < 157.5f && num >= 112.5f && num <= 157.5f)
				{
					result = InputDirection.DownLeft;
				}
			}
			return result;
		}

		private void HuaFuLineSendToMsg()
		{
			if (m_CurrentChooseModel.Name != "练习用符书")
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.HUAFU_CHECK, null, null);
			}
		}

		private float GetAngle(Vector3 v1, Vector3 v2, Vector3 MoveNormalized)
		{
			if (MoveNormalized.normalized.y < 0f)
			{
				return 0f - Vector3.Angle(Vector3.right, MoveNormalized);
			}
			return Vector3.Angle(Vector3.right, MoveNormalized);
		}

		private void LiKai()
		{
			ClearPointImg();
			Singleton<UIManager>.Instance.CloseWnd("HuaFuWnd.prefab");
		}

		public void ClearPanelBtnClick()
		{
			if (!m_isRunning)
			{
				ClearPointImg();
			}
		}

		private void WenHaoTogClick()
		{
			string text = $"符师封印法术入符纸，画符笔画长短方向内蕴天地法则。依照笔画次序，沿对应方向划过，即可控制运笔，注意转折时机，如此一气呵成，方可成符。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "画符静室");
		}

		public void WanChengBtnClick()
		{
			if (!m_isRunning)
			{
				m_PatingStatus = PatingStatus.CanContiue;
				m_CanReceiveInput = true;
				m_isRunning = true;
			}
		}

		private void LineCompelete(int index)
		{
			if (m_LineItemList.Count > index + 2)
			{
				m_PatingStatus = PatingStatus.CanContiue;
				m_CanReceiveInput = true;
				if (Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal != 100 && m_CurrentChooseModel.Name == "练习用符书")
				{
					HuaFuPanDuan(index + 4);
					return;
				}
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

		private void LineCompeleteSuccess()
		{
			RefiningSignReq_1005036 data = new RefiningSignReq_1005036
			{
				IsRefiningSuccess = true,
				MethodSignId = m_CurrentChooseModel.Id
			};
			if (m_CurrentChooseModel.Name == "练习用符书")
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal != 100)
				{
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
					GuideInfoUpdateReq_1002013 guideInfoUpdateReq_ = new GuideInfoUpdateReq_1002013();
					guideInfoUpdateReq_.Type = 7;
					Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_GUIDE, guideInfoUpdateReq_, (object _obj) =>
					{
					});
					Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal = 100;
				}
				MonoSingleton<GameStart>.Instance.PopUpAttention("您已完成学习，还请日后勤加练习，早日成为一代宗师!", () =>
				{
					Singleton<UIManager>.Instance.CloseWnd(this);
				});
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.REFINING_SIGN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RefiningSignResp_1005036 refiningSignResp_ = new RefiningSignResp_1005036();
					refiningSignResp_.MergeFrom(byteString);
					if (refiningSignResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(refiningSignResp_.LiveLevel, refiningSignResp_.LiveProficiency, RoleFuZhiType.ZhiFu);
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningSignResp_.CurrentVigor;
						string levelByRankName = WorldSetting.GetLevelByRankName(refiningSignResp_.Rank);
						PopSuccessBg(levelByRankName, m_CurrentChooseModel.Name, (m_CurrentChooseModel.Stage + 4).ToString());
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						Singleton<UIManager>.Instance.CloseWnd(this);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(refiningSignResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
			m_PatingStatus = PatingStatus.Compelete;
			m_isRunning = false;
		}

		private void LineCompeleteFail()
		{
			RefiningSignReq_1005036 data = new RefiningSignReq_1005036
			{
				IsRefiningSuccess = false,
				MethodSignId = m_CurrentChooseModel.Id
			};
			if (m_CurrentChooseModel.Name == "练习用符书")
			{
				m_isRunning = false;
				m_CanReceiveInput = false;
				m_CurrentLineIndex = 0;
				m_PatingStatus = PatingStatus.CanContiue;
				m_CurrentChooseStage = 1;
				m_RecatingTime = JiSuanShakeSpeed();
				InitPoint();
				WanChengBtnClick();
				HuaFuPanDuan(3);
			}
			else
			{
				if (!(m_CurrentChooseModel.Name != "练习用符书"))
				{
					return;
				}
				Singleton<NetManager>.Instance.SendMsg(MsgType.REFINING_SIGN, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						RefiningSignResp_1005036 refiningSignResp_ = new RefiningSignResp_1005036();
						refiningSignResp_.MergeFrom(byteString);
						if (refiningSignResp_.IsSuccess)
						{
							Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningSignResp_.CurrentVigor;
							PopFailBg((m_CurrentChooseModel.Stage + 4).ToString());
						}
						else
						{
							Singleton<UIManager>.Instance.CloseWnd(this);
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(refiningSignResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				m_isRunning = false;
			}
		}

		public void PopSuccessBg(string pinJie, string FuName, string JingLi)
		{
			Panel.Success_TitleText.text = $"恭喜！成功获取“{pinJie}”{FuName}";
			Panel.Success_MiddleText.text = $"你消耗了{JingLi}点精力";
			Panel.SuccessBg.gameObject.SetActive(value: true);
		}

		public void PopFailBg(string jingLiXiaoHao)
		{
			Panel.FailJingLiText.text = $"你消耗了{jingLiXiaoHao}点精力";
			Panel.FailBg.gameObject.SetActive(value: true);
		}

		public void ClearPointImg()
		{
			m_CurrentLineIndex = 1;
			foreach (HuaFuLineItem lineItem in m_LineItemList)
			{
				Object.Destroy(lineItem.gameObject);
			}
			m_LineItemList.Clear();
		}

		private double JiSuanShakeSpeed()
		{
			if (m_CurrentChooseModel == null)
			{
				return 0.30000001192092896;
			}
			double num = (double)(500f * Mathf.Pow(4f, m_CurrentChooseModel.Stage) / (float)((m_CurrentChooseModel.Stage - 4 < 1) ? 1 : (m_CurrentChooseModel.Stage - 4))) / (double)Singleton<RoleManager>.Instance.Role_Entity.CurrentPerception;
			CoefficientNumEntity coefficientNumEntity = Singleton<FacadeManager>.Instance.SelectHuaFuCoefficientNum(m_CurrentChooseModel.Rank, m_CurrentChooseModel.Stage);
			return num + coefficientNumEntity.CoefficientNum;
		}

		private void HuaFuPanDuan(int price)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal != 100 && m_CurrentChooseModel.Name == "练习用符书")
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 7;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = price;
				Singleton<GuideManager>.Instance.ShowGuideNew(7);
			}
		}
	}
}
