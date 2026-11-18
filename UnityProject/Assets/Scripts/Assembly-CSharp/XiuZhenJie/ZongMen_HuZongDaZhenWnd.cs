using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_HuZongDaZhenWnd : Window
	{
		private ZongMen_HuZongDaZhenPanel m_Panel;

		private long m_YanWuTaiXingJianShengYuTime;

		private int m_Shu;

		private int Stage;

		private bool m_ZhangLaoBool;

		private bool m_ZhangLaoPanDuan;

		private float m_Time;

		private IntoHuZongMatrixResp_1015048 m_IntoHuZongMatrixResp1015048 = new IntoHuZongMatrixResp_1015048();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_HuZongDaZhenPanel>();
			m_ZhangLaoPanDuan = (bool)paralist[1];
			m_IntoHuZongMatrixResp1015048 = paralist[2] as IntoHuZongMatrixResp_1015048;
			if (m_ZhangLaoPanDuan)
			{
				m_ZhangLaoBool = true;
			}
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.ShengJiButton, TuiChuZongMen);
			AddButtonClickListener(m_Panel.ShengJiTimeButton, XinJianJiShi);
			ButBangDing();
			DaKuai();
		}

		private void ButBangDing()
		{
			AddButtonClickListener(m_Panel.Li_Progress, Li_But);
			AddButtonClickListener(m_Panel.Zhen_Progress, Zhen_But);
			AddButtonClickListener(m_Panel.Gen_Progress, Gen_But);
			AddButtonClickListener(m_Panel.Qian_Progress, Qian_But);
			AddButtonClickListener(m_Panel.Kan_Progress, Kan_But);
			AddButtonClickListener(m_Panel.Kun_Progress, Kun_But);
			AddButtonClickListener(m_Panel.Xun_Progress, Xun_But);
			AddButtonClickListener(m_Panel.Dui_Progress, Dui_But);
		}

		private void DaKuai()
		{
			m_YanWuTaiXingJianShengYuTime = m_IntoHuZongMatrixResp1015048.UpgradeMatrixShengYuTime / 1000;
			if (m_YanWuTaiXingJianShengYuTime > 0)
			{
				m_Time++;
			}
			if (m_IntoHuZongMatrixResp1015048.ZhangLaoInfo != null)
			{
				ZhangLaoFuZhi(m_IntoHuZongMatrixResp1015048.ZhangLaoInfo);
				m_Panel.PortraitWu.gameObject.SetActive(value: false);
				if (Singleton<RoleManager>.Instance.Role_Entity.Id == m_IntoHuZongMatrixResp1015048.ZhangLaoInfo.RoleId)
				{
					m_ZhangLaoBool = true;
				}
			}
			else
			{
				m_Panel.PortraitWu.gameObject.SetActive(value: true);
			}
			m_Panel.ShengJiButton.gameObject.SetActive(value: true);
			m_Panel.ShengJiButton.interactable = m_ZhangLaoBool;
			m_Panel.ShengJiTimeButton.interactable = m_ZhangLaoBool;
			HuZongFaZhen(m_IntoHuZongMatrixResp1015048);
			m_Shu = m_IntoHuZongMatrixResp1015048.ZhenYan - 1;
			XianShi();
		}

		private void ZhangLaoFuZhi(IntoHuZongMatrixResp_1015048.Types.ZhangLaoInfoResp_1015049 xingXi)
		{
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(xingXi.Sex, xingXi.RoleImage, isOnline: true);
			m_Panel.PortraitImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = xingXi.RoleName;
			m_Panel.ZhanLiText.text = $"{xingXi.Fighting}";
			m_Panel.PromoteText.text = $"防御力增加{xingXi.TeXiaoVal}%";
			m_Panel.JingJieText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(xingXi.Level);
		}

		private void HuZongFaZhen(IntoHuZongMatrixResp_1015048 xingXi)
		{
			Stage = xingXi.Stage;
			m_Panel.DangQianZhenFaText.text = WorldSetting.GetStageName(xingXi.Stage) + "护宗大阵";
			m_Panel.LingShiXiaoHaoText.text = $"{xingXi.Stage * 12}灵石/每分钟";
			m_Panel.LingYuXiaoHaoText.text = $"{xingXi.JadeXiaoHao}天";
			m_Panel.DangQianZhuangTaiText.text = KaiQiZHuanTai(xingXi.IsSuccess);
			m_Panel.FangYuLiText.text = $"{xingXi.FangYuLi}";
		}

		private string KaiQiZHuanTai(bool IsKaiQi)
		{
			if (!IsKaiQi)
			{
				return "关闭";
			}
			return "开启";
		}

		private void Li_But()
		{
			m_Shu = 0;
			HuZongMatrixUpdateEye();
		}

		private void Kun_But()
		{
			m_Shu = 1;
			HuZongMatrixUpdateEye();
		}

		private void Dui_But()
		{
			m_Shu = 2;
			HuZongMatrixUpdateEye();
		}

		private void Qian_But()
		{
			m_Shu = 3;
			HuZongMatrixUpdateEye();
		}

		private void Kan_But()
		{
			m_Shu = 4;
			HuZongMatrixUpdateEye();
		}

		private void Gen_But()
		{
			m_Shu = 5;
			HuZongMatrixUpdateEye();
		}

		private void Zhen_But()
		{
			m_Shu = 6;
			HuZongMatrixUpdateEye();
		}

		private void Xun_But()
		{
			m_Shu = 7;
			HuZongMatrixUpdateEye();
		}

		private void XianShi()
		{
			ImageGuabBi();
			GuanBiXianShi();
			switch (m_Shu)
			{
			case 0:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Li_Progress.gameObject.GetComponent<Image>());
				m_Panel.Li_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Li_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 1:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Kun_Progress.gameObject.GetComponent<Image>());
				m_Panel.Kun_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Kun_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 2:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Dui_Progress.gameObject.GetComponent<Image>());
				m_Panel.Dui_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Dui_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 3:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Qian_Progress.gameObject.GetComponent<Image>());
				m_Panel.Qian_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Qian_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 4:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Kan_Progress.gameObject.GetComponent<Image>());
				m_Panel.Kan_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Kan_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 5:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Gen_Progress.gameObject.GetComponent<Image>());
				m_Panel.Gen_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Gen_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 6:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Zhen_Progress.gameObject.GetComponent<Image>());
				m_Panel.Zhen_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Zhen_Progress_Hui.gameObject.SetActive(value: false);
				break;
			case 7:
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", m_Panel.Xun_Progress.gameObject.GetComponent<Image>());
				m_Panel.Xun_Progress_Hong.gameObject.SetActive(value: true);
				m_Panel.Xun_Progress_Hui.gameObject.SetActive(value: false);
				break;
			}
		}

		private void HuZongMatrixUpdateEye()
		{
			if (!m_ZhangLaoBool)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("护宗长老才可调整阵眼位置！");
				return;
			}
			HuZongMatrixUpdateEyeReq_1015149 data = new HuZongMatrixUpdateEyeReq_1015149
			{
				ZhenYan = m_Shu + 1
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.HuZongMatrixUpdateEye, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					HuZongMatrixUpdateEyeResp_1015149 huZongMatrixUpdateEyeResp_ = new HuZongMatrixUpdateEyeResp_1015149();
					huZongMatrixUpdateEyeResp_.MergeFrom(byteString);
					if (huZongMatrixUpdateEyeResp_.IsSuccess)
					{
						XianShi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(huZongMatrixUpdateEyeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GuanBiXianShi()
		{
			m_Panel.Li_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Li_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Zhen_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Zhen_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Gen_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Gen_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Qian_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Qian_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Kan_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Kan_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Kun_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Kun_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Xun_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Xun_Progress_Hui.gameObject.SetActive(value: true);
			m_Panel.Dui_Progress_Hong.gameObject.SetActive(value: false);
			m_Panel.Dui_Progress_Hui.gameObject.SetActive(value: true);
		}

		private void ImageGuabBi()
		{
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Li_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Zhen_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Gen_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Qian_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Kan_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Kun_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Xun_Progress.gameObject.GetComponent<Image>());
			ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", m_Panel.Dui_Progress.gameObject.GetComponent<Image>());
		}


		private void TuiChuZongMen()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_HuZongDaZhenUpgradeWnd.prefab", true, Stage);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
				ShenJiUpdating();
			}
		}

		private void ShenJiUpdating()
		{
			if (m_YanWuTaiXingJianShengYuTime > 0 && m_ZhangLaoBool)
			{
				m_YanWuTaiXingJianShengYuTime--;
				string text = CommonMethod.SecondToHour((int)m_YanWuTaiXingJianShengYuTime);
				m_Panel.ShengJiTimeButton.gameObject.SetActive(value: true);
				m_Panel.ShengJiTimeText.text = text;
			}
			else
			{
				m_Panel.ShengJiTimeButton.gameObject.SetActive(value: false);
			}
		}

		private void XinJianJiShi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, (int)m_YanWuTaiXingJianShengYuTime / 1000 / 60 + 1, new UnityAction<int>(SpeedUpdate));
		}

		public void JiShi(int time)
		{
			m_Time++;
			m_YanWuTaiXingJianShengYuTime = time / 1000;
		}

		private void SpeedUpdate(int castLingYu)
		{
			UpgradeHuZongMatrixJadeBuyTimeReq_1015051 data = new UpgradeHuZongMatrixJadeBuyTimeReq_1015051
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeHuZongMatrixJadeBuyTime, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeHuZongMatrixJadeBuyTimeResp_1015051 upgradeHuZongMatrixJadeBuyTimeResp_ = new UpgradeHuZongMatrixJadeBuyTimeResp_1015051();
					upgradeHuZongMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeHuZongMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_YanWuTaiXingJianShengYuTime = (int)upgradeHuZongMatrixJadeBuyTimeResp_.ShengYuTime / 1000;
						if (m_YanWuTaiXingJianShengYuTime <= 0)
						{
							m_YanWuTaiXingJianShengYuTime = 0L;
							ShenJiUpdating();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeHuZongMatrixJadeBuyTimeResp_.Code);
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
			string text = $"1、护宗大阵需要设置阵眼 2、兽潮战或宗门战中，如若阵眼被攻破，则大阵防御力会急剧下降 3、兽潮战或宗门中时，大阵会持续消耗灵石，若灵石不足，则防御力将大幅度下降甚至直接破灭";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "宗门大殿");
		}
	}
}
