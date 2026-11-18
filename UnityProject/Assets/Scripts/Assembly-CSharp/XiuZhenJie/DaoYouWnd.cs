using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DaoYouWnd : Window
	{
		public DaoYouPanel Panel;

		private GameObject DaoYouFriendItem;

		private GameObject UnKnowDaoYouItem;

		private int m_StrangerCount;

		private int m_OnlineFriendCount;

		private Dictionary<string, int> FriendNoReadDic;

		private Dictionary<string, DaoYouItem> FriendNoReadEntityDic;

		private Dictionary<string, MingCiItem> MingCiEntityDic;

		private List<DaoYouEntity> AllFriendList;

		private List<MingCiEntity> AllMingCiList;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DaoYouPanel>();
			AddToggleClickListener(Panel.BaiHuiMingCiBtn, BaiHuiMingCiBtnClick);
			AddButtonClickListener(Panel.JieShiDaoYouBtn, JieShiDaoYouBtnClick);
			AddButtonClickListener(Panel.SouSuoBtn, SelectDaoYou);
			AddButtonClickListener(Panel.CloseBtn, CloseSouSuoPanel);
			AddButtonClickListener(Panel.DaoLvBtn, DaoLvButton);
			AddButtonClickListener(Panel.ShiTuBtn, ShiTuButton);
			FriendNoReadEntityDic = new Dictionary<string, DaoYouItem>();
			AllFriendList = new List<DaoYouEntity>();
			AllMingCiList = new List<MingCiEntity>();
			MingCiEntityDic = new Dictionary<string, MingCiItem>();
			DaoYouFriendItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DaoYouItem.prefab");
			UnKnowDaoYouItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/MingCiItem.prefab");
			ScrollViewInfiniteVertical daoYouVerticalContent = Panel.DaoYouVerticalContent;
			daoYouVerticalContent.DownScrollAction = (UnityAction<GameObject>)Delegate.Combine(daoYouVerticalContent.DownScrollAction, new UnityAction<GameObject>(ContentDownScroll));
			ScrollViewInfiniteVertical daoYouVerticalContent2 = Panel.DaoYouVerticalContent;
			daoYouVerticalContent2.UpScrollAction = (UnityAction<GameObject>)Delegate.Combine(daoYouVerticalContent2.UpScrollAction, new UnityAction<GameObject>(ContentUpScroll));
			ScrollViewInfiniteVertical mingCiVerticalContent = Panel.MingCiVerticalContent;
			mingCiVerticalContent.DownScrollAction = (UnityAction<GameObject>)Delegate.Combine(mingCiVerticalContent.DownScrollAction, new UnityAction<GameObject>(MingCiContentDownScroll));
			ScrollViewInfiniteVertical mingCiVerticalContent2 = Panel.MingCiVerticalContent;
			mingCiVerticalContent2.UpScrollAction = (UnityAction<GameObject>)Delegate.Combine(mingCiVerticalContent2.UpScrollAction, new UnityAction<GameObject>(MingCiContentUpScroll));
			FriendNoReadDic = new Dictionary<string, int>();
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveMsgAction = (UnityAction<FriendEntity>)Delegate.Combine(instance.ReceiveMsgAction, new UnityAction<FriendEntity>(Receive));
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			GetFriendData();
		}

		private void CloseSouSuoPanel()
		{
			Panel.JieShiDaoYouPanel.gameObject.SetActive(value: false);
		}

		private void Receive(FriendEntity friendEntity)
		{
			if (Singleton<UIManager>.Instance.FindWndByName<DaoYouChatWnd>("DaoYouChatWnd.prefab") == null)
			{
				if (FriendNoReadDic.ContainsKey(friendEntity.FromRoleId))
				{
					FriendNoReadDic[friendEntity.FromRoleId]++;
				}
				if (FriendNoReadEntityDic.ContainsKey(friendEntity.FromRoleId))
				{
					FriendNoReadEntityDic[friendEntity.FromRoleId].AddOneNoReadMsg();
				}
			}
		}

		public void DelectFriend(string roleId)
		{
			if (FriendNoReadDic.ContainsKey(roleId))
			{
				FriendNoReadDic.Remove(roleId);
			}
		}

		private void GetFriendData()
		{
			if (Panel.DaoYouScrollContent != null)
			{
				foreach (Transform item in Panel.DaoYouScrollContent)
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
			AllFriendList.Clear();
			FriendNoReadEntityDic.Clear();
			FriendNoReadDic.Clear();
			m_OnlineFriendCount = 0;
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_FRIEND, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterFriendResp_1014001 enterFriendResp_ = new EnterFriendResp_1014001();
					enterFriendResp_.MergeFrom(byteString);
					if (enterFriendResp_.IsSuccess)
					{
						GetFriendDataSuccess(enterFriendResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterFriendResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GetFriendDataSuccess(EnterFriendResp_1014001 enterFriendResp_1014001)
		{
			m_StrangerCount = enterFriendResp_1014001.StrangerCount;
			if (m_StrangerCount > 0 && !Panel.BaiHuiMingCiBtn.isOn)
			{
				Panel.MingCiNumText.text = m_StrangerCount.ToString();
				Panel.MingCiNumImg.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.MingCiNumImg.gameObject.SetActive(value: false);
			}
			List<string> list = new List<string>();
			if (enterFriendResp_1014001.Friend != null && enterFriendResp_1014001.Friend.Count > 0)
			{
				foreach (EnterFriendResp_1014001.Types.FriendResp_1014002 item in enterFriendResp_1014001.Friend)
				{
					list.Add(item.Id);
				}
			}
			if (list.Count == 0)
			{
				Panel.MinTitleText.text = $"共有道友{0}人,在线道友{0}人";
			}
			else
			{
				GetFirendOnlineData(enterFriendResp_1014001);
			}
		}

		private void GetFirendOnlineData(EnterFriendResp_1014001 enterFriendResp_1014001)
		{
			foreach (EnterFriendResp_1014001.Types.FriendResp_1014002 item2 in enterFriendResp_1014001.Friend)
			{
				if (item2.IsOnLine)
				{
					m_OnlineFriendCount++;
				}
				List<int> list = new List<int>();
				foreach (int item3 in item2.LiveType)
				{
					list.Add(item3);
				}
				DaoYouEntity item = new DaoYouEntity
				{
					headPortraitId = item2.HeadPortraitId,
					Id = item2.Id,
					Intimacy = item2.Intimacy,
					Level = item2.Level,
					Name = item2.Name,
					Sex = (item2.Sex ? 1 : 0),
					LiveType = list,
					IsOnline = item2.IsOnLine
				};
				AllFriendList.Add(item);
				if (!FriendNoReadDic.ContainsKey(item2.Id))
				{
					if (Singleton<ChatManager>.Instance.ChatRoleMsgDic.ContainsKey(item2.Id))
					{
						int count = Singleton<ChatManager>.Instance.ChatRoleMsgDic[item2.Id].Count;
						FriendNoReadDic.Add(item2.Id, count);
					}
					else
					{
						FriendNoReadDic.Add(item2.Id, 0);
					}
				}
			}
			AllFriendList.Sort((DaoYouEntity x, DaoYouEntity y) => -x.IsOnline.CompareTo(y.IsOnline));
			CreateFriendData();
			Panel.MinTitleText.text = $"共有道友{AllFriendList.Count}人,在线道友{m_OnlineFriendCount}人";
		}

		private void CreateFriendData()
		{
			if (AllFriendList != null && AllFriendList.Count > 0)
			{
				for (int i = 0; i < AllFriendList.Count; i++)
				{
					if (i < 7)
					{
						CreateDaoYouItem(i, AllFriendList[i]);
					}
				}
			}
			float y = SetVerticalListContentHight(AllFriendList.Count, 75f, 0f, 6f);
			RectTransform component = Panel.DaoYouScrollContent.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			Panel.DaoYouVerticalContent.SetCompelete(AllFriendList.Count);
		}

		private void CreateDaoYouItem(int index, DaoYouEntity item)
		{
			DaoYouItem component = SetListContentParent(Panel.DaoYouScrollContent, DaoYouFriendItem).GetComponent<DaoYouItem>();
			SetItemValue(index, component, item);
			if (!FriendNoReadEntityDic.ContainsKey(item.Id))
			{
				FriendNoReadEntityDic.Add(item.Id, component);
			}
		}

		private void SetItemValue(int index, DaoYouItem daoYouItem, DaoYouEntity item)
		{
			RoleFuZhiType fuZhi_One = RoleFuZhiType.None;
			RoleFuZhiType fuzhi_Two = RoleFuZhiType.None;
			if (item.LiveType != null && item.LiveType.Count >= 2)
			{
				fuZhi_One = (RoleFuZhiType)item.LiveType[0];
				fuzhi_Two = (RoleFuZhiType)item.LiveType[1];
			}
			int unReadMsgNum = 0;
			if (Singleton<ChatManager>.Instance.ChatRoleMsgDic.ContainsKey(item.Id))
			{
				unReadMsgNum = Singleton<ChatManager>.Instance.ChatRoleMsgDic[item.Id].Count;
			}
			daoYouItem.SetValue(index, item.Id, item.IsOnline, unReadMsgNum, item.Sex == 1, item.headPortraitId, item.Name, item.Level, item.Intimacy, ChuanYinToFriend, ChuanWuToFriend, fuZhi_One, fuzhi_Two);
		}

		private void ContentDownScroll(GameObject gameObj)
		{
			if (!gameObj.Equals(null))
			{
				DaoYouItem component = gameObj.GetComponent<DaoYouItem>();
				if (!component.Equals(null))
				{
					int index = component.index;
					int targetIndex = ((index + 7 > AllFriendList.Count - 1) ? (AllFriendList.Count - 1) : (index + 7));
					ScrollRestValue(targetIndex, component);
				}
			}
		}

		private void ContentUpScroll(GameObject gameObj)
		{
			DaoYouItem component = gameObj.GetComponent<DaoYouItem>();
			int index = component.index;
			int targetIndex = ((index - 7 >= 0) ? (index - 7) : 0);
			ScrollRestValue(targetIndex, component);
		}

		private void ScrollRestValue(int targetIndex, DaoYouItem daoYouItem)
		{
			DaoYouEntity daoYouEntity = AllFriendList[targetIndex];
			string friendRoleId = daoYouItem.FriendRoleId;
			SetItemValue(targetIndex, daoYouItem, daoYouEntity);
			if (FriendNoReadEntityDic.ContainsKey(friendRoleId))
			{
				FriendNoReadEntityDic.Remove(friendRoleId);
			}
			if (!FriendNoReadEntityDic.ContainsKey(daoYouEntity.Id))
			{
				FriendNoReadEntityDic.Add(daoYouEntity.Id, daoYouItem);
			}
		}

		private void BaiHuiMingCiBtnClick(bool isOn)
		{
			if (isOn)
			{
				Panel.MingCiNumImg.gameObject.SetActive(value: false);
				Panel.BaiHuiMingCiText.color = Color.white;
				Panel.MingCiScroll.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
				GetMingCiData();
				return;
			}
			foreach (Transform item in Panel.MingCiScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			Panel.BaiHuiMingCiText.color = Color.black;
			Panel.MingCiScroll.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1000f);
			RectTransform component = Panel.MingCiScrollContent.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, 0f);
			if (m_StrangerCount > 0)
			{
				Panel.MingCiNumText.text = m_StrangerCount.ToString();
				Panel.MingCiNumImg.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.MingCiNumImg.gameObject.SetActive(value: false);
			}
		}

		private void GetMingCiData()
		{
			foreach (Transform item in Panel.MingCiScrollContent)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_ADDFRIEND, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterAddFriendResp_1014003 enterAddFriendResp_ = new EnterAddFriendResp_1014003();
					enterAddFriendResp_.MergeFrom(byteString);
					if (enterAddFriendResp_.IsSuccess)
					{
						AllMingCiList.Clear();
						for (int i = 0; i < enterAddFriendResp_.Stranger.Count; i++)
						{
							MingCiEntity mingCiEntity = new MingCiEntity
							{
								headPortraitId = enterAddFriendResp_.Stranger[i].HeadPortraitId,
								Id = enterAddFriendResp_.Stranger[i].Id,
								Level = enterAddFriendResp_.Stranger[i].Level,
								LiveType = new List<int>(),
								Name = enterAddFriendResp_.Stranger[i].Name,
								Sex = (enterAddFriendResp_.Stranger[i].Sex ? 1 : 0)
							};
							foreach (int item2 in enterAddFriendResp_.Stranger[i].LiveType)
							{
								mingCiEntity.LiveType.Add(item2);
							}
							AllMingCiList.Add(mingCiEntity);
							if (i < 7)
							{
								CreateMingCiItem(i, mingCiEntity);
							}
						}
						SetMingCiContentHight();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterAddFriendResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetMingCiContentHight()
		{
			float y = SetVerticalListContentHight(AllMingCiList.Count, 75f, 0f, 6f);
			RectTransform component = Panel.MingCiScrollContent.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			Panel.MingCiVerticalContent.SetCompelete(AllMingCiList.Count);
		}

		private void MingCiContentDownScroll(GameObject gameObj)
		{
			MingCiItem component = gameObj.GetComponent<MingCiItem>();
			int targetIndex = Panel.MingCiVerticalContent.transform.GetChild(5).GetComponent<MingCiItem>().index + 1;
			MingCiScrollRestValue(targetIndex, component);
		}

		private void MingCiContentUpScroll(GameObject gameObj)
		{
			MingCiItem component = gameObj.GetComponent<MingCiItem>();
			int targetIndex = Panel.MingCiVerticalContent.transform.GetChild(1).GetComponent<MingCiItem>().index - 1;
			MingCiScrollRestValue(targetIndex, component);
		}

		private void MingCiScrollRestValue(int targetIndex, MingCiItem mingCiItem)
		{
			if (AllMingCiList.Count > targetIndex && targetIndex >= 0)
			{
				MingCiEntity mingCiEntity = AllMingCiList[targetIndex];
				string friendRoleId = mingCiItem.FriendRoleId;
				SetItemMingCiValue(targetIndex, mingCiItem, mingCiEntity);
				if (MingCiEntityDic.ContainsKey(friendRoleId))
				{
					MingCiEntityDic.Remove(friendRoleId);
				}
				if (!MingCiEntityDic.ContainsKey(mingCiEntity.Id))
				{
					MingCiEntityDic.Add(mingCiEntity.Id, mingCiItem);
				}
			}
		}

		private void SetItemMingCiValue(int index, MingCiItem mingCiItem, MingCiEntity item)
		{
			RoleFuZhiType fuZhi_One = RoleFuZhiType.None;
			RoleFuZhiType fuzhi_Two = RoleFuZhiType.None;
			if (item.LiveType != null && item.LiveType.Count >= 2)
			{
				fuZhi_One = (RoleFuZhiType)item.LiveType[0];
				fuzhi_Two = (RoleFuZhiType)item.LiveType[1];
			}
			mingCiItem.SetValue(index, item.Id, isOnline: false, item.Sex == 1, item.headPortraitId, item.Name, item.Level, JieShouFriend, RefusedToFriend, fuZhi_One, fuzhi_Two);
		}

		private void CreateMingCiItem(int index, MingCiEntity item)
		{
			MingCiItem component = SetListContentParent(Panel.MingCiScrollContent, UnKnowDaoYouItem).GetComponent<MingCiItem>();
			SetItemMingCiValue(index, component, item);
			if (!MingCiEntityDic.ContainsKey(item.Id))
			{
				MingCiEntityDic.Add(item.Id, component);
			}
		}

		private void JieShiDaoYouBtnClick()
		{
			Panel.JieShiDaoYouPanel.gameObject.SetActive(value: true);
			Panel.SendBtn.gameObject.SetActive(value: false);
			Panel.SouSuoBtn.gameObject.SetActive(value: true);
			Panel.JueSeMing.gameObject.SetActive(value: true);
			Panel.PlayerInfoRect.gameObject.SetActive(value: false);
		}

		private void ChuanWuToFriend(DaoYouFirendEntity daoYouFirendEntity)
		{
			Singleton<UIManager>.Instance.PopUpWnd("DaoYouChuanWuWnd.prefab", true, daoYouFirendEntity.RoleId, daoYouFirendEntity.RoleName);
		}

		private void ChuanYinToFriend(DaoYouFirendEntity daoYouFirendEntity)
		{
			if (FriendNoReadDic.ContainsKey(daoYouFirendEntity.RoleId))
			{
				FriendNoReadDic[daoYouFirendEntity.RoleId] = 0;
			}
			Singleton<UIManager>.Instance.PopUpWnd("DaoYouChatWnd.prefab", true, daoYouFirendEntity.RoleId, daoYouFirendEntity.RoleName, daoYouFirendEntity.RoleSex, daoYouFirendEntity.RoleHeadId);
		}

		private void JieShouFriend(int index, GameObject gameObj, string id, string name)
		{
			AddFriendReq_1014006 data = new AddFriendReq_1014006
			{
				TargetFriendId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ADD_FRIEND, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AddFriendResp_1014006 addFriendResp_ = new AddFriendResp_1014006();
					addFriendResp_.MergeFrom(byteString);
					if (addFriendResp_.IsSuccess)
					{
						DeleteMingCi(index, id, gameObj);
						SetMingCiContentHight();
						GetFriendData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addFriendResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void RefusedToFriend(int index, GameObject gameObj, string id, string name)
		{
			RefuseFriendReq_1014009 data = new RefuseFriendReq_1014009
			{
				TargetFriendId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REFUSE_FRIEND, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RefuseFriendResp_1014009 refuseFriendResp_ = new RefuseFriendResp_1014009();
					refuseFriendResp_.MergeFrom(byteString);
					if (refuseFriendResp_.IsSuccess)
					{
						DeleteMingCi(index, id, gameObj);
						SetMingCiContentHight();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(refuseFriendResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DeleteMingCi(int index, string id, GameObject gameObj)
		{
			int num = index;
			MingCiItem mingCiItem = null;
			int num2 = 0;
			List<MingCiItem> list = new List<MingCiItem>();
			foreach (Transform item in Panel.MingCiScrollContent)
			{
				switch (num2)
				{
				case 0:
					mingCiItem = item.gameObject.GetComponent<MingCiItem>();
					break;
				case 1:
					if (mingCiItem == null)
					{
						mingCiItem = item.gameObject.GetComponent<MingCiItem>();
					}
					break;
				}
				list.Add(item.gameObject.GetComponent<MingCiItem>());
				num2++;
			}
			num = mingCiItem.index;
			if (AllMingCiList.Count > index)
			{
				AllMingCiList.RemoveAt(index);
			}
			if (AllMingCiList.Count >= 7)
			{
				MingCiEntityDic.Clear();
				if (7 > AllMingCiList.Count - num)
				{
					num = AllMingCiList.Count - 7;
					Panel.MingCiVerticalContent.ReduceGameObj();
				}
				for (int i = 0; i < list.Count; i++)
				{
					MingCiItem mingCiItem2 = list[i];
					MingCiEntity mingCiEntity = AllMingCiList[num + i];
					SetItemMingCiValue(num + i, mingCiItem2, mingCiEntity);
					MingCiEntityDic.Add(mingCiEntity.Id, mingCiItem2);
				}
			}
			else
			{
				if (MingCiEntityDic.ContainsKey(id))
				{
					MingCiEntityDic.Remove(id);
				}
				UnityEngine.Object.Destroy(gameObj);
			}
		}

		private void SelectDaoYou()
		{
			AddFriendByNameReq_1014007 addFriendByNameReq_ = new AddFriendByNameReq_1014007
			{
				Name = Panel.JueSeMing.text
			};
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleName.Equals(addFriendByNameReq_.Name))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("无法给向自己发送名刺！");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.ADDFRIEND_BYNAME, addFriendByNameReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AddFriendByNameResp_1014007 addFriendByNameResp_ = new AddFriendByNameResp_1014007();
					addFriendByNameResp_.MergeFrom(byteString);
					if (addFriendByNameResp_.IsSuccess)
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addFriendByNameResp_.Code);
						hintMsgByCode = "发送名刺成功!";
						Panel.JueSeMing.text = "";
						MonoSingleton<GameStart>.Instance.PopRollingTextShow(hintMsgByCode);
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addFriendByNameResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShiTuButton()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ShiTuWnd.prefab", true);
		}

		private void DaoLvButton()
		{
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("功能暂未开放！");
		}

		public override void OnDisable()
		{
			base.OnDisable();
			ScrollViewInfiniteVertical daoYouVerticalContent = Panel.DaoYouVerticalContent;
			daoYouVerticalContent.DownScrollAction = (UnityAction<GameObject>)Delegate.Remove(daoYouVerticalContent.DownScrollAction, new UnityAction<GameObject>(ContentDownScroll));
			ScrollViewInfiniteVertical daoYouVerticalContent2 = Panel.DaoYouVerticalContent;
			daoYouVerticalContent2.UpScrollAction = (UnityAction<GameObject>)Delegate.Remove(daoYouVerticalContent2.UpScrollAction, new UnityAction<GameObject>(ContentUpScroll));
			ChatManager instance = Singleton<ChatManager>.Instance;
			instance.ReceiveMsgAction = (UnityAction<FriendEntity>)Delegate.Remove(instance.ReceiveMsgAction, new UnityAction<FriendEntity>(Receive));
		}
	}
}
