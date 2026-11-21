using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoShaiXuanWnd : Window
	{
		public DiHuoGeCaiLiaoShaiXuanPanel Panel;

		private DanFangModel m_DataSource;

		private GameObject m_caiLiaoPrefabItem;

		private List<CaiLiaoJiaGe> CaiLiaoList;

		private Dictionary<string, BaseNaJieGoodEntity> m_CaiLiaoDic;

		private List<string> m_CaiLiaoList;

		private List<CaiLiaoJiaGe> m_CanBuyCaiLiaoList;

		private int m_CaiLiaoCount;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DiHuoGeCaiLiaoShaiXuanPanel>();
			m_CaiLiaoList = new List<string>();
			m_CanBuyCaiLiaoList = new List<CaiLiaoJiaGe>();
			m_CaiLiaoDic = new Dictionary<string, BaseNaJieGoodEntity>();
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			AddButtonClickListener(Panel.GouMaiBtn, GouMaiClick);
			Panel.CloseBtn.interactable = false;
			Panel.QueRenBtn.interactable = false;
			Panel.GouMaiBtn.interactable = false;
			m_caiLiaoPrefabItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeCaiLiaoItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_CaiLiaoCount = 0;
			if (paralist.Length >= 1)
			{
				m_DataSource = paralist[0] as DanFangModel;
			}
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetAllGoods(() =>
			{
				CaiLiaoPanDuan();
			});
		}

		private void InitData()
		{
			if (m_DataSource == null)
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				return;
			}
			Panel.TitleName.text = m_DataSource.na;
			MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(CreateDataAsny());
		}

		private IEnumerator CreateDataAsny()
		{
			if (Panel != null && m_DataSource.bb != null)
			{
				foreach (DanFangStepModel item in m_DataSource.bb)
				{
					if (item.actionType != 2)
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
							if (CaiLiaoList[i].a == item.targetId)
							{
								panDuan = 1;
								m_CanBuyCaiLiaoList.Add(CaiLiaoList[i]);
								break;
							}
						}
						component.SetValue(item, m_CaiLiaoDic, CaiLiaoXuanZe, m_CaiLiaoList, () =>
						{
							MapBtnClick(item.mapId, item.mapName);
						}, panDuan);
					}
					else
					{
						component.SetValue(item, m_CaiLiaoDic, CaiLiaoXuanZe, m_CaiLiaoList, () =>
						{
							MapBtnClick(item.mapId, item.mapName);
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

		private void CaiLiaoPanDuan()
		{
			SearchLMStockReq_1004028 data = new SearchLMStockReq_1004028
			{
				TemplateId = m_DataSource.ei,
				Type = 2
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCH_LMSTOCK, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
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
			Singleton<NetManager>.Instance.SendMsg(MsgType.MAP_PARENT_NAME, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
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
			Singleton<NetManager>.Instance.SendMsg(MsgType.PARENT_MAP_AND_ONESELF_MAP, data2, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ParentMapAndOneselfMapResp_1007012 parentMapAndOneselfMapResp_ = new ParentMapAndOneselfMapResp_1007012();
					parentMapAndOneselfMapResp_.MergeFrom(byteString);
					if (parentMapAndOneselfMapResp_.IsSuccess)
					{
						List<MapEntity> list = new List<MapEntity>();
						foreach (string item3 in parentMapAndOneselfMapResp_.Name)
						{
							list.Add(new MapEntity
							{
								MapName = item3
							});
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

		private void CaiLiaoXuanZe(DanFangStepModel stepModel, List<BaseNaJieGoodEntity> dataSource, UnityAction<int> ChoosedCompelete, int index)
		{
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
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoJiaGeWnd.prefab", true, m_DataSource, list, 0);
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenClick()
		{
			CheckLianDanConditions();
		}

		private void CheckLianDanConditions()
		{
			VerifyIsCanRefiningElixirReq_1005026 data = new VerifyIsCanRefiningElixirReq_1005026
			{
				MethodElixirId = m_DataSource.ei
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFY_IS_CAN_REFINING_ELIXIR, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					VerifyIsCanRefiningElixirResp_1005026 verifyIsCanRefiningElixirResp_ = new VerifyIsCanRefiningElixirResp_1005026();
					verifyIsCanRefiningElixirResp_.MergeFrom(byteString);
					if (verifyIsCanRefiningElixirResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianZhiWnd.prefab", true, m_DataSource, m_CaiLiaoDic, DiHuoGeEnterType.DongFu);
						CloseWnd();
						Singleton<UIManager>.Instance.CloseWnd("DiHuoGeWnd.prefab");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyIsCanRefiningElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
