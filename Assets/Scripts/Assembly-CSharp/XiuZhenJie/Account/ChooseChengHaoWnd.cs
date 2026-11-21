using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChooseChengHaoWnd : Window
	{
		public ChooseChengHaoPanel Panel;

		private GameObject m_ChooseChengHaoItemObj;

		private GameObject m_ChengHaoEffectItemObj;

		private Dictionary<string, ChengHaoEffectItemEntity> ChengHaoEffectDic;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChooseChengHaoPanel>();
			ChengHaoEffectDic = new Dictionary<string, ChengHaoEffectItemEntity>();
			m_ChooseChengHaoItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChooseChengHaoItem.prefab");
			m_ChengHaoEffectItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChengHaoEffectItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			ChengHaoInfoEntity value = null;
			Singleton<FacadeManager>.Instance.SelectChengHaoInfo()?.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId, out value);
			Panel.CurrentChooseChengHao.text = ((value == null) ? "" : value.Name);
			InitList();
		}

		private void InitList()
		{
			CreateCanUseChengHaoList();
			EffectDealWith();
		}

		private void EffectDealWith()
		{
			ChengHaoEffectDic.Clear();
			foreach (KeyValuePair<int, ChengHaoInfoEntity> item in Singleton<RoleManager>.Instance.Role_Entity.TypeChengHaoDic)
			{
				if (ChengHaoEffectDic.ContainsKey(item.Value.EffectStr))
				{
					ChengHaoEffectDic[item.Value.EffectStr].EffectValue += item.Value.EffectValue;
					continue;
				}
				ChengHaoEffectDic.Add(item.Value.EffectStr, new ChengHaoEffectItemEntity
				{
					EffectStr = item.Value.EffectStr,
					EffectValue = item.Value.EffectValue,
					Type = item.Value.Type,
					TypeName = item.Value.TypeName
				});
			}
			InitEffectItemList();
		}

		private void InitEffectItemList()
		{
			GridLayoutGroup component = Panel.EffectScrollViewConent.GetComponent<GridLayoutGroup>();
			if (ChengHaoEffectDic != null)
			{
				foreach (KeyValuePair<string, ChengHaoEffectItemEntity> item in ChengHaoEffectDic)
				{
					CreateEffectItem(item.Value);
				}
			}
			SetGrticalListContentHight(ChengHaoEffectDic.Count, Panel.EffectScrollViewConent, component);
		}

		private void CreateEffectItem(ChengHaoEffectItemEntity entity)
		{
			if (!string.IsNullOrEmpty(entity.EffectStr))
			{
				ChengHaoEffectItem component = Object.Instantiate(m_ChengHaoEffectItemObj, Panel.EffectScrollViewConent).GetComponent<ChengHaoEffectItem>();
				if (entity.TypeName == "师徒" || entity.TypeName == "宗门" || entity.TypeName == "抽奖" || entity.Type == 21)
				{
					component.SetValue(entity.EffectStr, entity.EffectValue, isPercent: false);
				}
				else
				{
					component.SetValue(entity.EffectStr, entity.EffectValue, isPercent: true);
				}
			}
		}

		private void CreateCanUseChengHaoList()
		{
			float y = m_ChooseChengHaoItemObj.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = Panel.ScrollviewContent.GetComponent<VerticalLayoutGroup>();
			Singleton<RoleManager>.Instance.Role_Entity.OwnerTitleList.Sort();
			if (Singleton<RoleManager>.Instance.Role_Entity.OwnerTitleList != null)
			{
				foreach (int ownerTitle in Singleton<RoleManager>.Instance.Role_Entity.OwnerTitleList)
				{
					Object.Instantiate(m_ChooseChengHaoItemObj, Panel.ScrollviewContent).GetComponent<ChooseChengHaoItem>().SetValue(ownerTitle, ChooseChengHao);
				}
			}
			float y2 = SetVerticalListContentHight(Singleton<RoleManager>.Instance.Role_Entity.OwnerTitleList.Count, y, component);
			Panel.ScrollviewContent.sizeDelta = new Vector2(Panel.ScrollviewContent.sizeDelta.x, y2);
		}

		private void ChooseChengHao(ChengHaoInfoEntity chengHaoInfoEntity)
		{
			TitleUseReq_1002007 data = new TitleUseReq_1002007
			{
				Title = chengHaoInfoEntity.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TitleUse, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					TitleUseResp_1002007 titleUseResp_ = new TitleUseResp_1002007();
					titleUseResp_.MergeFrom(byteString);
					if (titleUseResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.UsingTitleId = chengHaoInfoEntity.Id;
						Panel.CurrentChooseChengHao.text = chengHaoInfoEntity.Name;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(titleUseResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public override void OnClose()
		{
			foreach (Transform item in Panel.ScrollviewContent)
			{
				Object.Destroy(item.gameObject);
			}
			base.OnClose();
		}
	}
}
