using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal sealed partial class SectCourt : MonoBehaviour
    {
        internal enum Page { Gate, Wave, Mine, Treasure, Formation, Region, Members, Replay, Income }
        private const MsgType ReadCourt = (MsgType)4901030;
        private const MsgType GiveOrder = (MsgType)4901031;
        private static readonly string[] Tactics = { "破阵", "固守", "探踪" };
        private static readonly string[] TacticKeys = { "strike", "ward", "scout" };
        private static readonly string[] TacticIcons = { "tide", "ward", "replay" };
        private static readonly string[] Plans = { "均衡", "采掘", "驻守" };
        private static readonly string[] PlanKeys = { "balanced", "mine", "guard" };
        private static readonly string[] PlanIcons = { "work", "mine", "ward" };
        private Page page;
        private SectCourtState data;
        private int entryGrade, category, choice;
        private bool busy, needsRead;
        private float atReceipt, refreshAfter, nextClock, pollAfter;
        private string entryName, notice;
        private Action leave, peaks, hall, legacyShop;
        private SectCourtState.Report review;
        private Coroutine playing;

        internal static SectCourt Open(Transform host, Font font, string name, int stage,
            Page first, Action leave, Action peaks = null, Action hall = null, Action legacyShop = null)
        {
            var node = Node(host, "SectCourt"); Box(node,0,0,1,1);
            var view = node.gameObject.AddComponent<SectCourt>();
            view.font = font; view.entryName = name; view.entryGrade = stage;
            view.leave = leave ?? (() => Destroy(view.gameObject));
            view.peaks = peaks; view.hall = hall; view.legacyShop = legacyShop;
            view.page = first; view.Fetch();
            return view;
        }

        internal bool Available => data != null;
        internal void Show(Page dest)
        {
            bool returning = !gameObject.activeSelf;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            page = dest; review = null;
            if (!needsRead && !busy) notice = null;
            if (returning && !busy) Fetch(); else Render();
        }

        private void Back()
        {
            if (peaks == null) { leave(); return; }
            if (page == Page.Gate) leave();
            else if (page == Page.Replay) Show(Page.Wave);
            else Show(Page.Gate);
        }

        private string PageName()
        {
            switch (page)
            {
                case Page.Gate: return "宗门";
                case Page.Wave: return "护山兽潮";
                case Page.Mine: return "宗门矿脉";
                case Page.Treasure: return "藏宝阁";
                case Page.Formation: return "护宗大阵";
                case Page.Region: return "宗门驻域";
                case Page.Members: return "门人职册";
                case Page.Income: return "宗门收益";
                default: return "战阵留影";
            }
        }

        private long ServerNow => data == null ? 0 : data.serverTime + (long)(Time.realtimeSinceStartup - atReceipt);
        private static string Date(long seconds) => seconds > 0
            ? DateTimeOffset.FromUnixTimeSeconds(seconds).ToOffset(TimeSpan.FromHours(8)).ToString("MM月dd日 HH:mm") : "—";
        private static string Office(int office) => office > 0 && office <= 15 ? ((ZhangLaoRenMing)office).ToString() : "普通门人";
        private static string WaveName(string kind) => kind == "large" ? "月末大潮" : "周日小潮";
        private static string Tactic(string key) { int i = Array.IndexOf(TacticKeys,key); return i >= 0 ? Tactics[i] : "出阵"; }
        private static string Plan(string key) { int i = Array.IndexOf(PlanKeys,key); return i >= 0 ? Plans[i] : "均衡"; }

        private void Fetch(string retain = null)
        {
            if (busy) return;
            busy = true; notice = retain; Render(true);
            ForgeWire.Send(ReadCourt,null,(ok,json) =>
            {
                if (this == null) return;
                busy = false;
                if (ok && Accept(json)) { notice = retain; needsRead = false; }
                else { needsRead = true; notice = Error(json,"暂时读不到宗务，请稍后刷新。"); }
                refreshAfter = Time.realtimeSinceStartup + 15f;
                Render(true);
            });
        }

        private bool Accept(string json)
        {
            if (string.IsNullOrEmpty(json)) return false;
            try
            {
                var incoming = JsonUtility.FromJson<SectCourtState>(json);
                if (incoming == null || incoming.stage < 1 || incoming.deadline <= 0) return false;
                if (!string.IsNullOrEmpty(incoming.stipendDay)) SyncStones(incoming.walletStones);
                data = incoming; atReceipt = Time.realtimeSinceStartup; pollAfter = atReceipt + 60f;
                return true;
            }
            catch (ArgumentException) { return false; }
        }

        private static string Error(string json, string fallback)
        {
            if (!string.IsNullOrEmpty(json))
            {
                try { return JsonUtility.FromJson<SectCourtState.Failure>(json)?.error ?? fallback; }
                catch (ArgumentException) { }
            }
            return fallback;
        }

        private void Confirm(string action, string value, string question, string result)
        {
            if (!Ready) return;
            long revision = data.revision, guildId = data.guildId;
            MonoSingleton<GameStart>.Instance.PopUpAttention(question,() =>
            {
                if (this == null || !Ready) return;
                if (data.revision != revision || data.guildId != guildId) { Fetch("宗务已更新，请核对后再下令。"); return; }
                Order(action,value,revision,result);
            },null,"宗门令");
        }

        private bool Ready => data != null && !busy && !needsRead && ServerNow < data.deadline;

        private void Order(string action, string value, long revision, string result)
        {
            if (!Ready) return;
            busy = true; notice = "正在呈交宗门令…"; Render(true);
            string request = JsonUtility.ToJson(new SectCourtState.Order { action=action, value=value, revision=revision, guildId=data.guildId });
            ForgeWire.Send(GiveOrder,request,(ok,json) =>
            {
                if (this == null) return;
                busy = false;
                if (!ok || !Accept(json))
                {
                    needsRead = true;
                    Fetch(Error(json,"尚未收到回执，正在核对宗门记录。"));
                    return;
                }
                notice = result; Render(true);
                if (hero != null)
                    InkRitualEffect.Play(hero,action == "fight" ? InkRitualEffect.Mark.Flare : InkRitualEffect.Mark.Resonance,Gold,true);
            });
        }

        private void Update()
        {
            if (data == null || nextClock > Time.realtimeSinceStartup) return;
            nextClock = Time.realtimeSinceStartup + .5f;
            long remaining = Math.Max(0,data.deadline - ServerNow);
            if (countdown != null)
            {
                var t = TimeSpan.FromSeconds(remaining);
                countdown.text = remaining <= 0 ? "兽潮正在结算" : $"距结算  {t.Days}天 {t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
            }
            bool top = transform.parent != null && transform.parent.parent != null &&
                transform.parent.GetSiblingIndex() == transform.parent.parent.childCount - 1;
            bool polling = top && page == Page.Wave && !needsRead && Time.realtimeSinceStartup >= pollAfter;
            bool dayChanged = page == Page.Income && !needsRead &&
                !string.IsNullOrEmpty(data.stipendDay) && (data.serverTime + 28800) / 86400 != (ServerNow + 28800) / 86400;
            bool minePulse = top && (page == Page.Mine || page == Page.Income) && !needsRead && data.serverTime / 900 < ServerNow / 900;
            if (!busy && Time.realtimeSinceStartup >= refreshAfter && (remaining == 0 || polling || minePulse || dayChanged))
                Fetch();
        }

        private void Render(bool keepPosition = false)
        {
            float offset = keepPosition && body != null ? Mathf.Max(0,body.anchoredPosition.y) : 0;
            if (playing != null) { StopCoroutine(playing); playing = null; }
            countdown = null; replayAnchor = null; replayCaption = null;
            Shell();
            if (data == null)
            {
                Hero(Scenery[Grade-1],entryName ?? "宗门",Seals[Grade-1] + "山境",GradeText,175);
                Note(busy ? "宗务簿展开中…" : notice ?? "尚无宗务记录。");
                if (!busy) ActionRow("重读宗务","order",() => Fetch());
                if (peaks != null) ActionRow("进入各峰","halls",peaks);
                if (legacyShop != null) ActionRow("贡献点兑换","merit",legacyShop);
            }
            else
            {
                switch (page)
                {
                    case Page.Gate: Gate(); break;
                    case Page.Wave: Wave(); break;
                    case Page.Mine: Mine(); break;
                    case Page.Treasure: Treasury(); break;
                    case Page.Formation: Formation(); break;
                    case Page.Region: Region(); break;
                    case Page.Members: Members(); break;
                    case Page.Replay: Replay(); break;
                    case Page.Income: Income(); break;
                }
                if (!string.IsNullOrEmpty(notice))
                {
                    // Feedback stays visible even for orders placed below a long inventory list.
                    var badge = Node(pageRoot,"Receipt");
                    Box(badge,0,0,1,0,5,56,5,-84);
                    Paint(badge,Accent);
                    TextAt(badge,notice,13,Ivory,TextAnchor.MiddleCenter);
                    cursor += 28;
                }
            }
            body.sizeDelta = new Vector2(0,cursor + 6);
            float limit = Mathf.Max(0,body.sizeDelta.y - scroll.viewport.rect.height);
            body.anchoredPosition = new Vector2(0,Mathf.Min(offset,limit));
        }

        private void Gate()
        {
            Hero(Scenery[Grade-1],data.guildName,
                $"{data.regionName}　·　{Office(data.office)}",GradeText + TierText,Grade <= 3 ? 177 : Grade <= 6 ? 193 : 210);
            Title(Seals[Grade-1] + "宗务","order");
            if (!string.IsNullOrEmpty(data.stipendDay))
                Tile("宗门收益",$"宗库自动入账　·　今日日俸 {data.stipend} 灵石","stipend",() => Show(Page.Income),data.stipendClaimed ? "已领" : "可领");
            if (Grade >= 7)
                Tile("山河封域",data.regionPath,"region",() => Show(Page.Region),GradeText,true);
            Tile(WaveName(data.waveKind) + " · " + data.foe,Date(data.deadline) + "  统一结算","tide",
                () => Show(Page.Wave),data.joinedElsewhere ? "原宗已出阵" : data.joined ? "已出阵" : "可出阵",true);
            if (Grade < 7)
                Tile("宗门驻域",data.regionPath,"region",() => Show(Page.Region),GradeText);
            if (Grade >= 4)
                Tile("护宗大阵",$"阵阶 {data.formation}  ·  驻守 {data.guards} 人","formation",() => Show(Page.Formation),"布阵");
            Tile("门人职册",$"{data.members} 位门人　·　我的职务  {Office(data.office)}","members",() => Show(Page.Members),"查看");
            if (Grade < 4)
                Tile("护宗大阵",$"阵阶 {data.formation}  ·  驻守 {data.guards} 人","formation",() => Show(Page.Formation),"布阵");
            if (peaks != null) Tile("各峰典务","前往主峰、长老殿及各峰设施","halls",peaks,"各峰图");
            if (hall != null) ActionRow("主峰议事","gate",hall);
        }

        private void Wave()
        {
            Hero("tide",data.foe,Date(data.deadline) + "  结阵",WaveName(data.waveKind),182);
            Title("守山战况","ward");
            var row = Tile($"{data.damage:N0} / {data.target:N0}", "正在核对结算时刻…","tide",null,data.joinedElsewhere ? "原宗留影" : data.joined ? "已留影" : "待出阵",true);
            countdown = row.GetComponentsInChildren<Text>()[1]; nextClock = 0;
            Meter(data.damage,data.target);
            Note("每周日与每月末日 20:00（东八区）结算。重合时只开大潮。门人可各自择时出阵，每轮一次。",50);
            if (!data.joined)
            {
                ChoiceRow(Tactics,TacticIcons,choice,i => { choice=i; Render(true); });
                string detail = choice == 0 ? "直接破阵，借自身战力斩击兽群。" : choice == 1 ? "固守阵眼，借守卫人数及阵阶御敌。" : "循迹伏击，借矿工和灵晶勘测寻找弱点。";
                Note(detail + " 携带对应战备可增加四分之一伤害。",40);
                ActionRow("以" + Tactics[choice] + "出阵",TacticIcons[choice],
                    () => Confirm("fight",TacticKeys[choice],"本轮只能出阵一次。以" + Tactics[choice] + "迎敌，并消耗已备的对应战备？","门人已出阵，留影已入卷。"),Ready);
            }
            Tile("本轮留影", data.joinedElsewhere ? "你已为原宗出阵，本轮仅可回看本宗门人记录。" : "按出阵记录回看门人的攻守招式","replay",() => Show(Page.Replay),"回看");
            Title("战果奖惩","merit");
            Note($"胜守得矿砂 {data.winOre}、兽牙 {data.winFang}，每位参战门人获 {data.winMerit} 功勋。",40);
            Note(data.@protected
                ? $"新宗立足保护中，失守免除损耗；参战获 {data.lossMerit} 功勋。"
                : $"失守预计损矿砂 {data.lossOre}，参战获 {data.lossMerit} 功勋。无人出阵时矿场减产，小潮 1 小时、大潮 2 小时。",52);
            Title("历次潮录","scroll");
            if (data.reports == null || data.reports.Length == 0) Note("第一份潮录将在结算后归档。",32);
            else foreach (var item in data.reports)
            {
                var report = item;
                Tile(WaveName(item.waveKind) + " · " + item.foe,Date(item.at) + "　" + item.participants + "人出阵","scroll",() =>
                { page=Page.Replay; review=report; Render(); },item.victory ? "守成" : "失守");
            }
        }

        private void Mine()
        {
            Hero("mine",data.mineName,$"{data.regionName}　·　{data.miners} 人开采","灵矿脉",193);
            InkArtGlint.Attach(hero.Find("Painting").GetComponent<Image>(),new Color(1,.86f,.51f,.62f),.3f);
            Title("灵脉出产","ore");
            Tile("矿砂",$"每小时产出 {data.oreRate:0.##}  ·  用于升阵、晋阶","ore",null,data.ore.ToString("N0"),true);
            Tile("灵晶",$"每小时产出 {data.crystalRate:0.##}  ·  护阵所需","sigil",null,data.crystal.ToString("N0"));
            Note(data.slowUntil > ServerNow ? "兽潮压境，矿场减产至 " + Date(data.slowUntil) + "，届时自动恢复。" : "矿班按排班自动采掘，离开宗门也会累计入库。",36);
            Title("矿班与守卫","work");
            Tile(Plan(data.plan) + "排班",$"人力 {data.workers}　采掘 {data.miners}　驻守 {data.guards}","work",null,"执行中",true);
            if (data.canAssign)
                ChoiceRow(Plans,PlanIcons,Array.IndexOf(PlanKeys,data.plan),i =>
                {
                    if (Ready && data.plan != PlanKeys[i]) Order("assign",PlanKeys[i],data.revision,"已更换排班。");
                });
            Note(data.canAssign ? "均衡兼顾采守，采掘优先产矿，驻守优先抵御兽潮。切换后自动执行。" : "宗主、内务长老和矿脉长老可调度工班。");
            ActionRow("前往护宗大阵","formation",() => Show(Page.Formation));
        }

        private void Formation()
        {
            Hero(Scenery[Grade-1],"护宗大阵",data.guildName + "　·　" + data.guards + " 人驻守","阵阶 " + data.formation,156);
            Title("阵眼与阵材","formation");
            Tile("矿砂", "升阵耗材","ore",null,$"{data.ore}/{data.formationOre}");
            Tile("灵晶", "升阵耗材","sigil",null,$"{data.crystal}/{data.formationCrystal}");
            bool capped = data.formation >= Grade + 2;
            bool enough = data.ore >= data.formationOre && data.crystal >= data.formationCrystal;
            string reason = capped ? "阵法已达当前上限" : !data.canForge ? "宗主、护宗或阵法长老可升阵" : !enough ? "阵材尚缺，矿班持续采掘中" : "提升一阶阵法";
            ActionRow(reason,"formation",() => Confirm("formation","",$"动用宗库矿砂 {data.formationOre}、灵晶 {data.formationCrystal} 提升护宗阵？","护宗阵已提升。"), Ready && !capped && data.canForge && enough);
            Tile("守阵弟子", "通过矿班排班调整守卫人数","work",() => Show(Page.Mine),data.guards + " 人");
        }

        private void Region()
        {
            Hero(Scenery[Grade-1],data.regionName,data.regionPath,TierText,170);
            Title("驻域册封","region");
            int tier = Mathf.Clamp(data.tier,1,3);
            string[] labels = { "郡域山门", "洲域仙府", "大陆圣地" };
            string[] rules = { "一至三阶 · 宗门根基所在郡", "四至六阶 · 根基所属洲", "七至九阶 · 根基所在大陆" };
            for (int i=0; i<3; i++) Tile(labels[i],rules[i],"region",null,i + 1 == tier ? "已绑定" : "",i+1 == tier);
            Advancement();
        }

        private void Members()
        {
            Hero(Scenery[Grade-1],"门人职册",$"{data.members} 位门人　·　我的职务  {Office(data.office)}","宗籍",149);
            Title("宗门名录","members");
            if (data.roles != null) foreach (var member in data.roles)
            {
                string duty = member.office == 15 ? "统领宗务、晋封、派工与护阵" :
                    member.office == 2 || member.office == 4 ? "调度矿班与守卫，可参与兽潮" :
                    member.office == 5 || member.office == 12 ? "掌护宗阵法，可参与兽潮" : "备战、出阵与领取兽潮功勋";
                Tile(member.name,duty,"members",null,Office(member.office),member.office == 15);
            }
            if (hall != null) ActionRow("往主峰任命与交接","halls",hall);
        }

        private void Treasury()
        {
            Hero("treasury","藏宝阁",$"个人功勋 {data.merit}　·　{data.guildName}","宗藏",167);
            ChoiceRow(new[] {"矿材","兽获","战备"},new[] {"ore","tide","treasury"},category,i =>
            {
                category=i; Render();
                InkRitualEffect.Play(hero,InkRitualEffect.Mark.Resonance,Gold);
            });
            if (category == 0)
            {
                Tile("矿砂","矿班开采或兽潮胜守所得，用于宗门晋封及升阵。","ore",null,data.ore.ToString("N0"),true);
                Tile("灵晶","矿脉自然产出，可镶入护宗阵眼。","sigil",null,data.crystal.ToString("N0"));
                ActionRow("查看矿班与产量","mine",() => Show(Page.Mine));
            }
            else if (category == 1)
            {
                Tile("兽牙","胜守兽潮后入宗库，晋封宗阶时取用。","tide",null,data.fang.ToString("N0"),true);
                ActionRow("查看兽潮战果","replay",() => Show(Page.Wave));
            }
            else
            {
                Note("战备由个人功勋换取。每种最多备一件，使用对应战术出阵时自动消耗。",40);
                if (data.treasures != null) foreach (var item in data.treasures)
                {
                    var treasure = item;
                    string icon = item.id == "tonic" ? "merit" : item.id == "sigil" ? "sigil" : "replay";
                    var row = Tile(item.name,item.description,icon,null,item.owned > 0 ? "已备" : item.cost + " 功勋",item.owned > 0);
                    InkArtGlint.Attach(row.Find(icon).GetComponent<Image>(),new Color(1,.85f,.48f,.70f));
                    if (item.owned == 0)
                    {
                        bool affordable = data.merit >= item.cost;
                        ActionRow(affordable ? "兑换" + item.name : "功勋不足","merit",
                            () => Confirm("redeem",treasure.id,$"消耗 {treasure.cost} 功勋换取{treasure.name}？","战备已记入个人名下。"),affordable && Ready);
                    }
                }
            }
            if (legacyShop != null) ActionRow("贡献点兑换","merit",legacyShop);
        }

        private void Replay()
        {
            var echoes = review == null ? data.echoes : review.echoes;
            string foe = review == null ? data.foe : review.foe;
            Hero("tide",foe,review == null ? "本轮战阵留影" : Date(review.at) + "  已归档","留影卷",181);
            replayAnchor = Node(hero,"Projection"); Box(replayAnchor,.35f,.28f,.7f,.93f);
            if (review != null)
            {
                Title(review.victory ? "守山成功" : "山阵失守","scroll");
                Note($"{review.participants} 人出阵　·　总伤害 {review.damage:N0}/{review.target:N0}\n宗库矿砂 {review.ore:+#;-#;0}，兽牙 {review.fang:+#;-#;0}　·　每位参战者 {review.merit} 功勋",52);
                if (review.slowUntil > 0) Note("空阵减产至 " + Date(review.slowUntil) + "。",30);
            }
            if (echoes == null || echoes.Length == 0) { Note("卷中尚无出阵留影。",38); return; }
            var strip = Line("ScreenCaptions",36);
            Paint(strip,Accent);
            replayCaption = TextAt(strip,"留影 " + echoes.Length + " 段",14,Ivory,TextAnchor.MiddleCenter);
            ActionRow("回放出阵留影","replay",() =>
            {
                if (playing != null) StopCoroutine(playing);
                playing = StartCoroutine(Project(echoes));
            });
            foreach (var echo in echoes)
            {
                int index = Array.IndexOf(TacticKeys,echo.tactic);
                Tile(echo.name,Tactic(echo.tactic) + "　·　" + Date(echo.at),index < 0 ? "tide" : TacticIcons[index],null,"+" + echo.damage);
            }
        }

        private IEnumerator Project(SectCourtState.Echo[] echoes)
        {
            scroll.verticalNormalizedPosition = 1f;
            for (int i=0; i<echoes.Length; ++i)
            {
                var echo = echoes[i];
                replayCaption.text = $"{i+1}/{echoes.Length}　{echo.name} · {Tactic(echo.tactic)}　伤害 {echo.damage}";
                if (illumination != null) illumination.Restart();
                var mark = echo.tactic == "ward" ? InkRitualEffect.Mark.Harmony : echo.tactic == "scout" ? InkRitualEffect.Mark.Swift : InkRitualEffect.Mark.Flare;
                InkRitualEffect.Play(replayAnchor,mark,Gold,true);
                yield return new WaitForSecondsRealtime(1.15f);
            }
            replayCaption.text = "放映完毕　·　共 " + echoes.Length + " 段留影";
            playing = null;
        }

        // Once the player enters the existing peak map, the same folio is available
        // there without recreating its network session or the peak's controls.
        internal static void AddGateButton(Transform host, Font source, Action open)
        {
            var rect = Node(host, "ReturnToSectCourt");
            Box(rect,0,1,0,1,12,-151,-113,109);
            var image = Paint(rect,Color.white,true); InkInterfaceArt.Set(image,"JadeButton",false);
            var button = rect.gameObject.AddComponent<Button>(); button.targetGraphic = image;
            button.onClick.AddListener(() => open());
            var label = Node(rect,"Text").gameObject.AddComponent<Text>();
            Box(label.rectTransform,0,0,1,1,27,0,3,0);
            label.text = "宗务总览"; label.font = source; label.fontSize = 15; label.color = Ivory;
            label.alignment = TextAnchor.MiddleCenter; label.raycastTarget = false;
            var emblem = Node(rect,"gate"); Box(emblem,0,0,0,1,3,7,-29,7);
            var art = Paint(emblem,Color.white);
            art.sprite = ReleasedResources.Load<Sprite>("SectCourt/icons/gate"); art.preserveAspect = true;
        }
    }
}
