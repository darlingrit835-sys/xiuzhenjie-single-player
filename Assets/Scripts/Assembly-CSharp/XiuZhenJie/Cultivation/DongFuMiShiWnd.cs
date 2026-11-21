using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
    public class DongFuMiShiWnd : Window
    {
        public DongFuMiShiPanel Panel;

        private static readonly int[] BoxPrices = { 10000, 100000, 1000, 10000 };
        private readonly List<string> boxes = new List<string>(5);
        private CaveChamberView view;
        private string roleId;
        private int session;
        private int actualGrade = -1;
        private int selectedGrade;
        private bool selectedByPlayer;
        private bool readingBoxes, haveBoxes, writing;
        private string boxError, rankError;

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            Panel = GameObject.GetComponent<DongFuMiShiPanel>();
            view = new CaveChamberView(this, Panel);
            DongFuResidenceState.LevelChanged += OnGradeChanged;
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            roleId = DongFuResidenceState.CurrentRoleId;
            session++;
            actualGrade = -1;
            selectedGrade = 0;
            selectedByPlayer = false;
            writing = false;
            boxes.Clear();
            view.CancelRename();
            Load();
        }

        private bool Current(int ticket) => ticket == session && GameObject != null &&
            roleId == DongFuResidenceState.CurrentRoleId;

        internal void Load()
        {
            if (writing) return;
            int ticket = session;
            var net = Singleton<NetManager>.Instance;
            boxError = rankError = null;
            readingBoxes = true;
            haveBoxes = false;
            Render();
            if (!net.TrySendGameRequest(MsgType.ENTER_DONGFU, null, bytes =>
            {
                if (!Current(ticket)) return;
                var reply = bytes == null ? null : EnterHomeResp_1005001.Parser.ParseFrom(bytes);
                if (reply != null && reply.IsSuccess)
                    ApplyGrade(DongFuResidenceState.ApplyRank(roleId, reply.Rank));
                else
                    rankError = "洞府品阶读取失败";
                Render();
            }))
                rankError = "未连接服务器";

            if (!net.TrySendGameRequest(MsgType.ENTER_SPACE_BOX, null, bytes =>
            {
                if (!Current(ticket)) return;
                readingBoxes = false;
                var reply = bytes == null ? null : EnterSpaceBoxResp_1005005.Parser.ParseFrom(bytes);
                if (reply != null && reply.IsSuccess && reply.Name.Count > 0)
                {
                    boxes.Clear();
                    for (int i = 0; i < Mathf.Min(reply.Name.Count, 5); i++)
                        boxes.Add(reply.Name[i]);
                    haveBoxes = true;
                }
                else
                    boxError = reply != null && !reply.IsSuccess ? Hint(reply.Code) : "箱册暂未读取";
                Render();
            }))
            {
                readingBoxes = false;
                boxError = "未连接服务器";
            }
            Render();
        }

        private static string Hint(int code) => MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code);

        private void ApplyGrade(int grade)
        {
            if (!selectedByPlayer || selectedGrade == actualGrade)
            {
                selectedGrade = grade;
                selectedByPlayer = false;
            }
            actualGrade = grade;
            rankError = null;
        }

        private void OnGradeChanged(string changedRole, int grade)
        {
            if (changedRole != roleId || GameObject == null || !GameObject.activeSelf) return;
            ApplyGrade(grade);
            Render();
            view.Pulse();
        }

        internal void Preview(int grade)
        {
            selectedByPlayer = grade != actualGrade;
            selectedGrade = grade;
            view.ShowGrade(actualGrade, selectedGrade);
            view.Pulse();
        }

        internal void ReturnToCurrent()
        {
            if (actualGrade >= 0) Preview(actualGrade);
        }

        internal void Upgrade()
        {
            Singleton<UIManager>.Instance.PopUpWnd("DongFuChooseWnd.prefab", true);
        }

        internal void Close() => Singleton<UIManager>.Instance.CloseWnd(this);

        private void Render()
        {
            view.ShowGrade(actualGrade, selectedGrade);
            string price = haveBoxes && boxes.Count < 5
                ? BoxPrices[boxes.Count - 1].ToString("N0") + (boxes.Count < 3 ? " 灵石" : " 灵玉")
                : "";
            view.ShowBoxes(boxes, haveBoxes, writing, price, readingBoxes ? "正在读取箱册…" : boxError,
                !readingBoxes && (rankError != null || boxError != null), rankError);
        }

        private bool Owns(int number) => !writing && haveBoxes && number >= 1 && number <= boxes.Count;

        internal void OpenBox(int number)
        {
            if (!Owns(number)) return;
            Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true, number.ToString());
        }

        internal void AskRename(int number)
        {
            if (Owns(number)) view.BeginRename(number, boxes[number - 1]);
        }

        internal void Rename(int number, string proposed)
        {
            if (!Owns(number)) return;
            string name = proposed.Trim();
            if (name.Length == 0 || name.IndexOfAny(new[] { '<', '>' }) >= 0)
            {
                MonoSingleton<GameStart>.Instance.PopUpAttention("请输入有效的箱名。");
                return;
            }
            if (name == boxes[number - 1])
            {
                view.CancelRename();
                return;
            }
            if (Singleton<RoleManager>.Instance.Role_Entity.ShinyJade < 5)
            {
                MonoSingleton<GameStart>.Instance.PopUpAttention("改名需要 5 灵玉。");
                return;
            }

            var request = new RenameBoxReq_1005007 { Location = number, NewName = name };
            Write(MsgType.RENAME_BOX, request, bytes =>
            {
                var reply = RenameBoxResp_1005007.Parser.ParseFrom(bytes);
                if (reply.IsSuccess)
                    Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = reply.ShinyJade;
                return reply.IsSuccess ? null : Hint(reply.Code);
            }, () =>
            {
                boxes[number - 1] = name;
                view.CancelRename();
            });
        }

        internal void AskBuy()
        {
            if (!haveBoxes || writing || boxes.Count == 0 || boxes.Count >= 5) return;
            int count = boxes.Count, ticket = session;
            int currency = count < 3 ? 1 : 2;
            var confirm = new UnityAction<int, int>((price, type) =>
            {
                if (Current(ticket) && haveBoxes && !writing && boxes.Count == count)
                    Buy();
            });
            Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, BoxPrices[count - 1], confirm, currency);
        }

        private void Buy()
        {
            string newName = null;
            Write(MsgType.BUYSPACEBOX, null, bytes =>
            {
                var reply = BuySpaceBoxResp_1005021.Parser.ParseFrom(bytes);
                if (!reply.IsSuccess) return Hint(reply.Code);
                var role = Singleton<RoleManager>.Instance.Role_Entity;
                role.ShinyJade = reply.ShinyJade;
                role.ShinyStone = reply.ShinyStone;
                newName = reply.Name;
                return null;
            }, () =>
            {
                boxes.Add(newName);
                view.ScrollToNewest();
            });
        }

        private void Write(MsgType command, IMessage request, Func<ByteString, string> receipt, Action updateView)
        {
            int ticket = session;
            string owner = roleId;
            writing = true;
            Render();
            if (Singleton<NetManager>.Instance.TrySendGameRequest(command, request, bytes =>
            {
                if (owner != DongFuResidenceState.CurrentRoleId) return;
                string error = bytes == null ? "回执未到，请重新读取箱册。" : receipt(bytes);
                if (bytes != null && error == null)
                    Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
                if (!Current(ticket)) return;
                writing = false;
                if (error == null)
                {
                    updateView();
                    view.Pulse();
                }
                else
                {
                    MonoSingleton<GameStart>.Instance.PopUpAttention(error);
                    if (bytes == null)
                    {
                        view.CancelRename();
                        haveBoxes = false;
                        boxError = error;
                    }
                }
                Render();
            })) return;
            writing = false;
            MonoSingleton<GameStart>.Instance.PopUpAttention("未连接服务器，请重新读取箱册。");
            view.CancelRename();
            haveBoxes = false;
            boxError = "未连接服务器";
            Render();
        }

        public override void OnDisable()
        {
            session++;
            base.OnDisable();
        }

        public override void OnClose()
        {
            DongFuResidenceState.LevelChanged -= OnGradeChanged;
            base.OnClose();
        }
    }
}
