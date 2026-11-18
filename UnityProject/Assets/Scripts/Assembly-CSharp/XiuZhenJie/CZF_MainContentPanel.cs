using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CZF_MainContentPanel : MonoBehaviour
	{
		public Text TitleText;

		public Text ContentText;

		public Text CompeleteStatuText;

		public Text CompeleteSuccess;

		public Text CompeleteRunning;

		public UI_GoodsItem Goods;

		public Button LingQuBtn;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, MissionEntity> m_MissionInfoDic;

		public void InitPanel()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			LingQuBtn.onClick.RemoveAllListeners();
			LingQuBtn.onClick.AddListener(LingQuBtnClick);
			m_MissionInfoDic = Singleton<FacadeManager>.Instance.SelectMissionInfo();
			MissionEntity value = null;
			if (m_MissionInfoDic.TryGetValue(Singleton<RoleManager>.Instance.Role_Entity.TaskId, out value))
			{
				BaseNaJieGoodEntity goods = new BaseNaJieGoodEntity
				{
					Name = value.award,
					Type = value.awardGoodsType,
					Level = value.awardGoodsStage,
					Rank = value.awardGoodsRank
				};
				SetPanelValue(value.name, value.content, value.condition, Singleton<RoleManager>.Instance.Role_Entity.TaskStatus == 1, goods);
			}
		}

		private void SetPanelValue(string Title, string Content, string CompeleteText, bool IsCompelete, BaseNaJieGoodEntity goods)
		{
			TitleText.text = Title;
			ContentText.text = Content;
			CompeleteStatuText.text = CompeleteText;
			if (IsCompelete)
			{
				CompeleteSuccess.gameObject.SetActive(value: true);
				CompeleteRunning.gameObject.SetActive(value: false);
				LingQuBtn.interactable = true;
			}
			else
			{
				CompeleteRunning.gameObject.SetActive(value: true);
				CompeleteSuccess.gameObject.SetActive(value: false);
				LingQuBtn.interactable = false;
			}
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(goods.Type, out value);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(goods.Rank), out value2);
			Goods.SetItem(goods, goods.Name, goods.FiveElement, goods.Level, value2, value, null);
		}

		private void LingQuBtnClick()
		{
			ReceiveAwardReq_1016002 data = new ReceiveAwardReq_1016002
			{
				TaskId = Singleton<RoleManager>.Instance.Role_Entity.TaskId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RECEIVE_AWARD, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ReceiveAwardResp_1016002 receiveAwardResp_ = new ReceiveAwardResp_1016002();
					receiveAwardResp_.MergeFrom(byteString);
					if (receiveAwardResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.TaskId++;
						Singleton<RoleManager>.Instance.Role_Entity.TaskStatus = receiveAwardResp_.Status;
						InitPanel();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(receiveAwardResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
