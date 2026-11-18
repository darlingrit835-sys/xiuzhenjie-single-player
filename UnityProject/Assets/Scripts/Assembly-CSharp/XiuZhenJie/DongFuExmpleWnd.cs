namespace XiuZhenJie
{
	public class DongFuExmpleWnd : Window
	{
		public DongFuExmplePanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuExmplePanel>();
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
			string textValue = $"本次突破为雷劫突破,中途不可退出,突破失败会遭受重创,严重时可能损毁道基。本次雷劫预计为{15}道雷霆,请慎重考虑！";
			MonoSingleton<GameStart>.Instance.PopUpAttention(textValue, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "TuPoExmpleScene");
			});
		}
	}
}
