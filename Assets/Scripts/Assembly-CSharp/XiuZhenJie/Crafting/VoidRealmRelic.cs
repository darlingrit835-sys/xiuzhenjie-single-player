using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Shared canvas mesh for the cast shadow, reflected light, rim and motes.
    // Only the cutout painting uses a separate texture. Neither layer takes input.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class VoidRealmRelic : MaskableGraphic
    {
        private static readonly string[] Halls = {"dan","qi","fu","shu","zhen","dao","qiao","zhi","shou"};
        private static readonly Color32[] Pigments = {
            new Color32(255,152,77,255),new Color32(157,230,239,255),new Color32(255,180,83,255),
            new Color32(200,158,255,255),new Color32(253,211,114,255),new Color32(240,224,164,255),
            new Color32(249,190,106,255),new Color32(135,237,169,255),new Color32(173,237,230,255)};
        private static readonly Color32[] Elements = {new Color32(251,217,134,255),new Color32(138,239,174,255),
            new Color32(120,213,253,255),new Color32(255,135,89,255),new Color32(219,183,124,255)};

        private RawImage art;
        private int hall, grade, stage, qi;
        private bool reveal;
        private float opened, lastFrame, time, pulse;
        private Color pigment;

        internal static void Mount(RectTransform parent, string hallId, int grade, int element, int stage, bool reveal, int qi)
        {
            int index = System.Array.IndexOf(Halls,hallId);
            if(index<0) return;
            var texture = ReleasedResources.Load<Texture2D>("VoidRealm/profession-relics");
            if(texture==null) return;
            var fx = parent.gameObject.AddComponent<VoidRealmRelic>();
            fx.raycastTarget=false;
            fx.hall=index; fx.grade=Mathf.Clamp(grade,1,9); fx.stage=stage; fx.reveal=reveal; fx.qi=qi;
            fx.pigment=Color.Lerp(Pigments[index],Elements[Mathf.Clamp(element-1,0,4)],.29f);
            fx.opened=Time.unscaledTime;
            var sprite=new GameObject("工坊器物",typeof(RectTransform),typeof(RawImage));
            sprite.layer=parent.gameObject.layer;
            sprite.transform.SetParent(parent,false);
            fx.art=sprite.GetComponent<RawImage>();
            fx.art.raycastTarget=false; fx.art.texture=texture;
            float tile=1f/3f;
            fx.art.uvRect=new Rect((index%3)*tile+.001f,(2-index/3)*tile+.001f,tile-.002f,tile-.002f);
            fx.Animate();
        }

        private void Update()
        {
            if(canvasRenderer.cull || Time.unscaledTime<lastFrame) return;
            lastFrame=Time.unscaledTime+1f/24;
            Animate();
        }

        private void Animate()
        {
            time=Time.unscaledTime-opened;
            pulse=Mathf.Max(0,1-time/(reveal?1.35f:.76f));
            float size=Mathf.Min(rectTransform.rect.width,rectTransform.rect.height);
            float phase=time*1.64f+hall*.41f;
            float scale=.69f+(reveal?.06f:0)-pulse*pulse*.08f;
            art.rectTransform.sizeDelta=Vector2.one*(size*scale);
            art.rectTransform.anchoredPosition=new Vector2(0,size*(.055f+.022f*Mathf.Sin(phase)));
            art.rectTransform.localRotation=Quaternion.Euler(0,0,Mathf.Sin(phase*.70f)*1.7f);
            art.color=Color.Lerp(Color.white,pigment,.065f+.06f*Mathf.Sin(phase));
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper m)
        {
            m.Clear();
            float size=Mathf.Min(rectTransform.rect.width,rectTransform.rect.height);
            if (size<4) return;
            Vector2 center=rectTransform.rect.center;
            Vector2 foot=center+Vector2.down*(size*.32f);
            Disc(m,foot+Vector2.down*(size*.012f),size*.37f,size*.09f,new Color(.025f,.037f,.024f,.67f));
            var light=pigment;light.a=.48f+.14f*pulse;
            Disc(m,foot,size*.33f,size*.071f,light);
            light.a=.21f+.16f*pulse;
            Disc(m,center,size*.52f,size*.47f,light);
            int coronas=1+(grade-1)/3;
            for(int ring=0;ring<coronas;ring++)
            {
                float radius=size*(.37f+.036f*ring);
                light.a=.46f+.22f*pulse;
                for(int k=0;k<4;k++)
                {
                    float start=k*Mathf.PI*.5f+time*(ring%2==0?.27f:-.35f);
                    Arc(m,center,radius,start,1.12f,size*.006f,light);
                }
            }
            int count=8+grade+(reveal?9:0);
            for(int i=0;i<count;i++)
            {
                float seed=Mathf.Repeat(i*.618034f+.23f,1f);
                float age=Mathf.Repeat(seed+time*(.14f+.11f*seed),1f);
                float angle=seed*6.28318f+time*(i%2==0?.28f:-.21f);
                float rad=size*(.32f+.13f*age);
                Vector2 p=center+new Vector2(Mathf.Cos(angle)*rad,Mathf.Sin(angle)*rad*.77f+size*(age-.45f)*.2f);
                light=Color.Lerp(pigment,Color.white,seed*.64f);
                light.a=Mathf.Sin(Mathf.PI*age)*(.54f+.24f*pulse);
                Disc(m,p,size*(.012f+seed*.009f),size*(.014f+seed*.013f),light,6);
            }
            DrawBreath(m,center,size);
            if(pulse<=0 || (stage<0 && !reveal)) return;
            light=pigment;light.a=pulse*(reveal?.85f:.58f);
            Arc(m,center,size*(.2f+.28f*(1-pulse)),0,Mathf.PI*2,size*.009f,light,38);
        }

        // Each breath has a movement language visible at workbench scale.
        // Seeded paths make the twelve effects repeatable without particle objects.
        private void DrawBreath(VertexHelper m,Vector2 center,float size)
        {
            float spread = .34f*size;
            for(int i=0;i<4+(grade+1)/3;i++)
            {
                float seed=Mathf.Repeat(.317f+i*.618034f,1);
                float life=Mathf.Repeat(seed+time*(qi==2?.80f:.32f),1);
                float lightness=Mathf.Sin(life*Mathf.PI);
                Color tint=Color.Lerp(pigment,Color.white,.4f);
                tint.a=.73f*lightness;
                var orbit=Direction(6.28318f*seed+time*.42f);
                Vector2 point=center+orbit*spread;
                switch(qi)
                {
                    case 1: // 风沿器侧掠过。
                        point=center+new Vector2((life-.5f)*size*.92f,(seed-.5f)*spread);
                        Streak(m,point,point+new Vector2(spread*.35f,spread*.04f),size*.005f,tint); break;
                    case 2: // 雷从边缘折入两次。
                        if(life>.43f) break;
                        Vector2 bend=point-orbit*size*.058f+new Vector2(orbit.y,-orbit.x)*size*.043f;
                        Streak(m,point,bend,size*.011f,tint);
                        Streak(m,bend,point-orbit*size*.16f,size*.008f,tint); break;
                    case 3: // 寒晶落在环上。
                        point.y-=life*size*.10f;
                        for(int j=0;j<3;j++) {
                            Vector2 tip=Direction(j*Mathf.PI/3)*size*.030f;
                            Streak(m,point-tip,point+tip,size*.006f,tint);
                        } break;
                    case 4: // 炎星上扬。
                        point=center+new Vector2((seed-.5f)*spread*1.7f,(life-.45f)*size*.76f);
                        Disc(m,point,size*.018f,size*.046f,new Color(1,.59f,.24f,tint.a),8); break;
                    case 5: // 润露沉降。
                        point=center+new Vector2((seed-.5f)*size*.77f,(.46f-life)*size*.66f);
                        Disc(m,point,size*.021f,size*.031f,tint,8); break;
                    case 6: // 燥气向外散为碎屑。
                        point=center+orbit*size*(.23f+.22f*life);
                        Streak(m,point,point+orbit*size*.045f,size*.008f,tint); break;
                    case 7: // 双叶随生气舒展。
                        Disc(m,point,size*.012f,size*.036f,new Color(.63f,1,.71f,tint.a),7);
                        Disc(m,point+Vector2.right*size*.017f,size*.028f,size*.014f,tint,7); break;
                    case 8: // 煞丝内敛，带朱色。
                        point=center+orbit*size*(.45f-.21f*life);
                        Streak(m,point,point+orbit*size*.105f,size*.011f,new Color(.94f,.42f,.48f,tint.a));break;
                    case 9: // 清气积成露环。
                        Arc(m,point,size*(.012f+.035f*life),0,6.28318f,size*.005f,tint,12);break;
                    case 10: // 浊气低浮成雾。
                        point.y=center.y-size*.21f+Mathf.Sin(time*.5f+i)*size*.04f;
                        Disc(m,point,size*.115f,size*.048f,new Color(.74f,.68f,.52f,.25f*lightness));break;
                    case 11: // 罡气化为连续护弧。
                        Arc(m,center,size*(.36f+.04f*life),seed*6.28318f+time*.35f,.43f,size*.011f,tint);break;
                    case 12: // 幻光成相对双影。
                        Disc(m,point,size*.032f,size*.051f,new Color(.75f,.68f,1,tint.a),8);
                        Disc(m,center-(point-center),size*.016f,size*.035f,new Color(.57f,.93f,1,tint.a),8);break;
                }
            }
        }

        private static void Streak(VertexHelper m,Vector2 a,Vector2 b,float width,Color tint)
        {
            Vector2 delta=(b-a).normalized;
            Vector2 side=new Vector2(-delta.y,delta.x)*width*.5f;
            int first=m.currentVertCount;
            m.AddVert(a+side,tint,Vector2.zero);m.AddVert(a-side,tint,Vector2.zero);
            m.AddVert(b-side,tint,Vector2.zero);m.AddVert(b+side,tint,Vector2.zero);
            m.AddTriangle(first,first+1,first+2);m.AddTriangle(first,first+2,first+3);
        }

        private static Vector2 Direction(float angle)
        {
            return new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
        }

        private static void Disc(VertexHelper m, Vector2 point,float rx,float ry,Color color,int segments=24)
        {
            int first=m.currentVertCount;
            m.AddVert(point,color,Vector2.zero);
            color.a=0;
            for(int i=0;i<=segments;i++)
            {
                var d=Direction(i*(2*Mathf.PI/segments));
                m.AddVert(point+new Vector2(d.x*rx,d.y*ry),color,Vector2.zero);
                if(i>0) m.AddTriangle(first,first+i,first+i+1);
            }
        }

        private static void Arc(VertexHelper m,Vector2 point,float radius,float start,float span,float width,Color color,int n=12)
        {
            int first=m.currentVertCount;
            for(int i=0;i<=n;i++)
            {
                Vector2 d=Direction(start+span*i/n);
                m.AddVert(point+d*(radius-width),color,Vector2.zero);
                m.AddVert(point+d*radius,color,Vector2.zero);
                if(i==0) continue;
                int a=first+(i-1)*2;
                m.AddTriangle(a,a+1,a+2);m.AddTriangle(a+1,a+3,a+2);
            }
        }
    }
}
