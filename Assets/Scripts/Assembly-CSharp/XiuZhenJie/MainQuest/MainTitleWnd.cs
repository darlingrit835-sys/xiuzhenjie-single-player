using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class MainTitleWnd : Window
	{
		public MainTitlePanel _panel;

		private RoleInfoEntity roleInfo;

		private Sprite CanWuSprite;

		private Sprite LiLianSprite;

		private Sprite NaJieMoRenSprite;

		private Sprite ZhanDouMoRenSprite;

		private Sprite DongFuMoRenSprite;

		private Sprite DaoYouMoRenSprite;

		private Sprite BottomCloseBtnSprite;

		private int m_EveryDayLingBao;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			InkMainHud.Apply(_panel);
			NaJieMoRenSprite = _panel.NaJie_btn.image.sprite;
			ZhanDouMoRenSprite = _panel.ZhanDou_btn.image.sprite;
			CanWuSprite = _panel.CanWu_btn.image.sprite;
			LiLianSprite = _panel.LiLian_btn.image.sprite;
			DaoYouMoRenSprite = _panel.DaoYou_btn.image.sprite;
			DongFuMoRenSprite = _panel.DongFu_btn.image.sprite;
			BottomCloseBtnSprite = InkInterfaceArt.Get("Close") ?? DongFuMoRenSprite;
			UIManager.FitTextToRect(_panel.ChengHao_Name);
			UIManager.FitTextToRect(_panel.VIP_ChengHaoText);
			roleInfo = Singleton<RoleManager>.Instance.Role_Entity;
			AddButtonClickListener(_panel.DuiHuanLingShi_btn, DuHuanLingShi);
			AddButtonClickListener(_panel.TouXiang_btn, TouXiangClick);
			var emblem = _panel.MenPaiIcon;
			var wandererButton = emblem.GetComponent<Button>() ?? emblem.AddComponent<Button>();
			wandererButton.targetGraphic = emblem.GetComponent<Image>();
			AddButtonClickListener(wandererButton, TouXiangClick);
			AddButtonClickListener(_panel.DuiHuanLingYu_btn, ChongZhi);
			AddButtonClickListener(_panel.NaJie_btn, NaJieBtnClick);
			AddButtonClickListener(_panel.ZhanDou_btn, ZhanDouBtnClick);
			AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
			AddButtonClickListener(_panel.CanWu_btn, CanWuBtnClick);
			AddButtonClickListener(_panel.DaoYou_btn, DaoYouBtnClick);
			AddButtonClickListener(_panel.ChengHao_btn, GengHuanChengHaoBtnClick);
			AddButtonClickListener(_panel.VIP_ChengHao_btn, GengHuanChengHaoBtnClick);
			AddButtonClickListener(_panel.LiLian_btn, LiLianBtnClick);
			AddButtonClickListener(_panel.ZongMenIconBtn_1, ZongMenIconBtnClick);
			AddButtonClickListener(_panel.ZongMenIconBtn_2, ZongMenIconBtnClick);
			AddButtonClickListener(_panel.ZongMenIconBtn_3, ZongMenIconBtnClick);
			AddButtonClickListener(_panel.ZongMenIconBtn_4, ZongMenIconBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ReFlashData();
			GuideShow();
		}

		private void ChongZhiBtnClick()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("充值系统暂未开放，无法进行充值");
		}

		private void DuHuanLingShi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("DuiHuanLingShiNewWnd.prefab", true);
		}

		private void GuideShow()
		{
			if (Singleton<GuideManager>.Instance.Practice?.Active == true)
			{
				Singleton<GuideManager>.Instance.ShowPractice();
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 2)
			{
				if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 1)
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
					Singleton<GuideManager>.Instance.ShowGuideNew(2);
				}
				else if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 4)
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
					Singleton<GuideManager>.Instance.ShowGuideNew(2);
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 3;
				}
				else if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 8)
				{
					Singleton<GuideManager>.Instance.ShowGuideNew(2);
				}
				else if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 16)
				{
					Singleton<GuideManager>.Instance.ShowGuideNew(2);
				}
				else if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 15)
				{
					Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 8;
					Singleton<GuideManager>.Instance.ShowGuideNew(2);
				}
			}
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			roleInfo = Singleton<RoleManager>.Instance.Role_Entity;
			Sprite currentRoleSprite = Singleton<RoleManager>.Instance.GetCurrentRoleSprite();
			if (currentRoleSprite != null)
			{
				_panel.TouXiang_btn.GetComponent<Image>().sprite = currentRoleSprite;
			}
			else
			{
				Debug.Log("头像获取失败");
			}
			_panel.Level_text.text = WorldSetting.GetRoleLevelNameByRoleLevel(roleInfo.RoleLevel);
			string text = WorldSetting.simplifyNumber((ulong)roleInfo.ShinyStone);
			_panel.LingShi_text.text = text;
			string text2 = WorldSetting.simplifyNumber((ulong)roleInfo.ShinyJade);
			_panel.LingYu_text.text = text2;
			_panel.Name_text.text = roleInfo.RoleName;
			_panel.TiLi_Text.text = $"体力 {roleInfo.CurrentPower}";
			if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 11)
			{
				_panel.VIP_HeadFrame.gameObject.SetActive(value: true);
			}
			else
			{
				_panel.VIP_HeadFrame.gameObject.SetActive(value: false);
			}
			float x = Singleton<RoleManager>.Instance.Role_Entity.RoleName.Length * 16;
			if (Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId != 0 && Singleton<RoleManager>.Instance.Role_Entity.VipLevel < 7)
			{
				RectTransform component = _panel.ChengHao_btn.gameObject.GetComponent<RectTransform>();
				component.anchoredPosition = new Vector2(x, component.anchoredPosition.y);
				_panel.ChengHao_btn.gameObject.SetActive(value: true);
				ChengHaoInfoEntity value = null;
				Singleton<FacadeManager>.Instance.SelectChengHaoInfo()?.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId, out value);
				_panel.ChengHao_Name.text = ((value == null) ? "" : value.Name);
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId != 0 && Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 7)
			{
				RectTransform component2 = _panel.VIP_ChengHaoBg.gameObject.GetComponent<RectTransform>();
				component2.anchoredPosition = new Vector2(x, component2.anchoredPosition.y);
				ChengHaoInfoEntity value2 = null;
				Singleton<FacadeManager>.Instance.SelectChengHaoInfo()?.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId, out value2);
				_panel.VIP_ChengHaoBg.gameObject.SetActive(value: true);
				_panel.ChengHao_btn.gameObject.SetActive(value: false);
				_panel.VIP_ChengHaoText.text = ((value2 == null) ? "" : value2.Name);
			}
			else
			{
				_panel.ChengHao_btn.gameObject.SetActive(value: false);
			}
			SetZongMenIcon();
		}

		private void SetZongMenIcon()
		{
			bool inSect = !string.IsNullOrEmpty(roleInfo.ZongMenId) && roleInfo.ZongMenId != "0";
			var kind = inSect ? (ZongMenLingPaiType)roleInfo.ZongMenIconIndex : ZongMenLingPaiType.None;
			_panel.ZongMenIconBtn_1.gameObject.SetActive(kind == ZongMenLingPaiType.RiYao);
			_panel.ZongMenIconBtn_2.gameObject.SetActive(kind == ZongMenLingPaiType.YueHui);
			_panel.ZongMenIconBtn_3.gameObject.SetActive(kind == ZongMenLingPaiType.XingGuang);
			_panel.ZongMenIconBtn_4.gameObject.SetActive(kind == ZongMenLingPaiType.XuanTie);

			Text name;
			Image suffix;
			switch (kind)
			{
			case ZongMenLingPaiType.RiYao:
				name = _panel.ZongMenSimpleNameText_1;
				suffix = _panel.ZongMenNameIconImg_1;
				break;
			case ZongMenLingPaiType.YueHui:
				name = _panel.ZongMenSimpleNameText_2;
				suffix = _panel.ZongMenNameIconImg_2;
				break;
			case ZongMenLingPaiType.XingGuang:
				name = _panel.ZongMenSimpleNameText_3;
				suffix = _panel.ZongMenNameIconImg_3;
				break;
			case ZongMenLingPaiType.XuanTie:
				name = _panel.ZongMenSimpleNameText_4;
				suffix = _panel.ZongMenNameIconImg_4;
				break;
			default:
				_panel.MenPaiIcon.SetActive(true);
				return;
			}

			_panel.MenPaiIcon.SetActive(false);
			name.text = roleInfo.ZongMenSimpleName;
			string glyph = roleInfo.ZongMenLastNameImgIndex switch
			{
				1 => "Men", 2 => "Zong", 3 => "Bang", 4 => "Jiao", 5 => "Gong", _ => "Pai"
			};
			var sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>(
				"Assets/GameData/Textures/ZongMen/ZongMenFont_" + glyph + ".png");
			if (sprite != null) suffix.sprite = sprite;
		}

		private void TouXiangClick()
		{
			var sheet = (CeHuaWnd)Singleton<UIManager>.Instance.PopUpWnd(Constants.CEHUA_WINDOW, true);
			sheet.TanChuWnd();
		}

		private void GengHuanChengHaoBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChooseChengHaoWnd.prefab", true);
		}

		private void ZongMenIconBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMenChatWnd.prefab", true);
		}

		private void NaJieBtnClick()
		{
			_panel.NaJie_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.NaJie_btn, CloseNaJieClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			InkMainHud.SetNavigationState(_panel.NaJie_btn, BottomCloseBtnSprite, true);
			ZhanDouCloseBtnClick();
			CanWuCloseClick();
			LiLianBtnCloseClick();
			DaoYouBtnCloseClick();
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "MainScene")
			{
				CloseDongFuNoChangeScene();
			}
			else
			{
				CloseDongFuNoChangeSceneInMain();
			}
			Singleton<UIManager>.Instance.PopUpWnd("NaJieSpaceWnd.prefab", true);
		}

		public void CloseNaJieClick()
		{
			InkMainHud.SetNavigationState(_panel.NaJie_btn, NaJieMoRenSprite, false);
			_panel.NaJie_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.NaJie_btn, NaJieBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("NaJieSpaceWnd.prefab");
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
			}
		}

		private void CanWuBtnClick()
		{
			_panel.CanWu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.CanWu_btn, CanWuCloseClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			InkMainHud.SetNavigationState(_panel.CanWu_btn, BottomCloseBtnSprite, true);
			CloseNaJieClick();
			ZhanDouCloseBtnClick();
			LiLianBtnCloseClick();
			DaoYouBtnCloseClick();
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "MainScene")
			{
				CloseDongFuNoChangeScene();
			}
			else
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, DongFuMoRenSprite, false);
			}
			Singleton<UIManager>.Instance.PopUpWnd("CanWuWnd.prefab", true);
		}

		private void CanWuCloseClick()
		{
			_panel.CanWu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.CanWu_btn, CanWuBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("CanWuWnd.prefab");
			InkMainHud.SetNavigationState(_panel.CanWu_btn, CanWuSprite, false);
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
			}
		}

		public void ZhanDouBtnClick()
		{
			_panel.ZhanDou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.ZhanDou_btn, ZhanDouCloseBtnClick);
			CloseNaJieClick();
			CanWuCloseClick();
			LiLianBtnCloseClick();
			DaoYouBtnCloseClick();
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "MainScene")
			{
				CloseDongFuNoChangeScene();
			}
			else
			{
				CloseDongFuNoChangeSceneInMain();
			}
			InkMainHud.SetNavigationState(_panel.ZhanDou_btn, BottomCloseBtnSprite, true);
			Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiWnd.prefab", true);
		}

		private void ZhanDouCloseBtnClick()
		{
			_panel.ZhanDou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.ZhanDou_btn, ZhanDouBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("RoleZhuangBeiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("BattleSettingWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("BattleSettingDetailWnd.prefab");
			InkMainHud.SetNavigationState(_panel.ZhanDou_btn, ZhanDouMoRenSprite, false);
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
			}
		}

		public void DongFuBtnClick()
		{
			_panel.DongFu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
			CloseNaJieClick();
			ZhanDouCloseBtnClick();
			CanWuCloseClick();
			LiLianBtnCloseClick();
			DaoYouBtnCloseClick();
			Singleton<UIManager>.Instance.PopUpWnd("DongFuRoomWnd.prefab", true);
			InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
		}

		public void CloseDongFuClick()
		{
			_panel.DongFu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "MainScene")
			{
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd("DongFuRoomWnd.prefab");
				Singleton<UIManager>.Instance.CloseWnd("DongFuMiShiWnd.prefab");
			}
			InkMainHud.SetNavigationState(_panel.DongFu_btn, DongFuMoRenSprite, false);
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("FaQiXiLianWnd.prefab");
		}

		public void CloseDongFuNoChangeSceneInMain()
		{
			_panel.DongFu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
			InkMainHud.SetNavigationState(_panel.DongFu_btn, DongFuMoRenSprite, false);
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "MainScene")
			{
				Singleton<UIManager>.Instance.CloseWnd("DongFuRoomWnd.prefab");
				Singleton<UIManager>.Instance.CloseWnd("DongFuMiShiWnd.prefab");
			}
			Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("FaQiXiLianWnd.prefab");
		}

		public void DongFuBtnClickNoChangeScene()
		{
			_panel.DongFu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
			InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
			CloseNaJieClick();
			ZhanDouCloseBtnClick();
			CanWuCloseClick();
			LiLianBtnCloseClick();
			DaoYouBtnCloseClick();
		}

		private void CloseDongFuNoChangeScene()
		{
			_panel.DongFu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClickNoChangeScene);
			InkMainHud.SetNavigationState(_panel.DongFu_btn, DongFuMoRenSprite, false);
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiWnd.prefab");
		}

		private void LiLianBtnClick()
		{
			_panel.LiLian_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.LiLian_btn, LiLianBtnCloseClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			InkMainHud.SetNavigationState(_panel.LiLian_btn, LiLianSprite, true);
			CloseNaJieClick();
			ZhanDouCloseBtnClick();
			CanWuCloseClick();
			DaoYouBtnCloseClick();
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "MainScene")
			{
				CloseDongFuNoChangeScene();
			}
			else
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, DongFuMoRenSprite, false);
			}
			WaiChuLiLianClick();
		}

		public void LiLianBtnCloseClick()
		{
			_panel.LiLian_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.LiLian_btn, LiLianBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("WorldMapWnd.prefab", destory: false);
			InkMainHud.SetNavigationState(_panel.LiLian_btn, LiLianSprite, false);
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
			}
		}

		private void DaoYouBtnClick()
		{
			_panel.DaoYou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DaoYou_btn, DaoYouBtnCloseClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			InkMainHud.SetNavigationState(_panel.DaoYou_btn, BottomCloseBtnSprite, true);
			CloseNaJieClick();
			ZhanDouCloseBtnClick();
			LiLianBtnCloseClick();
			CanWuCloseClick();
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "MainScene")
			{
				CloseDongFuNoChangeScene();
			}
			else
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, DongFuMoRenSprite, false);
			}
			Singleton<UIManager>.Instance.PopUpWnd("DaoYouWnd.prefab", true);
		}

		private void DaoYouBtnCloseClick()
		{
			_panel.DaoYou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DaoYou_btn, DaoYouBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("DaoYouWnd.prefab");
			InkMainHud.SetNavigationState(_panel.DaoYou_btn, DaoYouMoRenSprite, false);
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				InkMainHud.SetNavigationState(_panel.DongFu_btn, BottomCloseBtnSprite, true);
			}
			Singleton<UIManager>.Instance.CloseWnd("ShiTuWnd.prefab");
		}

		public void ShowOrHideBottomBg(bool isShow)
		{
			if (_panel.Bottom_img.gameObject.activeSelf != isShow)
			{
				_panel.Bottom_img.gameObject.SetActive(isShow);
			}
		}

		private void ChongZhi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("CashGoodsWnd.prefab", true, CashGoodsPanelType.LingYu);
		}

		public void WaiChuLiLianClick()
		{
			SibingsMapReq_1007001 data = new SibingsMapReq_1007001
			{
				MapId = Singleton<RoleManager>.Instance.Role_Entity.MapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_SIBINGS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SibingsMapResp_1007001 sibingsMapResp_ = new SibingsMapResp_1007001();
					sibingsMapResp_.MergeFrom(byteString);
					if (sibingsMapResp_.IsSuccess)
					{
						List<MapEntity> list = new List<MapEntity>();
						foreach (SibingsMapResp_1007001.Types.MapObjectResp_1007002 item2 in sibingsMapResp_.Object)
						{
							MapEntity item = new MapEntity
							{
								Depth = item2.Level,
								FiveElement = item2.FiveElements,
								Id = item2.Id,
								MapName = item2.Name,
								MapType = item2.Type,
								MaxLevel = item2.MaxLevel,
								MinLevel = item2.MinLevel,
								ParentName = item2.ParentName
							};
							list.Add(item);
						}
						Singleton<UIManager>.Instance.PopUpWnd("WorldMapWnd.prefab", true, MapEnterType.WaiChuLiLiang, list, new List<string>());
						if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu < 2)
						{
							Singleton<GuideManager>.Instance.ShowGuideNew(2);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sibingsMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
