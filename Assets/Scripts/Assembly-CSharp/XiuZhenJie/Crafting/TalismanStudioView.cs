using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Finish = XiuZhenJie.TalismanStudioArt.Finish;

namespace XiuZhenJie
{
    internal sealed partial class TalismanStudio
    {
        private static readonly Color32 Background = new Color32(30, 20, 21, 255);
        private static readonly Color32 Recess = new Color32(39, 28, 28, 242);
        private static readonly Color32 Gold = new Color32(250, 214, 143, 255);
        private static readonly Color32 Ivory = new Color32(253, 238, 206, 255);
        private static readonly Color32 Mist = new Color32(221, 198, 167, 255);
        private static Texture2D hall, relics;
        private RectTransform sheet;
        private string legacyTitle;
        private Text writingHint;

        private static RectTransform Area(string id, Transform parent, float left, float right, float top, float bottom, float inset = 0)
        {
            var rect = (RectTransform)new GameObject(id, typeof(RectTransform)).transform;
            rect.SetParent(parent, false); rect.gameObject.layer = parent.gameObject.layer;
            rect.anchorMin = new Vector2(left, 1 - bottom); rect.anchorMax = new Vector2(right, 1 - top);
            rect.offsetMin = new Vector2(inset, inset); rect.offsetMax = new Vector2(-inset, -inset);
            return rect;
        }

        private static Image Paint(RectTransform rect, Color tint, bool hit = false)
        {
            var image = rect.gameObject.AddComponent<Image>(); image.color = tint; image.raycastTarget = hit; return image;
        }

        private Text Label(Transform root, string id, string value, int size, Color color, float l, float r, float t, float b, TextAnchor align = TextAnchor.MiddleLeft)
        {
            var node = Area(id, root, l, r, t, b); var text = node.gameObject.AddComponent<Text>();
            text.font = font; text.fontSize = size; text.text = value; text.color = color; text.alignment = align;
            text.raycastTarget = false; text.resizeTextForBestFit = true; text.resizeTextMaxSize = size;
            text.resizeTextMinSize = Mathf.Max(10, size - 3); return text;
        }

        private static TalismanStudioArt FinishOn(RectTransform rect, Finish finish, bool hit = false)
        {
            var art = rect.gameObject.AddComponent<TalismanStudioArt>(); art.finish = finish; art.raycastTarget = hit; return art;
        }

        private static void Trim(Transform root, string id, Finish finish, float l, float r, float t, float b)
        {
            FinishOn(Area(id, root, l, r, t, b), finish);
        }

        // Clip landscape art at its native aspect; the lighted brush is to the right of the title.
        private static void Hall(Transform root, float bottom, float brightness)
        {
            var crop = Area("StudioScenery", root, 0, 1, 0, bottom);
            crop.gameObject.AddComponent<RectMask2D>();
            var frame = Area("Painting", crop, 0, 1, 0, 1);
            hall = hall ?? ReleasedResources.Load<Texture2D>("Artisan11/SigilHall");
            float aspect = 1.7f;
            if (hall != null)
            {
                var image = frame.gameObject.AddComponent<RawImage>(); image.texture = hall; image.raycastTarget = false;
                image.color = new Color(brightness,brightness,brightness,1f); aspect = (float)hall.width/hall.height;
            }
            else
            {
                var image = Paint(frame,Color.white); InkInterfaceArt.Set(image,"ForgeHall",false);
                image.color = new Color(brightness,brightness*.88f,brightness*.72f,1f);
                if (image.sprite != null) aspect = image.sprite.rect.width/image.sprite.rect.height;
            }
            var fit=frame.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.EnvelopeParent;fit.aspectRatio=aspect;
        }

        private static void Relic(Transform root, int cell, float l, float r, float t, float b)
        {
            var area=Area("Relic",root,l,r,t,b);
            relics = relics ?? ReleasedResources.Load<Texture2D>("Artisan11/SigilRelics");
            if(relics!=null)
            {
                var picture=Area("Object",area,0,1,0,1);var img=picture.gameObject.AddComponent<RawImage>();
                img.texture=relics;img.raycastTarget=false;img.uvRect=new Rect(cell*.25f,0,.25f,1);
                var fit=picture.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.FitInParent;
                fit.aspectRatio=(float)relics.width/(4*relics.height);
            }
            else
            {
                var image=Paint(area,Color.white);var icon = cell==0?"Scroll":cell==1?"Inkstone":cell==2?"Brush":"Talisman";
                InkInterfaceArt.Set(image,"Goods/"+icon,true);
            }
        }

