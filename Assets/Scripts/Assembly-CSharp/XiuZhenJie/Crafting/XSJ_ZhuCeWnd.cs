using UnityEngine;

namespace XiuZhenJie
{
	public class XSJ_ZhuCeWnd : Window
	{
		public XSJ_ZhuCePanel Panel;

		public XSJ_ZhuCeWndModel<object> m_DataSource;

		private GameObject m_YiZhuCeItemObj;

		private GameObject m_DaiShenHeItemObj;

		private GameObject m_KeZhuCeItemObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<XSJ_ZhuCePanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as XSJ_ZhuCeWndModel<object>;
			}
			Init();
		}

		private void Init()
		{
			if (m_DataSource != null)
			{
				Panel.LevelNameText.text = m_DataSource.LevelInfo;
				Panel.GongXianDianText.text = m_DataSource.GongXianDian;
				Panel.WndTitleText.text = m_DataSource.TitleText;
				InitYiZhuCe();
				InitDaiShenHe();
				InitKeZhuCe();
			}
		}

		private void InitYiZhuCe()
		{
		}

		private void InitDaiShenHe()
		{
		}

		private void InitKeZhuCe()
		{
		}

		private void ClearDaiShenHe()
		{
			foreach (Transform item in Panel.DaiShenHeListContent)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void ClearKeZhuCe()
		{
			foreach (Transform item in Panel.KeZhuCeListContent)
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void ZhuCeSuccess(string name, string newName)
		{
			int num = 0;
			object obj = null;
			for (int i = 0; i < m_DataSource.KeZhuCeItemList.Count; i++)
			{
				if (m_DataSource.KeZhuCeItemList[i] == name)
				{
					num = i;
					break;
				}
			}
			for (int j = 0; j < m_DataSource.KeZhuCeDataSource.Count; j++)
			{
				if (num == j)
				{
					obj = m_DataSource.KeZhuCeDataSource[j];
					break;
				}
			}
			m_DataSource.KeZhuCeItemList.RemoveAt(num);
			m_DataSource.KeZhuCeDataSource.RemoveAt(num);
			if (obj != null)
			{
				m_DataSource.DaiShenHeDataSource.Add(obj);
				m_DataSource.DaiShenHeItemList.Add(newName);
			}
			ClearKeZhuCe();
			ClearDaiShenHe();
			InitDaiShenHe();
			InitKeZhuCe();
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string titleText = m_DataSource.TitleText;
			if (!(titleText == "注册功法"))
			{
				if (titleText == "法术注册")
				{
					string text = $"当某一种法术创造成功后，创造者有权注册此法术专利信息。除去命名权、注册贡献点，当其他术师兑换该法术时，也能获得贡献点奖励。在所有已注册的法术中，创造者每天可以选一个进行抄录，抄录的法术通过邮件寄出，并且不会进行灵魂绑定，可以在市场自由交易。";
					Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "法术注册");
				}
			}
			else
			{
				string text2 = $"当某一种功法创造成功后，创造者有权注册此功法专利信息。除去命名权、注册贡献点，当其他布道师兑换该功法时，也能获得贡献点奖励。在所有已注册的功法中，创造者每三天可以选一个进行抄录，抄录的功法通过邮件寄出，并且不会进行灵魂绑定，可以在市场自由交易。";
				Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text2, "功法注册");
			}
		}
	}
}
