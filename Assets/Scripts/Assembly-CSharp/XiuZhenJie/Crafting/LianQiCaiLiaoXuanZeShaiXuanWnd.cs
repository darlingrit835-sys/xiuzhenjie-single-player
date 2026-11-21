using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiCaiLiaoXuanZeShaiXuanWnd : Window
	{
		public LianQiCaiLiaoXuanZeShaiXuanPanel Panel;

		private float ChuShiZongGaoDu;

		private GameObject ShuXingObj;

		private GameObject WeiZhiObj;

		private float WeiZhiItemHight;

		private CaiLiaoXuanZheModel m_DataSource;

		private List<MapEntity> m_DaLuMapList;

		private List<MapEntity> m_ZhouMapList;

		private List<MapEntity> m_JunMapList;

		private string[] WuXingStr = new string[5] { "金", "木", "水", "火", "土" };

		private string[] QiangDuDuStr = new string[4] { "全部", "软", "固", "硬" };

		private string[] RenDuStr = new string[4] { "全部", "脆", "柔", "韧" };

		private int m_CurrentDengJie;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiCaiLiaoXuanZeShaiXuanPanel>();
			m_DataSource = new CaiLiaoXuanZheModel();
			m_DataSource.lianZhiType = 12;
			if (paralist != null && paralist.Length != 0)
			{
				int.TryParse(paralist[0].ToString(), out m_CurrentDengJie);
			}
			AddButtonClickListener(Panel.BackGroundBtn, CloseWindow);
			AddToggleClickListener(Panel.ZhiWu, SelectZhiWu);
			AddToggleClickListener(Panel.SelfCreate, SelectCreateSelfZhiWu);
			AddToggleClickListener(Panel.KuangWu, SelectKuangWu);
			AddToggleClickListener(Panel.YaoShou, SelectDongWu);
			AddToggleClickListener(Panel.DaLuToggle, SelectDaLuTog);
			AddToggleClickListener(Panel.ZhouToggle, SelectZhouTog);
			AddToggleClickListener(Panel.JunToggle, SelectJunTog);
			AddButtonClickListener(Panel.QueDingButton, QueDingClick);
			AddButtonClickListener(Panel.QingKongXuanZeButton, ClearContent);
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

		private void SelectKuangWu(bool arg0)
		{
			if (!arg0)
			{
				Panel.KuangWu.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.black;
				return;
			}
			m_DataSource.materialType = 13;
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

		private void SelectCreateSelfZhiWu(bool arg0)
		{
			if (!arg0)
			{
				Panel.SelfCreate.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.black;
				return;
			}
			Panel.SelfCreate.transform.Find("Text").gameObject.GetComponent<Text>().color = Color.white;
			m_DataSource.materialType = 3;
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
			float y = Panel.WuXingRectTransform.sizeDelta.y + Panel.QiangDuRectTransform.sizeDelta.y + Panel.RenDuRectTransform.sizeDelta.y;
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

		private void SetWeiZhi(List<MapEntity> strs, RectTransform rectTransform, ToggleGroup group, UnityAction<MapEntity> select)
		{
			VerticalLayoutGroup component = rectTransform.GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < strs.Count; i++)
			{
				SetListContentParent(rectTransform, WeiZhiObj).GetComponent<CaiLiaoShaiXuanWeiZhiItem>().SetItem(strs[i], select, group);
			}
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, SetVerticalListContentHight(strs.Count, WeiZhiItemHight, component));
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

		private void SelectQiangDuItem(string str)
		{
			switch (str)
			{
			case "全部":
				m_DataSource.JianDu = 0;
				break;
			case "软":
				m_DataSource.JianDu = 1;
				break;
			case "固":
				m_DataSource.JianDu = 2;
				break;
			case "硬":
				m_DataSource.JianDu = 3;
				break;
			}
		}

		private void SelectRenDuItem(string str)
		{
			switch (str)
			{
			case "全部":
				m_DataSource.RenDu = 0;
				break;
			case "脆":
				m_DataSource.RenDu = 1;
				break;
			case "柔":
				m_DataSource.RenDu = 2;
				break;
			case "韧":
				m_DataSource.RenDu = 3;
				break;
			}
		}

		private void SetShuXingGameObject()
		{
			Panel.DaLuObject.SetActive(value: false);
			Panel.ZhouObject.SetActive(value: false);
			Panel.JunObject.SetActive(value: false);
			Panel.ShuXingGameObject.SetActive(value: true);
			ClearData(Panel.WuXingRectTransform);
			ClearData(Panel.QiangDuRectTransform);
			ClearData(Panel.RenDuRectTransform);
			SetItems(WuXingStr, Panel.WuXingRectTransform, Panel.WuXingGroup, SelectWuXingItem);
			SetItems(QiangDuDuStr, Panel.QiangDuRectTransform, Panel.QiangDuGroup, SelectQiangDuItem);
			SetItems(RenDuStr, Panel.RenDuRectTransform, Panel.RenDuGroup, SelectRenDuItem);
			SetZongGaoDu();
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
				else if (m_DataSource.materialType == 13)
				{
					SelectKuangWu();
				}
			}
		}

		private void SelectPlant()
		{
			SearchCreateEquipmentPlantMaterialReq_1008075 searchCreateEquipmentPlantMaterialReq_ = new SearchCreateEquipmentPlantMaterialReq_1008075
			{
				FiveElement = m_DataSource.fiveElement,
				Stage = m_CurrentDengJie,
				QiangDu = m_DataSource.JianDu,
				RenDu = m_DataSource.RenDu
			};
			if (m_DataSource.materialType == 3)
			{
				searchCreateEquipmentPlantMaterialReq_.IsRoleCreated = true;
			}
			else
			{
				searchCreateEquipmentPlantMaterialReq_.IsRoleCreated = false;
				searchCreateEquipmentPlantMaterialReq_.MapId = m_DataSource.mapId;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchCreateEquipmentPlantMaterial, searchCreateEquipmentPlantMaterialReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchCreateEquipmentPlantMaterialResp_1008075 searchCreateEquipmentPlantMaterialResp_ = new SearchCreateEquipmentPlantMaterialResp_1008075();
					searchCreateEquipmentPlantMaterialResp_.MergeFrom(byteString);
					if (searchCreateEquipmentPlantMaterialResp_.IsSuccess)
					{
						List<WorldCaiLiaoModel> list = new List<WorldCaiLiaoModel>();
						foreach (SearchCreateEquipmentPlantMaterialResp_1008075.Types.PlantMaterialResp_1008076 plant in searchCreateEquipmentPlantMaterialResp_.PlantList)
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
							Singleton<UIManager>.Instance.PopUpWnd("WorldCaiLiaoWnd.prefab", true, 2, list);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchCreateEquipmentPlantMaterialResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SelectAnimal()
		{
			SearchCreateEquipmentAnimalMaterialReq_1008077 data = new SearchCreateEquipmentAnimalMaterialReq_1008077
			{
				FiveElement = m_DataSource.fiveElement,
				MapId = m_DataSource.mapId,
				QiangDu = m_DataSource.JianDu,
				RenDu = m_DataSource.RenDu,
				Stage = m_DataSource.stage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchCreateEquipmentAnimalMaterial, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchCreateEquipmentAnimalMaterialResp_1008077 searchCreateEquipmentAnimalMaterialResp_ = new SearchCreateEquipmentAnimalMaterialResp_1008077();
					searchCreateEquipmentAnimalMaterialResp_.MergeFrom(byteString);
					if (searchCreateEquipmentAnimalMaterialResp_.IsSuccess)
					{
						List<WorldCaiLiaoModel> list = new List<WorldCaiLiaoModel>();
						foreach (SearchCreateEquipmentAnimalMaterialResp_1008077.Types.AnimalMaterialResp_1008078 animal in searchCreateEquipmentAnimalMaterialResp_.AnimalList)
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
							Singleton<UIManager>.Instance.PopUpWnd("WorldCaiLiaoWnd.prefab", true, 2, list);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchCreateEquipmentAnimalMaterialResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SelectKuangWu()
		{
			SearchCreateEquipmentMineralMaterialReq_1008079 data = new SearchCreateEquipmentMineralMaterialReq_1008079
			{
				FiveElement = m_DataSource.fiveElement,
				MapId = m_DataSource.mapId,
				QiangDu = m_DataSource.JianDu,
				RenDu = m_DataSource.RenDu,
				Stage = m_DataSource.stage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchCreateEquipmentMineralMaterial, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SearchCreateEquipmentMineralMaterialResp_1008079 searchCreateEquipmentMineralMaterialResp_ = new SearchCreateEquipmentMineralMaterialResp_1008079();
					searchCreateEquipmentMineralMaterialResp_.MergeFrom(byteString);
					if (searchCreateEquipmentMineralMaterialResp_.IsSuccess)
					{
						List<WorldCaiLiaoModel> list = new List<WorldCaiLiaoModel>();
						foreach (SearchCreateEquipmentMineralMaterialResp_1008079.Types.MineralMaterialResp_1008080 mineral in searchCreateEquipmentMineralMaterialResp_.MineralList)
						{
							list.Add(new WorldCaiLiaoModel
							{
								a = mineral.Id,
								c = mineral.Name,
								b = m_DataSource.materialType
							});
						}
						if (list.Count == 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("无匹配材料!");
						}
						else
						{
							Singleton<UIManager>.Instance.PopUpWnd("WorldCaiLiaoWnd.prefab", true, 2, list);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchCreateEquipmentMineralMaterialResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
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

		private void ClearData(RectTransform rectTransform)
		{
			for (int i = 0; i < rectTransform.transform.childCount; i++)
			{
				Object.Destroy(rectTransform.transform.GetChild(i).gameObject);
			}
		}
	}
}
