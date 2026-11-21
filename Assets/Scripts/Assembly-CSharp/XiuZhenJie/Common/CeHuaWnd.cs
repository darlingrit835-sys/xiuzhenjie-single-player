using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class CeHuaWnd : Window
	{
		public CeHuaPanel _panel;

		private RoleSidebarView view;
		private bool isOpen;
		private int signInSequence;
		private int pendingSignIn;
		private GameObject professionPrefab;
		private Sprite defaultPortrait;
		private readonly List<CeHuaItem> professionRows = new List<CeHuaItem>(4);

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = GameObject.GetComponent<CeHuaPanel>();
			defaultPortrait = _panel.TouXiang_img.sprite;
			professionPrefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>(Constants.CEHUA_ITEM);
			view = new RoleSidebarView(_panel);
			view.SetOpen(false, false);
			AddButtonClickListener(_panel.FanHui_btn, FanHuiBtnClick);
			AddButtonClickListener(_panel.ChuanXin_btn, ChuanXinBtnClick);
			AddButtonClickListener(_panel.Mask_btn, FanHuiBtnClick);
			AddButtonClickListener(_panel.SheZhi_btn, SheZhiBtnClick);
			AddButtonClickListener(_panel.LingGen_InsBtn, PopUpLingGenDescription);
			AddButtonClickListener(_panel.JingJie_InsBtn, PopUpJingJieDescription);
			AddButtonClickListener(_panel.ShouYuan_InsBtn, PopUpShouYuanDescription);
			AddButtonClickListener(_panel.ZongMen_InsBtn, PopUpZongMenDescription);
			AddButtonClickListener(_panel.JingXue_InsBtn, PopUpJingXueDescription);
			AddButtonClickListener(_panel.ZhenYuan_InsBtn, PopUpZhenYuanDescription);
			AddButtonClickListener(_panel.ShenShi_InsBtn, PopUpShenShiDescription);
			AddButtonClickListener(_panel.TiPo_InsBtn, PopUpTiPoDescription);
			AddButtonClickListener(_panel.DanDu_InsBtn, PopUpDanDuDescription);
			AddButtonClickListener(_panel.JingLi_InsBtn, PopUpJingLiDescription);
			AddButtonClickListener(_panel.WuXing_InsBtn, PopUpWuXingDescription);
			AddButtonClickListener(_panel.TiLi_InsBtn, PopUpTiLiDescription);
			AddButtonClickListener(_panel.HunLi_InsBtn, PopUpHunLiDesription);
			AddButtonClickListener(_panel.QiYun_InsBtn, PopUpQiYunDesription);
			AddButtonClickListener(_panel.ChongShengBtn, ChongShenBtnClick);
			AddButtonClickListener(_panel.LingShi_btn, PopUpLingShiDescription);
			AddButtonClickListener(_panel.LingYu_btn, PopUpLingYuDescription);
			AddButtonClickListener(_panel.HuoDong_btn, HuoDongClick);
		}

		public override void ReFlashData()
		{
			if (isOpen)
				RefreshRole();
		}

		private void RefreshRole()
		{
			var manager = Singleton<RoleManager>.Instance;
			var role = manager.Role_Entity;
			_panel.RoleName_text.text = string.IsNullOrWhiteSpace(role?.RoleName) ? "修士" : role.RoleName;
			_panel.TouXiang_img.sprite = (role != null ? manager.GetCurrentRoleSprite() : null) ?? defaultPortrait;
			_panel.VIP_HeadFrame.gameObject.SetActive(role != null && role.VipLevel >= 11);
			_panel.ZhenYuanTitle_text.text = EnergyName(role?.RoleLevel ?? 0);
			_panel.JingJieValue_text.text = RealmName(role);
			_panel.LingGenValue_text.text = string.IsNullOrWhiteSpace(role?.FlexibleRoot) ? "—" : role.FlexibleRoot;
			_panel.ZongMenValue_text.text = role == null ? "—" : string.IsNullOrWhiteSpace(role.ZongMenName) ? "散修" : role.ZongMenName;
			_panel.ShouYuanValu_text.text = role == null ? "—" : $"{role.CurrentAge}/{role.MaxAge:0}";
			_panel.LingShiValue_text.text = role?.ShinyStone.ToString() ?? "—";
			_panel.LingYuValue_text.text = role?.ShinyJade.ToString() ?? "—";
			view.FitWallet();
			_panel.JingXueValue_text.text = role?.CurrentPureBlood.ToString() ?? "—";
			int scale = role == null || role.RoleLevel < 31 ? 1 : role.RoleLevel < 61 ? 100 : 10000;
			_panel.ZhenYuanValue_text.text = role == null ? "—" : (role.CurrentSuperAir / scale).ToString();
			_panel.ShenShiValue_text.text = role?.CurrentPerception.ToString() ?? "—";
			_panel.JianRenValue_text.text = role?.CurrentPhysique.ToString() ?? "—";
			_panel.DanDuValue_text.text = role?.CurrentPoisonVal.ToString() ?? "—";
			_panel.HunLiValue_text.text = role?.HunLiVal.ToString() ?? "—";
			_panel.QiYunValue_text.text = role == null ? "—" : role.ExtendQiYun > 0 ? $"{role.QiYun} (+{role.ExtendQiYun})" : role.QiYun.ToString();
			_panel.WuXingValue_text.text = role == null ? "—" : $"{role.CurrentClever}/{role.Clever}";
			_panel.JingLiValue_text.text = role == null ? "—" : $"{role.CurrentVigor}/{role.Vigor}";
			_panel.TiLiValue_text.text = role == null ? "—" : $"{role.CurrentPower}/{role.Power}";
			RefreshProfessions(role);
		}

		private static string EnergyName(int level)
		{
			return level < 31 ? "灵气" : level < 61 ? "真气" : "真元";
		}

		private static string RealmName(RoleInfoEntity role)
		{
			if (role == null || role.RoleLevel <= 0)
				return "—";
			var levels = Singleton<FacadeManager>.Instance.SelectRoleLevelInfo();
			string level = role.RoleLevel.ToString();
			if (levels != null && levels.TryGetValue(level, out var entry) && !string.IsNullOrWhiteSpace(entry.level))
				return entry.level;
			return "等级 " + level;
		}

		private void RefreshProfessions(RoleInfoEntity role)
		{
			int count = 0;
			if (role != null)
			{
				BindProfession(role.Live3Type, role.Live3Level, role.Live3Value, ref count);
				BindProfession(role.Live4Type, role.Live4Level, role.Live4Value, ref count);
				BindProfession(role.Live1Type, role.Live1Level, role.Live1Value, ref count);
				BindProfession(role.Live2Type, role.Live2Level, role.Live2Value, ref count);
			}
			for (int i = count; i < professionRows.Count; i++)
				professionRows[i].gameObject.SetActive(false);
			view.SetProfessionCount(count);
		}

		private void BindProfession(int type, int level, int exp, ref int count)
		{
			if (type < (int)RoleFuZhiType.LianDan || type > (int)RoleFuZhiType.YuShou)
				return;
			var profession = (RoleFuZhiType)type;
			for (int i = 0; i < count; i++)
				if (professionRows[i].Profession == profession)
					return;
			if (count == professionRows.Count)
				professionRows.Add(Object.Instantiate(professionPrefab, _panel.FuZhiContent, false).GetComponent<CeHuaItem>());
			var row = professionRows[count++];
			row.gameObject.SetActive(true);
			row.SetItem(profession, level, exp);
		}

		public void TanChuWnd()
		{
			GameObject.transform.SetAsLastSibling();
			RefreshRole();
			if (isOpen)
				return;
			isOpen = true;
			view.SetOpen(true, true);
		}

		private void FanHuiBtnClick()
		{
			if (!isOpen)
				return;
			isOpen = false;
			CancelPendingSignIn();
			view.SetOpen(false, true);
		}

		public override void OnDisable()
		{
			isOpen = false;
			CancelPendingSignIn();
			view.SetOpen(false, false);
			base.OnDisable();
		}

		private void ChongShenBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChongShengWnd.prefab", true);
		}

		public void SheZhiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("OptionWnd.prefab", true);
		}

		public void ChuanXinBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChuanXinWnd.prefab", true);
		}

		private void PopUpJingJieDescription()
		{
			string text = $"修真界修士九大境界：练气、筑基、结丹、元婴、分神、炼虚、合体、渡劫、大乘。每个大境界分十个小境界，一至三层称前期，四至六层称中期，七至九层称后期，十层称为大圆满。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "境界");
		}

		private void PopUpLingGenDescription()
		{
			string text = "灵根是修士天生属性，灵根为纳，在五行灵气中，有此灵根方可纳该属性灵气为己用。灵根总值为百，各灵根均分此数，不同灵根数的修士具有不同的优势。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵根");
		}

		private void PopUpShouYuanDescription()
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			int level = role.RoleLevel;
			int years = level <= 1 || level >= 90 ? 0 : 4 << ((level - 1) / 10);
			string text = $"现年{role.CurrentAge}岁，寿限{role.MaxAge}岁。本层冲关耗{years}年。三路修行每累计两小时有效行功长一岁，兼修同时只计一份。每升一境，基础寿限翻倍。余寿只够本境冲关时会停下行功，可用历练、修为丹或传功补修为，也可服寿元丹续修。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "寿元");
		}

		private void PopUpZongMenDescription()
		{
			string text = "宗门是修士所属的门派。加入后，这里会显示宗门名称；尚未加入宗门的修士称为散修。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "宗门");
		}

		private void PopUpLingShiDescription()
		{
			string text = "灵石是一种修行资源，也因为其稀缺性成为了修真界的通用货币单位。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵石");
		}

		private void PopUpLingYuDescription()
		{
			string text = "灵玉是灵石受到巨量灵气滋养，形成的一种特殊玉质化灵石。由于其数量之稀少，可以兑换珍惜修行资源，第一商会玄天楼出售各种珍惜资源，回收灵玉。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵玉");
		}

		private void PopUpJingXueDescription()
		{
			string text = "精血是修士生命力的根本，当修士的精血耗尽，便意味着死亡，修真者只修今生不求来世，保护好精血不受太大损伤，是重中之重。为确保修士性命安全，精血低于8点，将无法进入历练地图。当精血减少后，每一次小突破可以恢复当前精血，锻体亦可以恢复当前精血，服食恢复精血类丹药也有同样效果。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "精血");
		}

		private void PopUpZhenYuanDescription()
		{
			string text = "修士通过纳灵气入下丹田进行修炼，每突破境界后下丹田可容纳灵气会剧增。元婴后，灵气凝为实质，称为真气，每一点真气与一百点灵气相当。合体后，真气凝练为真元，每一点真元与一百点真气相当。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, _panel.ZhenYuanTitle_text.text);
		}

		private void PopUpShenShiDescription()
		{
			string text = "神识是修士五感的延伸，大大强于五感。修士通过神识观察、探测，也通过神识操控、御物、御法。神识可以增加探测距离、锁敌距离、法器操控数量等等，亦可用于炼丹。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "神识");
		}

		private void PopUpTiPoDescription()
		{
			string text = "体魄的强弱，决定了自身的防御能力，体魄越强，防御值越高。进一步提升体魄效能的方式是穿戴强大的防具，这些防具会提供额外的防御值。修士恢复体魄损伤需要闭关，在外历练时无法进行恢复，故在外历练时，需时刻关注自身防御能力。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "体魄");
		}

		private void PopUpDanDuDescription()
		{
			string text = "修士可以通过服用丹药、炼化丹药快速修行，但丹药效能强大的同时亦存在大量毒副作用。淤积丹毒后，会导致修行效率骤降。幸有上界流传至本界的解毒神丹，可以解除本界丹药中蕴含的丹毒。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "丹毒");
		}

		private void PopUpJingLiDescription()
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			int cost = role.RoleLevel <= 1 || role.RoleLevel >= 90 ? 0 : 2 + (role.RoleLevel - 1) / 10;
			string text = $"当前精力{role.CurrentVigor}/{role.Vigor}，本层冲关耗{cost}点。在线与离线均每两分钟回复一点，达到上限后停止。制丹、制符、铸器、演法和历练共用精力。用丹回精，或用已掌握的方谱成批生产，可以腾出精力做别的事。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "精力");
		}

		private void PopUpTiLiDescription()
		{
			string text = "修士虽然强大，但体力也是有限的。行动或战斗需要消耗大量体力，每次进入历练，会消耗十点体力，进入秘境消耗二十点体力。每五分钟恢复体力一点，百点为上限，购买玄天令后得天道眷顾，上限再次提升一百点。无论是大突破还是小突破，都会将体力恢复至满。离线时，体力依照离线收益时间计算收益。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "体力");
		}

		private void PopUpWuXingDescription()
		{
			string text = "修士虽然强大，但悟性也是有限的。创造法术、功法、法器、灵植、符箓等都会消耗相应的悟性。每五分钟恢复悟性一点，百点为上限，购买玄天令后得天道眷顾，上限再次提升一百点。无论是大突破还是小突破，都会将悟性恢复至满。离线时，悟性依照离线收益时间计算收益。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "悟性");
		}

		private void PopUpHunLiDesription()
		{
			string text = "灵魂能量越强，则可以学习的书籍、添加的好友数量、灵田中育种室可保留的种子数量越多，每5点魂力可以增加一点数量（可遗忘），每升一级魂力能上涨1点，或可去玄天楼购买魂力丹。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "魂力");
		}

		private void PopUpQiYunDesription()
		{
			string text = "修真世界中气运非常重要，气运越高则获取高级物品的几率越高，但其作用是有限的，更重要的还是坚韧的性格。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "气运");
		}

		private void CancelPendingSignIn()
		{
			if (pendingSignIn == 0)
				return;
			pendingSignIn = 0;
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
		}

		private void HuoDongClick()
		{
			int request = ++signInSequence;
			pendingSignIn = request;
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchSignInAward, null, (object _obj) =>
			{
				if (pendingSignIn != request)
					return;
				CancelPendingSignIn();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchSignInAwardResp_1002018 searchSignInAwardResp_ = new SearchSignInAwardResp_1002018();
					searchSignInAwardResp_.MergeFrom(byteString);
					if (searchSignInAwardResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("QianDaoWnd.prefab", true, searchSignInAwardResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchSignInAwardResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnClose()
		{
			CancelPendingSignIn();
			foreach (var row in professionRows)
			{
				row.gameObject.SetActive(false);
				Object.Destroy(row.gameObject);
			}
			professionRows.Clear();
			base.OnClose();
		}
	}
}
