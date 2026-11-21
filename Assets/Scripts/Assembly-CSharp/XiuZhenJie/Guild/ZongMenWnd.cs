using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenWnd : Window
	{
		public ZongMenPanel Panel;

		private bool m_IsGuildMaster;

		private SectCourt court;

		private int m_ZongMenStage = 3;

		private string m_ZongMenId;

		private string m_ZongMenName;

		private IntoDaDianResp_1015020 m_IntoDaDianResp1015020;

		private GuildEnterResp_1015005 m_DataSource;

		private Dictionary<ZongMenShanFengDuty, ZongMenShanFengItem> ZongMenShanFengDic;

		private string m_ZhuFenName;

		private Text m_ZhuFengLabel;

		private long m_ZhuFenShenJiTime;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenPanel>();
			StyleMountainMap();
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
			SectCourt.AddGateButton(Panel.transform, Panel.ZongMenNameTitleText.font, () => court.Show(SectCourt.Page.Gate));
			court = SectCourt.Open(Panel.transform, Panel.ZongMenNameTitleText.font, m_ZongMenName,
				m_ZongMenStage, SectCourt.Page.Gate, CloseZongMenWindow, () => court.OpenPeakMap(Panel),
				() =>
				{
					if (m_IntoDaDianResp1015020 != null && m_IntoDaDianResp1015020.IsSuccess) ZhuFeng();
					else MonoSingleton<GameStart>.Instance.PopUpAttention("暂未读到主峰议事记录，请重新进入宗门。");
				}, ZongMenZhuFengWnd.EnterCangBaoGe);
		}

		internal bool HasSectCourt => court != null && court.Available;

		private void StyleMountainMap()
		{
			// The prefab has an opaque black root, a grey backdrop, and a second opaque map panel.
			InkInterfaceArt.Set(Panel.GetComponent<Image>(), "GuildHall", false);
			Panel.transform.Find("Bg").gameObject.SetActive(false);
			var map = (RectTransform)Panel.transform.Find("ShanFengContent");
			map.GetComponent<Image>().enabled = false;
			InkDialogArt.Inset(map, 0, 0, 0, 0);
			Panel.ZongMenNameTitleText.color = InkInterfaceArt.Ink;
			InkDialogArt.StyleButton(Panel.LiKaiBtn, "ParchmentHeading", InkInterfaceArt.Ink);
			Place((RectTransform)Panel.HelpBtn.transform, Vector2.one, Vector2.one, -45, -38, -5, -4);
			InkInterfaceArt.Set(Panel.HelpBtn.image, "Help");

			StyleMetric(Panel.ZhanLiText, "战力", 0);
			StyleMetric(Panel.FangHuText, "防护", 1);
			Place((RectTransform)Panel.ShouChaoBtn.transform, new Vector2(2f / 3, 1), Vector2.one,
				3, -100, -12, -53);
			InkDialogArt.StyleButton(Panel.ShouChaoBtn, "JadeButton", InkDialogArt.Ivory);
			StyleTwoLines(Panel.ShouChaoBtn.transform.Find("Text").GetComponent<Text>(), Panel.ShouChaoText,
				"兽潮", InkDialogArt.Ivory, InkDialogArt.Ivory);

			var main = Panel.ZhuFengBtn;
			var mainRect = (RectTransform)main.transform;
			mainRect.SetParent(map, false);
			// This hit area covers the painted halls on the upper right, as well as the plaque.
			Place(mainRect, new Vector2(.4f, .53f), new Vector2(.96f, .79f), 0, 0, 0, 0);
			var hit = main.GetComponent<Image>();
			hit.sprite = null;
			hit.color = Color.clear;
			hit.raycastTarget = true;
			main.targetGraphic = hit;
			main.transform.Find("Bg").gameObject.SetActive(false);
			main.transform.Find("BtnImg").gameObject.SetActive(false);
			var sign = (RectTransform)main.transform.Find("Icon");
			Place(sign, new Vector2(.5f, .25f), new Vector2(.5f, .25f), -55, -21, 55, 21);
			InkInterfaceArt.Set(sign.GetComponent<Image>(), "JadeButton", false);
			sign.GetComponent<Image>().raycastTarget = false;
			var level = Panel.ZhuFengStageText;
			level.rectTransform.SetParent(sign, false);
			main.transform.Find("Image").gameObject.SetActive(false);
			m_ZhuFengLabel = sign.Find("Text").GetComponent<Text>();
			StyleTwoLines(m_ZhuFengLabel, level, "宗门主峰", InkDialogArt.Ivory, InkDialogArt.Ivory);

			for (int i = 0; i < Panel.ShanFengList.Count; i++)
			{
				var peak = (RectTransform)Panel.ShanFengList[i].transform;
				peak.SetParent(map, false);
				int column = i % 3;
				float bottom = 65 + (2 - i / 3) * 56;
				Place(peak, new Vector2(column / 3f, 0), new Vector2((column + 1) / 3f, 0),
					column == 0 ? 12 : 3, bottom, column == 2 ? -12 : -3, bottom + 48);
			}
			StyleService(Panel.KuangMaiBtn, 0, "矿脉");
			StyleService(Panel.HuZongDaZhenBtn, 1, "护宗大阵");
		}

		private void StyleMetric(Text value, string caption, int index)
		{
			var rect = (RectTransform)value.transform.parent;
			Place(rect, new Vector2(index / 3f, 1), new Vector2((index + 1) / 3f, 1),
				index == 0 ? 12 : 3, -100, -3, -53);
			var surface = rect.GetComponent<Image>();
			InkInterfaceArt.Set(surface, "ParchmentHeading", false);
			surface.raycastTarget = false;
			var label = rect.GetChild(0).GetComponent<Text>();
			StyleTwoLines(label, value, caption, InkInterfaceArt.Ink, InkInterfaceArt.SoftInk);
		}

		private static void StyleService(Button button, int column, string caption)
		{
			var rect = (RectTransform)button.transform;
			Place(rect, new Vector2(column * .5f, 0), new Vector2((column + 1) * .5f, 0),
				column == 0 ? 12 : 3, 12, column == 1 ? -12 : -3, 57);
			button.targetGraphic = button.GetComponent<Image>();
			InkDialogArt.StyleButton(button, "JadeButton", InkDialogArt.Ivory);
			button.transform.Find("Icon").gameObject.SetActive(false);
			button.transform.Find("BtnImg").gameObject.SetActive(false);
			var label = button.transform.Find("Text").GetComponent<Text>();
			InkDialogArt.Inset(label.rectTransform, 3, 3, 0, 0);
			StyleLabel(label, caption, 16, InkDialogArt.Ivory);
		}

		internal static void StyleTwoLines(Text heading, Text detail, string caption, Color foreground, Color secondary)
		{
			Place(heading.rectTransform, new Vector2(0, .4f), Vector2.one, 3, 0, -3, -2);
			Place(detail.rectTransform, Vector2.zero, new Vector2(1, .43f), 3, 1, -3, 0);
			StyleLabel(heading, caption, 16, foreground);
			StyleLabel(detail, detail.text, 12, secondary);
		}

		private static void StyleLabel(Text label, string caption, int size, Color color)
		{
			label.text = caption;
			label.fontSize = size;
			label.resizeTextForBestFit = true;
			label.resizeTextMaxSize = size;
			label.resizeTextMinSize = Mathf.Min(size, 11);
			label.alignment = TextAnchor.MiddleCenter;
			label.color = color;
			label.raycastTarget = false;
		}

		private static void Place(RectTransform rect, Vector2 min, Vector2 max, float left, float bottom, float right, float top)
		{
			rect.localScale = Vector3.one;
			rect.localRotation = Quaternion.identity;
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = new Vector2(left, bottom);
			rect.offsetMax = new Vector2(right, top);
		}

		private void HelpBtnClick()
		{
			if (HasSectCourt)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("宗务总览可查门人职务、驻域及派工。各峰名牌可进入原有设施。兽潮每周日和月末 20:00（东八区）统一结算，重合时只开大潮。");
				return;
			}
			string text = "1、创建宗门后，宗主拥有调配资源的权利，其余各峰长老仅有调节本峰资源的权利\n2、初始创建的宗门，所有山峰均为无名峰，可于各山峰处点击山峰名字更换峰碑，需消耗大量资源\n3、宗门战力为不包括长老战力的各峰弟子战力总和，用以应对兽潮战，而排行榜处战力包含了长老战力\n4、宗门防护为护宗大阵主导，宗门弟子防护为辅\n5、点击各峰名牌可查看山峰职能和管理事务\n6、每日宗门所处洞天福地均会遭遇兽潮侵袭，时间为每日晚8点，击败兽潮可以获得奖励，被击败则损失宗门物品";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "宗门");
		}

		private void GetShanFengMsg()
		{
			Panel.ZhanLiText.text = m_DataSource.Fighting.ToString("0");
			Panel.FangHuText.text = m_DataSource.Defend.ToString("0");
			Panel.ShouChaoText.text = m_DataSource.BeastTideTime / 60 + "分钟";
			foreach (var entry in ZongMenShanFengDic.Values)
				entry.gameObject.SetActive(false);
			foreach (var hill in m_DataSource.GuildHills)
			{
				var duty = (ZongMenShanFengDuty)hill.Type;
				if (duty == ZongMenShanFengDuty.主峰)
				{
					m_ZhuFenName = hill.HillName;
					m_ZongMenStage = hill.Stage;
					bool named = !string.IsNullOrEmpty(hill.HillName) && hill.HillName != "主峰" && hill.HillName != "无名峰";
					m_ZhuFengLabel.text = named ? hill.HillName : "宗门主峰";
					Panel.ZhuFengStageText.text = (named ? "主峰 · " : "") + WorldSetting.GetStageName(hill.Stage);
				}
				else if (ZongMenShanFengDic.TryGetValue(duty, out var peak))
					peak.SetValue(hill.Type, hill.Stage, hill.HillName, EnterShanFeng);
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
			if (HasSectCourt) { court.Show(SectCourt.Page.Wave); return; }
			if (m_DataSource.SectWar <= 0)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ZongMenShouChaoWnd.prefab", true, m_ZongMenStage);
			}
		}

		private void KuangMaiClick()
		{
			if (HasSectCourt) { court.Show(SectCourt.Page.Mine); return; }
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
			if (HasSectCourt) { court.Show(SectCourt.Page.Formation); return; }
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