        private Button Key(Transform root, string id, string title, Action action, float l, float r, float t, float b, bool primary = false, bool enabled = true)
        {
            var node = Area(id, root, l, r, t, b, 2); var graphic = FinishOn(node, primary ? Finish.Seal : Finish.Key, true);
            var key = node.gameObject.AddComponent<Button>(); key.targetGraphic = graphic; key.interactable = enabled;
            var colors = key.colors; colors.pressedColor=new Color(1f,.83f,.58f);colors.disabledColor = new Color(.58f,.55f,.51f,.79f); key.colors = colors;
            Label(node, "Caption", title, 15, enabled ? Ivory : Mist, .035f, .965f, .05f, .95f, TextAnchor.MiddleCenter);
            key.onClick.AddListener(() => action()); return key;
        }

        private void Heading(string id,string title,float t,float b,int size=28)
        {
            var label = Label(sheet,id,title,size,Gold,.069f,.93f,t,b);
            label.fontStyle=FontStyle.Bold;var shadow=label.gameObject.AddComponent<Shadow>();
            shadow.effectColor=new Color(.12f,.045f,.01f,.91f);shadow.effectDistance=new Vector2(1.1f,-1.5f);
        }

        private void Frame(bool tabs)
        {
            if (sheet != null) { sheet.gameObject.SetActive(false); Destroy(sheet.gameObject); }
            sheet = Area("Sheet", transform, 0, 1, 0, 1);
            Hall(sheet,1,.38f);Hall(sheet,page==Page.Writing?.263f:.327f,1f);
            Trim(sheet,"TitleShadow",Finish.Shade,0,1,0,page==Page.Writing?.264f:.329f);
            Trim(sheet,"Carving",Finish.Rim,.009f,.991f,.006f,.994f);
            var title=Label(sheet,"Heading",pouchOnly?"随身符匣":"画符静室",15,Ivory,.065f,.49f,.016f,.080f);
            title.fontStyle=FontStyle.Bold;
            if (onLegacy != null) Key(sheet, "Legacy", legacyTitle, () => Close(onLegacy), .555f, .76f, .015f,.080f);
            Key(sheet, "Leave", "返回", () => Close(onClose), .768f,.944f,.015f,.080f);
            if (tabs && !pouchOnly)
            {
                Trim(sheet,"Tray",Finish.Panel,.026f,.974f,.903f,.982f);
                Key(sheet,"Stock","备纸研墨",()=>Navigate(Page.Stock),.038f,.344f,.909f,.975f,page==Page.Stock);
                Key(sheet,"Desk","符案",()=>Navigate(Page.Desk),.348f,.653f,.909f,.975f,page==Page.Desk);
                Key(sheet,"Pouch","成符匣",()=>Navigate(Page.Pouch),.657f,.962f,.909f,.975f,page==Page.Pouch);
            }
        }

        private void PaperRoll(RectTransform rect)
        {
            var texture = Paint(rect, Color.white);InkInterfaceArt.Set(texture,"Paper",false);
            texture.color=new Color(1f,.86f,.63f,1f);
            Trim(rect,"CutGold",Finish.Paper,.009f,.991f,.012f,.988f);
            Trim(rect,"UpperRoller",Finish.Roller,-.036f,1.036f,-.009f,.025f);
            Trim(rect,"LowerRoller",Finish.Roller,-.036f,1.036f,.975f,1.009f);
        }

        private void Waiting(string message, Action retry)
        {
            Frame(false);
            Trim(sheet,"WaitingPlaque",Finish.Panel,.06f,.94f,.35f,.72f);
            Label(sheet, "Message", message, 18, Ivory, .08f, .92f, .39f, .56f, TextAnchor.MiddleCenter);
            if (retry != null) Key(sheet, "Retry", "重新读取", retry, .25f, .75f, .60f, .69f, true);
        }

        private void Render()
        {
            Frame(page != Page.Writing);
            switch (page)
            {
                case Page.Desk: Desk(); break;
                case Page.Stock: Stock(); break;
                case Page.Pouch: Pouch(); break;
                case Page.Writing: Writing(); break;
            }
        }

