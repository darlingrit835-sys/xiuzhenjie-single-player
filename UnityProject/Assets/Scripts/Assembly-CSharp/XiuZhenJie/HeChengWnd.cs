using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class HeChengWnd : Window
	{
		private HeChengPanel m_Panel;

		private int m_WuPingShu = 2;

		private int m_HeChengWuPingShu = 1;

		private int m_KeHeChengShu;

		private BaseNaJieGoodEntity baseNaJieGoodEntity;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<HeChengPanel>();
			ResourceLoad();
			AddButtonClickListener(m_Panel.GuanBiButton, GuanBi);
			AddButtonClickListener(m_Panel.JiaHao, JiaHaoXiaoGuo);
			AddButtonClickListener(m_Panel.JianHao, JianHaoXiaoGuo);
			AddButtonClickListener(m_Panel.MaxButton, MaxButton);
			AddButtonClickListener(m_Panel.HeChengButton, HeCheng);
			if (paralist != null)
			{
				baseNaJieGoodEntity = new BaseNaJieGoodEntity();
				baseNaJieGoodEntity = paralist[0] as BaseNaJieGoodEntity;
			}
			XianShi();
			DanJia();
			m_Panel.HeChengWuPingShu.onEndEdit.AddListener(HeChengWuPingShu);
		}

		private void XianShi()
		{
			UI_GoodsItem component = m_Panel.HeChengQian.GetComponent<UI_GoodsItem>();
			m_GoodsIconSprite.TryGetValue(baseNaJieGoodEntity.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(baseNaJieGoodEntity.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetItem(baseNaJieGoodEntity, baseNaJieGoodEntity.Name, baseNaJieGoodEntity.FiveElement, baseNaJieGoodEntity.Level, value2, value, null, baseNaJieGoodEntity.IsUsing, m_WuPingShu);
			m_Panel.HeChenghou.GetComponent<UI_GoodsItem>().SetItem(baseNaJieGoodEntity, baseNaJieGoodEntity.Name, baseNaJieGoodEntity.FiveElement, baseNaJieGoodEntity.Level + 10, value2, value, null, baseNaJieGoodEntity.IsUsing, m_WuPingShu / 2);
		}

		private void JiaHaoXiaoGuo()
		{
			m_HeChengWuPingShu++;
			PanDu();
		}

		private void MaxButton()
		{
			m_HeChengWuPingShu = baseNaJieGoodEntity.GoodsNum / 2;
			PanDu();
		}

		private void JianHaoXiaoGuo()
		{
			m_HeChengWuPingShu--;
			PanDu();
		}

		private void HeChengWuPingShu(string count)
		{
			string text = m_Panel.HeChengWuPingShu.text;
			m_HeChengWuPingShu = int.Parse(text);
			PanDu();
		}

		private void PanDu()
		{
			if (baseNaJieGoodEntity.GoodsNum < 2)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("背包中当前物品数量不足2个，无法合成！");
				return;
			}
			if (m_HeChengWuPingShu * 2 > baseNaJieGoodEntity.GoodsNum)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention($"背包中该物品，最大可合成数量为{baseNaJieGoodEntity.GoodsNum / 2}！");
				m_HeChengWuPingShu = baseNaJieGoodEntity.GoodsNum / 2;
			}
			else if (m_HeChengWuPingShu < 2)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最低合成数量为1！");
				m_HeChengWuPingShu = 1;
			}
			XianShi();
			DanJia();
		}

		private void DanJia()
		{
			m_Panel.HeChengWuPingShu.text = m_HeChengWuPingShu.ToString();
			m_Panel.XiaoHaoShu.text = $"消耗{WorldSetting.GetChineseNumber(baseNaJieGoodEntity.GetGoodsStage())}阶{baseNaJieGoodEntity.Name} {m_HeChengWuPingShu * 2} 个";
		}

		private void HeCheng()
		{
			if (baseNaJieGoodEntity.GoodsNum < 2)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前物品数量不足2个，无法合成！");
				return;
			}
			GoodsCompoundReq_1004041 data = new GoodsCompoundReq_1004041
			{
				GoodsId = baseNaJieGoodEntity.GoodsId,
				Num = m_HeChengWuPingShu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_COMPOUND, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GoodsCompoundResp_1004041 goodsCompoundResp_ = new GoodsCompoundResp_1004041();
					goodsCompoundResp_.MergeFrom(byteString);
					if (goodsCompoundResp_.IsSuccess)
					{
						NaJieSpaceWnd naJieSpaceWnd = Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab");
						BaseNaJieGoodEntity entity = new BaseNaJieGoodEntity
						{
							FiveElement = goodsCompoundResp_.NewGoodsInfo.FiveElement,
							GoodsId = goodsCompoundResp_.NewGoodsInfo.GoodsId,
							TemplateId = goodsCompoundResp_.NewGoodsInfo.TemplateId,
							ConversionRate = goodsCompoundResp_.NewGoodsInfo.ConversionRate,
							Type = goodsCompoundResp_.NewGoodsInfo.Type,
							Level = goodsCompoundResp_.NewGoodsInfo.Level,
							Rank = goodsCompoundResp_.NewGoodsInfo.Rank,
							IsBinding = goodsCompoundResp_.NewGoodsInfo.IsBinding,
							Name = goodsCompoundResp_.NewGoodsInfo.Name,
							RecyclePrice = goodsCompoundResp_.NewGoodsInfo.RecyclePrice,
							GoodsVolume = goodsCompoundResp_.NewGoodsInfo.Volume,
							IsLock = goodsCompoundResp_.NewGoodsInfo.IsLock,
							GoodsNum = m_HeChengWuPingShu
						};
						Singleton<RoleManager>.Instance.RoleBoxDic[10].AddGoodsByTempleteId(entity);
						naJieSpaceWnd?.RemoveWuPin(baseNaJieGoodEntity.GoodsId, m_HeChengWuPingShu * 2);
						Singleton<RoleManager>.Instance.RoleBoxDic[10].NowCapacity = goodsCompoundResp_.NowCapacity;
						naJieSpaceWnd?.FlashData();
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsCompoundResp_.Code));
					GuanBi();
				}
			}, isShowWaitUI: true);
		}

		private void GuanBi()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}
	}
}
