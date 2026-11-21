using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    internal sealed class WorldAtlasView : MonoBehaviour
    {
        [SerializeField] private Text context;

        internal void Build(WorldMapPanel panel)
        {
            var font = panel.TitleText.font;
            var title = (RectTransform)panel.TitleText.transform.parent;
            Band(title, 0, 0, 0, 54);
            Flat(title.GetComponent<Image>(), Paper);
            var rule = RoleFolio.Image(title, "舆图页线", Rule);
            Band(rule.rectTransform, 12, 12, 53, 1);
            var plate = (RectTransform)panel.BackBtn.transform.parent;
            Fixed(plate, 0, 6, 4, 53, 46);
            var backdrop = plate.GetComponent<Image>();
            if (backdrop != null) Flat(backdrop, Color.clear);
            Fill((RectTransform)panel.BackBtn.transform);
            Touch(panel.BackBtn);
            var back = panel.BackBtn.GetComponentInChildren<Text>(true);
            if (back != null) { Fill(back.rectTransform, 14, 0, 0, 0); Style(back, 14, Ink, TextAnchor.MiddleCenter); }
            var arrow = panel.BackBtn.transform.Find("Img")?.GetComponent<Image>();
            if (arrow != null) { InkInterfaceArt.Set(arrow, "Back"); Fixed(arrow.rectTransform, 0, 0, 15, 15, 16); arrow.raycastTarget = false; }
            context = Label(title, font, "舆图", 11, Secondary, TextAnchor.MiddleCenter);
            Band(context.rectTransform, 61, 61, 3, 18);
            Band(panel.TitleText.rectTransform, 57, 57, 20, 29);
            Style(panel.TitleText, 21, Ink, TextAnchor.MiddleCenter, true, 13);

            var help = panel.XiaoWenHao;
            Fixed((RectTransform)help.transform, 1, -5, 5, 45, 43);
            Touch(help);
            foreach (Transform child in help.transform) child.gameObject.SetActive(false);
            var question = RoleFolio.Image(help.transform, "地图说明", Color.white);
            InkInterfaceArt.Set(question, "Help"); Fill(question.rectTransform, 11, 11, 10, 10);
            var content = panel.transform.Find("Content") as RectTransform;
            if (content != null) Fill(content, 0, 0, 55, 0);
            title.SetAsLastSibling(); help.transform.SetAsLastSibling();
            PrepareTransfer(panel, font);
        }

        internal void ShowContext(MapEnterType mode, CSZ_MapPanel map)
        {
            var sample = map != null && map.PopulatedCount > 0 ? map.BtnDataList[0].dataSource : null;
            if (mode == MapEnterType.WaiChuLiLiang)
            {
                var role = Singleton<RoleManager>.Instance.Role_Entity;
                var panel = GetComponent<WorldMapPanel>();
                bool gates = panel.ZongPaiShanMenTog.isOn;
                string sectionName = gates ? "山门寻访" : "历练舆图";
                context.text = role == null ? sectionName : $"{sectionName}   ·   境界 {role.RoleLevel} 级   ·   精力 {role.CurrentVigor}";
                return;
            }
            string purpose = mode == MapEnterType.CreateRole ? "出生地" : mode == MapEnterType.ChuanSong ? "传送舆图" : "历练舆图";
            string section = sample == null ? "" : sample.Depth switch { 1 => "大陆", 2 => "诸洲", 3 => "郡域", 4 => "城池山野", _ => "历练地" };
            context.text = section.Length == 0 ? purpose : purpose + "  ·  " + section;
        }

        private static void PrepareTransfer(WorldMapPanel panel, Font font)
        {
            var modal = panel.TiShiPanel;
            var root = (RectTransform)modal.transform;
            root.SetParent(panel.transform, false); Fill(root);
            foreach (Transform child in root) child.gameObject.SetActive(false);
            if (!root.TryGetComponent(out Image wash)) wash = root.gameObject.AddComponent<Image>();
            Flat(wash, new Color32(34, 51, 43, 130), true);
            var sheet = RoleFolio.Image(root, "传送函", Paper, true).rectTransform;
            sheet.anchorMin = new Vector2(.06f,.5f); sheet.anchorMax = new Vector2(.94f,.5f);
            sheet.pivot=new Vector2(.5f,.5f);sheet.sizeDelta=new Vector2(0,218);sheet.anchoredPosition=Vector2.zero;
            var heading=Label(sheet,font,"传送阵",12,Secondary);Band(heading.rectTransform,16,16,12,20);
            Move(modal.title_text.transform,sheet);Style(modal.title_text,19,Ink,TextAnchor.MiddleLeft,true,13);
            Band(modal.title_text.rectTransform,16,16,35,33);
            var line=RoleFolio.Image(sheet,"费用分隔",Rule);Band(line.rectTransform,16,16,77,1);
            Move(modal.JuLi_text.transform,sheet);modal.JuLi_text.text="传送费用";Style(modal.JuLi_text,14,Secondary,TextAnchor.MiddleLeft);
            Band(modal.JuLi_text.rectTransform,16,160,86,26);
            Move(modal.LingShi_text.transform,sheet);Style(modal.LingShi_text,17,Ink,TextAnchor.MiddleRight,true,12);
            Band(modal.LingShi_text.rectTransform,85,16,84,30);
            Move(modal.ShenYu_text.transform,sheet);Style(modal.ShenYu_text,14,Secondary,TextAnchor.MiddleRight,false,11);
            Band(modal.ShenYu_text.rectTransform,16,16,117,26);
            TransferAction(modal.Clsoe_btn,sheet,font,"返回舆图",false);
            TransferAction(modal.ChuanSong_btn,sheet,font,"确认传送",true);
        }

        private static void Move(Transform item, Transform target)
        {
            item.SetParent(target,false);item.gameObject.SetActive(true);
        }

        private static void TransferAction(Button button, RectTransform sheet, Font font, string caption, bool primary)
        {
            Move(button.transform,sheet);
            foreach(Transform child in button.transform)child.gameObject.SetActive(false);
            var rect=(RectTransform)button.transform;
            rect.anchorMin=new Vector2(primary?.5f:0,0);rect.anchorMax=new Vector2(primary?1:.5f,0);
            rect.offsetMin=new Vector2(primary?4:14,13);rect.offsetMax=new Vector2(primary?-14:-4,57);
            if (!button.TryGetComponent(out Image background)) background = button.gameObject.AddComponent<Image>();
            Flat(background,primary?Jade:Card,true);button.targetGraphic=background;button.transition=Selectable.Transition.ColorTint;
            var text=Label(rect,font,caption,15,primary?Paper:Ink,TextAnchor.MiddleCenter,true);Fill(text.rectTransform);
        }
    }
}
