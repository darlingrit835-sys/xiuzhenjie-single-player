using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class GuildBountyPage
    {
        private readonly ChengZhuFuMainWnd hall;
        private readonly QuestHallView view;
        private readonly List<KeyValuePair<Text, long>> timers = new List<KeyValuePair<Text, long>>();
        private IntoWaiWuResp_1015038 listing;
        private RoleInfoEntity role;
        private string guildId;
        private int version;
        private bool settling;
        private int settlingId;
        private float readTime;
        private long lastSecond;
        private string error;

        internal GuildBountyPage(ChengZhuFuMainWnd hall, QuestHallView view)
        {
            this.hall = hall;
            this.view = view;
        }

        internal void Open()
        {
            int revision = hall.Revision;
            int request = ++version;
            role = Singleton<RoleManager>.Instance.Role_Entity;
            guildId = role?.ZongMenId;
            listing = null;
            error = null;
            timers.Clear();
            view.Clear();
            if (string.IsNullOrWhiteSpace(guildId) || guildId == "0")
            {
                view.Empty("尚未加入宗门", "加入宗门后，可在此委托外务殿寻物并结算悬赏。");
                return;
            }

            view.Line(view.Content, "正在读取宗门悬赏…");
            ChengZhuFuMainWnd.Request(MsgType.INTOWAIWU, null, bytes =>
            {
                if (!hall.IsPage(1) || hall.Revision != revision || version != request || !SameGuild()) return;
                var reply = bytes == null ? null : IntoWaiWuResp_1015038.Parser.ParseFrom(bytes);
                if (reply == null || !reply.IsSuccess)
                {
                    view.Clear();
                    view.Empty("暂时无法读取宗门悬赏", Failure(reply?.Code, "请稍后重新加载。"), Open);
                    return;
                }
                listing = reply;
                readTime = Time.realtimeSinceStartup;
                Draw();
            });
        }

        private bool SameGuild() => role == Singleton<RoleManager>.Instance.Role_Entity && role?.ZongMenId == guildId;

        private static string Failure(int? code, string fallback)
        {
            if (code.GetValueOrDefault() == 0) return fallback;
            string reason = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code.Value);
            return string.IsNullOrWhiteSpace(reason) ? fallback : reason;
        }

        private void Draw()
        {
            view.Clear();
            timers.Clear();
            lastSecond = -1;
            var office = view.CardRow("宗门外务殿");
            view.Line(office, "宗门外务殿", 21, true);
            view.Line(office, $"我的悬赏  {listing.XuanShangInfo.Count} / 2    ·    宗门贡献  {listing.CurrentContribution}");
            if (!string.IsNullOrEmpty(error)) view.Line(office, error, 15, true);
            if (listing.XuanShangInfo.Count == 0)
                view.Line(office, "暂无寻物悬赏。发布时需提供物品全名并支付灵石报酬。");
            int elderBonus = listing.ZhangLaoInfo == null ? 0 : (listing.ZhangLaoInfo.Level - 1) / 10 + 1;
            bool hasSlot = listing.XuanShangInfo.Count < 2;
            view.Action(office, hasSlot ? "发布寻物悬赏" : "悬赏已满", () => hall.OpenPublisher(elderBonus), hasSlot && !settling);
            foreach (var entry in listing.XuanShangInfo)
            {
                var card = view.CardRow("寻物悬赏");
                string status = entry.Status == 1 ? "可结算" : entry.Status == 0 ? "寻物中" : "暂不可办理";
                view.Line(card, status, 13, true, true);
                view.Line(card, entry.GoodsName, 20, true);
                view.Line(card, $"悬赏报酬  {entry.Stone} 灵石    ·    成功率  {entry.SuccessRate}%");
                if (entry.Status == 0)
                    timers.Add(new KeyValuePair<Text, long>(view.Line(card, ""), entry.ShengYuTime));
                else if (entry.Status == 1)
                    view.Action(card, settling && settlingId == entry.Id ? "正在结算…" : "结算悬赏", () => Settle(entry.Id), !settling);
            }
            view.Action(view.Content, "刷新悬赏", Open, !settling);
            view.Action(view.Content, "进入外务殿", () => hall.OpenGuildOffice(listing), !settling);
            Update();
        }

        internal void Update()
        {
            if (!SameGuild())
            {
                Open();
                return;
            }
            if (timers.Count == 0) return;
            long seconds = (long)(Time.realtimeSinceStartup - readTime);
            if (seconds == lastSecond) return;
            lastSecond = seconds;
            foreach (var pair in timers)
            {
                long total = Math.Max(0, pair.Value);
                long remaining = total / 1000 + (total % 1000 > 0 ? 1 : 0) - seconds;
                pair.Key.text = remaining <= 0 ? "预计寻物时间已到，可刷新查看结果" :
                    remaining < 60 ? $"预计剩余  {remaining} 秒" : $"预计剩余  {remaining / 60} 分 {remaining % 60} 秒";
            }
        }

        private void Settle(int id)
        {
            if (settling || !SameGuild() || !hall.IsPage(1)) return;
            var owner = role;
            string ownerGuild = guildId;
            settling = true;
            settlingId = id;
            error = null;
            Draw();
            var request = new WaiWuJieSuanXuanShangReq_1015041 { Id = id };
            ChengZhuFuMainWnd.Request(MsgType.WAIWUJIESUANXUANSHANG, request, bytes =>
            {
                settling = false;
                if (!hall.IsPage(1) || owner != Singleton<RoleManager>.Instance.Role_Entity || owner?.ZongMenId != ownerGuild) return;
                var reply = bytes == null ? null : WaiWuJieSuanXuanShangResp_1015041.Parser.ParseFrom(bytes);
                if (reply != null && reply.IsSuccess)
                {
                    Open();
                    return;
                }
                if (listing == null) return;
                error = Failure(reply?.Code, "悬赏未能结算，请稍后重试。");
                Draw();
            });
        }
    }
}
