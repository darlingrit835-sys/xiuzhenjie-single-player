using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // Selected sites form a visible route in the drawing beneath the touch targets.
    [RequireComponent(typeof(CanvasRenderer))]
    internal sealed class VoidRealmWorkshopBoard : MaskableGraphic
    {
        private string hall;
        private Vector2[] sites = Array.Empty<Vector2>();
        private int[] path = Array.Empty<int>();
        private Color ink;
        private float touched, nextFrame;

        internal void Set(string hallId, Vector2[] points, int[] steps, Color pigment)
        {
            hall = hallId; sites = points; path = steps; ink = pigment;
            touched = Time.unscaledTime; raycastTarget = false; SetVerticesDirty();
        }

        private void Update()
        {
            float now = Time.unscaledTime;
            if (now < nextFrame || now - touched > 1.1f) return;
            nextFrame = now + .05f;
            SetVerticesDirty();
        }

        private static Vector2 Polar(float radius, float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            Rect rect = GetPixelAdjustedRect();
            if (rect.width < 1 || rect.height < 1) return;
            Vector2 c = rect.center;
            Color line = new Color(.76f, .65f, .39f, .36f);
            float r = Mathf.Min(rect.width * .44f, rect.height * .40f);
            switch (hall)
            {
                case "dan": // 炉身分层，三道火候依次收束。
                    Ring(mesh,c,r,20,line); Ring(mesh,c,r*.62f,20,line);
                    Stroke(mesh,c+new Vector2(-r*.92f,-r*.36f),c+new Vector2(r*.92f,-r*.36f),1.1f,line);break;
                case "qi": // 器胚的两道脊线。
                    Diamond(mesh,c,r*1.45f,r*.43f,line);
                    Stroke(mesh,c-Vector2.right*r*1.35f,c+Vector2.right*r*1.35f,1.2f,line);break;
                case "fu": // 自符头折入符胆，再回钩封尾。
                    Skeleton(mesh,rect,new[]{0,2,4,3,7,8},line);break;
                case "shu":
                    Ring(mesh,c,r,5,line);
                    for(int i=0;i<5;i++)Stroke(mesh,c+Polar(r,2*Mathf.PI*i/5),c+Polar(r,2*Mathf.PI*(i+2)/5),.8f,line);
                    break;
                case "zhen":
                    Ring(mesh,c,r,8,line);
                    for(int i=0;i<3;i++){ Skeleton(mesh,rect,new[]{i,i+6},line);Skeleton(mesh,rect,new[]{i*3,i*3+2},line); }
                    break;
                case "dao": // 两脉沿中轴回环。
                    Skeleton(mesh,rect,new[]{1,3,6,7,8,5,1,4,7},line);break;
                case "zhi": // 根、干、穗分层展开。
                    Skeleton(mesh,rect,new[]{6,4,0},line);Skeleton(mesh,rect,new[]{7,4,1},line);
                    Skeleton(mesh,rect,new[]{8,4,2},line);break;
                case "qiao":
                    Ring(mesh,c,r,12,line);Ring(mesh,c,r*.52f,8,line);
                    for(int i=0;i<8;i++)Stroke(mesh,c+Polar(r*.75f,i*Mathf.PI/4),c+Polar(r*1.14f,i*Mathf.PI/4),2.2f,line);
                    break;
                case "shou":
                    Skeleton(mesh,rect,new[]{6,3,1,5,8},line);
                    Ring(mesh,c,r*.49f,12,line);break;
            }
            Vector2 previous = Vector2.zero;
            Color routeInk = Color.Lerp(ink, new Color(1,.86f,.51f),.56f);
            float elapsed = Mathf.Clamp01((Time.unscaledTime-touched)/.64f);
            for (int i=0; i<path.Length;i++)
            {
                int index = path[i];
                if (index<0 || index>=sites.Length) continue;
                Vector2 center = rect.min + Vector2.Scale(rect.size, sites[index]);
                if (i>0)
                {
                    Vector2 destination = i==path.Length-1 ? Vector2.Lerp(previous,center,Mathf.SmoothStep(0,1,elapsed)) : center;
                    Stroke(mesh,previous,destination,2,routeInk);
                }
                Ring(mesh,center,8,8,routeInk);
                if (i==path.Length-1)
                {
                    var flare = routeInk; flare.a = .8f * (1-elapsed);
                    Ring(mesh,center,12+elapsed*23f,24,flare);
                }
                previous = center;
            }
        }

        internal static Vector2 Site(string id,int stage,int choice)
        {
            float row=.80f-stage*.30f, side=choice-1;
            switch(id)
            {
                case "dan": return new Vector2(.5f+side*(.29f-.026f*stage),row-.035f*side*side);
                case "qi": return new Vector2(.17f+stage*.33f,.5f+side*.30f+Mathf.Sin(stage*Mathf.PI/2)*.045f);
                case "fu": return new Vector2(.5f+(stage==1?-side:side)*.32f,row-.042f*side);
                case "shu": return new Vector2(.5f+side*.31f,row-.08f*side*side);
                case "dao": return new Vector2(.5f+side*(stage==1 ? .37f : .27f),row-.045f*side*side);
                case "zhi": return new Vector2(.5f+side*(.36f-.055f*stage),row+.05f*side*side);
                case "qiao": return new Vector2(.18f+stage*.32f,.5f+(stage==1?-side:side)*.3f);
                case "shou": return new Vector2(.5f+side*.29f+(stage==1 ? .055f : -.03f),row+.042f*side);
                default: return new Vector2(.5f+side*.31f,row);
            }
        }

        private void Skeleton(VertexHelper mesh,Rect rect,int[] ids,Color tint)
        {
            for(int i=1;i<ids.Length;i++)
                Stroke(mesh,rect.min+Vector2.Scale(rect.size,sites[ids[i-1]]),rect.min+Vector2.Scale(rect.size,sites[ids[i]]),1.1f,tint);
        }

        private static void Diamond(VertexHelper mesh,Vector2 c,float width,float height,Color tint)
        {
            var points=new[]{c+Vector2.left*width,c+Vector2.up*height,c+Vector2.right*width,c+Vector2.down*height};
            for(int i=0;i<4;i++)Stroke(mesh,points[i],points[(i+1)%4],1.2f,tint);
        }

        private static void Ring(VertexHelper mesh,Vector2 center,float radius,int sides,Color tint)
        {
            for(int i=0;i<sides;i++)
                Stroke(mesh,center+Polar(radius,i*Mathf.PI*2/sides),center+Polar(radius,(i+1)*Mathf.PI*2/sides),1.1f,tint);
        }

        private static void Stroke(VertexHelper mesh,Vector2 a,Vector2 b,float width,Color tint)
        {
            Vector2 delta = b-a;
            if (delta.sqrMagnitude<.001f) return;
            var offset = new Vector2(-delta.y,delta.x).normalized*(width*.5f);
            int v=mesh.currentVertCount;
            mesh.AddVert(a-offset,tint,Vector2.zero); mesh.AddVert(a+offset,tint,Vector2.zero);
            mesh.AddVert(b+offset,tint,Vector2.zero); mesh.AddVert(b-offset,tint,Vector2.zero);
            mesh.AddTriangle(v,v+1,v+2); mesh.AddTriangle(v,v+2,v+3);
        }
    }
}
