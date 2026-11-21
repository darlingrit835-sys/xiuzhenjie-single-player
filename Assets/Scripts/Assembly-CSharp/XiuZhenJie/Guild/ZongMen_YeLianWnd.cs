using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_YeLianWnd : Window
	{
		public ZongMen_YeLianPanel Panel;

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

		private IntoYeLianResp_1015067 m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_YeLianPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.UpdateDiHuoShiBtn, UpdateDiHuoShiClick);
			AddButtonClickListener(Panel.UpdateCangKuBtn, UpdateCangKuClick);
			AddButtonClickListener(Panel.UpdateJuHuoZhenBtn, UpdateJuLingZhenClick);
			AddButtonClickListener(Panel.SpeedUpUpdateDiHuoShiBtn, SpeedUpdateDiHuoShiBtnClick);
			AddButtonClickListener(Panel.SpeedUpUpdateCangKuBtn, SpeedUpdateCangKuBtnClick);
			AddButtonClickListener(Panel.SpeedUpUpdateJuHuoZhenBtn, SpeedUpdateJuHuoZhenBtnClick);
			AddButtonClickListener(Panel.Line_One_SureBtn, XiuGaiLianDanDiZiBtnClick);
			Panel.Line_One_ValueInput.onEndEdit.AddListener(InputNumChanged);
			m_CangKuUpdateDic = Singleton<FacadeManager>.Instance.SelectCangKuUpdateEntity();
			m_DiHuoShiUpdateDic = Singleton<FacadeManager>.Instance.SelectFuZhiUpdateEntity();
			m_JuHuoZhenUpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_HillDuty = ZongMenShanFengDuty.冶炼;
			Panel.ShanFengNameText.text = m_HillName;
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoYeLianResp_1015067;
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
						m_DataSource = intoYeLianResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoYeLianResp_.Code);
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
			YeLianDiZiNumReq_1015075 data = new YeLianDiZiNumReq_1015075
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_YeLianDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					YeLianDiZiNumResp_1015075 yeLianDiZiNumResp_ = new YeLianDiZiNumResp_1015075();
					yeLianDiZiNumResp_.MergeFrom(byteString);
					if (yeLianDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(yeLianDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.Line_One_BottomText.text = $"可用弟子数量{yeLianDiZiNumResp_.KeYongDiZiNum}";
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
				Panel.Line_Five_ValueText.text = $"提升灵矿产量{m_DataSource.MatrixVal}%";
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
			Panel.ZhangLaoEffectText.text = $"增加灵矿产量：{fuZhiValue}%";
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
			if (m_DiHuoShiUpdateDic.TryGetValue(m_DiHuoShiCurrentStage, out value))
			{
				List<string> list = new List<string>();
				list.Add(m_DiHuoShiUpdateDic[m_DiHuoShiCurrentStage + 1].ShinyStoneCast.ToString());
				list.Add(m_DiHuoShiUpdateDic[m_DiHuoShiCurrentStage + 1].MaterialCast.ToString());
				list.Add(m_DiHuoShiUpdateDic[m_DiHuoShiCurrentStage + 1].TimeCast + "秒");
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
				list.Add(m_CangKuUpdateDic[m_CangKuCurrentStage + 1].TimeCast + "秒");
				list.Add(GetDiHuoShiChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateCangKuPanel.gameObject.SetActive(value: true);
				Panel.UpdateCangKuPanel.SetValue(list, CangKuSureBtnClick);
			}
		}

		private void UpdateJuLingZhenClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_JuHuoZhenUpdateDic.TryGetValue(m_JuHuoZhenCurrentStage, out value))
			{
				List<string> list = new List<string>();
				list.Add(m_JuHuoZhenUpdateDic[m_JuHuoZhenCurrentStage + 1].ShinyStoneCast.ToString());
				list.Add(m_JuHuoZhenUpdateDic[m_JuHuoZhenCurrentStage + 1].MaterialCast.ToString());
				list.Add(m_JuHuoZhenUpdateDic[m_JuHuoZhenCurrentStage + 1].TimeCast + "秒");
				list.Add($"灵矿产量{m_JuHuoZhenCurrentStage + 1 - 3}%");
				list.Add(GetDiHuoShiChineseName(m_JuHuoZhenCurrentStage + 1));
				Panel.UpdateJuHuoZhenPanel.gameObject.SetActive(value: true);
				Panel.UpdateJuHuoZhenPanel.SetValue(list, JuHuoZhenSureBtnClick);
			}
		}

		private void DiHuoShiSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeYeLian, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYeLianResp_1015073 upgradeYeLianResp_ = new UpgradeYeLianResp_1015073();
					upgradeYeLianResp_.MergeFrom(byteString);
					if (upgradeYeLianResp_.IsSuccess)
					{
						DiHuoShiStartUpgrade((int)(upgradeYeLianResp_.ShengYuTime / 1000));
						Panel.UpdateDiHuoShiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYeLianResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DiHuoShiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpUpdateDiHuoShiBtn.gameObject.SetActive(value: true);
				m_DiHuoShiBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpUpdateDiHuoShiBtn.gameObject.SetActive(value: false);
				m_DiHuoShiBuildTimeRemaining = 0;
			}
		}

		private void CangKuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeYeLianSpace, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYeLianSpaceResp_1015069 upgradeYeLianSpaceResp_ = new UpgradeYeLianSpaceResp_1015069();
					upgradeYeLianSpaceResp_.MergeFrom(byteString);
					if (upgradeYeLianSpaceResp_.IsSuccess)
					{
						CangKuStartUpgrade((int)(upgradeYeLianSpaceResp_.ShengYuTime / 1000));
						Panel.UpdateCangKuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYeLianSpaceResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CangKuStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpUpdateCangKuBtn.gameObject.SetActive(value: true);
				m_CangKunBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpUpdateCangKuBtn.gameObject.SetActive(value: false);
			}
		}

		private void JuHuoZhenSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeYeLianMatrix, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYeLianMatrixResp_1015071 upgradeYeLianMatrixResp_ = new UpgradeYeLianMatrixResp_1015071();
					upgradeYeLianMatrixResp_.MergeFrom(byteString);
					if (upgradeYeLianMatrixResp_.IsSuccess)
					{
						JuHuoZhenStartUpgrade((int)(upgradeYeLianMatrixResp_.ShengYuTime / 1000));
						Panel.UpdateJuHuoZhenPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYeLianMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuHuoZhenStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpUpdateJuHuoZhenBtn.gameObject.SetActive(value: true);
				m_JuHuoZhenBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpUpdateJuHuoZhenBtn.gameObject.SetActive(value: false);
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
			UpgradeYeLianJadeBuyTimeReq_1015074 data = new UpgradeYeLianJadeBuyTimeReq_1015074
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeYeLianJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYeLianJadeBuyTimeResp_1015074 upgradeYeLianJadeBuyTimeResp_ = new UpgradeYeLianJadeBuyTimeResp_1015074();
					upgradeYeLianJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeYeLianJadeBuyTimeResp_.IsSuccess)
					{
						m_DiHuoShiBuildTimeRemaining = (int)(upgradeYeLianJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_DiHuoShiBuildTimeRemaining <= 0)
						{
							m_DiHuoShiBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYeLianJadeBuyTimeResp_.Code);
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
			UpgradeYeLianSpaceJadeBuyTimeReq_1015070 data = new UpgradeYeLianSpaceJadeBuyTimeReq_1015070
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeYeLianSpaceJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYeLianSpaceJadeBuyTimeResp_1015070 upgradeYeLianSpaceJadeBuyTimeResp_ = new UpgradeYeLianSpaceJadeBuyTimeResp_1015070();
					upgradeYeLianSpaceJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeYeLianSpaceJadeBuyTimeResp_.IsSuccess)
					{
						m_CangKunBuildTimeRemaining = (int)(upgradeYeLianSpaceJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_CangKunBuildTimeRemaining <= 0)
						{
							m_CangKunBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYeLianSpaceJadeBuyTimeResp_.Code);
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
			UpgradeYeLianMatrixJadeBuyTimeReq_1015072 data = new UpgradeYeLianMatrixJadeBuyTimeReq_1015072
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeYeLianMatrixJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeYeLianMatrixJadeBuyTimeResp_1015072 upgradeYeLianMatrixJadeBuyTimeResp_ = new UpgradeYeLianMatrixJadeBuyTimeResp_1015072();
					upgradeYeLianMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeYeLianMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_JuHuoZhenBuildTimeRemaining = (int)(upgradeYeLianMatrixJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_JuHuoZhenBuildTimeRemaining <= 0)
						{
							m_JuHuoZhenBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeYeLianMatrixJadeBuyTimeResp_.Code);
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
