using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class VoidRealmView
    {
        private VoidRealmHall craftHall;
        private VoidRealmClient.Work studySource;
        private int craftRank, craftCap, craftGrade, craftBrief, craftRoute, craftAdjust;
        private string draftName;
        private VoidRealmClient.Reply craftResult;

        private void OpenWorkshop(string hallId)
        {
            EnterStudio(hallId, null);
        }

        private void EnterStudio(string hallId, VoidRealmClient.Work source)
        {
            var definition = VoidRealmCatalog.Find(hallId);
            if (definition == null) { Notice("当前版本缺少该馆图录。"); return; }
            Request(new VoidRealmClient.Order { action = "enter", hallId = hallId, grade = source?.grade ?? 1 }, reply =>
            {
                if (reply.cap < 1 || reply.rank < 1 || (source != null && source.grade > reply.cap))
                {
                    Notice("当前职业或境界未达到此作品的等阶。");
                    return;
                }
                creation = source == null || source.version == 2 ? VoidRealmCreationCatalog.Find(hallId) : null;
                if ((source == null || source.version == 2) && creation == null)
                {
                    Notice("此馆创作图录尚未载入，请更新后再入馆。");
                    return;
                }
                if (source != null && source.version > 2) { Notice("这部作品需要新版工序图录。"); return; }
                craftHall = definition;
                craftCap = reply.cap;
                craftRank = reply.rank;
                studySource = source;
                craftGrade = source?.grade ?? 1;
                craftBrief = source?.brief ?? 0;
                craftRoute = source?.route ?? 0;
                craftAdjust = source?.adjust ?? 0;
                if (creation != null)
                {
                    if (source != null && !UsableCreation(source, creation)) { Notice("作品的工序手记不全。"); return; }
                    InitCreation(source);
                }
                ResetDraft();
                DrawStudio();
            });
        }

        private void ResetDraft()
        {
            craftResult = null;
            draftName = null;
        }

        private void StudioPage(string title)
        {
            BeginPage(title, craftHall.hall + " · " + VoidRealmCatalog.Rank(craftRank) + craftHall.job,
                true, craftRank, craftHall.id);
        }

        private string Targets(VoidRealmChoice brief)
        {
            string[] degrees = { "低", "中", "高" };
            return craftHall.axes[0] + " " + degrees[brief.targets[0]] + "　·　" +
                   craftHall.axes[1] + " " + degrees[brief.targets[1]];
        }

        private void DrawStudio()
        {
            if (creation != null) { DrawCreationStudio(); return; }
            bool study = studySource != null;
            var brief = craftHall.briefs[craftBrief];
            StudioPage(study ? "照谱复演" : craftHall.work + "推演");
            Heading(brief.name);
            Copy(brief.detail);
            Copy("题目要求　" + Targets(brief));
            if (study)
            {
                Section("原作手记");
                Copy("《" + studySource.name + "》　" + studySource.author + " 题");
                Copy(VoidRealmCatalog.Stage(craftGrade) + craftHall.work + "　" +
                    craftHall.routes[craftRoute].name + " · " + craftHall.adjusts[craftAdjust].name);
            }
            else
            {
                Row("论艺题", brief.name, "换题", SelectBrief);
                Row("作品等阶", VoidRealmCatalog.Stage(craftGrade) + craftHall.work,
                    "上限" + VoidRealmCatalog.Stage(craftCap), SelectGrade);
                Section(craftHall.routeLabel);
                for (int i = 0; i < craftHall.routes.Length; i++)
                {
                    int index = i;
                    var option = craftHall.routes[i];
                    Row(option.name, option.detail, i == craftRoute ? "已选" : "选用", () =>
                    {
                        if (craftRoute == index) return;
                        craftRoute = index;
                        ResetDraft();
                        DrawStudio();
                    });
                }
                Section(craftHall.adjustLabel);
                Row(craftHall.adjusts[craftAdjust].name, craftHall.adjusts[craftAdjust].detail, "调校", SelectAdjustment);
            }
            if (craftResult == null)
            {
                Copy("使用虚拟试材推演。");
                ActionRow(study ? "依谱验证" : "开阵推演", Simulate, true);
            }
            else
            {
                ActionRow("查看本轮评定", ResultPage, true);
            }
            if (!study)
            {
                Section("本馆规约");
                Copy(craftHall.constraint);
                ActionRow("进入" + craftHall.hall + "原有设施", () => LegacyHall(craftHall.id));
            }
            ActionRow(study ? "返回原作" : "返回大厅", () =>
            {
                if (study) OpenWork(studySource.id);
                else ShowHalls();
            });
        }

        private void SelectBrief()
        {
            StudioPage("择论艺题");
            for (int i = 0; i < craftHall.briefs.Length; i++)
            {
                int index = i;
                var brief = craftHall.briefs[i];
                Row(brief.name, brief.detail + "　" + Targets(brief), i == craftBrief ? "当前" : "揭题", () =>
                {
                    craftBrief = index;
                    ResetDraft();
                    DrawStudio();
                });
            }
            ActionRow("返回推演", DrawStudio);
        }

        private void SelectGrade()
        {
            StudioPage("选择作品等阶");
            Copy("作品受本职品阶与修士境界限制，本馆最高可推演" + VoidRealmCatalog.Stage(craftCap) + "。");
            for (int i = 1; i <= craftCap; i++)
            {
                int grade = i;
                ActionRow(VoidRealmCatalog.Stage(grade) + craftHall.work + (i == craftGrade ? "　·　当前" : ""), () =>
                {
                    craftGrade = grade;
                    ResetDraft();
                    DrawStudio();
                });
            }
            ActionRow("返回推演", DrawStudio);
        }

        private void SelectAdjustment()
        {
            StudioPage(craftHall.adjustLabel);
            for (int i = 0; i < craftHall.adjusts.Length; i++)
            {
                int index = i;
                var adjustment = craftHall.adjusts[i];
                Row(adjustment.name, adjustment.detail, i == craftAdjust ? "当前" : "选用", () =>
                {
                    craftAdjust = index;
                    ResetDraft();
                    DrawStudio();
                });
            }
            ActionRow("返回推演", DrawStudio);
        }

        private VoidRealmClient.Order DraftOrder(string action)
        {
            if (creation != null)
            {
                return new VoidRealmClient.Order {
                    action=action, hallId=craftHall.id, grade=craftGrade, version=2, brief=craftBrief,
                    route=craftRoute, adjust=craftAdjust, steps=new[]{process[0],process[1],process[2],mainQi},
                    quoteId=action=="publish" ? craftResult?.quoteId : null
                };
            }
            return new VoidRealmClient.Order {
                action = action, hallId = craftHall.id, grade = craftGrade,
                brief = craftBrief, route = craftRoute, adjust = craftAdjust
            };
        }

        private void Simulate()
        {
            Request(DraftOrder("simulate"), reply =>
            {
                craftResult = reply;
                ResultPage();
                Burst(reply.success, craftRank);
            });
        }

        private void ResultPage()
        {
            var r = craftResult;
            if (creation != null) { CreationResult(r); return; }
            StudioPage(r.success ? "推演合题" : "草案待修");
            Heading((r.title ?? "论艺评定") + "　" + r.score + " 分");
            Copy(craftHall.briefs[craftBrief].name + "　·　" + VoidRealmCatalog.Stage(craftGrade) + craftHall.work);
            if (r.axes != null)
                foreach (var axis in r.axes)
                    Copy(axis.name + "　" + axis.value + " / 目标 " + axis.target);
            Copy(r.reason);
            if (r.success) Copy(r.scoreText, 12);
            if (r.nodes > 0) Copy(r.nodes + (craftHall.id == "dao" ? " 处穴位" : craftHall.id == "shu" ? " 道手印" : " 种灵植") + "　·　" + r.costText);
            if (studySource != null)
            {
                if (studySource.own || studySource.authorId == lobby.roleId) Copy("作者自验，不计入传习人数。");
                else if (studySource.studied) Copy("你已为这件作品留过传习回执。");
                else if (r.success) ActionRow("留传习回执", RecordStudy, true);
                ActionRow("返回原作", () => OpenWork(studySource.id));
            }
            else
            {
                if (r.success) ActionRow("署名入册", NamePage, true);
                else Copy(r.next);
                ActionRow("调整再试", () => { ResetDraft(); DrawStudio(); }, !r.success);
            }
            ActionRow("返回大厅", ShowHalls);
        }

        private void NamePage()
        {
            StudioPage("署名入册");
            Heading(VoidRealmCatalog.Stage(craftGrade) + craftHall.work + "　·　" + craftResult.score + " 分");
            if (creation!=null)
            {
                Copy(creation.briefs[craftBrief].name+"　·　"+craftResult.affinityText);
                DrawQuote(craftResult);
            }
            else Copy(craftHall.briefs[craftBrief].name+"　"+craftHall.routes[craftRoute].name+" · "+craftHall.adjusts[craftAdjust].name);
            Copy("作者　" + lobby.name);
            string inherited = craftHall.id == "fu" ? VoidRealmCatalog.SigilName(craftBrief) : null;
            InputField input = null;
            if (inherited == null)
            {
                input = Field("输入作品名", 10);
                input.text = draftName ?? (creation==null ? "" : craftResult.suggestedName ?? "");
                input.onValueChanged.AddListener(value => draftName = value);
            }
            else Copy("所封法术　" + inherited);
            Copy(VoidRealmCatalog.NameHint(craftHall.id));
            ActionRow("落印入册", () =>
            {
                string name = inherited ?? input.text.Trim();
                if (string.IsNullOrEmpty(name)) { Notice("请先题写作品名。"); return; }
                var order = DraftOrder("publish");
                order.name = name;
                Request(order, reply =>
                {
                    if (reply.work == null) { Notice("未读取到署名作品，请刷新重试。"); return; }
                    ShowWork(reply.work);
                    if (creation!=null && reply.wallet!=null)
                        Copy("落印后余额　灵石 "+reply.wallet.stones+" · 悟性 "+reply.wallet.insight);
                    Burst(true, craftRank);
                });
            }, true);
            ActionRow("返回评定", ResultPage);
        }

        private void OpenWork(string workId)
        {
            Request(new VoidRealmClient.Order { action = "work", workId = workId }, reply =>
            {
                if (reply.work != null) ShowWork(reply.work);
                else Notice("未找到此作品。");
            });
        }

        private void ShowWork(VoidRealmClient.Work w)
        {
            var h = VoidRealmCatalog.Find(w.hallId);
            if (h==null || w.version>2) { Notice("这部作品需要更新的馆藏图录。"); return; }
            var blueprint=w.version==2 ? VoidRealmCreationCatalog.Find(w.hallId) : null;
            if (w.version==2 && !UsableCreation(w,blueprint) || w.version!=2 &&
                (w.brief<0 || w.brief>=h.briefs.Length || w.route<0 || w.route>=h.routes.Length || w.adjust<0 || w.adjust>=h.adjusts.Length))
            {
                Notice("作品的工序手记不全。");
                return;
            }
            BeginPage("署名作品", h.hall + " · " + VoidRealmCatalog.Stage(w.grade) + (blueprint?.product ?? h.work),true,w.grade,h.id,w);
            Heading(w.name);
            Copy(w.author + " 题　·　论艺 " + w.score + " 分　·　" + w.readerCount + " 人传习");
            if (blueprint!=null) DrawArtifact(w,blueprint);
            else
            {
                Section(h.briefs[w.brief].name);
                Copy(h.briefs[w.brief].detail);
                Copy(h.routeLabel + "　" + h.routes[w.route].name);
                Copy(h.adjustLabel + "　" + h.adjusts[w.adjust].name);
            }
            ActionRow("入馆复演", () => EnterStudio(w.hallId, w), true);
            ActionRow("附此卷留言", () => ShowBoard(w.id));
            ActionRow("查看排行榜", ShowRanks);
            ActionRow("返回大厅", ShowHalls);
        }

        private void RecordStudy()
        {
            var workId = studySource.id;
            Request(new VoidRealmClient.Order { action = "study", workId = workId }, reply =>
            {
                if (reply.work == null) { Notice("未读到回执，请重新查阅原作。"); return; }
                ShowWork(reply.work);
                Notice("传习回执已收录。");
            });
        }
    }
}
