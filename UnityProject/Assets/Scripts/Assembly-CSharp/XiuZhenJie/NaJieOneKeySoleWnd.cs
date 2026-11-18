using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class NaJieOneKeySoleWnd : Window
	{
		public NaJieOneKeySolePanel Panel;

		private Dictionary<int, List<BaseNaJieGoodEntity>> WuPinDic;

		private List<BaseNaJieGoodEntity> TypeList;

		private List<BaseNaJieGoodEntity> NoChooseTypeList;

		private List<BaseNaJieGoodEntity> FiveElementList;

		private List<BaseNaJieGoodEntity> NoChooseFiveElementList;

		private List<BaseNaJieGoodEntity> LevelList;

		private List<BaseNaJieGoodEntity> NoChooseLevelList;

		private List<BaseNaJieGoodEntity> RankList;

		private List<BaseNaJieGoodEntity> NoChooseRankList;

		private List<BaseNaJieGoodEntity> resultList;

		private XzjBoxEntity DataSource;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<NaJieOneKeySolePanel>();
			AddButtonClickListener(Panel.CancleBtn, CloseWnd);
			AddButtonClickListener(Panel.MaskBtn, CloseWnd);
			AddButtonClickListener(Panel.QueRenBtn, QueRenGouMaiBtnClick);
			AddToggleClickListener(Panel.FaQiTog, FaQiChoose);
			AddToggleClickListener(Panel.CaiLiaoTog, CaiLiaoChoose);
			AddToggleClickListener(Panel.ShuJiTog, ShuJiChoose);
			AddToggleClickListener(Panel.ZhenPanTog, ZhenPanChoose);
			AddToggleClickListener(Panel.FuLuTog, FuLuChoose);
			AddToggleClickListener(Panel.LianQiTog, LianQiChoose);
			AddToggleClickListener(Panel.ZhuJiTog, ZhuJiChoose);
			AddToggleClickListener(Panel.JieDanTog, JieDanChoose);
			AddToggleClickListener(Panel.YuanYingTog, YuanYinChoose);
			AddToggleClickListener(Panel.HuaShenTog, HuaShenChoose);
			AddToggleClickListener(Panel.LianXuTog, LianXuChoose);
			AddToggleClickListener(Panel.HeTiTog, HeTiChoose);
			AddToggleClickListener(Panel.DuJieTog, DuJieChoose);
			AddToggleClickListener(Panel.DaChengTog, DaChengChoose);
			AddToggleClickListener(Panel.HuangJieTog, HuangJie);
			AddToggleClickListener(Panel.XuanJieTog, XuanJie);
			AddToggleClickListener(Panel.DiJieTog, DiJie);
			AddToggleClickListener(Panel.WuXing_AllTog, WuXing_All);
			AddToggleClickListener(Panel.WuXing_JinTog, WuXing_Jin);
			AddToggleClickListener(Panel.WuXing_MuTog, WuXing_Mu);
			AddToggleClickListener(Panel.WuXing_ShuiTog, WuXing_Shui);
			AddToggleClickListener(Panel.WuXing_HuoTog, WuXing_Huo);
			AddToggleClickListener(Panel.WuXing_TuTog, WuXing_Tu);
			AddButtonClickListener(Panel.FiveElementAllChooseBtn, FiveElementQuanXuan);
			AddButtonClickListener(Panel.LeiBieAllChooseBtn, LeiBieQuanXuan);
			AddButtonClickListener(Panel.RankAllChooseBtn, RankQuanXuan);
			AddButtonClickListener(Panel.LevelAllChooseBtn, LevelQuanXuan);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			DataSource = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			WuPinDic = new Dictionary<int, List<BaseNaJieGoodEntity>>();
			TypeList = new List<BaseNaJieGoodEntity>();
			NoChooseTypeList = new List<BaseNaJieGoodEntity>();
			NoChooseLevelList = new List<BaseNaJieGoodEntity>();
			LevelList = new List<BaseNaJieGoodEntity>();
			RankList = new List<BaseNaJieGoodEntity>();
			NoChooseRankList = new List<BaseNaJieGoodEntity>();
			FiveElementList = new List<BaseNaJieGoodEntity>();
			NoChooseFiveElementList = new List<BaseNaJieGoodEntity>();
			resultList = new List<BaseNaJieGoodEntity>();
			InitDic();
			InitData();
		}

		private void InitDic()
		{
			WuPinDic.Clear();
			foreach (BaseNaJieGoodEntity goods in DataSource.GoodsList)
			{
				if (goods.Type == 29)
				{
					if (WuPinDic.ContainsKey(16))
					{
						WuPinDic[16].Add(goods);
						continue;
					}
					WuPinDic.Add(16, new List<BaseNaJieGoodEntity> { goods });
				}
				else if (WuPinDic.ContainsKey(goods.Type))
				{
					WuPinDic[goods.Type].Add(goods);
				}
				else
				{
					WuPinDic.Add(goods.Type, new List<BaseNaJieGoodEntity> { goods });
				}
			}
		}

		public void InitData()
		{
			base.ReFlashData();
			foreach (KeyValuePair<int, List<BaseNaJieGoodEntity>> item in WuPinDic)
			{
				foreach (BaseNaJieGoodEntity item2 in item.Value)
				{
					if (item2.Type != 19 && item2.Rank < 13 && ((item2.Type != 15 && item2.Type != 26) || !item2.IsUsing))
					{
						NoChooseTypeList.Add(item2);
						NoChooseLevelList.Add(item2);
						NoChooseRankList.Add(item2);
						NoChooseFiveElementList.Add(item2);
					}
				}
			}
		}

		private void QueRenGouMaiBtnClick()
		{
			if (resultList.Count == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("无符合条件的物品！");
				return;
			}
			GoodsSellReq_1004022 goodsSellReq_ = new GoodsSellReq_1004022();
			foreach (BaseNaJieGoodEntity result in resultList)
			{
				goodsSellReq_.GoodsInfos.Add(new GoodsSellReq_1004022.Types.GoodsInfoReq_1004010
				{
					GoodsId = result.GoodsId,
					Count = result.GoodsNum
				});
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.SELL_GOODS, goodsSellReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsSellResp_1004022 goodsSellResp_ = new GoodsSellResp_1004022();
					goodsSellResp_.MergeFrom(byteString);
					if (goodsSellResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = goodsSellResp_.ShinyStone;
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = goodsSellResp_.NowCapacity;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab").OnShow();
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsSellResp_.Code), goodsSellResp_.SuccessCount, goodsSellResp_.FailedCount, goodsSellResp_.AddShinyStone);
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						string textValue2 = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsSellResp_.Code));
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue2);
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
				}
			}, isShowWaitUI: true);
		}

		private void TypeShaiXuan(bool isOn, List<int> typeList)
		{
			if (isOn)
			{
				List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity noChooseType in NoChooseTypeList)
				{
					foreach (int type in typeList)
					{
						if (noChooseType.Type == type)
						{
							list.Add(noChooseType);
						}
					}
				}
				foreach (BaseNaJieGoodEntity item in list)
				{
					NoChooseTypeList.Remove(item);
					TypeList.Add(item);
				}
			}
			else
			{
				List<BaseNaJieGoodEntity> list2 = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity type2 in TypeList)
				{
					foreach (int type3 in typeList)
					{
						if (type2.Type == type3)
						{
							list2.Add(type2);
						}
					}
				}
				foreach (BaseNaJieGoodEntity item2 in list2)
				{
					TypeList.Remove(item2);
					NoChooseTypeList.Add(item2);
				}
			}
			JiSuanJieGuo();
		}

		private void LevelShaiXuan(bool isOn, int stage)
		{
			if (isOn)
			{
				List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity noChooseLevel in NoChooseLevelList)
				{
					if (noChooseLevel.GetGoodsStage() == stage)
					{
						list.Add(noChooseLevel);
					}
				}
				foreach (BaseNaJieGoodEntity item in list)
				{
					NoChooseLevelList.Remove(item);
					LevelList.Add(item);
				}
			}
			else
			{
				List<BaseNaJieGoodEntity> list2 = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity level in LevelList)
				{
					if (level.GetGoodsStage() == stage)
					{
						list2.Add(level);
					}
				}
				foreach (BaseNaJieGoodEntity item2 in list2)
				{
					LevelList.Remove(item2);
					NoChooseLevelList.Add(item2);
				}
			}
			JiSuanJieGuo();
		}

		private void RankShaiXuan(bool isOn, int rank)
		{
			if (isOn)
			{
				List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity noChooseRank in NoChooseRankList)
				{
					if (noChooseRank.GetGoodsRank() == rank)
					{
						list.Add(noChooseRank);
					}
				}
				foreach (BaseNaJieGoodEntity item in list)
				{
					NoChooseRankList.Remove(item);
					RankList.Add(item);
				}
			}
			else
			{
				List<BaseNaJieGoodEntity> list2 = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity rank2 in RankList)
				{
					if (rank2.GetGoodsRank() == rank)
					{
						list2.Add(rank2);
					}
				}
				foreach (BaseNaJieGoodEntity item2 in list2)
				{
					RankList.Remove(item2);
					NoChooseRankList.Add(item2);
				}
			}
			JiSuanJieGuo();
		}

		private void FiveElementShaiXuan(bool isOn, int fiveElement)
		{
			if (isOn)
			{
				List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity noChooseFiveElement in NoChooseFiveElementList)
				{
					if (noChooseFiveElement.FiveElement == fiveElement)
					{
						list.Add(noChooseFiveElement);
					}
				}
				foreach (BaseNaJieGoodEntity item in list)
				{
					NoChooseFiveElementList.Remove(item);
					FiveElementList.Add(item);
				}
			}
			else
			{
				List<BaseNaJieGoodEntity> list2 = new List<BaseNaJieGoodEntity>();
				foreach (BaseNaJieGoodEntity fiveElement2 in FiveElementList)
				{
					if (fiveElement2.FiveElement == fiveElement)
					{
						list2.Add(fiveElement2);
					}
				}
				foreach (BaseNaJieGoodEntity item2 in list2)
				{
					FiveElementList.Remove(item2);
					NoChooseFiveElementList.Add(item2);
				}
			}
			JiSuanJieGuo();
		}

		private List<BaseNaJieGoodEntity> ComputLastResult(List<BaseNaJieGoodEntity> one, List<BaseNaJieGoodEntity> two)
		{
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			if (one.Count != 0 && two.Count != 0)
			{
				foreach (BaseNaJieGoodEntity item in one)
				{
					foreach (BaseNaJieGoodEntity item2 in two)
					{
						if (item.GoodsId == item2.GoodsId)
						{
							list.Add(item2);
							break;
						}
					}
				}
			}
			return list;
		}

		private void JiSuanJieGuo()
		{
			resultList.Clear();
			resultList = ComputLastResult(TypeList, LevelList);
			resultList = ComputLastResult(resultList, RankList);
			resultList = ComputLastResult(resultList, FiveElementList);
			long num = 0L;
			int num2 = 0;
			foreach (BaseNaJieGoodEntity result in resultList)
			{
				num += (long)result.RecyclePrice * (long)result.GoodsNum;
				num2 += result.GoodsNum;
			}
			Panel.wuPinText.text = $"已选物品{num2}件";
			Panel.LingShiValueText.text = num.ToString();
		}

		private void FaQiChoose(bool isOn)
		{
			TypeShaiXuan(isOn, new List<int> { 14, 15, 16, 26, 29 });
		}

		private void CaiLiaoChoose(bool isOn)
		{
			TypeShaiXuan(isOn, new List<int> { 7, 8, 9, 10, 11, 12, 13 });
		}

		private void ShuJiChoose(bool isOn)
		{
			TypeShaiXuan(isOn, new List<int> { 1, 2, 3, 4, 5, 6 });
		}

		private void ZhenPanChoose(bool isOn)
		{
			TypeShaiXuan(isOn, new List<int> { 20 });
		}

		private void FuLuChoose(bool isOn)
		{
			TypeShaiXuan(isOn, new List<int> { 21 });
		}

		private void LianQiChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 1);
		}

		private void ZhuJiChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 2);
		}

		private void JieDanChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 3);
		}

		private void YuanYinChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 4);
		}

		private void HuaShenChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 5);
		}

		private void LianXuChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 6);
		}

		private void HeTiChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 7);
		}

		private void DuJieChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 8);
		}

		private void DaChengChoose(bool isOn)
		{
			LevelShaiXuan(isOn, 9);
		}

		private void HuangJie(bool isOn)
		{
			RankShaiXuan(isOn, 1);
		}

		private void XuanJie(bool isOn)
		{
			RankShaiXuan(isOn, 2);
		}

		private void DiJie(bool isOn)
		{
			RankShaiXuan(isOn, 3);
		}

		private void WuXing_All(bool isOn)
		{
			FiveElementShaiXuan(isOn, 0);
		}

		private void WuXing_Jin(bool isOn)
		{
			FiveElementShaiXuan(isOn, 1);
		}

		private void WuXing_Mu(bool isOn)
		{
			FiveElementShaiXuan(isOn, 2);
		}

		private void WuXing_Shui(bool isOn)
		{
			FiveElementShaiXuan(isOn, 3);
		}

		private void WuXing_Huo(bool isOn)
		{
			FiveElementShaiXuan(isOn, 4);
		}

		private void WuXing_Tu(bool isOn)
		{
			FiveElementShaiXuan(isOn, 5);
		}

		private void LeiBieQuanXuan()
		{
			Panel.FaQiTog.isOn = true;
			Panel.ShuJiTog.isOn = true;
			Panel.CaiLiaoTog.isOn = true;
			Panel.ZhenPanTog.isOn = true;
			Panel.FuLuTog.isOn = true;
		}

		private void RankQuanXuan()
		{
			Panel.HuangJieTog.isOn = true;
			Panel.XuanJieTog.isOn = true;
			Panel.DiJieTog.isOn = true;
		}

		private void FiveElementQuanXuan()
		{
			Panel.WuXing_JinTog.isOn = true;
			Panel.WuXing_MuTog.isOn = true;
			Panel.WuXing_ShuiTog.isOn = true;
			Panel.WuXing_HuoTog.isOn = true;
			Panel.WuXing_TuTog.isOn = true;
		}

		private void LevelQuanXuan()
		{
			Panel.LianQiTog.isOn = true;
			Panel.ZhuJiTog.isOn = true;
			Panel.JieDanTog.isOn = true;
			Panel.YuanYingTog.isOn = true;
			Panel.HuaShenTog.isOn = true;
			Panel.LianXuTog.isOn = true;
			Panel.HeTiTog.isOn = true;
			Panel.DuJieTog.isOn = true;
			Panel.DaChengTog.isOn = true;
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("NaJieOneKeySoleWnd.prefab");
		}
	}
}
