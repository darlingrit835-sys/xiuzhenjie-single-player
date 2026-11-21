using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QZGItemInformationWnd : Window
	{
		public QZGItemInformationPanel Panel;

		private QZG_ItemModel m_DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<QZGItemInformationPanel>();
			AddButtonClickListener(Panel.GouMaiBtn, YiKouJiaBtnClick);
			AddButtonClickListener(Panel.CloseBtn, LiKaiBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 1)
			{
				m_DataSource = paralist[0] as QZG_ItemModel;
				if (m_DataSource != null)
				{
					Panel.GoodsNameText.text = m_DataSource.name;
				}
			}
			if (m_DataSource == null) return;
			var listing = m_DataSource;
			listing.attributeList = new List<string>();
			SetGoodsInfo();
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(listing.goodsId, listing.goodsType, listing.level, listing.rank, listing.fiveElement, 0, (List<string> strList) =>
			{
				if (listing != m_DataSource || strList == null) return;
				listing.attributeList = strList;
				SetGoodsInfo();
			});
		}

		private void SetGoodsInfo()
		{
			Panel.YiKouJiaText.text = m_DataSource.fixedPrice + "灵石";
			SetGoodsDetailList();
		}

		private void SetGoodsDetailList()
		{
			foreach (Transform item in Panel.ItemInfoListContent)
			{
				Object.Destroy(item.gameObject);
			}
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/QZG_InfoItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ItemInfoListContent.GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < m_DataSource.attributeList.Count; i++)
			{
				QiZhenGeInfoItem component2 = SetListContentParent(Panel.ItemInfoListContent, gameObject).GetComponent<QiZhenGeInfoItem>();
				string[] array = m_DataSource.attributeList[i].Split(',');
				if (array.Length >= 2)
				{
					component2.SetItem(array[0] + ":", array[1]);
				}
			}
			Panel.ItemInfoListContent.sizeDelta = new Vector2(Panel.ItemInfoListContent.sizeDelta.x, SetVerticalListContentHight(m_DataSource.attributeList.Count, y, component));
		}

		private void YiKouJiaBtnClick()
		{
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(m_DataSource.fixedPrice) < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("您的灵石不足，无法购买!");
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention($"是否花费'{m_DataSource.fixedPrice}'灵石购买{m_DataSource.name}", () =>
			{
				AuctionBuyReq_1006010 data = new AuctionBuyReq_1006010
				{
					AuctionId = m_DataSource.id
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.QZG_BUY_GOODS, data, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						AuctionBuyResp_1006010 auctionBuyResp_ = new AuctionBuyResp_1006010();
						auctionBuyResp_.MergeFrom(byteString);
						if (auctionBuyResp_.IsSuccess)
						{
							Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = auctionBuyResp_.ShinyStone;
							Singleton<UIManager>.Instance.FindWndByName<QiZhenGeWnd>("QiZhenGeWnd.prefab")?.ReflashCurrentPage();
							Singleton<UIManager>.Instance.FindWndByName<QiZhenGeWoDeJingPaiWnd>("QZG_WoDeJingPaiWnd.prefab")?.OnShow();
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							AuctionHall.PurchaseReceipt(Panel.transform, Panel.GoodsNameText.font, m_DataSource, () => Singleton<UIManager>.Instance.CloseWnd(this));
							return;
						}
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(auctionBuyResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}, isShowWaitUI: true);
			});
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("QZG_ItmeInformation.prefab");
		}
	}
}
