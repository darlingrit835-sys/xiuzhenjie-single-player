using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class SheLiZongMenWnd : Window
	{
		public SheLiZongMenPanel Panel;

		private ZongMenLingPaiType m_CurrentChooseLingPai;

		private int m_ZongMenLastIndex;

		private string m_ZongMenLastNameStr;

		private string m_GoodsId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<SheLiZongMenPanel>();
			m_GoodsId = paralist[0].ToString();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.LiZongBtn, LiZongBtnClick);
			AddToggleClickListener(Panel.RiYaoLingPaiTog, RiYaoTog);
			AddToggleClickListener(Panel.YueHuiLingPaiTog, YueHuiTog);
			AddToggleClickListener(Panel.XingGuangLingPaiTog, XingGuangTog);
			AddToggleClickListener(Panel.XuanTieLingPaiTog, XuanTieTog);
			Panel.ZongMenJianChengInput.onValueChanged.AddListener(ZongMenJianChengValueChange);
			Panel.ZongMenLastName.onValueChanged.AddListener(LastNameChanged);
			m_CurrentChooseLingPai = ZongMenLingPaiType.None;
			GetFuDiQing();
		}

		private void GetFuDiQing()
		{
			new List<string>();
			GoodsGuildMapDetailsReq_1004038 data = new GoodsGuildMapDetailsReq_1004038
			{
				Id = m_GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_GUILDMAP_DETAILS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsGuildMapDetailsReap_1004038 goodsGuildMapDetailsReap_ = new GoodsGuildMapDetailsReap_1004038();
					goodsGuildMapDetailsReap_.MergeFrom(byteString);
					if (goodsGuildMapDetailsReap_.IsSuccess)
					{
						string text = goodsGuildMapDetailsReap_.DaLuName + "-" + goodsGuildMapDetailsReap_.ZhouName + "-" + goodsGuildMapDetailsReap_.YeWaiName;
						Panel.ZongMenDiDianText.text = text;
						Panel.LingFengShuLiangText.text = goodsGuildMapDetailsReap_.HillCount + "座";
						Panel.LingShiKuangMai.text = goodsGuildMapDetailsReap_.StoneCount + "座";
						Panel.LingYuKuangMai.text = goodsGuildMapDetailsReap_.JadeCount + "座";
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZongMenJianChengValueChange(string content)
		{
			Panel.RiYaoText.text = content;
			Panel.YueHuiText.text = content;
			Panel.XingGuangText.text = content;
			Panel.XuanTieText.text = content;
		}

		private void LastNameChanged(int index)
		{
			Sprite sprite = null;
			switch (index)
			{
			case 0:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Pai.png");
				m_ZongMenLastNameStr = "派";
				break;
			case 1:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Men.png");
				m_ZongMenLastNameStr = "门";
				break;
			case 2:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Zong.png");
				m_ZongMenLastNameStr = "宗";
				break;
			case 3:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Bang.png");
				m_ZongMenLastNameStr = "帮";
				break;
			case 4:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Jiao.png");
				m_ZongMenLastNameStr = "教";
				break;
			case 5:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Gong.png");
				m_ZongMenLastNameStr = "宫";
				break;
			default:
				sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/ZongMen/ZongMenFont_Pai.png");
				break;
			}
			m_ZongMenLastIndex = index;
			if (!(sprite == null))
			{
				Panel.RiYaoLastNameImg.sprite = sprite;
				Panel.YueHuiLastNameImg.sprite = sprite;
				Panel.XingGuangLastNameImg.sprite = sprite;
				Panel.XuanTieLastNameImg.sprite = sprite;
			}
		}

		private void RiYaoTog(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseLingPai = ZongMenLingPaiType.RiYao;
			}
		}

		private void YueHuiTog(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseLingPai = ZongMenLingPaiType.YueHui;
			}
		}

		private void XingGuangTog(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseLingPai = ZongMenLingPaiType.XingGuang;
			}
		}

		private void XuanTieTog(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseLingPai = ZongMenLingPaiType.XuanTie;
			}
		}

		private void LiZongBtnClick()
		{
			if (string.IsNullOrEmpty(Panel.ZongMenMingChenInput.text))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入宗门名称!");
				return;
			}
			if (string.IsNullOrEmpty(Panel.ZongMenJianChengInput.text))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入宗门简称!");
				return;
			}
			if (m_CurrentChooseLingPai == ZongMenLingPaiType.None)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请先选择令牌!");
				return;
			}
			GuildCreateReq_1015007 data = new GuildCreateReq_1015007
			{
				LastNameIndex = m_ZongMenLastIndex,
				Name = Panel.ZongMenMingChenInput.text,
				SimpleName = Panel.ZongMenJianChengInput.text,
				TokenIndex = (int)m_CurrentChooseLingPai
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_CREATE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildCreateResp_1015007 guildCreateResp_ = new GuildCreateResp_1015007();
					guildCreateResp_.MergeFrom(byteString);
					if (guildCreateResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = guildCreateResp_.ShinyStone;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = guildCreateResp_.ShinyJade;
						Singleton<RoleManager>.Instance.Role_Entity.ZongMenId = guildCreateResp_.GuildId.ToString();
						Singleton<RoleManager>.Instance.Role_Entity.ZongMenName = Panel.ZongMenMingChenInput.text;
						Singleton<RoleManager>.Instance.Role_Entity.ZongMenSimpleName = Panel.ZongMenJianChengInput.text;
						Singleton<RoleManager>.Instance.Role_Entity.ZongMenIconIndex = (int)m_CurrentChooseLingPai;
						Singleton<RoleManager>.Instance.Role_Entity.ZongMenLastNameImgIndex = m_ZongMenLastIndex;
						Singleton<ChatManager>.Instance.JoinZongMenChatServer(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
						EnterZongMen();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildCreateResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterZongMen()
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
						Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "ZongMenScene", Singleton<RoleManager>.Instance.Role_Entity.ZongMenId, Singleton<RoleManager>.Instance.Role_Entity.ZongMenName, guildEnterResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildEnterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Panel.ZongMenJianChengInput.onValueChanged.RemoveAllListeners();
			Panel.ZongMenLastName.onValueChanged.RemoveAllListeners();
		}
	}
}
