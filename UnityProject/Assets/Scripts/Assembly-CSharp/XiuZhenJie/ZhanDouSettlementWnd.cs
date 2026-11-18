using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanDouSettlementWnd : Window
	{
		public ZhanDouSettlementPanel Panel;

		private List<BaseNaJieGoodEntity> m_DiaoLuo;

		private Dictionary<string, UI_GoodsItem> m_DiaoLuoObjDic;

		private XzjBoxEntity m_NaJieDataSource;

		public Dictionary<int, Sprite> m_GoodsIconSprite;

		public Dictionary<int, Sprite> m_RankBgSprite;

		private double m_CurrentRongLiang;

		private double m_MaxRongLiang;

		private bool NaJieIsOpen;

		private GameObject ItemObj;

		private string m_BattleId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZhanDouSettlementPanel>();
			AddButtonClickListener(Panel.YiJianLingQuBtn, OneKeyLingQu);
			AddButtonClickListener(Panel.DaKaiNaJieBtn, OpenNaJie);
			AddButtonClickListener(Panel.WanChengBtn, CloseNaJie);
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtn);
			m_DiaoLuo = paralist[0] as List<BaseNaJieGoodEntity>;
			m_BattleId = paralist[2] as string;
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			m_DiaoLuoObjDic = new Dictionary<string, UI_GoodsItem>();
			ResourceLoad();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_NaJieDataSource = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieDataSource.GetAllGoods(() =>
			{
				m_CurrentRongLiang = m_NaJieDataSource.NowCapacity;
				m_MaxRongLiang = m_NaJieDataSource.MaxCapacity;
				RelfashData();
			}, isShowWaiting: false);
		}

		private void RelfashData()
		{
			SetRongLiangProgress();
			InitDiaoLuo(m_DiaoLuo);
		}

		private void SetRongLiangProgress()
		{
			if (!(Panel == null) && !(Panel.RongLiangImg == null) && !(Panel.RongLiangText == null))
			{
				m_CurrentRongLiang = Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity;
				Panel.RongLiangImg.fillAmount = (float)m_CurrentRongLiang / (float)m_MaxRongLiang;
				Panel.RongLiangText.text = $"{m_CurrentRongLiang}/{m_MaxRongLiang}";
			}
		}

		private void CleraData(RectTransform rectTransform)
		{
			for (int i = 0; i < rectTransform.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(rectTransform.transform.GetChild(i).gameObject);
			}
		}

		private void ResourceLoad()
		{
			ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void InitNaJie()
		{
			foreach (Transform item in Panel.NaJieContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (m_NaJieDataSource != null && m_NaJieDataSource.GoodsList != null)
			{
				foreach (BaseNaJieGoodEntity goods in m_NaJieDataSource.GoodsList)
				{
					InstanceNaJieGoodsItem(goods);
				}
			}
			SetGrticalListContentHight(m_NaJieDataSource.GoodsList.Count, Panel.NaJieContent, Panel.NaJieContent.gameObject.GetComponent<GridLayoutGroup>());
		}

		private void InstanceNaJieGoodsItem(BaseNaJieGoodEntity item)
		{
			UI_GoodsItem component = SetListContentParent(Panel.NaJieContent, ItemObj).GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out value);
			Sprite value2 = null;
			m_GoodsIconSprite.TryGetValue(item.Type, out value2);
			component.SetItem(item, item.Name, item.FiveElement, item.Level, value, value2, NaJieGoodsClick, isUsing: false, item.GoodsNum);
			SetGrticalListContentHight(Panel.NaJieContent.childCount, Panel.NaJieContent, Panel.NaJieContent.gameObject.GetComponent<GridLayoutGroup>());
		}

		private void NaJieGoodsClick(object obj, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = obj as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity == null)
			{
				return;
			}
			if (baseNaJieGoodEntity.IsLock)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("无法遗弃已锁定物品!");
			}
			else if (baseNaJieGoodEntity.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = baseNaJieGoodEntity,
					QueRenCallBack = (int num) =>
					{
						MoveGoodsOut(baseNaJieGoodEntity, gameObj, num);
					},
					TitleContnet = "丢弃"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				MoveGoodsOut(baseNaJieGoodEntity, gameObj, 1);
			}
		}

		private void MoveGoodsOut(BaseNaJieGoodEntity baseNaJieGoodEntity, GameObject gameObj, int num)
		{
			BattleEndRingOutGoodsReq_1004036 data = new BattleEndRingOutGoodsReq_1004036
			{
				BattleId = m_BattleId,
				GoodsId = baseNaJieGoodEntity.GoodsId,
				Count = num
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BATTLEEND_RINGOUT_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BattleEndRingOutGoodsResp_1004036 battleEndRingOutGoodsResp_ = new BattleEndRingOutGoodsResp_1004036();
					battleEndRingOutGoodsResp_.MergeFrom(byteString);
					if (battleEndRingOutGoodsResp_.IsSuccess)
					{
						BaseNaJieGoodEntity baseNaJieGoodEntity2 = baseNaJieGoodEntity.Copy();
						if (baseNaJieGoodEntity2.GoodsNum <= num)
						{
							if (gameObj != null)
							{
								UnityEngine.Object.Destroy(gameObj);
							}
						}
						else if (gameObj != null)
						{
							gameObj.GetComponent<UI_GoodsItem>().ReduceNum(num);
						}
						m_NaJieDataSource.RemoveGoods(baseNaJieGoodEntity.GoodsId, num);
						baseNaJieGoodEntity2.GoodsId = battleEndRingOutGoodsResp_.GoodsId;
						baseNaJieGoodEntity2.GoodsNum = num;
						AddGoodsByTempleteId(baseNaJieGoodEntity2);
						m_NaJieDataSource.NowCapacity = battleEndRingOutGoodsResp_.NowCapacity;
						SetRongLiangProgress();
						InitDiaoLuo(m_DiaoLuo);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(battleEndRingOutGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void InitDiaoLuo(List<BaseNaJieGoodEntity> data)
		{
			foreach (Transform item in Panel.DiaoLuoContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			if (data == null)
			{
				return;
			}
			foreach (BaseNaJieGoodEntity datum in data)
			{
				InstanceDiaoLuoGoodsItem(datum);
			}
		}

		private void InstanceDiaoLuoGoodsItem(BaseNaJieGoodEntity item)
		{
			UI_GoodsItem component = SetListContentParent(Panel.DiaoLuoContent, ItemObj).GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out value);
			Sprite value2 = null;
			m_GoodsIconSprite.TryGetValue(item.Type, out value2);
			component.SetItem(item, item.Name, item.FiveElement, item.Level, value, value2, DiaoLuoGoodsClick, isUsing: false, item.GoodsNum);
			SetGrticalListContentHight(Panel.DiaoLuoContent.childCount, Panel.DiaoLuoContent, Panel.DiaoLuoContent.gameObject.GetComponent<GridLayoutGroup>());
			if (!m_DiaoLuoObjDic.ContainsKey(item.TemplateId))
			{
				m_DiaoLuoObjDic.Add(item.TemplateId, component);
			}
		}

		private void DiaoLuoGoodsClick(object obj, GameObject gameObj)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = obj as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity == null)
			{
				return;
			}
			if (baseNaJieGoodEntity.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = baseNaJieGoodEntity,
					QueRenCallBack = (int num) =>
					{
						GetGoods(baseNaJieGoodEntity, gameObj, num);
					},
					TitleContnet = "获取"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				GetGoods(baseNaJieGoodEntity, gameObj, 1);
			}
		}

		private void GetGoods(BaseNaJieGoodEntity goods, GameObject obj, int num)
		{
			PickUpGoodsReq_1004026 data = new PickUpGoodsReq_1004026
			{
				GoodsId = goods.GoodsId,
				Count = num,
				BattleId = m_BattleId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PICK_UP_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PickUpGoodsResp_1004026 pickUpGoodsResp_ = new PickUpGoodsResp_1004026();
					pickUpGoodsResp_.MergeFrom(byteString);
					if (pickUpGoodsResp_.IsSuccess)
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
							RemoveGoodsByTempleteId(goods.GoodsId, num);
							baseNaJieGoodEntity.GoodsId = pickUpGoodsResp_.GoodsId;
							baseNaJieGoodEntity.GoodsNum = num;
							m_NaJieDataSource.AddGoodsByTempleteId(baseNaJieGoodEntity);
						}
						m_NaJieDataSource.NowCapacity = pickUpGoodsResp_.NowCapacity;
						SetRongLiangProgress();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(pickUpGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		public void RemoveGoodsByTempleteId(string templeteId, int num)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = null;
			foreach (BaseNaJieGoodEntity item in m_DiaoLuo)
			{
				if (templeteId == item.GoodsId)
				{
					baseNaJieGoodEntity = item;
					break;
				}
			}
			if (baseNaJieGoodEntity != null)
			{
				if (baseNaJieGoodEntity.GoodsNum <= num)
				{
					m_DiaoLuo.Remove(baseNaJieGoodEntity);
				}
				else
				{
					baseNaJieGoodEntity.GoodsNum -= num;
				}
			}
		}

		public void AddGoodsByTempleteId(BaseNaJieGoodEntity entity)
		{
			if (entity == null)
			{
				return;
			}
			foreach (BaseNaJieGoodEntity item in m_DiaoLuo)
			{
				if (item.Type != 14 && item.Type != 15 && item.Type != 16 && item.Type != 21 && item.Type != 26 && item.Type != 29 && item.Type != 32 && entity.TemplateId == item.TemplateId)
				{
					item.GoodsId = entity.GoodsId;
					item.GoodsNum += entity.GoodsNum;
					return;
				}
			}
			m_DiaoLuo.Add(entity);
		}

		private void LiKaiBtn()
		{
			BattleEndLeaveReq_1004037 battleEndLeaveReq_1004037 = new BattleEndLeaveReq_1004037
			{
				BattleId = m_BattleId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BattleEndLeaveReq, battleEndLeaveReq_1004037, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					new BattleEndLeaveResp_1004037();
					battleEndLeaveReq_1004037.MergeFrom(byteString);
				}
			});
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void OpenNaJie()
		{
			InitNaJie();
			TweenerCore<Vector2, Vector2, VectorOptions> tweenerCore = DOTween.To(() => Panel.NaJieRectTransform.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJieRectTransform.anchoredPosition = v;
			}, new Vector2(0f, -1.5f), 0.5f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				Panel.NaJieMaskBtn.gameObject.SetActive(value: true);
			}));
		}

		private void CloseNaJie()
		{
			DOTween.To(() => Panel.NaJieRectTransform.anchoredPosition, (Vector2 v) =>
			{
				Panel.NaJieRectTransform.anchoredPosition = v;
			}, new Vector2(0f, -522.22f), 0.5f);
			Panel.NaJieMaskBtn.gameObject.SetActive(value: false);
		}

		private void OneKeyLingQu()
		{
			Stack<Transform> stack = new Stack<Transform>();
			foreach (Transform item in Panel.DiaoLuoContent)
			{
				stack.Push(item);
			}
			if (stack.Count > 0)
			{
				MonoSingleton<GameStart>.Instance.ShowWaitReauestWnd();
				AutoDiaoLuoClick(stack);
			}
		}

		private void AutoDiaoLuoClick(Stack<Transform> diaoLuoQue)
		{
			if (diaoLuoQue == null || diaoLuoQue.Count == 0)
			{
				return;
			}
			Transform transform = diaoLuoQue.Pop();
			if (transform == null)
			{
				return;
			}
			UI_GoodsItem itemObj = transform.gameObject.GetComponent<UI_GoodsItem>();
			if (itemObj == null)
			{
				return;
			}
			BaseNaJieGoodEntity goods = itemObj.DataSource as BaseNaJieGoodEntity;
			if (goods == null)
			{
				return;
			}
			PickUpGoodsReq_1004026 data = new PickUpGoodsReq_1004026
			{
				GoodsId = goods.GoodsId,
				Count = goods.GoodsNum,
				BattleId = m_BattleId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PICK_UP_GOODS, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PickUpGoodsResp_1004026 pickUpGoodsResp_ = new PickUpGoodsResp_1004026();
					pickUpGoodsResp_.MergeFrom(byteString);
					if (pickUpGoodsResp_.IsSuccess)
					{
						BaseNaJieGoodEntity baseNaJieGoodEntity = goods.Copy();
						UnityEngine.Object.Destroy(itemObj.gameObject);
						if (m_DiaoLuoObjDic.ContainsKey(goods.TemplateId))
						{
							m_DiaoLuoObjDic.Remove(goods.TemplateId);
						}
						RemoveGoodsByTempleteId(goods.GoodsId, goods.GoodsNum);
						baseNaJieGoodEntity.GoodsId = pickUpGoodsResp_.GoodsId;
						baseNaJieGoodEntity.GoodsNum = goods.GoodsNum;
						m_NaJieDataSource.AddGoodsByTempleteId(baseNaJieGoodEntity);
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = pickUpGoodsResp_.NowCapacity;
						SetRongLiangProgress();
						if (diaoLuoQue.Count > 0)
						{
							AutoDiaoLuoClick(diaoLuoQue);
						}
						else
						{
							m_DiaoLuo.Clear();
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						}
					}
					else
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(pickUpGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}
	}
}
