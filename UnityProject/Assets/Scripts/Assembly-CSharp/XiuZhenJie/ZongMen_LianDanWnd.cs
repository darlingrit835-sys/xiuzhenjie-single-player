using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_LianDanWnd : Window
	{
		private ZongMen_LianDanPanel Panel;

		private ZongMenShanFengDuty m_HillDuty;

		private string m_HillName;

		private bool m_IsZongZhu;

		private int m_DiHuoShiBuildTimeRemaining;

		private int m_CangKunBuildTimeRemaining;

		private int m_JuHuoZhenBuildTimeRemaining;

		private float m_Time;

		private Dictionary<int, ZM_CangKuUpdateEntity> m_CangKuUpdateDic;

		private Dictionary<int, ZM_FuZhiUpdateEntity> m_DiHuoShiUpdateDic;

		private Dictionary<int, ZM_LianDanUpdateEntity> m_JuHuoZhenUpdateDic;

		private int m_CurrentZongMenStage = 3;

		private int m_JuHuoZhenCurrentStage = 3;

		private int m_DiHuoShiCurrentStage = 3;

		private int m_CangKuCurrentStage = 3;

		private int m_CurrentDiZiNum;

		private int m_CurrentKeYongDiZiNum;

		private IntoElixirResp_1015076 m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_LianDanPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.UpdateDiHuoShiBtn, UpdateDiHuoShiClick);
			AddButtonClickListener(Panel.UpdateCangKuBtn, UpdateCangKuClick);
			AddButtonClickListener(Panel.UpdateJuHuoZhenBtn, UpdateJuHuoZhenClick);
			AddButtonClickListener(Panel.SpeedUpdateDiHuoShiBtn, SpeedUpdateDiHuoShiBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateCangKuBtn, SpeedUpdateCangKuBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateJuHuoZhenBtn, SpeedUpdateJuHuoZhenBtnClick);
			AddButtonClickListener(Panel.Line_One_SureBtn, XiuGaiLianDanDiZiBtnClick);
			Panel.Line_One_ValueInput.onEndEdit.AddListener(InputNumChanged);
			m_CangKuUpdateDic = Singleton<FacadeManager>.Instance.SelectCangKuUpdateEntity();
			m_DiHuoShiUpdateDic = Singleton<FacadeManager>.Instance.SelectFuZhiUpdateEntity();
			m_JuHuoZhenUpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_HillDuty = ZongMenShanFengDuty.炼丹;
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoElixirResp_1015076;
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
				DiHuoShiUpdating();
				JuHuoZhenUpdating();
				CangKuUpdating();
			}
		}

		private void ReflashPanel()
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
						m_DataSource = intoElixirResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoElixirResp_.Code);
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
			ElixirDiZiNumReq_1015084 data = new ElixirDiZiNumReq_1015084
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_ElixirDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ElixirDiZiNumResp_1015084 elixirDiZiNumResp_ = new ElixirDiZiNumResp_1015084();
					elixirDiZiNumResp_.MergeFrom(byteString);
					if (elixirDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(elixirDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.Line_One_BottomText.text = $"可用弟子数量{elixirDiZiNumResp_.KeYongDiZiNum}";
				}
			}, isShowWaitUI: true);
		}

		private void SetDataSource()
		{
			if (m_DataSource != null)
			{
				m_CangKuCurrentStage = m_DataSource.SpaceStage;
				m_JuHuoZhenCurrentStage = m_DataSource.MatrixStage;
				m_DiHuoShiCurrentStage = m_DataSource.HillStage;
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
				Panel.Line_Five_ValueText.text = $"提升丹药产量{m_DataSource.MatrixVal}%";
				DiHuoShiStartUpgrade((int)(m_DataSource.UpgradeHillShengYuTime / 1000));
				CangKuStartUpgrade((int)(m_DataSource.UpgradeSpaceShengYuTime / 1000));
				JuHuoZhenStartUpgrade((int)(m_DataSource.UpgradeMatrixShengYuTime / 1000));
				ReflashDiZiNum();
			}
		}

		private void SetLeaderInfo(int sex, string headId, string name, string battleValue, int level, int fuZhiValue)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, headId);
			Panel.TouXianImg.sprite = touXiangSprite;
			Panel.ZhangLaoNameText.text = name;
			Panel.ZhangLaoZhanLiText.text = battleValue;
			Panel.ZhangLaoEffectText.text = $"增加丹药产量：{fuZhiValue}%";
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

		private string GetDiHuoShiChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶地火室", 
				4 => "四阶地火室", 
				5 => "五阶地火室", 
				6 => "六阶地火室", 
				7 => "七阶地火室", 
				8 => "八阶地火室", 
				9 => "九阶地火室", 
				_ => "三阶地火室", 
			};
		}

		private void UpdateDiHuoShiClick()
		{
			ZM_FuZhiUpdateEntity value = null;
			if (m_DiHuoShiUpdateDic.TryGetValue(m_DiHuoShiCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add(GetZongMenChineseName(m_DiHuoShiCurrentStage + 1));
				Panel.UpdateDiHuoShiPanel.gameObject.SetActive(value: true);
				Panel.UpdateDiHuoShiPanel.SetValue(list, DiHuoShiSureBtnClick);
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
				list.Add(GetDiHuoShiChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateCangKuPanel.gameObject.SetActive(value: true);
				Panel.UpdateCangKuPanel.SetValue(list, CangKuSureBtnClick);
			}
		}

		private void UpdateJuHuoZhenClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_JuHuoZhenUpdateDic.TryGetValue(m_JuHuoZhenCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add($"灵丹产量{m_JuHuoZhenCurrentStage + 1 - 3}%");
				list.Add(GetDiHuoShiChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateJuHuoZhenPanel.gameObject.SetActive(value: true);
				Panel.UpdateJuHuoZhenPanel.SetValue(list, JuHuoZhenSureBtnClick);
			}
		}

		private void DiHuoShiSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeElixir, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeElixirResp_1015082 upgradeElixirResp_ = new UpgradeElixirResp_1015082();
					upgradeElixirResp_.MergeFrom(byteString);
					if (upgradeElixirResp_.IsSuccess)
					{
						DiHuoShiStartUpgrade((int)(upgradeElixirResp_.ShengYuTime / 1000));
						Panel.UpdateDiHuoShiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DiHuoShiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateDiHuoShiBtn.gameObject.SetActive(value: true);
				m_DiHuoShiBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateDiHuoShiBtn.gameObject.SetActive(value: false);
				m_DiHuoShiBuildTimeRemaining = 0;
			}
		}

		private void CangKuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeElixirSpace, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeElixirSpaceResp_1015078 upgradeElixirSpaceResp_ = new UpgradeElixirSpaceResp_1015078();
					upgradeElixirSpaceResp_.MergeFrom(byteString);
					if (upgradeElixirSpaceResp_.IsSuccess)
					{
						CangKuStartUpgrade((int)(upgradeElixirSpaceResp_.ShengYuTime / 1000));
						Panel.UpdateCangKuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeElixirSpaceResp_.Code);
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

		private void JuHuoZhenSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeElixirMatrix, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeElixirMatrixResp_1015080 upgradeElixirMatrixResp_ = new UpgradeElixirMatrixResp_1015080();
					upgradeElixirMatrixResp_.MergeFrom(byteString);
					if (upgradeElixirMatrixResp_.IsSuccess)
					{
						JuHuoZhenStartUpgrade((int)(upgradeElixirMatrixResp_.ShengYuTime / 1000));
						Panel.UpdateJuHuoZhenPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeElixirMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuHuoZhenStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateJuHuoZhenBtn.gameObject.SetActive(value: true);
				m_JuHuoZhenBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateJuHuoZhenBtn.gameObject.SetActive(value: false);
				m_JuHuoZhenBuildTimeRemaining = 0;
			}
		}

		private void SpeedUpdateDiHuoShiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_DiHuoShiBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateDiHuoShi(castLingYu);
			}));
		}

		private void SpeedUpdateDiHuoShi(int CastLingYu)
		{
			UpgradeElixirJadeBuyTimeReq_1015083 data = new UpgradeElixirJadeBuyTimeReq_1015083
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeElixirJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeElixirJadeBuyTimeResp_1015083 upgradeElixirJadeBuyTimeResp_ = new UpgradeElixirJadeBuyTimeResp_1015083();
					upgradeElixirJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeElixirJadeBuyTimeResp_.IsSuccess)
					{
						m_DiHuoShiBuildTimeRemaining = (int)(upgradeElixirJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_DiHuoShiBuildTimeRemaining <= 0)
						{
							m_DiHuoShiBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeElixirJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateCangKuBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_CangKunBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateDiHuoShi(castLingYu);
			}));
		}

		private void SpeedUpdateCangKu(int CastLingYu)
		{
			UpgradeElixirSpaceJadeBuyTimeReq_1015079 data = new UpgradeElixirSpaceJadeBuyTimeReq_1015079
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeElixirSpaceJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeElixirSpaceJadeBuyTimeResp_1015079 upgradeElixirSpaceJadeBuyTimeResp_ = new UpgradeElixirSpaceJadeBuyTimeResp_1015079();
					upgradeElixirSpaceJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeElixirSpaceJadeBuyTimeResp_.IsSuccess)
					{
						m_CangKunBuildTimeRemaining = (int)(upgradeElixirSpaceJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_CangKunBuildTimeRemaining <= 0)
						{
							m_CangKunBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeElixirSpaceJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateJuHuoZhenBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_JuHuoZhenBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateDiHuoShi(castLingYu);
			}));
		}

		private void SpeedUpdateJuHuoZhen(int CastLingYu)
		{
			UpgradeElixirMatrixJadeBuyTimeReq_1015081 data = new UpgradeElixirMatrixJadeBuyTimeReq_1015081
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeElixirMatrixJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeElixirMatrixJadeBuyTimeResp_1015081 upgradeElixirMatrixJadeBuyTimeResp_ = new UpgradeElixirMatrixJadeBuyTimeResp_1015081();
					upgradeElixirMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeElixirMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_JuHuoZhenBuildTimeRemaining = (int)(upgradeElixirMatrixJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_JuHuoZhenBuildTimeRemaining <= 0)
						{
							m_JuHuoZhenBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeElixirMatrixJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DiHuoShiUpdating()
		{
			if (m_DiHuoShiBuildTimeRemaining > 0)
			{
				m_DiHuoShiBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_DiHuoShiBuildTimeRemaining);
				Panel.SpeedUpdateDiHuoShiBtnTimeText.text = text;
				if (m_DiHuoShiBuildTimeRemaining <= 0)
				{
					m_DiHuoShiBuildTimeRemaining = 0;
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
				Panel.SpeedUpdateCangKuBtnTimeText.text = text;
				if (m_CangKunBuildTimeRemaining <= 0)
				{
					m_CangKunBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void JuHuoZhenUpdating()
		{
			if (m_JuHuoZhenBuildTimeRemaining > 0)
			{
				m_JuHuoZhenBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_JuHuoZhenBuildTimeRemaining);
				Panel.SpeedUpdateJuHuoZhenBtnTimeText.text = text;
				if (m_JuHuoZhenBuildTimeRemaining <= 0)
				{
					m_JuHuoZhenBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}
	}
}
