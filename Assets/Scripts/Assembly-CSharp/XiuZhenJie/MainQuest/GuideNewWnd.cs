using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class GuideNewWnd : Window
	{
		private GuideNewPanel m_Panel;

		private PracticeGuideOverlay practiceOverlay;
		private PracticeGuideFlow practice;
		private bool legacySkipVisible;

		private Dictionary<int, GuideMaskItem> m_ZhanDouYingDao;

		private Dictionary<int, GuideMaskItem> m_ChuShi;

		private Dictionary<int, GuideMaskItem> m_ZhongJieFangShi;

		private Dictionary<int, GuideMaskItem> m_JingMai;

		private Dictionary<int, GuideMaskItem> m_HuaFu;

		private Dictionary<int, GuideMaskItem> m_LianDan;

		private Dictionary<int, GuideMaskItem> m_TuPo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<GuideNewPanel>();
			legacySkipVisible = m_Panel.TiaoGuo.gameObject.activeSelf;
			m_Panel.transform.SetAsFirstSibling();
			AddButtonClickListener(m_Panel.TiaoGuo, TiaoGuoGuide);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_Panel.XiuLianYinDao.gameObject.SetActive(false);
			m_ZhanDouYingDao = ResetGuideGroup(m_Panel.ZhanDouYinDao, GuideType.ZhanDouYinDao);
			m_ChuShi = ResetGuideGroup(m_Panel.ChuShi, GuideType.ChuShi);
			m_ZhongJieFangShi = ResetGuideGroup(m_Panel.ZhongJieFangShiYinDao, GuideType.ZhongJieFangShi);
			m_JingMai = ResetGuideGroup(m_Panel.JingMai, GuideType.JingMai);
			m_HuaFu = ResetGuideGroup(m_Panel.HuaFu, GuideType.HuaFu);
			m_LianDan = ResetGuideGroup(m_Panel.LianDan, GuideType.LianDan);
			m_TuPo = ResetGuideGroup(m_Panel.TuPo, GuideType.TuPO);
			var manager = Singleton<GuideManager>.Instance;
			practice = manager.YinDaoJilu1.YinDaoName == 1 ? manager.Practice : null;
			m_Panel.TiaoGuo.gameObject.SetActive(practice == null && legacySkipVisible);
			if (practice != null)
			{
				if (practiceOverlay == null)
					practiceOverlay = PracticeGuideOverlay.Create((RectTransform)Transform, m_Panel.XiuLianYinDao.GetComponentInChildren<UnityEngine.UI.Text>(true).font);
				practice.Attach(practiceOverlay);
			}
			else
			{
				practiceOverlay?.Hide();
				Guide();
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (practice != null)
			{
				// A socket reconnect can replace Role_Entity without rebuilding the current scene.
				var current = Singleton<GuideManager>.Instance.Practice;
				if (!ReferenceEquals(current, practice) && current?.Active == true)
				{
					practice.Detach(practiceOverlay);
					practice = current;
					practice.ResumeWindows();
					practice.Attach(practiceOverlay);
				}
				else
					practice.Tick();
				if (GameObject == null)
					return;
			}
			m_Panel.transform.SetAsLastSibling();
			Window offline = Singleton<UIManager>.Instance.FindWndByName(Constants.LIXIANSHOUY_WINDOW);
			if (offline?.GameObject != null && offline.GameObject.activeInHierarchy && offline.Transform.parent == m_Panel.transform.parent)
			{
				offline.Transform.SetAsLastSibling();
			}
		}

		public override void OnClose()
		{
			practice?.Detach(practiceOverlay);
			base.OnClose();
		}

		private static Dictionary<int, GuideMaskItem> ResetGuideGroup(RectTransform group, GuideType type)
		{
			var steps = new Dictionary<int, GuideMaskItem>();
			if (group == null)
			{
				return steps;
			}
			group.gameObject.SetActive(false);
			foreach (Transform child in group)
			{
				if (child.TryGetComponent(out GuideMaskItem step) && step.GuideMaskType == type)
				{
					child.gameObject.SetActive(false);
					steps.TryAdd(step.GuideIndex, step);
				}
			}
			return steps;
		}

		private void Guide()
		{
			int yinDaoName = Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName;
			int yinDaoShu = Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu;
			if (!Singleton<GuideManager>.Instance.YinDaoJilu1.JiLu)
			{
				Singleton<GuideManager>.Instance.YinDaoJilu1.JiLu = true;
			}
			PanDuanYinDao(yinDaoName, yinDaoShu);
		}

		private void PanDuanYinDao(int guideName, int guideShu)
		{
			switch (guideName)
			{
			case 2:
				Singleton<GuideManager>.Instance.ZhanDouYingDao(m_ZhanDouYingDao);
				break;
			case 4:
				if (guideShu == 100 || guideShu == 0)
				{
					Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				}
				break;
			case 5:
				Singleton<GuideManager>.Instance.ZhongJieFangShiYinDao(m_ZhongJieFangShi);
				break;
			case 6:
				Singleton<GuideManager>.Instance.JingMai(m_JingMai);
				break;
			case 7:
				Singleton<GuideManager>.Instance.HuaFu(m_HuaFu);
				break;
			case 8:
				Singleton<GuideManager>.Instance.LianDan(m_LianDan);
				break;
			case 9:
				Singleton<GuideManager>.Instance.TuPo(m_TuPo);
				break;
			case 10:
				Singleton<GuideManager>.Instance.ChuShi(m_ChuShi);
				break;
			case 3:
				break;
			}
		}

		private void TiaoGuoGuide()
		{
			Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
			TiaoGuo(Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName);
		}

		private void TiaoGuo(int guideType)
		{
			GuideInfoSkipReq_1002014 data = new GuideInfoSkipReq_1002014
			{
				Type = guideType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.COMPELETE_GUIDE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuideInfoSkipResp_1002014 guideInfoSkipResp_ = new GuideInfoSkipResp_1002014();
					guideInfoSkipResp_.MergeFrom(byteString);
					if (guideInfoSkipResp_.IsSuccess)
					{
						if (guideType == 6)
						{
							Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal = 100;
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 0;
						}
						if (guideType == 2)
						{
							Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal = 100;
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoName = 0;
							Singleton<GuideManager>.Instance.YinDaoJilu1.YinDaoShu = 1;
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guideInfoSkipResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
