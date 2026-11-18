using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMenChengYuanWnd : Window
	{
		public ZongMenChengYuanPanel Panel;

		private List<ZongMenRoleEntity> AllRoleList;

		private List<ZongMenRoleEntity> AllMingCiList;

		private Dictionary<string, ZongMenChengYuanItem> ZongMenChengYuanItemDic;

		private Dictionary<string, ZongMenMingCiItem> ZongMenMingCiItemDic;

		private GameObject ChengYuanInsObj;

		private GameObject MingCiInsObj;

		private bool m_IsZongZhu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenChengYuanPanel>();
			AllRoleList = new List<ZongMenRoleEntity>();
			AllMingCiList = new List<ZongMenRoleEntity>();
			ZongMenChengYuanItemDic = new Dictionary<string, ZongMenChengYuanItem>();
			ZongMenMingCiItemDic = new Dictionary<string, ZongMenMingCiItem>();
			AddButtonClickListener(Panel.BackBtn, CloseWindow);
			AddToggleClickListener(Panel.RuMenShenQingTog, RuMenShenQingPanelShow);
			ChengYuanInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMenChengYuanItem.prefab");
			MingCiInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMenMingCiItem.prefab");
			ScrollViewInfiniteVertical menRenVerticalContent = Panel.MenRenVerticalContent;
			menRenVerticalContent.DownScrollAction = (UnityAction<GameObject>)Delegate.Combine(menRenVerticalContent.DownScrollAction, new UnityAction<GameObject>(ZongMenMenRenDownScroll));
			ScrollViewInfiniteVertical menRenVerticalContent2 = Panel.MenRenVerticalContent;
			menRenVerticalContent2.UpScrollAction = (UnityAction<GameObject>)Delegate.Combine(menRenVerticalContent2.UpScrollAction, new UnityAction<GameObject>(ZongMenMenRenUpScroll));
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_IsZongZhu = (bool)paralist[0];
			GetGuildRoleListData();
			GetGuildJoinListData();
		}

		private void GetGuildRoleListData()
		{
			foreach (Transform item2 in Panel.MenRenScrollContent)
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
			Panel.MenRenVerticalContent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_ROLE_LIST, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildRoleListResp_1015008 guildRoleListResp_ = new GuildRoleListResp_1015008();
					guildRoleListResp_.MergeFrom(byteString);
					if (guildRoleListResp_.IsSuccess)
					{
						AllRoleList.Clear();
						ZongMenChengYuanItemDic.Clear();
						foreach (GuildRoleListResp_1015008.Types.GuildRoleInfoResp_1015009 role in guildRoleListResp_.RoleList)
						{
							ZongMenRoleEntity item = new ZongMenRoleEntity
							{
								IsOnline = role.IsOnLine,
								RoleId = role.RoleId,
								RoleImageId = int.Parse(role.RoleImage),
								RoleLastBackTime = role.ZhanDouLi,
								RoleLevel = role.Level,
								RoleProfession1 = role.Profession1,
								RoleProfession2 = role.Profession2,
								RoleSex = (role.Sex ? 1 : 0),
								RoleName = role.RoleName
							};
							AllRoleList.Add(item);
						}
						AllRoleList.Sort((ZongMenRoleEntity x, ZongMenRoleEntity y) => -x.IsOnline.CompareTo(y.IsOnline));
						CreateGuildRoleData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildRoleListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetGuildJoinListData()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN_LIST, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildJoinListResp_1015010 guildJoinListResp_ = new GuildJoinListResp_1015010();
					guildJoinListResp_.MergeFrom(byteString);
					if (guildJoinListResp_.IsSuccess)
					{
						AllMingCiList.Clear();
						ZongMenMingCiItemDic.Clear();
						foreach (GuildJoinListResp_1015010.Types.GuildRoleInfoResp_1015011 join in guildJoinListResp_.JoinList)
						{
							ZongMenRoleEntity item = new ZongMenRoleEntity
							{
								IsOnline = true,
								RoleAge = join.RoleAge,
								RoleId = join.RoleId,
								RoleImageId = int.Parse(join.RoleImage),
								RoleLevel = join.Level,
								RoleName = join.RoleName,
								RoleProfession1 = join.Profession1,
								RoleProfession2 = join.Profession2,
								RoleSex = (join.Sex ? 1 : 0)
							};
							AllMingCiList.Add(item);
						}
						GetMingCiRoleData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildJoinListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateGuildRoleData()
		{
			for (int i = 0; i < AllRoleList.Count; i++)
			{
				if (i < 7)
				{
					CreateDaoYouItem(i, AllRoleList[i]);
				}
			}
			float y = SetVerticalListContentHight(AllRoleList.Count, 75f, 0f, 6f);
			RectTransform component = Panel.MenRenScrollContent.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			Panel.MenRenVerticalContent.SetCompelete(AllRoleList.Count);
		}

		private void GetMingCiRoleData()
		{
			for (int i = 0; i < AllMingCiList.Count; i++)
			{
				if (i < 7)
				{
					CreateMingCiItem(i, AllMingCiList[i]);
				}
			}
			float y = SetVerticalListContentHight(AllMingCiList.Count, 75f, 0f, 6f);
			RectTransform component = Panel.MingCiScrollContent.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			Panel.MenRenVerticalContent.SetCompelete(AllMingCiList.Count);
		}

		private void CreateDaoYouItem(int index, ZongMenRoleEntity item)
		{
			ZongMenChengYuanItem component = SetListContentParent(Panel.MenRenScrollContent, ChengYuanInsObj).GetComponent<ZongMenChengYuanItem>();
			SetItemValue(index, component, item);
			if (!ZongMenChengYuanItemDic.ContainsKey(item.RoleId))
			{
				ZongMenChengYuanItemDic.Add(item.RoleId, component);
			}
		}

		private void CreateMingCiItem(int index, ZongMenRoleEntity item)
		{
			ZongMenMingCiItem component = SetListContentParent(Panel.MingCiScrollContent, MingCiInsObj).GetComponent<ZongMenMingCiItem>();
			SetMingCiValue(index, component, item);
			if (!ZongMenMingCiItemDic.ContainsKey(item.RoleId))
			{
				ZongMenMingCiItemDic.Add(item.RoleId, component);
			}
		}

		private void SetItemValue(int index, ZongMenChengYuanItem zongMenChengYuanItem, ZongMenRoleEntity item)
		{
			RoleFuZhiType roleFuZhiType = RoleFuZhiType.None;
			RoleFuZhiType roleFuZhiType2 = RoleFuZhiType.None;
			roleFuZhiType = (RoleFuZhiType)item.RoleProfession1;
			roleFuZhiType2 = (RoleFuZhiType)item.RoleProfession2;
			zongMenChengYuanItem.SetValue(index, m_IsZongZhu, item.RoleId, item.IsOnline, item.RoleSex == 1, item.RoleImageId.ToString(), item.RoleName, item.RoleLevel, item.RoleLastBackTime, roleFuZhiType, roleFuZhiType2, ZhuChuZongMen);
		}

		private void SetMingCiValue(int index, ZongMenMingCiItem zongMenMingCiItem, ZongMenRoleEntity item)
		{
			RoleFuZhiType roleFuZhiType = RoleFuZhiType.None;
			RoleFuZhiType roleFuZhiType2 = RoleFuZhiType.None;
			roleFuZhiType = (RoleFuZhiType)item.RoleProfession1;
			roleFuZhiType2 = (RoleFuZhiType)item.RoleProfession2;
			zongMenMingCiItem.SetValue(index, item.RoleId, item.IsOnline, item.RoleSex == 1, item.RoleImageId.ToString(), item.RoleName, item.RoleLevel, item.RoleAge, roleFuZhiType, roleFuZhiType2, JieShouShenQing, JuJueShenQing);
		}

		private void ZhuChuZongMen(int index, string id, string name, GameObject gameObject)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.Id == id)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("无法逐出宗门！");
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否把" + name + "逐出宗门!", () =>
			{
				ZhuChuZongMenReq(index, id, name, gameObject);
			});
		}

		private void ZhuChuZongMenReq(int index, string id, string name, GameObject gameObject)
		{
			GuildExpelReq_1015013 data = new GuildExpelReq_1015013
			{
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_EXPEL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!byteString.Equals(null))
				{
					GuildExpelResp_1015013 guildExpelResp_ = new GuildExpelResp_1015013();
					guildExpelResp_.MergeFrom(byteString);
					if (guildExpelResp_.IsSuccess)
					{
						DeleteChengYuan(index, id, gameObject);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildExpelResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JieShouShenQing(int index, string id, string name, GameObject gameObject)
		{
			GuildJoinHandleReq_1015012 data = new GuildJoinHandleReq_1015012
			{
				IsAgree = true,
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN_HANDLE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildJoinHandleResp_1015012 guildJoinHandleResp_ = new GuildJoinHandleResp_1015012();
					guildJoinHandleResp_.MergeFrom(byteString);
					if (guildJoinHandleResp_.IsSuccess)
					{
						GetGuildRoleListData();
						DeleteMingCi(index, id, gameObject);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildJoinHandleResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuJueShenQing(int index, string id, string name, GameObject gameObject)
		{
			GuildJoinHandleReq_1015012 data = new GuildJoinHandleReq_1015012
			{
				IsAgree = false,
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN_HANDLE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildJoinHandleResp_1015012 guildJoinHandleResp_ = new GuildJoinHandleResp_1015012();
					guildJoinHandleResp_.MergeFrom(byteString);
					if (guildJoinHandleResp_.IsSuccess)
					{
						DeleteMingCi(index, id, gameObject);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildJoinHandleResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZongMenMenRenDownScroll(GameObject gameObj)
		{
			if (!gameObj.Equals(null))
			{
				ZongMenChengYuanItem component = gameObj.GetComponent<ZongMenChengYuanItem>();
				if (!component.Equals(null))
				{
					int index = component.index;
					int targetIndex = ((index + 7 > AllRoleList.Count - 1) ? (AllRoleList.Count - 1) : (index + 7));
					ScrollRestValue(targetIndex, component);
				}
			}
		}

		private void ZongMenMenRenUpScroll(GameObject gameObj)
		{
			if (!gameObj.Equals(null))
			{
				ZongMenChengYuanItem component = gameObj.GetComponent<ZongMenChengYuanItem>();
				if (!component.Equals(null))
				{
					int index = component.index;
					int targetIndex = ((index - 7 >= 0) ? (index - 7) : 0);
					ScrollRestValue(targetIndex, component);
				}
			}
		}

		private void ScrollRestValue(int targetIndex, ZongMenChengYuanItem zongMenChengYuanItem)
		{
			ZongMenRoleEntity zongMenRoleEntity = AllRoleList[targetIndex];
			string friendRoleId = zongMenChengYuanItem.FriendRoleId;
			SetItemValue(targetIndex, zongMenChengYuanItem, zongMenRoleEntity);
			if (ZongMenChengYuanItemDic.ContainsKey(friendRoleId))
			{
				ZongMenChengYuanItemDic.Remove(friendRoleId);
			}
			if (!ZongMenChengYuanItemDic.ContainsKey(zongMenRoleEntity.RoleId))
			{
				ZongMenChengYuanItemDic.Add(zongMenRoleEntity.RoleId, zongMenChengYuanItem);
			}
		}

		private void RuMenShenQingPanelShow(bool isOn)
		{
			if (isOn)
			{
				Panel.MingCiScrollView.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 12f);
				return;
			}
			Panel.MingCiScrollView.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			Panel.MingCiScrollContent.gameObject.GetComponent<RectTransform>();
		}

		private void DeleteChengYuan(int index, string id, GameObject gameObj)
		{
			List<GameObject> list = new List<GameObject>();
			bool flag = false;
			foreach (Transform item in Panel.MenRenScrollContent)
			{
				if (item.gameObject.Equals(gameObj))
				{
					flag = true;
				}
				if (flag)
				{
					list.Add(item.gameObject);
				}
			}
			if (ZongMenChengYuanItemDic.ContainsKey(id))
			{
				ZongMenChengYuanItemDic.Remove(id);
			}
			if (AllRoleList.Count > index)
			{
				AllRoleList.RemoveAt(index);
			}
			int num = 0;
			if (AllRoleList.Count >= 7)
			{
				for (int i = 0; i < AllRoleList.Count; i++)
				{
					if (i >= index)
					{
						if (num >= list.Count)
						{
							break;
						}
						ZongMenChengYuanItem component = list[num].gameObject.GetComponent<ZongMenChengYuanItem>();
						SetItemValue(i, component, AllRoleList[i]);
						num++;
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(gameObj);
			}
		}

		private void DeleteMingCi(int index, string id, GameObject gameObj)
		{
			List<GameObject> list = new List<GameObject>();
			bool flag = false;
			foreach (Transform item in Panel.MingCiScrollContent)
			{
				if (item.gameObject.Equals(gameObj))
				{
					flag = true;
				}
				if (flag)
				{
					list.Add(item.gameObject);
				}
			}
			if (ZongMenMingCiItemDic.ContainsKey(id))
			{
				ZongMenMingCiItemDic.Remove(id);
			}
			if (AllMingCiList.Count > index)
			{
				AllMingCiList.RemoveAt(index);
			}
			int num = 0;
			if (AllMingCiList.Count >= 7)
			{
				for (int i = 0; i < AllMingCiList.Count; i++)
				{
					if (i >= index)
					{
						if (num >= list.Count)
						{
							break;
						}
						ZongMenChengYuanItem component = list[num].gameObject.GetComponent<ZongMenChengYuanItem>();
						SetItemValue(i, component, AllMingCiList[i]);
						num++;
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(gameObj);
			}
		}
	}
}
