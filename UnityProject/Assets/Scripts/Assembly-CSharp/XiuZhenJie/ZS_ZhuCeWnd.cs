using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_ZhuCeWnd : Window
	{
		private ZS_ZhuCePanel m_Panel;

		private int m_State = -1;

		private int m_CurrentPage = 1;

		private int m_MaxPagNum = 1;

		private int m_PageSize = 10;

		private int m_CurrentWuXing = -1;

		private int m_CurrentStage;

		private int m_CurrentPinJie;

		private int m_CurrentType;

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
			m_Panel = base.GameObject.GetComponent<ZS_ZhuCePanel>();
			m_Panel.TitleNameText.text = "阵图注册";
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
				ShaiXuanItemType.XSJ_ZhenFa
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
			m_Panel.JingYanText.text = $"{Singleton<RoleManager>.Instance.GetZhenFaCurrentExp()}/{Singleton<RoleManager>.Instance.GetZhenFaShiMaxExp()}";
			m_Panel.LevelInfoText.text = "【" + Singleton<RoleManager>.Instance.GetCurrentZhenFaShiLevelName() + "】";
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
					text += value2.ItemName;
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
			if (dictionary != null && dictionary.TryGetValue("类别", out var value5))
			{
				int.TryParse(value5.ItemTag, out var result5);
				m_LingZhiType = result5;
				if (value5.ItemName != "全部")
				{
					text = text + " " + value5.ItemName;
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
			EnterMethodMatrixRegisterReq_1008090 data = new EnterMethodMatrixRegisterReq_1008090
			{
				FiveElement = m_CurrentWuXing,
				Rank = m_CurrentPinJie,
				Stage = m_CurrentStage,
				Type = m_LingZhiType,
				RegisterState = m_State,
				PageNum = m_CurrentPage,
				PageSize = m_PageSize
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTERMETHODMATRIXREGISTER, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterMethodMatrixRegisterResp_1008090 enterMethodMatrixRegisterResp_ = new EnterMethodMatrixRegisterResp_1008090();
					enterMethodMatrixRegisterResp_.MergeFrom(byteString);
					if (enterMethodMatrixRegisterResp_.IsSuccess)
					{
						m_DataSource.list = new List<DST_CommonEntity>();
						if (enterMethodMatrixRegisterResp_.Object != null && enterMethodMatrixRegisterResp_.Object.Count > 0)
						{
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = "";
							foreach (EnterMethodMatrixRegisterResp_1008090.Types.MethodMatrixObjectResp_1008091 item2 in enterMethodMatrixRegisterResp_.Object)
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
							m_Panel.XianShiListRectTransform.gameObject.GetComponent<Text>().text = " 下拉搜寻阵图";
						}
						m_DataSource.pageNum = enterMethodMatrixRegisterResp_.PageNum;
						m_DataSource.pages = enterMethodMatrixRegisterResp_.Pages;
						m_DataSource.pageSize = enterMethodMatrixRegisterResp_.PageSize;
						m_MaxPagNum = enterMethodMatrixRegisterResp_.Pages;
						CreateData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterMethodMatrixRegisterResp_.Code);
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
				m_GoodsIconSprite.TryGetValue(3, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(item.Rank), out var value2);
				component2.SetItemRegister(item, item.Name, item.FiveElement, item.Level, value2, value, ClickXiangQing, Common_XiShenJie.ZhenFa, m_State);
			}
			SetGrticalListContentHight(m_DataSource.list.Count, m_Panel.XianShiListRectTransform, component);
		}

		private void ClickXiangQing(object obj, GameObject gameObj)
		{
			DST_CommonEntity entity = obj as DST_CommonEntity;
			if (entity != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(entity.Id, entity.Level, entity.Rank, entity.FiveElement, (List<string> list) =>
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
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenTu.png");
			m_GoodsIconSprite.Add(3, value);
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
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "阵图注册");
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
