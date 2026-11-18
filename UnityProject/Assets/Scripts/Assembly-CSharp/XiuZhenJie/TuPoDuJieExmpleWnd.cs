using System;
using UnityEngine;

namespace XiuZhenJie
{
	public class TuPoDuJieExmpleWnd : Window
	{
		public TuPoDuJieExmplePanel Panel;

		private GameObject m_ShanDianObj;

		private int m_ShanDianTimes;

		private int m_CurrentShanDianTimes;

		private double m_ShanDianDamage;

		private bool m_IsCanShanDuo;

		private double m_HuDunZhiValue;

		private double m_MaxSuperAir;

		private double m_CurrentSuperAir;

		private bool m_IsInputFangYu;

		private bool m_IsShanDuoSuccess;

		private double m_CurrentToughTenacity;

		private double m_CurrentPureBlood;

		private bool m_DuJieEnd;

		private double m_NextLuoLeiTime;

		private double m_CurrentLuoLeiTimes;

		private double m_ShanBiCDTime;

		private bool isBegin;

		private BaseSkillModel m_CurrentUsedFaQi;

		private GameObject m_CurrentUsedFaQi_UI;

		private bool DiaoYoug;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<TuPoDuJieExmplePanel>();
			AddButtonClickListener(Panel.ShanDuoBtn, ShanDuo);
			AddButtonClickListener(Panel.EnterLunHuiBtn, EnterLunHuiClick);
			InitData();
		}

