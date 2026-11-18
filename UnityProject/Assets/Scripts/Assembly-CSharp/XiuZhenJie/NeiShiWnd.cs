using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class NeiShiWnd : Window
	{
		public NeiShiPanel Panel;

		private UnityAction m_TongXueAction;

		private Dictionary<string, XueWeiModel> m_DataSource;

		private AcupointModel m_AcupointModel;

		private int m_PointNum;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<NeiShiPanel>();
			AddButtonClickListener(Panel.TiShiPanel_TongXueBtn, TiShiPanel_TongXueBtn);
			AddButtonClickListener(Panel.LiKai_Btn, LiKaiClick);
			AddButtonClickListener(Panel.TiShiPanel_CloseBtn, ClosePanel);
			AddButtonClickListener(Panel.TiShiPanel_WaitBtn, ClosePanel);
			AddButtonClickListener(Panel.ZongLan_Btn, OpenZongLanWnd);
			AddButtonClickListener(Panel.WenHao, BuQuanWenHaoTogClick);
			m_DataSource = new Dictionary<string, XueWeiModel>();
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: false);
			Singleton<NetManager>.Instance.SendMsg(MsgType.OPEN_ACUPOINT, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					OpenAcupointResp_1005016 openAcupointResp_ = new OpenAcupointResp_1005016();
					openAcupointResp_.MergeFrom(byteString);
					if (openAcupointResp_.IsSuccess)
					{
						Panel.NengLianText.text = $"剩余 {openAcupointResp_.AcupointValue} 点经脉点";
						List<XueWeiModel> list = new List<XueWeiModel>();
						if (openAcupointResp_.Acupoints != null)
						{
							foreach (OpenAcupointResp_1005016.Types.RoleAcupointResp_1005017 acupoint in openAcupointResp_.Acupoints)
							{
								XueWeiModel xueWeiModel = new XueWeiModel
								{
									a = acupoint.Id,
									b = acupoint.BrokenLevel
								};
								list.Add(xueWeiModel);
								m_DataSource.Add(xueWeiModel.a, xueWeiModel);
							}
						}
						MonoSingleton<JingMaiManager>.Instance.SetUsedXueWei(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(openAcupointResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
			JingMaiYinDaoPanDuan();
		}

		private void ReflashJingMaiPoint(int Point)
		{
			m_PointNum = Point;
			Panel.NengLianText.text = $"剩余 {Point} 点经脉点";
		}

		private void OpenZongLanWnd()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongLanWnd.prefab", true);
		}

		private void LiKaiClick()
		{
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DongFuScene");
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: true);
		}

		private void ClosePanel()
		{
			Panel.TiShiPanel.gameObject.SetActive(value: false);
		}

		public void ShowPanel(JingMaiItem JingMaiId, AcupointModel jingMai, UnityAction TongXueAction)
		{
			if (JingMaiId == null || jingMai == null)
			{
				return;
			}
			m_AcupointModel = jingMai;
			m_TongXueAction = TongXueAction;
			Panel.TiShiPanel.gameObject.SetActive(value: true);
			Panel.TiShiPanel_TitleText.text = jingMai.b;
			if (JingMaiId.CurrentStatus == XueWeiStatus.Disable)
			{
				Panel.TiShiPanel_TongXueBtn.interactable = false;
			}
			else
			{
				Panel.TiShiPanel_TongXueBtn.interactable = true;
			}
			XueWeiModel value = null;
			if (m_DataSource.TryGetValue(jingMai.a, out value))
			{
				Panel.TiShiPanel_DangQianCengShuText.text = value.b + "层";
				Panel.TiShiPanel_DangQianXiaoGuoText.text = GetXueWeiMiaoShu(jingMai, value.b);
				Panel.TiShiPanel_TuPoHouXiaoGuoText.text = GetXueWeiMiaoShu(jingMai, value.b + 1);
				Panel.TiShiPanel_GuiMaiText.text = jingMai.e;
				if (value.b == 3)
				{
					Panel.TiShiPanel_TongXueBtn.interactable = false;
					Panel.TiShiPanel_TuPoXiaoHaoText.text = $"<color=#A00200>{0}</color>经脉点";
					Panel.TiShiPanel_ChengGongJiLvText.text = $"<color=#A00200>{0}%</color>";
					ShowXiaoGuoContent(jingMai, 3);
					return;
				}
				if (value.b == 2)
				{
					Panel.TiShiPanel_TuPoXiaoHaoText.text = $"<color=#A00200>{jingMai.i * 5}</color>经脉点";
				}
				else if (value.b == 1)
				{
					Panel.TiShiPanel_TuPoXiaoHaoText.text = $"<color=#A00200>{jingMai.i * 3}</color>经脉点";
				}
				else if (value.b == 0)
				{
					Panel.TiShiPanel_TuPoXiaoHaoText.text = $"<color=#A00200>{jingMai.i}</color>经脉点";
				}
				if (value.b == 0)
				{
					Panel.TiShiPanel_ChengGongJiLvText.text = $"<color=#A00200>{100}%</color>";
				}
				else
				{
					int num = (10 - (1 + 4 * value.b)) * 10;
					if (Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic.ContainsKey(14))
					{
						ChengHaoInfoEntity chengHaoInfoEntity = Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic[14];
						num = ((chengHaoInfoEntity.Id == 801) ? (num + 20) : ((chengHaoInfoEntity.Id != 802) ? (num + 5) : (num + 10)));
					}
					if (num > 100)
					{
						num = 100;
					}
					Panel.TiShiPanel_ChengGongJiLvText.text = $"<color=#A00200>{num}%</color>";
				}
				ShowXiaoGuoContent(jingMai, value.b + 1);
			}
			else
			{
				Panel.TiShiPanel_DangQianCengShuText.text = "<color=#A00200>0</color>层";
				Panel.TiShiPanel_GuiMaiText.text = jingMai.e;
				Panel.TiShiPanel_TuPoXiaoHaoText.text = $"<color=#A00200>{jingMai.i}</color>经脉点";
				Panel.TiShiPanel_ChengGongJiLvText.text = $"<color=#A00200>{100}%</color>";
				ShowXiaoGuoContent(jingMai, 1);
			}
		}

		private void ShowXiaoGuoContent(AcupointModel jingMai, int level)
		{
			string[] array = jingMai.e.Split(',');
			string text = "";
			string[] array2 = array;
			foreach (string jingMai2 in array2)
			{
				text = text + GetXueWeiMiaoShu(jingMai2, level) + "\n";
			}
			Panel.TiShiPanel_XiaoGuoText.text = text;
		}

		private string GetXueWeiMiaoShu(AcupointModel jingMai, int level)
		{
			string result = "无";
			if (jingMai == null)
			{
				return result;
			}
			switch (jingMai.e)
			{
			case "任脉":
				result = "纳气速率提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "督脉":
				result = "纳气效率提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "冲脉":
				result = "精血上限提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "带脉":
				result = "精血恢复速度提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 200), 
					2 => string.Format(result, 500), 
					3 => string.Format(result, 2000), 
					_ => string.Format(result, 0), 
				};
			case "阴跷脉":
				result = "神识上限提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "阳跷脉":
				result = "五行伤害提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 1), 
					2 => string.Format(result, 2), 
					3 => string.Format(result, 3), 
					_ => string.Format(result, 0), 
				};
			case "阴维脉":
				result = "体魄上限提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "阳维脉":
				result = "五行防御提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 1), 
					2 => string.Format(result, 2), 
					3 => string.Format(result, 3), 
					_ => string.Format(result, 0), 
				};
			default:
				return "无";
			}
		}

		private string GetXueWeiMiaoShu(string jingMai, int level)
		{
			string result = "无";
			if (string.IsNullOrEmpty(jingMai))
			{
				return result;
			}
			switch (jingMai)
			{
			case "任脉":
				result = "纳气速率提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "督脉":
				result = "纳气效率提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "冲脉":
				result = "精血上限提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "带脉":
				result = "精血恢复速度提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 200), 
					2 => string.Format(result, 500), 
					3 => string.Format(result, 2000), 
					_ => string.Format(result, 0), 
				};
			case "阴跷脉":
				result = "神识上限提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "阳跷脉":
				result = "五行伤害提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 1), 
					2 => string.Format(result, 2), 
					3 => string.Format(result, 3), 
					_ => string.Format(result, 0), 
				};
			case "阴维脉":
				result = "体魄上限提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "阳维脉":
				result = "五行防御提升<color=#A00200>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 1), 
					2 => string.Format(result, 2), 
					3 => string.Format(result, 3), 
					_ => string.Format(result, 0), 
				};
			default:
				return "无";
			}
		}

		private bool IsCanTongXueMethod(int JianShaoJingMaiDian)
		{
			bool result = true;
			if (JianShaoJingMaiDian > m_PointNum)
			{
				return false;
			}
			return result;
		}

		private void TiShiPanel_TongXueBtn()
		{
			BreakAcupointReq_1005018 data = new BreakAcupointReq_1005018
			{
				AcupointId = m_AcupointModel.a
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BREAK_ACUPOINT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BreakAcupointResp_1005018 breakAcupointResp_ = new BreakAcupointResp_1005018();
					breakAcupointResp_.MergeFrom(byteString);
					if (breakAcupointResp_.IsSuccess)
					{
						ReflashJingMaiPoint(breakAcupointResp_.AcupointValue);
						if (breakAcupointResp_.BreakSuccess)
						{
							if (m_DataSource.ContainsKey(m_AcupointModel.a))
							{
								if (m_DataSource[m_AcupointModel.a].b == 3)
								{
									return;
								}
								m_DataSource[m_AcupointModel.a].b++;
							}
							else
							{
								m_DataSource.Add(m_AcupointModel.a, new XueWeiModel
								{
									a = m_AcupointModel.a,
									b = 1
								});
							}
							if (m_TongXueAction != null)
							{
								m_TongXueAction();
							}
						}
					}
					ClosePanel();
					ReFlashData();
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(breakAcupointResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void BuQuanWenHaoTogClick()
		{
			string text = $"1、经脉合计为8条，其中带脉只有一个穴位即带脉。 2、每条经脉完全突破后，会有额外的加成。 3、每个穴位可以突破3次。 4、穴位突破第一次成功率100%，第二次成功率50%，第三次成功率仅10%。 ";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "补全功法");
		}

		private void JingMaiYinDaoPanDuan()
		{
			if ((Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 2) & (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal == 2))
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 6;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 4;
				Singleton<GuideManager>.Instance.ShowGuideNew(6);
			}
			else if ((Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 2) & (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal == 1))
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 6;
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 2;
				Singleton<GuideManager>.Instance.ShowGuideNew(6);
			}
		}
	}
}
