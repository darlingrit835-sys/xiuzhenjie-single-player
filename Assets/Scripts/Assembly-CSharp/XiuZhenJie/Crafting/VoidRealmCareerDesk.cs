using System;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed class VoidRealmCareerDesk
    {
        private static readonly Color Ink = new Color32(32, 57, 54, 255);
        private static readonly Color Gold = new Color32(249, 221, 157, 255);
        private static readonly Color Paper = new Color32(244, 239, 218, 255);
        private readonly RectTransform root;
        private readonly Font font;
        private readonly Action refresh;
        private readonly int[] rooms = new int[2];
        private RectTransform mask, page;
        private bool ready, pending;

        internal int Grade { private get; set; }

        internal VoidRealmCareerDesk(RectTransform root, Font font, Action refresh)
        {
            this.root = root;
            this.font = font;
            this.refresh = refresh;
        }

        internal void SetRooms(int first, int second)
        {
            rooms[0] = first;
            rooms[1] = second;
            ready = true;
        }

        internal void Reset()
        {
            Close();
            ready = false;
        }

        private static string Room(int type)
        {
            var hall = Array.Find(VoidRealmCatalog.Halls, h => h.type == type);
            return hall == null ? "空室" : hall.room;
        }

        private void Close()
        {
            if (mask == null) return;
            mask.gameObject.SetActive(false);
            UnityEngine.Object.Destroy(mask.gameObject);
            mask = null;
        }

        private void Frame(string title)
        {
            Close();
            mask = RoleFolio.Image(root, "专职内室", new Color32(8, 19, 19, 193), true).rectTransform;
            RoleFolio.Fill(mask);
            page = RoleFolio.Image(mask, "开辟内室", Ink, true).rectTransform;
            Area(page, .045f, .16f, .955f, .84f);
            Label(title, 22, Gold, .07f, .83f, .75f, .96f);
            Button("收起", .78f, .84f, .94f, .95f, Close);
        }

        internal void Open()
        {
            if (pending) { Notice("府籍正在核对。"); return; }
            if (!ready) { Notice("请先重读府籍。"); return; }
            Frame("专职内室");
            Label("黄阶洞府可开一室，玄阶可开两室。开辟后取得相应的一品职印。", 16, Paper, .08f, .60f, .92f, .82f);
            for (int i = 0; i < rooms.Length; i++)
            {
                int index = i;
                string detail = rooms[i] != 0 ? Room(rooms[i]) + " · 改建" :
                    Grade > i ? "空室 · 开辟" : i == 0 ? "黄阶可开" : "玄阶可开";
                float top = .56f - i * .22f;
                Button((i == 0 ? "第一内室　" : "第二内室　") + detail, .08f, top - .17f, .92f, top,
                    rooms[i] != 0 || Grade > i ? (Action)(() => Choose(index)) : null);
            }
        }

        private void Choose(int index)
        {
            Frame(rooms[index] == 0 ? "选定专职" : "选择改修专职");
            var role = Singleton<RoleManager>.Instance.Role_Entity;
            foreach (var hall in VoidRealmCatalog.Halls)
            {
                int type = hall.type;
                bool held = (rooms[0] == type && role?.Live1Level > 0) || (rooms[1] == type && role?.Live2Level > 0) ||
                    (role?.Live3Type == type && role.Live3Level > 0) || (role?.Live4Type == type && role.Live4Level > 0);
                float left = .06f + (type - 1) % 3 * .30f, top = .78f - (type - 1) / 3 * .19f;
                Button(hall.job + (held ? " · 已修" : ""), left, top - .15f, left + .28f, top,
                    held ? null : (Action)(() => Confirm(index, hall)));
            }
            Button("返回内室", .20f, .07f, .80f, .19f, Open);
        }

        private void Confirm(int index, VoidRealmHall hall)
        {
            bool rebuild = rooms[index] != 0;
            string price = rebuild ? "20 灵玉" : (rooms[0] == 0 && rooms[1] == 0 ? "1,000 灵石" : "10,000 灵石");
            bool restore = rooms[index] == hall.type;
            Frame(restore ? "确认补修" : rebuild ? "确认改建" : "确认开辟");
            string detail = restore ? "此室已建，尚未入品。补修后可持一品职印入馆。" : rebuild ? "改建将撤去「" + Room(rooms[index]) + "」的职印，清除该室品阶及修习进度。已署名的作品保留。" : "开辟「" + hall.room + "」，取得" + hall.job + "一品职印。";
            Label(detail, 17, Paper, .08f, .53f, .92f, .80f);
            Label("新专职　" + hall.job + "　　所需　" + price, 16, Gold, .08f, .35f, .92f, .50f);
            Button("返回选择", .08f, .13f, .46f, .30f, () => Choose(index));
            Button("支付并" + (restore ? "补修" : rebuild ? "改建" : "开辟"), .51f, .13f, .92f, .30f, () => Submit(index, hall.type));
        }

        private void Submit(int index, int type)
        {
            if (pending) return;
            string owner = DongFuResidenceState.CurrentRoleId;
            bool rebuild = rooms[index] != 0;
            IMessage request = rebuild ? (IMessage)new RebuildLiveTypeReq_1005004 { Location=index+1, NewLiveType=type }
                : new BuildLiveTypeReq_1005003 { Location=index+1, LiveType=type };
            // Optional field 3 describes both rooms the player just confirmed.
            request.MergeFrom(new byte[] {24, (byte)(10 * rooms[0] + rooms[1])});
            pending = true;
            bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(rebuild ? MsgType.REBUILD_LIVE_TYPE : MsgType.BUILD_LIVE_TYPE, request, bytes =>
            {
                pending = false;
                if (root == null || owner != DongFuResidenceState.CurrentRoleId) return;
                bool ok = false;
                int code = 0;
                var role = Singleton<RoleManager>.Instance.Role_Entity;
                if (bytes != null && rebuild)
                {
                    var reply = RebuildLiveTypeResp_1005004.Parser.ParseFrom(bytes);
                    ok = reply.IsSuccess;
                    code = reply.Code;
                    if (ok && role != null) role.ShinyJade = reply.ShinyJade;
                }
                else if (bytes != null)
                {
                    var reply = BuildLiveTypeResp_1005003.Parser.ParseFrom(bytes);
                    ok = reply.IsSuccess;
                    code = reply.Code;
                    if (ok && role != null) role.ShinyStone = reply.ShinyStone;
                }
                if (ok && role != null)
                {
                    if (index == 0) { role.Live1Type = type; role.Live1Level = 1; role.Live1Value = 0; }
                    else { role.Live2Type = type; role.Live2Level = 1; role.Live2Value = 0; }
                    Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ReFlashData();
                }
                if (!root.gameObject.activeInHierarchy) return;
                refresh();
                Notice(ok ? "已修成「" + Room(type) + "」，一品职印已入府籍。" :
                    bytes == null ? "回执未抵达，正在重读府籍。" : code == 4008 ? "府籍已更新，请重选内室。" :
                    MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code));
            });
            if (!sent) { pending = false; Notice("未连接游戏服，请重连后再核对府籍。"); }
        }

        private static void Notice(string message) => MonoSingleton<GameStart>.Instance.PopUpAttention(message);

        private Text Label(string words, int size, Color color, float x1, float y1, float x2, float y2)
        {
            var text = RoleFolio.Label(page, font, words, size, color, TextAnchor.MiddleLeft);
            Area(text.rectTransform, x1, y1, x2, y2);
            return text;
        }

        private void Button(string words, float x1, float y1, float x2, float y2, Action action)
        {
            var image = RoleFolio.Image(page, words, new Color32(60, 83, 75, 255), true);
            Area(image.rectTransform, x1, y1, x2, y2);
            var label = RoleFolio.Label(image.transform, font, words, 15, action == null ? Color.gray : Gold, TextAnchor.MiddleCenter);
            RoleFolio.Fill(label.rectTransform, 3, 3);
            label.resizeTextForBestFit = true;
            label.resizeTextMinSize = 11;
            label.resizeTextMaxSize = 15;
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.interactable = action != null;
            if (action != null) button.onClick.AddListener(() => action());
        }

        private static void Area(RectTransform area, float left, float bottom, float right, float top)
        {
            area.anchorMin = new Vector2(left, bottom); area.anchorMax = new Vector2(right, top);
            area.offsetMin = area.offsetMax = Vector2.zero;
        }
    }
}
