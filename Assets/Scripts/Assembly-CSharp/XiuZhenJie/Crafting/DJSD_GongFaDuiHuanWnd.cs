using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DJSD_GongFaDuiHuanWnd : Window
	{
		private DJSD_GongFaDuiHuanPanel m_Panel;

		private int m_CurrentPage;

		private int m_MaxPagNum;

		private int m_PageSize = 10;

		private int m_CurrentWuXing;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentGongFaType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InstanceObj;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		private PageModel<DJSD_GongFaEntity> m_Result;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<DJSD_GongFaDuiHuanPanel>();
			m_Panel.TitleNameText.text = "功法兑换";
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtnClick);
			AddButtonClickListener(m_Panel.ShaiXuanButton, ShaiXuanBtnClick);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			m_Result = new PageModel<DJSD_GongFaEntity>();
			ResourceLoad();
			m_MaxPagNum = 1;
			m_CurrentPage = 1;
			m_CurrentWuXing = -1;
			m_CurrentPinJie = 0;
			m_CurrentStage = 0;
			m_CurrentGongFaType = 0;
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.GongFaType,
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.XuShenTaRank,
				ShaiXuanItemType.DengJie
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
			m_ScrollContentExtend = m_Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Combine(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.XiaLaAction = (UnityAction)Delegate.Combine(scrollContentExtend2.XiaLaAction, new UnityAction(XiaLaAction));
			}
			SelectGongFa();
			ReFlashData();
			ShaiXuanXianShi(" ");
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			m_Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		private void ResourceLoad()
		{
			m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DJSD_XuShenJieItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_GongFaShu.png");
			m_GoodsIconSprite.Add(1, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			m_Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentBuDaoLevelName() + "】";
			m_Panel.JingYanText.text = $"{Singleton<RoleManager>.Instance.GetBuDaoCurrentExp()}/{Singleton<RoleManager>.Instance.GetBuDaoMaxExp()}";
		}

		private void ShaiXuanType()
		{
			string text = "";
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			if (dictionary != null && dictionary.TryGetValue("功法类别", out var value))
			{
				int.TryParse(value.ItemTag, out var result);
				m_CurrentGongFaType = result;
				if (value.ItemName != "全部")
				{
					text += value.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("五行", out var value2))
			{
				int.TryParse(value2.ItemTag, out var result2);
				m_CurrentWuXing = ((result2 == 0) ? (-1) : result2);
				if (value2.ItemName != "全部")
				{
					text = text + " " + value2.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("品阶", out var value3))
			{
				int.TryParse(value3.ItemTag, out var result3);
				m_CurrentPinJie = result3;
				if (value3.ItemName != "全部")
				{
					text = text + " " + value3.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("等阶", out var value4))
			{
				int.TryParse(value4.ItemTag, out var result4);
				m_CurrentStage = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			SelectGongFa();
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
			SelectGongFa();
			ShowKeyBoardWnd();
			m_Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		private void SelectGongFa()
		{
			DuiHuanChaXunEntity duiHuanChaXunEntity = ZhunBeiCanShu();
			ClearData();
			EnterMethodAchievementExchangeReq_1008030 data = new EnterMethodAchievementExchangeReq_1008030
			{
				FiveElement = duiHuanChaXunEntity.fiveElement,
				PageNum = duiHuanChaXunEntity.pageNum,
				PageSize = duiHuanChaXunEntity.pageSize,
				Rank = duiHuanChaXunEntity.rank,
				Stage = duiHuanChaXunEntity.stage,
				Type = duiHuanChaXunEntity.effect
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DJSD_OPEN_GONGFADUIHUAN, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterMethodAchievementExchangeResp_1008030 enterMethodAchievementExchangeResp_ = new EnterMethodAchievementExchangeResp_1008030();
					enterMethodAchievementExchangeResp_.MergeFrom(byteString);
					if (enterMethodAchievementExchangeResp_.IsSuccess)
					{
						m_Result.list.Clear();
						if (enterMethodAchievementExchangeResp_.Object != null && enterMethodAchievementExchangeResp_.Object.Count > 0)
						{
							foreach (EnterMethodAchievementExchangeResp_1008030.Types.ExchangeMethodAchievementObjectResp_1008031 item2 in enterMethodAchievementExchangeResp_.Object)
							{
								m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
								DJSD_GongFaEntity item = new DJSD_GongFaEntity(item2);
								m_Result.list.Add(item);
							}
						}
						else
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻功法";
						}
						m_Result.pages = enterMethodAchievementExchangeResp_.Pages;
						m_Result.pageNum = enterMethodAchievementExchangeResp_.PageNum;
						m_Result.pageSize = enterMethodAchievementExchangeResp_.PageSize;
						m_MaxPagNum = m_Result.pages;
						CreateData(m_Result);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMethodAchievementExchangeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData(PageModel<DJSD_GongFaEntity> result)
		{
			if (m_InstanceObj == null)
			{
				m_InstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DJSD_XuShenJieItem.prefab");
			}
			GridLayoutGroup component = m_Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (DJSD_GongFaEntity item in result.list)
			{
				DJSD_XuShenJieItem component2 = SetListContentParent(m_Panel.XianShiListRectTransform, m_InstanceObj).gameObject.GetComponent<DJSD_XuShenJieItem>();
				m_GoodsIconSprite.TryGetValue(1, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItems(item, item.Name, item.FiveElement, item.Level, value2, value, (object obj, GameObject gameObj) =>
				{
					ItemBtnClick(item);
				});
			}
			SetGrticalListContentHight(result.list.Count, m_Panel.XianShiListRectTransform, component);
		}

		private void ItemBtnClick(DJSD_GongFaEntity model)
		{
			Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(model.Id, model.Level, model.Rank, model.FiveElement, (List<string> list) =>
			{
				PopUpXiangQingPanel(model.Name, "确定", null, list);
			});
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

		private DuiHuanChaXunEntity ZhunBeiCanShu()
		{
			return new DuiHuanChaXunEntity
			{
				pageNum = m_CurrentPage,
				pageSize = m_PageSize,
				rank = m_CurrentPinJie,
				stage = m_CurrentStage,
				effect = m_CurrentGongFaType,
				fiveElement = m_CurrentWuXing
			};
		}

		private void ClearData()
		{
			foreach (Transform item in m_Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void ShaiXuanBtnClick()
		{
			m_ShaiXuanWnd.PopUpPanel(isPop: true);
		}

		private void WenHaoTogClick()
		{
			string text = $"使用贡献点可以兑换不超过当前布道师品阶的法术，不同的功法兑换的价格不同。每一种功法兑换成功后，会将兑换所需贡献的5%给予功法的创造者。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "兑换功法");
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("Common_ShaiXuanWnd.prefab");
		}
	}
}
