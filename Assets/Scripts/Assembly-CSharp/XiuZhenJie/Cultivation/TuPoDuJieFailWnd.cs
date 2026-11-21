using UnityEngine;

namespace XiuZhenJie
{
	public class TuPoDuJieFailWnd : Window
	{
		public TuPoDuJieFailPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<TuPoDuJieFailPanel>();
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtn);
			AddButtonClickListener(Panel.JingXueTitleButton, JingXueTitlXiaoWenHao);
			AddButtonClickListener(Panel.ZhenYuanTitleTButton, ZhenYuanTitleTXiaoWenHao);
			AddButtonClickListener(Panel.TiPoTitleButton, TiPoTitleXiaoWenHao);
			AddButtonClickListener(Panel.ShenShiTitleButton, ShenShiTitleXiaoWenHao);
			AddButtonClickListener(Panel.ShouYuanTitleButton, ShouYuanTitleXiaoWenHao);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0 && paralist[0] is RoleInfoEntity roleInfoEntity)
			{
				Init(roleInfoEntity);
			}
		}

		private void Init(RoleInfoEntity roleInfoEntity)
		{
			RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
			Panel.ZhenYuanOldText.text = roleInfoEntity.SuperAir.ToString("0");
			Panel.TiPoOldText.text = roleInfoEntity.Physique.ToString("0");
			Panel.ShenShiOldText.text = roleInfoEntity.Perception.ToString("0");
			Panel.JingXueOldText.text = roleInfoEntity.PureBlood.ToString("0");
			Panel.ShouYuanOldText.text = roleInfoEntity.MaxAge.ToString("0");
			Panel.ZhenYuanNewText.text = role_Entity.CurrentSuperAir.ToString("f0");
			Panel.TiPoNewText.text = role_Entity.CurrentPhysique.ToString("f0");
			Panel.ShenShiNewText.text = role_Entity.CurrentPerception.ToString("f0");
			Panel.JingXueNewText.text = role_Entity.CurrentPureBlood.ToString("f0");
			Panel.ShouYuanNewText.text = role_Entity.MaxAge.ToString("0");
			Panel.JingXueDownIcon.gameObject.SetActive(Mathf.FloorToInt(role_Entity.CurrentPureBlood) < Mathf.FloorToInt(role_Entity.PureBlood));
			Panel.ShenShiDownIcon.gameObject.SetActive(Mathf.FloorToInt(role_Entity.CurrentPerception) < Mathf.FloorToInt(role_Entity.Perception));
			Panel.TiPoDownIcon.gameObject.SetActive(Mathf.FloorToInt(role_Entity.CurrentPhysique) < Mathf.FloorToInt(role_Entity.Physique));
			Panel.ZhenQiDownIcon.gameObject.SetActive(Mathf.FloorToInt(role_Entity.CurrentSuperAir) < Mathf.FloorToInt(role_Entity.SuperAir));
			Panel.ShouYuanDownIcon.gameObject.SetActive(Mathf.FloorToInt(role_Entity.CurrentAge) < Mathf.FloorToInt(role_Entity.MaxAge));
		}

		private void QueRenBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("TuPoDuJieWnd.prefab");
			if (Panel.TitleContentText.text != "重伤")
			{
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DongFuScene");
			}
		}

		private void JingXueTitlXiaoWenHao()
		{
			string text = "锻体或者服用恢复精血的丹药，可以将受损的精血弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "精血");
		}

		private void ZhenYuanTitleTXiaoWenHao()
		{
			string text = "纳气或者服用恢复灵气的丹药，可以将受损的灵气弥补回来，若不移除重伤状态，将无法再次渡劫";
			string text2 = "灵气";
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 30 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				text2 = "真气";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 60 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 91)
			{
				text2 = "真元";
			}
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, text2);
		}

		private void TiPoTitleXiaoWenHao()
		{
			string text = "锻体或者服用恢复体魄的丹药，可以将受损的体魄弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "体魄");
		}

		private void ShenShiTitleXiaoWenHao()
		{
			string text = "炼神或者服用恢复神识的丹药，可以将受损的神识弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "神识");
		}

		private void ShouYuanTitleXiaoWenHao()
		{
			string text = "服用恢复寿元的丹药，可以将受损的寿元弥补回来，若不移除重伤状态，将无法再次渡劫";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "寿元");
		}
	}
}
