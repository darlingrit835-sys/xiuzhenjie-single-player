using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class XuanTianLouWnd : Window
	{
		public XuanTianLouPanel Panel;

		private int m_CurrentTypeIndex;

		private XuanTianLouGoodsType xuanTianLouGoodsType;

		private GameObject m_InstanceItemObj;

		private GameObject m_InstanceTypeObj;

		private GridLayoutGroup m_Glgroup;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, XuanTianLouScrollItem> m_XuanTianLouScrollDic;

		private Dictionary<int, XuanTianLouEntity> m_GoodsDataInfo;

		private Dictionary<int, List<XuanTianLouEntity>> m_GoodsTypeDic;

		private List<XuanTianLouEntity> m_HotGoods;

		private int m_MaxSpeicalBuyTimes = 10;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<XuanTianLouPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddToggleClickListener(Panel.LeiXingYi, Type_OneClick);
			AddToggleClickListener(Panel.LeiXingEr, Type_TwoClick);
			Init(paralist);
			SetMaxSpeicalBuyTimes();
			ResourceLoad();
			GetSpecialData();
		}

		private void SetMaxSpeicalBuyTimes()
		{
			int vipLevel = Singleton<RoleManager>.Instance.Role_Entity.VipLevel;
			if (vipLevel >= 12)
			{
				m_MaxSpeicalBuyTimes += 10;
			}
			else if (vipLevel >= 10)
			{
				m_MaxSpeicalBuyTimes += 9;
			}
			else if (vipLevel >= 9)
			{
				m_MaxSpeicalBuyTimes += 8;
			}
			else if (vipLevel >= 8)
			{
				m_MaxSpeicalBuyTimes += 7;
			}
			else if (vipLevel >= 7)
			{
				m_MaxSpeicalBuyTimes += 6;
			}
			else if (vipLevel >= 4)
			{
				m_MaxSpeicalBuyTimes += 5;
			}
			else if (vipLevel >= 3)
			{
				m_MaxSpeicalBuyTimes += 4;
			}
			else if (vipLevel >= 2)
			{
				m_MaxSpeicalBuyTimes += 3;
			}
		}

		public void Init(params object[] paralist)
		{
			m_GoodsTypeDic = new Dictionary<int, List<XuanTianLouEntity>>();
			m_XuanTianLouScrollDic = new Dictionary<int, XuanTianLouScrollItem>();
			m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuanTianLouItem.prefab");
			m_InstanceTypeObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuanTianLouScrollItem.prefab");
			m_Glgroup = Panel.ItemListContent.GetComponent<GridLayoutGroup>();
			m_HotGoods = new List<XuanTianLouEntity>();
			m_CurrentTypeIndex = 2;
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			m_GoodsDataInfo = Singleton<FacadeManager>.Instance.SelectXuanTianLouGoodsInfo();
			foreach (KeyValuePair<int, XuanTianLouEntity> item in m_GoodsDataInfo)
			{
				int key = item.Key;
				if (m_GoodsDataInfo.ContainsKey(key) && m_GoodsDataInfo[key].XTL_GoodsType != -1)
				{
					if (!m_GoodsTypeDic.ContainsKey(m_GoodsDataInfo[key].XTL_GoodsType))
					{
						m_GoodsTypeDic.Add(m_GoodsDataInfo[key].XTL_GoodsType, new List<XuanTianLouEntity> { m_GoodsDataInfo[key] });
					}
					else
					{
						m_GoodsTypeDic[m_GoodsDataInfo[key].XTL_GoodsType].Add(m_GoodsDataInfo[key]);
					}
				}
			}
		}

		private void reflashData(bool isClearParent)
		{
			ClearData(isClearParent);
			if (isClearParent)
			{
				m_XuanTianLouScrollDic.Clear();
			}
			if (m_HotGoods != null && m_HotGoods.Count > 0 && m_CurrentTypeIndex == 2)
			{
				CreateScrollItems(isHot: true, m_HotGoods);
			}
			foreach (KeyValuePair<int, List<XuanTianLouEntity>> item in m_GoodsTypeDic)
			{
				if ((m_CurrentTypeIndex == 1 && item.Value[0].BuyType == 1) || (m_CurrentTypeIndex == 2 && item.Value[0].BuyType == 2))
				{
					CreateScrollItems(isHot: false, item.Value);
				}
			}
			float num = 0f;
			foreach (XuanTianLouScrollItem value in m_XuanTianLouScrollDic.Values)
			{
				num += value.GetContentHight();
			}
			if (m_HotGoods != null && m_HotGoods.Count > 0 && m_CurrentTypeIndex == 2)
			{
				num += 88f;
			}
			Panel.ItemListContent.sizeDelta = new Vector2(Panel.ItemListContent.sizeDelta.x, num);
		}

		private void CreateScrollItems(bool isHot, List<XuanTianLouEntity> dataSource)
		{
			if (dataSource == null || dataSource.Count == 0)
			{
				return;
			}
			string titleName = dataSource[0].TypeName;
			if (isHot)
			{
				titleName = "热销商品";
			}
			GameObject gameObject = SetListContentParent(Panel.ItemListContent, m_InstanceTypeObj);
			XuanTianLouScrollItem component = gameObject.GetComponent<XuanTianLouScrollItem>();
			component.SetValue(isHot, titleName, this);
			foreach (XuanTianLouEntity item in dataSource)
			{
				CreateScrollItems(isHot, gameObject, item, component);
			}
			if (!isHot && !m_XuanTianLouScrollDic.ContainsKey(dataSource[0].XTL_GoodsType))
			{
				m_XuanTianLouScrollDic.Add(dataSource[0].XTL_GoodsType, component);
			}
		}

		private void CreateScrollItems(bool isHot, GameObject gameObj, XuanTianLouEntity child, XuanTianLouScrollItem dataItem)
		{
			XuanTianLouItem component = SetListContentParent(gameObj.GetComponent<RectTransform>(), m_InstanceItemObj).GetComponent<XuanTianLouItem>();
			Sprite value = null;
			Sprite value2 = null;
			int levelByRank = WorldSetting.GetLevelByRank(child.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out value);
			m_GoodsIconSprite.TryGetValue(child.GoodsType, out value2);
			component.SetValue(child, isHot, child.GoodsPrice, (ZhiFuType)child.BuyType, child.Name, child.FiveElement, child.Level, value, value2, ClickGoods);
			dataItem.AddItem(component);
		}

		private void GetSpecialData()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_XUANTIANLOU, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterXuanTianLouResp_1004033 enterXuanTianLouResp_ = new EnterXuanTianLouResp_1004033();
					enterXuanTianLouResp_.MergeFrom(byteString);
					if (enterXuanTianLouResp_.IsSuccess)
					{
						List<XuanTianLouEntity> value = null;
						if (m_GoodsTypeDic.TryGetValue(4, out value))
						{
							for (int i = 0; i < enterXuanTianLouResp_.Goods.Count; i++)
							{
								int id = enterXuanTianLouResp_.Goods[i].Id;
								XuanTianLouEntity xuanTianLouEntity = null;
								for (int j = 0; j < value.Count; j++)
								{
									if (value[j].Id == id)
									{
										value[j].GoodsPrice = enterXuanTianLouResp_.Goods[i].SystemPrice;
										if (enterXuanTianLouResp_.Goods[i].Count >= m_MaxSpeicalBuyTimes)
										{
											xuanTianLouEntity = value[j];
										}
										break;
									}
								}
								if (xuanTianLouEntity != null)
								{
									value.Remove(xuanTianLouEntity);
									if (value.Count == 0)
									{
										m_GoodsTypeDic.Remove(4);
									}
								}
							}
						}
						m_HotGoods.Clear();
						if (enterXuanTianLouResp_.ReXiaoGoodsId != null && enterXuanTianLouResp_.ReXiaoGoodsId.Count > 0)
						{
							for (int k = 0; k < enterXuanTianLouResp_.ReXiaoGoodsId.Count && k < 4; k++)
							{
								XuanTianLouEntity item = m_GoodsDataInfo[enterXuanTianLouResp_.ReXiaoGoodsId[k]];
								m_HotGoods.Add(item);
							}
						}
						reflashData(isClearParent: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterXuanTianLouResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ClickGoods(object dataSource, GameObject itemObj)
		{
			XuanTianLouEntity xuanTianLouEntity = dataSource as XuanTianLouEntity;
			Singleton<UIManager>.Instance.PopUpWnd("XuanTianLouXiangQingWnd.prefab", true, xuanTianLouEntity);
		}

		public void BuySuccess(XuanTianLouEntity entity)
		{
			GetSpecialData();
		}

		private void Type_OneClick(bool isOn)
		{
			if (isOn && m_CurrentTypeIndex == 1)
			{
				m_CurrentTypeIndex = 2;
				reflashData(isClearParent: true);
				Panel.ItemListContent.anchoredPosition = Vector2.zero;
			}
		}

		private void Type_TwoClick(bool isOn)
		{
			if (isOn && m_CurrentTypeIndex == 2)
			{
				m_CurrentTypeIndex = 1;
				reflashData(isClearParent: true);
				Panel.ItemListContent.anchoredPosition = Vector2.zero;
			}
		}

		private void ClearData(bool isClearParent)
		{
			if (isClearParent)
			{
				foreach (Transform item in Panel.ItemListContent)
				{
					Object.Destroy(item.gameObject);
				}
				return;
			}
			if (m_XuanTianLouScrollDic == null)
			{
				return;
			}
			foreach (KeyValuePair<int, XuanTianLouScrollItem> item2 in m_XuanTianLouScrollDic)
			{
				item2.Value.ClearData();
			}
		}
	}
}
