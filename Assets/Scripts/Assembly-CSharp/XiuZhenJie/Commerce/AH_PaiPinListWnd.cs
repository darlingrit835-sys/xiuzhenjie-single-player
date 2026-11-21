using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class AH_PaiPinListWnd : Window
	{
		public AH_PaiPinListPanel Panel;

		private int m_Stage;

		private int m_Status;

		private List<PaiMai> m_PaiMaiList;

		private PaiMai m_PaiMai;

		private GameObject m_Item;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<AH_PaiPinListPanel>();
			m_Item = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/AH_Item.prefab");
			m_Stage = (int)paralist[0];
			PaiPinList();
			AddButtonClickListener(Panel.ExitBtn, Exit);
		}

		private void PaiPinList()
		{
			ClearNaJie();
			Singleton<NetManager>.Instance.SendMsg(MsgType.TREASURE_GOODS_LIST_REQ, null, (object obj) =>
			{
				m_PaiMaiList = new List<PaiMai>();
				m_Status = 0;
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					TreasureGoodsListResp_1006018 treasureGoodsListResp_ = new TreasureGoodsListResp_1006018();
					treasureGoodsListResp_.MergeFrom(byteString);
					if (treasureGoodsListResp_.IsSuccess)
					{
						foreach (TreasureGoodsListResp_1006018.Types.GoodsListResp_1006019 goods in treasureGoodsListResp_.GoodsList)
						{
							m_PaiMai = new PaiMai
							{
								FiveElement = goods.FiveElement,
								Rank = goods.Rank,
								GoodsName = goods.GoodsName,
								StartPrice = goods.FinalPrice,
								State = goods.Status
							};
							m_PaiMaiList.Add(m_PaiMai);
							if (goods.Status == 0)
							{
								m_Status++;
							}
						}
						AggregateShuXing(treasureGoodsListResp_.GoodsList.Count);
						ListShuXing();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(treasureGoodsListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						Exit();
					}
				}
			});
		}

		private void AggregateShuXing(int count)
		{
			Panel.AggregateText.text = $"共计{count}件  已完成{count - m_Status}件";
		}

		private void ListShuXing()
		{
			foreach (PaiMai paiMai in m_PaiMaiList)
			{
				SetListContentParent(Panel.ListRect, m_Item).GetComponent<AH_Item>().ShuaXinState(paiMai);
			}
			Panel.ListRect.sizeDelta = new Vector2(Panel.ListRect.sizeDelta.x, m_PaiMaiList.Count * 45);
		}

		private void ClearNaJie()
		{
			foreach (RectTransform item in Panel.ListRect)
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void Exit()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
