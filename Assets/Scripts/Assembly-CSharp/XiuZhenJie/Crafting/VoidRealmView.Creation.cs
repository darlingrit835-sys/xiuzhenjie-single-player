using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class VoidRealmView
    {
        private VoidRealmCreationHall creation;
        private int[] process;
        private int activeStage, mainQi, lastStage;
        private static readonly Color Inlay = new Color32(109,130,106,255);
        private static readonly string[] StepsChinese = {"一","二","三"};

        private static bool UsableCreation(VoidRealmClient.Work w,VoidRealmCreationHall h)
        {
            if (w==null || h==null || h.stages==null || h.stages.Length!=3 || h.briefs==null ||
                w.brief<0 || w.brief>=h.briefs.Length || w.steps==null || w.steps.Length!=4 ||
                w.adjust<1 || w.adjust>VoidRealmCreationCatalog.Polarities.Length ||
                w.route<1 || w.route>VoidRealmCreationCatalog.Elements.Length ||
                w.steps[3]<1 || w.steps[3]>VoidRealmCreationCatalog.Qi.Length) return false;
            for(int i=0;i<3;i++)
                if(h.stages[i]?.options==null || w.steps[i]<0 || w.steps[i]>=h.stages[i].options.Length) return false;
            return true;
        }

        private void InitCreation(VoidRealmClient.Work source)
        {
            process=source==null ? new[]{-1,-1,-1} : new[]{source.steps[0],source.steps[1],source.steps[2]};
            activeStage=0;lastStage=source==null ? -1 : 2;mainQi=source?.steps[3] ?? 1;
            if(source==null) { craftAdjust=3;craftRoute=1; }
        }

        private static string AffinityLabel(int yinYang,int element,int qi)
        {
            return VoidRealmCreationCatalog.Polarities[yinYang-1]+" · "+VoidRealmCreationCatalog.Elements[element-1]+
                " · "+VoidRealmCreationCatalog.Qi[qi-1];
        }

        private void DrawCreationStudio()
        {
            bool study=studySource!=null;
            var brief=creation.briefs[craftBrief];
            StudioPage(study ? "照谱复演" : creation.product+"创作");
            if(study) Copy("原作　"+studySource.author+"《"+studySource.name+"》");
            Heading(brief.name);Copy(brief.hint);
            var target=brief.target;
            if (target!=null && target.Length>0)
                Copy("命题目标　"+target[0]+creation.unit+"　·　"+AffinityLabel(brief.polarity,brief.element,brief.qi));
            Copy(creation.mechanic);
            ProcessPlate(craftHall.id,creation,process,craftGrade,craftRoute,false,study ? null : (Action<int,int>)((stage,pick)=>
            {
                activeStage=stage;ChooseStep(pick);
            }));
            if(study)
            {
                Section("原作工序");
                ShowRecordedSteps(creation,process);
                Copy("所用气性　"+AffinityLabel(craftAdjust,craftRoute,mainQi));
                Copy("照谱复演免费。");
                ActionRow("依谱验证",Simulate,true);
                ActionRow("返回原作",()=>OpenWork(studySource.id));
                return;
            }
            Section("手定工序");
            Choices(creation.stages.Select(stage=>stage.name).ToArray(),activeStage,index=>
            {
                activeStage=index;DrawStudio();
            });
            var stage=creation.stages[activeStage];
            for(int i=0;i<stage.options.Length;i++)
            {
                int pick=i;
                ActionRow(stage.options[i],()=>ChooseStep(pick),process[activeStage]==i);
            }
            Copy(creation.traits,13);
            if(process.Any(value=>value>=0))
                ActionRow("重走三段工序",()=>
                {
                    for(int i=0;i<3;i++)process[i]=-1;
                    activeStage=0;lastStage=-1;ResetDraft();DrawStudio();
                });
            Section("取性定气");
            Copy(creation.materialNote,13);
            Row("阴阳",VoidRealmCreationCatalog.Polarities[craftAdjust-1],"取性",()=>ChooseAffinity(0));
            Row("主行",VoidRealmCreationCatalog.Elements[craftRoute-1],"择行",()=>ChooseAffinity(1));
            Row("主气",VoidRealmCreationCatalog.Qi[mainQi-1],"引气",()=>ChooseAffinity(2));
            Row("论艺题",brief.name,"换题",ChooseCreationBrief);
            Row("作品等阶",VoidRealmCatalog.Stage(craftGrade)+creation.product,"换阶",SelectGrade);
            bool complete=process.All(value=>value>=0);
            if(!complete) Copy("尚待"+string.Join("、",creation.stages.Select((s,i)=>process[i]<0?s.name:null).Where(s=>s!=null))+"。");
            if(craftResult!=null) ActionRow("查看本轮评定",ResultPage,true);
            else ActionRow("开阵推演",complete ? (Action)Simulate : null,true);
            Copy("先推演看报价，确认署名时支付。",12);
            if(craftHall.id!="qiao" && craftHall.id!="shou")
                ActionRow("进入"+craftHall.hall+"原有设施",()=>LegacyHall(craftHall.id));
            ActionRow("返回大厅",ShowHalls);
        }

        private void ChooseStep(int option)
        {
            int updated=activeStage;lastStage=updated;
            process[updated]=option;ResetDraft();
            int next=Array.FindIndex(process,n=>n<0);
            if(next>=0) activeStage=next;
            DrawStudio();
            Notice(creation.stages[updated].name+"　"+creation.stages[updated].options[option]);
        }

        private void ChooseCreationBrief()
        {
            StudioPage("择论艺题");
            for(int i=0;i<creation.briefs.Length;i++)
            {
                int index=i;var task=creation.briefs[i];
                Row(task.name,task.hint,craftBrief==i?"当前":"揭题",()=>
                {
                    craftBrief=index;ResetDraft();DrawStudio();
                });
            }
            ActionRow("返回创作",DrawStudio);
        }

        private void ChooseAffinity(int dimension)
        {
            var entries=dimension==0 ? VoidRealmCreationCatalog.Polarities : dimension==1 ? VoidRealmCreationCatalog.Elements : VoidRealmCreationCatalog.Qi;
            int current=dimension==0 ? craftAdjust : dimension==1 ? craftRoute : mainQi;
            StudioPage(dimension==0 ? "取阴阳" : dimension==1 ? "择五行" : "引十二气");
            for(int start=0;start<entries.Length;start+=3)
            {
                int first=start;var group=entries.Skip(first).Take(3).ToArray();
                var band=Band("气性",46);
                for(int i=0;i<group.Length;i++)
                {
                    int id=first+i+1;var rect=Node(band,group[i]);
                    Box(rect,i/3f,0,(i+1)/3f,1,2,2,2,2);
                    Face(rect,group[i],()=>
                    {
                        if(dimension==0) craftAdjust=id;
                        else if(dimension==1) craftRoute=id;
                        else mainQi=id;
                        ResetDraft();DrawStudio();
                    },current==id);
                }
            }
            ActionRow("返回创作",DrawStudio);
        }

        private void ProcessPlate(string hallId,VoidRealmCreationHall art,int[] steps,int grade,int element,bool reveal,Action<int,int> place=null)
        {
            var bounds=((RectTransform)transform).rect;
            float height=bounds.width>bounds.height ? 118 : 140;
            var plate=Band("工艺印相",height,10,10,7);
            Paint(plate,new Color32(36,56,49,255));
            var edge=Node(plate,"金丝");Box(edge,0,0,1,0,0,0,0,-2);Paint(edge,Gold);
            float width=Mathf.Max(180,bounds.width-20);
            float size=Mathf.Min(height-12,width*.37f);
            var medallion=Node(plate,"职业印相");Box(medallion,0,.5f,0,.5f,4,-size/2,-4-size,-size/2);
            int count=steps.Take(3).Count(i=>i>=0);
            VoidRealmRelic.Mount(medallion,hallId,grade,element,reveal ? 2 : lastStage,reveal,steps.Length>3 ? steps[3] : mainQi);
            var drawing=Node(plate,"工序轨迹");Box(drawing,0,0,1,1,size+9,13,7,15);
            var sites=new Vector2[9];
            // 纵轴依次为三段工法，横向三道落点分别对应同段的三种选择。
            for(int i=0;i<9;i++)sites[i]=VoidRealmWorkshopBoard.Site(hallId,i/3,i%3);
            var trace=steps.Take(3).Select((value,i)=>value>=0 ? i*3+value : -1).Where(index=>index>=0).ToArray();
            var line=drawing.gameObject.AddComponent<VoidRealmWorkshopBoard>();
            line.Set(hallId,sites,trace,Inlay);
            for(int i=0;i<9;i++)
            {
                int stage=i/3, option=i%3;bool chosen=steps[stage]==option;
                var spot=Node(drawing,art.stages[stage].name+" · "+art.stages[stage].options[option]);
                var pt=sites[i];
                Box(spot,pt.x,pt.y,pt.x,pt.y,-17,-15,-17,-15);
                if (place!=null)
                {
                    var hit=Paint(spot,Color.clear,true);
                    var button=spot.gameObject.AddComponent<Button>();button.targetGraphic=hit;
                    button.onClick.AddListener(()=>place(stage,option));
                }
                var token=Node(spot,"落笔");Box(token,.5f,.5f,.5f,.5f,-9,-9,-9,-9);
                Paint(token,chosen ? new Color32(183,150,87,255) : new Color32(64,84,68,255));
                Caption(token,chosen ? StepsChinese[stage] : (option+1).ToString(),11,chosen ? Ink : Ivory,TextAnchor.MiddleCenter,chosen);
            }
            var cap=Caption(plate,reveal ? "成 卷" : "工 序　"+count+" / 3",11,Gold,TextAnchor.UpperRight);
            Box(cap.rectTransform,0,1,1,1,0,-18,7,1);
        }

        private void ShowRecordedSteps(VoidRealmCreationHall h,int[] steps)
        {
            for (int i=0;i<3;i++)
                Copy(StepsChinese[i]+"　"+h.stages[i].name+"　·　"+h.stages[i].options[steps[i]]);
        }

        private void CreationResult(VoidRealmClient.Reply result)
        {
            StudioPage(result.success ? "推演合题" : "草案待修");
            ProcessPlate(craftHall.id,creation,process,craftGrade,craftRoute,result.success);
            Heading((result.title??"创作评定")+"　"+result.score+" 分");
            Copy(result.reason);
            if(result.axes!=null)foreach(var axis in result.axes)Copy(axis.name+"　"+axis.value+" / 目标 "+axis.target);
            Copy(result.affinityText);Copy(result.qualityText);Copy(result.craftText);Copy(result.affixText);
            if(result.effectText!=result.reason) Copy(result.effectText);
            Copy(result.scoreText,12);
            if(studySource!=null)
            {
                if(studySource.own || studySource.authorId==lobby.roleId) Copy("作者自验，不计入传习人数。");
                else if(studySource.studied) Copy("你已为这件作品留过传习回执。");
                else if(result.success) ActionRow("留传习回执",RecordStudy,true);
                ActionRow("返回原作",()=>OpenWork(studySource.id));
            }
            else
            {
                if(result.success)
                {
                    DrawQuote(result);
                    if (CanFund(result)) ActionRow("署名入册",NamePage,true);
                    else
                    {
                        Copy("当前用度未齐，可补足灵石、悟性或材料后重新核价。");
                        ActionRow("重新核价",Simulate,true);
                    }
                }
                else Copy(result.next);
                ActionRow("调整再试",()=>{ ResetDraft();DrawStudio(); },!result.success);
            }
            ActionRow("返回大厅",ShowHalls);
        }

        private static bool CanFund(VoidRealmClient.Result price)
        {
            return price.wallet!=null && price.wallet.stones>=price.stoneCost+price.tuition &&
                price.wallet.insight>=price.insightCost && (price.materials==null || price.materials.All(m=>m.owned>=m.count));
        }

        private void DrawQuote(VoidRealmClient.Result price)
        {
            Section("成卷估值");
            Copy(price.value+" 灵石");Copy(price.valueText,13);
            Section("落印用度");
            string stones="制作用 "+price.stoneCost+" 灵石";
            if(price.tuition>0)stones+=" · 授业 "+price.tuition+" 灵石";
            Copy(stones+" · 悟性 "+price.insightCost);
            if(price.materials!=null)foreach(var mat in price.materials)
                Copy(mat.name+" × "+mat.count+"　·　纳戒可用 "+mat.owned);
            if(price.wallet!=null)Copy("现有灵石 "+price.wallet.stones+" · 悟性 "+price.wallet.insight,13);
        }

        private void DrawArtifact(VoidRealmClient.Work work,VoidRealmCreationHall h)
        {
            var steps=work.steps;
            ProcessPlate(work.hallId,h,steps,work.grade,work.route,true);
            Copy(work.affinityText);Copy(work.qualityText);Copy(work.affixText);Copy(work.effectText);
            Section(h.briefs[work.brief].name);
            ShowRecordedSteps(h,steps);
            Copy(work.craftText);Copy(work.scoreText,12);
            Section("作品估值");
            Copy(work.value+" 灵石");Copy(work.valueText,13);
            Section("成卷实耗");
            Copy("制作 "+work.stoneCost+" 灵石　·　授业 "+work.tuition+" 灵石　·　悟性 "+work.insightCost);
            if(work.materials!=null)foreach(var mat in work.materials)Copy(mat.name+" × "+mat.count);
        }
    }
}
