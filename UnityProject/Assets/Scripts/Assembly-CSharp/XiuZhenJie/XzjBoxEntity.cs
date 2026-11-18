using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class XzjBoxEntity
	{
		public int BoxLocation;

		private List<BaseNaJieGoodEntity> m_GoodList;

		public int MaxCapacity;

		public int NowCapacity;

		public List<BaseNaJieGoodEntity> GoodsList
		{
			get
			{
				if (m_GoodList == null)
				{
					return new List<BaseNaJieGoodEntity>();
				}
				return new List<BaseNaJieGoodEntity>(m_GoodList);
			}
		}

		public XzjBoxEntity(int locationId)
		{
			BoxLocation = locationId;
		}

		public void GetAllGoods(UnityAction compeletAction, bool isShowWaiting = true)
		{
			SpaceGoodsReq_1004002 spaceGoodsReq_ = new SpaceGoodsReq_1004002();
			SpaceGoodsCheckReq_1004004 spaceGoodsCheckReq_ = new SpaceGoodsCheckReq_1004004();
			spaceGoodsReq_.Location = BoxLocation;
			spaceGoodsCheckReq_.Location = BoxLocation;
			if (m_GoodList == null)
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_NAJIE_SPACE, spaceGoodsReq_, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = _obj as ByteString;
					if (!(byteString == null))
					{
						SpaceGoodsResp_1004002 spaceGoodsResp_ = new SpaceGoodsResp_1004002();
						spaceGoodsResp_.MergeFrom(byteString);
						if (spaceGoodsResp_.IsSuccess)
						{
							MaxCapacity = spaceGoodsResp_.MaxCapacity;
							NowCapacity = spaceGoodsResp_.NowCapacity;
							if (spaceGoodsResp_.GoodsInfos != null)
							{
								m_GoodList = new List<BaseNaJieGoodEntity>();
								foreach (SpaceGoodsResp_1004002.Types.GoodsInfoResp_1004003 goodsInfo in spaceGoodsResp_.GoodsInfos)
								{
									m_GoodList.Add(new BaseNaJieGoodEntity
									{
										FiveElement = goodsInfo.FiveElement,
										GoodsId = goodsInfo.GoodsId,
										Level = goodsInfo.Level,
										Rank = goodsInfo.Rank,
										Type = goodsInfo.Type,
										IsBinding = goodsInfo.IsBinding,
										IsUsing = goodsInfo.IsUsing,
										Name = goodsInfo.Name,
										RecyclePrice = goodsInfo.RecyclePrice,
										TemplateId = goodsInfo.TemplateId,
										ConversionRate = goodsInfo.ConversionRate,
										GoodsVolume = goodsInfo.Volume,
										IsLock = goodsInfo.IsLock,
										GoodsNum = goodsInfo.Count
									});
								}
							}
							else
							{
								m_GoodList = null;
							}
							if (compeletAction != null)
							{
								compeletAction();
							}
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(spaceGoodsResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaiting);
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.CHECK_SPACE, spaceGoodsCheckReq_, (object _obj) =>
			{
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					SpaceGoodsCheckResp_1004004 spaceGoodsCheckResp_ = new SpaceGoodsCheckResp_1004004();
					spaceGoodsCheckResp_.MergeFrom(byteString);
					if (spaceGoodsCheckResp_.IsSuccess)
					{
						string idsMd = GetIdsMd5();
						if (!spaceGoodsCheckResp_.Md5.ToLower().Equals(idsMd))
						{
							m_GoodList = null;
							GetAllGoods(compeletAction);
						}
						else if (compeletAction != null)
						{
							compeletAction();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(spaceGoodsCheckResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		public List<BaseNaJieGoodEntity> SelectGoods(int type)
		{
			new List<BaseNaJieGoodEntity>();
			return GoodsList.FindAll((BaseNaJieGoodEntity info) => (info.Type == type) ? true : false);
		}

		public List<BaseNaJieGoodEntity> SelectGoods(int type, int rank)
		{
			new List<BaseNaJieGoodEntity>();
			return GoodsList.FindAll((BaseNaJieGoodEntity info) =>
			{
				int levelByRank = WorldSetting.GetLevelByRank(info.Rank);
				int levelByRank2 = WorldSetting.GetLevelByRank(rank);
				return (info.Type == type && levelByRank == levelByRank2) ? true : false;
			});
		}

		public List<BaseNaJieGoodEntity> SelectGoods(int type, int rank, int fiveElement)
		{
			new List<BaseNaJieGoodEntity>();
			return GoodsList.FindAll((BaseNaJieGoodEntity info) =>
			{
				int levelByRank = WorldSetting.GetLevelByRank(info.Rank);
				int levelByRank2 = WorldSetting.GetLevelByRank(rank);
				return (info.FiveElement == fiveElement && levelByRank == levelByRank2 && info.Type == type) ? true : false;
			});
		}

		public List<BaseNaJieGoodEntity> SelectGoods(int type, int fiveElement, int rank, int level)
		{
			new List<BaseNaJieGoodEntity>();
			return GoodsList.FindAll((BaseNaJieGoodEntity info) =>
			{
				int levelByRank = WorldSetting.GetLevelByRank(info.Rank);
				int levelByRank2 = WorldSetting.GetLevelByRank(rank);
				return (info.Type == type && info.FiveElement == fiveElement && levelByRank == levelByRank2 && info.Level == level) ? true : false;
			});
		}

		public BaseNaJieGoodEntity GetGoodsInfo(string goodsId)
		{
			if (m_GoodList == null)
			{
				return null;
			}
			foreach (BaseNaJieGoodEntity good in m_GoodList)
			{
				if (goodsId == good.GoodsId)
				{
					return good;
				}
			}
			return null;
		}

		public List<BaseNaJieGoodEntity> GetSameTemplateGoods(string templateId)
		{
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			foreach (BaseNaJieGoodEntity good in m_GoodList)
			{
				if (good.TemplateId == templateId)
				{
					list.Add(good);
				}
			}
			return list;
		}

		public void LockOneGoods(string goodsId, UnityAction compeleteAction)
		{
			BaseNaJieGoodEntity entity = GetGoodsInfo(goodsId);
			if (entity == null)
			{
				return;
			}
			GoodsLockReq_1004039 data = new GoodsLockReq_1004039
			{
				GoodsId = goodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_LOCK, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsLockResp_1004039 goodsLockResp_ = new GoodsLockResp_1004039();
					goodsLockResp_.MergeFrom(byteString);
					if (goodsLockResp_.IsSuccess)
					{
						entity.IsLock = true;
						MonoSingleton<GameStart>.Instance.PopUpAttention("锁定成功!");
						compeleteAction?.Invoke();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsLockResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void UnLockOneGoods(string goodsId, UnityAction compeleteAction)
		{
			BaseNaJieGoodEntity entity = GetGoodsInfo(goodsId);
			if (entity == null)
			{
				return;
			}
			GoodsUnLockReq_1004040 data = new GoodsUnLockReq_1004040
			{
				GoodsId = goodsId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_UNLOCK, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsUnLockResp_1004040 goodsUnLockResp_ = new GoodsUnLockResp_1004040();
					goodsUnLockResp_.MergeFrom(byteString);
					if (goodsUnLockResp_.IsSuccess)
					{
						entity.IsLock = false;
						MonoSingleton<GameStart>.Instance.PopUpAttention("解锁成功!");
						compeleteAction?.Invoke();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsUnLockResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void AddGoods(BaseNaJieGoodEntity entity)
		{
			if (entity == null || (double)NowCapacity + entity.GoodsVolume > (double)MaxCapacity)
			{
				return;
			}
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (goods.Type != 14 && goods.Type != 15 && goods.Type != 16 && goods.Type != 21 && goods.Type != 26 && goods.Type != 29 && goods.Type != 32 && entity.GoodsId == goods.GoodsId)
				{
					goods.GoodsNum += entity.GoodsNum;
					return;
				}
			}
			m_GoodList.Add(entity);
			NowCapacity += (int)entity.GoodsVolume;
		}

		public void AddGoodsByTempleteId(BaseNaJieGoodEntity entity)
		{
			if (entity == null || (double)NowCapacity + entity.GoodsVolume > (double)MaxCapacity)
			{
				return;
			}
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (goods.Type != 14 && goods.Type != 15 && goods.Type != 16 && goods.Type != 21 && goods.Type != 26 && goods.Type != 29 && goods.Type != 32)
				{
					string text = entity.TemplateId + entity.Type;
					string templateId = goods.TemplateId;
					int type = entity.Type;
					if (text == templateId + type)
					{
						goods.GoodsId = entity.GoodsId;
						goods.GoodsNum += entity.GoodsNum;
						NowCapacity += (int)entity.GoodsVolume;
						return;
					}
				}
			}
			m_GoodList.Add(entity);
			NowCapacity += (int)entity.GoodsVolume;
		}

		public BaseNaJieGoodEntity SelectGoodsByTempleteId(string templeteId)
		{
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (templeteId == goods.TemplateId)
				{
					return goods;
				}
			}
			return null;
		}

		public void RemoveGoods(string goodsId)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = null;
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (goodsId == goods.GoodsId)
				{
					baseNaJieGoodEntity = goods;
					break;
				}
			}
			if (baseNaJieGoodEntity != null)
			{
				m_GoodList.Remove(baseNaJieGoodEntity);
			}
		}

		public void RemoveGoods(string goodsId, int num)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = null;
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (goodsId == goods.GoodsId)
				{
					baseNaJieGoodEntity = goods;
					break;
				}
			}
			if (baseNaJieGoodEntity != null)
			{
				if (baseNaJieGoodEntity.GoodsNum <= num)
				{
					m_GoodList.Remove(baseNaJieGoodEntity);
				}
				else
				{
					baseNaJieGoodEntity.GoodsNum -= num;
				}
			}
		}

		public void RemoveGoodsByTempleteId(string templeteId, int num)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = null;
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (templeteId == goods.TemplateId)
				{
					baseNaJieGoodEntity = goods;
					break;
				}
			}
			if (baseNaJieGoodEntity != null)
			{
				if (baseNaJieGoodEntity.GoodsNum <= num)
				{
					m_GoodList.Remove(baseNaJieGoodEntity);
				}
				else
				{
					baseNaJieGoodEntity.GoodsNum -= num;
				}
			}
		}

		public void ResetGoodsNum(string goodsId, int num)
		{
			BaseNaJieGoodEntity baseNaJieGoodEntity = null;
			foreach (BaseNaJieGoodEntity goods in GoodsList)
			{
				if (goodsId == goods.GoodsId)
				{
					baseNaJieGoodEntity = goods;
					break;
				}
			}
			if (baseNaJieGoodEntity != null)
			{
				if (baseNaJieGoodEntity.GoodsNum != 0)
				{
					m_GoodList.Remove(baseNaJieGoodEntity);
				}
				else
				{
					baseNaJieGoodEntity.GoodsNum = num;
				}
			}
		}

		public string GetIdsMd5()
		{
			if (m_GoodList == null)
			{
				return null;
			}
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>(m_GoodList);
			list.Sort((BaseNaJieGoodEntity a, BaseNaJieGoodEntity b) => a.GoodsId.CompareTo(b.GoodsId));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (BaseNaJieGoodEntity item in list)
			{
				stringBuilder.Append(item.GoodsId + item.GoodsNum);
			}
			return CommonMethod.GetMD5WithString(stringBuilder.ToString().ToLower());
		}

		public void GetGoodsInfomation(BaseNaJieGoodEntity good, UnityAction<List<string>> compeleteCallBack)
		{
			if (good == null)
			{
				compeleteCallBack(new List<string>());
				return;
			}
			if (compeleteCallBack != null)
			{
				UnityAction<List<string>> unityAction = compeleteCallBack;
				compeleteCallBack = (List<string> strList) =>
				{
					unityAction(strList);
					good.ContentList = new List<string>(strList);
				};
			}
			XuanTianLouEntity xuanTianLouEntity = Singleton<FacadeManager>.Instance.SelectXuanTianLouGoodsInfoByTempleteId(good.TemplateId);
			string id = good.GoodsId;
			if (xuanTianLouEntity != null && xuanTianLouEntity.XTL_GoodsType != -1)
			{
				id = ((xuanTianLouEntity == null) ? good.GoodsId : good.TemplateId);
			}
			GetGoodsInfomation(id, good.Type, good.Level, good.Rank, good.FiveElement, good.RecyclePrice, compeleteCallBack);
		}

		public void GetGoodsInfomation(string id, int type, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			switch ((GoodsTypeEnum)type)
			{
			case GoodsTypeEnum.BOOK_ACHIEVEMENT:
				GetGongFaBookXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.BOOK_ELIXIR:
				GetDanFangBookXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.BOOK_MATRIX:
				GetZhenTuBookXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.BOOK_EQUIPMENT:
				GetQiTuBookXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.BOOK_SIGN:
				GetFuShuXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.BOOK_SKILL:
				GetFaShuBookXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.MATERIAL_ANIMAL:
			case GoodsTypeEnum.MATERIAL_ANIMAL_SKIN:
			case GoodsTypeEnum.MATERIAL_ANIMAL_BLOOD:
			case GoodsTypeEnum.MATERIAL_ANIMAL_BONE:
			case GoodsTypeEnum.MATERIAL_ANIMAL_PELLET:
				GetAnimalXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.MATERIAL_PLANT:
				GetZhiWuXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.MATERIAL_MINERAL:
				GetKuangWuXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.EQUIPMENT_ATTACK:
				GetCanConfigAttackFaQiXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.EQUIPMENT_DEFENSE:
			case GoodsTypeEnum.EQUIPMENT_DEFENSE_XIE:
				GetDefenceFaQiXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.ELIXIR:
				if (Singleton<FacadeManager>.Instance.SelectXuanTianLouGoodsInfoByTempleteId(id) == null)
				{
					GetDanYaoXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				}
				else
				{
					GetLingXuanTianLouGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				}
				break;
			case GoodsTypeEnum.MATRIX:
				GetZhenPanXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.SIGN:
				GetFuLuXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.EQUIPMENT_DEFENSE_APPAREL:
				GetCanConfigDefenceFaQiXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.SECREAT_AREA:
			case GoodsTypeEnum.GOODS_SECRET:
			case GoodsTypeEnum.INHERIT_SECRET:
				GetMiJingXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.GUILD_SECRET:
				GetFuDiQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.LingKui:
				GetLingXuanTianLouGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.YunTie:
				GetLingXuanTianLouGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.WuDaoShi:
				GetLingXuanTianLouGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.Dao_Jing_CanJuan:
				GetLingXuanTianLouGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.TiShenKuiLei:
				GetLingXuanTianLouGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.HaoYunFu:
				GetHaoYunFuXiangQing(id, level, rank, fiveElement, recyclePrice, compeleteCallBack);
				break;
			case GoodsTypeEnum.DE_JING:
				GetDeJingGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.XIAN_JIN:
				GetXianJinGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.SHEN_SHU:
				GetShenShuGoodsXiangQing(id, level, rank, fiveElement, compeleteCallBack);
				break;
			case GoodsTypeEnum.EQUIPMENT:
			case GoodsTypeEnum.BOOK_EQUIPMENT_ATTACK:
			case GoodsTypeEnum.BOOK_EQUIPMENT_DEFENSE:
			case GoodsTypeEnum.SIGN_PEN:
			case GoodsTypeEnum.SIGN_PAPER:
			case GoodsTypeEnum.SIGN_INK:
			case GoodsTypeEnum.TOOL:
			case (GoodsTypeEnum)27:
				break;
			}
		}

		private void GetKuangWuXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsMaterialMineralDetailReq_1004006 data = new GoodsMaterialMineralDetailReq_1004006
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_Material_Mineral_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsMaterialMineralDetailResp_1004006 goodsMaterialMineralDetailResp_ = new GoodsMaterialMineralDetailResp_1004006();
					goodsMaterialMineralDetailResp_.MergeFrom(byteString);
					if (goodsMaterialMineralDetailResp_ == null || !goodsMaterialMineralDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsMaterialMineralDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(stageByLevel), "材料的等级，等级越高的材料价值越大"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则材料的"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行"));
						m_DataList.Add(string.Format("{0},{1},{2}", "产地", goodsMaterialMineralDetailResp_.MapName, "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsMaterialMineralDetailResp_.ConversionRate, "灵衍值越高，则材料能转换为能量的比例越高，故灵衍值越高的材料用于炼丹、炼器、制符、制造阵盘、喂养灵兽、制造工具则效果越好"));
						m_DataList.Add(string.Format("{0},{1},{2}", "坚度", WorldSetting.JianDuName(goodsMaterialMineralDetailResp_.Intensity), "坚度即材料的硬度，越坚硬则越脆，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "韧度", WorldSetting.RenDuName(goodsMaterialMineralDetailResp_.Tenacity), "韧度级材料的柔韧性，越柔韧则可弯折的角度越大，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "导灵性", goodsMaterialMineralDetailResp_.Affinity, "神识附着于此材料时的效用，数值越大则导灵越强，需要消耗的神识越少，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "重量", goodsMaterialMineralDetailResp_.Weight, "材料的重量决定了起回收价格和能量值，能量值越高则炼制出来的物品效果越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsMaterialMineralDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1}", "单位", goodsMaterialMineralDetailResp_.Unit));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetZhiWuXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsMaterialPlantDetailReq_1004007 data = new GoodsMaterialPlantDetailReq_1004007
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_Material_Plant_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsMaterialPlantDetailResp_1004007 goodsMaterialPlantDetailResp_ = new GoodsMaterialPlantDetailResp_1004007();
					goodsMaterialPlantDetailResp_.MergeFrom(byteString);
					if (goodsMaterialPlantDetailResp_ == null || !goodsMaterialPlantDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsMaterialPlantDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(stageByLevel), "材料的等级，等级越高的材料价值越大"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则材料的"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行"));
						m_DataList.Add((goodsMaterialPlantDetailResp_.MapName != "") ? string.Format("{0},{1},{2}", "产地", goodsMaterialPlantDetailResp_.MapName, "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料") : string.Format("{0},{1},{2}", "产地", "虚神界", "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsMaterialPlantDetailResp_.ConversionRate, "灵衍值越高，则材料能转换为能量的比例越高，故灵衍值越高的材料用于炼丹、炼器、制符、制造阵盘、喂养灵兽、制造工具则效果越好"));
						m_DataList.Add(string.Format("{0},{1},{2}", "四气", goodsMaterialPlantDetailResp_.FourGas, "四气包括：温、热、寒、凉，是丹方的配伍中需要参考的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五味", goodsMaterialPlantDetailResp_.FiveFlavour, "五味包括：辛、甘、苦、咸、酸，是丹方的配伍中需要参考的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "补泻", goodsMaterialPlantDetailResp_.ReinforcingDiarrhoea, "补泻包括：补、泻，是丹方的配伍中需要参考的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "归经", goodsMaterialPlantDetailResp_.AffiliationAcupoint, "归经包括：手太阳、手少阳、手太阴、手少阴、手明阳、手厥阴、足太阳、足少阳、足太阴、足少阴、足阳明、足厥阴，是丹方的配伍中需要参考的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "坚度", WorldSetting.JianDuName(goodsMaterialPlantDetailResp_.Intensity), "坚度即材料的硬度，越坚硬则越脆，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "韧度", WorldSetting.RenDuName(goodsMaterialPlantDetailResp_.Tenacity), "韧度级材料的柔韧性，越柔韧则可弯折的角度越大，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "导灵性", goodsMaterialPlantDetailResp_.Affinity, "神识附着于此材料时的效用，数值越大则导灵越强，需要消耗的神识越少，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "重量", goodsMaterialPlantDetailResp_.Weight, "材料的重量决定了起回收价格和能量值，能量值越高则炼制出来的物品效果越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsMaterialPlantDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1}", "单位", goodsMaterialPlantDetailResp_.Unit));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetAnimalXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsMaterialAnimalDetailReq_1004005 data = new GoodsMaterialAnimalDetailReq_1004005
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_MATERIAL_ANIMAL_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsMaterialAnimalDetailResp_1004005 goodsMaterialAnimalDetailResp_ = new GoodsMaterialAnimalDetailResp_1004005();
					goodsMaterialAnimalDetailResp_.MergeFrom(byteString);
					if (goodsMaterialAnimalDetailResp_ == null || !goodsMaterialAnimalDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsMaterialAnimalDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(stageByLevel), "材料的等级，等级越高的材料价值越大"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则材料的"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行"));
						m_DataList.Add(string.Format("{0},{1},{2}", "产地", goodsMaterialAnimalDetailResp_.MapName, "如果材料的产地名称与界面名称相同，则为世界掉落材料，否则则是各地特有材料"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsMaterialAnimalDetailResp_.ConversionRate, "灵衍值越高，则材料能转换为能量的比例越高，故灵衍值越高的材料用于炼丹、炼器、制符、制造阵盘、喂养灵兽、制造工具则效果越好"));
						if (!string.IsNullOrEmpty(goodsMaterialAnimalDetailResp_.FourGas))
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "四气", goodsMaterialAnimalDetailResp_.FourGas, "四气包括：温、热、寒、凉，是丹方的配伍中需要参考的属性"));
						}
						if (!string.IsNullOrEmpty(goodsMaterialAnimalDetailResp_.FiveFlavour))
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "五味", goodsMaterialAnimalDetailResp_.FiveFlavour, "五味包括：辛、甘、苦、咸、酸，是丹方的配伍中需要参考的属性"));
						}
						if (!string.IsNullOrEmpty(goodsMaterialAnimalDetailResp_.ReinforcingDiarrhoea))
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "补泻", goodsMaterialAnimalDetailResp_.ReinforcingDiarrhoea, "补泻包括：补、泻，是丹方的配伍中需要参考的属性"));
						}
						if (!string.IsNullOrEmpty(goodsMaterialAnimalDetailResp_.AffiliationAcupoint))
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "归经", goodsMaterialAnimalDetailResp_.AffiliationAcupoint, "归经包括：手太阳、手少阳、手太阴、手少阴、手明阳、手厥阴、足太阳、足少阳、足太阴、足少阴、足阳明、足厥阴，是丹方的配伍中需要参考的属性"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "坚度", WorldSetting.JianDuName(goodsMaterialAnimalDetailResp_.Intensity), "坚度即材料的硬度，越坚硬则越脆，是炼器中需要使用的属性"));
						m_DataList.Add(string.Format("{0},{1},{2}", "韧度", WorldSetting.RenDuName(goodsMaterialAnimalDetailResp_.Tenacity), "韧度级材料的柔韧性，越柔韧则可弯折的角度越大，是炼器中需要使用的属性"));
						if (goodsMaterialAnimalDetailResp_.Affinity != 0.0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "导灵性", goodsMaterialAnimalDetailResp_.Affinity, "神识附着于此材料时的效用，数值越大则导灵越强，需要消耗的神识越少，是炼器中需要使用的属性"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "重量", goodsMaterialAnimalDetailResp_.Weight, "材料的重量决定了起回收价格和能量值，能量值越高则炼制出来的物品效果越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsMaterialAnimalDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1}", "单位", goodsMaterialAnimalDetailResp_.Unit));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetGongFaBookXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsBookAchievementDetailReq_1004008 data = new GoodsBookAchievementDetailReq_1004008
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_BOOK_ACHIEVEMENT_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBookAchievementDetailResp_1004008 goodsBookAchievementDetailResp_ = new GoodsBookAchievementDetailResp_1004008();
					goodsBookAchievementDetailResp_.MergeFrom(byteString.CreateCodedInput());
					if (goodsBookAchievementDetailResp_ == null || !goodsBookAchievementDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBookAchievementDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						switch ((GongFaType)goodsBookAchievementDetailResp_.ActionScope)
						{
						case GongFaType.None:
							m_DataList.Add(string.Format("{0},{1},{2}", "功法类型", "未知", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						case GongFaType.LianShen:
							m_DataList.Add(string.Format("{0},{1},{2}", "功法类型", "炼神", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						case GongFaType.DuanTi:
							m_DataList.Add(string.Format("{0},{1},{2}", "功法类型", "锻体", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						case GongFaType.NaQi:
							m_DataList.Add(string.Format("{0},{1},{2}", "功法类型", "纳气", "功法共分为纳气、锻体、炼神三种类型，纳气是提升境界的唯一手段，锻体功法增强体魄即防御能力，炼神功法则可以增强神识提高感知和操控能力"));
							break;
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "学习和使用此功法的最低等级，低于此等级将无法学习和使用此功法"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则效果越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，功法的五行如与灵根不符，则效率低下，请选择适合自身灵根的五行"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsBookAchievementDetailResp_.ConversionRate, "吸纳外界灵气转换为自身丹田内灵气的比值，灵衍值越高则转换效率越高，功法效果越强"));
						if (goodsBookAchievementDetailResp_.EffectType1 != 0)
						{
							string title = "";
							string gongFaEffectType = WorldSetting.GetGongFaEffectType(goodsBookAchievementDetailResp_.EffectType1, ref title);
							string text = goodsBookAchievementDetailResp_.EffectValue1 + "%";
							m_DataList.Add(string.Format("{0},{1}", title, gongFaEffectType + text, "功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。"));
						}
						if (goodsBookAchievementDetailResp_.EffectType2 != 0)
						{
							string title2 = "";
							string gongFaEffectType2 = WorldSetting.GetGongFaEffectType(goodsBookAchievementDetailResp_.EffectType2, ref title2);
							string text2 = goodsBookAchievementDetailResp_.EffectValue2 + "%";
							m_DataList.Add(string.Format("{0},{1}", title2, gongFaEffectType2 + text2, "功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。"));
						}
						if (goodsBookAchievementDetailResp_.EffectType3 != 0)
						{
							string title3 = "";
							string gongFaEffectType3 = WorldSetting.GetGongFaEffectType(goodsBookAchievementDetailResp_.EffectType3, ref title3);
							string text3 = goodsBookAchievementDetailResp_.EffectValue3 + "%";
							m_DataList.Add(string.Format("{0},{1}", title3, gongFaEffectType3 + text3, "功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsBookAchievementDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsBookAchievementDetailResp_.CreatedBy, "此功法由谁创造而来，开辟悟道台成为布道师，结丹期后在虚神界即可创造功法，创造一本自己署名的功法流芳百世"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于功法的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetFaShuTypeValue(int type, int continueTime, List<string> m_DataList)
		{
			if (m_DataList != null)
			{
				switch ((FaShuType)type)
				{
				case FaShuType.gongji_suoding:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-锁定攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.gongji_dingxiang:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-定向攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.gongji_chixu:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-持续攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					m_DataList.Add(string.Format("{0},{1},{2}", "持续时间", continueTime / 1000 + "息", "此法术持续存在的时间，拥有持续时间的法术包括：持续攻击、范围攻击、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.gongji_fanwei:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-范围攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.gongji_fanwei_chixu:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "攻击-范围持续攻击", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					m_DataList.Add(string.Format("{0},{1},{2}", "持续时间", continueTime / 1000 + "息", "此法术持续存在的时间，拥有持续时间的法术包括：持续攻击、范围攻击、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.fangyu_fashu_hudun:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "防御-法术护盾", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.zengyi_shanghai:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "增益-伤害增益", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.zengyi_fangyu:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "增益-防御增益", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.kongzhi_yisu:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "控制-减低移速", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.kongzhi_xingdong:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "控制-限制行动", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.dunshu_wuxing:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "遁术-五行遁术", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				case FaShuType.dunshu_yujian:
					m_DataList.Add(string.Format("{0},{1},{2}", "技能类型", "遁术-御剑术", "法术技能分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术"));
					break;
				}
			}
		}

		private void GetFaShuBookXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsBookSkillDetailReq_1004013 data = new GoodsBookSkillDetailReq_1004013
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_BOOK_SKILL_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBookSkillDetailResp_1004013 goodsBookSkillDetailResp_ = new GoodsBookSkillDetailResp_1004013();
					goodsBookSkillDetailResp_.MergeFrom(byteString);
					if (goodsBookSkillDetailResp_ == null || !goodsBookSkillDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBookSkillDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						GetFaShuTypeValue(goodsBookSkillDetailResp_.SkillType, goodsBookSkillDetailResp_.ContinueTime, m_DataList);
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "学习和使用此法术的最低等级，低于此等级将无法学习和使用此法术"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则威力越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法术的五行如与灵根不符，则效率低下，请选择适合自身灵根的五行"));
						m_DataList.Add(string.Format("{0},{1},{2}", "最低消耗", goodsBookSkillDetailResp_.MinSpiritualityValue, "使用此法术的最低要求，依据消耗值计算法术伤害，消耗值与功法当前重数、功法与灵根的匹配、功法的灵衍值、角色等级等等数据均有关系，法力消耗值=每息输出*施法时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "最大消耗", goodsBookSkillDetailResp_.MaxSpiritualityValue, "使用此法术的最高可用灵气，当法力消耗值超过最大则取最大消耗，消耗值与功法当前重数、功法与灵根的匹配、功法的灵衍值、角色等级等等数据均有关系，法力消耗值=每息输出*施法时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "施法时间", goodsBookSkillDetailResp_.CastTime / 1000 + "息", "使用此法术需要消耗的时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsBookSkillDetailResp_.ConversionRate, "将丹田内的灵气转换成法术能量的比例，越高则转换效率越高，法术的伤害越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsBookSkillDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsBookSkillDetailResp_.CreatedBy, "此法术由谁创造而来，开辟修法室成为术法师，结丹期后在虚神界即可创造法术，创造一本自己署名的大神通法术流芳百世"));
						m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于法术的优劣进行评估，直接出售给修真界商贩的价格"));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetDanFangBookXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsBookElixirDetailReq_1004009 data = new GoodsBookElixirDetailReq_1004009
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_BOOK_ELIXIR_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBookElixirDetailResp_1004009 goodsBookElixirDetailResp_ = new GoodsBookElixirDetailResp_1004009();
					goodsBookElixirDetailResp_.MergeFrom(byteString);
					if (goodsBookElixirDetailResp_ == null || !goodsBookElixirDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBookElixirDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						string effectString = GetEffectString(goodsBookElixirDetailResp_.EffectType);
						string[] array = effectString.Split(',');
						effectString = string.Format("{0},{1},{2}", array[0], array[1], "丹药共有三种大类：恢复类、增长类、特殊类，恢复类用于恢复渡劫损伤，增长类用于增长属性（有些需要炼化），特殊类则是由玄天楼出售的丹药");
						m_DataList.Add(effectString);
						m_DataList.Add(string.Format("{0},{1},{2}", "学习所需的丹师等级", goodsBookElixirDetailResp_.ElixirLevel, "每一阶丹方都需要对应的丹师等级，否则将难以学习或参悟"));
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "丹方的等级，1级对应练气期，11级对应筑基期，以此类推"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则丹药效果越好"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法术的五行如与灵根不符，则药效会降低80%，请选择适合自身灵根的五行"));
						if (level < 31)
						{
							if (goodsBookElixirDetailResp_.EffectType == 22)
							{
								m_DataList.Add(string.Format("{0},{1},{2}", "药效", goodsBookElixirDetailResp_.EffectValue, "此丹方所炼制出来的丹药能获得的最小药效值"));
							}
							m_DataList.Add(string.Format("{0},{1},{2}", "服用所需灵气值", goodsBookElixirDetailResp_.SpiritualityValue, "服用此丹药要求的最低灵气值，自身修为灵气值低于此值，将无法服用"));
						}
						else if (level >= 31 && level < 61)
						{
							if (goodsBookElixirDetailResp_.EffectType == 22)
							{
								m_DataList.Add(string.Format("{0},{1},{2}", "药效", goodsBookElixirDetailResp_.EffectValue / 100, "此丹方所炼制出来的丹药能获得的最小药效值"));
							}
							m_DataList.Add(string.Format("{0},{1},{2}", "服用所需真气值", goodsBookElixirDetailResp_.SpiritualityValue / 100, "服用此丹药要求的最低真气值，自身修为灵气值低于此值，将无法服用"));
						}
						else if (level >= 61)
						{
							if (goodsBookElixirDetailResp_.EffectType == 22)
							{
								m_DataList.Add(string.Format("{0},{1},{2}", "药效", goodsBookElixirDetailResp_.EffectValue / 100 / 100, "此丹方所炼制出来的丹药能获得的最小药效值"));
							}
							m_DataList.Add(string.Format("{0},{1},{2}", "服用所需真元值", goodsBookElixirDetailResp_.SpiritualityValue / 100 / 100, "服用此丹药要求的最低真元值，自身修为灵气值低于此值，将无法服用"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(goodsBookElixirDetailResp_.Rank), "此丹方所炼制出来的丹药能获得的最小品阶"));
						m_DataList.Add(string.Format("{0},{1},{2}", "丹毒", goodsBookElixirDetailResp_.PoisonValue, "此丹方所炼制出来的丹药能获得的最小丹毒"));
						m_DataList.Add(string.Format("{0},{1},{2}", "炼制步骤数", goodsBookElixirDetailResp_.StepCount, "炼制此丹药一共所需的步骤数，等阶越高的丹药炼制步骤越繁琐"));
						for (int i = 0; i < goodsBookElixirDetailResp_.MaterialNames.Count; i++)
						{
							m_DataList.Add(string.Format("{0},{1}", "材料" + (i + 1), goodsBookElixirDetailResp_.MaterialNames[i]));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsBookElixirDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsBookElixirDetailResp_.CreatedBy, "此丹药由谁创造而来，开辟炼丹房成为丹师，炼制丹药即可使用或者售卖，结丹期后进入虚神界可以创造丹方"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetQiTuBookXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> detail = new List<string>();
			GoodsBookEquipmentDetailReq_1004011 data = new GoodsBookEquipmentDetailReq_1004011
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_BOOK_EQUIPMENT_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBookEquipmentDetailResp_1004011 goodsBookEquipmentDetailResp_ = new GoodsBookEquipmentDetailResp_1004011();
					goodsBookEquipmentDetailResp_.MergeFrom(byteString);
					if (goodsBookEquipmentDetailResp_ == null || !goodsBookEquipmentDetailResp_.IsSuccess)
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBookEquipmentDetailResp_.Code);
						if (compeleteCallBack != null)
						{
							compeleteCallBack(detail);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					else
					{
						int stageByLevel = WorldSetting.GetStageByLevel(level);
						detail.Add("类型," + GetQiTuTypeName(goodsBookEquipmentDetailResp_.Type));
						detail.Add("等阶," + WorldSetting.GetStageChineseName(stageByLevel) + ",法器的等级，限制了穿戴的最低等级");
						detail.Add("品阶," + WorldSetting.GetRankAllName(rank) + ",基于灵衍值给定的品级，品阶越高则法器造成的伤害越高");
						detail.Add("五行," + WorldSetting.GetWuXingName((WuXing)fiveElement) + ",分金、木、水、火、土五行，法器的五行如与灵根不符，则伤害会降低80%，请选择适合自身灵根的五行");
						detail.Add("可炼制最小品阶," + WorldSetting.GetRankAllName(goodsBookEquipmentDetailResp_.MinRank) + ",该器图可炼制出法器的最小品阶");
						detail.Add("可炼制最大品阶," + WorldSetting.GetRankAllName(goodsBookEquipmentDetailResp_.MaxRank) + ",该器图可炼制出法器的最大品阶");
						if (goodsBookEquipmentDetailResp_.Type == 16 || goodsBookEquipmentDetailResp_.Type == 29)
						{
							detail.Add(string.Format("占用体魄,{0},{1}", goodsBookEquipmentDetailResp_.UsedPhysique, "穿戴此法器需要占用的体魄值，占用的体魄值越高则防御力越强"));
						}
						else if (goodsBookEquipmentDetailResp_.Type == 15 || goodsBookEquipmentDetailResp_.Type == 26)
						{
							detail.Add(string.Format("占用神识,{0},{1}", goodsBookEquipmentDetailResp_.UsedPerception, "使用此法器时需要的神识，当神识不足时无法使用此法器，配置合理的神识占用可以在战斗中激活多件法器，攻击力会得到大幅提升"));
							if (stageByLevel > 6)
							{
								detail.Add(string.Format("激发真元值,{0},{1}", goodsBookEquipmentDetailResp_.MinSpiritualityValue / 100 / 100, "激活此法器需要的真元值，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
								detail.Add(string.Format("最大真元值,{0},{1}", goodsBookEquipmentDetailResp_.MaxSpiritualityValue / 100 / 100, "此法器可以承受的最大真元值上限，若注入法器的真元值超过此数值则以此数值为准，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
							}
							else if (stageByLevel > 3)
							{
								detail.Add(string.Format("激发真气值,{0},{1}", goodsBookEquipmentDetailResp_.MinSpiritualityValue / 100, "激活此法器需要的真气值，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
								detail.Add(string.Format("最大真气值,{0},{1}", goodsBookEquipmentDetailResp_.MaxSpiritualityValue / 100, "此法器可以承受的最大真气值上限，若注入法器的真气值超过此数值则以此数值为准，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
							}
							else if (stageByLevel > 0)
							{
								detail.Add(string.Format("激发灵气值,{0},{1}", goodsBookEquipmentDetailResp_.MinSpiritualityValue, "激活此法器需要的灵气值，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
								detail.Add(string.Format("最大灵气值,{0},{1}", goodsBookEquipmentDetailResp_.MaxSpiritualityValue, "此法器可以承受的最大灵气值上限，若注入法器的灵气值超过此数值则以此数值为准，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
							}
						}
						for (int i = 0; i < goodsBookEquipmentDetailResp_.MaterialNames.Count; i++)
						{
							detail.Add("材料" + (i + 1) + "," + goodsBookEquipmentDetailResp_.MaterialNames[i]);
						}
						detail.Add(string.Format("{0},{1}", "体积", goodsBookEquipmentDetailResp_.Volume));
						detail.Add(string.Format("{0},{1}", "创造人", goodsBookEquipmentDetailResp_.CreatedBy));
						if (recyclePrice > 0)
						{
							detail.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(detail);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private string GetQiTuTypeName(int type)
		{
			return type switch
			{
				15 => "可配置攻击法器", 
				16 => "可穿戴防御法器", 
				26 => "可配置防御法器", 
				29 => "靴", 
				_ => "无", 
			};
		}

		private void GetZhenTuBookXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsBookMatrixDetailReq_1004044 data = new GoodsBookMatrixDetailReq_1004044
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_ARTICLE_BOOK_MATRIX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBookMatrixDetailResp_1004044 goodsBookMatrixDetailResp_ = new GoodsBookMatrixDetailResp_1004044();
					goodsBookMatrixDetailResp_.MergeFrom(byteString);
					if (goodsBookMatrixDetailResp_ == null || !goodsBookMatrixDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBookMatrixDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1},{2}", "类型", GetZhenPanTypeName((BattleZhenFaType)goodsBookMatrixDetailResp_.Type), "学习和使用此法术的最低等级，低于此等级将无法学习和使用此法术"));
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "学习和使用此法术的最低等级，低于此等级将无法学习和使用此法术"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则威力越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法术的五行如与灵根不符，则效率低下，请选择适合自身灵根的五行"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵石消耗", goodsBookMatrixDetailResp_.UnitConsume, "使用此法术需要消耗的时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "持续时间", goodsBookMatrixDetailResp_.Duration / 1000 + "息", "使用此法术需要消耗的时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsBookMatrixDetailResp_.ConversionRate, "将丹田内的灵气转换成法术能量的比例，越高则转换效率越高，法术的伤害越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsBookMatrixDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsBookMatrixDetailResp_.CreatedBy, "此法术由谁创造而来，开辟修法室成为术法师，结丹期后在虚神界即可创造法术，创造一本自己署名的大神通法术流芳百世"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于法术的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetZhenPanXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleMatrixReq_1004043 data = new GoodsArticleMatrixReq_1004043
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_ARTICLE_MATRIX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleMatrixResp_1004043 goodsArticleMatrixResp_ = new GoodsArticleMatrixResp_1004043();
					goodsArticleMatrixResp_.MergeFrom(byteString);
					if (goodsArticleMatrixResp_ == null || !goodsArticleMatrixResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleMatrixResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1},{2}", "类型", GetZhenPanTypeName((BattleZhenFaType)goodsArticleMatrixResp_.Type), "阵法包括4种类型：缚灵阵（降低妖兽移速且无法逃脱）、杀阵（定时掉血）、幻阵（妖兽攻击会产生未命中）、回复阵法（回复法力或回复防御）。"));
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "高等阶的阵盘对低等阶战斗中，有额外加成作用，而低等级阵盘在高等阶战斗中，会被压制。"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则阵法的效果越好"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，对阵中对不同五行的妖兽会有克制或被克制效果"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵石消耗", goodsArticleMatrixResp_.UnitConsume, "放置阵盘会消耗灵石，等阶越高品阶越高的阵盘，消耗的灵石越多"));
						m_DataList.Add(string.Format("{0},{1},{2}", "持续时间", goodsArticleMatrixResp_.Duration / 1000 + "息", "阵盘在战斗中可以持续的时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsArticleMatrixResp_.ConversionRate, "灵气注入阵盘后，灵气转换为最终效果的比例，越高则转换效率越高，造成的最终效果越好"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleMatrixResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsArticleMatrixResp_.CreatedBy, "此阵盘由谁创造而来，开辟阵法室成为阵法大师，炼制阵法即可使用或者售卖，结丹期后进入虚神界可以创造阵法"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于阵法的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private string GetZhenPanTypeName(BattleZhenFaType battleZhenFaType)
		{
			return battleZhenFaType switch
			{
				BattleZhenFaType.Kill => "杀阵", 
				BattleZhenFaType.Kun => "缚灵阵", 
				BattleZhenFaType.Huan => "幻阵", 
				BattleZhenFaType.AddSuperAir => "复灵阵", 
				BattleZhenFaType.AddTiPo => "活血阵", 
				_ => "", 
			};
		}

		private string GetZhenPanTypeName(BattleZhenFaType battleZhenFaType, double value)
		{
			switch (battleZhenFaType)
			{
			case BattleZhenFaType.Kill:
				return "每息造成伤害：" + value.ToString("0");
			case BattleZhenFaType.Kun:
				return "无法逃离并减速：" + value.ToString("0");
			case BattleZhenFaType.Huan:
				return string.Format("{0}% 躲闪攻击", value.ToString("0"));
			case BattleZhenFaType.AddSuperAir:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 60)
				{
					return string.Format("每息真元恢复：{0}", (value / 100.0 / 100.0).ToString("0"));
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel > 30)
				{
					return string.Format("每息真气恢复：{0}", (value / 100.0).ToString("0"));
				}
				return string.Format("每息灵气恢复：{0}", value.ToString("0"));
			case BattleZhenFaType.AddTiPo:
				return string.Format("每息体魄恢复{0}：", value.ToString("0"));
			default:
				return "";
			}
		}

		private void GetFuShuXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsBookSignDetailReq_1004012 data = new GoodsBookSignDetailReq_1004012
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_BOOK_SIGN_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsBookSignDetailResp_1004012 goodsBookSignDetailResp_ = new GoodsBookSignDetailResp_1004012();
					goodsBookSignDetailResp_.MergeFrom(byteString);
					if (goodsBookSignDetailResp_ == null || !goodsBookSignDetailResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsBookSignDetailResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						GetFaShuTypeValue(goodsBookSignDetailResp_.SkillType, goodsBookSignDetailResp_.ContinueTime, m_DataList);
						if (m_DataList.Count > 0)
						{
							string[] array = m_DataList[0].Split(',');
							if (array.Length == 3)
							{
								array[2] = "符箓是通过封印法术而来，其类型与法术相同，分为：锁定攻击、持续攻击、范围攻击、法术护盾、伤害增益、防御增益、减低移速、限制行动、五行遁术、御剑术";
								m_DataList[0] = $"{array[0]},{array[1]},{array[2]}";
							}
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "学习和使用此符箓的最低等级，低于此等级将无法学习和使用此符箓"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则威力越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，符箓的五行与灵根无需匹配，所以符箓是增强自身战斗力的有效手段"));
						m_DataList.Add(string.Format("{0},{1},{2}", "施法消耗", goodsBookSignDetailResp_.SpiritualityValue, "使用此符箓需要消耗的灵气值，如法力值消耗无法达到要求则无法使用符箓，法力消耗值=每息输出*施法时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsBookSignDetailResp_.ConversionRate, "符箓释放所封印的威能值转换为法术能量的比例，越高则转换效率越高，释放的能量越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "施放时间", goodsBookSignDetailResp_.CastTime / 1000 + "息", "使用此符箓需要消耗的时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsBookSignDetailResp_.CreatedBy, "此符箓由谁创造而来，开辟画符静室成为符师，炼制符箓即可使用或者售卖，结丹期后进入虚神界可以创造符箓"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此符箓的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积", goodsBookSignDetailResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetDefenceFaQiXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleDefenseEquipmentReq_1004014 data = new GoodsArticleDefenseEquipmentReq_1004014
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_DEFENSE_EQUIPMENT_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleDefenseEquipmentResp_1004014 goodsArticleDefenseEquipmentResp_ = new GoodsArticleDefenseEquipmentResp_1004014();
					goodsArticleDefenseEquipmentResp_.MergeFrom(byteString);
					if (goodsArticleDefenseEquipmentResp_ == null || !goodsArticleDefenseEquipmentResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleDefenseEquipmentResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "法器的等级，限制了穿戴的最低等级"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则法器造成的伤害越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法器的五行如与灵根不符，则伤害会降低80%，请选择适合自身灵根的五行"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsArticleDefenseEquipmentResp_.ConversionRate, "灵气注入法器后，灵气转换为最终伤害或防御的比例，越高则转换效率越高，造成的伤害或形成的防护越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "占用体魄", goodsArticleDefenseEquipmentResp_.UsedPhysique, "穿戴此法器需要占用的体魄值，占用的体魄值越高则防御力越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "坚韧", goodsArticleDefenseEquipmentResp_.ToughTenacity, "法器的坚硬和柔韧程度，这个数值在可穿戴防具中是提供大量防御值的基础，在攻击性或防御型法器中对法器伤害或抵御伤害均有小幅度提升"));
						if (goodsArticleDefenseEquipmentResp_.EffectType1 != 0)
						{
							string title = "";
							string text = WorldSetting.FaQiXiLianType(goodsArticleDefenseEquipmentResp_.EffectType1, ref title);
							m_DataList.Add(title + "," + text + goodsArticleDefenseEquipmentResp_.EffectValue1 + "%");
						}
						if (goodsArticleDefenseEquipmentResp_.EffectType2 != 0)
						{
							string title2 = "";
							string text2 = WorldSetting.FaQiXiLianType(goodsArticleDefenseEquipmentResp_.EffectType2, ref title2);
							m_DataList.Add(title2 + "," + text2 + goodsArticleDefenseEquipmentResp_.EffectValue2 + "%");
						}
						if (goodsArticleDefenseEquipmentResp_.EffectType3 != 0)
						{
							string title3 = "";
							string text3 = WorldSetting.FaQiXiLianType(goodsArticleDefenseEquipmentResp_.EffectType3, ref title3);
							m_DataList.Add(title3 + "," + text3 + goodsArticleDefenseEquipmentResp_.EffectValue3 + "%");
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleDefenseEquipmentResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创建人", goodsArticleDefenseEquipmentResp_.CreatedBy, "此法器由谁创造而来，开辟炼器室成为炼器大师，炼制法器即可使用或者售卖，结丹期后进入虚神界可以创造法器"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetCanConfigDefenceFaQiXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleSetableDefenseEquipmentReq_1004019 data = new GoodsArticleSetableDefenseEquipmentReq_1004019
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_SETABLE_DEFENSE_EQUIPMENT_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleSetableDefenseEquipmentResp_1004019 goodsArticleSetableDefenseEquipmentResp_ = new GoodsArticleSetableDefenseEquipmentResp_1004019();
					goodsArticleSetableDefenseEquipmentResp_.MergeFrom(byteString);
					if (goodsArticleSetableDefenseEquipmentResp_ == null || !goodsArticleSetableDefenseEquipmentResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleSetableDefenseEquipmentResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "法器的等级，限制了穿戴的最低等级"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则法器造成的伤害越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法器的五行如与灵根不符，则伤害会降低80%，请选择适合自身灵根的五行"));
						if (level < 31)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "激发灵气值", goodsArticleSetableDefenseEquipmentResp_.MinSpiritualityValue, "激活此法器需要的灵气值，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
							m_DataList.Add(string.Format("{0},{1},{2}", "可承受灵气值", goodsArticleSetableDefenseEquipmentResp_.MaxSpiritualityValue, "此法器可以承受的最大灵气值上限，若注入法器的灵气值超过此数值则以此数值为准，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
						}
						else if (level >= 31 && level < 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "激发真气值", goodsArticleSetableDefenseEquipmentResp_.MinSpiritualityValue / 100, "激活此法器需要的真气值，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
							m_DataList.Add(string.Format("{0},{1},{2}", "可承受真气值", goodsArticleSetableDefenseEquipmentResp_.MaxSpiritualityValue / 100, "此法器可以承受的最大真气值上限，若注入法器的真气值超过此数值则以此数值为准，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
						}
						else if (level >= 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "激发真元值", goodsArticleSetableDefenseEquipmentResp_.MinSpiritualityValue / 100 / 100, "激活此法器需要的真元值，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
							m_DataList.Add(string.Format("{0},{1},{2}", "可承受真元值", goodsArticleSetableDefenseEquipmentResp_.MaxSpiritualityValue / 100 / 100, "此法器可以承受的最大真元值上限，若注入法器的真元值超过此数值则以此数值为准，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsArticleSetableDefenseEquipmentResp_.ConversionRate, "灵气注入法器后，灵气转换为最终伤害或防御的比例，越高则转换效率越高，造成的伤害或形成的防护越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "神识占用", goodsArticleSetableDefenseEquipmentResp_.UsedPerception, "使用此法器时需要的神识，当神识不足时无法使用此法器，配置合理的神识占用可以在战斗中激活多件法器，攻击力会得到大幅提升"));
						m_DataList.Add(string.Format("{0},{1},{2}", "坚韧", goodsArticleSetableDefenseEquipmentResp_.ToughTenacity, "法器的坚硬和柔韧程度，这个数值在可穿戴防具中是提供大量防御值的基础，在攻击性或防御型法器中对法器伤害或抵御伤害均有小幅度提升"));
						if (goodsArticleSetableDefenseEquipmentResp_.EffectType1 != 0)
						{
							string title = "";
							string text = WorldSetting.FaQiXiLianType(goodsArticleSetableDefenseEquipmentResp_.EffectType1, ref title);
							m_DataList.Add(title + "," + text + goodsArticleSetableDefenseEquipmentResp_.EffectValue1 + "%,功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。");
						}
						if (goodsArticleSetableDefenseEquipmentResp_.EffectType2 != 0)
						{
							string title2 = "";
							string text2 = WorldSetting.FaQiXiLianType(goodsArticleSetableDefenseEquipmentResp_.EffectType2, ref title2);
							m_DataList.Add(title2 + "," + text2 + goodsArticleSetableDefenseEquipmentResp_.EffectValue2 + "%,功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。");
						}
						if (goodsArticleSetableDefenseEquipmentResp_.EffectType3 != 0)
						{
							string title3 = "";
							string text3 = WorldSetting.FaQiXiLianType(goodsArticleSetableDefenseEquipmentResp_.EffectType3, ref title3);
							m_DataList.Add(title3 + "," + text3 + goodsArticleSetableDefenseEquipmentResp_.EffectValue3 + "%,功法第三重后方可激活特效，玄阶功法拥有1个特效，地阶2个，天阶3个。");
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleSetableDefenseEquipmentResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创建人", goodsArticleSetableDefenseEquipmentResp_.CreatedBy, "此法器由谁创造而来，开辟炼器室成为炼器大师，炼制法器即可使用或者售卖，结丹期后进入虚神界可以创造法器"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetCanConfigAttackFaQiXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleSetableAttackEquipmentReq_1004018 data = new GoodsArticleSetableAttackEquipmentReq_1004018
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_SETABLE_ATTACK_EQUIPMENT_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleSetableAttackEquipmentResp_1004018 goodsArticleSetableAttackEquipmentResp_ = new GoodsArticleSetableAttackEquipmentResp_1004018();
					goodsArticleSetableAttackEquipmentResp_.MergeFrom(byteString);
					if (goodsArticleSetableAttackEquipmentResp_ == null || !goodsArticleSetableAttackEquipmentResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleSetableAttackEquipmentResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "法器的等级，限制了穿戴的最低等级"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则法器造成的伤害越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法器的五行如与灵根不符，则伤害会降低80%，请选择适合自身灵根的五行"));
						if (level < 31)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "激发灵气值", goodsArticleSetableAttackEquipmentResp_.MinSpiritualityValue, "激活此法器需要的灵气值，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
							m_DataList.Add(string.Format("{0},{1},{2}", "可承受灵气值", goodsArticleSetableAttackEquipmentResp_.MaxSpiritualityValue, "此法器可以承受的最大灵气值上限，若注入法器的灵气值超过此数值则以此数值为准，该灵气值可以查看装备界面中的“每息输出”乘以施法时间"));
						}
						else if (level >= 31 && level < 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "激发真气值", goodsArticleSetableAttackEquipmentResp_.MinSpiritualityValue / 100, "激活此法器需要的真气值，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
							m_DataList.Add(string.Format("{0},{1},{2}", "可承受真气值", goodsArticleSetableAttackEquipmentResp_.MaxSpiritualityValue / 100, "此法器可以承受的最大真气值上限，若注入法器的真气值超过此数值则以此数值为准，该真气值可以查看装备界面中的“每息输出”乘以施法时间"));
						}
						else if (level >= 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "激发真元值", goodsArticleSetableAttackEquipmentResp_.MinSpiritualityValue / 100 / 100, "激活此法器需要的真元值，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
							m_DataList.Add(string.Format("{0},{1},{2}", "可承受真元值", goodsArticleSetableAttackEquipmentResp_.MaxSpiritualityValue / 100 / 100, "此法器可以承受的最大真元值上限，若注入法器的真元值超过此数值则以此数值为准，该真元值可以查看装备界面中的“每息输出”乘以施法时间"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsArticleSetableAttackEquipmentResp_.ConversionRate, "灵气注入法器后，灵气转换为最终伤害或防御的比例，越高则转换效率越高，造成的伤害或形成的防护越高"));
						m_DataList.Add(string.Format("{0},{1},{2}", "神识占用", goodsArticleSetableAttackEquipmentResp_.UsedPerception, "使用此法器时需要的神识，当神识不足时无法使用此法器，配置合理的神识占用可以在战斗中激活多件法器，攻击力会得到大幅提升"));
						m_DataList.Add(string.Format("{0},{1},{2}", "坚韧", goodsArticleSetableAttackEquipmentResp_.ToughTenacity, "法器的坚硬和柔韧程度，这个数值在可穿戴防具中是提供大量防御值的基础，在攻击性或防御型法器中对法器伤害或抵御伤害均有小幅度提升"));
						m_DataList.Add(string.Format("{0},{1},{2}", "攻击间隔", ((float)goodsArticleSetableAttackEquipmentResp_.CoolDown / 1000f).ToString("0.00") + "息", "法器聚集灵气再次攻击所需的时间，不包含经脉、特效等加成。"));
						if (goodsArticleSetableAttackEquipmentResp_.EffectType1 != 0)
						{
							string title = "";
							string text = WorldSetting.FaQiXiLianType(goodsArticleSetableAttackEquipmentResp_.EffectType1, ref title);
							m_DataList.Add(title + "," + text + goodsArticleSetableAttackEquipmentResp_.EffectValue1 + "%,玄阶法器拥有1个特效，地阶2个，天阶3个。");
						}
						if (goodsArticleSetableAttackEquipmentResp_.EffectType2 != 0)
						{
							string title2 = "";
							string text2 = WorldSetting.FaQiXiLianType(goodsArticleSetableAttackEquipmentResp_.EffectType2, ref title2);
							m_DataList.Add(title2 + "," + text2 + goodsArticleSetableAttackEquipmentResp_.EffectValue2 + "%,玄阶法器拥有1个特效，地阶2个，天阶3个。");
						}
						if (goodsArticleSetableAttackEquipmentResp_.EffectType3 != 0)
						{
							string title3 = "";
							string text3 = WorldSetting.FaQiXiLianType(goodsArticleSetableAttackEquipmentResp_.EffectType3, ref title3);
							m_DataList.Add(title3 + "," + text3 + goodsArticleSetableAttackEquipmentResp_.EffectValue3 + "%,玄阶法器拥有1个特效，地阶2个，天阶3个。");
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleSetableAttackEquipmentResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创建人", goodsArticleSetableAttackEquipmentResp_.CreatedBy, "此法器由谁创造而来，开辟炼器室成为炼器大师，炼制法器即可使用或者售卖，结丹期后进入虚神界可以创造法器"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private string GetFaQiEffectXiangQing(int Type)
		{
			return (EquipmentEffectTypeEnum)Type switch
			{
				EquipmentEffectTypeEnum.JIN_ATTACK_HARM_15 => "增加金系伤害15%", 
				EquipmentEffectTypeEnum.MU_ATTACK_HARM_15 => "增加木系伤害15%", 
				EquipmentEffectTypeEnum.SHUI_ATTACK_HARM_15 => "增加水系伤害15%", 
				EquipmentEffectTypeEnum.HUO_ATTACK_HARM_15 => "增加火系伤害15%", 
				EquipmentEffectTypeEnum.TU_ATTACK_HARM_15 => "增加土系伤害15%", 
				EquipmentEffectTypeEnum.JIN_ATTACK_HARM_25 => "增加金系伤害25%", 
				EquipmentEffectTypeEnum.MU_ATTACK_HARM_25 => "增加木系伤害25%", 
				EquipmentEffectTypeEnum.SHUI_ATTACK_HARM_25 => "增加水系伤害25%", 
				EquipmentEffectTypeEnum.HUO_ATTACK_HARM_25 => "增加火系伤害25%", 
				EquipmentEffectTypeEnum.TU_ATTACK_HARM_25 => "增加土系伤害25%", 
				EquipmentEffectTypeEnum.TU_ATTACK_SPEED_15 => "增加攻击速度15%", 
				EquipmentEffectTypeEnum.TU_ATTACK_SPEED_25 => "增加攻击速度25%", 
				EquipmentEffectTypeEnum.JIN_ENTRENCH_15 => "增加金系防护15%", 
				EquipmentEffectTypeEnum.JIN_ENTRENCH_25 => "增加金系防护25%", 
				EquipmentEffectTypeEnum.MU_ENTRENCH_15 => "增加木系防护15%", 
				EquipmentEffectTypeEnum.MU_ENTRENCH_25 => "增加木系防护25%", 
				EquipmentEffectTypeEnum.SHUI_ENTRENCH_15 => "增加水系防护15%", 
				EquipmentEffectTypeEnum.SHUI_ENTRENCH_25 => "增加水系防护25%", 
				EquipmentEffectTypeEnum.HUO_ENTRENCH_15 => "增加火系防护15%", 
				EquipmentEffectTypeEnum.HUO_ENTRENCH_25 => "增加火系防护25%", 
				EquipmentEffectTypeEnum.TU_ENTRENCH_15 => "增加土系防护15%", 
				EquipmentEffectTypeEnum.TU_ENTRENCH_25 => "增加土系防护25%", 
				EquipmentEffectTypeEnum.HUI_SPEED_25 => "增加真元恢复速度25%", 
				EquipmentEffectTypeEnum.HUI_SPEED_40 => "增加真元恢复速度40%", 
				EquipmentEffectTypeEnum.JI_XING_15 => "疾行 增加移动速度15%", 
				EquipmentEffectTypeEnum.FENG_XING_25 => "风行 增加移动速度25%", 
				EquipmentEffectTypeEnum.NONE => "无", 
				_ => "无", 
			};
		}

		private void GetMiJingXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleSecretAreaReq_1004017 data = new GoodsArticleSecretAreaReq_1004017
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_SECRET_AREA_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleSecretAreaResp_1004017 goodsArticleSecretAreaResp_ = new GoodsArticleSecretAreaResp_1004017();
					goodsArticleSecretAreaResp_.MergeFrom(byteString);
					if (goodsArticleSecretAreaResp_ == null || !goodsArticleSecretAreaResp_.IsSuccess)
					{
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
					else
					{
						if (goodsArticleSecretAreaResp_.AreaType == 62)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "秘境类型", "藏宝秘境", "此秘境地图的类型，包括藏宝秘境和传承秘境，藏宝秘境中有大量的妖兽和材料，传承秘境中则有大量的人形生物"));
						}
						else if (goodsArticleSecretAreaResp_.AreaType == 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "秘境类型", "传承秘境", "此秘境地图的类型，包括藏宝秘境和传承秘境，藏宝秘境中有大量的妖兽和材料，传承秘境中则有大量的人形生物"));
						}
						m_DataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level))));
						m_DataList.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(rank)));
						m_DataList.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement)));
						m_DataList.Add(string.Format("{0},{1}", "归属地图", goodsArticleSecretAreaResp_.MapName));
						m_DataList.Add(string.Format("{0},{1}", "地图五行", WorldSetting.GetWuXingName((WuXing)goodsArticleSecretAreaResp_.MapFiveElement)));
						m_DataList.Add(string.Format("{0},{1}", "最小进入等级", goodsArticleSecretAreaResp_.MinLevel));
						m_DataList.Add(string.Format("{0},{1}", "最大进入等级", goodsArticleSecretAreaResp_.MaxLevel));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleSecretAreaResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1}", "回收价格", recyclePrice));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetFuDiQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsGuildMapDetailsReq_1004038 data = new GoodsGuildMapDetailsReq_1004038
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GOODS_GUILDMAP_DETAILS, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsGuildMapDetailsReap_1004038 goodsGuildMapDetailsReap_ = new GoodsGuildMapDetailsReap_1004038();
					goodsGuildMapDetailsReap_.MergeFrom(byteString);
					if (goodsGuildMapDetailsReap_ == null || !goodsGuildMapDetailsReap_.IsSuccess)
					{
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1}", "所属大陆", goodsGuildMapDetailsReap_.DaLuName));
						m_DataList.Add(string.Format("{0},{1}", "所属洲", goodsGuildMapDetailsReap_.ZhouName));
						m_DataList.Add(string.Format("{0},{1}", "所属地", goodsGuildMapDetailsReap_.YeWaiName));
						m_DataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level))));
						m_DataList.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(rank)));
						m_DataList.Add(string.Format("{0},{1}", "灵峰数量", goodsGuildMapDetailsReap_.HillCount));
						m_DataList.Add(string.Format("{0},{1}", "灵石矿脉", goodsGuildMapDetailsReap_.StoneCount));
						m_DataList.Add(string.Format("{0},{1}", "灵玉矿脉", goodsGuildMapDetailsReap_.JadeCount));
						m_DataList.Add(string.Format("{0},{1}", "妖兽盘踞", goodsGuildMapDetailsReap_.MonsterCount));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1}", "回收价格", recyclePrice));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetDanYaoXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleElixirReq_1004015 data = new GoodsArticleElixirReq_1004015
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_ELIXIR_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleElixirResp_1004015 goodsArticleElixirResp_ = new GoodsArticleElixirResp_1004015();
					goodsArticleElixirResp_.MergeFrom(byteString);
					if (goodsArticleElixirResp_ == null || !goodsArticleElixirResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleElixirResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						string text = GetEffectString(goodsArticleElixirResp_.EffectType);
						string[] array = text.Split(',');
						if (array.Length >= 2)
						{
							text = string.Format("{0},{1},{2}", array[0], array[1], "丹药共有三种大类：恢复类、增长类、特殊类，恢复类用于恢复渡劫损伤，增长类用于增长属性（有些需要炼化），特殊类则是由玄天楼出售的丹药");
						}
						else
						{
							Debug.Log(array);
						}
						m_DataList.Add(text);
						string effectStringValue = GetEffectStringValue("", goodsArticleElixirResp_.EffectType, goodsArticleElixirResp_.EffectValue.ToString());
						if (!string.IsNullOrEmpty(effectStringValue))
						{
							m_DataList.Add(effectStringValue);
						}
						string effectStringValue2 = GetEffectStringValue("（原）", goodsArticleElixirResp_.EffectType, goodsArticleElixirResp_.RealityEffectValue.ToString());
						if (!string.IsNullOrEmpty(effectStringValue2))
						{
							m_DataList.Add(effectStringValue2);
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "已服用次数", goodsArticleElixirResp_.EatTime, "每一种丹药使用后都会产生耐药性，每一个类型的耐药性均有区别，当产生耐药性后药效会进行递减，恢复类丹药无耐药性，增长类丹药中需炼化的每多服用一颗降低20%药效，增长寿元、精血、经脉的丹药仅第一颗有效"));
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "丹药的等级，1级对应练气期，11级对应筑基期，以此类推"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则丹药效果越好"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，法术的五行如与灵根不符，则药效会降低80%，请选择适合自身灵根的五行"));
						if (level < 31)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "服用要求(灵气)", goodsArticleElixirResp_.SpiritualityValue, "服用此丹药要求的最低灵气值，自身修为灵气值低于此值，将无法服用"));
						}
						else if (level >= 31 && level < 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "服用要求(真气)", goodsArticleElixirResp_.SpiritualityValue / 100, "服用此丹药要求的最低真气值，自身修为灵气值低于此值，将无法服用"));
						}
						else if (level >= 61)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "服用要求(真元)", goodsArticleElixirResp_.SpiritualityValue / 100 / 100, "服用此丹药要求的最低真元值，自身修为灵气值低于此值，将无法服用"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsArticleElixirResp_.ConversionRate, "丹药中的药效转换为能量的比例，越高则转换效率越高，获得的药效越强，已计算进丹药的药效中"));
						m_DataList.Add(string.Format("{0},{1},{2}", "丹毒", goodsArticleElixirResp_.PoisonValue, "服用此丹药后，会在体内积累丹毒，丹毒会影响修行的效率，清除丹毒需要至玄天楼购买清毒丹"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleElixirResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsArticleElixirResp_.CreatedBy, "此丹方由谁创造而来，开辟炼丹房成为丹师，炼制丹药即可使用或者售卖，结丹期后进入虚神界可以创造丹方"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此物品的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetDanFang(int goodsType, int goodsLevel, List<string> dataList)
		{
			switch ((DanYaoEffect)goodsType)
			{
			case DanYaoEffect.None:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "未知"));
				break;
			case DanYaoEffect.HUIFU_TIPO:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-体魄"));
				break;
			case DanYaoEffect.HUIFU_SHENSHI:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-神识"));
				break;
			case DanYaoEffect.HUIFU_ZHENYUAN:
				if (goodsLevel < 31)
				{
					dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-灵气"));
				}
				else if (goodsLevel >= 31 && goodsLevel < 61)
				{
					dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-真气"));
				}
				else if (goodsLevel >= 61)
				{
					dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-真元"));
				}
				break;
			case DanYaoEffect.HUIFU_JINGXUE:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-精血"));
				break;
			case DanYaoEffect.HUIFU_SHOUYUAN:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "恢复类-寿元"));
				break;
			case DanYaoEffect.ZENGZHANG_TIPO:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-体魄"));
				break;
			case DanYaoEffect.ZENGZHANG_SHENSHI:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-神识"));
				break;
			case DanYaoEffect.ZENGZHANG_ZHENYUAN:
				if (goodsLevel < 31)
				{
					dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-灵气"));
				}
				else if (goodsLevel >= 31 && goodsLevel < 61)
				{
					dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-真气"));
				}
				else if (goodsLevel >= 61)
				{
					dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-真元"));
				}
				break;
			case DanYaoEffect.ZENGZHANG_JINGXUE:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-精血"));
				break;
			case DanYaoEffect.ZENGZHANG_SHOUYUAN:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-寿元"));
				break;
			case DanYaoEffect.ZENGZHANG_JINGMAI:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "增长类-经脉"));
				break;
			case DanYaoEffect.TESHU_JINGLI:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-增长精力"));
				break;
			case DanYaoEffect.TESHU_WUXING:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-增长悟性"));
				break;
			case DanYaoEffect.TESHU_JIEDU:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-解毒"));
				break;
			case DanYaoEffect.TESHU_JINGJIETUPO_ZHUJI:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-提升筑基成功率"));
				break;
			case DanYaoEffect.TESHU_JINGJIETUPO_LEIJIE:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-降低雷劫伤害"));
				break;
			case DanYaoEffect.TESHU_LINGGEN:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-补一种五行灵根"));
				break;
			case DanYaoEffect.TESHU_JINGMAI:
				dataList.Add(string.Format("{0},{1}", "丹药类型", "特殊类-修补受损经脉"));
				break;
			case (DanYaoEffect)1:
			case (DanYaoEffect)2:
			case (DanYaoEffect)3:
			case (DanYaoEffect)4:
			case (DanYaoEffect)5:
			case (DanYaoEffect)6:
			case (DanYaoEffect)7:
			case (DanYaoEffect)8:
			case (DanYaoEffect)9:
			case (DanYaoEffect)15:
			case (DanYaoEffect)16:
			case (DanYaoEffect)17:
			case (DanYaoEffect)18:
			case (DanYaoEffect)19:
			case (DanYaoEffect)26:
			case (DanYaoEffect)27:
			case (DanYaoEffect)28:
			case (DanYaoEffect)29:
				break;
			}
		}

		private string GetEffectString(int effectType)
		{
			switch ((DanYaoEffect)effectType)
			{
			case DanYaoEffect.None:
				return string.Format("{0},{1}", "丹药类型", "未知");
			case DanYaoEffect.HUIFU_TIPO:
				return string.Format("{0},{1}", "丹药类型", "恢复类-体魄");
			case DanYaoEffect.HUIFU_SHENSHI:
				return string.Format("{0},{1}", "丹药类型", "恢复类-神识");
			case DanYaoEffect.HUIFU_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return string.Format("{0},{1}", "丹药类型", "恢复类-灵气");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					return string.Format("{0},{1}", "丹药类型", "恢复类-真气");
				}
				return string.Format("{0},{1}", "丹药类型", "恢复类-真元");
			case DanYaoEffect.HUIFU_JINGXUE:
				return string.Format("{0},{1}", "丹药类型", "恢复类-精血");
			case DanYaoEffect.HUIFU_SHOUYUAN:
				return string.Format("{0},{1}", "丹药类型", "恢复类-寿元");
			case DanYaoEffect.ZENGZHANG_TIPO:
				return string.Format("{0},{1}", "丹药类型", "增长类-体魄");
			case DanYaoEffect.ZENGZHANG_SHENSHI:
				return string.Format("{0},{1}", "丹药类型", "增长类-神识");
			case DanYaoEffect.ZENGZHANG_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return string.Format("{0},{1}", "丹药类型", "增长类-灵气");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
				{
					return string.Format("{0},{1}", "丹药类型", "增长类-真气");
				}
				return string.Format("{0},{1}", "丹药类型", "增长类-真元");
			case DanYaoEffect.ZENGZHANG_JINGXUE:
				return string.Format("{0},{1}", "丹药类型", "增长类-精血");
			case DanYaoEffect.ZENGZHANG_SHOUYUAN:
				return string.Format("{0},{1}", "丹药类型", "增长类-寿元");
			case DanYaoEffect.ZENGZHANG_JINGMAI:
				return string.Format("{0},{1}", "丹药类型", "增长类-经脉");
			case DanYaoEffect.TESHU_JINGLI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-增长精力");
			case DanYaoEffect.TESHU_WUXING:
				return string.Format("{0},{1}", "丹药类型", "特殊类-增长悟性");
			case DanYaoEffect.TESHU_JIEDU:
				return string.Format("{0},{1}", "丹药类型", "特殊类-解毒");
			case DanYaoEffect.TESHU_JINGJIETUPO_ZHUJI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-提升筑基成功率");
			case DanYaoEffect.TESHU_JINGJIETUPO_LEIJIE:
				return string.Format("{0},{1}", "丹药类型", "特殊类-降低雷劫伤害");
			case DanYaoEffect.TESHU_LINGGEN:
				return string.Format("{0},{1}", "丹药类型", "特殊类-补一种五行灵根");
			case DanYaoEffect.TESHU_JINGMAI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-修补受损经脉");
			case DanYaoEffect.TESHU_HUNLI:
				return string.Format("{0},{1}", "丹药类型", "特殊类-增长魂力");
			default:
				return "";
			}
		}

		private string GetEffectStringValue(string title, int effectType, string value)
		{
			switch ((DanYaoEffect)effectType)
			{
			case DanYaoEffect.None:
				return string.Format("{0},{1}", "未知", "未知");
			case DanYaoEffect.HUIFU_TIPO:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_SHENSHI:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31)
				{
					_ = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
					_ = 61;
					return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_JINGXUE:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.HUIFU_SHOUYUAN:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_TIPO:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_SHENSHI:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_ZHENYUAN:
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
				{
					return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31)
				{
					_ = Singleton<RoleManager>.Instance.Role_Entity.RoleLevel;
					_ = 61;
					return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
				}
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_JINGXUE:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_SHOUYUAN:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.ZENGZHANG_JINGMAI:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGLI:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_WUXING:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JIEDU:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGJIETUPO_ZHUJI:
				return title + string.Format("{0},{1},{2}", "药效值", float.Parse(value) * 100f + "%", "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGJIETUPO_LEIJIE:
				return title + string.Format("{0},{1},{2}", "药效值", float.Parse(value) * 100f + "%", "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_LINGGEN:
				return title + string.Format("{0},{1},{2}", "药效值", WorldSetting.GetWuXingName((WuXing)int.Parse(value)), "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			case DanYaoEffect.TESHU_JINGMAI:
				return "";
			case DanYaoEffect.TESHU_HUNLI:
				return title + string.Format("{0},{1},{2}", "药效值", value, "服用此丹药后产生的效果，除增长纳气、增长神识、增长体魄这三类丹药需要炼化（修行效果翻倍）外，其余的丹药将直接产生效果");
			default:
				return "";
			}
		}

		public void GetDanYaoTimesXiangQing(string id, int level, int rank, int fiveElement, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleElixirChangeReq_1004016 data = new GoodsArticleElixirChangeReq_1004016
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_ELIXIR_CHANGE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleElixirChangeResp_1004016 goodsArticleElixirChangeResp_ = new GoodsArticleElixirChangeResp_1004016();
					goodsArticleElixirChangeResp_.MergeFrom(byteString);
					if (goodsArticleElixirChangeResp_ == null || !goodsArticleElixirChangeResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleElixirChangeResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						string effectStringValue = GetEffectStringValue("", goodsArticleElixirChangeResp_.Effect, goodsArticleElixirChangeResp_.EffectValue.ToString());
						m_DataList.Add(effectStringValue);
						m_DataList.Add(string.Format("{0},{1},{2}", "已服用次数", goodsArticleElixirChangeResp_.EatTime, "每一种丹药使用后都会产生耐药性，每一个类型的耐药性均有区别，当产生耐药性后药效会进行递减，恢复类丹药无耐药性，增长类丹药中需炼化的每多服用一颗降低20%药效，增长寿元、精血、经脉的丹药仅第一颗有效"));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetFuLuXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleSignReq_1004020 data = new GoodsArticleSignReq_1004020
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_SIGN_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleSignResp_1004020 goodsArticleSignResp_ = new GoodsArticleSignResp_1004020();
					goodsArticleSignResp_.MergeFrom(byteString);
					if (goodsArticleSignResp_ == null || !goodsArticleSignResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleSignResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						GetFaShuTypeValue(goodsArticleSignResp_.SkillType, goodsArticleSignResp_.ContinueTime, m_DataList);
						m_DataList.Add(string.Format("{0},{1},{2}", "剩余使用", goodsArticleSignResp_.UseTime, "每次使用后都会降低回收价格，使用次数为0后将无法继续使用"));
						m_DataList.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "学习和使用此符箓的最低等级，低于此等级将无法学习和使用此符箓"));
						m_DataList.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则威力越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，符箓的五行与灵根无需匹配，所以符箓是增强自身战斗力的有效手段"));
						m_DataList.Add(string.Format("{0},{1},{2}", "施法消耗", goodsArticleSignResp_.SpiritualityValue, "使用此符箓需要消耗的灵气值，如法力值消耗无法达到要求则无法使用符箓，法力消耗值=每息输出*施法时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "威能值", goodsArticleSignResp_.CalSpiritualityValue, "此符箓炼制时，封印的威能大小，威能值越高则效果越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "灵衍值", goodsArticleSignResp_.ConversionRate, "符箓释放所封印的威能值转换为法术能量的比例，越高则转换效率越高，释放的能量越强"));
						m_DataList.Add(string.Format("{0},{1},{2}", "施放时间", goodsArticleSignResp_.CastTime / 1000 + "息", "使用此符箓需要消耗的时间"));
						m_DataList.Add(string.Format("{0},{1},{2}", "体积（立方尺）", goodsArticleSignResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						m_DataList.Add(string.Format("{0},{1},{2}", "创造人", goodsArticleSignResp_.CreatedBy, "此符箓由谁创造而来，开辟画符静室成为符师，炼制符箓即可使用或者售卖，结丹期后进入虚神界可以创造符箓"));
						if (recyclePrice > 0)
						{
							m_DataList.Add(string.Format("{0},{1},{2}", "回收价格", recyclePrice, "基于此符箓的优劣进行评估，直接出售给修真界商贩的价格"));
						}
						m_DataList.Add(string.Format("{0},{1},{2}", "体积", goodsArticleSignResp_.Volume, "在纳戒中占用的体积大小，单位是立方尺"));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetHaoYunFuXiangQing(string id, int level, int rank, int fiveElement, int recyclePrice, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> list = new List<string>();
			list.Add(string.Format("{0},{1}", "效果", "增加气运2000"));
			list.Add(string.Format("{0},{1}", "持续时间", "60分钟"));
			list.Add(string.Format("{0},{1},{2}", "等阶", WorldSetting.GetStageChineseName(WorldSetting.GetStageByLevel(level)), "学习和使用此符箓的最低等级，低于此等级将无法学习和使用此符箓"));
			list.Add(string.Format("{0},{1},{2}", "品阶", WorldSetting.GetRankAllName(rank), "基于灵衍值给定的品级，品阶越高则威力越强"));
			list.Add(string.Format("{0},{1},{2}", "五行", WorldSetting.GetWuXingName((WuXing)fiveElement), "分金、木、水、火、土五行，符箓的五行与灵根无需匹配，所以符箓是增强自身战斗力的有效手段"));
			list.Add(string.Format("{0},{1},{2}", "体积（立方尺）", 0.1f, "在纳戒中占用的体积大小，单位是立方尺"));
			list.Add(string.Format("{0},{1},{2}", "回收价格", 20, "基于此符箓的优劣进行评估，直接出售给修真界商贩的价格"));
			compeleteCallBack?.Invoke(list);
		}

		public void GetFuLuTimesXiangQing(string id, int level, int rank, int fiveElement, UnityAction<List<string>> compeleteCallBack)
		{
			List<string> m_DataList = new List<string>();
			GoodsArticleSignUseTimeReq_1004021 data = new GoodsArticleSignUseTimeReq_1004021
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GET_ARTICLE_SIGN_USE_TIME, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GoodsArticleSignUseTimeResp_1004021 goodsArticleSignUseTimeResp_ = new GoodsArticleSignUseTimeResp_1004021();
					goodsArticleSignUseTimeResp_.MergeFrom(byteString);
					if (goodsArticleSignUseTimeResp_ == null || !goodsArticleSignUseTimeResp_.IsSuccess)
					{
						string textValue = string.Format(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(goodsArticleSignUseTimeResp_.Code));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
					}
					else
					{
						m_DataList.Add(string.Format("{0},{1},{2}", "剩余使用", goodsArticleSignUseTimeResp_.UseTime, "每次使用后都会降低回收价格，使用次数为0后将无法继续使用"));
						if (compeleteCallBack != null)
						{
							compeleteCallBack(m_DataList);
						}
					}
				}
			}, isShowWaitUI: true);
		}

		public void GetLingXuanTianLouGoodsXiangQing(string id, int level, int rank, int fiveElement, UnityAction<List<string>> compeleteCallBack)
		{
			XuanTianLouEntity xuanTianLouEntity = Singleton<FacadeManager>.Instance.SelectXuanTianLouGoodsInfoByTempleteId(id);
			List<string> arg = new List<string>();
			if (xuanTianLouEntity == null)
			{
				compeleteCallBack(arg);
				return;
			}
			arg = new List<string>(xuanTianLouEntity.GoodsDetail);
			int stageByLevel = WorldSetting.GetStageByLevel(level);
			string item = string.Format("{0},{1}", "品阶", WorldSetting.GetRankAllName(rank));
			string item2 = string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(stageByLevel));
			arg.Insert(0, item2);
			arg.Insert(0, item);
			compeleteCallBack(arg);
		}

		public void GetDeJingGoodsXiangQing(string id, int level, int rank, int fiveElement, UnityAction<List<string>> compeleteCallBack)
		{
			List<XuanTianLouEntity> list = Singleton<FacadeManager>.Instance.SelectZongMeCangBaoGeGoodsByTypeEntities(39);
			List<string> arg = new List<string>();
			if (list == null)
			{
				compeleteCallBack?.Invoke(arg);
				return;
			}
			XuanTianLouEntity xuanTianLouEntity = null;
			int stageByLevel = WorldSetting.GetStageByLevel(level);
			foreach (XuanTianLouEntity item in list)
			{
				if (item.Stage == stageByLevel)
				{
					xuanTianLouEntity = item;
				}
			}
			if (xuanTianLouEntity == null)
			{
				compeleteCallBack?.Invoke(arg);
				return;
			}
			arg = new List<string>(xuanTianLouEntity.GoodsDetail);
			compeleteCallBack?.Invoke(arg);
		}

		public void GetXianJinGoodsXiangQing(string id, int level, int rank, int fiveElement, UnityAction<List<string>> compeleteCallBack)
		{
			List<XuanTianLouEntity> list = Singleton<FacadeManager>.Instance.SelectZongMeCangBaoGeGoodsByTypeEntities(40);
			List<string> arg = new List<string>();
			if (list == null)
			{
				compeleteCallBack?.Invoke(arg);
				return;
			}
			XuanTianLouEntity xuanTianLouEntity = null;
			int stageByLevel = WorldSetting.GetStageByLevel(level);
			foreach (XuanTianLouEntity item in list)
			{
				if (item.Stage == stageByLevel)
				{
					xuanTianLouEntity = item;
				}
			}
			if (xuanTianLouEntity == null)
			{
				compeleteCallBack?.Invoke(arg);
				return;
			}
			arg = new List<string>(xuanTianLouEntity.GoodsDetail);
			compeleteCallBack?.Invoke(arg);
		}

		public void GetShenShuGoodsXiangQing(string id, int level, int rank, int fiveElement, UnityAction<List<string>> compeleteCallBack)
		{
			List<XuanTianLouEntity> list = Singleton<FacadeManager>.Instance.SelectZongMeCangBaoGeGoodsByTypeEntities(41);
			List<string> arg = new List<string>();
			if (list == null)
			{
				compeleteCallBack?.Invoke(arg);
				return;
			}
			XuanTianLouEntity xuanTianLouEntity = null;
			int stageByLevel = WorldSetting.GetStageByLevel(level);
			foreach (XuanTianLouEntity item in list)
			{
				if (item.Stage == stageByLevel)
				{
					xuanTianLouEntity = item;
				}
			}
			if (xuanTianLouEntity == null)
			{
				compeleteCallBack?.Invoke(arg);
				return;
			}
			arg = new List<string>(xuanTianLouEntity.GoodsDetail);
			compeleteCallBack?.Invoke(arg);
		}
	}
}
