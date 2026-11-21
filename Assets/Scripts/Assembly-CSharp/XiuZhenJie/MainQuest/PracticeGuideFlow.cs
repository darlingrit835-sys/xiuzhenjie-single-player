using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	// One instance per role login. Windows can be destroyed and recreated during the cave scene change.
	internal sealed class PracticeGuideFlow
	{
		internal const string StarterId = "gf_3_1";
		private const string FirstChapter = "第一卷 · 纳戒取经";
		private const string SecondChapter = "第二卷 · 静心参悟";
		private const string ThirdChapter = "第三卷 · 洞府纳气";

		private readonly RoleInfoEntity role = Singleton<RoleManager>.Instance.Role_Entity;
		private PracticeGuideOverlay overlay;
		private RectTransform lastTarget;
		private string lastTitle;
		private bool opened;
		private bool techniqueConfirmed;
		private bool saving;
		private bool saveFailed;
		private bool responseLost;
		private bool skipRequested;
		private bool confirmingSkip;
		private bool finished;
		private float missingSince = -1f;

		internal int Stage => role == null ? 100 : Mathf.Max(1, role.PracticeGuideVal);
		internal bool Active => IsCurrent && (Stage <= 3 || finished);
		private bool IsCurrent => role != null && ReferenceEquals(role, Singleton<RoleManager>.Instance.Role_Entity)
			&& ReferenceEquals(this, Singleton<GuideManager>.Instance.Practice);
		private string Chapter => Stage == 1 ? FirstChapter : Stage == 2 ? SecondChapter : ThirdChapter;

		internal static bool IsStarter(BaseNaJieGoodEntity item)
		{
			return item != null && item.Type == 1 && (item.TemplateId == StarterId ||
				(string.IsNullOrEmpty(item.TemplateId) && item.Name == "启灵经"));
		}

		internal void Attach(PracticeGuideOverlay view)
		{
			overlay = view;
			lastTitle = null;
			missingSince = -1f;
			Tick();
		}

		internal void Detach(PracticeGuideOverlay view)
		{
			if (view == overlay)
			{
				overlay?.Hide();
				overlay = null;
			}
		}

		internal void ResumeWindows()
		{
			var bag = Visible<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
			var study = Visible<CanWuWnd>("CanWuWnd.prefab");
			var room = Visible<DongFuWnd>("DongFuWnd.prefab");
			if (bag != null)
				bag.OnShow();
			else if (study != null)
				study.EnterCanWu();
			else if (room != null)
				room.OnShow();
		}

		internal void Tick()
		{
			if (overlay == null)
				return;
			if (!Active)
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				return;
			}
			overlay.SetChapterArt(Mathf.Clamp(Stage, 1, 3));
			var game = MonoSingleton<GameStart>.Instance;
			if (Visible<Window>(Constants.LIXIANSHOUY_WINDOW) != null ||
				Visible<Window>("AttentionWnd.prefab") != null || Visible<Window>(Constants.POPUP_WINDOW) != null ||
				Visible<Window>(Constants.NETMESSAGE_WINDOW) != null ||
				(game.WaitRequestAni != null && game.WaitRequestAni.activeInHierarchy))
			{
				if (lastTitle != null)
					overlay.Hide();
				lastTitle = null;
				return;
			}
			if (saving)
			{
				Message("正在记下修行进度", "请稍候。", null, null, false);
				return;
			}
			if (responseLost)
			{
				Message("重新核对进度", "尚未收到服务器回执。重新登录后，会从已保存的卷章继续。", "返回登录", game.ClearDataSource, false);
				return;
			}
			if (confirmingSkip)
			{
				const string title = "略过新手引导？";
				if (lastTitle != title)
				{
					lastTitle = title;
					lastTarget = null;
					overlay.ShowMessage(Chapter, title, "确认后，这个角色将结束修行引导。", "继续修行", CancelSkip, ConfirmSkip, "确认跳过");
				}
				return;
			}
			if (saveFailed)
			{
				Message("进度尚未记下", "请重新提交本次进度。", "重新提交", Retry, true);
				return;
			}
			if (finished)
			{
				Message("周天初成", "《启灵经》已在体内运转。今后可在练功房查看纳气进度。", "收卷", Leave, false);
				return;
			}
			if (Stage == 1 && !opened)
			{
				Message("山门初开", "你的纳戒里藏着一本《启灵经》。取出它，习得心法，再去洞府运行第一个周天。", "展开画卷", Open, true);
				return;
			}

			switch (Stage)
			{
			case 1: GuideReading(); break;
			case 2: GuideStudy(); break;
			case 3: GuideCultivation(); break;
			}
		}

		private void GuideReading()
		{
			var detail = Visible<NaJieTipWnd>("NaJieTipWnd.prefab");
			if (detail != null)
			{
				if (IsStarter(detail.m_DataSource))
					Focus("读下经文", "点亮处可使用这本经书，将口诀记入识海。", detail.NaJieTipPanel.MiddleBtn);
				else
					Focus("收起物品详情", "返回纳戒，寻找《启灵经》。", detail.NaJieTipPanel.TipCloseButton);
				return;
			}
			var bag = Visible<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
			if (bag == null)
				Focus("打开纳戒", "点开纳戒，查看入门时收到的功法书。", Hud()?.NaJie_btn);
			else if (bag.PracticeBook != null)
				Focus("取出启灵经", "点开这本《启灵经》，看看它记载了什么。", bag.PracticeBook);
			else
				Missing("正在寻经", bag.PracticeBagLoaded ? "纳戒中未找到《启灵经》。重新登录可核对入门礼包进度。" : "正在读取纳戒中的物品。");
		}

		private void GuideStudy()
		{
			var detail = Visible<Common_TipWnd>("Common_TipWnd.prefab");
			if (detail != null)
			{
				if (detail.Panel.TitleText.text == "启灵经" && detail.Panel.ButtonText.text == "参悟")
					Focus("参悟心法", "点「参悟」，将刚读过的口诀化为可修炼的功法。", detail.Panel.QueRenBtn);
				else
					Focus("收起详情", "回到书目，查看《启灵经》。", detail.Panel.CloseBtn);
				return;
			}
			var study = Visible<CanWuWnd>("CanWuWnd.prefab");
			if (study == null)
				Focus("打开参悟", "点开参悟，已读的经书都收在这里。", Hud()?.CanWu_btn);
			else if (study.PracticeListLoaded && study.PracticeBookUnderstood)
				RecordMilestone(2);
			else if (study.PracticeBook != null)
				Focus("翻开经卷", "在「可参悟」中点《启灵经》，查看这门心法。", study.PracticeBook);
			else
				Missing("正在展卷", "正在读取《启灵经》的参悟记录。");
		}

		private void GuideCultivation()
		{
			var chooser = Visible<GengHuanGongFaWnd>("GengHuanGongFaWnd.prefab");
			if (chooser != null)
			{
				if (chooser.m_CurrentChooseItem?.Id == StarterId)
					Focus("确认功法", "《启灵经》已选中，点确认将它装入纳气位。", chooser.Panel.QueRenGengHuan);
				else
					Focus("选定启灵经", "点选《启灵经》，把这门功法用于纳气。", chooser.PracticeBook);
				return;
			}
			var room = Visible<DongFuWnd>("DongFuWnd.prefab");
			if (room != null)
			{
				if (!room.PracticeRoomLoaded)
					Missing("正在入定", "正在读取练功房中的功法与周天状态。");
				else if (!techniqueConfirmed || !room.PracticeBookEquipped)
					Focus("设置纳气功法", "点开纳气功法栏，在已参悟的功法中选择《启灵经》。", room.Panel.NaQiGengHuanBtn);
				else if (room.PracticeRunning == RuningGongFaZuHeType.DuanTiLianShen)
					Focus("腾出一处周天", "两处周天都在运转。点锻体暂停这一门，接下来改为纳气。", room.Panel.DuanTiBtn);
				else if (room.PracticeRunning == RuningGongFaZuHeType.NaQi ||
					room.PracticeRunning == RuningGongFaZuHeType.DuanTiNaQi ||
					room.PracticeRunning == RuningGongFaZuHeType.LianShenNaQi)
					RecordMilestone(3);
				else
					Focus("开始纳气", "点纳气，让《启灵经》运行第一个周天。", room.Panel.NaQiBtn);
				return;
			}
			var residence = Visible<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			if (residence != null)
			{
				Focus("走进练功房", "点练功房，在这里装配并运行功法。", residence.Panel.LianGongFaBtn);
				return;
			}
			var study = Visible<CanWuWnd>("CanWuWnd.prefab");
			if (study != null && study.Panel.TiShiPanel.gameObject.activeInHierarchy)
				Focus("前往练功房", "功法已悟，点这里去洞府设置它。", study.Panel.TiShiGoSettingBtn);
			else
				Focus("返回洞府", "点洞府，前往安置功法的练功房。", Hud()?.DongFu_btn);
		}

		private static T Visible<T>(string windowName) where T : Window
		{
			T window = Singleton<UIManager>.Instance.FindWndByName<T>(windowName);
			return window?.GameObject != null && window.GameObject.activeInHierarchy ? window : null;
		}

		private static MainTitlePanel Hud() => Visible<MainTitleWnd>("MainTitleWnd.prefab")?._panel;

		private void Focus(string title, string text, Selectable control)
		{
			if (control == null || !control.isActiveAndEnabled || !control.IsInteractable())
			{
				Missing("正在寻找指引", "等当前界面就绪后，对应的操作会亮起。");
				return;
			}
			missingSince = -1f;
			var target = (RectTransform)control.transform;
			if (title == lastTitle && target == lastTarget)
				return;
			lastTitle = title;
			lastTarget = target;
			overlay.Show(Chapter, title, text, target, Skip);
		}

		private void Missing(string title, string text)
		{
			if (missingSince < 0f)
				missingSince = Time.unscaledTime;
			if (Time.unscaledTime - missingSince > 10f)
				Message("重新读取本卷", "当前界面未能取得所需数据。重新登录后会按角色记录接续。", "返回登录", MonoSingleton<GameStart>.Instance.ClearDataSource, true);
			else
				Message(title, text, null, null, true);
		}

		private void Message(string title, string text, string button, UnityAction action, bool allowSkip)
		{
			if (title == lastTitle)
				return;
			lastTitle = title;
			lastTarget = null;
			overlay.ShowMessage(Chapter, title, text, button, action, allowSkip ? Skip : (UnityAction)null);
		}

		private void Open() => opened = true;
		private void Skip()
		{
			if (!saving && !responseLost && !finished)
				confirmingSkip = true;
		}
		private void CancelSkip() => confirmingSkip = false;
		private void ConfirmSkip()
		{
			confirmingSkip = false;
			SendProgress(true);
		}
		private void Retry() => SendProgress(skipRequested);

		internal void ConfirmTechnique()
		{
			if (Active && Stage == 3)
				techniqueConfirmed = true;
		}

		internal bool RecordMilestone(int stage)
		{
			if (!Active || Stage != stage || saving || saveFailed || responseLost)
				return false;
			SendProgress(false);
			return true;
		}

		private void SendProgress(bool skip)
		{
			if (!Active || saving || finished || responseLost)
				return;
			saving = true;
			saveFailed = false;
			skipRequested = skip;
			int stage = Stage;
			var command = skip ? MsgType.COMPELETE_GUIDE : MsgType.UPDATE_GUIDE;
			IMessage request = skip ? (IMessage)new GuideInfoSkipReq_1002014 { Type = 1 } : new GuideInfoUpdateReq_1002013 { Type = 1 };
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(command, request, bytes =>
			{
				if (!IsCurrent)
					return;
				saving = false;
				if (bytes == null)
				{
					// The increment-only server command may already have committed. Reload before another write.
					responseLost = true;
					return;
				}
				bool accepted = skip ? GuideInfoSkipResp_1002014.Parser.ParseFrom(bytes).IsSuccess :
					GuideInfoUpdateResp_1002013.Parser.ParseFrom(bytes).IsSuccess;
				if (!accepted)
				{
					saveFailed = true;
					return;
				}
				role.PracticeGuideVal = skip || stage == 3 ? 100 : stage + 1;
				if (skip)
					Leave();
				else if (stage == 3)
					finished = true;
			}))
			{
				saving = false;
				saveFailed = true;
			}
		}

		private void Leave()
		{
			var guide = Singleton<GuideManager>.Instance;
			Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
			guide.InitProgress(100, role.BattleGuideVal);
		}
	}
}
