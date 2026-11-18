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
			UIManager.FitTextToRect(_panel.ChengHao_Name);
			UIManager.FitTextToRect(_panel.VIP_ChengHaoText);
			roleInfo = Singleton<RoleManager>.Instance.Role_Entity;
			AddButtonClickListener(_panel.DuiHuanLingShi_btn, DuHuanLingShi);
			AddButtonClickListener(_panel.TouXiang_btn, TouXiangClick);
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
			NaJieMoRenSprite = _panel.NaJie_btn.GetComponent<Image>().sprite;
			ZhanDouMoRenSprite = _panel.ZhanDou_btn.GetComponent<Image>().sprite;
			CanWuSprite = _panel.CanWu_btn.GetComponent<Image>().sprite;
			LiLianSprite = _panel.LiLian_btn.GetComponent<Image>().sprite;
			DaoYouMoRenSprite = _panel.DaoYou_btn.GetComponent<Image>().sprite;
			DongFuMoRenSprite = _panel.DongFu_btn.image.sprite;
			BottomCloseBtnSprite = InkInterfaceArt.Get("Close") ?? DongFuMoRenSprite;
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
			if (Singleton<RoleManager>.Instance.Role_Entity.PracticeGuideVal != 100 && Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName == 1)
			{
				if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 1 || Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 6)
				{
					Singleton<GuideManager>.Instance.ShowGuideNew(1);
				}
				else if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 10)
				{
					MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
					Timer.Register(0.3f, () =>
					{
						DongFuBtnClick();
					});
					Timer.Register(1.3f, () =>
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab").LianGongFangBtn();
					});
				}
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
			_panel.Level_text.text = $"{WorldSetting.GetRoleLevelNameByRoleLevel(roleInfo.RoleLevel)}({roleInfo.RoleLevel})";
			string text = WorldSetting.simplifyNumber((ulong)roleInfo.ShinyStone);
			_panel.LingShi_text.text = text;
			string text2 = WorldSetting.simplifyNumber((ulong)roleInfo.ShinyJade);
			_panel.LingYu_text.text = text2;
			_panel.Name_text.text = roleInfo.RoleName;
			_panel.TiLi_Text.text = $"体力:{roleInfo.CurrentPower}";
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
			if (!string.IsNullOrEmpty(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId) && Singleton<RoleManager>.Instance.Role_Entity.ZongMenId != "0")
			{
				Text text = null;
				Image image = null;
				Sprite sprite = null;
				switch ((ZongMenLingPaiType)Singleton<RoleManager>.Instance.Role_Entity.ZongMenIconIndex)
				{
				case ZongMenLingPaiType.RiYao:
					text = _panel.ZongMenSimpleNameText_1;
					image = _panel.ZongMenNameIconImg_1;
					_panel.ZongMenIconBtn_1.gameObject.SetActive(value: true);
					break;
				case ZongMenLingPaiType.YueHui:
					text = _panel.ZongMenSimpleNameText_2;
					image = _panel.ZongMenNameIconImg_2;
					_panel.ZongMenIconBtn_2.gameObject.SetActive(value: true);
					break;
				case ZongMenLingPaiType.XingGuang:
					text = _panel.ZongMenSimpleNameText_3;
					image = _panel.ZongMenNameIconImg_3;
					_panel.ZongMenIconBtn_3.gameObject.SetActive(value: true);
					break;
				case ZongMenLingPaiType.XuanTie:
					text = _panel.ZongMenSimpleNameText_4;
					image = _panel.ZongMenNameIconImg_4;
					_panel.ZongMenIconBtn_4.gameObject.SetActive(value: true);
					break;
				}
				sprite = Singleton<RoleManager>.Instance.Role_Entity.ZongMenLastNameImgIndex switch
				{
					0 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Pai.png"), 
					1 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Men.png"), 
					2 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Zong.png"), 
					3 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Bang.png"), 
					4 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Jiao.png"), 
					5 => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Gong.png"), 
					_ => Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Pai.png"), 
				};
				if (sprite != null && image != null)
				{
					image.sprite = sprite;
				}
				if (text != null)
				{
					text.text = Singleton<RoleManager>.Instance.Role_Entity.ZongMenSimpleName;
				}
				_panel.MenPaiIcon.gameObject.SetActive(value: false);
			}
			else
			{
				_panel.MenPaiIcon.gameObject.SetActive(value: true);
				_panel.ZongMenIconBtn_1.gameObject.SetActive(value: false);
				_panel.ZongMenIconBtn_2.gameObject.SetActive(value: false);
				_panel.ZongMenIconBtn_3.gameObject.SetActive(value: false);
				_panel.ZongMenIconBtn_4.gameObject.SetActive(value: false);
			}
		}

		private void TouXiangClick()
		{
			Singleton<UIManager>.Instance.FindWndByName<CeHuaWnd>("CeHuaWnd.prefab").TanChuWnd();
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
			_panel.NaJie_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
			_panel.NaJie_btn.gameObject.GetComponent<Image>().sprite = NaJieMoRenSprite;
			_panel.NaJie_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.NaJie_btn, NaJieBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("NaJieSpaceWnd.prefab");
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
			}
		}

		private void CanWuBtnClick()
		{
			_panel.CanWu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.CanWu_btn, CanWuCloseClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			_panel.CanWu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = DongFuMoRenSprite;
			}
			Singleton<UIManager>.Instance.PopUpWnd("CanWuWnd.prefab", true);
		}

		private void CanWuCloseClick()
		{
			_panel.CanWu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.CanWu_btn, CanWuBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("CanWuWnd.prefab");
			_panel.CanWu_btn.gameObject.GetComponent<Image>().sprite = CanWuSprite;
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
			_panel.ZhanDou_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
			Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiWnd.prefab", true);
		}

		private void ZhanDouCloseBtnClick()
		{
			_panel.ZhanDou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.ZhanDou_btn, ZhanDouBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("RoleZhuangBeiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("BattleSettingWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("BattleSettingDetailWnd.prefab");
			_panel.ZhanDou_btn.gameObject.GetComponent<Image>().sprite = ZhanDouMoRenSprite;
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
			_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
			_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = DongFuMoRenSprite;
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("FaQiXiLianWnd.prefab");
		}

		public void CloseDongFuNoChangeSceneInMain()
		{
			_panel.DongFu_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DongFu_btn, DongFuBtnClick);
			_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = DongFuMoRenSprite;
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
			_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
			_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = DongFuMoRenSprite;
			Singleton<UIManager>.Instance.CloseWnd("WuDaoTaiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("ZhanFaShiWnd.prefab");
		}

		private void LiLianBtnClick()
		{
			_panel.LiLian_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.LiLian_btn, LiLianBtnCloseClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			_panel.LiLian_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = DongFuMoRenSprite;
			}
			WaiChuLiLianClick();
		}

		public void LiLianBtnCloseClick()
		{
			_panel.LiLian_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.LiLian_btn, LiLianBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("WorldMapWnd.prefab", destory: false);
			_panel.LiLian_btn.gameObject.GetComponent<Image>().sprite = LiLianSprite;
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
			}
		}

		private void DaoYouBtnClick()
		{
			_panel.DaoYou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DaoYou_btn, DaoYouBtnCloseClick);
			_panel = base.GameObject.GetComponent<MainTitlePanel>();
			_panel.DaoYou_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = DongFuMoRenSprite;
			}
			Singleton<UIManager>.Instance.PopUpWnd("DaoYouWnd.prefab", true);
		}

		private void DaoYouBtnCloseClick()
		{
			_panel.DaoYou_btn.onClick.RemoveAllListeners();
			AddButtonClickListener(_panel.DaoYou_btn, DaoYouBtnClick);
			Singleton<UIManager>.Instance.CloseWnd("DaoYouWnd.prefab");
			_panel.DaoYou_btn.gameObject.GetComponent<Image>().sprite = DaoYouMoRenSprite;
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
			{
				_panel.DongFu_btn.onClick.RemoveAllListeners();
				AddButtonClickListener(_panel.DongFu_btn, CloseDongFuClick);
				_panel.DongFu_btn.gameObject.GetComponent<Image>().sprite = BottomCloseBtnSprite;
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
			Singleton<UIManager>.Instance.PopUpWnd("CashGoodsWnd.prefab", true);
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
