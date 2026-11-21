using System;
using DG.Tweening;
using UnityEngine;

namespace XiuZhenJie
{
	public class DongFuSceneManager : MonoSingleton<DongFuSceneManager>
	{
		public Action SmallToBigCompeleteEvent;

		public Action BigToSmallCompeleteEvent;

		public Camera SceneCamera;

		public GameObject BackGround;

		public GameObject PeopleBg;

		public GameObject JingMaiTreeBg;

		private GameObject XueWeiBg;

		private DongFuWnd DongFuMainWnd;

		private int m_BallCount;

		private Tween smallToBigMoveAni;

		private Tween smallToBigScalAni;

		private Tween bigToSmallMoveAni;

		private Tween bigToSmallScalAni;

		private RectTransform UIRootRect;

		public Animator XiuXingAnimator;

		private const float CaveFadeDuration = 0.42f;
		private readonly Sprite[] caveThemes = new Sprite[6];
		private readonly Sprite[] caveGrades = new Sprite[5];
		private SpriteRenderer caveBackdrop;
		private SpriteRenderer caveSky;
		private SpriteRenderer caveIncoming;
		private SpriteRenderer caveDecoration;
		private SpriteRenderer meditationFigure;
		private Sprite defaultAppearance;
		private Vector3 meditationBasePosition;
		private Vector3 meditationBaseScale;
		private Vector3 cultivationScale;
		private InkCultivationParticles cultivationParticles;
		private WuXing selectedCaveTheme;
		private WuXing practiceElement;
		private float caveFade;

		protected override void Awake()
		{
			base.Awake();
			BuildCultivationStage();
		}

		public void Start()
		{
			m_BallCount = 0;
		}

		private void BuildCultivationStage()
		{
			caveThemes[0] = ReleasedResources.Load<Sprite>("InkCultivation/Chamber");
			if (caveThemes[0] != null && SceneCamera != null && BackGround != null)
			{
				// The recovered scene has a background renderer on the manager itself.
				// Keep the painted landscape below the sky and both below the actual character.
				var figure = PeopleBg != null ? PeopleBg.GetComponent<SpriteRenderer>() : null;
				int foreground = figure != null ? figure.sortingOrder : 0;
				int layer = figure != null ? figure.sortingLayerID : 0;
				if (BackGround.TryGetComponent(out SpriteRenderer legacyBackground)) legacyBackground.enabled = false;
				caveBackdrop = CreateStageSprite("InkChamberBackdrop", BackGround.transform, foreground - 7);
				caveIncoming = CreateStageSprite("InkChamberIncoming", BackGround.transform, foreground - 6);
				caveBackdrop.sortingLayerID = caveIncoming.sortingLayerID = layer;
				var stars = InkCultivationInterface.Get("MeridianCosmos");
				if (stars != null)
				{
					caveSky = CreateStageSprite("InkChamberStarfield", BackGround.transform, foreground - 5);
					caveSky.sprite = stars;
					caveSky.sortingLayerID = layer;
					var skyWash = ReleasedResources.Load<Material>("InkCultivation/StarfieldWash");
					if (skyWash != null) caveSky.sharedMaterial = skyWash;
					else caveSky.color = new Color(1f, 1f, 1f, .78f);
					FitBackdrop(caveSky);
				}
				caveBackdrop.sprite = caveThemes[0];
				caveIncoming.enabled = false;
				FitBackdrop(caveBackdrop);
			}

			meditationFigure = PeopleBg != null ? PeopleBg.GetComponent<SpriteRenderer>() : null;
			if (meditationFigure == null || meditationFigure.sprite == null)
			{
				return;
			}
			defaultAppearance = meditationFigure.sprite;
			meditationBasePosition = meditationFigure.transform.localPosition;
			meditationBaseScale = meditationFigure.transform.localScale;
			cultivationScale = meditationBaseScale;
			SetMeditationAppearance(InkCultivationInterface.Get("Meditator"));
			if (XiuXingAnimator != null && XiuXingAnimator.TryGetComponent(out SpriteRenderer legacyAura))
			{
				// The controller still receives breakthrough triggers and animation events.
				legacyAura.enabled = false;
			}

			caveDecoration = CreateStageSprite("InkCaveGradeDecoration", meditationFigure.transform.parent, meditationFigure.sortingOrder - 2);
			caveDecoration.sortingLayerID = meditationFigure.sortingLayerID;
			caveDecoration.enabled = false;
			cultivationParticles = gameObject.AddComponent<InkCultivationParticles>();
			cultivationParticles.Configure(meditationFigure);
		}