		public void BeginDuJie()
		{
			isBegin = true;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!m_DuJieEnd && isBegin)
			{
				AnimatorStateInfo currentAnimatorStateInfo = Panel.LeiYunAnimator.GetCurrentAnimatorStateInfo(0);
				if (Panel.LeiYunAnimator.GetBool("IsLuoLei") && currentAnimatorStateInfo.normalizedTime > 0.01f && currentAnimatorStateInfo.IsName("YunLeiDian"))
				{
					Panel.LeiYunAnimator.SetBool("IsLuoLei", value: false);
					m_IsCanShanDuo = false;
					if (!m_IsShanDuoSuccess)
					{
						FaBaoJianShang(ref m_ShanDianDamage);
						if (!ChengShouShangHai(m_ShanDianDamage))
						{
							m_DuJieEnd = true;
							DuJieFail();
							JiSuanFangYu();
						}
					}
					else
					{
						m_IsShanDuoSuccess = false;
					}
					if (m_CurrentShanDianTimes >= m_ShanDianTimes)
					{
						if (m_CurrentPureBlood < 0.0)
						{
							DuJieFail();
							m_DuJieEnd = true;
							JiSuanFangYu();
							return;
						}
						m_DuJieEnd = true;
						DuJieSuccess();
						JiSuanFangYu();
					}
				}
				JiSuanFangYu();
				m_CurrentLuoLeiTimes += Time.deltaTime;
				if (m_CurrentLuoLeiTimes >= m_NextLuoLeiTime)
				{
					m_CurrentLuoLeiTimes = 0.0;
					m_CurrentShanDianTimes++;
					LuoLeiJiSuan();
					m_NextLuoLeiTime = 4.0;
				}
				if (m_ShanBiCDTime > 0.0)
				{
					m_ShanBiCDTime -= Time.deltaTime;
					Panel.ShanDuoImg.fillAmount = (float)m_ShanBiCDTime / 5f;
				}
				else if (Panel.ShanDuoImg.gameObject.activeSelf)
				{
					Panel.ShanDuoImg.gameObject.SetActive(value: false);
					Panel.ShanDuoBtn.interactable = true;
				}
			}
			if (m_IsInputFangYu && Panel.XuLiPanel.gameObject.activeSelf)
			{
				Panel.XuLiProgressImg.fillAmount += 0.33f * Time.deltaTime;
			}
			BeginDuJie();
		}

		private void InitData()
		{
			m_IsCanShanDuo = false;
			m_IsShanDuoSuccess = false;
			m_HuDunZhiValue = 0.0;
			m_NextLuoLeiTime = 4.0;
			m_ShanDianDamage = 10000.0;
			m_CurrentSuperAir = 71842600.0;
			m_MaxSuperAir = 71842600.0;
			m_ShanDianObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/TuPo_ShanDian.prefab");
			m_ShanDianTimes = 15;
			m_CurrentShanDianTimes = 0;
			m_CurrentToughTenacity = 36785.0;
			m_CurrentPureBlood = m_CurrentToughTenacity / 100.0;
			SetFaQi();
		}

		private bool ChengShouShangHai(double damage)
		{
			double num = damage;
			double num2 = 0.0;
			if (num > 0.0)
			{
				if (num >= m_CurrentToughTenacity)
				{
					num -= m_CurrentToughTenacity;
					num2 += m_CurrentToughTenacity;
					m_CurrentToughTenacity = 0.0;
				}
				else
				{
					m_CurrentToughTenacity -= num;
					num2 += num;
					num = 0.0;
				}
			}
			if (num > 0.0)
			{
				if (num >= m_CurrentPureBlood)
				{
					num -= m_CurrentPureBlood;
					num2 += m_CurrentPureBlood;
					m_CurrentPureBlood = 0.0;
				}
				else
				{
					m_CurrentPureBlood -= num;
					num2 += num;
					num = 0.0;
				}
			}
			MonoSingleton<GameStart>.Instance.PopRollingTextShow(string.Format("承受{0}点雷霆伤害", num2.ToString("f0")));
			if (num > 0.0)
			{
				return false;
			}
			return true;
		}

		private void LuoLeiJiSuan()
		{
			m_IsCanShanDuo = true;
			Panel.LeiYunAnimator.SetBool("IsLuoLei", value: true);
			UnityEngine.Object.Instantiate(m_ShanDianObj, Panel.transform);
			Timer.Register((float)GetTime(), () =>
			{
				m_IsCanShanDuo = false;
			});
		}

		private double GetTime()
		{
			int num = 4;
			int num2 = ((num - 4 <= 0) ? 1 : (num - 4));
			double num3 = 500f * Mathf.Pow(4f, num) / (float)num2;
			num3 /= 36785.0;
			num3 = 4.0 - num3;
			return (num3 < -2.0) ? (-2.0) : (0.20000000298023224 + num3 / 10.0);
		}

		private void ShanDuo()
		{
			if (m_IsCanShanDuo)
			{
				m_IsShanDuoSuccess = true;
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("成功规避了雷霆伤害！");
			}
			else
			{
				m_IsShanDuoSuccess = false;
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("规避雷霆伤害失败！");
			}
			m_ShanBiCDTime = 5.0;
			Panel.ShanDuoImg.gameObject.SetActive(value: true);
			Panel.ShanDuoBtn.interactable = false;
			Panel.ShanDuoImg.fillAmount = 1f;
		}

		private void JiSuanFangYu()
		{
			double num = 0.0;
			m_HuDunZhiValue = num + m_CurrentPureBlood + m_CurrentToughTenacity;
			Panel.FangYuZhiText.text = "防御值:" + m_HuDunZhiValue.ToString("f0");
			Panel.ZhenYuanValue.text = (m_CurrentSuperAir / 100.0).ToString("f0");
			Panel.ZhenYuanProgress.fillAmount = (float)(m_CurrentSuperAir * 1.0 / m_MaxSuperAir);
		}

		private void FaBaoJianShang(ref double ShanDianDamage)
		{
			if (m_CurrentUsedFaQi == null)
			{
				return;
			}
			double num = (long)m_CurrentUsedFaQi.usedSuperAir;
			m_CurrentSuperAir -= num;
			if (num != 0.0 && m_CurrentUsedFaQi != null)
			{
				ShanDianDamage -= m_CurrentUsedFaQi.baseDamage;
				if (ShanDianDamage < 0.0)
				{
					ShanDianDamage = 0.0;
				}
			}
		}

		private void SetFaQi()
		{
			Sprite iconSpr = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_WuQi.png");
			Sprite rankSpr = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			BaseSkillModel dataSource = new BaseSkillModel
			{
				baseDamage = 3000.0,
				fiveElement = 0,
				name = "五行灵剑",
				level = 1,
				rank = 1
			};
			Panel.ZhanDouJiNengItem_1.SetItem(dataSource, "五行灵剑", 0, 1, rankSpr, iconSpr, FaQiClick);
		}

		private void FaQiClick(object data, GameObject gameObj)
		{
			BaseSkillModel baseSkillModel = data as BaseSkillModel;
			if (m_CurrentUsedFaQi != null && m_CurrentUsedFaQi_UI != null)
			{
				ClearUsing(gameObj);
			}
			else if (m_CurrentSuperAir < (double)(long)baseSkillModel.usedSuperAir)
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("灵气不足");
				}
				else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("真气不足");
				}
				else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("真元不足");
				}
			}
			else
			{
				m_CurrentUsedFaQi = baseSkillModel;
				m_CurrentUsedFaQi_UI = gameObj;
				gameObj.GetComponent<UI_GoodsItem>();
				Panel.LineCDItem_1.gameObject.SetActive(value: true);
			}
		}

		private void ClearUsing(GameObject gameObj)
		{
			m_CurrentUsedFaQi = null;
			m_CurrentUsedFaQi_UI = null;
			Transform transform = gameObj.transform.parent.transform.Find("LineAni");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
		}

		private void DuJieSuccess()
		{
			DuJieFail();
		}

		private void DuJieFail()
		{
			Panel.TiShiPanel.gameObject.SetActive(value: true);
		}

		private void EnterLunHuiClick()
		{
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance2 = Singleton<NetManager>.Instance;
			instance2.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Combine(instance2.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance3 = Singleton<NetManager>.Instance;
			instance3.NetErrorEvent = (Action<ServerType>)Delegate.Remove(instance3.NetErrorEvent, new Action<ServerType>(ConnectError));
			NetManager instance4 = Singleton<NetManager>.Instance;
			instance4.NetErrorEvent = (Action<ServerType>)Delegate.Combine(instance4.NetErrorEvent, new Action<ServerType>(ConnectError));
			Singleton<NetManager>.Instance.StartFirstConnect(Constants.SocketServerIP, Constants.SocketServerPort);
			MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
		}

		private void ConnectSuccess(ServerType serverType, bool isRestart)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance2 = Singleton<NetManager>.Instance;
			instance2.NetErrorEvent = (Action<ServerType>)Delegate.Remove(instance2.NetErrorEvent, new Action<ServerType>(ConnectError));
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.PopUpWnd("CreateRoleWnd.prefab", true);
		}

		private void ConnectError(ServerType serverType)
		{
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			NetManager instance = Singleton<NetManager>.Instance;
			instance.ConnectSuccessEvent = (Action<ServerType, bool>)Delegate.Remove(instance.ConnectSuccessEvent, new Action<ServerType, bool>(ConnectSuccess));
			NetManager instance2 = Singleton<NetManager>.Instance;
			instance2.NetErrorEvent = (Action<ServerType>)Delegate.Remove(instance2.NetErrorEvent, new Action<ServerType>(ConnectError));
			MonoSingleton<GameStart>.Instance.PopUpAttention("无法连接到服务器，请稍后再试");
		}
	}
}
