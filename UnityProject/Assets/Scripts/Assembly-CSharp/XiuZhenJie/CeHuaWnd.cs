using System.Collections.Generic;
using DG.Tweening;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class CeHuaWnd : Window
	{
		public CeHuaPanel _panel;

		private RectTransform tanRect;

		private RoleInfoEntity roleInfo;

		private Dictionary<RoleFuZhiType, CeHuaItem> m_FuZhiItemDic;

		private GameObject m_CeHuaFuZhiItemObj;

		private List<GameObject> m_FuZhiGameObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<CeHuaPanel>();
			m_FuZhiItemDic = new Dictionary<RoleFuZhiType, CeHuaItem>();
			m_CeHuaFuZhiItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/CeHuaItem.prefab");
			m_FuZhiGameObj = new List<GameObject>();
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

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			roleInfo = Singleton<RoleManager>.Instance.Role_Entity;
			tanRect = base.GameObject.GetComponent<RectTransform>();
			reflash();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			reflash();
		}

		private void reflash()
		{
			roleInfo = Singleton<RoleManager>.Instance.Role_Entity;
			Sprite currentRoleSprite = Singleton<RoleManager>.Instance.GetCurrentRoleSprite();
			if (currentRoleSprite != null)
			{
				_panel.TouXiang_img.sprite = currentRoleSprite;
			}
			_panel.RoleName_text.text = roleInfo.RoleName;
			_panel.JianRenValue_text.text = $"{roleInfo.CurrentPhysique}";
			_panel.JingJieValue_text.text = $"{WorldSetting.GetRoleLevelNameByRoleLevel(roleInfo.RoleLevel)}({roleInfo.RoleLevel.ToString()})";
			_panel.JingXueValue_text.text = $"{roleInfo.CurrentPureBlood}";
			if (roleInfo.RoleLevel < 31)
			{
				_panel.ZhenYuanTitle_text.text = "灵气：";
				_panel.ZhenYuanValue_text.text = $"{roleInfo.CurrentSuperAir}";
			}
			else if (roleInfo.RoleLevel >= 31 && roleInfo.RoleLevel < 61)
			{
				_panel.ZhenYuanTitle_text.text = "真气：";
				_panel.ZhenYuanValue_text.text = $"{roleInfo.CurrentSuperAir / 100}";
			}
			else if (roleInfo.RoleLevel >= 61)
			{
				_panel.ZhenYuanTitle_text.text = "真元：";
				_panel.ZhenYuanValue_text.text = $"{roleInfo.CurrentSuperAir / 100 / 100}";
			}
			_panel.ShouYuanValu_text.text = string.Format("{0}/{1}", roleInfo.CurrentAge, roleInfo.MaxAge.ToString("0"));
			_panel.ShenShiValue_text.text = $"{roleInfo.CurrentPerception}";
			_panel.DanDuValue_text.text = $"{roleInfo.CurrentPoisonVal}";
			_panel.HunLiValue_text.text = $"{roleInfo.HunLiVal}";
			if (roleInfo.ExtendQiYun <= 0)
			{
				_panel.QiYunValue_text.text = $"{roleInfo.QiYun}";
			}
			else
			{
				_panel.QiYunValue_text.text = $"{roleInfo.QiYun}({roleInfo.ExtendQiYun})";
			}
			_panel.WuXingValue_text.text = $"{roleInfo.CurrentClever}/{roleInfo.Clever}";
			_panel.JingLiValue_text.text = $"{roleInfo.CurrentVigor}/{roleInfo.Vigor}";
			_panel.TiLiValue_text.text = $"{roleInfo.CurrentPower}/{roleInfo.Power}";
			_panel.LingGenValue_text.text = roleInfo.FlexibleRoot.ToString();
			_panel.LingShiValue_text.text = roleInfo.ShinyStone.ToString();
			_panel.LingYuValue_text.text = roleInfo.ShinyJade.ToString();
			ReflashFuZhiYe();
			if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 11)
			{
				_panel.VIP_HeadFrame.gameObject.SetActive(value: true);
			}
			else
			{
				_panel.VIP_HeadFrame.gameObject.SetActive(value: false);
			}
		}

		private void ReflashFuZhiYe()
		{
			foreach (CeHuaItem value in m_FuZhiItemDic.Values)
			{
				Object.Destroy(value.gameObject);
			}
			m_FuZhiItemDic.Clear();
			if (Singleton<RoleManager>.Instance.Role_Entity.Live3Type != 0)
			{
				RoleFuZhiType live3Type = (RoleFuZhiType)Singleton<RoleManager>.Instance.Role_Entity.Live3Type;
				if (m_FuZhiItemDic.ContainsKey(live3Type))
				{
					m_FuZhiItemDic[live3Type].Reflash();
				}
				else
				{
					CeHuaItem component = Object.Instantiate(m_CeHuaFuZhiItemObj, _panel.FuZhiContent).GetComponent<CeHuaItem>();
					component.SetItem(live3Type);
					m_FuZhiItemDic.Add(live3Type, component);
				}
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.Live4Type != 0)
			{
				RoleFuZhiType live4Type = (RoleFuZhiType)Singleton<RoleManager>.Instance.Role_Entity.Live4Type;
				if (m_FuZhiItemDic.ContainsKey(live4Type))
				{
					m_FuZhiItemDic[live4Type].Reflash();
				}
				else
				{
					CeHuaItem component2 = Object.Instantiate(m_CeHuaFuZhiItemObj, _panel.FuZhiContent).GetComponent<CeHuaItem>();
					component2.SetItem(live4Type);
					m_FuZhiItemDic.Add(live4Type, component2);
				}
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.Live1Type != 0)
			{
				RoleFuZhiType live1Type = (RoleFuZhiType)Singleton<RoleManager>.Instance.Role_Entity.Live1Type;
				if (m_FuZhiItemDic.ContainsKey(live1Type))
				{
					m_FuZhiItemDic[live1Type].Reflash();
				}
				else
				{
					CeHuaItem component3 = Object.Instantiate(m_CeHuaFuZhiItemObj, _panel.FuZhiContent).GetComponent<CeHuaItem>();
					component3.SetItem(live1Type);
					m_FuZhiItemDic.Add(live1Type, component3);
				}
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.Live2Type != 0)
			{
				RoleFuZhiType live2Type = (RoleFuZhiType)Singleton<RoleManager>.Instance.Role_Entity.Live2Type;
				if (m_FuZhiItemDic.ContainsKey(live2Type))
				{
					m_FuZhiItemDic[live2Type].Reflash();
					return;
				}
				CeHuaItem component4 = Object.Instantiate(m_CeHuaFuZhiItemObj, _panel.FuZhiContent).GetComponent<CeHuaItem>();
				component4.SetItem(live2Type);
				m_FuZhiItemDic.Add(live2Type, component4);
			}
		}

		public void TanChuWnd()
		{
			tanRect.SetAsLastSibling();
			tanRect.DOLocalMoveX(0f, 0.5f);
			_panel.MaskPanel.gameObject.SetActive(value: true);
		}

		private void FanHuiBtnClick()
		{
			tanRect.DOLocalMoveX(-218f, 0.5f);
			_panel.MaskPanel.gameObject.SetActive(value: false);
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

		private void DuiHuanLingYuBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DuiHuanLingYuWnd.prefab", true);
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
			string text = "漫漫修道路，唯求长生耳。凡人寿数堪堪数十载，练气修士寿元仅百。每突破一个大境界，寿元将翻倍增长。正因此，提升境界成为众修士的终极追求。相传，突破大乘，飞升成仙，可与天地同寿。寿数尽者，皆坠轮回，化为尘埃。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "寿元");
		}

		private void PopUpZongMenDescription()
		{
			string text = "暂未开放!";
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
			string text2 = "灵气";
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				text2 = "灵气：";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				text2 = "真气：";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				text2 = "真元：";
			}
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, text2);
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
			string text = "修士虽然强大，但精力确是有限的，贵在专精与一道。炼制物品需要消耗大量精力，通过休息恢复精力。在线每五分钟恢复精力一点，百点为上限，购买玄天令后得天道眷顾，上限再次提升一百点。无论是大突破还是小突破，都会将精力恢复至满。离线时，精力依照离线收益时间计算收益。";
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

		private void HuoDongClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchSignInAward, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
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
			base.OnClose();
			foreach (Transform item in _panel.FuZhiContent)
			{
				Object.Destroy(item.gameObject);
			}
			m_FuZhiItemDic.Clear();
		}
	}
}
