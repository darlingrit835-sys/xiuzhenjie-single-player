using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_RenZhengWnd : Window
	{
		public DST_RenZhengPanel _panel;

		private DST_KaoShiItemModel m_ChooseDanFang;

		private Dictionary<int, DanShiLevelInfoModel> danShiLevelDic;

		private List<DST_KaoShiItemModel> m_DataList;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<DST_RenZhengPanel>();
			danShiLevelDic = Singleton<FacadeManager>.Instance.SelectDanShiLevelInfo();
			AddButtonClickListener(_panel.StartBtn, StartLianZhiClick);
			AddButtonClickListener(_panel.FanHui, FanHuiBtnClick);
			AddButtonClickListener(_panel.WeiHao, WenHaoTogClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			GetInfo();
			ReFlashData();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			_panel.DanShiDengJi_text.text = "【" + Singleton<RoleManager>.Instance.GetCurrentDanShiLevelName() + "】";
			_panel.GongXianDian.text = Singleton<RoleManager>.Instance.Role_Entity.GongXianDian + "点";
			_panel.NowLevel.text = Singleton<RoleManager>.Instance.GetCurrentDanShiLevelName();
			_panel.NextLevel.text = Singleton<RoleManager>.Instance.GetNextDanShiLevelName();
		}

		private void GetInfo()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_DANSHIRENZHENG, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterElixirAuthResp_1008007 enterElixirAuthResp_ = new EnterElixirAuthResp_1008007();
					enterElixirAuthResp_.MergeFrom(byteString);
					if (enterElixirAuthResp_.IsSuccess)
					{
						List<DST_KaoShiItemModel> list = new List<DST_KaoShiItemModel>();
						foreach (EnterElixirAuthResp_1008007.Types.ExamineContentObjectResp_1008008 item in enterElixirAuthResp_.Object)
						{
							list.Add(new DST_KaoShiItemModel
							{
								ask = item.Ask,
								elixirTeacherId = item.MethodElixirId,
								name = item.Name,
								num = item.Count
							});
						}
						m_DataList = list;
						CreateData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterElixirAuthResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void CreateData()
		{
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DanShiItem.prefab");
			_ = gameObject.GetComponent<RectTransform>().sizeDelta;
			ToggleGroup component = _panel.ItemListContent.GetComponent<ToggleGroup>();
			GridLayoutGroup component2 = _panel.ItemListContent.GetComponent<GridLayoutGroup>();
			if (m_DataList == null)
			{
				return;
			}
			for (int i = 0; i < m_DataList.Count; i++)
			{
				DanShiTaItem component3 = SetListContentParent(_panel.ItemListContent, gameObject).GetComponent<DanShiTaItem>();
				component3.toggle.group = component;
				if (i == 0)
				{
					component3.SetValue(m_DataList[i], ToggleItemClick, isChoose: true);
				}
				else
				{
					component3.SetValue(m_DataList[i], ToggleItemClick);
				}
			}
			_panel.ItemListContent.sizeDelta = new Vector2(SetGridLayOutContentWight(m_DataList.Count, component2), _panel.ItemListContent.sizeDelta.y);
		}

		private void ToggleItemClick(bool isOn, DST_KaoShiItemModel pbj)
		{
			if (isOn)
			{
				m_ChooseDanFang = pbj;
			}
		}

		private void StartLianZhiClick()
		{
			KaoHeLianDan();
		}

		private void KaoHeLianDan()
		{
			ExamineElixirStepReq_1008009 data = new ExamineElixirStepReq_1008009
			{
				MethodElixirId = m_ChooseDanFang.elixirTeacherId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_DANFANGKAOHE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ExamineElixirStepResp_1008009 examineElixirStepResp_ = new ExamineElixirStepResp_1008009();
					examineElixirStepResp_.MergeFrom(byteString);
					if (examineElixirStepResp_.IsSuccess)
					{
						m_ChooseDanFang.methodElixir = new DanFangModel();
						m_ChooseDanFang.methodElixir.bb = new List<DanFangStepModel>();
						foreach (ExamineElixirStepResp_1008009.Types.ElixirStepObjectResp_1008010 item2 in examineElixirStepResp_.Object)
						{
							DanFangStepModel item = new DanFangStepModel
							{
								id = item2.Order.ToString(),
								actionType = item2.ActionType,
								bestVal = item2.BestVal,
								continuetime = item2.ContinueTime,
								maxVal = item2.MaxVal,
								minVal = item2.MinVal,
								order = item2.Order,
								targetType = item2.TargetType,
								valType = item2.ValType
							};
							m_ChooseDanFang.methodElixir.bb.Add(item);
						}
						Singleton<UIManager>.Instance.CloseWnd(this);
						Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "DiHuoGe_LianDan", DiHuoGeEnterType.KaoShi, m_ChooseDanFang);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(examineElixirStepResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FanHuiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = $"每次炼丹出了会提升此丹方的熟练度，也会提升当前品阶的熟练度。当熟练度修满之后可以参加丹师考核，进行丹师升品。高品丹师少之又少，每日最多可以考核三次，若暂时无法通过，可以精修自身神识以降低炼丹难度。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "丹师考核");
		}
	}
}
