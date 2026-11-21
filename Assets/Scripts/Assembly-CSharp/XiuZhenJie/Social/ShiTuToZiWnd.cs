using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShiTuToZiWnd : Window
	{
		internal const string PowerRules = "师尊须高出至少一个大境界，且相差至少 10 个小境界。消耗 5 点精力，弟子可领取按自身小境界计算的修为，在线同城时翻倍。";
		internal const string MeridianRules = "师尊须高出至少一个大境界，亲密达到 100。消耗 20 点精力，弟子可领取 1 点经脉点，在线同城时为 2 点。";
		internal const string PracticeRules = "每名弟子每日只可接受一项指导，北京时间零点重置。收益未领取前无法再次指导。弟子离线、异地在线、同城在线时，亲密分别增加 5、10、20。";
		internal const string GraduationRules = "亲密达到 300 后，由师尊安排出师。出师空出弟子名额，双方保留记录，师尊获得 50 气运。累计出师人数首次达到 1、5、10 时解锁对应称号，另得 200、500、1000 气运。";

		private ShiTuToZiPanel m_Panel;
		private Button m_Confirm;
		private Button m_Close;
		private RectTransform m_Card;
		private Text m_Note;
		private Timer m_Request;
		private string m_DefaultNote;
		private string m_RoleId;
		private bool m_PlayingGraduation;
		private readonly List<string> m_GraduationTitles = new List<string>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = GameObject.GetComponent<ShiTuToZiPanel>();
			InkDialogArt.SetWash(Transform);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist == null || paralist.Length == 0 || !(paralist[0] is ShiTuToZiList action))
			{
				CloseWindow();
				return;
			}
			m_RoleId = paralist.Length > 1 ? paralist[1] as string : null;
			string blocked = paralist.Length > 2 ? paralist[2] as string : null;
			m_Panel.QingAnBgImage.gameObject.SetActive(false);
			m_Panel.ZhuChuBgImage.gameObject.SetActive(false);
			m_Panel.ChuShiBgImage.gameObject.SetActive(false);
			m_Panel.ChuangGongBgImage.gameObject.SetActive(false);
			m_Panel.ToMaiBgImage.gameObject.SetActive(false);

			if (action == ShiTuToZiList.QingAn)
			{
				ShowCard(m_Panel.QingAnBgImage, m_Panel.QingAnGuanBiButton, m_Panel.QingAnQueRenButton,
					"向师尊请安", "每天可以请安一次。亲密增长随双方是否在线、是否同城而变化。", "奉礼请安", Greet);
				return;
			}
			if (string.IsNullOrEmpty(m_RoleId))
			{
				CloseWindow();
				return;
			}
			if (action == ShiTuToZiList.ZhuChu)
			{
				ShowCard(m_Panel.ZhuChuBgImage, m_Panel.ZhuChuGuanBiButton, m_Panel.ZhuChuQueRenButton,
					"逐出师门", "确认后，你与这名弟子的师徒关系将解除。", "确认逐出", Expel);
				return;
			}
			switch (action)
			{
			case ShiTuToZiList.ChuShi:
				ShowCard(m_Panel.ChuShiBgImage, m_Panel.ChuShiGuanBiButton, m_Panel.ChuShiQueRenButton,
					"弟子出师", GraduationRules, "确认出师", Graduate, expanded: true, blocked: blocked);
				break;
			case ShiTuToZiList.ChuangGong:
				int powerVigor = Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor;
				ShowCard(m_Panel.ChuangGongBgImage, m_Panel.ChuangGongGuanBiButton, m_Panel.ChuangGongQueRenButton,
					"传功", $"{PowerRules}\n\n{PracticeRules}\n\n当前精力 {powerVigor} 点。", "确认传功", Teach,
					expanded: true, blocked: blocked ?? (powerVigor < 5 ? "精力不足，传功需要 5 点。" : null));
				break;
			case ShiTuToZiList.ToMai:
				int meridianVigor = Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor;
				ShowCard(m_Panel.ToMaiBgImage, m_Panel.ToMaiGuanBiButton, m_Panel.ToMaiQueRenButton,
					"通脉", $"{MeridianRules}\n\n{PracticeRules}\n\n当前精力 {meridianVigor} 点。", "确认通脉", OpenMeridians,
					expanded: true, blocked: blocked ?? (meridianVigor < 20 ? "精力不足，通脉需要 20 点。" : null));
				break;
			default:
				CloseWindow();
				break;
			}
		}

		public override void OnDisable()
		{
			Timer.Cancel(m_Request);
			m_Request = null;
			m_PlayingGraduation = false;
			m_GraduationTitles.Clear();
			base.OnDisable();
		}

		private void ShowCard(Image card, Button close, Button confirm, string title, string copy, string confirmLabel,
			UnityEngine.Events.UnityAction submit, bool expanded = false, string blocked = null)
		{
			card.gameObject.SetActive(true);
			InkDialogArt.SetPaper(card);
			Transform root = card.transform;
			Transform heading = root.Find("TitleImg");
			Text headingText = heading.GetComponentInChildren<Text>(true);
			ShiTuArt.HideGraphic(heading.Find("TitleBg"));
			InkInterfaceArt.Set(heading.GetComponent<Image>(), "LightPlaque", false);
			ShiTuArt.AtTop((RectTransform)heading, 12f, 12f, 9f, 38f);
			headingText.transform.SetParent(heading, false);
			InkDialogArt.Inset(headingText.rectTransform, 37f, 37f, 0f, 0f);
			headingText.text = title;
			ShiTuArt.StyleText(headingText, 18, InkInterfaceArt.Ink, TextAnchor.MiddleCenter);

			ShiTuArt.PlaceTop((RectTransform)close.transform, true, 14f, 10f, 32f, 34f);
			InkDialogArt.StyleButton(close, "Close", InkInterfaceArt.Ink);
			close.transform.SetAsLastSibling();
			AddButtonClickListener(close, CloseWindow);

			Transform content = root.Find("ContentImg");
			Transform tip = root.Find("TiShiImg");
			ShiTuArt.HideGraphic(content);
			ShiTuArt.HideGraphic(content.Find("BottomImg"));
			ShiTuArt.HideGraphic(tip);
			m_Note = tip.GetComponentInChildren<Text>(true);
			ShiTuArt.StyleText(m_Note, 14, InkInterfaceArt.Ink, expanded ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter);
			m_Note.horizontalOverflow = HorizontalWrapMode.Wrap;
			m_Note.lineSpacing = 1.2f;
			if (copy != null)
			{
				card.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, expanded ? 298f : 202f);
				content.gameObject.SetActive(false);
				ShiTuArt.AtTop((RectTransform)tip, 24f, 24f, 56f, expanded ? 163f : 66f);
				InkDialogArt.Inset(m_Note.rectTransform, 0f, 0f, 0f, 0f);
				m_Note.text = blocked == null ? copy : $"{copy}\n{blocked}";
				var button = (RectTransform)confirm.transform;
				button.SetParent(root, false);
				button.anchorMin = button.anchorMax = new Vector2(0.5f, 0f);
				button.pivot = new Vector2(0.5f, 0f);
				button.anchoredPosition = new Vector2(0f, 24f);
				button.sizeDelta = new Vector2(144f, 38f);
			}
			else
			{
				foreach (Text label in content.GetComponentsInChildren<Text>(true))
					label.color = InkInterfaceArt.Ink;
			}
			m_DefaultNote = m_Note.text;
			m_Confirm = confirm;
			m_Close = close;
			m_Card = card.rectTransform;
			close.interactable = true;
			confirm.interactable = blocked == null;
			ShiTuArt.ButtonStyle(confirm, blocked == null ? confirmLabel : "条件未满足", blocked == null);
			AddButtonClickListener(confirm, submit);
		}

		private void Greet()
		{
			Send(MsgType.QING_AN, null, QingAnResp_1014015.Parser, reply =>
			{
				string fallback = reply.Code == 4565 ? "今日已经向师尊请过安。" : "请安失败，请稍后再试。";
				if (!ShiTuWnd.ActionSucceeded(reply.IsSuccess, reply.Code, 4566, fallback))
				{
					if (reply.Code == 4565)
					{
						MainWindow?.HuoQuShiFuXIngXi();
						CloseWindow();
					}
					return;
				}
				MainWindow?.MarkGreeting(reply.Intimacy);
				CloseWindow();
				string result = $"已向师尊请安。当前亲密 {reply.Intimacy}。";
				MentorJournal.Append("请安", result);
				MonoSingleton<GameStart>.Instance.PopUpAttention(result, title: "师门请安");
			});
		}

		private void Expel()
		{
			var data = new ZhuChuShiMenReq_1014026 { RoleId = m_RoleId };
			Send(MsgType.ZHU_CHU_SHI_MEN, data, ZhuChuShiMenResp_1014026.Parser, reply =>
			{
				if (!ShiTuWnd.ActionSucceeded(reply.IsSuccess, reply.Code, 4583, "暂无法逐出，请刷新弟子列表。"))
					return;
				MainWindow?.HuoQuShiFuXIngXi();
				CloseWindow();
				MonoSingleton<GameStart>.Instance.PopUpAttention("已解除与这名弟子的师徒关系。");
			});
		}

		private void Graduate()
		{
			var data = new ChuShiReq_1014027 { RoleId = m_RoleId };
			Send(MsgType.CHU_SHI, data, ChuShiResp_1014027.Parser, reply =>
			{
				if (!ShiTuWnd.ActionSucceeded(reply.IsSuccess, reply.Code, 4585, "暂无法出师，请刷新弟子信息。"))
					return;
				MainWindow?.HuoQuShiFuXIngXi();
				Singleton<RoleManager>.Instance.Role_Entity.QiYun = reply.QiYun;
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				PlayReceipt(ShiTuToZiList.ChuShi, "出师", $"弟子已出师，弟子名额已空出。\n当前气运 {reply.QiYun}。");
			});
		}

		private void Teach()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor < 5)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前精力不足 5 点，无法传功。");
				return;
			}
			var data = new ChuanGongReq_1014017 { RoleId = m_RoleId };
			Send(MsgType.CHAN_GONG, data, ChuanGongResp_1014017.Parser,
				reply => FinishTraining(reply.IsSuccess, reply.Code, reply.CurrentVigor, reply.Intimacy, $"{reply.TuDiSuperAir:N0} 点修为", false));
		}

		private void OpenMeridians()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor < 20)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前精力不足 20 点，无法通脉。");
				return;
			}
			var data = new TongMaiReq_1014016 { RoleId = m_RoleId };
			Send(MsgType.TONG_MAI, data, TongMaiResp_1014016.Parser,
				reply => FinishTraining(reply.IsSuccess, reply.Code, reply.CurrentVigor, reply.Intimacy, $"{reply.TuDiAcupointPoint} 点经脉点", true));
		}

		private void FinishTraining(bool success, int code, int vigor, int intimacy, string reward, bool meridians)
		{
			if (!success && code == 4683)
			{
				m_Note.text = meridians ? "通脉需要师尊高出至少一个大境界。" : "传功需要师尊高出至少一个大境界，且相差至少 10 个小境界。";
				return;
			}
			if (!ShiTuWnd.ActionSucceeded(success, code, 4581, meridians ? "通脉未成功。请检查亲密是否达到 100，再刷新重试。" : "传功未成功，请刷新后再试。"))
				return;
			MainWindow?.HuoQuShiFuXIngXi();
			Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = vigor;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			string title = meridians ? "通脉" : "传功";
			PlayReceipt(meridians ? ShiTuToZiList.ToMai : ShiTuToZiList.ChuangGong, title,
				$"{title}已完成，弟子可领取 {reward}。\n当前亲密 {intimacy}，剩余精力 {vigor} 点。");
		}

		internal static string RitualDialogue(ShiTuToZiList action)
		{
			return ClientHotUpdate.MentorDialogue(action) ?? (action switch
			{
				ShiTuToZiList.ChuangGong => "「静气凝神，接这一缕真元。」",
				ShiTuToZiList.ToMai => "「随我运息，让灵气沿经脉流转。」",
				_ => "「此后山高水长，望你守住自己的道。」"
			});
		}

		internal static string RitualCaption(ShiTuToZiList action)
		{
			return ClientHotUpdate.MentorCaption(action) ?? (action switch
			{
				ShiTuToZiList.ChuangGong => "灵气入体",
				ShiTuToZiList.ToMai => "灵流通脉",
				_ => "落印出师"
			});
		}

		private void PlayReceipt(ShiTuToZiList action, string title, string result)
		{
			string entry = $"{RitualDialogue(action)}\n\n{result}";
			MentorJournal.Append(title, entry);
			m_Confirm.interactable = false;
			m_Close.interactable = false;
			m_PlayingGraduation = action == ShiTuToZiList.ChuShi;
			MentorPracticeMotion.Play(m_Card, action, RitualCaption(action), () =>
			{
				if (GameObject == null || !GameObject.activeInHierarchy)
					return;
				if (m_GraduationTitles.Count > 0)
				{
					string titles = $"获得称号「{string.Join("、", m_GraduationTitles)}」。";
					MentorJournal.Append("师徒称号", titles);
					entry += "\n" + titles;
				}
				CloseWindow();
				MonoSingleton<GameStart>.Instance.PopUpAttention(entry, title: title + "回执");
			});
		}

		internal bool CollectGraduationTitle(string title)
		{
			if (!m_PlayingGraduation || GameObject == null || !GameObject.activeInHierarchy)
				return false;
			if (!m_GraduationTitles.Contains(title))
				m_GraduationTitles.Add(title);
			return true;
		}

		private void Send<T>(MsgType command, IMessage data, MessageParser<T> parser, Action<T> onReply)
			where T : class, IMessage<T>
		{
			var net = Singleton<NetManager>.Instance;
			if (!net.GameServerIsAlive())
			{
				m_Note.text = "游戏服已断开，请重连后再试。";
				return;
			}
			m_Confirm.interactable = false;
			m_Note.text = "正在等待回执…";
			var timeout = Timer.Register(8f, () =>
			{
				m_Request = null;
				if (GameObject != null && GameObject.activeInHierarchy)
				{
					m_Confirm.interactable = true;
					m_Note.text = "回执超时。可关闭并刷新师徒信息，确认处理结果。";
				}
			}, useRealTime: true);
			m_Request = timeout;
			net.SendMsg(command, data, response =>
			{
				if (m_Request != timeout || GameObject == null || !GameObject.activeInHierarchy)
					return;
				Timer.Cancel(timeout);
				m_Request = null;
				m_Confirm.interactable = true;
				if (!(response is ByteString bytes))
				{
					m_Note.text = "回执未送达，请刷新师徒信息后再试。";
					return;
				}
				T reply;
				try
				{
					reply = parser.ParseFrom(bytes);
				}
				catch (InvalidProtocolBufferException)
				{
					m_Note.text = "无法读取回执，请重试。";
					return;
				}
				m_Note.text = m_DefaultNote;
				onReply(reply);
			}, false, ServerType.GamesNormalServer, false);
		}

		private static ShiTuWnd MainWindow => Singleton<UIManager>.Instance.FindWndByName<ShiTuWnd>("ShiTuWnd.prefab");

		private void CloseWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
