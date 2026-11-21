using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DJSD_ZhuCeWnd : Window
	{
		private DJSD_ZhuCePanel m_Panel;

		private int m_State = -1;

		private int m_CurrentPage = 1;

		private int m_MaxPagNum = 1;

		private int m_PageSize = 10;

		private int m_CurrentWuXing = -1;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_GongFaType;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private GameObject m_ZhuCeItemInsObj;

		private PageModel<DST_BuQuanEntity> m_DataSource;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<DJSD_ZhuCePanel>();
			m_Panel.TitleNameText.text = "功法注册";
			AddButtonClickListener(m_Panel.LiKaiButton, FanHuiBtnClick);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.ShaiXuanButton, PopShaiXuanWnd);
			ResourceLoad();
			m_DataSource = new PageModel<DST_BuQuanEntity>();
			m_ScrollContentExtend = m_Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
			if (m_ScrollContentExtend != null)
			{
				ScrollViewContentExtend scrollContentExtend = m_ScrollContentExtend;
				scrollContentExtend.ShangLaAction = (UnityAction)Delegate.Combine(scrollContentExtend.ShangLaAction, new UnityAction(ShangLaAction));
				ScrollViewContentExtend scrollContentExtend2 = m_ScrollContentExtend;
				scrollContentExtend2.XiaLaAction = (UnityAction)Delegate.Combine(scrollContentExtend2.XiaLaAction, new UnityAction(XiaLaAction));
			}
			m_ShaiXuanWnd = Singleton<UIManager>.Instance.PopUpWnd("Common_ShaiXuanWnd.prefab", true, new List<ShaiXuanItemType>
			{
				ShaiXuanItemType.XSJ_ZhuCe,
				ShaiXuanItemType.WuXing,
				ShaiXuanItemType.PinJie,
				ShaiXuanItemType.DengJie,
				ShaiXuanItemType.GongFaType
			}, true) as Common_ShaiXuanWnd;
			if (m_ShaiXuanWnd != null)
			{
				Common_ShaiXuanWnd shaiXuanWnd = m_ShaiXuanWnd;
				shaiXuanWnd.ReChooseAction = (UnityAction)Delegate.Remove(shaiXuanWnd.ReChooseAction, new UnityAction(ShaiXuanType));
				Common_ShaiXuanWnd shaiXuanWnd2 = m_ShaiXuanWnd;
				shaiXuanWnd2.ReChooseAction = (UnityAction)Delegate.Combine(shaiXuanWnd2.ReChooseAction, new UnityAction(ShaiXuanType));
			}
			InitData();
			ReFlashData();
			ShaiXuanXianShi(" ");
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			m_Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		private void ResourceLoad()
		{
			m_ZhuCeItemInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/XuShenJieItem.prefab");
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

		public void InitData()
		{
			ClearData();
			EnterAchievementRegisterReq_1008034 data = new EnterAchievementRegisterReq_1008034
			{
				FiveElement = m_CurrentWuXing,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				RegisterState = m_State,
				Stage = m_CurrentStage,
				Type = m_GongFaType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DJSD_OPEN_GONGFAZHUCE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterAchievementRegisterResp_1008034 enterAchievementRegisterResp_ = new EnterAchievementRegisterResp_1008034();
					enterAchievementRegisterResp_.MergeFrom(byteString);
					if (enterAchievementRegisterResp_.IsSuccess)
					{
						m_DataSource.list = new List<DST_BuQuanEntity>();
						if (enterAchievementRegisterResp_.Object != null && enterAchievementRegisterResp_.Object.Count > 0)
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
							foreach (EnterAchievementRegisterResp_1008034.Types.MethodAchievementObjectResp_1008035 item2 in enterAchievementRegisterResp_.Object)
							{
								DST_BuQuanEntity item = new DST_BuQuanEntity
								{
									Id = item2.Id,
									Name = item2.Name,
									Level = item2.Level,
									FiveElement = item2.FiveElement,
									Rank = item2.Rank,
									Progress = item2.Part
								};
								m_DataSource.list.Add(item);
							}
							m_DataSource.pageNum = enterAchievementRegisterResp_.PageNum;
							m_DataSource.pages = enterAchievementRegisterResp_.Pages;
							m_DataSource.pageSize = enterAchievementRegisterResp_.PageSize;
							m_MaxPagNum = enterAchievementRegisterResp_.Pages;
							CreateData();
						}
						else
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻功法";
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterAchievementRegisterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void PopShaiXuanWnd()
		{
			m_ShaiXuanWnd.PopUpPanel(isPop: true);
		}

		private void CreateData()
		{
			GridLayoutGroup component = m_Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (DST_BuQuanEntity item in m_DataSource.list)
			{
				XuShenJieItem component2 = SetListContentParent(m_Panel.XianShiListRectTransform, m_ZhuCeItemInsObj).gameObject.GetComponent<XuShenJieItem>();
				m_GoodsIconSprite.TryGetValue(1, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItemRegister(item, item.Name, item.FiveElement, item.Level, value2, value, (object obj, GameObject gameObj) =>
				{
					ClickXiangQing(item);
				}, m_State);
			}
			SetGrticalListContentHight(m_DataSource.list.Count, m_Panel.XianShiListRectTransform, component);
		}

		private void ClickXiangQing(DST_BuQuanEntity buQuan)
		{
			Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(buQuan.Id, buQuan.Level, buQuan.Rank, buQuan.FiveElement, (List<string> list) =>
			{
				PopUpXiangQingPanel(buQuan.Name, "确定", null, list);
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

		private void ClickZHuCe(object obj, GameObject gameObj)
		{
			DST_BuQuanEntity dST_BuQuanEntity = obj as DST_BuQuanEntity;
			Singleton<UIManager>.Instance.PopUpWnd("DJSD_ZhuCeTipWnd.prefab", true, dST_BuQuanEntity, m_State);
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
			InitData();
			ShowKeyBoardWnd();
			m_Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		private void ShaiXuanType()
		{
			string text = "";
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			if (dictionary != null && dictionary.TryGetValue("状态", out var value))
			{
				int.TryParse(value.ItemTag, out var result);
				m_State = result;
				if (value.ItemName != "全部")
				{
					text += value.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("功法类别", out var value2))
			{
				int.TryParse(value2.ItemTag, out var result2);
				m_GongFaType = result2;
				if (value2.ItemName != "全部")
				{
					text = text + " " + value2.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("五行", out var value3))
			{
				int.TryParse(value3.ItemTag, out var result3);
				m_CurrentWuXing = ((result3 == 0) ? (-1) : result3);
				if (value3.ItemName != "全部")
				{
					text = text + " " + value3.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("品阶", out var value4))
			{
				int.TryParse(value4.ItemTag, out var result4);
				m_CurrentPinJie = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("等阶", out var value5))
			{
				int.TryParse(value5.ItemTag, out var result5);
				m_CurrentStage = result5;
				if (value5.ItemName != "全部")
				{
					text = text + " " + value5.ItemName;
				}
			}
			ShaiXuanXianShi(text);
			InitData();
		}

		private void ClearData()
		{
			foreach (Transform item in m_Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void FanHuiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd("Common_ShaiXuanWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, "", "功法注册");
		}
	}
}
