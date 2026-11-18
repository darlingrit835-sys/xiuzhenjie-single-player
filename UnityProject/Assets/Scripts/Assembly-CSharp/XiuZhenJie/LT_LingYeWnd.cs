using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LT_LingYeWnd : Window
	{
		private LT_LingYePanel m_Panel;

		private GameObject m_NaJieObj;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private List<LingYeGoodList> m_XiRangGoodList;

		private Dictionary<string, GameObject> m_GoodsDic;

		private List<LingTianEntily> m_LingTianEntily;

		private int m_ChuangTi;

		private BaseNaJieGoodEntity m_ChuangTia;

		private BaseNaJieGoodEntity m_ChuangTib;

		private BaseNaJieGoodEntity m_ChuangTic;

		private LingYeGoodList m_LingYed;

		private int m_CountShu;

		private Dictionary<string, BaseNaJieGoodEntity> m_CurrentlyAvailableMaterials;

		private Dictionary<string, int> m_TheMaterialIsCurrentlySelected;

		private XzjBoxEntity m_NaJieBox;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			ResourceLoad();
			m_Panel = base.GameObject.GetComponent<LT_LingYePanel>();
			AddButtonClickListener(m_Panel.TuiChu, TuiChu);
			AddButtonClickListener(m_Panel.WenHao, XiaoWenHao);
			AddButtonClickListener(m_Panel.XuanZhe_1, XuanZhe_1);
			AddButtonClickListener(m_Panel.XuanZhe_2, XuanZhe_2);
			AddButtonClickListener(m_Panel.XuanZhe_3, XuanZhe_3);
			AddButtonClickListener(m_Panel.XuanZhe_4, XuanZhe_4);
			AddButtonClickListener(m_Panel.TiaoPei, TiaoPei);
			Initialization();
			ObtainLingYe();
			ChuangTiShuXing();
		}

		private void JiaZaiShuJu()
		{
			m_CurrentlyAvailableMaterials = new Dictionary<string, BaseNaJieGoodEntity>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Type == 9)
				{
					m_CurrentlyAvailableMaterials.Add(goods.GoodsId, goods);
					m_CountShu += goods.GoodsNum;
				}
			}
		}

		private void Initialization()
		{
			m_TheMaterialIsCurrentlySelected = new Dictionary<string, int>();
			m_GoodsDic = new Dictionary<string, GameObject>();
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(JiaZaiShuJu);
		}

		private void ChuangTiShuXing()
		{
			m_ChuangTi = 0;
			m_ChuangTia = new BaseNaJieGoodEntity();
			m_ChuangTib = new BaseNaJieGoodEntity();
			m_ChuangTic = new BaseNaJieGoodEntity();
			m_LingYed = new LingYeGoodList();
			m_Panel.TiaoPei.interactable = false;
		}

		private void CaiLiaoShuXing()
		{
			m_Panel.XuanZhe_1_1.gameObject.SetActive(value: false);
			m_Panel.XuanZhe_1_2.gameObject.SetActive(value: false);
			m_Panel.XuanZhe_1_3.gameObject.SetActive(value: false);
			m_Panel.XuanZhe_1_4.gameObject.SetActive(value: false);
		}

		private void DiBuShuXing()
		{
			m_Panel.Lingua.gameObject.SetActive(value: false);
			m_Panel.Lingub.gameObject.SetActive(value: false);
			m_Panel.Linguc.gameObject.SetActive(value: false);
			m_Panel.Lingud.gameObject.SetActive(value: false);
		}

		private void InitNaJie()
		{
			HouQuCaiLiao();
		}

		private void HouQuCaiLiao()
		{
			if (m_CountShu > 2)
			{
				m_Panel.CaiLiaoBG.gameObject.SetActive(value: false);
				m_Panel.CaiLiaoBG.gameObject.SetActive(value: true);
				m_Panel.LingYeBG.gameObject.SetActive(value: false);
				CreatNaJieGoods();
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前纳戒精血总数不足三个，无法创造灵液");
			}
		}

		private void CreatNaJieGoods()
		{
			CaiLiaoClearData();
			GridLayoutGroup component = m_Panel.CaiLiao.GetComponent<GridLayoutGroup>();
			GameObject naJieKongJianList = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			component.enabled = true;
			SetNaJieKongJianList(naJieKongJianList);
			SetGrticalListContentHight(m_CurrentlyAvailableMaterials.Count, m_Panel.CaiLiao, component);
		}

		private void CaiLiaoClearData()
		{
			if (m_Panel == null || m_Panel.CaiLiao == null)
			{
				return;
			}
			foreach (RectTransform item in m_Panel.CaiLiao.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void SetNaJieKongJianList(GameObject obj)
		{
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> currentlyAvailableMaterial in m_CurrentlyAvailableMaterials)
			{
				CreateObjItem(obj, currentlyAvailableMaterial.Value);
			}
		}

		private void CreateObjItem(GameObject obj, BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(m_Panel.CaiLiao, obj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, (object objs, GameObject gameObj) =>
			{
				GoodsClick(data);
			}, isUsing: false, data.GoodsNum);
			if (!m_GoodsDic.ContainsKey(data.GoodsId))
			{
				m_GoodsDic.Add(data.GoodsId, gameObject);
			}
		}

		private void GoodsClick(BaseNaJieGoodEntity data)
		{
			List<string> list = new List<string>();
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(data, (List<string> strList) =>
			{
				if (strList != null)
				{
					foreach (string str in strList)
					{
						list.Add(str);
					}
				}
				PopUpXiangQingPanela(data.Name, "确认", UnityAction, list);
			});
			void UnityAction(object obj)
			{
				FuZhis(data);
			}
		}

		private void PopUpXiangQingPanela(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void FuZhis(BaseNaJieGoodEntity data)
		{
			switch (m_ChuangTi)
			{
			case 1:
				QuantityJudgment(m_ChuangTia, data);
				ChuangTiAs(data);
				break;
			case 2:
				QuantityJudgment(m_ChuangTib, data);
				ChuangTiBs(data);
				break;
			case 3:
				QuantityJudgment(m_ChuangTic, data);
				ChuangTiCs(data);
				break;
			}
			PanDuanTiaoPei();
		}

		private void QuantityJudgment(BaseNaJieGoodEntity chuangTis, BaseNaJieGoodEntity data)
		{
			if (chuangTis.GoodsId != null)
			{
				m_TheMaterialIsCurrentlySelected.Remove(chuangTis.GoodsId);
				BeiBaoTianJia(chuangTis);
			}
			if (m_TheMaterialIsCurrentlySelected.ContainsKey(data.GoodsId))
			{
				m_TheMaterialIsCurrentlySelected[data.GoodsId]++;
			}
			else
			{
				m_TheMaterialIsCurrentlySelected.Add(data.GoodsId, 1);
			}
			BeiBaoJianShao(data);
		}

		private void BeiBaoTianJia(BaseNaJieGoodEntity data)
		{
			if (m_CurrentlyAvailableMaterials.ContainsKey(data.GoodsId))
			{
				m_CurrentlyAvailableMaterials[data.GoodsId].GoodsNum++;
			}
			else
			{
				m_CurrentlyAvailableMaterials.Add(data.GoodsId, data);
			}
		}

		private void BeiBaoJianShao(BaseNaJieGoodEntity data)
		{
			if (m_CurrentlyAvailableMaterials.ContainsKey(data.GoodsId))
			{
				if (m_CurrentlyAvailableMaterials[data.GoodsId].GoodsNum > 1)
				{
					m_CurrentlyAvailableMaterials[data.GoodsId].GoodsNum--;
				}
				else
				{
					m_CurrentlyAvailableMaterials.Remove(data.GoodsId);
				}
			}
		}

		private void ChuangTiAs(BaseNaJieGoodEntity data)
		{
			m_ChuangTia = data;
			m_Panel.XuanZhe_1_1.gameObject.SetActive(value: true);
			UI_GoodsItem component = m_Panel.XuanZhe_1_1.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, (object objs, GameObject gameObj) =>
			{
				XuanZhe_1();
			});
		}

		private void ChuangTiBs(BaseNaJieGoodEntity data)
		{
			m_ChuangTib = data;
			m_Panel.XuanZhe_1_2.gameObject.SetActive(value: true);
			UI_GoodsItem component = m_Panel.XuanZhe_1_2.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, (object obj, GameObject gameObj) =>
			{
				XuanZhe_2();
			});
		}

		private void ChuangTiCs(BaseNaJieGoodEntity data)
		{
			m_ChuangTic = data;
			m_Panel.XuanZhe_1_3.gameObject.SetActive(value: true);
			UI_GoodsItem component = m_Panel.XuanZhe_1_3.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, (object obj, GameObject gameObj) =>
			{
				XuanZhe_3();
			});
		}

		private void PanDuanTiaoPei()
		{
			m_Panel.CaiLiaoBG.gameObject.SetActive(value: false);
			if (m_ChuangTia.Name != null && m_ChuangTib.Name != null && m_ChuangTic.Name != null && m_LingYed.Name != null)
			{
				m_Panel.TiaoPei.interactable = true;
			}
		}

		private void TiaoPei()
		{
			WaterDeployReq_1013017 data = new WaterDeployReq_1013017
			{
				GoodsId = { m_ChuangTia.GoodsId, m_ChuangTib.GoodsId, m_ChuangTic.GoodsId },
				WaterMaterialType = m_LingYed.Type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.WATER_DEPLOY_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					WaterDeployResp_1013017 waterDeployResp_ = new WaterDeployResp_1013017();
					waterDeployResp_.MergeFrom(byteString);
					if (waterDeployResp_.IsSuccess)
					{
						LingTianEntily canWuEntity = new LingTianEntily
						{
							Name = waterDeployResp_.Name,
							FiveElement = waterDeployResp_.FiveElement,
							GoodsId = waterDeployResp_.WaterId,
							Level = waterDeployResp_.Level,
							Rank = waterDeployResp_.Rank,
							ConversionRate = waterDeployResp_.ConversionRate,
							ReceiveVal = waterDeployResp_.ReceiveVal
						};
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = (int)waterDeployResp_.CurrentShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						m_TheMaterialIsCurrentlySelected = new Dictionary<string, int>();
						m_CountShu -= 3;
						CaiLiaoShuXing();
						JieGuoXianShi(canWuEntity);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(waterDeployResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void JieGuoXianShi(LingTianEntily canWuEntity)
		{
			m_Panel.JieGuo_1.gameObject.SetActive(value: true);
			UI_GoodsItem component = m_Panel.JieGuo_1.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(canWuEntity.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(canWuEntity, canWuEntity.Name, canWuEntity.FiveElement, canWuEntity.Level, value2, value, null);
			XiangQing(canWuEntity);
		}

		private void ObtainLingYe()
		{
			DiBuShuXing();
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchWaterAllResp, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchWaterAllResp_1013024 searchWaterAllResp_ = new SearchWaterAllResp_1013024();
					m_LingTianEntily = new List<LingTianEntily>();
					searchWaterAllResp_.MergeFrom(byteString);
					if (searchWaterAllResp_.IsSuccess)
					{
						if (searchWaterAllResp_.WaterAll.Count > 0)
						{
							m_Panel.WuLingYe.gameObject.SetActive(value: false);
							m_Panel.LingYe.gameObject.SetActive(value: true);
							foreach (SearchWaterAllResp_1013024.Types.WaterInfoResp_1013025 item2 in searchWaterAllResp_.WaterAll)
							{
								LingTianEntily item = new LingTianEntily
								{
									Name = item2.Name,
									FiveElement = item2.FiveElement,
									GoodsId = item2.Id,
									Level = item2.Level,
									Rank = item2.Rank,
									ConversionRate = item2.ConversionRate,
									ReceiveVal = item2.ReceiveVal
								};
								m_LingTianEntily.Add(item);
							}
							Buttons();
						}
						else
						{
							m_Panel.WuLingYe.gameObject.SetActive(value: true);
							m_Panel.LingYe.gameObject.SetActive(value: false);
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchWaterAllResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void Buttons()
		{
			for (int i = 0; i < m_LingTianEntily.Count; i++)
			{
				switch (i)
				{
				case 0:
					LingYeDianJieFuZhia(i);
					break;
				case 1:
					LingYeDianJieFuZhib(i);
					break;
				case 2:
					LingYeDianJieFuZhic(i);
					break;
				case 3:
					LingYeDianJieFuZhid(i);
					break;
				}
			}
		}

		private void LingYeDianJieFuZhia(int i)
		{
			AddButtonClickListener(m_Panel.Lingua, Lingua);
			m_Panel.Lingua.gameObject.SetActive(value: true);
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			m_Panel.UI_Goodsa.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value2, value, (object obj, GameObject gameObj) =>
			{
				FuZhi(0);
			});
		}

		private void LingYeDianJieFuZhib(int i)
		{
			AddButtonClickListener(m_Panel.Lingub, Lingub);
			m_Panel.Lingub.gameObject.SetActive(value: true);
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			m_Panel.UI_Goodsb.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value2, value, (object obj, GameObject gameObj) =>
			{
				FuZhi(1);
			});
		}

		private void LingYeDianJieFuZhic(int i)
		{
			AddButtonClickListener(m_Panel.Linguc, Linguc);
			m_Panel.Linguc.gameObject.SetActive(value: true);
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			m_Panel.UI_Goodsc.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value2, value, (object obj, GameObject gameObj) =>
			{
				FuZhi(2);
			});
		}

		private void LingYeDianJieFuZhid(int i)
		{
			AddButtonClickListener(m_Panel.Lingud, Lingud);
			m_Panel.Lingud.gameObject.SetActive(value: true);
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(m_LingTianEntily[i].Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			m_Panel.UI_Goodsd.SetItem(m_LingTianEntily[i], m_LingTianEntily[i].Name, m_LingTianEntily[i].FiveElement, m_LingTianEntily[i].Level, value2, value, (object obj, GameObject gameObj) =>
			{
				FuZhi(3);
			});
		}

		private void Lingua()
		{
			Forgotten(0);
		}

		private void Lingub()
		{
			Forgotten(1);
		}

		private void Linguc()
		{
			Forgotten(2);
		}

		private void Lingud()
		{
			Forgotten(3);
		}

		private void Forgotten(int forgottenShu)
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确定丢弃" + m_LingTianEntily[forgottenShu].Name + "?", () =>
			{
				DiscardWaterReq_1013026 data = new DiscardWaterReq_1013026
				{
					WaterId = m_LingTianEntily[forgottenShu].GoodsId
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.DISCARD_WATERREQ, data, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DiscardWaterResp_1013026 discardWaterResp_ = new DiscardWaterResp_1013026();
						discardWaterResp_.MergeFrom(byteString);
						if (discardWaterResp_.IsSuccess)
						{
							ObtainLingYe();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(discardWaterResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private void FuZhi(int a)
		{
			List<string> xiangQingData = new List<string>
			{
				"等阶," + WorldSetting.GetStageName(m_LingTianEntily[a].Level),
				"五行," + WorldSetting.GetWuXingName((WuXing)m_LingTianEntily[a].FiveElement),
				"品阶," + WorldSetting.GetRankAllName(m_LingTianEntily[a].Rank),
				$"灵衍值,{m_LingTianEntily[a].ConversionRate}",
				$"蕴灵值,{m_LingTianEntily[a].ReceiveVal}"
			};
			PopUpXiangQingPanel(m_LingTianEntily[a].Name, "确认", UnityAction, xiangQingData);
			void UnityAction(object obj)
			{
				JiaoGuan();
			}
		}

		private void JiaoGuan()
		{
			Singleton<UIManager>.Instance.CloseWnd("Common_TipWnd.prefab");
		}

		private void PopUpXiangQingPanel(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			if (m_NaJieObj == null)
			{
				m_NaJieObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			}
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/LingTian_LingYe.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ShouGu.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_PiMao.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_JingXue.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_KuangShi.png");
			m_GoodsIconSprite.Add(1, value);
			m_GoodsIconSprite.Add(10, value2);
			m_GoodsIconSprite.Add(8, value3);
			m_GoodsIconSprite.Add(9, value4);
			m_GoodsIconSprite.Add(13, value5);
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value6);
			m_RankBgSprite.Add(2, value7);
			m_RankBgSprite.Add(3, value8);
			m_RankBgSprite.Add(4, value9);
		}

		private void TuiChu()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void XiaoWenHao()
		{
			string text = "1、可以用于灵液调配的包括动物的精血。 2、蕴灵值越高、灵衍值越高，则灵液的效果越强，缩短的生长周期越长。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵液调配");
		}

		private void XuanZhe_1()
		{
			m_ChuangTi = 1;
			InitNaJie();
		}

		private void XuanZhe_2()
		{
			m_ChuangTi = 2;
			InitNaJie();
		}

		private void XuanZhe_3()
		{
			m_ChuangTi = 3;
			InitNaJie();
		}

		private void XuanZhe_4()
		{
			HouQuXiRang();
		}

		private void LingYeClearData()
		{
			if (m_Panel == null || m_Panel.LingYeXuanZhe == null)
			{
				return;
			}
			foreach (RectTransform item in m_Panel.LingYeXuanZhe.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void HouQuXiRang()
		{
			LingYeClearData();
			m_Panel.LingYeBG.gameObject.SetActive(value: true);
			m_Panel.CaiLiaoBG.gameObject.SetActive(value: false);
			Singleton<NetManager>.Instance.SendMsg(MsgType.SearchLingQuanResp, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchLingQuanResp_1013029 searchLingQuanResp_ = new SearchLingQuanResp_1013029();
					searchLingQuanResp_.MergeFrom(byteString);
					if (searchLingQuanResp_.IsSuccess)
					{
						m_XiRangGoodList = new List<LingYeGoodList>();
						foreach (SearchLingQuanResp_1013029.Types.LingQuanResp_1013030 item in searchLingQuanResp_.LingQuan)
						{
							m_XiRangGoodList.Add(new LingYeGoodList
							{
								Rank = item.Rank,
								Type = item.Type,
								Name = item.Name,
								ConversionRate = item.ConversionRate,
								Price = item.Price
							});
						}
						XiRangGoods(m_XiRangGoodList);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchLingQuanResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void XiRangGoods(List<LingYeGoodList> data)
		{
			foreach (LingYeGoodList datum in data)
			{
				XiRangObjItem(datum);
			}
		}

		private void XiRangObjItem(LingYeGoodList data)
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			GameObject gameObject = SetListContentParent(m_Panel.LingYeXuanZhe, insObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(data, data.Name, 0, 1, value2, value, (object objs, GameObject gameObj) =>
			{
				XirangClick(data);
			});
		}

		private void XirangClick(LingYeGoodList data)
		{
			List<string> xiangQingData = new List<string>
			{
				"品阶," + WorldSetting.GetRankAllName(data.Rank),
				$"灵衍值,{data.ConversionRate}",
				"价格," + data.Price + "灵石"
			};
			PopUpXiangQingPanelXiRang(data.Name, "购买", UnityAction, xiangQingData);
			void UnityAction(object obj)
			{
				GoMai(data);
			}
		}

		private void PopUpXiangQingPanelXiRang(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void GoMai(LingYeGoodList data)
		{
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(data.Price) >= 0.0)
			{
				m_Panel.XuanZhe_1_4.gameObject.SetActive(value: true);
				UI_GoodsItem component = m_Panel.XuanZhe_1_4.GetComponent<UI_GoodsItem>();
				m_GoodsIconSprite.TryGetValue(1, out var value);
				int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
				m_RankBgSprite.TryGetValue(levelByRank, out var value2);
				component.SetItem(data, data.Name, data.Type, 1, value2, value, (object obj, GameObject gameObj) =>
				{
					HouQuXiRang();
				});
				m_LingYed = data;
				m_Panel.LingYeBG.gameObject.SetActive(value: false);
				PanDuanTiaoPei();
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足，购买失败！");
			}
		}

		private void XiangQing(LingTianEntily canWuEntity)
		{
			m_Panel.XiangQingBG.gameObject.SetActive(value: true);
			m_Panel.BiaoTi.text = "恭喜！成功调配一份" + canWuEntity.Name;
			m_Panel.Name.text = canWuEntity.Name;
			m_GoodsIconSprite.TryGetValue(1, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(canWuEntity.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			m_Panel.JiGuXiangShi.SetItem(canWuEntity, canWuEntity.Name, 0, 1, value2, value, null);
			m_Panel.CaiLiaoXiaoa.text = "材料名: " + m_ChuangTia.Name;
			m_Panel.CaiLiaoXiaob.text = "材料名: " + m_ChuangTib.Name;
			m_Panel.CaiLiaoXiaoc.text = "材料名: " + m_ChuangTic.Name;
			m_Panel.CaiLiaoXiaod.text = "一瓶 : " + m_LingYed.Name;
			AddButtonClickListener(m_Panel.QiReng, XiangQingXian);
			AddButtonClickListener(m_Panel.GuanBi, XiangQingXian);
		}

		private void XiangQingXian()
		{
			m_Panel.XiangQingBG.gameObject.SetActive(value: false);
			ObtainLingYe();
			ChuangTiShuXing();
		}
	}
}
