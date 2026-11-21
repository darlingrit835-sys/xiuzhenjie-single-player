using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed partial class AlchemyWorkshop : MonoBehaviour
	{
		[Serializable] private class Recipe
		{
			public string id, name, primaryId, primaryName, memoryQuality, effectName;
			public string[] memoryAuxIds;
			public string effectDescription, effectBasis;
			public int baseEffect, poisonCost;
			public int[] sellPrices;
			public int primaryCount, minRank, mastery;
		}
		[Serializable] private class MaterialChoice { public string id, name, elementName, sourceText; public int count, value, buyPrice; }
		[Serializable] private class Catalog { public int rank, rankExp, rankLimit, supplyPrice, batchLimit, vigor; public string rankName; public bool canClaimStarter; public Recipe[] recipes; public MaterialChoice[] materials; }
		[Serializable] private class Stage { public string name; public int periodMs, targetBps, windowBps, direction; }
		[Serializable] private class Plan { public string runId, quality, effectName, effectBasis, effectText, priceText, shortageText; public bool canBuyMissing; public int minSuccessBps, maxSuccessBps, affinity, effect, sellPrice, shortageCost, poisonCost, copyVigor; public Stage[] stages; }
		[Serializable] private class Request { public string action, recipeId, requestId, runId; public string[] auxIds; public int expectedPrice, quantity; public int[] taps; }
		[Serializable] private class Receipt { public bool brewed; public string recipeName, quality, effectName, effectBasis, effectText, memoryQuality, message; public int effect, quantity, successBps, score, mastery, sellPrice, poisonCost, batches, vigorSpent, minSellPrice, maxSellPrice; }
		[Serializable] private class Failure { public string error; }

		private enum Page { Loading, Prep, Playing, Receipt, Pending }
		private Page page;
		private Catalog catalog;
		private Recipe recipe;
		private Plan plan, run;
		private readonly List<string> auxiliary = new List<string>();
		private readonly List<int> taps = new List<int>();
		private readonly List<int> hits = new List<int>();
		private Action onLeave, onLegacy;
		private string pendingKey, notice;
		private bool busy, closed, estimating;
		private int previewVersion, displayedMs;
		private int batchSize = 1;
		private float previewAt = -1, advanceAt = -1;
		private double stepBegan;
		private string pendingJson;
		private bool Active => this != null && !closed && isActiveAndEnabled;

		internal static void Open(RectTransform parent, Font font, Action close, Action legacy)
		{
			if (parent.Find("AlchemyWorkshop") != null) return;
			var root = Rect("AlchemyWorkshop", parent);
			Fill(root);
			root.gameObject.AddComponent<Image>().color = Ground;
			var view = root.gameObject.AddComponent<AlchemyWorkshop>();
			view.font = font;
			view.onLeave = close;
			view.onLegacy = legacy;
			view.pendingKey = $"AlchemyOrder.{Constants.LoginServerIP}.{Constants.LoginServerPort}.{Singleton<RoleManager>.Instance.currentRoleId}";
			view.pendingJson = PlayerPrefs.GetString(view.pendingKey, "");
			if (view.pendingJson.Length > 0) view.PendingView(false);
			else view.LoadCatalog();
		}

		internal static void CloseIn(Transform parent)
		{
			var view = parent.Find("AlchemyWorkshop")?.GetComponent<AlchemyWorkshop>();
			if (view != null) view.Close(null);
		}

		private void Close(Action next)
		{
			if (closed) return;
			closed = true;
			gameObject.SetActive(false);
			Destroy(gameObject);
			next?.Invoke();
		}

		private void LoadCatalog()
		{
			LoadingView("正在读取丹方与纳戒…", false);
			Send(new Request { action = "catalog" }, (ok, json) =>
			{
				if (!ok || !ReadCatalog(json)) { LoadingView(Error(json, "未读到丹方，请重试。"), true); return; }
				Prepare();
			});
		}

		private bool ReadCatalog(string json)
		{
			var data = Read<Catalog>(json);
			if (data?.recipes == null || data.recipes.Length == 0 || data.materials == null) return false;
			string id = recipe?.id;
			catalog = data;
			recipe = Array.Find(data.recipes, r => r.id == id) ?? data.recipes[0];
			return true;
		}

		private void Prepare()
		{
			if (string.IsNullOrEmpty(recipe.memoryQuality) || batchSize > catalog.batchLimit) batchSize = 1;
			run = null;
			taps.Clear();
			hits.Clear();
			advanceAt = -1;
			PrepView();
			RequestPreview();
		}

		private void NextRecipe(int direction)
		{
			if (busy) return;
			int n = catalog.recipes.Length;
			recipe = catalog.recipes[(Array.IndexOf(catalog.recipes, recipe) + direction + n) % n];
			auxiliary.Clear();
			notice = null;
			Prepare();
		}

		private void ChangeAux(MaterialChoice material, int delta)
		{
			if (busy) return;
			if (delta > 0)
			{
				if (auxiliary.Count >= 3) return;
				auxiliary.Add(material.id);
			}
			else if (!auxiliary.Remove(material.id)) return;
			notice = null;
			RequestPreview();
		}

		private void RestoreMaterials()
		{
			if (busy || string.IsNullOrEmpty(recipe.memoryQuality)) return;
			auxiliary.Clear();
			if (recipe.memoryAuxIds != null) auxiliary.AddRange(recipe.memoryAuxIds);
			notice = "已调回留影用料，可继续替换。";
			RequestPreview();
		}

		private int Stock(string id) => Array.Find(catalog.materials, m => m.id == id)?.count ?? 0;
		private bool HaveMaterials(int count = 1) => Stock(recipe.primaryId) >= recipe.primaryCount * count &&
			auxiliary.GroupBy(x => x).All(g => Stock(g.Key) >= g.Count() * count);

		private void CycleBatch()
		{
			if (busy) return;
			int next = batchSize == 1 ? 5 : batchSize == 5 ? 10 : batchSize == 10 ? 20 : 1;
			batchSize = next <= catalog.batchLimit ? next : 1;
			notice = null;
			PrepView();
			RequestPreview();
		}
		private Request Order(string action) => new Request
		{
			action = action, recipeId = recipe.id, auxIds = auxiliary.ToArray(),
			quantity = action == "replay" || action == "plan" || action == "buyMissing" ? batchSize : 0,
			requestId = action == "plan" ? null : Guid.NewGuid().ToString("N")
		};

		private void RequestPreview()
		{
			previewVersion++;
			plan = null;
			estimating = catalog.rank >= recipe.minRank;
			previewAt = estimating ? Time.unscaledTime + .12f : -1;
			UpdatePreparation();
		}

		private void Estimate()
		{
			previewAt = -1;
			int version = previewVersion;
			Send(Order("plan"), (ok, json) =>
			{
				if (page != Page.Prep || version != previewVersion) return;
				estimating = false;
				plan = ok ? Read<Plan>(json) : null;
				if (plan == null) notice = Error(json, "炉况未读到，点「重新估炉」再试。");
				UpdatePreparation();
			});
		}

		private void StartManual()
		{
			if (busy || estimating || page != Page.Prep) return;
			if (plan == null) { notice = null; RequestPreview(); return; }
			if (!HaveMaterials()) return;
			busy = true;
			notice = "正在起炉…";
			UpdatePreparation();
			Send(Order("start"), (ok, json) =>
			{
				busy = false;
				run = ok ? Read<Plan>(json) : null;
				if (run == null || string.IsNullOrEmpty(run.runId) || run.stages == null || run.stages.Length != 3 ||
					Array.Exists(run.stages, s => s == null || s.periodMs <= 0 || s.windowBps <= 0))
				{
					notice = Error(json, "炉火未起，请重试。"); LoadCatalog(); return;
				}
				notice = null;
				taps.Clear();
				hits.Clear();
				PlayView();
				BeginStep();
			});
		}

		private void Update()
		{
			if (page == Page.Prep && previewAt >= 0 && Time.unscaledTime >= previewAt) Estimate();
			if (page != Page.Playing) return;
			if (advanceAt >= 0)
			{
				if (Time.unscaledTime < advanceAt) return;
				advanceAt = -1;
				if (taps.Count == run.stages.Length)
				{
					var order = Order("finish");
					order.runId = run.runId;
					order.taps = taps.ToArray();
					Submit(order);
				}
				else BeginStep();
				return;
			}
		}

		// EventSystem dispatches clicks in Update. Advance the displayed needle after input
		// so that every tap samples the frame the player actually saw.
		private void LateUpdate()
		{
			if (page == Page.Playing && advanceAt < 0) UpdateNeedle();
		}

		private void BeginStep()
		{
			stepBegan = Time.realtimeSinceStartupAsDouble;
			StepView(run.stages[taps.Count]);
			UpdateNeedle();
		}

		private void UpdateNeedle()
		{
			Stage s = run.stages[taps.Count];
			// Submit the last rendered needle position, including taps after several full revolutions.
			displayedMs = (int)(((Time.realtimeSinceStartupAsDouble - stepBegan) * 1000) % s.periodMs);
			var phase = Mathf.Repeat((s.direction * displayedMs) / (float)s.periodMs, 1f);
			wheel.SetWheel(taps.Count, phase, s.targetBps / 10000f, s.windowBps / 10000f);
		}

		private void Strike()
		{
			if (page != Page.Playing || advanceAt >= 0) return;
			var s = run.stages[taps.Count];
			int sample = (displayedMs * 10000 / s.periodMs * s.direction + 10000) % 10000;
			int d = Math.Abs(sample - s.targetBps);
			int hit = Mathf.Clamp(100 - Math.Min(d, 10000 - d) * 50 / s.windowBps, 0, 100);
			taps.Add(displayedMs);
			hits.Add(hit);
			advanceAt = Time.unscaledTime + .72f;
			wheel.MarkHit(hit / 100f);
			HitView(s, hit);
		}

		private void Replay()
		{
			if (!busy && page == Page.Prep && plan != null && !string.IsNullOrEmpty(recipe.memoryQuality) && HaveMaterials(batchSize))
				Submit(Order("replay"));
		}

		private void Supply()
		{
			if (busy) return;
			var order = Order("supply");
			order.expectedPrice = catalog.canClaimStarter ? 0 : catalog.supplyPrice;
			Submit(order);
		}

		private void BuyMissing()
		{
			if (busy || plan == null || !plan.canBuyMissing || plan.shortageCost <= 0) return;
			var order = Order("buyMissing");
			order.expectedPrice = plan.shortageCost;
			Submit(order);
		}

		private void Submit(Request order)
		{
			pendingJson = JsonUtility.ToJson(order);
			PlayerPrefs.SetString(pendingKey, pendingJson);
			PlayerPrefs.Save();
			SendPending();
		}

		private void SendPending()
		{
			if (busy || string.IsNullOrEmpty(pendingJson)) return;
			busy = true;
			PendingView(true);
			string ticket = pendingJson;
			ForgeWire.Send(MsgType.AlchemyWorkshop, ticket, (ok, json) =>
			{
				if (!Active || ticket != PlayerPrefs.GetString(pendingKey)) return;
				busy = false;
				if (json == null) { PendingView(false); return; }
				var order = Read<Request>(ticket);
				bool boughtMaterials = order.action == "supply" || order.action == "buyMissing";
				var result = ok && !boughtMaterials ? Read<Receipt>(json) : null;
				if (ok && ((boughtMaterials && !ReadCatalog(json)) || (!boughtMaterials && (result == null || string.IsNullOrEmpty(result.message)))))
				{
					PendingView(false); return;
				}
				PlayerPrefs.DeleteKey(pendingKey);
				PlayerPrefs.Save();
				pendingJson = "";
				if (!ok) { notice = Error(json, "本炉未结算，请核对用料。"); LoadCatalog(); return; }
				var boxes = Singleton<RoleManager>.Instance.RoleBoxDic;
				if (boxes != null && boxes.TryGetValue(10, out var box))
					box.GetAllGoods(() => Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData(), false);
				if (boughtMaterials)
				{
					recipe = Array.Find(catalog.recipes, r => r.id == order.recipeId) ?? recipe;
					auxiliary.Clear();
					if (order.auxIds != null) auxiliary.AddRange(order.auxIds);
					notice = order.action == "buyMissing" ? $"已花 {order.expectedPrice} 灵石，本批缺材已入纳戒。" : "炉材已入纳戒。";
					Prepare(); return;
				}
				ReceiptView(result, order.action == "replay");
			});
		}

		private void Send(Request req, Action<bool, string> callback)
		{
			ForgeWire.Send(MsgType.AlchemyWorkshop, JsonUtility.ToJson(req), (ok, json) => { if (Active) callback(ok, json); });
		}

		private static T Read<T>(string json) where T : class
		{
			if (string.IsNullOrEmpty(json)) return null;
			try { return JsonUtility.FromJson<T>(json); }
			catch (ArgumentException) { return null; }
		}

		private static string Error(string json, string fallback) => Read<Failure>(json)?.error ?? fallback;
	}
}
