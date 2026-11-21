"""Check typed Unity call sites without a Unity installation."""
from pathlib import Path
from tempfile import TemporaryDirectory
import unittest

import refresh_go


class TypedWindowRequestTests(unittest.TestCase):
    def test_requests_keep_their_parser_channel_and_own_callback_fields(self):
        with TemporaryDirectory() as tmp:
            unity = Path(tmp)
            root = unity / "Assets/Scripts/Assembly-CSharp"
            network = root / "XiuZhenJie/Network"
            network.mkdir(parents=True)
            (network / "MsgType.cs").write_text("enum MsgType {\n PANEL = 1014012,\n GRADUATE = 1014027,\n}\n")
            for name, fields in {
                "EnterShiTuResp_1014012": ["ChuShiNum", "QiYun"],
                "ChuShiResp_1014027": ["QiYun"],
                "ChuShiReq_1014027": ["RoleId"],
            }.items():
                properties = "\n".join(f"public int {field} {{ get; set; }}" for field in fields)
                (root / f"{name}.cs").write_text(f"class {name} {{\n{properties}\n}}")
            source = """class TestWindow {
                void Open() {
                    Request(MsgType.PANEL, null, EnterShiTuResp_1014012.Parser, "读取…", reply => {
                        Show(reply.ChuShiNum);
                        var graduation = new ChuShiReq_1014027 { RoleId = "pupil" };
                        Send(MsgType.GRADUATE, graduation, ChuShiResp_1014027.Parser, reply2 => Show(reply2.QiYun));
                    });
                }
                void Forward(MsgType cmd, IMessage data) { var net = Singleton<NetManager>.Instance; net.SendMsg(cmd, data, null); }
            }"""
            (root / "TestWindow.cs").write_text(source)
            calls, *_ = refresh_go.collect(unity)
            panel, = calls["GamesNormalServer", 1014012]
            graduation, = calls["GamesNormalServer", 1014027]
            self.assertEqual((panel.request, panel.response_fields),
                             ("", (("EnterShiTuResp_1014012", ("ChuShiNum",)),)))
            self.assertEqual((graduation.request, graduation.response_fields),
                             ("ChuShiReq_1014027", (("ChuShiResp_1014027", ("QiYun",)),)))

    def test_alias_choice_wrapper_and_same_line_calls(self):
        with TemporaryDirectory() as tmp:
            unity = Path(tmp)
            root = unity / "Assets/Scripts/Assembly-CSharp"
            network = root / "XiuZhenJie/Network"
            network.mkdir(parents=True)
            (network / "MsgType.cs").write_text("enum MsgType {\n A = 1003001,\n B = 1003005,\n C = 1003006,\n}\n")
            for name in ("ListResp_1003001", "ReadResp_1003005", "ClaimResp_1003006"):
                (root / f"{name}.cs").write_text(f"class {name} {{ public int Code {{ get; set; }} }}")
            (root / "Inbox.cs").write_text("""class Inbox {
                void Open(bool list) {
                    var network = Singleton<NetManager>.Instance;
                    network.SendMsg(list ? MsgType.A : MsgType.B, null, bytes => {
                        if (list) Read(ListResp_1003001.Parser.ParseFrom(bytes));
                        else Read(ReadResp_1003005.Parser.ParseFrom(bytes));
                    });
                    network.SendMsg(MsgType.A, null, null); network.SendMsg(MsgType.B, null, null);
                }
                void Other() { var network = OtherService.Instance; network.SendMsg(MsgType.C, null, null); }
            }""")
            (root / "Actions.cs").write_text("""class Actions {
                void Claim() { SendAction(MsgType.C, null, ClaimResp_1003006.Parser, reply => Use(reply.Code)); }
            }""")
            calls, *_ = refresh_go.collect(unity)
            left, left_again = calls["GamesNormalServer", 1003001]
            right, right_again = calls["GamesNormalServer", 1003005]
            claim, = calls["GamesNormalServer", 1003006]
            self.assertEqual(left.offset, right.offset)
            self.assertNotEqual(left_again.offset, right_again.offset)
            self.assertEqual(left.response_fields, (("ListResp_1003001", ()),))
            self.assertEqual(right.response_fields, (("ReadResp_1003005", ()),))
            self.assertEqual(claim.response_fields, (("ClaimResp_1003006", ("Code",)),))

    def test_route_preview_and_forwarded_breakthrough(self):
        with TemporaryDirectory() as tmp:
            unity = Path(tmp)
            root = unity / "Assets/Scripts/Assembly-CSharp"
            network = root / "XiuZhenJie/Network"
            network.mkdir(parents=True)
            (network / "MsgType.cs").write_text("enum MsgType {\n ROOM = 1005008,\n BREAK = 1001010,\n}\n")
            for name in ("ChangeRunLocationReq_1001007", "RoomResp_1005008", "BreakResp_1001010"):
                (root / f"{name}.cs").write_text(f"class {name} {{ public bool IsSuccess {{ get; set; }} }}")
            (root / "Room.cs").write_text("""class Room {
                void Select() {
                    var req = new ChangeRunLocationReq_1001007();
                    Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.ROOM, req, bytes => {
                        var reply = RoomResp_1005008.Parser.ParseFrom(bytes); Show(reply.IsSuccess);
                    });
                    SendBreakRequest(MsgType.BREAK, bytes => {
                        var reply = BreakResp_1001010.Parser.ParseFrom(bytes); Show(reply.IsSuccess);
                    });
                }
                void SendBreakRequest(MsgType type, Action<ByteString> callback) {
                    Singleton<NetManager>.Instance.TrySendGameRequest(type, null, callback);
                }
            }""")
            calls, *_ = refresh_go.collect(unity)
            room, = calls["GamesNormalServer", 1005008]
            breakthrough, = calls["GamesNormalServer", 1001010]
            self.assertEqual((room.request, room.response_fields),
                             ("ChangeRunLocationReq_1001007", (("RoomResp_1005008", ("IsSuccess",)),)))
            self.assertEqual((breakthrough.request, breakthrough.response_fields),
                             ("", (("BreakResp_1001010", ("IsSuccess",)),)))

    def test_fallback_distinguishes_default_rejection_and_push_without_status(self):
        with TemporaryDirectory() as tmp:
            path = Path(tmp) / "GuildMonsterFightingBroadcastResp_1018002.cs"
            path.write_text("class Broadcast { public int Round { get; set; } }")
            types = {path.stem: path}
            fallback = {"1001002": "ActionBoolResp_1001001", "1018002": path.stem}
            normal, _ = refresh_go.behavior_for("GamesNormalServer", 1001002, {}, {}, {}, fallback, types, {1001002})
            self.assertIn("明确拒绝（4008）", normal)
            _, trial = refresh_go.behavior_for("GamesNormalServer", 1018002, {}, {}, {}, fallback, types, {1001002})
            self.assertEqual(trial, "无状态字段，试用模式不回包")


if __name__ == "__main__":
    unittest.main()
