using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public class XuShenJiTanWnd : Window
    {
        public XuShenJiTanPanel Panel;
        private VoidAltarView altar;

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            Panel = GameObject.GetComponent<XuShenJiTanPanel>();
            Font font = Panel.GetComponentInChildren<Text>(true)?.font;
            // The recovered panel contains five identical example reward labels.
            // Keep its root and font, and draw from the authoritative altar preview.
            foreach (Transform child in Panel.transform) child.gameObject.SetActive(false);
            altar = VoidAltarView.Create(Panel.transform, font, CloseWindow);
        }

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            altar?.Open();
        }

        public override void OnClose()
        {
            altar?.Cancel();
            base.OnClose();
        }
    }
}
