using UnityEngine;

namespace XiuZhenJie.Client.Release
{
    // Built into the player, so the very first release-check frame can draw without mounting a bundle.
    internal sealed class StartupLoadingArtwork : ScriptableObject
    {
        public Sprite landscape;
        public Sprite logo;
        public Sprite[] wings;
    }
}
