using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class VoidRealmLight : MaskableGraphic
    {
        private RectTransform painting;
        private RawImage cel;
        private int rank, frame=-1;
        private bool studio, success;
        private float opened, burstTime=-5, elapsed, impact, nextTick, radius;
        private Vector2 center;
        private Color glow;
        private static readonly Color Warm=new Color32(255,223,149,255), JadeLight=new Color32(156,251,216,255),
            WhiteInk=new Color32(227,237,210,255), Cold=new Color32(133,187,193,255);

        internal static VoidRealmLight Mount(RectTransform parent,RectTransform painting,int level,bool studio)
        {
            var root=new GameObject("云尘与星辉",typeof(RectTransform));
            root.layer=parent.gameObject.layer; var rect=(RectTransform)root.transform;
            rect.SetParent(parent,false); rect.anchorMin=Vector2.zero;rect.anchorMax=Vector2.one;
            rect.offsetMin=rect.offsetMax=Vector2.zero;
            var effect=root.AddComponent<VoidRealmLight>(); effect.raycastTarget=false;effect.painting=painting;
            effect.rank=Mathf.Clamp(level,1,9);effect.studio=studio;effect.opened=Time.unscaledTime;
            var atlas=ReleasedResources.Load<Texture2D>("VoidRealm/seal-frames");
            if(atlas!=null)
            {
                var ink=new GameObject("十六帧星盘",typeof(RectTransform),typeof(RawImage));
                ink.layer=root.layer;ink.transform.SetParent(rect,false);
                effect.cel=ink.GetComponent<RawImage>();effect.cel.raycastTarget=false;effect.cel.texture=atlas;
            }
            return effect;
        }

        internal void Burst(bool passed,int level)
        {
            success=passed;rank=Mathf.Clamp(level,1,9);burstTime=Time.unscaledTime;nextTick=0;
        }

        private void LateUpdate()
        {
            if(Time.unscaledTime<nextTick || painting==null) return;
            nextTick=Time.unscaledTime+1f/24;
            elapsed=Time.unscaledTime-opened;
            float age=Time.unscaledTime-burstTime;impact=Mathf.Clamp01(1-age/1.45f);
            glow=impact>0 && !success ? Cold : !studio || rank>=5 ? Warm : rank>=3 ? JadeLight : WhiteInk;
            float artX=studio ? .60f : .50f, artY=studio ? .39f : .82f;
            var source=painting.rect;
            Vector3 illustrated=painting.TransformPoint(new Vector2(source.xMin+artX*source.width,source.yMin+artY*source.height));
            center=(Vector2)rectTransform.InverseTransformPoint(illustrated);
            var r=rectTransform.rect;
            radius=Mathf.Min(r.width*.18f,r.height*.29f);
            if(cel!=null)
            {
                int f=(int)(elapsed*(impact>0?10f:5f))%16;
                if(f!=frame)
                {
                    frame=f;
                    // Unity UV从底行起，原画的第一帧在左上。
                    cel.uvRect=new Rect((f%4)*.25f,(3-f/4)*.25f,.25f,.25f);
                }
                var tile=cel.rectTransform;
                tile.localPosition=new Vector3(center.x,center.y,0);
                tile.sizeDelta=Vector2.one*(radius*(2.05f+.20f*impact));
                var tint=glow;tint.a=(studio ? .57f : .49f)+impact*.3f;cel.color=tint;
            }
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();if(radius<=0) return;
            var r=rectTransform.rect;
            var pulse=glow;pulse.a=.12f+.34f*impact+.028f*Mathf.Sin(elapsed*2.2f);
            Halo(mesh,center,radius*(1.4f+impact*.4f),pulse,24);
            int rings=studio ? (rank>=5 ? 3 : rank>=3 ? 2 : 1) : 1;
            for(int i=0;i<rings;i++)
            {
                float spin=(i%2==0 ? 1 : -1)*elapsed*(.11f+.04f*i);
                var tint=glow; tint.a=(studio ? .55f : .35f)+.3f*impact;
                int spans=i==0 ? 4 : 8;
                for(int j=0;j<spans;j++)
                {
                    float a=spin+2*Mathf.PI*j/spans, size=2*Mathf.PI/spans*.71f;
                    Arc(mesh,radius*(.83f+.18f*i+impact*.08f),a,size,1f,tint);
                }
            }
            int count=studio ? 12+rank*2 : 13;
            for(int i=0;i<count;i++)
            {
                float seed=Mathf.Repeat((i+1)*.618034f,1);
                float path=Mathf.Repeat(seed+elapsed*(.06f+.025f*seed),1);
                float angle=seed*Mathf.PI*2+(i%2==0?1:-1)*elapsed*.19f;
                float distance=radius*(.52f+path*.95f+impact*.36f);
                var v=new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)*.86f);
                var t=glow;t.a=Mathf.Sin(Mathf.PI*path)*(.62f+impact*.36f);
                Halo(mesh,center+v*distance,1.2f+2*seed+impact,t,6);
            }
            // 第二层轻尘自云阶向上流动，仍然合并在一个UI网格内。
            for(int i=0;i<10;i++)
            {
                float seed=Mathf.Repeat((i+2)*.7548777f,1), travel=Mathf.Repeat(seed+elapsed*.038f,1);
                var v=new Vector2(r.xMin+r.width*Mathf.Repeat(seed*2.7f+.035f*Mathf.Sin(elapsed*.6f+i),1),
                    r.yMin+r.height*travel);
                var t=Warm;t.a=Mathf.Sin(travel*Mathf.PI)*.5f;
                Halo(mesh,v,1.1f+seed*1.6f,t,6);
            }
            if(impact>0)
            {
                var edge=glow;edge.a=.7f*impact;
                Arc(mesh,radius*(2.1f-impact),0,2*Mathf.PI,1.5f+impact*2,edge,40);
            }
        }

        private void Arc(VertexHelper mesh,float distance,float angle,float span,float width,Color tint,int steps=9)
        {
            int start=mesh.currentVertCount;
            for(int i=0;i<=steps;i++)
            {
                float a=angle+span*i/steps;var d=new Vector2(Mathf.Cos(a),Mathf.Sin(a));
                mesh.AddVert(center+d*(distance-width*.5f),tint,Vector2.zero);
                mesh.AddVert(center+d*(distance+width*.5f),tint,Vector2.zero);
                if(i==0) continue;
                int v=start+i*2;mesh.AddTriangle(v-2,v-1,v);mesh.AddTriangle(v,v-1,v+1);
            }
        }

        private static void Halo(VertexHelper mesh,Vector2 origin,float size,Color tint,int steps)
        {
            int start=mesh.currentVertCount;mesh.AddVert(origin,tint,Vector2.zero);
            tint.a=0;
            for(int j=0;j<steps;j++)
            {
                float a=2*Mathf.PI*j/steps;
                mesh.AddVert(origin+new Vector2(Mathf.Cos(a),Mathf.Sin(a))*size,tint,Vector2.zero);
            }
            for(int j=0;j<steps;j++) mesh.AddTriangle(start,start+j+1,start+1+(j+1)%steps);
        }
    }
}
