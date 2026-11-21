using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_ZhiFaDianWnd : Window
	{
		private ZongMen_ZhiFaDianPanel m_Panel;

		private bool m_ZhangLaoPanDuan;

		private bool m_ZhangLaoBool;

		private long m_DiZiShuLiang;

		private double m_MeiXiaoShiSiWang;

		private long m_MeiXiaoShiRuZong;

		private long m_DiZiZhanDouLi;

		private long m_DiZhiFangYuLi;

		private int m_ZhiFaDiZhi;

		private int m_ZoMenHunLuanDu;

		private int m_ZhiFaDiZiShuLiang;

		private int KeYongDiZiNum;

		private bool ZhiFaCeLue;

		private bool dianJi;

		private IntoZhiFaResp_1015026 m_IntoZhiFaResp1015026 = new IntoZhiFaResp_1015026();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_ZhiFaDianPanel>();
			m_ZhangLaoBool = (bool)paralist[1];
			m_IntoZhiFaResp1015026 = paralist[2] as IntoZhiFaResp_1015026;
			if (m_ZhangLaoBool)
			{
				m_ZhangLaoPanDuan = true;
			}
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.TuiChuButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.BaoCunButton, BaoCun);
			AddButtonClickListener(m_Panel.YunXuShiToggle, YunXuShi);
			AddButtonClickListener(m_Panel.JinZhiToggle, YunXuShi);
			GetZhangLaoData();
		}

		private void GetZhangLaoData()
		{
			ZhiFaCeLue = m_IntoZhiFaResp1015026.ZhiFaCeLue;
			if (m_IntoZhiFaResp1015026.ZhangLaoInfo != null)
			{
				if (m_IntoZhiFaResp1015026.ZhangLaoInfo.RoleId == Singleton<RoleManager>.Instance.Role_Entity.Id)
				{
					m_ZhangLaoPanDuan = true;
				}
				m_Panel.PortraitWu.gameObject.SetActive(value: false);
				ZhangLaoFuZhi(m_IntoZhiFaResp1015026.ZhangLaoInfo);
			}
			else
			{
				m_Panel.PortraitWu.gameObject.SetActive(value: true);
			}
			m_Panel.BaoCunButton.gameObject.SetActive(value: true);
			m_Panel.TuiChuButton.gameObject.SetActive(value: true);
			m_Panel.BaoCunButton.interactable = m_ZhangLaoPanDuan;
			m_Panel.TuiChuButton.interactable = m_ZhangLaoPanDuan;
			PanDuanZhangLao(m_IntoZhiFaResp1015026.ZhiFaCeLue);
			ZhiFaDiZi(m_IntoZhiFaResp1015026);
			DiZhiQingKuang(m_IntoZhiFaResp1015026);
		}

		private void ZhangLaoFuZhi(IntoZhiFaResp_1015026.Types.ZhangLaoInfoResp_1015027 zongZhuInfo)
		{
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(zongZhuInfo.Sex, zongZhuInfo.RoleImage, isOnline: true);
			m_Panel.PortraitImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = zongZhuInfo.RoleName;
			m_Panel.ZhanLiText.text = $"{zongZhuInfo.Fighting}";
			m_Panel.PromoteText.text = $"增加稳定系数{zongZhuInfo.TeXiaoVal}%";
			m_Panel.JingJieText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(zongZhuInfo.Level);
		}


		private void DiZhiQingKuang(IntoZhiFaResp_1015026 xingXi)
		{
			m_DiZiShuLiang = xingXi.DiZiShuLiang;
			m_MeiXiaoShiSiWang = xingXi.SiWangLv;
			m_DiZiZhanDouLi = xingXi.DiZiZhanDouLi;
			m_DiZhiFangYuLi = xingXi.DiZiFangYuLi;
			m_ZhiFaDiZhi = xingXi.ZhiFaDiZiShuLiang;
			m_ZoMenHunLuanDu = xingXi.ZongMenHunLuanDu;
			ZhiFaDiZhiGengXing();
		}

		private void ZhiFaDiZhiGengXing()
		{
			m_Panel.DiZhiShuLiangText.text = $"{m_DiZiShuLiang}";
			m_Panel.TimeShiWangText.text = $"{m_MeiXiaoShiSiWang:0.00}%";
			m_Panel.TimeJingRuText.text = $"{m_MeiXiaoShiRuZong}";
			m_Panel.DiZhiZhanLiText.text = $"{(int)m_DiZiZhanDouLi}";
			m_Panel.DiZhiFangYuLiText.text = $"{(int)m_DiZhiFangYuLi}";
			m_Panel.ZhiFaDiZhiText.text = $"{m_ZhiFaDiZhi}";
			m_Panel.ZoMenHunLuanDuText.text = $"{m_ZoMenHunLuanDu}%";
		}

		private void PanDuanZhangLao(bool xingXi)
		{
			ZhiFaCeLue = xingXi;
			if (xingXi)
			{
				m_Panel.YunXuShiImage.gameObject.SetActive(value: true);
				m_Panel.JinZhiImage.gameObject.SetActive(value: false);
				dianJi = true;
			}
			else
			{
				m_Panel.YunXuShiImage.gameObject.SetActive(value: false);
				m_Panel.JinZhiImage.gameObject.SetActive(value: true);
				dianJi = false;
			}
		}

		private void YunXuShi()
		{
			if (m_ZhangLaoPanDuan)
			{
				ZhiFaCeLue = !ZhiFaCeLue;
				dianJi = !dianJi;
				m_Panel.YunXuShiImage.gameObject.SetActive(dianJi);
				m_Panel.JinZhiImage.gameObject.SetActive(!dianJi);
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("仅执法长老可设置！");
			}
		}

		private void ZhiFaDiZi(IntoZhiFaResp_1015026 xingXi)
		{
			KeYongDiZiNum = xingXi.KeYongDiZiNum;
			m_Panel.KeQongDiZhiText.text = $"可用弟子{xingXi.KeYongDiZiNum}名 ";
			m_ZhiFaDiZiShuLiang = xingXi.ZhiFaDiZiShuLiang;
			if (m_ZhangLaoPanDuan)
			{
				DangQianRenShu();
				m_Panel.DangQianRenShuInputField.onEndEdit.AddListener(DangQianRenShuInputField);
				AddButtonClickListener(m_Panel.DangQianRenShuJiaButton, JiaHaoXiaoGuo);
				AddButtonClickListener(m_Panel.DangQianRenShuJianButton, JianHaoXiaoGuo);
			}
			else
			{
				m_Panel.DangQianRenShuImage.gameObject.SetActive(value: true);
				m_Panel.DangQianRenShuText.text = $"执法弟子: {xingXi.ZhiFaDiZiShuLiang}人";
			}
		}

		private void DangQianRenShuInputField(string count)
		{
			if (!string.IsNullOrEmpty(m_Panel.DangQianRenShuInputField.text))
			{
				string text = m_Panel.DangQianRenShuInputField.text;
				m_ZhiFaDiZiShuLiang = int.Parse(text);
			}
			else
			{
				m_ZhiFaDiZiShuLiang = 0;
			}
			if (m_ZhiFaDiZiShuLiang < 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("执法弟子最低为0人！");
				m_Panel.DangQianRenShuInputField.text = "0";
				m_ZhiFaDiZiShuLiang = 0;
			}
			else
			{
				DangQianRenShu();
			}
		}

		private void JiaHaoXiaoGuo()
		{
			m_ZhiFaDiZiShuLiang += 10;
			KeYongDiZiNum -= 10;
			DangQianRenShu();
		}

		private void JianHaoXiaoGuo()
		{
			m_ZhiFaDiZiShuLiang -= 10;
			if (m_ZhiFaDiZiShuLiang < 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("执法弟子最低为0人！");
				m_Panel.DangQianRenShuInputField.text = "0";
				m_ZhiFaDiZiShuLiang = 0;
			}
			else
			{
				KeYongDiZiNum += 10;
				DangQianRenShu();
			}
		}

		private void DangQianRenShu()
		{
			m_Panel.DangQianRenShuInputField.text = $"{m_ZhiFaDiZiShuLiang}";
			JiShuanJiaChen();
		}

		private void JiShuanJiaChen()
		{
			ZhiFaDiZhiGengXing();
		}

		private void BaoCun()
		{
			SheZhiZhiFaCeLueDiZiNumReq_1015028 data = new SheZhiZhiFaCeLueDiZiNumReq_1015028
			{
				ZhiFaDiZiNum = m_ZhiFaDiZiShuLiang,
				ZhiFaCeLue = ZhiFaCeLue
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SHEZHIZHIFACELUE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SheZhiZhiFaCeLueDiZiNumResp_1015028 sheZhiZhiFaCeLueDiZiNumResp_ = new SheZhiZhiFaCeLueDiZiNumResp_1015028();
					sheZhiZhiFaCeLueDiZiNumResp_.MergeFrom(byteString);
					if (sheZhiZhiFaCeLueDiZiNumResp_.IsSuccess)
					{
						m_MeiXiaoShiSiWang = sheZhiZhiFaCeLueDiZiNumResp_.SiWangLv;
						m_ZoMenHunLuanDu = sheZhiZhiFaCeLueDiZiNumResp_.ZongMenHunLuanDu;
						m_Panel.KeQongDiZhiText.text = $"可用弟子{sheZhiZhiFaCeLueDiZiNumResp_.KeYongDiZiNum}名 ";
						m_ZhiFaDiZhi = m_ZhiFaDiZiShuLiang;
						ZhiFaDiZhiGengXing();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(sheZhiZhiFaCeLueDiZiNumResp_.Code);
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
			string text = $"1、执法殿用以维护宗门的纪律 2、稳定系数越高，则弟子的意外死亡率越低 3、设定执法策略为允许私斗则会增加弟子战斗力，禁止私斗则会增加稳定系数 4、执法弟子越多，则宗门稳定系数越高";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "执法殿");
		}
	}
}
