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

		private float ChuShiZongGaoDu;

		private GameObject ShuXingObj;

		private GameObject WeiZhiObj;

		private float WeiZhiItemHight;

		private CaiLiaoXuanZheModel m_DataSource;

		private List<MapEntity> m_DaLuMapList;

		private List<MapEntity> m_ZhouMapList;

		private List<MapEntity> m_JunMapList;

		private CaiLiaoShaiXuanSourceType m_CaiLiaoShaiXuanType;

		private int m_CurrentDengJie;

		private string[] DaLuStr = new string[5] { "大陆一", "大陆二", "大陆三", "大陆四", "大陆五" };

		private string[] ZhouStr = new string[12]
		{
			"洲一", "洲二", "洲三", "洲四", "洲五", "洲六", "洲七", "洲八", "洲九", "洲十",
			"洲十一", "洲十二"
		};

		private string[] JunStr = new string[5] { "郡一", "郡二", "郡三", "郡四", "郡五" };

		private string[] PinZhiStr = new string[9] { "一阶", "二阶", "三阶", "四阶", "五阶", "六阶", "七阶", "八阶", "九阶" };

		private string[] WuXingStr = new string[5] { "金", "木", "水", "火", "土" };

		private string[] SiQiStr = new string[5] { "全部", "温", "热", "凉", "寒" };

		private string[] WuWeiStr = new string[6] { "全部", "辛", "甘", "酸", "苦", "咸" };

		private string[] BuXieStr = new string[3] { "全部", "补", "泻" };

		private string[] GuiJingStr = new string[13]
		{
			"全部", "手厥阴", "手少阴", "足少阳", "手少阳", "足太阳", "足太阴", "手太阳", "手阳明", "手太阴",
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
			m_CaiLiaoShaiXuanType = CaiLiaoShaiXuanSourceType.LianDan;
			ChuShiZongGaoDu = Panel.ZongGanDu.sizeDelta.y;
			ShuXingObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShaiXuanShuXingItem.prefab");
			WeiZhiObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShaiXuanWeiZhiItem.prefab");
			WeiZhiItemHight = WeiZhiObj.GetComponent<RectTransform>().sizeDelta.y;
			SelectZhiWu(arg0: true);
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

		private void SelectDaLuInfo()
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

		private void SelectZhouInfo(string mapId)
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
						m_ZhouMapList = list;
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
						List<WorldCaiLiaoModel> list = new List<WorldCaiLiaoModel>();
						foreach (SearchPlantMaterialResp_1008039.Types.PlantMaterialResp_1008040 plant in searchPlantMaterialResp_.PlantList)
						{
							list.Add(new WorldCaiLiaoModel
							{
								a = plant.Id,
								c = plant.Name,
								b = m_DataSource.materialType
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
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
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
								b = m_DataSource.materialType
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("请选择材料归属地图!");
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
			float y = Panel.WuXingRectTransform.sizeDelta.y + Panel.SiQiRectTransform.sizeDelta.y + Panel.WuWeiRectTransform.sizeDelta.y + Panel.BuXieRectTransform.sizeDelta.y + Panel.GuiJingRectTransform.sizeDelta.y;
			Panel.ZongGanDu.sizeDelta = new Vector2(Panel.ZongGanDu.sizeDelta.x, y);
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

		private void SetWeiZhi(List<MapEntity> strs, RectTransform rectTransform, ToggleGroup group, UnityAction<MapEntity> select)
		{
			VerticalLayoutGroup component = rectTransform.GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < strs.Count; i++)
			{
				SetListContentParent(rectTransform, WeiZhiObj).GetComponent<CaiLiaoShaiXuanWeiZhiItem>().SetItem(strs[i], select, group);
			}
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, SetVerticalListContentHight(strs.Count, WeiZhiItemHight, component));
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

		private void SelectJunTog(bool arg0)
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

		private void SelectZhouTog(bool arg0)
		{
			if (arg0)
			{
				Panel.ZhouText.color = Color.white;
				ClearData(Panel.ZhouRectTransform);
				Panel.ZhouObject.SetActive(value: true);
				Panel.JunObject.SetActive(value: false);
				if (m_ZhouMapList == null)
				{
					m_ZhouMapList = new List<MapEntity>();
				}
				SetWeiZhi(m_ZhouMapList, Panel.ZhouRectTransform, Panel.ZhouGroup, SelectZhouWeiZhi);
			}
			else
			{
				Panel.ZhouText.color = Color.black;
			}
		}

		private void SelectDaLuTog(bool arg0)
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
			else
			{
				Panel.DaLuText.color = Color.black;
			}
		}

		private void SelectJunItem()
		{
			Panel.JunObject.SetActive(value: false);
		}

		private void SelectZhouItem()
		{
			Panel.ZhouObject.SetActive(value: false);
		}

		private void SelectDaLuItem()
		{
			Panel.DaLuObject.SetActive(value: false);
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
			SetItems(WuXingStr, Panel.WuXingRectTransform, Panel.WuXingGroup, SelectWuXingItem);
			SetItems(SiQiStr, Panel.SiQiRectTransform, Panel.SiQiGroup, SelectSiQiItem);
			SetItems(WuWeiStr, Panel.WuWeiRectTransform, Panel.WuWeiGroup, SelectWuWeiItem);
			SetItems(BuXieStr, Panel.BuXieRectTransform, Panel.BuXieGroup, SelectBuXieItem);
			SetItems(GuiJingStr, Panel.GuiJingRectTransform, Panel.GuiJingGroup, SelectGuiJingItem);
			SetZongGaoDu();
		}

		private void ClearContent()
		{
			int materialType = m_DataSource.materialType;
			m_DataSource = new CaiLiaoXuanZheModel();
			m_DataSource.materialType = materialType;
			Panel.DaLuText.color = Color.black;
			Panel.ZhouText.color = Color.black;
			Panel.JunText.color = Color.black;
			Panel.DaLuText.text = "选择大陆";
			Panel.ZhouText.text = "选择洲";
			Panel.JunText.text = "选择郡";
			m_DataSource.lianZhiType = 12;
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
			else
			{
				Panel.DaLuToggle.interactable = true;
				Panel.ZhouToggle.interactable = true;
			}
		}

		private void SelectKuangWu(bool arg0)
		{
			if (!arg0)
			{
				Panel.KuangWu.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.black;
				return;
			}
			m_DataSource.materialType = 3;
			ClearContent();
			Panel.KuangWu.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
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

		private void SelectDongWu(bool arg0)
		{
			if (!arg0)
			{
				Panel.YaoShou.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.black;
				return;
			}
			Panel.YaoShou.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
			m_DataSource.materialType = 7;
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
