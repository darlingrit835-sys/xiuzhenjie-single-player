using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuShenJieWnd : Window
	{
		public XuShenJiePanel Panel;

		private VoidRealmView realmView;
		private VoidRealmHallDoor hallDoor;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<XuShenJiePanel>();
			hallDoor = new VoidRealmHallDoor(this);
			InkCraftHalls.StyleDirectory(Panel.transform, "虚神界",
				(Panel.XuShenDianBtn, "虚神殿", "功法、术法与百工"),
				(Panel.MuCangShanBtn, "虚神塔", "闯塔与双榜"),
				(Panel.XuShenJiTanBtn, "虚神祭坛", "献卷求经"),
				(Panel.HeDaoTaiBtn, "合道台", "双卷合参"));
			AddButtonClickListener(Panel.BackBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.XuShenDianBtn, EnterXuShenDian);
			AddButtonClickListener(Panel.MuCangShanBtn, EnterXuShenTa);
			AddButtonClickListener(Panel.XuShenJiTanBtn, EnterXuShenJiTan);
			Panel.HeDaoTaiBtn.interactable = true;
			AddButtonClickListener(Panel.HeDaoTaiBtn, EnterHeDaoTai);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (realmView == null)
			{
				Font font = Panel.GetComponentInChildren<Text>(true)?.font;
				realmView = VoidRealmView.Open(Panel.transform, font, LiKaiBtnClick,
					EnterXuShenDian, EnterXuShenTa, EnterXuShenJiTan, EnterHeDaoTai, hallDoor.Open);
			}
		}

		public override void OnDisable()
		{
			hallDoor.Cancel();
			base.OnDisable();
		}

		public override void OnClose()
		{
			if (realmView != null)
			{
				realmView.gameObject.SetActive(false);
				Object.Destroy(realmView.gameObject);
				realmView = null;
			}
			base.OnClose();
		}

		private void EnterXuShenDian()
		{
			hallDoor.Cancel();
			Singleton<UIManager>.Instance.PopUpWnd("XuShenDianWnd.prefab", true);
		}

		private void EnterXuShenTa()
		{
			hallDoor.Cancel();
			Singleton<UIManager>.Instance.PopUpWnd("XuShenTaWnd.prefab", true);
		}

		private void EnterXuShenJiTan()
		{
			hallDoor.Cancel();
			Singleton<UIManager>.Instance.PopUpWnd("XuShenJiTanWnd.prefab", true);
		}

		private void EnterHeDaoTai()
		{
			hallDoor.Cancel();
			Singleton<UIManager>.Instance.PopUpWnd("HeDaoTaiWnd.prefab", true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
