using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class RoleZhuangBeiWnd : Window
	{
		public RoleZhuangBeiPanel Panel;

		private GameObject m_InstanceObj;

		private bool m_FieldLayoutReady;
		private Text m_EmptySkills;

		private GameObject m_XiangQingInstanceObj;

		private EquipmentVestmentView m_Vestments;

		internal Transform GuideEquipmentButton => m_Vestments?.InventoryButton?.transform;

		private RoleZhuangBeiType m_RoleZhuangBeiType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, Sprite> m_SkillIconSprite;

		private List<UI_GoodsItem> m_PeiZhiList;

		private Dictionary<string, UI_GoodsItem> m_CanPeiZhiList;


		private XzjBoxEntity m_XzjBoxEntity;

		private List<BaseNaJieGoodEntity> m_SkillList;

		private Dictionary<XiangQingType, string> SuperAirZongLanXiangQing;

		private Dictionary<XiangQingType, string> TiPoZongLanXiangQing;

		private Dictionary<XiangQingType, string> ShenShiZongLanXiangQing;

		private Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem> ZhuangBeiXiangQingDic;

		private Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem> JiNengXiangQingDic;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<RoleZhuangBeiPanel>();
			SuperAirZongLanXiangQing = new Dictionary<XiangQingType, string>();
			TiPoZongLanXiangQing = new Dictionary<XiangQingType, string>();
			ShenShiZongLanXiangQing = new Dictionary<XiangQingType, string>();
			ZhuangBeiXiangQingDic = new Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem>();
			JiNengXiangQingDic = new Dictionary<XiangQingType, RoleZhuangBeiXiangQingItem>();
			m_SkillList = new List<BaseNaJieGoodEntity>();
			m_PeiZhiList = new List<UI_GoodsItem>();
			m_CanPeiZhiList = new Dictionary<string, UI_GoodsItem>();
			m_XzjBoxEntity = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			AddButtonClickListener(Panel.XiaoWenHao, WenHaoTogClick);
			AddToggleClickListener(Panel.ZhuangBeiTog, ZhuangBeiShow);
			AddToggleClickListener(Panel.JiNengTog, JiNengShow);









			AddToggleClickListener(Panel.JiNengRect.Tog_Level_1, FaShuTogClick);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_2, FaQiTogClick);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_3, FuLuTogClick);
			AddToggleClickListener(Panel.JiNengRect.Tog_Level_4, ZhenPanTogClick);
			m_RoleZhuangBeiType = RoleZhuangBeiType.DefencePanel;
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_XiangQingInstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiShuXingContentItem.prefab");
			ResourceLoad();

		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (!m_FieldLayoutReady)
			{
				StyleEquipment();
				m_FieldLayoutReady = true;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu < 2)
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 2;
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
			}
			JiNengInit();
			GetRoleChuanDaiZhuangBei();
		}

		private void StyleEquipment()
		{
			StyleEquipmentPage(Panel.ZhuangBeiRect.transform, "护身行装");
			m_Vestments = EquipmentVestmentView.Build(Panel.ZhuangBeiRect, Panel.ZhuangBeiTog.GetComponentInChildren<Text>(true).font,
			    BrowseEquipment, gear => ShowEquipmentDetail(gear, true));
			ForgeWorkshop.AttachLibraryEntry(Panel.ZhuangBeiRect.transform.Find("Title_image"), Panel.transform, ZhuangBeiReflashData);
			StyleEquipmentPage(Panel.JiNengRect.transform, "斗法配置");
			var skillHeader = Panel.JiNengRect.transform.Find("Title_image");
			var practicePlate = InkBattleHud.Make<Image>(skillHeader, "PracticeEntry");
			InkInterfaceArt.Set(practicePlate, "SealButton", false);
			var practiceRect = practicePlate.rectTransform;
			practiceRect.anchorMin = practiceRect.anchorMax = new Vector2(1, .5f);
			practiceRect.pivot = new Vector2(1, .5f);
			practiceRect.anchoredPosition = new Vector2(-42, 0);
			practiceRect.sizeDelta = new Vector2(76, 30);
			var practiceButton = practicePlate.gameObject.AddComponent<Button>();
			practiceButton.targetGraphic = practicePlate;
			var practiceText = InkBattleHud.Label(practiceRect, skillHeader.GetComponentInChildren<Text>().font,
				"Caption", "修习法术", 15, InkDialogArt.Ivory);
			practiceText.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(practiceText.rectTransform, 3, 3, 1, 1);
			AddButtonClickListener(practiceButton, () => Singleton<UIManager>.Instance.PopUpWnd("ZhanFaShiWnd.prefab", true));
			m_EmptySkills = InkBattleHud.Label(Panel.JiNengRect.GoodsScrollView.transform,
				Panel.JiNengTog.GetComponentInChildren<Text>(true).font,"EmptySkills","",15,InkInterfaceArt.Ink);
			m_EmptySkills.alignment = TextAnchor.MiddleCenter;
			InkDialogArt.Inset(m_EmptySkills.rectTransform,12,12,0,0);
			m_EmptySkills.gameObject.SetActive(false);
			string[] groupNames = {"一组","二组","三组"};
			for (int i = 0; i < Panel.JiNengRect.PeiZhiList.Count; i++)
			{
				var placeholder = Panel.JiNengRect.PeiZhiList[i].transform.Find("ItemImg").GetComponent<Image>();
				InkInterfaceArt.Set(placeholder,"JadeSelection",false);
				Text slot = InkBattleHud.Label(placeholder.transform,m_EmptySkills.font,"Position",
					"",12,InkInterfaceArt.SoftInk);
				slot.text = groupNames[Mathf.Min(i/3,2)] + "·" + (i%3+1);
				slot.alignment = TextAnchor.MiddleCenter;
				InkDialogArt.Inset(slot.rectTransform,1,1,0,0);
			}
			InkInventoryArt.SetSurface(Panel.transform.Find("BottomBg"));
			StyleEquipmentTab(Panel.ZhuangBeiTog, 16);
			StyleEquipmentTab(Panel.JiNengTog, 16);
			InkInterfaceArt.Set(Panel.XiaoWenHao.image, "Help");
			var help = (RectTransform)Panel.XiaoWenHao.transform;
			help.anchoredPosition = new Vector2(-22f, -25f);
			help.sizeDelta = new Vector2(28f, 28f);
			help.SetAsLastSibling();
			help.gameObject.SetActive(true);
			Panel.XiaoWenHao.image.color = InkBattleHud.Gold;
		}

		private static void StyleEquipmentPage(Transform page, string title)
		{
			InkInventoryArt.SetSurface(page);
			var header = (RectTransform)page.Find("Title_image");
			InkInterfaceArt.Set(header.GetComponent<Image>(), "JadeButton", false);
			InkBattleHud.Band(header,6,6,5,39);
			var caption = header.Find("TitleName").GetComponent<Text>();
			InkBattleHud.Tint(caption,19,InkDialogArt.Ivory);
			caption.text = title;
			InkDialogArt.Inset(caption.rectTransform,14,45,0,0);
			BattleHudReveal.Attach(header);
			var slots = page.Find("YiChuanDai/Scroll View").GetComponent<ScrollRect>();
			var book = (RectTransform)page.Find("YiChuanDai/ShuXing");
			var slotRect = (RectTransform)slots.transform;
			slotRect.anchorMin = Vector2.zero;
			slotRect.anchorMax = new Vector2(.59f,1f);
			slotRect.offsetMin = new Vector2(6,3);
			slotRect.offsetMax = new Vector2(-2,-3);
			book.anchorMin = new Vector2(.59f,0);
			book.anchorMax = Vector2.one;
			book.offsetMin = new Vector2(2,3);
			book.offsetMax = new Vector2(-6,-3);
			var stats = book.Find("Scroll View").GetComponent<ScrollRect>().content;
			stats.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			stats.sizeDelta = new Vector2(0,stats.sizeDelta.y);
			InkDialogArt.Inset(slots.viewport,0,0,0,0);
			var grid = slots.content.GetComponent<GridLayoutGroup>();
			float side = Mathf.Clamp((slotRect.rect.width-16)/3f,36,57);
			grid.cellSize = Vector2.one * side;
			grid.spacing = Vector2.one * 4f;
			grid.padding = new RectOffset(3,3,3,3);
			grid.childAlignment = TextAnchor.MiddleCenter;
			grid.enabled = true;
			InkInventoryArt.StyleLedger(page.Find("Scroll View"));
			InkInventoryArt.SetSurface(page.Find("StageContent"), "LightPlaque");
			Transform stages = page.Find("StageContent/Content");
			if (stages != null)
			{
				InkInventoryArt.SetSurface(stages);
				foreach (Transform stage in stages)
				{
					if (stage.TryGetComponent(out Toggle toggle))
						StyleEquipmentTab(toggle, 13);
				}
			}

			Transform overview = page.Find("YiChuanDai/ShuXing");
			InkInventoryArt.SetSurface(overview, "ParchmentHeading");
			InkInventoryArt.SetSurface(overview?.Find("Scroll View"), "InkLedgerPaper");
			Transform statHeader = overview?.Find("Title");
			InkInventoryArt.SetSurface(statHeader, "LightPlaque");
			InkInventoryArt.StyleLabel(statHeader?.Find("Text")?.GetComponent<Text>(), 12);
		}

		private static void StyleEquipmentTab(Toggle toggle, int minSize)
		{
			InkInterfaceArt.Set(toggle.targetGraphic as Image, "LightPaper", false);
			if (toggle.graphic is Image selection)
			{
				InkInterfaceArt.Set(selection, "LightPlaque", false);
				selection.color = new Color32(166, 205, 175, 255);
			}
			InkInventoryArt.StyleLabel(toggle.transform.Find("Text")?.GetComponent<Text>(), minSize);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			m_SkillIconSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_Xie.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FangJu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DefenceWeapon.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuLu.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenPan.png");
			m_GoodsIconSprite.Add(29, value);
			m_GoodsIconSprite.Add(15, value2);
			m_GoodsIconSprite.Add(16, value3);
			m_GoodsIconSprite.Add(26, value4);
			m_GoodsIconSprite.Add(21, value5);
			m_GoodsIconSprite.Add(20, value6);
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value7);
			m_RankBgSprite.Add(2, value8);
			m_RankBgSprite.Add(3, value9);
			m_RankBgSprite.Add(4, value10);
			Sprite value11 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_AreaDamage.png");
			Sprite value12 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_ContinueDamage.png");
			Sprite value13 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DefenceSkill.png");
			Sprite value14 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DownStatus.png");
			Sprite value15 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_NoMove.png");
			Sprite value16 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_OneDamage.png");
			Sprite value17 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_UpStatus.png");
			Sprite value18 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			Sprite value19 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			m_SkillIconSprite.Add(14, value11);
			m_SkillIconSprite.Add(12, value12);
			m_SkillIconSprite.Add(20, value13);
			m_SkillIconSprite.Add(40, value14);
			m_SkillIconSprite.Add(41, value15);
			m_SkillIconSprite.Add(10, value16);
			m_SkillIconSprite.Add(30, value17);
			m_SkillIconSprite.Add(31, value17);
			m_SkillIconSprite.Add(50, value18);
			m_SkillIconSprite.Add(51, value18);
			m_SkillIconSprite.Add(60, value19);
		}

		private void JiNengInit()
		{
			m_PeiZhiList.Clear();
			foreach (UI_GoodsItem peiZhi in Panel.JiNengRect.PeiZhiList)
			{
				m_PeiZhiList.Add(peiZhi);
			}
		}

		private void InitZongLan(EnterEquipmentResp_1009005 enterEquipmentResp_1009005)
		{
			SuperAirZongLanXiangQing.Clear();
			TiPoZongLanXiangQing.Clear();
			ShenShiZongLanXiangQing.Clear();
			double num = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir;

				num3 = enterEquipmentResp_1009005.SuperAirRecoverValueOnBattle;
				num4 = enterEquipmentResp_1009005.SuperAirRecoverValue;
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100;

				num3 = enterEquipmentResp_1009005.SuperAirRecoverValueOnBattle / 100.0;
				num4 = enterEquipmentResp_1009005.SuperAirRecoverValue / 100.0;
			}
			else
			{
				num = Singleton<RoleManager>.Instance.Role_Entity.SuperAir / 100 / 100;

				num3 = enterEquipmentResp_1009005.SuperAirRecoverValueOnBattle / 100.0 / 100.0;
				num4 = enterEquipmentResp_1009005.SuperAirRecoverValue / 100.0 / 100.0;
			}
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAir, num.ToString());
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAirPower, enterEquipmentResp_1009005.SuperAirPower.ToString("0"));
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAirRecoverOnBattle, num3.ToString());
			SuperAirZongLanXiangQing.Add(XiangQingType.SuperAirRecover, num4.ToString());
			long num5 = ((enterEquipmentResp_1009005.CanUsePhysique > Singleton<RoleManager>.Instance.Role_Entity.Physique) ? Singleton<RoleManager>.Instance.Role_Entity.Physique : enterEquipmentResp_1009005.CanUsePhysique);
			TiPoZongLanXiangQing.Add(XiangQingType.Physique, Singleton<RoleManager>.Instance.Role_Entity.Physique.ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.CanUsePhysique, num5.ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.DefenceTotalValue, enterEquipmentResp_1009005.CurrentPhysiqueDefense.ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.BattleBlood, enterEquipmentResp_1009005.CurrentPureBloodDefense.ToString());
			TiPoZongLanXiangQing.Add(XiangQingType.PhysiqueRecoverOnBattle, enterEquipmentResp_1009005.DefenseRecoverValueOnBattle.ToString("0"));
			TiPoZongLanXiangQing.Add(XiangQingType.PhysiqueRecover, enterEquipmentResp_1009005.DefenseRecoverValue.ToString("0"));
			ShenShiZongLanXiangQing.Add(XiangQingType.Perception, Singleton<RoleManager>.Instance.Role_Entity.Perception.ToString());
			ShenShiZongLanXiangQing.Add(XiangQingType.FullLoadPerception, enterEquipmentResp_1009005.FullLoadPerception.ToString());
			ShenShiZongLanXiangQing.Add(XiangQingType.CurrentMoveSpeed, enterEquipmentResp_1009005.CurrentMoveSpeed.ToString("0.##"));
		}

		private void ReSetZongLan(XiangQingType title, string newValue)
		{
			string value = "";
			if (SuperAirZongLanXiangQing.TryGetValue(title, out value))
			{
				SuperAirZongLanXiangQing[title] = newValue;
			}
			else if (TiPoZongLanXiangQing.TryGetValue(title, out value))
			{
				TiPoZongLanXiangQing[title] = newValue;
			}
			else if (ShenShiZongLanXiangQing.TryGetValue(title, out value))
			{
				ShenShiZongLanXiangQing[title] = newValue;
			}
		}

		private void InitZongLanXiangQing()
		{
			if (Panel == null || !Panel.isActiveAndEnabled)
			{
				return;
			}
			ZhuangBeiXiangQingDic.Clear();
			JiNengXiangQingDic.Clear();
			foreach (Transform item in Panel.ZhuangBeiRect.XiangQingScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in Panel.JiNengRect.XiangQingScrollViewContent)
			{
				Object.Destroy(item2.gameObject);
			}
			InitShuXingContent(GongFaType.LianShen, XiangQingType.Perception, ShenShiZongLanXiangQing, Panel.ZhuangBeiRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.DuanTi, XiangQingType.Physique, TiPoZongLanXiangQing, Panel.ZhuangBeiRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.NaQi, XiangQingType.SuperAir, SuperAirZongLanXiangQing, Panel.ZhuangBeiRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.LianShen, XiangQingType.Perception, ShenShiZongLanXiangQing, Panel.JiNengRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.DuanTi, XiangQingType.Physique, TiPoZongLanXiangQing, Panel.JiNengRect.XiangQingScrollViewContent);
			InitShuXingContent(GongFaType.NaQi, XiangQingType.SuperAir, SuperAirZongLanXiangQing, Panel.JiNengRect.XiangQingScrollViewContent);
		}

		private void InitShuXingContent(GongFaType gongFaType, XiangQingType xiangQingType, Dictionary<XiangQingType, string> data, RectTransform rectTransform)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string value = "";
			foreach (KeyValuePair<XiangQingType, string> datum in data)
			{
				if (datum.Key == xiangQingType)
				{
					value = datum.Value;
					continue;
				}
				string xiangQingTitle = GetXiangQingTitle(datum.Key);
				if (!dictionary.ContainsKey(xiangQingTitle))
				{
					dictionary.Add(xiangQingTitle, datum.Value);
				}
			}
			var group = Object.Instantiate(m_XiangQingInstanceObj, rectTransform).GetComponent<RoleZhuangBeiShuXingContentItem>();
			group.SetValue(gongFaType, value, dictionary);
			InkInterfaceArt.Set(group.TitleImg,"JadeButton",false);
			InkInventoryArt.StyleLabel(group.TitleValue, 12);
			group.TitleValue.color = InkDialogArt.Ivory;
			UIManager.FitTextToRect(group.TitleValue);
			foreach (var row in group.GetComponentsInChildren<RoleZhuangBeiXiangQingItem>(true))
			{
				InkInventoryArt.StyleLabel(row.ContentText, 11);
				InkInterfaceArt.Set(row.BackGrounBg, "LightPlaque", false);
			}
		}

		private string GetXiangQingTitle(XiangQingType titleType)
		{
			return titleType switch
			{
				XiangQingType.SuperAir => "真元总值", 
				XiangQingType.SuperAirPower => "斗法攻击", 
				XiangQingType.SuperAirRecoverOnBattle => "战斗恢复", 
				XiangQingType.SuperAirRecover => "脱战恢复", 
				XiangQingType.Physique => "体魄总值", 
				XiangQingType.CanUsePhysique => "可用体魄", 
				XiangQingType.DefenceTotalValue => "斗法防御",
				XiangQingType.BattleBlood => "气血上限", 
				XiangQingType.PhysiqueRecoverOnBattle => "战斗恢复", 
				XiangQingType.PhysiqueRecover => "脱战恢复", 
				XiangQingType.Perception => "神识总值", 
				XiangQingType.FullLoadPerception => "满载神识", 
				XiangQingType.CurrentMoveSpeed => "常驻移速",
				XiangQingType.MethodMovePercent => "功法身法",
				XiangQingType.NextSigilMovePercent => "下场轻身符",
				_ => "", 
			};
		}

		private void GetRoleChuanDaiZhuangBei()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_EQUIPMENT, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterEquipmentResp_1009005 enterEquipmentResp_ = new EnterEquipmentResp_1009005();
					enterEquipmentResp_.MergeFrom(byteString);
					if (Panel == null || !Panel.isActiveAndEnabled || m_Vestments == null)
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						return;
					}
					if (enterEquipmentResp_.IsSuccess)
					{
						InitZongLan(enterEquipmentResp_);
						var movement = byteString.CreateCodedInput();
						uint tag;
						while ((tag = movement.ReadTag()) != 0)
						{
							if (tag == 112) ShenShiZongLanXiangQing[XiangQingType.NextSigilMovePercent] = "+" + movement.ReadInt32() + "%";
							else if (tag == 120) ShenShiZongLanXiangQing[XiangQingType.MethodMovePercent] = "+" + movement.ReadInt32() + "%";
							else movement.SkipLastField();
						}
						m_Vestments.ClearWorn();
						for (int i = 0; i < enterEquipmentResp_.DressedEquipments.Count; i++)
						{
							EnterEquipmentResp_1009005.Types.EquipmentInfoResp_1009006 equipmentInfoResp_ = enterEquipmentResp_.DressedEquipments[i];
							BaseNaJieGoodEntity dataSource = new BaseNaJieGoodEntity
							{
								FiveElement = equipmentInfoResp_.FiveElement,
								GoodsId = equipmentInfoResp_.GoodsId,
								BlankId = equipmentInfoResp_.BlankId,
								TemplateId = equipmentInfoResp_.TemplateId,
								IsBinding = true,
								IsLock = equipmentInfoResp_.IsLock,
								IsUsing = true,
								Level = equipmentInfoResp_.Level,
								Name = equipmentInfoResp_.Name,
								Rank = equipmentInfoResp_.Rank,
								Type = equipmentInfoResp_.Type
							};
							m_Vestments.SetWorn(dataSource);
						}
						m_Vestments.Refresh();
						ZhuangBeiReflashData();
						InitZongLanXiangQing();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}
			}, isShowWaitUI: true);
		}

		private void ZhuangBeiReflashData()
		{
			m_XzjBoxEntity.GetAllGoods(GetGoodsCompelete);
		}

		private void GetGoodsCompelete()
		{
			if (Panel != null && Panel.isActiveAndEnabled && m_Vestments != null)
				m_Vestments.SetCandidates(m_XzjBoxEntity.GoodsList);
		}

		private void BrowseEquipment(int type)
		{
			var guide = Singleton<GuideManager>.Instance.YinDaoJilu1;
			bool firstDress = type == 0 && Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 &&
				guide.YinDaoName == 2 && (guide.YinDaoShu == 2 || guide.YinDaoShu == 3);
			m_XzjBoxEntity.GetAllGoods(() =>
			{
				if (Panel == null || !Panel.isActiveAndEnabled) return;
				var items = m_XzjBoxEntity.GoodsList;
				m_Vestments.SetCandidates(items);
				if (firstDress)
				{
					var starter = items.Find(g => EquipmentPicker.Slot(g) == 26 && string.IsNullOrEmpty(EquipmentPicker.BlockReason(g)));
					if (starter != null) { ShowEquipmentDetail(starter, false); return; }
				}
				EquipmentPicker.Show(Panel.transform, Panel.ZhuangBeiTog.GetComponentInChildren<Text>(true).font,
					items, type, gear => ShowEquipmentDetail(gear, false));
			});
		}

		private void GetGoodsIcon(int type, int rank, ref Sprite GoodsIcon, ref Sprite RankImg)
		{
			m_GoodsIconSprite.TryGetValue(type, out GoodsIcon);
			m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(rank), out RankImg);
		}

		private void ShowEquipmentDetail(BaseNaJieGoodEntity gear, bool worn)
		{
			int slot = EquipmentPicker.Slot(gear);
			string position = EquipmentPicker.SlotName(slot);
			string blocked = worn ? "" : EquipmentPicker.BlockReason(gear);
			var current = m_Vestments.Worn(slot);
			bool usable = string.IsNullOrEmpty(blocked);
			string button = worn ? "卸下" : !usable ? "暂不可用" : !gear.IsBinding ? "滴血认主" : current == null ? "穿戴" : "更换";
			m_XzjBoxEntity.GetGoodsInfomation(gear, raw =>
			{
				if (Panel == null || !Panel.isActiveAndEnabled) return;
				var lines = new List<string>(raw);
				lines.RemoveAll(line => line.StartsWith("穿戴部位,"));
				lines.Insert(0, "部位效果," + EquipmentVestmentView.SlotEffect(gear));
				lines.Insert(0, "穿戴部位," + position);
				if (!worn && current != null) lines.Insert(1, "当前装备," + current.Name + "（换下后收入纳戒）");
				if (!usable) lines.Insert(1, "使用限制," + blocked);
				if (gear.IsLock) lines.Insert(1, "物品锁," + (gear.OfferPending ? "传物交接中，本件暂不可操作" : "已保护，防止出售或销毁，可正常穿脱"));
				if (!worn && !gear.IsBinding && usable) lines.Insert(1, "认主,认主后可穿戴，无法转售");
				UnityAction<object> action = null;
				if (worn) action = TuoXiaZhuangBei;
				else if (usable) action = GoodsTipClick;
				Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, lines, gear.Name, button, gear.Type, action, gear);
			});
		}

		private void GoodsTipClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			string blocked = EquipmentPicker.BlockReason(baseNaJieGoodEntity);
			if (!string.IsNullOrEmpty(blocked))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention(blocked);
				return;
			}
			if (!baseNaJieGoodEntity.IsBinding)
			{
				DiXueRenZhu(baseNaJieGoodEntity, true);
				return;
			}
			DressEquipmentReq_1009001 data = new DressEquipmentReq_1009001
			{
				EquipmentId = baseNaJieGoodEntity.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.DRESS_EQUIPMENT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					DressEquipmentResp_1009001 dressEquipmentResp_ = new DressEquipmentResp_1009001();
					dressEquipmentResp_.MergeFrom(byteString);
					if (dressEquipmentResp_.IsSuccess)
					{
						if (Panel != null) EquipmentPicker.Dismiss(Panel.transform);
						m_Vestments?.PlayChange(baseNaJieGoodEntity, false);
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 4;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
							ShangChuan(2);
						}
						GetRoleChuanDaiZhuangBei();
					}
					else
					{
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 2;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
						}
						string hintMsgByCode = EquipmentError(dressEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShangChuan(int Guide)
		{
			GuideInfoUpdateReq_1002013 guideInfoUpdateReq_ = new GuideInfoUpdateReq_1002013();
			guideInfoUpdateReq_.Type = Guide;
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_GUIDE, guideInfoUpdateReq_, (object _obj) =>
			{
			});
			Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal++;
		}

		private void DiXueRenZhu(BaseNaJieGoodEntity dataSource, bool dressing = false)
		{
			GoodsBindingReq_1004023 data = new GoodsBindingReq_1004023
			{
				GoodsId = dataSource.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BINDING_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBindingResp_1004023 goodsBindingResp_ = new GoodsBindingResp_1004023();
					goodsBindingResp_.MergeFrom(byteString);
					if (goodsBindingResp_ != null && goodsBindingResp_.IsSuccess)
					{
						dataSource.IsBinding = true;
						if (dressing && Panel != null && Panel.isActiveAndEnabled)
						{
							ZhuangBeiReflashData();
							ShowEquipmentDetail(dataSource, false);
							return;
						}
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(EquipmentError(goodsBindingResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private static string EquipmentError(int code)
		{
			if (code == 4509) return "物品正在交接，暂时无法操作。";
			if (code == 4105) return "当前境界不足，无法穿戴此物。";
			if (code == 4520 || code == 4603) return "未找到这件装备，请重新挑选。";
			if (code == 4513 || code == 4606) return "纳戒空间不足，无法收回换下的装备。";
			return MonoSingleton<GameStart>.Instance.GetHintMsgByCode(code);
		}

		private void TuoXiaZhuangBei(object data)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			UndressEquipmentReq_1009002 data2 = new UndressEquipmentReq_1009002
			{
				EquipmentId = baseNaJieGoodEntity.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UNDRESS_EQUIPMENT, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UndressEquipmentResp_1009002 undressEquipmentResp_ = new UndressEquipmentResp_1009002();
					undressEquipmentResp_.MergeFrom(byteString);
					if (undressEquipmentResp_.IsSuccess)
					{
						m_Vestments?.PlayChange(baseNaJieGoodEntity, true);
						GetRoleChuanDaiZhuangBei();
					}
					else
					{
						string hintMsgByCode = EquipmentError(undressEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetRoleJiNeng()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_SKILL, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterSkillResp_1009007 enterSkillResp_ = new EnterSkillResp_1009007();
					enterSkillResp_.MergeFrom(byteString);
					if (enterSkillResp_.IsSuccess)
					{
						ClearChuanDaiZhuangBeiAndFuLu();
						m_SkillList.Clear();
						for (int i = 0; i < enterSkillResp_.SetableSkills.Count; i++)
						{
							EnterSkillResp_1009007.Types.SkillInfoResp_1009008 skillInfoResp_ = enterSkillResp_.SetableSkills[i];
							if (skillInfoResp_ != null && !string.IsNullOrEmpty(skillInfoResp_.GoodsId))
							{
								BaseNaJieGoodEntity dataSource = new BaseNaJieGoodEntity
								{
									FiveElement = skillInfoResp_.FiveElement,
									GoodsId = skillInfoResp_.GoodsId,
									IsBinding = true,
									IsUsing = true,
									Level = skillInfoResp_.Level,
									Name = skillInfoResp_.Name,
									Rank = skillInfoResp_.Rank,
									Type = skillInfoResp_.Type,
									SecondType = skillInfoResp_.SettingType
								};
								m_SkillList.Add(dataSource);
								ChuanDaiZhuangBeiAndFulu(dataSource, skillInfoResp_.CombatSettingId);
							}
						}
						for (int j = 0; j < enterSkillResp_.CanuseSkills.Count; j++)
						{
							EnterSkillResp_1009007.Types.SkillInfoResp_1009008 skillInfoResp_2 = enterSkillResp_.CanuseSkills[j];
							if (skillInfoResp_2 != null && !string.IsNullOrEmpty(skillInfoResp_2.GoodsId))
							{
								BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
								{
									FiveElement = skillInfoResp_2.FiveElement,
									GoodsId = skillInfoResp_2.GoodsId,
									IsBinding = true,
									IsUsing = false,
									Level = skillInfoResp_2.Level,
									Name = skillInfoResp_2.Name,
									Rank = skillInfoResp_2.Rank,
									Type = skillInfoResp_2.Type,
									SecondType = skillInfoResp_2.SettingType
								};
								m_SkillList.Add(item);
							}
						}
						Panel.JiNengRect.Tog_Level_1.isOn = true;
						FaShuTogClick(isOn: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
				}
			});
		}

		private void GetFaShuIcon(int effect, ref Sprite FaShuIcon)
		{
			m_SkillIconSprite.TryGetValue(effect, out FaShuIcon);
		}

		private void FaShuTogClick(bool isOn)
		{
			if (isOn) ShowSkillCategory(1, "法术");
		}

		private void FaQiTogClick(bool isOn)
		{
			if (isOn) ShowSkillCategory(2, "法器");
		}

		private void FuLuTogClick(bool isOn)
		{
			if (isOn) ShowSkillCategory(3, "符箓");
		}

		private void ZhenPanTogClick(bool isOn)
		{
			if (isOn) ShowSkillCategory(4, "阵盘");
		}

		private void ShowSkillCategory(int kind, string category)
		{
			ClearJiNengObjs();
			m_CanPeiZhiList.Clear();
			foreach (var skill in m_SkillList)
				if (!skill.IsUsing && skill.SecondType == kind)
					CreateZhuangBeiAndFuLuItem(skill);

			if (kind != 1)
			{
				foreach (var goods in m_XzjBoxEntity.GoodsList)
				{
					int goodsKind = goods.Type == 15 || goods.Type == 26 ? 2 : goods.Type == 21 ? 3 : goods.Type == 20 ? 4 : 0;
					if (goodsKind != kind || goods.IsUsing || m_SkillList.Exists(skill => skill.GoodsId == goods.GoodsId))
						continue;
					goods.SecondType = kind;
					CreateZhuangBeiAndFuLuItem(goods);
				}
			}
			ShowEmptySkills(m_CanPeiZhiList.Count,category);
			var content = Panel.JiNengRect.GoodsScrollViewContent;
			SetGrticalListContentHight(m_CanPeiZhiList.Count,content,content.GetComponent<GridLayoutGroup>());
		}

		private void ShowEmptySkills(int count, string category)
		{
			m_EmptySkills.text = "暂无可配置的" + category;
			m_EmptySkills.gameObject.SetActive(count == 0);
		}

		private void CreateZhuangBeiAndFuLuItem(BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(Panel.JiNengRect.GoodsScrollViewContent, m_InstanceObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite RankImg = null;
			Sprite GoodsIcon = null;
			GetGoodsIcon(_data.Type, _data.Rank, ref GoodsIcon, ref RankImg);
			if (_data.SecondType == 1)
			{
				GetFaShuIcon(_data.Type, ref GoodsIcon);
			}
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, RankImg, GoodsIcon, (object _obj, GameObject _gameObj) =>
			{
				FuluAndWeaponClick(_obj);
			});
			if (!m_CanPeiZhiList.ContainsKey(_data.GoodsId))
			{
				m_CanPeiZhiList.Add(_data.GoodsId, component);
			}
		}

		private void FuluAndWeaponClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			new List<string>();
			string btnName = "配置";
			if (!baseNaJieGoodEntity.IsBinding && (baseNaJieGoodEntity.Type == 26 || baseNaJieGoodEntity.Type == 15))
			{
				btnName = "滴血认主";
			}
			if (baseNaJieGoodEntity.SecondType != 1)
			{
				m_XzjBoxEntity.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						BtnName = btnName,
						CloseBtnClick = null,
						ContentList = list,
						DataSource = dataSource,
						SureBtnClick = JiNengTipClick,
						TitleName = baseNaJieGoodEntity.Name
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
			else
			{
				Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity.Level, baseNaJieGoodEntity.Rank, baseNaJieGoodEntity.FiveElement, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						BtnName = btnName,
						CloseBtnClick = null,
						ContentList = list,
						DataSource = dataSource,
						SureBtnClick = JiNengTipClick,
						TitleName = baseNaJieGoodEntity.Name
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
		}

		private void JiNengTipClick(object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (!baseNaJieGoodEntity.IsBinding && (baseNaJieGoodEntity.Type == 15 || baseNaJieGoodEntity.Type == 26))
			{
				DiXueRenZhu(baseNaJieGoodEntity);
				return;
			}
			int settingId = -1;
			for (int i = 0; i < m_PeiZhiList.Count; i++)
			{
				if (m_PeiZhiList[i].IsEmpty)
				{
					settingId = i;
					break;
				}
			}
			if (settingId == -1)
			{
				return;
			}
			SetSkillReq_1009003 data = new SetSkillReq_1009003
			{
				SkillId = baseNaJieGoodEntity.GoodsId,
				SkillType = baseNaJieGoodEntity.SecondType,
				CombatSettingId = settingId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SET_SKILL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SetSkillResp_1009003 setSkillResp_ = new SetSkillResp_1009003();
					setSkillResp_.MergeFrom(byteString);
					if (setSkillResp_.IsSuccess)
					{
						if (baseNaJieGoodEntity.Type == 15 || baseNaJieGoodEntity.Type == 26)
						{
							ReSetZongLan(XiangQingType.FullLoadPerception, setSkillResp_.FullLoadPerception.ToString());
						}
						InitZongLanXiangQing();
						ChuanDaiZhuangBeiAndFulu(baseNaJieGoodEntity, settingId);
						if (m_CanPeiZhiList.ContainsKey(baseNaJieGoodEntity.GoodsId))
						{
							UI_GoodsItem uI_GoodsItem = m_CanPeiZhiList[baseNaJieGoodEntity.GoodsId];
							m_CanPeiZhiList.Remove(baseNaJieGoodEntity.GoodsId);
							Object.Destroy(uI_GoodsItem.gameObject);
						}
						if (!m_SkillList.Contains(baseNaJieGoodEntity))
							ZhuangBeiReflashData();
						m_EmptySkills.gameObject.SetActive(m_CanPeiZhiList.Count == 0);
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 8;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
							ShangChuan(2);
						}
					}
					else
					{
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
						{
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 6;
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(setSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChuanDaiZhuangBeiAndFulu(BaseNaJieGoodEntity dataSource, int index)
		{
			UI_GoodsItem uI_GoodsItem = null;
			for (int i = 0; i < m_PeiZhiList.Count; i++)
			{
				if (i == index && m_PeiZhiList[i].IsEmpty)
				{
					uI_GoodsItem = m_PeiZhiList[i];
					break;
				}
			}
			if (uI_GoodsItem != null)
			{
				Sprite RankImg = null;
				Sprite GoodsIcon = null;
				GetGoodsIcon(dataSource.Type, dataSource.Rank, ref GoodsIcon, ref RankImg);
				if (dataSource.SecondType == 1)
				{
					GetFaShuIcon(dataSource.Type, ref GoodsIcon);
				}
				uI_GoodsItem.SetItem(dataSource, dataSource.Name, dataSource.FiveElement, dataSource.Level, RankImg, GoodsIcon, PeiZhiClick);
				dataSource.IsUsing = true;
			}
		}

		private void PeiZhiClick(object data, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity.SecondType != 1)
			{
				m_XzjBoxEntity.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> list) =>
				{
					Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, list, baseNaJieGoodEntity.Name, "卸下", baseNaJieGoodEntity.Type, (UnityAction<object>)((object obj) =>
					{
						JieChuPeiZhi(obj, gameObj);
					}), baseNaJieGoodEntity);
				});
				return;
			}
			Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity.Level, baseNaJieGoodEntity.Rank, baseNaJieGoodEntity.FiveElement, (List<string> list) =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiTipWnd.prefab", true, list, baseNaJieGoodEntity.Name, "卸下", baseNaJieGoodEntity.Type, (UnityAction<object>)((object obj) =>
				{
					JieChuPeiZhi(obj, gameObj);
				}), baseNaJieGoodEntity);
			});
		}

		private void JieChuPeiZhi(object data, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = data as BaseNaJieGoodEntity;
			RemoveSkillReq_1009004 data2 = new RemoveSkillReq_1009004
			{
				SkillId = baseNaJieGoodEntity.GoodsId,
				SkillType = baseNaJieGoodEntity.SecondType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REMOVE_SKILL, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RemoveSkillResp_1009004 removeSkillResp_ = new RemoveSkillResp_1009004();
					removeSkillResp_.MergeFrom(byteString);
					if (removeSkillResp_.IsSuccess)
					{
						JieChuChengGong(removeSkillResp_, baseNaJieGoodEntity, gameObj);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(removeSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JieChuChengGong(RemoveSkillResp_1009004 removeSkillResp_1009004, BaseNaJieGoodEntity baseNaJieGoodEntity, GameObject gameObj)
		{
			baseNaJieGoodEntity.IsUsing = false;
			BaseNaJieGoodEntity goodsInfo = m_XzjBoxEntity.GetGoodsInfo(baseNaJieGoodEntity.GoodsId);
			if (goodsInfo != null)
			{
				goodsInfo.IsUsing = false;
			}
			if (baseNaJieGoodEntity.Type == 15 || baseNaJieGoodEntity.Type == 26)
			{
				ReSetZongLan(XiangQingType.FullLoadPerception, removeSkillResp_1009004.FullLoadPerception.ToString());
				InitZongLanXiangQing();
			}
			UI_GoodsItem component = gameObj.GetComponent<UI_GoodsItem>();
			component.ClearItem();
			switch (baseNaJieGoodEntity.SecondType)
			{
			case 1:
				if (!Panel.JiNengRect.Tog_Level_1.isOn)
				{
					Panel.JiNengRect.Tog_Level_1.isOn = true;
				}
				else
				{
					FaShuTogClick(isOn: true);
				}
				break;
			case 2:
				if (!Panel.JiNengRect.Tog_Level_2.isOn)
				{
					Panel.JiNengRect.Tog_Level_2.isOn = true;
				}
				else
				{
					FaQiTogClick(isOn: true);
				}
				break;
			case 3:
				if (!Panel.JiNengRect.Tog_Level_3.isOn)
				{
					Panel.JiNengRect.Tog_Level_3.isOn = true;
				}
				else
				{
					FuLuTogClick(isOn: true);
				}
				break;
			case 4:
				if (!Panel.JiNengRect.Tog_Level_4.isOn)
				{
					Panel.JiNengRect.Tog_Level_4.isOn = true;
				}
				else
				{
					ZhenPanTogClick(isOn: true);
				}
				break;
			}
		}

		private void ClearChuanDaiZhuangBeiAndFuLu()
		{
			foreach (UI_GoodsItem peiZhi in m_PeiZhiList)
			{
				if (!peiZhi.IsEmpty)
				{
					peiZhi.ClearItem();
				}
			}
		}

		public void ClearJiNengObjs()
		{
			if (Panel.JiNengRect.GoodsScrollViewContent == null)
			{
				return;
			}
			foreach (Transform item in Panel.JiNengRect.GoodsScrollViewContent)
			{
				if (item.gameObject != null)
				{
					Object.Destroy(item.gameObject);
				}
			}
		}

		private void ZhuangBeiShow(bool isOn)
		{
			if (m_Vestments != null) m_Vestments.enabled = isOn;
			if (isOn)
			{
				if (m_RoleZhuangBeiType != RoleZhuangBeiType.DefencePanel)
				{
					GetRoleChuanDaiZhuangBei();
					Panel.ZhuangBeiRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 33.885f);
					m_RoleZhuangBeiType = RoleZhuangBeiType.DefencePanel;
				}
			}
			else
			{
				Panel.ZhuangBeiRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			}
		}

		public void JiNengShow(bool isOn)
		{
			if (isOn)
			{
				if (m_RoleZhuangBeiType != RoleZhuangBeiType.SkillPanel)
				{
					Panel.JiNengRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 33.885f);
					GetRoleJiNeng();
					m_RoleZhuangBeiType = RoleZhuangBeiType.SkillPanel;
				}
			}
			else
			{
				Panel.JiNengRect.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			}
		}

		public override void OnDisable()
		{
			if (Panel != null) EquipmentPicker.Dismiss(Panel.transform);
			base.OnDisable();
		}

		private void WenHaoTogClick()
		{
			if (m_RoleZhuangBeiType == RoleZhuangBeiType.SkillPanel)
			{
				Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true,
					"选择法术、法器、符箓或阵盘，将招式装入空位。进入战场后，每组三个位置，点招式组按钮切换。", "斗法配置");
				return;
			}
			string text = "主手执兵刃，衣甲护身躯，镜类法器入护身位，靴履只入足部。每个部位一件，更换会将旧件收入纳戒。\n\n"
				+ "器物需要认主，并达到物品所在境界。护身器增加气血，足具提高移速。衣甲与足具同穿提升装备防御，再配主手提升装备攻击。\n\n"
				+ "点击空位可以挑选，点击已穿装备可看详情或卸下。物品锁保护装备免于出售和销毁，处于传物交接中的装备暂不可操作。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "装备");
		}
	}
}
