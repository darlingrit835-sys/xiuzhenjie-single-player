using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_NeiWuDianWnd : Window
	{
		private ZongMen_NeiWuDianPanel m_Panel;

		private bool m_ZhangLaoPanDuan;

		private int m_Contribution;

		private int m_LingShiShu;

		private int m_LingShiShenyuCiShu;

		private int m_LingYuShu;

		private int m_LingYuShenyuCiShu;

		private long m_LingShiXiaoHao;

		private int m_LingYuXiaoHao;

		private int m_DanYaoXiaoHao;

		private int m_FaQiXiaoHao;

		private int m_FuLuXiaoHao;

		private int m_ZhenPanXiaoHao;

		private IntoDaDianResp_1015020 m_IntoDaDianResp1015020;

		private bool m_PanDuanXiuGai;

		private IntoNeiWuResp_1015034 m_IntoNeiWuResp1015034 = new IntoNeiWuResp_1015034();

		public override void Awake(params object[] paralist)
		{
			m_Panel = base.GameObject.GetComponent<ZongMen_NeiWuDianPanel>();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			if (paralist[0] != null)
			{
				m_IntoDaDianResp1015020 = paralist[0] as IntoDaDianResp_1015020;
			}
			m_ZhangLaoPanDuan = (bool)paralist[1];
			m_IntoNeiWuResp1015034 = paralist[2] as IntoNeiWuResp_1015034;
			if (m_ZhangLaoPanDuan)
			{
				m_PanDuanXiuGai = true;
			}
			Shezhi();
			if (m_IntoDaDianResp1015020 != null)
			{
				ShuaXin(m_IntoDaDianResp1015020.Influence);
			}
			HuoQuZhangLaoXingXi();
		}

		private void HuoQuZhangLaoXingXi()
		{
			m_Contribution = (int)m_IntoNeiWuResp1015034.CurrentContribution;
			ShuaXinNew(m_IntoNeiWuResp1015034.PreYingXiangLi);
			if (m_IntoNeiWuResp1015034.ZhangLaoInfo != null)
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.Id == m_IntoNeiWuResp1015034.ZhangLaoInfo.RoleId)
				{
					m_PanDuanXiuGai = true;
				}
				m_Panel.ZhangLaoWuImage.gameObject.SetActive(value: false);
				ZhangLaoFuZhi(m_IntoNeiWuResp1015034.ZhangLaoInfo);
			}
			else
			{
				m_Panel.ZhangLaoWuImage.gameObject.SetActive(value: true);
			}
			FuZhiJiChuXingXi(m_IntoNeiWuResp1015034);
			m_LingShiShenyuCiShu = m_IntoNeiWuResp1015034.StoneShengYuCount;
			m_LingYuShenyuCiShu = m_IntoNeiWuResp1015034.JadeShengYuCount;
			m_LingYuNew();
			m_LingShiNew();
			JuanZhen();
			PanDuan();
			LingShiFuZhi((int)m_IntoNeiWuResp1015034.StoneKuCun, (int)m_IntoNeiWuResp1015034.StoneChanLiang);
			LingYuFuZhi((int)m_IntoNeiWuResp1015034.JadeKuCun, (int)m_IntoNeiWuResp1015034.JadeChanLiang);
			DanYaoFuZhi((int)m_IntoNeiWuResp1015034.DanYaoKuCun, m_IntoNeiWuResp1015034.DanYaoChanLiang);
			FaQiFuZhi((int)m_IntoNeiWuResp1015034.FaQiKuCun, m_IntoNeiWuResp1015034.FaQiChanLiang);
			FuLuFuZhi((int)m_IntoNeiWuResp1015034.FuLuKuCun, m_IntoNeiWuResp1015034.FuLuChanLiang);
			FuLuKuCunFuZhi((int)m_IntoNeiWuResp1015034.ZhenPanKuCun, m_IntoNeiWuResp1015034.ZhenPanChanLiang);
		}

		private void FuZhiJiChuXingXi(IntoNeiWuResp_1015034 intoNeiWuResp1015034)
		{
			m_LingShiXiaoHao = intoNeiWuResp1015034.StoneSalary;
			m_LingYuXiaoHao = intoNeiWuResp1015034.JadeSalary;
			m_ZhenPanXiaoHao = intoNeiWuResp1015034.MatrixSalary;
			m_DanYaoXiaoHao = intoNeiWuResp1015034.ElixirSalary;
			m_FaQiXiaoHao = intoNeiWuResp1015034.EquipmentSalary;
			m_FuLuXiaoHao = intoNeiWuResp1015034.SignSalary;
			LingShi_XiaoHao();
			LingYu_XiaoHao();
			ZhenPan_XiaoHao();
			DanYao_XiaoHao();
			FaQi_XiaoHao();
			FuLu_XiaoHao();
		}

		private void PanDuan()
		{
			m_Panel.BaoCuButton.interactable = m_PanDuanXiuGai;
			m_Panel.TuiChuButton.interactable = m_PanDuanXiuGai;
			if (m_PanDuanXiuGai)
			{
				m_Panel.LingShi_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.LingShi_ShuRuText.gameObject.SetActive(value: false);
				m_Panel.LingYu_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.LingYu_ShuRuText.gameObject.SetActive(value: false);
				m_Panel.YaoCai_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.YaoCai_ShuRuText.gameObject.SetActive(value: false);
				m_Panel.DanYao_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.DanYao_ShuRuText.gameObject.SetActive(value: false);
				m_Panel.FaQi_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.FaQi_ShuRuText.gameObject.SetActive(value: false);
				m_Panel.FuLu_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.FuLu_ShuRuText.gameObject.SetActive(value: false);
				m_Panel.ZhenPan_ShuRuInput.gameObject.SetActive(value: true);
				m_Panel.ZhenPan_ShuRuText.gameObject.SetActive(value: false);
				JiaZhai();
			}
			else
			{
				m_Panel.LingShi_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.LingShi_ShuRuText.gameObject.SetActive(value: true);
				m_Panel.LingYu_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.LingYu_ShuRuText.gameObject.SetActive(value: true);
				m_Panel.YaoCai_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.YaoCai_ShuRuText.gameObject.SetActive(value: true);
				m_Panel.DanYao_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.DanYao_ShuRuText.gameObject.SetActive(value: true);
				m_Panel.FaQi_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.FaQi_ShuRuText.gameObject.SetActive(value: true);
				m_Panel.FuLu_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.FuLu_ShuRuText.gameObject.SetActive(value: true);
				m_Panel.ZhenPan_ShuRuInput.gameObject.SetActive(value: false);
				m_Panel.ZhenPan_ShuRuText.gameObject.SetActive(value: true);
				FuZhi();
			}
		}

		private void FuZhi()
		{
			m_Panel.LingShi_ShuRuText.text = $"{m_LingShiXiaoHao}";
			m_Panel.LingYu_ShuRuText.text = $"{m_LingYuXiaoHao}";
			m_Panel.DanYao_ShuRuText.text = $"{m_DanYaoXiaoHao}";
			m_Panel.FaQi_ShuRuText.text = $"{m_FaQiXiaoHao}";
			m_Panel.FuLu_ShuRuText.text = $"{m_FuLuXiaoHao}";
			m_Panel.ZhenPan_ShuRuText.text = $"{m_ZhenPanXiaoHao}";
		}

		private void ZhangLaoFuZhi(IntoNeiWuResp_1015034.Types.ZhangLaoInfoResp_1015035 xingXi)
		{
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(xingXi.Sex, xingXi.RoleImage, isOnline: true);
			m_Panel.PortraitImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = xingXi.RoleName;
			m_Panel.ZhanLiText.text = $"{xingXi.Fighting}";
			m_Panel.ProductionText.text = $"结算降低薪俸发放数额{xingXi.TeXiaoVal}%";
			m_Panel.JingJieText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(xingXi.Level);
		}


		private void Shezhi()
		{
			string text = "";
			string text2 = "";
			switch (m_IntoDaDianResp1015020.Stage)
			{
			case 3:
				text = "10000";
				text2 = "40";
				break;
			case 4:
				text = "30000";
				text2 = "50";
				break;
			case 5:
				text = "70000";
				text2 = "60";
				break;
			case 6:
				text = "120000";
				text2 = "70";
				break;
			case 7:
				text = "200000";
				text2 = "80";
				break;
			case 8:
				text = "500000";
				text2 = "90";
				break;
			case 9:
				text = "1000000";
				text2 = "100";
				break;
			}
			m_Panel.JuanZengLeft_ShuReText.text = text;
			m_Panel.JuanZengRight_ShuReText.text = text2;
		}

		private void JuanZhen()
		{
			m_Panel.ContributionText.text = $"当前贡献:{m_Contribution}";
			AddButtonClickListener(m_Panel.JuanZengLeft_JuanZengButton, JuanZengLeft_JuanZeng);
			AddButtonClickListener(m_Panel.JuanZengRight_JuanZengButton, JuanZengRight_JuanZeng);
		}

		private void m_LingShiNew()
		{
			m_Panel.JuanZengLeft_NumText.text = $"{m_LingShiShenyuCiShu}次";
		}

		private void m_LingYuNew()
		{
			m_Panel.JuanZengRight_NumText.text = $"{m_LingYuShenyuCiShu}次";
		}

		private void JuanZengLeft_JuanZeng()
		{
			if (m_LingShiShenyuCiShu < 1)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("可用捐赠次数不足！");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.NEIWUJUANZENGSTONE, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					NeiWuJuanZengStoneResp_1015036 neiWuJuanZengStoneResp_ = new NeiWuJuanZengStoneResp_1015036();
					neiWuJuanZengStoneResp_.MergeFrom(byteString);
					if (neiWuJuanZengStoneResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = neiWuJuanZengStoneResp_.CurrentStone;
						m_Panel.ContributionText.text = $"当前贡献:{neiWuJuanZengStoneResp_.CurrentContribution}";
						m_LingShiShenyuCiShu--;
						m_LingShiNew();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(neiWuJuanZengStoneResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuanZengRight_JuanZeng()
		{
			if (m_LingYuShenyuCiShu < 1)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("可用捐赠次数不足！");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.NEIWUJUANZENGJADE, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					NeiWuJuanZengJadeResp_1015037 neiWuJuanZengJadeResp_ = new NeiWuJuanZengJadeResp_1015037();
					neiWuJuanZengJadeResp_.MergeFrom(byteString);
					if (neiWuJuanZengJadeResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = neiWuJuanZengJadeResp_.CurrentJade;
						m_Panel.ContributionText.text = $"当前贡献:{neiWuJuanZengJadeResp_.CurrentContribution}";
						m_LingYuShenyuCiShu--;
						m_LingYuNew();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(neiWuJuanZengJadeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JiaZhai()
		{
			m_Panel.LingShi_ShuRuInput.onEndEdit.AddListener(LingShi);
			m_Panel.LingYu_ShuRuInput.onEndEdit.AddListener(LingYu);
			m_Panel.DanYao_ShuRuInput.onEndEdit.AddListener(DanYao);
			m_Panel.FaQi_ShuRuInput.onEndEdit.AddListener(FaQi);
			m_Panel.FuLu_ShuRuInput.onEndEdit.AddListener(FuLu);
			m_Panel.ZhenPan_ShuRuInput.onEndEdit.AddListener(ZhenPan);
			AddButtonClickListener(m_Panel.BaoCuButton, BaoCu);
			AddButtonClickListener(m_Panel.TuiChuButton, LiKaiBtn);
		}

		private void BaoCu()
		{
			NeiWuSalarySettingReq_1015153 neiWuSalarySettingReq_ = new NeiWuSalarySettingReq_1015153
			{
				StoneSalary = m_LingShiXiaoHao,
				JadeSalary = m_LingYuXiaoHao,
				MatrixSalary = m_ZhenPanXiaoHao,
				ElixirSalary = m_DanYaoXiaoHao,
				EquipmentSalary = m_FaQiXiaoHao,
				SignSalary = m_FuLuXiaoHao
			};
			if (neiWuSalarySettingReq_.StoneSalary < 1)
			{
				neiWuSalarySettingReq_.StoneSalary = -1L;
			}
			if (neiWuSalarySettingReq_.JadeSalary < 1)
			{
				neiWuSalarySettingReq_.JadeSalary = -1;
			}
			if (neiWuSalarySettingReq_.MatrixSalary < 1)
			{
				neiWuSalarySettingReq_.MatrixSalary = -1;
			}
			if (neiWuSalarySettingReq_.ElixirSalary < 1)
			{
				neiWuSalarySettingReq_.ElixirSalary = -1;
			}
			if (neiWuSalarySettingReq_.EquipmentSalary < 1)
			{
				neiWuSalarySettingReq_.EquipmentSalary = -1;
			}
			if (neiWuSalarySettingReq_.SignSalary < 1)
			{
				neiWuSalarySettingReq_.SignSalary = -1;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.NeiWuSalarySetting, neiWuSalarySettingReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					NeiWuSalarySettingResp_1015153 neiWuSalarySettingResp_ = new NeiWuSalarySettingResp_1015153();
					neiWuSalarySettingResp_.MergeFrom(byteString);
					if (neiWuSalarySettingResp_.IsSuccess)
					{
						ShuaXinNew(neiWuSalarySettingResp_.PreYingXiangLi);
						FuZhi();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(neiWuSalarySettingResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void LingShiFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.LingShi_CunLiangText.text = $"{cunLiang}";
			m_Panel.LingShi_ChangLiangText.text = $"{changLiang}";
		}

		private void LingYuFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.LingYu_CunLiangText.text = $"{cunLiang}";
			m_Panel.LingYu_ChangLiangText.text = $"{changLiang}";
		}

		private void YaoCaiFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.YaoCai_CunLiangText.text = $"{cunLiang}";
			m_Panel.YaoCai_ChangLiangText.text = $"{changLiang}";
		}

		private void KuangShiLiangFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.KuangShi_CunLiangText.text = $"{cunLiang}";
			m_Panel.KuangShi_ChangLiangText.text = $"{changLiang}";
		}

		private void YuanCaiKuCunFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.YuanCai_CunLiangText.text = $"{cunLiang}";
			m_Panel.YuanCai_ChangLiangText.text = $"{changLiang}";
		}

		private void DanYaoFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.DanYao_CunLiangText.text = $"{cunLiang}";
			m_Panel.DanYao_ChangLiangText.text = $"{changLiang}";
		}

		private void FaQiFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.FaQi_CunLiangText.text = $"{cunLiang}";
			m_Panel.FaQi_ChangLiangText.text = $"{changLiang}";
		}

		private void FuLuFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.FuLu_CunLiangText.text = $"{cunLiang}";
			m_Panel.FuLu_ChangLiangText.text = $"{changLiang}";
		}

		private void FuLuKuCunFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.ZhenPan_CunLiangText.text = $"{cunLiang}";
			m_Panel.ZhenPan_ChangLiangText.text = $"{changLiang}";
		}

		private void YaoShouChanLiangFuZhi(int cunLiang, int changLiang)
		{
			m_Panel.YaoShou_CunLiangText.text = $"{cunLiang}";
			m_Panel.YaoShouChangLiangText.text = $"{changLiang}";
		}

		private void LingShi(string count)
		{
			if (!string.IsNullOrEmpty(count))
			{
				m_LingShiXiaoHao = int.Parse(count);
			}
			else
			{
				m_LingShiXiaoHao = 0L;
			}
			LingShi_XiaoHao();
		}

		private void LingShi_XiaoHao()
		{
			m_Panel.LingShi_ShuRuInput.text = $"{m_LingShiXiaoHao}";
			m_Panel.LingShi_XiaoHaoText.text = $"每日共消耗{m_LingShiXiaoHao * m_IntoDaDianResp1015020.DiZiShuLiang}灵石";
		}

		private void LingYu(string count)
		{
			if (!string.IsNullOrEmpty(count))
			{
				m_LingYuXiaoHao = int.Parse(count);
			}
			else
			{
				m_LingYuXiaoHao = 0;
			}
			LingYu_XiaoHao();
		}

		private void LingYu_XiaoHao()
		{
			m_Panel.LingYu_ShuRuInput.text = $"{m_LingYuXiaoHao}";
			m_Panel.LingYu_XiaoHaoText.text = $"每日共消耗{m_LingYuXiaoHao * m_IntoDaDianResp1015020.DiZiShuLiang}灵玉";
		}

		private void DanYao(string count)
		{
			if (!string.IsNullOrEmpty(count))
			{
				m_DanYaoXiaoHao = int.Parse(count);
			}
			else
			{
				m_DanYaoXiaoHao = 0;
			}
			DanYao_XiaoHao();
		}

		private void DanYao_XiaoHao()
		{
			m_Panel.DanYao_ShuRuInput.text = $"{m_DanYaoXiaoHao}";
			m_Panel.DanYao_XiaoHaoText.text = $"每日共消耗{m_DanYaoXiaoHao * m_IntoDaDianResp1015020.DiZiShuLiang}丹药";
		}

		private void FaQi(string count)
		{
			if (!string.IsNullOrEmpty(count))
			{
				m_FaQiXiaoHao = int.Parse(count);
			}
			else
			{
				m_FaQiXiaoHao = 0;
			}
			FaQi_XiaoHao();
		}

		private void FaQi_XiaoHao()
		{
			m_Panel.FaQi_ShuRuInput.text = $"{m_FaQiXiaoHao}";
			m_Panel.FaQi_XiaoHaoText.text = $"每日共消耗{m_FaQiXiaoHao * m_IntoDaDianResp1015020.DiZiShuLiang}法器";
		}

		private void FuLu(string count)
		{
			if (!string.IsNullOrEmpty(count))
			{
				m_FuLuXiaoHao = int.Parse(count);
			}
			else
			{
				m_FuLuXiaoHao = 0;
			}
			FuLu_XiaoHao();
		}

		private void FuLu_XiaoHao()
		{
			m_Panel.FuLu_ShuRuInput.text = $"{m_FuLuXiaoHao}";
			m_Panel.FuLu_XiaoHaoText.text = $"每日共消耗{m_FuLuXiaoHao * m_IntoDaDianResp1015020.DiZiShuLiang}符箓";
		}

		private void ZhenPan(string count)
		{
			if (!string.IsNullOrEmpty(count))
			{
				m_ZhenPanXiaoHao = int.Parse(count);
			}
			else
			{
				m_ZhenPanXiaoHao = 0;
			}
			ZhenPan_XiaoHao();
		}

		private void ZhenPan_XiaoHao()
		{
			m_Panel.ZhenPan_ShuRuInput.text = $"{m_ZhenPanXiaoHao}";
			m_Panel.ZhenPan_XiaoHaoText.text = $"每日共消耗{m_ZhenPanXiaoHao * m_IntoDaDianResp1015020.DiZiShuLiang}阵盘";
		}

		private void ShuaXin(int yinxiangli)
		{
			m_Panel.YingXiangLiText.text = $"当前影响力{YingXiangLi(yinxiangli)}({yinxiangli})点";
		}

		private void ShuaXinNew(int yinxiangli)
		{
			m_Panel.ZhanDoLiText.text = $"次日结算后预计影响力{YingXiangLi(yinxiangli)}({yinxiangli})点";
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

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、所有长老可以在内务殿捐赠灵石、灵玉以换去宗门贡献 2、宗门等阶越高则可以捐赠的灵石、灵玉以及换去的贡献越多 3、设置弟子的薪俸，每日0点结算，结算后，会影响整天的宗门影响力 4、宗门影响力越高，则入宗弟子越多，为保证宗门昌盛，尽可能提高弟子待遇";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "内务殿");
		}
	}
}
