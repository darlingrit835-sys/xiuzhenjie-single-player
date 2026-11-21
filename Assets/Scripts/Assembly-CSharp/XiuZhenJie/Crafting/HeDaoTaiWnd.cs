using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
    public class HeDaoTaiWnd : Window
    {
        public HeDaoTaiPanel Panel;
        private ConcordDeskView view;

        public override void Awake(params object[] args)
        {
            base.Awake(args);
            Panel = GameObject.GetComponent<HeDaoTaiPanel>();
        }

        public override void OnShow(params object[] args)
        {
            base.OnShow(args);
            if (view == null)
            {
                var art = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(sprite => sprite.name == "Bg_HeDaoTai")
                    ?? Panel.transform.Find("BG")?.GetComponent<Image>()?.sprite;
                var font = Panel.GetComponentInChildren<Text>(true)?.font;
                view = ConcordDeskView.Open(Panel.transform, art, font, () => Singleton<UIManager>.Instance.CloseWnd(this));
            }
            view.Refresh();
        }

        public override void OnDisable()
        {
            if (view != null) view.Suspend();
            base.OnDisable();
        }

        public override void OnClose()
        {
            if (view != null) { view.gameObject.SetActive(false); Object.Destroy(view.gameObject); view = null; }
            base.OnClose();
        }
    }
}
