using System;
using UnityEngine;

namespace XiuZhenJie
{
    internal static class ConcordDeskClient
    {
        [Serializable] internal class Order { public string action, main, second, quote, requestId; public long revision; }
        [Serializable] internal class Book { public string id, name, kind, element, note; public int grade; public long count, free; }
        [Serializable] internal class Receipt { public string text, relation; public int pills, ink, points; }
        [Serializable] internal class Reply
        {
            public string error, quote, relation, primerText;
            public bool retry, affordable;
            public long stones, revision;
            public int copyCost, bonus, points, pills, ink;
            public Book main, second;
            public Book[] books;
            public Receipt receipt;
        }
        static string Scope()
        {
            var r = Singleton<RoleManager>.Instance;
            return "ConcordDesk|" + Constants.LoginServerIP + "|" + Constants.LoginServerPort + "|" + r.ServerId + "|" + r.UserId + "|" + r.currentRoleId;
        }
        internal static Order Pending()
        {
            var json = PlayerPrefs.GetString(Scope(), "");
            if (json.Length == 0) return null;
            try { return JsonUtility.FromJson<Order>(json); } catch (ArgumentException) { return null; }
        }
        internal static void Send(Order order, Action<Reply, string> done)
        {
            var key = Scope();
            bool write = order.action == "commit" || order.action == "copy";
            if (write && string.IsNullOrEmpty(order.requestId)) order.requestId = Guid.NewGuid().ToString("N");
            string json = JsonUtility.ToJson(order);
            if (write)
            {
                string saved = PlayerPrefs.GetString(key, "");
                if (saved.Length > 0 && saved != json) { done(null, "请先核对上一笔回执"); return; }
                PlayerPrefs.SetString(key, json); PlayerPrefs.Save();
            }
            ForgeWire.Send((MsgType)4901072, json, (ok, raw) =>
            {
                Reply reply = null;
                if (!string.IsNullOrEmpty(raw)) { try { reply = JsonUtility.FromJson<Reply>(raw); } catch (ArgumentException) { } }
                if (write && reply != null && (ok || (!reply.retry && !string.IsNullOrEmpty(reply.error))) && PlayerPrefs.GetString(key) == json)
                { PlayerPrefs.DeleteKey(key); PlayerPrefs.Save(); }
                if (key != Scope()) { done(null, "道籍已更换，请重新打开合道台"); return; }
                if (!ok || reply == null) { done(null, reply?.error ?? (write ? "尚未收到回执，核对上次操作即可重试。" : "暂未连上书案，请刷新。")); return; }
                if (write)
                {
                    var role = Singleton<RoleManager>.Instance;
                    if (role.Role_Entity != null) role.Role_Entity.ShinyStone = reply.stones;
                    Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
                    if (role.RoleBoxDic != null && role.RoleBoxDic.TryGetValue(10, out var ring))
                        ring.GetAllGoods(() => Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData(), false);
                }
                done(reply, null);
            });
        }
    }
}
