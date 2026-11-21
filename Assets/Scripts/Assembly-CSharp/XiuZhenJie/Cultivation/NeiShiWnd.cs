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

		private bool m_AcupointsLoaded;
		private int m_AcupointBalance;
		private Timer m_BreakTimer;
		private MeridianAtlasPanel m_Atlas;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<NeiShiPanel>();
			InkCultivationPages.PaperDialog(Panel.transform, "TiShiPanel", Panel.TiShiPanel_TongXueBtn, Panel.TiShiPanel_WaitBtn);
			m_Atlas = MeridianAtlasPanel.Build(Panel);
			AddButtonClickListener(Panel.TiShiPanel_TongXueBtn, TiShiPanel_TongXueBtn);
			AddButtonClickListener(Panel.LiKai_Btn, LiKaiClick);
			AddButtonClickListener(Panel.TiShiPanel_CloseBtn, ClosePanel);
			AddButtonClickListener(Panel.TiShiPanel_WaitBtn, ClosePanel);
			AddButtonClickListener(Panel.ZongLan_Btn, OpenZongLanWnd);
			AddButtonClickListener(Panel.WenHao, BuQuanWenHaoTogClick);
			m_DataSource = new Dictionary<string, XueWeiModel>();
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: false);
			LoadAcupoints();
		}

		private void LoadAcupoints()
		{
			m_AcupointsLoaded = false;
			Singleton<NetManager>.Instance.SendMsg(MsgType.OPEN_ACUPOINT, null, response =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (GameObject == null || !GameObject.activeInHierarchy || !(response is ByteString bytes))
					return;
				var reply = OpenAcupointResp_1005016.Parser.ParseFrom(bytes);
				if (!reply.IsSuccess)
				{
					ShowResult(reply.Code);
					return;
				}
				m_DataSource.Clear();
				var levels = new List<XueWeiModel>();
				foreach (var point in reply.Acupoints)
				{
					var saved = new XueWeiModel { a = point.Id, b = point.BrokenLevel };
					m_DataSource[point.Id] = saved;
					levels.Add(saved);
				}
				MonoSingleton<JingMaiManager>.Instance.SetUsedXueWei(levels);
				ReflashJingMaiPoint(reply.AcupointValue);
				m_AcupointsLoaded = true;
				m_Atlas.Refresh();
				JingMaiYinDaoPanDuan();
			}, isShowWaitUI: true);
		}

		public override void OnDisable()
		{
			Timer.Cancel(m_BreakTimer);
			m_BreakTimer = null;
			base.OnDisable();
		}

		private void ReflashJingMaiPoint(int Point)
		{
			m_AcupointBalance = Point;
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
			if (!m_AcupointsLoaded || m_BreakTimer != null || JingMaiId == null || jingMai == null)
				return;

			m_Atlas.Refresh();
			m_AcupointModel = jingMai;
			m_TongXueAction = TongXueAction;
			m_DataSource.TryGetValue(jingMai.a, out XueWeiModel saved);
			int level = saved?.b ?? 0;
			int cost = level < 3 ? jingMai.i * (2 * level + 1) : 0;
			int chance = level == 0 ? 100 : 0;
			int linkBonus = 0;
			if (level == 1 || level == 2)
			{
				var titles = Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic;
				int bonus = titles != null && titles.TryGetValue(14, out ChengHaoInfoEntity title) ? title.EffectValue : 0;
				var neighbors = new HashSet<string>((jingMai.g + "," + jingMai.h).Split(','));
				foreach (string id in neighbors)
					if (m_DataSource.TryGetValue(id, out XueWeiModel neighbor) && neighbor.b > 0)
						linkBonus = System.Math.Min(20, linkBonus + 10);
				chance = System.Math.Min(100, 90 - 40 * level + bonus + linkBonus);
			}

			Panel.TiShiPanel.gameObject.SetActive(true);
			Panel.TiShiPanel_TitleText.text = jingMai.b;
			m_Atlas.SetRequirement(jingMai, JingMaiId.CurrentStatus == XueWeiStatus.Disable, level >= 3);
			Panel.TiShiPanel_TongXueBtn.interactable = level < 3 && m_AcupointBalance >= cost && JingMaiId.CurrentStatus != XueWeiStatus.Disable;
			Panel.TiShiPanel_DangQianCengShuText.text = level + "层";
			Panel.TiShiPanel_DangQianXiaoGuoText.text = GetXueWeiMiaoShu(jingMai, level);
			Panel.TiShiPanel_TuPoHouXiaoGuoText.text = GetXueWeiMiaoShu(jingMai, System.Math.Min(3, level + 1));
			Panel.TiShiPanel_GuiMaiText.text = jingMai.e;
			Panel.TiShiPanel_TuPoXiaoHaoText.text = level == 3 ? "已圆满" : $"{cost} 经脉点" + (m_AcupointBalance < cost ? "（不足）" : "");
			Panel.TiShiPanel_ChengGongJiLvText.text = level == 3 ? "已圆满" : $"{chance}%" + (linkBonus > 0 ? $"（合流 +{linkBonus}%）" : "");
			ShowXiaoGuoContent(jingMai, System.Math.Min(3, level + 1));
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			if (role.RoleLevel >= 2 && role.AcupointGuideVal == 1 && jingMai.a == "1" && level == 0)
			{
				if (Panel.TiShiPanel_TongXueBtn.interactable) Singleton<GuideManager>.Instance.ShowMeridianStep(3);
				else Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
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
			if (jingMai == null || string.IsNullOrEmpty(jingMai.e)) return "无";
			return string.Join("、", System.Array.ConvertAll(jingMai.e.Split(','), thread => GetXueWeiMiaoShu(thread, level)));
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
				result = "纳气速率提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "督脉":
				result = "纳气效率提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "冲脉":
				result = "精血上限提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "带脉":
				result = "精血恢复速度提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 200), 
					2 => string.Format(result, 500), 
					3 => string.Format(result, 2000), 
					_ => string.Format(result, 0), 
				};
			case "阴跷脉":
				result = "神识上限提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "阳跷脉":
				result = "五行伤害提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 1), 
					2 => string.Format(result, 2), 
					3 => string.Format(result, 3), 
					_ => string.Format(result, 0), 
				};
			case "阴维脉":
				result = "体魄上限提升<color=#F3D295>{0}</color>%";
				return level switch
				{
					1 => string.Format(result, 0.5), 
					2 => string.Format(result, 0.8), 
					3 => string.Format(result, 1), 
					_ => string.Format(result, 0), 
				};
			case "阳维脉":
				result = "五行防御提升<color=#F3D295>{0}</color>%";
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

		private void TiShiPanel_TongXueBtn()
		{
			if (!m_AcupointsLoaded || m_BreakTimer != null || m_AcupointModel == null || !Panel.TiShiPanel_TongXueBtn.interactable)
				return;
			if (!Singleton<NetManager>.Instance.GameServerIsAlive())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("游戏服连接已断开，重连后可重新查看内视。");
				return;
			}

			string id = m_AcupointModel.a;
			m_Atlas.Charge(id);
			UnityAction markUsed = m_TongXueAction;
			Panel.TiShiPanel_TongXueBtn.interactable = false;
			var timer = Timer.Register(8f, () =>
			{
				m_BreakTimer = null;
				m_Atlas.CancelCharge();
				ClosePanel();
				LoadAcupoints();
			}, useRealTime: true);
			m_BreakTimer = timer;
			var data = new BreakAcupointReq_1005018 { AcupointId = id };
			Singleton<NetManager>.Instance.SendMsg(MsgType.BREAK_ACUPOINT, data, response =>
			{
				if (m_BreakTimer != timer || !(response is ByteString bytes))
					return;
				Timer.Cancel(timer);
				m_BreakTimer = null;
				var reply = BreakAcupointResp_1005018.Parser.ParseFrom(bytes);
				if (reply.IsSuccess)
				{
					ReflashJingMaiPoint(reply.AcupointValue);
					if (reply.BreakSuccess)
					{
						m_DataSource[id] = new XueWeiModel { a = id, b = reply.BrokenLevel };
						markUsed?.Invoke();
					}
				}
				ClosePanel();
				if (reply.IsSuccess) m_Atlas.Resolve(id, reply.BreakSuccess, reply.BrokenLevel);
				else { m_Atlas.CancelCharge(); ShowResult(reply.Code); }
				if (reply.IsSuccess && reply.BreakSuccess && id == "1" && reply.BrokenLevel == 1)
					JingMaiYinDaoPanDuan(true);
			}, false, ServerType.GamesNormalServer, false);
		}

		private static void ShowResult(int code)
		{
			string message = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(message);
		}

		private void BuQuanWenHaoTogClick()
		{
			string text = $"升境可得经脉点。先通胞中，再沿脉络选择相邻穴位。初通必成，二层五成、三层一成。每处已通邻穴为冲高层再添一成，至多两成。失败会消耗本次经脉点。任脉缩短纳气周天，督脉增加每周天纳气所得。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "经脉");
		}

		private void JingMaiYinDaoPanDuan(bool showEcho = false)
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			if (role.RoleLevel < 2 || role.AcupointGuideVal == 100) return;
			var guide = Singleton<GuideManager>.Instance;
			if (role.AcupointGuideVal == 2) guide.ShowMeridianStep(4);
			else if (role.AcupointGuideVal == 1)
			{
				if (m_DataSource.ContainsKey("1"))
					guide.SaveMeridianCheckpoint(1, () => {
						void ShowOverviewHint()
						{
							if (GameObject != null && GameObject.activeInHierarchy && role.AcupointGuideVal == 2 &&
								!Panel.TiShiPanel.gameObject.activeSelf && Singleton<UIManager>.Instance.FindWndByName("ZongLanWnd.prefab") == null)
								guide.ShowMeridianStep(4);
						}
						if (showEcho) Timer.Register(2.65f, ShowOverviewHint, useRealTime: true, autoDestroyOwner: Panel);
						else ShowOverviewHint();
					});
				else if (m_AcupointBalance > 0) guide.ShowMeridianStep(2);
			}
		}
	}
}
