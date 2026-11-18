using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ZongMen_DaDianXieRenWnd : Window
	{
		private ZongMen_DaDianXieRenPanel m_Panel;

		private GameObject m_BigItem;

		private string roleId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_DaDianXieRenPanel>();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			m_BigItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZongMen_ZhangLaoItem.prefab");
		}

		private void HuoQuXingXi()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_ROLE_LIST, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
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
			foreach (GuildRoleListResp_1015008.Types.GuildRoleInfoResp_1015009 role in xingXi.RoleList)
			{
				SetListContentParent(m_Panel.ReNamePanel, m_BigItem).GetComponent<ZongMen_ZhangLaoItem>().SetValues(role.RoleId, role.Sex, role.RoleImage, role.IsOnLine, role.RoleId, role.RoleName, role.ToString(), role.Level, TuiChuZongMenClick);
			}
			float y = SetVerticalListContentHight(xingXi.RoleList.Count, 90f, 0f, 3f);
			RectTransform component = m_Panel.ReNamePanel.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		}

		private void TuiChuZongMenClick(string roleIds, GameObject gameObject)
		{
			roleId = roleIds;
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确认任命该长老为宗主?", RenMing);
		}

		private void RenMing()
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
						HuoQuXingXi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(xieRenResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
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
