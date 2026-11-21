using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public class CZF_MainContentPanel : MonoBehaviour
    {
        public Text TitleText;
        public Text ContentText;
        public Text CompeleteStatuText;
        public Text CompeleteSuccess;
        public Text CompeleteRunning;
        public UI_GoodsItem Goods;
        public Button LingQuBtn;
        private QuestHallView view;
        private ChengZhuFuMainWnd hall;
        private RoleInfoEntity lastRole;
        private int lastTask;
        private int lastStatus;
        private int pending;
        private string error;

        internal void Initialize(ChengZhuFuMainWnd owner, QuestHallView layout)
        {
            hall = owner;
            view = layout;
        }

        internal void Refresh(bool force)
        {
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            int task = role?.TaskId ?? 0;
            int status = role?.TaskStatus ?? 0;
            bool changed = role != lastRole || task != lastTask || status != lastStatus;
            if (!force && !changed) return;
            if (changed) error = null;
            lastRole = role;
            lastTask = task;
            lastStatus = status;
            view.Clear();
            var configs = Singleton<FacadeManager>.Instance.SelectMissionInfo();
            if (task <= 0 || configs == null || !configs.TryGetValue(task, out var mission))
            {
                bool finished = configs != null && configs.Count > 0 && task > configs.Keys.Max();
                view.Empty(finished ? "主线已完成" : "暂无可显示的主线任务", finished ? "本卷任务均已完成。" : "角色任务资料暂未就绪。");
                return;
            }

            var card = view.CardRow("当前任务");
            view.Line(card, status == 1 ? "可领取奖励" : status == 2 ? "已领取" : "进行中", 14, true, true);
            TitleText = view.Line(card, mission.name, 24, true);
            if (!string.IsNullOrWhiteSpace(mission.content))
                ContentText = view.Line(card, mission.content);
            var goals = view.CardRow("任务条件");
            view.Line(goals, "完成条件", 14, true, true);
            CompeleteStatuText = view.Line(goals, mission.condition, 17, true);
            if (!string.IsNullOrWhiteSpace(mission.award))
            {
                var icons = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
                Sprite icon = null;
                icons?.TryGetValue(mission.awardGoodsType, out icon);
                string quality = mission.awardGoodsRank > 0 ? WorldSetting.GetRankAllName(mission.awardGoodsRank) : "";
                view.Reward(view.CardRow("任务奖励"), mission.award, mission.awardNum, icon, quality);
            }
            if (!string.IsNullOrEmpty(error)) view.Line(view.Content, error);
            string label = pending == task ? "正在领取…" : status == 1 ? "领取奖励" : status == 2 ? "奖励已领取" : "完成任务后领取";
            LingQuBtn = view.Action(view.Content, label, Claim, status == 1 && pending == 0);
        }

        private void Claim()
        {
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            if (role == null || pending != 0 || role.TaskStatus != 1) return;
            int task = role.TaskId;
            pending = task;
            error = null;
            Refresh(true);
            var request = new ReceiveAwardReq_1016002 { TaskId = task };
            ChengZhuFuMainWnd.Request(MsgType.RECEIVE_AWARD, request, bytes =>
            {
                pending = 0;
                var response = bytes == null ? null : ReceiveAwardResp_1016002.Parser.ParseFrom(bytes);
                if (response != null && response.IsSuccess)
                {
                    if (role == Singleton<RoleManager>.Instance.Role_Entity && role.TaskId == task)
                    {
                        role.TaskId = task + 1;
                        role.TaskStatus = response.Status;
                        Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
                    }
                }
                else error = "未能领取奖励，请稍后重试。";
                if (hall.IsPage(0)) Refresh(true);
            });
        }
    }
}