        private void Desk()
        {
            var p = data.plan;
            var trade = data.rank == 0 ? "符道初学" : $"{data.rank}品符师";
            var rank = $"{data.realm}  ·  {trade}  ·  {data.brush}";
            Heading("Name",p.name,.100f,.190f,30);
            Label(sheet,"Rank",rank + $" · 精力 {data.vigor}",15,Ivory,.072f,.73f,.19f,.244f);
            Trim(sheet,"DeskPanel",Finish.Panel,.029f,.971f,.270f,.884f);
            float width=.88f/data.effects.Length;
            for(int i=0;i<data.effects.Length;i++)
            {
                var choice=data.effects[i];
                Key(sheet,"Effect_"+choice.id,choice.name,()=>Choose(choice.id,seal),.06f+i*width,.06f+(i+1)*width,.286f,.352f,effect==choice.id);
            }
            var art=Area("FormulaPaper",sheet,.080f,.40f,.378f,.630f);PaperRoll(art);
            AddPad(art,.05f,.95f,.075f,.925f).Setup(p.strokes,p.strokes.Length,p.tolerance,null,null);
            Label(sheet,"Effect",p.description,18,Ivory,.452f,.927f,.383f,.506f);
            Label(sheet,"PlanCost",p.cost,15,Gold,.452f,.927f,.513f,.611f);
            float sealWidth=.486f/data.seals.Length;
            for(int i=0;i<data.seals.Length;i++)
            {
                var choice=data.seals[i];
                Key(sheet,"Seal_"+choice.id,choice.name,()=>Choose(effect,choice.id),.445f+i*sealWidth,.445f+(i+1)*sealWidth,.604f,.67f,seal==choice.id);
            }
            var sealChoice=Array.Find(data.seals,x=>x.id==seal);
            Label(sheet,"SealInfo",sealChoice?.description,14,Mist,.075f,.932f,.679f,.734f);
            int ink = seal == "strong" ? 2 : 1;
            int copyCost = (batchSize*ink+2)/3;
            bool hasPaper = Array.Exists(data.materials, m => m.id == "mat.shuanglu_paper" && m.count >= batchSize);
            bool hasInk = Array.Exists(data.materials, m => m.id == "mat.chenglu_ink" && m.count >= batchSize*ink);
            string guidance = p.learned ? $"本式熟练 {p.mastery}/100，{batchSize}纸、{batchSize*ink}墨、{copyCost}精力。" : $"共 {p.strokes.Length} 笔。可以逐笔描写，也可随时用辅笔。";
            if (!p.canDraw) guidance="纸墨尚缺，可先试笔，或前往备纸研墨。";
            Label(sheet,"Guidance",notice??guidance,14,Ivory,.075f,.932f,.739f,.792f);
            if (data.batchLimit > 1)
                Key(sheet,"BatchSize",$"{batchSize}张 ↻",CycleBatch,.056f,.234f,.8f,.867f,false,p.learned);
            Key(sheet,"Copy",$"复写{batchSize}张",()=>Order("copy"),data.batchLimit>1?.238f:.056f,.486f,.8f,.867f,false,hasPaper&&hasInk&&p.learned);
            Key(sheet,"Write",written.Count>0?"继续上张":p.canDraw?"展开符纸":"试笔",()=>Navigate(Page.Writing),.494f,.944f,.8f,.867f,true);
        }

        private RectTransform ScrollRows(string id, float top, float bottom, float height)
        {
            var view = Area(id, sheet, .038f, .962f, top, bottom); Paint(view, Recess, true);
            view.gameObject.AddComponent<RectMask2D>(); var scroll = view.gameObject.AddComponent<ScrollRect>();
            var rows = Area("Content", view, 0, 1, 0, 0); rows.pivot = new Vector2(.5f, 1);
            rows.sizeDelta = new Vector2(0, height); rows.anchoredPosition = Vector2.zero;
            scroll.viewport = view; scroll.content = rows; scroll.horizontal = false; scroll.movementType = ScrollRect.MovementType.Clamped;
            return rows;
        }

