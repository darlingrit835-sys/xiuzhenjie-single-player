namespace XiuZhenJie
{
    public class AH_MainSceneWndl : Window
    {
        public AH_MainScenePanel Panel;

        public override void Awake(params object[] paralist)
        {
            base.Awake(paralist);
            Panel = GameObject.GetComponent<AH_MainScenePanel>();
            var font = Panel.Aggregate.font;
            foreach (UnityEngine.Transform child in Transform) child.gameObject.SetActive(false);
            bool market = paralist != null && paralist.Length > 0 && paralist[0] as string == "market";
            AuctionHall.Open(this, font, market, () => Singleton<UIManager>.Instance.CloseWnd(this));
        }
    }
}
