using System.Collections.Generic;
using System.Linq;
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
			InkInventoryArt.StyleLedger(Panel.ScrollView.transform);
			InkInventoryArt.StyleLabel(Panel.TitleText, 16);
			InkDialogArt.StyleButton(Panel.MainBtn, "JadeButton", InkDialogArt.Ivory);
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
			Panel.MainBtn.interactable = m_CompeleteAction != null;
			CreateDataList();
		}

		private void CreateDataList()
		{
			var content = Panel.ScrollViewContent;
			foreach (Transform old in content)
			{
				old.gameObject.SetActive(false);
				Object.Destroy(old.gameObject);
			}
			if (m_DataList == null) return;
			var prefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			var rows = m_DataList.Select(line => line.Split(new[] { ',' }, 3)).Where(row => row.Length >= 2);
			int index = 0;
			foreach (var row in rows.OrderBy(row => DetailOrder(row[0])))
			{
				var view = SetListContentParent(content, prefab).GetComponent<NaJieTipItem>();
				view.SetDetailItem(row[0], row[1], row.Length == 3 ? row[2] : string.Empty, index++);
			}
			InkDialogArt.FitDetailRows(Panel.ScrollView);
		}

		private static int DetailOrder(string title)
		{
			switch (title)
			{
			case "穿戴部位": case "部位效果": case "当前装备": case "使用限制": case "物品锁": return -1;
			case "基础攻击": case "基础防御": return 0;
			case "五行共鸣": case "词条": case "材纹": return 1;
			default: return 2;
			}
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
