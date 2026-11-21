using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NaJieSpaceWnd : Window
	{
		public NaJieSpacePanel Panel;

		internal Button PracticeBook => ledger?.PracticeBook;
		internal bool PracticeBagLoaded { get; private set; }

		private XzjBoxEntity m_DataSource;

		private Common_ShaiXuanWnd m_Common_ShaiXuanWnd;

		private RingLedger ledger;
		private bool salePending;

		private const int EnterMiJingCost_Stage_1 = 800;

		private const int EnterMiJingCost_Stage_2 = 4000;

		private const int EnterMiJingCost_Stage_3 = 20000;

		private const int EnterMiJingCost_Stage_4 = 100000;

		private const int EnterMiJingCost_Stage_5 = 500000;

		private const int EnterMiJingCost_Stage_6 = 1000000;

		private const int EnterMiJingCost_Stage_7 = 5000000;

		private const int EnterMiJingCost_Stage_8 = 10000000;

		private const int EnterMiJingCost_Stage_9 = 30000000;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<NaJieSpacePanel>();
			StyleStorage();
			ledger = new RingLedger(Panel, (goods, row) => GoodsClick(goods, row, UseGoods, SoleOneWuPin));
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.ShaiXuanBtn, ShaiXuanWnd);
			AddButtonClickListener(Panel.YiJianChuShouBtn, OneKeySole);
			AddButtonClickListener(Panel.ShengJiBtn, ShengJiBeiBao);
		}

		private void AttachFilters()
		{
			if (m_Common_ShaiXuanWnd?.GameObject != null) return;
			var fields = new List<ShaiXuanItemType> {
				ShaiXuanItemType.AllGoodsType, ShaiXuanItemType.WuXing,
				ShaiXuanItemType.PinJie, ShaiXuanItemType.DengJie
			};
			var materials = new[] { "木材", "灵液", "灵土", "灵种", "金属", "翎羽", "符纸", "符墨" };
			m_Common_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpNamedWnd("RingGoodsFilter", "Common_ShaiXuanWnd.prefab", false, true,
				fields, true, materials) as Common_ShaiXuanWnd;
			if (m_Common_ShaiXuanWnd != null)
				m_Common_ShaiXuanWnd.ReChooseAction += FlashData;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (ledger.ReloadCategories()) ledger.RefreshCategoryStrip();
			m_DataSource = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			PracticeBagLoaded = false;
			m_DataSource.GetAllGoods(FlashData);
		}

		private void StyleStorage()
		{
			InkInventoryArt.SetSurface(Panel.transform);
			InkInventoryArt.StyleTitle(Panel.transform);
			InkInventoryArt.StyleLedger(Panel.transform.Find("Scroll View"));

			Transform filter = Panel.ShaiXuanBtn.transform;
			if (Panel.ShaiXuanBtn.targetGraphic is Image filterIcon)
				filterIcon.color = InkInterfaceArt.Ink;
			InkInventoryArt.StyleLabel(filter.Find("ShaiXuan_text")?.GetComponent<Text>(), 13);

			Transform back = Panel.CloseBtn.transform;
			InkInventoryArt.SetSurface(back.parent, "LightPlaque");
			InkInterfaceArt.Set(back.Find("Img")?.GetComponent<Image>(), "Back");
			InkInventoryArt.StyleLabel(back.Find("Text")?.GetComponent<Text>(), 13);
			InkDialogArt.StyleButton(Panel.ShengJiBtn, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(Panel.YiJianChuShouBtn, "LightPlaque", InkInterfaceArt.Ink);

			InkInterfaceArt.Set(Panel.RongLiangBg, "LightPlaque", false);
			Panel.RongLiangValueProgress.color = InkInventoryArt.Selected;
			Transform caption = Panel.RongLiangBg.transform.Find("TitleBg");
			InkInventoryArt.SetSurface(caption, "LightPlaque");
			InkInventoryArt.StyleLabel(caption?.Find("Text")?.GetComponent<Text>(), 11);
			Text storage = Panel.RongLiangText;
			InkInventoryArt.StyleLabel(storage, 14);
			storage.alignment = TextAnchor.MiddleLeft;
			storage.rectTransform.anchorMin = new Vector2(0f, 0.5f);
			storage.rectTransform.anchorMax = new Vector2(1f, 0.5f);
			storage.rectTransform.offsetMin = new Vector2(10f, -10f);
			storage.rectTransform.offsetMax = new Vector2(-60f, 10f);
			UIManager.FitTextToRect(storage);
			storage.resizeTextMinSize = 11;
		}

		public void FlashData()
		{
			if (GameObject == null || Panel == null || m_DataSource == null) return;
			var result = m_Common_ShaiXuanWnd?.GetResult();
			var filtered = ShaiXuanGoods(result, m_DataSource.GoodsList);
			ledger.SetItems(m_DataSource.GoodsList, filtered, Singleton<GuideManager>.Instance.Practice?.Stage == 1);
			RongLiangReflash();
			PracticeBagLoaded = true;
		}

		private List<BaseNaJieGoodEntity> ShaiXuanGoods(Dictionary<string, ShaiXuanParamsModel> result, List<BaseNaJieGoodEntity> data)
		{
			List<BaseNaJieGoodEntity> list = null;
			list = ((data != null) ? new List<BaseNaJieGoodEntity>(data) : new List<BaseNaJieGoodEntity>());
			if (result == null)
				return list;
			ShaiXuanParamsModel leiBie = null;
			if (result.TryGetValue("类别", out leiBie))
			{
				list = list.Where(g => RingGoodsCatalog.MatchesLegacyFilter(g, leiBie.ItemTag, leiBie.ItemName)).ToList();
			}
			ShaiXuanParamsModel fiveElement = null;
			if (result.TryGetValue("五行", out fiveElement) && fiveElement.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => x.FiveElement == int.Parse(fiveElement.ItemTag)).ToList();
			}
			ShaiXuanParamsModel pinJie = null;
			if (result.TryGetValue("品阶", out pinJie) && pinJie.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => WorldSetting.GetLevelByRank(x.Rank) == int.Parse(pinJie.ItemTag)).ToList();
			}
			ShaiXuanParamsModel dengJie = null;
			if (result.TryGetValue("等阶", out dengJie) && dengJie.ItemTag != null)
			{
				list = list.Where((BaseNaJieGoodEntity x) => WorldSetting.GetStageByLevel(x.Level) == int.Parse(dengJie.ItemTag)).ToList();
			}
			return list;
		}

		private void GoodsClick(object dataSource, GameObject gameObj, UnityAction<int, object, GameObject> DyBtnClick, UnityAction<int, object> XiaoHuiBtnClick)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			NaJieTipModel naJieTipModel = new NaJieTipModel();
			string wuPinIsCanUse = TalismanStudio.IsStudioGood(baseNaJieGoodEntity) ? "打开符匣" : WorldSetting.GetWuPinIsCanUse(baseNaJieGoodEntity.Type);
			if (wuPinIsCanUse == "采集" && !RingGoodsCatalog.IsHarvestablePlant(baseNaJieGoodEntity))
				wuPinIsCanUse = "";
			bool equipment = RingGoodsCatalog.Section(baseNaJieGoodEntity) == 1;
			if (equipment)
			{
				naJieTipModel.MiddleBtnText = "前往穿戴";
				DyBtnClick = (_type, obj, row) => UseZhuangBei(baseNaJieGoodEntity);
			}
			else if (wuPinIsCanUse == "合成")
			{
				if (baseNaJieGoodEntity.Level < 81)
				{
					naJieTipModel.MiddleBtnText = "合成";
					DyBtnClick = (int _type, object _obj, GameObject _gameObj) =>
					{
						Singleton<UIManager>.Instance.PopUpWnd("HeChengWnd.prefab", true, baseNaJieGoodEntity);
					};
				}
				else
				{
					DyBtnClick = (int _type, object _obj, GameObject _gameObj) =>
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention("已达到最大等级，无法向上合成！");
					};
				}
			}
			else
			{
				naJieTipModel.MiddleBtnText = wuPinIsCanUse;
			}
			if (equipment && !baseNaJieGoodEntity.IsBinding)
			{
				naJieTipModel.MiddleBtnText = "滴血认主";
				DyBtnClick = (int _type, object obj, GameObject _gameObj) =>
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("滴血认主后,将无法进行转售,是否确认滴血认主？", () =>
					{
						GoodsBindingReq_1004023 data = new GoodsBindingReq_1004023
						{
							GoodsId = baseNaJieGoodEntity.GoodsId
						};
						Singleton<NetManager>.Instance.SendMsg(MsgType.BINDING_GOODS, data, (object _obj) =>
						{
							MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
							ByteString byteString = _obj as ByteString;
							if (!(byteString == null))
							{
								GoodsBindingResp_1004023 goodsBindingResp_ = new GoodsBindingResp_1004023();
								goodsBindingResp_.MergeFrom(byteString);
								if (goodsBindingResp_ != null && goodsBindingResp_.IsSuccess)
								{
									baseNaJieGoodEntity.IsBinding = true;
									FlashData();
								}
								MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBindingResp_.Code));
							}
						}, isShowWaitUI: true);
					});
				};
			}
			naJieTipModel.JiaGe = baseNaJieGoodEntity.RecyclePrice;
			naJieTipModel.FromRing = true;
			naJieTipModel.LeftBtnText = "出售";
			naJieTipModel.LeftBtnClickEvent = RingLedger.Tradeable(baseNaJieGoodEntity) &&
				!(Singleton<GuideManager>.Instance.Practice?.Stage == 1 && PracticeGuideFlow.IsStarter(baseNaJieGoodEntity)) ? XiaoHuiBtnClick : null;
			naJieTipModel.MiddleBtnClickEvent = (int type, object dataObj) =>
			{
				if (DyBtnClick != null)
				{
					DyBtnClick(type, dataObj, gameObj);
				}
			};
			naJieTipModel.RightBtnClickEvent = LockOrUnLockGoods;
			naJieTipModel.Name = baseNaJieGoodEntity.Name;
			naJieTipModel.ContentList = baseNaJieGoodEntity.ContentList;
			naJieTipModel.type = baseNaJieGoodEntity.Type;
			naJieTipModel.dataSource = baseNaJieGoodEntity;
			naJieTipModel.IsLock = baseNaJieGoodEntity.IsLock;
			Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, naJieTipModel);

		}

		private void LockOrUnLockGoods(int index, object dataSource)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = dataSource as BaseNaJieGoodEntity;
			if (baseNaJieGoodEntity.IsLock)
				m_DataSource.UnLockOneGoods(baseNaJieGoodEntity.GoodsId, FlashData);
			else
				m_DataSource.LockOneGoods(baseNaJieGoodEntity.GoodsId, FlashData);
		}

		public void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void ShaiXuanWnd()
		{
			AttachFilters();
			m_Common_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void OneKeySole()
		{
			if (salePending) return;
			if (!ledger.Choosing) { ledger.SetChoosing(true); return; }
			var selected = ledger.Selected.ToList();
			if (selected.Count == 0) return;
			var request = new GoodsSellReq_1004022();
			foreach (var g in selected)
				request.GoodsInfos.Add(new GoodsSellReq_1004022.Types.GoodsInfoReq_1004010 { GoodsId = g.GoodsId, Count = g.GoodsNum });
			var preview = string.Join("、", selected.Take(3).Select(g => $"{g.Name}×{g.GoodsNum}"));
			if (selected.Count > 3) preview += " 等";
			var quantity = selected.Sum(g => (long)g.GoodsNum);
			var value = selected.Sum(g => (decimal)g.GoodsNum * g.RecyclePrice);
			MonoSingleton<GameStart>.Instance.PopUpAttention($"出售{preview}？共 {quantity} 件，可得 {value} 灵石。", () => Sell(request));
		}

		private void SoleOneWuPin(int type, object dataSource)
		{
			var goods = (BaseNaJieGoodEntity)dataSource;
			if (!RingLedger.Tradeable(goods) || salePending) return;
			void Confirm(int count)
			{
				if (count < 1 || count > goods.GoodsNum) return;
				var request = new GoodsSellReq_1004022();
				request.GoodsInfos.Add(new GoodsSellReq_1004022.Types.GoodsInfoReq_1004010 {
					Count = count, GoodsId = goods.GoodsId
				});
				string grade = WorldSetting.GetRankAllName(goods.Rank);
				MonoSingleton<GameStart>.Instance.PopUpAttention(
					$"出售{grade}「{goods.Name}」×{count}，可得 {(long)goods.RecyclePrice * count} 灵石。确认出售？",
					() => Sell(request));
			}
			if (goods.GoodsNum == 1) { Confirm(1); return; }
			var selection = new Common_ChooseNumEntity {
				Goods = goods, QueRenCallBack = Confirm, TitleContnet = "出售"
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, selection);
		}

		private void Sell(GoodsSellReq_1004022 request)
		{
			if (salePending) return;
			salePending = true;
			Singleton<NetManager>.Instance.SendMsg(MsgType.SELL_GOODS, request, (object _obj) =>
			{
				salePending = false;
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
						if (GameObject != null && Panel != null) ledger.SetChoosing(false);
						m_DataSource.GetAllGoods(FlashData);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsSellResp_.Code), goodsSellResp_.SuccessCount, goodsSellResp_.FailedCount, goodsSellResp_.AddShinyStone);
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
						RongLiangReflash();
					}
					else
					{
						string textValue2 = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsSellResp_.Code));
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue2);
					}
				}
			}, isShowWaitUI: true);
		}

		public void RemoveWuPin(string id, int num)
		{
			m_DataSource.RemoveGoods(id, num);
			m_DataSource.NowCapacity = m_DataSource.GoodsList.Count;
			FlashData();
		}

		private void UseGoods(int type, object dataSource, GameObject gameObject)
		{
			BaseNaJieGoodEntity data = dataSource as BaseNaJieGoodEntity;
			switch ((GoodsTypeEnum)type)
			{
			case GoodsTypeEnum.BOOK_ACHIEVEMENT:
				UseGongFa(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_SKILL:
				UseJiNeng(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_EQUIPMENT:
				UseQiTu(data, gameObject);
				break;
			case GoodsTypeEnum.EQUIPMENT:
			case GoodsTypeEnum.EQUIPMENT_ATTACK:
			case GoodsTypeEnum.EQUIPMENT_DEFENSE:
			case GoodsTypeEnum.MATRIX:
			case GoodsTypeEnum.EQUIPMENT_DEFENSE_APPAREL:
				UseZhuangBei(data);
				break;
			case GoodsTypeEnum.ELIXIR:
				UseDanYao(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_ELIXIR:
				UseDanFang(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_SIGN:
				UseFuShu(data, gameObject);
				break;
			case GoodsTypeEnum.SIGN:
				UseFuLu(data, gameObject);
				break;
			case GoodsTypeEnum.SECREAT_AREA:
			case GoodsTypeEnum.GOODS_SECRET:
			case GoodsTypeEnum.INHERIT_SECRET:
				UseMiJingKey(data, gameObject);
				break;
			case GoodsTypeEnum.GUILD_SECRET:
				UseDongTianFuDi(data, gameObject);
				break;
			case GoodsTypeEnum.MATERIAL_PLANT:
				if (RingGoodsCatalog.IsHarvestablePlant(data)) UseZhiWu(data, gameObject);
				break;
			case GoodsTypeEnum.HaoYunFu:
				UseHaoYunFu(data, gameObject);
				break;
			case GoodsTypeEnum.BOOK_MATRIX:
				UseZhenTu(data, gameObject);
				break;
			}
		}

		private void UseJiNeng(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseGongFa(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			var practice = Singleton<GuideManager>.Instance.Practice;
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
						if (PracticeGuideFlow.IsStarter(_data) && practice?.RecordMilestone(1) == true)
							return;
						string message = "《" + _data.Name + "》已收入参悟书目。";
						if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < _data.Level)
							message += "达到" + WorldSetting.GetRoleLevelNameByRoleLevel(_data.Level) + "后即可参悟。";
						else
							message += "可在「可参悟」中翻阅。";
						MonoSingleton<GameStart>.Instance.PopUpAttention(message);
						return;
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseQiTu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseZhuangBei(BaseNaJieGoodEntity _data)
		{
			var hud = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (hud != null) hud.ZhanDouBtnClick();
			else Singleton<UIManager>.Instance.PopUpWnd("RoleZhuangBeiWnd.prefab", true);
		}

		private void UseDanFang(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseFuShu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseZhenTu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseZhenPan(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
		}

		private void UseFuLu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			if (TalismanStudio.IsStudioGood(_data))
			{
				TalismanStudio.Open(Panel.transform, Panel.RongLiangText.font, null, null, null, _data.GoodsId);
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention("请在战斗设置中设置符箓");
		}

		private void UseDanYao(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			bool flag = false;
			for (int i = 0; i < (_data.ContentList?.Count ?? 0); i++)
			{
				if (_data.ContentList[i].Contains("增长类-体魄") || _data.ContentList[i].Contains("增长类-神识") || _data.ContentList[i].Contains("增长类-灵气") || _data.ContentList[i].Contains("增长类-真气") || _data.ContentList[i].Contains("增长类-真元"))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("如果当前存在未炼化完全的丹药，将直接覆盖丹药效果！", () =>
				{
					UseElixir(_data, itemObj);
				});
			}
			else
			{
				UseElixir(_data, itemObj);
			}
		}

		private void UseElixir(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			UsedElixirReq_1004032 data = new UsedElixirReq_1004032
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USED_ELIXIR, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UsedElixirResp_1004032 usedElixirResp_ = new UsedElixirResp_1004032();
					usedElixirResp_.MergeFrom(byteString);
					if (usedElixirResp_ != null && usedElixirResp_.IsSuccess)
					{
						RemoveWuPin(_data.GoodsId, 1);
						RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
						role_Entity.PureBlood = usedElixirResp_.PureBlood;
						role_Entity.CurrentPureBlood = usedElixirResp_.CurrentPureBlood;
						role_Entity.FlexibleRoot = usedElixirResp_.FlexibleRoot;
						role_Entity.SuperAir = usedElixirResp_.SuperAir;
						role_Entity.CurrentSuperAir = usedElixirResp_.CurrentSuperAir;
						role_Entity.Perception = usedElixirResp_.Perception;
						role_Entity.CurrentPerception = usedElixirResp_.CurrentPerception;
						role_Entity.Physique = usedElixirResp_.Physique;
						role_Entity.CurrentPhysique = usedElixirResp_.CurrentPhysique;
						role_Entity.CurrentVigor = usedElixirResp_.CurrentVigor;
						role_Entity.CurrentClever = usedElixirResp_.CurrentClever;
						role_Entity.CurrentPoisonVal = usedElixirResp_.CurrentPoisonVal;
						role_Entity.JinLingGen = usedElixirResp_.HasGold;
						role_Entity.MuLingGen = usedElixirResp_.HasWood;
						role_Entity.ShuiLingGen = usedElixirResp_.HasWater;
						role_Entity.TuLingGen = usedElixirResp_.HasEarth;
						role_Entity.HuoLingGen = usedElixirResp_.HasFire;
						role_Entity.HunLiVal = usedElixirResp_.HunLiVal;
						role_Entity.CurrentPower = usedElixirResp_.CurrentPower;
						role_Entity.CurrentAge = usedElixirResp_.Age;
						role_Entity.MaxAge = usedElixirResp_.MaxAge;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						if (Singleton<GameMapManager>.Instance.CurrentMapName == "DongFuScene")
						{
							Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab")?.OnShow();
						}
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(usedElixirResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseHaoYunFu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					if (goodsUseResp_ != null && goodsUseResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ExtendQiYun = 2000;
						RemoveWuPin(_data.GoodsId, 1);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void UseMiJingKey(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.CurrentPower < 20)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前体力不足20，无法打开秘境");
				return;
			}
			UsedSecretReq_1004031 data = new UsedSecretReq_1004031
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USED_SECRET, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UsedSecretResp_1004031 usedSecretResp_ = new UsedSecretResp_1004031();
					usedSecretResp_.MergeFrom(byteString);
					if (usedSecretResp_ != null && usedSecretResp_.IsSuccess)
					{
						PrepareEnterMiJing(usedSecretResp_.MapId, usedSecretResp_.MapType, usedSecretResp_.FiveElement, _data, itemObj);
					}
					else
					{
						MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(usedSecretResp_.Code));
					}
				}
			}, isShowWaitUI: true);
		}

		private void UseZhiWu(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GatherSeedReq_1013021 data = new GatherSeedReq_1013021
			{
				SeedId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GATHER_SEED_RESP, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GatherSeedResp_1013021 gatherSeedResp_ = new GatherSeedResp_1013021();
					gatherSeedResp_.MergeFrom(byteString);
					if (gatherSeedResp_.IsSuccess)
					{
						string textValue = "采集灵种成功！";
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(gatherSeedResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void PrepareEnterMiJing(string mapId, int mapType, int fiveElement, BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			BuildDungeonReq_1010001 data = new BuildDungeonReq_1010001
			{
				MapType = mapType,
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUILDDUNGEON, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuildDungeonResp_1010001 buildDungeonResp_ = new BuildDungeonResp_1010001();
					buildDungeonResp_.MergeFrom(byteString);
					string text = "";
					string text2 = "";
					if (buildDungeonResp_.IsSuccess)
					{
						text = buildDungeonResp_.MapId;
						text2 = buildDungeonResp_.DungeonId;
						MapEntity mapModel = new MapEntity
						{
							Id = text,
							MapName = _data.Name,
							FiveElement = fiveElement
						};
						RoleEnterDungeon(text, text2, mapModel, buildDungeonResp_.Address, buildDungeonResp_.Port);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buildDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					Singleton<RoleManager>.Instance.Role_Entity.CurrentPower = buildDungeonResp_.CurrentPower;
					Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
				}
			}, isShowWaitUI: true);
		}

		private void RoleEnterDungeon(string mapId, string dungeonId, MapEntity mapModel, string ipAddress, int port)
		{
			RoleJoinDungeonReq_1010002 data = new RoleJoinDungeonReq_1010002
			{
				DungeonId = dungeonId,
				MapId = mapId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLEJOINDUNGEON, data, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleJoinDungeonResp_1010002 roleJoinDungeonResp_ = new RoleJoinDungeonResp_1010002();
					roleJoinDungeonResp_.MergeFrom(byteString);
					if (roleJoinDungeonResp_.IsSuccess)
					{
						BattleLifeModel roleInfo = BattleManager.ReadEntryRole(roleJoinDungeonResp_, byteString);
						Singleton<BattleManager>.Instance.StartBattle(mapModel, BattleType.MiJingBattle, roleInfo, dungeonId, ipAddress, port);
					}
					else
					{
						string hintMsgByCode = roleJoinDungeonResp_.Code == 4290
							? $"精力不足，本次入场需 {ExpeditionPlace.DungeonVigor(mapId)} 点精力。"
							: MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleJoinDungeonResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void UseDongTianFuDi(BaseNaJieGoodEntity _data, GameObject itemObj)
		{
			GoodsUseReq_1004024 data = new GoodsUseReq_1004024
			{
				GoodsId = _data.GoodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.USE_GOODS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUseResp_1004024 goodsUseResp_ = new GoodsUseResp_1004024();
					goodsUseResp_.MergeFrom(byteString);
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUseResp_.Code));
				}
			}, isShowWaitUI: true);
		}

		private void RongLiangReflash()
		{
			if (!(Panel == null) && !(Panel.RongLiangText == null) && !(Panel.RongLiangValueProgress == null) && m_DataSource != null)
			{
				float fillAmount = ((m_DataSource.MaxCapacity == 0) ? 0f : (1f * (float)m_DataSource.NowCapacity / (float)m_DataSource.MaxCapacity));
				Panel.RongLiangText.text = $"已占 {m_DataSource.NowCapacity} / {m_DataSource.MaxCapacity} 格　余 {Math.Max(0, m_DataSource.MaxCapacity - m_DataSource.NowCapacity)} 格";
				Panel.RongLiangValueProgress.fillAmount = fillAmount;
			}
		}

		private void ShengJiBeiBao()
		{
			if (ledger.Choosing) { ledger.SetChoosing(false); return; }
			if (m_DataSource == null) return;
			int capacity = m_DataSource.MaxCapacity;
			int baseCapacity = capacity - (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 11 ? 50 : 0);
			int[] costs = { 10, 30, 60, 100, 150, 200, 300, 400, 500, 600 };
			int tier = (baseCapacity - 50) / 10;
			if (baseCapacity < 50 || baseCapacity % 10 != 0 || tier >= costs.Length)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("纳戒已达扩容上限。");
				return;
			}
			int price = costs[tier];
			if (Singleton<RoleManager>.Instance.ShengYuLingYu(price) < 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention($"扩充至 {capacity + 10} 格需要 {price} 灵玉，余额不足。");
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention($"消耗 {price} 灵玉，将纳戒由 {capacity} 格扩充至 {capacity + 10} 格？", SendUpdateRequest);
		}

		private void SendUpdateRequest()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPDATE_NAJIE, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GradeRingResp_1004025 gradeRingResp_ = new GradeRingResp_1004025();
					gradeRingResp_.MergeFrom(byteString);
					if (gradeRingResp_.IsSuccess)
					{
						m_DataSource.MaxCapacity = gradeRingResp_.MaxCapacity;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = gradeRingResp_.ShinyJade;
						RongLiangReflash();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(gradeRingResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			TalismanStudio.CloseIn(Panel.transform);
			ledger?.CloseCategoryOptions();
			ledger?.CloseSortOptions();
			if (m_Common_ShaiXuanWnd != null)
			{
				m_Common_ShaiXuanWnd.ReChooseAction -= FlashData;
				if (m_Common_ShaiXuanWnd.GameObject != null)
					Singleton<UIManager>.Instance.CloseWnd(m_Common_ShaiXuanWnd);
				m_Common_ShaiXuanWnd = null;
			}

		}
	}
}
