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

		private readonly HashSet<CSZ_MapPanel> m_TemporaryPanels = new HashSet<CSZ_MapPanel>();

		private CSZ_MapPanel m_CurrentMapPanel;

		private List<string> m_ParentIds = new List<string>();

		private ChuanSongType m_ChuanSongType;

		private GameObject m_MapZongMenItemInsObj;

		private int m_NavigationRevision;

		private WorldAtlasView atlas;

		private ExpeditionAtlas expedition;
		private int guildQuery;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WorldMapPanel>();
			if (!Panel.TryGetComponent(out atlas))
			{
				InkInterfaceArt.StyleMap(Panel);
				atlas = Panel.gameObject.AddComponent<WorldAtlasView>();
				atlas.Build(Panel);
			}
			StyleTravelTab(Panel.LiLianDiTuTog);
			StyleTravelTab(Panel.ZongPaiShanMenTog);
			AddButtonClickListener(Panel.BackBtn, BackPage);
			AddButtonClickListener(Panel.XiaoWenHao, XiaoWenHaoXQ);
			AddToggleClickListener(Panel.ZongPaiShanMenTog, ZongPaiShanMenTogClick);
			m_MapZongMenItemInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/MapZongMenItem.prefab");
		}

		private static void StyleTravelTab(Toggle tab)
		{
			if (tab.graphic is Image selection)
				selection.color = new Color32(208, 232, 217, 255);
			if (tab.TryGetComponent(out ToggleItemTextChangeColor label))
			{
				label.IsOnTrueColor = InkInterfaceArt.Ink;
				label.IsOnFalseColor = InkInterfaceArt.SoftInk;
				label.ContentText.color = tab.isOn ? label.IsOnTrueColor : label.IsOnFalseColor;
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist == null || paralist.Length < 2 || !(paralist[0] is MapEnterType enterType))
			{
				return;
			}
			ResetNavigation();
			m_EnterFrom = enterType;
			if (enterType == MapEnterType.WaiChuLiLiang && expedition == null)
				expedition = Panel.GetComponentInChildren<ExpeditionAtlas>(true) ?? ExpeditionAtlas.Build(Panel);
			m_DataSource = paralist[1] as List<MapEntity> ?? new List<MapEntity>();
			m_ParentIds = paralist.Length > 2 && paralist[2] is List<string> parentIds
				? new List<string>(parentIds) : new List<string>();
			m_ChuanSongType = paralist.Length > 3 && paralist[3] is ChuanSongType teleportType ? teleportType : default;
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
			if (m_EnterFrom == MapEnterType.WaiChuLiLiang && !Panel.LiLianDiTuTog.isOn)
			{
				Panel.LiLianDiTuTog.isOn = true;
				return;
			}
			if (CanReturnToParent())
			{
				CSZ_MapPanel leaving = m_CurrentMapPanel;
				leaving.ClearValue();
				if (m_TemporaryPanels.Remove(leaving))
				{
					Object.Destroy(leaving.gameObject);
				}
				m_CurrentMapPanel = m_PanelStack.Pop();
				m_CurrentMapPanel.gameObject.SetActive(true);
				if (m_EnterFrom == MapEnterType.CreateRole)
				{
					ClearBirthplaceChoice();
					InkBirthplaceMap.Show(m_CurrentMapPanel, m_CurrentMapPanel.PopulatedCount);
				}
				else if (m_EnterFrom == MapEnterType.ChuanSong)
				{
					TravelLandmarks.Show(m_CurrentMapPanel, true);
				}
				UpdateNavigationTitle();
				return;
			}

			if (m_EnterFrom == MapEnterType.CreateRole)
			{
				ClearBirthplaceChoice();
				UpdateNavigationTitle();
			}
			else if (m_EnterFrom == MapEnterType.WaiChuLiLiang)
			{
				Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.LiLianBtnCloseClick();
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd("WorldMapWnd.prefab", destory: false);
			}
		}

		private void XiaoWenHaoXQ()
		{
			string text = m_EnterFrom == MapEnterType.CreateRole
				? "点击大陆、洲、郡逐层展开舆图。选中可作为出生地的城池后，点下方按钮继续。"
				: m_EnterFrom == MapEnterType.WaiChuLiLiang
					? "点地标可查看所在地、开放条件和迁行消耗。在林地可选择区域历练；切到宗派山门可查访这片林地的驻地。"
					: "点击舆图逐层查看大陆、洲、郡和城池。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "地图");
		}

		private void WaiChuLiLianInit()
		{
			MapType mapType = m_DataSource.Count switch
			{
				9 => MapType.MapJun_9,
				10 => MapType.MapJun_10,
				11 => MapType.MapJun_11,
				_ => MapType.MapJun_12
			};
			SwitchChildWnd(null, mapType, m_DataSource);
		}

		private void ChuanSongInit()
		{
			if (m_ChuanSongType == ChuanSongType.XiongCheng)
			{
				SwitchChildWnd(null, MapType.DaLu, m_DataSource);
				return;
			}
			if (m_ParentIds.Count < 2 || m_DataSource.Count < 9)
			{
				WaiChuLiLianInit();
				return;
			}
			int revision = m_NavigationRevision;
			string mapId = m_ParentIds[m_ParentIds.Count - 2];
			MapInfoReq_1007009 data = new MapInfoReq_1007009
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_INFO, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (revision != m_NavigationRevision || GameObject == null)
				{
					return;
				}
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
						SwitchChildWnd(null, mapType == MapType.None ? MapType.MapJun_12 : mapType, m_DataSource);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateRoleInit()
		{
			SwitchChildWnd(null, MapType.DaLu, m_DataSource);
		}

		public void SwitchChildWnd(CSZ_MapPanel FromPanel, MapType _mapType, List<MapEntity> mapDataSource, string parentName = null)
		{
			if (FromPanel != null && FromPanel != m_CurrentMapPanel)
			{
				return;
			}
			CSZ_MapPanel next = GetWndPanel<CSZ_MapPanel>(_mapType);
			if (next == null)
			{
				return;
			}
			if (next == m_CurrentMapPanel || m_PanelStack.Contains(next))
			{
				// Province and county depths share the same city template. Keep the previous instance intact for Back.
				next = Object.Instantiate(next, next.transform.parent, false);
				m_TemporaryPanels.Add(next);
			}
			string roleMap = Singleton<RoleManager>.Instance.Role_Entity?.MapId;
			if (!string.IsNullOrEmpty(roleMap) && !m_ParentIds.Contains(roleMap))
			{
				m_ParentIds.Add(roleMap);
			}
			if (m_CurrentMapPanel != null)
			{
				m_CurrentMapPanel.ClearValue();
				m_PanelStack.Push(m_CurrentMapPanel);
			}
			if (m_EnterFrom == MapEnterType.CreateRole)
			{
				ClearBirthplaceChoice();
			}
			m_CurrentMapPanel = next;
			next.SetValue(_mapType, mapDataSource, m_ParentIds, m_EnterFrom);
			if (!string.IsNullOrEmpty(parentName)) next.TitleName = parentName;
			UpdateNavigationTitle();
		}

		private void ClearBirthplaceChoice()
		{
			Singleton<UIManager>.Instance.FindWndByName<CreateRoleWnd>("CreateRoleWnd.prefab")
				?.MapChooseCompelete(null, isCanChoose: false);
		}

		private bool CanReturnToParent()
		{
			return m_PanelStack.Count > 0 &&
				(m_EnterFrom != MapEnterType.ChuanSong || m_ChuanSongType == ChuanSongType.XiongCheng);
		}

		private void UpdateNavigationTitle()
		{
			string title = m_CurrentMapPanel?.TitleName;
			Panel.TitleText.text = string.IsNullOrEmpty(title) ? "舆图" : title;
			atlas.ShowContext(m_EnterFrom, m_CurrentMapPanel);
			if (m_EnterFrom == MapEnterType.WaiChuLiLiang && m_CurrentMapPanel != null)
				expedition.Present(m_CurrentMapPanel, Panel.ZongPaiShanMenTog.isOn);
			bool choosingBirthplace = m_EnterFrom == MapEnterType.CreateRole;
			bool showBack = !choosingBirthplace || m_PanelStack.Count > 0;
			Panel.BackBtn.gameObject.SetActive(showBack);
			Transform backPlate = Panel.transform.Find("Title_image/BackImg");
			if (backPlate != null && Panel.BackBtn.transform.IsChildOf(backPlate))
			{
				backPlate.gameObject.SetActive(showBack);
			}
			Text label = Panel.BackBtn.GetComponentInChildren<Text>(true);
			if (label != null)
			{
				label.text = CanReturnToParent() ? "返回" : "离开";
				label.color = InkInterfaceArt.Ink;
			}
		}

		private void ResetNavigation()
		{
			m_NavigationRevision++;
			guildQuery++;
			if (expedition != null)
			{
				expedition.gameObject.SetActive(false);
				Panel.ZongPaiShanMenTog.SetIsOnWithoutNotify(false);
				Panel.LiLianDiTuTog.SetIsOnWithoutNotify(true);
			}
			if (Panel.TiShiPanel.gameObject.activeSelf)
				Panel.TiShiPanel.ClosePanel();
			if (m_CurrentMapPanel != null)
			{
				m_CurrentMapPanel.ClearValue();
				m_CurrentMapPanel = null;
			}
			m_PanelStack.Clear();
			foreach (CSZ_MapPanel temporary in m_TemporaryPanels)
			{
				if (temporary != null)
				{
					Object.Destroy(temporary.gameObject);
				}
			}
			m_TemporaryPanels.Clear();
		}

		public void ZongPaiShanMenTogClick(bool isOn)
		{
			if (expedition != null && m_EnterFrom == MapEnterType.WaiChuLiLiang && m_CurrentMapPanel != null)
				UpdateNavigationTitle();
		}

		public void PopChuanSongCityPanel(int amount, string targetMapId, string cityName)
		{
			RoleInfoEntity role = Singleton<RoleManager>.Instance.Role_Entity;
			int discountPercent = role.VipLevel >= 9 ? 50 : role.VipLevel >= 5 ? 80 : 100;
			int price = (int)((long)amount * discountPercent / 100);
			Panel.TiShiPanel.SetValue(cityName, price, role.ShinyStone - price, () =>
			{
				if (role.ShinyStone >= price)
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
			int level = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
			int xiaoHaoJingLi = ExpeditionPlace.TravelVigor(level);
			float num = ExpeditionPlace.TravelYears(level);
			MonoSingleton<GameStart>.Instance.PopUpAttention($"前往{cityName}将消耗 {xiaoHaoJingLi} 点精力，增加 {num:0.##} 年年龄。确定出发吗？", () =>
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
									MonoSingleton<GameStart>.Instance.PopRollingTextShow($"已抵达{cityName}");
								}
								else
								{
									MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(childrenMapResp_.Code));
								}
							}
						});
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(mapTransmitResp_1007010.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void GetZongMenData(string mapId, string heXinMapId)
		{
			int request = ++guildQuery;
			var origin = m_CurrentMapPanel;
			expedition.GuildStatus("正在查访当地山门…");
			foreach (Transform item in Panel.MapWaiWei_1_Extend_Content)
			{
				item.gameObject.SetActive(false);
				Object.Destroy(item.gameObject);
			}
			GuildListReq_1015001 data = new GuildListReq_1015001
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_LIST, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (request != guildQuery || m_CurrentMapPanel != origin || expedition == null) return;
				ByteString byteString = _obj as ByteString;
				if (byteString == null)
				{
					expedition.GuildStatus("查访未获回音", () => GetZongMenData(mapId, heXinMapId));
					return;
				}
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
					expedition.GuildStatus(num == 0 ? "此地暂无山门驻扎" : $"当地驻地  ·  {num} 处", count: num);
					Panel.MapWaiWei_1_Extend_Content.anchoredPosition = Vector2.zero;
					float y = m_MapZongMenItemInsObj.gameObject.GetComponent<RectTransform>().sizeDelta.y;
					VerticalLayoutGroup component = Panel.MapWaiWei_1_Extend_Content.gameObject.GetComponent<VerticalLayoutGroup>();
					float y2 = SetVerticalListContentHight(num, y, component);
					Panel.MapWaiWei_1_Extend_Content.sizeDelta = new Vector2(Panel.MapWaiWei_1_Extend_Content.sizeDelta.x, y2);
				}
				else
				{
					expedition.GuildStatus("当地山门名册读取失败", () => GetZongMenData(mapId, heXinMapId));
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
			GameObject gameObject = Panel.MapWaiWei_1_Extend_Content.transform.Find("SelfZongMenItem")?.gameObject;
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

		public override void ReFlashData()
		{
			if (m_EnterFrom == MapEnterType.WaiChuLiLiang && m_CurrentMapPanel != null && expedition != null && expedition.isActiveAndEnabled)
				UpdateNavigationTitle();
		}

		public override void OnDisable()
		{
			base.OnDisable();
			ResetNavigation();
		}
	}
}