		private static SpriteRenderer CreateStageSprite(string name, Transform parent, int order)
		{
			var node = new GameObject(name, typeof(SpriteRenderer));
			node.layer = parent.gameObject.layer;
			node.transform.SetParent(parent, false);
			var art = node.GetComponent<SpriteRenderer>();
			art.sortingOrder = order;
			return art;
		}

		// Equipment visuals can supply a replacement sprite through this entry point.
		// A null sprite restores the scene's default, with the hem anchored to the same floor point.
		public void SetMeditationAppearance(Sprite equipmentAppearance)
		{
			if (meditationFigure == null)
			{
				return;
			}
			Sprite appearance = equipmentAppearance != null ? equipmentAppearance : defaultAppearance;
			Bounds baseline = defaultAppearance.bounds;
			Bounds bounds = appearance.bounds;
			float scale = Mathf.Min(baseline.size.x / bounds.size.x, baseline.size.y / bounds.size.y);
			cultivationScale = meditationBaseScale * scale;
			Vector3 position = meditationBasePosition;
			position.x += baseline.center.x * meditationBaseScale.x - bounds.center.x * cultivationScale.x;
			position.y += baseline.min.y * meditationBaseScale.y - bounds.min.y * cultivationScale.y;
			meditationFigure.sprite = appearance;
			meditationFigure.transform.localPosition = position;
			meditationFigure.transform.localScale = cultivationScale;
		}

		private void LateUpdate()
		{
			if (caveBackdrop != null)
			{
				FitBackdrop(caveBackdrop);
				if (caveSky != null) FitBackdrop(caveSky);
				UpdateCaveFade();
			}
			if (meditationFigure != null && meditationFigure.gameObject.activeInHierarchy)
			{
				float breath = Mathf.Sin(Time.time * 1.2f) * 0.013f;
				meditationFigure.transform.localScale =
					Vector3.Scale(cultivationScale, new Vector3(1f - breath * 0.25f, 1f + breath, 1f));
			}
			if (caveDecoration != null && caveDecoration.sprite != null)
			{
				caveDecoration.enabled = meditationFigure.gameObject.activeInHierarchy && meditationFigure.enabled;
				Bounds figureBounds = meditationFigure.bounds;
				float scale = figureBounds.size.x * 1.18f / caveDecoration.sprite.bounds.size.x;
				caveDecoration.transform.SetPositionAndRotation(
					new Vector3(figureBounds.center.x, figureBounds.min.y + 0.08f, figureBounds.center.z), Quaternion.identity);
				caveDecoration.transform.localScale = Vector3.one * scale;
			}
		}

		private void FitBackdrop(SpriteRenderer art)
		{
			Vector3 size = art.sprite.bounds.size;
			float viewHeight = SceneCamera.orthographicSize * 2f;
			float scale = Mathf.Max(viewHeight / size.y, viewHeight * SceneCamera.aspect / size.x);
			art.transform.localScale = Vector3.one * scale;
			Vector3 center = SceneCamera.transform.position;
			art.transform.position = new Vector3(center.x, center.y, BackGround.transform.position.z + 3f);
		}

