using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_YanWuTaiWnd : Window
	{
		private ZongMen_YanWuTaiPanel m_Panel;

		private bool m_ZhangLaoPanDuan;

		private IntoDaDianResp_1015020 m_IntoDaDianResp1015020;

		private int m_YanWuTaiLevel;

		private int m_YanWuTaiNum;

		private int m_MatrixStage;

		private bool m_Special;

		private long m_LingShi;

		private int m_DanYao;

		private int m_FaQi;

		private Dictionary<int, ZM_YanWuChangUpdateEntity> m_UpdateDic;

		private long m_YanWuTaiXingJianShengYuTime;

		private long m_YanWuTaiShengJiShengYuTime;

		private long m_ZhenFaShengJiShengYuTime;

		private float m_Time;

		private IntoYanWuResp_1015030 intoYanWuResp_1015030 = new IntoYanWuResp_1015030();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_IntoDaDianResp1015020 = (IntoDaDianResp_1015020)paralist[0];
			m_ZhangLaoPanDuan = (bool)paralist[1];
			intoYanWuResp_1015030 = paralist[2] as IntoYanWuResp_1015030;
			m_Panel = base.GameObject.GetComponent<ZongMen_YanWuTaiPanel>();
			m_Panel.XinJianButton.interactable = m_ZhangLaoPanDuan;
			m_Panel.ShengJiButton.interactable = m_ZhangLaoPanDuan;
			m_Panel.ShengJiFangHuButton.interactable = m_ZhangLaoPanDuan;
			m_Panel.XinJianJiShiButton.interactable = m_ZhangLaoPanDuan;
			m_Panel.ShengJiJiShiButton.interactable = m_ZhangLaoPanDuan;
			m_Panel.ShengJiJiShiFangHuButton.interactable = m_ZhangLaoPanDuan;
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.GuanBiButton, GuanBiBut);
			m_UpdateDic = Singleton<FacadeManager>.Instance.SelectYanWuChangUpdateEntities();
			AddButtonClickListener(m_Panel.XinJianButton, XinJian);
			AddButtonClickListener(m_Panel.ShengJiButton, ShengJi);
			AddButtonClickListener(m_Panel.ShengJiFangHuButton, ShengJiFangHu);
			AddButtonClickListener(m_Panel.XinJianJiShiButton, XinJianJiShi);
			AddButtonClickListener(m_Panel.ShengJiJiShiButton, ShengJiJiShi);
			AddButtonClickListener(m_Panel.ShengJiJiShiFangHuButton, ShengJiJiShiFangHu);
			IntoYanWu();
		}

		private void IntoYanWu()
		{
			Special(intoYanWuResp_1015030.JiangLiSuoDing);
			FuZhiBangDing(intoYanWuResp_1015030);
			BiWuTaiXingXi(intoYanWuResp_1015030);
		}

		private void XinJian()
		{
			m_Panel.XuanZheImage.gameObject.SetActive(value: true);
			m_Panel.DutyTitleText.text = "新建演武台";
			m_Panel.Line_One_TitleText.text = "最大个数";
			m_Panel.Line_One_BottomText.text = $"{m_YanWuTaiLevel}个";
			m_Panel.Line_Two_Text.text = "当前个数";
			m_Panel.Line_Two_ValueText.text = $"{m_YanWuTaiNum}个";
			m_Panel.Line_Three_Text.text = "灵材消耗";
			m_Panel.Line_Three_ValueText.text = $"{m_UpdateDic[m_YanWuTaiNum + 1].MaterialCast}";
			m_Panel.Line_Four_Text.text = "灵矿消耗";
			m_Panel.Line_Four_ValueText.text = $"{m_UpdateDic[m_YanWuTaiNum + 1].ShinyStoneCast}";
			m_Panel.Line_Five_Text.text = "升级用时";
			m_Panel.Line_Five_ValueText.text = $"{m_UpdateDic[m_YanWuTaiNum + 1].TimeCast}";
			m_Panel.Line_Six_Text.text = "升级要求";
			m_Panel.Line_Six_ValueText.text = WorldSetting.GetStageName(m_YanWuTaiLevel) + "演武台";
			m_Panel.QuRengShengJiButton.onClick.AddListener(XinJianBut);
		}

		private void XinJianBut()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.CREATEYANWUTAI, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					CreateYanWuTaiResp_1015032 createYanWuTaiResp_ = new CreateYanWuTaiResp_1015032();
					createYanWuTaiResp_.MergeFrom(byteString);
					if (createYanWuTaiResp_.IsSuccess)
					{
						m_YanWuTaiXingJianShengYuTime = createYanWuTaiResp_.ShengYuTime / 1000;
						m_Time++;
						GuanBiBut();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createYanWuTaiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShengJi()
		{
			m_Panel.XuanZheImage.gameObject.SetActive(value: true);
			m_Panel.DutyTitleText.text = "升级演武台";
			m_Panel.Line_One_TitleText.text = "当前等级";
			m_Panel.Line_One_BottomText.text = YanWuTaiLevel(m_YanWuTaiLevel) + "演武台";
			m_Panel.Line_Two_Text.text = "下个等级";
			m_Panel.Line_Two_ValueText.text = YanWuTaiLevel(m_YanWuTaiLevel + 1) + "演武台";
			m_Panel.Line_Three_Text.text = "灵材消耗";
			m_Panel.Line_Three_ValueText.text = $"{m_UpdateDic[m_YanWuTaiLevel + 1].MaterialCast}";
			m_Panel.Line_Four_Text.text = "灵矿消耗";
			m_Panel.Line_Four_ValueText.text = $"{m_UpdateDic[m_YanWuTaiLevel + 1].ShinyStoneCast}";
			m_Panel.Line_Five_Text.text = "升级用时";
			m_Panel.Line_Five_ValueText.text = $"{m_UpdateDic[m_YanWuTaiLevel + 1].TimeCast}";
			m_Panel.Line_Six_Text.text = "升级要求";
			m_Panel.Line_Six_ValueText.text = WorldSetting.GetStageName(m_YanWuTaiLevel) + "演武台";
			m_Panel.QuRengShengJiButton.onClick.AddListener(ShengJiBut);
		}

		private void ShengJiBut()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADEYANWUTAI, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYanWuTaiResp_1015142 upgradeYanWuTaiResp_ = new UpgradeYanWuTaiResp_1015142();
					upgradeYanWuTaiResp_.MergeFrom(byteString);
					if (upgradeYanWuTaiResp_.IsSuccess)
					{
						m_YanWuTaiShengJiShengYuTime = upgradeYanWuTaiResp_.ShengYuTime / 1000;
						m_Time++;
						GuanBiBut();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYanWuTaiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShengJiFangHu()
		{
			m_Panel.XuanZheImage.gameObject.SetActive(value: true);
			m_Panel.DutyTitleText.text = "升级防护阵法";
			m_Panel.Line_One_TitleText.text = "当前等级";
			m_Panel.Line_One_BottomText.text = YanWuTaiLevel(m_MatrixStage) + "防护阵法";
			m_Panel.Line_Two_Text.text = "下个等级";
			m_Panel.Line_Two_ValueText.text = YanWuTaiLevel(m_MatrixStage + 1) + "防护阵法";
			m_Panel.Line_Three_Text.text = "灵材消耗";
			m_Panel.Line_Three_ValueText.text = $"{m_UpdateDic[m_MatrixStage + 1].MaterialCast}";
			m_Panel.Line_Four_Text.text = "灵矿消耗";
			m_Panel.Line_Four_ValueText.text = $"{m_UpdateDic[m_MatrixStage + 1].ShinyStoneCast}";
			m_Panel.Line_Five_Text.text = "升级用时";
			m_Panel.Line_Five_ValueText.text = $"{m_UpdateDic[m_MatrixStage + 1].TimeCast}";
			m_Panel.Line_Six_Text.text = "升级要求";
			m_Panel.Line_Six_ValueText.text = WorldSetting.GetStageName(m_YanWuTaiLevel) + "演武台";
			m_Panel.QuRengShengJiButton.onClick.AddListener(ZhenFaBut);
		}

		private void ZhenFaBut()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADEYANWUMATRIX, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYanWuMatrixResp_1015033 upgradeYanWuMatrixResp_ = new UpgradeYanWuMatrixResp_1015033();
					upgradeYanWuMatrixResp_.MergeFrom(byteString);
					if (upgradeYanWuMatrixResp_.IsSuccess)
					{
						m_ZhenFaShengJiShengYuTime = upgradeYanWuMatrixResp_.ShengYuTime / 1000;
						m_Time++;
						GuanBiBut();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYanWuMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
				XingJianUpdating();
				ShengJiUpdating();
				FangYuUpdating();
			}
		}

		private void XingJianUpdating()
		{
			if (m_YanWuTaiXingJianShengYuTime > 0)
			{
				m_YanWuTaiXingJianShengYuTime--;
				string text = CommonMethod.SecondToHour((int)m_YanWuTaiXingJianShengYuTime);
				m_Panel.XinJianJiShiButton.gameObject.SetActive(value: true);
				m_Panel.XinJianJiShiText.text = text;
			}
			else
			{
				m_Panel.XinJianJiShiButton.gameObject.SetActive(value: false);
			}
		}

		private void ShengJiUpdating()
		{
			if (m_YanWuTaiShengJiShengYuTime > 0)
			{
				m_YanWuTaiShengJiShengYuTime--;
				string text = CommonMethod.SecondToHour((int)m_YanWuTaiShengJiShengYuTime);
				m_Panel.ShengJiJiShiButton.gameObject.SetActive(value: true);
				m_Panel.ShengJiJiShiText.text = text;
			}
			else
			{
				m_Panel.ShengJiJiShiButton.gameObject.SetActive(value: false);
			}
		}

		private void FangYuUpdating()
		{
			if (m_ZhenFaShengJiShengYuTime > 0)
			{
				m_ZhenFaShengJiShengYuTime--;
				string text = CommonMethod.SecondToHour((int)m_ZhenFaShengJiShengYuTime);
				m_Panel.ShengJiJiShiFangHuButton.gameObject.SetActive(value: true);
				m_Panel.ShengJiJiShiFangHuText.text = text;
			}
			else
			{
				m_Panel.ShengJiJiShiFangHuButton.gameObject.SetActive(value: false);
			}
		}

		private void XinJianJiShi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, (int)m_YanWuTaiXingJianShengYuTime / 60 + 1, new UnityAction<int>(SpeedUpdate));
		}

		private void SpeedUpdate(int castLingYu)
		{
			CreateYanWuTaiJadeBuyTimeReq_1015056 data = new CreateYanWuTaiJadeBuyTimeReq_1015056
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CREATEYANWUTAIJADEBUYTIME, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					CreateYanWuTaiJadeBuyTimeResp_1015056 createYanWuTaiJadeBuyTimeResp_ = new CreateYanWuTaiJadeBuyTimeResp_1015056();
					createYanWuTaiJadeBuyTimeResp_.MergeFrom(byteString);
					if (createYanWuTaiJadeBuyTimeResp_.IsSuccess)
					{
						m_YanWuTaiXingJianShengYuTime = (int)(createYanWuTaiJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_YanWuTaiXingJianShengYuTime <= 0)
						{
							m_YanWuTaiXingJianShengYuTime = 0L;
							XingJianUpdating();
							m_YanWuTaiNum++;
							m_Panel.YanWuTaiNumText.text = $"{m_YanWuTaiNum}";
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createYanWuTaiJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShengJiJiShiFangHu()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, (int)m_YanWuTaiShengJiShengYuTime / 60 + 1, new UnityAction<int>(ShengJiJiShiFangHuSpeedUpdate));
		}

		private void ShengJiJiShiFangHuSpeedUpdate(int castLingYu)
		{
			UpgradeYanWuTaiJadeBuyTimeReq_1015055 data = new UpgradeYanWuTaiJadeBuyTimeReq_1015055
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADEYANWUTAIJADEBUYTIME, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYanWuTaiJadeBuyTimeResp_1015055 upgradeYanWuTaiJadeBuyTimeResp_ = new UpgradeYanWuTaiJadeBuyTimeResp_1015055();
					upgradeYanWuTaiJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeYanWuTaiJadeBuyTimeResp_.IsSuccess)
					{
						m_YanWuTaiShengJiShengYuTime = (int)(upgradeYanWuTaiJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_YanWuTaiShengJiShengYuTime <= 0)
						{
							m_YanWuTaiShengJiShengYuTime = 0L;
							ShengJiUpdating();
							m_YanWuTaiLevel++;
							m_Panel.YanWuTaiText.text = YanWuTaiLevel(m_YanWuTaiLevel) + "演武台";
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYanWuTaiJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShengJiJiShi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, (int)m_ZhenFaShengJiShengYuTime / 60 + 1, new UnityAction<int>(ShengJiJiShiSpeedUpdate));
		}

		private void ShengJiJiShiSpeedUpdate(int castLingYu)
		{
			UpgradeYanWuMatrixJadeBuyTimeReq_1015054 data = new UpgradeYanWuMatrixJadeBuyTimeReq_1015054
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADEYANWUMATRIXJADEBUYTIME, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYanWuMatrixJadeBuyTimeResp_1015054 upgradeYanWuMatrixJadeBuyTimeResp_ = new UpgradeYanWuMatrixJadeBuyTimeResp_1015054();
					upgradeYanWuMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeYanWuMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_ZhenFaShengJiShengYuTime = (int)(upgradeYanWuMatrixJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_ZhenFaShengJiShengYuTime <= 0)
						{
							m_ZhenFaShengJiShengYuTime = 0L;
							ShengJiUpdating();
							m_MatrixStage++;
							m_Panel.MatrixStageText.text = WorldSetting.GetStageName(m_MatrixStage) + "防护阵法";
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYanWuMatrixJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void BiWuTaiXingXi(IntoYanWuResp_1015030 intoYanWuResp1015030)
		{
			m_YanWuTaiLevel = intoYanWuResp1015030.YanWuTaiStage;
			m_Panel.YanWuTaiText.text = YanWuTaiLevel(intoYanWuResp1015030.YanWuTaiStage) + "演武台";
			m_YanWuTaiNum = intoYanWuResp1015030.YanWuTaiNum;
			m_Panel.YanWuTaiNumText.text = $"{intoYanWuResp1015030.YanWuTaiNum}";
			m_MatrixStage = intoYanWuResp1015030.MatrixStage;
			m_Panel.MatrixStageText.text = WorldSetting.GetStageName(intoYanWuResp1015030.MatrixStage) + "防护阵法";
			m_Panel.ZhiSiLvText.text = $"{intoYanWuResp1015030.ZhiSiLv}%";
			m_Panel.JiangDiHunLuanDuText.text = $"{intoYanWuResp1015030.JiangDiHunLuanDu}%";
			m_Panel.TodaySiWangDiZiNumText.text = $"{intoYanWuResp1015030.TodaySiWangDiZiNum}";
			m_YanWuTaiXingJianShengYuTime = intoYanWuResp1015030.CreateYanWuTaiShengYuTime / 1000;
			m_YanWuTaiShengJiShengYuTime = intoYanWuResp1015030.UpgradeYanWuTaiShengYuTime / 1000;
			m_ZhenFaShengJiShengYuTime = intoYanWuResp1015030.UpgradeYanWuTaiMatrixShengYuTime / 1000;
			if (m_YanWuTaiXingJianShengYuTime > 0 || m_YanWuTaiShengJiShengYuTime > 0 || m_ZhenFaShengJiShengYuTime > 0)
			{
				m_Time++;
			}
		}

		private string YanWuTaiLevel(int level)
		{
			return level switch
			{
				3 => "楠木", 
				4 => "铁木", 
				5 => "黑木", 
				6 => "青石", 
				7 => "软金", 
				8 => "玄金", 
				9 => "翠玉", 
				_ => "", 
			};
		}

		private void Special(bool dQSpecial)
		{
			m_Special = dQSpecial;
			m_Panel.Special.isOn = dQSpecial;
			ShuRePanDuan();
		}

		private void XuanZhe(long shu, int fenLei)
		{
			YanWuSuoDingJiangLiReq_1015031 yanWuSuoDingJiangLiReq_ = new YanWuSuoDingJiangLiReq_1015031
			{
				JiangLi1 = m_LingShi,
				JiangLi2 = m_DanYao,
				JiangLi3 = m_FaQi
			};
			if (m_LingShi <= 0)
			{
				yanWuSuoDingJiangLiReq_.JiangLi1 = -1L;
			}
			if (m_DanYao <= 0)
			{
				yanWuSuoDingJiangLiReq_.JiangLi2 = -1;
			}
			if (m_FaQi <= 0)
			{
				yanWuSuoDingJiangLiReq_.JiangLi3 = -1;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.YANWUSUODINGJIANGLI, yanWuSuoDingJiangLiReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					YanWuSuoDingJiangLiResp_1015031 yanWuSuoDingJiangLiResp_ = new YanWuSuoDingJiangLiResp_1015031();
					yanWuSuoDingJiangLiResp_.MergeFrom(byteString);
					if (yanWuSuoDingJiangLiResp_.IsSuccess)
					{
						switch (fenLei)
						{
						case 1:
							LingShiInputField(m_LingShi);
							break;
						case 2:
							DanYaoinInputField(m_DanYao);
							break;
						case 3:
							FaQiInputField(m_FaQi);
							break;
						}
					}
					else
					{
						switch (fenLei)
						{
						case 1:
							LingShiInputField(0L);
							break;
						case 2:
							DanYaoinInputField(0L);
							break;
						case 3:
							FaQiInputField(0L);
							break;
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(yanWuSuoDingJiangLiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					JiSuan();
				}
			}, isShowWaitUI: true);
		}

		private void ShuRePanDuan()
		{
			m_Panel.LingShiImage.gameObject.SetActive(!m_ZhangLaoPanDuan);
			m_Panel.DanYaoinImage.gameObject.SetActive(!m_ZhangLaoPanDuan);
			m_Panel.FaQiImage.gameObject.SetActive(!m_ZhangLaoPanDuan);
		}

		private void FuZhiBangDing(IntoYanWuResp_1015030 intoYanWuResp1015030)
		{
			m_LingShi = intoYanWuResp1015030.JiangLi1;
			m_Panel.LingShiInputField.text = $"{m_LingShi}";
			LingShiText(intoYanWuResp1015030.JiangLi1);
			m_DanYao = intoYanWuResp1015030.JiangLi2;
			m_Panel.DanYaoinInputField.text = $"{m_DanYao}";
			DanYaoinText(intoYanWuResp1015030.JiangLi2);
			m_FaQi = intoYanWuResp1015030.JiangLi3;
			m_Panel.FaQiInputField.text = $"{m_FaQi}";
			FaQiText(intoYanWuResp1015030.JiangLi3);
			m_Panel.LingShiInputField.onEndEdit.AddListener(LingShiInputField);
			m_Panel.DanYaoinInputField.onEndEdit.AddListener(DanYaoinInputField);
			m_Panel.FaQiInputField.onEndEdit.AddListener(FaQiInputField);
			JiSuan();
		}

		private void JiSuan()
		{
			long num = m_LingShi / 1000;
			if (num < 0)
			{
				num = 0L;
			}
			int num2 = m_DanYao / 10;
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = m_FaQi;
			if (num3 < 0)
			{
				num3 = 0;
			}
			long yingXiang = (num + num2 + num3) / m_IntoDaDianResp1015020.Stage;
			JiSuanYingXiang(yingXiang);
		}

		private void JiSuanYingXiang(long yingXiang)
		{
			string text;
			string text2;
			if (yingXiang < 1000)
			{
				text = "毫无兴趣";
				text2 = "0%";
			}
			else if (yingXiang <= 3000)
			{
				text = "略感兴趣";
				text2 = "2%";
			}
			else if (yingXiang <= 10000)
			{
				text = "议论纷纷";
				text2 = "4%";
			}
			else if (yingXiang <= 50000)
			{
				text = "摩拳擦掌";
				text2 = "6%";
			}
			else if (yingXiang <= 100000)
			{
				text = "空前高涨";
				text2 = "8%";
			}
			else
			{
				text = "热血沸腾";
				text2 = "10%";
			}
			m_Panel.MaxYingXiangLiText.text = text;
			m_Panel.MaxZhangDoLiText.text = "宗门战力提升" + text2;
		}

		private void LingShiInputField(string count)
		{
			if (!string.IsNullOrEmpty(m_Panel.LingShiInputField.text))
			{
				long lingShi = m_LingShi;
				string text = m_Panel.LingShiInputField.text;
				m_LingShi = int.Parse(text);
				LingShiText(m_LingShi);
				XuanZhe(lingShi, 1);
			}
		}

		private void LingShiText(long lingShi)
		{
			m_Panel.LingShiText.text = $"{lingShi}";
		}

		private void LingShiInputField(long lingShi)
		{
			m_Panel.LingShiInputField.text = $"{lingShi}";
		}

		private void DanYaoinInputField(string count)
		{
			if (!string.IsNullOrEmpty(m_Panel.DanYaoinInputField.text))
			{
				long lingShi = m_LingShi;
				string text = m_Panel.DanYaoinInputField.text;
				m_DanYao = int.Parse(text);
				DanYaoinText(m_DanYao);
				XuanZhe(lingShi, 2);
			}
		}

		private void DanYaoinText(int danYao)
		{
			m_Panel.DanYaoinText.text = $"{danYao}";
		}

		private void DanYaoinInputField(long danYao)
		{
			m_Panel.DanYaoinInputField.text = $"{danYao}";
		}

		private void FaQiInputField(string count)
		{
			if (!string.IsNullOrEmpty(m_Panel.FaQiInputField.text))
			{
				long lingShi = m_LingShi;
				string text = m_Panel.FaQiInputField.text;
				m_FaQi = int.Parse(text);
				FaQiText(m_FaQi);
				XuanZhe(lingShi, 3);
			}
		}

		private void FaQiText(long faQi)
		{
			m_Panel.FaQiText.text = $"{faQi}";
		}

		private void FaQiInputField(long faQi)
		{
			m_Panel.FaQiInputField.text = $"{faQi}";
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、演武场可以设定每日宗门大比，宗门大比奖励越丰厚，则弟子热情越高，宗门战斗力越强  2、19点的宗门大比过后，结算出的战斗力会作用到20点的兽潮战中 3、比武切磋是解决弟子私怨的地方，演武台的品阶越高、防护阵法越强则弟子的死亡率越低 4、演武台的数量越多，则提高的宗门稳定系数越多";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "演武场");
		}

		private void GuanBiBut()
		{
			m_Panel.QuRengShengJiButton.onClick.RemoveListener(XinJianBut);
			m_Panel.QuRengShengJiButton.onClick.RemoveListener(ZhenFaBut);
			m_Panel.QuRengShengJiButton.onClick.RemoveListener(ShengJiBut);
			m_Panel.XuanZheImage.gameObject.SetActive(value: false);
		}
	}
}
