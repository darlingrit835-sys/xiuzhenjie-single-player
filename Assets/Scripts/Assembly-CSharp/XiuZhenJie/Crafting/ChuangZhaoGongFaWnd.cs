using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ChuangZhaoGongFaWnd : Window
	{
		public ChuangZhaoGongFaPanel Panel;

		private List<AcupointModel> m_DataSource;

		private List<ChuangZhaoGongFaItem> m_GongFaItemList;

		private XSJ_GongFaModel m_ChuangZhaoGongFa;

		private int m_FailCode;

		private List<Vector3> m_PosList;

		private bool m_ChuangZhaoCompelete;

		private bool m_RequesHaveDone;

		private int wuxin;

		private bool m_IsBuQuan;

		private string m_ID;

		private List<string> m_AcupointList;

		private List<Vector3> m_YiYouPosList;

		private List<ChuangZhaoGongFaItem> m_YiYouItemList;

		private List<ChuangZhaoGongFaItem> m_ZoItemList;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab")?.ShowOrHideBottomBg(isShow: false);
			Panel = base.GameObject.GetComponent<ChuangZhaoGongFaPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			m_DataSource = Singleton<FacadeManager>.Instance.SelectAcupointModels();
			m_ChuangZhaoCompelete = false;
			m_RequesHaveDone = false;
			Init();
			m_AcupointList = new List<string>();
			m_YiYouItemList = new List<ChuangZhaoGongFaItem>();
			m_YiYouPosList = new List<Vector3>();
			if (paralist != null && paralist.Length != 0)
			{
				m_IsBuQuan = true;
				Panel.BiaoTiName.text = "补全功法";
				m_ID = paralist[0] as string;
				AddButtonClickListener(Panel.WeiHao, BuQuanWenHaoTogClick);
				AddButtonClickListener(Panel.MoNiBtn, BuQuanBtnClick);
				Panel.RandomBtn.interactable = false;
				HuoQuAcupoint();
			}
			else
			{
				m_IsBuQuan = false;
				Panel.BiaoTiName.text = "创造功法";
				Panel.RandomBtn.interactable = true;
				AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
				AddButtonClickListener(Panel.RandomBtn, RadomXueWei);
				AddButtonClickListener(Panel.MoNiBtn, MoNiChuangZhaoBtnClick);
				Panel.MaskImage.gameObject.SetActive(value: false);
			}
		}

		private void BuQuanBtnClick()
		{
			if (!IsCanChuanZhaoGongFa(out var resultModel) || resultModel == null)
			{
				return;
			}
			VerifyCompletionMethodAchievementReq_1008080 verifyCompletionMethodAchievementReq_ = new VerifyCompletionMethodAchievementReq_1008080();
			foreach (string item in resultModel.a)
			{
				verifyCompletionMethodAchievementReq_.AcupointId.Add(item);
			}
			verifyCompletionMethodAchievementReq_.MethodAchievementId = m_ID;
			Singleton<NetManager>.Instance.SendMsg(MsgType.VerifyCompletionMethodAchievement, verifyCompletionMethodAchievementReq_, (object obj) =>
			{
				m_RequesHaveDone = true;
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCompletionMethodAchievementResp_1008080 verifyCompletionMethodAchievementResp_ = new VerifyCompletionMethodAchievementResp_1008080();
					verifyCompletionMethodAchievementResp_.MergeFrom(byteString);
					if (verifyCompletionMethodAchievementResp_.IsSuccess)
					{
						if (verifyCompletionMethodAchievementResp_.CreatedIsSuccess)
						{
							GetSuccessDataDetailList(verifyCompletionMethodAchievementResp_);
						}
						else
						{
							m_ChuangZhaoGongFa = null;
							m_FailCode = verifyCompletionMethodAchievementResp_.Code;
						}
						wuxin = Singleton<RoleManager>.Instance.Role_Entity.CurrentClever - verifyCompletionMethodAchievementResp_.CurrentClever;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCompletionMethodAchievementResp_.CurrentClever;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						m_ChuangZhaoGongFa = null;
						m_FailCode = verifyCompletionMethodAchievementResp_.Code;
					}
				}
			});
			Panel.Bg.gameObject.SetActive(value: false);
			Panel.BgMask.gameObject.SetActive(value: false);
			Panel.MoNiBtn.gameObject.SetActive(value: false);
			MonoSingleton<ChuangZhaoGongFaSceneManager>.Instance.SetPoint(m_PosList);
			MonoSingleton<ChuangZhaoGongFaSceneManager>.Instance.StartLine();
		}

		private void HuoQuAcupoint()
		{
			AchievementAcupointReq_1008083 data = new AchievementAcupointReq_1008083
			{
				MethodAchievementId = m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ACHIEVEMENT_ACUPOINT, data, (object obj) =>
			{
				m_RequesHaveDone = true;
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					AchievementAcupointResp_1008083 achievementAcupointResp_ = new AchievementAcupointResp_1008083();
					achievementAcupointResp_.MergeFrom(byteString);
					if (achievementAcupointResp_.IsSuccess)
					{
						m_AcupointList = new List<string>();
						foreach (string item in achievementAcupointResp_.Acupoint)
						{
							m_AcupointList.Add(item);
						}
						ChaZao();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(achievementAcupointResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void ChaZao()
		{
			m_YiYouItemList = new List<ChuangZhaoGongFaItem>();
			m_YiYouPosList = new List<Vector3>();
			foreach (string acupoint in m_AcupointList)
			{
				foreach (ChuangZhaoGongFaItem zoItem in m_ZoItemList)
				{
					if (zoItem.GreenName.text == acupoint)
					{
						zoItem.Choose();
						if (!m_YiYouPosList.Contains(zoItem.gameObject.transform.position))
						{
							m_YiYouPosList.Add(zoItem.gameObject.transform.position);
						}
						if (!m_YiYouItemList.Contains(zoItem))
						{
							m_YiYouItemList.Add(zoItem);
						}
					}
				}
			}
			Panel.MaskImage.gameObject.SetActive(value: false);
		}

		private bool PanDuan(ChuangZhaoGongFaItem data)
		{
			foreach (string acupoint in m_AcupointList)
			{
				if (data.GreenName.text == acupoint)
				{
					return true;
				}
			}
			return false;
		}

		private void Init()
		{
			int num = 0;
			m_GongFaItemList = new List<ChuangZhaoGongFaItem>();
			m_ZoItemList = new List<ChuangZhaoGongFaItem>();
			m_PosList = new List<Vector3>();
			if (m_DataSource == null)
			{
				return;
			}
			foreach (Transform item in Panel.ItemContent)
			{
				if (m_DataSource.Count > num)
				{
					ChuangZhaoGongFaItem component = item.gameObject.GetComponent<ChuangZhaoGongFaItem>();
					component.SetValue(Panel, m_DataSource[num], ChooseItemClick, CancleChooseItemClick);
					m_ZoItemList.Add(component);
					num++;
				}
			}
		}

		private void RadomXueWei()
		{
			if (m_GongFaItemList != null)
			{
				foreach (ChuangZhaoGongFaItem gongFaItem in m_GongFaItemList)
				{
					gongFaItem.CancleChoose();
				}
			}
			m_GongFaItemList.Clear();
			m_PosList.Clear();
			List<int> list = new List<int>();
			RangeNum(list);
			foreach (int item in list)
			{
				Panel.ItemContent.GetChild(item).gameObject.GetComponent<ChuangZhaoGongFaItem>().Choose();
			}
		}

		private void RangeNum(List<int> data)
		{
			int item = Random.Range(0, 63);
			if (!data.Contains(item))
			{
				data.Add(item);
			}
			else if (data.Contains(item) && data.Count < 8)
			{
				RangeNum(data);
			}
		}

		private void ShowGuiZe()
		{
			string text = "功法创造步骤说明:\n1、点击穴位，可以查看穴位详情\n2、点击选定，当前穴位会被选定\n3、选择多个穴位开始模拟功法运行\n4、一周天后，即可得知是否创造成功\n功法创造要求：\n1、无法创造超过自身境界的功法\n2、无法创造超过自身布道师等阶的功法\n3、练气期功法8个穴位，每提升一个境界添加1个穴位\n4、创造成功扣除所选穴位个数与功法品阶乘积的悟性点，悟性可能出现负数\n5、创造失败扣除等同于所选穴位个数的悟性点\n\n\n";
			Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, text, null, "创造规则", 12);
		}

		private void WenHaoTogClick()
		{
			string text = $"灵气在经脉中的流通方式不同，可以产生各种不同的功法，其品阶也天差地别。不同的穴位，有不同的经脉归属、不同的导灵性，通过穴位顺序搭配，可以产生各种不同的功法。一阶功法需要选择8个穴位，每高一阶需要增加一个穴位，九阶功法则需要16个不同穴位。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "创造功法");
		}

		private void BuQuanWenHaoTogClick()
		{
			string text = $"1、功法补全时，上一篇的最后一个穴位为起点。 2、每次补全不可使用之前所使用的穴位。 3、每本功法需要补全2次，初次创造为上篇，第一次补全为中篇，最后补全为下篇。 4、每次补全需要使用的穴位为上篇的穴位个数减1。5、补全后消耗的悟性和初次创造时一致。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "补全功法");
		}

		private void ChooseItemClick(ChuangZhaoGongFaItem obj)
		{
			if (PanDuan(obj))
			{
				obj.Choose();
				return;
			}
			m_PosList.Add(obj.gameObject.transform.position);
			m_GongFaItemList.Add(obj);
		}

		private void CancleChooseItemClick(ChuangZhaoGongFaItem obj)
		{
			if (PanDuan(obj))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("补全功法时，无法取消！");
				obj.Choose();
				return;
			}
			if (m_PosList.Contains(obj.gameObject.transform.position))
			{
				m_PosList.Remove(obj.gameObject.transform.position);
			}
			if (m_GongFaItemList.Contains(obj))
			{
				m_GongFaItemList.Remove(obj);
			}
		}

		public void ChuangZhaoCompelete()
		{
			m_ChuangZhaoCompelete = true;
			if (m_RequesHaveDone)
			{
				if (m_ChuangZhaoGongFa == null)
				{
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(m_FailCode);
					ChuangZhaoShiBai(hintMsgByCode);
				}
				else
				{
					ChuangZhaoSuccess();
				}
			}
			else
			{
				MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			}
		}

		private void ChuangZhaoSuccess()
		{
			if (Panel.BiaoTiName.text == "创造功法")
			{
				wuxin = m_GongFaItemList.Count;
			}
			else
			{
				wuxin = m_GongFaItemList.Count + 1;
			}
			Singleton<UIManager>.Instance.PopUpWnd("ChuangZhaoGongFaJieGuoWnd.prefab", false, true, true, wuxin, m_ChuangZhaoGongFa);
			m_ChuangZhaoGongFa = null;
			m_FailCode = 0;
		}

		private void ChuangZhaoShiBai(string content)
		{
			if (Panel.BiaoTiName.text == "创造功法")
			{
				wuxin = m_GongFaItemList.Count;
			}
			else
			{
				wuxin = m_GongFaItemList.Count + 1;
			}
			Singleton<UIManager>.Instance.PopUpWnd("ChuangZhaoGongFaJieGuoWnd.prefab", false, true, false, wuxin, content);
			m_ChuangZhaoGongFa = null;
			m_FailCode = 0;
		}

		public void OnceAgainShow()
		{
			MonoSingleton<ChuangZhaoGongFaSceneManager>.Instance.ClearData();
			m_PosList.Clear();
			foreach (ChuangZhaoGongFaItem gongFaItem in m_GongFaItemList)
			{
				gongFaItem.CancleChoose();
			}
			m_ChuangZhaoGongFa = null;
			m_ChuangZhaoGongFa = null;
			m_GongFaItemList.Clear();
			m_ChuangZhaoCompelete = false;
			m_RequesHaveDone = false;
			Panel.Bg.gameObject.SetActive(value: true);
			Panel.BgMask.gameObject.SetActive(value: true);
			Panel.MoNiBtn.gameObject.SetActive(value: true);
			Init();
			ChaZao();
		}

		private bool IsCanChuanZhaoGongFa(out ChuangZhaoParamsModel resultModel)
		{
			int count = m_GongFaItemList.Count;
			List<string> list = new List<string>();
			int buDaoLevel = Singleton<RoleManager>.Instance.GetBuDaoLevel();
			int roleStage = Singleton<RoleManager>.Instance.GetRoleStage();
			int num = ((roleStage > buDaoLevel) ? buDaoLevel : roleStage);
			num = ((num == 0) ? 1 : num);
			foreach (ChuangZhaoGongFaItem gongFaItem in m_GongFaItemList)
			{
				list.Add(gongFaItem.m_DataSource.b);
			}
			resultModel = new ChuangZhaoParamsModel();
			resultModel.a = list;
			resultModel.b = num;
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentClever < count)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前悟性不足,至少需要" + count + "点悟性");
				resultModel = null;
				return false;
			}
			return true;
		}

		public void MoNiChuangZhaoBtnClick()
		{
			ChuangZhaoParamsModel resultModel = null;
			if (!IsCanChuanZhaoGongFa(out resultModel) || resultModel == null)
			{
				return;
			}
			VerifyCreateMethodAchievementReq_1008033 verifyCreateMethodAchievementReq_ = new VerifyCreateMethodAchievementReq_1008033();
			foreach (string item in resultModel.a)
			{
				verifyCreateMethodAchievementReq_.AcupointId.Add(item);
			}
			verifyCreateMethodAchievementReq_.Stage = resultModel.b;
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DJSD_CREATE, verifyCreateMethodAchievementReq_, (object _obj) =>
			{
				m_RequesHaveDone = true;
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCreateMethodAchievementResp_1008033 verifyCreateMethodAchievementResp_ = new VerifyCreateMethodAchievementResp_1008033();
					verifyCreateMethodAchievementResp_.MergeFrom(byteString);
					if (verifyCreateMethodAchievementResp_.IsSuccess)
					{
						wuxin = Singleton<RoleManager>.Instance.Role_Entity.CurrentClever - verifyCreateMethodAchievementResp_.CurrentClever;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodAchievementResp_.CurrentClever;
						if (verifyCreateMethodAchievementResp_.CreatedIsSuccess)
						{
							GetSuccessDataDetailList(verifyCreateMethodAchievementResp_);
						}
						else
						{
							m_ChuangZhaoGongFa = null;
							m_FailCode = verifyCreateMethodAchievementResp_.Code;
						}
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						m_ChuangZhaoGongFa = null;
						m_FailCode = verifyCreateMethodAchievementResp_.Code;
					}
				}
			});
			Panel.Bg.gameObject.SetActive(value: false);
			Panel.BgMask.gameObject.SetActive(value: false);
			Panel.MoNiBtn.gameObject.SetActive(value: false);
			MonoSingleton<ChuangZhaoGongFaSceneManager>.Instance.SetPoint(m_PosList);
			MonoSingleton<ChuangZhaoGongFaSceneManager>.Instance.StartLine();
		}

		private void GetSuccessDataDetailList(VerifyCreateMethodAchievementResp_1008033 data)
		{
			m_ChuangZhaoGongFa = new XSJ_GongFaModel();
			List<string> list = new List<string>();
			switch ((GongFaType)data.ActionScope)
			{
			case GongFaType.None:
				list.Add(string.Format("{0},{1}", "功法类型", "未知"));
				break;
			case GongFaType.LianShen:
				list.Add(string.Format("{0},{1}", "功法类型", "炼神"));
				break;
			case GongFaType.DuanTi:
				list.Add(string.Format("{0},{1}", "功法类型", "锻体"));
				break;
			case GongFaType.NaQi:
				list.Add(string.Format("{0},{1}", "功法类型", "纳气"));
				break;
			}
			data.Stage = ((data.Stage <= 0) ? 1 : data.Stage);
			list.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(data.Stage)));
			list.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetLevelByRankName(data.Rank)));
			list.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)data.FiveElement)));
			list.Add(string.Format("{0},{1}", "灵衍值", data.ConversionRate.ToString("0.00")));
			list.Add(string.Format("{0},{1}", "最大重数", data.MaxStep));
			if (data.EffectType1 != 0)
			{
				list.Add(string.Format("{0},{1}", "特效一", WorldSetting.GetGongFaEffectValuetType(data.EffectValue1)));
			}
			if (data.EffectType2 != 0)
			{
				list.Add(string.Format("{0},{1}", "特效二", WorldSetting.GetGongFaEffectValuetType(data.EffectValue2)));
			}
			list.Add(string.Format("{0},{1}", "创造人", Singleton<RoleManager>.Instance.Role_Entity.RoleName));
			m_ChuangZhaoGongFa.c = data.Rank;
			m_ChuangZhaoGongFa.g = list;
		}

		private void GetSuccessDataDetailList(VerifyCompletionMethodAchievementResp_1008080 data)
		{
			m_ChuangZhaoGongFa = new XSJ_GongFaModel();
			List<string> list = new List<string>();
			switch ((GongFaType)data.ActionScope)
			{
			case GongFaType.None:
				list.Add(string.Format("{0},{1}", "功法类型", "未知"));
				break;
			case GongFaType.LianShen:
				list.Add(string.Format("{0},{1}", "功法类型", "炼神"));
				break;
			case GongFaType.DuanTi:
				list.Add(string.Format("{0},{1}", "功法类型", "锻体"));
				break;
			case GongFaType.NaQi:
				list.Add(string.Format("{0},{1}", "功法类型", "纳气"));
				break;
			}
			data.Stage = ((data.Stage <= 0) ? 1 : data.Stage);
			list.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(data.Stage)));
			list.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetLevelByRankName(data.Rank)));
			list.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)data.FiveElement)));
			list.Add(string.Format("{0},{1}", "灵衍值", data.ConversionRate.ToString("0.00")));
			list.Add(string.Format("{0},{1}", "最大重数", data.MaxStep));
			if (data.EffectType1 != 0)
			{
				list.Add(string.Format("{0},{1}", "特效一", WorldSetting.GetGongFaEffectValuetType(data.EffectValue1)));
			}
			if (data.EffectType2 != 0)
			{
				list.Add(string.Format("{0},{1}", "特效二", WorldSetting.GetGongFaEffectValuetType(data.EffectValue2)));
			}
			list.Add(string.Format("{0},{1}", "创造人", Singleton<RoleManager>.Instance.Role_Entity.RoleName));
			m_ChuangZhaoGongFa.c = data.Rank;
			m_ChuangZhaoGongFa.g = list;
		}

		public void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
		}
	}
}
