using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	// One particle emitter and one dynamic ribbon mesh for the entire battle.
	public sealed class BattleFlourish : MonoBehaviour
	{
		enum Gesture { Cast, Strike, Gather, Flight }

		struct Cue
		{
			public Gesture Kind;
			public EntityBase Unit;
			public Transform Visual;
			public Vector3 Rest, Point, Direction;
			public Quaternion Rotation;
			public Color Color;
			public int Element;
			public float Start, Duration;
		}

		sealed class Readout
		{
			public TextMesh Text, Shadow;
			public Vector3 Origin;
			public Color Ink;
			public float Start;
		}

		readonly List<Readout> readouts = new List<Readout>(12);
		Font readoutFont;

		const int MaxCues = 24;
		readonly List<Cue> cues = new List<Cue>(MaxCues);
		readonly List<Vector3> verts = new List<Vector3>(4096);
		readonly List<Vector2> uvs = new List<Vector2>(4096);
		readonly List<Color32> colors = new List<Color32>(4096);
		readonly List<int> indices = new List<int>(6144);
		ParticleSystem sparks;
		Mesh ribbons;
		Material lightInk;
		BattleSceneManager scene;
		uint noise = 0x9e3779b9;
		float Noise()
		{
			noise ^= noise << 13; noise ^= noise >> 17; noise ^= noise << 5;
			return (noise & 65535) / 65535f;
		}

		void Awake()
		{
			scene = GetComponent<BattleSceneManager>();
			lightInk = new Material(ReleasedResources.Load<Shader>("shaders/BattleRadiance"));
			var emitter = new GameObject("五行流萤");
			emitter.transform.SetParent(transform, false);
			sparks = emitter.AddComponent<ParticleSystem>();
			sparks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			var main = sparks.main;
			main.loop = true;
			main.startSpeed = 0;
			main.maxParticles = 384;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.scalingMode = ParticleSystemScalingMode.Shape;
			var emission = sparks.emission;
			emission.enabled = false;
			var shape = sparks.shape;
			shape.enabled = false;
			var overLife = sparks.colorOverLifetime;
			overLife.enabled = true;
			var fade = new UnityEngine.Gradient();
			fade.SetKeys(new[] { new GradientColorKey(Color.white,0), new GradientColorKey(Color.white,1) },
				new[] { new GradientAlphaKey(.9f,0), new GradientAlphaKey(.8f,.35f), new GradientAlphaKey(0,1) });
			overLife.color = fade;
			var particleRenderer = sparks.GetComponent<ParticleSystemRenderer>();
			particleRenderer.sharedMaterial = lightInk;
			particleRenderer.sortingOrder = 32;
			sparks.Play();

			var lines = new GameObject("剑光与符纹");
			lines.transform.SetParent(transform, false);
			ribbons = new Mesh { name = "法术笔画" };
			ribbons.MarkDynamic();
			lines.AddComponent<MeshFilter>().sharedMesh = ribbons;
			var brush = lines.AddComponent<MeshRenderer>();
			brush.sharedMaterial = lightInk;
			brush.sortingOrder = 31;
		}

		static Color Element(int element)
		{
			switch (element)
			{
				case 1: return new Color(1,.79f,.28f);
				case 2: return new Color(.30f,1,.56f);
				case 3: return new Color(.30f,.83f,1);
				case 4: return new Color(1,.32f,.13f);
				case 5: return new Color(1,.57f,.24f);
				default: return new Color(.77f,.60f,1);
			}
		}

		static Vector3 Direction(float angle)
		{
			return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle),0);
		}

		void Spark(Vector3 point, Vector3 speed, Color color, float size, float life)
		{
			point.z = -2;
			var mote = new ParticleSystem.EmitParams
			{
				position = point, velocity = speed, startColor = color,
				startSize = size, startLifetime = life, rotation = Noise()*90
			};
			sparks.Emit(mote, 1);
		}

		public void VistaMotes(Vector3 origin, int kind, bool discovery)
		{
			Color color;
			switch (kind)
			{
				case 1: color = new Color(1, .79f, .31f); break;
				case 2: color = new Color(.38f, 1, .92f); break;
				case 3: color = new Color(.60f, .70f, 1); break;
				case 4: color = new Color(.72f, .48f, 1); break;
				case 5: color = new Color(1, .42f, .20f); break;
				default: color = new Color(1, .69f, .37f); break;
			}
			int count = discovery ? 17 : 1;
			for (int i = 0; i < count; i++)
			{
				Vector3 ray = Direction(Noise() * 6.283f);
				Vector3 position = origin + new Vector3(ray.x * (1.0f + Noise()), .60f + ray.y * (1f + Noise()));
				Vector3 velocity = kind == 1 || kind == 6 ? new Vector3(.34f, -.29f)
					: kind == 2 ? new Vector3(ray.x * .22f, -.34f) : new Vector3(ray.x * .08f, .36f);
				Spark(position, velocity, color, .07f + Noise() * .12f, .65f + Noise() * .7f);
			}
		}

		public void Footfall(EntityBase unit, Vector3 direction, float shoeOffset = 0f)
		{
			Vector3 basePoint = unit.GetPos() - direction*.27f + new Vector3(-direction.y, direction.x) * shoeOffset;
			Color jade = Element(3);
			for (int i=0; i<3; i++)
				Spark(basePoint + Direction(Noise()*6.28f)*.12f, -direction*(.4f+Noise()) + Vector3.up*.18f, jade, .18f+Noise()*.14f,.3f+Noise()*.16f);
		}

		void Add(Gesture kind, EntityBase unit, Vector3 point, Vector3 direction, int element, float duration)
		{
			for (int i=cues.Count-1; i>=0; i--)
			{
				if (cues[i].Unit != unit) continue;
				if (cues[i].Kind == kind) Remove(i);
				else RestorePose(i);
			}
			if (cues.Count == MaxCues) Remove(0);
			Transform visual = scene.FieldView == null ? null : scene.FieldView.GetVisual(unit.BaseController);
			cues.Add(new Cue { Kind=kind, Unit=unit, Point=point, Direction=direction, Color=Element(element), Element=element,
				Start=Time.time, Duration=duration, Visual=visual,
				Rest=visual != null ? visual.localPosition : Vector3.zero,
				Rotation=visual != null ? visual.localRotation : Quaternion.identity });
		}

		public void Cast(EntityBase unit, int element, float windup)
		{
			Color hue = Element(element);
			Vector3 origin = unit.GetPos();
			Vector3 toward = unit.Target != null ? (unit.Target.GetPos()-origin).normalized : Vector3.up;
			Add(Gesture.Cast, unit, origin, toward, element, Mathf.Max(.32f, windup));
			for (int i=0; i<22; i++)
			{
				Vector3 spoke = Direction((i+Noise()*.4f)*Mathf.PI/11);
				float radius = .6f+Noise()*.75f, life = Mathf.Clamp(windup,.22f,.75f);
				Spark(origin+spoke*radius, -spoke*radius/life, hue, .11f+Noise()*.22f,life);
			}
		}

		public void Launch(EntityBase caster, EntityBase target, int element)
		{
			if (target == null || !target.VisibleToPlayer() || !caster.VisibleToPlayer()) return;
			Vector3 start = caster.GetPos() + Vector3.up * .44f;
			Vector3 path = target.GetPos() + Vector3.up * .44f - start;
			if (path.sqrMagnitude < .16f) return;
			if (cues.Count == MaxCues) Remove(0);
			cues.Add(new Cue { Kind=Gesture.Flight, Unit=caster, Point=start, Direction=path,
				Element=element, Color=Element(element), Start=Time.time, Duration=.46f });
		}

		void DrawFlight(Cue cue, float progress)
		{
			Vector3 forward = cue.Direction.normalized;
			Vector3 side = new Vector3(-forward.y, forward.x);
			Color core = Color.Lerp(cue.Color, Color.white, .64f);
			for (int blade = -1; blade <= 1; blade++)
			{
				float t = Mathf.Clamp01(progress * 1.25f - Mathf.Abs(blade) * .13f);
				float tail = Mathf.Max(0f, t - .35f);
				Vector3 tip = cue.Point + cue.Direction*t + side*(blade*Mathf.Sin(t*Mathf.PI)*.70f);
				if (!scene.FieldView.IsVisible(tip)) continue;
				Vector3 last = tip;
				for (int k=1; k<=6; k++)
				{
					float along = Mathf.Lerp(t,tail,k/6f);
					Vector3 point = cue.Point + cue.Direction*along + side*(blade*Mathf.Sin(along*Mathf.PI)*.70f);
					Color tint = cue.Color;
					tint.a = (1f-k/8f)*(1f-progress*.55f);
					Line(last,point,.12f*(1f-k/8f),tint);
					Line(last,point,.027f,tint);
					last=point;
				}
				if (cue.Element == 1)
				{
					Vector3 hilt = tip-forward*.58f;
					Line(hilt,tip,.080f,core);
					Line(hilt-side*.14f,hilt+side*.14f,.048f,cue.Color);
				}
				else
					Arc(tip,.13f+Mathf.Sin(t*Mathf.PI)*.16f,t*4,4.7f,8,.055f,core);
			}
		}

		public void CancelCast(EntityBase unit)
		{
			for (int i=cues.Count-1;i>=0;i--) if (cues[i].Unit==unit && cues[i].Kind==Gesture.Cast) Remove(i);
		}

		public void Strike(EntityBase struck, Vector3 source, int element)
		{
			Vector3 point = struck.GetPos();
			Vector3 dir = (point-source).normalized;
			if (dir.sqrMagnitude < .01f) dir = Vector3.right;
			Color color = Element(element);
			Add(Gesture.Strike, struck, point,dir,element,.46f);
			for (int i=0; i<18; i++)
			{
				Vector3 spoke = Direction(Noise()*6.28f);
				Spark(point+spoke*.12f, (spoke+dir*.45f)*(.6f+Noise()*2.5f), color,.13f+Noise()*.32f,.22f+Noise()*.35f);
			}
		}

		public void Gather(EntityBase unit, Vector3 node, bool ore)
		{
			Color color = Element(ore ? 1 : 2);
			Add(Gesture.Gather,unit,node,(node-unit.GetPos()).normalized,ore ? 1 : 2,.62f);
			for (int i=0;i<22;i++)
			{
				Vector3 point = node+Direction(Noise()*6.28f)*(.18f+Noise()*.48f);
				Spark(point,(unit.GetPos()-point)*1.6f+Vector3.up*(.5f+Noise()),color,.15f+Noise()*.26f,.38f+Noise()*.32f);
			}
		}

		void RestorePose(int index)
		{
			Cue cue=cues[index];
			if (cue.Visual == null) return;
			cue.Visual.localPosition=cue.Rest;
			cue.Visual.localRotation=cue.Rotation;
			cue.Visual=null;
			cues[index]=cue;
		}

		void Remove(int index)
		{
			RestorePose(index);
			cues.RemoveAt(index);
		}

		void Line(Vector3 a, Vector3 b, float width, Color color)
		{
			var d = (b-a).normalized;
			var edge = new Vector3(-d.y,d.x,0)*width;
			a.z=b.z=-1.9f;
			int start=verts.Count;
			verts.Add(a-edge); verts.Add(a+edge); verts.Add(b+edge); verts.Add(b-edge);
			uvs.Add(new Vector2(.5f,0)); uvs.Add(new Vector2(.5f,1)); uvs.Add(new Vector2(.5f,1)); uvs.Add(new Vector2(.5f,0));
			for (int n=0;n<4;n++) colors.Add(color);
			indices.Add(start); indices.Add(start+1); indices.Add(start+2);
			indices.Add(start); indices.Add(start+2); indices.Add(start+3);
		}

		void Arc(Vector3 center,float radius,float rotation,float sweep,int steps,float width,Color color)
		{
			var p = center+Direction(rotation)*radius;
			for (int i=1;i<=steps;i++)
			{
				var end = center+Direction(rotation+sweep*i/steps)*radius;
				Line(p,end,width,color);
				p=end;
			}
		}

		void ImpactLines(Cue cue, Vector3 point, float t, Color color)
		{
			float angle = Mathf.Atan2(cue.Direction.y, cue.Direction.x);
			Vector3 side = new Vector3(-cue.Direction.y, cue.Direction.x);
			switch (cue.Element)
			{
				case 2: // Wood: a climbing branch with pairs of leaves.
					for (int i = 0; i < 3; i++)
					{
						Vector3 stem = point + Vector3.up * (i * .26f + t * .28f);
						Line(stem, stem + Vector3.up * .31f, .04f, color);
						for (int sign = -1; sign <= 1; sign += 2)
							Arc(stem + Vector3.right * sign * .20f, .23f, sign < 0 ? 0 : 1.4f, 1.8f, 5, .035f, color);
					}
					break;
				case 3: // Water: ripples of different radii.
					Arc(point, .24f + t, angle, 6.283f, 26, .046f, color);
					Arc(point, .12f + t * .72f, -angle, 6.283f, 20, .03f, color);
					break;
				case 4: // Fire: a fan of rising tongues.
					for (int i = -2; i <= 2; i++)
					{
						Vector3 tip = point + new Vector3(i * .27f, .58f + t * .55f - Mathf.Abs(i) * .12f);
						Vector3 bend = Vector3.Lerp(point, tip, .55f) + side * .15f;
						Line(point, bend, .082f, color);
						Line(bend, tip, .04f, color);
					}
					break;
				case 5: // Earth: an expanding broken mineral seal.
					for (int i = 0; i < 4; i++)
					{
						Vector3 a = Direction(.78f + i * Mathf.PI / 2);
						Vector3 b = Direction(.78f + (i + 1) * Mathf.PI / 2);
						float radius = .40f + t * .70f;
						Line(point + a * radius, point + Vector3.Lerp(a,b,.8f) * radius, .055f, color);
					}
					break;
				default:
					Arc(point,.42f+t*.63f,angle-1.4f+t,2.25f,13,.07f*(1-t)+.01f,color);
					Vector3 slash=Direction(angle+.75f);
					Line(point-slash*(.25f+.5f*t),point+slash*(.9f+t*.35f),.08f*(1-t),color);
					break;
			}
		}

		TextMesh CreateText(Transform parent, int order)
		{
			var obj = new GameObject("战斗飘字");
			obj.transform.SetParent(parent,false);
			var label = obj.AddComponent<TextMesh>();
			label.font = readoutFont;
			label.fontSize = 44;
			label.characterSize = .15f;
			label.anchor = TextAnchor.MiddleCenter;
			label.alignment = TextAlignment.Center;
			var renderer = obj.GetComponent<MeshRenderer>();
			renderer.sharedMaterial = readoutFont.material;
			renderer.sortingOrder = order;
			return label;
		}

		public void ShowReadout(Controller actor, string value, Color ink)
		{
			if (!actor || !scene.FieldView.IsVisible(actor.transform.position)) return;
			if (!readoutFont)
			{
				var label = actor.GetComponentInChildren<Text>(true);
				readoutFont = label && label.font ? label.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			}
			Readout slot = null;
			foreach (var item in readouts)
				if (!item.Text.gameObject.activeSelf) { slot=item; break; }
			if (slot==null)
			{
				if (readouts.Count < 12)
				{
					var text = CreateText(transform,23);
					slot = new Readout { Text=text, Shadow=CreateText(text.transform,22) };
					slot.Shadow.transform.localPosition = new Vector3(.025f,-.035f,0);
					readouts.Add(slot);
				}
				else
				{
					slot = readouts[0];
					foreach (var item in readouts) if (item.Start < slot.Start) slot=item;
				}
			}
			slot.Start=Time.time;
			slot.Origin=actor.transform.position + new Vector3((Noise()-.5f)*.3f,1.65f,-2);
			slot.Ink=ink;
			slot.Text.text=slot.Shadow.text=value;
			slot.Text.transform.position=slot.Origin;
			slot.Text.color=ink;
			slot.Shadow.color=new Color(.13f,.10f,.08f,.85f);
			slot.Text.gameObject.SetActive(true);
		}

		void AnimateReadouts()
		{
			foreach (var slot in readouts)
			{
				if (!slot.Text.gameObject.activeSelf) continue;
				float t=(Time.time-slot.Start)/.72f;
				if (t>=1 || !scene.FieldView.IsVisible(slot.Origin-Vector3.up*1.65f))
				{
					slot.Text.gameObject.SetActive(false);
					continue;
				}
				float rise=1-(1-t)*(1-t);
				slot.Text.transform.position=slot.Origin+Vector3.up*(rise*.80f);
				slot.Text.transform.localScale=Vector3.one*(1+.18f*Mathf.Sin(Mathf.PI*Mathf.Min(1,t*3)));
				Color color=slot.Ink;
				color.a=1-Mathf.SmoothStep(0,1,(t-.45f)/.55f);
				slot.Text.color=color;
				slot.Shadow.color=new Color(.13f,.10f,.08f,color.a*.85f);
			}
		}

		void LateUpdate()
		{
			AnimateReadouts();
			if (cues.Count == 0 && verts.Count == 0) return;
			verts.Clear(); uvs.Clear(); colors.Clear(); indices.Clear();
			for (int i=cues.Count-1; i>=0; i--)
			{
				Cue cue=cues[i];
				float t=(Time.time-cue.Start)/cue.Duration;
				if (t>=1 || cue.Unit.BaseController == null || !cue.Unit.VisibleToPlayer()) { Remove(i); continue; }
				if (cue.Kind == Gesture.Flight) { DrawFlight(cue,t); continue; }
				var point=cue.Kind==Gesture.Gather ? cue.Point : cue.Unit.GetPos();
				var c=cue.Color;
				float wave=Mathf.Sin(Mathf.PI*t);
				if (cue.Visual != null)
				{
					float lean=cue.Kind==Gesture.Strike ? .14f : -.12f;
					cue.Visual.localPosition=cue.Rest + (cue.Direction*lean+Vector3.up*.07f)*wave;
					cue.Visual.localRotation=cue.Rotation*Quaternion.Euler(0,0,Mathf.Sign(cue.Direction.x)*wave*lean*55);
				}
				if (cue.Kind == Gesture.Cast)
				{
					float radius=1.40f-.30f*t, spin=(Time.time-cue.Start)*2.5f;
					for (int j=0;j<6;j++)
					{
						float a=spin+j*Mathf.PI/3;
						Arc(point,radius,a,.78f,5,.032f,c);
						Vector3 tick = Direction(a);
						Line(point+tick*(radius+.03f),point+tick*(radius+.17f),.035f,c);
						float inner = j*Mathf.PI/3-spin;
						Line(point+Direction(inner)*radius*.72f, point+Direction(inner+2.094f)*radius*.72f,.016f,c);
					}
				}
				else if (cue.Kind==Gesture.Strike)
				{
					c.a=1-t;
					ImpactLines(cue,point,t,c);
				}
				else
				{
					c.a=1-t;
					Arc(point,.25f+.65f*t,t*2,Mathf.PI*2,22,.035f,c);
				}
			}
			ribbons.Clear(false);
			if (verts.Count==0) return;
			ribbons.SetVertices(verts); ribbons.SetUVs(0,uvs); ribbons.SetColors(colors);
			ribbons.SetTriangles(indices,0,true);
		}

		void OnDestroy()
		{
			while (cues.Count>0) Remove(cues.Count-1);
			if (ribbons != null) Destroy(ribbons);
			if (lightInk != null) Destroy(lightInk);
		}
	}
}
