using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ZongMen_ZhangLaoWnd : Window
	{
		private ZongMen_ZhangLaoPanel m_Panel;

		private List<ZongMenRoleEntity> m_AllMingCiList;

		private Dictionary<string, ZongMenMingCiItem> m_ZongMenMingCiItemDic;

		private List<ZongMenZhangLaoRoleEntity> m_AllRoleList;

		private Dictionary<string, ZongMen_ZhangLaoDianItem> m_ZongMenChengYuanItemDic;

		private GameObject m_ChengYuanInsObj;

		private GameObject m_MingCiInsObj;

		private int ZongMenRenShu;

		private int DaiChuliMingCi;

		private bool m_IsZongZhu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_ZhangLaoPanel>();
			m_IsZongZhu = (bool)paralist[1];
			m_Panel.TuiChuButton.interactable = m_IsZongZhu;
			m_AllRoleList = new List<ZongMenZhangLaoRoleEntity>();
			m_AllMingCiList = new List<ZongMenRoleEntity>();
			m_ZongMenMingCiItemDic = new Dictionary<string, ZongMenMingCiItem>();
			m_ZongMenChengYuanItemDic = new Dictionary<string, ZongMen_ZhangLaoDianItem>();
			m_ChengYuanInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMen_ZhangLaoDianItem.prefab");
			m_MingCiInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMenMingCiItem.prefab");
			RuZongShenQiang();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.TuiChuButton, RuZongShenQiang);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			RuZongLiKai();
			GetGuildRoleListData();
		}

		public void GetGuildRoleListData()
		{
			foreach (Transform item2 in m_Panel.ReNamePanel)
			{
				Object.Destroy(item2.gameObject);
			}
			m_Panel.ReNamePanel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_ROLE_LIST, null, (object obj) =>
			{
				m_AllRoleList.Clear();
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuildRoleListResp_1015008 guildRoleListResp_ = new GuildRoleListResp_1015008();
					guildRoleListResp_.MergeFrom(byteString);
					if (guildRoleListResp_.IsSuccess)
					{
						foreach (GuildRoleListResp_1015008.Types.GuildRoleInfoResp_1015009 role in guildRoleListResp_.RoleList)
						{
							ZongMenZhangLaoRoleEntity item = new ZongMenZhangLaoRoleEntity
							{
								IsOnline = role.IsOnLine,
								RoleId = role.RoleId,
								RoleImageId = int.Parse(role.RoleImage),
								RoleLastBackTime = role.ZhanDouLi,
								RoleLevel = role.Level,
								RoleProfession1 = role.ZhiWei,
								RoleSex = (role.Sex ? 1 : 0),
								RoleName = role.RoleName,
								Profession1 = role.Profession1,
								Profession2 = role.Profession2,
								Profession1Stage = role.Profession1Stage,
								Profession2Stage = role.Profession2Stage,
								PlantLiveStage = role.PlantLiveStage
							};
							m_AllRoleList.Add(item);
						}
						if (DaiChuliMingCi > 0)
						{
							m_Panel.TuiChuImage.gameObject.SetActive(value: true);
							m_Panel.TuiChuText.text = $"{DaiChuliMingCi}";
						}
						else
						{
							m_Panel.TuiChuImage.gameObject.SetActive(value: false);
						}
						m_AllRoleList.Sort((ZongMenZhangLaoRoleEntity x, ZongMenZhangLaoRoleEntity y) => -x.IsOnline.CompareTo(y.IsOnline));
						CreateGuildRoleData();
						ZongMenRenShu = m_AllRoleList.Count;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildRoleListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateGuildRoleData()
		{
			int num = 0;
			foreach (ZongMenZhangLaoRoleEntity allRole in m_AllRoleList)
			{
				num++;
				CreateDaoYouItem(num, allRole);
			}
			float y = SetVerticalListContentHight(m_AllRoleList.Count, 85f, 0f, 6f);
			RectTransform component = m_Panel.ReNamePanel.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			m_Panel.MenRenVerticalContent.SetCompelete(m_AllRoleList.Count);
		}

		private void CreateDaoYouItem(int index, ZongMenZhangLaoRoleEntity item)
		{
			ZongMen_ZhangLaoDianItem component = SetListContentParent(m_Panel.ReNamePanel, m_ChengYuanInsObj).GetComponent<ZongMen_ZhangLaoDianItem>();
			SetItemValue(index, component, item);
			if (!m_ZongMenChengYuanItemDic.ContainsKey(item.RoleId))
			{
				m_ZongMenChengYuanItemDic.Add(item.RoleId, component);
			}
		}

		private void SetItemValue(int index, ZongMen_ZhangLaoDianItem zongMenChengYuanItem, ZongMenZhangLaoRoleEntity item)
		{
			zongMenChengYuanItem.SetValue(index, m_IsZongZhu, item.RoleId, item.IsOnline, item.RoleSex == 1, item.RoleImageId.ToString(), item.RoleName, item.RoleLevel, item.RoleLastBackTime, item.RoleProfession1, ZhuChuZongMen, ZhuChuZongMens, item);
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
				ZhuChuZongMenReq(index, id, gameObject);
			});
		}

		private void ZhuChuZongMenReq(int index, string id, GameObject gameObject)
		{
			GuildExpelReq_1015013 data = new GuildExpelReq_1015013
			{
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_EXPEL, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if ((object)byteString == null || !byteString.Equals(null))
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

		private void DeleteChengYuan(int index, string id, GameObject gameObj)
		{
			List<GameObject> list = new List<GameObject>();
			bool flag = false;
			foreach (Transform item in m_Panel.ReNamePanel)
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
			if (m_ZongMenChengYuanItemDic.ContainsKey(id))
			{
				m_ZongMenChengYuanItemDic.Remove(id);
			}
			if (m_AllRoleList.Count > index)
			{
				m_AllRoleList.RemoveAt(index);
			}
			int num = 0;
			if (m_AllRoleList.Count >= 7)
			{
				for (int i = 0; i < m_AllRoleList.Count; i++)
				{
					if (i >= index)
					{
						if (num >= list.Count)
						{
							break;
						}
						ZongMen_ZhangLaoDianItem component = list[num].gameObject.GetComponent<ZongMen_ZhangLaoDianItem>();
						SetItemValue(i, component, m_AllRoleList[i]);
						num++;
					}
				}
			}
			else
			{
				Object.Destroy(gameObj);
			}
		}

		private void ZhuChuZongMens(ZongMenZhangLaoRoleEntity index, string id, string name, GameObject gameObject)
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_ZhangLaoRenMingWnd.prefab", true, id, name, index);
		}

		private void RuZongShenQiang()
		{
			m_Panel.RuZhongImage.gameObject.SetActive(value: true);
			m_Panel.TuiChuButton.gameObject.SetActive(value: false);
			AddButtonClickListener(m_Panel.RuZongLiKaiButton, RuZongLiKai);
			AddButtonClickListener(m_Panel.RuZongWenHaoButton, WenHaoTogClick);
			GetGuildJoinListData();
		}

		private void RuZongLiKai()
		{
			m_Panel.RuZhongImage.gameObject.SetActive(value: false);
			m_Panel.TuiChuButton.gameObject.SetActive(value: true);
		}

		private void XiangQing()
		{
			m_Panel.ShengQingXiangQingText.text = $"宗门共有{ZongMenRenShu}人   未处理名刺{DaiChuliMingCi}名";
		}

		private void GetGuildJoinListData()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN_LIST, null, (object obj) =>
			{
				foreach (Transform item2 in m_Panel.MingCiScrollContent)
				{
					Object.Destroy(item2.gameObject);
				}
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuildJoinListResp_1015010 guildJoinListResp_ = new GuildJoinListResp_1015010();
					guildJoinListResp_.MergeFrom(byteString);
					if (guildJoinListResp_.IsSuccess)
					{
						m_AllMingCiList.Clear();
						m_ZongMenMingCiItemDic.Clear();
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
							m_AllMingCiList.Add(item);
						}
						DaiChuliMingCi = m_AllMingCiList.Count;
						XiangQing();
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

		private void GetMingCiRoleData()
		{
			for (int i = 0; i < m_AllMingCiList.Count; i++)
			{
				CreateMingCiItem(i, m_AllMingCiList[i]);
			}
			float y = SetVerticalListContentHight(m_AllMingCiList.Count, 75f, 0f, 6f);
			RectTransform component = m_Panel.MingCiScrollContent.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			m_Panel.MingCiVerticalContent.SetCompelete(m_AllMingCiList.Count);
		}

		private void CreateMingCiItem(int index, ZongMenRoleEntity item)
		{
			ZongMenMingCiItem component = SetListContentParent(m_Panel.MingCiScrollContent, m_MingCiInsObj).GetComponent<ZongMenMingCiItem>();
			SetMingCiValues(index, component, item);
			if (!m_ZongMenMingCiItemDic.ContainsKey(item.RoleId))
			{
				m_ZongMenMingCiItemDic.Add(item.RoleId, component);
			}
		}

		private void SetMingCiValues(int index, ZongMenMingCiItem zongMenMingCiItem, ZongMenRoleEntity item)
		{
			RoleFuZhiType roleProfession = (RoleFuZhiType)item.RoleProfession1;
			RoleFuZhiType roleProfession2 = (RoleFuZhiType)item.RoleProfession2;
			zongMenMingCiItem.SetValue(index, item.RoleId, item.IsOnline, item.RoleSex == 1, item.RoleImageId.ToString(), item.RoleName, item.RoleLevel, item.RoleAge, roleProfession, roleProfession2, JieShouShenQing, JuJueShenQing);
		}

		private void JuJueShenQing(int index, string id, string name, GameObject gameObject)
		{
			GuildJoinHandleReq_1015012 data = new GuildJoinHandleReq_1015012
			{
				IsAgree = false,
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN_HANDLE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
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

		private void JieShouShenQing(int index, string id, string name, GameObject gameObject)
		{
			GuildJoinHandleReq_1015012 data = new GuildJoinHandleReq_1015012
			{
				IsAgree = true,
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JOIN_HANDLE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuildJoinHandleResp_1015012 guildJoinHandleResp_ = new GuildJoinHandleResp_1015012();
					guildJoinHandleResp_.MergeFrom(byteString);
					if (guildJoinHandleResp_.IsSuccess)
					{
						GetGuildRoleListData();
						DeleteMingCi(index, id, gameObject);
						ZongMenRenShu++;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildJoinHandleResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DeleteMingCi(int index, string id, GameObject gameObj)
		{
			List<GameObject> list = new List<GameObject>();
			bool flag = false;
			foreach (Transform item in m_Panel.MingCiScrollContent)
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
			if (m_ZongMenMingCiItemDic.ContainsKey(id))
			{
				m_ZongMenMingCiItemDic.Remove(id);
			}
			if (m_AllMingCiList.Count > index)
			{
				m_AllMingCiList.RemoveAt(index);
			}
			int num = 0;
			if (m_AllMingCiList.Count >= 7)
			{
				for (int i = 0; i < m_AllMingCiList.Count; i++)
				{
					if (i >= index)
					{
						if (num >= list.Count)
						{
							break;
						}
						ZongMenMingCiItem component = list[num].gameObject.GetComponent<ZongMenMingCiItem>();
						SetMingCiValues(i, component, m_AllMingCiList[i]);
						num++;
					}
				}
			}
			else
			{
				Object.Destroy(gameObj);
			}
			DaiChuliMingCi--;
			XiangQing();
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、每个等阶的宗门可以接受的长老人数有限，三阶宗门最大为15人，4阶18人，5阶21人，6接24人，7阶27人，8阶30人，9阶35人 2、若长老退出宗门则3天内无法再行加入其他宗门 3、若长老被宗主逐出宗门则1天内无法再行加入其他宗门 4、每个职务可以任命一名长老，会根据长老的副职或等阶提升效率";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "长老殿");
		}
	}
}