        private static RectTransform Row(Transform parent, string id, float y, float height)
        {
            var rect = Area(id, parent, 0, 1, 0, 0); rect.offsetMin = new Vector2(3, -y-height+1); rect.offsetMax = new Vector2(-3,-y-1);
            FinishOn(rect,Finish.Row); return rect;
        }

        private void Stock()
        {
            var recipes = data.recipes ?? Array.Empty<Recipe>(); var materials = data.materials ?? Array.Empty<Material>();
            Heading("BenchTitle","纸墨作坊",.103f,.193f);
            Label(sheet,"Wallet",$"灵石 {data.stones}   ·   {data.brush}",15,Ivory,.072f,.75f,.19f,.245f);
            if (data.batchLimit > 1) Key(sheet,"StockBatch",$"每次{batchSize}份 ↻",CycleBatch,.67f,.945f,.19f,.25f);
            Trim(sheet,"MaterialTray",Finish.Panel,.029f,.971f,.269f,.884f);
            var rows=ScrollRows("StockRows",.286f,.720f,recipes.Length*74+materials.Length*59+117);
            var training=Row(rows,"Training",0,81);
            Label(training,"Progress",$"符师修习 {data.training} / {data.nextRank}",16,Gold,.035f,.96f,.035f,.38f);
            Label(training,"Technique","护身、净秽随境界、符师品阶、本式熟练提高。三品执金翎笔或本式熟练圆满，护田多封一重。",14,Mist,.035f,.96f,.40f,.97f);
            float y=81;
            foreach(var recipe in recipes)
            {
                var row=Row(rows,"Recipe_"+recipe.id,y,74);y+=74;
                int cell=recipe.id=="paper"?0:recipe.id=="brush"||recipe.id=="brushKit"?2:1;
                Relic(row,cell,.022f,.175f,.11f,.89f);
                Label(row,"Name",recipe.name,17,Gold,.19f,.73f,.015f,.38f);
                Label(row,"Cost","每份 "+recipe.cost+" → "+recipe.output,14,Ivory,.19f,.74f,.38f,.96f);
                Key(row,"Make_"+recipe.id,recipe.id=="brush"||recipe.id=="brushKit"?"制作":$"制{batchSize}份",()=>Order("make",recipe.id),.755f,.98f,.20f,.80f,false,recipe.available&&(recipe.id=="brush"||recipe.id=="brushKit"||batchSize==1||recipe.maxSets>=batchSize));
            }
            Label(Row(rows,"Sources",y,36),"Heading","原料来处",17,Gold,.035f,.96f,0,1);y+=36;
            foreach(var mat in materials)
            {
                var row=Row(rows,"Material_"+mat.id,y,59);y+=59;
                Label(row,"Quantity",$"{mat.name} ×{mat.count}",15,Ivory,.035f,.98f,.025f,.46f);
                Label(row,"Source",mat.source,14,Mist,.035f,.98f,.46f,.965f);
            }
            Label(sheet,"Message",notice??("料包内含 "+data.supplyText),14,Mist,.067f,.933f,.725f,.791f);
            int price = data.supplyPrice + (batchSize-1)*36;
            Key(sheet,"Supply",price==0?"领取入门原料":$"购 {batchSize} 包 · {price} 灵石",()=>Order("supply"),.057f,.943f,.8f,.867f,true,data.stones>=price);
        }

        private static TalismanStrokePad AddPad(Transform parent, float left, float right, float top, float bottom)
        {
            return Area("StrokePad", parent, left, right, top, bottom).gameObject.AddComponent<TalismanStrokePad>();
        }

