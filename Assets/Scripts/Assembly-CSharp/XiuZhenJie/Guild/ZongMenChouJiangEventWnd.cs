using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ZongMenChouJiangEventWnd : Window
	{
		public ZongMenChouJiangEventPanel Panel;

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

		private int m_ZongMenStage;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenChouJiangEventPanel>();
			m_EventGoodsDic = Singleton<FacadeManager>.Instance.SelectEventGoods();
			AddButtonClickListener(Panel.OneBtn, OneBtnClick);
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.TenBtn, TenBtnClick);
			AddButtonClickListener(Panel.ShouQuBtn, ShiQuClick);
			m_TenGoods = new List<BaseNaJieGoodEntity>();
			m_UIGoods_Item = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_RankSpriteDic = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			m_IconSpriteDic = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_ZongMenStage = int.Parse(paralist[0].ToString());
			m_CurrentChooseStage = m_ZongMenStage;
			Init();
			GetServerData();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
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
			foreach (ChouJiangEventItem chouJiangItem in Panel.ChouJiangItemList)
			{
				chouJiangItem.GoodsItem.ResetLevel(WorldSetting.GetLevelByStage(m_ZongMenStage));
			}
		}

		private void GetServerData()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.EnterGuildChouJiangChi, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterGuildChouJiangChiResp_1015155 enterGuildChouJiangChiResp_ = new EnterGuildChouJiangChiResp_1015155();
					enterGuildChouJiangChiResp_.MergeFrom(byteString);
					if (enterGuildChouJiangChiResp_.IsSuccess)
					{
						Panel.ChouQuTimesText.text = $"剩余次数：{enterGuildChouJiangChiResp_.Count}次";
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterGuildChouJiangChiResp_.Code);
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
			GuildChouJiangReq_1015156 data = new GuildChouJiangReq_1015156
			{
				IsContinuous = false,
				Stage = m_ZongMenStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildChouJiang, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildChouJiangResp_1015156 guildChouJiangResp_ = new GuildChouJiangResp_1015156();
					guildChouJiangResp_.MergeFrom(byteString);
					if (guildChouJiangResp_.IsSuccess)
					{
						Panel.StageDropDown.interactable = false;
						foreach (GuildChouJiangResp_1015156.Types.GoodsInfoResp_1015157 item in guildChouJiangResp_.GoodsInfo)
						{
							m_TargetGoodsIndex = item.Id - 1;
							m_OneGoods = new BaseNaJieGoodEntity
							{
								Name = (string.IsNullOrEmpty(item.Name) ? "宗门贡献" : item.Name),
								GoodsId = item.GoodsId,
								Rank = ((item.Rank == 0) ? 1 : item.Rank),
								Level = ((item.Level == 0) ? WorldSetting.GetLevelByStage(m_ZongMenStage) : item.Level),
								FiveElement = item.FiveElement,
								Type = item.Type,
								GoodsNum = item.Count,
								TemplateId = item.TemplateId
							};
						}
						Panel.ChouQuTimesText.text = $"剩余次数：{guildChouJiangResp_.Count}次";
						m_IsTen = false;
						m_StartedIndex = m_CurrentChooseGoodsIndex;
						m_ZhuanPanStatus = ZhuanPanStatus.Running;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildChouJiangResp_.Code);
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
			GuildChouJiangReq_1015156 data = new GuildChouJiangReq_1015156
			{
				IsContinuous = true,
				Stage = m_ZongMenStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildChouJiang, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GuildChouJiangResp_1015156 guildChouJiangResp_ = new GuildChouJiangResp_1015156();
					guildChouJiangResp_.MergeFrom(byteString);
					m_TenGoods.Clear();
					if (guildChouJiangResp_.IsSuccess)
					{
						Panel.StageDropDown.interactable = false;
						foreach (GuildChouJiangResp_1015156.Types.GoodsInfoResp_1015157 item2 in guildChouJiangResp_.GoodsInfo)
						{
							m_TargetGoodsIndex = item2.Id - 1;
							BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
							{
								Name = (string.IsNullOrEmpty(item2.Name) ? "宗门贡献" : item2.Name),
								GoodsId = item2.GoodsId,
								Rank = ((item2.Rank == 0) ? 1 : item2.Rank),
								Level = ((item2.Level == 0) ? WorldSetting.GetLevelByStage(m_ZongMenStage) : item2.Level),
								FiveElement = item2.FiveElement,
								Type = item2.Type,
								GoodsNum = item2.Count,
								TemplateId = item2.TemplateId
							};
							m_TenGoods.Add(item);
						}
						Panel.ChouQuTimesText.text = $"剩余次数：{guildChouJiangResp_.Count}次";
						m_StartedIndex = m_CurrentChooseGoodsIndex;
						m_IsTen = true;
						m_ZhuanPanStatus = ZhuanPanStatus.Running;
					}
					else
					{
						Debug.Log("十连抽失败");
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildChouJiangResp_.Code);
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
	}
}
