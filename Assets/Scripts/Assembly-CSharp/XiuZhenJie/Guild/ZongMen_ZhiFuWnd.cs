using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_ZhiFuWnd : Window
	{
		public ZongMen_ZhiFuPanel Panel;

		private ZongMenShanFengDuty m_HillDuty;

		private string m_HillName;

		private bool m_IsZongZhu;

		private int m_JingShiBuildTimeRemaining;

		private int m_FuZhinBuildTimeRemaining;

		private int m_FuBiBuildTimeRemaining;

		private float m_Time;

		private Dictionary<int, ZM_LianDanUpdateEntity> m_FuZhiAndFuBiUpdateDic;

		private Dictionary<int, ZM_FuZhiUpdateEntity> m_JingShiUpdateDic;

		private int m_CurrentZongMenStage = 3;

		private int m_JingShiCurrentStage = 3;

		private int m_FuBiCurrentStage = 3;

		private int m_FuZhiCurrentStage = 3;

		private int m_CurrentDiZiNum;

		private int m_CurrentKeYongDiZiNum;

		private IntoSignResp_1015112 m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_ZhiFuPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.UpdateFuBiBtn, UpdateFuBiClick);
			AddButtonClickListener(Panel.UpdateFuZhiBtn, UpdateFuZhiClick);
			AddButtonClickListener(Panel.UpdateJingShiBtn, UpdateJingShiClick);
			AddButtonClickListener(Panel.SpeedUpdateFuBiBtn, SpeedUpdateFuBiBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateFuZhiBtn, SpeedUpdateFuZhiBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateJingShiBtn, SpeedUpdateJingShiBtnClick);
			AddButtonClickListener(Panel.Line_One_SureBtn, XiuGaiLianDanDiZiBtnClick);
			Panel.Line_One_ValueInput.onEndEdit.AddListener(InputNumChanged);
			m_FuZhiAndFuBiUpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_JingShiUpdateDic = Singleton<FacadeManager>.Instance.SelectFuZhiUpdateEntity();
			m_HillDuty = ZongMenShanFengDuty.制符;
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoSignResp_1015112;
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
				FuBiUpdating();
				JingShiUpdating();
				FuZhiUpdating();
			}
		}

		private void ReflashPanel()
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
						m_DataSource = intoSignResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoSignResp_.Code);
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
			SignDiZiNumReq_1015120 data = new SignDiZiNumReq_1015120
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_SignDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SignDiZiNumResp_1015120 signDiZiNumResp_ = new SignDiZiNumResp_1015120();
					signDiZiNumResp_.MergeFrom(byteString);
					if (signDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(signDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.Line_One_BottomText.text = $"可用弟子数量{signDiZiNumResp_.KeYongDiZiNum}";
				}
			}, isShowWaitUI: true);
		}

		private void SetDataSource()
		{
			if (m_DataSource != null)
			{
				m_FuZhiCurrentStage = m_DataSource.FuZhiStage;
				m_JingShiCurrentStage = m_DataSource.HillStage;
				m_FuBiCurrentStage = m_DataSource.FuBiStage;
				if (m_DataSource.ZhangLaoInfo != null)
				{
					SetLeaderInfo(m_DataSource.ZhangLaoInfo.Sex ? 1 : 0, m_DataSource.ZhangLaoInfo.RoleImage, m_DataSource.ZhangLaoInfo.RoleName, m_DataSource.ZhangLaoInfo.Fighting.ToString(), m_DataSource.ZhangLaoInfo.Level, m_DataSource.ZhangLaoInfo.TeXiaoVal);
				}
				else
				{
					Panel.WuZhangLaoPanel.gameObject.SetActive(value: true);
				}
				Panel.FuZhiValueText.text = m_DataSource.KuCun + "张";
				m_CurrentDiZiNum = m_DataSource.DiZiNum;
				m_CurrentKeYongDiZiNum = m_DataSource.KeYongDiZiNum;
				Panel.Line_Two_ValueText.text = m_DataSource.CurrentChanLiang + "/小时";
				Panel.Line_Three_ValueText.text = m_DataSource.MaxChanLiang + "/小时";
				Panel.Line_Four_ValueText.text = WorldSetting.GetStageName(m_FuZhiCurrentStage) + "符纸";
				Panel.Line_Five_ValueText.text = WorldSetting.GetStageName(m_FuBiCurrentStage) + "符笔";
				FuBiStartUpgrade((int)(m_DataSource.UpgradeFuBiShengYuTime / 1000));
				FuZhiStartUpgrade((int)(m_DataSource.UpgradeFuZhiShengYuTime / 1000));
				JingShiStartUpgrade((int)(m_DataSource.UpgradeHillShengYuTime / 1000));
				ReflashDiZiNum();
			}
		}

		private void SetLeaderInfo(int sex, string headId, string name, string battleValue, int level, int fuZhiValue)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, headId);
			Panel.TouXianImg.sprite = touXiangSprite;
			Panel.ZhangLaoNameText.text = name;
			Panel.ZhangLaoZhanLiText.text = battleValue;
			Panel.ZhangLaoEffectText.text = $"增加符箓产量：{fuZhiValue}%";
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

		private string GetJingShiChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶静室", 
				4 => "四阶静室", 
				5 => "五阶静室", 
				6 => "六阶静室", 
				7 => "七阶静室", 
				8 => "八阶静室", 
				9 => "九阶静室", 
				_ => "三阶静室", 
			};
		}

		private void UpdateFuBiClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_FuZhiAndFuBiUpdateDic.TryGetValue(m_FuBiCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add(GetJingShiChineseName(m_FuBiCurrentStage + 1));
				list.Add($"符箓产量{m_FuBiCurrentStage + 1 - 3}");
				Panel.UpdateFuBiPanel.gameObject.SetActive(value: true);
				Panel.UpdateFuBiPanel.SetValue(list, FuBiSureBtnClick);
			}
		}

		private void UpdateFuZhiClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_FuZhiAndFuBiUpdateDic.TryGetValue(m_FuBiCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add(GetJingShiChineseName(m_FuZhiCurrentStage + 1));
				list.Add($"符箓产量{m_FuZhiCurrentStage + 1 - 3}");
				Panel.UpdateFuZhiPanel.gameObject.SetActive(value: true);
				Panel.UpdateFuZhiPanel.SetValue(list, FuZhiSureBtnClick);
			}
		}

		private void UpdateJingShiClick()
		{
			ZM_FuZhiUpdateEntity value = null;
			if (m_JingShiUpdateDic.TryGetValue(m_JingShiCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add(GetZongMenChineseName(m_FuZhiCurrentStage + 1));
				Panel.UpdateJingShiPanel.gameObject.SetActive(value: true);
				Panel.UpdateJingShiPanel.SetValue(list, JingShiSureBtnClick);
			}
		}

		private void FuBiSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeFuBi, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeFuBiResp_1015116 upgradeFuBiResp_ = new UpgradeFuBiResp_1015116();
					upgradeFuBiResp_.MergeFrom(byteString);
					if (upgradeFuBiResp_.IsSuccess)
					{
						FuZhiStartUpgrade((int)(upgradeFuBiResp_.ShengYuTime / 1000));
						Panel.UpdateFuZhiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeFuBiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FuBiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateFuBiBtn.gameObject.SetActive(value: true);
				m_FuBiBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateFuBiBtn.gameObject.SetActive(value: false);
				m_FuBiBuildTimeRemaining = 0;
			}
		}

		private void FuZhiSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeFuZhi, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeFuZhiResp_1015114 upgradeFuZhiResp_ = new UpgradeFuZhiResp_1015114();
					upgradeFuZhiResp_.MergeFrom(byteString);
					if (upgradeFuZhiResp_.IsSuccess)
					{
						FuZhiStartUpgrade((int)(upgradeFuZhiResp_.ShengYuTime / 1000));
						Panel.UpdateFuZhiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeFuZhiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FuZhiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateFuZhiBtn.gameObject.SetActive(value: true);
				m_FuZhinBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateFuZhiBtn.gameObject.SetActive(value: false);
			}
		}

		private void JingShiSureBtnClick()
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
						JingShiStartUpgrade((int)(upgradeMatrixResp_.ShengYuTime / 1000));
						Panel.UpdateJingShiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JingShiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpdateJingShiBtn.gameObject.SetActive(value: true);
				m_JingShiBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpdateJingShiBtn.gameObject.SetActive(value: false);
				m_JingShiBuildTimeRemaining = 0;
			}
		}

		private void SpeedUpdateFuBiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_FuBiBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateFuBi(castLingYu);
			}));
		}

		private void SpeedUpdateFuBi(int CastLingYu)
		{
			UpgradeFuBiJadeBuyTimeReq_1015117 data = new UpgradeFuBiJadeBuyTimeReq_1015117
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeFuBiJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeFuBiJadeBuyTimeResp_1015117 upgradeFuBiJadeBuyTimeResp_ = new UpgradeFuBiJadeBuyTimeResp_1015117();
					upgradeFuBiJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeFuBiJadeBuyTimeResp_.IsSuccess)
					{
						m_FuBiBuildTimeRemaining = (int)(upgradeFuBiJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_FuBiBuildTimeRemaining <= 0)
						{
							m_FuBiBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeFuBiJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateFuZhiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_FuZhinBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateFuBi(castLingYu);
			}));
		}

		private void SpeedUpdateFuZhi(int CastLingYu)
		{
			UpgradeFuZhiJadeBuyTimeReq_1015115 data = new UpgradeFuZhiJadeBuyTimeReq_1015115
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeFuZhiJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeFuZhiJadeBuyTimeResp_1015115 upgradeFuZhiJadeBuyTimeResp_ = new UpgradeFuZhiJadeBuyTimeResp_1015115();
					upgradeFuZhiJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeFuZhiJadeBuyTimeResp_.IsSuccess)
					{
						m_FuZhinBuildTimeRemaining = (int)(upgradeFuZhiJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_FuZhinBuildTimeRemaining <= 0)
						{
							m_FuZhinBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeFuZhiJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateJingShiBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_JingShiBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateFuBi(castLingYu);
			}));
		}

		private void SpeedUpdateJingShi(int CastLingYu)
		{
			UpgradeSignJadeBuyTimeReq_1015119 data = new UpgradeSignJadeBuyTimeReq_1015119
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeSignJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeSignJadeBuyTimeResp_1015119 upgradeSignJadeBuyTimeResp_ = new UpgradeSignJadeBuyTimeResp_1015119();
					upgradeSignJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeSignJadeBuyTimeResp_.IsSuccess)
					{
						m_JingShiBuildTimeRemaining = (int)(upgradeSignJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_JingShiBuildTimeRemaining <= 0)
						{
							m_JingShiBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeSignJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FuBiUpdating()
		{
			if (m_FuBiBuildTimeRemaining > 0)
			{
				m_FuBiBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_FuBiBuildTimeRemaining);
				Panel.SpeedUpdateFuBiBtnTimeText.text = text;
				if (m_FuBiBuildTimeRemaining <= 0)
				{
					m_FuBiBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void FuZhiUpdating()
		{
			if (m_FuZhinBuildTimeRemaining > 0)
			{
				m_FuZhinBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_FuZhinBuildTimeRemaining);
				Panel.SpeedUpdateFuZhiBtnTimeText.text = text;
				if (m_FuZhinBuildTimeRemaining <= 0)
				{
					m_FuZhinBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void JingShiUpdating()
		{
			if (m_JingShiBuildTimeRemaining > 0)
			{
				m_JingShiBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_JingShiBuildTimeRemaining);
				Panel.SpeedUpdateJingShiBtnTimeText.text = text;
				if (m_JingShiBuildTimeRemaining <= 0)
				{
					m_JingShiBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}
	}
}
