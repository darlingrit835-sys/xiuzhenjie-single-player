using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SFH_ZhuCeTipWnd : Window
	{
		public SFH_ZhuCeTipPanel Panel;

		private DST_CommonEntity m_DataSource;

		private GameObject m_ItemObj;

		private UnityAction<string> ZhuCeSuccessAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<SFH_ZhuCeTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as DST_CommonEntity;
				int num = int.Parse(paralist[1].ToString());
				if (num == 1 || num == 2)
				{
					Panel.DanNameText.text = m_DataSource.Name;
					Panel.QueRenBtn.interactable = false;
				}
			}
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			AddButtonClickListener(Panel.ShanChuBtn, ShanChuBtnClick);
			InitDetail();
		}

		private void ShanChuBtnClick()
		{
			DeleteMethodSignReq_1008097 deleteMethodSignReq_1008097 = new DeleteMethodSignReq_1008097
			{
				MethodSignId = m_DataSource.Id
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodSign, deleteMethodSignReq_1008097, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodSignResp_1008097 deleteMethodSignResp_ = new DeleteMethodSignResp_1008097();
						deleteMethodSignResp_.MergeFrom(byteString);
						if (deleteMethodSignResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<SFH_ZhuCeWnd>("WFG_ZhuCeWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodSignResp_.Code));
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private void InitDetail()
		{
			if (m_DataSource == null)
			{
				return;
			}
			Singleton<RoleManager>.Instance.SelectTemplateSignDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> detail) =>
			{
				GridLayoutGroup component = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
				foreach (string item in detail)
				{
					string[] array = item.Split(',');
					DST_ZhuCeTipItem component2 = SetListContentParent(Panel.ScrollViewContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
					if (array != null && array.Length == 2)
					{
						component2?.SetValue(array[0] + "：", array[1], "");
					}
					else if (array != null && array.Length == 3)
					{
						component2?.SetValue(array[0] + "：", array[1], array[2]);
					}
				}
				SetGrticalListContentHight(detail.Count, Panel.ScrollViewContent, component);
			});
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenBtnClick()
		{
			RegisterMethodSignReq_1008050 data = new RegisterMethodSignReq_1008050
			{
				MethodSignId = m_DataSource.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.FUSHU_ZHUCE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterMethodSignResp_1008050 registerMethodSignResp_ = new RegisterMethodSignResp_1008050();
					registerMethodSignResp_.MergeFrom(byteString);
					if (registerMethodSignResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<SFH_ZhuCeWnd>("WFG_ZhuCeWnd.prefab")?.InitData();
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodSignResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
