using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GuideManager : Singleton<GuideManager>
	{
		public YinDaoJilu YinDaoJilu1 = new YinDaoJilu();

		private bool m_isShowGuide;

		private Dictionary<GuideType, int> m_GuideProgressDic;

		internal PracticeGuideFlow Practice { get; private set; }

		public bool IsShowGuide => m_isShowGuide;

		public void Init()
		{
			m_GuideProgressDic = new Dictionary<GuideType, int>();
		}

		public void InitProgress(int runningProgress, int battleProgress)
		{
			YinDaoJilu1 = new YinDaoJilu();
			savingMeridian = false;
			Practice = new PracticeGuideFlow();
			if (runningProgress >= 0 && runningProgress <= 3)
			{
				YinDaoJilu1.YinDaoName = 1;
				return;
			}
			if (runningProgress == 100 && battleProgress == 1)
			{
				YinDaoJilu1.YinDaoName = 2;
				YinDaoJilu1.YinDaoShu = 1;
				return;
			}
			if (runningProgress == 100 && battleProgress == 2)
			{
				YinDaoJilu1.YinDaoName = 2;
				YinDaoJilu1.YinDaoShu = 4;
			}
			else if (runningProgress == 100 && battleProgress == 3)
			{
				YinDaoJilu1.YinDaoName = 2;
				YinDaoJilu1.YinDaoShu = 8;
			}
			else if (runningProgress == 100 && battleProgress == 4)
			{
				YinDaoJilu1.YinDaoName = 2;
				YinDaoJilu1.YinDaoShu = 16;
			}
		}

		public void ShowGuideNew(int index)
		{
			if (index == 1)
			{
				ShowPractice();
				return;
			}
			if (index == 6 && !ClientHotUpdate.MeridianEntryEnabled)
				return;
			if (Singleton<UIManager>.Instance.FindWndByName<GuideNewWnd>("GuideNewWnd.prefab") != null)
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
			}
			switch (index)
			{
			case 2:
				if (Singleton<RoleManager>.Instance.Role_Entity.BattleGuideVal != 100)
				{
					YinDaoJilu1.ZhanDou = true;
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 5:
				if (Singleton<RoleManager>.Instance.Role_Entity.ShopGuideVal != 100)
				{
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 6:
				if (Singleton<RoleManager>.Instance.Role_Entity.AcupointGuideVal != 100)
				{
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 7:
				if (Singleton<RoleManager>.Instance.Role_Entity.SignGuideVal != 100)
				{
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 8:
				if (Singleton<RoleManager>.Instance.Role_Entity.ElixirGuideVal != 100)
				{
					Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				}
				break;
			case 9:
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				break;
			case 10:
				YinDaoJilu1.YinDaoName = 10;
				YinDaoJilu1.YinDaoShu = 1;
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				break;
			case 3:
			case 4:
				break;
			}
		}

		public void ShowGuideNew(int index, int step)
		{
			if (index == 1)
			{
				ShowPractice();
				return;
			}
			if (Singleton<UIManager>.Instance.FindWndByName<GuideNewWnd>("GuideNewWnd.prefab") != null)
			{
				return;
			}
			switch (index)
			{
			case 2:
				YinDaoJilu1.ZhanDou = true;
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				break;
			case 3:
			{
				int num = Singleton<RoleManager>.Instance.GetGuideProcessByType(2);
				if (num == -1)
				{
					Debug.Log("新手引导进度获取失败");
				}
				if (!YinDaoJilu1.ZhanDou)
				{
					if (num == 1)
					{
						num++;
						YinDaoJilu1.YinDaoName = 2;
						YinDaoJilu1.YinDaoShu = num;
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
					}
					if (num == 4)
					{
						YinDaoJilu1.YinDaoName = 2;
						YinDaoJilu1.YinDaoShu = num;
						Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
					}
				}
				YinDaoJilu1.ZhanDou = true;
				break;
			}
			case 10:
				YinDaoJilu1.YinDaoName = 10;
				YinDaoJilu1.YinDaoShu = 1;
				Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
				break;
			}
		}

		internal void ShowPractice()
		{
			if (Practice == null || !Practice.Active || Singleton<UIManager>.Instance.FindWndByName("GuideNewWnd.prefab") != null)
				return;
			YinDaoJilu1.YinDaoName = 1;
			YinDaoJilu1.XiuLian = true;
			Singleton<UIManager>.Instance.PopUpWnd("GuideNewWnd.prefab", true);
		}

		public void ZhanDouYingDao(Dictionary<int, GuideMaskItem> yindaoLeiXing)
		{
			RoleZhuangBeiWnd roleZhuangBeiWnd = Singleton<UIManager>.Instance.FindWndByName<RoleZhuangBeiWnd>("RoleZhuangBeiWnd.prefab");
			switch (YinDaoJilu1.YinDaoShu)
			{
			case 0:
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				break;
			case 1:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 2:
			{
				ShowBattleControl(yindaoLeiXing, 2, roleZhuangBeiWnd?.GuideEquipmentButton);
				break;
			}
			case 3:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 4:
				ShowBattleControl(yindaoLeiXing, 4, roleZhuangBeiWnd?.Panel?.JiNengTog?.transform);
				break;
			case 5:
				ShowBattleControl(yindaoLeiXing, 5, roleZhuangBeiWnd?.Panel?.JiNengRect?.Tog_Level_2?.transform);
				break;
			case 6:
			{
				ShowGridStep(yindaoLeiXing, roleZhuangBeiWnd?.Panel?.JiNengRect?.GoodsScrollViewContent, 6);
				break;
			}
			case 7:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 8:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 9:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 10:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 11:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 12:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 13:
				ShowBattleControl(yindaoLeiXing, 13, Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab")?.GuideArtifactButton());
				break;
			case 14:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 15:
				ShowBattleControl(yindaoLeiXing, 15, Singleton<UIManager>.Instance.FindWndByName<BattleWnd>("BattleWnd.prefab")?.Panel?.LiKaiKillBossBtn?.transform);
				break;
			case 16:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 17:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 18:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 19:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 20:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 21:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 22:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 100:
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				break;
			}
		}

		private void ShowBattleControl(Dictionary<int, GuideMaskItem> steps, int step, Transform control)
		{
			if (control is RectTransform rect && steps.TryGetValue(step, out var mask))
				mask.FollowTarget(rect);
			PanDuanBuZhou(steps, step);
		}

		public void ChuShi(Dictionary<int, GuideMaskItem> yindaoLeiXing)
		{
			switch (YinDaoJilu1.YinDaoShu)
			{
			case 1:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 2:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 3:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 4:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 5:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 6:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 7:
				break;
			}
		}

		public void ZhongJieFangShiYinDao(Dictionary<int, GuideMaskItem> yindaoLeiXing)
		{
			switch (YinDaoJilu1.YinDaoShu)
			{
			case 1:
				var market = Singleton<UIManager>.Instance.FindWndByName<ZhongJieFangShiWnd>(Constants.ZHONGJIEFANGSHI_WINDOW);
				if (market?.Panel != null && yindaoLeiXing.TryGetValue(1, out var first))
					first.FollowTarget((RectTransform)market.Panel.ZaHuoPu_Btn.transform);
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 2:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 3:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 4:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 5:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 6:
				break;
			}
		}

		public void JingMai(Dictionary<int, GuideMaskItem> steps)
		{
			var ui = Singleton<UIManager>.Instance;
			int step = YinDaoJilu1.YinDaoShu;
			if (!steps.TryGetValue(step, out var mask)) return;
			Button button = null;
			UnityAction action;
			if (step == 2)
			{
				var map = MonoSingleton<JingMaiManager>.Instance;
				if (map?.Spectrum == null) return;
				map.Spectrum.Focus("1");
				var point = map.Spectrum.FocusedItem;
				mask.FollowWorldTarget(point.transform, map.SceneCamera);
				action = () => map.ShowAcupoint(point);
			}
			else
			{
				switch (step)
				{
				case 1: button = ui.FindWndByName<DongFuWnd>("DongFuWnd.prefab")?.Panel?.EnterNeiShiBtn; break;
				case 3: button = ui.FindWndByName<NeiShiWnd>("NeiShiWnd.prefab")?.Panel?.TiShiPanel_TongXueBtn; break;
				case 4: button = ui.FindWndByName<NeiShiWnd>("NeiShiWnd.prefab")?.Panel?.ZongLan_Btn; break;
				case 5: button = ui.FindWndByName<ZongLanWnd>("ZongLanWnd.prefab")?.Panel?.CloseBtn; break;
				}
				if (button == null) return;
				mask.FollowTarget((RectTransform)button.transform);
				action = () => {
					if (!button.IsActive() || !button.IsInteractable()) return;
					ui.CloseWnd("GuideNewWnd.prefab");
					button.onClick.Invoke();
					if (step == 5) SaveMeridianCheckpoint(2);
				};
			}
			var hint = mask.transform.Find("Img/Text")?.GetComponent<Text>();
			if (hint != null)
			{
				string[] hints = { "", "打开内视，查看正在形成的经脉。", "点选胞中。这里是任脉、督脉与冲脉的交会处。",
					"初通胞中只需 1 点经脉点，必定成功。", "打开八脉总览，查看胞中带来的三脉收益。",
					"胞中已计入三条经脉。读完返回，沿相邻星穴继续行气。" };
				hint.text = hints[step];
			}
			mask.transform.parent.gameObject.SetActive(true);
			mask.gameObject.SetActive(true);
			mask.BeginShow(action);
		}

		internal void ShowMeridianStep(int step)
		{
			YinDaoJilu1.YinDaoName = 6;
			YinDaoJilu1.YinDaoShu = step;
			ShowGuideNew(6);
		}

		private bool savingMeridian;
		internal void SaveMeridianCheckpoint(int expected, UnityAction saved = null)
		{
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			if (role.AcupointGuideVal != expected || savingMeridian) return;
			savingMeridian = true;
			var request = new GuideInfoUpdateReq_1002013 { Type = 6 };
			bool sent = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.UPDATE_GUIDE, request, bytes => {
				savingMeridian = false;
				if (!GuideInfoUpdateResp_1002013.Parser.ParseFrom(bytes).IsSuccess ||
					!ReferenceEquals(role, Singleton<RoleManager>.Instance.Role_Entity) || role.AcupointGuideVal != expected) return;
				role.AcupointGuideVal = expected == 1 ? 2 : 100;
				if (expected == 2) YinDaoJilu1.YinDaoName = 0;
				saved?.Invoke();
			});
			if (!sent) savingMeridian = false;
		}

		public void HuaFu(Dictionary<int, GuideMaskItem> yindaoLeiXing)
		{
			switch (YinDaoJilu1.YinDaoShu)
			{
			case 1:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 2:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 3:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 4:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 5:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 6:
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
				break;
			case 7:
			case 8:
			case 9:
				break;
			}
		}

		public void LianDan(Dictionary<int, GuideMaskItem> yindaoLeiXing)
		{
			int yinDaoShu = YinDaoJilu1.YinDaoShu;
			if ((uint)(yinDaoShu - 1) > 7u)
			{
				_ = 9;
			}
			else
			{
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
			}
		}

		public void TuPo(Dictionary<int, GuideMaskItem> yindaoLeiXing)
		{
			int yinDaoShu = YinDaoJilu1.YinDaoShu;
			if ((uint)(yinDaoShu - 1) > 7u)
			{
				_ = 9;
			}
			else
			{
				PanDuanBuZhou(yindaoLeiXing, YinDaoJilu1.YinDaoShu);
			}
		}

		private void ShowGridStep(Dictionary<int, GuideMaskItem> steps, RectTransform content, int offset)
		{
			if (content == null || !content.TryGetComponent(out GridLayoutGroup grid))
			{
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				return;
			}
			PanDuanBuZhou(steps, YinDaoJilu1.YinDaoShu, grid, offset);
		}

		public void PanDuanBuZhou(Dictionary<int, GuideMaskItem> yindaoLeiXing, int yinDaoShu, GridLayoutGroup targetGridLayoutGroup = null, int offsetValue = 5)
		{
			if (!yindaoLeiXing.TryGetValue(yinDaoShu, out GuideMaskItem guideMaskItem))
			{
				Debug.LogWarning($"缺少引导步骤 {YinDaoJilu1.YinDaoName}/{yinDaoShu}");
				Singleton<UIManager>.Instance.CloseWnd("GuideNewWnd.prefab");
				return;
			}
			guideMaskItem.transform.parent.gameObject.SetActive(value: true);
			guideMaskItem.gameObject.SetActive(value: true);
			if (targetGridLayoutGroup != null)
			{
				RectTransform component = guideMaskItem.gameObject.GetComponent<RectTransform>();
				float num = targetGridLayoutGroup.padding.left;
				component.anchoredPosition = new Vector2(component.sizeDelta.x / 2f + num - (float)offsetValue, component.anchoredPosition.y);
			}
			guideMaskItem.BeginShow();
		}

		private void GetAllGuideProgress()
		{
			string key = "GuideTypeIndex{2}";
			string key2 = "GuideTypeIndex{3}";
			string key3 = "GuideTypeIndex{4}";
			string key4 = "GuideTypeIndex{5}";
			string key5 = "GuideTypeIndex{6}";
			string key6 = "GuideTypeIndex{7}";
			string key7 = "GuideTypeIndex{8}";
			string key8 = "GuideTypeIndex{9}";
			string key9 = "GuideTypeIndex{10}";
			string key10 = "GuideTypeIndex{11}";
			PlayerPrefs.GetInt("GuideTypeIndex{1}");
			int value = PlayerPrefs.GetInt(key);
			int value2 = PlayerPrefs.GetInt(key2);
			int value3 = PlayerPrefs.GetInt(key3);
			int value4 = PlayerPrefs.GetInt(key4);
			int value5 = PlayerPrefs.GetInt(key5);
			int value6 = PlayerPrefs.GetInt(key6);
			int value7 = PlayerPrefs.GetInt(key7);
			int value8 = PlayerPrefs.GetInt(key8);
			int value9 = PlayerPrefs.GetInt(key9);
			int value10 = PlayerPrefs.GetInt(key10);
			if (m_GuideProgressDic != null)
			{
				m_GuideProgressDic.Add(GuideType.Battle, value);
				m_GuideProgressDic.Add(GuideType.Learn, value);
				m_GuideProgressDic.Add(GuideType.LianDan, value2);
				m_GuideProgressDic.Add(GuideType.DongFuCanWu, value3);
				m_GuideProgressDic.Add(GuideType.XiuXingYinDao, value4);
				m_GuideProgressDic.Add(GuideType.ZhanDouYinDao, value5);
				m_GuideProgressDic.Add(GuideType.ChuShi, value6);
				m_GuideProgressDic.Add(GuideType.JingMai, value7);
				m_GuideProgressDic.Add(GuideType.ZhongJieFangShi, value8);
				m_GuideProgressDic.Add(GuideType.HuaFu, value9);
				m_GuideProgressDic.Add(GuideType.TuPO, value10);
			}
		}

		public bool IsCanShowGuide(GuideType type, int index)
		{
			return !(type == GuideType.Learn && Practice?.Active == true);
		}

		public void ShowGuide(GuideType type, int index)
		{
			m_GuideProgressDic.TryGetValue(type, out var value);
			if (value == -1)
			{
				return;
			}
			value++;
			if (value == index && Singleton<UIManager>.Instance.PopUpWnd("GuideWnd.prefab", true) is GuideWnd guideWnd)
			{
				guideWnd.ShowGuide(type, value);
				GuideCompelete(type, index);
				if (guideWnd.IsGuideEnd(type, index))
				{
					NoShowGuide(type);
				}
			}
		}

		public void ShowCoherentGuide(GuideType type, int index, UnityAction compeleteCallBack = null)
		{
		}

		public void GuideCompelete(GuideType type, int statusIndex)
		{
			m_GuideProgressDic[type] = statusIndex;
			PlayerPrefs.SetInt($"GuideTypeIndex{(int)type}", statusIndex);
		}

		public void NoShowGuide(GuideType type)
		{
			if (m_GuideProgressDic != null && m_GuideProgressDic.ContainsKey(type))
			{
				GuideCompelete(type, -1);
			}
		}
	}
}
