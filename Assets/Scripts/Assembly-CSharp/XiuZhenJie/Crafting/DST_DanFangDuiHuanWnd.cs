using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_DanFangDuiHuanWnd : Window
	{
		public DST_DanFangDuiHuanPanel Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int m_PageSize = 10;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentDanYaoEffect;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InstanceObj;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private PageModel<DST_DanFangEntity> m_Result;

		private int GongXianDian;

		private void ResourceLoad()
		{
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieSItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanShu.png");
			m_GoodsIconSprite.Add(2, value);
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
			Panel = base.GameObject.GetComponent<DST_DanFangDuiHuanPanel>();
			AddButtonClickListener(Panel.LiKaiButton, BackBtnClick);
			AddButtonClickListener(Panel.ShaiXuanButton, ShaiXuanBtnClick);
			AddButtonClickListener(Panel.WenHaoButton, WenHaoTogClick);
			Panel.TitleNameText.text = "丹方兑换";
			m_Result = new PageModel<DST_DanFangEntity>();
			ResourceLoad();
			m_MaxPagNum = 1;
			m_CurrentPage = 1;
			m_CurrentWuXing = -1;
			m_CurrentPinJie = 0;
			m_CurrentStage = 0;
			m_CurrentDanYaoEffect = 0;
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.XuShenTaRank,
				ShaiXuanItemType.DengJie,
				ShaiXuanItemType.DanFangType
			}, true) as Common_ShaiXuanWnd;
			if (m_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
				shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(ShaiXuanType));
				Common_ShaiXuanWnd shaiXuanWnd2 = m_ShaiXuanWnd;
				shaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(shaiXuanWnd2.ReChooseAction, new UnityAction(ShaiXuanType));
			}
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
			SelectDanFang();
			ReFlashData();
			ShaiXuanXianShi(" ");
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentDanShiLevelName() + "】";
			Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.JingYanText.text = Singleton<RoleManager>.Instance.GetLianDanCurrentExp() + "/" + Singleton<RoleManager>.Instance.GetLianDanMaxExp();
		}

		public void ReflashCurrentPage()
		{
			PageChage(m_CurrentPage);
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
			if (m_CurrentPage == m_MaxPagNum)
			{
				Debug.Log("maxPageNum：" + m_MaxPagNum);
				return;
			}
			m_CurrentPage++;
			if (m_CurrentPage >= m_MaxPagNum)
			{
				m_CurrentPage = m_MaxPagNum;
			}
			PageChage(m_CurrentPage);
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
			SelectDanFang();
			ShowKeyBoardWnd();
			Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
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
			if (obj.TryGetValue("药效", out value4))
			{
				int result4 = 0;
				int.TryParse(value4.ItemTag, out result4);
				m_CurrentDanYaoEffect = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			SelectDanFang();
			ShaiXuanXianShi(text);
		}

		private void SelectDanFang()
		{
			ClearData();
			EnterMethodElixirExchangeReq_1008012 data = new EnterMethodElixirExchangeReq_1008012
			{
				Effect = m_CurrentDanYaoEffect,
				FiveElement = m_CurrentWuXing,
				Rank = m_CurrentPinJie,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Stage = m_CurrentStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_OPEN_DANFANGDUIHUAN, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterMethodElixirExchangeResp_1008012 enterMethodElixirExchangeResp_ = new EnterMethodElixirExchangeResp_1008012();
					enterMethodElixirExchangeResp_.MergeFrom(byteString);
					if (enterMethodElixirExchangeResp_.IsSuccess)
					{
						m_Result.list.Clear();
						if (enterMethodElixirExchangeResp_.Object != null && enterMethodElixirExchangeResp_.Object.Count > 0)
						{
							foreach (EnterMethodElixirExchangeResp_1008012.Types.ExchangeMethodElixirObjectResp_1008013 item2 in enterMethodElixirExchangeResp_.Object)
							{
								Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
								DST_DanFangEntity item = new DST_DanFangEntity(item2);
								m_Result.list.Add(item);
							}
						}
						else
						{
							Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻丹方";
						}
						m_Result.pages = enterMethodElixirExchangeResp_.Pages;
						m_Result.pageNum = enterMethodElixirExchangeResp_.PageNum;
						m_Result.pageSize = enterMethodElixirExchangeResp_.PageSize;
						m_MaxPagNum = m_Result.pages;
						CreateData(m_Result);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMethodElixirExchangeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
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

		private void CreateData(PageModel<DST_DanFangEntity> result)
		{
			GridLayoutGroup component = Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (DST_DanFangEntity item in result.list)
			{
				Common_XuShenJieSItem component2 = SetListContentParent(Panel.XianShiListRectTransform, m_InstanceObj).gameObject.GetComponent<Common_XuShenJieSItem>();
				m_GoodsIconSprite.TryGetValue(2, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItems(item, item.Name, item.FiveElement, item.Level, value2, value, ItemBtnClick, Common_XiShenJie.LianDan, item.ExchangeContribution);
			}
			SetGrticalListContentHight(result.list.Count, Panel.XianShiListRectTransform, component);
		}

		private void ItemBtnClick(object model, GameObject gameObj)
		{
			DST_DanFangEntity entity = model as DST_DanFangEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateElixirDetail(entity.Id, entity.Level, entity.Rank, entity.FiveElement, (List<string> list) =>
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

		private void BackBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd(m_ShaiXuanWnd);
		}

		private void ShaiXuanBtnClick()
		{
			m_ShaiXuanWnd?.PopUpPanel(isPop: true);
		}

		private void WenHaoTogClick()
		{
			string text = $"使用贡献点可以兑换不超过当前丹师品阶的丹方，不同的丹方兑换的价格不同。每一种丹方兑换成功后，会将兑换所需贡献的5%给予丹方的创造者。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "兑换丹方");
		}

		public override void OnClose()
		{
			base.OnClose();
			Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
			shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(ShaiXuanType));
		}
	}
}
