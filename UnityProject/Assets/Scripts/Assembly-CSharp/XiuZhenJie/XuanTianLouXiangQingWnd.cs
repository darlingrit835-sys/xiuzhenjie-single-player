using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuanTianLouXiangQingWnd : Window
	{
		private XuanTianLouEntity modelSource;

		public XuanTianLouXiangQingItem _panel;

		private UnityAction<XuanTianLouEntity> BuySuccessAction;

		private List<string> m_DetailList;

		private int m_GoodsNum = 1;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<XuanTianLouXiangQingItem>();
			AddButtonClickListener(_panel.QueRen, QueRenClick);
			AddButtonClickListener(_panel.Close, CloseWindow);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 1)
			{
				modelSource = paralist[0] as XuanTianLouEntity;
			}
			if (modelSource != null)
			{
				if (modelSource.XTL_GoodsType == 4)
				{
					_panel.AddBtn.interactable = false;
					_panel.RemoveBtn.interactable = false;
					_panel.NumText.text = "1";
				}
				m_DetailList = new List<string>(modelSource.GoodsDetail);
				InitDetailList();
				ReflashData();
			}
		}

		private void AddBtnClick()
		{
			m_GoodsNum++;
		}

		private void RemoveBtnClick()
		{
			if (m_GoodsNum > 1)
			{
				m_GoodsNum--;
			}
		}

		private void InitDetailList()
		{
			string item = string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(modelSource.Rank));
			string item2 = string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(modelSource.Stage));
			m_DetailList.Insert(0, item2);
			m_DetailList.Insert(0, item);
		}

		public void ReflashData()
		{
			_panel.Name.text = modelSource.Name;
			_panel.ItemPriceText.text = modelSource.GoodsPrice.ToString();
			switch (modelSource.BuyType)
			{
			case 1:
			{
				string arg2 = ((Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - modelSource.GoodsPrice * m_GoodsNum > 0) ? (Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - modelSource.GoodsPrice * m_GoodsNum).ToString() : "0");
				_panel.ShenYuLingShi.text = $"剩余灵石:{arg2}";
				break;
			}
			case 2:
			{
				string arg = ((Singleton<RoleManager>.Instance.Role_Entity.ShinyJade - modelSource.GoodsPrice * m_GoodsNum > 0) ? (Singleton<RoleManager>.Instance.Role_Entity.ShinyJade - modelSource.GoodsPrice * m_GoodsNum).ToString() : "0");
				_panel.ShenYuLingShi.text = $"剩余灵玉:{arg}";
				break;
			}
			}
			_panel.DanJia.text = ((modelSource.BuyType == 1) ? "灵石" : "灵玉");
			_panel.Des.text = modelSource.GoodsDescribe;
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShangPuXiangQingItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = _panel.ItemListContent.GetComponent<VerticalLayoutGroup>();
			if (m_DetailList != null && m_DetailList.Count > 0)
			{
				for (int i = 0; i < m_DetailList.Count; i++)
				{
					string[] array = m_DetailList[i].Split(',');
					SetListContentParent(_panel.ItemListContent, gameObject).GetComponent<ShangPuXiangQingItem>().SetValue(array[0], array[1]);
				}
				_panel.ItemListContent.sizeDelta = new Vector2(_panel.ItemListContent.sizeDelta.x, SetVerticalListContentHight(m_DetailList.Count, y, component));
			}
		}

		private void QueRenClick()
		{
			GouMaiDanYao();
		}

		private void GouMaiDanYao()
		{
			if (modelSource.BuyType == 2 && Singleton<RoleManager>.Instance.Role_Entity.ShinyJade < modelSource.GoodsPrice * m_GoodsNum)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("身上所携灵玉不足");
				return;
			}
			BuyXuanTianLouGoodsReq_1004035 data = new BuyXuanTianLouGoodsReq_1004035
			{
				Id = modelSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUY_XUANTIANLOU_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuyXuanTianLouGoodsResp_1004035 buyXuanTianLouGoodsResp_ = new BuyXuanTianLouGoodsResp_1004035();
					buyXuanTianLouGoodsResp_.MergeFrom(byteString);
					if (buyXuanTianLouGoodsResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<XuanTianLouWnd>("XuanTianLouWnd.prefab")?.BuySuccess(modelSource);
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)buyXuanTianLouGoodsResp_.ShinyJade;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buyXuanTianLouGoodsResp_.ShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						CloseWindow();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buyXuanTianLouGoodsResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
