using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    internal static class StartupArtwork
    {
        private static Sprite s_Landscape;
        private static GameObject s_CranePrefab;

        internal static GameObject CranePrefab
        {
            get
            {
                if (s_CranePrefab == null)
                    s_CranePrefab = ReleasedResources.Load<GameObject>("prefab/LoadingCrane");

                return s_CranePrefab;
            }
        }

        internal static bool TryApplyLandscape(Image background)
        {
            if (background == null)
                return false;

            if (s_Landscape == null)
                s_Landscape = ReleasedResources.Load<Sprite>("InkBrightLandscape");
            if (s_Landscape == null)
                return false;

            background.sprite = s_Landscape;
            background.overrideSprite = null;
            background.type = Image.Type.Simple;
            background.preserveAspect = false;
            background.color = Color.white;
            return true;
        }
    }
}
