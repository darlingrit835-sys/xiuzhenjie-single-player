using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZongMenWnd : Window
	{
		public ZongMenPanel Panel;

		private bool m_IsGuildMaster;

		private int m_ZongMenStage = 3;

		private string m_ZongMenId;

		private string m_ZongMenName;

		private IntoDaDianResp_1015020 m_IntoDaDianResp1015020;

		private GuildEnterResp_1015005 m_DataSource;

		private Dictionary<ZongMenShanFengDuty, ZongMenShanFengItem> ZongMenShanFengDic;

		private string m_ZhuFenName;

		private long m_ZhuFenShenJiTime;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseZongMenWindow);
			HuoQuZhangLaoXingXi();
			AddButtonClickListener(Panel.ShouChaoBtn, EnterShouChao);
			AddButtonClickListener(Panel.HelpBtn, HelpBtnClick);
			ZongMenShanFengDic = new Dictionary<ZongMenShanFengDuty, ZongMenShanFengItem>();
			m_ZongMenId = paralist[0].ToString();
			m_ZongMenName = paralist[1].ToString();
			m_DataSource = paralist[2] as GuildEnterResp_1015005;
			if (m_DataSource != null)
			{
				m_ZhuFenShenJiTime = m_DataSource.UpgradeGuildShengYuTime;
			}
			Panel.ZongMenNameTitleText.text = m_ZongMenName;
			if (m_DataSource != null)
			{
				m_IsGuildMaster = m_DataSource.IsGuildMaster;
			}
			if (Panel.ShanFengList != null)
			{
				for (int i = 0; i < Panel.ShanFengList.Count; i++)
				{
					ZongMenShanFengDic.Add((ZongMenShanFengDuty)(i + 1), Panel.ShanFengList[i]);
				}
			}
			GetShanFengMsg();
		}

		private void HelpBtnClick()
		{
			string text = "1、创建宗门后，宗主拥有调配资源的权利，其余各峰长老仅有调节本峰资源的权利\n2、初始创建的宗门，所有山峰均为无名峰，可于各山峰处点击山峰名字更换峰碑，需消耗大量资源\n3、宗门战力为不包括长老战力的各峰弟子战力总和，用以应对兽潮战，而排行榜处战力包含了长老战力\n4、宗门防护为护宗大阵主导，宗门弟子防护为辅\n5、每个山峰左下角角标显示了该山峰的职能状态\n6、每日宗门所处洞天福地均会遭遇兽潮侵袭，时间为每日晚8点，击败兽潮可以获得奖励，被击败则损失宗门物品";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "宗门");
		}

		private void GetShanFengMsg()
		{
			m_ZhuFenName = m_DataSource.GuildHills[9].HillName.ToString();
			Panel.ZhanLiText.text = m_DataSource.Fighting.ToString("0");
			Panel.FangHuText.text = m_DataSource.Defend.ToString("0");
			Panel.ShouChaoText.text = m_DataSource.BeastTideTime / 60 + "分钟";
			m_ZongMenStage = m_DataSource.GuildHills[9].Stage;
			Panel.ZhuFengStageText.text = WorldSetting.GetStageName(m_ZongMenStage);
			for (int i = 0; i < 8; i++)
			{
				Panel.ShanFengList[i].SetValue(m_DataSource.GuildHills[i].Type, m_DataSource.GuildHills[i].Stage, m_DataSource.GuildHills[i].HillName, EnterShanFeng);
			}
		}

		private void HuoQuZhangLaoXingXi()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.INTODADIAN, null, (object obj) =>
			{
				m_IntoDaDianResp1015020 = new IntoDaDianResp_1015020();
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoDaDianResp_1015020 intoDaDianResp_ = new IntoDaDianResp_1015020();
					intoDaDianResp_.MergeFrom(byteString);
					if (intoDaDianResp_.IsSuccess)
					{
						m_IntoDaDianResp1015020 = intoDaDianResp_;
						AddButtonClickListener(Panel.ZhuFengBtn, ZhuFeng);
						AddButtonClickListener(Panel.KuangMaiBtn, KuangMaiClick);
						AddButtonClickListener(Panel.HuZongDaZhenBtn, HuZongDaZhenClick);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoDaDianResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZhuFeng()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMenZhuFengWnd.prefab", true, m_IntoDaDianResp1015020, m_ZhuFenName, m_ZhuFenShenJiTime, m_IsGuildMaster);
		}

		private void EnterShanFeng(int index, string hillName)
		{
			switch ((ZongMenShanFengDuty)index)
			{
			case ZongMenShanFengDuty.灵田:
				EnterLingTian(hillName);
				break;
			case ZongMenShanFengDuty.冶炼:
				EnterYeLian(hillName);
				break;
			case ZongMenShanFengDuty.工坊:
				EnterGongFang(hillName);
				break;
			case ZongMenShanFengDuty.内门:
				EnterNeiMen(hillName);
				break;
			case ZongMenShanFengDuty.炼丹:
				EnterLianDan(hillName);
				break;
			case ZongMenShanFengDuty.炼器:
				EnterLianQi(hillName);
				break;
			case ZongMenShanFengDuty.制符:
				EnterZhiFu(hillName);
				break;
			case ZongMenShanFengDuty.阵法:
				EnterZhenFa(hillName);
				break;
			case ZongMenShanFengDuty.御兽:
				EnterYuShou(hillName);
				break;
			case ZongMenShanFengDuty.主峰:
				m_ZhuFenName = hillName;
				break;
			}
		}

		private void EnterLingTian(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_INTOLINGTIAN, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoLingTianResp_1015057 intoLingTianResp_ = new IntoLingTianResp_1015057();
					intoLingTianResp_.MergeFrom(byteString);
					if (intoLingTianResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_LingTianWnd.prefab", true, hillName, intoLingTianResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoLingTianResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterYeLian(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoYeLian, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoYeLianResp_1015067 intoYeLianResp_ = new IntoYeLianResp_1015067();
					intoYeLianResp_.MergeFrom(byteString);
					if (intoYeLianResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_YeLianWnd.prefab", true, hillName, intoYeLianResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoYeLianResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterGongFang(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoGongFang, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoGongFangResp_1015103 intoGongFangResp_ = new IntoGongFangResp_1015103();
					intoGongFangResp_.MergeFrom(byteString);
					if (intoGongFangResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_GongFangWnd.prefab", true, hillName, intoGongFangResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoGongFangResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterNeiMen(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoNeiMen, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoNeiMenResp_1015130 intoNeiMenResp_ = new IntoNeiMenResp_1015130();
					intoNeiMenResp_.MergeFrom(byteString);
					if (intoNeiMenResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_NeiMenWnd.prefab", true, hillName, intoNeiMenResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoNeiMenResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterLianDan(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoElixir, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoElixirResp_1015076 intoElixirResp_ = new IntoElixirResp_1015076();
					intoElixirResp_.MergeFrom(byteString);
					if (intoElixirResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_LianDanWnd.prefab", true, hillName, intoElixirResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterLianQi(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoEquipment, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoEquipmentResp_1015085 intoEquipmentResp_ = new IntoEquipmentResp_1015085();
					intoEquipmentResp_.MergeFrom(byteString);
					if (intoEquipmentResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_LianQiWnd.prefab", true, hillName, intoEquipmentResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterZhiFu(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoSign, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoSignResp_1015112 intoSignResp_ = new IntoSignResp_1015112();
					intoSignResp_.MergeFrom(byteString);
					if (intoSignResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_ZhiFuWnd.prefab", true, hillName, intoSignResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoSignResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterZhenFa(string hillName)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoMatrix, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoMatrixResp_1015094 intoMatrixResp_ = new IntoMatrixResp_1015094();
					intoMatrixResp_.MergeFrom(byteString);
					if (intoMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_ZhenFaWnd.prefab", true, hillName, intoMatrixResp_, m_IsGuildMaster);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void EnterYuShou(string hillName)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("暂未开放");
		}

		private void CloseZongMenWindow()
		{
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
		}

		private void EnterShouChao()
		{
			if (m_DataSource.SectWar <= 0)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ZongMenShouChaoWnd.prefab", true, m_ZongMenStage);
			}
		}

		private void KuangMaiClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.IntoKuangMai, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoKuangMaiResp_1015042 intoKuangMaiResp_ = new IntoKuangMaiResp_1015042();
					intoKuangMaiResp_.MergeFrom(byteString);
					if (intoKuangMaiResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_KuangMaiWnd.prefab", true, m_IntoDaDianResp1015020.Stage, m_IsGuildMaster, intoKuangMaiResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoKuangMaiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void HuZongDaZhenClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.IntoHuZongMatrix, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoHuZongMatrixResp_1015048 intoHuZongMatrixResp_ = new IntoHuZongMatrixResp_1015048();
					intoHuZongMatrixResp_.MergeFrom(byteString);
					if (intoHuZongMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_HuZongDaZhenWnd.prefab", true, m_IntoDaDianResp1015020, m_IsGuildMaster, intoHuZongMatrixResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoHuZongMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
