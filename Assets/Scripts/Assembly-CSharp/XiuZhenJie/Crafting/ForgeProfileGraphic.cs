using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace XiuZhenJie
{
    /// <summary>Deform the painted blank at the three hammer stations, preserving its painted surface.</summary>
    internal sealed class ForgeProfileGraphic : Image
    {
        private const int Divisions = 10;
        private int[] blows = new int[3];
        private bool upright;

        internal void Set(string blankId, int slotType, int[] stations)
        {
            sprite = InkInterfaceArt.Get("Goods/" + ForgeWorkshop.PatternArt(blankId));
            upright = slotType == 16 || slotType == 26 || slotType == 29;
            blows = stations;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper mesh)
        {
            mesh.Clear();
            if (sprite == null) return;
            Rect bounds = rectTransform.rect;
            Vector4 uv = DataUtility.GetOuterUV(sprite);
            float aspect = sprite.rect.width / sprite.rect.height;
            float height = Mathf.Min(bounds.height, bounds.width / aspect);
            // The painted weapons run from lower left to upper right. Lay the
            // blank on the anvil with its point to the right and tang to the left.
            for (int row = 0; row <= Divisions; row++)
            {
                float v = (float)row / Divisions;
                for (int col = 0; col <= Divisions; col++)
                {
                    float u = (float)col / Divisions;
                    float along = 1f - (u + v) * .5f;
                    float cross = (u - v) * .5f;
                    float ridge = 1f - 2f * Mathf.Abs(along - .5f);
                    float width = 1f + .09f * (blows[1] * ridge + blows[2] * (1f - along) - blows[0] * along);
                    float x = .07f + .80f * along + blows[0] * .025f * along * along;
                    float y = .48f + .15f * along + cross * width * 1.25f;
                    var vertex = new Vector2(bounds.xMin+x*bounds.width, bounds.yMin+y*bounds.height);
                    if (upright)
                    {
                        float local = Mathf.Lerp(blows[2], blows[0], v) + blows[1] * (1f - Mathf.Abs(2f * v - 1f));
                        vertex = bounds.center + new Vector2((u - .5f) * height * aspect * (1f + .025f * local), (v - .5f) * height);
                    }
                    mesh.AddVert(vertex,
                        Color.white, new Vector2(Mathf.Lerp(uv.x,uv.z,u), Mathf.Lerp(uv.y,uv.w,v)));
                }
            }
            for (int row = 0; row < Divisions; row++)
            {
                for (int col = 0; col < Divisions; col++)
                {
                    int a = row*(Divisions+1)+col, b = a+Divisions+1;
                    mesh.AddTriangle(a,b,a+1);
                    mesh.AddTriangle(a+1,b,b+1);
                }
            }
        }
    }
}
