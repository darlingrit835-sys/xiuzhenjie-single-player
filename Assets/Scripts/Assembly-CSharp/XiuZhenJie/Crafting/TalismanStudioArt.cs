using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Drawn at canvas resolution so gilt inlays and fine corners stay sharp on the writing desk.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class TalismanStudioArt : MaskableGraphic
    {
        internal enum Finish { Shade, Panel, Row, Key, Seal, Paper, Roller, Rim }
        internal Finish finish;
        private static readonly Color32 Bronze = new Color32(150, 107, 54, 255);
        private static readonly Color32 Gold = new Color32(239, 204, 134, 255);
        private static readonly Color32 Patina = new Color32(101, 67, 34, 255);
        private static readonly Color32 LightInk = new Color32(91, 57, 35, 255);
        private Rect bounds;
        private VertexHelper mesh;

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            mesh = helper; mesh.Clear(); bounds = GetPixelAdjustedRect();
            float w = bounds.width, h = bounds.height;
            if (w <= 0 || h <= 0) return;
            if (finish == Finish.Shade)
            {
                var left = new Color32(20, 12, 11, 230); var right = new Color32(22, 13, 10, 30);
                Fill(0, 0, w, h, left, left, right, right);
                Gradient(0, 0, w, h * .27f, new Color32(19, 13, 13, 224), new Color32(24, 16, 12, 0));
                Gradient(0, h * .78f, w, h, new Color32(24, 16, 12, 0), new Color32(20, 14, 12, 105));
                return;
            }
            if (finish == Finish.Roller)
            {
                Gradient(0, 0, w, h * .58f, new Color32(57, 32, 22, 255), Bronze);
                Gradient(0, h * .58f, w, h, Gold, new Color32(58, 33, 24, 255));
                foreach (float x in new[] { .015f, .041f, .959f, .985f }) Quad(w*x-1, 0, w*x+1, h, Gold);
                return;
            }
            bool paper = finish == Finish.Paper, rim = finish == Finish.Rim;
            if (!paper && !rim)
            {
                Color32 low = new Color32(26, 20, 21, 252), high = new Color32(62, 44, 36, 248);
                if (finish == Finish.Row) { low = new Color32(32, 24, 24, 232); high = new Color32(54, 39, 34, 224); }
                if (finish == Finish.Key) { low = new Color32(38, 29, 30, 255); high = new Color32(81, 59, 45, 255); }
                if (finish == Finish.Seal) { low = new Color32(98, 35, 30, 255); high = new Color32(157, 67, 43, 255); }
                Gradient(0, 0, w, h, low, high);
                if (finish == Finish.Row)
                {
                    Gradient(0, h-1, w, h, new Color32(109, 77, 49, 120), new Color32(109, 77, 49, 120));
                    return;
                }
                if (finish == Finish.Panel)
                {
                    for (int i = 1; i < 9; i++)
                    {
                        float y = h * i / 9f;
                        Bar(new Vector2(3,y), new Vector2(w-3,y+Mathf.Sin(i*7)*4), 1, new Color32(169, 110, 66, 14));
                    }
                }
            }
            var edge = paper ? new Color32(151, 77, 41, 213) : rim ? new Color32(179, 137, 69, 146) : Bronze;
            Outline(1, edge);
            if (w < 45 || h < 21) return;
            Outline(paper ? 6 : 3,new Color32(edge.r,edge.g,edge.b,105));
            if (finish == Finish.Key || finish == Finish.Seal)
            {
                for (int side = -1; side <= 1; side += 2)
                {
                    float x=side<0?4:w-4;float y=h/2;
                    Bar(new Vector2(x,y-3),new Vector2(x-side*3,y),.8f,Gold);
                    Bar(new Vector2(x-side*3,y),new Vector2(x,y+3),.8f,Gold);
                }
                return;
            }
            for (int side=0;side<2;side++) for(int top=0;top<2;top++) Corner(side==0?0:w,top==0?0:h,side==0?1:-1,top==0?1:-1,paper?LightInk:Gold);
        }

        private void Corner(float x,float y,int sx,int sy,Color32 tint)
        {
            Vector2 P(float a,float b)=>new Vector2(x+sx*a,y+sy*b);
            Bar(P(4,17),P(4,4),1.0f,tint); Bar(P(4,4),P(17,4),1.0f,tint);
            Bar(P(7,13),P(7,7),1.1f,tint); Bar(P(7,7),P(13,7),1.1f,tint);
            Bar(P(7,13),P(13,13),.8f,tint); Bar(P(13,13),P(13,9),.8f,tint);
        }
        private void Outline(float n,Color32 tint)
        {
            float w=bounds.width,h=bounds.height;
            Quad(n,n,w-n,n+.8f,tint);Quad(n,h-n-.8f,w-n,h-n,tint);
            Quad(n,n,n+.8f,h-n,tint);Quad(w-n-.8f,n,w-n,h-n,tint);
        }
        private void Bar(Vector2 a,Vector2 b,float width,Color32 tint)
        {
            Vector2 dir=(b-a).normalized;var side=new Vector2(-dir.y,dir.x)*width*.5f;
            Vertices(a-side,a+side,b+side,b-side,tint,tint,tint,tint);
        }
        private void Quad(float x,float y,float x2,float y2,Color32 c)=>Fill(x,y,x2,y2,c,c,c,c);
        private void Gradient(float x,float y,float x2,float y2,Color32 bottom,Color32 top)=>Fill(x,y,x2,y2,bottom,top,top,bottom);
        private void Fill(float x,float y,float x2,float y2,Color32 a,Color32 b,Color32 c,Color32 d)=>Vertices(new Vector2(x,y),new Vector2(x,y2),new Vector2(x2,y2),new Vector2(x2,y),a,b,c,d);
        private void Vertices(Vector2 a,Vector2 b,Vector2 c,Vector2 d,Color32 ca,Color32 cb,Color32 cc,Color32 cd)
        {
            int i=mesh.currentVertCount;var origin=bounds.min;
            mesh.AddVert(origin+a,ca,Vector2.zero);mesh.AddVert(origin+b,cb,Vector2.zero);
            mesh.AddVert(origin+c,cc,Vector2.zero);mesh.AddVert(origin+d,cd,Vector2.zero);
            mesh.AddTriangle(i,i+1,i+2);mesh.AddTriangle(i,i+2,i+3);
        }
    }
}
