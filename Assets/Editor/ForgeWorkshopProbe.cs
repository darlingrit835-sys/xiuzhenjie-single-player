using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using XiuZhenJie;

// Batch-mode Play Mode check. The caller supplies a separate project copy, role
// and loopback server. Never run this against a player account.
[InitializeOnLoad]
public static class ForgeWorkshopProbe
{
    [Serializable] private class Fixture { public string phone, password, roleId, workName; public int accountPort, gamePort; }
    private const string Active = "ForgeWorkshopProbe.Active";
    private const string Result = "ForgeWorkshopProbe.Result";
    private static Fixture fixture;
    private static int stage;
    private static double deadline, entered, lastDialog;
    private static bool relocating, capturedShape;
    private const int BatchSize = 8;
    private static string templateId;
    private static int supplyCount, batchQuantity = 1;

    private static void Require(bool ok, string message)
    {
        if (!ok) throw new InvalidOperationException(message);
    }
    private static RenderTexture captureTarget;
    private static void Capture(string name)
    {
        var camera = Singleton<UIManager>.Instance.UICamera;
        if (captureTarget == null)
        {
            captureTarget = new RenderTexture(540, 1200, 24);
            camera.targetTexture = captureTarget;
            foreach (var canvas in UnityEngine.Object.FindObjectsOfType<Canvas>().Where(c => c.isRootCanvas))
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
            }
        }
        Canvas.ForceUpdateCanvases();
        camera.Render();
        var previous = RenderTexture.active;
        RenderTexture.active = captureTarget;
        var image = new Texture2D(captureTarget.width,captureTarget.height,TextureFormat.RGB24,false);
        image.ReadPixels(new Rect(0,0,captureTarget.width,captureTarget.height),0,0);
        image.Apply();
        File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(Application.dataPath),name+".png"),image.EncodeToPNG());
        RenderTexture.active = previous;
        UnityEngine.Object.Destroy(image);
    }

    static ForgeWorkshopProbe()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (!SessionState.GetBool(Active, false)) return;
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                fixture = ReadFixture();
                Constants.LoginServerIP = "127.0.0.1"; Constants.LoginServerPort = fixture.accountPort;
                Constants.UseEmailRegistration = false;
                Next(0, 80); EditorApplication.update += Tick;
            }
            if (state == PlayModeStateChange.EnteredEditMode && SessionState.GetInt(Result, -1) >= 0)
            {
                SessionState.EraseBool(Active);
                EditorApplication.Exit(SessionState.GetInt(Result, 1));
            }
        };
    }

    private static Fixture ReadFixture() => JsonUtility.FromJson<Fixture>(File.ReadAllText(Environment.GetEnvironmentVariable("XZJ_FORGE_QA_FIXTURE")));
    public static void Run()
    {
        if (!Application.isBatchMode || !(Application.dataPath.Contains("/.qa-forge.") || Application.dataPath.Contains("/.qa-workshop-ring.")) || ReadFixture().accountPort <= 0)
            throw new InvalidOperationException("Use an isolated .qa-forge.* project and loopback fixture.");
        SessionState.SetBool(Active, true); SessionState.SetInt(Result, -1);
        EditorSceneManager.OpenScene("Assets/Scenes/StartLoadScene.unity");
        EditorApplication.EnterPlaymode();
    }

    private static T Window<T>(string name) where T : Window =>
        Singleton<UIManager>.Instance.FindWndByName<T>(name + ".prefab") is T w && w.GameObject != null && w.GameObject.activeInHierarchy ? w : null;
    private static T Panel<T>(string name) where T : Component => Window<Window>(name)?.GameObject.GetComponent<T>();
    private static bool Ready(Selectable value) => value != null && value.gameObject.activeInHierarchy && value.IsInteractable();
    private static bool Click(Button value)
    {
        if (!Ready(value)) return false;
        if (captureTarget != null && value.GetComponentsInParent<Transform>().Any(t => t.name == "ForgeWorkshop"))
        {
            Canvas.ForceUpdateCanvases();
            var rect=(RectTransform)value.transform;
            var pointer=new PointerEventData(EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(Singleton<UIManager>.Instance.UICamera,rect.TransformPoint(rect.rect.center))};
            var hits=new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer,hits);
            if(hits.Count==0 || hits[0].gameObject.GetComponentInParent<Button>()!=value) return false;
            pointer.pointerPressRaycast=hits[0];
            ExecuteEvents.Execute(value.gameObject,pointer,ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(value.gameObject,pointer,ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(value.gameObject,pointer,ExecuteEvents.pointerClickHandler);
            return true;
        }
        value.onClick.Invoke(); return true;
    }
    private static bool CenterOn(ScrollRect scroll, RectTransform item)
    {
        Canvas.ForceUpdateCanvases();
        float range = scroll.content.rect.height - scroll.viewport.rect.height;
        if (range < 1) return false;
        float y = -scroll.content.InverseTransformPoint(item.TransformPoint(item.rect.center)).y;
        float target = 1f - Mathf.Clamp01((y - scroll.viewport.rect.height / 2) / range);
        if (Mathf.Abs(scroll.verticalNormalizedPosition - target) < .005f) return false;
        scroll.verticalNormalizedPosition = target;
        return true;
    }

    private static void Next(int value, int seconds = 30)
    {
        stage = value; entered = EditorApplication.timeSinceStartup; deadline = entered + seconds;
        Debug.Log("XZJ_FORGE_UI step " + stage);
    }
    private static void Finish(bool ok, string detail)
    {
        Debug.Log("XZJ_FORGE_UI " + (ok ? "PASS " : "FAIL ") + detail);
        EditorApplication.update -= Tick;
        SessionState.SetInt(Result, ok ? 0 : 1); EditorApplication.ExitPlaymode();
    }

    private static void Tick()
    {
        try
        {
            var notice = Panel<AttentionWndPanel>("AttentionWnd");
            var association = Panel<LianQi_ChuangZaoMianPanel>("LianQi_ChuangZaoMianWnd");
            Transform workshop = association == null ? null : association.transform.Find("ForgeWorkshop");
            Button ButtonAt(string path) => workshop == null ? null : workshop.Find(path)?.GetComponent<Button>();
            string message = workshop == null ? "" : workshop.Find("Status")?.GetComponent<Text>()?.text;
            if (EditorApplication.timeSinceStartup > deadline)
            {
                var roles = Singleton<RoleManager>.Instance;
                var shown = string.Join(",",UnityEngine.Object.FindObjectsOfType<Canvas>().Where(c=>c.gameObject.activeInHierarchy).Select(c=>c.name).Take(12));
                Finish(false, $"timeout step {stage}: {notice?.ContentText?.text} / {message} role={roles.currentRoleId} map={roles.Role_Entity?.MapId} city={Panel<MainCityPanel>("MainCityWnd")!=null} visible={shown}"); return;
            }
            if (notice != null && notice.ContentText.text.Contains("确认放弃离线收益")) { lastDialog = EditorApplication.timeSinceStartup; Click(notice.QueRenBtn); return; }
            var offline = Panel<LiXianShouYiPanel>("LiXianShouYiWnd");
            if (offline != null) { lastDialog = EditorApplication.timeSinceStartup; Click(offline.FangQiBtnClick); return; }
            if (EditorApplication.timeSinceStartup - lastDialog < .75) return;
            switch (stage)
            {
                case 0:
                    var login = Panel<SignInPanel>("SignInWnd");
                    if (!Ready(login?.SignInBtn)) return;
                    login.UserNameFiled.text = fixture.phone; login.PassWordFiled.text = fixture.password;
                    Click(login.SignInBtn); Next(1, 50); return;
                case 1:
                    var choose = Window<ChooseServerWnd>("ChooseServerWnd");
                    if (choose == null || !choose._serverList.Any(s => s.port == fixture.gamePort && s.currentRoleVO != null)) return;
                    if (Click(choose._panel.LastChooseBtn)) Next(2, 65); return;
                case 2:
                    if (Singleton<RoleManager>.Instance.currentRoleId != fixture.roleId) return;
                    var city = Panel<MainCityPanel>("MainCityWnd");
                    if (Click(city?.XuShenJieBtn)) { Next(3); return; }
                    if (city != null && !relocating && Singleton<RoleManager>.Instance.Role_Entity.MapId == "1111")
                    {
                        // This fixture may start at the county node. Move to the actual city through the game service.
                        relocating=true;
                        var net=Singleton<NetManager>.Instance;
                        net.SendMsg(MsgType.MAP_CITYMIGRATE,new MapCityMigrateReq_1007011{MapId="111102"},move=>
                        {
                            if(!MapCityMigrateResp_1007011.Parser.ParseFrom((ByteString)move).IsSuccess) {Finish(false,"fixture travel failed");return;}
                            net.SendMsg(MsgType.MAP_CHILDREN,new ChildrenMapReq_1007003{MapId="111102"},reply=>
                            {
                                var buildings=ChildrenMapResp_1007003.Parser.ParseFrom((ByteString)reply).Object.Select(b=>new MapEntity
                                {Id=b.Id,MapName=b.Name,MapType=b.Type,Depth=b.Level,FiveElement=b.FiveElements,MaxLevel=b.MaxLevel,MinLevel=b.MinLevel,ParentName=b.ParentName}).ToList();
                                Singleton<RoleManager>.Instance.SetNewMapInfo(buildings,"111102","临仙城");
                            });
                        });
                    }
                    return;
                case 3:
                    if (Click(Panel<XuShenJiePanel>("XuShenJieWnd")?.XuShenDianBtn)) Next(4);
                    return;
                case 4:
                    if (Click(Panel<XuShenDianPanel>("XuShenDianWnd")?.LianQI)) Next(5);
                    return;
                case 5:
                    if (association != null && Click(association.GetComponentsInChildren<Button>().FirstOrDefault(b => b.name == "ForgeEntry"))) Next(6);
                    return;
                case 6:
                    if (!Ready(ButtonAt("PatternLibrary"))) return;
                    Capture("首制器胚");
                    if (Click(ButtonAt("PatternLibrary"))) Next(41); return;
                case 41:
                    var allPatterns = workshop.Find("PatternBook/PatternList/List")?.GetComponentsInChildren<Button>();
                    var intricate = allPatterns?.FirstOrDefault(btn => btn.GetComponentInChildren<Text>().text.Contains("惊雷枪"));
                    if (intricate == null) return;
                    if (CenterOn(workshop.Find("PatternBook/PatternList").GetComponent<ScrollRect>(),(RectTransform)intricate.transform)) return;
                    if (Click(intricate)) Next(42); return;
                case 42:
                    if (EditorApplication.timeSinceStartup - entered < .65) return;
                    string patternCopy = workshop.Find("PatternBook/Viewport/PatternDetails").GetComponent<Text>().text;
                    Require(patternCopy.Contains("五难") && patternCopy.Contains("四重雷纹") && patternCopy.Contains("+8"), "High difficulty pattern missing real tradeoffs");
                    Capture("惊雷枪器图");
                    if (Click(ButtonAt("PatternBook/TracePattern"))) Next(43); return;
                case 43:
                    if (workshop.Find("PatternBook") != null) return;
                    if (Click(ButtonAt("Step3"))) Next(44); return;
                case 44:
                    if (Click(ButtonAt("ShapeBlank"))) Next(45); return;
                case 45:
                    if (EditorApplication.timeSinceStartup - entered < .65) return;
                    var hardGuide = workshop.Find("Shaping/Guide")?.GetComponent<Text>();
                    if (hardGuide == null) return;
                    Require(hardGuide.text.Contains("1 / 8 / 1 / 8"), "Intricate pattern did not select its strike rhythm");
                    Capture("惊雷枪砧台");
                    if (Click(ButtonAt("Shaping/Back"))) Next(46); return;
                case 46:
                    if (workshop.Find("Shaping") != null) return;
                    if (Click(ButtonAt("PatternLibrary"))) Next(7); return;
                case 7:
                    var patterns = workshop.Find("PatternBook/PatternList/List")?.GetComponentsInChildren<Button>();
                    var targetPattern = patterns?.FirstOrDefault(btn => btn.GetComponentInChildren<Text>().text.Contains("归潮刀"));
                    if (targetPattern == null) return;
                    if (CenterOn(workshop.Find("PatternBook/PatternList").GetComponent<ScrollRect>(),(RectTransform)targetPattern.transform)) return;
                    if (Click(targetPattern)) Next(53); return;
                case 53:
                    if (EditorApplication.timeSinceStartup - entered < .65) return;
                    var patternDetails = workshop.Find("PatternBook/Viewport/PatternDetails").GetComponent<Text>().text;
                    Require(patternDetails.Contains("器图难度") && patternDetails.Contains("临图首炼"), "Missing pattern challenge");
                    Capture("器图工序");
                    if (Click(ButtonAt("PatternBook/TracePattern"))) Next(8); return;
                case 8:
                    if (workshop.Find("PatternBook") != null) return;
                    var name = workshop.Find("WorkName").GetComponent<InputField>();
                    Require(name.text == "归潮刀","Trace did not fill weapon name");
                    name.text = fixture.workName;
                    if (Click(ButtonAt("StepNext"))) Next(9); return;
                case 9:
                    if (supplyCount < 5)
                    {
                        if (Click(ButtonAt("GetSupplies"))) supplyCount++;
                        return;
                    }
                    if (Click(ButtonAt("StepNext"))) Next(10); return;
                case 10:
                    if (!Ready(ButtonAt("SelectFlame"))) return;
                    Capture("本炉控火");
                    if (Click(ButtonAt("StepNext"))) Next(14); return;
                case 14:
                    if (Click(ButtonAt("ShapeBlank"))) Next(15); return;
                case 15:
                    var hint = workshop.Find("Shaping/StrikeHint")?.GetComponent<Text>();
                    if (hint == null) return;
                    if (hint.text.Contains("四锤定形"))
                    {
                        Require(hint.text.Contains("精模"), "Four centered strikes did not form a fine mold");
                        if (Click(ButtonAt("Shaping/Keep"))) Next(16);
                        return;
                    }
                    var needle = (RectTransform)workshop.Find("Shaping/Gauge/HeatMarker");
                    var target = (RectTransform)workshop.Find("Shaping/Gauge/StrikeWindow");
                    if (hint.text.Contains("第 3 锤") && !capturedShape) { Capture("炼器塑形"); capturedShape = true; }
                    if (Mathf.Abs(needle.anchorMin.x - (target.anchorMin.x+target.anchorMax.x)/2) < .085f) Click(ButtonAt("Shaping/Strike0"));
                    return;
                case 16:
                    if (workshop.Find("Shaping") != null) return;
                    if (Click(ButtonAt("StepNext"))) Next(17); return;
                case 17:
                    Require(!Ready(ButtonAt("Create")), "First furnace bypassed real-input quote");
                    if (Click(ButtonAt("Estimate"))) Next(18); return;
                case 18:
                    var first = workshop.Find("FirstQuote/Viewport/Details")?.GetComponent<Text>();
                    if (first == null) return;
                    Require(first.text.Contains("器图难度") && first.text.Contains("控火") && first.text.Contains("修习与晋阶") &&
                            first.text.Contains("寒纹铁") && first.text.Contains("配方契合"), "Incomplete first estimate: " + first.text);
                    Debug.Log("XZJ_FORGE_UI first quote " + first.text);
                    Capture("首炉试算");
                    if (Click(ButtonAt("FirstQuote/Back"))) Next(19); return;
                case 19:
                    if (workshop.Find("FirstQuote") != null) return;
                    if (Click(ButtonAt("Step2"))) Next(20); return;
                case 20:
                    if (Click(ButtonAt("SelectFlame"))) Next(21); return;
                case 21:
                    var options = workshop.Find("Choices/Options/List")?.GetComponentsInChildren<Button>();
                    if (options == null) return;
                    var otherFire = options.First(o => !o.GetComponentInChildren<Text>().text.Contains("已选"));
                    if (Click(otherFire)) Next(22); return;
                case 22:
                    if (workshop.Find("Choices") != null) return;
                    if (Click(ButtonAt("Step4"))) Next(23); return;
                case 23:
                    Require(!Ready(ButtonAt("Create")), "Fire change left stale first quote");
                    if (Click(ButtonAt("Estimate"))) Next(24); return;
                case 24:
                    if (workshop.Find("FirstQuote/Viewport/Details") == null) return;
                    if (Click(ButtonAt("FirstQuote/Back"))) Next(25); return;
                case 25:
                    if (workshop.Find("FirstQuote") != null) return;
                    if (Click(ButtonAt("Step1"))) Next(26); return;
                case 26:
                    var iron = workshop.Find("Materials/Rows/Material_mat.hanwen_iron");
                    var viewport=workshop.Find("Materials").GetComponent<ScrollRect>();
                    if (CenterOn(viewport,(RectTransform)iron)) return;
                    if (Click(iron.Find("Minus").GetComponent<Button>())) Next(27); return;
                case 27:
                    if (Click(ButtonAt("Step4"))) Next(28); return;
                case 28:
                    Require(!Ready(ButtonAt("Create")), "Input change left stale first quote");
                    if (Click(ButtonAt("Estimate"))) Next(29); return;
                case 29:
                    if (Click(ButtonAt("FirstQuote/Create"))) Next(30); return;
                case 30:
                    var record = workshop.Find("Receipt/Viewport/Details")?.GetComponent<Text>();
                    if (record == null) return;
                    if (record.text.Contains("未成器"))
                    {
                        Require(record.text.Contains("实耗材料") && record.text.Contains("实际返还"), "Missing failure amounts");
                        Debug.Log("XZJ_FORGE_UI failure receipt " + record.text);
                        Capture("败炉复盘");
                        if (Click(ButtonAt("Receipt/CloseReceipt"))) Next(31); return;
                    }
                    Require(record.text.Contains("精模已收入模架") && record.text.Contains("修习与晋阶") &&
                            record.text.Contains("实耗材料"), "Missing first forge receipt");
                    Debug.Log("XZJ_FORGE_UI first receipt " + record.text);
                    Capture("首炉复盘");
                    if (Click(ButtonAt("Receipt/CloseReceipt"))) Next(34); return;
                case 31:
                    if (workshop.Find("Receipt") != null) return;
                    if (Click(ButtonAt("GetSupplies"))) Next(32); return;
                case 32:
                    if (Click(ButtonAt("Step4"))) Next(28); return;
                case 34:
                    if (workshop.Find("Receipt") != null) return;
                    if (Click(ButtonAt("MoldRack"))) Next(35); return;
                case 35:
                    var rackList = workshop.Find("Molds/Viewport/List");
                    if (rackList == null) return;
                    var mold = rackList.GetComponentsInChildren<Button>().FirstOrDefault(btn => btn.GetComponentInChildren<Text>().text.Contains(fixture.workName));
                    if (mold == null) return;
                    Require(mold.GetComponentInChildren<Text>().text.Contains("器图难度"), "Mold difficulty missing");
                    if (CenterOn(workshop.Find("Molds/Viewport").GetComponent<ScrollRect>(), (RectTransform)mold.transform)) return;
                    if (Click(mold)) Next(36); return;
                case 36:
                    if (workshop.Find("Molds") != null) return;
                    if (batchQuantity < BatchSize)
                    {
                        if (Click(ButtonAt("More"))) batchQuantity++;
                        return;
                    }
                    if (Click(ButtonAt("SelectFlame"))) Next(51); return;
                case 51:
                    var beforeBatch = workshop.Find("Choices/Options/List");
                    if (beforeBatch == null) return;
                    if (Click(beforeBatch.GetComponentsInChildren<Button>().First())) Next(52); return;
                case 52:
                    if (workshop.Find("Choices") != null) return;
                    Next(37); return;
                case 37:
                    if (Click(ButtonAt("Estimate"))) Next(38); return;
                case 38:
                    var quote = workshop.Find("CastQuote/Viewport/Details")?.GetComponent<Text>();
                    if (quote == null) return;
                    Require(quote.text.Contains("器图难度") && quote.text.Contains("实耗") && quote.text.Contains("节省") &&
                            quote.text.Contains("本炉控火"), "Incomplete batch budget: " + quote.text);
                    Debug.Log("XZJ_FORGE_UI batch quote " + quote.text);
                    Capture("模具实投验料");
                    if (Click(ButtonAt("CastQuote/Back"))) Next(47); return;
                case 47:
                    if (workshop.Find("CastQuote") != null) return;
                    if (Click(ButtonAt("SelectFlame"))) Next(48); return;
                case 48:
                    var fireList = workshop.Find("Choices/Options/List");
                    if (fireList == null) return;
                    var alternative = fireList.GetComponentsInChildren<Button>().First(o=>!o.GetComponentInChildren<Text>().text.Contains("已选"));
                    if (Click(alternative)) Next(49); return;
                case 49:
                    if (workshop.Find("Choices") != null) return;
                    Require(!Ready(ButtonAt("Create")), "Fire change left stale batch quote");
                    if (Click(ButtonAt("Estimate"))) Next(50); return;
                case 50:
                    var changedQuote = workshop.Find("CastQuote/Viewport/Details")?.GetComponent<Text>();
                    if (changedQuote == null) return;
                    Debug.Log("XZJ_FORGE_UI repriced batch " + changedQuote.text);
                    Require(!changedQuote.text.Contains("节省 0"), "Eight-piece alternative furnace did not save a whole unit");
                    Capture("换火重验");
                    if (Click(ButtonAt("CastQuote/Pour"))) Next(39); return;
                case 39:
                    var summary=workshop.Find("Receipt/Viewport/Details");
                    if(summary==null || !summary.GetComponent<Text>().text.Contains("同模浇铸 ×" + BatchSize)) return;
                    Require(!summary.GetComponent<Text>().text.Contains("节省 0"), "Actual expenditure did not include the quoted saving");
                    Debug.Log("XZJ_FORGE_UI cast receipt " + summary.GetComponent<Text>().text);
                    Capture("批铸实耗");
                    if (Click(ButtonAt("Receipt/CloseReceipt"))) Next(40); return;
                case 40:
                    if (workshop.Find("Receipt") != null) return;
                    if (Click(ButtonAt("Close"))) Next(12); return;
                case 12:
                    if (workshop != null) return;
                    if (association != null) { Click(association.FanHui); return; }
                    var palace = Panel<XuShenDianPanel>("XuShenDianWnd");
                    if (palace != null) { Click(palace.FanHui); return; }
                    var world = Panel<XuShenJiePanel>("XuShenJieWnd");
                    if (world != null) { Click(world.BackBtn); return; }
                    if (Click(Panel<MainTitlePanel>("MainTitleWnd")?.NaJie_btn)) Next(13); return;
                case 13:
                    if (Window<NaJieSpaceWnd>("NaJieSpaceWnd") == null) return;
                    var goods = Singleton<RoleManager>.Instance.RoleBoxDic[10].GoodsList.Where(g=>g.Name==fixture.workName).ToList();
                    if (goods.Select(g=>g.GoodsId).Distinct().Count()<BatchSize+1) return;
                    var item = goods.FirstOrDefault();
                    if (item == null || string.IsNullOrEmpty(item.TemplateId) || string.IsNullOrEmpty(item.GoodsId)) return;
                    templateId = item.TemplateId;
                    Finish(true, "role=" + fixture.roleId + " template=" + templateId + " goods=" + item.GoodsId); return;
            }
        }
        catch (Exception ex) { Finish(false, ex.ToString()); }
    }
}
