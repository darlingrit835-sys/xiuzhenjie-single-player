using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class AH_MainSceneWndl : Window
	{
		public AH_MainScenePanel Panel;

		private int m_QdDengji = 1;

		private int m_Stage;

		private int m_ToEnterAuctionTime;

		private int m_ToOnTheShelfTime;

		private List<PaiMai> m_PaiMaiList;

		private string m_Content;

		private PaiMai m_PaiMai;

		private GameObject m_Item;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<AH_MainScenePanel>();
			AddButtonClickListener(Panel.ExitBtn, Exit);
			AddButtonClickListener(Panel.AuctionBtn, AuctionBut);
			AddButtonClickListener(Panel.AKeyToBuyBtn, AkeyToBuy);
			AddButtonClickListener(Panel.EntranceBtn, Entrance);
			Panel.XuanZhelan.onValueChanged.AddListener((int arge) =>
			{
				m_QdDengji = arge + 1;
				PaiPinList();
			});
			m_Item = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/AH_Item.prefab");
			PaiPinList();
		}

		private void PaiPinList()
		{
			ClearNaJie();
			EnterTreasureRoomReq_1006011 data = new EnterTreasureRoomReq_1006011
			{
				Stage = m_QdDengji
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_TREASURE_ROOM_REQ, data, (object obj) =>
			{
				m_PaiMaiList = new List<PaiMai>();
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterTreasureRoomResp_1006011 enterTreasureRoomResp_ = new EnterTreasureRoomResp_1006011();
					enterTreasureRoomResp_.MergeFrom(byteString);
					if (enterTreasureRoomResp_.IsSuccess)
					{
						m_ToEnterAuctionTime = enterTreasureRoomResp_.ToEnterAuctionTime;
						m_ToOnTheShelfTime = enterTreasureRoomResp_.ToOnTheShelfTime;
						if (enterTreasureRoomResp_.GoodsList.Count > 0)
						{
							foreach (EnterTreasureRoomResp_1006011.Types.GoodsListResp_1006012 goods in enterTreasureRoomResp_.GoodsList)
							{
								m_PaiMai = new PaiMai
								{
									FiveElement = goods.FiveElement,
									Rank = goods.Rank,
									GoodsName = goods.GoodsName,
									StartPrice = goods.StartPrice
								};
								m_PaiMaiList.Add(m_PaiMai);
							}
							ListShuXing();
						}
						AggregateShuXing(enterTreasureRoomResp_.GoodsList?.Count ?? 0);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterTreasureRoomResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void ClearNaJie()
		{
			foreach (RectTransform paiPin in Panel.PaiPinList)
			{
				Object.Destroy(paiPin.gameObject);
			}
		}

		private void ListShuXing()
		{
			foreach (PaiMai paiMai in m_PaiMaiList)
			{
				SetListContentParent(Panel.PaiPinList, m_Item).GetComponent<AH_Item>().ShuaXin(paiMai);
			}
			Panel.PaiPinList.sizeDelta = new Vector2(Panel.PaiPinList.sizeDelta.x, m_PaiMaiList.Count * 45);
		}

		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_ToEnterAuctionTime > 0)
			{
				m_ToEnterAuctionTime -= (int)(Time.fixedDeltaTime * 1000f);
				Panel.EntranceTimeText.text = string.Format(TimeToString(m_ToEnterAuctionTime));
				if (Panel.EntranceBtn.interactable)
				{
					Panel.EntranceBtn.interactable = false;
				}
			}
			else
			{
				Panel.EntranceTimeText.text = string.Format(TimeToString(0));
				if (!Panel.EntranceBtn.interactable)
				{
					Panel.EntranceBtn.interactable = true;
				}
			}
			if (m_ToOnTheShelfTime > 0)
			{
				m_ToOnTheShelfTime -= (int)(Time.fixedDeltaTime * 1000f);
				Panel.AKeyToBuyTimeText.text = string.Format(TimeToString(m_ToOnTheShelfTime));
				if (Panel.AKeyToBuyBtn.interactable)
				{
					Panel.AKeyToBuyBtn.interactable = false;
				}
			}
			else
			{
				Panel.AKeyToBuyTimeText.text = string.Format(TimeToString(0));
				if (!Panel.AKeyToBuyBtn.interactable)
				{
					Panel.AKeyToBuyBtn.interactable = true;
				}
			}
		}

		private string TimeToString(int result)
		{
			int num = result / 1000 / 60 / 60;
			int num2 = (result - num * 3600000) / 60000;
			int num3 = (result - num * 3600000 - num2 * 60000) / 1000;
			return $"{num:D2}:{num2:D2}:{num3:D2}";
		}

		private void AggregateShuXing(int count)
		{
			Panel.Aggregate.text = $"共计{count}件";
		}

		private void AuctionBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("AH_WoDeGuaPaiWnd.prefab", true);
		}

		private void AkeyToBuy()
		{
			Singleton<UIManager>.Instance.PopUpWnd("AH_WoDePaiMaoWnd.prefab", true);
		}

		private void Entrance()
		{
			Panel.XuanZheBG.gameObject.SetActive(value: true);
			AddButtonClickListener(Panel.JingJie, JingJie);
			AddButtonClickListener(Panel.JingJie_1, JingJie_1);
			AddButtonClickListener(Panel.JingJie_2, JingJie_2);
			AddButtonClickListener(Panel.JingJie_3, JingJie_3);
			AddButtonClickListener(Panel.JingJie_4, JingJie_4);
			AddButtonClickListener(Panel.JingJie_5, JingJie_5);
			AddButtonClickListener(Panel.JingJie_6, JingJie_6);
			AddButtonClickListener(Panel.JingJie_7, JingJie_7);
			AddButtonClickListener(Panel.JingJie_8, JingJie_8);
		}

		private void JingJie()
		{
			m_Stage = 1;
			PanDuanJingRu(5);
		}

		private void JingJie_1()
		{
			m_Stage = 2;
			PanDuanJingRu(10);
		}

		private void JingJie_2()
		{
			m_Stage = 3;
			PanDuanJingRu(15);
		}

		private void JingJie_3()
		{
			m_Stage = 4;
			PanDuanJingRu(20);
		}

		private void JingJie_4()
		{
			m_Stage = 5;
			PanDuanJingRu(25);
		}

		private void JingJie_5()
		{
			m_Stage = 6;
			PanDuanJingRu(30);
		}

		private void JingJie_6()
		{
			m_Stage = 7;
			PanDuanJingRu(35);
		}

		private void JingJie_7()
		{
			m_Stage = 8;
			PanDuanJingRu(40);
		}

		private void JingJie_8()
		{
			m_Stage = 9;
			PanDuanJingRu(45);
		}

		private void PanDuanJingRu(int lingYu)
		{
			if (m_Stage == Singleton<RoleManager>.Instance.GetRoleStage() || m_Stage == Singleton<RoleManager>.Instance.GetRoleStage() + 1)
			{
				if (lingYu <= Singleton<RoleManager>.Instance.Role_Entity.ShinyJade)
				{
					Singleton<UIManager>.Instance.PopUpWnd("AH_PaiMaiHuWnd.prefab", true, m_Stage);
				}
				else
				{
					m_Content = "灵玉不足，禁止进入！";
					MonoSingleton<GameStart>.Instance.PopUpAttention(m_Content);
				}
			}
			else
			{
				m_Content = "等级不足，禁止进入！";
				MonoSingleton<GameStart>.Instance.PopUpAttention(m_Content);
			}
		}

		private void Exit()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
