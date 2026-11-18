using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiTipWnd : Window
	{
		public RoleZhuangBeiTipPanel Panel;

		public List<string> m_DataList;

		public string m_Name;

		public string m_BtnName;

		public int m_Type;

		public UnityAction<object> m_CompeleteAction;

		private object m_Arg;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<RoleZhuangBeiTipPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.MainBtn, QueRenBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_DataList = paralist[0] as List<string>;
			m_Name = paralist[1].ToString();
			m_BtnName = paralist[2].ToString();
			m_Type = (int)paralist[3];
			m_CompeleteAction = paralist[4] as UnityAction<object>;
			m_Arg = paralist[5];
			Panel.TitleText.text = m_Name;
			Panel.MainBtnText.text = m_BtnName;
			CreateDataList();
		}

		private void CreateDataList()
		{
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			if (m_DataList == null)
			{
				return;
			}
			foreach (string data in m_DataList)
			{
				NaJieTipItem component = SetListContentParent(Panel.ScrollViewContent, gameObject).GetComponent<NaJieTipItem>();
				string[] array = data.Split(',');
				if (array.Length == 2)
				{
					component.SetItem(array[0], array[1], "");
				}
				else if (array.Length == 3)
				{
					component.SetItem(array[0], array[1], array[2]);
				}
			}
			VerticalLayoutGroup component2 = Panel.ScrollViewContent.gameObject.GetComponent<VerticalLayoutGroup>();
			int itemCount = ((m_DataList == null) ? 1 : m_DataList.Count);
			Panel.ScrollViewContent.sizeDelta = new Vector2(Panel.ScrollViewContent.sizeDelta.x, SetVerticalListContentHight(itemCount, y, component2));
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenBtnClick()
		{
			if (m_CompeleteAction != null)
			{
				m_CompeleteAction(m_Arg);
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
