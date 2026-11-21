using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WFG_ZhuCeTipWnd : Window
	{
		public WFG_ZhuCeTipPanel Panel;

		private DST_CommonEntity m_DataSource;

		private GameObject m_ItemObj;

		private UnityAction<string> ZhuCeSuccessAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WFG_ZhuCeTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as DST_CommonEntity;
				int num = int.Parse(paralist[1].ToString());
				if (num == 1 || num == 2)
				{
					Panel.DanNameText.text = m_DataSource.Name;
					Panel.DanNameText.interactable = false;
					Panel.QueRenBtn.interactable = false;
				}
			}
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			AddButtonClickListener(Panel.WenHaoBtn, WenHaoBtnClick);
			AddButtonClickListener(Panel.ShanChuBtn, ShanChuBtnClick);
			InitDetail();
		}

		private void WenHaoBtnClick()
		{
			string text = "命名规则:\n1、至少3个字\n2、至多5个字\n3、最后一个字以 “闪,爆,破,斩,劲,杀,轰,劈,吟,击,盾,御,体,术,印,诀,伤,咒,封,解,散,毒,瘴,遁” 结尾\n\n\n";
			Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, text, null, "命名规则", 6);
		}

		private void ShanChuBtnClick()
		{
			DeleteMethodSkillReq_1008098 deleteMethodSkillReq_1008098 = new DeleteMethodSkillReq_1008098
			{
				MethodSkillId = m_DataSource.Id
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodSkill, deleteMethodSkillReq_1008098, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodSkillResp_1008098 deleteMethodSkillResp_ = new DeleteMethodSkillResp_1008098();
						deleteMethodSkillResp_.MergeFrom(byteString);
						if (deleteMethodSkillResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<WFG_ZhuCeWnd>("WFG_ZhuCeWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodSkillResp_.Code));
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
			Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> detail) =>
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
			ZhuCeDanFangParamsModel zhuCeDanFangParamsModel = new ZhuCeDanFangParamsModel();
			zhuCeDanFangParamsModel.registerId = m_DataSource.Id;
			zhuCeDanFangParamsModel.name = Panel.DanNameText.text;
			if (string.IsNullOrEmpty(zhuCeDanFangParamsModel.name) || zhuCeDanFangParamsModel.name.Length == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入法术名称！");
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
			string[] obj = new string[24]
			{
				"闪", "爆", "破", "斩", "劲", "杀", "轰", "劈", "吟", "击",
				"盾", "御", "体", "术", "印", "诀", "伤", "咒", "封", "解",
				"散", "毒", "瘴", "遁"
			};
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("最后一个字以“闪, 爆, 破, 斩, 劲, 杀, 轰, 劈, 吟, 击, 盾, 御, 体, 术, 印, 诀, 伤, 咒, 封, 解, 散, 毒, 瘴, 遁”结尾");
				return;
			}
			RegisterMethodSkillReq_1008028 data = new RegisterMethodSkillReq_1008028
			{
				MethodSkillId = m_DataSource.Id,
				Rename = zhuCeDanFangParamsModel.name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_WFG_FASHUZHUCE, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterMethodSkillResp_1008028 registerMethodSkillResp_ = new RegisterMethodSkillResp_1008028();
					registerMethodSkillResp_.MergeFrom(byteString);
					if (registerMethodSkillResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<WFG_ZhuCeWnd>("WFG_ZhuCeWnd.prefab")?.InitData();
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodSkillResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
