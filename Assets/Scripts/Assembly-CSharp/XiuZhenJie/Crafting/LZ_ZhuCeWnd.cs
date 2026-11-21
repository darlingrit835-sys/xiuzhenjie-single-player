using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_ZhuCeWnd : Window
	{
		private LZ_ZhuCePanel m_Panel;

		private int m_State = -1;

		private int m_CurrentPage = 1;

		private int m_MaxPagNum = 1;

		private int m_PageSize = 10;

		private int m_CurrentWuXing = -1;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentSiQi;

		private int m_CurrentWuWei;

		private int m_CurrentBuXie;

		private int m_CurrentGuiJing;

		private int m_LingZhiType;

		private KeyBoardWnd m_KeyWnd;

		private ScrollViewContentExtend m_ScrollContentExtend;

		private GameObject m_ZhuCeItemInsObj;

		private PageModel<DST_CommonEntity> m_DataSource;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Common_ShaiXuanWnd m_ShaiXuanWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<LZ_ZhuCePanel>();
			m_Panel.TitleNameText.text = "灵植注册";
			AddButtonClickListener(m_Panel.LiKaiButton, FanHuiBtnClick);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.ShaiXuanButton, PopShaiXuanPanel);
			m_DataSource = new PageModel<DST_CommonEntity>();
			ResourceLoad();
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
				ShaiXuanItemType.XSJ_LingZhi
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

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			m_Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentLingZhiLevelName() + "】";
			m_Panel.JingYanText.text = Singleton<RoleManager>.Instance.GetLingZhiCurrentExp() + "/" + Singleton<RoleManager>.Instance.GetLingZhiMaxExp();
		}

		private void ShaiXuanXianShi(string tiaoJian)
		{
			m_Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		private void ShaiXuanType()
		{
			Dictionary<string, ShaiXuanParamsModel> dictionary = m_ShaiXuanWnd?.GetResult();
			string text = "";
			if (dictionary != null && dictionary.TryGetValue("状态", out var value))
			{
				int.TryParse(value.ItemTag, out var result);
				m_State = result;
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
			if (dictionary != null && dictionary.TryGetValue("四气", out var value5))
			{
				int.TryParse(value5.ItemTag, out var result5);
				m_CurrentSiQi = result5;
				if (value5.ItemName != "全部")
				{
					text = text + " " + value5.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("五味", out var value6))
			{
				int.TryParse(value6.ItemTag, out var result6);
				m_CurrentWuWei = result6;
				if (value6.ItemName != "全部")
				{
					text = text + " " + value6.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("补泻", out var value7))
			{
				int.TryParse(value7.ItemTag, out var result7);
				m_CurrentBuXie = result7;
				if (value7.ItemName != "全部")
				{
					text = text + " " + value7.ItemName;
				}
			}
			if (dictionary != null && dictionary.TryGetValue("归经", out var value8))
			{
				int.TryParse(value8.ItemTag, out var result8);
				m_CurrentGuiJing = result8;
				if (value8.ItemName != "全部")
				{
					text = text + " " + value8.ItemName;
				}
			}
			ShaiXuanXianShi(text);
			InitData();
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

		public void InitData()
		{
			ClearData();
			EnterTemplatePlantRegisterReq_1008059 data = new EnterTemplatePlantRegisterReq_1008059
			{
				FiveElement = m_CurrentWuXing,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				RegisterState = m_State,
				Stage = m_CurrentStage,
				SiQi = m_CurrentSiQi,
				WuWei = m_CurrentWuWei,
				BuXie = m_CurrentBuXie,
				GuiJing = m_CurrentGuiJing
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_TEMPLATE_PLANT_REGISTER_RESP, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterTemplatePlantRegisterResp_1008059 enterTemplatePlantRegisterResp_ = new EnterTemplatePlantRegisterResp_1008059();
					enterTemplatePlantRegisterResp_.MergeFrom(byteString);
					if (enterTemplatePlantRegisterResp_.IsSuccess)
					{
						m_DataSource.list = new List<DST_CommonEntity>();
						if (enterTemplatePlantRegisterResp_.Object != null && enterTemplatePlantRegisterResp_.Object.Count > 0)
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
							foreach (EnterTemplatePlantRegisterResp_1008059.Types.TemplatePlantObjectResp_1008060 item2 in enterTemplatePlantRegisterResp_.Object)
							{
								DST_CommonEntity item = new DST_CommonEntity
								{
									Id = item2.Id,
									Name = item2.Name,
									Rank = item2.Rank,
									Level = item2.Level,
									FiveElement = item2.FiveElement
								};
								m_DataSource.list.Add(item);
							}
						}
						else
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻灵植";
						}
						m_DataSource.pageNum = enterTemplatePlantRegisterResp_.PageNum;
						m_DataSource.pages = enterTemplatePlantRegisterResp_.Pages;
						m_DataSource.pageSize = enterTemplatePlantRegisterResp_.PageSize;
						m_MaxPagNum = enterTemplatePlantRegisterResp_.Pages;
						CreateData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterTemplatePlantRegisterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData()
		{
			GridLayoutGroup component = m_Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (DST_CommonEntity item in m_DataSource.list)
			{
				Common_XuShenJieItem component2 = SetListContentParent(m_Panel.XianShiListRectTransform, m_ZhuCeItemInsObj).gameObject.GetComponent<Common_XuShenJieItem>();
				m_GoodsIconSprite.TryGetValue(12, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItemRegister(item, item.Name, item.FiveElement, item.Level, value2, value, ClickXiangQing, Common_XiShenJie.LZ, m_State);
			}
			SetGrticalListContentHight(m_DataSource.list.Count, m_Panel.XianShiListRectTransform, component);
		}

		private void ClickXiangQing(object obj, GameObject gameObj)
		{
			DST_CommonEntity entity = obj as DST_CommonEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(entity.Id, (List<string> list) =>
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

		private void ResourceLoad()
		{
			m_ZhuCeItemInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhiWuShu.png");
			m_GoodsIconSprite.Add(12, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void PopShaiXuanPanel()
		{
			m_ShaiXuanWnd.PopUpPanel(isPop: true);
		}

		private void FanHuiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("KeyBoardWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("Common_ShaiXuanWnd.prefab");
		}

		private void WenHaoTogClick()
		{
			string text = "";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵植注册");
		}

		private void ClearData()
		{
			foreach (Transform item in m_Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}
}
