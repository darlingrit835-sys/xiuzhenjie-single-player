using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XiaoWenHaoWnd : Window
	{
		public XiaoWenHaoPanel xiaoWenHaoPanel;

		private int m_RowNew;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			xiaoWenHaoPanel = base.GameObject.GetComponent<XiaoWenHaoPanel>();
			AddButtonClickListener(xiaoWenHaoPanel.BenShen, CloseThisWnd);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length < 3)
			{
				xiaoWenHaoPanel.XianShiLianDan.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiZhanDo.gameObject.SetActive(value: false);
				xiaoWenHaoPanel.XianShiYi.gameObject.SetActive(value: true);
				xiaoWenHaoPanel.Name.text = paralist[1].ToString();
				ZiShiYingHightNew(paralist[0].ToString());
			}
			else if (paralist[2].ToString() == "炼丹")
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

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (xiaoWenHaoPanel.NameBg.rectTransform.sizeDelta.x != xiaoWenHaoPanel.Name.rectTransform.sizeDelta.x + 15f)
			{
				xiaoWenHaoPanel.NameBg.rectTransform.sizeDelta = new Vector2(xiaoWenHaoPanel.Name.rectTransform.sizeDelta.x + 15f, 27.1f);
			}
		}

		public void ZiShiYingHightNew(string contentText)
		{
			int num = contentText.Length / 24;
			xiaoWenHaoPanel.gameObject.GetComponent<RectTransform>();
			_ = (float)((num + 3) * 2);
			_ = xiaoWenHaoPanel.text.sizeDelta;
			xiaoWenHaoPanel.text.GetComponent<Text>().text = contentText;
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("XiaoWenHaoWnd.prefab");
		}
	}
}
