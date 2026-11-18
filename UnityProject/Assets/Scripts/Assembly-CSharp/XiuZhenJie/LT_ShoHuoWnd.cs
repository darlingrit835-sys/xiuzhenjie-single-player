using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LT_ShoHuoWnd : Window
	{
		public LT_ShoHuoPanel panel;

		private GameObject m_Backpack;

		private Dictionary<int, Sprite> m_Item;

		private Dictionary<int, Sprite> m_Order;

		private List<BaseNaJieGoodEntity> m_SetDataVector;

		private LT_LingTianItem m_LingTianItem;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			panel = base.GameObject.GetComponent<LT_ShoHuoPanel>();
			AddButtonClickListener(panel.QRan, CloseWndClick);
			AddButtonClickListener(panel.TuiChu, CloseWndClick);
			if (paralist.Length != 0)
			{
				m_LingTianItem = paralist[0] as LT_LingTianItem;
			}
			AssignmentId();
			ResourceLoad();
			Obtained();
		}

		private void ResourceLoad()
		{
			m_Item = new Dictionary<int, Sprite>();
			m_Order = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhiWu.png");
			m_Item.Add(1, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_Order.Add(1, value2);
			m_Order.Add(2, value3);
			m_Order.Add(3, value4);
			m_Order.Add(4, value5);
		}

		private void AssignmentId()
		{
			string text = $"第{m_LingTianItem.m_ID}块灵田-收获";
			panel.BiaoTiName.text = text;
		}

		private void Obtained()
		{
			HarvestPlantReq_1013018 data = new HarvestPlantReq_1013018
			{
				LandId = m_LingTianItem.m_ID
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.HARVEST_PLANT_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					HarvestPlantResp_1013018 harvestPlantResp_ = new HarvestPlantResp_1013018();
					m_SetDataVector = new List<BaseNaJieGoodEntity>();
					harvestPlantResp_.MergeFrom(byteString);
					if (harvestPlantResp_.IsSuccess)
					{
						foreach (HarvestPlantResp_1013018.Types.PlantInfoResp_1013019 item2 in harvestPlantResp_.PlantInfo)
						{
							BaseNaJieGoodEntity item = new BaseNaJieGoodEntity
							{
								Name = item2.Name,
								FiveElement = item2.FiveElement,
								GoodsId = item2.Id,
								Level = item2.Level,
								Rank = item2.Rank
							};
							m_SetDataVector.Add(item);
						}
						if (m_SetDataVector != null || m_SetDataVector.Count != 0)
						{
							Addset(m_SetDataVector);
						}
						Singleton<RoleManager>.Instance.SetLingZhiCurrentExp(harvestPlantResp_.LiveLevel, harvestPlantResp_.LiveProficiency);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(harvestPlantResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void Addset(List<BaseNaJieGoodEntity> naJieData)
		{
			GameObject obj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			for (int i = 0; i < naJieData.Count; i++)
			{
				CreateObjItem(obj, naJieData[i]);
			}
			GridLayoutGroup component = panel.ShoHuoLieBiao.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(naJieData.Count, panel.ShoHuoLieBiao, component);
			m_LingTianItem.m_Status = 0;
			m_LingTianItem.ShuXingShuJu();
		}

		private void CreateObjItem(GameObject obj, BaseNaJieGoodEntity _data)
		{
			GameObject gameObject = SetListContentParent(panel.ShoHuoLieBiao, obj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_Item.TryGetValue(1, out value);
			int levelByRank = WorldSetting.GetLevelByRank(_data.Rank);
			Sprite value2 = null;
			m_Order.TryGetValue(levelByRank, out value2);
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, value2, value, (object _obj, GameObject _gameObj) =>
			{
			});
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
