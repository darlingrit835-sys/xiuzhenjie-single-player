using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XSJ_ZhuCeTipWnd : Window
	{
		public XSJ_ZhuCeTipPanel Panel;

		private XSJ_ZhuCeTipWndModel m_DataSource;

		private GameObject m_ItemObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<XSJ_ZhuCeTipPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.WenHaoBtn, WenHaoBtnClick);
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as XSJ_ZhuCeTipWndModel;
			}
			Init();
		}

		private void WenHaoBtnClick()
		{
			string text = "";
			if (m_DataSource != null)
			{
				text = m_DataSource.NameMingGuiZe;
			}
			Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, text, null, "命名规则", 6);
		}

		private void Init()
		{
			if (m_DataSource == null)
			{
				return;
			}
			if (m_DataSource.isCanZhuCe)
			{
				Panel.NameText.interactable = true;
				Panel.ZhuCeBtn.interactable = true;
				Panel.ZhuCeBtn.onClick.RemoveAllListeners();
				Panel.ZhuCeBtn.onClick.AddListener(() =>
				{
					ZhuCeBtnClick();
				});
			}
			else
			{
				Panel.NameText.interactable = false;
				Panel.NameText.text = m_DataSource.TiShiName;
				Panel.ZhuCeBtn.interactable = false;
			}
			GridLayoutGroup component = Panel.ScrollContent.gameObject.GetComponent<GridLayoutGroup>();
			foreach (string detail in m_DataSource.detailList)
			{
				string[] array = detail.Split(',');
				DST_ZhuCeTipItem component2 = SetListContentParent(Panel.ScrollContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
				if (array.Length == 2)
				{
					component2?.SetValue(array[0], array[1], "");
				}
				else if (array.Length == 3)
				{
					component2?.SetValue(array[0], array[1], array[2]);
				}
			}
			SetGrticalListContentHight(m_DataSource.detailList.Count, Panel.ScrollContent, component);
		}

		private void ZhuCeBtnClick()
		{
			if (string.IsNullOrEmpty(Panel.NameText.text) || Panel.NameText.text.Length == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入名称！");
				return;
			}
			for (int i = 0; i < Panel.NameText.text.Length; i++)
			{
				if (Panel.NameText.text[i] <= '\u007f')
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请输入汉字！");
					return;
				}
			}
			if (m_DataSource.QueRenBtnClickAction != null)
			{
				m_DataSource.QueRenBtnClickAction(Panel.NameText.text);
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
