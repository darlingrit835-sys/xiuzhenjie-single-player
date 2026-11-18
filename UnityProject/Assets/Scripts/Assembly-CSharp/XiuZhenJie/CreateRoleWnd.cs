using System.Collections.Generic;
using DG.Tweening;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CreateRoleWnd : Window
	{
		public CreateRolePanel _panel;

		public Dictionary<string, Sprite> SpriteDic = new Dictionary<string, Sprite>();

		public Dictionary<WuXing, string> WuXingImagePath = new Dictionary<WuXing, string>();

		public Dictionary<WuXing, Button> WuXingButtonDic = new Dictionary<WuXing, Button>();

		public Dictionary<WuXing, Button> WuXingChooseButtonDic = new Dictionary<WuXing, Button>();

		private MapEntity m_CurrentChooseMap;

		public List<WuXing> ChooseButtonsList = new List<WuXing>();

		public Image touXiangYi;

		public Image touXiangEr;

		public Image touXiangSan;

		public Image touXiangSi;

		public Image touXiangWu;

		public Image touXiangLiu;

		public CreateRoleModel _roleModel;

		public Sprite currentTouXiangSprite;

		public string RolerName => _panel.Name_Ipt.text;

		public override void Awake(params object[] paralist)
		{
			base.Awake();
			_roleModel = new CreateRoleModel();
			_panel = base.GameObject.GetComponent<CreateRolePanel>();
			Init();
		}

		public void Init()
		{
			WuXingButtonDic.Add(WuXing.Jin, _panel.Jin_Btn);
			WuXingChooseButtonDic.Add(WuXing.Jin, _panel.JinChoose_Btn);
			WuXingButtonDic.Add(WuXing.Mu, _panel.Mu_Btn);
			WuXingChooseButtonDic.Add(WuXing.Mu, _panel.MuChoose_Btn);
			WuXingButtonDic.Add(WuXing.Shui, _panel.Shui_Btn);
			WuXingChooseButtonDic.Add(WuXing.Shui, _panel.ShuiChoose_Btn);
			WuXingButtonDic.Add(WuXing.Huo, _panel.Huo_Btn);
			WuXingChooseButtonDic.Add(WuXing.Huo, _panel.HuoChoose_Btn);
			WuXingButtonDic.Add(WuXing.Tu, _panel.Tu_Btn);
			WuXingChooseButtonDic.Add(WuXing.Tu, _panel.TuChoose_Btn);
			ApplyElementArt();
			LoadSprite();
			AddButtonClickListener(_panel.Panel_1_BackBtn, CloseWindow);
			AddButtonClickListener(_panel.Jin_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.Mu_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.Shui_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.Huo_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.Tu_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.JinChoose_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.MuChoose_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.ShuiChoose_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.HuoChoose_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.TuChoose_Btn, WuXingBtnClick);
			AddButtonClickListener(_panel.ManBtn, ManOrWomenClick);
			AddButtonClickListener(_panel.WomenBtn, ManOrWomenClick);
			AddButtonClickListener(_panel.NextBtn, PageSkip);
			AddButtonClickListener(_panel.Part2NextBtn, PageSkip);
			AddButtonClickListener(_panel.Part2BackBtn, PageSkip);
			AddButtonClickListener(_panel.Part3BackBtn, PageSkip);
			AddToggleClickListener(_panel.TouXiangYi_toggle, TouXiangToggleYiClick);
			AddToggleClickListener(_panel.TouXiangEr_toggle, TouXiangToggleErClick);
			AddToggleClickListener(_panel.TouXiangSan_toggle, TouXiangToggleSanClick);
			AddToggleClickListener(_panel.TouXiangSi_toggle, TouXiangToggleSiClick);
			AddToggleClickListener(_panel.TouXiangWu_toggle, TouXiangToggleWuClick);
			AddToggleClickListener(_panel.TouXiangLiu_toggle, TouXiangToggleLiuClick);
			AddButtonClickListener(_panel.Createrolerbtn, CreateRolerBtnClick);
			AddButtonClickListener(_panel.Part4BackBtn, Part4BackBtnClick);
			AddButtonClickListener(_panel.XiaoWenHao, XiaoWenHaoXQ);
			_panel.Name_Ipt.onValueChanged.AddListener(PanDuanShuRu);
			Transform XieYiPanel = _panel.transform.Find("XieYiPanel");
			Button btn = XieYiPanel.transform.Find("Image").transform.Find("YongHuXieYiBtn").GetComponent("Button") as Button;
			Button btn2 = XieYiPanel.transform.Find("Image").transform.Find("YinSiBaoHuXieYiBtn").GetComponent("Button") as Button;
			Button btn3 = XieYiPanel.transform.Find("Image").Find("JuJueBtn").GetComponent("Button") as Button;
			Button btn4 = XieYiPanel.transform.Find("Image").Find("TongYiBtn").GetComponent("Button") as Button;
			AddButtonClickListener(btn3, () =>
			{
				MonoSingleton<GameRoot>.Instance.ExitApplication();
			});
			AddButtonClickListener(btn4, () =>
			{
				XieYiPanel.gameObject.SetActive(value: false);
			});
			AddButtonClickListener(btn, () =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("Common_WebWnd.prefab", true, "https://www.baidu.com");
			});
			AddButtonClickListener(btn2, () =>
			{
				Singleton<UIManager>.Instance.PopUpWnd("Common_WebWnd.prefab", true, "https://www.bilibili.com");
			});
		}

		private void ApplyElementArt()
		{
			Font font = _panel.RoleNameText.font;
			foreach (var entry in WuXingButtonDic)
			{
				string name = WorldSetting.GetWuXingName(entry.Key);
				InkElementMotion.StyleChoice(entry.Value, entry.Key, name, font, selected: false);
				InkElementMotion.StyleChoice(WuXingChooseButtonDic[entry.Key], entry.Key, name, font, selected: true);
			}
			InkElementMotion.StyleSummary(_panel.WuXingLingGenJin, WuXing.Jin);
			InkElementMotion.StyleSummary(_panel.WuXingLingGenMu, WuXing.Mu);
			InkElementMotion.StyleSummary(_panel.WuXingLingGenShui, WuXing.Shui);
			InkElementMotion.StyleSummary(_panel.WuXingLingGenHuo, WuXing.Huo);
			InkElementMotion.StyleSummary(_panel.WuXingLingGenTu, WuXing.Tu);
			Text rule = _panel.Part2Panel.Find("WuXingJieShao")?.GetComponent<Text>();
			if (rule != null)
			{
				rule.text = "可多选，所选灵根平分 100 点。";
				rule.color = InkInterfaceArt.SoftInk;
				RectTransform caption = rule.rectTransform;
				const float captionWidth = 245f;
				float extraWidth = captionWidth - caption.rect.width;
				caption.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, captionWidth);
				caption.anchoredPosition += Vector2.right * (caption.pivot.x * extraWidth);
				rule.horizontalOverflow = HorizontalWrapMode.Wrap;
				UIManager.FitTextToRect(rule);
			}
		}

		public void LoadSprite()
		{
			touXiangYi = _panel.TouXiangYi_toggle.transform.Find("Background").gameObject.GetComponent<Image>();
			touXiangEr = _panel.TouXiangEr_toggle.transform.Find("Background").gameObject.GetComponent<Image>();
			touXiangSan = _panel.TouXiangSan_toggle.transform.Find("Background").gameObject.GetComponent<Image>();
			touXiangSi = _panel.TouXiangSi_toggle.transform.Find("Background").gameObject.GetComponent<Image>();
			touXiangWu = _panel.TouXiangWu_toggle.transform.Find("Background").gameObject.GetComponent<Image>();
			touXiangLiu = _panel.TouXiangLiu_toggle.transform.Find("Background").gameObject.GetComponent<Image>();
			_roleModel.sex = true;
			ChangeTouXiangImg(_roleModel.sex);
		}

		private void PanDuanShuRu(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				_panel.NextBtn.interactable = false;
			}
			else
			{
				_panel.NextBtn.interactable = true;
			}
		}

		public void PageSkip(Button _nextBtn)
		{
			if (_nextBtn == _panel.NextBtn)
			{
				if (string.IsNullOrEmpty(RolerName))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请输入姓名！");
					return;
				}
				for (int i = 0; i < RolerName.Length; i++)
				{
					if (RolerName[i] <= '\u007f')
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("请输入汉字！");
						return;
					}
				}
				if (!MonoSingleton<GameStart>.Instance.IsCanUseString(RolerName))
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("无效的姓名！");
					return;
				}
				CheckNameReq_1002005 data = new CheckNameReq_1002005
				{
					Name = RolerName
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.CheckCreateRoleName, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						CheckNameResp_1002005 checkNameResp_ = new CheckNameResp_1002005();
						checkNameResp_.MergeFrom(byteString);
						if (checkNameResp_.IsSuccess)
						{
							_roleModel.name = RolerName;
							_panel.Part2Panel.DOMoveY(0f, 0.4f);
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(checkNameResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
			else if (_nextBtn == _panel.Part2NextBtn)
			{
				if (ChooseButtonsList.Count < 1)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("必须选择一个五行灵根！");
					return;
				}
				ClearWuXingValue();
				_panel.RoleNameText.text = RolerName;
				switch (ChooseButtonsList.Count)
				{
				case 1:
					PanDuanWuXingLingGenValue(1.0);
					break;
				case 2:
					PanDuanWuXingLingGenValue(0.5);
					break;
				case 3:
					PanDuanWuXingLingGenValue(1.0 / 3.0);
					break;
				case 4:
					PanDuanWuXingLingGenValue(0.25);
					break;
				case 5:
					PanDuanWuXingLingGenValue(0.2);
					break;
				}
				ChangeImageSprite(currentTouXiangSprite, _panel.TouXiang);
				QueRenBtnClick();
			}
			else if (_nextBtn == _panel.Part2BackBtn)
			{
				_panel.Part2Panel.DOMoveY(-10f, 0.4f);
			}
			else if (_nextBtn == _panel.Part3BackBtn)
			{
				_panel.Part3Pane.DOMoveY(-10f, 0.4f);
			}
		}

		private void PanDuanWuXingLingGenValue(double value)
		{
			foreach (WuXing chooseButtons in ChooseButtonsList)
			{
				switch (chooseButtons)
				{
				case WuXing.Jin:
					_roleModel.gold = value;
					_panel.WuXingLingGenJin.gameObject.SetActive(value: true);
					break;
				case WuXing.Mu:
					_roleModel.wood = value;
					_panel.WuXingLingGenMu.gameObject.SetActive(value: true);
					break;
				case WuXing.Shui:
					_roleModel.water = value;
					_panel.WuXingLingGenShui.gameObject.SetActive(value: true);
					break;
				case WuXing.Huo:
					_roleModel.fire = value;
					_panel.WuXingLingGenHuo.gameObject.SetActive(value: true);
					break;
				case WuXing.Tu:
					_roleModel.earth = value;
					_panel.WuXingLingGenTu.gameObject.SetActive(value: true);
					break;
				}
			}
		}

		public void ManOrWomenClick(Button btn)
		{
			btn.gameObject.GetComponent<Image>().color = Color.black;
			if (btn == _panel.ManBtn)
			{
				_panel.TouXiangTitle_text.text = $"选择头像(男性)";
				_roleModel.sex = true;
				ChangeTouXiangImg(IsMan: true);
				_panel.WomenBtn.gameObject.GetComponent<Image>().color = Color.white;
				_panel.WomenChooseImg.gameObject.SetActive(value: false);
				_panel.WomenUnChooseImg.gameObject.SetActive(value: true);
				_panel.ManChooseImg.gameObject.SetActive(value: true);
				_panel.ManUnChooseImg.gameObject.SetActive(value: false);
			}
			else if (btn == _panel.WomenBtn)
			{
				_panel.TouXiangTitle_text.text = $"选择头像(女性)";
				_roleModel.sex = false;
				ChangeTouXiangImg(IsMan: false);
				_panel.ManBtn.gameObject.GetComponent<Image>().color = Color.white;
				_panel.ManChooseImg.gameObject.SetActive(value: false);
				_panel.ManUnChooseImg.gameObject.SetActive(value: true);
				_panel.WomenChooseImg.gameObject.SetActive(value: true);
				_panel.WomenUnChooseImg.gameObject.SetActive(value: false);
			}
		}

		public void ChangeTouXiangImg(bool IsMan)
		{
			Image[] portraitSlots = { touXiangYi, touXiangEr, touXiangSan, touXiangSi, touXiangWu, touXiangLiu };
			int sex = IsMan ? 1 : 2;
			RoleManager roles = Singleton<RoleManager>.Instance;
			for (int i = 0; i < portraitSlots.Length; i++)
			{
				ChangeImageSprite(roles.GetTouXiangSprite(sex, (i + 1).ToString()), portraitSlots[i]);
			}
			TouXiangToggleYiClick(isOn: true);
		}

		public void WuXingBtnClick(Button wuxingBtn)
		{
			switch (wuxingBtn.name)
			{
			case "Jin":
				WuXingBtn(WuXing.Jin);
				break;
			case "JinChoose":
				WuXingBtn(WuXing.Jin);
				break;
			case "Mu":
				WuXingBtn(WuXing.Mu);
				break;
			case "MuChoose":
				WuXingBtn(WuXing.Mu);
				break;
			case "Shui":
				WuXingBtn(WuXing.Shui);
				break;
			case "ShuiChoose":
				WuXingBtn(WuXing.Shui);
				break;
			case "Huo":
				WuXingBtn(WuXing.Huo);
				break;
			case "HuoChoose":
				WuXingBtn(WuXing.Huo);
				break;
			case "Tu":
				WuXingBtn(WuXing.Tu);
				break;
			case "TuChoose":
				WuXingBtn(WuXing.Tu);
				break;
			}
		}

		public void TouXiangToggleYiClick(bool isOn)
		{
			if (isOn)
			{
				_roleModel.headPortraitId = 1;
				currentTouXiangSprite = touXiangYi.sprite;
			}
		}

		public void TouXiangToggleErClick(bool isOn)
		{
			if (isOn)
			{
				_roleModel.headPortraitId = 2;
				currentTouXiangSprite = touXiangEr.sprite;
			}
		}

		public void TouXiangToggleSanClick(bool isOn)
		{
			if (isOn)
			{
				_roleModel.headPortraitId = 3;
				currentTouXiangSprite = touXiangSan.sprite;
			}
		}

		public void TouXiangToggleSiClick(bool isOn)
		{
			if (isOn)
			{
				_roleModel.headPortraitId = 4;
				currentTouXiangSprite = touXiangSi.sprite;
			}
		}

		public void TouXiangToggleWuClick(bool isOn)
		{
			if (isOn)
			{
				_roleModel.headPortraitId = 5;
				currentTouXiangSprite = touXiangWu.sprite;
			}
		}

		public void TouXiangToggleLiuClick(bool isOn)
		{
			if (isOn)
			{
				_roleModel.headPortraitId = 6;
				currentTouXiangSprite = touXiangLiu.sprite;
			}
		}

		public void MapChooseCompelete(MapEntity mapEntity, bool isCanChoose)
		{
			_panel.Createrolerbtn.interactable = isCanChoose;
			m_CurrentChooseMap = mapEntity;
		}

		public void QueRenBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_ANCESTOR, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AncestorMapResp_1007008 ancestorMapResp_ = new AncestorMapResp_1007008();
					ancestorMapResp_.MergeFrom(byteString);
					if (ancestorMapResp_.IsSuccess)
					{
						ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
						{
							MapId = ancestorMapResp_.Id
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _item) =>
						{
							ByteString byteString2 = _item as ByteString;
							if (!(byteString2 == null))
							{
								ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
								childrenMapResp_.MergeFrom(byteString2);
								if (childrenMapResp_.IsSuccess)
								{
									List<MapEntity> list = new List<MapEntity>();
									foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_.Object)
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
									Singleton<UIManager>.Instance.PopUpWnd("WorldMapWnd.prefab", true, MapEnterType.CreateRole, list, new List<string>());
									_panel.Part2Panel.gameObject.SetActive(value: false);
									_panel.Part1Panel.gameObject.SetActive(value: false);
									_panel.MapPartRect.gameObject.SetActive(value: true);
									base.GameObject.transform.SetAsLastSibling();
								}
								else
								{
									string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code);
									MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
								}
							}
						});
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(ancestorMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateRolerBtnClick()
		{
			if (m_CurrentChooseMap.MapType != 51)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请选择最低等级为1的城市做为出生地!");
				return;
			}
			_roleModel.userId = Singleton<RoleManager>.Instance.UserId;
			_roleModel.serverId = Singleton<RoleManager>.Instance.ServerId;
			_roleModel.mapId = m_CurrentChooseMap.Id;
			_roleModel.clientIp = CommonMethod.GetIP(ADDRESSFAM.IPv4);
			RoleCreateReq_1002001 data = new RoleCreateReq_1002001
			{
				ClientIp = _roleModel.clientIp,
				Earth = _roleModel.earth,
				Fire = _roleModel.fire,
				Gold = _roleModel.gold,
				Water = _roleModel.water,
				Wood = _roleModel.wood,
				HeadPortraitId = _roleModel.headPortraitId,
				MapId = _roleModel.mapId,
				Name = _roleModel.name,
				ServerId = _roleModel.serverId,
				Sex = _roleModel.sex,
				UserId = _roleModel.userId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CREATE, data, CreateRoleCallBack);
			Singleton<UIManager>.Instance.PopUpWnd("NetMessageWnd.prefab", true, "正在创建人物");
		}

		private void CreateRoleCallBack(object _obj)
		{
			ByteString byteString = _obj as ByteString;
			if (!(byteString == null))
			{
				RoleCreateResp_1002001 roleCreateResp_ = new RoleCreateResp_1002001();
				roleCreateResp_.MergeFrom(byteString);
				if (roleCreateResp_.IsSuccess)
				{
					AfterCreateLogin(roleCreateResp_.RoleId);
					return;
				}
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab");
				string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleCreateResp_.Code);
				MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
			}
		}

		private void AfterCreateLogin(string roleId)
		{
			RoleLoginReq_1002002 data = new RoleLoginReq_1002002
			{
				GameServerIpManageId = Singleton<RoleManager>.Instance.gameServerIpManageId,
				RoleId = roleId,
				LoginIp = CommonMethod.GetIP(ADDRESSFAM.IPv4),
				GameServerId = Singleton<RoleManager>.Instance.ServerId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_LOGIN, data, (object _item) =>
			{
				ByteString byteString = _item as ByteString;
				if (!(byteString == null))
				{
					RoleLoginResp_1002002 roleInfo = new RoleLoginResp_1002002();
					roleInfo.MergeFrom(byteString);
					if (roleInfo.IsSuccess)
					{
						ChildrenMapReq_1007003 data2 = new ChildrenMapReq_1007003
						{
							MapId = roleInfo.MapId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data2, (object _obj) =>
						{
							ByteString byteString2 = _obj as ByteString;
							if (!(byteString2 == null))
							{
								ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
								childrenMapResp_.MergeFrom(byteString2);
								if (childrenMapResp_.IsSuccess)
								{
									List<MapEntity> list = new List<MapEntity>();
									foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_.Object)
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
									BuglyAgent.SetUserId($"roleId:{roleInfo.Id},roleName{roleInfo.Name}");
									Singleton<RoleManager>.Instance.SetValue(roleInfo, list);
									Singleton<ChatManager>.Instance.Init();
									Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
								}
								else
								{
									string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code);
									MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
								}
							}
						});
					}
					else
					{
						Singleton<UIManager>.Instance.CloseWnd(this);
						Singleton<UIManager>.Instance.CloseWnd("NetMessageWnd.prefab");
						MonoSingleton<GameStart>.Instance.PopUpAttention($"登录失败,{roleInfo.Code}");
					}
				}
			});
		}

		private void Part4BackBtnClick()
		{
			_panel.Part3Pane.gameObject.SetActive(value: true);
			_panel.Part2Panel.gameObject.SetActive(value: true);
			_panel.Part1Panel.gameObject.SetActive(value: true);
			_panel.MapPartRect.gameObject.SetActive(value: false);
			Singleton<UIManager>.Instance.CloseWnd("WorldMapWnd.prefab", destory: false);
		}

		private void XiaoWenHaoXQ()
		{
			string text = "灵根总值为100点，所有灵根平分灵根总值。每多选择一种灵根意味着可以多使用一种五行功法，但同时也意味着降低了纳气效率。天灵根（单一属性）的修士修行速度非常快，但是需要控灵阵的辅助，花费不菲。五行灵根的修士修行速度虽然慢，但基础稳固，可以使用不同五行功法，战斗时不容易被针对。玄天楼出售补天丹，每位修士可以服用一枚，补足一个灵根，当然此八阶丹药需要渡劫期方可服用，且售价惊人，非常昂贵。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵根");
		}

		private void OnLoadSprite(string path, Object obj, object param1 = null, object param2 = null, object param3 = null)
		{
			if (obj != null)
			{
				Sprite value = obj as Sprite;
				if (!SpriteDic.ContainsKey(path))
				{
					SpriteDic.Add(path, value);
				}
			}
		}

		public void WuXingBtn(WuXing wuXing)
		{
			if (!ChooseButtonsList.Contains(wuXing))
			{
				if (ChooseButtonsList.Count >= 5)
				{
					WuXingButtonDic[ChooseButtonsList[0]].gameObject.SetActive(value: true);
					WuXingChooseButtonDic[ChooseButtonsList[0]].gameObject.SetActive(value: false);
					ChooseButtonsList.Remove(ChooseButtonsList[0]);
				}
				WuXingButtonDic[wuXing].gameObject.SetActive(value: false);
				WuXingChooseButtonDic[wuXing].gameObject.SetActive(value: true);
				ChooseButtonsList.Add(wuXing);
				_panel.Part2NextBtn.interactable = true;
			}
			else
			{
				ChooseButtonsList.Remove(wuXing);
				WuXingButtonDic[wuXing].gameObject.SetActive(value: true);
				WuXingChooseButtonDic[wuXing].gameObject.SetActive(value: false);
				if (ChooseButtonsList.Count <= 0)
				{
					_panel.Part2NextBtn.interactable = false;
				}
			}
		}

		public void ClearWuXingValue()
		{
			_roleModel.gold = 0.0;
			_roleModel.earth = 0.0;
			_roleModel.fire = 0.0;
			_roleModel.wood = 0.0;
			_roleModel.water = 0.0;
			_panel.WuXingLingGenJin.gameObject.SetActive(value: false);
			_panel.WuXingLingGenMu.gameObject.SetActive(value: false);
			_panel.WuXingLingGenShui.gameObject.SetActive(value: false);
			_panel.WuXingLingGenHuo.gameObject.SetActive(value: false);
			_panel.WuXingLingGenTu.gameObject.SetActive(value: false);
		}

		public override void OnClose()
		{
			base.OnClose();
			_panel.Name_Ipt.onValueChanged.RemoveAllListeners();
		}
	}
}
