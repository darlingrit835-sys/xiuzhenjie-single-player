using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_DaDianWnd : Window
	{
		private ZongMen_DaDianPanel m_Panel;

		private bool m_ZongZhuBool;

		private IntoDaDianResp_1015020 m_IntoDaDianResp1015020;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_DaDianPanel>();
			if (paralist != null)
			{
				m_IntoDaDianResp1015020 = paralist[0] as IntoDaDianResp_1015020;
				PanDuan();
				JiBenQingKuang();
				CaiWuQingKuang();
				ZhangLaoQingKuang();
				DiZiQingKuang();
			}
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.XieRenButton, XieRen);
			AddButtonClickListener(m_Panel.TuiChuZongMenButton, TuiChuZongMen);
			AddButtonClickListener(m_Panel.JieSanZongMenBtn, JieSanZongMenBtnClick);
			AddButtonClickListener(m_Panel.QueRenJieSanBtn, JieSanZongMenReq);
			AddToggleClickListener(m_Panel.QueRenJieSanTog, GoXuanHo);
			AddButtonClickListener(m_Panel.CloseJieSanPanelBtn, CloseJieSanPanelBtnClick);
			m_Panel.TuiChuZongMenButton.gameObject.SetActive(value: true);
		}

		private void CloseJieSanPanelBtnClick()
		{
			m_Panel.QueRenJieSanTog.isOn = false;
			m_Panel.QueRenJieSanInput.text = string.Empty;
			m_Panel.QueRenJieSanBtn.interactable = false;
			m_Panel.JieSanZongMenPanel.gameObject.SetActive(value: false);
		}

		private void GoXuanHo(bool t)
		{
			if (t)
			{
				m_Panel.QueRenJieSanBtn.interactable = true;
			}
			else
			{
				m_Panel.QueRenJieSanBtn.interactable = false;
			}
		}

		private void JieSanZongMenBtnClick()
		{
			m_Panel.JieSanZongMenPanel.gameObject.SetActive(value: true);
		}

		private void JieSanZongMenReq()
		{
			if (m_Panel.QueRenJieSanInput.text != "解散宗门")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请正确输入“解散宗门”");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildJieSan, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString data = _obj as ByteString;
				GuildJieSanResp_1015158 guildJieSanResp_ = new GuildJieSanResp_1015158();
				guildJieSanResp_.MergeFrom(data);
				if (guildJieSanResp_.IsSuccess)
				{
					LiKaiZongMen();
				}
				else
				{
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildJieSanResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void PanDuan()
		{
			m_ZongZhuBool = Singleton<RoleManager>.Instance.Role_Entity.Id == m_IntoDaDianResp1015020.ZongZhuInfo.RoleId;
			ZhangLaoFuZhi(m_IntoDaDianResp1015020.ZongZhuInfo);
			XieRePanDuan();
		}

		private void ZhangLaoFuZhi(IntoDaDianResp_1015020.Types.ZongZhuInfoResp_1015021 zongZhuInfo)
		{
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(zongZhuInfo.Sex, zongZhuInfo.RoleImage, isOnline: true);
			m_Panel.PortraitImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = zongZhuInfo.RoleName;
			m_Panel.ZhanLiText.text = $"{zongZhuInfo.Fighting}";
			m_Panel.JingJieText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(zongZhuInfo.Level);
		}


		private void JiBenQingKuang()
		{
			m_Panel.ZongMenDengJieTexe.text = WorldSetting.GetStageName(m_IntoDaDianResp1015020.Stage) + "宗门";
			m_Panel.MaxYingXiangLiTexe.text = $"{YingXiangLi(m_IntoDaDianResp1015020.Influence)}({m_IntoDaDianResp1015020.Influence})";
			m_Panel.ZongMenZhanDoLiTexe.text = $"{(int)m_IntoDaDianResp1015020.Fighting}";
			m_Panel.ZongMenFangYuLiTexe.text = $"{(int)m_IntoDaDianResp1015020.FangYu}";
		}

		private static string YingXiangLi(int inFlyece)
		{
			if (inFlyece < 3001)
			{
				return "籍籍无名";
			}
			if (inFlyece < 10001)
			{
				return "略有名气";
			}
			if (inFlyece < 20001)
			{
				return "影响深广";
			}
			if (inFlyece >= 30001)
			{
				return "如日中天";
			}
			return "声名远播";
		}

		private void CaiWuQingKuang()
		{
			m_Panel.StoneKuCun.text = $"{(int)m_IntoDaDianResp1015020.StoneKuCun}";
			m_Panel.StoneChanLiang.text = $"{(int)m_IntoDaDianResp1015020.StoneChanLiang}";
			m_Panel.JadeKuCun.text = $"{(int)m_IntoDaDianResp1015020.JadeKuCun}";
			m_Panel.JadeChanLiang.text = $"{(int)m_IntoDaDianResp1015020.JadeChanLiang}";
			m_Panel.YaoCaiKuCun.text = $"{(int)m_IntoDaDianResp1015020.YaoCaiKuCun}";
			m_Panel.YaoCaiLiang.text = $"{m_IntoDaDianResp1015020.YaoCaiChanLiang}";
			m_Panel.KuangShiKuCun.text = $"{(int)m_IntoDaDianResp1015020.KuangShiKuCun}";
			m_Panel.KuangShiLiang.text = $"{m_IntoDaDianResp1015020.KuangShiChanLiang}";
			m_Panel.YuanCaiKuCun.text = $"{(int)m_IntoDaDianResp1015020.YuanCaiKuCun}";
			m_Panel.YuanCaiChanLiang.text = $"{m_IntoDaDianResp1015020.YuanCaiChanLiang}";
			m_Panel.DanYaoKuCun.text = $"{(int)m_IntoDaDianResp1015020.DanYaoKuCun}";
			m_Panel.DanYaoChanLiang.text = $"{m_IntoDaDianResp1015020.DanYaoChanLiang}";
			m_Panel.FaQiKuCun.text = $"{(int)m_IntoDaDianResp1015020.FaQiKuCun}";
			m_Panel.FaQiChanLiang.text = $"{m_IntoDaDianResp1015020.FaQiChanLiang}";
			m_Panel.FuLuKuCun.text = $"{(int)m_IntoDaDianResp1015020.FuLuKuCun}";
			m_Panel.FuLuChanLiang.text = $"{m_IntoDaDianResp1015020.FuLuChanLiang}";
			m_Panel.ZhenPanKuCun.text = $"{(int)m_IntoDaDianResp1015020.ZhenPanKuCun}";
			m_Panel.ZhenPanChanLiang.text = $"{m_IntoDaDianResp1015020.ZhenPanChanLiang}";
			m_Panel.YaoShouKuCun.text = $"{m_IntoDaDianResp1015020.YaoShouKuCun}";
			m_Panel.YaoShouChanLiang.text = $"{m_IntoDaDianResp1015020.YaoShouChanLiang}";
		}

		private void ZhangLaoQingKuang()
		{
			m_Panel.B_1Text.text = $"{m_IntoDaDianResp1015020.ZhangLaoShuLiang}";
			m_Panel.B_2Text.text = $"{(int)m_IntoDaDianResp1015020.ZhangLaoZhanDouLi}";
			m_Panel.B_3Text.text = $"{(int)m_IntoDaDianResp1015020.ZhangLaoFangYuLi}";
		}

		private void DiZiQingKuang()
		{
			m_Panel.C_1Text.text = $"{m_IntoDaDianResp1015020.DiZiShuLiang}";
			m_Panel.C_4Text.text = $"{m_IntoDaDianResp1015020.ZhiFaDiZiShuLiang}";
			m_Panel.C_5Text.text = $"{m_IntoDaDianResp1015020.ZongMenHunLuanDu}";
			m_Panel.C_6Text.text = $"{(int)m_IntoDaDianResp1015020.MeiXiaoShiSiWang}%";
			m_Panel.C_7Text.text = $"{m_IntoDaDianResp1015020.MeiXiaoShiRuZong}";
			m_Panel.C_8Text.text = $"{m_IntoDaDianResp1015020.NeiMenDiZiShuLiang}";
			m_Panel.C_9Text.text = $"{m_IntoDaDianResp1015020.GongFangDiZiShuLiang}";
			m_Panel.C_10Text.text = $"{m_IntoDaDianResp1015020.LianDanDiZiShuLiang}";
			m_Panel.C_11Text.text = $"{m_IntoDaDianResp1015020.LianQiDiZiShuLiang}";
			m_Panel.C_12Text.text = $"{m_IntoDaDianResp1015020.ZhiFuDiZiShuLiang}";
			m_Panel.SiYangYaoShouDiZiShuLiang.text = $"{m_IntoDaDianResp1015020.GongFangDiZiShuLiang}";
			m_Panel.KaiCaiLingYuDiZiShuLiang.text = $"{m_IntoDaDianResp1015020.KaiCaiLingYuDiZiShuLiang}";
			m_Panel.KaiCaiLingShiDiZiShuLiang.text = $"{m_IntoDaDianResp1015020.KaiCaiLingShiDiZiShuLiang}";
			m_Panel.ZhongZhiDiZiShuLiang.text = $"{m_IntoDaDianResp1015020.ZhongZhiDiZiShuLiang}";
			m_Panel.YeLianDiZiShuLiang.text = $"{m_IntoDaDianResp1015020.YeLianDiZiShuLiang}";
			m_Panel.ZhenPanDiZiShuLiang.text = $"{m_IntoDaDianResp1015020.ZhenPanDiZiShuLiang}";
		}

		private void XieRePanDuan()
		{
			m_Panel.XieRenButton.gameObject.SetActive(m_ZongZhuBool);
			m_Panel.JieSanZongMenBtn.gameObject.SetActive(m_ZongZhuBool);
		}

		private void XieRen()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_DaDianXieRenWnd.prefab", true);
		}

		private void TuiChuZongMen()
		{
			if (m_ZongZhuBool)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("宗主卸任后，才能退出宗门！");
			}
			else
			{
				TuiChuZongMenClick();
			}
		}

		private void TuiChuZongMenClick()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("退出宗门后,需等待三天才可重新加入宗门,是否确认离开" + Singleton<RoleManager>.Instance.Role_Entity.ZongMenName + "?", TuiChuZongMenReq);
		}

		private void TuiChuZongMenReq()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_EXIT, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuildExitResp_1015014 guildExitResp_ = new GuildExitResp_1015014();
					guildExitResp_.MergeFrom(byteString);
					if (guildExitResp_.IsSuccess)
					{
						LiKaiZongMen();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildExitResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiZongMen()
		{
			Singleton<ChatManager>.Instance.LiKaiChatRoom(Singleton<RoleManager>.Instance.Role_Entity.ZongMenId);
			string zongMenName = Singleton<RoleManager>.Instance.Role_Entity.ZongMenName;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenId = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenName = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenSimpleName = string.Empty;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenIconIndex = 0;
			Singleton<RoleManager>.Instance.Role_Entity.ZongMenLastNameImgIndex = 0;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "ZongMenScene")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("您已经脱离了" + zongMenName, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				}, () =>
				{
					Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				});
				return;
			}
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("您已经脱离了" + zongMenName);
			ZongMenChatWnd zongMenChatWnd = Singleton<UIManager>.Instance.FindWndByName<ZongMenChatWnd>("ZongMenChatWnd.prefab");
			if (zongMenChatWnd != null)
			{
				Singleton<UIManager>.Instance.CloseWnd(zongMenChatWnd);
			}
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、宗主可以卸任，将宗主之位交予他人 2、宗主解散宗门后，需谨慎确认，一旦解散将无法挽回 3、可以在宗门大殿查看整个宗门的情况，用以做出决策";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "宗门大殿");
		}
	}
}
