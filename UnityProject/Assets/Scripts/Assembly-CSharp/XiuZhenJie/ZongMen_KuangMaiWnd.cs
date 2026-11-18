using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_KuangMaiWnd : Window
	{
		private ZongMen_KuangMaiPanel m_Panel;

		private int m_ChangKuCurrentStage = 3;

		private int m_GongJuCurrentStage = 3;

		private long m_LingShiKuCunShu;

		private long m_MaxLingShiKuCunShu;

		private long m_LingYuKuCunShu;

		private long m_MaxLingYuKuCunShu;

		private int m_KeYongDiZiNum;

		private int m_KuangMaiDengJi;

		private int m_LingShiDanRenShu;

		private int LingYuDanRenShu = 1;

		private bool m_ZhangLaoBool;

		private int m_LingShiKaiCaiRenShu;

		private int m_LingYuKaiCaiRenShu;

		private int m_LingShiKaiCaiRenShuNew;

		private int m_LingYuKaiCaiRenShuNew;

		private long m_ShengJiCangKuTimeShengYuTime;

		private long m_ShengJiGongJuTimeYuTime;

		private float m_Time;

		private Dictionary<int, ZM_CangKuUpdateEntity> m_CangKuUpdateDic;

		private Dictionary<int, ZM_KuangMaiUpdateEntity> m_GongJuUpdateDic;

		private bool m_ZhangLaoPanDuan;

		private IntoKuangMaiResp_1015042 m_IntoKuangMaiResp1015042 = new IntoKuangMaiResp_1015042();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_KuangMaiPanel>();
			m_KuangMaiDengJi = (int)paralist[0];
			m_ZhangLaoPanDuan = (bool)paralist[1];
			m_IntoKuangMaiResp1015042 = paralist[2] as IntoKuangMaiResp_1015042;
			if (m_ZhangLaoPanDuan)
			{
				m_ZhangLaoBool = true;
			}
			m_CangKuUpdateDic = Singleton<FacadeManager>.Instance.SelectCangKuUpdateEntity();
			m_GongJuUpdateDic = Singleton<FacadeManager>.Instance.SelectKuangMaiUpdateEntities();
			JiSuan();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.LingShiKaiCaiRenShuButton, LingShiKaiCaiRenShu);
			AddButtonClickListener(m_Panel.LingYuKaiCaiRenShuButton, LingYuKaiCaiRenShu);
			AddButtonClickListener(m_Panel.ShengJiCangKuButton, ShengJiCangKu);
			AddButtonClickListener(m_Panel.ShengJiGongJuButton, ShengJiGongJu);
			AddButtonClickListener(m_Panel.ShengJiCangKuTimeButton, ShengJiCangKuTime);
			AddButtonClickListener(m_Panel.ShengJiGongJuTimeButton, ShengJiGongJuTime);
			m_Panel.LingShiKaiCaiRenShuInput.onEndEdit.AddListener(LingShiShu);
			m_Panel.LingYuKaiCaiRenShuInput.onEndEdit.AddListener(LingYuShu);
			HuoQuZhangLaoXingXi();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
				ShengJiCangKubut();
				ShengJiGongJubut();
			}
		}

		private void PanDuan()
		{
			m_Panel.ShengJiCangKuButton.gameObject.SetActive(value: true);
			m_Panel.ShengJiGongJuButton.gameObject.SetActive(value: true);
			m_Panel.ShengJiCangKuButton.interactable = m_ZhangLaoBool;
			m_Panel.ShengJiGongJuButton.interactable = m_ZhangLaoBool;
			m_Panel.ShengJiCangKuTimeButton.interactable = m_ZhangLaoBool;
			m_Panel.ShengJiGongJuTimeButton.interactable = m_ZhangLaoBool;
		}

		private void ShengJiCangKubut()
		{
			if (m_ShengJiCangKuTimeShengYuTime > 0 && m_ZhangLaoBool)
			{
				m_ShengJiCangKuTimeShengYuTime--;
				string text = CommonMethod.SecondToHour((int)m_ShengJiCangKuTimeShengYuTime);
				m_Panel.ShengJiCangKuTimeButton.gameObject.SetActive(value: true);
				m_Panel.ShengJiCangKuText.text = text;
			}
			else
			{
				m_Panel.ShengJiCangKuTimeButton.gameObject.SetActive(value: false);
			}
		}

		private void ShengJiGongJubut()
		{
			if (m_ShengJiGongJuTimeYuTime > 0 && m_ZhangLaoBool)
			{
				m_ShengJiGongJuTimeYuTime--;
				string text = CommonMethod.SecondToHour((int)m_ShengJiGongJuTimeYuTime);
				m_Panel.ShengJiGongJuTimeButton.gameObject.SetActive(value: true);
				m_Panel.ShengJiGongJuText.text = text;
			}
			else
			{
				m_Panel.ShengJiGongJuTimeButton.gameObject.SetActive(value: false);
			}
		}

		private void ShengJiCangKuTime()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, (int)m_ShengJiCangKuTimeShengYuTime / 60 + 1, new UnityAction<int>(SpeedUpdate));
		}

		private void SpeedUpdate(int castLingYu)
		{
			UpgradeKuangMaiSpaceJadeBuyTimeReq_1015053 data = new UpgradeKuangMaiSpaceJadeBuyTimeReq_1015053
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeKuangMaiSpaceJadeBuyTime, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeKuangMaiSpaceJadeBuyTimeResp_1015053 upgradeKuangMaiSpaceJadeBuyTimeResp_ = new UpgradeKuangMaiSpaceJadeBuyTimeResp_1015053();
					upgradeKuangMaiSpaceJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeKuangMaiSpaceJadeBuyTimeResp_.IsSuccess)
					{
						m_ShengJiCangKuTimeShengYuTime = (int)(upgradeKuangMaiSpaceJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_ShengJiCangKuTimeShengYuTime <= 0)
						{
							m_ShengJiCangKuTimeShengYuTime = 0L;
							ShengJiCangKubut();
							KuCunXianShi();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeKuangMaiSpaceJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShengJiGongJuTime()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, (int)m_ShengJiGongJuTimeYuTime / 60 + 1, new UnityAction<int>(ShengJiJiShiFangHuSpeedUpdate));
		}

		private void ShengJiJiShiFangHuSpeedUpdate(int castLingYu)
		{
			UpgradeKuangMaiFaQiJadeBuyTimeReq_1015052 data = new UpgradeKuangMaiFaQiJadeBuyTimeReq_1015052
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeKuangMaiFaQiJadeBuyTime, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeKuangMaiFaQiJadeBuyTimeResp_1015052 upgradeKuangMaiFaQiJadeBuyTimeResp_ = new UpgradeKuangMaiFaQiJadeBuyTimeResp_1015052();
					upgradeKuangMaiFaQiJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeKuangMaiFaQiJadeBuyTimeResp_.IsSuccess)
					{
						m_ShengJiGongJuTimeYuTime = (int)(upgradeKuangMaiFaQiJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_ShengJiGongJuTimeYuTime <= 0)
						{
							m_ShengJiGongJuTimeYuTime = 0L;
							ShengJiGongJubut();
							KuCunXianShi();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeKuangMaiFaQiJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JiSuan()
		{
			switch (m_KuangMaiDengJi)
			{
			case 3:
				m_LingShiDanRenShu = 300;
				break;
			case 4:
				m_LingShiDanRenShu = 350;
				break;
			case 5:
				m_LingShiDanRenShu = 400;
				break;
			case 6:
				m_LingShiDanRenShu = 600;
				break;
			case 7:
				m_LingShiDanRenShu = 1000;
				break;
			case 8:
				m_LingShiDanRenShu = 2000;
				break;
			case 9:
				m_LingShiDanRenShu = 3000;
				break;
			}
		}

		private void HuoQuZhangLaoXingXi()
		{
			m_ShengJiCangKuTimeShengYuTime = m_IntoKuangMaiResp1015042.UpgradeSpaceShengYuTime / 1000;
			m_ShengJiGongJuTimeYuTime = m_IntoKuangMaiResp1015042.UpgradeFaQiShengYuTime / 1000;
			if (m_ShengJiCangKuTimeShengYuTime > 0 || m_ShengJiGongJuTimeYuTime > 0)
			{
				m_Time++;
			}
			m_LingShiKuCunShu = m_IntoKuangMaiResp1015042.StoneKuCun;
			m_LingYuKuCunShu = m_IntoKuangMaiResp1015042.JadeKuCun;
			m_KeYongDiZiNum = m_IntoKuangMaiResp1015042.KeYongDiZiNum;
			m_ChangKuCurrentStage = m_IntoKuangMaiResp1015042.SpaceStage;
			m_GongJuCurrentStage = m_IntoKuangMaiResp1015042.FaQiStage;
			m_MaxLingShiKuCunShu = long.Parse(m_CangKuUpdateDic[m_IntoKuangMaiResp1015042.SpaceStage].MaxShinyStone);
			m_MaxLingYuKuCunShu = m_CangKuUpdateDic[m_IntoKuangMaiResp1015042.SpaceStage].MaxShinyJade;
			if (m_IntoKuangMaiResp1015042.ZhangLaoInfo != null)
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.Id == m_IntoKuangMaiResp1015042.ZhangLaoInfo.RoleId)
				{
					m_ZhangLaoBool = true;
				}
				m_Panel.PortraitWu.gameObject.SetActive(value: false);
				ZhangLaoFuZhi(m_IntoKuangMaiResp1015042.ZhangLaoInfo);
			}
			else
			{
				m_Panel.PortraitWu.gameObject.SetActive(value: true);
			}
			PanDuan();
			KuCunXianShi();
			KaiCaiGongJu(m_IntoKuangMaiResp1015042);
			LingShiKuang(m_IntoKuangMaiResp1015042);
			LingYuKuang(m_IntoKuangMaiResp1015042);
			KeYOngDiZi();
		}

		private void ZhangLaoFuZhi(IntoKuangMaiResp_1015042.Types.ZhangLaoInfoResp_1015043 xingXi)
		{
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(xingXi.Sex, xingXi.RoleImage, isOnline: true);
			m_Panel.PortraitImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = xingXi.RoleName;
			m_Panel.ZhanLiText.text = $"{xingXi.Fighting}";
			m_Panel.PromoteText.text = $"增加矿脉产量{xingXi.TeXiaoVal}%";
			m_Panel.JingJieText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(xingXi.Level);
		}


		private void KaiCaiGongJu(IntoKuangMaiResp_1015042 xingXi)
		{
			m_Panel.FaQiDengJiText.text = WorldSetting.GetStageChineseName(xingXi.FaQiStage) + "法器";
			m_Panel.KaiCaiShuDuText.text = $"提升{xingXi.KaiCaiRate}%";
		}

		private void LingShiKuang(IntoKuangMaiResp_1015042 xingXi)
		{
			m_Panel.LingShiKuangLevelText.text = WorldSetting.GetStageName(m_KuangMaiDengJi) + "灵石矿";
			m_LingShiKaiCaiRenShu = xingXi.StoneKaiCaiDiZiNum;
			if (m_ZhangLaoBool)
			{
				m_Panel.LingShiKaiCaiRenShuImage.gameObject.SetActive(value: true);
				m_Panel.LingShiKaiCaiRenShuInput.text = $"{xingXi.StoneKaiCaiDiZiNum}";
			}
			else
			{
				m_Panel.LingShiKaiCaiRenShuImage.gameObject.SetActive(value: false);
				m_Panel.LingShiKaiCaiRenShuText.text = $"{xingXi.StoneKaiCaiDiZiNum}";
			}
			m_Panel.LingShiCanLiangText.text = $"{xingXi.CurrentStoneChanLiang}/小时";
			m_Panel.LingShiMaxCanLiangText.text = $"{m_GongJuUpdateDic[m_GongJuCurrentStage].MaxLingShiMiners}人";
			m_Panel.LingShiChuLaingText.text = $"{xingXi.StoneChuLiang}/小时";
		}

		private void LingYuKuang(IntoKuangMaiResp_1015042 xingXi)
		{
			m_Panel.LingYuKuangLevelText.text = WorldSetting.GetStageName(m_KuangMaiDengJi) + "灵玉矿";
			m_LingYuKaiCaiRenShu = xingXi.JadeKaiCaiDiZiNum;
			if (m_ZhangLaoBool)
			{
				m_Panel.LingYuKaiCaiRenShuImage.gameObject.SetActive(value: true);
				m_Panel.LingYuKaiCaiRenShuInput.text = $"{xingXi.JadeKaiCaiDiZiNum}";
			}
			else
			{
				m_Panel.LingYuKaiCaiRenShuImage.gameObject.SetActive(value: false);
				m_Panel.LingYuKaiCaiRenShuText.text = $"{xingXi.JadeKaiCaiDiZiNum}";
			}
			m_Panel.LingYuCanLiangText.text = $"{xingXi.CurrentJadeChanLiang}/小时";
			m_Panel.LingYuMaxCanLiangText.text = $"{m_GongJuUpdateDic[m_GongJuCurrentStage].MaxLingYuMiners}人";
			m_Panel.LingYuChuLaingText.text = $"{xingXi.JadeChuLiang}/小时";
		}

		private void KeYOngDiZi()
		{
			m_Panel.LingShiText.text = $"可用弟子{m_KeYongDiZiNum}名";
			m_Panel.LingYuText.text = $"可用弟子{m_KeYongDiZiNum}名";
		}

		private void LingShiShu(string count)
		{
			if (!string.IsNullOrEmpty(m_Panel.LingShiKaiCaiRenShuInput.text))
			{
				string text = m_Panel.LingShiKaiCaiRenShuInput.text;
				if (int.Parse(text) > 0 && m_KeYongDiZiNum + m_LingShiKaiCaiRenShu >= int.Parse(text))
				{
					m_LingShiKaiCaiRenShuNew = int.Parse(text);
				}
				else
				{
					m_Panel.LingShiKaiCaiRenShuInput.text = $"{m_LingShiKaiCaiRenShu}";
				}
			}
			else
			{
				m_LingShiKaiCaiRenShuNew = m_LingShiKaiCaiRenShu;
				m_Panel.LingShiKaiCaiRenShuInput.text = $"{m_LingShiKaiCaiRenShuNew}";
			}
			m_Panel.LingShiCanLiangText.text = $"{m_LingShiKaiCaiRenShuNew * m_LingShiDanRenShu}/小时";
		}

		private void LingYuShu(string count)
		{
			if (!string.IsNullOrEmpty(m_Panel.LingYuKaiCaiRenShuInput.text))
			{
				string text = m_Panel.LingYuKaiCaiRenShuInput.text;
				if (int.Parse(text) > 9 && int.Parse(text) % 10 != 0)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("灵玉矿工人数必须为10的倍数");
					m_LingYuKaiCaiRenShuNew = m_LingYuKaiCaiRenShu;
					m_Panel.LingYuKaiCaiRenShuInput.text = $"{m_LingYuKaiCaiRenShuNew}";
					return;
				}
				if (m_KeYongDiZiNum + m_LingYuKaiCaiRenShu >= int.Parse(text))
				{
					m_LingYuKaiCaiRenShuNew = int.Parse(text);
				}
				else
				{
					m_Panel.LingYuKaiCaiRenShuInput.text = $"{m_LingYuKaiCaiRenShu}";
				}
			}
			else
			{
				m_LingYuKaiCaiRenShuNew = m_LingYuKaiCaiRenShu;
				m_Panel.LingYuKaiCaiRenShuInput.text = $"{m_LingYuKaiCaiRenShuNew}";
			}
			m_Panel.LingYuCanLiangText.text = $"{m_LingYuKaiCaiRenShuNew * LingYuDanRenShu / 10}/小时";
		}

		private void LingShiKaiCaiRenShu()
		{
			KuangMaiStoneDiZiNumReq_1015044 kuangMaiStoneDiZiNumReq_ = new KuangMaiStoneDiZiNumReq_1015044
			{
				StoneKaiCaiDiZiNum = m_LingShiKaiCaiRenShuNew
			};
			if (m_LingShiKaiCaiRenShuNew < 1)
			{
				kuangMaiStoneDiZiNumReq_.StoneKaiCaiDiZiNum = -1;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.KuangMaiStoneDiZiNum, kuangMaiStoneDiZiNumReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					KuangMaiStoneDiZiNumResp_1015044 kuangMaiStoneDiZiNumResp_ = new KuangMaiStoneDiZiNumResp_1015044();
					kuangMaiStoneDiZiNumResp_.MergeFrom(byteString);
					if (kuangMaiStoneDiZiNumResp_.IsSuccess)
					{
						m_KeYongDiZiNum = kuangMaiStoneDiZiNumResp_.KeYongDiZiNum;
						m_Panel.LingShiKaiCaiRenShuInput.text = $"{m_LingShiKaiCaiRenShuNew}";
						KeYOngDiZi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(kuangMaiStoneDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LingYuKaiCaiRenShu()
		{
			KuangMaiJadeDiZiNumReq_1015045 kuangMaiJadeDiZiNumReq_ = new KuangMaiJadeDiZiNumReq_1015045
			{
				JadeKaiCaiDiZiNum = m_LingYuKaiCaiRenShuNew
			};
			if (m_LingYuKaiCaiRenShuNew < 1)
			{
				kuangMaiJadeDiZiNumReq_.JadeKaiCaiDiZiNum = -1;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.KuangMaiJadeDiZiNum, kuangMaiJadeDiZiNumReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					KuangMaiJadeDiZiNumResp_1015045 kuangMaiJadeDiZiNumResp_ = new KuangMaiJadeDiZiNumResp_1015045();
					kuangMaiJadeDiZiNumResp_.MergeFrom(byteString);
					if (kuangMaiJadeDiZiNumResp_.IsSuccess)
					{
						m_KeYongDiZiNum = kuangMaiJadeDiZiNumResp_.KeYongDiZiNum;
						m_Panel.LingYuKaiCaiRenShuInput.text = $"{m_LingYuKaiCaiRenShuNew}";
						KeYOngDiZi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(kuangMaiJadeDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void KuCunXianShi()
		{
			m_Panel.LingShiKuCunJingDuImage.fillAmount = (float)m_LingShiKuCunShu / (float)m_MaxLingShiKuCunShu;
			m_Panel.LingShiKuCunShuText.text = $"{m_LingShiKuCunShu}/{m_MaxLingShiKuCunShu}";
			m_Panel.LingShiKuCunShuBaiFenBiText.text = $"{(int)((double)m_LingShiKuCunShu / (double)m_MaxLingShiKuCunShu * 100.0)}%";
			m_Panel.LingYuKuCunJingDuImage.fillAmount = (float)m_LingYuKuCunShu / (float)m_MaxLingYuKuCunShu;
			m_Panel.LingYuKuCunShuText.text = $"{m_LingYuKuCunShu}/{m_MaxLingYuKuCunShu}";
			m_Panel.LingYuKuCunShuBaiFenBiText.text = $"{(int)((double)m_LingYuKuCunShu / (double)m_MaxLingYuKuCunShu * 100.0)}%";
		}

		private void ShengJiCangKu()
		{
			if (m_CangKuUpdateDic.TryGetValue(m_ChangKuCurrentStage, out var value))
			{
				List<string> values = new List<string>
				{
					value.MaxShinyStone,
					m_CangKuUpdateDic[m_ChangKuCurrentStage + 1].MaxShinyStone,
					value.MaxShinyJade.ToString(),
					m_CangKuUpdateDic[m_ChangKuCurrentStage + 1].MaxShinyJade.ToString(),
					m_CangKuUpdateDic[m_ChangKuCurrentStage + 1].ShinyStoneCast.ToString(),
					m_CangKuUpdateDic[m_ChangKuCurrentStage + 1].MaterialCast.ToString(),
					$"{long.Parse(m_CangKuUpdateDic[m_ChangKuCurrentStage + 1].TimeCast) / 3600}小时",
					GetZongMenChineseName(m_ChangKuCurrentStage + 1)
				};
				m_Panel.UpdateCangKuPanel.SetValue(values, CangKuSureBtnClick);
			}
			m_Panel.UpdateCangKuPanel.gameObject.SetActive(value: true);
		}

		private void CangKuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeKuangMaiSpace, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeKuangMaiSpaceResp_1015046 upgradeKuangMaiSpaceResp_ = new UpgradeKuangMaiSpaceResp_1015046();
					upgradeKuangMaiSpaceResp_.MergeFrom(byteString);
					if (upgradeKuangMaiSpaceResp_.IsSuccess)
					{
						m_ShengJiCangKuTimeShengYuTime = upgradeKuangMaiSpaceResp_.ShengYuTime / 1000;
						m_Time++;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeKuangMaiSpaceResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private string GetZongMenChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶宗门", 
				4 => "四阶宗门", 
				5 => "五阶宗门", 
				6 => "六阶宗门", 
				7 => "七阶宗门", 
				8 => "八阶宗门", 
				9 => "九阶宗门", 
				_ => "三阶宗门", 
			};
		}

		private void ShengJiGongJu()
		{
			if (m_GongJuUpdateDic.TryGetValue(m_GongJuCurrentStage, out var value))
			{
				List<string> values = new List<string>
				{
					value.HourYield.ToString(),
					m_GongJuUpdateDic[m_GongJuCurrentStage + 1].HourYield.ToString(),
					value.TimeLingYu.ToString(),
					m_GongJuUpdateDic[m_GongJuCurrentStage + 1].TimeLingYu.ToString(),
					m_GongJuUpdateDic[m_GongJuCurrentStage + 1].MultiplierConsumption.ToString(),
					$"{long.Parse(m_CangKuUpdateDic[m_ChangKuCurrentStage + 1].TimeCast) / 3600}小时",
					GetZongMenChineseName(m_GongJuCurrentStage + 1)
				};
				m_Panel.UpdateDiHuoShiPanel.SetValue(values, GongJuSureBtnClick);
			}
			m_Panel.UpdateDiHuoShiPanel.gameObject.SetActive(value: true);
		}

		private void GongJuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeKuangMaiFaQi, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeKuangMaiFaQiResp_1015047 upgradeKuangMaiFaQiResp_ = new UpgradeKuangMaiFaQiResp_1015047();
					upgradeKuangMaiFaQiResp_.MergeFrom(byteString);
					if (upgradeKuangMaiFaQiResp_.IsSuccess)
					{
						m_ShengJiGongJuTimeYuTime = upgradeKuangMaiFaQiResp_.ShengYuTime / 1000;
						m_Time++;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeKuangMaiFaQiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = string.Format("");
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "矿脉");
		}
	}
}
