using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    // A single soft glint across the roof of the current place. The source art supplies the silhouette.
    internal sealed class AtlasEaveLight : MonoBehaviour
    {
        private static Sprite brush;
        private Image silhouette;
        private Image light;
        private float opened;

        internal static void Set(Image art, bool active)
        {
            var effect = art.GetComponent<AtlasEaveLight>();
            if (effect == null && !active) return;
            if (effect == null) effect = art.gameObject.AddComponent<AtlasEaveLight>();
            if (effect.silhouette == null)
            {
                // Instantiated map levels also clone their visual tree, which the presenter rebuilds.
                effect.silhouette = RoleFolio.Image(art.transform, "屋檐轮廓", Color.white);
                RoleFolio.Fill(effect.silhouette.rectTransform);
                effect.silhouette.preserveAspect = true;
                effect.silhouette.gameObject.AddComponent<Mask>().showMaskGraphic = false;
                effect.light = RoleFolio.Image(effect.silhouette.transform, "流动檐光", Color.clear);
                effect.light.sprite = Brush();
                var rt = effect.light.rectTransform;
                rt.sizeDelta = new Vector2(19, 2.4f);
                rt.localRotation = Quaternion.Euler(0, 0, 8);
            }
            effect.silhouette.sprite = art.sprite;
            effect.silhouette.gameObject.SetActive(active);
            effect.enabled = active;
            effect.opened = Time.unscaledTime;
            effect.light.color = Color.clear;
        }

        private void Update()
        {
            float beat = Mathf.Repeat(Time.unscaledTime - opened + .27f, 3.8f);
            float t = Mathf.Clamp01(beat / .65f);
            var rt = light.rectTransform;
            rt.anchorMin = rt.anchorMax = new Vector2(Mathf.Lerp(.27f,.73f,t), Mathf.Lerp(.55f,.61f,t));
            rt.anchoredPosition = Vector2.zero;
            light.color = new Color(1f,.89f,.64f,t < 1f ? Mathf.Sin(t * Mathf.PI) * .60f : 0f);
        }

        private static Sprite Brush()
        {
            if (brush != null) return brush;
            const int width = 48, height = 8;
            var texture = new Texture2D(width,height,TextureFormat.RGBA32,false);
            texture.wrapMode=TextureWrapMode.Clamp; texture.filterMode=FilterMode.Bilinear;
            var pixels = new Color32[width*height];
            for(int y=0;y<height;y++) for(int x=0;x<width;x++)
            {
                float t=(x+.5f)/width, s=(y+.5f)/height;
                float a=Mathf.Pow(Mathf.Sin(t*Mathf.PI),2.5f)*Mathf.Pow(Mathf.Sin(s*Mathf.PI),1.4f);
                pixels[y*width+x]=new Color32(255,255,255,(byte)Mathf.RoundToInt(a*255f));
            }
            texture.SetPixels32(pixels);texture.Apply(false,true);
            brush=Sprite.Create(texture,new Rect(0,0,width,height),new Vector2(.5f,.5f),100);
            brush.name="现址屋檐微光";
            return brush;
        }
    }
}
