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


if __name__ == "__main__":
    unittest.main()
