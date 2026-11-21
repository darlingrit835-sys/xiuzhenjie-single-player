using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Soft, moving flecks are drawn in one canvas mesh independently of the
    // illustrated four-frame actions. Neither layer participates in hit testing.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class SectCourtLight : MaskableGraphic
    {
        private static readonly Dictionary<string,Sprite[]> Clips = new Dictionary<string,Sprite[]>();
        private Image cel;
        private Sprite[] frames;
        private string kind;
        private int grade, last = -1;
        private float nextTick, elapsed, onset;
        private Color lightTint;

        internal static SectCourtLight Mount(RectTransform parent, string key, int stage, Color accent)
        {
            var node = RoleFolio.Box(parent,"山岚流光");
            RoleFolio.Fill(node);
            var effect=node.gameObject.AddComponent<SectCourtLight>();
            effect.raycastTarget=false; effect.kind=key; effect.grade=stage;
            effect.lightTint = key == "mine" ? new Color(.56f,1f,.87f) : key == "tide" ? new Color(.93f,.68f,.49f) :
                key == "treasury" ? new Color(1f,.83f,.42f) : Color.Lerp(accent,new Color(1f,.93f,.68f),.67f);
            if (key=="mine" || key=="treasury" || key=="tide")
            {
                if (!Clips.TryGetValue(key,out effect.frames))
                {
                    var sequence=new Sprite[4];
                    for (int i=0;i<sequence.Length;i++)
                        sequence[i]=ReleasedResources.Load<Sprite>("SectCourt/fx/"+key+i);
                    if (System.Array.TrueForAll(sequence,s => s!=null))
                        Clips[key]=effect.frames=sequence;
                }
                if (effect.frames!=null)
                {
                    effect.cel=RoleFolio.Image(node,"逐帧 · "+key,Color.white);
                    var rect=effect.cel.rectTransform;
                    rect.anchorMin=new Vector2(key=="tide" ? .35f : .48f,.12f);
                    rect.anchorMax=new Vector2(.95f,.92f);
                    rect.offsetMin=rect.offsetMax=Vector2.zero;
                    effect.cel.preserveAspect=true;
                }
            }
            effect.Restart();
            return effect;
        }

        internal void Restart()
        {
            onset=Time.unscaledTime; nextTick=0; last=-1;
            Advance();
        }

        private void Update()
        {
            if (Time.unscaledTime<nextTick) return;
            Advance();
        }

        private void Advance()
        {
            elapsed=Time.unscaledTime-onset;
            nextTick=Time.unscaledTime+.055f;
            if (cel!=null)
            {
                float cycle=kind=="treasury" ? 4.3f : 3.15f;
                float phase=Mathf.Repeat(elapsed,cycle);
                int frame=Mathf.Min(3,(int)(phase*5f));
                if (kind=="mine") frame=(int)Mathf.PingPong(elapsed*3.5f,3.99f);
                if (last!=frame) { cel.sprite=frames[frame]; last=frame; }
                float alpha=kind=="mine" ? .85f : Mathf.Clamp01((1.25f-phase)*3f+.08f);
                cel.color=new Color(1,1,1,alpha);
                cel.rectTransform.localScale=Vector3.one*(1f+.033f*Mathf.Sin(phase*4.4f));
            }
            SetVerticesDirty();
        }

        private static float Seed(int n) => Mathf.Repeat(n*.61803399f,1f);

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            Rect r=GetPixelAdjustedRect();
            if (r.width<=0 || r.height<=0) return;
            int count=8+grade*2;
            for (int i=0;i<count;i++)
            {
                float seed=Seed(i+1);
                float x=Mathf.Repeat(seed+elapsed*(.006f+.006f*Seed(i+5)),1);
                float y=Mathf.Repeat(Seed(i*3+8)+elapsed*(.014f+.017f*seed),1);
                float phase=(elapsed*.8f+seed*8f+i);
                var center=new Vector2(r.xMin+x*r.width,r.yMin+(.12f+.83f*y)*r.height);
                float radius=(1.1f+2.9f*seed)*(1+.21f*Mathf.Sin(phase));
                var tint=lightTint;
                tint.a=.20f+.41f*Mathf.Sin(Mathf.PI*y)*(.5f+.5f*Mathf.Sin(phase));
                int v=mesh.currentVertCount;
                mesh.AddVert(center,tint,Vector2.zero);
                tint.a=0;
                for (int k=0;k<8;k++)
                {
                    float angle=k*Mathf.PI/4;
                    var delta=new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
                    mesh.AddVert(center+delta*radius,tint,Vector2.zero);
                }
                for (int k=0;k<8;k++) mesh.AddTriangle(v,v+1+k,v+1+(k+1)%8);
            }
        }
    }
}
