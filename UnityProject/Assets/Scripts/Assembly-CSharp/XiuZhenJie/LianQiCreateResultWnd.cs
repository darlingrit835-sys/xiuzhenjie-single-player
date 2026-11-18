using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiCreateResultWnd : Window
	{
		public LianQiCreateResultPanel Panel;

		private bool isSuccess;

		private List<string> m_DataSource;

		private GameObject m_SuccessItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiCreateResultPanel>();
			m_DataSource = new List<string>();
			AddButtonClickListener(Panel.FailOnceAgainBtn, ZaiShiYiCiBtn);
			AddButtonClickListener(Panel.FailLiKaiBtn, LiKaiBtn);
			AddButtonClickListener(Panel.SuccessLiKaiBtn, LiKaiBtn);
			AddButtonClickListener(Panel.SuccessOnceAgainBtn, ZaiShiYiCiBtn);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 2)
			{
				isSuccess = (bool)paralist[0];
				if (isSuccess)
				{
					int wuXingXiaoHao = int.Parse(paralist[1].ToString());
					m_DataSource = paralist[2] as List<string>;
					InitSuccess(wuXingXiaoHao);
				}
				else
				{
					Panel.FailPanel.gameObject.SetActive(value: true);
					Panel.FailWuXingXiaoHao.text = int.Parse(paralist[1].ToString()) + "点";
					string text = paralist[2]?.ToString();
					Panel.FailChuangZaoShiBaiContentText.text = text;
				}
			}
		}

		private void InitSuccess(int wuXingXiaoHao)
		{
			Panel.SuccessPanel.gameObject.SetActive(value: true);
			Panel.SuccessWuXingXiaoHao.text = wuXingXiaoHao + "点";
			if (m_SuccessItem == null)
			{
				m_SuccessItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/CZGF_Tip_SuccessItem.prefab");
			}
			GridLayoutGroup component = Panel.SuccessContent.gameObject.GetComponent<GridLayoutGroup>();
			foreach (string item in m_DataSource)
			{
				CZGF_SuccessItem component2 = SetListContentParent(Panel.SuccessContent, m_SuccessItem).GetComponent<CZGF_SuccessItem>();
				string[] array = item.Split(',');
				if (array.Length == 2)
				{
					component2.SetValue(array[0], array[1]);
				}
			}
			SetGrticalListContentHight(m_DataSource.Count, Panel.SuccessContent, component);
		}

		private void ZaiShiYiCiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.ShowWnd("ChuangZhaoGongFaWnd.prefab", false);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("LianQiBuZhouSheZhiWnd.prefab");
		}
	}
}
