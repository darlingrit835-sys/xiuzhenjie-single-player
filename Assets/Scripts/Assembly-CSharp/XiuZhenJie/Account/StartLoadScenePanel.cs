using UnityEngine.UI;

namespace XiuZhenJie
{
	public class StartLoadScenePanel : MonoSingleton<StartLoadScenePanel>
	{
		public Scrollbar ProgressScrollbar;

		public Text ProgressText;

		public Image MaskImg;

		public Image ProgressValue;

		protected override void Awake()
		{
			base.Awake();
		}
	}
}
