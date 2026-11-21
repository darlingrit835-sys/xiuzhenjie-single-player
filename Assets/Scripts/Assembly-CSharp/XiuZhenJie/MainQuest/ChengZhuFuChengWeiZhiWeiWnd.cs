using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChengZhuFuChengWeiZhiWeiWnd : Window
	{
		public ChengZhuFuChengWeiZhiWeiPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ChengZhuFuChengWeiZhiWeiPanel>();
			AddButtonClickListener(_panel.Close, CLosePanel);
			AddButtonClickListener(_panel.ChengWei, ChengWeiZhiWei);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			SetPanel(paralist[0].ToString(), paralist[1].ToString(), paralist[2].ToString(), "1", paralist[3].ToString());
		}

		public void SetPanel(string ZhiWei, string JingJieYaoQiu, string HaoGanDu, string MeiNianRenWu, string MeiNianGongFeng)
		{
			_panel.ZhiWei.text = ZhiWei;
			_panel.JingJieYaoQiu.text = JingJieYaoQiu;
			_panel.HaoGanDu.text = HaoGanDu;
			_panel.MeiNianRenWu.text = MeiNianRenWu;
			_panel.MeiNianGongFeng.text = MeiNianGongFeng;
			_panel.ChengWei.transform.Find("Text").GetComponent<Text>().text = ZhiWei;
		}

		public void CLosePanel()
		{
			Singleton<UIManager>.Instance.CloseWnd("ChengZhuFuChengWeiKeQingWnd.prefab");
		}

		public void ChengWeiZhiWei()
		{
		}
	}
}
