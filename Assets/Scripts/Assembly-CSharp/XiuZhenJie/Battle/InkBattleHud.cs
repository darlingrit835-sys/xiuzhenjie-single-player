using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkBattleHud
	{
		internal static readonly Color Gold = new Color32(232, 199, 136, 255);
		private static readonly Color Mist = new Color32(199, 221, 205, 255);
		private static readonly Color Guard = new Color32(195, 166, 101, 255);
		private static readonly Color Blood = new Color32(219, 105, 85, 255);
		private static readonly Vector2 NW = new Vector2(0f, 1f);
		private static readonly Vector2 SE = new Vector2(1f, 0f);
		private readonly BattlePanel panel;
		private readonly RectTransform safe;
		private readonly RectTransform window;
		private readonly Text interactCaption;
		internal readonly Button Interact;
		private readonly RectTransform header, vitals, location, targetCard, skillDock;
		private readonly Text explored, targetName, emptyGroup, movement;
		private double shownSpeed = -1;
		private int shownMethod = -1, shownSigil = -1;
		private readonly Image targetHealth, targetPortrait;
		internal readonly Text Group;
		private Rect lastSafe, lastWindow;
		private int shownExplored = int.MinValue;
		private EntityBase shownTarget;
		private float lastTargetHealth = -1f;

		internal InkBattleHud(BattlePanel panel)
		{
			this.panel = panel;
			window = (RectTransform)panel.transform;

			safe = Make<RectTransform>(window, "FieldHudSafeArea");
			safe.SetSiblingIndex(panel.MapNameText.transform.parent.GetSiblingIndex());
			Font font = panel.MapNameText.font;

			header = Adopt(panel.MapNameText.transform.parent);
			Plate(header);
			Image emblem = Make<Image>(header, "Compass");
			InkInterfaceArt.Set(emblem, "Compass");
			emblem.raycastTarget = false;
			Place(emblem.rectTransform, NW, 10,-10,30,30);
			Tint(panel.MapNameText, 20, InkDialogArt.Ivory);
			Band(panel.MapNameText.rectTransform, 48,8,4,26);

			location = Adopt(panel.PosText.transform.parent);
			location.SetParent(header, false);
			location.GetComponent<Image>().enabled = false;
			location.Find("Image").gameObject.SetActive(false);
			Band(location, 48,6,30,17);
			InkDialogArt.Inset(panel.PosText.rectTransform,0,0,0,0);
			Tint(panel.PosText, 12, Mist);

			Adopt(panel.SmallMapTran);
			Plate(panel.SmallMapTran);
			explored = Label(safe, font, "Exploration", "", 12, Gold);
			explored.alignment = TextAnchor.MiddleCenter;
			var cartouche = Make<Image>(safe,"ExplorationLabel");
			Plate(cartouche.rectTransform);
			explored.transform.SetParent(cartouche.transform,false);
			InkDialogArt.Inset(explored.rectTransform,1,1,0,0);

			PrepareExit(panel.LiKaiBtn, "撤离");
			PrepareExit(panel.LiKaiKillBossBtn, "归城");
			panel.OverTimeText.transform.SetParent(header,false);
			panel.MonsterDieProgressText.transform.SetParent(header,false);
			Band(panel.OverTimeText.rectTransform,10,80,49,18);
			Tint(panel.OverTimeText,12,Gold);
			Place(panel.MonsterDieProgressText.rectTransform, Vector2.one, -8,-49,85,18);
			Tint(panel.MonsterDieProgressText,12,Mist);
			panel.MonsterDieProgressText.alignment = TextAnchor.MiddleRight;
			panel.MonsterDieProgressText.text = "清敌  0%";
			movement = Label(header, font, "MovementSpeed", "", 12, Mist);
			Band(movement.rectTransform,10,6,67,17);
			panel.OverTimeText_AfterKillBoss.gameObject.SetActive(false);
			panel.MonsterDieProgressBossText.gameObject.SetActive(false);

			targetCard = Make<Image>(safe,"Target").rectTransform;
			Plate(targetCard);
			targetPortrait = Make<Image>(targetCard,"Portrait");
			targetPortrait.raycastTarget = false;
			targetPortrait.preserveAspect = true;
			Place(targetPortrait.rectTransform,NW,3,-2,27,27);
			targetName = Label(targetCard,font,"TargetName","",13,InkDialogArt.Ivory);
			Band(targetName.rectTransform,9,6,2,19);
			Image rail = Make<Image>(targetCard,"HealthTrack");
			rail.color = new Color32(67,81,69,255);
			rail.raycastTarget = false;
			Band(rail.rectTransform,9,9,23,3);
			targetHealth = Make<Image>(rail.transform,"Health");
			targetHealth.raycastTarget = false;
			targetHealth.color = Blood;
			InkDialogArt.Inset(targetHealth.rectTransform,0,0,0,0);
			targetCard.gameObject.SetActive(false);

			vitals = Adopt(panel.PlayerTouXiangImg.transform.parent.parent);
			Plate(vitals);
			Place((RectTransform)panel.PlayerTouXiangImg.transform.parent,new Vector2(0f,.5f),9,0,46,46);
			var affinity = (RectTransform)panel.TiFiveElementIcon.transform.parent;
			Place(affinity,new Vector2(0f,.5f),58,0,14,51);
			ArrangeVital("FangHuTitle",panel.FangHuValueText,panel.FangHuValueImg, Guard,8);
			ArrangeVital("ZhenYuanTitle",panel.ZhenYuanValueText,panel.ZhenYuanValueImg,new Color32(125,198,193,255),24);
			ArrangeVital("ShenShiTitle",panel.ShenShiValueText,panel.ShenShiValueImg,new Color32(153,194,144,255),40);
			panel.BloodValueImg.color = Blood;
			// A thin blood gauge remains visible even when guard is full.
			InkDialogArt.Inset(panel.BloodValueImg.rectTransform,0,0,3,0);
			InkDialogArt.Inset(panel.FangHuValueImg.rectTransform,0,0,0,2);
			var buff = (RectTransform)panel.Buff_Content.parent.parent;
			buff.SetParent(safe,false);
			Place(buff,Vector2.zero,7,66,136,24);

			Adopt(panel.OpenNaJieBtn.transform);
			InkDialogArt.StyleButton(panel.OpenNaJieBtn,"ParchmentHeading",InkInterfaceArt.Ink);
			Text bag = Label(panel.OpenNaJieBtn.transform,font,"Caption","纳戒",14,InkInterfaceArt.Ink);
			bag.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(bag.rectTransform,2,2,0,0);

			Interact = Make<Image>(safe,"FieldInteract").gameObject.AddComponent<Button>();
			Interact.targetGraphic = Interact.GetComponent<Image>();
			InkDialogArt.StyleButton(Interact,"JadeButton",InkDialogArt.Ivory);
			interactCaption = Label(Interact.transform,font,"Action","",14,InkBattleHud.Gold);
			interactCaption.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(interactCaption.rectTransform,5,5,1,1);
			Interact.gameObject.SetActive(false);
			skillDock = Adopt(panel.SkillGroupContnet);
			skillDock.GetComponent<Image>().raycastTarget = false;
			// Keep waiting slots active so their cooldowns continue while another group is selected.
			if (!panel.SkillGroupWatingContnet.TryGetComponent(out CanvasGroup waiting))
				waiting = panel.SkillGroupWatingContnet.gameObject.AddComponent<CanvasGroup>();
			waiting.alpha = 0;
			waiting.blocksRaycasts = false;
			waiting.interactable = false;
			if (skillDock.TryGetComponent(out LayoutGroup originalLayout)) originalLayout.enabled = false;
			RectTransform rowRect = Make<RectTransform>(skillDock,"ActionRow");
			InkDialogArt.Inset(rowRect,0,0,0,0);
			panel.SkillGroupContnet = rowRect;
			var row = rowRect.gameObject.AddComponent<HorizontalLayoutGroup>();
			row.childAlignment = TextAnchor.LowerRight;
			row.childForceExpandWidth = row.childForceExpandHeight = false;
			row.childControlWidth = row.childControlHeight = false;
			row.spacing = 3;
			emptyGroup = Label(safe,font,"EmptyGroup","此组未配置招式",12,Mist);
			emptyGroup.alignment = TextAnchor.MiddleCenter;
			Adopt(panel.ChangeSkillGroupBtn.transform);
			InkDialogArt.StyleButton(panel.ChangeSkillGroupBtn,"JadeButton",InkDialogArt.Ivory);
			panel.ChangeSkillGroupBtn.transform.Find("ChangeSkillGroup").gameObject.SetActive(false);
			Group = Label(panel.ChangeSkillGroupBtn.transform,font,"Group","",13,InkDialogArt.Ivory);
			Group.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(Group.rectTransform,4,4,0,0);
			StyleCards(panel);
			panel.ContinueClearYaoSou.Find("Image/Text").GetComponent<Text>().text = "此地敌患已清";
			panel.ContinueClearYaoSou.Find("BottomImg/ContinuieClearBtn/Text").GetComponent<Text>().text = "继续探索";
			RefreshLayout();
			BattleHudReveal.Attach(header);
			BattleHudReveal.Attach(vitals, .09f);
		}

		internal void RefreshLayout()
		{
			Rect area = Screen.safeArea;
			if (area == lastSafe && window.rect == lastWindow) return;
			lastSafe = area;
			lastWindow = window.rect;
			// The battle window fills the screen. Normalized insets also follow changes in the canvas render target.
			float sx = Mathf.Max(1, Screen.width), sy = Mathf.Max(1, Screen.height);
			safe.anchorMin = new Vector2(Mathf.Clamp01(area.xMin / sx), Mathf.Clamp01(area.yMin / sy));
			safe.anchorMax = new Vector2(Mathf.Clamp01(area.xMax / sx), Mathf.Clamp01(area.yMax / sy));
			safe.offsetMin = safe.offsetMax = Vector2.zero;
			float width = safe.rect.width;
			float mapSize = width < 330 ? 76 : 88;
			float titleWidth = Mathf.Min(260, width - mapSize - 22);
			Place(header, NW, 8,-34,titleWidth,90);
			Place(panel.SmallMapTran, Vector2.one,-8,-9,mapSize,mapSize);
			Place((RectTransform)explored.transform.parent,Vector2.one,-8,-10-mapSize,mapSize,19);
			Place((RectTransform)panel.LiKaiBtn.transform,NW,8,-130,53,32);
			Place((RectTransform)panel.LiKaiKillBossBtn.transform,NW,8,-130,53,32);
			Place(targetCard,NW,69,-130,titleWidth-61,32);
			vitals.pivot = new Vector2(.5f,0);
			vitals.anchorMin = Vector2.zero;
			vitals.anchorMax = new Vector2(1,0);
			vitals.offsetMin = new Vector2(6,5);
			vitals.offsetMax = new Vector2(-6,64);
			Place((RectTransform)panel.OpenNaJieBtn.transform,Vector2.zero,8,93,59,35);
			Place(skillDock,SE,-8,70,180,77);
			Place((RectTransform)Interact.transform,Vector2.zero,8,145,Mathf.Min(142f,width-125f),34);
			Place(emptyGroup.rectTransform,SE,-8,84,138,39);
			Place((RectTransform)panel.ChangeSkillGroupBtn.transform,SE,-8,151,107,32);
			// The dynamic joystick uses root-canvas coordinates and keeps its original parent.
			InkDialogArt.Inset((RectTransform)panel.EtcAreaImg.transform,0,0,(1-safe.anchorMax.y)*window.rect.height + 100,safe.anchorMin.y*window.rect.height + 65);
		}

		private Text discoveryLabel;
		private CanvasGroup discoveryGroup;
		internal void ShowDiscovery(string title, float alpha)
		{
			if (discoveryLabel == null)
			{
				if (title == null) return;
				discoveryLabel = Label(safe, targetName.font, "DiscoveredLandmark", title, 23, new Color32(255,236,177,255));
				discoveryLabel.alignment = TextAnchor.MiddleCenter;
				discoveryLabel.fontStyle = FontStyle.Bold;
				Place(discoveryLabel.rectTransform, new Vector2(.5f,1f),0,-158,226,38);
				var shadow = discoveryLabel.gameObject.AddComponent<Shadow>();
				shadow.effectColor = new Color(.09f,.14f,.14f,.92f);
				shadow.effectDistance = new Vector2(1.2f,-1.4f);
				discoveryGroup = discoveryLabel.gameObject.AddComponent<CanvasGroup>();
				discoveryGroup.blocksRaycasts = false;
			}
			discoveryLabel.gameObject.SetActive(title != null);
			if (title == null) return;
			if (discoveryLabel.text != title) discoveryLabel.text = title;
			discoveryGroup.alpha = alpha;
		}

		internal void ShowInteraction(string action)
		{
			bool available = !string.IsNullOrEmpty(action);
			if (Interact.gameObject.activeSelf != available) Interact.gameObject.SetActive(available);
			if (available && interactCaption.text != action) interactCaption.text = action;
		}

		internal void ShowGroup(int group, bool empty)
		{
			Group.text = $"招式  {group} / 3   ›";
			emptyGroup.gameObject.SetActive(empty);
		}

		internal string ExplorationSummary => explored.text;

		internal void ShowMovement(BattleLifeModel life)
		{
			double speed = life.CurrentMoveSpeed;
			if (speed == shownSpeed && life.MethodMovePercent == shownMethod && life.SigilMovePercent == shownSigil) return;
			shownSpeed = speed;
			shownMethod = life.MethodMovePercent;
			shownSigil = life.SigilMovePercent;
			movement.text = $"移速 {speed:0.##}/秒" + (shownMethod > 0 ? $"  功法 +{shownMethod}%" : "")
				+ (shownSigil > 0 ? $"  符 +{shownSigil}%" : "");
		}

		internal void ShowExplored(int percent)
		{
			if (shownExplored == percent) return;
			shownExplored = percent;
			explored.text = percent < 0 ? "方位图" : $"已探  {percent}%";
		}

		internal void ShowTarget(EntityBase target, BattleFieldView field)
		{
			if (shownTarget != target)
			{
				shownTarget = target;
				targetCard.gameObject.SetActive(target != null);
				if (target == null) return;
				targetPortrait.sprite = field != null ? field.PortraitFor(target.BattleModel) : null;
				targetPortrait.enabled = targetPortrait.sprite != null;
				int left = targetPortrait.enabled ? 34 : 9;
				Band(targetName.rectTransform,left,6,2,19);
				Band((RectTransform)targetHealth.transform.parent,left,9,23,3);
				targetName.text = "锁定  " + target.BattleModel.Name;
				lastTargetHealth = -1;
			}
			if (target == null) return;
			var model = target.BattleModel;
			float fraction = (float)(((double)model.CurrentBlood + model.CurrentJianRen) /
				System.Math.Max(1d, (double)model.Max_Blood + model.Max_JianRen));
			fraction = Mathf.Clamp01(fraction);
			if (Mathf.Approximately(fraction,lastTargetHealth)) return;
			lastTargetHealth = fraction;
			targetHealth.rectTransform.anchorMax = new Vector2(fraction,1);
		}

		private RectTransform Adopt(Transform node)
		{
			node.SetParent(safe,false);
			return (RectTransform)node;
		}

		private void ArrangeVital(string titleName, Text number, Image fill, Color tint, float top)
		{
			var title = vitals.Find(titleName).GetComponent<Text>();
			Place(title.rectTransform,NW,79,-top + 1,32,15);
			Tint(title,11,Mist);
			RectTransform rail = (RectTransform)fill.transform.parent;
			Band(rail,114,52,top+5,5);
			rail.GetComponent<Image>().color = new Color32(73,92,78,255);
			InkDialogArt.Inset(fill.rectTransform,0,0,0,0);
			fill.color = tint;
			Tint(number,11,InkDialogArt.Ivory);
			number.alignment = TextAnchor.MiddleRight;
			Place(number.rectTransform,Vector2.one,-7,-top+1,42,15);
		}

		private void PrepareExit(Button button, string caption)
		{
			Adopt(button.transform);
			InkDialogArt.StyleButton(button,"ParchmentHeading",InkInterfaceArt.Ink);
			var label = button.transform.Find("Text").GetComponent<Text>();
			Tint(label,14,InkInterfaceArt.Ink);
			label.text = caption;
			label.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(label.rectTransform,0,0,0,0);
		}

		private static void StyleCards(BattlePanel p)
		{
			InkInterfaceArt.Set(p.NaJiePanel.GetComponent<Image>(),"InkLedgerPaper",false);
			InkInterfaceArt.Set(p.NaJieScrollView.GetComponent<Image>(),"LightPaper",false);
			InkDialogArt.StyleButton(p.CloseNaJieBtn,"JadeButton",InkDialogArt.Ivory);
			Place((RectTransform)p.CloseNaJieBtn.transform,new Vector2(1,.5f),-6,0,56,31);
			foreach (RectTransform card in new[] {p.ChuangTaSuccess,p.ChuangTaFail,p.ContinueClearYaoSou})
			{
				InkDialogArt.SetPaper(card.GetComponent<Image>(),"InkLedgerPaper");
				InkInterfaceArt.Set(card.Find("Image").GetComponent<Image>(),"ParchmentHeading",false);
				card.Find("Image/Text").GetComponent<Text>().color = InkInterfaceArt.Ink;
				card.Find("BottomImg").GetComponent<Image>().enabled = false;
				foreach (var button in card.GetComponentsInChildren<Button>(true))
					InkDialogArt.StyleButton(button,"JadeButton",InkDialogArt.Ivory);
				BattleHudReveal.Attach(card);
			}
		}

		internal static void Place(RectTransform rect, Vector2 anchor, float x,float y,float w,float h)
		{
			rect.anchorMin = rect.anchorMax = rect.pivot = anchor;
			rect.sizeDelta = new Vector2(w,h);
			rect.anchoredPosition = new Vector2(x,y);
		}

		internal static void Band(RectTransform rect,float left,float right,float top,float height)
		{
			rect.anchorMin = NW;
			rect.anchorMax = Vector2.one;
			rect.offsetMin = new Vector2(left,-top-height);
			rect.offsetMax = new Vector2(-right,-top);
		}

		internal static void Tint(Text label,int size,Color color)
		{
			label.fontSize = size;
			label.resizeTextMinSize = Mathf.Min(size,10);
			label.resizeTextMaxSize = size;
			label.resizeTextForBestFit = true;
			label.alignment = TextAnchor.MiddleLeft;
			label.color = color;
			label.raycastTarget = false;
		}

		internal static Text Label(Transform parent,Font font,string name,string contents,int size,Color color)
		{
			Text label = Make<Text>(parent,name);
			label.font = font;
			label.text = contents;
			Tint(label,size,color);
			return label;
		}

		private static void Plate(RectTransform rect)
		{
			Image paper = rect.GetComponent<Image>();
			InkInterfaceArt.Set(paper,"JadeButton",false);
			paper.color = new Color32(215,237,224,245);
			paper.raycastTarget = false;
		}

		internal static T Make<T>(Transform parent,string name) where T : Component
		{
			var obj = new GameObject(name,typeof(RectTransform));
			obj.layer = parent.gameObject.layer;
			obj.transform.SetParent(parent,false);
			return obj.TryGetComponent(out T value) ? value : obj.AddComponent<T>();
		}
	}
}
