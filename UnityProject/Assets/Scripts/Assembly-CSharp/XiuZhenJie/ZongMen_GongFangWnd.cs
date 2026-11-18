using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class ZongMen_GongFangWnd : Window
	{
		public ZongMen_GongFangPanel Panel;

		private ZongMenShanFengDuty m_HillDuty;

		private string m_HillName;

		private bool m_IsZongZhu;

		private int m_GongFangBuildTimeRemaining;

		private int m_CangKunBuildTimeRemaining;

		private int m_GongJuBuildTimeRemaining;

		private float m_Time;

		private Dictionary<int, ZM_CangKuUpdateEntity> m_CangKuUpdateDic;

		private Dictionary<int, ZM_FuZhiUpdateEntity> m_GongFangUpdateDic;

		private Dictionary<int, ZM_LianDanUpdateEntity> m_GongJuUpdateDic;

		private int m_CurrentZongMenStage = 3;

		private int m_GongJuCurrentStage = 3;

		private int m_GongFangCurrentStage = 3;

		private int m_CangKuCurrentStage = 3;

		private int m_CurrentDiZiNum;

		private int m_CurrentKeYongDiZiNum;

		private IntoGongFangResp_1015103 m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_GongFangPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.UpdateGongFangBtn, UpdateGongFangClick);
			AddButtonClickListener(Panel.UpdateCangKuBtn, UpdateCangKuClick);
			AddButtonClickListener(Panel.UpdateGongJuBtn, UpdateGongJuClick);
			AddButtonClickListener(Panel.SpeedUpdateGongFangBtn, SpeedUpdateGongFangBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateCangKuBtn, SpeedUpdateCangKuBtnClick);
			AddButtonClickListener(Panel.SpeedUpdateGongJuBtn, SpeedUpdateGongJuBtnClick);
			AddButtonClickListener(Panel.Line_One_SureBtn, XiuGaiLianDanDiZiBtnClick);
			Panel.Line_One_ValueInput.onEndEdit.AddListener(InputNumChanged);
			m_CangKuUpdateDic = Singleton<FacadeManager>.Instance.SelectCangKuUpdateEntity();
			m_GongFangUpdateDic = Singleton<FacadeManager>.Instance.SelectFuZhiUpdateEntity();
			m_GongJuUpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_HillDuty = ZongMenShanFengDuty.工坊;
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoGongFangResp_1015103;
			m_IsZongZhu = (bool)paralist[2];
			Panel.ShanFengNameText.text = m_HillName;
			m_Time = 0f;
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
				GongFangUpdating();
				GongJuUpdating();
				CangKuUpdating();
			}
		}

		private void ReflashPanel()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoGongFang, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoGongFangResp_1015103 intoGongFangResp_ = new IntoGongFangResp_1015103();
					intoGongFangResp_.MergeFrom(byteString);
					if (intoGongFangResp_.IsSuccess)
					{
						m_DataSource = intoGongFangResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoGongFangResp_.Code);
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
			GongFangDiZiNumReq_1015111 data = new GongFangDiZiNumReq_1015111
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_GongFangDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GongFangDiZiNumResp_1015111 gongFangDiZiNumResp_ = new GongFangDiZiNumResp_1015111();
					gongFangDiZiNumResp_.MergeFrom(byteString);
					if (gongFangDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(gongFangDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Panel.Line_One_BottomText.text = $"可用弟子数量{gongFangDiZiNumResp_.KeYongDiZiNum}";
				}
			}, isShowWaitUI: true);
		}

		private void SetDataSource()
		{
			if (m_DataSource != null)
			{
				m_CangKuCurrentStage = m_DataSource.SpaceStage;
				m_GongJuCurrentStage = m_DataSource.FaQiStage;
				m_GongFangCurrentStage = m_DataSource.HillStage;
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
				Panel.Line_One_ValueInput.text = m_DataSource.DiZiNum.ToString();
				Panel.Line_One_BottomText.text = $"可用弟子数量{m_DataSource.KeYongDiZiNum}";
				Panel.Line_Two_ValueText.text = m_DataSource.CurrentChanLiang.ToString();
				Panel.Line_Three_ValueText.text = m_DataSource.MaxChanLiang.ToString();
				m_CurrentDiZiNum = m_DataSource.DiZiNum;
				m_CurrentKeYongDiZiNum = m_DataSource.KeYongDiZiNum;
				Panel.Line_Two_ValueText.text = m_DataSource.CurrentChanLiang + "/小时";
				Panel.Line_Three_ValueText.text = m_DataSource.MaxChanLiang + "/小时";
				Panel.Line_Four_ValueText.text = WorldSetting.GetStageName(m_DataSource.FaQiStage);
				Panel.Line_Five_ValueText.text = $"提升灵材产量{m_DataSource.FaQiVal}%";
				GongFangStartUpgrade((int)(m_DataSource.UpgradeHillShengYuTime / 1000));
				CangKuStartUpgrade((int)(m_DataSource.UpgradeSpaceShengYuTime / 1000));
				GongJuStartUpgrade((int)(m_DataSource.UpgradeFaQiShengYuTime / 1000));
				ReflashDiZiNum();
			}
		}

		private void SetLeaderInfo(int sex, string headId, string name, string battleValue, int level, int fuZhiValue)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, headId);
			Panel.TouXianImg.sprite = touXiangSprite;
			Panel.ZhangLaoNameText.text = name;
			Panel.ZhangLaoZhanLiText.text = battleValue;
			Panel.ZhangLaoEffectText.text = $"增加灵材产量：{fuZhiValue}%";
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

		private string GetGongFangChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶工坊", 
				4 => "四阶工坊", 
				5 => "五阶工坊", 
				6 => "六阶工坊", 
				7 => "七阶工坊", 
				8 => "八阶工坊", 
				9 => "九阶工坊", 
				_ => "三阶工坊", 
			};
		}

		private void UpdateGongFangClick()
		{
			ZM_FuZhiUpdateEntity value = null;
			if (m_GongFangUpdateDic.TryGetValue(m_GongFangCurrentStage, out value))
			{
				List<string> list = new List<string>();
				list.Add(m_GongFangUpdateDic[m_GongFangCurrentStage + 1].ShinyStoneCast.ToString());
				list.Add(m_GongFangUpdateDic[m_GongFangCurrentStage + 1].MaterialCast.ToString());
				list.Add(m_GongFangUpdateDic[m_GongFangCurrentStage + 1].TimeCast + "秒");
				list.Add(GetZongMenChineseName(m_GongFangCurrentStage + 1));
				Panel.UpdateGongFangPanel.gameObject.SetActive(value: true);
				Panel.UpdateGongFangPanel.SetValue(list, GongFangSureBtnClick);
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
				list.Add(GetGongFangChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateCangKuPanel.gameObject.SetActive(value: true);
				Panel.UpdateCangKuPanel.SetValue(list, CangKuSureBtnClick);
			}
		}

		private void UpdateGongJuClick()
		{
			ZM_LianDanUpdateEntity value = null;
			if (m_GongJuUpdateDic.TryGetValue(m_GongJuCurrentStage, out value))
			{
				List<string> list = new List<string>();
				list.Add(m_GongJuUpdateDic[m_GongJuCurrentStage + 1].ShinyStoneCast.ToString());
				list.Add(m_GongJuUpdateDic[m_GongJuCurrentStage + 1].MaterialCast.ToString());
				list.Add(m_GongJuUpdateDic[m_GongJuCurrentStage + 1].TimeCast + "秒");
				list.Add($"灵材产量{m_GongJuCurrentStage + 1 - 3}%");
				list.Add(GetGongFangChineseName(m_CangKuCurrentStage + 1));
				Panel.UpdateGongJuPanel.gameObject.SetActive(value: true);
				Panel.UpdateGongJuPanel.SetValue(list, GongJuSureBtnClick);
			}
		}

		private void GongFangSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeGongFang, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGongFangResp_1015109 upgradeGongFangResp_ = new UpgradeGongFangResp_1015109();
					upgradeGongFangResp_.MergeFrom(byteString);
					if (upgradeGongFangResp_.IsSuccess)
					{
						GongFangStartUpgrade((int)(upgradeGongFangResp_.ShengYuTime / 1000));
						Panel.UpdateCangKuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGongFangResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GongFangStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				m_GongFangBuildTimeRemaining = UpgradeSecond;
				Panel.SpeedUpdateGongFangBtn.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.SpeedUpdateGongFangBtn.gameObject.SetActive(value: false);
				m_GongFangBuildTimeRemaining = 0;
			}
		}

		private void CangKuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeGongFangSpace, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGongFangSpaceResp_1015105 upgradeGongFangSpaceResp_ = new UpgradeGongFangSpaceResp_1015105();
					upgradeGongFangSpaceResp_.MergeFrom(byteString);
					if (upgradeGongFangSpaceResp_.IsSuccess)
					{
						CangKuStartUpgrade((int)(upgradeGongFangSpaceResp_.ShengYuTime / 1000));
						Panel.UpdateCangKuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGongFangSpaceResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CangKuStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				m_CangKunBuildTimeRemaining = UpgradeSecond;
				Panel.SpeedUpdateCangKuBtn.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.SpeedUpdateCangKuBtn.gameObject.SetActive(value: false);
			}
		}

		private void GongJuSureBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeGongFangMatrix, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGongFangFaQiResp_1015107 upgradeGongFangFaQiResp_ = new UpgradeGongFangFaQiResp_1015107();
					upgradeGongFangFaQiResp_.MergeFrom(byteString);
					if (upgradeGongFangFaQiResp_.IsSuccess)
					{
						GongJuStartUpgrade((int)(upgradeGongFangFaQiResp_.ShengYuTime / 1000));
						Panel.UpdateGongJuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGongFangFaQiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GongJuStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				m_GongJuBuildTimeRemaining = UpgradeSecond;
				Panel.SpeedUpdateGongJuBtn.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.SpeedUpdateGongJuBtn.gameObject.SetActive(value: false);
				m_GongJuBuildTimeRemaining = 0;
			}
		}

		private void SpeedUpdateGongFangBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_GongFangBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateGongFang(castLingYu);
			}));
		}

		private void SpeedUpdateGongFang(int CastLingYu)
		{
			UpgradeGongFangJadeBuyTimeReq_1015110 data = new UpgradeGongFangJadeBuyTimeReq_1015110
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeGongFangJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGongFangJadeBuyTimeResp_1015110 upgradeGongFangJadeBuyTimeResp_ = new UpgradeGongFangJadeBuyTimeResp_1015110();
					upgradeGongFangJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeGongFangJadeBuyTimeResp_.IsSuccess)
					{
						m_GongFangBuildTimeRemaining = (int)(upgradeGongFangJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_GongFangBuildTimeRemaining <= 0)
						{
							m_GongFangBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGongFangJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SpeedUpdateCangKuBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_CangKunBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateGongFang(castLingYu);
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

		private void SpeedUpdateGongJuBtnClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_GongJuBuildTimeRemaining / 60 + 1, (UnityAction<int>)((int castLingYu) =>
			{
				SpeedUpdateGongJu(castLingYu);
			}));
		}

		private void SpeedUpdateGongJu(int CastLingYu)
		{
			UpgradeGongFangFaQiJadeBuyTimeReq_1015108 data = new UpgradeGongFangFaQiJadeBuyTimeReq_1015108
			{
				Jade = CastLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_UpgradeGongFangMatrixJadeBuyTime, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGongFangFaQiJadeBuyTimeResp_1015108 upgradeGongFangFaQiJadeBuyTimeResp_ = new UpgradeGongFangFaQiJadeBuyTimeResp_1015108();
					upgradeGongFangFaQiJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeGongFangFaQiJadeBuyTimeResp_.IsSuccess)
					{
						m_GongJuBuildTimeRemaining = (int)(upgradeGongFangFaQiJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_GongJuBuildTimeRemaining <= 0)
						{
							m_GongJuBuildTimeRemaining = 0;
							ReflashPanel();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGongFangFaQiJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GongFangUpdating()
		{
			if (m_GongFangBuildTimeRemaining > 0)
			{
				m_GongFangBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_GongFangBuildTimeRemaining);
				Panel.SpeedUpdateGongFangBtnTimeText.text = text;
				if (m_GongFangBuildTimeRemaining <= 0)
				{
					m_GongFangBuildTimeRemaining = 0;
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

		private void GongJuUpdating()
		{
			if (m_GongJuBuildTimeRemaining > 0)
			{
				m_GongJuBuildTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_GongJuBuildTimeRemaining);
				Panel.SpeedUpdateGongJuBtnTimeText.text = text;
				if (m_GongJuBuildTimeRemaining <= 0)
				{
					m_GongJuBuildTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}
	}
}
