using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace XiuZhenJie
{
    // ForgeWire sends the JSON envelope on the existing authenticated game socket.
    internal static class VoidRealmClient
    {
        private const MsgType Command = (MsgType)4901070;

        [Serializable]
        internal sealed class Order
        {
            public string action, hallId, name, tag, text, workId, requestId, mode, quoteId;
            public int version;
            public int[] steps;
            public int grade, brief, route, adjust, page;
        }

        [Serializable]
        internal sealed class Slot
        {
            public int type, level;
        }

        [Serializable]
        internal sealed class Hall
        {
            public string id;
            public int rank, cap;
        }

        [Serializable]
        internal sealed class Material
        {
            public string templateId, name;
            public int count, owned;
        }

        [Serializable]
        internal sealed class Wallet
        {
            public long stones, insight;
        }

        [Serializable]
        internal sealed class Axis
        {
            public string name;
            public int value, target;
        }

        [Serializable]
        internal class Result
        {
            public bool eligible, success;
            public int score, nodes, cost;
            public string title, reason, next, scoreText, costText;
            public string quoteId, suggestedName, effectText, craftText, qualityText, affixText, affinityText, valueText;
            public long value, tuition, stoneCost, insightCost;
            public Material[] materials;
            public Wallet wallet;
            public Axis[] axes;
        }

        [Serializable]
        internal sealed class Work
        {
            public string id, name, hallId, author, authorId;
            public int grade, brief, route, adjust, score, readerCount, place, version;
            public int[] steps;
            public string effectText, craftText, qualityText, affixText, affinityText, valueText, scoreText;
            public long value, tuition, stoneCost, insightCost;
            public Material[] materials;
            public long time;
            public bool studied, own;
        }

        [Serializable]
        internal sealed class Post
        {
            public string id, author, authorId, tag, text, workId;
            public long time;
        }

        [Serializable]
        internal sealed class Reply : Result
        {
            public string name, roleId, id, error;
            public int roleLevel, rank, cap;
            public bool more, retry;
            public Slot[] slots;
            public Hall[] halls;
            public Work work;
            public Work[] works;
            public Post post;
            public Post[] posts;
            public Result result;
        }

        private static string ServerScope()
        {
            var role = Singleton<RoleManager>.Instance;
            return Constants.LoginServerIP + ":" + Constants.LoginServerPort + "/" + role.ServerId;
        }

        internal static void Send(Order order, Action<Reply, string> done)
        {
            string role = Singleton<RoleManager>.Instance.currentRoleId;
            if (string.IsNullOrEmpty(role))
            {
                done(null, "请先登录角色，再入虚神界。");
                return;
            }

            string server = ServerScope();
            string receipt = null;
            if (order.action == "post" || order.action == "publish")
            {
                // Only the ticket is local. The service owns works, posts and receipts.
                // A lost response and a later retry of the same content share a ticket.
                order.requestId = null;
                using (var hash = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(server + "\n" + role + "\n" + JsonUtility.ToJson(order));
                    receipt = "VoidRealm.Pending." + BitConverter.ToString(hash.ComputeHash(bytes)).Replace("-", "");
                }
                order.requestId = PlayerPrefs.GetString(receipt);
                if (string.IsNullOrEmpty(order.requestId))
                {
                    order.requestId = Guid.NewGuid().ToString("N");
                    PlayerPrefs.SetString(receipt, order.requestId);
                    PlayerPrefs.Save();
                }
            }

            string ticket = order.requestId;
            ForgeWire.Send(Command, JsonUtility.ToJson(order), (ok, json) =>
            {
                Reply reply = null;
                if (!string.IsNullOrEmpty(json))
                {
                    try { reply = JsonUtility.FromJson<Reply>(json); }
                    catch (ArgumentException) { }
                }

                if (receipt != null && reply != null && (ok || (!reply.retry && !string.IsNullOrEmpty(reply.error))) &&
                    PlayerPrefs.GetString(receipt) == ticket)
                {
                    PlayerPrefs.DeleteKey(receipt);
                    PlayerPrefs.Save();
                }

                if (role != Singleton<RoleManager>.Instance.currentRoleId || server != ServerScope())
                {
                    done(null, "当前道籍已更换，请重新打开虚神界。");
                    return;
                }
                if (!ok || reply == null)
                {
                    string hint = receipt == null ? "暂未取得虚神界回执，请稍后重试。" : "回执尚未抵达。用相同内容重试即可核对原单。";
                    done(null, string.IsNullOrWhiteSpace(reply?.error) ? hint : reply.error);
                    return;
                }

                if (order.action == "publish" && reply.wallet != null)
                {
                    var manager = Singleton<RoleManager>.Instance;
                    if (manager.Role_Entity != null)
                    {
                        manager.Role_Entity.ShinyStone = reply.wallet.stones;
                        manager.Role_Entity.CurrentClever = (int)Math.Min(int.MaxValue, reply.wallet.insight);
                        Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
                        if (manager.RoleBoxDic != null && manager.RoleBoxDic.TryGetValue(10, out var ring))
                            ring.GetAllGoods(() => Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData(), false);
                    }
                }
                done(reply, null);
            });
        }
    }

    // Both the original seven-door directory and the new workshop use this server check.
    internal sealed class VoidRealmHallDoor
    {
        private readonly Window owner;
        private int revision;

        internal VoidRealmHallDoor(Window window) { owner = window; }
        internal void Cancel() { revision++; }

        internal void Open(string id)
        {
            string prefab;
            switch (id)
            {
                case "dan": prefab = "DanShiTaWnd.prefab"; break;
                case "qi": prefab = "LianQi_ChuangZaoMianWnd.prefab"; break;
                case "fu": prefab = "SFH_ShenFuHuiWnd.prefab"; break;
                case "shu": prefab = "WanFaGeWnd.prefab"; break;
                case "zhen": prefab = "ZS_MianWnd.prefab"; break;
                case "dao": prefab = "DaoJingShengDianWnd.prefab"; break;
                case "zhi": prefab = "LZ_MianWnd.prefab"; break;
                default: return;
            }
            int request = ++revision;
            VoidRealmClient.Send(new VoidRealmClient.Order {action = "enter", hallId = id}, (reply, error) =>
            {
                if (request != revision || owner.GameObject == null || !owner.GameObject.activeInHierarchy) return;
                if (reply == null || reply.id != id || reply.rank <= 0)
                {
                    MonoSingleton<GameStart>.Instance.PopUpAttention(error ?? "未取得本馆职印，请重试。");
                    return;
                }
                Singleton<UIManager>.Instance.PopUpWnd(prefab, true);
            });
        }
    }
}
