using System;
using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    // Reuse the bundled input, dropdown and toggle bindings so IME, focus and the
    // wire fields behave exactly like the other name-entry screens.
    internal sealed class SectCharter
    {
        internal Text status;
        internal Action onPageChanged;
        internal bool IsPreparing => prepBody.gameObject.activeSelf;
        private readonly SheLiZongMenPanel panel;
        private readonly Font font;
        private readonly RectTransform body, prepBody;
        private readonly ScrollRect scroll;
        private readonly Button formTab, prepTab;
        private readonly Text formSummary;
        private float prepY;
        private readonly Text[] inscriptions = new Text[4];
        private readonly string[] tokenNames = { "日曜", "月辉", "星光", "玄铁" };

        internal SectCharter(SheLiZongMenPanel p)
        {
            panel = p; font = p.ZongMenDiDianText.font;
            foreach (Transform child in p.transform) child.gameObject.SetActive(false);
            var root = Box(p.transform,"山门印契"); Fill(root);
            Flat(root.gameObject.AddComponent<Image>(),Paper,true);
            var header = RoleFolio.Image(root,"卷首",Card); Band(header.rectTransform,0,0,0,43);
            var heading = Label(header.transform,font,"开宗立派",20,Ink,TextAnchor.MiddleCenter,true);
            Fill(heading.rectTransform,55,55);
            var back = (RectTransform)p.CloseBtn.transform; back.SetParent(header.transform,false);
            back.gameObject.SetActive(true); Fixed(back,1,-5,3,44,37);
            foreach (Transform child in back) child.gameObject.SetActive(false);
            InkInterfaceArt.Set(p.CloseBtn.image,"Back");

            var tabs = Box(root,"筹建页签"); Band(tabs,9,9,43,39);
            formTab = Tab(tabs,0,"山门宗契",() => DisplaySupplies(false));
            prepTab = Tab(tabs,1,"建宗筹备",() => DisplaySupplies(true));
            var viewport = Box(root,"立宗草契"); Fill(viewport,0,0,83,58);
            Flat(viewport.gameObject.AddComponent<Image>(),Color.clear,true);
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport; scroll.horizontal = false; scroll.movementType = ScrollRect.MovementType.Clamped;
            body = Box(viewport,"宗契"); body.anchorMin = Vector2.up; body.anchorMax = Vector2.one;
            body.pivot = new Vector2(.5f,1); body.sizeDelta = new Vector2(0,513); body.anchoredPosition = Vector2.zero;
            scroll.content = body;
            prepBody=Box(viewport,"筹建簿"); prepBody.anchorMin=Vector2.up; prepBody.anchorMax=Vector2.one;
            prepBody.pivot=new Vector2(.5f,1); prepBody.sizeDelta=Vector2.zero; prepBody.anchoredPosition=Vector2.zero;
            prepBody.gameObject.SetActive(false);
            var vista = RoleFolio.Image(body,"福地",Color.white);
            Band(vista.rectTransform,0,0,0,116);
            vista.rectTransform.gameObject.AddComponent<RectMask2D>();
            var art = RoleFolio.Image(vista.transform,"松麓山境",Color.white);
            art.sprite = ReleasedResources.Load<Sprite>("SectCourt/foothills") ?? InkInterfaceArt.Get("GuildHall");
            if (art.sprite != null)
            {
                var fit = art.gameObject.AddComponent<AspectRatioFitter>();
                fit.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
                fit.aspectRatio = art.sprite.rect.width/art.sprite.rect.height;
            }
            var wash = Box(vista.transform,"山雾"); Fill(wash); wash.gameObject.AddComponent<SectCourtShade>().raycastTarget = false;
            var title = Label(vista.transform,font,"择址立山门",25,Paper,TextAnchor.MiddleLeft,true);
            Band(title.rectTransform,13,10,66,40);

            var path = p.ZongMenDiDianText; path.transform.SetParent(body,false); path.gameObject.SetActive(true);
            Band(path.rectTransform,13,12,120,30); Style(path,15,Jade,TextAnchor.MiddleLeft,true,12);
            // These totals come from the chosen site rather than the selected seal.
            var sites = Box(body,"地势"); Band(sites,13,13,153,26);
            var totals = new[] { p.LingFengShuLiangText,p.LingShiKuangMai,p.LingYuKuangMai };
            for (int i=0; i<totals.Length; i++)
            {
                var total = totals[i]; total.transform.SetParent(sites,false); total.gameObject.SetActive(true);
                Slot(total.rectTransform,i,3); Style(total,14,Secondary,TextAnchor.MiddleCenter,false,11);
            }

            Caption("宗名",182,26);
            Field(p.ZongMenMingChenInput,"3 至 5 个字",210,40,true);
            var suffix = (RectTransform)p.ZongMenLastName.transform;
            suffix.SetParent(body,false); suffix.gameObject.SetActive(true); Fixed(suffix,1,-13,210,76,40);
            InkInterfaceArt.Set(p.ZongMenLastName.image,"ParchmentHeading",false);
            p.ZongMenLastName.captionText.color = Ink;
            Caption("印上简称",256,25);
            Field(p.ZongMenJianChengInput,"1 至 2 个字",282,38,false);
            Caption("掌门印契",329,25);
            var tokens = Box(body,"四印"); Band(tokens,10,10,357,77);
            var group = tokens.gameObject.AddComponent<ToggleGroup>(); group.allowSwitchOff = false;
            var toggles = new[] { p.RiYaoLingPaiTog,p.YueHuiLingPaiTog,p.XingGuangLingPaiTog,p.XuanTieLingPaiTog };
            string[] images = { "Sun","Moon","Star","Iron" };
            for (int i=0; i<4; i++)
            {
                var tog = toggles[i]; var rect = (RectTransform)tog.transform;
                rect.SetParent(tokens,false); tog.gameObject.SetActive(true); Slot(rect,i,4);
                foreach (Transform child in rect) child.gameObject.SetActive(false);
                var bg = tog.GetComponent<Image>() ?? tog.gameObject.AddComponent<Image>();
                Flat(bg,Card,true); tog.targetGraphic = bg;
                var picked = RoleFolio.Image(rect,"印座",new Color32(187,210,184,255)); Fill(picked.rectTransform,2,2,2,2);
                tog.graphic = picked; tog.group = group;
                var emblem = RoleFolio.Image(rect,tokenNames[i],Color.white);
                emblem.sprite = InkInterfaceArt.Get("HudEmblems/" + images[i]); emblem.preserveAspect=true;
                Band(emblem.rectTransform,0,0,3,44);
                inscriptions[i]=Label(rect,font,tokenNames[i],13,Ink,TextAnchor.MiddleCenter,true);
                Band(inscriptions[i].rectTransform,1,1,49,23);
            }
            var help = Label(body,font,"四种门徽均免费，仅作外观。立宗消耗一枚角色绑定的开山令，本郡立山门，四阶晋封属洲，七阶晋封大陆。",13,Secondary);
            Band(help.rectTransform,15,15,438,47);
            status=Label(body,font,"",13,Jade); Band(status.rectTransform,15,15,484,28);
            var action = (RectTransform)p.LiZongBtn.transform; action.SetParent(root,false); action.gameObject.SetActive(true);
            action.anchorMin=Vector2.zero; action.anchorMax=Vector2.right; action.offsetMin=new Vector2(12,7); action.offsetMax=new Vector2(-12,52);
            InkDialogArt.StyleButton(p.LiZongBtn,"JadeButton",Paper);
            var submit = p.LiZongBtn.GetComponentInChildren<Text>(true);
            submit.text="持令立宗"; Fill(submit.rectTransform); Style(submit,17,Paper,TextAnchor.MiddleCenter,true);
            p.ZongMenLastName.transform.SetAsLastSibling();
            formSummary=Label(body,font,"",14,Jade,TextAnchor.MiddleCenter,true);
            Band(formSummary.rectTransform,10,10,513,35);
            body.sizeDelta=new Vector2(0,558);
            DisplaySupplies(false);
        }

        private Button Tab(RectTransform parent,int index,string caption,Action press)
        {
            var surface=RoleFolio.Image(parent,caption,Card,true);
            Slot(surface.rectTransform,index,2);
            var button=surface.gameObject.AddComponent<Button>(); button.targetGraphic=surface;
            button.onClick.AddListener(() => press());
            var text=Label(surface.transform,font,caption,15,Ink,TextAnchor.MiddleCenter,true);
            Fill(text.rectTransform);
            return button;
        }

        internal void DisplaySupplies(bool show)
        {
            body.gameObject.SetActive(!show); prepBody.gameObject.SetActive(show);
            scroll.content=show ? prepBody : body;
            scroll.StopMovement(); scroll.content.anchoredPosition=Vector2.zero;
            InkDialogArt.StyleButton(formTab,show ? "ParchmentHeading" : "JadeButton",show ? Ink : Paper);
            InkDialogArt.StyleButton(prepTab,show ? "JadeButton" : "ParchmentHeading",show ? Paper : Ink);
            onPageChanged?.Invoke();
        }

        internal void ResetSupplies(string overview)
        {
            foreach (Transform child in prepBody)
            { child.gameObject.SetActive(false); UnityEngine.Object.Destroy(child.gameObject); }
            prepY=7; formSummary.text=overview;
        }

        internal void SupplyLine(string name,string detail,string icon,Action press=null,string action=null)
        {
            float height=press==null ? 70 : 106;
            var card=RoleFolio.Image(prepBody,name,Card);
            Band(card.rectTransform,10,10,prepY,height); prepY+=height+6;
            var badge=RoleFolio.Image(card.transform,icon,Color.white);
            badge.sprite=ReleasedResources.Load<Sprite>("SectCourt/icons/"+icon); badge.preserveAspect=true;
            Fixed(badge.rectTransform,0,6,7,40,46);
            var title=Label(card.transform,font,name,16,Jade,TextAnchor.MiddleLeft,true);
            Band(title.rectTransform,52,8,4,24);
            var description=Label(card.transform,font,detail,13,Secondary);
            Band(description.rectTransform,52,8,28,39);
            if (press!=null)
            {
                var surface=RoleFolio.Image(card.transform,action,Color.white,true);
                Band(surface.rectTransform,52,8,67,32);
                var trigger=surface.gameObject.AddComponent<Button>(); trigger.targetGraphic=surface;
                var caption=Label(surface.transform,font,action,14,Paper,TextAnchor.MiddleCenter,true);
                Fill(caption.rectTransform); InkDialogArt.StyleButton(trigger,"JadeButton",Paper);
                trigger.onClick.AddListener(() => press());
            }
            prepBody.sizeDelta=new Vector2(0,prepY);
        }

        private static void Slot(RectTransform rect,int i,int count)
        {
            rect.anchorMin=new Vector2((float)i/count,0); rect.anchorMax=new Vector2((float)(i+1)/count,1);
            rect.offsetMin=new Vector2(2,0); rect.offsetMax=new Vector2(-2,0);
        }

        private void Caption(string name,float top,float height)
        {
            Band(Label(body,font,name,15,Jade,TextAnchor.MiddleLeft,true).rectTransform,14,14,top,height);
        }

        private void Field(InputField box,string hint,float top,float height,bool withSuffix)
        {
            var rect=(RectTransform)box.transform; rect.SetParent(body,false); box.gameObject.SetActive(true);
            Band(rect,13,withSuffix ? 94 : 13,top,height);
            InkInterfaceArt.Set(box.image,"LightPaper",false);
            var placeholder=box.placeholder as Text;
            if (placeholder != null) { placeholder.text=hint; Style(placeholder,14,Secondary,TextAnchor.MiddleLeft); Fill(placeholder.rectTransform,8,8); }
            Style(box.textComponent,16,Ink,TextAnchor.MiddleLeft);
            Fill(box.textComponent.rectTransform,8,8);
            // Unity's built-in limit counts UTF-16 units. Validate Unicode scalars at submit.
            box.characterLimit=withSuffix ? 10 : 4;
        }

        internal void Stamp(string shortName, string suffix, int selected)
        {
            for (int i=0; i<inscriptions.Length;i++)
                inscriptions[i].text = i == selected && !string.IsNullOrEmpty(shortName) ? shortName+suffix : tokenNames[i];
        }
    }
}
