using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class TalismanStudio : MonoBehaviour
    {
        [Serializable] private class Choice { public string id, name, description; }
        [Serializable] private class Recipe { public string id, name, cost, output; public int maxSets; public bool available; }
        [Serializable] private class Material { public string id, name, source; public int count; }
        [Serializable] private class Good { public string id, name, description; public int count; public bool usable; }
        [Serializable] private class Plan
        {
            public string effect, seal, name, description, cost;
            public int power, tolerance, mastery; public bool canDraw, learned;
            public TalismanStroke[] strokes;
        }
        [Serializable] private class Catalog
        {
            public int rank, level, brushTier, training, nextRank, realmTier, supplyPrice, ward, battleWard, battleHaste, poison, batchLimit, vigor;
            public long stones;
            public string realm, brush, message, supplyText;
            public Choice[] effects, seals; public Recipe[] recipes; public Material[] materials; public Good[] goods; public Plan plan;
        }
        [Serializable] private class Request
        {
            public string action, effect, seal, recipe, goodsId, requestId;
            public int expectedPrice, quantity; public TalismanStroke[] strokes;
        }
        [Serializable] private class Rejection { public string error; }
        private enum Page { Desk, Stock, Writing, Pouch }
        private Page page;
        private Catalog data;
        private readonly List<TalismanStroke> written = new List<TalismanStroke>();
        private string pendingKey, effect = "ward", seal = "simple", notice, focusGood;
        private bool pouchOnly, closed, busy;
        private int batchSize = 1;
        private Action onClose, onLegacy;
        private Font font;

        internal static bool IsStudioGood(BaseNaJieGoodEntity good) => good != null && good.Type == (int)GoodsTypeEnum.SIGN &&
            good.TemplateId != null && (good.TemplateId == "preview-sign" || good.TemplateId.StartsWith("sigil.", StringComparison.Ordinal));

        internal static void DescribeGood(string id, Action<string> received)
        {
            ForgeWire.Send(MsgType.TalismanStudio, JsonUtility.ToJson(new Request { action = "catalog" }), (ok, json) => {
                var catalog = ok ? Read<Catalog>(json) : null;
                var row = catalog?.goods == null ? null : Array.Find(catalog.goods, g => g.id == id);
                received(row?.description ?? "可在画符静室的符匣查验并启用。");
            });
        }

        internal static void Open(Transform parent, Font sourceFont, Action close, Action legacy, string oldTitle, string bagGood = null)
        {
            if (parent.Find("TalismanStudio") != null) return;
            var root = Area("TalismanStudio", parent, 0, 1, 0, 1);
            var backdrop = root.gameObject.AddComponent<Image>();
            backdrop.color = Background;
            var view = root.gameObject.AddComponent<TalismanStudio>();
            view.font = sourceFont; view.onClose = close; view.onLegacy = legacy; view.legacyTitle = oldTitle;
            view.pouchOnly = bagGood != null; view.focusGood = bagGood;
            view.page = view.pouchOnly ? Page.Pouch : Page.Desk;
            view.pendingKey = $"TalismanStudio.{Constants.LoginServerIP}.{Constants.LoginServerPort}.{Singleton<RoleManager>.Instance.currentRoleId}";
            if (PlayerPrefs.HasKey(view.pendingKey)) view.Waiting("上次操作等待核对回执。", view.Resume);
            else view.Fetch();
        }

        internal static void CloseIn(Transform parent) => parent.Find("TalismanStudio")?.GetComponent<TalismanStudio>()?.Close(null);

        private void Close(Action next)
        {
            if (closed) return;
            closed = true;
            transform.SetParent(null, false);
            gameObject.SetActive(false);
            Destroy(gameObject);
            next?.Invoke();
        }

        private static T Read<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json)) return null;
            try { return JsonUtility.FromJson<T>(json); } catch (ArgumentException) { return null; }
        }

        private bool Receive(string json)
        {
            var value = Read<Catalog>(json);
            var p = value?.plan;
            if (p?.strokes == null || p.strokes.Length == 0 || value.effects == null || value.seals == null ||
                Array.Exists(p.strokes, s => s?.points == null || s.points.Length < 2 || Array.Exists(s.points, v => v == null))) return false;
            data = value; effect = p.effect; seal = p.seal;
            return true;
        }

        private void Fetch()
        {
            if (busy) return;
            busy = true;
            Waiting("正在展开符谱…", null);
            var query = new Request { action = data == null ? "catalog" : "plan", effect = effect, seal = seal };
            ForgeWire.Send(MsgType.TalismanStudio, JsonUtility.ToJson(query), (ok, json) => {
                if (this == null || closed) return;
                busy = false;
                if (!ok || !Receive(json)) { Waiting(Read<Rejection>(json)?.error ?? "未读到符谱，请重试。", Fetch); return; }
                Render();
            });
        }

        private void CycleBatch()
        {
            if (busy) return;
            int next = batchSize == 1 ? 5 : batchSize == 5 ? 10 : batchSize == 10 ? 20 : 1;
            batchSize = next <= data.batchLimit ? next : 1;
            notice = null;
            Render();
        }

        private void Order(string action, string recipeId = null, string goodsId = null)
        {
            if (busy || PlayerPrefs.HasKey(pendingKey)) return;
            var request = new Request { action = action, effect = effect, seal = seal, recipe = recipeId, goodsId = goodsId,
                requestId = Guid.NewGuid().ToString("N"), expectedPrice = data.supplyPrice + (action == "supply" ? (batchSize-1)*36 : 0),
                quantity = action == "copy" || action == "supply" || (action == "make" && recipeId != "brushKit" && recipeId != "brush") ? batchSize : 0,
                strokes = action == "draw" ? written.ToArray() : null };
            PlayerPrefs.SetString(pendingKey, JsonUtility.ToJson(request)); PlayerPrefs.Save();
            Resume();
        }

        private void Resume()
        {
            if (busy) return;
            string ticket = PlayerPrefs.GetString(pendingKey);
            var request = Read<Request>(ticket);
            if (request == null || string.IsNullOrEmpty(request.requestId)) { PlayerPrefs.DeleteKey(pendingKey); PlayerPrefs.Save(); Fetch(); return; }
            busy = true;
            Waiting("正在核对本次操作…", null);
            ForgeWire.Send(MsgType.TalismanStudio, ticket, (ok, json) => {
                if (this == null || closed || ticket != PlayerPrefs.GetString(pendingKey)) return;
                busy = false;
                if (json == null || (ok && !Receive(json))) { Waiting("回执尚未送达，可按原单重查。", Resume); return; }
                PlayerPrefs.DeleteKey(pendingKey); PlayerPrefs.Save();
                if (!ok) { notice = Read<Rejection>(json)?.error ?? "未能结算，请核对用料。"; Fetch(); return; }
                notice = data.message;
                var role = Singleton<RoleManager>.Instance.Role_Entity;
                if (role != null)
                {
                    role.ShinyStone = data.stones; role.CurrentPoisonVal = data.poison;
                    if (role.Live1Type == 3) { role.Live1Level = data.rank; role.Live1Value = data.training; }
                    else if (role.Live2Type == 3) { role.Live2Level = data.rank; role.Live2Value = data.training; }
                }
                switch (request.action)
                {
                    case "draw": case "copy": case "use": page = Page.Pouch; focusGood = null; written.Clear(); break;
                    default: page = pouchOnly ? Page.Pouch : Page.Stock; break;
                }
                var boxes = Singleton<RoleManager>.Instance.RoleBoxDic;
                if (boxes != null && boxes.TryGetValue(10, out var box))
                    box.GetAllGoods(() => Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData(), false);
                Render();
            });
        }

        private void Navigate(Page next)
        {
            if (busy) return;
            notice = null; page = next; Render();
        }

        private void Choose(string selectedEffect, string selectedSeal)
        {
            if (busy || (effect == selectedEffect && seal == selectedSeal)) return;
            effect = selectedEffect; seal = selectedSeal; notice = null; written.Clear(); Fetch();
        }
    }
}
