using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // The full scene, authentic item sprite and foreground lettering remain still.
    // Only this small mesh ticks, independently of game messages.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class AuctionDaisLight : MaskableGraphic
    {
        internal int Rank;
        private float tick, phase;
        private void Update()
        {
            if(Time.unscaledTime<tick)return;
            tick=Time.unscaledTime+.1f; phase=Time.unscaledTime*.45f;SetVerticesDirty();
        }

        private static void Quad(VertexHelper mesh,Vector2 a,Vector2 b,Vector2 c,Vector2 d,Color ca,Color cb)
        {
            int i=mesh.currentVertCount;
            mesh.AddVert(a,ca,Vector2.zero);mesh.AddVert(b,ca,Vector2.zero);
            mesh.AddVert(c,cb,Vector2.zero);mesh.AddVert(d,cb,Vector2.zero);
            mesh.AddTriangle(i,i+1,i+2);mesh.AddTriangle(i,i+2,i+3);
        }
        private static Vector2 Dir(float a)=>new Vector2(Mathf.Cos(a),Mathf.Sin(a));
        private static void Ring(VertexHelper mesh,Vector2 center,float radius,float width,Color inner,Color outer)
        {
            for(int i=0;i<64;i++)
            {
                var a=Dir(i*Mathf.PI/32);var b=Dir((i+1)*Mathf.PI/32);
                Quad(mesh,center+a*radius,center+b*radius,center+b*(radius+width),center+a*(radius+width),inner,outer);
            }
        }
        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();Rect rect=GetPixelAdjustedRect(); float r=Mathf.Min(rect.width,rect.height)*.40f;var center=rect.center;
            if(r<1)return;
            Color gold=new Color32(245,197,106,238),mist=new Color32(255,225,137,0);
            Quad(mesh,center+new Vector2(-r*.40f,0),center+new Vector2(r*.40f,0),
                center+new Vector2(r*.85f,r*1.82f),center+new Vector2(-r*.85f,r*1.82f),new Color32(255,221,152,65),mist);
            Color aura=Rank>=4 ? new Color32(255,190,91,110) : Rank==3 ? new Color32(212,169,244,100) :
                Rank==2 ? new Color32(158,228,194,100) : new Color32(241,217,157,85);
            float breath=.5f+.5f*Mathf.Sin(phase*3f);
            Ring(mesh,center,0,r,new Color32(74,73,60,84),new Color32(35,45,52,112));
            Ring(mesh,center,r*.93f,r*.025f,gold,gold);
            Ring(mesh,center,r,r*.048f,new Color32(146,99,42,248),gold);
            Ring(mesh,center,r*1.045f,r*(.22f+.12f*breath),aura,new Color(aura.r,aura.g,aura.b,0));
            for(int i=0;i<32;i++)
            {
                var dir=Dir(i*Mathf.PI/16);var tangent=new Vector2(-dir.y,dir.x);
                var head=center+dir*r*1.012f; var tail=head+dir*(i%4==0 ? r*.15f:r*.054f);
                Quad(mesh,head-tangent*.6f,head+tangent*.6f,tail+tangent*.25f,tail-tangent*.25f,gold,gold);
            }
            for(int i=0;i<8;i++)
            {
                float a=phase*(i%2==0 ? .48f:-.28f)+i*2.39996f;
                var d=Dir(a);var c=center+d*r*(1.16f+.14f*Mathf.Sin(i*2.2f+phase));
                float w=1.6f+i%3;var spark=new Color(1,.9f,.64f,.56f+.4f*Mathf.Sin(phase*2.3f+i));
                Quad(mesh,c+Vector2.up*w,c+Vector2.right*.65f,c+Vector2.down*w,c+Vector2.left*.65f,spark,spark);
            }
        }
    }
}
