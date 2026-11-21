using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ShiTuWnd : Window
	{
		private const string ApplyHint = "师尊须比你高至少 11 个小境界";
		private readonly List<TuDiEntity> m_Disciples = new List<TuDiEntity>();
		private readonly List<TuDiEntity> m_Applications = new List<TuDiEntity>();
		private ShiTuPanel m_Panel;
		private ShiTuArt m_Art;
		private GameObject m_DisciplePrefab;
		private GameObject m_ApplicationPrefab;
		private Timer m_RequestTimer;
		private int m_RequestNumber;
		private int m_PendingCount;
		private bool m_ShowApplications;
		private bool m_ManageDisciples;
		private bool m_HasMaster;
		private bool m_Greeted;
		private bool m_RelationsLoaded;
		private bool m_InboxLoaded;
		private string m_LastGraduation;
		private int m_ClaimType;
		private int m_ClaimAmount;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = GameObject.GetComponent<ShiTuPanel>();
			m_Art = new ShiTuArt(m_Panel);
			m_DisciplePrefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShiTuItem.prefab");
			m_ApplicationPrefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShoTuItem.prefab");

			AddButtonClickListener(m_Art.Back, CloseWindow);
			AddButtonClickListener(m_Panel.DaoYouButton, CloseWindow);
			AddButtonClickListener(m_Panel.WenHaoButton, ShowHelp);
			AddButtonClickListener(m_Panel.BaiShiButton, SendApplication);
			AddButtonClickListener(m_Panel.ChuShiButton, LeaveMaster);
			AddButtonClickListener(m_Panel.QingAnButton, GreetMaster);
			AddButtonClickListener(m_Panel.BaiShiListButton, ShowApplications);
			AddButtonClickListener(m_Art.Disciples, ShowDisciples);
			AddButtonClickListener(m_Art.Refresh, RefreshPage);
			AddButtonClickListener(m_Art.Manage, ToggleManage);
			AddButtonClickListener(m_Art.Journal, ShowJournal);
			m_Panel.NaneInput.onValueChanged.AddListener(OnNameChanged);
			m_Art.FormNote.text = ApplyHint;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			UpdateMasterCards();
			UpdateView();
			RefreshPage();
		}

		public override void OnDisable()
		{
			m_ClaimAmount = 0;
			EndRequest();
			base.OnDisable();
		}

		public override void OnClose()
		{
			m_Panel.NaneInput.onValueChanged.RemoveListener(OnNameChanged);
			base.OnClose();
		}

		public void HuoQuShiFuXIngXi()
		{
			ReadRelationships(false);
		}

		private void ReadRelationships(bool refreshInbox)
		{
			Request(MsgType.ENTER_SHITU, null, EnterShiTuResp_1014012.Parser, "正在读取师徒信息…", reply =>
			{
				if (!reply.IsSuccess)
				{
					SetStatus(Message(reply.Code, "暂时无法读取师徒信息，可点击刷新。"));
					return;
				}
				m_RelationsLoaded = true;
				m_PendingCount = reply.BaiShiTieNum;
				m_Art.SetGraduatedCount(reply.ChuShiNum);
				var master = reply.ShiFu;
				m_HasMaster = master != null && !string.IsNullOrEmpty(master.Name);
				m_Greeted = m_HasMaster && master.IsQingAn;
				if (m_HasMaster)
				{
					m_Panel.ShiFuToXiangImage.sprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(master.Sex, master.HeadPortraitId, master.IsOnLine);
					m_Panel.NameText.text = master.Name;
					m_Panel.GradeText.text = WorldSetting.GetRoleLevelNameByRoleLevel(master.Level) + (master.IsOnLine ? "  ·  在线" : "  ·  离线");
					Intimecy(master.Intimacy);
				}
				m_Disciples.Clear();
				foreach (var disciple in reply.TuDis)
				{
					m_Disciples.Add(new TuDiEntity
					{
						Id = disciple.Id,
						Name = disciple.Name,
						HeadPortraitId = disciple.HeadPortraitId,
						Level = disciple.Level,
						Intimacy = disciple.Intimacy,
						Sex = disciple.Sex ? 1 : 0,
						IsOnline = disciple.IsOnLine,
						IsTongMaiOrChuanGong = disciple.IsTongMaiOrChuanGong
					});
				}
				UpdateMasterCards();
				RenderDisciples();
				UpdateView();
				if (!reply.IsGetTmcg)
					ToZi(reply.TmcgType, reply.TmcgNum);
				else if (refreshInbox)
					ReadApplications();
			});
		}

		private void ShowDisciples()
		{
			m_ShowApplications = false;
			m_ManageDisciples = false;
			UpdateView();
			if (!m_RelationsLoaded)
				HuoQuShiFuXIngXi();
			else
				RenderDisciples();
		}

		private void ShowApplications()
		{
			m_ShowApplications = true;
			UpdateView();
			ReadApplications();
		}

		private void RefreshPage()
		{
			ReadRelationships(m_ShowApplications);
		}

		private void ReadApplications()
		{
			Request(MsgType.BAI_SHITIE, null, BaiShiTieResp_1014018.Parser, "正在读取拜师帖…", reply =>
			{
				if (!reply.IsSuccess)
				{
					SetStatus(Message(reply.Code, "暂时无法读取拜师帖，可点击刷新。"));
					return;
				}
				m_InboxLoaded = true;
				m_Applications.Clear();
				foreach (var applicant in reply.BaiShiDiZis)
				{
					m_Applications.Add(new TuDiEntity
					{
						Id = applicant.Id,
						Name = applicant.Name,
						HeadPortraitId = applicant.HeadPortraitId,
						Level = applicant.Level,
						Sex = applicant.Sex ? 1 : 0
					});
				}
				m_PendingCount = m_Applications.Count;
				RenderApplications();
				UpdateView();
			});
		}

		private void ToggleManage()
		{
			m_ManageDisciples = !m_ManageDisciples;
			RenderDisciples();
			UpdateView();
		}

		private void SendApplication()
		{
			string name = m_Panel.NaneInput.text.Trim();
			if (name.Length == 0 || m_HasMaster)
				return;
			if (string.Equals(Singleton<RoleManager>.Instance.Role_Entity?.RoleName, name, StringComparison.Ordinal))
			{
				m_Art.FormNote.text = "不能向自己递帖。";
				return;
			}

			Request(MsgType.BAI_SHI, new BaiShiReq_1014021 { RoleName = name }, BaiShiResp_1014021.Parser,
				"正在递交拜师帖…", reply =>
				{
					if (!ActionSucceeded(reply.IsSuccess, reply.Code, 4570, "递帖失败，请重试。"))
						return;
					m_Panel.NaneInput.text = string.Empty;
					m_Art.FormNote.text = $"已向{name}递帖，等待对方处理";
				});
		}

		private void AcceptApplication(string id)
		{
			Request(MsgType.SHOUTU, new ShouTuReq_1014020 { RoleId = id }, ShouTuResp_1014020.Parser,
				"正在收徒…", reply =>
				{
					if (!ActionSucceeded(reply.IsSuccess, reply.Code, 4568, "收徒失败，请刷新拜师帖。"))
						return;
					RemoveApplication(id);
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("已收为弟子");
					HuoQuShiFuXIngXi();
				});
		}

		private void RejectApplication(string id)
		{
			Request(MsgType.BAI_SHI_HU_LUE, new BaiShiHuLueReq_1014023 { RoleId = id }, BaiShiHuLueResp_1014023.Parser,
				"正在处理拜师帖…", reply =>
				{
					if (!ActionSucceeded(reply.IsSuccess, reply.Code, 4576, "处理失败，请刷新拜师帖。"))
						return;
					RemoveApplication(id);
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("已婉拒拜师帖");
				});
		}

		private void RemoveApplication(string id)
		{
			m_Applications.RemoveAll(entry => entry.Id == id);
			m_PendingCount = m_Applications.Count;
			RenderApplications();
			UpdateView();
		}

		private void LeaveMaster()
		{
			if (!m_HasMaster)
				return;
			MonoSingleton<GameStart>.Instance.PopUpAttention("离开师门后，三天内无法再次拜师。确定离开吗？", () =>
			{
				Request(MsgType.TUO_LISHIMEN, null, TuoLiShiMenResp_1014022.Parser, "正在离开师门…", reply =>
				{
					if (!ActionSucceeded(reply.IsSuccess, reply.Code, 4574, "离开师门失败，请刷新后重试。"))
						return;
					m_HasMaster = false;
					m_Art.FormNote.text = "三天内无法再次递交拜师帖";
					UpdateMasterCards();
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("已离开师门");
					HuoQuShiFuXIngXi();
				});
			});
		}

		private void GreetMaster()
		{
			if (m_HasMaster && !m_Greeted && m_RequestTimer == null)
				Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, ShiTuToZiList.QingAn);
		}

		public void Intimecy(int intimacy)
		{
			m_Panel.IntimacyText.text = $"亲密 {intimacy}";
			m_Panel.IntimacyGradeText.text = intimacy >= 100 ? "亲传弟子" : "记名弟子";
		}

		internal void MarkGreeting(int intimacy)
		{
			if (GameObject == null || !GameObject.activeInHierarchy)
				return;
			Intimecy(intimacy);
			m_Greeted = true;
			UpdateMasterCards();
		}

		public void ToZi(int type, int amount)
		{
			if (GameObject == null || !GameObject.activeInHierarchy || amount <= 0 || (type != 1 && type != 2))
				return;
			bool meridians = type == 1;
			var action = meridians ? ShiTuToZiList.ToMai : ShiTuToZiList.ChuangGong;
			string message = meridians ? $"师尊为你通脉，确认领取 {amount} 点经脉点。" : $"师尊为你传功，确认领取 {amount:N0} 点修为。";
			MonoSingleton<GameStart>.Instance.PopUpAttention($"{ShiTuToZiWnd.RitualDialogue(action)}\n\n{message}", () =>
			{
				m_ClaimType = type;
				m_ClaimAmount = amount;
			}, title: meridians ? "师门通脉" : "师门传功");
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			// A push may arrive while a panel refresh is still waiting for its reply.
			if (m_ClaimAmount <= 0 || m_RequestTimer != null)
				return;
			int amount = m_ClaimAmount;
			bool meridians = m_ClaimType == 1;
			m_ClaimAmount = 0;
			Request(MsgType.JIE_SHOU_TMCG, null, JieShouTMCGResp_1014025.Parser, "正在领取…", reply =>
			{
				if (!ActionSucceeded(reply.IsSuccess, reply.Code, 4581, "暂无法领取，请稍后重试。"))
					return;
				if (m_HasMaster)
					Intimecy(reply.Intimacy);
				var action = meridians ? ShiTuToZiList.ToMai : ShiTuToZiList.ChuangGong;
				string line = ShiTuToZiWnd.RitualDialogue(action);
				string result = meridians ? $"已领取 {amount} 点经脉点。" : $"已领取 {amount:N0} 点修为。";
				MentorJournal.Append(meridians ? "领取通脉" : "领取传功", $"{line}\n{result}");
				MentorPracticeMotion.Play((RectTransform)m_Panel.transform, action, ShiTuToZiWnd.RitualCaption(action),
					() => MonoSingleton<GameStart>.Instance.PopRollingTextShow(result));
				if (m_ShowApplications)
					ReadApplications();
			});
		}

		private void RenderDisciples()
		{
			ClearRows(m_Panel.TudiListRectTransform);
			for (int index = 0; index < m_Disciples.Count; index++)
			{
				var item = SetListContentParent(m_Panel.TudiListRectTransform, m_DisciplePrefab).GetComponent<ShiTuItem>();
				if (m_ManageDisciples)
					item.ZuChuSetValue(index, m_Disciples[index]);
				else
					item.SetValue(index, m_Disciples[index]);
			}
			ResizeList(m_Panel.TudiListRectTransform, m_Disciples.Count);
		}

		private void RenderApplications()
		{
			ClearRows(m_Panel.PlayerInfoRect);
			for (int index = 0; index < m_Applications.Count; index++)
			{
				var item = SetListContentParent(m_Panel.PlayerInfoRect, m_ApplicationPrefab).GetComponent<ShoTuItem>();
				item.SetValue(index, m_Applications[index], AcceptApplication, RejectApplication);
			}
			ResizeList(m_Panel.PlayerInfoRect, m_Applications.Count);
		}

		private static void ClearRows(RectTransform content)
		{
			foreach (Transform row in content)
			{
				row.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(row.gameObject);
			}
		}

		private static void ResizeList(RectTransform content, int count)
		{
			content.sizeDelta = new Vector2(content.sizeDelta.x, count * ShiTuArt.RowPitch);
			content.anchoredPosition = Vector2.zero;
		}

		private void Request<T>(MsgType command, IMessage data, MessageParser<T> parser, string progress, Action<T> callback)
			where T : class, IMessage<T>
		{
			if (GameObject == null || !GameObject.activeInHierarchy || m_RequestTimer != null)
				return;
			NetManager network = Singleton<NetManager>.Instance;
			if (!network.GameServerIsAlive())
			{
				SetStatus("游戏服连接已断开，重连后可刷新。");
				return;
			}

			int request = ++m_RequestNumber;
			m_RequestTimer = Timer.Register(8f, () =>
			{
				EndRequest();
				SetStatus("未收到回执，可点击刷新确认结果。");
				UpdateView();
			}, useRealTime: true);
			SetStatus(progress);
			UpdateView();
			network.SendMsg(command, data, response =>
			{
				if (request != m_RequestNumber || GameObject == null || !GameObject.activeInHierarchy)
					return;
				EndRequest();
				UpdateView();
				if (!(response is ByteString bytes))
				{
					SetStatus("未收到回执，可点击刷新。");
					return;
				}
				T reply;
				try
				{
					reply = parser.ParseFrom(bytes);
					if (command == MsgType.ENTER_SHITU)
						m_LastGraduation = ReadGraduation(bytes);
				}
				catch (InvalidProtocolBufferException)
				{
					SetStatus("未能读取服务器回执，可点击刷新。");
					return;
				}
				SetStatus(string.Empty);
				callback(reply);
			}, false, ServerType.GamesNormalServer, false);
		}

		// Older servers omit the two optional graduation fields on the mentor panel.
		private static string ReadGraduation(ByteString bytes)
		{
			var input = bytes.CreateCodedInput();
			string name = null;
			int intimacy = 0;
			uint tag;
			while ((tag = input.ReadTag()) != 0)
			{
				switch (tag)
				{
				case 162: name = input.ReadString(); break; // Field 20, former teacher name.
				case 168: intimacy = input.ReadInt32(); break; // Field 21, final intimacy.
				default: input.SkipLastField(); break;
				}
			}
			return string.IsNullOrEmpty(name) ? null : $"已从{name}门下出师 · 亲密 {intimacy}";
		}

		private void EndRequest()
		{
			++m_RequestNumber;
			Timer.Cancel(m_RequestTimer);
			m_RequestTimer = null;
		}

		internal static bool ActionSucceeded(bool isSuccess, int code, int expected, string fallback)
		{
			if (isSuccess && (!Constants.UsesPackagedTrialData || code == expected))
				return true;
			MonoSingleton<GameStart>.Instance.PopUpAttention(Message(code, fallback));
			return false;
		}

		private static string Message(int code, string fallback)
		{
			if (Constants.UsesPackagedTrialData && code == 2000)
				return "当前修真界暂无法处理此操作。";
			if (Constants.UsesPackagedTrialData && code == 4577)
				return "当前没有待领取的师门收益，可刷新师徒页面确认。";
			if (Constants.UsesPackagedTrialData && code == 4580)
				return "该弟子今日已受指导，或还有收益待领。";
			if (Constants.UsesPackagedTrialData && code == 4584)
				return "亲密达到 300 后才能出师，请刷新查看当前亲密。";
			if (code == 4008)
				return fallback;
			string hint = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code);
			return string.IsNullOrEmpty(hint) || hint.StartsWith("Error Code", StringComparison.Ordinal) ? fallback : hint;
		}

		private void SetStatus(string value)
		{
			m_Art.Status.text = value;
		}

		private void OnNameChanged(string value)
		{
			if (value.Length != 0)
				m_Art.FormNote.text = ApplyHint;
			UpdateButtons();
		}

		private void UpdateMasterCards()
		{
			m_Panel.ShiFuBeiJinImage.gameObject.SetActive(m_HasMaster);
			m_Panel.WuShiFuImage.gameObject.SetActive(!m_HasMaster);
			m_Art.FormTitle.text = m_RelationsLoaded ? (m_LastGraduation ?? "尚未拜师") : "拜师名帖";
			ShiTuArt.ButtonStyle(m_Panel.QingAnButton, m_Greeted ? "已请安" : "请安", !m_Greeted);
			UpdateButtons();
		}

		private void UpdateView()
		{
			m_Art.SetTab(m_ShowApplications, m_Disciples.Count, m_PendingCount, m_ManageDisciples);
			bool empty = m_ShowApplications ? m_Applications.Count == 0 : m_Disciples.Count == 0;
			m_Art.Empty.gameObject.SetActive(empty);
			m_Art.Empty.text = m_ShowApplications
				? (m_InboxLoaded ? "暂无拜师帖" : string.Empty)
				: (m_RelationsLoaded ? "尚未收徒\n收到的拜师帖可在上方查看" : string.Empty);
			UpdateButtons();
		}

		private void UpdateButtons()
		{
			bool ready = m_RequestTimer == null;
			m_Panel.NaneInput.interactable = ready && m_RelationsLoaded && !m_HasMaster;
			m_Panel.BaiShiButton.interactable = m_Panel.NaneInput.interactable && !string.IsNullOrWhiteSpace(m_Panel.NaneInput.text);
			m_Panel.QingAnButton.interactable = ready && !m_Greeted;
			m_Panel.ChuShiButton.interactable = ready;
			m_Panel.BaiShiListButton.interactable = ready;
			m_Art.Disciples.interactable = ready;
			m_Art.Refresh.interactable = ready;
			if (m_Art.Manage != null)
				m_Art.Manage.interactable = ready && m_Disciples.Count > 0;
		}

		private static void ShowHelp()
		{
			string copy = "每位修士可拜一人为师，师尊须比你高至少 11 个小境界，最多收 3 名弟子。\n\n"
				+ "你可以每天向师尊请安一次。双方在线或相聚同城时，获得的亲密更多。亲密达到 100 后晋为亲传弟子。\n\n"
				+ "离开师门后，三天内不能再次拜师。";
			copy += $"\n\n传功\n{ShiTuToZiWnd.PowerRules}\n\n通脉\n{ShiTuToZiWnd.MeridianRules}\n\n"
				+ $"授业次数与亲密\n{ShiTuToZiWnd.PracticeRules}\n\n出师\n{ShiTuToZiWnd.GraduationRules}";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, copy, "师徒规则");
		}

		private static void ShowJournal()
		{
			string entries = MentorJournal.ReadText();
			string content = string.IsNullOrEmpty(entries) ? "本机暂无记录。完成师徒互动后，可在这里重读对话和结果。" : entries;
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, content, "师门札记 · 本机");
		}
	}
}
