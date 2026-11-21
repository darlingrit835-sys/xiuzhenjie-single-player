using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DJSD_ZhuCeTipWnd : Window
	{
		public DJSD_ZhuCeTipPanel Panel;

		private DST_BuQuanEntity m_DataSource;

		private GameObject m_ItemObj;

		private UnityAction<string> ZhuCeSuccessAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DJSD_ZhuCeTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as DST_BuQuanEntity;
				int num = int.Parse(paralist[1].ToString());
				if (num == 1 || num == 2)
				{
					if (m_DataSource != null)
					{
						Panel.DanNameText.text = m_DataSource.Name;
					}
					Panel.DanNameText.interactable = false;
					Panel.QueRenBtn.interactable = false;
					Panel.ShanChuBtn.interactable = false;
				}
			}
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			AddButtonClickListener(Panel.ShanChuBtn, ShanChuBtnClick);
			AddButtonClickListener(Panel.WenHaoBtn, WenHaoBtnClick);
			InitDetail();
		}

		private void WenHaoBtnClick()
		{
			string text = "命名规则:\n1、至少3个字\n2、至多5个字\n3、最后一个字以“本,诀,谱,录,经,功,卷,法,书,典”结尾\n\n\n";
			Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, text, null, "命名规则", 6);
		}

		private void InitDetail()
		{
			if (m_DataSource == null)
			{
				return;
			}
			if (m_DataSource.Progress != 1)
			{
				Panel.DanNameText.text = m_DataSource.Name;
				Panel.DanNameText.interactable = false;
				Panel.QueRenBtn.interactable = true;
			}
			Singleton<RoleManager>.Instance.SelectTemplateAchievementDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> detail) =>
			{
				GridLayoutGroup component = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
				foreach (string item in detail)
				{
					DST_ZhuCeTipItem component2 = SetListContentParent(Panel.ScrollViewContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
					string[] array = item.Split(',');
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

		private void ShanChuBtnClick()
		{
			DeleteMethodAchievementReq_1008093 deleteMethodAchievementReq_1008093 = new DeleteMethodAchievementReq_1008093
			{
				MethodAchievementId = m_DataSource.Id,
				Part = m_DataSource.Progress
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodAchievement, deleteMethodAchievementReq_1008093, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodAchievementResp_1008093 deleteMethodAchievementResp_ = new DeleteMethodAchievementResp_1008093();
						deleteMethodAchievementResp_.MergeFrom(byteString);
						if (deleteMethodAchievementResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<DJSD_ZhuCeWnd>("DJSD_ZhuCeWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodAchievementResp_.Code));
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenBtnClick()
		{
			ZhuCeDanFangParamsModel zhuCeDanFangParamsModel = new ZhuCeDanFangParamsModel();
			zhuCeDanFangParamsModel.registerId = m_DataSource.Id;
			zhuCeDanFangParamsModel.name = Panel.DanNameText.text;
			if (string.IsNullOrEmpty(zhuCeDanFangParamsModel.name) || zhuCeDanFangParamsModel.name.Length == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入功法名称！");
				return;
			}
			for (int i = 0; i < zhuCeDanFangParamsModel.name.Length; i++)
			{
				if (zhuCeDanFangParamsModel.name[i] <= '\u007f')
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请输入汉字！");
					return;
				}
			}
			if (zhuCeDanFangParamsModel.name.Length < 3)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("至少需要3个字");
				return;
			}
			if (zhuCeDanFangParamsModel.name.Length > 5)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最多只能5个字");
				return;
			}
			string[] obj = new string[10] { "本", "诀", "谱", "录", "经", "功", "卷", "法", "书", "典" };
			string text = zhuCeDanFangParamsModel.name.Substring(zhuCeDanFangParamsModel.name.Length - 1, 1);
			bool flag = false;
			string[] array = obj;
			foreach (string value in array)
			{
				if (text.Equals(value))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最后一个字以“本,诀,谱,录,经,功,卷,法,书,典”结尾");
				return;
			}
			RegisterMethodAchievementReq_1008036 data = new RegisterMethodAchievementReq_1008036
			{
				MethodAchievementId = m_DataSource.Id,
				Rename = zhuCeDanFangParamsModel.name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DJSD_GONGFAZHUCE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterMethodElixirResp_1008017 registerMethodElixirResp_ = new RegisterMethodElixirResp_1008017();
					registerMethodElixirResp_.MergeFrom(byteString);
					if (registerMethodElixirResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<DJSD_ZhuCeWnd>("DJSD_ZhuCeWnd.prefab")?.InitData();
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodElixirResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
