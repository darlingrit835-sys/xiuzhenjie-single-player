using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WFG_FaShuDuiHuanWnd : Window
	{
		public WFG_FaShuDuiHuanPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int m_PageSize = 10;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentFaShuType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InstanceObj;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private PageModel<WFG_FaShuEntity> m_Result;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WFG_FaShuDuiHuanPanel>();
			AddButtonClickListener(Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(Panel.ShaiXuanButton, ShaiXuanBtn);
			Panel.TitleNameText.text = "法术兑换";
			AddButtonClickListener(Panel.WenHaoButton, WenHaoTogClick);
			m_Result = new PageModel<WFG_FaShuEntity>();
			ResourceLoad();
			m_MaxPagNum = 1;
			m_CurrentPage = 1;
			m_CurrentWuXing = -1;
			m_CurrentPinJie = 0;
			m_CurrentStage = 0;
			m_CurrentFaShuType = 0;
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.XuShenTaRank,
				ShaiXuanItemType.DengJie,
				ShaiXuanItemType.FaShuType
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
			SelectFaShu();
			ReFlashData();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.JingYanText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentShuShiLevelName() + "】";
			Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.LevelInfoText.text = $"{Singleton<RoleManager>.Instance.GetShuShiCurrentExp()}/{Singleton<RoleManager>.Instance.GetShuShiMaxExp()}";
		}

		private void ResourceLoad()
		{
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FaSuShu.png");
			m_GoodsIconSprite.Add(6, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
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
			if (obj.TryGetValue("法术类别", out value4))
			{
				int result4 = 0;
				int.TryParse(value4.ItemTag, out result4);
				m_CurrentFaShuType = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			SelectFaShu();
			ShaiXuanXianShi(text);
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

		public void PageChage(int targetPage)
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
			SelectFaShu();
			ShowKeyBoardWnd();
			Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		private void SelectFaShu()
		{
			ClearData();
			EnterMethodSkillExchangeReq_1008022 data = new EnterMethodSkillExchangeReq_1008022
			{
				FiveElement = m_CurrentWuXing,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				Stage = m_CurrentStage,
				Type = m_CurrentFaShuType,
				PageNum = m_CurrentPage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_WFG_OPEN_DUIHUAN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterMethodSkillExchangeResp_1008022 enterMethodSkillExchangeResp_ = new EnterMethodSkillExchangeResp_1008022();
					enterMethodSkillExchangeResp_.MergeFrom(byteString);
					if (enterMethodSkillExchangeResp_.IsSuccess)
					{
						m_Result.list.Clear();
						Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
						if (enterMethodSkillExchangeResp_.Object != null && enterMethodSkillExchangeResp_.Object.Count > 0)
						{
							foreach (EnterMethodSkillExchangeResp_1008022.Types.ExchangeMethodSkillObjectResp_1008023 item2 in enterMethodSkillExchangeResp_.Object)
							{
								WFG_FaShuEntity item = new WFG_FaShuEntity(item2);
								m_Result.list.Add(item);
							}
						}
						else
						{
							Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻法术";
						}
						m_Result.pages = enterMethodSkillExchangeResp_.Pages;
						m_Result.pageSize = enterMethodSkillExchangeResp_.PageSize;
						m_Result.pageNum = enterMethodSkillExchangeResp_.PageNum;
						m_MaxPagNum = m_Result.pages;
						CreateData(m_Result);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMethodSkillExchangeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData(PageModel<WFG_FaShuEntity> result)
		{
			if (m_InstanceObj == null)
			{
				m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			}
			GridLayoutGroup component = Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (WFG_FaShuEntity item in result.list)
			{
				Common_XuShenJieSItem component2 = SetListContentParent(Panel.XianShiListRectTransform, m_InstanceObj).gameObject.GetComponent<Common_XuShenJieSItem>();
				m_GoodsIconSprite.TryGetValue(6, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItems(item, item.Name, item.FiveElement, item.Level, value2, value, ItemBtnClick, Common_XiShenJie.FaShu, item.ExchangeContribution);
			}
			SetGrticalListContentHight(result.list.Count, Panel.XianShiListRectTransform, component);
		}

		private void ItemBtnClick(object model, GameObject gameObj)
		{
			WFG_FaShuEntity entity = model as WFG_FaShuEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(entity.Id, entity.Level, entity.Rank, entity.FiveElement, (List<string> list) =>
				{
					PopUpXiangQingPanel(entity.Name, "确定", null, list);
				});
			}
		}

		private void PopUpXiangQingPanel(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}

		private void ShaiXuanBtn()
		{
			m_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void WenHaoTogClick()
		{
			string text = $"使用贡献点可以兑换不超过当前术师品阶的法术，不同的法术兑换的价格不同。每一种法术兑换成功后，会将兑换所需贡献的5%给予法术的创造者。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "兑换法术");
		}

		private void ClearData()
		{
			foreach (Transform item in Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd(m_ShaiXuanWnd);
		}

		public override void OnClose()
		{
			base.OnClose();
			Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
			shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(ShaiXuanType));
		}
	}
}
