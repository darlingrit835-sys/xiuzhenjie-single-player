using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DiHuoGeKaiShiLianZhiWnd : Window
	{
		public DiHuoGeKaiShiLianZhiPanel Panel;

		private DanFangModel m_DataSource;

		private Dictionary<string, BaseNaJieGoodEntity> m_CaiLiaoDic;

		private List<BaseNaJieGoodEntity> m_CaiLiaoList;

		private List<string> m_StepNameList;

		private DiHuoGeEnterType m_EnterType;

		private bool m_CreateSuceess;

		private DST_KaoShiItemModel m_KaoShiModel;

		private GameObject m_LeftCaiLiao;

		private GameObject m_RightCaiLiao;

		private int m_CurrentCaiLiaoIndex;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_CurrentCaiLiaoIndex = 0;
			m_CaiLiaoList = new List<BaseNaJieGoodEntity>();
			m_LeftCaiLiao = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/TouZhiLeftCaiLiao.prefab");
			m_RightCaiLiao = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/TouZhiRightCaiLiao.prefab");
			MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
			Panel = base.GameObject.GetComponent<DiHuoGeKaiShiLianZhiPanel>();
			DiHuoGeLianDanControlPanel stepControlPanel = Panel.StepControlPanel;
			stepControlPanel.FailerAction = (UnityAction<List<DanFangStepModel>, Dictionary<string, double>>)Delegate.Remove(stepControlPanel.FailerAction, new UnityAction<List<DanFangStepModel>, Dictionary<string, double>>(Failer));
			DiHuoGeLianDanControlPanel stepControlPanel2 = Panel.StepControlPanel;
			stepControlPanel2.FailerAction = (UnityAction<List<DanFangStepModel>, Dictionary<string, double>>)Delegate.Combine(stepControlPanel2.FailerAction, new UnityAction<List<DanFangStepModel>, Dictionary<string, double>>(Failer));
			DiHuoGeLianDanControlPanel stepControlPanel3 = Panel.StepControlPanel;
			stepControlPanel3.SuccessAction = (UnityAction<List<DanFangStepModel>, Dictionary<string, double>>)Delegate.Remove(stepControlPanel3.SuccessAction, new UnityAction<List<DanFangStepModel>, Dictionary<string, double>>(Success));
			DiHuoGeLianDanControlPanel stepControlPanel4 = Panel.StepControlPanel;
			stepControlPanel4.SuccessAction = (UnityAction<List<DanFangStepModel>, Dictionary<string, double>>)Delegate.Combine(stepControlPanel4.SuccessAction, new UnityAction<List<DanFangStepModel>, Dictionary<string, double>>(Success));
			AddButtonClickListener(Panel.LiKaiBtn, CloseWnd);
			AddButtonClickListener(Panel.XiaoWenHao, WenHaoTogClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 1)
			{
				m_DataSource = paralist[0] as DanFangModel;
				m_CaiLiaoDic = paralist[1] as Dictionary<string, BaseNaJieGoodEntity>;
				m_CaiLiaoList?.Clear();
				if (m_CaiLiaoDic != null)
				{
					foreach (KeyValuePair<string, BaseNaJieGoodEntity> item in m_CaiLiaoDic)
					{
						m_CaiLiaoList.Add(item.Value);
					}
				}
				m_EnterType = (DiHuoGeEnterType)paralist[2];
				switch (m_EnterType)
				{
				case DiHuoGeEnterType.ChuangZao:
					m_CreateSuceess = (bool)paralist[3];
					break;
				case DiHuoGeEnterType.KaoShi:
					m_KaoShiModel = paralist[3] as DST_KaoShiItemModel;
					m_DataSource = m_KaoShiModel.methodElixir;
					break;
				}
			}
			MonoSingleton<DiHuoGe_LianDanManager>.Instance.ShowOrHideDanLuBg(isShow: true);
			Init();
			InitStep();
			Panel.StepControlPanel.InitDataSource(m_DataSource, this);
		}

		private void Init()
		{
			m_StepNameList = new List<string>();
			if (m_DataSource.bb == null || m_DataSource.bb.Count <= 0)
			{
				return;
			}
			foreach (DanFangStepModel item2 in m_DataSource.bb)
			{
				string item = "";
				switch ((LianZhiActionStepType)item2.actionType)
				{
				case LianZhiActionStepType.ACTION_FIRE:
					item = "点火";
					break;
				case LianZhiActionStepType.ACTION_THROW:
					item = "投掷";
					break;
				case LianZhiActionStepType.ACTION_COLLECT:
					item = "收丹";
					break;
				case LianZhiActionStepType.ACTION_CONTROLL_FIRE:
					item = "控火";
					break;
				case LianZhiActionStepType.ACTION_GATHER:
					item = "凝液";
					break;
				}
				m_StepNameList.Add(item);
			}
		}

		private void InitStep()
		{
			Panel.StepPanel.InitSteps(m_StepNameList, this);
		}

		public void NextStep()
		{
			Panel.StepPanel.NextStep();
		}

		public void LeftTouZhi()
		{
			if (m_LeftCaiLiao != null)
			{
				UnityEngine.Object.Instantiate(m_LeftCaiLiao, base.GameObject.transform).GetComponent<TouZhiCaiLiaoItem>()?.SetImgAndRun(GetCaiLiaoType());
			}
			m_CurrentCaiLiaoIndex++;
		}

		public void RightTouZhi()
		{
			if (m_RightCaiLiao != null)
			{
				UnityEngine.Object.Instantiate(m_RightCaiLiao, base.GameObject.transform).GetComponent<TouZhiCaiLiaoItem>()?.SetImgAndRun(GetCaiLiaoType());
			}
			m_CurrentCaiLiaoIndex++;
		}

		private TouZhiCaiLiaoTypeEnum GetCaiLiaoType()
		{
			if (m_DataSource.ei == "LianXiDanFang")
			{
				return TouZhiCaiLiaoTypeEnum.ZhiWu;
			}
			BaseNaJieGoodEntity baseNaJieGoodEntity = null;
			if (m_CaiLiaoList.Count > m_CurrentCaiLiaoIndex)
			{
				baseNaJieGoodEntity = m_CaiLiaoList[m_CurrentCaiLiaoIndex];
			}
			if (baseNaJieGoodEntity != null)
			{
				switch ((GoodsTypeEnum)baseNaJieGoodEntity.Type)
				{
				case GoodsTypeEnum.MATERIAL_ANIMAL:
				case GoodsTypeEnum.MATERIAL_ANIMAL_SKIN:
				case GoodsTypeEnum.MATERIAL_ANIMAL_BONE:
				case GoodsTypeEnum.MATERIAL_ANIMAL_PELLET:
					return TouZhiCaiLiaoTypeEnum.YaoDan;
				case GoodsTypeEnum.MATERIAL_ANIMAL_BLOOD:
					return TouZhiCaiLiaoTypeEnum.JingXue;
				case GoodsTypeEnum.MATERIAL_PLANT:
					return TouZhiCaiLiaoTypeEnum.ZhiWu;
				default:
					return TouZhiCaiLiaoTypeEnum.ZhiWu;
				}
			}
			return TouZhiCaiLiaoTypeEnum.ZhiWu;
		}

		public void Success(List<DanFangStepModel> compeleteList, Dictionary<string, double> timeDic)
		{
			if (m_DataSource.ei == "LianXiDanFang")
			{
				if (Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
				{
					GuideInfoUpdateReq_1002013 guideInfoUpdateReq_ = new GuideInfoUpdateReq_1002013();
					guideInfoUpdateReq_.Type = 8;
					Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_GUIDE, guideInfoUpdateReq_, (object _obj) =>
					{
					});
					Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal = 100;
				}
				MonoSingleton<GameStart>.Instance.PopUpAttention("您已完成学习，还请日后勤加练习，早日成为一代宗师!", CloseWnd);
				return;
			}
			m_CurrentCaiLiaoIndex = 0;
			MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
			if (m_EnterType == DiHuoGeEnterType.DongFu)
			{
				LianZhiParameter paramseter = GetParamseter(isSuccess: true, compeleteList, timeDic);
				SendResultToServer(paramseter);
			}
			else if (m_EnterType == DiHuoGeEnterType.KaoShi)
			{
				KaoShiParameter paramseter2 = GetParamseter(isSuccess: true);
				KaoShiReceiveServerCallBackSuccess(paramseter2);
			}
			else
			{
				if (m_EnterType != DiHuoGeEnterType.ChuangZao)
				{
					return;
				}
				CreateMethodElixirRefiningResultReq_1008020 data = new CreateMethodElixirRefiningResultReq_1008020
				{
					IsSuccess = m_CreateSuceess,
					MethodElixirId = m_DataSource.ei
				};
				if (m_CreateSuceess)
				{
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
								m_DataSource.aa = danYaoXiangQing;
								Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(createMethodElixirRefiningResultResp_.LiveLevel, createMethodElixirRefiningResultResp_.LiveProficiency, RoleFuZhiType.LianDan);
								Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = createMethodElixirRefiningResultResp_.CurrentClever;
								Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoSucess, m_DataSource);
							}
							else
							{
								string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createMethodElixirRefiningResultResp_.Code);
								Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoFail, m_DataSource, hintMsgByCode);
							}
						}
					}, isShowWaitUI: true);
					return;
				}
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
							Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = createMethodElixirRefiningResultResp_.CurrentClever;
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(createMethodElixirRefiningResultResp_.Code);
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoFail, m_DataSource, hintMsgByCode);
					}
				}, isShowWaitUI: true);
			}
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

		private void Failer(List<DanFangStepModel> compeleteList, Dictionary<string, double> timeDic)
		{
			if (m_DataSource.ei == "LianXiDanFang")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请再接再厉!", CloseWnd);
				return;
			}
			m_CurrentCaiLiaoIndex = 0;
			MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
			if (m_EnterType == DiHuoGeEnterType.DongFu)
			{
				LianZhiParameter paramseter = GetParamseter(isSuccess: false, compeleteList, timeDic);
				SendResultToServer(paramseter);
			}
			else if (m_EnterType == DiHuoGeEnterType.KaoShi)
			{
				KaoShiParameter paramseter2 = GetParamseter(isSuccess: false);
				KaoShiReceiveServerCallBackFailer(paramseter2);
			}
			else
			{
				if (m_EnterType != DiHuoGeEnterType.ChuangZao)
				{
					return;
				}
				CreateMethodElixirRefiningResultReq_1008020 data = new CreateMethodElixirRefiningResultReq_1008020
				{
					IsSuccess = false,
					MethodElixirId = m_DataSource.ei
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
							Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = createMethodElixirRefiningResultResp_.CurrentClever;
						}
						string text = "这个配方好像没什么问题，可能只是神炼此丹方时，操控不当，再试一次吧";
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.ChuangZhaoFail, m_DataSource, text);
					}
				}, isShowWaitUI: true);
			}
		}

		private KaoShiParameter GetParamseter(bool isSuccess)
		{
			return new KaoShiParameter
			{
				assessElixirId = Singleton<RoleManager>.Instance.GetCurrentDanShiLevel(),
				isSuccess = isSuccess
			};
		}

		private LianZhiParameter GetParamseter(bool isSuccess, List<DanFangStepModel> compeleteList, Dictionary<string, double> timeDic)
		{
			LianZhiParameter lianZhiParameter = new LianZhiParameter();
			lianZhiParameter.elixirId = m_DataSource.ei;
			lianZhiParameter.isSuccess = isSuccess;
			List<ClientPracticeArticleElixirStepData> list = new List<ClientPracticeArticleElixirStepData>();
			foreach (DanFangStepModel compelete in compeleteList)
			{
				ClientPracticeArticleElixirStepData clientPracticeArticleElixirStepData = new ClientPracticeArticleElixirStepData();
				BaseNaJieGoodEntity value = null;
				if (m_CaiLiaoDic.TryGetValue(compelete.id, out value))
				{
					clientPracticeArticleElixirStepData.goodsId = value.GoodsId;
					clientPracticeArticleElixirStepData.goodsType = value.Type;
				}
				clientPracticeArticleElixirStepData.stepElixirId = compelete.id;
				double value2 = 0.0;
				timeDic.TryGetValue(compelete.id, out value2);
				clientPracticeArticleElixirStepData.bestTime = value2;
				list.Add(clientPracticeArticleElixirStepData);
			}
			lianZhiParameter.list = list;
			return lianZhiParameter;
		}

		private void SendResultToServer(LianZhiParameter model)
		{
			RefiningElixirReq_1005027 refiningElixirReq_ = new RefiningElixirReq_1005027
			{
				IsRefiningSuccess = model.isSuccess,
				MethodElixirId = model.elixirId
			};
			foreach (ClientPracticeArticleElixirStepData item in model.list)
			{
				refiningElixirReq_.StepDetailList.Add(new RefiningElixirReq_1005027.Types.StepDetailReq_1005028
				{
					BestTime = item.bestTime,
					GoodsId = ((item.goodsId == null) ? "" : item.goodsId),
					StepId = item.stepElixirId
				});
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.REFINING_ELIXIR, refiningElixirReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RefiningElixirResp_1005027 refiningElixirResp_ = new RefiningElixirResp_1005027();
					refiningElixirResp_.MergeFrom(byteString);
					if (refiningElixirResp_.IsSuccess)
					{
						if (model.isSuccess)
						{
							ReceiveServerCallBackSuccess(refiningElixirResp_, model);
						}
						else
						{
							ReceiveServerCallBackFailer(refiningElixirResp_, model);
						}
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(refiningElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ReceiveServerCallBackSuccess(RefiningElixirResp_1005027 refiningElixirResp_1005027, LianZhiParameter model)
		{
			Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningElixirResp_1005027.CurrentVigor;
			Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(refiningElixirResp_1005027.LiveLevel, refiningElixirResp_1005027.LiveProficiency, RoleFuZhiType.LianDan);
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			for (int i = 0; i < refiningElixirResp_1005027.Count; i++)
			{
				list.Add(new BaseNaJieGoodEntity
				{
					Rank = refiningElixirResp_1005027.Rank,
					Name = m_DataSource.na
				});
			}
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, model, list, m_CaiLiaoDic, LianZhiJieGuoType.LianDanSucess);
		}

		private void ReceiveServerCallBackFailer(RefiningElixirResp_1005027 refiningElixirResp_1005027, LianZhiParameter model)
		{
			Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningElixirResp_1005027.CurrentVigor;
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, model, null, m_CaiLiaoDic, LianZhiJieGuoType.LianDanFail);
		}

		private void KaoShiReceiveServerCallBackSuccess(KaoShiParameter model)
		{
			ElixirExamineResultReq_1008011 data = new ElixirExamineResultReq_1008011
			{
				IsSuccess = true
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_DANSHIKAOHERESULT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ElixirExamineResultResp_1008011 elixirExamineResultResp_ = new ElixirExamineResultResp_1008011();
					elixirExamineResultResp_.MergeFrom(byteString);
					if (elixirExamineResultResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.KaoShiSuccess, m_KaoShiModel);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(elixirExamineResultResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode, () =>
						{
							Singleton<UIManager>.Instance.CloseAllWnd();
							Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
						}, () =>
						{
							Singleton<UIManager>.Instance.CloseAllWnd();
							Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
						});
					}
				}
			}, isShowWaitUI: true);
		}

		private void KaoShiReceiveServerCallBackFailer(KaoShiParameter model)
		{
			ElixirExamineResultReq_1008011 data = new ElixirExamineResultReq_1008011
			{
				IsSuccess = false
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_DANSHIKAOHERESULT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ElixirExamineResultResp_1008011 elixirExamineResultResp_ = new ElixirExamineResultResp_1008011();
					elixirExamineResultResp_.MergeFrom(byteString);
					if (elixirExamineResultResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.CloseWnd(this);
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, null, null, null, LianZhiJieGuoType.KaoShiFail, m_KaoShiModel);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(elixirExamineResultResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode, () =>
						{
							Singleton<UIManager>.Instance.CloseAllWnd();
							Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
						}, () =>
						{
							Singleton<UIManager>.Instance.CloseAllWnd();
							Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
						});
					}
				}
			}, isShowWaitUI: true);
		}

		public void ShowTiShi(TouZhiType _type)
		{
			switch (_type)
			{
			case TouZhiType.Left:
				Panel.TiShiLeft.gameObject.SetActive(value: true);
				Panel.TiShiRight.gameObject.SetActive(value: false);
				Panel.TiShiUp.gameObject.SetActive(value: false);
				break;
			case TouZhiType.Right:
				Panel.TiShiLeft.gameObject.SetActive(value: false);
				Panel.TiShiRight.gameObject.SetActive(value: true);
				Panel.TiShiUp.gameObject.SetActive(value: false);
				break;
			case TouZhiType.Up:
				Panel.TiShiLeft.gameObject.SetActive(value: false);
				Panel.TiShiRight.gameObject.SetActive(value: false);
				Panel.TiShiUp.gameObject.SetActive(value: true);
				break;
			case TouZhiType.None:
				Panel.TiShiLeft.gameObject.SetActive(value: false);
				Panel.TiShiRight.gameObject.SetActive(value: false);
				Panel.TiShiUp.gameObject.SetActive(value: false);
				break;
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			DiHuoGeLianDanControlPanel stepControlPanel = Panel.StepControlPanel;
			stepControlPanel.FailerAction = (UnityAction<List<DanFangStepModel>, Dictionary<string, double>>)Delegate.Remove(stepControlPanel.FailerAction, new UnityAction<List<DanFangStepModel>, Dictionary<string, double>>(Failer));
			DiHuoGeLianDanControlPanel stepControlPanel2 = Panel.StepControlPanel;
			stepControlPanel2.SuccessAction = (UnityAction<List<DanFangStepModel>, Dictionary<string, double>>)Delegate.Remove(stepControlPanel2.SuccessAction, new UnityAction<List<DanFangStepModel>, Dictionary<string, double>>(Success));
		}

		private void WenHaoTogClick()
		{
			string text = $"炼丹";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "炼丹", "炼丹");
		}

		public void CloseWnd()
		{
			switch (m_EnterType)
			{
			case DiHuoGeEnterType.ChuangZao:
				MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
				MonoSingleton<DiHuoGe_LianDanManager>.Instance.ShowOrHideDanLuBg(isShow: false);
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.ShowWnd("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab", true);
				break;
			case DiHuoGeEnterType.DongFu:
				MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
				MonoSingleton<DiHuoGe_LianDanManager>.Instance.ShowOrHideDanLuBg(isShow: false);
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeWnd.prefab", true);
				break;
			case DiHuoGeEnterType.KaoShi:
				MonoSingleton<DiHuoGe_LianDanManager>.Instance.ChooseFire(FireStatus.None);
				MonoSingleton<DiHuoGe_LianDanManager>.Instance.ShowOrHideDanLuBg(isShow: false);
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
				break;
			case DiHuoGeEnterType.DiHuoGe:
				break;
			}
		}
	}
}
