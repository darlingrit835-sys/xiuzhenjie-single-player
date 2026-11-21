using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_LingTianWnd : Window
	{
		public ZongMen_LingTianPanel Panel;

		private ZongMenShanFengDuty m_HillDuty;

		private string m_HillName;

		private bool m_IsZongZhu;

		private int m_LingTianBuildTimeRemaining;

		private int m_CangKunBuildTimeRemaining;

		private int m_JuLingZhenBuildTimeRemaining;

		private Dictionary<int, ZM_CangKuUpdateEntity> m_CangKuUpdateDic;

		private Dictionary<int, ZM_FuZhiUpdateEntity> m_LingTianUpdateDic;

		private Dictionary<int, ZM_LianDanUpdateEntity> m_JuLingZhenUpdateDic;

		private int m_CurrentZongMenStage = 3;

		private int m_JuLingZhenCurrentStage = 3;

		private int m_LingTianCurrentStage = 3;

		private int m_CangKuCurrentStage = 3;

		private IntoLingTianResp_1015057 m_DataSource;

		private int m_CurrentDiZiNum;

		private int m_CurrentKeYongDiZiNum;

		private float m_Time;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_LingTianPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.Line_One_SureBtn, XiuGaiLianDanDiZiBtnClick);
			AddButtonClickListener(Panel.UpdateLingTianBtn, UpdateLingTianClick);
			AddButtonClickListener(Panel.UpdateCangKuBtn, UpdateCangKuClick);
			AddButtonClickListener(Panel.UpdateJuLingZhenBtn, UpdateJuLingZhenClick);
			AddButtonClickListener(Panel.SpeedUpCangKuBtn, SpeedUpdateCangKuBtnClick);
			AddButtonClickListener(Panel.SpeedUpLingTianBtn, SpeedUpdateLingTianBtnClick);
			AddButtonClickListener(Panel.SpeedUpJuLingZhenButton, SpeedUpdateJuLingZhenBtnClick);
			Panel.Line_One_ValueInput.onEndEdit.AddListener(InputNumChanged);
			m_CangKuUpdateDic = Singleton<FacadeManager>.Instance.SelectCangKuUpdateEntity();
			m_LingTianUpdateDic = Singleton<FacadeManager>.Instance.SelectFuZhiUpdateEntity();
			m_JuLingZhenUpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_HillDuty = ZongMenShanFengDuty.灵田;
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoLingTianResp_1015057;
			m_IsZongZhu = (bool)paralist[2];
			Panel.ShanFengNameText.text = m_HillName;
			m_Time = 0f;
			Panel.ShanFengNameText.text = m_HillName;
			SetDataSource();
		}

		public override void OnClose()
		{
			base.OnClose();
			Panel.Line_One_ValueInput.onEndEdit.RemoveListener(InputNumChanged);
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
				LingTianUpdating();
				JuLingZhenUpdating();
				CangKuUpdating();
			}
		}

		private void SetDataSource()
		{
			if (m_DataSource != null)
			{
				m_CangKuCurrentStage = m_DataSource.SpaceStage;
				m_JuLingZhenCurrentStage = m_DataSource.MatrixStage;
				m_LingTianCurrentStage = m_DataSource.HillStage;
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
				Panel.Line_Five_ValueText.text = $"提升灵植产量{m_DataSource.MatrixVal}%";
				LingTianStartUpgrade((int)(m_DataSource.UpgradeHillShengYuTime / 1000));
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
			Panel.ZhangLaoEffectText.text = $"增加灵植产量：{fuZhiValue}%";
			Panel.ZhangLaoJingJieText.text = WorldSetting.GetRoleLevelNameByRoleLevel(level);
		}

		private void ReflashPanel()
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
						m_DataSource = intoLingTianResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoLingTianResp_.Code);
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
			LingTianDiZiNumReq_1015066 data = new LingTianDiZiNumReq_1015066
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_LingTianDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					LingTianDiZiNumResp_1015066 lingTianDiZiNumResp_ = new LingTianDiZiNumResp_1015066();
					lingTianDiZiNumResp_.MergeFrom(byteString);
					if (lingTianDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(lingTianDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.Line_One_BottomText.text = $"可用弟子数量{lingTianDiZiNumResp_.KeYongDiZiNum}";
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

		private string GetLingTianChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶灵田", 
				4 => "四阶灵田", 
				5 => "五阶灵田", 
				6 => "六阶灵田", 
				7 => "七阶灵田", 
				8 => "八阶灵田", 
				9 => "九阶灵田", 
				_ => "三阶灵田", 
			};
		}

		private void UpdateLingTianClick()
		{
			ZM_FuZhiUpdateEntity value = null;
			if (m_LingTianUpdateDic.TryGetValue(m_LingTianCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add(GetZongMenChineseName(m_LingTianCurrentStage + 1));
				Panel.UpdateLingTianPanel.gameObject.SetActive(value: true);
				Panel.UpdateLingTianPanel.SetValue(list, LingTianSureBtnClick);
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
				list.Add(GetLingTianChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateCangKuPanel.gameObject.SetActive(value: true);
				Panel.UpdateCangKuPanel.SetValue(list, CangKuSureBtnClick);
			}
		}

		private void UpdateJuLingZhenClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_JuLingZhenUpdateDic.TryGetValue(m_JuLingZhenCurrentStage + 1, out value))
			{
				List<string> list = new List<string>();
				list.Add(value.ShinyStoneCast.ToString());
				list.Add(value.MaterialCast.ToString());
				list.Add(value.TimeCast / 60 + "分");
				list.Add($"灵植产量{m_JuLingZhenCurrentStage + 1 - 3}%");
				list.Add(GetLingTianChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateJuLingZhenPanel.gameObject.SetActive(value: true);
				Panel.UpdateJuLingZhenPanel.SetValue(list, JuLingZhenSureBtnClick);
			}
		}

		private void LingTianSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeLingTian, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeLingTianResp_1015064 upgradeLingTianResp_ = new UpgradeLingTianResp_1015064();
					upgradeLingTianResp_.MergeFrom(byteString);
					if (upgradeLingTianResp_.IsSuccess)
					{
						LingTianStartUpgrade((int)(upgradeLingTianResp_.ShengYuTime / 1000));
						Panel.UpdateLingTianPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeLingTianResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LingTianStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpLingTianBtn.gameObject.SetActive(value: true);
				m_LingTianBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpLingTianBtn.gameObject.SetActive(value: false);
				m_LingTianBuildTimeRemaining = 0;
			}
		}

		private void CangKuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UPGRADELINGTIANSPACE, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeLingTianSpaceResp_1015060 upgradeLingTianSpaceResp_ = new UpgradeLingTianSpaceResp_1015060();
					upgradeLingTianSpaceResp_.MergeFrom(byteString);
					if (upgradeLingTianSpaceResp_.IsSuccess)
					{
						CangKuStartUpgrade((int)(upgradeLingTianSpaceResp_.ShengYuTime / 1000));
						Panel.UpdateCangKuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeLingTianSpaceResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CangKuStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpCangKuBtn.gameObject.SetActive(value: true);
				m_CangKunBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpCangKuBtn.gameObject.SetActive(value: false);
			}
		}

		private void JuLingZhenSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UPGRADE_LINGTIAN_MATRIX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeLingTianMatrixResp_1015062 upgradeLingTianMatrixResp_ = new UpgradeLingTianMatrixResp_1015062();
					upgradeLingTianMatrixResp_.MergeFrom(byteString);
					if (upgradeLingTianMatrixResp_.IsSuccess)
					{
						JuLingZhenStartUpgrade((int)(upgradeLingTianMatrixResp_.ShengYuTime / 1000));
						Panel.UpdateJuLingZhenPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeLingTianMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuLingZhenStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpJuLingZhenButton.gameObject.SetActive(value: true);
				m_JuLingZhenBuildTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpJuLingZhenButton.gameObject.SetActive(value: false);
				m_JuLingZhenBuildTimeRemaining = 0;
			}
		}

		private void SpeedUpdateLingTianBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_LingTianBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateLingTian(castLingYu);
			}));
		}

		private void SpeedUpdateLingTian(int CastLingYu)
		{
			UpgradeLingTianJadeBuyTimeReq_1015065 data = new UpgradeLingTianJadeBuyTimeReq_1015065
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeLingTianJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeLingTianJadeBuyTimeResp_1015065 upgradeLingTianJadeBuyTimeResp_ = new UpgradeLingTianJadeBuyTimeResp_1015065();
					upgradeLingTianJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeLingTianJadeBuyTimeResp_.IsSuccess)
					{
						m_LingTianBuildTimeRemaining = (int)(upgradeLingTianJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_LingTianBuildTimeRemaining <= 0)
						{
							m_LingTianBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeLingTianJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateCangKuBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_CangKunBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateLingTian(castLingYu);
			}));
		}

		private void SpeedUpdateCangKu(int CastLingYu)
		{
			UpgradeLingTianSpaceJadeBuyTimeReq_1015061 data = new UpgradeLingTianSpaceJadeBuyTimeReq_1015061
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UPGRADE_LINGTIAN_SPACEJADE_BUYTIME, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeLingTianSpaceJadeBuyTimeResp_1015061 upgradeLingTianSpaceJadeBuyTimeResp_ = new UpgradeLingTianSpaceJadeBuyTimeResp_1015061();
					upgradeLingTianSpaceJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeLingTianSpaceJadeBuyTimeResp_.IsSuccess)
					{
						m_CangKunBuildTimeRemaining = (int)(upgradeLingTianSpaceJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_CangKunBuildTimeRemaining <= 0)
						{
							m_CangKunBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeLingTianSpaceJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateJuLingZhenBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_JuLingZhenBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateLingTian(castLingYu);
			}));
		}

		private void SpeedUpdateJuLingZhen(int CastLingYu)
		{
			UpgradeLingTianMatrixJadeBuyTimeReq_1015063 data = new UpgradeLingTianMatrixJadeBuyTimeReq_1015063
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UPGRADELINGTIANMATRIXJADEBUYTIME, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeLingTianMatrixJadeBuyTimeResp_1015063 upgradeLingTianMatrixJadeBuyTimeResp_ = new UpgradeLingTianMatrixJadeBuyTimeResp_1015063();
					upgradeLingTianMatrixJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeLingTianMatrixJadeBuyTimeResp_.IsSuccess)
					{
						m_JuLingZhenBuildTimeRemaining = (int)(upgradeLingTianMatrixJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_JuLingZhenBuildTimeRemaining <= 0)
						{
							m_JuLingZhenBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeLingTianMatrixJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LingTianUpdating()
		{
			if (m_LingTianBuildTimeRemaining > 0)
			{
				m_LingTianBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_LingTianBuildTimeRemaining);
				Panel.UpdateLingTianOtherTimeText.text = text;
				if (m_LingTianBuildTimeRemaining <= 0)
				{
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
				Panel.UpdateCangKuOtherTimeText.text = text;
				if (m_CangKunBuildTimeRemaining <= 0)
				{
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
				Panel.UpdateJuLingZhenOtherTimeText.text = text;
				if (m_JuLingZhenBuildTimeRemaining <= 0)
				{
					ReflashPanel();
				}
			}
		}
	}
}