		private void UpdateCaveFade()
		{
			if (!caveIncoming.enabled)
			{
				return;
			}
			FitBackdrop(caveIncoming);
			caveFade = Mathf.MoveTowards(caveFade, 1f, Time.unscaledDeltaTime / CaveFadeDuration);
			caveIncoming.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, caveFade));
			if (caveFade >= 1f)
			{
				caveBackdrop.sprite = caveIncoming.sprite;
				FitBackdrop(caveBackdrop);
				caveIncoming.enabled = false;
			}
		}

		internal void SelectCaveTheme(WuXing theme)
		{
			selectedCaveTheme = theme;
			RefreshCaveTheme();
		}

		private void RefreshCaveTheme()
		{
			if (caveBackdrop == null)
			{
				return;
			}
			WuXing theme = selectedCaveTheme == WuXing.All ? practiceElement : selectedCaveTheme;
			int index = (int)theme;
			if (index < 0 || index >= caveThemes.Length)
			{
				return;
			}
			if (caveThemes[index] == null)
			{
				caveThemes[index] = ReleasedResources.Load<Sprite>("InkCultivation/Chamber" + theme);
			}
			Sprite next = caveThemes[index];
			if (next == null || next == (caveIncoming.enabled ? caveIncoming.sprite : caveBackdrop.sprite))
			{
				return;
			}
			if (caveIncoming.enabled && caveFade > 0.5f)
			{
				caveBackdrop.sprite = caveIncoming.sprite;
			}
			caveIncoming.sprite = next;
			caveIncoming.color = new Color(1f, 1f, 1f, 0f);
			caveIncoming.enabled = true;
			caveFade = 0f;
			FitBackdrop(caveIncoming);
		}

		internal void SetCaveGrade(int grade)
		{
			cultivationParticles?.SetGrade(grade);
			if (caveDecoration == null)
			{
				return;
			}
			grade = Mathf.Clamp(grade, 0, caveGrades.Length - 1);
			if (grade > 0 && caveGrades[grade] == null)
			{
				caveGrades[grade] = ReleasedResources.Load<Sprite>("InkCultivation/CaveGrade" + grade);
			}
			caveDecoration.sprite = caveGrades[grade];
			caveDecoration.enabled = caveDecoration.sprite != null;
		}

		internal void ShowCultivationQi(WuXing primary, WuXing secondary)
		{
			practiceElement = primary != WuXing.All ? primary : secondary;
			RefreshCaveTheme();
			if (cultivationParticles != null)
			{
				cultivationParticles.enabled = true;
				cultivationParticles.SetElements(primary, secondary);
			}
		}

		internal void HideCultivationQi()
		{
			if (cultivationParticles != null)
			{
				cultivationParticles.enabled = false;
			}
		}

		public void StartTuPo()
		{
			cultivationParticles?.Surge();
			XiuXingAnimator.SetBool("StartTuPo", value: true);
			Timer.Register(0.5f, () =>
			{
				XiuXingAnimator.SetBool("StartTuPo", value: false);
			});
		}

		public void EndTuPo()
		{
			cultivationParticles?.Surge();
			XiuXingAnimator.SetBool("EndTuPo", value: true);
			Timer.Register(0.5f, () =>
			{
				XiuXingAnimator.SetBool("EndTuPo", value: false);
			});
		}

		public void PeopleBgSmallToBigCompelete()
		{
			if (SmallToBigCompeleteEvent != null)
			{
				SmallToBigCompeleteEvent();
			}
		}

		public void PeopleBigToSmallCompelete()
		{
			if (BigToSmallCompeleteEvent != null)
			{
				BigToSmallCompeleteEvent();
			}
		}

		public void EnterJingMaiWnd()
		{
			XueWeiBg.gameObject.SetActive(value: true);
			PeopleBg.gameObject.SetActive(value: false);
			XueWeiBg.transform.DOScale(2f, 1f);
			DOTween.To(() => SceneCamera.orthographicSize, (float v) =>
			{
				SceneCamera.orthographicSize = v;
			}, 15f, 1f);
		}

		public void ExitJingMaiWnd()
		{
			XueWeiBg.gameObject.SetActive(value: false);
			PeopleBg.gameObject.SetActive(value: true);
			XueWeiBg.transform.DOScale(0.2914222f, 1f);
			SceneCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
			SceneCamera.orthographicSize = 5f;
		}
	}
}
