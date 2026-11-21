using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeLianDanJieSuanWnd : Window
	{
		public DiHuoGeLianDanJieSuanPanel Panel;

		private LianZhiParameter m_DataSource;

		private List<BaseNaJieGoodEntity> m_DanYaoList;

		private LianZhiJieGuoType m_CurrentJieGuoType;

		private DST_KaoShiItemModel m_KaoShiModel;

		private DanFangModel m_DangFangSource;

		private Dictionary<string, BaseNaJieGoodEntity> m_CaiLiaoDic;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DiHuoGeLianDanJieSuanPanel>();
			AddButtonClickListener(Panel.Fail_SureBtn, SureBtnClick);
			AddButtonClickListener(Panel.Success_SureBtn, SureBtnClick);
			Panel.SuccessPanel.gameObject.SetActive(value: false);
			Panel.FailPanel.gameObject.SetActive(value: false);
			Panel.ChuangZhaoSuccess.gameObject.SetActive(value: false);
			Panel.ChuangZhaoFailRect.gameObject.SetActive(value: false);
			Panel.KaoHeSuccessRect.gameObject.SetActive(value: false);
			Panel.KaoHeFailRect.gameObject.SetActive(value: false);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as LianZhiParameter;
				m_DanYaoList = paralist[1] as List<BaseNaJieGoodEntity>;
				m_CaiLiaoDic = paralist[2] as Dictionary<string, BaseNaJieGoodEntity>;
				m_CurrentJieGuoType = (LianZhiJieGuoType)paralist[3];
				switch (m_CurrentJieGuoType)
				{
				case LianZhiJieGuoType.LianDanSucess:
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanSuccess);
					Panel.SuccessPanel.gameObject.SetActive(value: true);
					InitSuccessPanel();
					break;
				case LianZhiJieGuoType.LianDanFail:
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanFail);
					Panel.FailPanel.gameObject.SetActive(value: true);
					InitFailPanel();
					break;
				case LianZhiJieGuoType.ChuangZhaoSucess:
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanSuccess);
					m_DangFangSource = paralist[4] as DanFangModel;
					Panel.ChuangZhaoSuccess.gameObject.SetActive(value: true);
					InitChuangZaoSuccess();
					break;
				case LianZhiJieGuoType.ChuangZhaoFail:
				case LianZhiJieGuoType.ChuangZhaoControlFail:
				{
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanFail);
					m_DangFangSource = paralist[4] as DanFangModel;
					string content = paralist[5]?.ToString();
					Panel.ChuangZhaoFailRect.gameObject.SetActive(value: true);
					InitChuangZaoShiBai(content);
					break;
				}
				case LianZhiJieGuoType.KaoShiSuccess:
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanSuccess);
					m_KaoShiModel = paralist[4] as DST_KaoShiItemModel;
					Panel.KaoHeSuccessRect.gameObject.SetActive(value: true);
					InitKaoHeSuccessPanel();
					break;
				case LianZhiJieGuoType.KaoShiFail:
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.LianDanFail);
					m_KaoShiModel = paralist[4] as DST_KaoShiItemModel;
					Panel.KaoHeFailRect.gameObject.SetActive(value: true);
					InitKaoHeFailPanel();
					break;
				}
			}
		}

		private void InitSuccessPanel()
		{
			int num = ((m_DataSource.list != null) ? m_DataSource.list.Count : 0);
			InitCaiLiaoList();
			Panel.Success_JingLiXiaoHaoText.text = num + "点";
			if (m_DanYaoList != null && m_DanYaoList.Count > 0)
			{
				Panel.Success_NameText.text = $"{m_DanYaoList[0].Name}{m_DanYaoList.Count}枚";
				string levelByRankName = WorldSetting.GetLevelByRankName(m_DanYaoList[0].Rank);
				Panel.Success_TitleText.text = $"恭喜！成功炼制 '{levelByRankName}' {m_DanYaoList[0].Name} {m_DanYaoList.Count}枚";
			}
		}

		private void InitFailPanel()
		{
			int count = m_DataSource.list.Count;
			InitCaiLiaoList();
			Panel.Fail_JingLiXiaoHaoText.text = count + "点";
			Panel.Fail_TitleText.text = "糟糕！炼丹失败";
		}

		private void InitKaoHeSuccessPanel()
		{
			Panel.KaoHeSuccessLiKaiBtn.onClick.RemoveAllListeners();
			Panel.KaoHeSuccessLiKaiBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiWnd.prefab");
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			});
		}

		private void InitKaoHeFailPanel()
		{
			Panel.KaoHeFailLiKaiBtn.onClick.RemoveAllListeners();
			Panel.KaoHeFailLiKaiBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			});
			Panel.KaoHeZaiShiYiCiBtn.onClick.RemoveAllListeners();
			Panel.KaoHeZaiShiYiCiBtn.onClick.AddListener(() =>
			{
				ExamineElixirStepReq_1008009 data = new ExamineElixirStepReq_1008009
				{
					MethodElixirId = m_KaoShiModel.elixirTeacherId
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_DANFANGKAOHE, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						ExamineElixirStepResp_1008009 examineElixirStepResp_ = new ExamineElixirStepResp_1008009();
						examineElixirStepResp_.MergeFrom(byteString);
						if (examineElixirStepResp_.IsSuccess)
						{
							m_KaoShiModel.methodElixir = new DanFangModel();
							m_KaoShiModel.methodElixir.bb = new List<DanFangStepModel>();
							foreach (ExamineElixirStepResp_1008009.Types.ElixirStepObjectResp_1008010 item2 in examineElixirStepResp_.Object)
							{
								DanFangStepModel item = new DanFangStepModel
								{
									id = item2.Order.ToString(),
									actionType = item2.ActionType,
									bestVal = item2.BestVal,
									continuetime = item2.ContinueTime,
									maxVal = item2.MaxVal,
									minVal = item2.MinVal,
									order = item2.Order,
									targetType = item2.TargetType,
									valType = item2.ValType
								};
								m_KaoShiModel.methodElixir.bb.Add(item);
							}
							Singleton<UIManager>.Instance.CloseWnd(this);
							Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DiHuoGe_LianDan", DiHuoGeEnterType.KaoShi, m_KaoShiModel);
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(examineElixirStepResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private int GetXiaoHaoWuXing()
		{
			int num = 0;
			if (m_DangFangSource != null)
			{
				foreach (DanFangStepModel item in m_DangFangSource.bb)
				{
					if (item.actionType == 2)
					{
						num++;
					}
				}
			}
			return num;
		}

		private void InitChuangZaoSuccess()
		{
			Panel.ZhuCeBtn.onClick.RemoveAllListeners();
			Panel.ZhuCeBtn.onClick.AddListener(() =>
			{
				DST_DanFangModel dST_DanFangModel = new DST_DanFangModel
				{
					a = m_DangFangSource.ei,
					b = m_DangFangSource.na,
					c = m_DangFangSource.r,
					d = m_DangFangSource.v,
					e = m_DangFangSource.l,
					f = m_DangFangSource.q,
					g = m_DangFangSource.aa,
					h = m_DangFangSource.s
				};
				Singleton<UIManager>.Instance.PopUpWnd("DST_ZhuCeTipWnd.prefab", true, dST_DanFangModel, 0);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiWnd.prefab");
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab");
			});
			Panel.ShaoHouZhuCeBtn.onClick.RemoveAllListeners();
			Panel.ShaoHouZhuCeBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiWnd.prefab");
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab");
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			});
			int xiaoHaoWuXing = GetXiaoHaoWuXing();
			xiaoHaoWuXing = m_DangFangSource.r * xiaoHaoWuXing;
			Panel.SuccessWuXingXiaoHaoText.text = xiaoHaoWuXing + "点";
		}

		private void InitChuangZaoShiBai(string content)
		{
			Panel.TiaoZhengDanFangBtn.onClick.RemoveAllListeners();
			Panel.TiaoZhengDanFangBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.ShowWnd("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab", true);
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiWnd.prefab");
			});
			Panel.LiKaiBtn.onClick.RemoveAllListeners();
			Panel.LiKaiBtn.onClick.AddListener(() =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiWnd.prefab");
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab");
				Singleton<UIManager>.Instance.PopUpWnd("ChuangZhao_LianZhiDengJieWnd.prefab", true);
			});
			int num = m_DangFangSource.c + 4;
			Panel.WuXingXiaoHaoText.text = num + "点";
			Panel.FailContentText.text = content;
		}

		private void InitCaiLiaoList()
		{
			RectTransform parentRect = (m_DataSource.isSuccess ? Panel.Success_Content : Panel.Fail_Content);
			if (m_DataSource.list != null && m_DataSource.list.Count > 0)
			{
				foreach (ClientPracticeArticleElixirStepData item in m_DataSource.list)
				{
					BaseNaJieGoodEntity value = null;
					if (m_CaiLiaoDic.TryGetValue(item.stepElixirId, out value))
					{
						InitCaiLiaoItem(value, parentRect);
					}
				}
				return;
			}
			if (m_DataSource.list.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> item2 in m_CaiLiaoDic)
			{
				InitCaiLiaoItem(item2.Value, parentRect);
			}
		}

		private void InitCaiLiaoItem(BaseNaJieGoodEntity data, RectTransform parentRect)
		{
			if (data != null && m_DataSource != null)
			{
				GameObject gameObject = SetListContentParent(parentRect, Panel.CaiLiaoListItem);
				gameObject.gameObject.SetActive(value: true);
				gameObject.GetComponent<Text>().text = data.Name;
			}
		}

		private void SureBtnClick()
		{
			DiHuoGeKaiShiLianZhiWnd diHuoGeKaiShiLianZhiWnd = Singleton<UIManager>.Instance.FindWndByName<DiHuoGeKaiShiLianZhiWnd>("DiHuoGeLianZhiWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			diHuoGeKaiShiLianZhiWnd?.CloseWnd();
		}
	}
}
