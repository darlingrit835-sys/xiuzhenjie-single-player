using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CaiLiaoShaiXuanWnd : Window
	{
		public CaiLiaoShaiXuanPanel Panel;

		private MaterialFilterView view;

		private GameObject ShuXingObj;

		private GameObject WeiZhiObj;

		private string continentId;

		private string provinceId;

		private int continentRequest;

		private int provinceRequest;

		private int countyRequest;

		private int nextSearch;

		private int pendingSearch;

		private CaiLiaoXuanZheModel m_DataSource;

		private List<MapEntity> m_DaLuMapList;

		private List<MapEntity> m_ZhouMapList;

		private List<MapEntity> m_JunMapList;

		private int m_CurrentDengJie;

		private static readonly string[] WuXingStr = new string[6] { "不限", "金", "木", "水", "火", "土" };

		private static readonly string[] SiQiStr = new string[5] { "不限", "温", "热", "凉", "寒" };

		private static readonly string[] WuWeiStr = new string[6] { "不限", "辛", "甘", "酸", "苦", "咸" };

		private static readonly string[] BuXieStr = new string[3] { "不限", "补", "泻" };

		private static readonly string[] GuiJingStr = new string[13]
		{
			"不限", "手厥阴", "手少阴", "足少阳", "手少阳", "足太阳", "足太阴", "手太阳", "手阳明", "手太阴",
			"足阳明", "足厥阴", "足少阴"
		};

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<CaiLiaoShaiXuanPanel>();
			m_DataSource = new CaiLiaoXuanZheModel();
			m_DataSource.lianZhiType = 12;
			if (paralist != null && paralist.Length != 0)
			{
				int.TryParse(paralist[0].ToString(), out m_CurrentDengJie);
			}
			AddButtonClickListener(Panel.BackGroundBtn, BackGroundBtnClick);
			AddToggleClickListener(Panel.ZhiWu, SelectZhiWu);
			AddToggleClickListener(Panel.KuangWu, SelectKuangWu);
			AddToggleClickListener(Panel.YaoShou, SelectDongWu);
			AddToggleClickListener(Panel.DaLuToggle, SelectDaLuTog);
			AddToggleClickListener(Panel.ZhouToggle, SelectZhouTog);
			AddToggleClickListener(Panel.JunToggle, SelectJunTog);
			AddButtonClickListener(Panel.QueDingButton, QueDingClick);
			AddButtonClickListener(Panel.QingKongXuanZeButton, QingKongButtonClick);
			ShuXingObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShaiXuanShuXingItem.prefab");
			WeiZhiObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShaiXuanWeiZhiItem.prefab");
			Panel.DaLuToggle.group.allowSwitchOff = true;
			view = new MaterialFilterView(Panel);
			SelectZhiWu(true);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			SelectDaLuInfo();
		}

		private void BackGroundBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public override void OnDisable()
		{
			ClearPendingSearch();
			base.OnDisable();
		}

		public override void OnClose()
		{
			ClearPendingSearch();
			continentRequest++;
			provinceRequest++;
			countyRequest++;
			view?.Dispose();
			base.OnClose();
		}

		private void SelectDaLuInfo()
		{
			int request = ++continentRequest;
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_ANCESTOR, null, response =>
			{
				if (request != continentRequest || !(response is ByteString bytes))
					return;
				var ancestor = AncestorMapResp_1007008.Parser.ParseFrom(bytes);
				if (!ancestor.IsSuccess)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(ancestor.Code));
					return;
				}
				LoadChildren(ancestor.Id, maps =>
				{
					if (request != continentRequest)
						return;
					m_DaLuMapList = maps;
					if (Panel.DaLuObject.activeSelf)
						SelectDaLuTog(true);
				});
			});
		}

		private void SelectZhouInfo(string mapId)
		{
			int request = ++provinceRequest;
			LoadChildren(mapId, maps =>
			{
				if (request != provinceRequest || mapId != continentId)
					return;
				m_ZhouMapList = maps;
				if (Panel.ZhouObject.activeSelf)
					SelectZhouTog(true);
			});
		}

		private void SelectJunInfo(string mapId)
		{
			int request = ++countyRequest;
			LoadChildren(mapId, maps =>
			{
				if (request != countyRequest || mapId != provinceId)
					return;
				m_JunMapList = maps;
				if (Panel.JunObject.activeSelf)
					SelectJunTog(true);
			});
		}

		private static void LoadChildren(string parentId, UnityAction<List<MapEntity>> complete)
		{
			var data = new ChildrenMapReq_1007003 { MapId = parentId };
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, response =>
			{
				if (!(response is ByteString bytes))
					return;
				var children = ChildrenMapResp_1007003.Parser.ParseFrom(bytes);
				if (!children.IsSuccess)
					return;
				var maps = new List<MapEntity>(children.Object.Count);
				foreach (var entry in children.Object)
				{
					maps.Add(new MapEntity
					{
						Depth = entry.Level,
						FiveElement = entry.FiveElements,
						Id = entry.Id,
						MapName = entry.Name,
						MapType = entry.Type,
						MaxLevel = entry.MaxLevel,
						MinLevel = entry.MinLevel,
						ParentName = entry.ParentName
					});
				}
				complete(maps);
			});
		}

		private void QingKongButtonClick()
		{
			ClearContent();
		}

		private void QueDingClick()
		{
			if (CheckCaiLiao())
			{
				m_DataSource.stage = m_CurrentDengJie;
				if (m_DataSource.materialType == 12 || m_DataSource.materialType == 3)
				{
					SelectPlant();
				}
				else if (m_DataSource.materialType == 7)
				{
					SelectAnimal();
				}
			}
		}

		private void ClearPendingSearch()
		{
			if (pendingSearch == 0)
				return;
			pendingSearch = 0;
			MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
		}

		private bool AcceptSearch(int request)
		{
			if (request != pendingSearch)
				return false;
			ClearPendingSearch();
			return true;
		}

		private void SelectPlant()
		{
			int request = ++nextSearch;
			pendingSearch = request;
			int materialType = m_DataSource.materialType;
			SearchPlantMaterialReq_1008039 searchPlantMaterialReq_ = new SearchPlantMaterialReq_1008039
			{
				BuXie = m_DataSource.buXie,
				FiveElement = m_DataSource.fiveElement,
				GuiJing = m_DataSource.guiJing,
				SiQi = m_DataSource.siQi,
				Stage = m_CurrentDengJie,
				WuWei = m_DataSource.wuWei
			};
			if (materialType == 3)
			{
				searchPlantMaterialReq_.IsRoleCreated = true;
			}
			else
			{
				searchPlantMaterialReq_.IsRoleCreated = false;
				searchPlantMaterialReq_.MapId = m_DataSource.mapId;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_PLANT_MATERIAL, searchPlantMaterialReq_, (object _obj) =>
			{
				if (!AcceptSearch(request))
					return;
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchPlantMaterialResp_1008039 searchPlantMaterialResp_ = new SearchPlantMaterialResp_1008039();
					searchPlantMaterialResp_.MergeFrom(byteString);
					if (searchPlantMaterialResp_.IsSuccess)
					{
						List<WorldCaiLiaoModel> list = new List<WorldCaiLiaoModel>();
						foreach (SearchPlantMaterialResp_1008039.Types.PlantMaterialResp_1008040 plant in searchPlantMaterialResp_.PlantList)
						{
							list.Add(new WorldCaiLiaoModel
							{
								a = plant.Id,
								c = plant.Name,
								b = materialType
							});
						}
						if (list.Count == 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无匹配材料!");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("WorldCaiLiaoWnd.prefab", true, 1, list);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchPlantMaterialResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SelectAnimal()
		{
			int request = ++nextSearch;
			pendingSearch = request;
			int materialType = m_DataSource.materialType;
			SearchAnimalMaterialReq_1008041 data = new SearchAnimalMaterialReq_1008041
			{
				BuXie = m_DataSource.buXie,
				FiveElement = m_DataSource.fiveElement,
				GuiJing = m_DataSource.guiJing,
				MapId = m_DataSource.mapId,
				SiQi = m_DataSource.siQi,
				Stage = m_CurrentDengJie,
				WuWei = m_DataSource.wuWei
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_ANIMAL_MATERIAL, data, (object _obj) =>
			{
				if (!AcceptSearch(request))
					return;
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchAnimalMaterialResp_1008041 searchAnimalMaterialResp_ = new SearchAnimalMaterialResp_1008041();
					searchAnimalMaterialResp_.MergeFrom(byteString);
					if (searchAnimalMaterialResp_.IsSuccess)
					{
						List<WorldCaiLiaoModel> list = new List<WorldCaiLiaoModel>();
						foreach (SearchAnimalMaterialResp_1008041.Types.AnimalMaterialResp_1008042 animal in searchAnimalMaterialResp_.AnimalList)
						{
							list.Add(new WorldCaiLiaoModel
							{
								a = animal.Id,
								c = animal.Name,
								b = materialType
							});
						}
						if (list.Count == 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无匹配材料!");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("WorldCaiLiaoWnd.prefab", true, 1, list);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchAnimalMaterialResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private bool CheckCaiLiao()
		{
			if (m_DataSource == null)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请完整选择材料属性!");
				return false;
			}
			if (string.IsNullOrEmpty(m_DataSource.mapId) && m_DataSource.materialType != 3)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先选择大陆和洲，郡可不限。");
				return false;
			}
			if (m_DataSource.materialType == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请选择材料种类!");
				return false;
			}
			return true;
		}

		private void SetZongGaoDu()
		{
			view.LayoutAttributes();
		}

		private void SetItems(string[] names, RectTransform content, ToggleGroup group, UnityAction<string> select)
		{
			group.allowSwitchOff = true;
			for (int i = 0; i < names.Length; i++)
			{
				var item = SetListContentParent(content, ShuXingObj).GetComponent<CaiLiaoShaiXuanItem>();
				item.SetItem(names[i], select, group);
				item.toggle.SetIsOnWithoutNotify(i == 0);
				view.StyleAttribute(item);
			}
			group.allowSwitchOff = false;
		}

		private void SelectWuXingItem(string str)
		{
			int previous = m_DataSource.fiveElement;
			switch (str)
			{
			case "全部":
			case "不限":
				m_DataSource.fiveElement = 0;
				break;
			case "金":
				m_DataSource.fiveElement = 1;
				break;
			case "木":
				m_DataSource.fiveElement = 2;
				break;
			case "水":
				m_DataSource.fiveElement = 3;
				break;
			case "火":
				m_DataSource.fiveElement = 4;
				break;
			case "土":
				m_DataSource.fiveElement = 5;
				break;
			}
			if (previous != m_DataSource.fiveElement)
				ClearPendingSearch();
			view.Refresh(m_DataSource);
		}

		private void SelectSiQiItem(string str)
		{
			int previous = m_DataSource.siQi;
			switch (str)
			{
			case "全部":
			case "不限":
				m_DataSource.siQi = 0;
				break;
			case "温":
				m_DataSource.siQi = 3;
				break;
			case "热":
				m_DataSource.siQi = 4;
				break;
			case "凉":
				m_DataSource.siQi = 2;
				break;
			case "寒":
				m_DataSource.siQi = 1;
				break;
			}
			if (previous != m_DataSource.siQi)
				ClearPendingSearch();
			view.Refresh(m_DataSource);
		}

		private void SelectWuWeiItem(string str)
		{
			int previous = m_DataSource.wuWei;
			switch (str)
			{
			case "全部":
			case "不限":
				m_DataSource.wuWei = 0;
				break;
			case "辛":
				m_DataSource.wuWei = 1;
				break;
			case "甘":
				m_DataSource.wuWei = 4;
				break;
			case "酸":
				m_DataSource.wuWei = 3;
				break;
			case "苦":
				m_DataSource.wuWei = 2;
				break;
			case "咸":
				m_DataSource.wuWei = 5;
				break;
			}
			if (previous != m_DataSource.wuWei)
				ClearPendingSearch();
			view.Refresh(m_DataSource);
		}

		private void SelectBuXieItem(string str)
		{
			int previous = m_DataSource.buXie;
			switch (str)
			{
			case "全部":
			case "不限":
				m_DataSource.buXie = 0;
				break;
			case "补":
				m_DataSource.buXie = 1;
				break;
			case "泻":
				m_DataSource.buXie = 2;
				break;
			}
			if (previous != m_DataSource.buXie)
				ClearPendingSearch();
			view.Refresh(m_DataSource);
		}

		private void SetWeiZhi(List<MapEntity> maps, RectTransform content, ToggleGroup group, string chosenId, UnityAction<MapEntity> select)
		{
			group.allowSwitchOff = true;
			ClearData(content);
			float rowHeight = WeiZhiObj.GetComponent<RectTransform>().rect.height;
			bool hasSelection = false;
			int count = maps == null ? 0 : maps.Count;
			for (int i = 0; i < count; i++)
			{
				var item = SetListContentParent(content, WeiZhiObj).GetComponent<CaiLiaoShaiXuanWeiZhiItem>();
				item.SetItem(maps[i], select, group);
				bool selected = maps[i].Id == chosenId;
				item.toggle.SetIsOnWithoutNotify(selected);
				view.StylePlace(item);
				hasSelection |= selected;
				float preferred = LayoutUtility.GetPreferredHeight((RectTransform)item.transform);
				rowHeight = preferred > 0f ? preferred : ((RectTransform)item.transform).rect.height;
			}
			group.allowSwitchOff = !hasSelection;
			float height = SetVerticalListContentHight(count, rowHeight, content.GetComponent<VerticalLayoutGroup>());
			content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
		}

		private void SelectGuiJingItem(string str)
		{
			int previous = m_DataSource.guiJing;
			switch (str)
			{
			case "全部":
			case "不限":
				m_DataSource.guiJing = 0;
				break;
			case "手厥阴":
				m_DataSource.guiJing = 1;
				break;
			case "手少阴":
				m_DataSource.guiJing = 2;
				break;
			case "足少阳":
				m_DataSource.guiJing = 3;
				break;
			case "手少阳":
				m_DataSource.guiJing = 4;
				break;
			case "足太阳":
				m_DataSource.guiJing = 5;
				break;
			case "足太阴":
				m_DataSource.guiJing = 6;
				break;
			case "手太阳":
				m_DataSource.guiJing = 7;
				break;
			case "手阳明":
				m_DataSource.guiJing = 8;
				break;
			case "手太阴":
				m_DataSource.guiJing = 9;
				break;
			case "足阳明":
				m_DataSource.guiJing = 10;
				break;
			case "足厥阴":
				m_DataSource.guiJing = 11;
				break;
			case "足少阴":
				m_DataSource.guiJing = 12;
				break;
			}
			if (previous != m_DataSource.guiJing)
				ClearPendingSearch();
			view.Refresh(m_DataSource);
		}

		private void SelectJunTog(bool open)
		{
			Panel.JunGroup.allowSwitchOff = true;
			Panel.JunObject.SetActive(open);
			if (open)
			{
				var counties = new List<MapEntity> { new MapEntity { Id = provinceId, MapName = "不限" } };
				if (m_JunMapList != null)
					counties.AddRange(m_JunMapList);
				SetWeiZhi(counties, Panel.JunRectTransform, Panel.JunGroup, m_DataSource.mapId, SelectJunWeiZhi);
			}
			view.Refresh(m_DataSource);
		}

		private void SelectZhouTog(bool open)
		{
			Panel.ZhouGroup.allowSwitchOff = true;
			Panel.ZhouObject.SetActive(open);
			if (open)
				SetWeiZhi(m_ZhouMapList, Panel.ZhouRectTransform, Panel.ZhouGroup, provinceId, SelectZhouWeiZhi);
			view.Refresh(m_DataSource);
		}

		private void SelectDaLuTog(bool open)
		{
			Panel.DaLuGroup.allowSwitchOff = true;
			Panel.DaLuObject.SetActive(open);
			if (open)
				SetWeiZhi(m_DaLuMapList, Panel.DaLuRectTransform, Panel.DaLuGroup, continentId, SelectDaLuWeiZhi);
			view.Refresh(m_DataSource);
		}

		private void SelectJunWeiZhi(MapEntity map)
		{
			if (map.Id != m_DataSource.mapId)
				ClearPendingSearch();
			bool wholeProvince = map.Id == provinceId;
			m_DataSource.JunName = wholeProvince ? string.Empty : map.MapName;
			m_DataSource.mapId = map.Id;
			Panel.JunText.text = wholeProvince ? "郡不限" : map.MapName;
			Panel.JunToggle.SetIsOnWithoutNotify(false);
			Panel.JunObject.SetActive(false);
			view.Refresh(m_DataSource);
		}

		private void SelectZhouWeiZhi(MapEntity map)
		{
			if (map.Id != provinceId)
			{
				ClearPendingSearch();
				provinceId = map.Id;
				m_DataSource.ZhouName = map.MapName;
				m_DataSource.JunName = string.Empty;
				m_DataSource.mapId = provinceId;
				Panel.ZhouText.text = map.MapName;
				Panel.JunText.text = "郡不限";
				Panel.JunToggle.interactable = true;
				m_JunMapList = null;
			}
			Panel.ZhouToggle.SetIsOnWithoutNotify(false);
			Panel.ZhouObject.SetActive(false);
			view.Refresh(m_DataSource);
			if (m_JunMapList == null)
				SelectJunInfo(provinceId);
		}

		private void SelectDaLuWeiZhi(MapEntity map)
		{
			if (map.Id != continentId)
			{
				ClearPendingSearch();
				continentId = map.Id;
				provinceId = null;
				countyRequest++;
				m_DataSource.DaLuName = map.MapName;
				m_DataSource.ZhouName = string.Empty;
				m_DataSource.JunName = string.Empty;
				m_DataSource.mapId = string.Empty;
				Panel.DaLuText.text = map.MapName;
				Panel.ZhouText.text = "选择洲";
				Panel.JunText.text = "郡不限";
				Panel.ZhouToggle.interactable = true;
				Panel.JunToggle.interactable = false;
				m_ZhouMapList = null;
				m_JunMapList = null;
			}
			Panel.DaLuToggle.SetIsOnWithoutNotify(false);
			Panel.DaLuObject.SetActive(false);
			view.Refresh(m_DataSource);
			if (m_ZhouMapList == null)
				SelectZhouInfo(continentId);
		}

		private void SetShuXingGameObject()
		{
			Panel.DaLuObject.SetActive(value: false);
			Panel.ZhouObject.SetActive(value: false);
			Panel.JunObject.SetActive(value: false);
			Panel.ShuXingGameObject.SetActive(value: true);
			ClearData(Panel.WuXingRectTransform);
			ClearData(Panel.SiQiRectTransform);
			ClearData(Panel.WuWeiRectTransform);
			ClearData(Panel.BuXieRectTransform);
			ClearData(Panel.GuiJingRectTransform);
			SetItems(WuXingStr, Panel.WuXingRectTransform, Panel.WuXingGroup, SelectWuXingItem);
			SetItems(SiQiStr, Panel.SiQiRectTransform, Panel.SiQiGroup, SelectSiQiItem);
			SetItems(WuWeiStr, Panel.WuWeiRectTransform, Panel.WuWeiGroup, SelectWuWeiItem);
			SetItems(BuXieStr, Panel.BuXieRectTransform, Panel.BuXieGroup, SelectBuXieItem);
			SetItems(GuiJingStr, Panel.GuiJingRectTransform, Panel.GuiJingGroup, SelectGuiJingItem);
			SetZongGaoDu();
		}

		private void ClearContent()
		{
			ClearPendingSearch();
			m_DataSource = new CaiLiaoXuanZheModel { materialType = m_DataSource.materialType, lianZhiType = 12 };
			continentId = null;
			provinceId = null;
			provinceRequest++;
			countyRequest++;
			m_ZhouMapList = null;
			m_JunMapList = null;

			bool roleCreated = m_DataSource.materialType == 3;
			Panel.DaLuText.text = roleCreated ? "虚神界" : "选择大陆";
			Panel.ZhouText.text = roleCreated ? "灵植协会" : "选择洲";
			Panel.JunText.text = "郡不限";
			Panel.DaLuToggle.interactable = !roleCreated;
			Panel.ZhouToggle.interactable = false;
			Panel.JunToggle.interactable = false;
			Panel.DaLuToggle.SetIsOnWithoutNotify(false);
			Panel.ZhouToggle.SetIsOnWithoutNotify(false);
			Panel.JunToggle.SetIsOnWithoutNotify(false);
			SetShuXingGameObject();
			view.Refresh(m_DataSource);
		}

		private void SelectKuangWu(bool selected)
		{
			if (selected && m_DataSource.materialType != 3)
			{
				m_DataSource.materialType = 3;
				ClearContent();
			}
		}

		private void SelectZhiWu(bool selected)
		{
			if (selected && m_DataSource.materialType != 12)
			{
				m_DataSource.materialType = 12;
				ClearContent();
			}
		}

		private void SelectDongWu(bool selected)
		{
			if (selected && m_DataSource.materialType != 7)
			{
				m_DataSource.materialType = 7;
				ClearContent();
			}
		}

		private static void ClearData(RectTransform content)
		{
			for (int i = content.childCount - 1; i >= 0; i--)
			{
				var child = content.GetChild(i);
				var toggle = child.GetComponent<Toggle>();
				if (toggle == null)
					continue;
				child.gameObject.SetActive(false);
				toggle.group = null;
				child.SetParent(null, false);
				Object.Destroy(child.gameObject);
			}
		}
	}
}
