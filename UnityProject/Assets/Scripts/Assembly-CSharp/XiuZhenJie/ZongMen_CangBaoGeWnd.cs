using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_CangBaoGeWnd : Window
	{
		public ZongMen_CangBaoGePanel Panel;

		private GameObject m_InstanceItemObj;

		private GameObject m_InstanceTypeObj;

		private GridLayoutGroup m_Glgroup;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, List<XuanTianLouEntity>> m_DataSource;

		private int m_GongXianDian;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_CangBaoGePanel>();
			m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuanTianLouItem.prefab");
			m_InstanceTypeObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuanTianLouScrollItem.prefab");
			m_DataSource = Singleton<FacadeManager>.Instance.SelectZongMeCangBaoGeGoodsEntities();
			m_Glgroup = Panel.Content.GetComponent<GridLayoutGroup>();
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(Panel.WenHaoBtn, WenHaoTogClick);
			CreateData();
			if (paralist.Length != 0)
			{
				m_GongXianDian = int.Parse(paralist[0].ToString());
			}
			Panel.GongXianDianText.text = m_GongXianDian.ToString();
		}

		private void CreateData()
		{
			foreach (KeyValuePair<int, List<XuanTianLouEntity>> item in m_DataSource)
			{
				CreateScrollItems(item.Value);
			}
		}

		private void CreateScrollItems(List<XuanTianLouEntity> dataSource)
		{
			if (dataSource == null || dataSource.Count == 0)
			{
				return;
			}
			string typeName = dataSource[0].TypeName;
			GameObject gameObject = SetListContentParent(Panel.Content, m_InstanceTypeObj);
			XuanTianLouScrollItem component = gameObject.GetComponent<XuanTianLouScrollItem>();
			component.SetValue(isHot: false, typeName, this);
			foreach (XuanTianLouEntity item in dataSource)
			{
				CreateScrollItem(gameObject, item, component);
			}
		}

		private void CreateScrollItem(GameObject gameObj, XuanTianLouEntity child, XuanTianLouScrollItem dataItem)
		{
			XuanTianLouItem component = SetListContentParent(gameObj.GetComponent<RectTransform>(), m_InstanceItemObj).GetComponent<XuanTianLouItem>();
			Sprite value = null;
			Sprite value2 = null;
			int levelByRank = WorldSetting.GetLevelByRank(child.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out value);
			m_GoodsIconSprite.TryGetValue(child.GoodsType, out value2);
			component.SetValue(child, isHot: false, child.GoodsPrice, ZhiFuType.GongXianDian, child.Name, child.FiveElement, child.Level, value, value2, ClickGoods);
			dataItem.AddItem(component);
		}

		private void ClickGoods(object dataSource, GameObject itemObj)
		{
			XuanTianLouEntity goods = dataSource as XuanTianLouEntity;
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = "兑换",
				DataSource = goods,
				SureBtnClick = (object obj) =>
				{
					SureBuy(goods.TemplateId, goods.GoodsType);
				},
				TitleName = goods.Name,
				ContentList = goods.GoodsDetail
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void SureBuy(string id, int type)
		{
			GouMaiWuPinReq_1015146 data = new GouMaiWuPinReq_1015146
			{
				GoodsTemplateId = id,
				GoodsType = type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GouMaiWuPin, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GouMaiWuPinResp_1015146 gouMaiWuPinResp_ = new GouMaiWuPinResp_1015146();
					gouMaiWuPinResp_.MergeFrom(byteString);
					if (gouMaiWuPinResp_.IsSuccess)
					{
						m_GongXianDian = gouMaiWuPinResp_.GongXianDian;
						Panel.GongXianDianText.text = m_GongXianDian.ToString();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(gouMaiWuPinResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、德经：用以提升功法品阶，出售后返还购买价四分之一的贡献点\n2、仙金：用以提升法器品阶，出售后返还购买价四分之一的贡献点\n3、神术：用以提升法术品阶，出售后返还购买价四分之一的贡献点";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "藏宝阁");
		}
	}
}
