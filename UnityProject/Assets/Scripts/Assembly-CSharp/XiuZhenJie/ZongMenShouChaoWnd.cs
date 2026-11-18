using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenShouChaoWnd : Window
	{
		public ZongMenShouChaoPanel Panel;

		private Queue<DanMuItem> m_CanUseDanMuQue;

		private GameObject m_ChatDanMuItem;

		private int m_ZongMenStage;

		private List<int> m_MonsterBattlePowers;

		private int m_3_OnceMonsterBattlePowers = 5000;

		private int m_4_OnceMonsterBattlePowers = 10000;

		private int m_5_OnceMonsterBattlePowers = 20000;

		private int m_6_OnceMonsterBattlePowers = 50000;

		private int m_7_OnceMonsterBattlePowers = 100000;

		private int m_8_OnceMonsterBattlePowers = 500000;

		private int m_9_OnceMonsterBattlePowers = 1000000;

		private int m_CDSeconds;

		private float m_Time;

		private int m_TotalDefence;

		private bool m_IsFinish;

		private Queue<string> m_ReceiveQue;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenShouChaoPanel>();
			m_ReceiveQue = new Queue<string>();
			m_MonsterBattlePowers = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 };
			m_ChatDanMuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DanMuItem.prefab");
			m_CanUseDanMuQue = new Queue<DanMuItem>();
			m_ZongMenStage = int.Parse(paralist[0].ToString());
			AddButtonClickListener(Panel.Help_DuiBtn, DuiBtnClick);
			AddButtonClickListener(Panel.Help_GenBtn, GenBtnClick);
			AddButtonClickListener(Panel.Help_KanBtn, KanBtnClick);
			AddButtonClickListener(Panel.Help_KunBtn, KunBtnClick);
			AddButtonClickListener(Panel.Help_LiBtn, LiBtnClick);
			AddButtonClickListener(Panel.Help_QianBtn, QianBtnClick);
			AddButtonClickListener(Panel.Help_XunBtn, XunBtnClick);
			AddButtonClickListener(Panel.Help_ZhenBtn, ZhenBtnClick);
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.ChouJiangBtn, ChouJiangClick);
			AddButtonClickListener(Panel.QueRenBtn, () =>
			{
				Panel.ShouChaoFinishPanel.gameObject.SetActive(value: false);
			});
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
				if (m_CDSeconds > 0)
				{
					m_CDSeconds--;
					Panel.Help_CD_Text.text = $"支援冷却      {m_CDSeconds}秒";
					if (m_CDSeconds <= 0)
					{
						Panel.Help_PanelMaskImg.gameObject.SetActive(value: false);
					}
				}
			}
			if (m_ReceiveQue.Count > 0)
			{
				CreateChatDanMuItem(m_ReceiveQue.Dequeue());
			}
		}

		private void ChouJiangClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMenChouJiangEventWnd.prefab", true, m_ZongMenStage);
		}

		public void ReflashShouChaoData()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildMonsterFightingBroadcast, null, null);
		}

		private void ExmpleData()
		{
			Panel.Li_MonsterContent.AddMonster(10);
			Timer.Register(10f, () =>
			{
				Panel.Li_MonsterContent.AddMonster(2);
				Panel.Li_MonsterContent.RemoveMonster(8);
				Panel.Kun_MonsterContent.AddMonster(10);
				Timer.Register(10f, () =>
				{
					Panel.Li_MonsterContent.AddMonster(8);
					Panel.Kun_MonsterContent.RemoveMonster(8);
					Panel.Kun_MonsterContent.AddMonster(3);
					Panel.Dui_MonsterContent.AddMonster(10);
					Timer.Register(10f, () =>
					{
						Panel.Dui_MonsterContent.RemoveMonster(5);
						Panel.Dui_MonsterContent.AddMonster(4);
						Panel.Xun_MonsterContent.AddMonster(10);
						Timer.Register(10f, () =>
						{
							Panel.Xun_MonsterContent.AddMonster(5);
							Panel.Zhen_MonsterContent.AddMonster(10);
							Timer.Register(10f, () =>
							{
								Panel.Zhen_MonsterContent.AddMonster(6);
								Panel.Gen_MonsterContent.AddMonster(10);
								Timer.Register(10f, () =>
								{
									Panel.Gen_MonsterContent.AddMonster(7);
									Panel.Kan_MonsterContent.AddMonster(10);
									Timer.Register(10f, () =>
									{
										Panel.Kan_MonsterContent.AddMonster(8);
										Panel.Qian_MonsterContent.AddMonster(10);
										Timer.Register(10f, () =>
										{
											Panel.Qian_MonsterContent.AddMonster(9);
										});
									});
								});
							});
						});
					});
				});
			});
		}

		public void GetData(ByteString byteStr)
		{
			if (byteStr == null)
			{
				return;
			}
			GuildMonsterFightingBroadcastResp_1018002 guildMonsterFightingBroadcastResp_ = new GuildMonsterFightingBroadcastResp_1018002();
			guildMonsterFightingBroadcastResp_.MergeFrom(byteStr);
			if (!guildMonsterFightingBroadcastResp_.IsFinished)
			{
				m_IsFinish = false;
				Panel.ShouChaoMsgText.text = $"第{guildMonsterFightingBroadcastResp_.Times}/9波兽潮来袭";
				int num = 0;
				for (int i = 0; i < guildMonsterFightingBroadcastResp_.Defenses.Count; i++)
				{
					num += guildMonsterFightingBroadcastResp_.Defenses[i];
					GetZhiYuanDefenceText(i).text = $"{guildMonsterFightingBroadcastResp_.Defenses[i]}%";
					GetZhenPanDefenceText(i).text = $"{guildMonsterFightingBroadcastResp_.Defenses[i]}%";
				}
				m_TotalDefence = num;
				Panel.TotalDefenceValueText.text = $"防御值剩余:{num / 8}%";
				for (int j = 0; j < guildMonsterFightingBroadcastResp_.MonsterPowers.Count; j++)
				{
					BattlePowerComputeMonsterNum(j, guildMonsterFightingBroadcastResp_.MonsterPowers[j]);
				}
			}
		}

		private Text GetZhiYuanDefenceText(int index)
		{
			return index switch
			{
				0 => Panel.Li_DefenceValueText, 
				1 => Panel.Kun_DefenceValueText, 
				2 => Panel.Dui_DefenceValueText, 
				3 => Panel.Qian_DefenceValueText, 
				4 => Panel.Kan_DefenceValueText, 
				5 => Panel.Gen_DefenceValueText, 
				6 => Panel.Zhen_DefenceValueText, 
				7 => Panel.Xun_DefenceValueText, 
				_ => Panel.Li_DefenceValueText, 
			};
		}

		private Text GetZhenPanDefenceText(int index)
		{
			return index switch
			{
				0 => Panel.Current_Li_DefenceValueText, 
				1 => Panel.Current_Kun_DefenceValueText, 
				2 => Panel.Current_Dui_DefenceValueText, 
				3 => Panel.Current_Qian_DefenceValueText, 
				4 => Panel.Current_Kan_DefenceValueText, 
				5 => Panel.Current_Gen_DefenceValueText, 
				6 => Panel.Current_Zhen_DefenceValueText, 
				7 => Panel.Current_Xun_DefenceValueText, 
				_ => Panel.Current_Li_DefenceValueText, 
			};
		}

		private MonsterContent GetMonsterContent(int index)
		{
			return index switch
			{
				0 => Panel.Li_MonsterContent, 
				1 => Panel.Kun_MonsterContent, 
				2 => Panel.Dui_MonsterContent, 
				3 => Panel.Qian_MonsterContent, 
				4 => Panel.Kan_MonsterContent, 
				5 => Panel.Gen_MonsterContent, 
				6 => Panel.Zhen_MonsterContent, 
				7 => Panel.Xun_MonsterContent, 
				_ => Panel.Li_MonsterContent, 
			};
		}

		private void LiBtnClick()
		{
			ZhiYuanZhenFa(0);
		}

		private void KunBtnClick()
		{
			ZhiYuanZhenFa(1);
		}

		private void DuiBtnClick()
		{
			ZhiYuanZhenFa(2);
		}

		private void QianBtnClick()
		{
			ZhiYuanZhenFa(3);
		}

		private void KanBtnClick()
		{
			ZhiYuanZhenFa(4);
		}

		private void GenBtnClick()
		{
			ZhiYuanZhenFa(5);
		}

		private void ZhenBtnClick()
		{
			ZhiYuanZhenFa(6);
		}

		private void XunBtnClick()
		{
			ZhiYuanZhenFa(7);
		}

		private void ZhiYuanZhenFa(int index)
		{
			GuildMonsterFightingRoleHelpReq_1018003 data = new GuildMonsterFightingRoleHelpReq_1018003
			{
				HelpIndex = index,
				RoleId = Singleton<RoleManager>.Instance.Role_Entity.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildMonsterFightingRoleHelp, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildMonsterFightingRoleHelpResp_1018003 guildMonsterFightingRoleHelpResp_ = new GuildMonsterFightingRoleHelpResp_1018003();
					guildMonsterFightingRoleHelpResp_.MergeFrom(byteString);
					if (guildMonsterFightingRoleHelpResp_.IsSuccess)
					{
						ReflashShouChaoData();
						m_CDSeconds = 30;
						Panel.Help_PanelMaskImg.gameObject.SetActive(value: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildMonsterFightingRoleHelpResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private int GetCurrentStageOnceMonsterPower()
		{
			return m_ZongMenStage switch
			{
				3 => m_3_OnceMonsterBattlePowers, 
				4 => m_4_OnceMonsterBattlePowers, 
				5 => m_5_OnceMonsterBattlePowers, 
				6 => m_6_OnceMonsterBattlePowers, 
				7 => m_7_OnceMonsterBattlePowers, 
				8 => m_8_OnceMonsterBattlePowers, 
				9 => m_9_OnceMonsterBattlePowers, 
				_ => m_3_OnceMonsterBattlePowers, 
			};
		}

		private void BattlePowerComputeMonsterNum(int index, int newBattlePower)
		{
			MonsterContent monsterContent = GetMonsterContent(index);
			int currentStageOnceMonsterPower = GetCurrentStageOnceMonsterPower();
			int num = m_MonsterBattlePowers[index] / currentStageOnceMonsterPower;
			int num2 = newBattlePower / currentStageOnceMonsterPower;
			if (m_MonsterBattlePowers[index] > newBattlePower)
			{
				if (num - num2 >= 1)
				{
					monsterContent.RemoveMonster(num - num2);
				}
			}
			else if (m_MonsterBattlePowers[index] < newBattlePower && num2 - num >= 1)
			{
				monsterContent.AddMonster(num2 - num);
			}
			m_MonsterBattlePowers[index] = newBattlePower;
		}

		public void ShouChaoFinish(ByteString byteStr)
		{
			GuildMonsterFinishedResp_1018004 guildMonsterFinishedResp_ = new GuildMonsterFinishedResp_1018004();
			guildMonsterFinishedResp_.MergeFrom(byteStr);
			Panel.Li_MonsterContent.FinishShouChao();
			Panel.Kun_MonsterContent.FinishShouChao();
			Panel.Dui_MonsterContent.FinishShouChao();
			Panel.Qian_MonsterContent.FinishShouChao();
			Panel.Kan_MonsterContent.FinishShouChao();
			Panel.Gen_MonsterContent.FinishShouChao();
			Panel.Zhen_MonsterContent.FinishShouChao();
			Panel.Xun_MonsterContent.FinishShouChao();
			m_IsFinish = true;
			Panel.ZhenFaStatus.text = m_TotalDefence / 8 + "%";
			Panel.ShangWangDiZiValue.text = guildMonsterFinishedResp_.DeathCount + "名";
			Panel.JiShaYaoShouValue.text = guildMonsterFinishedResp_.KillCount + "只";
			Panel.ShinyStoneValue.text = guildMonsterFinishedResp_.StoneCount.ToString();
			Panel.ShinyJadeValue.text = guildMonsterFinishedResp_.JadeCount.ToString();
			Panel.LingZhiValue.text = guildMonsterFinishedResp_.LingZhiCount.ToString();
			Panel.LingKuangValue.text = guildMonsterFinishedResp_.LingKuangCount.ToString();
			Panel.YuanCaiValue.text = guildMonsterFinishedResp_.LingCaiCount.ToString();
			Panel.GongXianDianValue.text = guildMonsterFinishedResp_.ChouJiangCishu.ToString();
			Panel.ShouChaoFinishPanel.gameObject.SetActive(value: true);
		}

		public void ReceiveBattleRadio(ByteString byteStr)
		{
			GuildMonsterFightMessageBroadcastResp_1018005 guildMonsterFightMessageBroadcastResp_ = new GuildMonsterFightMessageBroadcastResp_1018005();
			guildMonsterFightMessageBroadcastResp_.MergeFrom(byteStr);
			m_ReceiveQue.Enqueue(guildMonsterFightMessageBroadcastResp_.Message);
		}

		private void Exmpledasdasd()
		{
			m_ReceiveQue.Enqueue("12312312312312312");
		}

		private void CreateChatDanMuItem(string content)
		{
			RectTransform rectTransform = ChooseCanUseDanMuContent();
			if (m_CanUseDanMuQue.Count == 0)
			{
				GameObject gameObject = SetListContentParent(rectTransform, m_ChatDanMuItem);
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				gameObject.GetComponent<DanMuItem>().SetValueNoRole(content, DanMuMoveCompelete, null);
			}
			else
			{
				DanMuItem danMuItem = m_CanUseDanMuQue.Dequeue();
				danMuItem.transform.SetParent(rectTransform);
				danMuItem.ReSetValueNoRole(content, null);
			}
		}

		private void DanMuMoveCompelete(DanMuItem danMuItem)
		{
			danMuItem.transform.SetParent(Panel.DanMuStandByContent);
			m_CanUseDanMuQue.Enqueue(danMuItem);
		}

		private RectTransform ChooseCanUseDanMuContent()
		{
			RectTransform danMuContent_ = Panel.DanMuContent_1;
			Dictionary<float, RectTransform> dictionary = new Dictionary<float, RectTransform>();
			List<float> list = new List<float>();
			if (Panel.DanMuContent_1.childCount == 0)
			{
				return Panel.DanMuContent_1;
			}
			if (Panel.DanMuContent_2.childCount == 0)
			{
				return Panel.DanMuContent_2;
			}
			if (Panel.DanMuContent_3.childCount == 0)
			{
				return Panel.DanMuContent_3;
			}
			if (Panel.DanMuContent_4.childCount == 0)
			{
				return Panel.DanMuContent_4;
			}
			if (Panel.DanMuContent_5.childCount == 0)
			{
				return Panel.DanMuContent_5;
			}
			if (Panel.DanMuContent_6.childCount == 0)
			{
				return Panel.DanMuContent_6;
			}
			if (Panel.DanMuContent_7.childCount == 0)
			{
				return Panel.DanMuContent_7;
			}
			if (Panel.DanMuContent_1.childCount > 0)
			{
				float endDistance = Panel.DanMuContent_1.GetChild(Panel.DanMuContent_1.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance))
				{
					dictionary.Add(endDistance, Panel.DanMuContent_1);
					list.Add(endDistance);
				}
			}
			if (Panel.DanMuContent_2.childCount > 0)
			{
				float endDistance2 = Panel.DanMuContent_2.GetChild(Panel.DanMuContent_2.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance2))
				{
					dictionary.Add(endDistance2, Panel.DanMuContent_2);
					list.Add(endDistance2);
				}
			}
			if (Panel.DanMuContent_3.childCount > 0)
			{
				float endDistance3 = Panel.DanMuContent_3.GetChild(Panel.DanMuContent_3.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance3))
				{
					dictionary.Add(endDistance3, Panel.DanMuContent_3);
					list.Add(endDistance3);
				}
			}
			if (Panel.DanMuContent_4.childCount > 0)
			{
				float endDistance4 = Panel.DanMuContent_4.GetChild(Panel.DanMuContent_4.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance4))
				{
					dictionary.Add(endDistance4, Panel.DanMuContent_4);
					list.Add(endDistance4);
				}
			}
			if (Panel.DanMuContent_5.childCount > 0)
			{
				float endDistance5 = Panel.DanMuContent_5.GetChild(Panel.DanMuContent_5.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance5))
				{
					dictionary.Add(endDistance5, Panel.DanMuContent_5);
					list.Add(endDistance5);
				}
			}
			if (Panel.DanMuContent_6.childCount > 0)
			{
				float endDistance6 = Panel.DanMuContent_6.GetChild(Panel.DanMuContent_6.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance6))
				{
					dictionary.Add(endDistance6, Panel.DanMuContent_6);
					list.Add(endDistance6);
				}
			}
			if (Panel.DanMuContent_7.childCount > 0)
			{
				float endDistance7 = Panel.DanMuContent_7.GetChild(Panel.DanMuContent_7.childCount - 1).GetComponent<DanMuItem>().GetEndDistance();
				if (!dictionary.ContainsKey(endDistance7))
				{
					dictionary.Add(endDistance7, Panel.DanMuContent_7);
					list.Add(endDistance7);
				}
			}
			list.Sort((float right, float left) =>
			{
				if (left > right)
				{
					return 1;
				}
				return (left != right) ? (-1) : 0;
			});
			if (dictionary[list[0]] == null)
			{
				return danMuContent_;
			}
			return dictionary[list[0]];
		}
	}
}
