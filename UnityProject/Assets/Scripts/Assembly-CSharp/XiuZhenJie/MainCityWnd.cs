using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;

namespace XiuZhenJie
{
	public class MainCityWnd : Window
	{
		public MainCityPanel mainCityPanel;

		public Queue<string> MessageRadioQueue;

		private bool isRolling;

		private List<MapEntity> buildModel;

		private float m_ScreenWith;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			MessageRadioQueue = new Queue<string>();
			mainCityPanel = base.GameObject.GetComponent<MainCityPanel>();
			InkInterfaceArt.StyleCity(mainCityPanel);
			AddButtonClickListener(mainCityPanel.XuShenJieBtn, JinRuXuShenJie);
			AddButtonClickListener(mainCityPanel.ZhongJieFangShiBtn, EnterZhongJieFangShi);
			AddButtonClickListener(mainCityPanel.XuanTianLouBtn, XuanTianLou);
			AddButtonClickListener(mainCityPanel.ChuanSongDaZhenBtn, ChuanSongDaZhenClick);
			AddButtonClickListener(mainCityPanel.QianDaoBtn, QianDaoBtnClick);
			AddButtonClickListener(mainCityPanel.HuoDongBtn, HuoDongBtnClick);
			AddButtonClickListener(mainCityPanel.ChengZhuFuBtn, ChengZhuFu);
			AddButtonClickListener(mainCityPanel.LingQuanJiuSiBtn, EnterLingQuanJiuSi);
			AddButtonClickListener(mainCityPanel.XinJianBtn, XinJianBtnClick);
			AddButtonClickListener(mainCityPanel.YueKe, XuanTianLing);
			AddButtonClickListener(mainCityPanel.TianDaoLingBtn, TianDaoLing);
			AddButtonClickListener(mainCityPanel.GongLueBtn, OpenGongLueWnd);
			AddButtonClickListener(mainCityPanel.RankBtn, OpenRankWnd);
			AddButtonClickListener(mainCityPanel.ReturnLiBaoBtn, HuoDongBtnClick);
			RollingTextInit();
			m_ScreenWith = Singleton<UIManager>.Instance.m_UiRoot.sizeDelta.x;
		}

		public override void OnShow(params object[] paralist)
		{
			buildModel = new List<MapEntity>();
			base.OnShow(paralist);
			ReFlashData();
			ReflashMail(Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter);
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			CloseAllBtn();
			mainCityPanel.TitleText.text = Singleton<RoleManager>.Instance.Role_Entity.MapName;
			buildModel = ((Singleton<RoleManager>.Instance.CityBuilds == null) ? new List<MapEntity>() : Singleton<RoleManager>.Instance.CityBuilds);
			int num = Singleton<RoleManager>.Instance.Role_Entity.MaxAge - Singleton<RoleManager>.Instance.Role_Entity.CurrentAge;
			if (num <= 20)
			{
				mainCityPanel.ShengYuShouYuanText.text = $"剩余寿元：<size=14><color=red>{num}</color></size> 年";
			}
			else
			{
				mainCityPanel.ShengYuShouYuanText.text = $"剩余寿元：<size=14>{num}</size> 年";
			}
			foreach (MapEntity item in buildModel)
			{
				if (item.MapName.Contains("城主府"))
				{
					mainCityPanel.ChengZhuFuBtn.gameObject.gameObject.SetActive(value: true);
				}
				else if (item.MapName.Contains("玄天楼"))
				{
					mainCityPanel.XuanTianLouBtn.gameObject.gameObject.SetActive(value: true);
				}
				else if (item.MapName.Contains("虚神界"))
				{
					mainCityPanel.XuShenJieBtn.gameObject.gameObject.SetActive(value: true);
				}
				else if (item.MapName.Contains("坊市"))
				{
					mainCityPanel.ZhongJieFangShiBtn.gameObject.gameObject.SetActive(value: true);
				}
				else if (item.MapName.Contains("传送大阵"))
				{
					mainCityPanel.ChuanSongDaZhenBtn.gameObject.gameObject.SetActive(value: true);
				}
				else if (item.MapName.Contains("灵泉酒肆"))
				{
					mainCityPanel.LingQuanJiuSiBtn.gameObject.gameObject.SetActive(value: true);
				}
			}
			ReflashMissionStatus();
			YueKaTianShu();
		}

