using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DaoYouChuanWuWnd : Window
	{
		public DaoYouChuanWuPanel Panel;

		private List<BaseNaJieGoodEntity> m_NaJieList;

		private Dictionary<string, BaseNaJieGoodEntity> m_BaseNaJieGoods;

		private Dictionary<string, BaseNaJieGoodEntity> m_ChoosedGoods;

		private Dictionary<string, UI_GoodsItem> m_NaJieGoods;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_UI_TogGoodsItem;

		private string m_RoleName;

		private string m_RoleId;
		private bool offerSending;
		private ByteString lastOffer;
		private string offerToken;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DaoYouChuanWuPanel>();
			InkSocialArt.ApplyBackdrop(Panel.transform, Panel.GetComponent<Image>());
			InkInterfaceArt.Set(Panel.TitleText.transform.parent.GetComponent<Image>(), "LightPaper", false);
			Panel.TitleText.color = InkInterfaceArt.Ink;
			UIManager.FitTextToRect(Panel.TitleText);
			InkDialogArt.StyleButton(Panel.ChuanWuBtn, "LightAction", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(Panel.LiKaiBtn, "LightPlaque", InkInterfaceArt.Ink);
			AddButtonClickListener(Panel.LiKaiBtn, LiKai);
			AddButtonClickListener(Panel.ChuanWuBtn, SendClick);
			m_BaseNaJieGoods = new Dictionary<string, BaseNaJieGoodEntity>();
			m_NaJieGoods = new Dictionary<string, UI_GoodsItem>();
			m_ChoosedGoods = new Dictionary<string, BaseNaJieGoodEntity>();
			m_NaJieList = new List<BaseNaJieGoodEntity>();
			m_UI_TogGoodsItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			ResourceLoad();
			GetNaJieGoods();
			m_RoleId = paralist[0].ToString();
			m_RoleName = paralist[1].ToString();
			Panel.TitleText.text = "传物给" + m_RoleName;
			Panel.LingShiHuaFeiText.text = 0 + "灵石";
		}

		private void GetNaJieGoods()
		{
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetAllGoods(() =>
			{
				if (GameObject == null)
					return;
				m_BaseNaJieGoods.Clear();
				m_NaJieGoods.Clear();
				foreach (Transform child in Panel.NaJieContent)
				{
					child.gameObject.SetActive(false);
					Object.Destroy(child.gameObject);
				}
				m_NaJieList = new List<BaseNaJieGoodEntity>(Singleton<RoleManager>.Instance.RoleBoxDic[10].GoodsList);
				foreach (BaseNaJieGoodEntity naJie in m_NaJieList)
				{
					if (!m_BaseNaJieGoods.ContainsKey(naJie.GoodsId))
					{
						m_BaseNaJieGoods.Add(naJie.GoodsId, naJie);
					}
				}
				CreateNaJieGoods();
			});
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void LiKai()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void SendClick()
		{
			if (offerSending || m_ChoosedGoods == null || m_ChoosedGoods.Count == 0)
			{
				return;
			}
			RemoteSendGoodsReq_1014005 remoteSendGoodsReq_ = new RemoteSendGoodsReq_1014005
			{
				TargetFriendId = m_RoleId
			};
			bool flag = false;
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> choosedGood in m_ChoosedGoods)
			{
				if (choosedGood.Value.IsLock)
				{
					flag = true;
				}
				remoteSendGoodsReq_.Goods.Add(new RemoteSendGoodsReq_1014005.Types.GoodsReq_1014011
				{
					Count = choosedGood.Value.GoodsNum,
					GoodsId = choosedGood.Value.GoodsId
				});
			}
			if (flag)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("存在已经锁定的物品,请先解锁！");
				return;
			}
			if (FriendGoodsOffers.Enabled)
			{
				SendOffer(remoteSendGoodsReq_);
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.REMOTE_SENDGOODS, remoteSendGoodsReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RemoteSendGoodsResp_1014005 remoteSendGoodsResp_ = new RemoteSendGoodsResp_1014005();
					remoteSendGoodsResp_.MergeFrom(byteString);
					if (remoteSendGoodsResp_.IsSuccess &&
						(!Constants.UsesPackagedTrialData || remoteSendGoodsResp_.Code == 4397))
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = remoteSendGoodsResp_.CurrentShinyStone;
						ClearSendGoods();
						Panel.LingShiHuaFeiText.text = 0 + "灵石";
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(remoteSendGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode2 = Constants.UsesPackagedTrialData && (remoteSendGoodsResp_.Code == 2000 || remoteSendGoodsResp_.Code == 4008)
							? "当前修真界暂未开放传物。"
							: MonoSingleton<GameStart>.Instance.GetHintMsgByCode(remoteSendGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SendOffer(RemoteSendGoodsReq_1014005 request)
		{
			var payload = request.ToByteString();
			if (!payload.Equals(lastOffer))
			{
				lastOffer = payload;
				offerToken = System.Guid.NewGuid().ToString("N");
			}
			FriendGoodsOffers.AttachToken(request, offerToken);
			offerSending = Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.FriendOfferCreate, request, data =>
			{
				offerSending = false;
				var game = MonoSingleton<GameStart>.Instance;
				var response = data == null ? null : ActionBoolResp_1001001.Parser.ParseFrom(data);
				if (response?.IsSuccess == true)
				{
					lastOffer = null;
					if (GameObject != null)
					{
						ClearSendGoods();
						Panel.LingShiHuaFeiText.text = "0灵石";
						GetNaJieGoods();
					}
					game.PopRollingTextShow("物品已锁定，等待对方确认。接受时才扣除灵石。");
					return;
				}
				game.PopRollingTextShow(response == null ? "暂未收到回复，可重试这份传物。" :
					response.Code == 4410 ? "对方还有太多待确认传物，请稍后再试。" :
					game.GetHintMsgByCode(response.Code) ?? "暂时无法送出传物。");
			});
			if (!offerSending)
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("尚未连接修真界，请重试。");
		}

		public override void ReFlashData()
		{
			if (FriendGoodsOffers.Enabled && GameObject != null && !offerSending)
			{
				ClearSendGoods();
				SetChoosedGoods();
				GetNaJieGoods();
			}
		}

		private void ClearSendGoods()
		{
			UI_GoodsItem[] waitSendGoods = Panel.WaitSendGoods;
			for (int i = 0; i < waitSendGoods.Length; i++)
			{
				waitSendGoods[i].ClearItem();
			}
			m_ChoosedGoods.Clear();
		}

		private void CreateNaJieGoods()
		{
			int num = 0;
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> baseNaJieGood in m_BaseNaJieGoods)
			{
				if (!baseNaJieGood.Value.IsBinding && !baseNaJieGood.Value.IsUsing)
				{
					CreateItem(baseNaJieGood.Value);
					num++;
				}
			}
			GridLayoutGroup component = Panel.NaJieContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(num, Panel.NaJieContent, component);
		}

		private void CreateItem(BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(Panel.NaJieContent, m_UI_TogGoodsItem);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(data.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			component.SetItem(data, data.Name, data.FiveElement, data.Level, value2, value, GoodsChoosed, data.IsUsing, data.GoodsNum);
			if (data.IsLock)
			{
				component.ClickBtn.interactable = false;
				var label = new GameObject("TransferLocked", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
				label.transform.SetParent(gameObject.transform, false);
				label.font = Panel.TitleText.font;
				label.raycastTarget = false;
				label.text = "锁定";
				DaoYouItem.Place(label.rectTransform, 0f, 1f, 1f, 0f, 0f, -18f, -1f);
				DaoYouItem.StyleText(label, 12, new Color32(139, 54, 39, 255));
			}
			if (!m_NaJieGoods.ContainsKey(data.GoodsId))
			{
				m_NaJieGoods.Add(data.GoodsId, component);
			}
		}

		private void GoodsChoosed(object data, GameObject itemObj)
		{
			BaseNaJieGoodEntity goods = data as BaseNaJieGoodEntity;
			itemObj.GetComponent<UI_GoodsItem>();
			if (m_ChoosedGoods.Count == 5 && !m_ChoosedGoods.ContainsKey(goods.GoodsId))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("单次传物数量已达上限！");
			}
			else if (goods.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = goods,
					QueRenCallBack = (int num) =>
					{
						if (!m_ChoosedGoods.ContainsKey(goods.GoodsId))
						{
							BaseNaJieGoodEntity baseNaJieGoodEntity2 = goods.Copy();
							baseNaJieGoodEntity2.GoodsNum = num;
							m_ChoosedGoods.Add(goods.GoodsId, baseNaJieGoodEntity2);
						}
						else
						{
							m_ChoosedGoods[goods.GoodsId].GoodsNum += num;
						}
						if (goods.GoodsNum > num)
						{
							goods.GoodsNum -= num;
							if (m_NaJieGoods.ContainsKey(goods.GoodsId))
							{
								m_NaJieGoods[goods.GoodsId].ReduceNum(num);
							}
						}
						else
						{
							m_BaseNaJieGoods.Remove(goods.GoodsId);
							if (m_NaJieGoods.ContainsKey(goods.GoodsId))
							{
								GameObject gameObject2 = m_NaJieGoods[goods.GoodsId].gameObject;
								m_NaJieGoods.Remove(goods.GoodsId);
								Object.Destroy(gameObject2);
							}
							if (m_BaseNaJieGoods.ContainsKey(goods.GoodsId))
							{
								m_BaseNaJieGoods.Remove(goods.GoodsId);
							}
						}
						SetChoosedGoods();
					},
					TitleContnet = "传物"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				if (!m_ChoosedGoods.ContainsKey(goods.GoodsId))
				{
					BaseNaJieGoodEntity baseNaJieGoodEntity = goods.Copy();
					baseNaJieGoodEntity.GoodsNum = 1;
					m_ChoosedGoods.Add(goods.GoodsId, baseNaJieGoodEntity);
				}
				else
				{
					m_ChoosedGoods[goods.GoodsId].GoodsNum++;
				}
				if (m_NaJieGoods.ContainsKey(goods.GoodsId))
				{
					GameObject gameObject = m_NaJieGoods[goods.GoodsId].gameObject;
					m_NaJieGoods.Remove(goods.GoodsId);
					Object.Destroy(gameObject);
				}
				if (m_BaseNaJieGoods.ContainsKey(goods.GoodsId))
				{
					m_BaseNaJieGoods.Remove(goods.GoodsId);
				}
				SetChoosedGoods();
			}
		}

		private void SetChoosedGoods()
		{
			UI_GoodsItem[] waitSendGoods = Panel.WaitSendGoods;
			for (int i = 0; i < waitSendGoods.Length; i++)
			{
				waitSendGoods[i].ClearItem();
			}
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> choosedGood in m_ChoosedGoods)
			{
				Sprite value = null;
				m_GoodsIconSprite.TryGetValue(choosedGood.Value.Type, out value);
				int levelByRank = WorldSetting.GetLevelByRank(choosedGood.Value.Rank);
				Sprite value2 = null;
				m_RankBgSprite.TryGetValue(levelByRank, out value2);
				Panel.WaitSendGoods[num].SetItem(choosedGood.Value, choosedGood.Value.Name, choosedGood.Value.FiveElement, choosedGood.Value.Level, value2, value, TakeDownGoods, isUsing: false, choosedGood.Value.GoodsNum);
				num++;
				num2 += choosedGood.Value.Rank * choosedGood.Value.GetGoodsStage() * 100 * choosedGood.Value.GoodsNum;
			}
			ComputeCastLingShi(num2);
			if (m_ChoosedGoods.Count <= 0)
			{
				Panel.ChuanWuBtn.interactable = false;
			}
			else
			{
				Panel.ChuanWuBtn.interactable = true;
			}
		}

		private void ComputeCastLingShi(int price)
		{
			int num = price;
			if (Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic.ContainsKey(1))
			{
				price = Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic[1].EffectValue * price / 100;
				string text = $"{num} - {price} 灵石";
				Panel.LingShiHuaFeiText.text = text;
			}
			else
			{
				Panel.LingShiHuaFeiText.text = num + "灵石";
			}
		}

		private void TakeDownGoods(object data, GameObject itemObj)
		{
			BaseNaJieGoodEntity goods = data as BaseNaJieGoodEntity;
			itemObj.GetComponent<UI_GoodsItem>();
			if (goods.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = goods,
					QueRenCallBack = (int num) =>
					{
						if (goods.GoodsNum > num)
						{
							m_ChoosedGoods[goods.GoodsId].GoodsNum -= num;
						}
						else
						{
							m_ChoosedGoods.Remove(goods.GoodsId);
						}
						BaseNaJieGoodEntity baseNaJieGoodEntity2 = goods.Copy();
						baseNaJieGoodEntity2.GoodsNum = num;
						if (!m_NaJieGoods.ContainsKey(baseNaJieGoodEntity2.GoodsId))
						{
							CreateItem(baseNaJieGoodEntity2);
						}
						else
						{
							m_NaJieGoods[baseNaJieGoodEntity2.GoodsId].AddNum(num);
						}
						if (!m_BaseNaJieGoods.ContainsKey(baseNaJieGoodEntity2.GoodsId))
						{
							m_BaseNaJieGoods.Add(baseNaJieGoodEntity2.GoodsId, baseNaJieGoodEntity2);
						}
						else
						{
							m_BaseNaJieGoods[baseNaJieGoodEntity2.GoodsId].GoodsNum += num;
						}
						SetChoosedGoods();
					},
					TitleContnet = "取下"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				m_ChoosedGoods.Remove(goods.GoodsId);
				if (!m_NaJieGoods.ContainsKey(goods.GoodsId))
				{
					BaseNaJieGoodEntity baseNaJieGoodEntity = goods.Copy();
					baseNaJieGoodEntity.GoodsNum = 1;
					CreateItem(baseNaJieGoodEntity);
				}
				else
				{
					m_NaJieGoods[goods.GoodsId].AddNum(1);
				}
				if (!m_BaseNaJieGoods.ContainsKey(goods.GoodsId))
				{
					m_BaseNaJieGoods.Add(goods.GoodsId, goods);
				}
				else
				{
					m_BaseNaJieGoods[goods.GoodsId].GoodsNum++;
				}
				SetChoosedGoods();
			}
		}
	}
}
