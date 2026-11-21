using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class VoidAltarIncense : MaskableGraphic
    {
        private static readonly Color32 Amber=new Color32(255,191,91,255), Smoke=new Color32(225,230,194,255);
        private int lit;
        private float flash=-10,nextFrame,clock;
        private RectTransform burner;

        internal void Bind(RectTransform target){burner=target;}
        internal void Set(int amount,bool ignition)
        { lit=Mathf.Clamp(amount,0,8);if(ignition) flash=Time.unscaledTime;SetVerticesDirty(); }

        private void Update()
        {if(Time.unscaledTime<nextFrame)return;clock=Time.unscaledTime;nextFrame=clock+.05f;SetVerticesDirty();}

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();if(burner==null)return;
            var bounds=burner.rect;if(bounds.width<1)return;
            var rim=(Vector2)rectTransform.InverseTransformPoint(burner.TransformPoint(new Vector2(bounds.center.x,bounds.yMin+bounds.height*.65f)));
            float size=bounds.width,burst=Mathf.Clamp01(1-(clock-flash)/1.7f);
            for(int i=0;i<8;i++)
            {
                float a=2*Mathf.PI*i/8+clock*.13f,radius=size*(.40f+burst*.10f);
                var point=rim+new Vector2(Mathf.Cos(a)*radius,Mathf.Sin(a)*radius*.32f-size*.21f);
                Glow(mesh,point,size*(i<lit?.025f:.010f),Amber,i<lit?.90f:.32f);
            }
            for(int i=0;i<3;i++){
                Vector2 previous=rim;float phase=i*Mathf.PI*2/3;
                for(int n=1;n<=13;n++){
                    float t=n/13f;var point=rim+new Vector2(size*(.025f+.11f*t)*Mathf.Sin(t*7-clock*1.2f+phase),
                        size*t*(.52f+.14f*burst));
                    Strip(mesh,previous,point,size*(.006f+.018f*t),Smoke,(1-t)*(.14f+burst*.21f));previous=point;
                }
            }
            for(int i=0;i<13;i++){
                float p=Mathf.Repeat(clock*(.21f+i%3*.025f)+i*.618f,1);
                var point=rim+new Vector2(size*(.04f+.23f*p)*Mathf.Sin(8*p+i),size*p*(.40f+burst*.27f));
                Glow(mesh,point,size*(.010f+burst*.012f),Amber,Mathf.Sin(p*Mathf.PI)*(.42f+burst*.48f));
            }
            Glow(mesh,rim,size*(.19f+.14f*burst),Amber,.12f+.34f*burst+.03f*Mathf.Sin(clock*3));
        }
        private static void Glow(VertexHelper m,Vector2 v,float radius,Color color,float alpha)
        {
            int first=m.currentVertCount;color.a=alpha;m.AddVert(v,color,Vector2.zero);color.a=0;
            for(int k=0;k<=12;k++){float a=k*Mathf.PI/6;m.AddVert(v+new Vector2(Mathf.Cos(a),Mathf.Sin(a))*radius,color,Vector2.zero);if(k>0)m.AddTriangle(first,first+k,first+k+1);}
        }
        private static void Strip(VertexHelper m,Vector2 a,Vector2 b,float radius,Color color,float alpha)
        {
            var d=(b-a).normalized;var side=new Vector2(-d.y,d.x)*radius;int k=m.currentVertCount;
            color.a=alpha;m.AddVert(a-side,color,Vector2.zero);m.AddVert(a+side,color,Vector2.zero);
            m.AddVert(b+side,color,Vector2.zero);m.AddVert(b-side,color,Vector2.zero);m.AddTriangle(k,k+1,k+2);m.AddTriangle(k,k+2,k+3);
        }
    }
}
