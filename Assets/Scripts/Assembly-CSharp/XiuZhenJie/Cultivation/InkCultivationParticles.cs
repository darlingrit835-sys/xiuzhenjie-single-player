using UnityEngine;

namespace XiuZhenJie
{
	[DisallowMultipleComponent]
	public sealed class InkCultivationParticles : MonoBehaviour
	{
		private const float Turn = 2f * Mathf.PI;

		private struct Mote
		{
			public SpriteRenderer Renderer;
			public float Phase;
			public float Variation;
		}

		private struct Flight
		{
			public Vector2 Offset;
			public float Size;
			public float Angle;
			public float Opacity;
			public bool InFront;

			public Flight(float x, float y, float size, float angle, float opacity, bool inFront = false)
			{
				Offset = new Vector2(x, y);
				Size = size;
				Angle = angle;
				Opacity = opacity;
				InFront = inFront;
			}
		}

		private readonly Sprite[] marks = new Sprite[6];
		private SpriteRenderer figure;
		private Transform field;
		private Mote[] mist;
		private Mote[] mainQi;
		private Mote[] extraQi;
		private int mistCount;
		private int mainCount;
		private int extraCount;
		private WuXing primary;
		private WuXing secondary;
		private int caveGrade;
		private float gradeIntensity;
		private float time;
		private float elementFade;
		private float surge;

		internal void Configure(SpriteRenderer figure)
		{
			if (field != null || figure == null)
			{
				return;
			}

			this.figure = figure;
			field = new GameObject("Cultivation Qi").transform;
			field.SetParent(transform, false);
			mist = CreateMotes("Ink mist", 10, 0.1f);
			mainQi = CreateMotes("Primary Qi", 18, 0.3f);
			extraQi = CreateMotes("Secondary Qi", 11, 0.7f);
			gradeIntensity = caveGrade * 0.25f;
			ApplyMarks();
		}

		internal void SetGrade(int grade)
		{
			grade = Mathf.Clamp(grade, 0, 4);
			if (grade == caveGrade)
			{
				return;
			}

			caveGrade = grade;
			if (field != null)
			{
				ApplyDensity();
			}
		}

		internal void SetElements(WuXing primary, WuXing secondary = WuXing.All)
		{
			primary = ValidElement(primary);
			secondary = ValidElement(secondary);
			if (primary == WuXing.All)
			{
				primary = secondary;
				secondary = WuXing.All;
			}
			if (primary == secondary)
			{
				secondary = WuXing.All;
			}
			if (this.primary == primary && this.secondary == secondary)
			{
				return;
			}

			this.primary = primary;
			this.secondary = secondary;
			elementFade = 0f;
			if (field != null)
			{
				ApplyMarks();
			}
		}

		internal void Surge()
		{
			surge = 1f;
		}

		private static WuXing ValidElement(WuXing element)
		{
			return (uint)element <= (uint)WuXing.Tu ? element : WuXing.All;
		}

		private Mote[] CreateMotes(string label, int count, float cycleOffset)
		{
			var motes = new Mote[count];
			for (int i = 0; i < count; i++)
			{
				var node = new GameObject(label, typeof(SpriteRenderer));
				node.layer = figure.gameObject.layer;
				node.transform.SetParent(field, false);
				motes[i] = new Mote
				{
					Renderer = node.GetComponent<SpriteRenderer>(),
					// Start each larger group across the whole orbit rather than adding one sector at a time.
					Phase = Mathf.Repeat(i * 0.618034f + cycleOffset, 1f),
					Variation = Mathf.Repeat(i * 0.7548777f + cycleOffset, 1f)
				};
			}
			return motes;
		}

		private void ApplyMarks()
		{
			ApplyMark(mist, WuXing.All);
			ApplyMark(mainQi, primary);
			ApplyMark(extraQi, secondary);
			ApplyDensity();
		}

		private void ApplyMark(Mote[] motes, WuXing element)
		{
			int index = (int)element;
			Sprite sprite = marks[index];
			if (sprite == null)
			{
				sprite = ReleasedResources.Load<Sprite>("InkCultivation/Qi" + element);
				marks[index] = sprite;
			}
			if (sprite == null)
			{
				sprite = marks[(int)WuXing.All];
			}
			foreach (Mote mote in motes)
			{
				mote.Renderer.sprite = sprite;
				mote.Renderer.color = Color.clear;
			}
		}

