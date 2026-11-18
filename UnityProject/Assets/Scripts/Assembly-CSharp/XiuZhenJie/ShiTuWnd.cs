using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShiTuWnd : Window
	{
		private ShiTuPanel m_Panel;

		private int m_Intimacy;

		private Dictionary<string, ShiTuItem> m_TuDiXingXiDic;

		private List<TuDiEntity> m_TuDiXingXi;

		private Dictionary<string, ShoTuItem> m_BaiShiDiZisDic;

		private List<TuDiEntity> m_BaiShiDiZis;

		private bool m_QingAn;

		private int m_BaiShiTieNum;

		private bool m_IsGetTmcg;

		private int m_TmcgType;

		private int m_TmcgNum;

		private int m_ChuShiNum;

		private int m_QinChuanNum;

		private int m_JiMingNum;

		private GameObject ShoTuItem;

		private GameObject ShiTuItem;

		private bool m_QiHuang = true;

		private bool m_QiHuangXianShi = true;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ShiTuPanel>();
			AddButtonClickListener(m_Panel.WenHaoButton, XiaoWenHao);
			AddButtonClickListener(m_Panel.BaiShiButton, BaiShiButton);
			AddButtonClickListener(m_Panel.DaoYouButton, DaoYouButton);
			AddButtonClickListener(m_Panel.DaoLvButton, DaoLvButton);
			AddButtonClickListener(m_Panel.BaiShiListButton, BaiShiListButton);
			ShoTuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShoTuItem.prefab");
			ShiTuItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShiTuItem.prefab");
			m_BaiShiDiZisDic = new Dictionary<string, ShoTuItem>();
			m_TuDiXingXiDic = new Dictionary<string, ShiTuItem>();
			HuoQuShiFuXIngXi();
			QiHuangXianShi();
		}

		private void Shanchu()
		{
			if (m_Panel == null || m_Panel.TudiListRectTransform == null)
			{
				return;
			}
			foreach (RectTransform item in m_Panel.TudiListRectTransform.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		public void HuoQuShiFuXIngXi()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_SHITU, null, (object obj) =>
			{
				Shanchu();
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterShiTuResp_1014012 enterShiTuResp_ = new EnterShiTuResp_1014012();
					enterShiTuResp_.MergeFrom(byteString);
					if (enterShiTuResp_.IsSuccess)
					{
						m_BaiShiTieNum = enterShiTuResp_.BaiShiTieNum;
						BaiShiTieNew();
						m_IsGetTmcg = enterShiTuResp_.IsGetTmcg;
						m_TmcgType = enterShiTuResp_.TmcgType;
						m_TmcgNum = enterShiTuResp_.TmcgNum;
						Tmcg();
						m_JiMingNum = enterShiTuResp_.JiMingNum;
						m_ChuShiNum = enterShiTuResp_.ChuShiNum;
						m_QinChuanNum = enterShiTuResp_.QinChuanNum;
						TuDiNum();
						if (enterShiTuResp_.ShiFu?.Name != null)
						{
							PanDuanShiFu(enterShiTuResp_);
						}
						else
						{
							m_Panel.ShiFuBeiJinImage.gameObject.SetActive(value: false);
							m_Panel.WuShiFuImage.gameObject.SetActive(value: true);
						}
						TuDiXingXi(enterShiTuResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterShiTuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						TuiChu();
					}
				}
			}, isShowWaitUI: true);
		}

		public void ToZi(int tmcgType, int tmcgNum)
		{
			m_TmcgType = tmcgType;
			m_TmcgNum = tmcgNum;
			m_IsGetTmcg = false;
			Tmcg();
		}

		private void Tmcg()
		{
			if (m_IsGetTmcg)
			{
				return;
			}
			string textValue = "";
			switch (m_TmcgType)
			{
			case 1:
				textValue = $"师尊消耗大量精力，为你洗经伐髓，打通经脉，你的经脉点增加了{m_TmcgNum}点！";
				break;
			case 2:
				textValue = $"师尊消耗大量精力，助你快速突破境界，为你传输了功力{m_TmcgNum}点！";
				break;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention(textValue, () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.JIE_SHOU_TMCG, null, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						JieShouTMCGResp_1014025 jieShouTMCGResp_ = new JieShouTMCGResp_1014025();
						jieShouTMCGResp_.MergeFrom(byteString);
						if (jieShouTMCGResp_.IsSuccess)
						{
							m_Intimacy = jieShouTMCGResp_.Intimacy;
							Intimecy();
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(jieShouTMCGResp_.Code));
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private void HenShu()
		{
			if (m_QiHuangXianShi)
			{
				m_Panel.ZhenKai.gameObject.SetActive(value: true);
				m_Panel.BiaoTiText.text = "弟子";
			}
			else
			{
				m_Panel.ZhenKai.gameObject.SetActive(value: false);
				m_Panel.BiaoTiText.text = "拜师帖";
			}
		}

		private void QiHuangXianShi()
		{
			AddButtonClickListener(m_Panel.ShuButton, ShuBtn);
			AddButtonClickListener(m_Panel.HenButton, HenBtn);
		}

		private void ShuBtn()
		{
			m_Panel.ShuButton.gameObject.SetActive(value: false);
			m_Panel.HenButton.gameObject.SetActive(value: true);
			m_QiHuang = false;
			ShuaXing();
		}

		private void HenBtn()
		{
			m_Panel.ShuButton.gameObject.SetActive(value: true);
			m_Panel.HenButton.gameObject.SetActive(value: false);
			m_QiHuang = true;
			ShuaXing();
		}

		private void ShuaXing()
		{
			Shanchu();
			int num = 0;
			foreach (TuDiEntity item2 in m_TuDiXingXi)
			{
				TuDiEntity item = new TuDiEntity
				{
					Id = item2.Id,
					Name = item2.Name,
					HeadPortraitId = item2.HeadPortraitId,
					Level = item2.Level,
					Intimacy = item2.Intimacy,
					Sex = item2.Sex,
					IsOnline = item2.IsOnline,
					IsTongMaiOrChuanGong = item2.IsTongMaiOrChuanGong
				};
				CreateShiTuItem(num, item);
				num++;
			}
		}

		private void PanDuanShiFu(EnterShiTuResp_1014012 enterShiTuResp1014012)
		{
			m_Panel.ShiFuBeiJinImage.gameObject.SetActive(value: true);
			m_Panel.WuShiFuImage.gameObject.SetActive(value: false);
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(enterShiTuResp1014012.ShiFu.Sex, enterShiTuResp1014012.ShiFu.HeadPortraitId, enterShiTuResp1014012.ShiFu.IsOnLine);
			m_Panel.ShiFuToXiangImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = enterShiTuResp1014012.ShiFu.Name;
			m_Panel.GradeText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(enterShiTuResp1014012.ShiFu.Level);
			m_Intimacy = enterShiTuResp1014012.ShiFu.Intimacy;
			Intimecy();
			m_QingAn = enterShiTuResp1014012.ShiFu.IsQingAn;
			AddButtonClickListener(m_Panel.ChuShiButton, PanTaoBtn);
			AddButtonClickListener(m_Panel.QingAnButton, QingAnBtn);
		}

		private void Intimecy()
		{
			m_Panel.IntimacyText.text = $"亲密: {m_Intimacy}";
			m_Panel.IntimacyGradeText.text = IntimacyGradePanDuan(m_Intimacy) ?? "";
		}

		public void Intimecy(int intimacy)
		{
			m_Panel.IntimacyText.text = $"亲密: {intimacy}";
			m_Panel.IntimacyGradeText.text = IntimacyGradePanDuan(intimacy) ?? "";
		}

		private string IntimacyGradePanDuan(int intimacy)
		{
			if (intimacy >= 100)
			{
				return "亲传弟子";
			}
			return "记名弟子";
		}

		private void PanTaoBtn()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("叛逃师门后，三天之内无法再次拜师！", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.TUO_LISHIMEN, null, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						TuoLiShiMenResp_1014022 tuoLiShiMenResp_ = new TuoLiShiMenResp_1014022();
						tuoLiShiMenResp_.MergeFrom(byteString);
						if (tuoLiShiMenResp_.IsSuccess)
						{
							m_Panel.ShiFuBeiJinImage.gameObject.SetActive(value: false);
							m_Panel.WuShiFuImage.gameObject.SetActive(value: true);
						}
						else
						{
							HuoQuShiFuXIngXi();
						}
						MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(tuoLiShiMenResp_.Code));
					}
				}, isShowWaitUI: true);
			});
		}

		private void QingAnBtn()
		{
			if (m_QingAn)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("今日已请安！");
				return;
			}
			Singleton<UIManager>.Instance.PopUpWnd("ShiTuToZiWnd.prefab", false, true, ShiTuToZiList.QingAn);
		}

		private void BaiShiButton()
		{
			BaiShiReq_1014021 baiShiReq_ = new BaiShiReq_1014021
			{
				RoleName = m_Panel.NaneInput.text
			};
			if ("".Equals(baiShiReq_.RoleName))
			{
				return;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleName.Equals(baiShiReq_.RoleName))
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("无法给向自己发送拜师请求！");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.BAI_SHI, baiShiReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					BaiShiResp_1014021 baiShiResp_ = new BaiShiResp_1014021();
					baiShiResp_.MergeFrom(byteString);
					if (baiShiResp_.IsSuccess)
					{
						m_Panel.NaneInput.text = "";
						MonoSingleton<GameStart>.Instance.PopRollingTextShow("发送拜师请求成功!");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(baiShiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void TuDiNum()
		{
			m_Panel.TuDiNumText.text = $"出师弟子{m_ChuShiNum}人 亲传弟子{m_QinChuanNum}人 记名弟子{m_JiMingNum}人";
		}

		private void TuDiXingXi(EnterShiTuResp_1014012 enterShiTuResp1014012)
		{
			int num = 0;
			m_TuDiXingXi = new List<TuDiEntity>();
			foreach (EnterShiTuResp_1014012.Types.TuDiResp_1014014 tuDi in enterShiTuResp1014012.TuDis)
			{
				TuDiEntity item = new TuDiEntity
				{
					Id = tuDi.Id,
					Name = tuDi.Name,
					HeadPortraitId = tuDi.HeadPortraitId,
					Level = tuDi.Level,
					Intimacy = tuDi.Intimacy,
					Sex = (tuDi.Sex ? 1 : 0),
					IsOnline = tuDi.IsOnLine,
					IsTongMaiOrChuanGong = tuDi.IsTongMaiOrChuanGong
				};
				CreateShiTuItem(num, item);
				m_TuDiXingXi.Add(item);
				num++;
			}
		}

		private void CreateShiTuItem(int index, TuDiEntity item)
		{
			ShiTuItem component = SetListContentParent(m_Panel.TudiListRectTransform, ShiTuItem).GetComponent<ShiTuItem>();
			SetItemValue(index, component, item);
			if (!m_TuDiXingXiDic.ContainsKey(item.Id))
			{
				m_TuDiXingXiDic.Add(item.Id, component);
			}
		}

		private void SetItemValue(int index, ShiTuItem daoYouItem, TuDiEntity item)
		{
			if (m_QiHuang)
			{
				daoYouItem.SetValue(index, item);
			}
			else
			{
				daoYouItem.ZuChuSetValue(index, item);
			}
		}

		private void BaiShiTieNew()
		{
			if (m_BaiShiTieNum > 0)
			{
				m_Panel.BaiShiListImage.gameObject.SetActive(value: true);
				m_Panel.BaiShiListText.text = $"{m_BaiShiTieNum}";
			}
			else
			{
				m_Panel.BaiShiListImage.gameObject.SetActive(value: false);
			}
		}

		private void BaiShiListButton()
		{
			if (m_QiHuangXianShi)
			{
				m_Panel.PlayerInfoImage.gameObject.SetActive(value: true);
				foreach (Transform item in m_Panel.PlayerInfoRect)
				{
					Object.Destroy(item.gameObject);
				}
				Singleton<NetManager>.Instance.SendMsg(MsgType.BAI_SHITIE, null, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						BaiShiTieResp_1014018 baiShiTieResp_ = new BaiShiTieResp_1014018();
						baiShiTieResp_.MergeFrom(byteString);
						if (baiShiTieResp_.IsSuccess)
						{
							BaiShiXingXi(baiShiTieResp_);
							m_QiHuangXianShi = false;
							HenShu();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(baiShiTieResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			}
			else
			{
				m_Panel.PlayerInfoImage.gameObject.SetActive(value: false);
				m_QiHuangXianShi = true;
				HenShu();
			}
		}

		private void BaiShiXingXi(BaiShiTieResp_1014018 baiShiTieResp1014018)
		{
			int num = 0;
			m_BaiShiDiZis = new List<TuDiEntity>();
			foreach (BaiShiTieResp_1014018.Types.BaiShiDiZiResp_1014019 baiShiDiZi in baiShiTieResp1014018.BaiShiDiZis)
			{
				TuDiEntity item = new TuDiEntity
				{
					Id = baiShiDiZi.Id,
					Name = baiShiDiZi.Name,
					HeadPortraitId = baiShiDiZi.HeadPortraitId,
					Level = baiShiDiZi.Level,
					Sex = (baiShiDiZi.Sex ? 1 : 0)
				};
				m_BaiShiDiZis.Add(item);
				CreateMingCiItem(num, item);
				num++;
			}
			m_BaiShiTieNum = m_BaiShiDiZisDic.Count;
			BaiShiTieNew();
			m_Panel.PlayerInfoRect.sizeDelta = new Vector2(m_Panel.PlayerInfoRect.sizeDelta.x, (float)m_BaiShiDiZisDic.Count * 80f);
		}

		private void SetMingCiContentHight()
		{
			float y = SetVerticalListContentHight(m_BaiShiDiZis.Count, 75f, 0f, 3f);
			RectTransform component = m_Panel.PlayerInfoRect.gameObject.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			m_Panel.PlayerVerticalContent.SetCompelete(m_BaiShiDiZis.Count);
		}

		private void CreateMingCiItem(int index, TuDiEntity item)
		{
			ShoTuItem component = SetListContentParent(m_Panel.PlayerInfoRect, ShoTuItem).GetComponent<ShoTuItem>();
			SetItemMingCiValue(index, component, item);
			if (!m_BaiShiDiZisDic.ContainsKey(item.Id))
			{
				m_BaiShiDiZisDic.Add(item.Id, component);
			}
		}

		private void JieShoAction(int index, GameObject gameObj, string id, string name)
		{
			ShouTuReq_1014020 data = new ShouTuReq_1014020
			{
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SHOUTU, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					ShouTuResp_1014020 shouTuResp_ = new ShouTuResp_1014020();
					shouTuResp_.MergeFrom(byteString);
					if (shouTuResp_.IsSuccess)
					{
						DeleteMingCi(index, id, gameObj);
						HuoQuShiFuXIngXi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(shouTuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void HuLueAction(int index, GameObject gameObj, string id, string name)
		{
			BaiShiHuLueReq_1014023 data = new BaiShiHuLueReq_1014023
			{
				RoleId = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.BAI_SHI_HU_LUE, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					BaiShiHuLueResp_1014023 baiShiHuLueResp_ = new BaiShiHuLueResp_1014023();
					baiShiHuLueResp_.MergeFrom(byteString);
					if (baiShiHuLueResp_.IsSuccess)
					{
						DeleteMingCi(index, id, gameObj);
						m_BaiShiTieNum--;
						BaiShiTieNew();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(baiShiHuLueResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void DeleteMingCi(int index, string id, GameObject gameObj)
		{
			if (m_BaiShiDiZisDic.ContainsKey(id))
			{
				m_BaiShiDiZisDic.Remove(id);
			}
			if (m_BaiShiDiZis.Count >= index)
			{
				m_BaiShiDiZis.RemoveAt(index);
			}
			int num = 0;
			ClearData();
			foreach (TuDiEntity baiShiDiZi in m_BaiShiDiZis)
			{
				CreateMingCiItem(num, baiShiDiZi);
				num++;
			}
			m_Panel.PlayerInfoRect.sizeDelta = new Vector2(m_Panel.PlayerInfoRect.sizeDelta.x, (float)m_BaiShiDiZisDic.Count * 80f);
		}

		private void ClearData()
		{
			if (m_Panel == null || m_Panel.PlayerInfoRect == null)
			{
				return;
			}
			foreach (RectTransform item in m_Panel.PlayerInfoRect.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void SetItemMingCiValue(int index, ShoTuItem mingCiItem, TuDiEntity item)
		{
			mingCiItem.SetValue(index, item.Id, isOnline: false, item.Sex == 1, item.HeadPortraitId, item.Name, item.Level, JieShoAction, HuLueAction);
		}

		private void ContentDownScroll(GameObject gameObj)
		{
			if (!gameObj.Equals(null))
			{
				ShiTuItem component = gameObj.GetComponent<ShiTuItem>();
				if (!component.Equals(null))
				{
					int index = component.index;
					int targetIndex = ((index + 4 > m_TuDiXingXi.Count - 1) ? (m_TuDiXingXi.Count - 1) : (index + 4));
					ScrollRestValue(targetIndex, component);
				}
			}
		}

		private void ContentUpScroll(GameObject gameObj)
		{
			ShiTuItem component = gameObj.GetComponent<ShiTuItem>();
			int index = component.index;
			int targetIndex = ((index - 4 >= 0) ? (index - 4) : 0);
			ScrollRestValue(targetIndex, component);
		}

		private void ScrollRestValue(int targetIndex, ShiTuItem daoYouItem)
		{
			TuDiEntity tuDiEntity = m_TuDiXingXi[targetIndex];
			string friendRoleId = daoYouItem.FriendRoleId;
			SetItemValue(targetIndex, daoYouItem, tuDiEntity);
			if (m_TuDiXingXiDic.ContainsKey(friendRoleId))
			{
				m_TuDiXingXiDic.Remove(friendRoleId);
			}
			if (!m_TuDiXingXiDic.ContainsKey(tuDiEntity.Id))
			{
				m_TuDiXingXiDic.Add(tuDiEntity.Id, daoYouItem);
			}
		}

		private void ShoTuContentDownScroll(GameObject gameObj)
		{
			ShoTuItem component = gameObj.GetComponent<ShoTuItem>();
			if (!component.Equals(null) && !gameObj.Equals(null))
			{
				int index = component.index;
				int targetIndex = ((index + 4 > m_BaiShiDiZis.Count - 1) ? (m_BaiShiDiZis.Count - 1) : (index + 4));
				MingCiScrollRestValue(targetIndex, component);
			}
		}

		private void ShoTuContentUpScroll(GameObject gameObj)
		{
			ShoTuItem component = gameObj.GetComponent<ShoTuItem>();
			int index = component.index;
			int targetIndex = ((index - 4 >= 0) ? (index - 4) : 0);
			MingCiScrollRestValue(targetIndex, component);
		}

		private void MingCiScrollRestValue(int targetIndex, ShoTuItem mingCiItem)
		{
			TuDiEntity tuDiEntity = m_BaiShiDiZis[targetIndex];
			string friendRoleId = mingCiItem.FriendRoleId;
			SetItemMingCiValue(targetIndex, mingCiItem, tuDiEntity);
			if (m_BaiShiDiZisDic.ContainsKey(friendRoleId))
			{
				m_BaiShiDiZisDic.Remove(friendRoleId);
			}
			if (!m_BaiShiDiZisDic.ContainsKey(tuDiEntity.Id))
			{
				m_BaiShiDiZisDic.Add(tuDiEntity.Id, mingCiItem);
			}
		}

		private void DaoYouButton()
		{
			TuiChu();
		}

		private void DaoLvButton()
		{
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("功能暂未开放！");
		}

		private void TuiChu()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private static void XiaoWenHao()
		{
			string text = " 1、当前仅可拜入一人名下，且修为境界必须超过自身10个小境界 2、无论是请安、传功、通脉，获得的亲密均为师徒双方共享 3、请安、传功、通脉获得的亲密，基础为5，同时在线则为10，同时在线且同城则为20 4、亲密度大于等于100后，由记名弟子晋升为真传弟子 5、传功给予弟子的修为与弟子的当前境界有关，同城且均在线，则收益翻倍 6、通脉给予弟子的经脉点恒定为1点，同城且均在线，则收益翻倍 7、每位修士至多可以同时拥有3名弟子 8、师徒亲密达到300后，师尊可以选择让徒弟出师 9、弟子修为大境界与师尊修为大境界一致后,无法接受传功和通脉 10、每有一位弟子出师，则师尊的气运增加50点 11、师徒称号有3个，分别为：传道受业、桃李满门、德高望重，用于增加气运，气运可以提升掉落几率;";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "师徒");
		}

	}
}
