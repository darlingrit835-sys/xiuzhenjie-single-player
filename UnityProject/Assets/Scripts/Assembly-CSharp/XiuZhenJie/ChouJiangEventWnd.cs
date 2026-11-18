using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ChouJiangEventWnd : Window
	{
		public ChouJiangEventPanel Panel;

		private float m_RotateSpeed;

		private int m_StartedIndex;

		private int m_TargetGoodsIndex;

		private int m_CurrentChooseGoodsIndex;

		private float m_IntervalTime;

		private float m_CurrentTime;

		private int m_NumberOfTurns;

		private ZhuanPanStatus m_ZhuanPanStatus;

		private bool m_IsTen;

		private int m_CurrentChooseStage;

		private Dictionary<int, List<EventGoodsEntity>> m_EventGoodsDic;

		private Dictionary<int, Sprite> m_RankSpriteDic;

		private Dictionary<int, Sprite> m_IconSpriteDic;

		private BaseNaJieGoodEntity m_OneGoods;

		private List<BaseNaJieGoodEntity> m_TenGoods;

		private GameObject m_UIGoods_Item;

		private long m_CurrentJiFen;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChouJiangEventPanel>();
			m_EventGoodsDic = Singleton<FacadeManager>.Instance.SelectEventGoods();
			m_TenGoods = new List<BaseNaJieGoodEntity>();
			AddButtonClickListener(Panel.OneBtn, OneBtnClick);
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.TenBtn, TenBtnClick);
			AddButtonClickListener(Panel.ShouQuBtn, ShiQuClick);
			AddButtonClickListener(Panel.OpenJifenBtn, OpenJifenWnd);
			Panel.StageDropDown.onValueChanged.AddListener(StageValueChange);
			m_UIGoods_Item = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_RankSpriteDic = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			m_IconSpriteDic = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_CurrentChooseStage = 1;
			Init();
			ReflashStageData();
			GetServerData();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ReflashLingYuXiaoHao();
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_ZhuanPanStatus != ZhuanPanStatus.Stop)
			{
				m_CurrentTime += Time.fixedDeltaTime * m_RotateSpeed;
				OneGetRotate();
			}
		}

		private void ReflashLingYuXiaoHao()
		{
			int num = 20;
			int num2 = num * 10;
			Panel.OncePriceText.text = num * m_CurrentChooseStage + "灵玉";
			Panel.TenPriceText.text = num2 * m_CurrentChooseStage + "灵玉";
		}

		private bool ShowEventBtn()
		{
			HuoDongEntity huoDongEntity = null;
			for (int i = 0; i < Singleton<RoleManager>.Instance.EventInfoList.Count; i++)
			{
				if (Singleton<RoleManager>.Instance.EventInfoList[i].type == 2)
				{
					huoDongEntity = Singleton<RoleManager>.Instance.EventInfoList[i];
					break;
				}
			}
			if (huoDongEntity == null)
			{
				return false;
			}
			long createtime = CommonMethod.GetCreatetime();
			if (createtime >= huoDongEntity.startAt && createtime < huoDongEntity.endAt)
			{
				Panel.TenBtn.gameObject.SetActive(value: true);
				Panel.OneBtn.gameObject.SetActive(value: true);
				return true;
			}
			Panel.TenBtn.gameObject.SetActive(value: false);
			Panel.OneBtn.gameObject.SetActive(value: false);
			return false;
		}

		private void Init()
		{
			m_RotateSpeed = 1f;
			m_TargetGoodsIndex = 7;
			m_CurrentChooseGoodsIndex = 0;
			m_IntervalTime = 0.04f;
			m_ZhuanPanStatus = ZhuanPanStatus.Stop;
			m_StartedIndex = 0;
			m_NumberOfTurns = 0;
		}

		private void GetServerData()
		{
			EnterChouJiangChiReq_1017001 data = new EnterChouJiangChiReq_1017001
			{
				Stage = m_CurrentChooseStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.EnterChouJiangChi, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterChouJiangChiResp_1017001 enterChouJiangChiResp_ = new EnterChouJiangChiResp_1017001();
					enterChouJiangChiResp_.MergeFrom(byteString);
					if (enterChouJiangChiResp_.IsSuccess)
					{
						m_CurrentJiFen = enterChouJiangChiResp_.CurrentChouJiangJiFen;
						Panel.ChouQuTimesText.text = $"已抽取:{enterChouJiangChiResp_.Count}次";
						Panel.CurrentJiFenText.text = $"当前积分:{enterChouJiangChiResp_.CurrentChouJiangJiFen}";
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterChouJiangChiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ReflashStageData()
		{
			List<EventGoodsEntity> value = new List<EventGoodsEntity>();
			if (m_EventGoodsDic.TryGetValue(m_CurrentChooseStage, out value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					EventGoodsEntity eventGoodsEntity = value[i];
					Panel.ChouJiangItemList[i].SetValue(Panel.ContentGroup, null, eventGoodsEntity.Name, eventGoodsEntity.FiveElement, eventGoodsEntity.Level, eventGoodsEntity.Rank, eventGoodsEntity.GoodsType, EventGoodsClick, eventGoodsEntity.Num);
				}
			}
		}

		private void EventGoodsClick(object obj, GameObject gameObj)
		{
		}

		private void StageValueChange(int index)
		{
			m_CurrentChooseStage = index + 1;
			ReflashLingYuXiaoHao();
			ReflashStageData();
			GetServerData();
		}

		private void OneGetRotate()
		{
			if (!(m_CurrentTime >= m_IntervalTime))
			{
				return;
			}
			if (m_NumberOfTurns == 11 && m_CurrentChooseGoodsIndex == m_TargetGoodsIndex)
			{
				m_ZhuanPanStatus = ZhuanPanStatus.Stop;
				if (!m_IsTen)
				{
					ComputeOneTimesEndIndex();
				}
				else
				{
					ComputeTenTimesEndIndex();
				}
				return;
			}
			m_CurrentTime = 0f;
			m_CurrentChooseGoodsIndex++;
			if (m_CurrentChooseGoodsIndex >= 8)
			{
				m_CurrentChooseGoodsIndex = 0;
			}
			Panel.ChouJiangItemList[m_CurrentChooseGoodsIndex].SetToggleSwitch(isOn: true);
			if (m_CurrentChooseGoodsIndex == m_StartedIndex)
			{
				m_NumberOfTurns++;
			}
			if (m_NumberOfTurns == 10)
			{
				m_ZhuanPanStatus = ZhuanPanStatus.ReadyStop;
			}
			if (m_ZhuanPanStatus == ZhuanPanStatus.ReadyStop)
			{
				m_IntervalTime += 0.05f;
			}
		}

		private void TenGetRotate()
		{
			if (!(m_CurrentTime >= m_IntervalTime))
			{
				return;
			}
			if (m_NumberOfTurns == 14 && m_CurrentChooseGoodsIndex == m_TargetGoodsIndex)
			{
				m_ZhuanPanStatus = ZhuanPanStatus.Stop;
				ComputeTenTimesEndIndex();
				return;
			}
			m_CurrentTime = 0f;
			m_CurrentChooseGoodsIndex++;
			if (m_CurrentChooseGoodsIndex >= 8)
			{
				m_CurrentChooseGoodsIndex = 0;
			}
			Panel.ChouJiangItemList[m_CurrentChooseGoodsIndex].SetToggleSwitch(isOn: true);
			if (m_CurrentChooseGoodsIndex == m_StartedIndex)
			{
				m_NumberOfTurns++;
			}
			if (m_NumberOfTurns == 10)
			{
				m_ZhuanPanStatus = ZhuanPanStatus.ReadyStop;
			}
		}

		private void OneBtnClick()
		{
			if (m_ZhuanPanStatus != ZhuanPanStatus.Stop)
			{
				return;
			}
			ChouJiangReq_1017002 data = new ChouJiangReq_1017002
			{
				IsContinuous = false,
				Stage = m_CurrentChooseStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ChouJiang, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChouJiangResp_1017002 chouJiangResp_ = new ChouJiangResp_1017002();
					chouJiangResp_.MergeFrom(byteString);
					if (chouJiangResp_.IsSuccess)
					{
						Panel.StageDropDown.interactable = false;
						foreach (ChouJiangResp_1017002.Types.GoodsInfoResp_1017003 item in chouJiangResp_.GoodsInfo)
						{
							m_TargetGoodsIndex = item.Id - 1;
							m_OneGoods = new BaseNaJieGoodEntity
							{
								Name = item.Name,
								GoodsId = item.GoodsId,
								Rank = item.Rank,
								Level = item.Level,
								FiveElement = item.FiveElement,
								Type = item.Type,
								GoodsNum = item.Count,
								TemplateId = item.TemplateId
							};
						}
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = chouJiangResp_.CurrentShinyJade;
						Panel.ChouQuTimesText.text = $"已抽取:{chouJiangResp_.Count}次";
						Panel.CurrentJiFenText.text = $"当前积分:{chouJiangResp_.CurrentChouJiangJiFen}";
						m_CurrentJiFen = chouJiangResp_.CurrentChouJiangJiFen;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						m_IsTen = false;
						m_StartedIndex = m_CurrentChooseGoodsIndex;
						m_ZhuanPanStatus = ZhuanPanStatus.Running;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(chouJiangResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void TenBtnClick()
		{
			if (m_ZhuanPanStatus != ZhuanPanStatus.Stop)
			{
				return;
			}
			ChouJiangReq_1017002 data = new ChouJiangReq_1017002
			{
				IsContinuous = true,
				Stage = m_CurrentChooseStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ChouJiang, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChouJiangResp_1017002 chouJiangResp_ = new ChouJiangResp_1017002();
					chouJiangResp_.MergeFrom(byteString);
					m_TenGoods.Clear();
					if (chouJiangResp_.IsSuccess)
					{
						Panel.StageDropDown.interactable = false;
						foreach (ChouJiangResp_1017002.Types.GoodsInfoResp_1017003 item2 in chouJiangResp_.GoodsInfo)
						{
							m_TargetGoodsIndex = item2.Id - 1;
							BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
							{
								Name = item2.Name,
								GoodsId = item2.GoodsId,
								Rank = item2.Rank,
								Level = item2.Level,
								FiveElement = item2.FiveElement,
								Type = item2.Type,
								GoodsNum = item2.Count
							};
							m_TenGoods.Add(item);
						}
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = chouJiangResp_.CurrentShinyJade;
						Panel.ChouQuTimesText.text = $"已抽取:{chouJiangResp_.Count}次";
						Panel.CurrentJiFenText.text = $"当前积分:{chouJiangResp_.CurrentChouJiangJiFen}";
						m_CurrentJiFen = chouJiangResp_.CurrentChouJiangJiFen;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						m_StartedIndex = m_CurrentChooseGoodsIndex;
						m_IsTen = true;
						m_ZhuanPanStatus = ZhuanPanStatus.Running;
					}
					else
					{
						Debug.Log("十连抽失败");
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(chouJiangResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ComputeOneTimesEndIndex()
		{
			Init();
			if (m_OneGoods != null)
			{
				Panel.JiangLiPanel.gameObject.SetActive(value: true);
				Panel.OneGoodsItem.gameObject.SetActive(value: true);
				Sprite value = null;
				Sprite value2 = null;
				int levelByRank = WorldSetting.GetLevelByRank(m_OneGoods.Rank);
				m_RankSpriteDic.TryGetValue(levelByRank, out value);
				m_IconSpriteDic.TryGetValue(m_OneGoods.Type, out value2);
				Panel.OneGoodsItem.SetItem(m_OneGoods, m_OneGoods.Name, m_OneGoods.FiveElement, m_OneGoods.Level, value, value2, JiangLiGoodsClick, isUsing: false, m_OneGoods.GoodsNum);
				Panel.StageDropDown.interactable = true;
			}
		}

		private void ComputeTenTimesEndIndex()
		{
			Init();
			Panel.JiangLiPanel.gameObject.SetActive(value: true);
			Panel.TenJiangLiGoodsContent.gameObject.SetActive(value: true);
			if (m_TenGoods.Count >= 10)
			{
				for (int i = 0; i < 10; i++)
				{
					GameObject gameObject = SetListContentParent(Panel.TenJiangLiGoodsContent, m_UIGoods_Item);
					BaseNaJieGoodEntity baseNaJieGoodEntity = m_TenGoods[i];
					UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
					Sprite value = null;
					Sprite value2 = null;
					int levelByRank = WorldSetting.GetLevelByRank(baseNaJieGoodEntity.Rank);
					m_RankSpriteDic.TryGetValue(levelByRank, out value);
					m_IconSpriteDic.TryGetValue(baseNaJieGoodEntity.Type, out value2);
					component.SetItem(baseNaJieGoodEntity, baseNaJieGoodEntity.Name, baseNaJieGoodEntity.FiveElement, baseNaJieGoodEntity.Level, value, value2, JiangLiGoodsClick, isUsing: false, baseNaJieGoodEntity.GoodsNum);
				}
				Panel.StageDropDown.interactable = true;
			}
		}

		private void JiangLiGoodsClick(object obj, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = obj as BaseNaJieGoodEntity;
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(baseNaJieGoodEntity, (List<string> strList) =>
			{
				Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
				{
					TitleName = baseNaJieGoodEntity.Name,
					ContentList = strList
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
			});
		}

		private void ShiQuClick()
		{
			foreach (Transform item in Panel.TenJiangLiGoodsContent)
			{
				Object.Destroy(item.gameObject);
			}
			Panel.OneGoodsItem.gameObject.SetActive(value: false);
			Panel.TenJiangLiGoodsContent.gameObject.SetActive(value: false);
			Panel.JiangLiPanel.gameObject.SetActive(value: false);
		}

		private void OpenJifenWnd()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.EnterJiFen, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterJiFenResp_1017004 enterJiFenResp_ = new EnterJiFenResp_1017004();
					enterJiFenResp_.MergeFrom(byteString);
					if (enterJiFenResp_.IsSuccess)
					{
						List<bool> list = new List<bool>();
						foreach (EnterJiFenResp_1017004.Types.JiFenResp_1017005 item in enterJiFenResp_.JiFen)
						{
							list.Add(item.IsGet);
						}
						Singleton<UIManager>.Instance.PopUpWnd("JiFenDuiHuanWnd.prefab", true, m_CurrentJiFen, list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterJiFenResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnClose()
		{
			base.OnClose();
			Panel.StageDropDown.onValueChanged.RemoveAllListeners();
		}
	}
}