		private void ApplyDensity()
		{
			mistCount = 5 + (mist.Length - 5) * caveGrade / 4;
			mainCount = primary == WuXing.All ? 0 : 9 + (mainQi.Length - 9) * caveGrade / 4;
			extraCount = secondary == WuXing.All ? 0 : 5 + (extraQi.Length - 5) * caveGrade / 4;
			SetVisibleMotes(mist, mistCount);
			SetVisibleMotes(mainQi, mainCount);
			SetVisibleMotes(extraQi, extraCount);
		}

		private static void SetVisibleMotes(Mote[] motes, int count)
		{
			bool hasSprite = motes[0].Renderer.sprite != null;
			for (int i = 0; i < motes.Length; i++)
			{
				SpriteRenderer renderer = motes[i].Renderer;
				bool visible = hasSprite && i < count;
				if (renderer.enabled != visible)
				{
					renderer.color = Color.clear;
					renderer.enabled = visible;
				}
			}
		}

		private void OnDisable()
		{
			if (field != null)
			{
				field.gameObject.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			if (field != null)
			{
				Destroy(field.gameObject);
			}
		}

		private void LateUpdate()
		{
			if (field == null)
			{
				return;
			}

			bool visible = figure != null && figure.gameObject.activeInHierarchy && figure.enabled && figure.sprite != null;
			if (field.gameObject.activeSelf != visible)
			{
				field.gameObject.SetActive(visible);
			}
			if (!visible)
			{
				return;
			}

			float elapsed = Time.deltaTime;
			gradeIntensity = Mathf.MoveTowards(gradeIntensity, caveGrade * 0.25f, elapsed * 2.4f);
			time += elapsed * (0.88f + 0.22f * gradeIntensity);
			elementFade = Mathf.MoveTowards(elementFade, 1f, elapsed * 2.4f);
			surge = Mathf.MoveTowards(surge, 0f, elapsed * 0.75f);

			Bounds bounds = figure.bounds;
			Vector3 inheritedScale = field.lossyScale;
			Vector2 scaleFactor = new Vector2(
				1f / Mathf.Max(Mathf.Abs(inheritedScale.x), 0.001f),
				1f / Mathf.Max(Mathf.Abs(inheritedScale.y), 0.001f));
			DrawMotes(mist, mistCount, WuXing.All, bounds, scaleFactor, primary == WuXing.All ? 0.75f : 0.36f);
			if (primary != WuXing.All)
			{
				DrawMotes(mainQi, mainCount, primary, bounds, scaleFactor, elementFade * (secondary == WuXing.All ? 1f : 0.82f));
			}
			if (secondary != WuXing.All)
			{
				DrawMotes(extraQi, extraCount, secondary, bounds, scaleFactor, elementFade * 0.72f);
			}
		}

		private void DrawMotes(Mote[] motes, int count, WuXing element, Bounds bounds, Vector2 scaleFactor, float opacity)
		{
			Sprite mark = motes[0].Renderer.sprite;
			if (mark == null)
			{
				return;
			}

			Vector3 center = bounds.center;
			Vector3 extent = bounds.extents;
			float spriteWidth = Mathf.Max(mark.bounds.size.x, mark.bounds.size.y);
			float artScale = Mathf.Min(extent.x, extent.y) * (1f + 0.08f * gradeIntensity + 0.18f * surge) / spriteWidth;
			float radiusX = extent.x * (1f + 0.12f * gradeIntensity + 0.14f * surge);
			float radiusY = extent.y * (1f + 0.06f * gradeIntensity);
			float densityOpacity = opacity * (0.72f + 0.42f * gradeIntensity) * (1f + 0.38f * surge);
			Color pigment = Color.Lerp(Pigment(element), Color.white, gradeIntensity * 0.12f);
			for (int i = 0; i < count; i++)
			{
				Mote mote = motes[i];
				Flight flight = Move(element, mote.Phase, mote.Variation, time);
				float size = flight.Size * artScale;
				Vector3 position = center + new Vector3(flight.Offset.x * radiusX, flight.Offset.y * radiusY, 0f);

				SpriteRenderer renderer = mote.Renderer;
				renderer.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, flight.Angle));
				renderer.transform.localScale = new Vector3(size * scaleFactor.x, size * scaleFactor.y, 1f);
				renderer.sortingLayerID = figure.sortingLayerID;
				renderer.sortingOrder = figure.sortingOrder + (flight.InFront ? 1 : -1);
				pigment.a = Mathf.Clamp01(flight.Opacity * densityOpacity);
				renderer.color = pigment;
			}
		}

