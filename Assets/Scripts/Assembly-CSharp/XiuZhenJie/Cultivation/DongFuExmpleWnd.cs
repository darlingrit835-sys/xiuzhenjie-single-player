namespace XiuZhenJie
{
	public class DongFuExmpleWnd : Window
	{
		public DongFuExmplePanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuExmplePanel>();
			InkCultivationPages.PaperDialog(Panel.transform, "HuiYiPanel/LiXianBg", Panel.QuDuJieBtn, null);
			AddButtonClickListener(Panel.BiGuanTuPoBtn, BiGuanTuPoBtnClick);
			AddButtonClickListener(Panel.QuDuJieBtn, () =>
			{
				Panel.HuiYiPanel.gameObject.SetActive(value: false);
			});
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Panel.HuiYiPanel.gameObject.SetActive(value: true);
		}

		public void BiGuanTuPoBtnClick()
		{
			string textValue = "玄玉真人将迎三道雷劫。看清雷相，择印化解，再入轮回。要进入这段前尘吗？";
			MonoSingleton<GameStart>.Instance.PopUpAttention(textValue, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "TuPoExmpleScene");
			});
		}
	}
}
