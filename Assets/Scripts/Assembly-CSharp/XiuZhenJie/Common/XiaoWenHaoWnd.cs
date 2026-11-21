using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XiaoWenHaoWnd : Window
	{
		public XiaoWenHaoPanel xiaoWenHaoPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			xiaoWenHaoPanel = base.GameObject.GetComponent<XiaoWenHaoPanel>();
			AddButtonClickListener(xiaoWenHaoPanel.BenShen, CloseThisWnd);
			InkDialogArt.StyleHelp(xiaoWenHaoPanel, CloseThisWnd);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist == null || paralist.Length < 3)
			{
				xiaoWenHaoPanel.XianShiLianDan.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiZhanDo.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiYi.gameObject.SetActive(value: true);
				xiaoWenHaoPanel.Name.text = paralist != null && paralist.Length > 1
					? paralist[1]?.ToString() ?? "提示说明" : "提示说明";
				ZiShiYingHightNew(paralist != null && paralist.Length > 0 ? paralist[0]?.ToString() : null);
			}
			else if (paralist[2]?.ToString() == "炼丹")
			{
				xiaoWenHaoPanel.XianShiZhanDo.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiYi.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiLianDan.gameObject.SetActive(value: true);
			}
			else
			{
				xiaoWenHaoPanel.XianShiZhanDo.gameObject.SetActive(value: true);
				xiaoWenHaoPanel.XianShiYi.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiLianDan.gameObject.SetActive(value: false);
			}
		}

		public void ZiShiYingHightNew(string contentText)
		{
			xiaoWenHaoPanel.text.GetComponent<Text>().text = contentText ?? string.Empty;
			ScrollRect scroll = xiaoWenHaoPanel.ContentScrollView.GetComponent<ScrollRect>();
			scroll.StopMovement();
			scroll.verticalNormalizedPosition = 1f;
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("XiaoWenHaoWnd.prefab");
		}
	}
}
