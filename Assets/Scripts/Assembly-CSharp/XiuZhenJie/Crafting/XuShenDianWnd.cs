using System.Collections.Generic;

namespace XiuZhenJie
{
	public class XuShenDianWnd : Window
	{
		public XuShenDianPanel _panel;

		public Queue<string> MessageRadioQueue = new Queue<string>();

		private bool isRolling;

		private VoidRealmHallDoor hallDoor;
		private VoidTempleReading reading;
		private bool oldDoors;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<XuShenDianPanel>();
			hallDoor = new VoidRealmHallDoor(this);
			InkCraftHalls.StyleDirectory(_panel.transform, "虚神殿",
				(_panel.DJSD_btn, "道境圣殿", "功法注册与兑换"),
				(_panel.WFG_btn, "万法阁", "法术注册与兑换"),
				(_panel.DST_btn, "丹师塔", "丹方与丹师考核"),
				(_panel.LianQI, "炼器协会", "器图与工坊"),
				(_panel.SFH_btn, "神符会", "符书创作与兑换"),
				(_panel.ZFXH_btn, "阵法协会", "阵图创作与兑换"),
				(_panel.LZXH_btn, "灵植协会", "灵植培育与注册"));
			AddButtonClickListener(_panel.FanHui, FanHuiBtnClick);
			AddButtonClickListener(_panel.DST_btn, DanShiTaEnter);
			AddButtonClickListener(_panel.WFG_btn, WanFaGeEnter);
			AddButtonClickListener(_panel.SFH_btn, ShenFuHuiEnter);
			AddButtonClickListener(_panel.LZXH_btn, LingZhiXieHuiEnter);
			AddButtonClickListener(_panel.ZFXH_btn, ZhenFaXieHuiEnter);
			AddButtonClickListener(_panel.LianQI, LianQIXieHuiEnter);
			AddButtonClickListener(_panel.DJSD_btn, DaoJingShengDianEnter);
		}

        public override void OnShow(params object[] paralist)
        {
            base.OnShow(paralist);
            oldDoors = false;
            if (reading == null)
                reading = new VoidTempleReading(this, () => Singleton<UIManager>.Instance.CloseWnd(this),
                    () => oldDoors = true, id => OpenArchive(id, false), id => OpenArchive(id, true));
            reading.Home();
        }

        private void OpenArchive(string id, bool work)
        {
            var main = Singleton<UIManager>.Instance.FindWndByName<XuShenJieWnd>("XuShenJieWnd.prefab");
            var view = main?.Panel.GetComponentInChildren<VoidRealmView>(true);
            if (view == null) { MonoSingleton<GameStart>.Instance.PopUpAttention("请从虚神界大厅进入虚神殿。"); return; }
            Singleton<UIManager>.Instance.CloseWnd(this);
            if (work) view.OpenTempleWork(id); else view.OpenTempleWorkshop(id);
        }

		private void DanShiTaEnter()
		{
			hallDoor.Open("dan");
		}

		private void WanFaGeEnter()
		{
			hallDoor.Open("shu");
		}

		private void DaoJingShengDianEnter()
		{
			hallDoor.Open("dao");
		}

		private void ShenFuHuiEnter()
		{
			hallDoor.Open("fu");
		}

		private void LingZhiXieHuiEnter()
		{
			hallDoor.Open("zhi");
		}

		private void ZhenFaXieHuiEnter()
		{
			hallDoor.Open("zhen");
		}

		private void LianQIXieHuiEnter()
		{
			hallDoor.Open("qi");
		}

		public override void OnDisable()
		{
			hallDoor.Cancel();
			reading?.Hide();
			base.OnDisable();
		}

		private void FanHuiBtnClick()
		{
			if (oldDoors) { oldDoors=false; reading.Home(); return; }
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
