using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuWuXiangWnd : Window
	{
		public ChuWuXiangPanel Panel;

		private XzjBoxEntity m_DataSource;

		private XzjBoxEntity m_NaJieInitInfo;

		private List<string> NaJieInitWuPinData;

		public Dictionary<int, Sprite> m_GoodsIconSprite;

		public Dictionary<int, Sprite> m_RankBgSprite;

		private bool collecting;

		private GameObject ItemObj;

		private GameObject ItemTogObj;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private List<ShaiXuanItemType> UseShaiXuanType;

		private bool m_NaJieIsCompelete;

		private bool m_ChuWuXiangIsCompelete;

		private bool NaJieIsOpen;

		private string m_CangKuId;

		private bool m_IsOneKeyMode;

		private List<GameObject> m_ChooseEntityList;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			NaJieIsOpen = false;
			m_IsOneKeyMode = false;
			Panel = base.GameObject.GetComponent<ChuWuXiangPanel>();
			m_ChooseEntityList = new List<GameObject>();
			AddButtonClickListener(Panel.CloseBtn, CloseBtn);
			AddButtonClickListener(Panel.OpenNaJieBtn, OpenNaJie);
			AddButtonClickListener(Panel.NaJieMaskBtn, CloseNaJie);
			AddButtonClickListener(Panel.WanChengBtn, CloseNaJie);
			AddButtonClickListener(Panel.ShaiXuanBtn, ShaiXuanPop);
			AddButtonClickListener(Panel.ChooseYiJianLingQuBtn, OpenYiJianLingQu);
			AddButtonClickListener(Panel.YiJianLingQuBtn, YiJianLingQu);
			ResourceLoad();
		}

		private void ShaiXuanPop()
		{
			m_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void InitShaiXuan()
		{
			UseShaiXuanType = new List<ShaiXuanItemType>();
			UseShaiXuanType.Add(ShaiXuanItemType.AllGoodsType);
			UseShaiXuanType.Add(ShaiXuanItemType.WuXing);
			UseShaiXuanType.Add(ShaiXuanItemType.PinJie);
			UseShaiXuanType.Add(ShaiXuanItemType.DengJie);
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpNamedWnd("StorageGoodsFilter", "Common_ShaiXuanWnd.prefab", false, true, UseShaiXuanType, true) as Common_ShaiXuanWnd;
			if (m_ShaiXuanWnd != null)
				m_ShaiXuanWnd.ReChooseAction += RelfashData;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (m_ShaiXuanWnd?.GameObject == null) InitShaiXuan();
			m_NaJieInitInfo = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			if (paralist.Length != 0)
			{
				m_CangKuId = paralist[0].ToString();
				int result = 1;
				if (int.TryParse(m_CangKuId, out result))
				{
					m_DataSource = Singleton<RoleManager>.Instance.RoleBoxDic[result];
				}
				else
				{
					m_DataSource = Singleton<RoleManager>.Instance.RoleBoxDic[1];
				}
			}
			NaJieInitWuPinData = new List<string>();
			m_DataSource.GetAllGoods(() =>
			{
				m_ChuWuXiangIsCompelete = true;
			});
			m_NaJieInitInfo.GetAllGoods(() =>
			{
				m_NaJieIsCompelete = true;
			});
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			if (m_NaJieIsCompelete && m_ChuWuXiangIsCompelete)
			{
				m_NaJieIsCompelete = false;
				m_ChuWuXiangIsCompelete = false;
				SetRongLiangProgress();
				ChuWuXiangDataInit();
			}
		}

		private void RelfashData()
		{
			SetRongLiangProgress();
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			if (dictionary != null)
			{
				CleraData(Panel.NaJieListConstant);
				CleraData(Panel.ChuWuXiangListConstant);
				List<BaseNaJieGoodEntity> data = ShaiXuanGoods(dictionary, m_DataSource.GoodsList);
				List<BaseNaJieGoodEntity> data2 = ShaiXuanGoods(dictionary, m_NaJieInitInfo.GoodsList);
				if (!m_IsOneKeyMode)
				{
					ClickToggle(data, Panel.ChuWuXiangListConstant, 2);
				}
				else
				{
					ClickToggleCreateTog(data);
				}
				if (NaJieIsOpen)
				{
					ClickToggle(data2, Panel.NaJieListConstant, 1);
				}
			}
		}

		private List<BaseNaJieGoodEntity> ShaiXuanGoods(Dictionary<string, ShaiXuanParamsModel> result, List<BaseNaJieGoodEntity> data)
		{
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>(data);
			ShaiXuanParamsModel leiBie = null;
			if (result.TryGetValue("类别", out leiBie))
			{
				if (leiBie.ItemTag != null)
				{
					list = list.Where((BaseNaJieGoodEntity x) => x.Type == int.Parse(leiBie.ItemTag)).ToList();
				}
				else if (leiBie.ItemName == "法器")
				{
					list = list.Where((BaseNaJieGoodEntity x) => x.Type == 14 || x.Type == 15 || x.Type == 16 || x.Type == 26).ToList();
				}
				else if (leiBie.ItemName == "动物")
				{
					list = list.Where((BaseNaJieGoodEntity x) => x.Type == 7 || x.Type == 8 || x.Type == 9 || x.Type == 10 || x.Type == 11).ToList();
				}
			}
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			return list;
		}

		private void ResourceLoad()
		{
			ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			ItemTogObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void SetRongLiangProgress()
		{
			Panel.RongLiangValueProgress.fillAmount = (float)m_DataSource.NowCapacity * 1f / (float)m_DataSource.MaxCapacity;
			Panel.RongLiangText.text = string.Format("{0}/{1}", m_DataSource.NowCapacity.ToString("0"), m_DataSource.MaxCapacity.ToString("0"));
		}

		private void ChuWuXiangDataInit()
		{
			if (!m_IsOneKeyMode)
			{
				ClickToggle(m_DataSource.GoodsList, Panel.ChuWuXiangListConstant, 2);
			}
			else
			{
				ClickToggleCreateTog(m_DataSource.GoodsList);
			}
		}

		private void YiJianLingQu()
		{
			if (collecting) return;
			m_ChooseEntityList.RemoveAll(row => row == null);
			if (m_ChooseEntityList.Count == 0)
			{
				SwitchBottomBtns(ShowOpenNaJieBtn: true);
				return;
			}
			var row = m_ChooseEntityList[0];
			var item = row.GetComponent<UI_GoodsTogItem>();
			SetManyGoodsToNaJie((BaseNaJieGoodEntity)item.DataSource, row);
		}

		private void OpenYiJianLingQu()
		{
			m_IsOneKeyMode = !m_IsOneKeyMode;
			if (m_IsOneKeyMode)
			{
				Panel.ChooseYiJianLingQuBtn.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
				Panel.OpenNaJieBtn.interactable = false;
			}
			else
			{
				m_ChooseEntityList.Clear();
				Panel.OpenNaJieBtn.interactable = true;
				Panel.ChooseYiJianLingQuBtn.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			}
			ChuWuXiangDataInit();
		}

		private void OpenNaJie()
		{
			NaJieIsOpen = true;
			NaJieDataInit();
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => Panel.NaJiePanel.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJiePanel.anchoredPosition = v;
			}, new Vector2(0f, -1.5f), 0.5f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				Panel.NaJieMaskBtn.gameObject.SetActive(value: true);
			}));
		}

		private void CloseNaJie()
		{
			CleraData(Panel.NaJieListConstant);
			NaJieIsOpen = false;
			DOTween.To(() => Panel.NaJiePanel.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJiePanel.anchoredPosition = v;
			}, new Vector2(0f, -522.22f), 0.5f);
			Panel.NaJieMaskBtn.gameObject.SetActive(value: false);
		}

		private void NaJieDataInit()
		{
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			if (dictionary != null)
			{
				List<BaseNaJieGoodEntity> data = ShaiXuanGoods(dictionary, m_NaJieInitInfo.GoodsList);
				ClickToggle(data, Panel.NaJieListConstant, 1);
			}
		}

		private void CloseBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void GrticalListContentHight()
		{
			SetGrticalListContentHight(m_DataSource.GoodsList.Count, Panel.ChuWuXiangListConstant, Panel.ChuWuXiangListConstant.GetComponent<GridLayoutGroup>());
			SetGrticalListContentHight(m_NaJieInitInfo.GoodsList.Count, Panel.NaJieListConstant, Panel.NaJieListConstant.gameObject.GetComponent<GridLayoutGroup>());
		}

		private void ClickToggle(List<BaseNaJieGoodEntity> data, RectTransform rectTransform, int type)
		{
			if (data != null)
			{
				CleraData(rectTransform);
				CreateData(data, type);
				GrticalListContentHight();
			}
		}

		private void ClickToggleCreateTog(List<BaseNaJieGoodEntity> data)
		{
			if (data != null)
			{
				m_ChooseEntityList.Clear();
				SwitchBottomBtns(ShowOpenNaJieBtn: true);
				CleraData(Panel.ChuWuXiangListConstant);
				CreateTogData(data);
				GrticalListContentHight();
			}
		}

		private void CreateTogData(List<BaseNaJieGoodEntity> data)
		{
			data.Sort((BaseNaJieGoodEntity a, BaseNaJieGoodEntity b) => a.FiveElement.CompareTo(b.FiveElement) * 2 + a.Level.CompareTo(b.Level));
			for (int num = 0; num < data.Count && data[num] != null; num++)
			{
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(data[num].Type, out value);
				int levelByRank = WorldSetting.GetLevelByRank(data[num].Rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				UI_GoodsTogItem component = SetListContentParent(Panel.ChuWuXiangListConstant, ItemTogObj).GetComponent<UI_GoodsTogItem>();
				if (component != null)
				{
					component.SetValue(data[num], data[num].Name, data[num].FiveElement, data[num].Level, value2, value, null, (bool _isOn, object _obj, GameObject _gameObj) =>
					{
						ChooseTogItem(_obj, _isOn, _gameObj);
					}, num: data[num].GoodsNum);
				}
			}
		}

		private void ChooseTogItem(object obj, bool isOn, GameObject gameObj)
		{
			if (isOn)
			{
				if (!m_ChooseEntityList.Contains(gameObj))
				{
					if (m_ChooseEntityList.Count == 0)
					{
						SwitchBottomBtns(ShowOpenNaJieBtn: false);
					}
					m_ChooseEntityList.Add(gameObj);
				}
			}
			else if (m_ChooseEntityList.Contains(gameObj))
			{
				m_ChooseEntityList.Remove(gameObj);
				if (m_ChooseEntityList.Count == 0)
				{
					SwitchBottomBtns(ShowOpenNaJieBtn: true);
				}
			}
		}

		private void SwitchBottomBtns(bool ShowOpenNaJieBtn)
		{
			Panel.ChooseYiJianLingQuBtn.gameObject.SetActive(ShowOpenNaJieBtn);
			Panel.YiJianLingQuBtn.gameObject.SetActive(!ShowOpenNaJieBtn);
			Panel.OpenNaJieBtn.gameObject.SetActive(ShowOpenNaJieBtn);
		}

		private void CreateData(List<BaseNaJieGoodEntity> data, int type)
		{
			data.Sort((BaseNaJieGoodEntity a, BaseNaJieGoodEntity b) => a.FiveElement.CompareTo(b.FiveElement) * 2 + a.Level.CompareTo(b.Level));
			for (int num = 0; num < data.Count; num++)
			{
				SetItem(data[num], type);
			}
		}

		private void SetItem(BaseNaJieGoodEntity wupinList, int type)
		{
			if (!(ItemObj != null))
			{
				return;
			}
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(wupinList.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(wupinList.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			if (type == 1)
			{
				UI_GoodsItem component = SetListContentParent(Panel.NaJieListConstant, ItemObj).GetComponent<UI_GoodsItem>();
				if (component != null)
				{
					PutGoods(component, wupinList, value2, value);
				}
				return;
			}
			UI_GoodsItem component2 = SetListContentParent(Panel.ChuWuXiangListConstant, ItemObj).GetComponent<UI_GoodsItem>();
			if (component2 != null)
			{
				GetGoods(component2, wupinList, value2, value);
			}
		}

		private void GetGoods(UI_GoodsItem diaoLuoItem, BaseNaJieGoodEntity wupinList, Sprite rankImg, Sprite goodsIcon)
		{
			diaoLuoItem.SetItem(wupinList, wupinList.Name, wupinList.FiveElement, wupinList.Level, rankImg, goodsIcon, (object _obj, GameObject _gameObj) =>
			{
				BaseNaJieGoodEntity baseNaJieGoodEntity = _obj as BaseNaJieGoodEntity;
				m_DataSource.GetGoodsInfomation(baseNaJieGoodEntity, (List<string> _list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						BtnName = "取出",
						ContentList = _list,
						DataSource = baseNaJieGoodEntity,
						SureBtnClick = (object _data) =>
						{
							if (baseNaJieGoodEntity.GoodsNum > 1)
							{
								Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
								{
									Goods = baseNaJieGoodEntity,
									QueRenCallBack = (int num) =>
									{
										SetGoodsToNaJie(baseNaJieGoodEntity, diaoLuoItem.gameObject, num);
									},
									TitleContnet = "取出"
								};
								Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
							}
							else
							{
								SetGoodsToNaJie(baseNaJieGoodEntity, diaoLuoItem.gameObject, 1);
							}
						},
						TitleName = baseNaJieGoodEntity.Name
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}, isUsing: false, wupinList.GoodsNum);
		}

		private void PutGoods(UI_GoodsItem diaoLuoItem, BaseNaJieGoodEntity wupinList, Sprite rankImg, Sprite goodsIcon)
		{
			diaoLuoItem.SetItem(wupinList, wupinList.Name, wupinList.FiveElement, wupinList.Level, rankImg, goodsIcon, (object _obj, GameObject _gameObj) =>
			{
				BaseNaJieGoodEntity baseNaJieGoodEntity = _obj as BaseNaJieGoodEntity;
				if (baseNaJieGoodEntity.GoodsNum <= 1)
				{
					SetGoodsToChuWuXiang(baseNaJieGoodEntity, diaoLuoItem.gameObject, 1);
				}
				else
				{
					Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
					{
						Goods = baseNaJieGoodEntity,
						QueRenCallBack = (int num) =>
						{
							SetGoodsToChuWuXiang(baseNaJieGoodEntity, diaoLuoItem.gameObject, num);
						},
						TitleContnet = "存入"
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
				}
			}, wupinList.IsUsing, wupinList.GoodsNum);
		}

		private void SetGoodsToChuWuXiang(BaseNaJieGoodEntity goods, GameObject obj, int num)
		{
			if (goods.IsUsing)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("该物品正在使用中,请先解除使用");
				return;
			}
			PutInSpaceBoxReq_1005022 data = new PutInSpaceBoxReq_1005022
			{
				GoodsId = goods.GoodsId,
				Location = int.Parse(m_CangKuId),
				Count = num
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PUT_IN_SPACEBOX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PutInSpaceBoxResp_1005022 putInSpaceBoxResp_ = new PutInSpaceBoxResp_1005022();
					putInSpaceBoxResp_.MergeFrom(byteString);
					if (putInSpaceBoxResp_.IsSuccess)
					{
						BaseNaJieGoodEntity baseNaJieGoodEntity = goods.Copy();
						m_NaJieInitInfo.RemoveGoods(goods.GoodsId, num);
						baseNaJieGoodEntity.GoodsId = putInSpaceBoxResp_.GoodsId;
						baseNaJieGoodEntity.GoodsNum = num;
						m_DataSource.ApplyStoredMove(baseNaJieGoodEntity);
						m_DataSource.NowCapacity = putInSpaceBoxResp_.NowBoxCapacity;
						m_NaJieInitInfo.NowCapacity = putInSpaceBoxResp_.NowRingCapacity;
						SetRongLiangProgress();
						ChuWuXiangDataInit();
						NaJieDataInit();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(putInSpaceBoxResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetGoodsToNaJie(BaseNaJieGoodEntity goods, GameObject obj, int num)
		{
			GetOutSpaceBoxReq_1005023 data = new GetOutSpaceBoxReq_1005023
			{
				GoodsId = goods.GoodsId,
				Location = int.Parse(m_CangKuId),
				Count = num
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GETOUT_SPACEBOX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GetOutSpaceBoxResp_1005023 getOutSpaceBoxResp_ = new GetOutSpaceBoxResp_1005023();
					getOutSpaceBoxResp_.MergeFrom(byteString);
					if (getOutSpaceBoxResp_.IsSuccess)
					{
						BaseNaJieGoodEntity baseNaJieGoodEntity = goods.Copy();
						if (goods.GoodsNum <= num)
						{
							if (obj != null)
							{
								UnityEngine.Object.Destroy(obj);
							}
						}
						else
						{
							obj.GetComponent<UI_GoodsItem>().ReduceNum(num);
						}
						if (goods != null)
						{
							m_DataSource.RemoveGoods(goods.GoodsId, num);
							baseNaJieGoodEntity.GoodsId = getOutSpaceBoxResp_.GoodsId;
							baseNaJieGoodEntity.GoodsNum = num;
							m_NaJieInitInfo.ApplyStoredMove(baseNaJieGoodEntity);
						}
						m_DataSource.NowCapacity = getOutSpaceBoxResp_.NowBoxCapacity;
						m_NaJieInitInfo.NowCapacity = getOutSpaceBoxResp_.NowRingCapacity;
						GrticalListContentHight();
						SetRongLiangProgress();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(getOutSpaceBoxResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetManyGoodsToNaJie(BaseNaJieGoodEntity goods, GameObject row)
		{
			var request = new GetOutSpaceBoxReq_1005023
			{
				GoodsId = goods.GoodsId,
				Location = int.Parse(m_CangKuId),
				Count = goods.GoodsNum
			};
			var game = MonoSingleton<GameStart>.Instance;
			collecting = true;
			if (Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.GETOUT_SPACEBOX, request, bytes =>
			{
				if (!collecting) return;
				collecting = false;
				game.HideWaitReauestWnd();
				if (bytes == null)
				{
					CloseBtn();
					game.PopUpAttention("领取回执未到，请重新打开储物箱核对数量。");
					return;
				}
				var reply = GetOutSpaceBoxResp_1005023.Parser.ParseFrom(bytes);
				if (!reply.IsSuccess)
				{
					game.PopUpAttention(game.GetHintMsgByCode(reply.Code));
					return;
				}
				var received = goods.Copy();
				received.GoodsId = reply.GoodsId;
				received.GoodsNum = request.Count;
				m_DataSource.RemoveGoods(request.GoodsId, request.Count);
				m_NaJieInitInfo.ApplyStoredMove(received);
				m_DataSource.NowCapacity = reply.NowBoxCapacity;
				m_NaJieInitInfo.NowCapacity = reply.NowRingCapacity;
				m_ChooseEntityList.Remove(row);
				if (row != null) UnityEngine.Object.Destroy(row);
				SetRongLiangProgress();
				GrticalListContentHight();
				YiJianLingQu();
			}))
			{
				game.ShowWaitReauestWnd();
				return;
			}
			collecting = false;
			game.PopUpAttention("未连接游戏服，请重连后领取。");
		}

		private void CleraData(RectTransform rectTransform)
		{
			for (int i = 0; i < rectTransform.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(rectTransform.transform.GetChild(i).gameObject);
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
			if (collecting) MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
			collecting = false;
			m_ChooseEntityList.Clear();
			if (m_ShaiXuanWnd != null)
			{
				m_ShaiXuanWnd.ReChooseAction -= RelfashData;
				if (m_ShaiXuanWnd.GameObject != null)
					Singleton<UIManager>.Instance.CloseWnd(m_ShaiXuanWnd);
				m_ShaiXuanWnd = null;
			}
			CleraData(Panel.NaJieListConstant);
			CleraData(Panel.ChuWuXiangListConstant);
		}
	}
}