		public void ReflashMissionStatus()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.TaskStatus == 1 || Singleton<RoleManager>.Instance.Role_Entity.TianShuBeiStatus == 1)
			{
				mainCityPanel.ChengZhuFuHintImg.gameObject.SetActive(value: true);
			}
			else
			{
				mainCityPanel.ChengZhuFuHintImg.gameObject.SetActive(value: false);
			}
		}

		private void CloseAllBtn()
		{
			mainCityPanel.ChengZhuFuBtn.gameObject.gameObject.SetActive(value: false);
			mainCityPanel.XuanTianLouBtn.gameObject.gameObject.SetActive(value: false);
			mainCityPanel.XuShenJieBtn.gameObject.gameObject.SetActive(value: false);
			mainCityPanel.ZhongJieFangShiBtn.gameObject.gameObject.SetActive(value: false);
			mainCityPanel.ChuanSongDaZhenBtn.gameObject.gameObject.SetActive(value: false);
			mainCityPanel.LingQuanJiuSiBtn.gameObject.gameObject.SetActive(value: false);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (isRolling)
			{
				float num = 0f - (mainCityPanel.rollingText.rectTransform.sizeDelta.x + m_ScreenWith);
				if (mainCityPanel.rollingText.rectTransform.anchoredPosition.x < num)
				{
					string text = mainCityPanel.rollingText.text;
					MessageRadioQueue.Enqueue(text);
					mainCityPanel.rollingText.rectTransform.anchoredPosition = new Vector2(0f, 0f);
					isRolling = false;
				}
				mainCityPanel.rollingText.rectTransform.anchoredPosition += new Vector2(-1f, 0f);
				if (MessageRadioQueue != null && MessageRadioQueue.Count > 0 && !isRolling)
				{
					isRolling = true;
					TextRolling(MessageRadioQueue.Dequeue());
				}
			}
		}

		private void RollingTextInit()
		{
			MessageRadioQueue.Enqueue("目前虚神界已开放创造功能，可以自由创造属于自己的天阶丹方、法术、符书等，可于结丹期进入虚神界体验！");
			MessageRadioQueue.Enqueue("渡劫失败会有严重的伤势，玄天楼中出售的大还丹可以恢复经脉伤势，玩家炼制的恢复类丹药可以恢复其他伤势（寿元、神识、体魄等）");
			MessageRadioQueue.Enqueue("虚神界中的功法、法术、丹方、符书等，兑换时需要贡献点，贡献点则可通过创造获取，创造的物品被他人兑换也会收到贡献点奖励");
			MessageRadioQueue.Enqueue("战斗中，可以同时激发多柄法器（神识足够），并可以同时使用某一种法术，可以最大化伤害");
			MessageRadioQueue.Enqueue("术法师在战法室中修习法术，可以提升法术的熟练等级，熟练等级越高则施法速度越快，对敌中造成的伤害越大");
			MessageRadioQueue.Enqueue("丹师可以炼制各式各样的丹药，用以提升修行速度，也包括可以增加自己的寿元、恢复自身的伤势等");
			MessageRadioQueue.Enqueue("符师可以将已有的法术，封印入符，在战斗中快速施放，且施放的过程消耗极微，是快速提升战力的有效手段");
			MessageRadioQueue.Enqueue("布道师传道天下，他们可以快速领悟功法，修行中事半功倍，创造出强大的功法更是所有修行人追捧的对象");
			MessageRadioQueue.Enqueue("虚神塔在虚神界中，所有结丹期及以上的修士均可进入，虚神塔中幻兽凶猛，挑战更高的塔层可以留名虚神榜，且获得大量奖励");
			MessageRadioQueue.Enqueue("渡劫前，可以服用渡劫丹用以降低雷劫伤害，使用法器硬撼雷劫可以抵消部分伤害，法器伤害越高则抵消伤害越多，瞬移可以直接规避所有伤害");
			isRolling = true;
		}

		private void TextRolling(string text)
		{
			_ = text.Length;
			mainCityPanel.rollingText.text = text;
			mainCityPanel.rollingText.rectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		public void ReflashMail(bool isNew)
		{
			SetMailIsHaveNew(isNew);
		}

		private void QianDaoBtnClick()
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

		private void XinJianBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChuanXinWnd.prefab", true);
			SetMailIsHaveNew(isHaveNew: false);
		}

		public void YueKaTianShu()
		{
		}

		private bool ShowEventBtn()
		{
			HuoDongEntity huoDongEntity = null;
			for (int i = 0; i < Singleton<RoleManager>.Instance.EventInfoList.Count; i++)
			{
				if (Singleton<RoleManager>.Instance.EventInfoList[i].type == 3)
				{
					huoDongEntity = Singleton<RoleManager>.Instance.EventInfoList[i];
					break;
				}
			}
			if (huoDongEntity == null)
			{
				return false;
			}
			long createtime = CommonMethod.GetCreatetime();
			if (createtime >= huoDongEntity.startAt && createtime < huoDongEntity.endAt)
			{
				mainCityPanel.ReturnLiBaoBtn.gameObject.SetActive(value: true);
				return true;
			}
			mainCityPanel.ReturnLiBaoBtn.gameObject.SetActive(value: false);
			return false;
		}

		private void XuanTianLing()
		{
			Singleton<UIManager>.Instance.PopUpWnd("CashGoodsWnd.prefab", true, this);
		}

		private void TianDaoLing()
		{
			Singleton<UIManager>.Instance.PopUpWnd("CashGoodsWnd.prefab", true, this);
		}

		private void OpenGongLueWnd()
		{
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(ShowGongLue());
		}

		private IEnumerator ShowGongLue()
		{
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("version", "1.3.5.1");
			UnityWebRequest www = HttpsRequest.UnityGetRequest(Constants.LoginServerIP, Constants.LoginServerPort, "/externalLinks/query", dictionary);
			yield return www.SendWebRequest();
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			string text = www.downloadHandler.text;
			if (string.IsNullOrEmpty(text))
			{
				yield break;
			}
			HttpsRequest.DealwithShellModel(text, (HttpsRequestStatus status, ShellModel sm, string content) =>
			{
				switch (status)
				{
				case HttpsRequestStatus.ServerError:
					MonoSingleton<GameStart>.Instance.PopUpAttention("暂时无法打开论坛!");
					break;
				case HttpsRequestStatus.SuccessFul:
					Singleton<UIManager>.Instance.PopUpWnd("Common_WebWnd.prefab", true, sm.msgBody.bizInfo.dataString);
					break;
				}
			});
		}

		private void OpenRankWnd()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LevelRankWnd.prefab", true);
		}

		private void HuoDongBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("GuangGaoWnd.prefab", true);
		}

		private void ChengZhuFu()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChengZhuFuMainWnd.prefab", true);
		}

		private void XuanTianLou()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XuanTianLouWnd.prefab", true);
		}

		private void JinRuXuShenJie()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel <= 20)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("进入虚神界需要结丹一阶以上");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.Into_Unreal_World, null, null);
			Singleton<UIManager>.Instance.PopUpWnd("XuShenJieWnd.prefab", true);
		}

		private void EnterLingQuanJiuSi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LingQuanJiuSiWnd.prefab", true);
		}

		private void EnterZhongJieFangShi()
		{
			ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
			{
				MapId = Singleton<RoleManager>.Instance.Role_Entity.MapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
					childrenMapResp_.MergeFrom(byteString);
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
						Singleton<UIManager>.Instance.PopUpWnd("ZhongJieFangShiWnd.prefab", true, list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void ChuanSongDaZhenClick()
		{
			MapInfoReq_1007009 data = new MapInfoReq_1007009
			{
				MapId = Singleton<RoleManager>.Instance.Role_Entity.MapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_INFO, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MapInfoResp_1007009 mapInfoResp_ = new MapInfoResp_1007009();
					mapInfoResp_.MergeFrom(byteString);
					if (mapInfoResp_.IsSuccess)
					{
						MapEntity mapEntity = new MapEntity(mapInfoResp_);
						if (mapEntity.MapType == 54)
						{
							XiongChengChuanSong();
						}
						else if (mapEntity.MapType == 53)
						{
							ZhouChengChuanSong(mapEntity);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("该城池无法使用传送大阵！");
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(mapInfoResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void XiongChengChuanSong()
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
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							ByteString byteString2 = _item as ByteString;
							if (!(byteString2 == null))
							{
								ChildrenMapResp_1007003 childrenMapResp_1007003 = new ChildrenMapResp_1007003();
								childrenMapResp_1007003.MergeFrom(byteString2);
								if (childrenMapResp_1007003.IsSuccess)
								{
									List<MapEntity> mapList = new List<MapEntity>();
									foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_1007003.Object)
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
											ParentName = item2.ParentName,
											Sort = item2.Sort
										};
										mapList.Add(item);
									}
									ParentMapIdReq_1007005 data2 = new ParentMapIdReq_1007005
									{
										MapId = Singleton<RoleManager>.Instance.Role_Entity.MapId
									};
									Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_PARENT_ID, data2, (object _o) =>
									{
										MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
										ByteString byteString3 = _o as ByteString;
										if (!(byteString3 == null))
										{
											ParentMapIdResp_1007005 parentMapIdResp_ = new ParentMapIdResp_1007005();
											parentMapIdResp_.MergeFrom(byteString3);
											if (parentMapIdResp_.IsSuccess)
											{
												List<string> list = new List<string>();
												foreach (string item3 in parentMapIdResp_.Id)
												{
													list.Add(item3);
												}
												Singleton<UIManager>.Instance.PopUpWnd("WorldMapWnd.prefab", true, MapEnterType.ChuanSong, mapList, list, ChuanSongType.XiongCheng);
											}
											else
											{
												string hintMsgByCode3 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_1007003.Code);
												MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode3);
											}
										}
									}, isShowWaitUI: true);
								}
								else
								{
									string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_1007003.Code);
									MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
								}
							}
						}, isShowWaitUI: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(ancestorMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZhouChengChuanSong(MapEntity mapSource)
		{
			SibingsMapReq_1007001 data = new SibingsMapReq_1007001
			{
				MapId = mapSource.ParentId
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
						List<MapEntity> mapList = new List<MapEntity>();
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
							mapList.Add(item);
						}
						ParentMapIdReq_1007005 data2 = new ParentMapIdReq_1007005
						{
							MapId = Singleton<RoleManager>.Instance.Role_Entity.MapId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_PARENT_ID, data2, (object _o) =>
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							ByteString byteString2 = _o as ByteString;
							if (!(byteString2 == null))
							{
								ParentMapIdResp_1007005 parentMapIdResp_ = new ParentMapIdResp_1007005();
								parentMapIdResp_.MergeFrom(byteString2);
								if (parentMapIdResp_.IsSuccess)
								{
									List<string> list = new List<string>();
									foreach (string item3 in parentMapIdResp_.Id)
									{
										list.Add(item3);
									}
									Singleton<UIManager>.Instance.PopUpWnd("WorldMapWnd.prefab", true, MapEnterType.ChuanSong, mapList, list, ChuanSongType.ZhouCheng);
								}
								else
								{
									string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(parentMapIdResp_.Code);
									MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
								}
							}
						}, isShowWaitUI: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sibingsMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
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
						List<MapEntity> mapList = new List<MapEntity>();
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
							mapList.Add(item);
						}
						ParentMapIdReq_1007005 data2 = new ParentMapIdReq_1007005
						{
							MapId = Singleton<RoleManager>.Instance.Role_Entity.MapId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_PARENT_ID, data2, (object _o) =>
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							ByteString byteString2 = _o as ByteString;
							if (!(byteString2 == null))
							{
								ParentMapIdResp_1007005 parentMapIdResp_ = new ParentMapIdResp_1007005();
								parentMapIdResp_.MergeFrom(byteString2);
								if (parentMapIdResp_.IsSuccess)
								{
									List<string> list = new List<string>();
									foreach (string item3 in parentMapIdResp_.Id)
									{
										list.Add(item3);
									}
									Singleton<UIManager>.Instance.PopUpWnd("WorldMapWnd.prefab", true, MapEnterType.WaiChuLiLiang, mapList, list);
									_ = Singleton<GuideManager>.Instance.YinDaoJilu1.ZhanDou;
								}
								else
								{
									string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(parentMapIdResp_.Code);
									MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
								}
							}
						}, isShowWaitUI: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sibingsMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void SetMailIsHaveNew(bool isHaveNew)
		{
			mainCityPanel.NewXinJianTagImg.gameObject.SetActive(isHaveNew);
		}
	}
}
