using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeLianZhiBuZhouSheZhiWnd : Window
	{
		public DiHuoGeLianZhiBuZhouSheZhiPanel Panel;

		private int m_CurrentDengJie;

		private int m_CurrentStepNum;

		private int m_EndTimes;

		private int m_TouZhiTimes;

		private List<KaiShiLianZhiStepModel> m_AddStepList;

		private List<LianZhiBuZhouSheZhiItem> m_StepItemObjList;

		private bool m_IsOpenRest;

		private int m_ChooseIndex;

		private GameObject m_StepItemObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_EndTimes = 0;
			m_TouZhiTimes = 0;
			m_ChooseIndex = 0;
			m_IsOpenRest = false;
			if (paralist.Length != 0)
			{
				int.TryParse(paralist[0].ToString(), out m_CurrentDengJie);
			}
			Panel = base.GameObject.GetComponent<DiHuoGeLianZhiBuZhouSheZhiPanel>();
			m_StepItemObjList = new List<LianZhiBuZhouSheZhiItem>();
			AddButtonClickListener(Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(Panel.KaiShiLianZhiBUtton, KaiShiLianZhi);
			AddButtonClickListener(Panel.LeftNingYeBtn, NingYeBtnClick);
			AddButtonClickListener(Panel.RightNingYeBtn, NingYeBtnClick);
			AddButtonClickListener(Panel.LeftKongHuoBtn, KongHuoBtnClick);
			AddButtonClickListener(Panel.RightKongHuoBtn, KongHuoBtnClick);
			AddButtonClickListener(Panel.LeftTouZhiBtn, TouZhiCaoYaoClick);
			AddButtonClickListener(Panel.RightTouZhiBtn, TouZhiCaoYaoClick);
			AddButtonClickListener(Panel.LeftShouDanBtn, ShouDanClick);
			AddButtonClickListener(Panel.RightShouDanBtn, ShouDanClick);
			AddButtonClickListener(Panel.LeftCancleBtn, () =>
			{
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			});
			AddButtonClickListener(Panel.RightCancleBtn, () =>
			{
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			});
			AddButtonClickListener(Panel.AddStepBtn, AddStep);
			m_AddStepList = new List<KaiShiLianZhiStepModel>();
			m_AddStepList.Add(new KaiShiLianZhiStepModel
			{
				a = 1,
				d = 1
			});
			m_CurrentStepNum = 1;
			m_StepItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/LianZhiBuZhouSheZhiItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ResetRectContentHight();
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.PopUpWnd("ChuangZhao_LianZhiDengJieWnd.prefab", true);
		}

		private void KaiShiLianZhi()
		{
			VerifyCreateMethodElixirReq_1008018 verifyCreateMethodElixirReq_ = new VerifyCreateMethodElixirReq_1008018
			{
				Stage = m_CurrentDengJie
			};
			KaiShiLianZhiModel kaiShiLianZhiModel = new KaiShiLianZhiModel();
			kaiShiLianZhiModel.b = m_CurrentDengJie;
			kaiShiLianZhiModel.a = m_AddStepList;
			m_TouZhiTimes = 0;
			foreach (KaiShiLianZhiStepModel item in kaiShiLianZhiModel.a)
			{
				ChuangZhaoStepType a = (ChuangZhaoStepType)item.a;
				Debug.Log(a.ToString() + "order:" + item.d);
				VerifyCreateMethodElixirReq_1008018.Types.ElixirStepObjectReq_1008062 elixirStepObjectReq_ = new VerifyCreateMethodElixirReq_1008018.Types.ElixirStepObjectReq_1008062();
				elixirStepObjectReq_.ActionType = item.a;
				elixirStepObjectReq_.Order = item.d;
				elixirStepObjectReq_.TargetId = (string.IsNullOrEmpty(item.b) ? "" : item.b);
				if (item.c == 3)
				{
					elixirStepObjectReq_.TargetType = 12;
				}
				else
				{
					elixirStepObjectReq_.TargetType = item.c;
				}
				verifyCreateMethodElixirReq_.Object.Add(elixirStepObjectReq_);
				if (!string.IsNullOrEmpty(item.b))
				{
					m_TouZhiTimes++;
				}
			}
			if (m_EndTimes > 1)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("只允许有一个“收丹”步骤！");
				return;
			}
			if (m_EndTimes <= 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("必须有“收丹”步骤！");
				return;
			}
			if (m_AddStepList == null || m_AddStepList.Count == 0 || m_AddStepList[m_AddStepList.Count - 1].a != 5)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("“收丹”必须是末尾步骤！");
				return;
			}
			if (m_AddStepList == null || m_AddStepList.Count == 0 || m_AddStepList[m_AddStepList.Count - 2].a != 7)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("“凝液”必须在“收丹”前一步！");
				return;
			}
			if (m_TouZhiTimes > kaiShiLianZhiModel.b + 4)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention($"当前等阶下,投掷材料步骤数量最多为{kaiShiLianZhiModel.b + 4}步");
				return;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentClever < m_TouZhiTimes)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前悟性不足!至少需要" + m_TouZhiTimes + "点悟性!");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_CREATE_DANFANG, verifyCreateMethodElixirReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCreateMethodElixirResp_1008018 verifyCreateMethodElixirResp_ = new VerifyCreateMethodElixirResp_1008018();
					verifyCreateMethodElixirResp_.MergeFrom(byteString);
					if (verifyCreateMethodElixirResp_.IsSuccess)
					{
						ReceiveChuangZaoResult(verifyCreateMethodElixirResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ReceiveChuangZaoResult(VerifyCreateMethodElixirResp_1008018 verifyCreateMethodElixirResp_1008018)
		{
			new CreateMethodElixirRefiningResultReq_1008020
			{
				IsSuccess = false,
				MethodElixirId = verifyCreateMethodElixirResp_1008018.MethodElixirId
			};
			DanFangModel danFangModel = new DanFangModel
			{
				ei = verifyCreateMethodElixirResp_1008018.MethodElixirId,
				c = m_CurrentDengJie,
				bb = new List<DanFangStepModel>()
			};
			foreach (VerifyCreateMethodElixirResp_1008018.Types.ElixirStepObjectResp_1008019 item2 in verifyCreateMethodElixirResp_1008018.Object)
			{
				DanFangStepModel item = new DanFangStepModel
				{
					actionType = item2.ActionType,
					continuetime = item2.ContinueTime,
					bestVal = item2.BestVal,
					targetType = item2.TargetType,
					maxVal = item2.MaxVal,
					minVal = item2.MinVal,
					order = item2.Order,
					valType = item2.ValType,
					id = item2.Order.ToString()
				};
				danFangModel.bb.Add(item);
				danFangModel.f = verifyCreateMethodElixirResp_1008018.Rank;
			}
			if (!verifyCreateMethodElixirResp_1008018.CreatedIsSuccess)
			{
				string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodElixirResp_1008018.Code);
				Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodElixirResp_1008018.CurrentClever;
				Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoFail, danFangModel, hintMsgByCode);
			}
			else
			{
				danFangModel.r = verifyCreateMethodElixirResp_1008018.Rank;
				CreateSuccess(danFangModel);
			}
			Singleton<UIManager>.Instance.HideWnd(this);
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: false);
		}

		private void CreateSuccess(DanFangModel danFangModel)
		{
			CreateMethodElixirRefiningResultReq_1008020 data = new CreateMethodElixirRefiningResultReq_1008020
			{
				IsSuccess = true,
				MethodElixirId = danFangModel.ei
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_CREATE_DANFANG_RESULT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					CreateMethodElixirRefiningResultResp_1008020 createMethodElixirRefiningResultResp_ = new CreateMethodElixirRefiningResultResp_1008020();
					createMethodElixirRefiningResultResp_.MergeFrom(byteString);
					if (createMethodElixirRefiningResultResp_.IsSuccess)
					{
						List<string> danYaoXiangQing = GetDanYaoXiangQing(createMethodElixirRefiningResultResp_);
						danFangModel.aa = danYaoXiangQing;
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(createMethodElixirRefiningResultResp_.LiveLevel, createMethodElixirRefiningResultResp_.LiveProficiency, RoleFuZhiType.LianDan);
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = createMethodElixirRefiningResultResp_.CurrentClever;
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoSucess, danFangModel);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createMethodElixirRefiningResultResp_.Code);
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoFail, danFangModel, hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private List<string> GetDanYaoXiangQing(CreateMethodElixirRefiningResultResp_1008020 data)
		{
			List<string> list = new List<string>();
			switch ((DanYaoEffect)data.EffectType)
			{
			case DanYaoEffect.None:
				list.Add(string.Format("{0},{1}", "丹药类型", "未知"));
				break;
			case DanYaoEffect.HUIFU_TIPO:
				list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-体魄"));
				break;
			case DanYaoEffect.HUIFU_SHENSHI:
				list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-神识"));
				break;
			case DanYaoEffect.HUIFU_ZHENYUAN:
				if (data.ElixirLevel < 31)
				{
					list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-灵气"));
				}
				else if (data.ElixirLevel >= 31 && data.ElixirLevel < 61)
				{
					list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-真气"));
				}
				else if (data.ElixirLevel >= 61)
				{
					list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-真元"));
				}
				break;
			case DanYaoEffect.HUIFU_JINGXUE:
				list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-精血"));
				break;
			case DanYaoEffect.HUIFU_SHOUYUAN:
				list.Add(string.Format("{0},{1}", "丹药类型", "恢复类-寿元"));
				break;
			case DanYaoEffect.ZENGZHANG_TIPO:
				list.Add(string.Format("{0},{1}", "丹药类型", "增长类-体魄"));
				break;
			case DanYaoEffect.ZENGZHANG_SHENSHI:
				list.Add(string.Format("{0},{1}", "丹药类型", "增长类-神识"));
				break;
			case DanYaoEffect.ZENGZHANG_ZHENYUAN:
				if (data.ElixirLevel < 31)
				{
					list.Add(string.Format("{0},{1}", "丹药类型", "增长类-灵气"));
				}
				else if (data.ElixirLevel >= 31 && data.ElixirLevel < 61)
				{
					list.Add(string.Format("{0},{1}", "丹药类型", "增长类-真气"));
				}
				else if (data.ElixirLevel >= 61)
				{
					list.Add(string.Format("{0},{1}", "丹药类型", "增长类-真元"));
				}
				break;
			case DanYaoEffect.ZENGZHANG_JINGXUE:
				list.Add(string.Format("{0},{1}", "丹药类型", "增长类-精血"));
				break;
			case DanYaoEffect.ZENGZHANG_SHOUYUAN:
				list.Add(string.Format("{0},{1}", "丹药类型", "增长类-寿元"));
				break;
			case DanYaoEffect.ZENGZHANG_JINGMAI:
				list.Add(string.Format("{0},{1}", "丹药类型", "增长类-经脉"));
				break;
			case DanYaoEffect.TESHU_JINGLI:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-增长精力"));
				break;
			case DanYaoEffect.TESHU_WUXING:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-增长悟性"));
				break;
			case DanYaoEffect.TESHU_JIEDU:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-解毒"));
				break;
			case DanYaoEffect.TESHU_JINGJIETUPO_ZHUJI:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-提升筑基成功率"));
				break;
			case DanYaoEffect.TESHU_JINGJIETUPO_LEIJIE:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-降低雷劫伤害"));
				break;
			case DanYaoEffect.TESHU_LINGGEN:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-补一种五行灵根"));
				break;
			case DanYaoEffect.TESHU_JINGMAI:
				list.Add(string.Format("{0},{1}", "丹药类型", "特殊类-修补受损经脉"));
				break;
			}
			return list;
		}

		private void AddStep()
		{
			m_IsOpenRest = false;
			RectTransform component = Panel.AddStepBtn.gameObject.GetComponent<RectTransform>();
			Panel.ScrollView.gameObject.GetComponent<RectTransform>();
			_ = Panel.ScrollViewContent.sizeDelta;
			_ = Panel.ScrollView.gameObject.GetComponent<RectTransform>().sizeDelta;
			if (component.anchoredPosition.x < 100f)
			{
				float x = component.position.x + 0.4f;
				float y = component.position.y - 1.6f;
				ShowStepPanel(isLeft: true, x, y);
			}
			else
			{
				float x2 = component.position.x - 0.4f;
				float y2 = component.position.y - 1.6f;
				ShowStepPanel(isLeft: false, x2, y2);
			}
		}

		private void ReSetStep(RectTransform targetItem)
		{
			m_IsOpenRest = true;
			Panel.ScrollView.gameObject.GetComponent<RectTransform>();
			_ = Panel.ScrollViewContent.sizeDelta;
			_ = Panel.ScrollView.gameObject.GetComponent<RectTransform>().sizeDelta;
			if (targetItem.anchoredPosition.x < 100f)
			{
				float x = targetItem.position.x + 0.4f;
				float y = targetItem.position.y - 1.6f;
				ShowStepPanel(isLeft: true, x, y);
			}
			else
			{
				float x2 = targetItem.position.x - 0.4f;
				float y2 = targetItem.position.y - 1.6f;
				ShowStepPanel(isLeft: false, x2, y2);
			}
		}

		private void NingYeBtnClick()
		{
			if (m_IsOpenRest)
			{
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel = null;
				for (int i = 0; i < m_AddStepList.Count; i++)
				{
					if (i == m_ChooseIndex - 1)
					{
						kaiShiLianZhiStepModel = m_AddStepList[i];
						break;
					}
				}
				LianZhiBuZhouSheZhiItem lianZhiBuZhouSheZhiItem = null;
				for (int j = 0; j < m_StepItemObjList.Count; j++)
				{
					if (j == m_ChooseIndex - 2)
					{
						lianZhiBuZhouSheZhiItem = m_StepItemObjList[j];
						break;
					}
				}
				if (kaiShiLianZhiStepModel.a == 7)
				{
					ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
					return;
				}
				if (kaiShiLianZhiStepModel != null && lianZhiBuZhouSheZhiItem != null)
				{
					if (kaiShiLianZhiStepModel.a == 5)
					{
						m_EndTimes = ((m_EndTimes >= 1) ? (m_EndTimes - 1) : 0);
					}
					kaiShiLianZhiStepModel.a = 7;
					kaiShiLianZhiStepModel.d = m_ChooseIndex;
					kaiShiLianZhiStepModel.b = "";
					lianZhiBuZhouSheZhiItem.ResetValue("凝液");
				}
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			}
			else
			{
				InstanceItemObj(ChuangZhaoStepType.NingYe);
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel2 = new KaiShiLianZhiStepModel();
				kaiShiLianZhiStepModel2.a = 7;
				kaiShiLianZhiStepModel2.d = m_CurrentStepNum;
				kaiShiLianZhiStepModel2.b = "";
				m_AddStepList.Add(kaiShiLianZhiStepModel2);
			}
		}

		private void KongHuoBtnClick()
		{
			if (m_IsOpenRest)
			{
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel = null;
				for (int i = 0; i < m_AddStepList.Count; i++)
				{
					if (i == m_ChooseIndex - 1)
					{
						kaiShiLianZhiStepModel = m_AddStepList[i];
						break;
					}
				}
				LianZhiBuZhouSheZhiItem lianZhiBuZhouSheZhiItem = null;
				for (int j = 0; j < m_StepItemObjList.Count; j++)
				{
					if (j == m_ChooseIndex - 2)
					{
						lianZhiBuZhouSheZhiItem = m_StepItemObjList[j];
						break;
					}
				}
				if (kaiShiLianZhiStepModel.a == 6)
				{
					ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
					return;
				}
				if (kaiShiLianZhiStepModel != null && lianZhiBuZhouSheZhiItem != null)
				{
					if (kaiShiLianZhiStepModel.a == 5)
					{
						m_EndTimes = ((m_EndTimes >= 1) ? (m_EndTimes - 1) : 0);
					}
					kaiShiLianZhiStepModel.a = 6;
					kaiShiLianZhiStepModel.d = m_ChooseIndex;
					kaiShiLianZhiStepModel.b = "";
					lianZhiBuZhouSheZhiItem.ResetValue("控火");
				}
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			}
			else
			{
				InstanceItemObj(ChuangZhaoStepType.KongHuo);
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel2 = new KaiShiLianZhiStepModel();
				kaiShiLianZhiStepModel2.a = 6;
				kaiShiLianZhiStepModel2.b = "";
				kaiShiLianZhiStepModel2.d = m_CurrentStepNum;
				m_AddStepList.Add(kaiShiLianZhiStepModel2);
			}
		}

		private void TouZhiCaoYaoClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("CaiLiaoXuanZeShaiXuanWnd.prefab", true, m_CurrentDengJie);
		}

		public void ChoosedCaoYao(WorldCaiLiaoModel chooseCaiLiao)
		{
			if (m_IsOpenRest)
			{
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel = null;
				for (int i = 0; i < m_AddStepList.Count; i++)
				{
					if (i == m_ChooseIndex - 1)
					{
						kaiShiLianZhiStepModel = m_AddStepList[i];
						break;
					}
				}
				LianZhiBuZhouSheZhiItem lianZhiBuZhouSheZhiItem = null;
				for (int j = 0; j < m_StepItemObjList.Count; j++)
				{
					if (j == m_ChooseIndex - 2)
					{
						lianZhiBuZhouSheZhiItem = m_StepItemObjList[j];
						break;
					}
				}
				if (kaiShiLianZhiStepModel != null && lianZhiBuZhouSheZhiItem != null)
				{
					if (kaiShiLianZhiStepModel.a == 5)
					{
						m_EndTimes = ((m_EndTimes >= 1) ? (m_EndTimes - 1) : 0);
					}
					kaiShiLianZhiStepModel.a = 2;
					kaiShiLianZhiStepModel.d = m_ChooseIndex;
					kaiShiLianZhiStepModel.b = chooseCaiLiao.a;
					kaiShiLianZhiStepModel.c = chooseCaiLiao.b;
					string miaoShu = $"投掷\n{chooseCaiLiao.c}";
					lianZhiBuZhouSheZhiItem.ResetValue(miaoShu);
				}
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			}
			else
			{
				InstanceItemObj(chooseCaiLiao);
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel2 = new KaiShiLianZhiStepModel();
				kaiShiLianZhiStepModel2.a = 2;
				kaiShiLianZhiStepModel2.b = chooseCaiLiao.a;
				kaiShiLianZhiStepModel2.c = chooseCaiLiao.b;
				kaiShiLianZhiStepModel2.d = m_CurrentStepNum;
				m_AddStepList.Add(kaiShiLianZhiStepModel2);
			}
		}

		private void ShouDanClick()
		{
			if (m_IsOpenRest)
			{
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel = null;
				for (int i = 0; i < m_AddStepList.Count; i++)
				{
					if (i == m_ChooseIndex - 1)
					{
						kaiShiLianZhiStepModel = m_AddStepList[i];
						break;
					}
				}
				LianZhiBuZhouSheZhiItem lianZhiBuZhouSheZhiItem = null;
				for (int j = 0; j < m_StepItemObjList.Count; j++)
				{
					if (j == m_ChooseIndex - 2)
					{
						lianZhiBuZhouSheZhiItem = m_StepItemObjList[j];
						break;
					}
				}
				if (kaiShiLianZhiStepModel.a == 5)
				{
					ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
					return;
				}
				if (kaiShiLianZhiStepModel != null && lianZhiBuZhouSheZhiItem != null)
				{
					kaiShiLianZhiStepModel.a = 5;
					kaiShiLianZhiStepModel.d = m_ChooseIndex;
					kaiShiLianZhiStepModel.b = "";
					lianZhiBuZhouSheZhiItem.ResetValue("收丹");
				}
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			}
			else
			{
				InstanceItemObj(ChuangZhaoStepType.ShouDan);
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel2 = new KaiShiLianZhiStepModel();
				kaiShiLianZhiStepModel2.a = 5;
				kaiShiLianZhiStepModel2.b = "";
				kaiShiLianZhiStepModel2.d = m_CurrentStepNum;
				m_AddStepList.Add(kaiShiLianZhiStepModel2);
			}
			m_EndTimes++;
		}

		private void InstanceItemObj(ChuangZhaoStepType stepType)
		{
			if (m_StepItemObj == null)
			{
				m_StepItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/LianZhiBuZhouSheZhiItem.prefab");
			}
			GameObject gameObject = SetListContentParent(Panel.ScrollViewContent, m_StepItemObj);
			ToggleGroup component = Panel.ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			LianZhiBuZhouSheZhiItem component2 = gameObject.gameObject.GetComponent<LianZhiBuZhouSheZhiItem>();
			string miaoShu = string.Empty;
			switch (stepType)
			{
			case ChuangZhaoStepType.KongHuo:
				miaoShu = "控火";
				break;
			case ChuangZhaoStepType.TouZhi:
				miaoShu = "投掷";
				break;
			case ChuangZhaoStepType.NingYe:
				miaoShu = "凝液";
				break;
			case ChuangZhaoStepType.ShouDan:
				miaoShu = "收丹";
				break;
			}
			component2.SetValue(++m_CurrentStepNum, miaoShu, component, StepReChoose, StepRemove);
			m_StepItemObjList.Add(component2);
			ResetRectContentHight();
			Panel.AddStepBtn.transform.SetAsLastSibling();
			ShowStepPanel(isLeft: true, 0f, 0f, isShow: false);
		}

		private void InstanceItemObj(WorldCaiLiaoModel chooseCaiLiao)
		{
			if (m_StepItemObj == null)
			{
				m_StepItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/LianZhiBuZhouSheZhiItem.prefab");
			}
			GameObject gameObject = SetListContentParent(Panel.ScrollViewContent, m_StepItemObj);
			ToggleGroup component = Panel.ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			LianZhiBuZhouSheZhiItem component2 = gameObject.gameObject.GetComponent<LianZhiBuZhouSheZhiItem>();
			string empty = string.Empty;
			empty = $"投掷\n{chooseCaiLiao.c}";
			component2.SetValue(++m_CurrentStepNum, empty, component, StepReChoose, StepRemove);
			m_StepItemObjList.Add(component2);
			ResetRectContentHight();
			Panel.AddStepBtn.transform.SetAsLastSibling();
			ShowStepPanel(isLeft: true, 0f, 0f, isShow: false);
		}

		private void StepRemove(LianZhiBuZhouSheZhiItem stepItem, string index)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确定移除该步骤？", () =>
			{
				KaiShiLianZhiStepModel kaiShiLianZhiStepModel = null;
				bool flag = false;
				for (int i = 0; i < m_AddStepList.Count; i++)
				{
					if (i == int.Parse(index) - 1)
					{
						kaiShiLianZhiStepModel = m_AddStepList[i];
						flag = true;
					}
					else if (flag)
					{
						m_AddStepList[i].d--;
					}
				}
				if (kaiShiLianZhiStepModel != null)
				{
					m_AddStepList.Remove(kaiShiLianZhiStepModel);
				}
				LianZhiBuZhouSheZhiItem lianZhiBuZhouSheZhiItem = null;
				bool flag2 = false;
				for (int j = 0; j < m_StepItemObjList.Count; j++)
				{
					if (j == int.Parse(index) - 2)
					{
						lianZhiBuZhouSheZhiItem = m_StepItemObjList[j];
						flag2 = true;
					}
					else if (flag2)
					{
						int index2 = int.Parse(m_StepItemObjList[j].ShunXuText.text) - 1;
						m_StepItemObjList[j].ShunXuText.text = index2.ToString();
						m_StepItemObjList[j].index = index2.ToString();
						m_StepItemObjList[j]._index = index2;
					}
				}
				if (lianZhiBuZhouSheZhiItem != null)
				{
					m_StepItemObjList.Remove(lianZhiBuZhouSheZhiItem);
				}
				Object.Destroy(stepItem.gameObject);
				if (stepItem.MiaoShuText.text == "收丹")
				{
					m_EndTimes = ((m_EndTimes >= 1) ? (m_EndTimes - 1) : 0);
				}
				else if (stepItem.MiaoShuText.text.Contains("投掷"))
				{
					m_TouZhiTimes = ((m_TouZhiTimes >= 1) ? (m_TouZhiTimes - 1) : 0);
				}
				m_CurrentStepNum--;
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			});
		}

		private void StepReChoose(bool isOn, int index, LianZhiBuZhouSheZhiItem stepItem)
		{
			if (isOn)
			{
				RectTransform component = stepItem.gameObject.GetComponent<RectTransform>();
				m_ChooseIndex = index;
				ReSetStep(component);
			}
			else
			{
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			}
		}

		private void ShowStepPanel(bool isLeft, float x, float y, bool isShow = true)
		{
			if (!isShow)
			{
				Panel.LeftStepPanel.gameObject.SetActive(value: false);
				Panel.RightStepPanel.gameObject.SetActive(value: false);
			}
			else if (isLeft)
			{
				Panel.LeftStepPanel.gameObject.SetActive(value: true);
				Panel.RightStepPanel.gameObject.SetActive(value: false);
				Panel.LeftStepPanel.position = new Vector2(x, y);
			}
			else
			{
				Panel.LeftStepPanel.gameObject.SetActive(value: false);
				Panel.RightStepPanel.gameObject.SetActive(value: true);
				Panel.RightStepPanel.position = new Vector2(x, y);
			}
		}

		private void ResetRectContentHight()
		{
			GridLayoutGroup component = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(m_CurrentStepNum + 2, Panel.ScrollViewContent, component);
			float y = Panel.ScrollViewContent.sizeDelta.y;
			float y2 = Panel.ScrollView.gameObject.GetComponent<RectTransform>().sizeDelta.y;
			if (y > y2)
			{
				Panel.ScrollViewContent.anchoredPosition = new Vector2(Panel.ScrollViewContent.anchoredPosition.x, y - y2);
			}
		}
	}
}
