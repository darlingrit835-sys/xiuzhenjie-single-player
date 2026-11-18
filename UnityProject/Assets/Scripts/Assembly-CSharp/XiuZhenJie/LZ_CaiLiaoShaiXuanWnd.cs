using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_CaiLiaoShaiXuanWnd : Window
	{
		public LZ_CaiLiaoShaiXuanPanel Panel;

		private GameObject ShuXingObj;

		private float m_WeiZhiItemHight;

		private CaiLiaoXuanZheModel m_DataSource;

		private List<MapEntity> m_DaLuMapList;

		private List<MapEntity> m_ContinentMapList;

		private List<MapEntity> m_JunMapList;

		private CaiLiaoShaiXuanSourceType m_CaiLiaoShaiXuanType;

		private int m_CurrentDengJie;

		private int m_CurrentShu;

		private float WeiZhiItemHight;

		private GameObject WeiZhiObj;

		private string[] m_DaLuStr = new string[5] { "大陆一", "大陆二", "大陆三", "大陆四", "大陆五" };

		private string[] m_ZhouStr = new string[12]
		{
			"洲一", "洲二", "洲三", "洲四", "洲五", "洲六", "洲七", "洲八", "洲九", "洲十",
			"洲十一", "洲十二"
		};

		private string[] m_JunStr = new string[5] { "郡一", "郡二", "郡三", "郡四", "郡五" };

		private string[] m_PinZhiStr = new string[9] { "一阶", "二阶", "三阶", "四阶", "五阶", "六阶", "七阶", "八阶", "九阶" };

		private string[] m_WuXingStr = new string[5] { "金", "木", "水", "火", "土" };

		private string[] m_SiQiStr = new string[5] { "全部", "温", "热", "凉", "寒" };

		private string[] m_WuWeiStr = new string[6] { "全部", "辛", "甘", "酸", "苦", "咸" };

		private string[] m_BuXieStr = new string[3] { "全部", "补", "泻" };

		private string[] m_GuiJingStr = new string[13]
		{
			"全部", "手厥阴", "手少阴", "足少阳", "手少阳", "足太阳", "足太阴", "手太阳", "手阳明", "手太阴",
			"足阳明", "足厥阴", "足少阴"
		};

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LZ_CaiLiaoShaiXuanPanel>();
			m_DataSource = new CaiLiaoXuanZheModel();
			m_DataSource.lianZhiType = 12;
			if (paralist != null && paralist.Length != 0)
			{
				int.TryParse(paralist[0].ToString(), out m_CurrentDengJie);
				int.TryParse(paralist[1].ToString(), out m_CurrentShu);
			}
			ShuXingObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShaiXuanShuXingItem.prefab");
			WeiZhiObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShaiXuanWeiZhiItem.prefab");
			WeiZhiItemHight = WeiZhiObj.GetComponent<RectTransform>().sizeDelta.y;
			AddButtonClickListener(Panel.BackGroundBtn, BackGroundBtn);
			ClearContent();
			AddToggleClickListener(Panel.DaLuToggle, DaLuToggle);
			AddToggleClickListener(Panel.ZhouToggle, ZhouToggle);
			AddToggleClickListener(Panel.JunToggle, JunToggle);
			AddButtonClickListener(Panel.QueDingButton, QueDingClick);
			AddButtonClickListener(Panel.QingKongXuanZeButton, QingKongButtonClick);
			AddToggleClickListener(Panel.ZhiWu, SelectZhiWu);
			AddToggleClickListener(Panel.ZiChuangPlant, SelectKuangWu);
			GetPlants();
		}

		private void SelectKuangWu(bool arg0)
		{
			if (!arg0)
			{
				Panel.ZiChuangPlant.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.black;
				return;
			}
			m_DataSource.materialType = 3;
			Panel.ZiChuangPlant.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
			ClearContent();
		}

		private void SelectZhiWu(bool arg0)
		{
			if (!arg0)
			{
				Panel.ZhiWu.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.black;
				return;
			}
			Panel.ZhiWu.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
			m_DataSource.materialType = 12;
			ClearContent();
		}

		private void BackGroundBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void GetPlants()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_ANCESTOR, null, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AncestorMapResp_1007008 ancestorMapResp_ = new AncestorMapResp_1007008();
					ancestorMapResp_.MergeFrom(byteString);
					if (ancestorMapResp_.IsSuccess)
					{
						ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
						{
							MapId = ancestorMapResp_.Id
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _o) =>
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							ByteString byteString2 = _o as ByteString;
							if (!(byteString2 == null))
							{
								ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
								childrenMapResp_.MergeFrom(byteString2);
								if (childrenMapResp_.IsSuccess)
								{
									List<MapEntity> list = new List<MapEntity>();
									foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_.Object)
									{
										MapEntity item = new MapEntity
										{
											Depth = item2.Level,
											FiveElement = item2.FiveElements,
											Id = item2.Id,
											MapName = item2.Name,
											MapType = item2.Type,
											MaxLevel = item2.MaxLevel,
											MinLevel = item2.MinLevel,
											ParentName = item2.ParentName
										};
										list.Add(item);
									}
									m_DaLuMapList = list;
								}
							}
						});
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(ancestorMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ClearContent()
		{
			int materialType = m_DataSource.materialType;
			m_DataSource = new CaiLiaoXuanZheModel();
			Panel.XuanZeGameObject.SetActive(value: true);
			Panel.DaLuText.text = "选择大陆";
			Panel.DaLuText.color = Color.black;
			Panel.ZhouText.color = Color.black;
			Panel.JunText.color = Color.black;
			Panel.ZhouText.text = "选择洲";
			Panel.JunText.text = "选择郡";
			m_DataSource.lianZhiType = 12;
			m_DataSource.materialType = materialType;
			Panel.DaLuToggle.interactable = true;
			Panel.ZhouToggle.interactable = true;
			ToggleGroup component = Panel.ZhouToggle.transform.parent.GetComponent<ToggleGroup>();
			component.allowSwitchOff = true;
			Panel.ZhouToggle.isOn = false;
			Panel.JunToggle.isOn = false;
			Panel.DaLuToggle.isOn = false;
			component.allowSwitchOff = false;
			Panel.DaLuObject.SetActive(value: false);
			Panel.ZhouObject.SetActive(value: false);
			Panel.JunObject.SetActive(value: false);
			Panel.ShuXingGameObject.SetActive(value: false);
			if (m_DataSource.materialType == 3)
			{
				Panel.DaLuText.text = "虚神界";
				Panel.ZhouText.text = "灵植协会";
				Panel.DaLuToggle.interactable = false;
				Panel.ZhouToggle.interactable = false;
				SetShuXingGameObject();
			}
		}

		private void QueDingClick()
		{
			if (CheckCaiLiao())
			{
				SelectPlant();
			}
		}

		private void SelectPlant()
		{
			SearchPlantMaterialReq_1008039 searchPlantMaterialReq_ = new SearchPlantMaterialReq_1008039
			{
				BuXie = m_DataSource.buXie,
				FiveElement = m_DataSource.fiveElement,
				GuiJing = m_DataSource.guiJing,
				SiQi = m_DataSource.siQi,
				Stage = m_CurrentDengJie,
				WuWei = m_DataSource.wuWei
			};
			if (m_DataSource.materialType == 3)
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
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchPlantMaterialResp_1008039 searchPlantMaterialResp_ = new SearchPlantMaterialResp_1008039();
					searchPlantMaterialResp_.MergeFrom(byteString);
					if (searchPlantMaterialResp_.IsSuccess)
					{
						List<LZ_WorldCaiLiaoModel> list = new List<LZ_WorldCaiLiaoModel>();
						foreach (SearchPlantMaterialResp_1008039.Types.PlantMaterialResp_1008040 plant in searchPlantMaterialResp_.PlantList)
						{
							list.Add(new LZ_WorldCaiLiaoModel
							{
								a = plant.Id,
								c = plant.Name,
								b = m_DataSource.materialType,
								Rank = plant.Rank
							});
						}
						if (list.Count == 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无匹配材料!");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("LZ_WorldCaiLiaoWnd.prefab", true, list, m_CurrentShu);
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

		private bool CheckCaiLiao()
		{
			if (m_DataSource == null)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请完整选择材料属性!");
				return false;
			}
			if (string.IsNullOrEmpty(m_DataSource.mapId) && m_DataSource.materialType != 3)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请选择材料归属地图!");
				return false;
			}
			return true;
		}

		private void JunToggle(bool arg0)
		{
			if (arg0)
			{
				Panel.JunText.color = Color.white;
				ClearData(Panel.JunRectTransform);
				Panel.JunObject.SetActive(value: true);
				if (m_JunMapList == null)
				{
					m_JunMapList = new List<MapEntity>();
				}
				SetWeiZhi(m_JunMapList, Panel.JunRectTransform, Panel.JunGroup, SelectJunWeiZhi);
			}
			else
			{
				Panel.JunText.color = Color.black;
			}
		}

		private void SelectJunWeiZhi(MapEntity str)
		{
			Panel.JunText.text = str.MapName;
			m_DataSource.mapId = str.Id;
			m_DataSource.JunName = str.MapName;
			if (Panel.DaLuText.text != "选择大陆" && Panel.JunText.text != "选择郡" && Panel.ZhouText.text != "选择洲")
			{
				SetShuXingGameObject();
			}
			Panel.JunObject.SetActive(value: false);
		}

		private void ZhouToggle(bool arg0)
		{
			if (arg0)
			{
				Panel.ZhouText.color = Color.white;
				ClearData(Panel.ZhouRectTransform);
				Panel.ZhouObject.SetActive(value: true);
				Panel.JunObject.SetActive(value: false);
				if (m_ContinentMapList == null)
				{
					m_ContinentMapList = new List<MapEntity>();
				}
				SetWeiZhi(m_ContinentMapList, Panel.ZhouRectTransform, Panel.ZhouGroup, SelectZhouWeiZhi);
			}
			else
			{
				Panel.ZhouText.color = Color.black;
			}
		}

		private void DaLuToggle(bool arg0)
		{
			if (arg0)
			{
				Panel.DaLuText.color = Color.white;
				ClearData(Panel.DaLuRectTransform);
				Panel.DaLuObject.SetActive(value: true);
				Panel.ZhouObject.SetActive(value: false);
				Panel.JunObject.SetActive(value: false);
				if (m_DaLuMapList == null)
				{
					m_DaLuMapList = new List<MapEntity>();
				}
				SetWeiZhi(m_DaLuMapList, Panel.DaLuRectTransform, Panel.DaLuGroup, SelectDaLuWeiZhi);
			}
		}

		private void SelectZhouWeiZhi(MapEntity str)
		{
			SelectJunInfo(str.Id);
			Panel.ZhouText.text = str.MapName;
			m_DataSource.mapId = str.Id;
			m_DataSource.ZhouName = str.MapName;
			m_DataSource.siQi = 0;
			m_DataSource.wuWei = 0;
			m_DataSource.guiJing = 0;
			m_DataSource.buXie = 0;
			m_DataSource.fiveElement = 0;
			if (Panel.DaLuText.text != "选择大陆")
			{
				SetShuXingGameObject();
			}
			Panel.ZhouObject.SetActive(value: false);
		}

		private void SelectDaLuWeiZhi(MapEntity str)
		{
			SelectZhouInfo(str.Id);
			m_DataSource.mapId = string.Empty;
			m_DataSource.DaLuName = str.MapName;
			m_DataSource.siQi = 0;
			m_DataSource.wuWei = 0;
			m_DataSource.guiJing = 0;
			m_DataSource.buXie = 0;
			m_DataSource.fiveElement = 0;
			Panel.DaLuText.text = str.MapName;
			Panel.ZhouText.text = "选择洲";
			Panel.JunText.text = "选择郡";
			if (Panel.DaLuText.text != "选择大陆" && Panel.JunText.text != "选择郡" && Panel.ZhouText.text != "选择洲")
			{
				SetShuXingGameObject();
			}
			Panel.DaLuObject.SetActive(value: false);
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
			SetItems(m_WuXingStr, Panel.WuXingRectTransform, Panel.WuXingGroup, SelectWuXingItem);
			SetItems(m_SiQiStr, Panel.SiQiRectTransform, Panel.SiQiGroup, SelectSiQiItem);
			SetItems(m_WuWeiStr, Panel.WuWeiRectTransform, Panel.WuWeiGroup, SelectWuWeiItem);
			SetItems(m_BuXieStr, Panel.BuXieRectTransform, Panel.BuXieGroup, SelectBuXieItem);
			SetItems(m_GuiJingStr, Panel.GuiJingRectTransform, Panel.GuiJingGroup, SelectGuiJingItem);
			SetZongGaoDu();
		}

		private void SelectWuXingItem(string str)
		{
			switch (str)
			{
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
		}

		private void SelectSiQiItem(string str)
		{
			switch (str)
			{
			case "全部":
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
		}

		private void SelectWuWeiItem(string str)
		{
			switch (str)
			{
			case "全部":
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
		}

		private void SelectBuXieItem(string str)
		{
			switch (str)
			{
			case "全部":
				m_DataSource.buXie = 0;
				break;
			case "补":
				m_DataSource.buXie = 1;
				break;
			case "泻":
				m_DataSource.buXie = 2;
				break;
			}
		}

		private void SelectGuiJingItem(string str)
		{
			switch (str)
			{
			case "全部":
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
		}

		private void SetItems(string[] strs, RectTransform rectTransform, ToggleGroup group, UnityAction<string> select)
		{
			GridLayoutGroup component = rectTransform.GetComponent<GridLayoutGroup>();
			for (int i = 0; i < strs.Length; i++)
			{
				SetListContentParent(rectTransform, ShuXingObj).GetComponent<CaiLiaoShaiXuanItem>().SetItem(strs[i], select, group);
			}
			SetGrticalListContentHight(strs.Length, rectTransform, component);
		}

		private void SetZongGaoDu()
		{
			float y = Panel.WuXingRectTransform.sizeDelta.y + Panel.SiQiRectTransform.sizeDelta.y + Panel.WuWeiRectTransform.sizeDelta.y + Panel.BuXieRectTransform.sizeDelta.y + Panel.GuiJingRectTransform.sizeDelta.y;
			Panel.ZongGanDu.sizeDelta = new Vector2(Panel.ZongGanDu.sizeDelta.x, y);
		}

		private void SelectZhouInfo(string mapId)
		{
			ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
					childrenMapResp_.MergeFrom(byteString);
					if (childrenMapResp_.IsSuccess)
					{
						List<MapEntity> list = new List<MapEntity>();
						foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_.Object)
						{
							MapEntity item = new MapEntity
							{
								Depth = item2.Level,
								FiveElement = item2.FiveElements,
								Id = item2.Id,
								MapName = item2.Name,
								MapType = item2.Type,
								MaxLevel = item2.MaxLevel,
								MinLevel = item2.MinLevel,
								ParentName = item2.ParentName
							};
							list.Add(item);
						}
						m_ContinentMapList = list;
					}
				}
			}, isShowWaitUI: true);
		}

		private void SelectJunInfo(string mapId)
		{
			ChildrenMapReq_1007003 data = new ChildrenMapReq_1007003
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_CHILDREN, data, (object _o) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _o as ByteString;
				if (!(byteString == null))
				{
					ChildrenMapResp_1007003 childrenMapResp_ = new ChildrenMapResp_1007003();
					childrenMapResp_.MergeFrom(byteString);
					if (childrenMapResp_.IsSuccess)
					{
						List<MapEntity> list = new List<MapEntity>();
						foreach (ChildrenMapResp_1007003.Types.MapObjectResp_1007004 item2 in childrenMapResp_.Object)
						{
							MapEntity item = new MapEntity
							{
								Depth = item2.Level,
								FiveElement = item2.FiveElements,
								Id = item2.Id,
								MapName = item2.Name,
								MapType = item2.Type,
								MaxLevel = item2.MaxLevel,
								MinLevel = item2.MinLevel,
								ParentName = item2.ParentName
							};
							list.Add(item);
						}
						m_JunMapList = list;
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetWeiZhi(List<MapEntity> strs, RectTransform rectTransform, ToggleGroup group, UnityAction<MapEntity> select)
		{
			VerticalLayoutGroup component = rectTransform.GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < strs.Count; i++)
			{
				SetListContentParent(rectTransform, WeiZhiObj).GetComponent<CaiLiaoShaiXuanWeiZhiItem>().SetItem(strs[i], select, group);
			}
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, SetVerticalListContentHight(strs.Count, WeiZhiItemHight, component));
		}

		private void QingKongButtonClick()
		{
			ClearContent();
		}

		private void ClearData(RectTransform rectTransform)
		{
			for (int i = 0; i < rectTransform.transform.childCount; i++)
			{
				Object.Destroy(rectTransform.transform.GetChild(i).gameObject);
			}
		}
	}
}