        private void Writing()
        {
            var p = data.plan;
            int index = written.Count;
            bool complete = index == p.strokes.Length;
            Heading("Name",p.name,.085f,.174f,29);
            Label(sheet,"Progress",complete?$"{index} 笔已成":$"第 {index+1} / {p.strokes.Length} 笔　{p.strokes[index].name}",16,Ivory,.073f,.935f,.174f,.235f);
            var mat=Area("WritingPaper",sheet,.110f,.89f,.252f,.702f);PaperRoll(mat);
            AddPad(mat,.04f,.96f,.047f,.953f).Setup(p.strokes,index,p.tolerance,complete?(Action<TalismanStroke>)null:AppendStroke,hint=>writingHint.text=hint);
            Trim(sheet,"WritingRest",Finish.Panel,.030f,.970f,.726f,.975f);
            writingHint=Label(sheet,"Guidance",notice??(complete?(p.canDraw?p.cost+"。封符后成品收入纳戒。":"试笔已成，备齐材料即可封符。"):
                "从朱点起笔，沿金线描至笔尾再抬手。没有时限。"),15,Ivory,.070f,.93f,.739f,.815f);
            Key(sheet,"Undo","退一笔",()=>{written.RemoveAt(written.Count-1);Render();},.055f,.303f,.818f,.889f,false,index>0);
            Key(sheet,"Assist","辅笔 · 描一笔",()=>AppendStroke(p.strokes[written.Count]),.307f,.667f,.818f,.889f,false,!complete);
            Key(sheet,"Sign",p.canDraw?"封符":"去备料",()=>{if(p.canDraw)Order("draw");else Navigate(Page.Stock);},.671f,.945f,.818f,.889f,true,complete);
            Key(sheet,"BackToDesk","收起符纸",()=>Navigate(Page.Desk),.20f,.80f,.896f,.958f);
        }

        private void AppendStroke(TalismanStroke stroke)
        {
            if (page != Page.Writing || busy || written.Count >= data.plan.strokes.Length) return;
            notice = null; written.Add(stroke); Render();
        }

        private void Pouch()
        {
            Heading("PouchTitle","成符匣",.099f,.194f);
            var goods=(data.goods??Array.Empty<Good>()).OrderByDescending(g=>g.id==focusGood).ToArray();
            Label(sheet,"PouchCount",$"已藏成符 {goods.Sum(g=>g.count)} 张",15,Ivory,.072f,.78f,.190f,.246f);
            float end=pouchOnly?.968f:.884f;
            Trim(sheet,"PouchTray",Finish.Panel,.029f,.971f,.268f,end);
            string[] captions={"入战护盾","体内毒性","护田重数","下场轻身"};
            string[] values={data.battleWard.ToString(),data.poison.ToString(),data.ward+" / 3",data.battleHaste+"%"};
            for(int i=0;i<captions.Length;i++)
            {
                float l=.052f+i*.225f;var stat=Area("Status"+i,sheet,l,l+.217f,.287f,.391f);
                FinishOn(stat,Finish.Row);Label(stat,"Value",values[i],22,Gold,.02f,.98f,.025f,.56f,TextAnchor.MiddleCenter);
                Label(stat,"Kind",captions[i],13,Mist,.02f,.98f,.56f,.99f,TextAnchor.MiddleCenter);
            }
            if (goods.Length <= 1)
            {
                // A single talisman gets a display compartment of its own in the open case.
                var shelf=Area("OwnedSigns",sheet,.052f,.948f,.407f,end-.125f);
                var single=goods.FirstOrDefault();
                var cell=Area(single==null?"Empty":"Good_"+single.id,shelf,0,1,0,1);
                FinishOn(cell,Finish.Panel);
                Relic(cell,3,.053f,.46f,.11f,.89f);
                if(single==null) Label(cell,"NoGoods","匣中尚无成符。",17,Mist,.49f,.923f,.14f,.83f);
                else
                {
                    Label(cell,"Name",$"{single.name} ×{single.count}",21,Gold,.483f,.946f,.105f,.37f);
                    Label(cell,"Description",single.description,15,Ivory,.483f,.946f,.37f,.655f);
                    Key(cell,"Use_"+single.id,"启符",()=>Order("use",goodsId:single.id),.48f,.941f,.693f,.893f,true,single.usable);
                }
            }
            else
            {
                var rows=ScrollRows("OwnedSigns",.407f,end-.123f,goods.Length*94);
                float y=0;
                foreach(var good in goods)
                {
                    var row=Row(rows,"Good_"+good.id,y,94);y+=94;
                    Relic(row,3,.018f,.19f,.065f,.93f);
                    Label(row,"Name",$"{good.name} ×{good.count}",17,Gold,.209f,.75f,.035f,.42f);
                    Label(row,"Description",good.description,15,Ivory,.209f,.755f,.42f,.956f);
                    Key(row,"Use_"+good.id,"启符",()=>Order("use",goodsId:good.id),.76f,.98f,.230f,.77f,true,good.usable);
                }
            }
            Label(sheet,"Message",notice,15,Ivory,.065f,.935f,end-.118f,end-.016f);
        }
    }
}
