using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Crop a single illustration or a specimen atlas cell to the laid-out slot.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class SpiritGardenImage : RawImage
    {
        private Rect source;
        private Vector2 focus;
        internal void Set(Texture2D picture, Rect region, Vector2 focalPoint)
        {
            source = region; focus = focalPoint; texture = picture; raycastTarget = false;
            Reframe();
        }
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange(); Reframe();
        }
        private void Reframe()
        {
            var slot = rectTransform.rect;
            if (texture == null || source.height <= 0 || slot.width < 1 || slot.height < 1) return;
            var region = source;
            float scale = (slot.width / slot.height) / (texture.width * source.width / (texture.height * source.height));
            if (scale < 1) region.width *= scale;
            else region.height /= scale;
            region.position = source.position + Vector2.Scale(source.size-region.size, focus);
            uvRect = region;
        }
    }

    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class SpiritGardenArt : MaskableGraphic
    {
        internal enum Motif { Plate, ShadeLeft, ShadeBottom, Furrows, Ward, Beds, Seed, Gate, Book }
        private Motif motif;
        private Color top, bottom, rim;
        internal void Set(Motif style, Color a, Color b, Color edge)
        {
            motif=style; top=a; bottom=b; rim=edge; raycastTarget=false; SetVerticesDirty();
        }
        private static readonly Vector2[] Corners = {
            new Vector2(0,1),new Vector2(1,1),new Vector2(1,0),new Vector2(0,0)
        };

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            var r = rectTransform.rect;
            if (r.width <= 1 || r.height <= 1) return;
            if (motif == Motif.ShadeLeft || motif == Motif.ShadeBottom)
            {
                for(int i=0;i<4;i++) {
                    var p=Corners[i];
                    mesh.AddVert(new Vector2(Mathf.Lerp(r.xMin,r.xMax,p.x),Mathf.Lerp(r.yMin,r.yMax,p.y)),
                        Color.Lerp(top,bottom,motif==Motif.ShadeLeft?p.x:p.y),Vector2.zero);
                }
                mesh.AddTriangle(0,1,2); mesh.AddTriangle(0,2,3); return;
            }
            if (motif == Motif.Plate)
            {
                float bevel = Mathf.Min(7, r.height * .17f, r.width * .09f);
                int center=mesh.currentVertCount; mesh.AddVert(r.center,Color.Lerp(top,bottom,.5f),Vector2.zero);
                for(int i=0;i<8;i++){
                    var p=Corner(r,i,bevel);
                    mesh.AddVert(p,Color.Lerp(bottom,top,Mathf.InverseLerp(r.yMin,r.yMax,p.y)),Vector2.zero);
                }
                for(int i=0;i<8;i++){
                    int n=(i+1)%8; mesh.AddTriangle(center,center+i+1,center+n+1);
                    Line(mesh,Corner(r,i,bevel),Corner(r,n,bevel),1.1f,rim);
                }
                var inner=new Rect(r.x+3,r.y+3,r.width-6,r.height-6);
                if (inner.width>12 && inner.height>12){
                    var c=rim;c.a*=.42f;
                    for(int i=0;i<8;i++)Line(mesh,Corner(inner,i,bevel*.7f),Corner(inner,(i+1)%8,bevel*.7f),.7f,c);
                }
                return;
            }
            var origin=r.center;
            float unit=Mathf.Min(r.width,r.height)*.45f;
            if (motif==Motif.Ward) {
                Ring(mesh,origin,unit*.79f,unit*.54f,54,1.8f,top);
                Ring(mesh,origin,unit*.94f,unit*.66f,54,.8f,rim);
                for(int i=0;i<8;i++){
                    var a=Orbit(origin,unit*.82f,unit*.56f,i*45);
                    var b=Orbit(origin,unit*.99f,unit*.70f,i*45);
                    Line(mesh,a,b,1.4f,rim);
                    Diamond(mesh,b,2.4f,top);
                    Line(mesh,a,Orbit(origin,unit*.82f,unit*.56f,(i+3)*45),.7f,bottom);
                }
                return;
            }
            Vector2 P(float x,float y) => origin+new Vector2(x*unit,y*unit);
            void Stroke(float x,float y,float a,float b,float w=1.65f) => Line(mesh,P(x,y),P(a,b),w,top);
            switch(motif){
                case Motif.Beds: case Motif.Furrows:
                    for(int i=0;i<3;i++){
                        float y=-.52f+i*.47f;
                        Stroke(-.93f,y,.02f,y+.28f);Stroke(.02f,y+.28f,.91f,y+.07f);
                    }
                    break;
                case Motif.Seed:
                    Stroke(-.49f,-.81f,.35f,.76f);Stroke(-.33f,-.32f,-.68f,-.10f);Stroke(-.68f,-.10f,-.64f,.22f);Stroke(-.64f,.22f,-.20f,-.04f);
                    Stroke(.01f,.13f,.60f,.18f);Stroke(.60f,.18f,.72f,.57f);Stroke(.72f,.57f,.17f,.44f);break;
                case Motif.Gate:
                    Stroke(-.90f,.12f,0,.81f);Stroke(0,.81f,.90f,.12f);Stroke(-.78f,.13f,.78f,.13f);
                    Stroke(-.53f,.07f,-.53f,-.67f);Stroke(.53f,.07f,.53f,-.67f);Stroke(-.79f,-.72f,.79f,-.72f);break;
                case Motif.Book:
                    Stroke(-.69f,.67f,.54f,.67f);Stroke(.54f,.67f,.54f,-.65f);Stroke(.54f,-.65f,-.69f,-.65f);Stroke(-.69f,-.65f,-.69f,.67f);
                    Stroke(-.39f,.61f,-.39f,-.59f);Stroke(-.15f,.25f,.32f,.25f);Stroke(-.15f,-.11f,.22f,-.11f);break;
            }
        }
        private static Vector2 Corner(Rect r,int i,float s) {
            switch (i) {
            case 0:return new Vector2(r.xMin+s,r.yMax); case 1:return new Vector2(r.xMax-s,r.yMax);
            case 2:return new Vector2(r.xMax,r.yMax-s); case 3:return new Vector2(r.xMax,r.yMin+s);
            case 4:return new Vector2(r.xMax-s,r.yMin); case 5:return new Vector2(r.xMin+s,r.yMin);
            case 6:return new Vector2(r.xMin,r.yMin+s); default:return new Vector2(r.xMin,r.yMax-s);
            }
        }
        private static Vector2 Orbit(Vector2 p,float w,float h,float deg){
            float t=deg*Mathf.Deg2Rad;return p+new Vector2(Mathf.Cos(t)*w,Mathf.Sin(t)*h);
        }
        private static void Ring(VertexHelper mesh,Vector2 c,float w,float h,int sides,float width,Color tint){
            for(int i=0;i<sides;i++)Line(mesh,Orbit(c,w,h,360f*i/sides),Orbit(c,w,h,360f*(i+1)/sides),width,tint);
        }
        private static void Line(VertexHelper mesh,Vector2 a,Vector2 b,float width,Color tint) {
            var d=(b-a).normalized; var n=new Vector2(-d.y,d.x)*width*.5f;
            Quad(mesh,a+n,b+n,b-n,a-n,tint);
        }
        private static void Diamond(VertexHelper mesh,Vector2 p,float size,Color tint) {
            Quad(mesh,p+Vector2.up*size,p+Vector2.right*size,p+Vector2.down*size,p+Vector2.left*size,tint);
        }
        private static void Quad(VertexHelper mesh,Vector2 a,Vector2 b,Vector2 c,Vector2 d,Color tint) {
            int n=mesh.currentVertCount;mesh.AddVert(a,tint,Vector2.zero);mesh.AddVert(b,tint,Vector2.zero);
            mesh.AddVert(c,tint,Vector2.zero);mesh.AddVert(d,tint,Vector2.zero);mesh.AddTriangle(n,n+1,n+2);mesh.AddTriangle(n,n+2,n+3);
        }
    }
}
