using UnityEngine;

namespace XiuZhenJie
{
	public class LT_LingTianMainWnd : Window
	{
		public LT_LingTianMainPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = GameObject.GetComponent<LT_LingTianMainPanel>();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			SpiritGarden.Open((RectTransform)Panel.transform, Panel.LingTianNumber.font,
				() => Singleton<UIManager>.Instance.CloseWnd(this));
		}
	}
}
