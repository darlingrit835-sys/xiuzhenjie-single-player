using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace XiuZhenJie
{
    // One transparent mesh for the live atlas. The scene's original transforms and colliders remain the source of position.
    internal sealed class InkMeridianSpectrum : MonoBehaviour
    {
        private struct Point
        {
            internal JingMaiItem item;
            internal AcupointModel data;
            internal Vector2 pos;
            internal Color ink;
        }
        private struct Link { internal int a, b; internal Vector2 bend; }
        private static readonly Color Gold = new Color32(255, 213, 128, 255);
        private static readonly Color Jade = new Color32(103, 243, 207, 255);
        private readonly List<Point> points = new List<Point>(64);
        private readonly List<Link> links = new List<Link>(100);
        private readonly Dictionary<string,int> index = new Dictionary<string,int>();
        private readonly List<Vector3> vertices = new List<Vector3>(24000);
        private readonly List<Color32> colors = new List<Color32>(24000);
        private readonly List<int> triangles = new List<int>(42000);
        private Mesh mesh;
        private Material material, spiritVeil;
        private SpriteRenderer sky;
        private Vector2 skySize;
        private int effectPoint;
        private JingMaiManager map;
        private int focus, nextChoice;
        internal string Thread { get; private set; }
        private float phaseTime, releaseTime, nextFrame, sizeTarget;
        private bool zoom, easing, success;
        private Vector2 cameraGoal;
        private int phase; // 1 gather, 2 resolved. Idle still carries light along the channels.
        private readonly Vector2[] constellations = new Vector2[40];

        internal static InkMeridianSpectrum Create(JingMaiManager map, Dictionary<string,JingMaiItem> items, Dictionary<string,AcupointModel> models)
        {
            var root = new GameObject("奇经八脉 · 行气", typeof(MeshFilter), typeof(MeshRenderer), typeof(InkMeridianSpectrum));
            root.layer = map.PeopleXueWeiBg.gameObject.layer;
            root.transform.SetParent(map.PeopleXueWeiBg, false);
            var atlas = root.GetComponent<InkMeridianSpectrum>();
            atlas.map = map;
            atlas.Build(items, models);
            return atlas;
        }

        private void Build(Dictionary<string,JingMaiItem> items, Dictionary<string,AcupointModel> models)
        {
            foreach (var pair in items) {
                if (!models.TryGetValue(pair.Key, out var data)) continue;
                var item = pair.Value;
                index.Add(pair.Key, points.Count);
                points.Add(new Point { item=item, data=data, pos=item.transform.localPosition, ink=Element(data.c) });
                item.ItemImg.enabled = false;
                item.MaskImg.enabled = false;
                item.ItemText.GetComponent<MeshRenderer>().enabled = false;
            }
            for (int i = 0; i < points.Count; i++) {
                string children = points[i].data.h;
                if (string.IsNullOrEmpty(children)) continue;
                foreach (var child in children.Split(',')) {
                    if (!index.TryGetValue(child.Trim(), out int j)) continue;
                    Vector2 a = points[i].pos, b = points[j].pos, dir = b-a;
                    links.Add(new Link { a=i, b=j, bend=(a+b)*.5f + Vector2.Perpendicular(dir).normalized * Mathf.Min(.13f,dir.magnitude*.18f) });
                }
            }
            var body = map.PeopleXueWeiBg.GetComponent<SpriteRenderer>();
            var cosmos = InkCultivationInterface.Get("MeridianCosmos");
            if (cosmos != null) {
                sky = new GameObject("天外星河", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                sky.gameObject.layer = gameObject.layer;
                sky.transform.SetParent(transform, false);
                sky.sprite = cosmos;
                sky.sortingLayerID = body.sortingLayerID;
                sky.sortingOrder = body.sortingOrder - 2;
                skySize = cosmos.bounds.size;
            }
            var painting = InkCultivationInterface.Get("MeridianSpirit");
            if (painting != null) {
                var image = new GameObject("灵身绢绘", typeof(SpriteRenderer));
                image.layer = gameObject.layer;
                image.transform.SetParent(transform, false);
                image.transform.localPosition = new Vector3(0,.214f,.03f);
                image.transform.localScale = Vector3.one * (10.433f / painting.bounds.size.x);
                var renderer = image.GetComponent<SpriteRenderer>();
                renderer.sprite = painting;
                var veil = ReleasedResources.Load<Shader>("shaders/MeridianVeil");
                if (veil != null) {
                    spiritVeil = new Material(veil);
                    renderer.sharedMaterial = spiritVeil;
                }
                renderer.sortingLayerID = body.sortingLayerID;
                renderer.sortingOrder = body.sortingOrder;
                body.enabled = false;
            }
            mesh = new Mesh { name = "经络、星轨及穴位" };
            mesh.MarkDynamic();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            var draw = GetComponent<MeshRenderer>();
            material = new Material(body.sharedMaterial) { name = "经络灵光" };
            material.mainTexture = Texture2D.whiteTexture;
            draw.sharedMaterial = material;
            draw.sortingLayerID = body.sortingLayerID;
            draw.sortingOrder = body.sortingOrder+8;
            draw.shadowCastingMode = ShadowCastingMode.Off;
            draw.receiveShadows = false;
            var rng = new System.Random(802);
            for (int i=0;i<constellations.Length;i++)
                constellations[i] = new Vector2((float)(rng.NextDouble()*10-5), (float)(rng.NextDouble()*17-8.5));
            index.TryGetValue("1",out focus);
            map.SceneCamera.backgroundColor = new Color32(9, 26, 31, 255);
            map.SceneCamera.clearFlags = CameraClearFlags.SolidColor;
            // The first-time guide uses the original near view for its first target.
            bool guide = Singleton<RoleManager>.Instance.Role_Entity?.AcupointGuideVal < 100;
            SetView(guide, true);
        }

        private static Color Element(string e)
        {
            switch (e) {
                case "金": return Gold;
                case "木": return new Color32(143,236,168,255);
                case "水": return new Color32(117,213,255,255);
                case "火": return new Color32(255,148,109,255);
                case "土": return new Color32(237,186,112,255);
                default: return Jade;
            }
        }

        internal string NameOf(string id) => index.TryGetValue(id,out int i) ? points[i].data.b : null;
        internal AcupointModel Focused => points.Count==0 ? null : points[focus].data;
        internal JingMaiItem FocusedItem => points.Count==0 ? null : points[focus].item;
        internal int OpenedCount {
            get { int n=0; foreach(var p in points) if(p.item.CurrentStatus==XueWeiStatus.Used) n++; return n; }
        }
        internal int PointCount => points.Count;
        internal bool IsCloseView => zoom;
        internal int CountInThread(string thread, bool openedOnly) {
            int count=0;
            foreach(var p in points)
                if (p.data.e != null && p.data.e.Contains(thread) && (!openedOnly || p.item.CurrentStatus==XueWeiStatus.Used)) count++;
            return count;
        }

        internal void Focus(string id)
        {
            if (!index.TryGetValue(id, out int target)) return;
            focus=target;
            if (zoom) SetView(true);
        }

        internal void FollowThread(string thread)
        {
            if(points.Count==0) return;
            Thread=thread;
            // Cycle within the selected meridian; prefer actionable points before locked ones.
            for(int pass=0;pass<2;pass++) for(int n=0;n<points.Count;n++) {
                int i=(nextChoice+n)%points.Count;
                var point=points[i];
                if (point.data.e==null || !point.data.e.Contains(thread) || (pass==0 && point.item.CurrentStatus==XueWeiStatus.Disable)) continue;
                focus=i; nextChoice=(i+1)%points.Count; SetView(true); return;
            }
        }

        internal void SetView(bool near,bool immediate=false)
        {
            zoom=near;
            if(!near) Thread=null;
            float aspect=Mathf.Max(.3f,map.SceneCamera.aspect);
            sizeTarget=near ? Mathf.Max(1.55f,1.30f/aspect) : Mathf.Max(5.6f,4.42f/aspect);
            Vector3 center=map.PeopleXueWeiBg.position;
            cameraGoal=(Vector2)center + (near && points.Count>0 ? points[focus].pos : new Vector2(0,.20f));
            easing=true;
            if(immediate) {
                var camera=map.SceneCamera;
                camera.orthographicSize=sizeTarget;
                camera.transform.position=new Vector3(cameraGoal.x,cameraGoal.y,camera.transform.position.z);
                easing=false;
            }
        }

        internal void CancelTravel() { easing=false; }
        internal void Charge(string id)
        {
            Focus(id);
            effectPoint=focus;
            phase=1;
            phaseTime=Time.unscaledTime;
        }
        internal void Resolve(string id,bool won)
        {
            if (phase!=1 || !index.TryGetValue(id,out int picked) || picked!=effectPoint) Charge(id);
            success=won;
            releaseTime=Mathf.Max(phaseTime+.48f,Time.unscaledTime);
            phase=2;
        }
        internal void StopCharge() { phase=0; }

        internal JingMaiItem Nearest(Vector2 touch)
        {
            float range=Mathf.Clamp(Screen.width*.031f,12f,32f), best=range*range;
            JingMaiItem found=null;
            foreach (var p in points) {
                Vector2 pos=map.SceneCamera.WorldToScreenPoint(p.item.transform.position);
                float dist=(pos-touch).sqrMagnitude;
                if(dist<best) { best=dist; found=p.item; }
            }
            return found;
        }

        private void LateUpdate()
        {
            if (mesh==null || !map || !map.SceneCamera || points.Count==0) return;
            if(easing) {
                Camera cam=map.SceneCamera;
                float mix=1-Mathf.Exp(-10*Time.unscaledDeltaTime);
                cam.orthographicSize=Mathf.Lerp(cam.orthographicSize,sizeTarget,mix);
                var p=cam.transform.position;
                cam.transform.position=Vector3.Lerp(p,new Vector3(cameraGoal.x,cameraGoal.y,p.z),mix);
                if (Mathf.Abs(cam.orthographicSize-sizeTarget)<.003f && ((Vector2)p-cameraGoal).sqrMagnitude<.0001f) easing=false;
            }
            if (sky != null) {
                var cam=map.SceneCamera;
                var view=cam.transform.position;
                var center=map.PeopleXueWeiBg.position;
                var scale=Mathf.Max(2*cam.orthographicSize*cam.aspect/skySize.x,2*cam.orthographicSize/skySize.y)*1.10f;
                sky.transform.localScale=Vector3.one*scale;
                // Distant stars drift a fraction of the chart movement when the player pans.
                sky.transform.position=new Vector3(Mathf.Lerp(view.x,center.x,.025f),Mathf.Lerp(view.y,center.y,.025f),center.z+.45f);
            }
            float time=Time.unscaledTime;
            if(time<nextFrame) return;
            nextFrame=time+1f/30f;
            vertices.Clear(); colors.Clear(); triangles.Clear();
            float unit=Mathf.Clamp(map.SceneCamera.orthographicSize*.013f,.035f,.13f);
            for(int i=0;i<constellations.Length;i++) {
                var s=constellations[i];
                s.y+=Mathf.Sin(time*.2f+i)*.16f;
                float glow=.42f+.28f*Mathf.Sin(time*1.1f+i*1.9f);
                Disc(s,unit*(.20f+(i%4)*.06f),Alpha(i%4==0?Gold:Jade,glow),6);
            }
            var origin=new Vector2(0,.08f);
            Arc(origin,4.44f,.006f,time*.06f,Mathf.PI*1.62f,Alpha(Gold,.40f),94);
            Arc(origin,4.53f,.014f,-time*.04f,Mathf.PI*.68f,Alpha(Jade,.31f),44);
            for(int i=0;i<48;i++) {
                float a=i*Mathf.PI/24+time*.045f;
                var dir=Rad(a);
                Beam(origin+dir*4.47f,origin+dir*(i%4==0?4.61f:4.53f),.008f,Alpha(Gold,.52f));
            }
            foreach(var link in links) {
                Point p=points[link.a], q=points[link.b];
                if (Thread!=null && !p.data.e.Contains(Thread) && !q.data.e.Contains(Thread)) continue;
                bool lit=p.item.CurrentStatus==XueWeiStatus.Used, selected=link.a==focus||link.b==focus;
                Color c=lit ? Color.Lerp(p.ink,q.ink,.5f) : selected ? Gold : Color.Lerp(p.ink,Jade,.48f);
                Vector2 last=p.pos;
                for(int s=1;s<=6;s++) {
                    Vector2 pos=Curve(p.pos,link.bend,q.pos,s/6f);
                    Beam(last,pos,unit*(lit?.48f:.33f),Alpha(new Color32(4,29,35,255),lit?.65f:.60f));
                    Beam(last,pos,unit*(lit?.24f:.145f),Alpha(c,lit?.96f:selected?.92f:.72f));
                    last=pos;
                }
                if(lit || selected) {
                    float t=Mathf.Repeat(time*(lit?.55f:.36f)+link.a*.217f,1);
                    var pos=Curve(p.pos,link.bend,q.pos,t);
                    Disc(pos,unit*.48f,Alpha(c,.78f),8); Disc(pos,unit*.16f,Color.white,6);
                }
            }
            for(int i=0;i<points.Count;i++) {
                Point p=points[i]; bool open=p.item.CurrentStatus==XueWeiStatus.Used, available=p.item.CurrentStatus==XueWeiStatus.Usable;
                Color c=open?p.ink:available?Gold:Color.Lerp(p.ink,Jade,.50f);
                float pulse=.5f+.5f*Mathf.Sin(time*2.3f+i);
                if(open||available||focus==i) {
                    Disc(p.pos,unit*(1.54f+.35f*pulse),Alpha(c,.42f),12);
                    Ring(p.pos,unit*(.66f+.12f*pulse),unit*.065f,Alpha(c,.84f),14);
                }
                // A dark rim keeps the clickable stars legible on both gold clouds and illuminated fabric.
                Disc(p.pos,unit*(open?1.14f:.88f),new Color32(3,23,32,247),10);
                if(!open && !available) Ring(p.pos,unit*.48f,unit*.16f,Alpha(c,.89f),10);
                Disc(p.pos,unit*(open?.91f:available?.84f:.49f),c,10);
                if(available||open) Disc(p.pos,unit*.35f,Color.white,8);
                if(open) Star(p.pos,unit,Alpha(c,.89f));
                if(i==focus) {
                    Arc(p.pos,unit*1.7f,unit*.115f,time*1.2f,4.92f,Gold,25);
                    Arc(p.pos,unit*2.03f,unit*.065f,-time*.8f,3.78f,Jade,22);
                }
            }
            if(phase!=0) DrawBurst(time,unit);
            mesh.Clear(false);
            mesh.SetVertices(vertices);mesh.SetColors(colors);mesh.SetTriangles(triangles,0,false);
            mesh.bounds=new Bounds(Vector3.zero,new Vector3(30,40,2));
        }

        private void DrawBurst(float time,float unit)
        {
            bool charging=phase==1 || time<releaseTime;
            float age=time-(charging ? phaseTime : releaseTime);
            Vector2 center=points[effectPoint].pos;
            if(!charging && age>2.15f) { phase=0; return; }
            if(charging && age>10f) { phase=0; return; }
            Color tint=charging?Jade:success?Gold:new Color32(250,137,116,255);
            float fade=charging?1:1-Mathf.Clamp01(age/2.15f);
            for(int i=0;i<4;i++) {
                float f=charging ? 1-Mathf.Repeat(age*.9f+i*.25f,1):Mathf.Clamp01(age*(.75f+i*.18f));
                Arc(center,unit*(1+f*10),unit*.16f,time*(i%2==0?1:-1)+i,Mathf.PI*(1.35f+i*.12f),Alpha(tint,fade*.86f),44);
            }
            for(int i=0;i<26;i++) {
                float a=i*2.399963f+(charging?age:-.22f*age);
                float reach=charging ? 1-Mathf.Repeat(age*.87f+i*.17f,1) : Mathf.Min(1,age*(.78f+(i%7)*.12f));
                var pos=center+Rad(a)*unit*(1+reach*(5+i%10));
                Star(pos,unit*(.3f+.6f*fade),Alpha(i%3==0?Jade:tint,fade));
            }
            Disc(center,unit*(3.5f+fade),Alpha(tint,fade*.55f),24);
            if(charging) {
                foreach (var link in links) {
                    if (link.a!=effectPoint && link.b!=effectPoint) continue;
                    var near=points[link.a==effectPoint ? link.b : link.a];
                    float flow=Mathf.Repeat(age*2.1f,1);
                    Star(Curve(near.pos,link.bend,center,flow),unit*1.65f,Alpha(Jade,.98f));
                }
            }
            if(!charging && success) {
                DrawSkyRipple(center,age,unit);
                Star(center,unit*(3+fade*3),Alpha(Color.white,fade));
                foreach(var link in links) {
                    if(link.a!=effectPoint && link.b!=effectPoint) continue;
                    var neighbour=points[link.a==effectPoint ? link.b : link.a];
                    float front=Mathf.Clamp01(age/.68f);
                    for(int i=0;i<8;i++) {
                        float t=front-i*.032f;
                        if(t<=0 || (front==1 && age>.85f)) continue;
                        var pos=Curve(center,link.bend,neighbour.pos,t);
                        Disc(pos,unit*(.95f-i*.085f),Alpha(neighbour.ink,fade*(1-i/9f)),10);
                        if(i==0) Star(pos,unit*1.55f,Alpha(Color.white,fade));
                    }
                    if(age>=.63f) {
                        float echo=Mathf.Clamp01((age-.63f)/.92f);
                        Ring(neighbour.pos,unit*(.7f+3.1f*echo),unit*.13f,Alpha(neighbour.ink,1-echo),26);
                        Star(neighbour.pos,unit*(2.8f-1.8f*echo),Alpha(Gold,1-echo));
                    }
                }
            }
        }

        // Two expanding orbital planes cross the body and fade back into the star field.
        private void DrawSkyRipple(Vector2 origin,float age,float unit)
        {
            for(int band=0;band<2;band++) {
                float t=Mathf.Clamp01((age-.16f-band*.16f)/1.55f);
                if(t<=0 || t>=1) continue;
                float radius=Mathf.Lerp(.11f,3.95f,1-(1-t)*(1-t));
                float brightness=Mathf.Sin(Mathf.PI*Mathf.Pow(t,.55f));
                Color tint=band==0?Gold:Jade;
                float angle=band==0?.42f:-.47f;
                Vector2 axis=Rad(angle), across=Vector2.Perpendicular(axis);
                Vector2 previous=origin+radius*axis;
                for(int i=1;i<=96;i++) {
                    float a=i*(Mathf.PI*2/96);
                    Vector2 p=origin+radius*(axis*Mathf.Cos(a)+across*(.42f*Mathf.Sin(a)));
                    float light=brightness*(.63f+.34f*Mathf.Cos(a+age*2));
                    Beam(previous,p,unit*.65f,Alpha(tint,light*.25f));
                    Beam(previous,p,unit*.20f,Alpha(tint,light));
                    if(i%8==0) Star(p,unit*(1.18f+light),Alpha(tint,light));
                    previous=p;
                }
            }
        }

        private static Vector2 Rad(float a) => new Vector2(Mathf.Cos(a),Mathf.Sin(a));
        private static Color Alpha(Color c,float a) { c.a=a; return c; }
        private static Vector2 Curve(Vector2 a,Vector2 b,Vector2 c,float t) { float u=1-t; return u*u*a+2*u*t*b+t*t*c; }
        private void V(Vector2 p, Color c) { vertices.Add(new Vector3(p.x,p.y,-.055f)); colors.Add(c); }
        private void Tri(int a,int b,int c) { triangles.Add(a);triangles.Add(b);triangles.Add(c); }
        private void Beam(Vector2 a,Vector2 b,float width,Color c)
        {
            var n=Vector2.Perpendicular(b-a).normalized * (width*.5f); int i=vertices.Count;
            V(a-n,c); V(a+n,c); V(b+n,c); V(b-n,c); Tri(i,i+1,i+2);Tri(i,i+2,i+3);
        }
        private void Disc(Vector2 c,float r,Color tint,int steps)
        {
            int start=vertices.Count; V(c,tint); Color rim=Alpha(tint,0);
            for(int i=0;i<=steps;i++) {V(c+Rad(i*2*Mathf.PI/steps)*r,rim);if(i>0)Tri(start,start+i,start+i+1);}
        }
        private void Arc(Vector2 c,float r,float width,float a,float span,Color tint,int steps)
        {
            int start=vertices.Count;
            for(int i=0;i<=steps;i++) {
                var dir=Rad(a+span*i/steps);
                V(c+dir*(r-width*.5f),tint);V(c+dir*(r+width*.5f),tint);
                if(i>0){int v=start+2*i;Tri(v-2,v-1,v);Tri(v,v-1,v+1);}
            }
        }
        private void Ring(Vector2 c,float r,float width,Color tint,int steps) => Arc(c,r,width,0,Mathf.PI*2,tint,steps);
        private void Star(Vector2 p,float radius,Color tint)
        {
            for(int i=0;i<4;i++) {
                Vector2 d=Rad(i*Mathf.PI*.5f), n=Vector2.Perpendicular(d);
                int k=vertices.Count;V(p-n*radius*.13f,tint);V(p+d*radius,Alpha(tint,0));V(p+n*radius*.13f,tint);Tri(k,k+1,k+2);
            }
        }
        private void OnDestroy() { if(mesh) Destroy(mesh); if(material) Destroy(material); if(spiritVeil) Destroy(spiritVeil); }
    }
}
