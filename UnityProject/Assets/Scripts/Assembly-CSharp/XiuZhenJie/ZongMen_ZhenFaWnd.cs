using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_ZhenFaWnd : Window
	{
		public ZongMen_ZhenFaPanel Panel;

		private ZongMenShanFengDuty m_HillDuty;

		private string m_HillName;

		private bool m_IsZongZhu;

		private int m_ZhenFaShiBuildTimeRemaining;

		private int m_CangKunBuildTimeRemaining;

		private int m_JuLingZhenBuildTimeRemaining;

		private float m_Time;

		private Dictionary<int, ZM_CangKuUpdateEntity> m_CangKuUpdateDic;

		private Dictionary<int, ZM_FuZhiUpdateEntity> m_ZhenFaShiUpdateDic;

		private Dictionary<int, ZM_LianDanUpdateEntity> m_JuLingZhenUpdateDic;

		private int m_CurrentZongMenStage = 3;

		private int m_JuLingZhenCurrentStage = 3;

		private int m_ZhenFaShiCurrentStage = 3;

		private int m_CangKuCurrentStage = 3;

		private int m_CurrentDiZiNum;

		private int m_CurrentKeYongDiZiNum;

		private IntoMatrixResp_1015094 m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_ZhenFaPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.UpdateZhenFaShiBtn, UpdateZhenFaShiClick);
			AddButtonClickListener(Panel.UpdateCangKuBtn, UpdateCangKuClick);
			AddButtonClickListener(Panel.UpdateJuLingZhenBtn, UpdateJuLingZhenClick);
			AddButtonClickListener(Panel.SpeedUpdateZhenFaShiBtn, SpeedUpdateZhenFaShiBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateCangKuBtn, SpeedUpdateCangKuBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateJuLingZhenBtn, SpeedUpdateJuLingZhenBtnClick);
			AddButtonClickListener(Panel.Line_One_SureBtn, XiuGaiLianDanDiZiBtnClick);
			Panel.Line_One_ValueInput.onEndEdit.AddListener(InputNumChanged);
			m_CangKuUpdateDic = Singleton<FacadeManager>.Instance.SelectCangKuUpdateEntity();
			m_ZhenFaShiUpdateDic = Singleton<FacadeManager>.Instance.SelectFuZhiUpdateEntity();
			m_JuLingZhenUpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_HillDuty = ZongMenShanFengDuty.阵法;
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoMatrixResp_1015094;
			m_IsZongZhu = (bool)paralist[2];
			Panel.ShanFengNameText.text = m_HillName;
			m_Time = 0f;
			SetDataSource();
		}

		public override void OnClose()
		{
			base.OnClose();
			Panel.Line_One_ValueInput.onEndEdit.RemoveAllListeners();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
				ZhenFaShiUpdating();
				JuLingZhenUpdating();
				CangKuUpdating();
			}
		}

		private void ReflashPanel()
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
						m_DataSource = intoMatrixResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ReflashDiZiNum()
		{
			Panel.Line_One_ValueInput.text = m_CurrentDiZiNum.ToString();
			Panel.Line_One_BottomText.text = $"剩余可用弟子人数{m_CurrentKeYongDiZiNum}";
		}

		private void InputNumChanged(string value)
		{
			int num = int.Parse(value);
			if (num > m_CurrentDiZiNum)
			{
				if (num - m_CurrentDiZiNum > m_CurrentKeYongDiZiNum)
				{
					m_CurrentDiZiNum += m_CurrentKeYongDiZiNum;
					m_CurrentKeYongDiZiNum = 0;
				}
				else
				{
					m_CurrentKeYongDiZiNum -= num - m_CurrentDiZiNum;
					m_CurrentDiZiNum = num;
				}
			}
			else if (num <= m_CurrentDiZiNum && num > 0)
			{
				m_CurrentKeYongDiZiNum += m_CurrentDiZiNum - num;
				m_CurrentDiZiNum = num;
			}
			else if (num < 0)
			{
				m_CurrentKeYongDiZiNum += m_CurrentDiZiNum;
				m_CurrentDiZiNum = 0;
			}
			ReflashDiZiNum();
		}

		private void XiuGaiLianDanDiZiBtnClick()
		{
			MatrixDiZiNumReq_1015102 data = new MatrixDiZiNumReq_1015102
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_MatrixDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					MatrixDiZiNumResp_1015102 matrixDiZiNumResp_ = new MatrixDiZiNumResp_1015102();
					matrixDiZiNumResp_.MergeFrom(byteString);
					if (matrixDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(matrixDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.Line_One_BottomText.text = $"可用弟子数量{matrixDiZiNumResp_.KeYongDiZiNum}";
				}
			}, isShowWaitUI: true);
		}

		private void SetDataSource()
		{
			if (m_DataSource != null)
			{
				m_CangKuCurrentStage = m_DataSource.SpaceStage;
				m_JuLingZhenCurrentStage = m_DataSource.MatrixStage;
				m_ZhenFaShiCurrentStage = m_DataSource.HillStage;
				if (m_DataSource.ZhangLaoInfo != null)
				{
					SetLeaderInfo(m_DataSource.ZhangLaoInfo.Sex ? 1 : 0, m_DataSource.ZhangLaoInfo.RoleImage, m_DataSource.ZhangLaoInfo.RoleName, m_DataSource.ZhangLaoInfo.Fighting.ToString(), m_DataSource.ZhangLaoInfo.Level, m_DataSource.ZhangLaoInfo.TeXiaoVal);
				}
				else
				{
					Panel.WuZhangLaoPanel.gameObject.SetActive(value: true);
				}
				long num = long.Parse(m_CangKuUpdateDic[m_CangKuCurrentStage].MaxYuanCai);
				Panel.CangKuProgresssImg.fillAmount = (float)m_DataSource.KuCun * 1f / (float)num;
				Panel.CangKuValueProgressText.text = ((float)m_DataSource.KuCun * 1f / (float)num * 100f).ToString("0") + "%";
				Panel.CangKuValueText.text = $"{m_DataSource.KuCun}/{m_CangKuUpdateDic[m_CangKuCurrentStage].MaxYuanCai}";
				m_CurrentDiZiNum = m_DataSource.DiZiNum;
				m_CurrentKeYongDiZiNum = m_DataSource.KeYongDiZiNum;
				Panel.Line_Two_ValueText.text = m_DataSource.CurrentChanLiang + "/小时";
				Panel.Line_Three_ValueText.text = m_DataSource.MaxChanLiang + "/小时";
				Panel.Line_Four_ValueText.text = WorldSetting.GetStageName(m_DataSource.MatrixStage);
				Panel.Line_Five_ValueText.text = $"提升阵盘产量{m_DataSource.MatrixVal}%";
				ZhenFaShiStartUpgrade((int)(m_DataSource.UpgradeHillShengYuTime / 1000));
				CangKuStartUpgrade((int)(m_DataSource.UpgradeSpaceShengYuTime / 1000));
				JuLingZhenStartUpgrade((int)(m_DataSource.UpgradeMatrixShengYuTime / 1000));
				ReflashDiZiNum();
			}
		}

		private void SetLeaderInfo(int sex, string headId, string name, string battleValue, int level, int fuZhiValue)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, headId);
			Panel.TouXianImg.sprite = touXiangSprite;
			Panel.ZhangLaoNameText.text = name;
			Panel.ZhangLaoZhanLiText.text = battleValue;
			Panel.ZhangLaoEffectText.text = $"增加阵盘产量：{fuZhiValue}%";
			Panel.ZhangLaoJingJieText.text = WorldSetting.GetRoleLevelNameByRoleLevel(level);
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

		private string GetZhenFaShiChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶阵法室", 
				4 => "四阶阵法室", 
				5 => "五阶阵法室", 
				6 => "六阶阵法室", 
				7 => "七阶阵法室", 
				8 => "八阶阵法室", 
				9 => "九阶阵法室", 
				_ => "三阶阵法室", 
			};
		}

		private void UpdateZhenFaShiClick()
		{
			ZM_FuZhiUpdateEntity value = null;
			if (m_ZhenFaShiUpdateDic.TryGetValue(m_ZhenFaShiCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add(GetZongMenChineseName(m_ZhenFaShiCurrentStage + 1));
				Panel.UpdateZhenFaShiPanel.gameObject.SetActive(value: true);
				Panel.UpdateZhenFaShiPanel.SetValue(list, ZhenFaShiSureBtnClick);
			}
		}

		private void UpdateCangKuClick()
		{
			ZM_CangKuUpdateEntity value = null;
			if (m_CangKuUpdateDic.TryGetValue(m_CangKuCurrentStage, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.MaxShinyStone.ToString());
				list.Add(m_CangKuUpdateDic[m_CangKuCurrentStage + 1].MaxShinyStone.ToString());
				list.Add(value.MaxShinyJade.ToString());
				list.Add(m_CangKuUpdateDic[m_CangKuCurrentStage + 1].MaxShinyJade.ToString());
				list.Add(m_CangKuUpdateDic[m_CangKuCurrentStage + 1].ShinyStoneCast.ToString());
				list.Add(m_CangKuUpdateDic[m_CangKuCurrentStage + 1].MaterialCast.ToString());
				list.Add(long.Parse(m_CangKuUpdateDic[m_CangKuCurrentStage + 1].TimeCast) / 60 + "分");
				list.Add(GetZhenFaShiChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateCangKuPanel.gameObject.SetActive(value: true);
				Panel.UpdateCangKuPanel.SetValue(list, CangKuSureBtnClick);
			}
		}

		private void UpdateJuLingZhenClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_JuLingZhenUpdateDic.TryGetValue(m_JuLingZhenCurrentStage + 1, out value))
			{
				new List<string>
				{
					value.ShinyStoneCast.ToString(),
					value.MaterialCast.ToString(),
					value.TimeCast / 60 + "分",
					$"阵盘产量{m_JuLingZhenCurrentStage + 1 - 3}%",
					GetZhenFaShiChineseName(m_CangKuCurrentStage + 1)
				};
				Panel.UpdateJuLingZhenPanel.gameObject.SetActive(value: true);
				Panel.UpdateJuLingZhenPanel.SetValue(new List<string>(), JuLingZhenSureBtnClick);
			}
		}

		private void ZhenFaShiSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeMatrix, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeMatrixResp_1015100 upgradeMatrixResp_ = new UpgradeMatrixResp_1015100();
					upgradeMatrixResp_.MergeFrom(byteString);
					if (upgradeMatrixResp_.IsSuccess)
					{
						ZhenFaShiStartUpgrade((int)(upgradeMatrixResp_.ShengYuTime / 1000));
						Panel.UpdateZhenFaShiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZhenFaShiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateZhenFaShiBtn.gameObject.SetActive(value: true);
				m_ZhenFaShiBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateZhenFaShiBtn.gameObject.SetActive(value: false);
				m_ZhenFaShiBuildTimeRemaining = 0;
			}
		}

		private void CangKuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeMatrixSpace, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeMatrixSpaceResp_1015096 upgradeMatrixSpaceResp_ = new UpgradeMatrixSpaceResp_1015096();
					upgradeMatrixSpaceResp_.MergeFrom(byteString);
					if (upgradeMatrixSpaceResp_.IsSuccess)
					{
						CangKuStartUpgrade((int)(upgradeMatrixSpaceResp_.ShengYuTime / 1000));
						Panel.UpdateCangKuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeMatrixSpaceResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CangKuStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateCangKuBtn.gameObject.SetActive(value: true);
				m_CangKunBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateCangKuBtn.gameObject.SetActive(value: false);
			}
		}

		private void JuLingZhenSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeMatrixMatrix, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeMatrixMatrixResp_1015098 upgradeMatrixMatrixResp_ = new UpgradeMatrixMatrixResp_1015098();
					upgradeMatrixMatrixResp_.MergeFrom(byteString);
					if (upgradeMatrixMatrixResp_.IsSuccess)
					{
						JuLingZhenStartUpgrade((int)(upgradeMatrixMatrixResp_.ShengYuTime / 1000));
						Panel.UpdateJuLingZhenPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeMatrixMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuLingZhenStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateJuLingZhenBtn.gameObject.SetActive(value: true);
				m_JuLingZhenBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateJuLingZhenBtn.gameObject.SetActive(value: false);
				m_JuLingZhenBuildTimeRemaining = 0;
			}
		}

		private void SpeedUpdateZhenFaShiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_ZhenFaShiBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateZhenFaShi(castLingYu);
			}));
		}

		private void SpeedUpdateZhenFaShi(int CastLingYu)
		{
			UpgradeMatrixMatrixJadeBuyTimeReq_1015099 data = new UpgradeMatrixMatrixJadeBuyTimeReq_1015099
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeMatrixMatrixJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeMatrixMatrixJadeBuyTimeResp_1015099 upgradeMatrixMatrixJadeBuyTimeResp_ = new UpgradeMatrixMatrixJadeBuyTimeResp_1015099();
					upgradeMatrixMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeMatrixMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_ZhenFaShiBuildTimeRemaining = (int)(upgradeMatrixMatrixJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_ZhenFaShiBuildTimeRemaining <= 0)
						{
							m_ZhenFaShiBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeMatrixMatrixJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateCangKuBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_CangKunBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateZhenFaShi(castLingYu);
			}));
		}

		private void SpeedUpdateCangKu(int CastLingYu)
		{
			UpgradeGongFangSpaceJadeBuyTimeReq_1015106 data = new UpgradeGongFangSpaceJadeBuyTimeReq_1015106
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeGongFangSpaceJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGongFangSpaceJadeBuyTimeResp_1015106 upgradeGongFangSpaceJadeBuyTimeResp_ = new UpgradeGongFangSpaceJadeBuyTimeResp_1015106();
					upgradeGongFangSpaceJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeGongFangSpaceJadeBuyTimeResp_.IsSuccess)
					{
						m_CangKunBuildTimeRemaining = (int)(upgradeGongFangSpaceJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_CangKunBuildTimeRemaining <= 0)
						{
							m_CangKunBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGongFangSpaceJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateJuLingZhenBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_JuLingZhenBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateZhenFaShi(castLingYu);
			}));
		}

		private void SpeedUpdateJuLingZhen(int CastLingYu)
		{
			UpgradeMatrixMatrixJadeBuyTimeReq_1015099 data = new UpgradeMatrixMatrixJadeBuyTimeReq_1015099
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeMatrixMatrixJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeMatrixMatrixJadeBuyTimeResp_1015099 upgradeMatrixMatrixJadeBuyTimeResp_ = new UpgradeMatrixMatrixJadeBuyTimeResp_1015099();
					upgradeMatrixMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeMatrixMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_JuLingZhenBuildTimeRemaining = (int)(upgradeMatrixMatrixJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_JuLingZhenBuildTimeRemaining <= 0)
						{
							m_JuLingZhenBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeMatrixMatrixJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZhenFaShiUpdating()
		{
			if (m_ZhenFaShiBuildTimeRemaining > 0)
			{
				m_ZhenFaShiBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_ZhenFaShiBuildTimeRemaining);
				Panel.SpeedUpdateZhenFaShiOtherTimeText.text = text;
				if (m_ZhenFaShiBuildTimeRemaining <= 0)
				{
					m_ZhenFaShiBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void CangKuUpdating()
		{
			if (m_CangKunBuildTimeRemaining > 0)
			{
				m_CangKunBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_CangKunBuildTimeRemaining);
				Panel.SpeedUpdateCangKuOtherTimeText.text = text;
				if (m_CangKunBuildTimeRemaining <= 0)
				{
					m_CangKunBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void JuLingZhenUpdating()
		{
			if (m_JuLingZhenBuildTimeRemaining > 0)
			{
				m_JuLingZhenBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_JuLingZhenBuildTimeRemaining);
				Panel.SpeedUpdateJuLingZhenOtherTimeText.text = text;
				if (m_JuLingZhenBuildTimeRemaining <= 0)
				{
					m_JuLingZhenBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}
	}
}
