using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class GengHuanGongFaWnd : Window
	{
		public GengHuanGongFaPanel Panel;

		internal Toggle PracticeBook { get; private set; }

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private List<GameObject> m_ItemList;

		private List<GongFaEntity> m_DataSource;

		private List<string> m_DetailList;

		private GongFaType m_CurrentType;

		private RuningGongFaZuHeType m_GongFaRunningType;

		public GongFaEntity m_CurrentChooseItem;

		private GameObject m_TogInsObj;

		public GameObject m_XiangQingInstanceObj;

		private Common_ShaiXuanWnd m_Common_ShaiXuanWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_ItemList = new List<GameObject>();
			m_DetailList = new List<string>();
			Panel = base.GameObject.GetComponent<GengHuanGongFaPanel>();
			InkCultivationInterface.StyleTechniqueChooser(Panel);
			ResourceLoad();
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.QueRenGengHuan, GengHuanBtnClick);
			AddButtonClickListener(Panel.ShaiXuanBtn, ShaiXuanWnd);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 4)
			{
				m_CurrentChooseItem = paralist[0] as GongFaEntity;
				m_DataSource = paralist[1] as List<GongFaEntity>;
				m_CurrentType = (GongFaType)paralist[2];
				m_GongFaRunningType = (RuningGongFaZuHeType)paralist[3];
				if (m_CurrentChooseItem != null)
				{
					ChoosedGongFa(isOn: true, m_CurrentChooseItem, null);
				}
			}
			List<ShaiXuanItemType> list = new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.PinJie,
				ShaiXuanItemType.DengJie
			};
			m_Common_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, list, true) as Common_ShaiXuanWnd;
			if (m_Common_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd common_ShaiXuanWnd = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(common_ShaiXuanWnd.ReChooseAction, new UnityAction(FlashData));
				Common_ShaiXuanWnd common_ShaiXuanWnd2 = m_Common_ShaiXuanWnd;
				common_ShaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(common_ShaiXuanWnd2.ReChooseAction, new UnityAction(FlashData));
			}
			FlashData();
			ShowGuide(GuideType.Learn, 8);
		}

		private void ShaiXuanWnd()
		{
			m_Common_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void ResourceLoad()
		{
			m_XiangQingInstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiXiangQingItem.prefab");
			m_TogInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = InkCultivationInterface.Get("TechniqueScroll") ??
				Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_GongFaShu.png");
			m_GoodsIconSprite.Add(1, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void ShowGuide(GuideType _type, int _index)
		{
			MonoSingleton<GameStart>.Instance.StartCoroutine(delayShowGuide(_type, _index));
		}

		private IEnumerator delayShowGuide(GuideType _type, int _index)
		{
			yield return new WaitForEndOfFrame();
			if (Singleton<GuideManager>.Instance.IsCanShowGuide(_type, _index))
			{
				Singleton<GuideManager>.Instance.ShowGuide(_type, _index);
			}
		}

		public void FlashData()
		{
			if (m_DataSource == null)
			{
				return;
			}
			foreach (Transform item in Panel.KeYongGongFaScrollViewContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			Dictionary<string, ShaiXuanParamsModel> result = m_Common_ShaiXuanWnd?.GetResult();
			List<GongFaEntity> dataList = ShaiXuanGoods(result, m_DataSource);
			PracticeBook = null;
			if (Singleton<GuideManager>.Instance.Practice?.Stage == 3 && m_CurrentType == GongFaType.NaQi)
			{
				var starter = m_DataSource.FirstOrDefault(method => method.Id == PracticeGuideFlow.StarterId);
				if (starter != null)
				{
					dataList.Remove(starter);
					dataList.Insert(0, starter);
				}
			}
			InitKeGengHuan(dataList);
		}

		private List<GongFaEntity> ShaiXuanGoods(Dictionary<string, ShaiXuanParamsModel> result, List<GongFaEntity> data)
		{
			List<GongFaEntity> list = null;
			list = ((data != null) ? new List<GongFaEntity>(data) : new List<GongFaEntity>());
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((GongFaEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((GongFaEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((GongFaEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			return list;
		}

		private void InitKeGengHuan(List<GongFaEntity> dataList)
		{
			string text = "";
			if (m_CurrentChooseItem != null)
			{
				text = m_CurrentChooseItem.Id;
			}
			if (dataList == null || dataList.Count <= 0)
			{
				return;
			}
			foreach (GongFaEntity data in dataList.OrderByDescending(book => SystemMethodShelf.IsSystem(book.Id)))
			{
				UI_GoodsTogItem component = SetListContentParent(Panel.KeYongGongFaScrollViewContent, m_TogInsObj).GetComponent<UI_GoodsTogItem>();
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(1, out value);
				WorldSetting.GetLevelByRank(data.Rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(data.Rank), out value2);
				component.SetValue(group: Panel.KeYongGongFaScrollViewContent.gameObject.GetComponent<ToggleGroup>(), dataSource: data, name: data.Name, fiveElement: data.FiveElement, level: data.Level, rankSpr: value2, iconSpr: value, ClickAction: ChoosedGongFa, isOn: data.Id == text);
				if (data.Id == PracticeGuideFlow.StarterId)
					PracticeBook = component.ClickTog;
			}
			GridLayoutGroup component2 = Panel.KeYongGongFaScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(m_DataSource.Count, Panel.KeYongGongFaScrollViewContent, component2);
		}

		private void ChoosedGongFa(bool isOn, object dataSource, GameObject gameObj)
		{
			if (!isOn)
			{
				return;
			}
			m_DetailList.Clear();
			GongFaEntity gongFaEntity = dataSource as GongFaEntity;
			Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(gongFaEntity.Id, gongFaEntity.Level, gongFaEntity.Rank, gongFaEntity.FiveElement, (List<string> list) =>
			{
				m_DetailList = list;
				var system = SystemMethodShelf.Find(gongFaEntity.Id);
				if (system != null && list != null)
					list.Insert(0, "传承气相," + SystemMethodShelf.Signature(system) + "," + system.description.Replace(',', '，'));
				GetDetail(gongFaEntity);
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(1, out value);
				int levelByRank = WorldSetting.GetLevelByRank(gongFaEntity.Rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				if (gongFaEntity != null)
				{
					m_CurrentChooseItem = gongFaEntity;
					Panel.CurrentGongFaChoosed.SetItem(gongFaEntity, gongFaEntity.Name, gongFaEntity.FiveElement, gongFaEntity.Level, value2, value, null);
				}
			});
		}

		private void GetDetail(GongFaEntity gongFaEntity)
		{
			foreach (Transform item in Panel.XiangQingScrollViewContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (m_DetailList != null)
			{
				int num = 0;
				m_DetailList.Insert(Math.Max(0, m_DetailList.Count - 3), string.Format("{0},{1}", "当前重数", gongFaEntity.AchievementLevel));
				m_DetailList.Insert(Math.Max(0, m_DetailList.Count - 2), string.Format("{0},{1}", "周天效率", gongFaEntity.Circle));
				foreach (string detail in m_DetailList)
				{
					InitDetailItem(num, detail);
					num++;
				}
			}
			VerticalLayoutGroup component = Panel.XiangQingScrollViewContent.GetComponent<VerticalLayoutGroup>();
			float y = m_XiangQingInstanceObj.GetComponent<RectTransform>().sizeDelta.y;
			float y2 = SetVerticalListContentHight(m_DetailList.Count + 2, y, component);
			Panel.XiangQingScrollViewContent.sizeDelta = new Vector2(Panel.XiangQingScrollViewContent.sizeDelta.x, y2);
		}

		private void InitDetailItem(int index, string data)
		{
			RoleZhuangBeiXiangQingItem component = UnityEngine.Object.Instantiate(m_XiangQingInstanceObj, Panel.XiangQingScrollViewContent).GetComponent<RoleZhuangBeiXiangQingItem>();
			string[] array = data.Split(',');
			string content = $"{array[0]}：{array[1]}";
			component.SetValue(index, content);
			InkCultivationInterface.StyleTechniqueDetail(component);
		}

		public void GengHuanBtnClick()
		{
			if (m_CurrentChooseItem == null || string.IsNullOrEmpty(m_CurrentChooseItem.Id))
			{
				return;
			}
			GongFaEntity submittedTechnique = m_CurrentChooseItem;
			GongFaType submittedType = m_CurrentType;
			var practice = Singleton<GuideManager>.Instance.Practice;
			ChangeAchievementReq_1005011 data = new ChangeAchievementReq_1005011
			{
				AchievementId = submittedTechnique.Id,
				ScopeType = (int)submittedType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_ACHIEVEMENT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChangeAchievementResp_1005011 changeAchievementResp_ = new ChangeAchievementResp_1005011();
					changeAchievementResp_.MergeFrom(byteString);
					DongFuWnd dongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
					if (changeAchievementResp_.IsSuccess && dongFuWnd != null)
					{
						switch (submittedType)
						{
						case GongFaType.NaQi:
							dongFuWnd.ResetNaQiXiaoLv(changeAchievementResp_.AchievementCircleValue);
							dongFuWnd.ResetNaQiName(submittedTechnique.Name);
							if (submittedTechnique.Id == PracticeGuideFlow.StarterId)
								practice?.ConfirmTechnique();
							break;
						case GongFaType.LianShen:
							dongFuWnd.ResetLianShenXiaoLv(changeAchievementResp_.AchievementCircleValue);
							dongFuWnd.ResetLianShenName(submittedTechnique.Name);
							break;
						case GongFaType.DuanTi:
							dongFuWnd.ResetDuanTiXiaoLv(changeAchievementResp_.AchievementCircleValue);
							dongFuWnd.ResetDuanTiName(submittedTechnique.Name);
							break;
						}
						dongFuWnd.RecordCultivationTechnique(submittedType, submittedTechnique);
					}
					CloseWnd();
					if (changeAchievementResp_.IsSuccess)
					{
						ShowGuide(GuideType.Learn, 10);
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(changeAchievementResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("GengHuanGongFaWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(m_Common_ShaiXuanWnd);
		}
	}
}
