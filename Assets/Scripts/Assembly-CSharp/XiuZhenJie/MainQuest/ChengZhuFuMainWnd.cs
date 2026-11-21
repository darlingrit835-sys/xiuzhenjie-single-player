using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
    public class ChengZhuFuMainWnd : Window
    {
        public ChengZhuFuMainPanel Panel;
        private QuestHallView view;
        private GuildBountyPage bounties;
        private int page = -1;
        private int previous;
        private int revision;
        private bool shown;
        private string guildWindow;

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            Panel = GameObject.GetComponent<ChengZhuFuMainPanel>();
            view = new QuestHallView(Panel, () => Select(0), () => Select(1), () => Select(2), Back);
            Panel.CZF_ContentPanel.Initialize(this, view);
            Panel.CZF_TSB_ContentPanel.Initialize(this, view);
            bounties = new GuildBountyPage(this, view);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            shown = true;
            Select(page < 0 ? 0 : page, true);
        }

        private void Back()
        {
            if (page == 2) Select(previous);
            else CloseWindow();
        }

        private void Select(int next, bool refresh = false)
        {
            if (!refresh && page == next) return;
            if (next == 2 && page != 2) previous = page < 0 ? 0 : page;
            page = next;
            revision++;
            view.Select(page);
            if (page == 0) Panel.CZF_ContentPanel.Refresh(true);
            else if (page == 1) bounties.Open();
            else Panel.CZF_TSB_ContentPanel.Open();
        }

        internal bool IsPage(int index) => shown && GameObject != null && page == index;
        internal int Revision => revision;

        internal static void Request(MsgType type, IMessage data, UnityAction<ByteString> result)
        {
            if (!Singleton<NetManager>.Instance.TrySendGameRequest(type, data, bytes => result(bytes)))
                result(null);
        }

        internal void OpenPublisher(int skill)
        {
            guildWindow = "ZongMen_WaiWuDianXiangQingWnd.prefab";
            Singleton<UIManager>.Instance.PopUpWnd(guildWindow, true, skill);
        }

        internal void OpenGuildOffice(IntoWaiWuResp_1015038 response)
        {
            guildWindow = "ZongMen_WaiWuDianWnd.prefab";
            Singleton<UIManager>.Instance.PopUpWnd(guildWindow, true, new object[] { null, null, response });
        }

        public override void OnUpdate()
        {
            if (IsPage(0)) Panel.CZF_ContentPanel.Refresh(false);
            if (!IsPage(1)) return;
            bounties.Update();
            if (guildWindow != null && Singleton<UIManager>.Instance.FindWndByName(guildWindow) == null)
            {
                guildWindow = null;
                Select(1, true);
            }
        }

        public override void ReFlashData()
        {
            if (IsPage(0)) Panel.CZF_ContentPanel.Refresh(false);
        }

        public override void OnDisable()
        {
            shown = false;
            revision++;
            base.OnDisable();
        }

        public override void OnClose()
        {
            shown = false;
            revision++;
            view?.Dispose();
            base.OnClose();
        }
    }
}
