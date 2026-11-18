using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class ShaoDangWnd : Window
	{
		public ShaoDangPanel Panel;

		private bool m_IsRunning;

		private float m_MaxTime = 10f;

		private float m_CurrentTime;

		private MapEntity m_MapModel;

		private List<BaseNaJieGoodEntity> m_DiaoLuoList;

		private List<MonsterModel> m_MonsterModels;

		private int LingShiValue;

		private string DiTuID;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ShaoDangPanel>();
			m_DiaoLuoList = new List<BaseNaJieGoodEntity>();
			m_MapModel = paralist[0] as MapEntity;
			if (Singleton<RoleManager>.Instance.Role_Entity.TianDaoLing)
			{
				m_MaxTime = 0.1f;
			}
			InitContent();
			AddButtonClickListener(Panel.LingQuBtn, LingQuBtnClick);
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.KaiShiShaoDangBtn, KaiShiShaoDangBtnClick);
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_IsRunning)
			{
				m_CurrentTime += Time.fixedDeltaTime;
				Panel.RunningProgressImg.fillAmount = m_CurrentTime / m_MaxTime;
				Panel.RunningProgressText.text = string.Format("{0}%", (m_CurrentTime / m_MaxTime * 100f).ToString("0.0"));
				if (m_CurrentTime >= m_MaxTime)
				{
					m_CurrentTime = 0f;
					m_IsRunning = false;
					RunningCompelete();
				}
			}
		}

		private void InitContent()
		{
			string text = "扫荡大约需要10秒，大约只能清理50%地图资源。本次将消耗10点体力。";
			string text2 = "扫荡大约需要10秒，大约只能清理150%地图资源。本次将消耗10点体力。";
			string text3 = "扫荡大约需要0秒，能清理200%地图资源。本次将消耗10点体力。";
			string text4 = "";
			text4 = ((!Singleton<RoleManager>.Instance.Role_Entity.TianDaoLing) ? ((Singleton<RoleManager>.Instance.Role_Entity.YueKaDays > 0) ? text2 : text) : text3);
			Panel.Content_1.text = text4;
			Panel.Content_2.text = text4;
			Panel.Content_3.text = text4;
		}

		private void KaiShiShaoDangBtnClick()
		{
			Panel.CloseBtn.gameObject.SetActive(value: false);
			Panel.QueRenPanel.SetActive(value: false);
			Panel.RunningPanel.SetActive(value: true);
			GetData();
		}

		private void GetData()
		{
			CleanMapReq_1010004 data = new CleanMapReq_1010004
			{
				MapId = m_MapModel.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.CLEAN_MAP, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					CleanMapResp_1010004 cleanMapResp_ = new CleanMapResp_1010004();
					cleanMapResp_.MergeFrom(byteString);
					if (cleanMapResp_.IsSuccess)
					{
						m_IsRunning = true;
						List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
						foreach (CleanMapResp_1010004.Types.DropGoodsInfoResp_1010005 dropGood in cleanMapResp_.DropGoods)
						{
							BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
							{
								GoodsId = dropGood.GoodsId,
								Rank = dropGood.Rank,
								FiveElement = dropGood.FiveElement,
								Name = dropGood.Name,
								Level = dropGood.Level,
								GoodsVolume = dropGood.Volume,
								Type = dropGood.Type,
								GoodsNum = dropGood.Count,
								TemplateId = dropGood.TemplateId,
								RecyclePrice = dropGood.RecyclePrice
							};
							list.Add(item);
						}
						DiTuID = cleanMapResp_.BattleId;
						MonoSingleton<GameStart>.Instance.PopRollingTextShow("消耗了10点体力!");
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = cleanMapResp_.CurrentShinyStone;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentPower = cleanMapResp_.CurrentPower;
						m_DiaoLuoList = list;
						LingShiValue = cleanMapResp_.DropStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(cleanMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
				}
			}, isShowWaitUI: true);
		}

		private void RunningCompelete()
		{
			Panel.RunningPanel.SetActive(value: false);
			Panel.CompeletePanel.SetActive(value: true);
			ZhanDuYinDao();
		}

		private void LingQuBtnClick()
		{
			if (LingShiValue != 0)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow($"获取{LingShiValue}灵石");
			}
			Panel.CompeletePanel.SetActive(value: false);
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			Singleton<UIManager>.Instance.PopUpWnd("ZhanDouSettlementWnd.prefab", true, m_DiaoLuoList, new EndBattleInfoModel(), DiTuID);
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void ZhanDuYinDao()
		{
			if (Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu == 20 && Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100)
			{
				Singleton<GuideManager>.Instance.ShowGuideNew(2);
			}
		}
	}
}
