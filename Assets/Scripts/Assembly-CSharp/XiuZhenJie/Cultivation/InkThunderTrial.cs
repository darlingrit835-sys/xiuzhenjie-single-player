using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	internal sealed class InkThunderTrial : MonoBehaviour
	{
		private const string ViewName = "InkThunderTrial";
		private static readonly Color Ivory = InkDialogArt.Ivory;
		private static readonly Color Gold = new Color32(239, 206, 144, 255);
		private static readonly Color Deep = new Color32(27, 48, 46, 255);
		private static readonly string[] Number = { "一", "二", "三" };
		private readonly Button[] seals = new Button[6];
		private readonly Text[] sealHints = new Text[6];
		private readonly System.Random random = new System.Random();
		private ThunderTrialRound round;
		private WuXing selected;
		private Action advanceRealm;
		private Action<bool> leaveMemory;
		private readonly string[] journal = new string[ThunderTrialRound.Waves];
		private Text report;
		private int roots;
		private Font font;
		private Image backdrop, shade, figure, qiMark, pulse, wardFill;
		private InkThunderBolt bolt;
		private Text chapter, storm, forecast, ward, air, response, record, submitLabel;
		private Button submit;
		private RectTransform hand;
		private bool striking, closing;
		private RectTransform window, canvasRect;
		private readonly Vector3[] canvasCorners = new Vector3[4];
		internal static bool IsOpen(DongFuPanel panel) => panel != null && panel.transform.Find(ViewName) != null;

		internal static void AttachEntry(DongFuPanel panel, Action open)
		{
			var entry = RoleFolio.Image(panel.transform, "ThunderRehearsal", Color.white, true);
			RoleFolio.Fixed(entry.rectTransform, 1f, -12f, 34f, 86f, 29f);
			InkInterfaceArt.Set(entry, "JadeButton", false);
			var text = RoleFolio.Label(entry.transform, panel.DongFuNameText.font, "试演雷劫", 14, Ivory, TextAnchor.MiddleCenter);
			RoleFolio.Fill(text.rectTransform);
			var button = entry.gameObject.AddComponent<Button>();
			button.targetGraphic = entry;
			button.onClick.AddListener(() => open());
		}

		internal static void Open(DongFuPanel panel, Action advanceRealm)
		{
			if (IsOpen(panel)) return;
			Create(panel.transform, panel.DongFuNameText.font, Singleton<RoleManager>.Instance.Role_Entity, advanceRealm, null);
		}

		internal static void OpenMemory(TuPoDuJieExmplePanel panel, Action<bool> finish)
		{
			var elder = new RoleInfoEntity { RoleName = "玄玉真人", Sex = 1, HeadPortraitId = 1, ShuiLingGen = true };
			Create(panel.transform, panel.FangYuZhiText.font, elder, null, finish);
		}

		private static void Create(Transform parent, Font font, RoleInfoEntity role, Action advanceRealm, Action<bool> leaveMemory)
		{
			var background = RoleFolio.Image(parent, ViewName, Deep, true);
			RoleFolio.Fill(background.rectTransform);
			var view = background.gameObject.AddComponent<InkThunderTrial>();
			view.font = font;
			view.advanceRealm = advanceRealm;
			view.leaveMemory = leaveMemory;
			view.roots = (role.JinLingGen ? 2 : 0) | (role.MuLingGen ? 4 : 0) | (role.ShuiLingGen ? 8 : 0) |
				(role.HuoLingGen ? 16 : 0) | (role.TuLingGen ? 32 : 0);
			view.window = background.rectTransform;
			view.FitCanvas();
			view.Build(role);
			view.StartRound();
		}

		private void LateUpdate() => FitCanvas();

		private void FitCanvas()
		{
			if (!canvasRect) canvasRect = window.GetComponentInParent<Canvas>()?.rootCanvas.transform as RectTransform;
			if (!canvasRect || !(window.parent is RectTransform parent)) return;
			canvasRect.GetWorldCorners(canvasCorners);
			window.anchorMin = window.anchorMax = parent.pivot;
			Vector2 lower = parent.InverseTransformPoint(canvasCorners[0]);
			Vector2 upper = parent.InverseTransformPoint(canvasCorners[2]);
			if ((window.offsetMin - lower).sqrMagnitude > .001f) window.offsetMin = lower;
			if ((window.offsetMax - upper).sqrMagnitude > .001f) window.offsetMax = upper;
		}

		private void Build(RoleInfoEntity role)
		{
			var stage = RoleFolio.Box(transform, "劫云");
			Area(stage, 0, .48f, 1, 1);
			stage.gameObject.AddComponent<RectMask2D>();
			var cosmos = RoleFolio.Image(stage, "星河", Color.white);
			cosmos.sprite = InkCultivationInterface.Get("MeridianCosmos");
			var skyCrop = cosmos.gameObject.AddComponent<AspectRatioFitter>();
			skyCrop.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
			skyCrop.aspectRatio = cosmos.sprite ? cosmos.sprite.rect.width / cosmos.sprite.rect.height : .6f;
			backdrop = RoleFolio.Image(stage, "洞天", new Color(1f, 1f, 1f, .24f));
			var crop = backdrop.gameObject.AddComponent<AspectRatioFitter>();
			crop.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
			crop.aspectRatio = .75f;
			shade = RoleFolio.Image(stage, "天色", new Color32(20, 30, 40, 122));
			RoleFolio.Fill(shade.rectTransform);
			bolt = RoleFolio.Box(stage, "天雷").gameObject.AddComponent<InkThunderBolt>();
			RoleFolio.Fill(bolt.rectTransform);
			bolt.raycastTarget = false;
			var chamber = leaveMemory == null ? MonoSingleton<DongFuSceneManager>.Instance : null;
			var worn = chamber != null && chamber.PeopleBg != null ? chamber.PeopleBg.GetComponent<SpriteRenderer>() : null;
			var appearance = worn != null && worn.sprite != null ? worn.sprite : InkCultivationInterface.Get("Meditator");
			figure = Illustration(stage, "应劫", appearance, .08f, .04f, .91f, .75f);
			qiMark = Illustration(stage, "雷相", null, .70f, .29f, .90f, .57f);
			qiMark.color = new Color(1, 1, 1, .81f);
			pulse = Illustration(stage, "落雷", InkCultivationInterface.Get("Breakthrough"), .08f, .06f, .91f, .95f);
			pulse.enabled = false;

			chapter = Caption(stage, "", 13, Gold, .06f, .82f, .73f, .97f);
			storm = Caption(stage, "", 28, Ivory, .06f, .66f, .89f, .86f);
			Shadow(chapter);
			Shadow(storm);
			Button back = MakeButton(stage, "收阵", "JadeButton", .79f, .835f, .96f, .97f, out Text backText);
			backText.text = leaveMemory == null ? "收阵" : "出阵";
			back.onClick.AddListener(Close);
			var strip = RoleFolio.Image(stage, "雷讯", new Color32(28, 47, 44, 227));
			Area(strip.rectTransform, 0, 0, 1, .155f);
			forecast = Caption(strip.transform, "", 14, Ivory, .06f, 0, .97f, 1);

			var paper = RoleFolio.Image(transform, "符案", RoleFolio.Paper);
			Area(paper.rectTransform, 0, 0, 1, .48f);
			var p = paper.transform;
			var portrait = leaveMemory != null ? InkCultivationInterface.Get("Meditator") : Singleton<RoleManager>.Instance.GetTouXiangSprite(role.Sex, role.HeadPortraitId.ToString());
			Illustration(p, "道身", portrait, .045f, .77f, .20f, .97f);
			Caption(p, role.RoleName, 16, RoleFolio.Ink, .215f, .84f, .58f, .98f);
			ward = Caption(p, "", 13, RoleFolio.Jade, .215f, .73f, .60f, .85f);
			air = Caption(p, "", 14, RoleFolio.Jade, .63f, .76f, .96f, .95f, TextAnchor.MiddleRight);
			var rail = RoleFolio.Image(p, "护脉底", RoleFolio.Rule);
			Area(rail.rectTransform, .05f, .70f, .95f, .715f);
			wardFill = RoleFolio.Image(rail.transform, "护脉余量", RoleFolio.Jade);
			record = Caption(p, "", 13, RoleFolio.Secondary, .05f, .56f, .95f, .69f);

			hand = RoleFolio.Box(p, "五行法印");
			Area(hand, .045f, .275f, .955f, .55f);
			for (int i = 1; i <= 5; i++)
			{
				var seal = (WuXing)i;
				var card = RoleFolio.Image(hand, "Seal" + seal, Color.white, true);
				Area(card.rectTransform, (i - 1) * .2f + .006f, 0, i * .2f - .006f, 1);
				InkInterfaceArt.Set(card, "ParchmentHeading", false);
				Illustration(card.transform, seal.ToString(), InkInterfaceArt.Get("Element" + seal), .06f, .37f, .6f, .96f);
				Caption(card.transform, ThunderTrialRound.Names[i], 15, RoleFolio.Ink, .53f, .40f, .97f, .94f);
				sealHints[i] = Caption(card.transform, "", 12, RoleFolio.Secondary, .04f, .04f, .96f, .44f, TextAnchor.MiddleCenter);
				seals[i] = card.gameObject.AddComponent<Button>();
				seals[i].targetGraphic = card;
				seals[i].onClick.AddListener(() => Select(seal));
			}
			report = Caption(p, "", 13, RoleFolio.Ink, .07f, .28f, .93f, .56f);
			response = Caption(p, "", 14, RoleFolio.Ink, .05f, .14f, .95f, .27f, TextAnchor.MiddleCenter);
			submit = MakeButton(p, "应劫", "JadeButton", .20f, .014f, .80f, .145f, out submitLabel);
			submit.onClick.AddListener(Act);
		}

		private void StartRound()
		{
			round = new ThunderTrialRound(roots, random);
			Array.Clear(journal, 0, journal.Length);
			report.text = "";
			selected = WuXing.All;
			hand.gameObject.SetActive(true);
			var rootNames = new System.Text.StringBuilder();
			for (int i = 1; i <= 5; i++)
				if (round.HasRoot((WuXing)i)) rootNames.Append(ThunderTrialRound.Names[i]);
			record.text = rootNames.Length == 0 ? "点选法印，可查看承伤与真元变化。" : $"本命{rootNames}灵根 · 同属法印减伤 3 点";
			Refresh();
		}

		private static string Name(WuXing e) => ThunderTrialRound.Names[(int)e];

		private void Refresh()
		{
			ward.text = $"护脉  {round.Ward} / {ThunderTrialRound.MaxWard}";
			air.text = $"真元  {round.Qi} / {ThunderTrialRound.MaxQi}";
			Area(wardFill.rectTransform, 0, 0, (float)round.Ward / ThunderTrialRound.MaxWard, 1);
			string mode = leaveMemory != null ? "前尘雷劫" : advanceRealm == null ? "雷劫试演" : "破境雷劫";
			if (round.Ended)
			{
				hand.gameObject.SetActive(false);
				bolt.enabled = qiMark.enabled = false;
				shade.color = round.Won ? new Color32(46, 59, 44, 63) : new Color32(31, 38, 47, 168);
				record.text = "应劫实录";
				report.text = string.Join("\n", journal, 0, round.Wave);
				chapter.text = mode + "  /  劫后";
				storm.text = round.Won ? "云开见道" : "护脉已尽";
				forecast.text = round.Won ? "三道雷霆已过" : $"止于第{Number[round.Wave - 1]}道 · {ThunderTrialRound.Stages[round.Wave - 1]}";
				response.text = round.Won && leaveMemory != null ? "前尘已尽，转入轮回。" : round.Won && advanceRealm != null ? "三印已合，可凝气突破。" : "可再入阵，重新推演三道雷相。";
				submitLabel.text = round.Won && leaveMemory != null ? "离开回忆" : round.Won && advanceRealm != null ? "凝气破境" : "再试一阵";
				submit.interactable = true;
				return;
			}
			bolt.enabled = qiMark.enabled = true;
			shade.color = round.Thunder switch {
				WuXing.Jin => new Color32(91, 67, 24, 100), WuXing.Mu => new Color32(17, 68, 46, 108),
				WuXing.Shui => new Color32(21, 47, 75, 110), WuXing.Huo => new Color32(109, 41, 31, 100),
				_ => new Color32(85, 61, 44, 108)
			};
			chapter.text = $"{mode}  /  第{Number[round.Wave]}道 · {ThunderTrialRound.Stages[round.Wave]}";
			storm.text = Name(round.Thunder) + "雷将至";
			forecast.text = $"雷威 {round.Power}     ·     " + (round.Next == WuXing.All ? "末劫" : "下一道  " + Name(round.Next) + "雷");
			backdrop.sprite = InkCultivationInterface.Get("Chamber" + round.Thunder);
			qiMark.sprite = InkInterfaceArt.Get("Element" + round.Thunder);
			bolt.Tint(round.Thunder, .45f);
			for (int i = 1; i <= 5; i++)
			{
				var seal = (WuXing)i;
				var preview = round.Preview(seal);
				InkInterfaceArt.Set(seals[i].image, seal == selected ? "JadeSelection" : "ParchmentHeading", false);
				sealHints[i].text = preview.Ready ? preview.Name : "缺真元";
				seals[i].interactable = preview.Ready;
			}
			if (selected == WuXing.All)
			{
				response.text = "选一道法印应劫";
				submitLabel.text = "结印";
				submit.interactable = false;
				return;
			}
			var choice = round.Preview(selected);
			string qi = choice.QiChange > 0 ? "真元 +1" : choice.QiChange < 0 ? "真元 −1" : "真元不变";
			string affinity = round.HasRoot(selected) ? "（本命减 3）" : "";
			string sealName = Name(selected), sky = Name(round.Thunder);
			string relation = choice.Name switch
			{
				"截雷" => sealName + "克" + sky, "引息" => sky + "生" + sealName, "逆冲" => sky + "克" + sealName,
				_ => sealName + "印护脉"
			};
			response.text = $"{relation} · 承伤 {choice.Hit}{affinity} · {qi}";
			submitLabel.text = $"结{Name(selected)}印";
			submit.interactable = choice.Ready;
		}

		private void Select(WuXing seal)
		{
			if (striking || !round.Preview(seal).Ready) return;
			selected = seal;
			Refresh();
		}

		private void Act()
		{
			if (striking || closing) return;
			if (round.Ended)
			{
				if (round.Won && leaveMemory != null) Close();
				else if (round.Won && advanceRealm != null)
				{
					Action finish = advanceRealm;
					advanceRealm = null;
					Close();
					finish();
				}
				else StartRound();
				return;
			}
			var effect = round.Preview(selected);
			WuXing thunder = round.Thunder;
			if (!round.Resolve(selected)) return;
			record.text = $"{Name(selected)}印{effect.Name} · {Name(thunder)}雷落下，护脉 −{effect.Hit}";
			journal[round.Wave - 1] = $"{Number[round.Wave - 1]}  {Name(thunder)}雷 · {Name(selected)}印{effect.Name}    护脉 −{effect.Hit}";
			selected = WuXing.All;
			StartCoroutine(PlayStrike());
		}

		private IEnumerator PlayStrike()
		{
			striking = true;
			submit.interactable = false;
			pulse.enabled = true;
			float start = Time.unscaledTime;
			while (Time.unscaledTime - start < .46f)
			{
				float t = (Time.unscaledTime - start) / .46f;
				pulse.color = new Color(1, 1, 1, (1f - t) * .78f);
				bolt.Tint(bolt.Element, 1f - t * .55f);
				pulse.rectTransform.localScale = Vector3.one * (.8f + t * .4f);
				figure.rectTransform.localScale = Vector3.one * (1f - Mathf.Sin(Mathf.PI * t) * .04f);
				yield return null;
			}
			pulse.enabled = false;
			figure.rectTransform.localScale = Vector3.one;
			striking = false;
			Refresh();
		}

		private void Close()
		{
			if (closing) return;
			closing = true;
			gameObject.SetActive(false);
			Destroy(gameObject);
			leaveMemory?.Invoke(round.Won);
		}

		private Text Caption(Transform parent, string value, int size, Color color,
			float left, float bottom, float right, float top, TextAnchor anchor = TextAnchor.MiddleLeft)
		{
			Text t = RoleFolio.Label(parent, font, value, size, color, anchor);
			Area(t.rectTransform, left, bottom, right, top);
			UIManager.FitTextToRect(t);
			return t;
		}

		private static void Shadow(Text text)
		{
			var shadow = text.gameObject.AddComponent<Shadow>();
			shadow.effectColor = Deep;
			shadow.effectDistance = new Vector2(1, -1);
		}

		private Button MakeButton(Transform parent, string name, string skin,
			float left, float bottom, float right, float top, out Text label)
		{
			Image plate = RoleFolio.Image(parent, name, Color.white, true);
			Area(plate.rectTransform, left, bottom, right, top);
			InkInterfaceArt.Set(plate, skin, false);
			label = Caption(plate.transform, name, 15, Ivory, .03f, 0, .97f, 1, TextAnchor.MiddleCenter);
			var button = plate.gameObject.AddComponent<Button>();
			button.targetGraphic = plate;
			return button;
		}

		private static Image Illustration(Transform parent, string title, Sprite sprite,
			float left, float bottom, float right, float top)
		{
			Image art = RoleFolio.Image(parent, title, Color.white);
			Area(art.rectTransform, left, bottom, right, top);
			art.sprite = sprite;
			art.preserveAspect = true;
			return art;
		}

		private static void Area(RectTransform rect, float left, float bottom, float right, float top)
		{
			rect.anchorMin = new Vector2(left, bottom);
			rect.anchorMax = new Vector2(right, top);
			rect.offsetMin = rect.offsetMax = Vector2.zero;
		}
	}

	// Fixed forks mark the path to the figure. Opacity rises once as the player commits a seal.
	[RequireComponent(typeof(CanvasRenderer))]
	internal sealed class InkThunderBolt : MaskableGraphic
	{
		internal WuXing Element { get; private set; }
		private static readonly Color32[] Tints = { new Color32(252, 239, 209, 255),
			new Color32(255, 226, 143, 255), new Color32(192, 243, 192, 255), new Color32(180, 226, 255, 255),
			new Color32(255, 193, 160, 255), new Color32(249, 215, 174, 255) };
		private static readonly Vector2[] Trunk = { new Vector2(.77f, 1.02f), new Vector2(.72f, .90f),
			new Vector2(.73f, .86f), new Vector2(.65f, .78f), new Vector2(.67f, .76f),
			new Vector2(.56f, .66f), new Vector2(.53f, .58f) };
		private static readonly Vector2[] Fork = { Trunk[3], new Vector2(.77f, .75f), new Vector2(.80f, .70f),
			new Vector2(.86f, .68f) };

		internal void Tint(WuXing element, float alpha)
		{
			Element = element;
			Color tint = Tints[(int)element];
			tint.a = alpha;
			color = tint;
		}

		protected override void OnPopulateMesh(VertexHelper mesh)
		{
			mesh.Clear();
			Stroke(mesh, Trunk, 3f, .25f);
			Stroke(mesh, Trunk, 1.15f, 1f);
			Stroke(mesh, Fork, .65f, .85f);
		}

		private void Stroke(VertexHelper mesh, Vector2[] path, float width, float alpha)
		{
			Rect bounds = rectTransform.rect;
			Color tint = color;
			tint.a *= alpha;
			for (int i = 1; i < path.Length; i++)
			{
				Vector2 a = bounds.min + Vector2.Scale(bounds.size, path[i - 1]);
				Vector2 b = bounds.min + Vector2.Scale(bounds.size, path[i]);
				Vector2 normal = Vector2.Perpendicular(b - a).normalized * width;
				int index = mesh.currentVertCount;
				mesh.AddVert(a - normal, tint, Vector2.zero);
				mesh.AddVert(a + normal, tint, Vector2.zero);
				mesh.AddVert(b + normal * .65f, tint, Vector2.zero);
				mesh.AddVert(b - normal * .65f, tint, Vector2.zero);
				mesh.AddTriangle(index, index + 1, index + 2);
				mesh.AddTriangle(index + 2, index + 3, index);
			}
		}
	}

}
