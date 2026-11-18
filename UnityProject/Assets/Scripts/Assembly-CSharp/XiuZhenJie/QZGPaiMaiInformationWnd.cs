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

		private bool m_IsYiKouJia;

		private BaseNaJieGoodEntity m_DataSource;

		private StoreType m_CurrentStoreType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<QZGPaiMaiInformationPanel>();
			AddButtonClickListener(_panel.Close, CloseBtnClick);
			AddToggleClickListener(_panel.PuTongPaiMai, (bool isOn) =>
			{
				Text component = _panel.PuTongPaiMai.transform.Find("Text").GetComponent<Text>();
				ChangeTextColor(component, isOn);
			});
			AddButtonClickListener(_panel.Add, AddBtnClick);
			AddButtonClickListener(_panel.StackedAdd, StackedAddBtnClick);
			AddButtonClickListener(_panel.Less, LessBtnClick);
			AddButtonClickListener(_panel.StackedLess, StackLessBtnClick);
			AddButtonClickListener(_panel.StackedMax, StackedMaxBut);
			AddButtonClickListener(_panel.QueRenFaBu, QueRenFaBuBtnClick);
			AddInputEndEditListener(_panel.QiPaiJiaInput, EndChangeInputValue);
			AddInputEndEditListener(_panel.StackedQiPaiJiaInput, StackedQiPaiJiaInput);
			AddToggleClickListener(_panel.YiKouJiaTog, YiKouJiaTog);
			AddToggleClickListener(_panel.YiKouJiaTog, JingPaiTog);
			m_IsYiKouJia = true;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as BaseNaJieGoodEntity;
				m_CurrentStoreType = (StoreType)paralist[1];
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
				_panel.QiPaiJiaInput.text = CommonMethod.HuoBiLongToString(ItemPrice, 4);
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
			m_ShouXuPrice = ((ItemPrice * GoodsNum * num / 100 < 1) ? 1 : (ItemPrice * GoodsNum * num / 100));
			_panel.WeiTuoShouXuFei.text = m_ShouXuPrice + "灵石";
			_panel.WeiTuoSHouXuFeiPercent.text = $"委托价格的{num}%";
		}

		private void YiKouJiaTog(bool IsOn)
		{
			if (IsOn)
			{
				_panel.QiPaiJia_Title.text = "一口价：";
				_panel.MiaoShuRect.gameObject.SetActive(value: false);
				Text component = _panel.YiKouJiaTog.transform.Find("Text").GetComponent<Text>();
				ChangeTextColor(component, IsOn);
				m_IsYiKouJia = true;
			}
		}

		private void JingPaiTog(bool IsOn)
		{
			if (IsOn)
			{
				_panel.QiPaiJia_Title.text = "起拍价：";
				_panel.MiaoShuRect.gameObject.SetActive(value: true);
				Text component = _panel.JingPaiTog.transform.Find("Text").GetComponent<Text>();
				ChangeTextColor(component, IsOn);
				m_IsYiKouJia = false;
			}
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
			if (ItemPrice >= 100)
			{
				ItemPrice -= 100L;
			}
			else
			{
				ItemPrice = 0L;
			}
			ReflashData();
		}

		private void YiKouJiaBtnClick()
		{
		}

		private void EndChangeInputValue(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				ItemPrice = 100L;
			}
			else
			{
				ItemPrice = int.Parse(str);
				if (ItemPrice > 1000000000)
				{
					ItemPrice = 1000000000L;
				}
				if (ItemPrice < 1)
				{
					ItemPrice = 1L;
				}
			}
			ReflashData();
		}

		private void QueRenFaBuBtnClick()
		{
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
			EntrustAuctionReq_1006008 data = new EntrustAuctionReq_1006008
			{
				FixedPrice = ItemPrice,
				GoodsId = m_DataSource.GoodsId,
				ShopType = (int)m_CurrentStoreType,
				Count = GoodsNum
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.QZG_GOODS_SHANGJIA, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EntrustAuctionResp_1006008 entrustAuctionResp_ = new EntrustAuctionResp_1006008();
					entrustAuctionResp_.MergeFrom(byteString);
					if (entrustAuctionResp_.IsSuccess)
					{
						if (m_DataSource.GoodsNum > 0)
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].ResetGoodsNum(m_DataSource.GoodsId, m_DataSource.GoodsNum);
						}
						else
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].RemoveGoods(m_DataSource.GoodsId);
						}
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = entrustAuctionResp_.NowRingCapacity;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = entrustAuctionResp_.ShinyStone;
					}
					CloseBtnClick();
					Singleton<UIManager>.Instance.CloseWnd("QZG_PaiMaiWnd.prefab");
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(entrustAuctionResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void StackedAddBtnClick()
		{
			GoodsNum += 10;
			if (GoodsNum > m_DataSource.GoodsNum)
			{
				GoodsNum = m_DataSource.GoodsNum;
			}
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
			if (string.IsNullOrEmpty(str))
			{
				GoodsNum = 1;
			}
			else
			{
				GoodsNum = int.Parse(str);
				if (GoodsNum > m_DataSource.GoodsNum)
				{
					GoodsNum = m_DataSource.GoodsNum;
				}
				if (GoodsNum < 1)
				{
					GoodsNum = 1;
				}
			}
			StackedReflashData();
		}

		private void Count()
		{
			_panel.CountText.text = $"{ItemPrice * GoodsNum}";
		}

		private void ChangeTextColor(Text _text, bool isChoose)
		{
			if (isChoose)
			{
				_text.color = Color.white;
			}
			else
			{
				_text.color = Color.black;
			}
		}
	}
}
