using UnityEngine;

namespace XiuZhenJie
{
	public class ChengZhuFuMainWnd : Window
	{
		public ChengZhuFuMainPanel Panel;

		private const string m_ChengZhuFuContent = "城主府广开修行之路，为诸位之修行提供最大助力，为修真界至繁荣奠定最牢根基，凡有修行资质之人，均可通过修行任务获取丰厚奖励！";

		private const string m_TianShuBeiContent = "每一名修士的修行之路，对整个界面的修行都有莫大的意义，我们汇总统计这些信息，发出奖励，给后来之修士以极大的参考，让诸位仙途更为宽广。";

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChengZhuFuMainPanel>();
			AddButtonClickListener(Panel.QieHuanPanelBtn, QieHuanTsbPanel);
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			Panel.GongGaoText.text = "城主府广开修行之路，为诸位之修行提供最大助力，为修真界至繁荣奠定最牢根基，凡有修行资质之人，均可通过修行任务获取丰厚奖励！";
			Panel.CZF_ContentPanel.InitPanel();
			Panel.CZF_TSB_ContentPanel.InitPanel(this);
		}

		private void QieHuanTsbPanel()
		{
			Panel.CZF_TSB_ContentPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -12.19f);
			Panel.QieHuanPanelBtn.onClick.RemoveAllListeners();
			Panel.QieHuanPanelBtn.onClick.AddListener(QieHuanCzfPanel);
			Panel.QieHuanText.text = "城主任务";
			Panel.GongGaoText.text = "每一名修士的修行之路，对整个界面的修行都有莫大的意义，我们汇总统计这些信息，发出奖励，给后来之修士以极大的参考，让诸位仙途更为宽广。";
		}

		private void QieHuanCzfPanel()
		{
			Panel.CZF_TSB_ContentPanel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			Panel.QieHuanPanelBtn.onClick.RemoveAllListeners();
			Panel.QieHuanPanelBtn.onClick.AddListener(QieHuanTsbPanel);
			Panel.QieHuanText.text = "天书碑";
			Panel.GongGaoText.text = "城主府广开修行之路，为诸位之修行提供最大助力，为修真界至繁荣奠定最牢根基，凡有修行资质之人，均可通过修行任务获取丰厚奖励！";
		}
	}
}
