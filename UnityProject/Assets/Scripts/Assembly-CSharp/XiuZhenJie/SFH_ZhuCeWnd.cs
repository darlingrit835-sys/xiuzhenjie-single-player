using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SFH_ZhuCeWnd : Window
	{
		public SFH_ZhuCePanel Panel;

		private int m_State = -1;

		private int m_CurrentPage = 1;

		private int m_MaxPagNum = 1;

		private int m_PageSize = 10;

		private int m_CurrentWuXing = -1;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_FaShuType;

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
			Panel = base.GameObject.GetComponent<SFH_ZhuCePanel>();
			Panel.TitleNameText.text = "符书注册";
			AddButtonClickListener(Panel.LiKaiButton, FanHuiBtnClick);
			AddButtonClickListener(Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(Panel.ShaiXuanButton, PopShaiXuanPanel);
			m_DataSource = new PageModel<DST_CommonEntity>();
			ResourceLoad();
			m_ScrollContentExtend = Panel.ScrollView.gameObject.GetComponent<ScrollViewContentExtend>();
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
				ShaiXuanItemType.FaShuType
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
			Panel.ShaiXuanTiaoJianText.text = "筛选条件:" + tiaoJian;
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.ContributionText.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentFuShiLevelName() + "】";
			Panel.JingYanText.text = $"{Singleton<RoleManager>.Instance.GetFuShiCurrentExp()}/{Singleton<RoleManager>.Instance.GetFuShiMaxExp()}";
		}

		private void ResourceLoad()
		{
			m_ZhuCeItemInsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_XuShenJieItem.prefab");
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuShu.png");
			m_GoodsIconSprite.Add(5, value);
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

		public void InitData()
		{
			ClearData();
			EnterSignRegisterReq_1008048 data = new EnterSignRegisterReq_1008048
			{
				FiveElement = m_CurrentWuXing,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize,
				Rank = m_CurrentPinJie,
				RegisterState = m_State,
				Stage = m_CurrentStage,
				Type = m_FaShuType
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.OPEN_FUSHU_ZHUCE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterSignRegisterResp_1008048 enterSignRegisterResp_ = new EnterSignRegisterResp_1008048();
					enterSignRegisterResp_.MergeFrom(byteString);
					if (enterSignRegisterResp_.IsSuccess)
					{
						m_DataSource.list = new List<DST_CommonEntity>();
						if (enterSignRegisterResp_.Object != null && enterSignRegisterResp_.Object.Count > 0)
						{
							Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
							foreach (EnterSignRegisterResp_1008048.Types.MethodSignObjectResp_1008049 item2 in enterSignRegisterResp_.Object)
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
							Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻符书";
						}
						m_DataSource.pageNum = enterSignRegisterResp_.PageNum;
						m_DataSource.pages = enterSignRegisterResp_.Pages;
						m_DataSource.pageSize = enterSignRegisterResp_.PageSize;
						m_MaxPagNum = enterSignRegisterResp_.Pages;
						CreateData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSignRegisterResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateData()
		{
			GridLayoutGroup component = Panel.XianShiListRectTransform.gameObject.GetComponent<GridLayoutGroup>();
			foreach (DST_CommonEntity item in m_DataSource.list)
			{
				Common_XuShenJieItem component2 = SetListContentParent(Panel.XianShiListRectTransform, m_ZhuCeItemInsObj).gameObject.GetComponent<Common_XuShenJieItem>();
				m_GoodsIconSprite.TryGetValue(5, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItemRegister(item, item.Name, item.FiveElement, item.Level, value2, value, ClickXiangQing, Common_XiShenJie.FuShu, m_State);
			}
			SetGrticalListContentHight(m_DataSource.list.Count, Panel.XianShiListRectTransform, component);
		}

		private void ClickXiangQing(object obj, GameObject gameObj)
		{
			DST_CommonEntity entity = obj as DST_CommonEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateSignDetail(entity.Id, entity.Level, entity.Rank, entity.FiveElement, (List<string> list) =>
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
			InitData();
			ShowKeyBoardWnd();
			Panel.XianShiListRectTransform.anchoredPosition = new Vector2(0f, 0f);
		}

		private void ShaiXuanType()
		{
			Dictionary<string, ShaiXuanParamsModel> obj = m_ShaiXuanWnd?.GetResult();
			string text = "";
			ShaiXuanParamsModel value = null;
			if (obj.TryGetValue("状态", out value))
			{
				int result = -1;
				int.TryParse(value.ItemTag, out result);
				m_State = result;
				if (value.ItemName != "全部")
				{
					text += value.ItemName;
				}
			}
			ShaiXuanParamsModel value2 = null;
			if (obj.TryGetValue("五行", out value2))
			{
				int result2 = -1;
				int.TryParse(value2.ItemTag, out result2);
				m_CurrentWuXing = ((result2 == 0) ? (-1) : result2);
				if (value2.ItemName != "全部")
				{
					text = text + " " + value2.ItemName;
				}
			}
			ShaiXuanParamsModel value3 = null;
			if (obj.TryGetValue("品阶", out value3))
			{
				int result3 = 0;
				int.TryParse(value3.ItemTag, out result3);
				m_CurrentPinJie = result3;
				if (value3.ItemName != "全部")
				{
					text = text + " " + value3.ItemName;
				}
			}
			ShaiXuanParamsModel value4 = null;
			if (obj.TryGetValue("等阶", out value4))
			{
				int result4 = 0;
				int.TryParse(value4.ItemTag, out result4);
				m_CurrentStage = result4;
				if (value4.ItemName != "全部")
				{
					text = text + " " + value4.ItemName;
				}
			}
			ShaiXuanParamsModel value5 = null;
			if (obj.TryGetValue("法术类别", out value5))
			{
				int result5 = 0;
				int.TryParse(value5.ItemTag, out result5);
				m_FaShuType = result5;
				if (value5.ItemName != "全部")
				{
					text = text + " " + value5.ItemName;
				}
			}
			InitData();
			ShaiXuanXianShi(text);
		}

		private void ClearData()
		{
			foreach (Transform item in Panel.XianShiListRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
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
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "符书注册");
		}
	}
}
