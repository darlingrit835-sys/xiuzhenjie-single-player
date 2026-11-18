using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ChuangZaoFaShuWnd : Window
	{
		public ChuangZaoFaShuPanel Panel;

		private List<WorldHandSealModel> m_DataSource;

		private List<ChuangZaoFaShuItem> m_ChoosedDataSource;

		private List<ChuangZaoFaShuItem> m_ItemList;

		private Dictionary<string, string> m_AssociatedData;

		private GameObject m_BigItemObj;

		private bool isCreating;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChuangZaoFaShuPanel>();
			m_BigItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChuangZaoFaShuItem.prefab");
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.MoNiChuangZaoBtn, MoNiChuangZaoBtnClick);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			m_ItemList = new List<ChuangZaoFaShuItem>();
			m_ChoosedDataSource = new List<ChuangZaoFaShuItem>();
			m_AssociatedData = new Dictionary<string, string>();
			_ = m_DataSource;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.YIN_SELECT, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					HandSealListResp_1008053 handSealListResp_ = new HandSealListResp_1008053();
					handSealListResp_.MergeFrom(byteString);
					if (handSealListResp_.IsSuccess)
					{
						List<WorldHandSealModel> list = new List<WorldHandSealModel>();
						if (handSealListResp_.HandSeal != null)
						{
							foreach (HandSealListResp_1008053.Types.HandSealResp_1008064 item2 in handSealListResp_.HandSeal)
							{
								WorldHandSealModel item = new WorldHandSealModel
								{
									a = item2.Id,
									b = item2.Name,
									c = WorldSetting.GetWuXingName((WuXing)item2.FiveElement),
									d = item2.Rage
								};
								list.Add(item);
							}
						}
						m_DataSource = list;
						if (m_DataSource.Count != 0)
						{
							m_DataSource.Insert(1, null);
							m_DataSource.Insert(4, null);
							m_DataSource.Insert(31, null);
							m_DataSource.Insert(34, null);
							Init();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(handSealListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GuiZeBtNClick()
		{
			string text = "法术创造步骤说明:\n1、点击手印，可以查看手印详情\n2、点击选定，当前手印被选定\n3、选择多个手印后开始模拟印法\n4、印法按照顺序模拟结束后，即可得知是否创造成功\n法术创造要求：\n1、无法创造超过自身境界的法术\n2、无法创造超过自身术师等阶的法术\n3、练气期可选择5个手印，每提升一个境界添加1个手印\n4、创造成功扣除所选手印个数与法术品阶乘积的悟性点，悟性可能出现负数\n5、创造失败扣除等同于所选手印个数的悟性点\n\n\n";
			Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, text, null, "创造规则", 12);
		}

		private void WenHaoTogClick()
		{
			string text = $"术师通过手诀，引动灵气，不同的手诀引动灵气的结果截然不同。通过手诀的顺序搭配，可以创造出各种不同效果的术法。手诀的导灵等信息是创造法术成功与否的重要依据。一阶法术仅需5种手印即可，每高一阶都需要增加一个手印，九阶法术则需要13种手印。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "创造法术");
		}

		private void Init()
		{
			if (m_DataSource == null)
			{
				return;
			}
			for (int i = 0; i < m_DataSource.Count; i++)
			{
				ChuangZaoFaShuItem component = SetListContentParent(Panel.BigItemContent, m_BigItemObj).GetComponent<ChuangZaoFaShuItem>();
				if (i == 1 || ((i == 4) | (i == 31)) || i == 34)
				{
					component.SetValue(Panel, null, ItemClick);
					continue;
				}
				component.SetValue(Panel, m_DataSource[i], ItemClick);
				m_ItemList.Add(component);
				string value = "";
				if (i < 1)
				{
					value = $"ShouYin_{i + 1}";
				}
				else if (i > 1 && i < 4)
				{
					value = $"ShouYin_{i + 1 - 1}";
				}
				else if (i > 4 && i < 31)
				{
					value = $"ShouYin_{i + 1 - 2}";
				}
				else if (i > 31 && i < 34)
				{
					value = $"ShouYin_{i + 1 - 3}";
				}
				else if (i > 34)
				{
					value = $"ShouYin_{i + 1 - 4}";
				}
				m_AssociatedData.Add(m_DataSource[i].b, value);
			}
		}

		private void ItemClick(bool isOn, ChuangZaoFaShuItem data)
		{
			if (isOn)
			{
				if (m_ChoosedDataSource.Count >= 13)
				{
					data.ClearChoose();
					MonoSingleton<GameStart>.Instance.PopRollingTextShow("最多只允许选择13个手印！");
				}
				else
				{
					m_ChoosedDataSource.Add(data);
				}
			}
			else if (m_ChoosedDataSource.Contains(data))
			{
				m_ChoosedDataSource.Remove(data);
			}
		}

		private void MoNiChuangZaoBtnClick()
		{
			if (isCreating)
			{
				return;
			}
			ChuangZhaoParamsModel resultModel = null;
			if (!IsCanChuanZhaoGongFa(out resultModel))
			{
				return;
			}
			isCreating = true;
			if (resultModel == null)
			{
				return;
			}
			VerifyCreateMethodSkillReq_1008025 verifyCreateMethodSkillReq_ = new VerifyCreateMethodSkillReq_1008025();
			foreach (string item in resultModel.a)
			{
				verifyCreateMethodSkillReq_.HandSealId.Add(item);
			}
			verifyCreateMethodSkillReq_.Stage = resultModel.b;
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_WFG_CREATE, verifyCreateMethodSkillReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCreateMethodSkillResp_1008025 verifyCreateMethodSkillResp_ = new VerifyCreateMethodSkillResp_1008025();
					verifyCreateMethodSkillResp_.MergeFrom(byteString);
					if (verifyCreateMethodSkillResp_.IsSuccess)
					{
						if (verifyCreateMethodSkillResp_.CreatedIsSuccess)
						{
							XSJ_FaShuEntity result = new XSJ_FaShuEntity
							{
								Rank = verifyCreateMethodSkillResp_.Rank,
								DetailList = GetSuccessDataDetailList(verifyCreateMethodSkillResp_)
							};
							MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(StartJieYinAni(result));
							Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodSkillResp_.CurrentClever;
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodSkillResp_.Code);
							MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(StartJieYinAni(hintMsgByCode));
						}
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(StartJieYinAni(hintMsgByCode2));
					}
				}
			}, isShowWaitUI: true);
		}

		private void RadomXueWei()
		{
			if (m_ItemList != null)
			{
				foreach (ChuangZaoFaShuItem item in m_ChoosedDataSource)
				{
					item.ClearChoose();
				}
			}
			m_ChoosedDataSource.Clear();
			List<int> data = new List<int>();
			RangeNum(data);
			foreach (ChuangZaoFaShuItem item2 in m_ItemList)
			{
				item2.Choose();
			}
		}

		private void RangeNum(List<int> data)
		{
			int item = Random.Range(0, m_DataSource.Count);
			if (!data.Contains(item))
			{
				data.Add(item);
			}
			else if (data.Contains(item) && data.Count < 4)
			{
				RangeNum(data);
			}
		}

		private List<string> GetSuccessDataDetailList(VerifyCreateMethodSkillResp_1008025 data)
		{
			List<string> list = new List<string>();
			switch ((FaShuType)data.SkillType)
			{
			case FaShuType.gongji_suoding:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-锁定攻击"));
				break;
			case FaShuType.gongji_dingxiang:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-定向攻击"));
				break;
			case FaShuType.gongji_chixu:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-持续攻击"));
				list.Add(string.Format("{0},{1}", "持续时间", data.ContinueTime / 1000 + "息"));
				break;
			case FaShuType.gongji_fanwei:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-范围攻击"));
				break;
			case FaShuType.gongji_fanwei_chixu:
				list.Add(string.Format("{0},{1}", "技能类型", "攻击-范围持续攻击"));
				list.Add(string.Format("{0},{1}", "持续时间", data.ContinueTime / 1000 + "息"));
				break;
			case FaShuType.fangyu_fashu_hudun:
				list.Add(string.Format("{0},{1}", "技能类型", "防御-法术护盾"));
				break;
			case FaShuType.zengyi_shanghai:
				list.Add(string.Format("{0},{1}", "技能类型", "增益-伤害增益"));
				break;
			case FaShuType.zengyi_fangyu:
				list.Add(string.Format("{0},{1}", "技能类型", "增益-防御增益"));
				break;
			case FaShuType.kongzhi_yisu:
				list.Add(string.Format("{0},{1}", "技能类型", "控制-减低移速"));
				break;
			case FaShuType.kongzhi_xingdong:
				list.Add(string.Format("{0},{1}", "技能类型", "控制-限制行动"));
				break;
			case FaShuType.dunshu_wuxing:
				list.Add(string.Format("{0},{1}", "技能类型", "遁术-五行遁术"));
				break;
			case FaShuType.dunshu_yujian:
				list.Add(string.Format("{0},{1}", "技能类型", "遁术-御剑术"));
				break;
			}
			data.Stage = ((data.Stage <= 0) ? 1 : data.Stage);
			list.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(data.Stage)));
			list.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetLevelByRankName(data.Rank)));
			list.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)data.FiveElement)));
			list.Add(string.Format("{0},{1}", "最低消耗", data.MinSpiritualityValue));
			list.Add(string.Format("{0},{1}", "最大消耗", data.MaxSpiritualityValue));
			list.Add(string.Format("{0},{1}", "施法时间", data.CastTime / 1000 + "息"));
			list.Add(string.Format("{0},{1}", "灵衍值", data.ConversionRate.ToString("0.00")));
			list.Add(string.Format("{0},{1}", "创造人", Singleton<RoleManager>.Instance.Role_Entity.RoleName));
			return list;
		}

		public void OnceAgainShow()
		{
			foreach (Transform item in Panel.BigItemContent)
			{
				Object.Destroy(item.gameObject);
			}
			Panel.SmallContent.ClearAllImg();
			m_ChoosedDataSource.Clear();
			m_ItemList.Clear();
			m_AssociatedData.Clear();
			Init();
		}

		private bool IsCanChuanZhaoGongFa(out ChuangZhaoParamsModel resultModel)
		{
			int shuShiLevel = Singleton<RoleManager>.Instance.GetShuShiLevel();
			int roleStage = Singleton<RoleManager>.Instance.GetRoleStage();
			int num = ((roleStage > shuShiLevel) ? shuShiLevel : roleStage);
			num = ((num == 0) ? 1 : num);
			_ = m_ChoosedDataSource.Count;
			List<string> list = new List<string>();
			foreach (ChuangZaoFaShuItem item in m_ChoosedDataSource)
			{
				list.Add(item.m_DataSource.b);
			}
			resultModel = new ChuangZhaoParamsModel();
			resultModel.a = list;
			resultModel.b = num;
			return true;
		}

		private void CreateSuccess(XSJ_FaShuEntity result)
		{
			isCreating = false;
			Singleton<UIManager>.Instance.PopUpWnd("ChuangZhaoFaShuJieGuoWnd.prefab", false, true, true, m_ChoosedDataSource.Count, result);
		}

		private void CreateFail(string codeContent)
		{
			isCreating = false;
			Singleton<UIManager>.Instance.PopUpWnd("ChuangZhaoFaShuJieGuoWnd.prefab", false, true, false, m_ChoosedDataSource.Count, codeContent);
		}

		private IEnumerator StartJieYinAni(XSJ_FaShuEntity result)
		{
			Panel.BigItemContent.gameObject.SetActive(value: false);
			for (int i = 0; i < m_ChoosedDataSource.Count; i++)
			{
				Panel.SmallContent.AddItemImg(m_ChoosedDataSource[i].m_DataSource.b);
				Panel.JieYinPanel.ShowImg(Panel.JieYinImg, m_ChoosedDataSource[i].m_DataSource.b, m_AssociatedData);
				yield return new WaitForSeconds(0.5f);
			}
			Panel.JieYinImg.gameObject.SetActive(value: false);
			Panel.BigItemContent.gameObject.SetActive(value: true);
			CreateSuccess(result);
		}

		private IEnumerator StartJieYinAni(string content)
		{
			Panel.BigItemContent.gameObject.SetActive(value: false);
			for (int i = 0; i < m_ChoosedDataSource.Count; i++)
			{
				Panel.SmallContent.AddItemImg(m_ChoosedDataSource[i].m_DataSource.b);
				Panel.JieYinPanel.ShowImg(Panel.JieYinImg, m_ChoosedDataSource[i].m_DataSource.b, m_AssociatedData);
				yield return new WaitForSeconds(0.5f);
			}
			Panel.JieYinImg.gameObject.SetActive(value: false);
			Panel.BigItemContent.gameObject.SetActive(value: true);
			CreateFail(content);
		}

		public void LiKaiBtnClick()
		{
			foreach (Transform item in Panel.BigItemContent)
			{
				Object.Destroy(item.gameObject);
			}
			Panel.SmallContent.ClearAllImg();
			m_ChoosedDataSource.Clear();
			m_ItemList.Clear();
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
		}
	}
}
