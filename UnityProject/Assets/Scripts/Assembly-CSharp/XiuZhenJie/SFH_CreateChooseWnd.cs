using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class SFH_CreateChooseWnd : Window
	{
		public SFH_CreateChoosePanel Panel;

		private int m_CurrentChoosedStage;

		private GameObject m_FuLuItemObj;

		private FuLuChooseItemEntity m_ChooseData;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<SFH_CreateChoosePanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.KaiShiHuiZhiBtn, KaiShiHuiZhiBtnClick);
			AddButtonClickListener(Panel.ChaKanXiangQingBtn, ChaKanXiangQing);
			m_CurrentChoosedStage = 1;
			m_ChooseData = null;
			m_FuLuItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Common_TogItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			InitStage();
			GetFaShuData();
		}

		private void InitStage()
		{
			GridLayoutGroup component = Panel.Level_Content.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.Level_Content.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 9; i++)
			{
				SetListContentParent(Panel.Level_Content, m_FuLuItemObj).GetComponent<Common_TogItem>().SetValue(null, WorldSetting.GetStageName(i + 1), i + 1, component2, ChooseStageItem, i == 0);
			}
			SetGrticalListContentHight(9, Panel.Level_Content, component);
		}

		private void GetFaShuData()
		{
			UnderstandSkillReq_1001015 data = new UnderstandSkillReq_1001015
			{
				Stage = m_CurrentChoosedStage
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.UNDERSTAND_SKILL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UnderstandSkillResp_1001015 understandSkillResp_ = new UnderstandSkillResp_1001015();
					understandSkillResp_.MergeFrom(byteString);
					if (understandSkillResp_.IsSuccess)
					{
						List<FuLuChooseItemEntity> list = new List<FuLuChooseItemEntity>();
						foreach (UnderstandSkillResp_1001015.Types.SkillObjectResp_1001002 skill in understandSkillResp_.Skills)
						{
							list.Add(new FuLuChooseItemEntity
							{
								Id = skill.Id,
								Name = skill.Name,
								Rank = skill.Rank
							});
						}
						CreateFuLuContent(list);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(understandSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CreateFuLuContent(List<FuLuChooseItemEntity> data)
		{
			m_ChooseData = null;
			foreach (Transform item in Panel.FuLu_ScrollViewContent)
			{
				Object.Destroy(item.gameObject);
			}
			GridLayoutGroup component = Panel.FuLu_ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.FuLu_ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			int num = 0;
			foreach (FuLuChooseItemEntity datum in data)
			{
				SetListContentParent(Panel.FuLu_ScrollViewContent, m_FuLuItemObj).GetComponent<Common_TogItem>().SetValue(datum, datum.Name, num, component2, ChooseFuLuItem, num == 0);
				num++;
			}
			SetGrticalListContentHight(data.Count, Panel.FuLu_ScrollViewContent, component);
		}

		private void ChooseFuLuItem(string name, int index, object dataSource)
		{
			m_ChooseData = dataSource as FuLuChooseItemEntity;
			Panel.ChaKanXiangQingBtn.interactable = true;
			Panel.KaiShiHuiZhiBtn.interactable = true;
		}

		private void ChooseStageItem(string name, int index, object dataSource)
		{
			m_CurrentChoosedStage = index;
			Panel.ChaKanXiangQingBtn.interactable = false;
			Panel.KaiShiHuiZhiBtn.interactable = false;
			GetFaShuData();
		}

		private void KaiShiHuiZhiBtnClick()
		{
			if (m_ChooseData == null || string.IsNullOrEmpty(m_ChooseData.Id))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先选择法术!");
				return;
			}
			Singleton<UIManager>.Instance.PopUpWnd("SFH_CreateWnd.prefab", true, new FuLuEntity
			{
				Id = m_ChooseData.Id,
				Name = m_ChooseData.Name,
				Stage = m_CurrentChoosedStage,
				Rank = m_ChooseData.Rank
			});
		}

		private void ChaKanXiangQing()
		{
			if (m_ChooseData != null)
			{
				Singleton<RoleManager>.Instance.SelectTemplateSkillDetail(m_ChooseData.Id, m_ChooseData.Level, m_ChooseData.Rank, m_ChooseData.FiveElement, (List<string> list) =>
				{
					Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
					{
						TitleName = m_ChooseData.Name,
						ContentList = list
					};
					Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
				});
			}
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
