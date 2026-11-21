using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class CSZ_MapPanel : BaseMapPanel
	{
		public string TitleName;

		public MapType mapType;

		public List<CSZ_CityItemPanel> BtnDataList = new List<CSZ_CityItemPanel>();

		internal int PopulatedCount { get; private set; }

		private MapEnterType enterType;

		private CreateRoleWnd createRoleWnd;

		private int navigationRevision;

		private void Awake()
		{
			BtnDataList.Clear();
			foreach (Transform item in base.transform)
			{
				CSZ_CityItemPanel component = item.gameObject.GetComponent<CSZ_CityItemPanel>();
				if (!(component == null))
				{
					BtnDataList.Add(component);
				}
			}
		}

		public override void SetValue(MapType _mapType, List<MapEntity> _list, List<string> _parentIds, MapEnterType _enterType)
		{
			navigationRevision++;
			base.gameObject.SetActive(value: true);
			mapType = _mapType;
			InkInterfaceArt.Set(GetComponent<UnityEngine.UI.Image>(), _mapType == MapType.DaLu ? "WorldMap" : "RegionMap", false);
			enterType = _enterType;
			if (enterType != MapEnterType.CreateRole)
			{
				InkBirthplaceMap.Hide(this);
			}
			else
			{
				TravelLandmarks.Hide(this);
			}
			createRoleWnd = enterType == MapEnterType.CreateRole
				? Singleton<UIManager>.Instance.FindWndByName<CreateRoleWnd>("CreateRoleWnd.prefab")
				: null;

			List<MapEntity> maps = _list?.Where(map => map != null).ToList() ?? new List<MapEntity>();
			maps = maps.Count <= 5 ? maps.OrderBy(map => map.Sort).ToList()
				: maps.OrderBy(map => map.MinLevel).ThenBy(map => map.MaxLevel).ThenBy(map => map.Id).ToList();
			PopulatedCount = Mathf.Min(maps.Count, BtnDataList.Count);
			TitleName = PopulatedCount > 0 ? maps[0].ParentName : string.Empty;
			var parentIds = _parentIds ?? new List<string>();
			for (int i = 0; i < BtnDataList.Count; i++)
			{
				CSZ_CityItemPanel slot = BtnDataList[i];
				bool populated = i < PopulatedCount;
				slot.gameObject.SetActive(populated);
				slot.ClearData();
				if (!populated)
				{
					continue;
				}
				MapEntity map = maps[i];
				bool isHere = enterType != MapEnterType.CreateRole && parentIds.Contains(map.Id);
				if (enterType == MapEnterType.ChuanSong)
				{
					slot.SetValue(i + 1, map, clicked => ChuanSongClickCallBack(clicked, parentIds), null, isHere, enterType);
				}
				else
				{
					slot.SetValue(i + 1, map, ClickCallBack, ShaoDangClick, isHere, enterType);
				}
			}
			if (enterType == MapEnterType.CreateRole)
			{
				InkBirthplaceMap.Show(this, PopulatedCount);
			}
			else if (enterType == MapEnterType.ChuanSong)
			{
				TravelLandmarks.Show(this);
			}
		}

		private void OnDisable()
		{
			navigationRevision++;
		}

		private bool IsNavigationCurrent(int revision)
		{
			return this != null && isActiveAndEnabled && navigationRevision == revision;
		}

		private void ClickCallBack(CSZ_CityItemPanel clickItem)
		{
			MapEntity mapModel = clickItem.dataSource;
			if (mapModel == null)
			{
				return;
			}
			int revision = ++navigationRevision;
			if (mapModel.Depth == 1)
			{
				if (enterType == MapEnterType.CreateRole && clickItem.dataSource.MinLevel > 1)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请选择最低等级是1的大陆");
					return;
				}
				ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
				{
					MapId = mapModel.Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _o) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					if (!IsNavigationCurrent(revision))
					{
						return;
					}
					ByteString byteString = _o as ByteString;
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
							InitDaLu(childrenMapResp_.Object.Count, clickItem, list);
						}
					}
				}, isShowWaitUI: true);
			}
			else if (mapModel.Depth == 2)
			{
				if (enterType == MapEnterType.CreateRole && clickItem.dataSource.MinLevel > 1)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请选择最低等级是1的洲");
					return;
				}
				ChildrenMapReq_1007003 data2 = new ChildrenMapReq_1007003
				{
					MapId = mapModel.Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data2, (object _o) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					if (!IsNavigationCurrent(revision))
					{
						return;
					}
					ByteString byteString = _o as ByteString;
					if (!(byteString == null))
					{
						ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
						childrenMapResp_.MergeFrom(byteString);
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
							InitZhou(childrenMapResp_.Object.Count, clickItem, list);
						}
					}
				}, isShowWaitUI: true);
			}
			else if (mapModel.Depth == 3)
			{
				if (enterType == MapEnterType.CreateRole && clickItem.dataSource.MinLevel > 1)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请选择最低等级是1的郡");
					return;
				}
				ChildrenMapReq_1007003 data3 = new ChildrenMapReq_1007003
				{
					MapId = mapModel.Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data3, (object _o) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					if (!IsNavigationCurrent(revision))
					{
						return;
					}
					ByteString byteString = _o as ByteString;
					if (!(byteString == null))
					{
						ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
						childrenMapResp_.MergeFrom(byteString);
						if (childrenMapResp_.IsSuccess)
						{
							List<MapEntity> list = new List<MapEntity>();
							foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item4 in childrenMapResp_.Object)
							{
								MapEntity item = new MapEntity
								{
									Depth = item4.Level,
									FiveElement = item4.FiveElements,
									Id = item4.Id,
									MapName = item4.Name,
									MapType = item4.Type,
									MaxLevel = item4.MaxLevel,
									MinLevel = item4.MinLevel,
									ParentName = item4.ParentName
								};
								list.Add(item);
							}
							InitJun(childrenMapResp_.Object.Count, clickItem, list);
						}
					}
				}, isShowWaitUI: true);
			}
			else if (mapModel.Depth == 4)
			{
				if (mapModel.MapType == 5 || mapModel.MapType == 51 || mapModel.MapType == 52 || mapModel.MapType == 53 || mapModel.MapType == 54)
				{
					if (enterType == MapEnterType.CreateRole)
					{
						bool canChoose = mapModel.MapType == (int)MapLevelType.CITY_LOW && mapModel.MinLevel <= 1;
						SelectBirthplace(canChoose ? clickItem : null);
						if (!canChoose)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("请选择最低等级为1的城市做为出生地!");
						}
					}
					else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < mapModel.MinLevel)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("境界等级不足,无法传送!");
					}
					else
					{
						Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab").PopWaiChuLiLianMsg(mapModel.Id, mapModel.MapName);
					}
				}
				else
				{
					if (mapModel.MapType != 4)
					{
						return;
					}
					if (enterType == MapEnterType.ChuanSong)
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("请选择城市");
						return;
					}
					if (enterType == MapEnterType.WaiChuLiLiang)
					{
						WorldMapWnd mapWnd = Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab");
						LiLianChildrenMapReq_1007013 data4 = new LiLianChildrenMapReq_1007013
						{
							MapId = mapModel.Id
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.LILIAN_CHILDREN_MAP, data4, (object _o) =>
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							if (!IsNavigationCurrent(revision) || mapWnd == null)
							{
								return;
							}
							ByteString byteString = _o as ByteString;
							if (!(byteString == null))
							{
								LiLianChildrenMapResp_1007013 liLianChildrenMapResp_ = new LiLianChildrenMapResp_1007013();
								liLianChildrenMapResp_.MergeFrom(byteString);
								if (liLianChildrenMapResp_.IsSuccess)
								{
									List<MapEntity> list = new List<MapEntity>();
									foreach (LiLianChildrenMapResp_1007013.Types.LiLianMapObjectResp_1007014 item5 in liLianChildrenMapResp_.Object)
									{
										MapEntity item = new MapEntity
										{
											Depth = item5.Level,
											FiveElement = item5.FiveElements,
											Id = item5.Id,
											MapName = item5.Name,
											MapType = item5.Type,
											MaxLevel = item5.MaxLevel,
											MinLevel = item5.MinLevel,
											ParentName = item5.ParentName,
											IsCleanMap = item5.IsCleanMap
										};
										list.Add(item);
									}
									MapType mapType = list.Count == 3 ? MapType.MapWaiWei_1 : MapType.MapWaiWei_2;
									mapWnd.SwitchChildWnd(this, mapType, list, mapModel.MapName);
									mapWnd.GetZongMenData(mapModel.Id, list.Count > 0 ? list[list.Count - 1].Id : string.Empty);
								}
							}
						}, isShowWaitUI: true);
						return;
					}
					SelectBirthplace(null);
				}
			}
			else if (mapModel.Depth == 5)
			{
				MaoXian(mapModel);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("无效的地图!");
			}
		}

		private void SelectBirthplace(CSZ_CityItemPanel selected)
		{
			for (int i = 0; i < PopulatedCount; i++)
			{
				CSZ_CityItemPanel slot = BtnDataList[i];
				slot.ChoosedItem(slot == selected);
			}
			createRoleWnd?.MapChooseCompelete(selected?.dataSource, selected != null);
		}

		private void InitDaLu(int count, CSZ_CityItemPanel clickItem, List<MapEntity> list)
		{
			WorldMapWnd worldMapWnd = Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab");
			MapType mapType = MapType.None;
			switch (clickItem.index)
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
			worldMapWnd?.SwitchChildWnd(this, mapType, list);
		}

		private void InitZhou(int count, CSZ_CityItemPanel clickItem, List<MapEntity> list)
		{
			WorldMapWnd worldMapWnd = Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab");
			MapType mapType = MapType.None;
			mapType = MapType.MapJun_12;
			worldMapWnd?.SwitchChildWnd(this, mapType, list);
		}

		private void InitJun(int count, CSZ_CityItemPanel clickItem, List<MapEntity> list)
		{
			WorldMapWnd worldMapWnd = Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab");
			MapType mapType = list.Count switch
			{
				9 => MapType.MapJun_9,
				10 => MapType.MapJun_10,
				11 => MapType.MapJun_11,
				_ => MapType.MapJun_12
			};
			worldMapWnd?.SwitchChildWnd(this, mapType, list);
		}

		private void ChuanSongClickCallBack(CSZ_CityItemPanel clickItem, List<string> _parentIds)
		{
			int revision = ++navigationRevision;
			MapEntity dataSource = clickItem.dataSource;
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < dataSource.MinLevel)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("境界等级不足,无法传送!");
			}
			else if (dataSource.Depth == 1)
			{
				ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
				{
					MapId = dataSource.Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _o) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					if (!IsNavigationCurrent(revision)) return;
					ByteString cs = _o as ByteString;
					if (!(cs == null))
					{
						ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
						childrenMapResp_.MergeFrom(cs);
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
							if (!_parentIds.Contains(clickItem.dataSource.Id))
							{
								foreach (MapEntity item4 in list)
								{
									if (item4.MapType == 22)
									{
										ChildrenMapReq_1007003 data3 = new ChildrenMapReq_1007003
										{
											MapId = item4.Id
										};
										Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data3, (object _map) =>
										{
											MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
											if (!IsNavigationCurrent(revision)) return;
											ByteString data4 = _map as ByteString;
											if (data4 != null)
											{
												ChildrenMapResp_1007003 childrenMapResp_2 = new ChildrenMapResp_1007003();
												childrenMapResp_2.MergeFrom(data4);
												if (childrenMapResp_2.IsSuccess)
												{
													List<MapEntity> list2 = new List<MapEntity>();
													foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item5 in childrenMapResp_2.Object)
													{
														MapEntity item2 = new MapEntity
														{
															Depth = item5.Level,
															FiveElement = item5.FiveElements,
															Id = item5.Id,
															MapName = item5.Name,
															MapType = item5.Type,
															MaxLevel = item5.MaxLevel,
															MinLevel = item5.MinLevel,
															ParentName = item5.ParentName
														};
														list2.Add(item2);
													}
													MapEntity mapEntity = null;
													foreach (MapEntity item6 in list2)
													{
														if (item6.MapType == 54)
														{
															mapEntity = item6;
															break;
														}
													}
													if (mapEntity == null)
													{
														MonoSingleton<GameStart>.Instance.PopUpAttention("无法传送!没有找到雄城!");
														Debug.Log("没有找到雄城!");
													}
													else
													{
														Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab").PopChuanSongCityPanel(5000000, mapEntity.Id, mapEntity.MapName);
													}
												}
											}
										}, isShowWaitUI: true);
										break;
									}
								}
								return;
							}
							InitDaLu(list.Count, clickItem, list);
						}
					}
				}, isShowWaitUI: true);
			}
			else if (dataSource.Depth == 2)
			{
				ChildrenMapReq_1007003 data2 = new ChildrenMapReq_1007003
				{
					MapId = dataSource.Id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data2, (object _o) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					if (!IsNavigationCurrent(revision)) return;
					ByteString byteString = _o as ByteString;
					if (!(byteString == null))
					{
						ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
						childrenMapResp_.MergeFrom(byteString);
						if (childrenMapResp_.IsSuccess)
						{
							List<MapEntity> list = new List<MapEntity>();
							foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item7 in childrenMapResp_.Object)
							{
								MapEntity item = new MapEntity
								{
									Depth = item7.Level,
									FiveElement = item7.FiveElements,
									Id = item7.Id,
									MapName = item7.Name,
									MapType = item7.Type,
									MaxLevel = item7.MaxLevel,
									MinLevel = item7.MinLevel,
									ParentName = item7.ParentName
								};
								list.Add(item);
							}
							MapEntity mapEntity = null;
							foreach (MapEntity item8 in list)
							{
								if (item8.MapType == 53 || item8.MapType == 54)
								{
									mapEntity = item8;
									break;
								}
							}
							if (mapEntity == null)
							{
								MonoSingleton<GameStart>.Instance.PopUpAttention("无法传送!");
								Debug.Log("没有找到州城!");
							}
							else
							{
								Singleton<UIManager>.Instance.FindWndByName<WorldMapWnd>("WorldMapWnd.prefab").PopChuanSongCityPanel(50000, mapEntity.Id, mapEntity.MapName);
							}
						}
					}
				}, isShowWaitUI: true);
			}
			else if (dataSource.Depth != 3)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("无效的地图!");
			}
		}

		private void ShaoDangClick(CSZ_CityItemPanel clickItem)
		{
			Singleton<UIManager>.Instance.PopUpWnd("ShaoDangWnd.prefab", true, clickItem.dataSource);
		}

		private void MaoXian(MapEntity mapModel)
		{
			Singleton<BattleManager>.Instance.BuildDungeon(mapModel, BattleType.MapBattle, MapLevelType.MASSIFAREA);
		}

		public void ClearData()
		{
			PopulatedCount = 0;
			TitleName = string.Empty;
			for (int i = 0; i < BtnDataList.Count; i++)
			{
				BtnDataList[i].ClearData();
				BtnDataList[i].gameObject.SetActive(false);
			}
		}

		public override void ClearValue()
		{
			base.ClearValue();
			base.gameObject.SetActive(value: false);
		}
	}
}
