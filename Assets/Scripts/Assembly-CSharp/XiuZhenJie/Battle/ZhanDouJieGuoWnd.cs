using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanDouJieGuoWnd : Window
	{
		public ZhanDouJieGuoPanel _panel;

		private string m_DungeonId;

		private int roleIndex;

		private Text m_Report;
		private RectTransform m_Card;
		private ScrollRect m_Receipts;
		private BattleFieldLedger m_Ledger;
		private int m_ReceiptRevision;
		private const int ReceiptHeight = 23;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ZhanDouJieGuoPanel>();
			AddButtonClickListener(_panel.LingQuButton, LingQuJiangLi);
		}

		private void StyleResult()
		{
			Button returnButton = _panel.LingQuButton;
			m_Card = (RectTransform)returnButton.transform.parent;
			var card = m_Card;
			InkDialogArt.SetWash(Transform,new Color32(9,25,23,172));
			InkDialogArt.SetPaper(card.GetComponent<Image>(),"InkLedgerPaper");
			card.Find("BottomBg").GetComponent<Image>().enabled = false;
			var heading = card.Find("Bg").GetComponent<Image>();
			InkInterfaceArt.Set(heading,"JadeButton",false);
			InkBattleHud.Band(heading.rectTransform,10,10,10,47);
			Font font = returnButton.GetComponentInChildren<Text>(true).font;
			Text title = InkBattleHud.Label(heading.transform,font,"Title","历练战报",22,InkDialogArt.Ivory);
			title.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(title.rectTransform,4,4,0,0);
			m_Report = InkBattleHud.Label(card,font,"Report","",16,InkInterfaceArt.Ink);
			m_Report.alignment = TextAnchor.MiddleCenter;
			m_Report.lineSpacing = 1.25f;
			InkBattleHud.Band(m_Report.rectTransform,14,14,60,48);
			Text headingLoot = InkBattleHud.Label(card,font,"LootTitle","本场收获",14,InkInterfaceArt.Ink);
			headingLoot.fontStyle = FontStyle.Bold;
			InkBattleHud.Band(headingLoot.rectTransform,23,23,115,23);
			Image line = InkBattleHud.Make<Image>(card,"Rule");
			line.color = new Color32(154,132,84,160);
			line.raycastTarget = false;
			InkBattleHud.Band(line.rectTransform,23,23,139,1);
			Image view = InkBattleHud.Make<Image>(card,"Receipts");
			view.color = Color.clear;
			view.gameObject.AddComponent<RectMask2D>();
			m_Receipts = view.gameObject.AddComponent<ScrollRect>();
			m_Receipts.horizontal = false;
			m_Receipts.movementType = ScrollRect.MovementType.Clamped;
			m_Receipts.scrollSensitivity = ReceiptHeight;
			m_Receipts.viewport = view.rectTransform;
			m_Receipts.content = InkBattleHud.Make<RectTransform>(view.transform,"Items");
			m_Receipts.content.pivot = new Vector2(.5f,1f);
			Text caption = returnButton.GetComponentInChildren<Text>(true);
			caption.text = "返回主城";
			InkDialogArt.StyleButton(returnButton,"JadeButton",InkDialogArt.Ivory);
			InkBattleHud.Place((RectTransform)returnButton.transform,new Vector2(.5f,0),0,14,144,39);
			BattleHudReveal.Attach(heading.transform);
			BattleHudReveal.Attach(m_Report.transform,.08f);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (m_Report == null) StyleResult();
			m_Ledger = paralist.Length > 2 && paralist[2] is BattleFieldLedger reportLedger
				? reportLedger : Singleton<BattleManager>.Instance.m_BattleSceneMgr?.Ledger;
			m_ReceiptRevision = -1;
			DrawReceipts();
			var battle = Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab");
			m_Report.text = battle == null ? "历练结束" : battle.FieldReport;
			if (paralist.Length >= 2)
			{
				roleIndex = 0;
				int.TryParse(paralist[0].ToString(), out roleIndex);
				m_DungeonId = paralist[1].ToString();
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_Ledger != null && m_ReceiptRevision != m_Ledger.Revision) DrawReceipts();
		}

		private void DrawReceipts()
		{
			m_ReceiptRevision = m_Ledger?.Revision ?? 0;
			int rows = m_Ledger?.Totals.Count ?? 0;
			int visible = Mathf.Clamp(rows,1,4);
			m_Card.sizeDelta = new Vector2(Mathf.Min(320,((RectTransform)Transform).rect.width - 20),211 + visible * ReceiptHeight);
			InkBattleHud.Band(m_Receipts.viewport,23,23,144,visible * ReceiptHeight);
			RectTransform content = m_Receipts.content;
			InkBattleHud.Band(content,0,0,0,Mathf.Max(1,rows) * ReceiptHeight);
			foreach (Transform old in content)
			{
				old.gameObject.SetActive(false);
				Object.Destroy(old.gameObject);
			}
			m_Receipts.vertical = rows > visible;
			m_Receipts.verticalNormalizedPosition = 1f;
			if (rows == 0)
			{
				Text empty = InkBattleHud.Label(content,m_Report.font,"Empty","本场未领取物资",14,InkInterfaceArt.Ink);
				InkBattleHud.Band(empty.rectTransform,0,0,0,ReceiptHeight);
				return;
			}
			int top = 0;
			foreach (var item in m_Ledger.Totals)
			{
				Text name = InkBattleHud.Label(content,m_Report.font,"Item",item.Key,15,InkInterfaceArt.Ink);
				InkBattleHud.Band(name.rectTransform,0,84,top,ReceiptHeight);
				Text amount = InkBattleHud.Label(content,m_Report.font,"Count","× " + item.Value,15,InkInterfaceArt.Ink);
				amount.alignment = TextAnchor.MiddleRight;
				InkBattleHud.Band(amount.rectTransform,0,0,top,ReceiptHeight);
				top += ReceiptHeight;
			}
		}

		private void LingQuJiangLi()
		{
			RoleLeaveDungeonReq_8001008 data = new RoleLeaveDungeonReq_8001008
			{
				DungeonId = m_DungeonId,
				RoleIndex = roleIndex
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_LEAVE_DUNGEON, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (!(_obj is ByteString bytes)) return;
				var leave = RoleLeaveDungeonResp_8001008.Parser.ParseFrom(bytes);
				if (!leave.IsSuccess)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(leave.Code));
					return;
				}
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene", null, () =>
				{
					Singleton<NetManager>.Instance.NetClose(ServerType.BattleServer);
				});
			}, isShowWaitUI: true, ServerType.BattleServer);
		}
	}
}
