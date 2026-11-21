using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ZongMen_DaDianXieRenWnd : Window
	{
		private ZongMen_DaDianXieRenPanel m_Panel;

		private GameObject m_BigItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_DaDianXieRenPanel>();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			m_BigItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMen_ZhangLaoItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			HuoQuXingXi();
		}

		private void HuoQuXingXi()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_ROLE_LIST, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (byteString != null && GameObject != null)
				{
					GuildRoleListResp_1015008 guildRoleListResp_ = new GuildRoleListResp_1015008();
					guildRoleListResp_.MergeFrom(byteString);
					if (guildRoleListResp_.IsSuccess)
					{
						Assignment(guildRoleListResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildRoleListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void Assignment(GuildRoleListResp_1015008 xingXi)
		{
			ShanChu();
			if (m_BigItem == null)
			{
				m_BigItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMen_ZhangLaoItem.prefab");
			}
			string ownId = Singleton<RoleManager>.Instance.Role_Entity.Id;
			int candidates = 0;
			foreach (GuildRoleListResp_1015008.Types.GuildRoleInfoResp_1015009 role in xingXi.RoleList)
			{
				if (string.IsNullOrEmpty(role.RoleId) || role.RoleId == ownId || role.ZhiWei < 0 || role.ZhiWei >= (int)ZhangLaoRenMing.宗主)
				{
					continue;
				}
				var row = SetListContentParent(m_Panel.ReNamePanel, m_BigItem).GetComponent<ZongMen_ZhangLaoItem>();
				string office = role.ZhiWei == 0 ? "普通成员" : ((ZhangLaoRenMing)role.ZhiWei).ToString();
				row.SetValues(role.RoleId, role.Sex, role.RoleImage, role.IsOnLine, role.RoleName,
					role.ZhanDouLi.ToString(), office, role.Level,
					(id, _) => MonoSingleton<GameStart>.Instance.PopUpAttention($"确认由{role.RoleName}接任宗主？", () => RenMing(id)));
				candidates++;
			}
			float y = SetVerticalListContentHight(candidates, 90f, 0f, 3f);
			RectTransform component = m_Panel.ReNamePanel.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		}

		private void RenMing(string roleId)
		{
			XieRenReq_1015022 data = new XieRenReq_1015022
			{
				RoleId = roleId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XIEREN, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					XieRenResp_1015022 xieRenResp_ = new XieRenResp_1015022();
					xieRenResp_.MergeFrom(byteString);
					if (xieRenResp_.IsSuccess)
					{
						RefreshGuildAfterHandover();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(xieRenResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void RefreshGuildAfterHandover()
		{
			var ui = Singleton<UIManager>.Instance;
			ui.CloseWnd(this);
			if (Singleton<GameMapManager>.Instance.CurrentMapName != "ZongMenScene")
			{
				return;
			}
			// 大殿、主峰和宗门首页均在打开时缓存宗主权限。
			ui.CloseWnd(Constants.ZONGMEN_DADIAN_WINDOW);
			ui.CloseWnd(Constants.ZONGMEN_ZHUFENG_WINDOW);
			ui.CloseWnd(Constants.ZongMen_Window);
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_DENTER, null, obj =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (Singleton<GameMapManager>.Instance.CurrentMapName != "ZongMenScene")
				{
					return;
				}
				if (obj is ByteString bytes)
				{
					var guild = GuildEnterResp_1015005.Parser.ParseFrom(bytes);
					if (guild.IsSuccess)
					{
						var role = Singleton<RoleManager>.Instance.Role_Entity;
						ui.PopUpWnd(Constants.ZongMen_Window, true, role.ZongMenId, role.ZongMenName, guild);
						return;
					}
				}
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			}, isShowWaitUI: true);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void ShanChu()
		{
			foreach (RectTransform item in m_Panel.ReNamePanel)
			{
				Object.Destroy(item.gameObject);
			}
		}
	}
}
