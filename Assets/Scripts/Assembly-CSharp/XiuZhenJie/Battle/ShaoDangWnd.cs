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
			InkDialogArt.SetWash(Panel.transform,new Color32(9,25,23,170));
			foreach (GameObject step in new[] {Panel.QueRenPanel,Panel.RunningPanel,Panel.CompeletePanel})
			{
				var card = (RectTransform)step.transform;
				InkDialogArt.SetPaper(card.GetComponent<UnityEngine.UI.Image>(),"InkLedgerPaper");
				InkBattleHud.Band((RectTransform)card.Find("Title"),7,7,7,37);
				InkInterfaceArt.Set(card.Find("Title").GetComponent<UnityEngine.UI.Image>(),"JadeButton",false);
				card.Find("Title/Image").GetComponent<UnityEngine.UI.Image>().enabled = false;
				var heading = card.Find("Title/Image/Text").GetComponent<UnityEngine.UI.Text>();
				heading.color = InkDialogArt.Ivory;
				heading.text = step == Panel.RunningPanel ? "巡山中" : step == Panel.CompeletePanel ? "巡山所得" : "巡山扫荡";
				var footer = card.Find("BottomImg");
				if (footer != null) footer.GetComponent<UnityEngine.UI.Image>().enabled = false;
				InkInterfaceArt.Set(card.Find("Content").GetComponent<UnityEngine.UI.Image>(),"ParchmentHeading",false);
				BattleHudReveal.Attach(card);
			}
			InkDialogArt.StyleButton(Panel.KaiShiShaoDangBtn, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(Panel.LingQuBtn, "JadeButton", InkDialogArt.Ivory);
			Panel.Content_1.color = Panel.Content_2.color = Panel.Content_3.color = InkInterfaceArt.Ink;
			Panel.RunningProgressImg.color = new Color32(65, 118, 91, 255);
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
				float progress = Mathf.Clamp01(m_CurrentTime / m_MaxTime);
				Panel.RunningProgressImg.fillAmount = progress;
				Panel.RunningProgressText.text = $"{progress:P0}";
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
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			int rewardPercent = role.TianDaoLing ? 200 : role.YueKaDays > 0 ? 150 : 50;
			string duration = role.TianDaoLing ? "即时完成" : $"约 {m_MaxTime:0} 秒";
			Panel.Content_1.text = $"{m_MapModel.MapName}\n消耗 10 点体力　{duration}\n预计资源获取率 {rewardPercent}%";
			Panel.Content_2.text = $"正在扫荡{m_MapModel.MapName}";
		}

		private void KaiShiShaoDangBtnClick()
		{
			Panel.CloseBtn.gameObject.SetActive(value: false);
			Panel.QueRenPanel.SetActive(value: false);
			Panel.RunningPanel.SetActive(value: true);
			Panel.RunningProgressImg.fillAmount = 0f;
			Panel.RunningProgressText.text = "等待回执";
			GetData();
		}

		private void GetData()
		{
			CleanMapReq_1010004 data = new CleanMapReq_1010004
			{
				MapId = m_MapModel.Id
			};
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.CLEAN_MAP, data, byteString =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (GameObject == null)
					return;
				if (byteString == null)
				{
					Singleton<UIManager>.Instance.CloseWnd(this);
					MonoSingleton<GameStart>.Instance.PopUpAttention("扫荡结果尚未收到，请重新进入查看体力与奖励。");
					return;
				}
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
			});
			if (sent)
			{
				MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
			}
			else
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前未连接至服务器，无法开始扫荡。");
			}
		}

		private void RunningCompelete()
		{
			Panel.RunningPanel.SetActive(value: false);
			Panel.CompeletePanel.SetActive(value: true);
			Panel.Content_3.text = $"{m_MapModel.MapName}扫荡完成\n获得 {LingShiValue:N0} 灵石，{m_DiaoLuoList.Count} 项掉落";
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
