using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQi_DuiHuanWnd : Window
	{
		public LianQi_DuiHuanPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int m_PageSize = 10;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_Type;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InstanceObj;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private PageModel<LQ_LianQiEntity> m_Result;

		private string m_LevelInfo;

		private void ResourceLoad()
		{
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_QiTu.png");
			m_GoodsIconSprite.Add(4, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQi_DuiHuanPanel>();
			m_MaxPagNum = 1;
			m_CurrentPage = 1;
			m_CurrentWuXing = -1;
			m_CurrentPinJie = 0;
			m_CurrentStage = 0;
			Panel.TitleNameText.text = "器图兑换";
			ResourceLoad();
			ReFlashData();
			AddButtonClickListener(Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(Panel.ShaiXuanButton, ShaiXuanBtn);
			AddButtonClickListener(Panel.WenHaoButton, WenHaoTogClick);
			m_Result = new PageModel<LQ_LianQiEntity>();
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.DengJie,
				ShaiXuanItemType.XSJ_FaQi
			}, true) as Common_ShaiXuanWnd;
			if (m_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
				shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(ShaiXuanType));
				Common_ShaiXuanWnd shaiXuanWnd2 = m_ShaiXuanWnd;
				shaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(shaiXuanWnd2.ReChooseAction, new UnityAction(ShaiXuanType));
			}
			ShaiXuanXianShi(" ");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_ScrollContentExtend = Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Combine(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.XiaLaAction = (UnityAction)Delegate.Combine(scrollContentExtend2.XiaLaAction, new UnityAction(XiaLaAction));
			}
			m_ShaiXuanWnd.PopUpPanel(isPop: true);
			SelectFuShu();
			ReFlashData();
		}

		private void ShangLaAction()
		{
			m_CurrentPage--;
			if (m_CurrentPage <= 0)
			{
				m_CurrentPage = 1;
			}
			PageChage(m_CurrentPage);
		}

		private void XiaLaAction()
		{
			if (m_CurrentPage != m_MaxPagNum)
			{
				m_CurrentPage++;
				if (m_CurrentPage >= m_MaxPagNum)
				{
					m_CurrentPage = m_MaxPagNum;
				}
				PageChage(m_CurrentPage);
			}
		}

		private void PageChage(int targetPage)
		{
			if (targetPage > m_MaxPagNum)
			{
				targetPage = m_MaxPagNum;
			}
			if (targetPage <= 0)
			{
				targetPage = 1;
			}
			m_CurrentPage = targetPage;
			SelectFuShu();
			ShowKeyBoardWnd();
			Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		private void ShowKeyBoardWnd()
		{
			if (m_KeyWnd == null)
			{
				Singleton<UIManager>.Instance.PopUpWnd("KeyBoardWnd.prefab", true, m_CurrentPage, m_MaxPagNum);
				m_KeyWnd = Singleton<UIManager>.Instance.FindWndByName<KeyBoardWnd>("KeyBoardWnd.prefab");
				KeyBoardWnd keyWnd = m_KeyWnd;
				keyWnd.ChangePageAction = (Action<int>)Delegate.Remove(keyWnd.ChangePageAction, new Action<int>(PageChage));
				KeyBoardWnd keyWnd2 = m_KeyWnd;
				keyWnd2.ChangePageAction = (Action<int>)Delegate.Combine(keyWnd2.ChangePageAction, new Action<int>(PageChage));
			}
			else
			{
				m_KeyWnd.OnShow(m_CurrentPage, m_MaxPagNum);
			}
		}

		private void ShaiXuanBtn()
		{
			m_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void SelectFuShu()
		{
			ClearData();
			EnterMethodEquipmentExchangeReq_1008067 data = new EnterMethodEquipmentExchangeReq_1008067
			{
				FiveElement = m_CurrentWuXing,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				Stage = m_CurrentStage,
				Effect = m_Type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_METHOD_EQUIPMENT_EXCHANGE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterMethodEquipmentExchangeResp_1008067 enterMethodEquipmentExchangeResp_ = new EnterMethodEquipmentExchangeResp_1008067();
					enterMethodEquipmentExchangeResp_.MergeFrom(byteString);
					if (enterMethodEquipmentExchangeResp_.IsSuccess)
					{
						m_Result.list.Clear();
						Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
						if (enterMethodEquipmentExchangeResp_.Object != null && enterMethodEquipmentExchangeResp_.Object.Count > 0)
						{
							foreach (EnterMethodEquipmentExchangeResp_1008067.Types.ExchangeMethodEquipmentObjectResp_1008068 item2 in enterMethodEquipmentExchangeResp_.Object)
							{
								LQ_LianQiEntity item = new LQ_LianQiEntity(item2);
								m_Result.list.Add(item);
							}
						}
						else
						{
							Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻器图";
						}
						m_Result.pages = enterMethodEquipmentExchangeResp_.Pages;
						m_Result.pageSize = enterMethodEquipmentExchangeResp_.PageSize;
						m_Result.pageNum = enterMethodEquipmentExchangeResp_.PageNum;
						m_MaxPagNum = m_Result.pages;
						CreateData(m_Result);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMethodEquipmentExchangeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShaiXuanType()
		{
			string text = "";
			Dictionary<string, ShaiXuanParamsModel> obj = m_ShaiXuanWnd?.GetResult();
			ShaiXuanParamsModel value = null;
			if (obj.TryGetValue("五行", out value))
			{
				int result = -1;
				int.TryParse(value.ItemTag, out result);
				m_CurrentWuXing = ((result == 0) ? (-1) : result);
				if (value.ItemName != "全部")
				{
					text += value.ItemName;
				}
			}
			ShaiXuanParamsModel value2 = null;
			if (obj.TryGetValue("品阶", out value2))
			{
				int result2 = 0;
				int.TryParse(value2.ItemTag, out result2);
				m_CurrentPinJie = result2;
				if (value2.ItemName != "全部")
				{
					text = text + " " + value2.ItemName;
				}
			}
			ShaiXuanParamsModel value3 = null;
			if (obj.TryGetValue("等阶", out value3))
			{
				int result3 = 0;
				int.TryParse(value3.ItemTag, out result3);
				m_CurrentStage = result3;
				if (value3.ItemName != "全部")
				{
					text = text + " " + value3.ItemName;
				}
			}
			ShaiXuanParamsModel value4 = null;
			if (obj.TryGetValue("功能类别", out value4))
			{
				int result4 = 0;
				int.TryParse(value4.ItemTag, out result4);
				m_CurrentStage = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			ShaiXuanXianShi(text);
			SelectFuShu();
		}

		private void CreateData(PageModel<LQ_LianQiEntity> result)
		{
			if (m_InstanceObj == null)
			{
				m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			}
			GridLayoutGroup component = Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (LQ_LianQiEntity item in result.list)
			{
				Common_XuShenJieSItem component2 = SetListContentParent(Panel.XianShiListRectTransform, m_InstanceObj).gameObject.GetComponent<Common_XuShenJieSItem>();
				m_GoodsIconSprite.TryGetValue(4, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItems(item, item.Name, item.FiveElement, item.Level, value2, value, ItemBtnClick, Common_XiShenJie.LianQi, item.ExchangeContribution);
			}
			SetGrticalListContentHight(result.list.Count, Panel.XianShiListRectTransform, component);
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.JingYanText.text = $"{Singleton<RoleManager>.Instance.GetLianQiShiCurrentExp()}/{Singleton<RoleManager>.Instance.GetLianQiShiMaxExp()}";
			Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentLianQiShiLevelName() + "】";
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		private void ItemBtnClick(object model, GameObject gameObj)
		{
			LQ_LianQiEntity entity = model as LQ_LianQiEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateQiTuDetail(entity.Id, entity.Level, entity.Rank, entity.FiveElement, (List<string> list) =>
				{
					PopUpXiangQingPanel(entity.Name, "确定", null, list);
				});
			}
		}

		private void PopUpXiangQingPanel(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			List<string> list = new List<string>();
			foreach (string xiangQingDatum in xiangQingData)
			{
				if (!(xiangQingDatum.Substring(0, 2) == "材料"))
				{
					list.Add(xiangQingDatum);
				}
			}
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = list,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void ClearData()
		{
			foreach (Transform item in Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void WenHaoTogClick()
		{
			string text = $"使用贡献点可以兑换不超过当前炼器等级的器图，所有的器图兑换价格各不相同。每一种器图兑换成功后，会将兑换所需的5%贡献基于器图的创造者。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "器图兑换");
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd(m_ShaiXuanWnd);
		}
	}
}
