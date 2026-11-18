using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZhaoFaShuJieGuoWnd : Window
	{
		public ChuangZhaoFaShuJieGuoPanel Panel;

		private bool isSuccess;

		private XSJ_FaShuEntity m_DataSource;

		private GameObject m_SuccessItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			isSuccess = false;
			Panel = base.GameObject.GetComponent<ChuangZhaoFaShuJieGuoPanel>();
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
					int stepNum = int.Parse(paralist[1].ToString());
					m_DataSource = paralist[2] as XSJ_FaShuEntity;
					InitSuccess(stepNum);
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

		private void InitSuccess(int stepNum)
		{
			if (m_DataSource == null)
			{
				return;
			}
			Panel.SuccessPanel.gameObject.SetActive(value: true);
			Panel.SuccessWuXingXiaoHao.text = stepNum * m_DataSource.Rank + "点";
			if (m_SuccessItem == null)
			{
				m_SuccessItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/CZGF_Tip_SuccessItem.prefab");
			}
			GridLayoutGroup component = Panel.SuccessContent.gameObject.GetComponent<GridLayoutGroup>();
			if (m_DataSource.DetailList == null)
			{
				return;
			}
			foreach (string detail in m_DataSource.DetailList)
			{
				CZGF_SuccessItem component2 = SetListContentParent(Panel.SuccessContent, m_SuccessItem).GetComponent<CZGF_SuccessItem>();
				string[] array = detail.Split(',');
				if (array.Length == 2)
				{
					component2.SetValue(array[0], array[1]);
				}
			}
			SetGrticalListContentHight(m_DataSource.DetailList.Count, Panel.SuccessContent, component);
		}

		private void ZaiShiYiCiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.FindWndByName<ChuangZaoFaShuWnd>("ChuangZaoFaShuWnd.prefab")?.OnceAgainShow();
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("ChuangZaoFaShuWnd.prefab");
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
		}
	}
}
