using System;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QZGPaiMaiInformationWnd : Window
	{
		private QZGPaiMaiInformationPanel _panel;

		private long ItemPrice = 100L;

		private int GoodsNum = 1;

		private const int MAX_PRICE = 1000000000;

		private long m_ShouXuPrice;

		private bool posting;

		private BaseNaJieGoodEntity m_DataSource;

		private StoreType m_CurrentStoreType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<QZGPaiMaiInformationPanel>();
			AddButtonClickListener(_panel.Close, CloseBtnClick);
			_panel.JingPaiTog.gameObject.SetActive(false);
			_panel.YiKouJiaTog.SetIsOnWithoutNotify(true);
			_panel.YiKouJiaTog.interactable = false;
			_panel.PuTongPaiMai.interactable = false;
			_panel.MiaoShuRect.gameObject.SetActive(false);
			_panel.PuTongPaiMai.GetComponentInChildren<Text>(true).text = "自由挂牌";
			_panel.PaiMaiFangShi.GetComponent<Text>().text = "交易方式";
			_panel.GuaPaiShiChang.text = "24 小时";
			_panel.GuaPaiShiChang.transform.parent.GetComponent<Text>().text = "挂牌时长";
			_panel.WeiTuoShouXuFei.transform.parent.GetComponent<Text>().text = "挂牌费用";
			_panel.WeiTuoSHouXuFeiPercent.transform.parent.GetComponent<Text>().text = "挂牌费率";
			_panel.Des.text = "买家可按整组总价直接购入。";
			_panel.QiPaiJia_Title.text = "每件单价";
			_panel.QueRenFaBu.GetComponentInChildren<Text>(true).text = "确认挂牌";
			_panel.QiPaiJiaInput.contentType = InputField.ContentType.IntegerNumber;
			_panel.StackedQiPaiJiaInput.contentType = InputField.ContentType.IntegerNumber;
			AddButtonClickListener(_panel.Add, AddBtnClick);
			AddButtonClickListener(_panel.StackedAdd, StackedAddBtnClick);
			AddButtonClickListener(_panel.Less, LessBtnClick);
			AddButtonClickListener(_panel.StackedLess, StackLessBtnClick);
			AddButtonClickListener(_panel.StackedMax, StackedMaxBut);
			AddButtonClickListener(_panel.QueRenFaBu, QueRenFaBuBtnClick);
			AddInputEndEditListener(_panel.QiPaiJiaInput, EndChangeInputValue);
			AddInputEndEditListener(_panel.StackedQiPaiJiaInput, StackedQiPaiJiaInput);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as BaseNaJieGoodEntity;
				m_CurrentStoreType = (StoreType)paralist[1];
				GoodsNum = 1;
				ItemPrice = 100;
				_panel.StackedQiPaiJiaInput.text = "1";
			}
			Init();
		}

		private void Init()
		{
			ReflashData();
		}

		private void ReflashData()
		{
			if (m_DataSource != null)
			{
				_panel.ItemName.text = m_DataSource.Name;
				_panel.QiPaiJiaInput.text = ItemPrice.ToString();
				ComputeShouXuFei();
				Count();
			}
		}

		private void ComputeShouXuFei()
		{
			int num = 10;
			if (Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic.ContainsKey(2))
			{
				ChengHaoInfoEntity chengHaoInfoEntity = Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic[2];
				num -= chengHaoInfoEntity.EffectValue;
			}
			if (num < 5)
			{
				num = 5;
			}
			long total = ItemPrice * GoodsNum;
			m_ShouXuPrice = Math.Max(1, total / 100 * num + total % 100 * num / 100);
			_panel.WeiTuoShouXuFei.text = m_ShouXuPrice + "灵石";
			_panel.WeiTuoSHouXuFeiPercent.text = $"整组总价的 {num}%";
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void AddBtnClick()
		{
			ItemPrice += 100L;
			if (ItemPrice > 1000000000)
			{
				ItemPrice = 1000000000L;
			}
			ReflashData();
		}

		private void LessBtnClick()
		{
			ItemPrice = Math.Max(100, ItemPrice - 100);
			ReflashData();
		}

		private void EndChangeInputValue(string str)
		{
			if (long.TryParse(str, out long value))
				ItemPrice = Math.Max(100, Math.Min(MAX_PRICE, value));
			ReflashData();
		}

		private void QueRenFaBuBtnClick()
		{
			if (posting) return;
			if (ItemPrice > 1000000000)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("委托费上限为:" + 1000000000 + "灵石!");
				return;
			}
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(m_ShouXuPrice) < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足,无法支付委托手续费");
				return;
			}
			if (ItemPrice < 100)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("一口价不能低于100灵石");
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention(
				$"将「{m_DataSource.Name}」×{GoodsNum} 挂到自由市场，整组售价 {ItemPrice * GoodsNum:N0} 灵石。挂牌 24 小时，现收挂牌费 {m_ShouXuPrice:N0} 灵石。",
				PostListing);
		}

		private void PostListing()
		{
			if (posting) return;
			posting = true;
			EntrustAuctionReq_1006008 data = new EntrustAuctionReq_1006008
			{
				FixedPrice = ItemPrice,
				GoodsId = m_DataSource.GoodsId,
				ShopType = (int)m_CurrentStoreType,
				Count = GoodsNum
			};
			int remaining = m_DataSource.GoodsNum - GoodsNum;
			if (!Singleton<NetManager>.Instance.TrySendGameRequest(MsgType.QZG_GOODS_SHANGJIA, data, byteString =>
			{
				posting = false;
				if (byteString == null)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("尚未收到挂牌回执，请先到「我的挂牌」核对。");
					return;
				}
				var entrustAuctionResp_ = EntrustAuctionResp_1006008.Parser.ParseFrom(byteString);
				if (!entrustAuctionResp_.IsSuccess)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(entrustAuctionResp_.Code));
					return;
				}
				if (remaining > 0)
				{
					Singleton<RoleManager>.Instance.RoleBoxDic[10].ResetGoodsNum(data.GoodsId, remaining);
				}
				else
				{
					Singleton<RoleManager>.Instance.RoleBoxDic[10].RemoveGoods(data.GoodsId);
				}
				Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = entrustAuctionResp_.NowRingCapacity;
				Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = entrustAuctionResp_.ShinyStone;
				CloseBtnClick();
				Singleton<UIManager>.Instance.CloseWnd("QZG_PaiMaiWnd.prefab");
				Singleton<UIManager>.Instance.FindWndByName<QiZhenGeWnd>("QiZhenGeWnd.prefab")?.ReflashCurrentPage();
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("挂牌已生效，可在「我的挂牌」查看。");
			}))
			{
				posting = false;
				MonoSingleton<GameStart>.Instance.PopUpAttention("连接中断，请重新连接后挂牌。");
			}
		}

		private void StackedAddBtnClick()
		{
			GoodsNum = (int)Math.Min(m_DataSource.GoodsNum, (long)GoodsNum + 10);
			StackedReflashData();
		}

		private void StackLessBtnClick()
		{
			if (GoodsNum > 10)
			{
				GoodsNum -= 10;
			}
			else
			{
				GoodsNum = 1;
			}
			StackedReflashData();
		}

		private void StackedReflashData()
		{
			_panel.StackedQiPaiJiaInput.text = $"{GoodsNum}";
			Count();
			ComputeShouXuFei();
		}

		private void StackedMaxBut()
		{
			GoodsNum = m_DataSource.GoodsNum;
			StackedReflashData();
		}

		private void StackedQiPaiJiaInput(string str)
		{
			if (long.TryParse(str, out long value))
				GoodsNum = (int)Math.Max(1, Math.Min(m_DataSource.GoodsNum, value));
			StackedReflashData();
		}

		private void Count()
		{
			_panel.CountText.text = $"{ItemPrice * GoodsNum}";
		}

	}
}
