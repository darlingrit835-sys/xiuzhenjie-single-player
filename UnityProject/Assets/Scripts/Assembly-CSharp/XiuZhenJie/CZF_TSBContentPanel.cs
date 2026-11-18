using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class CZF_TSBContentPanel : MonoBehaviour
	{
		public RectTransform Content;

		private GameObject m_Tsb_Item;

		private Window m_ParentWnd;

		public void InitPanel(Window window)
		{
			m_Tsb_Item = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/TSB_Item.prefab");
			m_ParentWnd = window;
			Singleton<NetManager>.Instance.SendMsg(MsgType.OPEN_TIANSHUBEI, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					OpenTianShuBeiResp_1016004 openTianShuBeiResp_ = new OpenTianShuBeiResp_1016004();
					openTianShuBeiResp_.MergeFrom(byteString);
					if (openTianShuBeiResp_.IsSuccess)
					{
						List<TSB_ItemEntity> list = new List<TSB_ItemEntity>();
						foreach (OpenTianShuBeiResp_1016004.Types.TianShuBeiObjResp_1016005 item2 in openTianShuBeiResp_.TianShuBeiObj)
						{
							TSB_ItemEntity item = new TSB_ItemEntity
							{
								CurrentProgress = item2.CurrentVal,
								MaxProgress = item2.MaxVal,
								Id = item2.Id,
								Type = item2.Type,
								isCompelete = (item2.Status == 1)
							};
							list.Add(item);
						}
						CreateContent(list);
					}
				}
			}, isShowWaitUI: true);
		}

		public void CreateContent(List<TSB_ItemEntity> dataList)
		{
			foreach (TSB_ItemEntity data in dataList)
			{
				TSB_Item component = m_ParentWnd.SetListContentParent(Content, m_Tsb_Item).GetComponent<TSB_Item>();
				string titleName = "";
				string titleContent = "";
				GetContentTitle(data.Type, ref titleName, ref titleContent);
				component.SetItem(data.Id, data.Type, titleName, titleContent, data.CurrentProgress, data.MaxProgress, data.isCompelete, LingQuBtnClick);
			}
		}

		private void GetContentTitle(int type, ref string titleName, ref string titleContent)
		{
			switch (type)
			{
			case 1:
				titleName = "财";
				titleContent = "获得{0}灵石";
				break;
			case 2:
				titleName = "侣";
				titleContent = "结交{0}位好友";
				break;
			case 3:
				titleName = "法";
				titleContent = "参悟{0}本书籍";
				break;
			case 4:
				titleName = "地";
				titleContent = "练功房灵气效率{0}";
				break;
			case 5:
				titleName = "杀";
				titleContent = "击杀{0}只妖兽";
				break;
			case 6:
				titleName = "缘";
				titleContent = "完成{0}次随机事件";
				break;
			}
		}

		private void LingQuBtnClick(int id, int type, TSB_Item itemObj)
		{
			TianShuBeiReceiveAwardReq_1016006 data = new TianShuBeiReceiveAwardReq_1016006
			{
				Id = id,
				Type = type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TIANSHUBEI_RECEIVE_AWARD, data, (object _Obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _Obj as ByteString;
				if (!(byteString == null))
				{
					TianShuBeiReceiveAwardResp_1016006 tianShuBeiReceiveAwardResp_ = new TianShuBeiReceiveAwardResp_1016006();
					tianShuBeiReceiveAwardResp_.MergeFrom(byteString);
					if (tianShuBeiReceiveAwardResp_.IsSuccess)
					{
						string titleName = "";
						string titleContent = "";
						GetContentTitle(type, ref titleName, ref titleContent);
						itemObj.ResetItem(tianShuBeiReceiveAwardResp_.Id, tianShuBeiReceiveAwardResp_.Status, titleContent, tianShuBeiReceiveAwardResp_.MaxVal);
						Singleton<RoleManager>.Instance.Role_Entity.TianShuBeiStatus = 2;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(tianShuBeiReceiveAwardResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
