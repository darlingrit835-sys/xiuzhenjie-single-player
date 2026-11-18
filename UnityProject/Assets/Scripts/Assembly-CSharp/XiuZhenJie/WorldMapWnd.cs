using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WorldMapWnd : Window
	{
		public const int WAICHU_JINGLI = 50;

		public const int WAICHU_SHOUYUAN = 1;

		public WorldMapPanel Panel;

		private MapEnterType m_EnterFrom;

		private List<MapEntity> m_DataSource = new List<MapEntity>();

		private Stack<CSZ_MapPanel> m_PanelStack = new Stack<CSZ_MapPanel>();

		private CSZ_MapPanel m_CurrentMapPanel;

		private List<string> m_ParentIds = new List<string>();

		private ChuanSongType m_ChuanSongType;

		private GameObject m_MapZongMenItemInsObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WorldMapPanel>();
			InkInterfaceArt.StyleMap(Panel);
			AddButtonClickListener(Panel.BackBtn, BackPage);
			AddButtonClickListener(Panel.XiaoWenHao, XiaoWenHaoXQ);
			AddToggleClickListener(Panel.ZongPaiShanMenTog, ZongPaiShanMenTogClick);
			m_MapZongMenItemInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/MapZongMenItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_EnterFrom = (MapEnterType)paralist[0];
			m_DataSource = paralist[1] as List<MapEntity>;
			if (paralist.Length == 3)
			{
				m_ParentIds = ((!(paralist[2] is List<string>)) ? new List<string>() : (paralist[2] as List<string>));
			}
			else if (paralist.Length == 4)
			{
				m_ParentIds = ((!(paralist[2] is List<string>)) ? new List<string>() : (paralist[2] as List<string>));
				m_ChuanSongType = (ChuanSongType)paralist[3];
			}
			switch (m_EnterFrom)
			{
			case MapEnterType.CreateRole:
				CreateRoleInit();
				break;
			case MapEnterType.WaiChuLiLiang:
				WaiChuLiLianInit();
				break;
			case MapEnterType.ChuanSong:
				ChuanSongInit();
				break;
			}
		}

		private void BackPage()
		{
			if (m_EnterFrom == MapEnterType.CreateRole)
			{
				Singleton<UIManager>.Instance.FindWndByName<CreateRoleWnd>("CreateRoleWnd.prefab")?.MapChooseCompelete(null, isCanChoose: false);
				if (m_PanelStack.Count > 0)
				{
					CSZ_MapPanel cSZ_MapPanel = m_PanelStack.Pop();
					cSZ_MapPanel.gameObject.SetActive(value: true);
					m_CurrentMapPanel.gameObject.SetActive(value: false);
					m_CurrentMapPanel = cSZ_MapPanel;
					Panel.TitleText.text = m_CurrentMapPanel.TitleName;
				}
				else
				{
					Panel.BackBtn.gameObject.SetActive(value: false);
				}
			}
			else if (m_EnterFrom == MapEnterType.WaiChuLiLiang)
			{
				if (!Panel.LiLianDiTuTog.isOn)
				{
					Panel.LiLianDiTuTog.isOn = true;
				}
				if (m_PanelStack.Count == 0)
				{
					Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.LiLianBtnCloseClick();
					return;
				}
				CSZ_MapPanel cSZ_MapPanel2 = m_PanelStack.Pop();
				cSZ_MapPanel2.gameObject.SetActive(value: true);
				m_CurrentMapPanel.gameObject.SetActive(value: false);
				m_CurrentMapPanel = cSZ_MapPanel2;
				Panel.TitleText.text = m_CurrentMapPanel.TitleName;
			}
			else if (m_ChuanSongType == ChuanSongType.XiongCheng)
			{
				if (m_PanelStack.Count == 0)
				{
					Singleton<UIManager>.Instance.CloseWnd("WorldMapWnd.prefab", destory: false);
					return;
				}
				CSZ_MapPanel cSZ_MapPanel3 = m_PanelStack.Pop();
				cSZ_MapPanel3.gameObject.SetActive(value: true);
				m_CurrentMapPanel.gameObject.SetActive(value: false);
				m_CurrentMapPanel = cSZ_MapPanel3;
				Panel.TitleText.text = m_CurrentMapPanel.TitleName;
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd("WorldMapWnd.prefab", destory: false);
			}
		}

		private void XiaoWenHaoXQ()
		{
			string text = "修身界共有五片大陆，每大陆有12个州，每个州均分布有12个地块。有些地方人类修士建立了城池，有些地方依旧妖兽横行。城池中可以修整战斗状态、交易战利品，而妖兽横行的区域则是获取修行资源、获取奇珍异宝的好地方。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "地图");
		}

		private void WaiChuLiLianInit()
		{
			switch (m_DataSource.Count)
			{
			case 9:
				SwitchChildWnd(null, MapType.MapJun_9, m_DataSource);
				break;
			case 10:
				SwitchChildWnd(null, MapType.MapJun_10, m_DataSource);
				break;
			case 11:
				SwitchChildWnd(null, MapType.MapJun_11, m_DataSource);
				break;
			case 12:
				SwitchChildWnd(null, MapType.MapJun_12, m_DataSource);
				break;
			}
		}

		private void ChuanSongInit()
		{
			if (m_ChuanSongType == ChuanSongType.XiongCheng)
			{
				SwitchChildWnd(null, MapType.DaLu, m_DataSource);
				return;
			}
			string mapId = m_ParentIds[m_ParentIds.Count - 2];
			MapInfoReq_1007009 data = new MapInfoReq_1007009
			{
				MapId = mapId
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
						MapType mapType = MapType.None;
						switch (m_DataSource.Count)
						{
						case 9:
							switch (mapEntity.Sort)
							{
							case 1:
								mapType = MapType.Map_1_9;
								break;
							case 2:
								mapType = MapType.Map_2_9;
								break;
							case 3:
								mapType = MapType.Map_3_9;
								break;
							case 4:
								mapType = MapType.Map_5_9;
								break;
							case 5:
								mapType = MapType.Map_4_9;
								break;
							}
							break;
						case 10:
							switch (mapEntity.Sort)
							{
							case 1:
								mapType = MapType.Map_1_10;
								break;
							case 2:
								mapType = MapType.Map_2_10;
								break;
							case 3:
								mapType = MapType.Map_3_10;
								break;
							case 4:
								mapType = MapType.Map_5_10;
								break;
							case 5:
								mapType = MapType.Map_4_10;
								break;
							}
							break;
						case 11:
							switch (mapEntity.Sort)
							{
							case 1:
								mapType = MapType.Map_1_11;
								break;
							case 2:
								mapType = MapType.Map_2_11;
								break;
							case 3:
								mapType = MapType.Map_3_11;
								break;
							case 4:
								mapType = MapType.Map_5_11;
								break;
							case 5:
								mapType = MapType.Map_4_11;
								break;
							}
							break;
						case 12:
							switch (mapEntity.Sort)
							{
							case 1:
								mapType = MapType.Map_1_12;
								break;
							case 2:
								mapType = MapType.Map_2_12;
								break;
							case 3:
								mapType = MapType.Map_3_12;
								break;
							case 4:
								mapType = MapType.Map_4_12;
								break;
							case 5:
								mapType = MapType.Map_5_12;
								break;
							}
							break;
						}
						SwitchChildWnd(null, mapType, m_DataSource);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateRoleInit()
		{
			SwitchChildWnd(null, MapType.DaLu, m_DataSource);
		}

		public void SwitchChildWnd(CSZ_MapPanel FromPanel, MapType _mapType, List<MapEntity> mapDataSource)
		{
			CSZ_MapPanel cSZ_MapPanel = null;
			if (m_CurrentMapPanel != null && m_CurrentMapPanel.mapType == _mapType)
			{
				GameObject gameObject = Object.Instantiate(m_CurrentMapPanel.gameObject);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				gameObject.transform.SetParent(m_CurrentMapPanel.gameObject.transform.parent);
				component.sizeDelta = Vector2.zero;
				component.anchoredPosition = Vector2.zero;
				component.localScale = Vector3.one;
				Object.Destroy(gameObject.GetComponent<CSZ_MapPanel>());
				cSZ_MapPanel = gameObject.AddComponent<CSZ_MapPanel>();
			}
			else
			{
				cSZ_MapPanel = GetWndPanel<CSZ_MapPanel>(_mapType);
			}
			if (Singleton<RoleManager>.Instance.Role_Entity != null)
			{
				m_ParentIds.Add(Singleton<RoleManager>.Instance.Role_Entity.MapId);
			}
			cSZ_MapPanel.SetValue(_mapType, mapDataSource, m_ParentIds, m_EnterFrom);
			if (FromPanel != null)
			{
				FromPanel.ClearValue();
			}
			if (m_CurrentMapPanel != null)
			{
				m_PanelStack.Push(m_CurrentMapPanel);
			}
			if (m_EnterFrom != MapEnterType.CreateRole)
			{
				if (m_PanelStack.Count >= 1)
				{
					Panel.BackBtn.gameObject.SetActive(value: true);
				}
			}
			else if (m_PanelStack.Count > 0)
			{
				Panel.BackBtn.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.BackBtn.gameObject.SetActive(value: false);
			}
			m_CurrentMapPanel = cSZ_MapPanel;
			Panel.TitleText.text = m_CurrentMapPanel.TitleName;
		}

		public void ZongPaiShanMenTogClick(bool isOn)
		{
			if (isOn)
			{
				Panel.MapWaiWei_1_Extend.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
			}
			else
			{
				Panel.MapWaiWei_1_Extend.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			}
		}

		public void PopChuanSongCityPanel(int amount, string targetMapId, string cityName)
		{
			float num = 1f;
			if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 9)
			{
				num = 0.5f;
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 5)
			{
				num = 0.8f;
			}
			float price = (float)amount * num;
			Panel.TiShiPanel.SetValue(cityName, (int)price, Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - amount, () =>
			{
				if (Singleton<RoleManager>.Instance.ShengYuLingShi(price) > 0.0)
				{
					ChuanSongToNewCity(targetMapId, cityName, MoveCityType.ChuangSong);
				}
				else
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("您的灵石不足，无法传送");
				}
				Panel.TiShiPanel.ClosePanel();
			});
		}

		public void PopWaiChuLiLianMsg(string targetMapId, string cityName)
		{
			int xiaoHaoJingLi = 50;
			float num = 1f;
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 11)
			{
				xiaoHaoJingLi -= (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel - 11) * 2;
				num -= (float)(Singleton<RoleManager>.Instance.Role_Entity.RoleLevel - 11) * 0.05f;
				xiaoHaoJingLi = ((xiaoHaoJingLi >= 0) ? xiaoHaoJingLi : 0);
				num = ((num < 0f) ? 0f : num);
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention(string.Format("移动到{0}需要消耗{1}点体力和增长{2}点寿元,是否移动到{0}", cityName, xiaoHaoJingLi, num.ToString("0.00")), () =>
			{
				if (targetMapId == Singleton<RoleManager>.Instance.Role_Entity.MapId)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("已在该城池中！");
				}
				else if (Singleton<RoleManager>.Instance.CheckJingLi(xiaoHaoJingLi))
				{
					ChuanSongToNewCity(targetMapId, cityName, MoveCityType.Move);
				}
				else
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("精力不足,无法移动！");
				}
			});
		}

		public void ChuanSongToNewCity(string targetMapId, string cityName, MoveCityType moveCityType)
		{
			if (moveCityType == MoveCityType.Move)
			{
				MapCityMigrateReq_1007011 data = new MapCityMigrateReq_1007011
				{
					MapId = targetMapId
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CITYMIGRATE, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						MapCityMigrateResp_1007011 mapCityMigrateResp_1007011 = new MapCityMigrateResp_1007011();
						mapCityMigrateResp_1007011.MergeFrom(byteString);
						if (mapCityMigrateResp_1007011.IsSuccess)
						{
							ChildrenMapReq_1007003 data3 = new ChildrenMapReq_1007003
							{
								MapId = targetMapId
							};
							Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data3, (object _item) =>
							{
								MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
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
										Singleton<RoleManager>.Instance.Role_Entity.CurrentPower = mapCityMigrateResp_1007011.CurrentPower;
										Singleton<RoleManager>.Instance.Role_Entity.CurrentAge = mapCityMigrateResp_1007011.CurrentAge;
										Singleton<RoleManager>.Instance.SetNewMapInfo(list, targetMapId, cityName);
										BackPage();
									}
									else
									{
										MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code));
									}
								}
							}, isShowWaitUI: true);
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(mapCityMigrateResp_1007011.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
				return;
			}
			MapTransmitReq_1007010 data2 = new MapTransmitReq_1007010
			{
				MapId = targetMapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_TRANSMIT, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MapTransmitResp_1007010 mapTransmitResp_1007010 = new MapTransmitResp_1007010();
					mapTransmitResp_1007010.MergeFrom(byteString);
					if (mapTransmitResp_1007010.IsSuccess)
					{
						ChildrenMapReq_1007003 data3 = new ChildrenMapReq_1007003
						{
							MapId = targetMapId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data3, (object _item) =>
						{
							ByteString byteString2 = _item as ByteString;
							if (!(byteString2 == null))
							{
								ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
								childrenMapResp_.MergeFrom(byteString2);
								if (childrenMapResp_.IsSuccess)
								{
									List<MapEntity> list = new List<MapEntity>();
									foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item3 in childrenMapResp_.Object)
									{
										MapEntity item = new MapEntity
										{
											Depth = item3.Level,
											FiveElement = item3.FiveElements,
											Id = item3.Id,
											MapName = item3.Name,
											MapType = item3.Type,
											MaxLevel = item3.MaxLevel,
											MinLevel = item3.MinLevel,
											ParentName = item3.ParentName
										};
										list.Add(item);
									}
									Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = mapTransmitResp_1007010.ShinyStone;
									Singleton<RoleManager>.Instance.SetNewMapInfo(list, targetMapId, cityName);
									Singleton<UIManager>.Instance.CloseWnd(this);
								}
								else
								{
									MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code));
								}
							}
						});
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(mapTransmitResp_1007010.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public void GetZongMenData(string mapId, string heXinMapId)
		{
			foreach (Transform item in Panel.MapWaiWei_1_Extend_Content)
			{
				Object.Destroy(item.gameObject);
			}
			GuildListReq_1015001 data = new GuildListReq_1015001
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_LIST, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildListResp_1015001 guildListResp_ = new GuildListResp_1015001();
					guildListResp_.MergeFrom(byteString);
					if (guildListResp_.IsSuccess)
					{
						int num = 0;
						if (guildListResp_.SelfGuild != null)
						{
							if (!guildListResp_.SelfGuild.IsCreatedGuild)
							{
								CreateSelfZongMenItem(heXinMapId, guildListResp_.SelfGuild.GuildMapId, guildListResp_.SelfGuild.RemainMonsterCount, guildListResp_.SelfGuild.MonsterNum);
							}
							else
							{
								CreateMapZongMenItem(guildListResp_.SelfGuild.GuildId.ToString(), guildListResp_.SelfGuild.Name, isYour: true);
							}
							num++;
						}
						for (int i = 0; i < guildListResp_.GuildList.Count; i++)
						{
							CreateMapZongMenItem(guildListResp_.GuildList[i].GuildId.ToString(), guildListResp_.GuildList[i].Name, isYour: false);
							num++;
						}
						float y = m_MapZongMenItemInsObj.gameObject.GetComponent<RectTransform>().sizeDelta.y;
						VerticalLayoutGroup component = Panel.MapWaiWei_1_Extend_Content.gameObject.GetComponent<VerticalLayoutGroup>();
						float y2 = SetVerticalListContentHight(num, y, component);
						Panel.MapWaiWei_1_Extend_Content.sizeDelta = new Vector2(Panel.MapWaiWei_1_Extend_Content.sizeDelta.x, y2);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateSelfZongMenItem(string mapid, string goodsId, int currentCount, int maxCount)
		{
			GameObject gameObject = SetListContentParent(Panel.MapWaiWei_1_Extend_Content, m_MapZongMenItemInsObj);
			gameObject.name = "SelfZongMenItem";
			gameObject.GetComponent<MapZongMenItem>().SetDongTianFuDiValue(mapid, goodsId, currentCount, maxCount, QingLiYaoShou, KaiZongLiPai);
		}

		public void ReflashSelfItem(string zongMenId, string zongMenName)
		{
			GameObject gameObject = Panel.MapWaiWei_1_Extend_Content.transform.Find("SelfZongMenItem").gameObject;
			if (!(gameObject == null))
			{
				gameObject.GetComponent<MapZongMenItem>().ReflashZongValue(zongMenId, zongMenName, FanHuiZongMen);
			}
		}

		private void CreateMapZongMenItem(string id, string name, bool isYour)
		{
			SetListContentParent(Panel.MapWaiWei_1_Extend_Content, m_MapZongMenItemInsObj).GetComponent<MapZongMenItem>().SetZongMenValue(id, name, isYour, BaiRuShanMen, FanHuiZongMen);
		}

		private void QingLiYaoShou(string mapId, string goodsId)
		{
			MapEntity mapModel = new MapEntity
			{
				Id = mapId,
				MapName = "洞天福地"
			};
			Singleton<BattleManager>.Instance.BuildDungeon(mapModel, BattleType.OpenZongMenBattle, MapLevelType.ZONGMEN);
		}

		private void KaiZongLiPai(string mapId, string goodsId)
		{
			Singleton<UIManager>.Instance.PopUpWnd("SheLiZongMenWnd.prefab", true, goodsId);
		}

		private void BaiRuShanMen(string id, string name)
		{
			Singleton<UIManager>.Instance.PopUpWnd("BaiRuShanMenWnd.prefab", true, id, name);
		}

		private void FanHuiZongMen(string id, string name)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_DENTER, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildEnterResp_1015005 guildEnterResp_ = new GuildEnterResp_1015005();
					guildEnterResp_.MergeFrom(byteString);
					if (guildEnterResp_.IsSuccess)
					{
						Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "ZongMenScene", id, name, guildEnterResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildEnterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private T GetWndPanel<T>(MapType _mapType) where T : BaseMapPanel
		{
			switch (_mapType)
			{
			case MapType.DaLu:
				return Panel.DaLuMap as T;
			case MapType.Map_1_9:
			case MapType.Map_1_10:
			case MapType.Map_1_11:
			case MapType.Map_1_12:
				return Panel.Map_1_12 as T;
			case MapType.Map_2_9:
			case MapType.Map_2_10:
			case MapType.Map_2_11:
			case MapType.Map_2_12:
				return Panel.Map_2_12 as T;
			case MapType.Map_3_9:
			case MapType.Map_3_10:
			case MapType.Map_3_11:
			case MapType.Map_3_12:
				return Panel.Map_3_12 as T;
			case MapType.Map_4_9:
			case MapType.Map_4_10:
			case MapType.Map_4_11:
			case MapType.Map_4_12:
				return Panel.Map_4_12 as T;
			case MapType.Map_5_9:
			case MapType.Map_5_10:
			case MapType.Map_5_11:
			case MapType.Map_5_12:
				return Panel.Map_5_12 as T;
			case MapType.MapJun_9:
			case MapType.MapJun_10:
			case MapType.MapJun_11:
			case MapType.MapJun_12:
				return Panel.MapJun_12 as T;
			case MapType.MapWaiWei_1:
				return Panel.MapWaiWei_1 as T;
			case MapType.MapWaiWei_2:
				return Panel.MapWaiWei_2 as T;
			default:
				return null;
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Panel.LiLianDiTuTog.isOn = true;
			m_CurrentMapPanel.gameObject.SetActive(value: false);
		}
	}
}