		private static Flight Move(WuXing element, float phase, float variation, float time)
		{
			float orbit;
			float progress;
			float life;
			float side = variation < 0.5f ? -1f : 1f;

			switch (element)
			{
				case WuXing.Jin:
					progress = Mathf.Repeat(phase + time / (2.1f + 1.7f * variation), 1f);
					life = Mathf.Pow(Mathf.Sin(progress * Mathf.PI), 8f);
					orbit = (phase + variation) * Turn + time * 0.32f;
					return new Flight(
						Mathf.Cos(orbit) * (1.0f + 0.3f * variation),
						-0.26f + 0.73f * Mathf.Sin(orbit), 0.045f + life * 0.16f,
						orbit * Mathf.Rad2Deg * 0.13f, life * 0.94f, Mathf.Sin(orbit) < 0f);

				case WuXing.Mu:
					orbit = (phase + time / (6.5f + 2.5f * variation)) * Turn;
					life = 0.68f + 0.31f * Mathf.Sin(time * 1.9f + phase * Turn);
					return new Flight(
						side * (0.91f + 0.44f * Mathf.Cos(orbit)),
						-0.18f + 0.85f * Mathf.Sin(orbit), 0.12f + variation * 0.11f,
						side * 18f + 44f * Mathf.Sin(orbit * 1.5f + time * 0.8f),
						life * 0.65f);

				case WuXing.Shui:
					orbit = (phase + time / (4.1f + 0.8f * variation)) * Turn;
					float waterX = Mathf.Cos(orbit);
					float waterY = Mathf.Sin(orbit);
					return new Flight(
						waterX * (1.32f + variation * 0.15f),
						-0.53f + waterY * (0.27f + variation * 0.12f),
						0.13f + 0.13f * variation, -waterX * 19f,
						0.38f + 0.40f * Mathf.Abs(waterX), waterY < 0f);

				case WuXing.Huo:
					progress = Mathf.Repeat(phase + time / (1.7f + 1.55f * variation), 1f);
					life = Mathf.Sin(progress * Mathf.PI);
					float taper = 1f - progress * 0.63f;
					return new Flight(
						side * (1.12f - progress * 0.32f + 0.22f * Mathf.Sin(progress * 8f + phase * Turn)),
						-0.88f + 1.52f * progress, (0.14f + 0.12f * variation) * taper,
						-side * 17f + 16f * Mathf.Sin(progress * 9f + phase * 3f),
						life * (0.67f + 0.16f * Mathf.Sin(time * 13.5f + phase * Turn)),
						progress < 0.42f);

				case WuXing.Tu:
					progress = Mathf.Repeat(phase + time / (3.6f + 2.2f * variation), 1f);
					life = Mathf.Sin(progress * Mathf.PI);
					return new Flight(
						side * (0.36f + progress * (1.42f + 0.17f * variation)),
						-0.89f + life * (0.13f + 0.27f * variation),
						0.08f + 0.115f * variation, (phase * 300f + progress * 135f) * side,
						life * 0.68f, true);

				default:
					orbit = (phase + time / (8.4f + 2.8f * variation)) * Turn;
					life = 0.66f + 0.34f * Mathf.Cos(orbit * 2f + phase * Turn);
					return new Flight(
						Mathf.Cos(orbit) * (1.10f + 0.43f * variation),
						-0.20f + 0.86f * Mathf.Sin(orbit), 0.08f + variation * 0.15f,
						orbit * 20f, life * 0.58f);
			}
		}

		private static Color Pigment(WuXing element)
		{
			return element switch
			{
				WuXing.Jin => new Color32(241, 194, 91, 255),
				WuXing.Mu => new Color32(103, 168, 104, 255),
				WuXing.Shui => new Color32(101, 179, 202, 255),
				WuXing.Huo => new Color32(242, 111, 66, 255),
				WuXing.Tu => new Color32(180, 137, 80, 255),
				_ => new Color32(218, 224, 207, 255)
			};
		}
	}
}
