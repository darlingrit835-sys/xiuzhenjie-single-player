using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiBuZhouSheZhiWnd : Window
	{
		public LianQiBuZhouSheZhiPanel Panel;

		private int m_CurrentDengJie;

		private int m_CurrentGoodsType;

		private int m_CurrentStepNum;

		private List<KaiShiLianZhiStepModel> m_AddStepList;

		private List<LianZhiBuZhouSheZhiItem> m_StepItemObjList;

		private bool m_IsOpenRest;

		private int m_ChooseIndex;

		private GameObject m_StepItemObj;

		private int m_TouZhiTimes;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiBuZhouSheZhiPanel>();
			m_AddStepList = new List<KaiShiLianZhiStepModel>();
			m_StepItemObjList = new List<LianZhiBuZhouSheZhiItem>();
			if (paralist.Length != 0)
			{
				m_CurrentDengJie = int.Parse(paralist[0].ToString());
				m_CurrentGoodsType = int.Parse(paralist[1].ToString());
			}
			AddButtonClickListener(Panel.LiKaiButton, CloseWindow);
			AddButtonClickListener(Panel.LeftTouZhiBtn, TouZhiCaoYaoClick);
			AddButtonClickListener(Panel.RightTouZhiBtn, TouZhiCaoYaoClick);
			AddButtonClickListener(Panel.WenHaoBtn, WenHaoTogClick);
			AddButtonClickListener(Panel.LeftCancleBtn, () =>
			{
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			});
			AddButtonClickListener(Panel.RightCancleBtn, () =>
			{
				ShowStepPanel(isLeft: false, 0f, 0f, isShow: false);
			});
			AddButtonClickListener(Panel.AddStepBtn, AddStep);
			AddButtonClickListener(Panel.KaiShiLianZhiBUtton, KaiShiLianZhi);
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
				float x = component.position.x + 0.5f;
				float y = component.position.y - 0.9f;
				ShowStepPanel(isLeft: true, x, y);
			}
			else
			{
				float x2 = component.position.x - 0.5f;
				float y2 = component.position.y - 0.9f;
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
				float x = targetItem.position.x + 0.5f;
				float y = targetItem.position.y - 0.9f;
				ShowStepPanel(isLeft: true, x, y);
			}
			else
			{
				float x2 = targetItem.position.x - 0.5f;
				float y2 = targetItem.position.y - 0.9f;
				ShowStepPanel(isLeft: false, x2, y2);
			}
		}

		private void TouZhiCaoYaoClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LianQiCaiLiaoXuanZeShaiXuanWnd.prefab", true, m_CurrentDengJie);
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

		public void ChoosedCaiLiao(WorldCaiLiaoModel chooseCaiLiao)
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
					if (j == m_ChooseIndex - 1)
					{
						lianZhiBuZhouSheZhiItem = m_StepItemObjList[j];
						break;
					}
				}
				if (kaiShiLianZhiStepModel != null && lianZhiBuZhouSheZhiItem != null)
				{
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
					if (j == int.Parse(index) - 1)
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

		private void KaiShiLianZhi()
		{
			VerifyCreateMethodEquipmentReq_1008073 verifyCreateMethodEquipmentReq_ = new VerifyCreateMethodEquipmentReq_1008073
			{
				EquipmentType = m_CurrentGoodsType
			};
			KaiShiLianZhiModel obj = new KaiShiLianZhiModel
			{
				b = m_CurrentDengJie,
				a = m_AddStepList
			};
			m_TouZhiTimes = 0;
			foreach (KaiShiLianZhiStepModel item in obj.a)
			{
				VerifyCreateMethodEquipmentReq_1008073.Types.EquipmentStepObjectReq_1008074 equipmentStepObjectReq_ = new VerifyCreateMethodEquipmentReq_1008073.Types.EquipmentStepObjectReq_1008074();
				equipmentStepObjectReq_.Order = item.d;
				equipmentStepObjectReq_.TargetId = (string.IsNullOrEmpty(item.b) ? "" : item.b);
				if (item.c == 3)
				{
					equipmentStepObjectReq_.TargetType = 12;
				}
				else
				{
					equipmentStepObjectReq_.TargetType = item.c;
				}
				verifyCreateMethodEquipmentReq_.Object.Add(equipmentStepObjectReq_);
				if (!string.IsNullOrEmpty(item.b))
				{
					m_TouZhiTimes++;
				}
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentClever < m_TouZhiTimes)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前悟性不足!至少需要" + m_TouZhiTimes + "点悟性!");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFY_CREATE_METHOD_EQUIPMENT, verifyCreateMethodEquipmentReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCreateMethodEquipmentResp_1008073 verifyCreateMethodEquipmentResp_ = new VerifyCreateMethodEquipmentResp_1008073();
					verifyCreateMethodEquipmentResp_.MergeFrom(byteString);
					if (verifyCreateMethodEquipmentResp_.IsSuccess)
					{
						ReceiveChuangZaoResult(verifyCreateMethodEquipmentResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ReceiveChuangZaoResult(VerifyCreateMethodEquipmentResp_1008073 verifyCreateMethodElixirResp_1008018)
		{
			if (!verifyCreateMethodElixirResp_1008018.CreatedIsSuccess)
			{
				CreateFail(verifyCreateMethodElixirResp_1008018);
			}
			else
			{
				CreateSuccess(verifyCreateMethodElixirResp_1008018);
			}
			Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodElixirResp_1008018.CurrentClever;
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
		}

		private void CreateSuccess(VerifyCreateMethodEquipmentResp_1008073 data)
		{
			List<string> equipmentDetail = GetEquipmentDetail(data);
			int num = Singleton<RoleManager>.Instance.Role_Entity.CurrentClever - data.CurrentClever;
			Singleton<UIManager>.Instance.PopUpWnd("LianQiCreateResultWnd.prefab", false, true, true, num, equipmentDetail);
		}

		private void CreateFail(VerifyCreateMethodEquipmentResp_1008073 data)
		{
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(data.Code);
			int num = Singleton<RoleManager>.Instance.Role_Entity.CurrentClever - data.CurrentClever;
			Singleton<UIManager>.Instance.PopUpWnd("LianQiCreateResultWnd.prefab", false, true, false, num, hintMsgByCode);
		}

		private List<string> GetEquipmentDetail(VerifyCreateMethodEquipmentResp_1008073 data)
		{
			_ = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
			List<string> list = new List<string>();
			list.Add(string.Format("{0},{1}", "类型", GetQiTuTypeName(data.Type)));
			list.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(data.Stage)));
			list.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)data.FiveElement)));
			list.Add(string.Format("{0},{1}", "可炼制最小品阶", WorldSetting.GetRankAllName(data.MinRank)));
			list.Add(string.Format("{0},{1}", "可炼制最大品阶", WorldSetting.GetRankAllName(data.MaxRank)));
			if (data.Type == 16 || data.Type == 29)
			{
				list.Add(string.Format("{0},{1}", "占用体魄", data.UsedPhysique));
			}
			else if (data.Type == 15 || data.Type == 26)
			{
				list.Add(string.Format("{0},{1}", "占用神识", data.UsedPerception));
				if (data.Stage > 6)
				{
					list.Add(string.Format("{0},{1}", "激发真元值", data.MinSpiritualityValue / 100 / 100));
					list.Add(string.Format("{0},{1}", "最大可承受真元值", data.MaxSpiritualityValue / 100 / 100));
				}
				else if (data.Stage > 3)
				{
					list.Add(string.Format("{0},{1}", "激发真气值", data.MinSpiritualityValue / 100));
					list.Add(string.Format("{0},{1}", "最大可承受真气值", data.MaxSpiritualityValue / 100));
				}
				else if (data.Stage > 0)
				{
					list.Add(string.Format("{0},{1}", "激发灵气值", data.MinSpiritualityValue));
					list.Add(string.Format("{0},{1}", "最大可承受灵气值", data.MaxSpiritualityValue));
				}
			}
			list.Add(string.Format("{0},{1}", "体积", data.Volume));
			list.Add(string.Format("{0},{1}", "创造人", data.CreatedBy));
			return list;
		}

		private string GetQiTuTypeName(int type)
		{
			return type switch
			{
				15 => "可配置攻击法器", 
				16 => "可穿戴防御法器", 
				26 => "可配置防御法器", 
				29 => "靴", 
				_ => "无", 
			};
		}

		private void WenHaoTogClick()
		{
			string text = "1、练气期的法器需要5个材料，每个等阶需要多加入一个材料。 2、材料可选择矿物、植物、动物皮毛和骨骼。 3、创造成功后无需炼制，经过虚神界推演会直接给出结果。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "器图创造规则");
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
