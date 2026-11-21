using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiCaiLiaoShaiXuanWnd : Window
	{
		public LianQiCaiLiaoShaiXuanPanel Panel;

		private GameObject m_caiLiaoPrefabItem;

		private List<CaiLiaoJiaGe> CaiLiaoList;

		private Dictionary<string, BaseNaJieGoodEntity> m_CaiLiaoDic;

		private List<string> m_CaiLiaoList;

		private List<CaiLiaoJiaGe> m_CanBuyCaiLiaoList;

		private int m_CaiLiaoCount;

		private QiTuEntity m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiCaiLiaoShaiXuanPanel>();
			m_DataSource = new QiTuEntity();
			m_CaiLiaoList = new List<string>();
			m_CanBuyCaiLiaoList = new List<CaiLiaoJiaGe>();
			m_CaiLiaoDic = new Dictionary<string, BaseNaJieGoodEntity>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			AddButtonClickListener(Panel.GouMaiBtn, GouMaiClick);
			m_caiLiaoPrefabItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeCaiLiaoItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_CaiLiaoCount = 0;
			if (paralist.Length >= 1)
			{
				m_DataSource = paralist[0] as QiTuEntity;
			}
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetAllGoods(CaiLiaoPanDuan);
		}

		private void InitData()
		{
			if (m_DataSource == null)
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				return;
			}
			Panel.TitleName.text = m_DataSource.Name;
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(CreateDataAsny());
		}

		private IEnumerator CreateDataAsny()
		{
			if (Panel != null && m_DataSource.CaiLiaoList != null)
			{
				foreach (QiTuCaiLiaoEntity item in m_DataSource.CaiLiaoList)
				{
					if (item.TargetType == 0)
					{
						continue;
					}
					m_CaiLiaoCount++;
					DiHuoGeCaiLiaoItem component = SetListContentParent(Panel.ScrollContent, m_caiLiaoPrefabItem).GetComponent<DiHuoGeCaiLiaoItem>();
					if (CaiLiaoList != null && CaiLiaoList.Count > 0)
					{
						int panDuan = 2;
						for (int i = 0; i < CaiLiaoList.Count; i++)
						{
							if (CaiLiaoList[i].a == item.TargetId)
							{
								panDuan = 1;
								m_CanBuyCaiLiaoList.Add(CaiLiaoList[i]);
								break;
							}
						}
						component.SetQiTuValue(item, m_CaiLiaoDic, CaiLiaoXuanZe, m_CaiLiaoList, () =>
						{
							MapBtnClick(item.MapId, item.MapName);
						}, panDuan);
					}
					else
					{
						component.SetQiTuValue(item, m_CaiLiaoDic, CaiLiaoXuanZe, m_CaiLiaoList, () =>
						{
							MapBtnClick(item.MapId, item.MapName);
						}, 2);
					}
					yield return new WaitForEndOfFrame();
				}
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				GridLayoutGroup component2 = Panel.ScrollContent.gameObject.GetComponent<GridLayoutGroup>();
				SetGrticalListContentHight(m_CaiLiaoCount, Panel.ScrollContent, component2);
				Panel.QueRenBtn.interactable = true;
				Panel.GouMaiBtn.interactable = true;
			}
			Panel.CloseBtn.interactable = true;
		}

		private void MapBtnClick(string mapId, string name)
		{
			if (name == "世界掉落")
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("世界掉落!");
				return;
			}
			if (string.IsNullOrEmpty(mapId))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("虚神界创造!");
				return;
			}
			ParentMapReq_1007006 data = new ParentMapReq_1007006
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_PARENT_NAME, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ParentMapResp_1007006 parentMapResp_ = new ParentMapResp_1007006();
					parentMapResp_.MergeFrom(byteString);
					if (parentMapResp_.IsSuccess)
					{
						List<MapEntity> list = new List<MapEntity>();
						foreach (ParentMapResp_1007006.Types.MapObjectResp_1007007 item2 in parentMapResp_.Object)
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
						Panel.MapPanel.SetValue(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(parentMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
			ParentMapAndOneselfMapReq_1007012 data2 = new ParentMapAndOneselfMapReq_1007012
			{
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PARENT_MAP_AND_ONESELF_MAP, data2, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ParentMapAndOneselfMapResp_1007012 parentMapAndOneselfMapResp_ = new ParentMapAndOneselfMapResp_1007012();
					parentMapAndOneselfMapResp_.MergeFrom(byteString);
					if (parentMapAndOneselfMapResp_.IsSuccess)
					{
						List<MapEntity> list = new List<MapEntity>();
						foreach (string item3 in parentMapAndOneselfMapResp_.Name)
						{
							MapEntity item = new MapEntity
							{
								MapName = item3
							};
							list.Add(item);
						}
						Panel.MapPanel.SetValue(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(parentMapAndOneselfMapResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CaiLiaoPanDuan()
		{
			SearchLMStockReq_1004028 data = new SearchLMStockReq_1004028
			{
				TemplateId = m_DataSource.Id,
				Type = 4
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_LMSTOCK, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchLMStockResp_1004028 searchLMStockResp_ = new SearchLMStockResp_1004028();
					searchLMStockResp_.MergeFrom(byteString);
					CaiLiaoList = new List<CaiLiaoJiaGe>();
					if (searchLMStockResp_.IsSuccess)
					{
						foreach (SearchLMStockResp_1004028.Types.MaterialObjReap_1004029 material in searchLMStockResp_.MaterialList)
						{
							CaiLiaoJiaGe item = new CaiLiaoJiaGe
							{
								a = material.TemplateId,
								b = material.Price,
								c = material.GoodsName
							};
							CaiLiaoList.Add(item);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchLMStockResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					InitData();
				}
			}, isShowWaitUI: true);
		}

		private void CaiLiaoXuanZe(QiTuCaiLiaoEntity stepModel, List<BaseNaJieGoodEntity> dataSource, UnityAction<int> choosedCompelete, int index)
		{
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoChooseWnd.prefab", true, Panel.TitleName.text, stepModel.TargetName, dataSource, (UnityAction<int, BaseNaJieGoodEntity>)((int num, BaseNaJieGoodEntity goods) =>
			{
				if (m_CaiLiaoList.Contains(goods.GoodsId) && m_CaiLiaoList[index] != goods.GoodsId)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("更换材料失败,该材料已经被占用!");
				}
				else
				{
					choosedCompelete(num);
					m_CaiLiaoList[index] = goods.GoodsId;
					if (m_CaiLiaoDic.Count > num + 1)
					{
						m_CaiLiaoDic[stepModel.TargetId] = goods;
					}
				}
			}), index);
		}

		private void QueRenClick()
		{
			CheckLianDanConditions();
		}

		private void CheckLianDanConditions()
		{
			VerifyIsCanRefiningEquipmentReq_1005048 data = new VerifyIsCanRefiningEquipmentReq_1005048
			{
				MethodEquipmentId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFY_IS_CAN_REFINING_EQUIPMENT, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					VerifyIsCanRefiningEquipmentResp_1005048 verifyIsCanRefiningEquipmentResp_ = new VerifyIsCanRefiningEquipmentResp_1005048();
					verifyIsCanRefiningEquipmentResp_.MergeFrom(byteString);
					if (verifyIsCanRefiningEquipmentResp_.IsSuccess)
					{
						StartLianZhi();
						CloseWindow();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyIsCanRefiningEquipmentResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void StartLianZhi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LianQiFangWnd.prefab", true, m_DataSource);
		}

		private void GouMaiClick()
		{
			List<CaiLiaoJiaGe> list = new List<CaiLiaoJiaGe>();
			foreach (CaiLiaoJiaGe caiLiao in CaiLiaoList)
			{
				List<BaseNaJieGoodEntity> sameTemplateGoods = Singleton<RoleManager>.Instance.RoleBoxDic[10].GetSameTemplateGoods(caiLiao.a);
				if (sameTemplateGoods == null || sameTemplateGoods.Count == 0)
				{
					list.Add(caiLiao);
				}
			}
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoJiaGeWnd.prefab", true, m_DataSource, list, 2);
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
