using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuanXinWnd : Window
	{
		public ChuanXinPanel Panel;

		public List<RoleMailInfoModel> currentMails;

		private MainCityWnd m_MainCityWnd;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChuanXinPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseThisWnd);
			AddButtonClickListener(Panel.PopUpCloseBtn, () =>
			{
				Panel.PopUpWnd.gameObject.SetActive(value: false);
			});
			AddButtonClickListener(Panel.YiJianShanChuBtn, YiJianShanChuBtnClick);
			AddButtonClickListener(Panel.YiJianLingQuBtn, YiJianLingQuBtnClick);
			ResourceLoad();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			currentMails = new List<RoleMailInfoModel>();
			m_MainCityWnd = Singleton<UIManager>.Instance.FindWndByName<MainCityWnd>("MainCityWnd.prefab");
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_QUERY_LETTERS, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					LetterQueryResp_1003001 letterQueryResp_ = new LetterQueryResp_1003001();
					letterQueryResp_.MergeFrom(byteString);
					if (letterQueryResp_.IsSuccess)
					{
						foreach (LetterQueryResp_1003001.Types.LetterInfoResp_1003002 letter in letterQueryResp_.Letters)
						{
							RoleMailInfoModel roleMailInfoModel = new RoleMailInfoModel
							{
								c = letter.Content,
								h = letter.HasAward,
								i = letter.Id,
								s = letter.Status,
								title = letter.Title,
								isGetAll = letter.IsGetAll
							};
							roleMailInfoModel.l = new List<XzjRoleLetterAwardVO>();
							foreach (LetterQueryResp_1003001.Types.LetterInfoResp_1003002.Types.LetterAwardResp_1003003 award in letter.Awards)
							{
								roleMailInfoModel.l.Add(new XzjRoleLetterAwardVO
								{
									count = award.GoodsCount,
									id = award.Id,
									name = award.GoodsName,
									goodsId = award.GoodsId,
									type = award.GoodsType,
									status = award.Status,
									rank = award.Rank
								});
							}
							currentMails.Add(roleMailInfoModel);
						}
						Init();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(letterQueryResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void Init()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChuanXinItem.prefab");
			if (currentMails == null || currentMails.Count == 0)
			{
				Panel.NullBg.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.NullBg.gameObject.SetActive(value: false);
			}
			ClearChuanXinWndData();
			foreach (RoleMailInfoModel item in currentMails)
			{
				GameObject gameObject = SetListContentParent(Panel.Content, insObj);
				ChuanXinItem ci = gameObject.GetComponent<ChuanXinItem>();
				if (ci == null || item == null)
				{
					return;
				}
				string title = item.title;
				ci.SetValue(item.i, item.s, "信件", title, () =>
				{
					ReadMailDetail(ci, item);
				});
			}
			if (currentMails.Count > 0)
			{
				float y = currentMails.Count * 34 + (currentMails.Count - 1) * 4;
				Panel.Content.sizeDelta = new Vector2(Panel.Content.sizeDelta.x, y);
			}
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
		}

		private void ReadMailDetail(ChuanXinItem ci, RoleMailInfoModel item)
		{
			ReadLetterReq_1003005 data = new ReadLetterReq_1003005
			{
				Id = item.i
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_READ_LETTER, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ActionBoolResp_1001001 actionBoolResp_ = new ActionBoolResp_1001001();
					actionBoolResp_.MergeFrom(byteString);
					if (actionBoolResp_.IsSuccess)
					{
						ci.SetReaded();
						SetPopUpValue(item);
						Panel.PopUpWnd.gameObject.SetActive(value: true);
						item.s = true;
						Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter = false;
						if (currentMails != null)
						{
							for (int i = 0; i < currentMails.Count; i++)
							{
								if (!currentMails[i].s)
								{
									Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter = true;
									break;
								}
							}
						}
						m_MainCityWnd?.ReflashMail(Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public void ClearPopUpWndData()
		{
			foreach (RectTransform item in Panel.PopScrollContent)
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void ClearChuanXinWndData()
		{
			foreach (RectTransform item in Panel.Content)
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void SetPopUpValue(RoleMailInfoModel _dataSource)
		{
			ClearPopUpWndData();
			Panel.PopUpTextScrollContent.GetComponent<Text>().text = _dataSource.c;
			Panel.TitleText.text = _dataSource.title;
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ChuanXinGoodsItem.prefab");
			Panel.PopUpLingQuBtn.interactable = false;
			if (_dataSource.l != null)
			{
				for (int i = 0; i < _dataSource.l.Count; i++)
				{
					ChuanXinGoodsItem component = SetListContentParent(Panel.PopScrollContent, insObj).GetComponent<ChuanXinGoodsItem>();
					Sprite value = null;
					m_GoodsIconSprite.TryGetValue(_dataSource.l[i].type, out value);
					int levelByRank = WorldSetting.GetLevelByRank(_dataSource.l[i].rank);
					Sprite value2 = null;
					m_RankBgSprite.TryGetValue(levelByRank, out value2);
					component.SetValue(_dataSource.l[i].name, _dataSource.l[i].count.ToString(), value2, value, _dataSource.l[i].status);
					if (!_dataSource.l[i].status)
					{
						Panel.PopUpLingQuBtn.interactable = true;
					}
				}
				GridLayoutGroup component2 = Panel.PopScrollContent.gameObject.GetComponent<GridLayoutGroup>();
				SetGrticalListContentHight(_dataSource.l.Count, Panel.PopScrollContent, component2);
			}
			Panel.PopUpLingQuBtn.onClick.RemoveAllListeners();
			AddButtonClickListener(Panel.PopUpLingQuBtn, () =>
			{
				PopUpLingQuBtnClick(_dataSource, () =>
				{
					Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData();
				});
			});
		}

		private void PopUpLingQuBtnClick(RoleMailInfoModel _dataSource, UnityAction LingQuSuccessCallBack = null)
		{
			_dataSource.l.Sum((XzjRoleLetterAwardVO t) => t.volume);
			ReceiveLetterReq_1003006 data = new ReceiveLetterReq_1003006
			{
				Id = _dataSource.i
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_RECEIVE_LETTER, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ReceiveLetterResp_1003006 receiveLetterResp_ = new ReceiveLetterResp_1003006();
					receiveLetterResp_.MergeFrom(byteString);
					if (receiveLetterResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = receiveLetterResp_.ShinyStone;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)receiveLetterResp_.ShinyJade;
						Panel.PopUpWnd.gameObject.SetActive(value: false);
						if (currentMails != null)
						{
							for (int i = 0; i < currentMails.Count; i++)
							{
								if (_dataSource != null && currentMails[i].i == _dataSource.i)
								{
									currentMails[i].s = true;
									break;
								}
							}
						}
						if (_dataSource != null)
						{
							foreach (XzjRoleLetterAwardVO item in _dataSource.l)
							{
								item.status = true;
							}
						}
						foreach (Transform item2 in Panel.Content)
						{
							Object.Destroy(item2.gameObject);
						}
						Init();
						Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter = false;
						if (currentMails != null)
						{
							for (int j = 0; j < currentMails.Count; j++)
							{
								if (!currentMails[j].s)
								{
									Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter = true;
									break;
								}
							}
						}
						m_MainCityWnd?.ReflashMail(Singleton<RoleManager>.Instance.Role_Entity.IsHaveNewLetter);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						if (LingQuSuccessCallBack != null)
						{
							LingQuSuccessCallBack();
						}
					}
					else
					{
						OnShow();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(receiveLetterResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public void YiJianShanChuBtnClick()
		{
			List<RoleMailInfoModel> list = new List<RoleMailInfoModel>();
			DeleteLetterReq_1003004 deleteLetterReq_ = new DeleteLetterReq_1003004();
			foreach (RoleMailInfoModel currentMail in currentMails)
			{
				if (!currentMail.s)
				{
					continue;
				}
				bool flag = true;
				if (currentMail.l != null && currentMail.l.Count > 0)
				{
					foreach (XzjRoleLetterAwardVO item in currentMail.l)
					{
						if (!item.status)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					deleteLetterReq_.Id.Add(currentMail.i);
					list.Add(currentMail);
				}
			}
			if (list.Count > 0)
			{
				MailDelete(deleteLetterReq_, list);
			}
		}

		private void MailDelete(DeleteLetterReq_1003004 deleteLetterReq_1003004, List<RoleMailInfoModel> _waitDel)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_DELETE_LETTER, deleteLetterReq_1003004, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ActionBoolResp_1001001 actionBoolResp_ = new ActionBoolResp_1001001();
					actionBoolResp_.MergeFrom(byteString);
					if (actionBoolResp_.IsSuccess)
					{
						DeleteCompelete(_waitDel);
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void DeleteCompelete(List<RoleMailInfoModel> _waitDel)
		{
			foreach (RoleMailInfoModel item in _waitDel)
			{
				foreach (Transform item2 in Panel.Content)
				{
					if (item2.gameObject.GetComponent<ChuanXinItem>().MailId == item.i)
					{
						Object.Destroy(item2.gameObject);
						break;
					}
				}
				currentMails.Remove(item);
			}
			if (currentMails.Count <= 0)
			{
				Panel.NullBg.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.NullBg.gameObject.SetActive(value: false);
			}
		}

		public void YiJianLingQuBtnClick()
		{
			List<RoleMailInfoModel> list = new List<RoleMailInfoModel>();
			foreach (RoleMailInfoModel currentMail in currentMails)
			{
				if (currentMail.l != null && currentMail.l.Count > 0 && !currentMail.isGetAll)
				{
					list.Add(currentMail);
				}
			}
			if (list.Count != 0)
			{
				int i = 0;
				LingQuAllMail(i, list);
			}
		}

		private void LingQuAllMail(int i, List<RoleMailInfoModel> dataList)
		{
			PopUpLingQuBtnClick(dataList[i], () =>
			{
				string i2 = dataList[i].i;
				foreach (Transform item in Panel.Content)
				{
					ChuanXinItem component = item.gameObject.GetComponent<ChuanXinItem>();
					if (component.MailId == i2)
					{
						component?.SetReaded();
						m_MainCityWnd?.ReflashMail(isNew: false);
						break;
					}
				}
				i++;
				if (i >= dataList.Count)
				{
					Singleton<UIManager>.Instance.FindWndByName<NaJieSpaceWnd>("NaJieSpaceWnd.prefab")?.FlashData();
				}
				else
				{
					LingQuAllMail(i, dataList);
				}
			});
		}
	}
}
