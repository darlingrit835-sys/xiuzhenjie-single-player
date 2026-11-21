using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XiuZhenJie
{
    [Serializable] internal sealed class TalismanPoint
    {
        public int x, y;
        internal Vector2 Position => new Vector2(x, y);
        internal static TalismanPoint From(Vector2 p) => new TalismanPoint { x = Mathf.RoundToInt(p.x), y = Mathf.RoundToInt(p.y) };
    }

    [Serializable] internal sealed class TalismanStroke
    {
        public string name;
        public TalismanPoint[] points;
    }

    // The same normalized, top-down coordinate system drives the preview, touch input and server receipt.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class TalismanStrokePad : MaskableGraphic, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private static readonly Color32 Shadow = new Color32(119, 73, 44, 110);
        private static readonly Color32 Ink = new Color32(110, 34, 26, 255);
        private static readonly Color32 Vermilion = new Color32(181, 53, 26, 255);
        private static readonly Color32 Grid = new Color32(125, 67, 37, 62);
        private TalismanStroke[] pattern;
        private int index, tolerance, nextPoint;
        private Action<TalismanStroke> commit;
        private Action<string> hint;
        private TalismanPoint[] crossings;
        private Vector2 previous;
        private int? pen;

        internal void Setup(TalismanStroke[] strokes, int completed, int radius, Action<TalismanStroke> finished, Action<string> guidance)
        {
            pattern = strokes; index = completed; tolerance = Mathf.Max(1, radius);
            commit = finished; hint = guidance; pen = null; crossings = null;
            raycastTarget = commit != null;
            SetVerticesDirty();
        }

        private float Scale => Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) * .0092f;
        private Vector2 OnPaper(Vector2 point) => rectTransform.rect.center + new Vector2(point.x - 50, 50 - point.y) * Scale;
        private bool ReadPoint(PointerEventData e, out Vector2 point)
        {
            point = Vector2.zero;
            if (Scale <= 0 || !RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, e.position, e.pressEventCamera, out var local)) return false;
            Vector2 delta = (local - rectTransform.rect.center) / Scale;
            point = new Vector2(50 + delta.x, 50 - delta.y);
            return true;
        }

        public void OnPointerDown(PointerEventData e)
        {
            if (pen.HasValue || commit == null || index >= pattern.Length || !ReadPoint(e, out var point)) return;
            var path = pattern[index].points;
            if (Vector2.Distance(point, path[0].Position) > tolerance || Vector2.Distance(Sample(point).Position, path[0].Position) > tolerance) { hint?.Invoke("从朱点起笔，沿红线写到笔尾。"); return; }
            pen = e.pointerId; previous = point; nextPoint = 1;
            crossings = new TalismanPoint[path.Length]; crossings[0] = Sample(point);
            e.useDragThreshold = false;
            SetVerticesDirty();
        }

        public void OnDrag(PointerEventData e)
        {
            if (pen == e.pointerId && ReadPoint(e, out var point)) Move(point);
        }

        private static TalismanPoint Sample(Vector2 p) => TalismanPoint.From(new Vector2(Mathf.Clamp(p.x, 0, 100), Mathf.Clamp(p.y, 0, 100)));

        private void Move(Vector2 end)
        {
            var path = pattern[index].points;
            Vector2 start = previous;
            while (nextPoint < path.Length)
            {
                var target = path[nextPoint].Position;
                Vector2 segment = end - start;
                float fraction = segment.sqrMagnitude < .001f ? 0f : Mathf.Clamp01(Vector2.Dot(target - start, segment) / segment.sqrMagnitude);
                var nearest = start + fraction * segment;
                // Compare the actual quantized sample that the server will receive.
                var sample = Sample(nearest);
                if (Vector2.Distance(target, nearest) > tolerance || Vector2.Distance(target, sample.Position) > tolerance) break;
                crossings[nextPoint++] = sample;
                start = nearest;
            }
            previous = end;
            SetVerticesDirty();
        }

        public void OnPointerUp(PointerEventData e)
        {
            if (pen != e.pointerId) return;
            if (ReadPoint(e, out var end)) Move(end);
            var stroke = pattern[index];
            var samples = crossings;
            pen = null; crossings = null;
            SetVerticesDirty();
            if (nextPoint == stroke.points.Length) commit?.Invoke(new TalismanStroke { name = stroke.name, points = samples });
            else hint?.Invoke("这一笔还没写到笔尾，可重描这一笔或点「辅笔」。");
        }

        private static readonly Color32 GoldLight = new Color32(255, 225, 133, 255);
        private static readonly Color32 Ember = new Color32(243, 122, 32, 95);
        private float shimmerAt;

        private void Update()
        {
            if (commit == null || Time.unscaledTime < shimmerAt) return;
            shimmerAt=Time.unscaledTime+.05f;SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (pattern == null || Scale <= 0) return;
            float nib=Mathf.Max(1.75f,Scale*1.55f);
            var center=OnPaper(new Vector2(50,50));
            Ring(mesh,center,Scale*43,.72f,Grid,64);Ring(mesh,center,Scale*39.3f,.50f,Grid,56);
            for(int i=0;i<32;i++)
            {
                float angle=i*Mathf.PI/16f;var radial=new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
                bool cardinal=i%4==0;
                Line(mesh,center+radial*Scale*(cardinal?36.0f:40.5f),center+radial*Scale*42.8f,cardinal?1.15f:.65f,.65f,Grid);
            }
            for(int i=0;i<pattern.Length;i++)
            {
                var points=pattern[i].points;
                bool active=commit!=null&&i==index,done=i<index;
                for(int j=1;j<points.Length;j++)
                {
                    var a=OnPaper(points[j-1].Position);var b=OnPaper(points[j].Position);
                    var edge=(b-a).normalized;var perp=new Vector2(-edge.y,edge.x);
                    float w=(done?2.05f:active?1.61f:.82f)*nib;
                    float tail=w*(j==points.Length-1?.22f:.83f);
                    if(done||active)
                    {
                        Line(mesh,a,b,w*3.5f,tail*3.2f,new Color32(241,145,36,35));
                        Line(mesh,a,b,w*2.15f,tail*2.3f,Ember);
                        Line(mesh,a-perp*.7f,b-perp*.7f,w*1.16f,tail*1.1f,Ink);
                    }
                    Line(mesh,a,b,w,tail,done?Vermilion:active?new Color32(188,86,32,255):Shadow);
                    if(done||active) Line(mesh,a+perp*w*.23f,b+perp*tail*.23f,w*.26f,tail*.1f,GoldLight);
                    if(j<points.Length-1) Dot(mesh,b,tail*.52f,done?Vermilion:active?new Color32(186,88,34,255):Shadow);
                }
                if(active)
                {
                    var origin=OnPaper(points[0].Position);float pulse=1f+.13f*Mathf.Sin(Time.unscaledTime*3f);
                    Dot(mesh,origin,nib*3.7f*pulse,new Color32(255,150,28,45));
                    Ring(mesh,origin,nib*2.45f,nib*.34f,Ink,28);
                    Dot(mesh,origin,nib*1.72f,Vermilion);Dot(mesh,origin,nib*.74f,GoldLight);
                    // The glint shows direction only; input has no time window.
                    float travel=Mathf.Repeat(Time.unscaledTime*.45f,1f)*(points.Length-1);
                    int segment=Mathf.Min(Mathf.FloorToInt(travel),points.Length-2);
                    var head=OnPaper(Vector2.Lerp(points[segment].Position,points[segment+1].Position,travel-segment));
                    Dot(mesh,head,nib*2f,new Color32(255,160,38,50));Dot(mesh,head,nib*.65f,GoldLight);
                }
            }
            if(crossings==null||!pen.HasValue) return;
            for(int i=1;i<nextPoint;i++) Line(mesh,OnPaper(crossings[i-1].Position),OnPaper(crossings[i].Position),nib*1.8f,nib*1.1f,Ink);
            var tip=OnPaper(previous);Line(mesh,OnPaper(crossings[nextPoint-1].Position),tip,nib*1.75f,nib*.75f,Ink);
            Dot(mesh,tip,nib*1.45f,GoldLight);
        }

        private static void Ring(VertexHelper mesh,Vector2 center,float radius,float width,Color32 ink,int sides)
        {
            var last=center+Vector2.right*radius;
            for(int i=1;i<=sides;i++)
            {
                float a=2*Mathf.PI*i/sides;
                var point=center+new Vector2(Mathf.Cos(a),Mathf.Sin(a))*radius;
                Line(mesh,last,point,width,width,ink);last=point;
            }
        }

        private static void Line(VertexHelper mesh, Vector2 a, Vector2 b, float widthA, float widthB, Color32 tint)
        {
            var axis = (b - a).normalized;
            var side = new Vector2(-axis.y, axis.x) * .5f;
            int start = mesh.currentVertCount;
            mesh.AddVert(a - widthA * side, tint, Vector2.zero); mesh.AddVert(a + widthA * side, tint, Vector2.zero);
            mesh.AddVert(b + widthB * side, tint, Vector2.zero); mesh.AddVert(b - widthB * side, tint, Vector2.zero);
            mesh.AddTriangle(start, start + 1, start + 2); mesh.AddTriangle(start, start + 2, start + 3);
        }

        private static void Dot(VertexHelper mesh, Vector2 center, float radius, Color32 tint)
        {
            int start = mesh.currentVertCount;
            mesh.AddVert(center, tint, Vector2.zero);
            for (int i = 0; i <= 12; i++)
            {
                float angle = i * Mathf.PI / 6;
                mesh.AddVert(center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius, tint, Vector2.zero);
                if (i > 0) mesh.AddTriangle(start, start + i, start + i + 1);
            }
        }
    }
}
