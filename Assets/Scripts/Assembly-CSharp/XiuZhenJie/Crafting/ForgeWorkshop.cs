using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed partial class ForgeWorkshop : MonoBehaviour
	{
		[Serializable] private class Choice { public string id, name, gradeName, affinityLabel, veinLabel, controlText, difficultyName, difficultyText; public int count, difficulty, slotType; }
		[Serializable] private class Grade { public int id; public string name; }
		[Serializable] private class Catalog { public Pattern[] patterns; public Choice[] blanks, materials, flames; public Grade[] grades; public string smithRankName, methodName, trainingText; public bool canClaimStarter; public int supplyPrice, moldVersion, batchLimit; public Mold[] molds; }
		[Serializable] private class Ingredient { public string templateId; public int quantity; }
		[Serializable] private class CreateOrder { public string requestId, blankId, flameId, name, patternId; public int gradeId; public Ingredient[] inputs; public int[] strokes; public int quantity; public string moldId; }
		[Serializable] private class SupplyOrder { public string requestId; public int expectedPrice; }
		[Serializable] private class Pending { public int command; public string json; }
		[Serializable] private class Receipt { public bool forged, canFit; public int freeSlots, experience, quantity, maxByMaterial, baseAttack, baseDefense, vigorCost, availableVigor, appraisal, recyclePrice; public string moldQuality, moldId, costText, materialText, effectText;
            public int difficulty, baseUnits, spentUnits, savedUnits, slotType;
            public string difficultyName, difficultyText, trainingText, controlText, factorText, spentText, castingText, flameName, smithRankName;
            public CastCost[] costs; public string name, gradeName, elementName, polarityName, qiName, bonusText, processAffixText, message, returnedText; }
		[Serializable] private class Estimate { public int successBps, difficulty, successExperience, failureExperience; public string difficultyName, difficultyText, costText, factorText, controlText, trainingText, moldQuality; public string smithRankName, methodName, flameName, gradeName, processText; }
		[Serializable] private class Failure { public string error; }

		private readonly Dictionary<string, int> picked = new Dictionary<string, int>();
		private Font font;
		private Catalog catalog;
		private int blank, flame, grade;
		private bool busy;
		private Pending pending;
		private string pendingKey;
		private Text blankTitle, training, materialTitle, status, preparation;
		private Button previous, next, submit, supply, flameButton, gradeButton, estimate;
		private InputField workName;
		private RectTransform rows;
		private readonly List<Action> updateRows = new List<Action>();
		private bool Locked => busy || pending != null;
		private int Total => picked.Values.Sum();

		internal static Button Attach(LianQi_ChuangZaoMianPanel panel)
		{
			var original = panel.MoMiChuangZao;
			var entry = Instantiate(original, original.transform.parent, false);
			entry.name = "ForgeEntry";
			InkCraftHalls.StyleDestinations(
				(panel.LingZhiDuiHuan, "器图兑换", "查阅器图"), (panel.MoMiChuangZao, "推演器图", "模拟炼器"),
				(panel.LingZhiZhuCe, "器图注册", "登记自创器图"), (entry, "铸器工坊", "塑形留模 · 按料浇铸"));
			entry.onClick = new Button.ButtonClickedEvent();
			entry.onClick.AddListener(() => OpenIn(panel.transform, panel.Des.font));
            return entry;
		}

		internal static void CloseIn(LianQi_ChuangZaoMianPanel panel)
		{
			var root = panel.transform.Find("ForgeWorkshop");
			if (root != null) Destroy(root.gameObject);
		}

		internal static void OpenIn(Transform parent, Font sourceFont, Action onClose = null, bool libraryFirst = false)
        {
            if (parent.Find("ForgeWorkshop") != null) return;
            var root = Node("ForgeWorkshop", parent);
            Stretch(root);
            InkInterfaceArt.Set(root.gameObject.AddComponent<Image>(), "Parchment", false);
            var workshop = root.gameObject.AddComponent<ForgeWorkshop>();
            workshop.onWorkshopClose = onClose;
            workshop.openLibraryOnLoad = libraryFirst;
            workshop.Open(sourceFont);
        }

        private Action onWorkshopClose;
        private void Open(Font sourceFont)
		{
			font = sourceFont;
			pendingKey = $"ForgePending.{Constants.LoginServerIP}.{Constants.LoginServerPort}.{Singleton<RoleManager>.Instance.currentRoleId}";
			if (PlayerPrefs.HasKey(pendingKey))
				pending = JsonUtility.FromJson<Pending>(PlayerPrefs.GetString(pendingKey));
			Build();
			Refresh();
		}

		private void Build()
		{
			var title = Label(transform, "Title", "铸器工坊", 21);
			title.fontStyle = FontStyle.Bold;
			Top(title.rectTransform, 12, 140, 4, 32);
			var close = MakeButton(transform, "Close", "返回", () => { Destroy(gameObject); onWorkshopClose?.Invoke(); });
			Right((RectTransform)close.transform, 8, 5, 53, 28);
			previous = MakeButton(transform, "PreviousBlank", "〈", () => MoveBlank(-1));
			Left((RectTransform)previous.transform, 10, 43, 37, 32);
			next = MakeButton(transform, "NextBlank", "〉", () => MoveBlank(1));
			Right((RectTransform)next.transform, 10, 43, 37, 32);
			blankTitle = Label(transform, "SelectedBlank", "读取器胚…", 14);
			blankTitle.alignment = TextAnchor.MiddleCenter;
			Top(blankTitle.rectTransform, 50, 50, 38, 40);
			flameButton = MakeButton(transform, "SelectFlame", "选择炉火", () => OpenChoices(false));
			gradeButton = MakeButton(transform, "SelectGrade", "目标品阶", () => OpenChoices(true));
			HalfTop((RectTransform)flameButton.transform, 0, .5f, 79, 30);
			HalfTop((RectTransform)gradeButton.transform, .5f, 1, 79, 30);
			training = Label(transform, "Training", "", 13);
			Top(training.rectTransform, 12, 12, 110, 23);

			var inputRect = Node("WorkName", transform);
			Top(inputRect, 12, 12, 134, 32);
			InkInterfaceArt.Set(inputRect.gameObject.AddComponent<Image>(), "ParchmentHeading", false);
			workName = inputRect.gameObject.AddComponent<InputField>();
			var placeholder = Label(inputRect, "Placeholder", "作品名称（1 至 20 字）", 15);
			Stretch(placeholder.rectTransform, 8);
			placeholder.color = InkInterfaceArt.SoftInk;
			var copy = Label(inputRect, "Text", "", 15);
			Stretch(copy.rectTransform, 8);
			workName.textComponent = copy;
			workName.placeholder = placeholder;
			workName.lineType = InputField.LineType.SingleLine;
			workName.onValueChanged.AddListener(_ => UpdateControls());
			BuildMoldControls();
			materialTitle = Label(transform, "MaterialCount", "投入材料", 15);
			Top(materialTitle.rectTransform, 12, 12, 198, 25);

			var scrollRect = Node("Materials", transform);
			scrollRect.anchorMin = Vector2.zero; scrollRect.anchorMax = Vector2.one;
			scrollRect.offsetMin = new Vector2(10, 133); scrollRect.offsetMax = new Vector2(-10, -224);
			scrollRect.gameObject.AddComponent<Image>().color = InkDialogArt.Ivory;
			scrollRect.gameObject.AddComponent<RectMask2D>();
			var scroll = scrollRect.gameObject.AddComponent<ScrollRect>();
			rows = Node("Rows", scrollRect);
			rows.anchorMin = new Vector2(0, 1); rows.anchorMax = Vector2.one; rows.pivot = new Vector2(.5f, 1);
			rows.sizeDelta = Vector2.zero; rows.anchoredPosition = Vector2.zero;
			scroll.viewport = scrollRect; scroll.content = rows; scroll.horizontal = false;
			scroll.movementType = ScrollRect.MovementType.Clamped;
			status = Label(transform, "Status", "", 14);
			status.rectTransform.anchorMin = Vector2.zero; status.rectTransform.anchorMax = new Vector2(1, 0);
			status.rectTransform.offsetMin = new Vector2(12, 68); status.rectTransform.offsetMax = new Vector2(-12, 131);
			preparation = Label(transform, "Preparation", "", 13);
			preparation.color = InkInterfaceArt.SoftInk;
			preparation.rectTransform.anchorMin = Vector2.zero;
			preparation.rectTransform.anchorMax = new Vector2(1, 0);
			preparation.rectTransform.offsetMin = new Vector2(12, 44);
			preparation.rectTransform.offsetMax = new Vector2(-12, 68);
			supply = MakeButton(transform, "GetSupplies", "领取材料", Supply);
			estimate = MakeButton(transform, "Estimate", "估火候", EstimateOrder);
			submit = MakeButton(transform, "Create", "开炉", Submit);
			Footer((RectTransform)supply.transform, 0, .4f);
			Footer((RectTransform)estimate.transform, .4f, .68f);
			Footer((RectTransform)submit.transform, .68f, 1);
			InkInterfaceArt.Set(submit.GetComponent<Image>(), "JadeButton", false);
			submit.GetComponentInChildren<Text>(true).color = Color.white;
			BuildPatternLibrary();
			BuildProcess();
		}

		private void Refresh(string keep = null)
		{
			busy = true;
			status.text = keep ?? "正在读取器胚和纳戒材料…";
			UpdateControls();
			ForgeWire.Send(MsgType.ForgeWorkshopCatalog, null, (ok, json) =>
			{
				if (this == null) return;
				busy = false;
				if (ok && ReadCatalog(json))
					status.text = keep ?? (pending != null ? "上一笔操作尚未确认，请点重试查询原回执。" : "");
				else
				{
					catalog = null;
					status.text = (keep == null ? "" : keep + "\n") + ErrorText(json, "暂未读到工坊，请刷新。");
				}
				UpdateControls();
			});
		}

		private bool ReadCatalog(string json)
		{
			var data = JsonUtility.FromJson<Catalog>(json);
			if (data?.blanks == null || data.materials == null || data.flames == null || data.grades == null) return false;
			string selected = catalog != null && blank < catalog.blanks.Length ? catalog.blanks[blank].id : null;
			string oldFlame = catalog != null && flame < catalog.flames.Length ? catalog.flames[flame].id : null;
			int oldGrade = catalog != null && grade < catalog.grades.Length ? catalog.grades[grade].id : data.moldVersion > 0 ? 3 : 1;
			InvalidateCast();
			catalog = data;
			blank = Mathf.Max(0, Array.FindIndex(data.blanks, choice => choice.id == selected));
			flame = Mathf.Max(0, Array.FindIndex(data.flames, choice => choice.id == oldFlame));
			grade = Mathf.Max(0, Array.FindIndex(data.grades, choice => choice.id == oldGrade));
			var quantities = new Dictionary<string, int>(picked);
			picked.Clear();
			updateRows.Clear();
			foreach (Transform child in rows) { child.gameObject.SetActive(false); Destroy(child.gameObject); }
			var materials = data.materials.OrderByDescending(m => m.count > 0).ToArray();
            for (int i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
				int count = quantities.TryGetValue(material.id, out int old) ? Mathf.Min(old, material.count) : 0;
				if ((activeMold != null || sourcePattern != null) && quantities.ContainsKey(material.id)) count = quantities[material.id];
				if (count > 0) picked[material.id] = count;
				BuildRow(material, i);
			}
			rows.sizeDelta = new Vector2(0, data.materials.Length * MaterialRowHeight);
			return true;
		}

		private const int MaterialRowHeight = 62;

		private static string WithAffinity(string title, Choice choice, string separator = " · ")
		{
			return string.IsNullOrEmpty(choice.affinityLabel) ? title : title + separator + choice.affinityLabel;
		}

		private void BuildRow(Choice m, int i)
		{
			var row = Node("Material_" + m.id, rows);
			Top(row, 0, 0, i * MaterialRowHeight, MaterialRowHeight);
			var backdrop = row.gameObject.AddComponent<Image>();
			backdrop.raycastTarget = false;
			var name = Label(row, "Name", $"{m.name}　{m.gradeName}　持有 {m.count}", 14);
			Top(name.rectTransform, 8, 108, 1, 28);
			var affinity = Label(row, "Affinity", string.IsNullOrEmpty(m.veinLabel) ? m.affinityLabel : m.affinityLabel + "\n材纹 · " + m.veinLabel, 13);
			affinity.color = InkInterfaceArt.SoftInk;
			Top(affinity.rectTransform, 8, 100, 23, 38);
			var minus = MakeButton(row, "Minus", "−", () => Change(m, -1));
			Right((RectTransform)minus.transform, 69, 6, 35, 38);
			var qty = Label(row, "Quantity", "", 14); qty.alignment = TextAnchor.MiddleCenter;
			Right(qty.rectTransform, 39, 6, 30, 38);
			var plus = MakeButton(row, "Plus", "+", () => Change(m, 1));
			Right((RectTransform)plus.transform, 4, 6, 35, 38);
			updateRows.Add(() =>
			{
				picked.TryGetValue(m.id, out int n);
				qty.text = n.ToString();
				backdrop.color = n > 0 ? new Color32(220, 235, 219, 255) : InkDialogArt.Ivory;
				qty.color = n > 0 ? InkInterfaceArt.Ink : InkInterfaceArt.SoftInk;
				minus.interactable = !Locked && n > 0;
				plus.interactable = !Locked && (activeMold != null || n < m.count) && Total < 12;
			});
		}

		private void Change(Choice m, int delta)
		{
			picked.TryGetValue(m.id, out int n);
			int desired = n + delta;
			if (Locked || desired < 0 || (delta > 0 && activeMold == null && desired > m.count) || Total + delta > 12) return;
			if (desired == 0) picked.Remove(m.id); else picked[m.id] = desired;
			InvalidateCast();
			status.text = "";
			UpdateControls();
		}

		private void MoveBlank(int delta)
		{
			if (Locked || catalog == null || catalog.blanks.Length == 0) return;
			strokes.Clear();
			sourcePattern = null;
			InvalidateCast();
			blank = (blank + delta + catalog.blanks.Length) % catalog.blanks.Length;
			status.text = "";
			UpdateControls();
		}

		private void UpdateControls()
		{
			bool hasBlank = catalog?.blanks.Length > 0;
			if (hasBlank)
			{
				var choice = catalog.blanks[blank];
				blankTitle.text = WithAffinity($"{ForgeSlotName(choice.slotType)}器胚　{choice.name} · {choice.gradeName}", choice, "\n");
			}
			bool hasTools = catalog?.flames.Length > 0 && catalog.grades.Length > 0;
			flameButton.interactable = gradeButton.interactable = !Locked && hasTools;
			if (hasTools)
			{
				var selectedFlame = catalog.flames[flame];
				flameButton.GetComponentInChildren<Text>(true).text = WithAffinity("炉火 · " + selectedFlame.name, selectedFlame);
				gradeButton.GetComponentInChildren<Text>(true).text = "目标 · " + catalog.grades[grade].name;
				training.text = $"炼器师　{catalog.smithRankName}　　{catalog.methodName}";
			}
			materialTitle.text = $"入炉材料　{Total} / 12 份";
			foreach (var update in updateRows) update();
			previous.interactable = next.interactable = !Locked && hasBlank && catalog.blanks.Length > 1;
			workName.interactable = !Locked && catalog != null;
			supply.interactable = !Locked && catalog != null;
			supply.GetComponentInChildren<Text>(true).text = catalog?.canClaimStarter == false
				? $"补料 · {catalog.supplyPrice} 灵石" : "领取入门材料";
			estimate.interactable = !Locked && hasBlank && hasTools && Total >= 2;
			submit.GetComponentInChildren<Text>(true).text = pending != null ? "重试上一笔" : catalog == null ? "刷新工坊" : "开炉";
			bool named = !string.IsNullOrWhiteSpace(workName.text);
			submit.interactable = !busy && (pending != null || catalog == null || (hasBlank && hasTools && Total >= 2 && named));
			if (catalog == null || Locked)
				preparation.text = "";
			else if (!hasBlank || !hasTools)
				preparation.text = "尚缺可用的器胚、炉火或目标品阶。";
			else if (Total < 2)
				preparation.text = $"再选 {2 - Total} 份材料可估算火候。";
			else
				preparation.text = named ? "" : "填好作品名称即可开炉。";
			UpdateMoldControls();
			UpdatePatternLibrary();
			UpdateProcess();
		}

		private void OpenChoices(bool grades)
		{
			if (Locked || catalog == null || transform.Find("Choices") != null) return;
			var list = Node("Choices", transform); Stretch(list);
			InkInterfaceArt.Set(list.gameObject.AddComponent<Image>(), "Parchment", false);
			Top(Label(list, "Heading", grades ? "选择目标品阶" : "选择炉火", 18).rectTransform, 12, 65, 5, 35);
			var close = MakeButton(list, "CloseChoices", "返回", () => Destroy(list.gameObject));
			Right((RectTransform)close.transform, 10, 8, 50, 30);
			var viewport = Node("Options", list); Stretch(viewport); viewport.offsetMin = new Vector2(8, 8); viewport.offsetMax = new Vector2(-8, -44);
			viewport.gameObject.AddComponent<Image>().color = Color.white; viewport.gameObject.AddComponent<RectMask2D>();
			var scroll = viewport.gameObject.AddComponent<ScrollRect>();
			var content = Node("List", viewport);
			content.anchorMin = new Vector2(0, 1); content.anchorMax = Vector2.one; content.pivot = new Vector2(.5f, 1);
			int count = grades ? catalog.grades.Length : catalog.flames.Length;
			int rowHeight = grades ? 42 : 78;
			content.sizeDelta = new Vector2(0, count * rowHeight); scroll.content = content; scroll.viewport = viewport;
			scroll.horizontal = false; scroll.movementType = ScrollRect.MovementType.Clamped;
			for (int i = 0; i < count; i++)
			{
				int index = i;
				var option = MakeButton(content, "Option_" + (grades ? catalog.grades[i].id.ToString() : catalog.flames[i].id),
					grades ? catalog.grades[i].name : WithAffinity(catalog.flames[i].name + " · " + catalog.flames[i].gradeName, catalog.flames[i]) + ExtraLine(catalog.flames[i].controlText), () =>
					{
						if (index != (grades ? grade : flame))
						{
							InvalidateCast();
							if (grades && sourcePattern != null)
                            {
                                sourcePattern = null;
                                strokes.Clear();
                            }
						}
						if (grades) grade = index; else flame = index;
						Destroy(list.gameObject); status.text = ""; UpdateControls();
					});
				Top((RectTransform)option.transform, 3, 3, i * rowHeight, rowHeight - 3);
				if (index == (grades ? grade : flame))
				{
					InkDialogArt.StyleButton(option, "JadeButton", InkDialogArt.Ivory);
					option.GetComponentInChildren<Text>(true).text += "　已选";
				}
			}
		}

		private CreateOrder BuildOrder()
		{
			return new CreateOrder { patternId = sourcePattern?.id, strokes = strokes.ToArray(), moldId = activeMold?.id, quantity = batchCount, blankId = catalog.blanks[blank].id, gradeId = catalog.grades[grade].id, flameId = catalog.flames[flame].id,
				inputs = picked.OrderBy(v => v.Key, StringComparer.Ordinal).Select(v => new Ingredient { templateId = v.Key, quantity = v.Value }).ToArray() };
		}

		private void EstimateOrder()
		{
			if (!estimate.interactable || Locked) return;
			if (activeMold != null) { EstimateCast(); return; }
			if (firstEstimate != null) { ShowFirstQuote(); return; }
			busy = true; UpdateControls();
			ForgeWire.Send(MsgType.ForgeEstimate, JsonUtility.ToJson(BuildOrder()), (ok, json) =>
			{
				if (this == null) return;
				busy = false;
				if (ok)
				{
					firstEstimate = JsonUtility.FromJson<Estimate>(json);
					var value = firstEstimate;
					status.text = $"估计成功率　{value.successBps / 100f:0.##}%　{value.moldQuality} · {value.gradeName}";
					ShowFirstQuote();
				}
				else status.text = ErrorText(json, "未收到火候预估，可重试。");
				UpdateControls();
			});
		}

		private void Submit()
		{
			if (busy) return;
			if (pending != null) { SendPending(); return; }
			if (catalog == null) { Refresh(); return; }
			if (!submit.interactable) return;
			if (activeMold != null) { SubmitCast(); return; }
			if (firstEstimate == null) return;
            var order = BuildOrder(); order.requestId = Guid.NewGuid().ToString("N"); order.name = workName.text.Trim();
			SaveAndSend(MsgType.ForgeCreate, JsonUtility.ToJson(order));
		}

		private void Supply()
		{
			if (Locked || catalog == null) return;
			var order = new SupplyOrder { requestId = Guid.NewGuid().ToString("N"), expectedPrice = catalog.canClaimStarter ? 0 : catalog.supplyPrice };
			SaveAndSend(MsgType.ForgeSupplies, JsonUtility.ToJson(order));
		}

		private void SaveAndSend(MsgType command, string json)
		{
			pending = new Pending { command = (int)command, json = json };
			PlayerPrefs.SetString(pendingKey, JsonUtility.ToJson(pending)); PlayerPrefs.Save();
			SendPending();
		}

		private void SendPending()
		{
			if (busy || pending == null) return;
			busy = true;
			status.text = "正在确认工坊回执…";
			UpdateControls();
			var order = pending;
			string key = pendingKey;
			ForgeWire.Send((MsgType)order.command, order.json, (ok, json) =>
			{
				// A later operation may already have replaced this one after a reopen.
				if (this == null) return; // Keep the ticket for the reopened workshop.
                if (PlayerPrefs.GetString(key) != JsonUtility.ToJson(order))
                {
                    pending = PlayerPrefs.HasKey(key) ? JsonUtility.FromJson<Pending>(PlayerPrefs.GetString(key)) : null;
                    busy = false; Refresh(); return;
                }
				if (json != null)
				{
					PlayerPrefs.DeleteKey(key); PlayerPrefs.Save();
				}
				busy = false;
				if (json == null)
				{
					status.text = "暂未收到回执。点重试将确认同一笔操作。";
					UpdateControls(); return;
				}
				pending = null;
				if (!ok) { Refresh(ErrorText(json, "本次操作未完成。")); return; }
				if (Singleton<RoleManager>.Instance.RoleBoxDic.TryGetValue(10, out var box))
					box.GetAllGoods(() => Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData(), false);
				if ((MsgType)order.command == MsgType.ForgeClaimSample)
				{
					var sample = JsonUtility.FromJson<Receipt>(json);
					Refresh(sample.name + "已入纳戒，可在护身行装的" + ForgeSlotName(sample.slotType) + "位穿戴。");
					return;
				}
				if ((MsgType)order.command == MsgType.ForgeSupplies)
				{
					if (!ReadCatalog(json)) { Refresh("材料已入袋。"); return; }
					status.text = "材料已入袋。"; UpdateControls(); return;
				}
				var receipt = JsonUtility.FromJson<Receipt>(json);
				if (!receipt.forged)
				{
					InvalidateCast();
					Refresh(receipt.message + ExtraLine(receipt.returnedText));
					ShowReceipt(receipt); return;
				}
				if (receipt.quantity == 0) { strokes.Clear(); picked.Clear(); workName.text = ""; sourcePattern = null; processStep = 0; }
                InvalidateCast();
                string result = $"已入纳戒 · {receipt.name} ×{Mathf.Max(1,receipt.quantity)}（{receipt.gradeName}）\n{receipt.moldQuality}　攻 {receipt.baseAttack}　防 {receipt.baseDefense}";
				Refresh(result);
				ShowReceipt(receipt);
			});
		}

		private void ShowReceipt(Receipt receipt)
		{
            string title = receipt.forged ? receipt.quantity > 0 ? "批铸入库" : "首炉成器" : "败炉复盘";
            var sheet = ReportSheet("Receipt", title, ReceiptText(receipt), true);
            var close = MakeButton(sheet, "CloseReceipt", receipt.forged ? "返回工坊" : "调整再炼", () =>
            {
                Destroy(sheet.gameObject);
                if (!receipt.forged) { processStep = 1; UpdateControls(); }
            });
            Footer((RectTransform)close.transform, 0, 1);
            if (receipt.forged) ForgeEffects.Play(sheet, receipt.quantity > 0 ? "cast" : "mold");
		}

		private static string ErrorText(string json, string fallback)
		{
			if (string.IsNullOrEmpty(json)) return fallback;
			try { return JsonUtility.FromJson<Failure>(json)?.error ?? fallback; }
			catch (ArgumentException) { return fallback; }
		}

		private static RectTransform Node(string name, Transform parent)
		{
			var rect = (RectTransform)new GameObject(name, typeof(RectTransform)).transform;
			rect.SetParent(parent, false);
			return rect;
		}

		private Text Label(Transform parent, string name, string value, int size)
		{
			var label = Node(name, parent).gameObject.AddComponent<Text>();
			label.font = font; label.fontSize = size; label.text = value ?? ""; label.color = InkInterfaceArt.Ink;
			label.supportRichText = false; label.raycastTarget = false; label.alignment = TextAnchor.MiddleLeft;
			return label;
		}

		private Button MakeButton(Transform parent, string name, string value, UnityEngine.Events.UnityAction click)
		{
			var rect = Node(name, parent); var image = rect.gameObject.AddComponent<Image>();
			InkInterfaceArt.Set(image, "ParchmentHeading", false);
			var button = rect.gameObject.AddComponent<Button>(); button.targetGraphic = image;
			var text = Label(rect, "Label", value, 14); text.alignment = TextAnchor.MiddleCenter; Stretch(text.rectTransform);
			button.onClick.AddListener(click); return button;
		}

		private static void Stretch(RectTransform rect, int x = 0)
		{
			rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(x, 0); rect.offsetMax = new Vector2(-x, 0);
		}

		private static void Top(RectTransform rect, int left, int right, int y, int h)
		{
			rect.anchorMin = new Vector2(0, 1); rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left, -y-h); rect.offsetMax = new Vector2(-right, -y);
		}

		private static void Left(RectTransform rect, int x, int y, int w, int h)
		{
			rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
			rect.anchoredPosition = new Vector2(x, -y); rect.sizeDelta = new Vector2(w, h);
		}

		private static void Right(RectTransform rect, int x, int y, int w, int h)
		{
			rect.anchorMin = rect.anchorMax = rect.pivot = Vector2.one;
			rect.anchoredPosition = new Vector2(-x, -y); rect.sizeDelta = new Vector2(w, h);
		}

		private static void Footer(RectTransform rect, float a, float b)
		{
			rect.anchorMin = new Vector2(a, 0); rect.anchorMax = new Vector2(b, 0);
			rect.offsetMin = new Vector2(8, 6); rect.offsetMax = new Vector2(-8, 42);
		}

		private static void HalfTop(RectTransform rect, float a, float b, int y, int height)
		{
			rect.anchorMin = new Vector2(a, 1); rect.anchorMax = new Vector2(b, 1);
			rect.offsetMin = new Vector2(8, -y-height); rect.offsetMax = new Vector2(-8, -y);
		}
	}
}
