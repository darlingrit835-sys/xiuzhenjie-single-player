using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_WaiWuDianXiangQingWnd : Window
	{
		private ZongMen_WaiWuDianXiangQingPanel m_Panel;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private BaseNaJieGoodEntity m_XuanZhoWuPing;

		private int m_LingShiShu;

		private bool publishing;
		private int searchVersion;

		private int m_Current = 12;

		private int zhangLaoTisheng;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			zhangLaoTisheng = (int)paralist[0];
			m_Panel = base.GameObject.GetComponent<ZongMen_WaiWuDianXiangQingPanel>();
			ResourceLoad();
			AddButtonClickListener(m_Panel.GuanBiButton, CloseWindow);
			m_XuanZhoWuPing = new BaseNaJieGoodEntity();
			m_Panel.RewardButton.interactable = false;
			m_Panel.FaBuXuanShangButton.interactable = false;
			AddToggleClickListener(m_Panel.PowerPlantItem, active => SelectKind(12, active));
			AddToggleClickListener(m_Panel.MineralItem, active => SelectKind(13, active));
			AddToggleClickListener(m_Panel.BloodItem, active => SelectKind(9, active));
			AddToggleClickListener(m_Panel.SkeletonItem, active => SelectKind(10, active));
			AddToggleClickListener(m_Panel.DemonDanItem, active => SelectKind(11, active));
			AddButtonClickListener(m_Panel.ShoSuoButton, ShoSuo);
			AddButtonClickListener(m_Panel.RewardButton, Reward);
			AddButtonClickListener(m_Panel.FaBuXuanShangButton, FaBuXuanShang);
		}

		private void FaBuXuanShang()
		{
			if (publishing || !ReadOffer()) return;
			if (m_XuanZhoWuPing == null || string.IsNullOrEmpty(m_XuanZhoWuPing.GoodsId))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请先选择要寻找的物品。");
				return;
			}
			publishing = true;
			m_Panel.FaBuXuanShangButton.interactable = false;
			var role = Singleton<RoleManager>.Instance.Role_Entity;
			var request = new WaiWuFaBuXuanShangReq_1015059
			{
				GoodsTemplateId = m_XuanZhoWuPing.GoodsId,
				GoodsType = m_XuanZhoWuPing.Type,
				Award = m_LingShiShu
			};
			ChengZhuFuMainWnd.Request(MsgType.WaiWuFaBuXuanShang, request, bytes =>
			{
				publishing = false;
				var reply = bytes == null ? null : WaiWuFaBuXuanShangResp_1015059.Parser.ParseFrom(bytes);
				if (role != Singleton<RoleManager>.Instance.Role_Entity) return;
				if (reply != null && reply.IsSuccess)
				{
					role.ShinyStone = reply.CurrentStone;
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					Singleton<UIManager>.Instance.FindWndByName<ZongMen_WaiWuDianWnd>("ZongMen_WaiWuDianWnd.prefab")?.RefreshOrders();
					if (GameObject != null) CloseWindow();
					return;
				}
				if (GameObject == null) return;
				m_Panel.FaBuXuanShangButton.interactable = true;
				string reason = reply == null ? null : MonoSingleton<GameStart>.Instance.GetHintMsgByCode(reply.Code);
				MonoSingleton<GameStart>.Instance.PopUpAttention(reason ?? "发布失败，请稍后重试。");
			});
		}

		private bool ReadOffer()
		{
			if (int.TryParse(m_Panel.RewardInputField.text, out m_LingShiShu) && m_LingShiShu > 0)
				return true;
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("请输入 1 至 2147483647 之间的灵石数。");
			return false;
		}

		private void Reward()
		{
			if (ReadOffer()) JiSuan();
		}

		private void SelectKind(int kind, bool active)
		{
			if (!active || kind == m_Current) return;
			m_Current = kind;
			searchVersion++;
			LingYeClearData();
			m_XuanZhoWuPing = null;
			m_Panel.RewardButton.interactable = false;
			m_Panel.FaBuXuanShangButton.interactable = false;
			m_Panel.SuccessRateText.text = "—";
			m_Panel.ContributionText.text = "—";
		}

		private void LingYeClearData()
		{
			if (m_Panel == null || m_Panel.ScrollRect == null)
			{
				return;
			}
			foreach (RectTransform item in m_Panel.ScrollRect.gameObject.GetComponent<RectTransform>())
			{
				item.gameObject.SetActive(false);
				Object.Destroy(item.gameObject);
			}
		}

		private void ShoSuo()
		{
			int kind = m_Current;
			string name = m_Panel.ShoSuoInputField.text.Trim();
			if (kind < 9 || kind > 13 || name.Length == 0)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("请选择物品类别，并输入完整物品名。");
				return;
			}
			LingYeClearData();
			m_XuanZhoWuPing = null;
			m_Panel.RewardButton.interactable = false;
			m_Panel.FaBuXuanShangButton.interactable = false;
			int currentSearch = ++searchVersion;
			var request = new WaiWuXuanShangGoodsSearchReq_1015143 { Name = name, GoodsType = kind };
			ChengZhuFuMainWnd.Request(MsgType.WaiWuXuanShangGoodsSearch, request, bytes =>
			{
				if (GameObject == null || currentSearch != searchVersion || m_Current != kind) return;
				var reply = bytes == null ? null : WaiWuXuanShangGoodsSearchResp_1015143.Parser.ParseFrom(bytes);
				if (reply == null || !reply.IsSuccess)
				{
					string reason = reply == null ? null : MonoSingleton<GameStart>.Instance.GetHintMsgByCode(reply.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(reason ?? "物品查询失败，请稍后重试。");
					return;
				}
				var list = new List<BaseNaJieGoodEntity>();
				foreach (var goods in reply.GoodsInfoList)
					list.Add(new BaseNaJieGoodEntity { Name = goods.Name, FiveElement = goods.FiveElement, GoodsId = goods.Id,
						Level = goods.Level, Rank = goods.Rank, Type = kind, RecyclePrice = goods.RecyclePrice });
				if (list.Count > 0) Addset(list);
				else MonoSingleton<GameStart>.Instance.PopRollingTextShow("未找到这个名称的物品。");
			});
		}

		private void Addset(List<BaseNaJieGoodEntity> naJieData)
		{
			GameObject gmeObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			foreach (BaseNaJieGoodEntity naJieDatum in naJieData)
			{
				CreateObjItem(gmeObject, naJieDatum);
			}
			GridLayoutGroup component = m_Panel.ScrollRect.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(naJieData.Count, m_Panel.ScrollRect, component);
		}

		private void CreateObjItem(GameObject gmeObject, BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(m_Panel.ScrollRect, gmeObject);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, (object obj, GameObject gameObj) =>
			{
				ShuXing(data);
			});
		}

		private void ShuXing(BaseNaJieGoodEntity data)
		{
			m_Panel.RewardButton.interactable = true;
			m_Panel.FaBuXuanShangButton.interactable = !publishing;
			m_XuanZhoWuPing = data;
			JiSuan();
		}

		private void JiSuan()
		{
			if (m_XuanZhoWuPing == null || m_XuanZhoWuPing.RecyclePrice <= 0) return;
			long chance = m_LingShiShu / (20L * m_XuanZhoWuPing.RecyclePrice) * 5 + zhangLaoTisheng;
			m_Panel.SuccessRateText.text = $"{System.Math.Min(100L, chance)}%";
			m_Panel.ConsumeText.text = "1小时";
			m_Panel.ContributionText.text = $"{m_XuanZhoWuPing.Level * 20}点";
		}

		private static string TimeToString(int result)
		{
			int num = result * 3600000 / 60000;
			if (num > 0 && num < 2)
			{
				num = 1;
			}
			return $"{num}分钟";
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		public override void CloseWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
