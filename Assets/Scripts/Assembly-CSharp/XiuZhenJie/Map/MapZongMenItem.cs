using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    public class MapZongMenItem : MonoBehaviour
    {
        // Original prefab bindings also supply the game's typeface and buttons.
        public RectTransform ZongMenPanel, FuDiPanel;
        public Button BaiRuShanMenBtn, QingChuYaoSouBtn;
        public Text QingChuYaoSouBtnText, BaiRuShanMenBtnText, ZongMenName, QingChuProgress;
        private Image stamp;
        private ExpeditionGlow motion;
        private Text status;

        public void SetZongMenValue(string zongMenId, string zongMenName, bool isYour,
            UnityAction<string, string> baiRuShanMenAction, UnityAction<string, string> backZongMen)
        {
            Build();
            ZongMenName.text = zongMenName;
            var guildId = Singleton<RoleManager>.Instance.Role_Entity?.ZongMenId;
            bool member = !string.IsNullOrEmpty(guildId) && guildId != "0";
            status.text = isYour ? "本宗驻地 · 可进入" : "山门可拜访";
            QingChuProgress.text = isYour ? "可返回宗门主峰。" : member ? "可查阅此宗门的名册。" : "可查看宗门详情、递交拜帖。";
            InkInterfaceArt.Set(stamp, "AtlasGate");
            motion.Bind(stamp, ExpeditionAtlas.Gold, false, transform.GetSiblingIndex());
            BaiRuShanMenBtnText.text = isYour ? "返回宗门" : "拜访山门";
            SetAction(BaiRuShanMenBtn, isYour ? backZongMen : baiRuShanMenAction, zongMenId, zongMenName);
            QingChuYaoSouBtn.gameObject.SetActive(false);
        }

        public void ReflashZongValue(string zongMenId, string zongMenName, UnityAction<string, string> backZongMen)
        {
            SetZongMenValue(zongMenId, zongMenName, true, null, backZongMen);
        }

        public void SetDongTianFuDiValue(string mapId, string goodsId, int currentProgress, int maxProgress,
            UnityAction<string, string> qingLiYaoShou, UnityAction<string, string> kaiZongLiPaiAction)
        {
            Build();
            ZongMenName.text = "洞天福地";
            bool cleared = currentProgress <= 0;
            status.text = cleared ? "可立山门" : "立派未解锁";
            QingChuProgress.text = cleared ? "本郡空地。持开山令可立宗，首枚可向城主府领取。" : $"剩余妖兽 {currentProgress} / {maxProgress}，清剿后可立派。";
            InkInterfaceArt.Set(stamp, cleared ? "AtlasGate" : "AtlasLair");
            motion.Bind(stamp, cleared ? ExpeditionAtlas.Gold : new Color32(196, 221, 226, 255), !cleared, transform.GetSiblingIndex());
            QingChuYaoSouBtnText.text = cleared ? "开宗立派" : "清理妖兽";
            SetAction(QingChuYaoSouBtn, cleared ? kaiZongLiPaiAction : qingLiYaoShou, mapId, goodsId);
            if (!cleared && string.IsNullOrEmpty(mapId))
            {
                QingChuYaoSouBtn.interactable = false;
                status.text = "暂不可前往";
                QingChuProgress.text = "此地暂无可清剿的历练区域。";
            }
            BaiRuShanMenBtn.gameObject.SetActive(false);
        }

        private void SetAction(Button button, UnityAction<string, string> action, string id, string name)
        {
            button.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            button.interactable = action != null;
            if (action != null) button.onClick.AddListener(() => { motion.Press(); action(id, name); });
        }

        private void Build()
        {
            if (stamp != null) return;
            var root = (RectTransform)transform;
            ZongMenPanel.gameObject.SetActive(false); FuDiPanel.gameObject.SetActive(false);
            Flat(GetComponent<Image>(), Color.clear);
            var ruler = RoleFolio.Image(root, "墨线", Rule);
            Band(ruler.rectTransform, 3, 3, 0, 1);
            stamp = RoleFolio.Image(root, "驻地地标", Color.white);
            // The motion component owns the floating art inside this fixed cell.
            var holder = Box(root, "驻地插画");
            Fixed(holder, 0, 1, 7, 64, 72);
            stamp.rectTransform.SetParent(holder, false); Fill(stamp.rectTransform);
            motion = stamp.gameObject.AddComponent<ExpeditionGlow>();
            ZongMenName.rectTransform.SetParent(root, false);
            Style(ZongMenName, 17, Ink, TextAnchor.MiddleLeft, true, 12);
            Band(ZongMenName.rectTransform, 69, 8, 6, 26);
            status = Label(root, ZongMenName.font, "", 13, Jade, TextAnchor.MiddleLeft, true);
            Band(status.rectTransform, 69, 85, 32, 20);
            QingChuProgress.rectTransform.SetParent(root, false);
            Style(QingChuProgress, 12, Secondary, TextAnchor.MiddleLeft, false, 10);
            Band(QingChuProgress.rectTransform, 69, 86, 52, 34);
            StyleAction(BaiRuShanMenBtn, BaiRuShanMenBtnText);
            StyleAction(QingChuYaoSouBtn, QingChuYaoSouBtnText);
        }

        private void StyleAction(Button button, Text text)
        {
            var rt = (RectTransform)button.transform;
            rt.SetParent(transform, false);
            Fixed(rt, 1, -2, 38, 80, 44);
            InkInterfaceArt.Set(button.image, "InkPlaque", false);
            Style(text, 14, Paper, TextAnchor.MiddleCenter, true, 11);
            Fill(text.rectTransform, 4, 4);
        }
    }
}
