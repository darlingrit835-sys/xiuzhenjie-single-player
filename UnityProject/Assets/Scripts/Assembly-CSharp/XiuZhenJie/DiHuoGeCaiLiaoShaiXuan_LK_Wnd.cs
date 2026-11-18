using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoShaiXuan_LK_Wnd : Window
	{
		public DiHuoGeCaiLiaoShaiXuan_LK_Panel Panel;

		private DanFangModel m_DataSource;

		private GameObject m_caiLiaoPrefabItem;

		private List<CaiLiaoJiaGe> CaiLiaoList;

		private Dictionary<string, BaseNaJieGoodEntity> m_CaiLiaoDic;

		private List<string> m_CaiLiaoList;

		private List<CaiLiaoJiaGe> m_CanBuyCaiLiaoList;

		private int m_CaiLiaoCount;

		private BaseNaJieGoodEntity m_LingKuiGoods;

		private bool m_IsHaveLingKui;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DiHuoGeCaiLiaoShaiXuan_LK_Panel>();
			m_CaiLiaoList = new List<string>();
			m_CanBuyCaiLiaoList = new List<CaiLiaoJiaGe>();
			m_CaiLiaoDic = new Dictionary<string, BaseNaJieGoodEntity>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.QueRenBtn, QueRenClick);
			AddButtonClickListener(Panel.GouMaiBtn, GouMaiClick);
			m_caiLiaoPrefabItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeCaiLiaoItem.prefab");
			Panel.CloseBtn.interactable = false;
			Panel.QueRenBtn.interactable = false;
			Panel.GouMaiBtn.interactable = false;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_CaiLiaoCount = 0;
			if (paralist.Length >= 1)
			{
				m_DataSource = paralist[0] as DanFangModel;
				if (m_DataSource != null)
				{
					string levelByRankName = WorldSetting.GetLevelByRankName(m_DataSource.r);
					WorldSetting.GetLevelByRank(m_DataSource.r);
					Panel.ContentTitleText.text = "灵傀炼制需消耗与丹方同品阶的灵傀，\n当前选择丹方品阶为：<color=red>" + levelByRankName + "</color> ";
				}
				if (m_DataSource != null)
				{
					Singleton<FacadeManager>.Instance.SelectGoodsRankBg().TryGetValue(WorldSetting.GetLevelByRank(m_DataSource.r), out var value);
					Singleton<FacadeManager>.Instance.SelectGoodsSprite().TryGetValue(36, out var value2);
					List<BaseNaJieGoodEntity> list = Singleton<RoleManager>.Instance.RoleBoxDic[10].SelectGoods(36, m_DataSource.r);
					if (list == null || list.Count == 0)
					{
						m_IsHaveLingKui = false;
					}
					else
					{
						m_IsHaveLingKui = true;
						m_LingKuiGoods = list[0];
					}
					Panel.LingKuiGoodsItem.SetItem(null, "灵傀", 0, 1, value, value2, null);
				}
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
			if (Panel == null)
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
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoChooseWnd.prefab", true, Panel.TitleName.text, stepModel.targetName, dataSource, (UnityAction<int, BaseNaJieGoodEntity>)((int num, BaseNaJieGoodEntity goods) =>
			{
				if (m_CaiLiaoList.Contains(goods.GoodsId) && m_CaiLiaoList[index] != goods.GoodsId)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("更换材料失败,该材料已经被占用!");
				}
				else
				{
					ChoosedCompelete(num);
					m_CaiLiaoList[index] = goods.GoodsId;
					if (m_CaiLiaoDic.Count > num + 1)
					{
						m_CaiLiaoDic[stepModel.id] = goods;
					}
				}
			}), index);
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
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoJiaGeWnd.prefab", true, m_DataSource, list, 1);
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
						StartLianZhi();
						CloseWindow();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyIsCanRefiningElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void StartLianZhi()
		{
			PuppetRefiningElixirReq_1005045 data = new PuppetRefiningElixirReq_1005045
			{
				MethodElixirId = m_DataSource.ei
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PUPPET_REFINING_ELIXIR, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PuppetRefiningElixirResp_1005045 puppetRefiningElixirResp_ = new PuppetRefiningElixirResp_1005045();
					puppetRefiningElixirResp_.MergeFrom(byteString);
					if (puppetRefiningElixirResp_.IsSuccess)
					{
						if (m_LingKuiGoods != null)
						{
							m_LingKuiGoods.GoodsNum = puppetRefiningElixirResp_.PuppetCount;
						}
						List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
						for (int i = 0; i < puppetRefiningElixirResp_.Count; i++)
						{
							list.Add(new BaseNaJieGoodEntity
							{
								Rank = puppetRefiningElixirResp_.Rank,
								Name = m_DataSource.na
							});
						}
						Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianDanJieSuanWnd.prefab", true, new LianZhiParameter
						{
							elixirId = m_DataSource.ei,
							isSuccess = true,
							list = new List<ClientPracticeArticleElixirStepData>()
						}, list, m_CaiLiaoDic, LianZhiJieGuoType.LianDanSucess);
						CloseWindow();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(puppetRefiningElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
