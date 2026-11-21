using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Spell = EnterBattleHomeResp_1005038.Types.SkillResp_1005039;

namespace XiuZhenJie
{
	public class ZhanFaShiWnd : Window
	{
		private static readonly string[] Stages = { "", "生疏", "小成", "大成", "小圆满", "大圆满" };
		private static readonly string[] Marks = { "未结纹", "烈纹", "疾纹", "合纹" };
		private static readonly int[] Doses = { 1, 5, 10 };
		private readonly List<Spell> spells = new List<Spell>();
		private readonly List<Button> slots = new List<Button>();
		private readonly Button[] doseButtons = new Button[3];
		private RectTransform hall;
		private Image sigil, bar;
		private Text title, mastery, power, engraving, receipt, vigor, pageNumber;
		private Button force, haste, train, reroll, accept, keep, reload, previousPage, nextPage;
		private Font font;
		private int current, path = 1, dose = 5, balance, generation;
		private bool busy, loaded;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			var panel = GameObject.GetComponent<ZhanFaShiPanel>();
			font = panel.GetComponentInChildren<Text>(true).font;
			foreach (Transform child in panel.transform)
				child.gameObject.SetActive(false);
			var background = InkBattleHud.Make<Image>(panel.transform, "PracticeHall");
			hall = background.rectTransform;
			InkDialogArt.Inset(hall, 0, 0, 0, 0);
			InkInterfaceArt.Set(background, "Parchment", false);
			Label("HallTitle", "战法室", 21, .04f, .01f, .235f, .08f);
			vigor = Label("Vigor", "", 15, .25f, .015f, .445f, .08f);
			previousPage = Choice("PreviousPage", "上卷", () => TurnPage(-1), .448f, .018f, .55f, .078f);
			pageNumber = Label("PageNumber", "", 13, .559f, .018f, .652f, .078f);
			pageNumber.alignment = TextAnchor.MiddleCenter;
			nextPage = Choice("NextPage", "下卷", () => TurnPage(1), .66f, .018f, .764f, .078f);
			Choice("Close", "返回", () => Singleton<UIManager>.Instance.CloseWnd(this), .78f, .018f, .96f, .077f);
			sigil = InkBattleHud.Make<Image>(hall, "Element");
			sigil.raycastTarget = false;
			Place(sigil.rectTransform, .045f, .31f, .24f, .455f);
			title = Label("SelectedSpell", "", 21, .26f, .304f, .95f, .368f);
			power = Label("Power", "", 15, .26f, .37f, .97f, .443f);
			mastery = Label("Mastery", "", 14, .055f, .446f, .96f, .497f);
			var track = InkBattleHud.Make<Image>(hall, "MasteryTrack");
			track.color = new Color32(177, 178, 147, 170);
			track.raycastTarget = false;
			Place(track.rectTransform, .055f, .502f, .945f, .511f);
			bar = InkBattleHud.Make<Image>(track.transform, "Filled");
			bar.raycastTarget = false;
			bar.color = InkInterfaceArt.Seal;
			force = Choice("Force", "凝元 · 重威力", () => SelectPath(1), .05f, .533f, .49f, .604f);
			haste = Choice("Haste", "疾诀 · 快结印", () => SelectPath(2), .51f, .533f, .95f, .604f);
			for (int i = 0; i < Doses.Length; i++)
			{
				int amount = Doses[i];
				doseButtons[i] = Choice("Dose" + amount, amount + " 精力", () => { dose = amount; Render(); },
					.05f + i * .174f, .625f, .211f + i * .174f, .687f);
			}
			train = Choice("Practice", "修习", () => Submit(path), .586f, .618f, .95f, .693f);
			engraving = Label("Inscription", "", 15, .055f, .703f, .945f, .79f);
			reroll = Choice("Reroll", "重悟 · 6 精力", () => Submit(3), .05f, .795f, .56f, .863f);
			accept = Choice("Accept", "换用新纹", () => Submit(4), .05f, .795f, .49f, .863f);
			keep = Choice("Keep", "留下旧纹", () => Submit(5), .51f, .795f, .95f, .863f);
			receipt = Label("Receipt", "", 15, .055f, .875f, .945f, .987f);
			reload = Choice("Reload", "重读修习记录", Fetch, .25f, .72f, .75f, .80f);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Fetch();
		}

		public override void OnClose()
		{
			generation++;
			base.OnClose();
		}

		// Legacy serialized item animations address the window through this callback.
		public void PanduanQieHuan(bool enabled)
		{
			foreach (var slot in slots) slot.interactable = enabled && !busy;
		}

		private void Fetch()
		{
			int token = ++generation;
			string role = DongFuResidenceState.CurrentRoleId;
			loaded = false;
			busy = true;
			receipt.text = "展开法卷……";
			Render();
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ENTER_BATTLEHOME, null, bytes =>
			{
				if (!StillHere(token, role)) return;
				busy = false;
				if (bytes == null) { Unavailable(); return; }
				var response = EnterBattleHomeResp_1005038.Parser.ParseFrom(bytes);
				if (!response.IsSuccess || response.Skill.Count == 0 || response.Skill[0].BaseCastMS == 0)
				{
					Unavailable();
					return;
				}
				spells.Clear();
				spells.AddRange(response.Skill);
				foreach (var b in slots)
				{
					m_AllButton.Remove(b);
					b.gameObject.SetActive(false);
					Object.Destroy(b.gameObject);
				}
				slots.Clear();
				for (int i = 0; i < spells.Count; i++)
				{
					int index = i;
					float x = .05f + (i % 4) * .227f;
					float y = .105f + (i % 8 / 4) * .093f;
					slots.Add(Choice("Spell_" + spells[i].SkillId, spells[i].Name, () =>
					{
						current = index;
						receipt.text = "";
						Render();
					}, x, y, x + .216f, y + .076f));
				}
				current = Mathf.Clamp(current, 0, spells.Count - 1);
				balance = response.CurrentVigor;
				Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = balance;
				loaded = true;
				receipt.text = "";
				Render();
			})) Unavailable();
		}

		private bool StillHere(int token, string role)
		{
			return token == generation && GameObject != null && GameObject.activeInHierarchy && role == DongFuResidenceState.CurrentRoleId;
		}

		private void Unavailable()
		{
			busy = loaded = false;
			receipt.text = "修习记录未能同步，请重读法卷后再试。";
			Render();
		}

		private void TurnPage(int step)
		{
			current = Mathf.Clamp(current / 8 + step, 0, (spells.Count - 1) / 8) * 8;
			receipt.text = "";
			Render();
		}

		private void SelectPath(int choice)
		{
			path = choice;
			Render();
		}

		private static string SpellForm(int type)
		{
			switch (type)
			{
			case 10: return "锁敌";
			case 12: return "持续";
			case 14: return "范围";
			case 20: return "护身";
			case 30: return "助攻";
			case 31: return "御守";
			case 40: return "迟滞";
			case 41: return "定身";
			case 50: return "身法";
			default: return "法术";
			}
		}

		private static int Stage(int effort)
		{
			return effort >= 60 ? 5 : effort >= 40 ? 4 : effort >= 25 ? 3 : effort >= 10 ? 2 : 1;
		}

		private static int NextThreshold(int stage)
		{
			return stage == 1 ? 10 : stage == 2 ? 25 : stage == 3 ? 40 : 60;
		}

		private static int AffixBonus(int stage) => 8 + 2 * Mathf.Max(0, stage - 2);
		private static string Mark(int affix, int stage)
		{
			int bonus = AffixBonus(stage);
			switch (affix)
			{
			case 1: return "烈纹 · 威力 +" + bonus + "%";
			case 2: return "疾纹 · 结印缩短 " + bonus + "%";
			case 3: return "合纹 · 威力 +" + bonus / 2 + "%，结印缩短 " + bonus / 2 + "%";
			default: return "修至小成，凝出第一道法纹";
			}
		}

		private static (int power, int cast) Stats(int effort, int forceEffort, int affix, int baseCast)
		{
			int bonus = AffixBonus(Stage(effort));
			int power = 100 + effort + forceEffort / 2 + (affix == 1 ? bonus : affix == 3 ? bonus / 2 : 0);
			int haste = (effort - forceEffort) / 2 + (affix == 2 ? bonus : affix == 3 ? bonus / 2 : 0);
			return (power, (baseCast * (100 - haste) + 50) / 100);
		}

		private void Render()
		{
			bool active = loaded && !busy;
			reload.gameObject.SetActive(!loaded && !busy);
			int page = current / 8;
			int pageCount = (spells.Count + 7) / 8;
			pageNumber.text = loaded ? (page + 1) + "/" + pageCount : "";
			previousPage.interactable = active && page > 0;
			nextPage.interactable = active && page + 1 < pageCount;
			foreach (var slot in slots) slot.interactable = active;
			force.interactable = haste.interactable = active;
			foreach (var b in doseButtons) b.interactable = active;
			train.interactable = reroll.interactable = accept.interactable = keep.interactable = false;
			if (!loaded)
			{
				train.gameObject.SetActive(false);
				accept.gameObject.SetActive(false);
				keep.gameObject.SetActive(false);
				reroll.gameObject.SetActive(false);
				return;
			}
			train.gameObject.SetActive(true);
			var spell = spells[current];
			int effort = spell.Proficiency / 5;
			int stage = spell.ProficiencyLevel;
			int amount = Mathf.Min(dose, 60 - effort);
			int next = effort + amount;
			int nextAffix = spell.PracticeAffix == 0 && next >= 10 ? path : spell.PracticeAffix;
			var before = Stats(effort, spell.ForceEffort, spell.PracticeAffix, spell.BaseCastMS);
			var after = Stats(next, spell.ForceEffort + (path == 1 ? amount : 0), nextAffix, spell.BaseCastMS);
			vigor.text = "精力  " + balance;
			title.text = spell.Name + "　" + Stages[stage];
			string metric = spell.Type == 20 ? "护盾" : spell.Type == 30 || spell.Type == 31 ? "增益" : spell.Type == 40 || spell.Type == 41 ? "控制时长" : spell.Type == 50 ? "身法时长" : "威力";
			power.text = SpellForm(spell.Type) + " · " + metric + " " + before.power + "% → " + after.power + "%\n结印  " + before.cast + " → " + after.cast + " 毫秒";
			int threshold = NextThreshold(stage);
			mastery.text = "熟练 " + spell.Proficiency + " / 300　" + (stage < 5 ? Stages[stage + 1] + "还需 " + ((threshold - effort) * 5) : "此术已至圆满");
			Place(bar.rectTransform, 0, 0, effort / 60f, 1);
			InkInterfaceArt.Set(sigil, "Element" + (WuXing)spell.FiveElement);
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].gameObject.SetActive(i / 8 == page);
				if (i / 8 == page) Select(slots[i], i == current);
			}
			Select(force, path == 1);
			Select(haste, path == 2);
			for (int i = 0; i < Doses.Length; i++) Select(doseButtons[i], dose == Doses[i]);
			train.interactable = active && amount > 0 && balance >= amount;
			Caption(train).text = amount == 0 ? "已圆满" : balance < amount ? "精力不足" : "修习 · " + amount + " 精力";
			engraving.text = spell.PendingAffix == 0 ? Mark(spell.PracticeAffix, stage) : "现有  " + Mark(spell.PracticeAffix, stage) + "\n候选  " + Mark(spell.PendingAffix, stage);
			bool choosing = spell.PendingAffix != 0;
			accept.gameObject.SetActive(choosing);
			keep.gameObject.SetActive(choosing);
			reroll.gameObject.SetActive(!choosing);
			reroll.interactable = active && spell.PracticeAffix != 0 && balance >= 6;
			accept.interactable = keep.interactable = active && choosing;
		}

		private void Submit(int action)
		{
			if (!loaded || busy) return;
			var spell = spells[current];
			int amount = action <= 2 ? Mathf.Min(dose, 60 - spell.Proficiency / 5) : 0;
			var request = new PracticeSkillReq_1005040 { MethodSkillId = spell.SkillId, PracticeNum = amount,
				PracticeAction = action, ExpectedRevision = spell.PracticeRevision };
			int oldStage = spell.ProficiencyLevel;
			var oldStats = Stats(spell.Proficiency / 5, spell.ForceEffort, spell.PracticeAffix, spell.BaseCastMS);
			int token = ++generation;
			string role = DongFuResidenceState.CurrentRoleId;
			busy = true;
			Render();
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.PRACTICE_SKILL, request, bytes =>
			{
				if (!StillHere(token, role)) return;
				busy = false;
				if (bytes == null) { Unavailable(); return; }
				var response = PracticeSkillResp_1005040.Parser.ParseFrom(bytes);
				if (!response.IsSuccess || response.PracticeRevision <= spell.PracticeRevision)
				{
					Unavailable();
					receipt.text = response.Code == 4290 ? "精力不足。重读法卷可查看当前精力。" : "修习记录已变化，请重读法卷。";
					return;
				}
				spell.Proficiency = response.Proficiency;
				spell.ProficiencyLevel = response.Level;
				spell.ForceEffort = response.ForceEffort;
				spell.PracticeAffix = response.PracticeAffix;
				spell.PendingAffix = response.PendingAffix;
				spell.PracticeRevision = response.PracticeRevision;
				balance = response.CurrentVigor;
				Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = balance;
				var now = Stats(spell.Proficiency / 5, spell.ForceEffort, spell.PracticeAffix, spell.BaseCastMS);
				if (action <= 2)
				{
					string prefix = oldStage < response.Level ? "突破" + Stages[response.Level] + "！" : "行功已毕。";
					receipt.text = prefix + "熟练 +" + (amount * 5) + "，威力 " + oldStats.power + "% → " + now.power + "%。";
					if (oldStage == 1 && response.Level >= 2) receipt.text += " 初悟" + Marks[spell.PracticeAffix] + "。";
				}
				else if (action == 3) receipt.text = "消耗 6 精力，参得" + Marks[spell.PendingAffix] + "。择定前，旧纹继续生效。";
				else receipt.text = action == 4 ? "新纹已入诀。" + Mark(spell.PracticeAffix, response.Level) : "旧纹已保留。";
				Render();
				if (action != 5)
					SpellPracticeEffects.Reveal(sigil.rectTransform, spell.FiveElement, action == 3 ? spell.PendingAffix : spell.PracticeAffix, oldStage < response.Level || action >= 3);
			})) Unavailable();
		}

		private static void Select(Button button, bool selected)
		{
			InkInterfaceArt.Set(button.image, selected ? "JadeButton" : "IvoryPlaque", false);
			Caption(button).color = selected ? InkDialogArt.Ivory : InkInterfaceArt.Ink;
		}

		private static Text Caption(Button button) => button.GetComponentInChildren<Text>();

		private Button Choice(string name, string value, UnityAction click, float left, float top, float right, float bottom)
		{
			var image = InkBattleHud.Make<Image>(hall, name);
			Place(image.rectTransform, left, top, right, bottom);
			var button = image.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			var text = InkBattleHud.Label(image.transform, font, "Caption", value, 15, InkInterfaceArt.Ink);
			text.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(text.rectTransform, 3, 3, 1, 1);
			AddButtonClickListener(button, click);
			Select(button, name == "Practice" || name == "Accept");
			return button;
		}

		private Text Label(string name, string value, int size, float left, float top, float right, float bottom)
		{
			var label = InkBattleHud.Label(hall, font, name, value, size, InkInterfaceArt.Ink);
			Place(label.rectTransform, left, top, right, bottom);
			return label;
		}

		private static void Place(RectTransform rect, float left, float top, float right, float bottom)
		{
			rect.anchorMin = new Vector2(left, 1 - bottom);
			rect.anchorMax = new Vector2(right, 1 - top);
			rect.offsetMin = rect.offsetMax = Vector2.zero;
		}
	}
}
