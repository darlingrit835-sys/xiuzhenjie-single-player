using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LiXianShouYiWnd : Window
	{
		public LiXianShouYiPanel Panel;

		private OfflineEntity m_SourceModel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LiXianShouYiPanel>();
			StyleRewardPanel();
			AddButtonClickListener(Panel.FangQiBtnClick, FangQiBtnClick);
			AddButtonClickListener(Panel.QueRenBtnClick, LingQuBtnClick);
		}

		private void StyleRewardPanel()
		{
			InkDialogArt.SetWash(Panel.transform);
			Transform card = Panel.transform.Find("LiXianBg");
			InkDialogArt.SetPaper(card.GetComponent<Image>());

			Transform heading = card.Find("TitleBg");
			var headingRect = (RectTransform)heading;
			headingRect.anchoredPosition = new Vector2(0f, -28f);
			InkInterfaceArt.Set(heading.GetComponent<Image>(), "LightPlaque", false);
			Text title = heading.Find("Text").GetComponent<Text>();
			title.color = InkInterfaceArt.Ink;
			foreach (string art in new[] { "TitleLine", "BottomImg" })
			{
				card.Find(art).GetComponent<Image>().color = Color.clear;
			}

			Transform stats = card.Find("ContentTextBg");
			InkInterfaceArt.Set(stats.GetComponent<Image>(), "LightPaper", false);
			foreach (Transform row in stats)
			{
				row.Find("Text").GetComponent<Text>().color = InkInterfaceArt.Ink;
				Transform valuePlate = row.Find("Image");
				InkInterfaceArt.Set(valuePlate.GetComponent<Image>(), "LightPlaque", false);
				Text value = valuePlate.Find("Text").GetComponent<Text>();
				value.color = InkInterfaceArt.Seal;
				value.alignment = TextAnchor.MiddleLeft;
				value.rectTransform.anchorMin = Vector2.zero;
				value.rectTransform.anchorMax = Vector2.one;
				value.rectTransform.offsetMin = new Vector2(12f, 0f);
				value.rectTransform.offsetMax = new Vector2(-10f, 0f);
				UIManager.FitTextToRect(value);
				value.resizeTextMinSize = 11;
			}

			InkDialogArt.StyleButton(Panel.QueRenBtnClick, "LightAction", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(Panel.FangQiBtnClick, "LightPlaque", InkInterfaceArt.Ink);
			if (Constants.UsesPackagedTrialData) Panel.FangQiBtnClick.GetComponentInChildren<Text>().text = "关闭";
			((RectTransform)Panel.QueRenBtnClick.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 42f);
			((RectTransform)Panel.FangQiBtnClick.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 42f);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 1)
			{
				m_SourceModel = paralist[0] as OfflineEntity;
			}
			SetValue();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			base.GameObject.transform.SetAsLastSibling();
		}

		private void SetValue()
		{
			if (m_SourceModel != null)
			{
				string text = CommonMethod.parseTimeSeconds(m_SourceModel.OfflineTime);
				Panel.LiXianShiJian.text = (m_SourceModel.IsTopValue ? text + "（已达收益上限）" : text);
				Panel.ShenShiZengZhang.text = $"{m_SourceModel.Perception}点";
				Panel.TiPoZengZhang.text = $"{m_SourceModel.Physique}点";
				ZhenYuanShow(m_SourceModel.SuperAir);
				Panel.ShouYuanZengZhang.text = string.Format("{0}年", m_SourceModel.Age.ToString("0.00"));
				Panel.WuXing.text = $"{m_SourceModel.OfflineClever}点";
				if (m_SourceModel.OfflineVigor.Equals(0))
				{
					Debug.Log("出现了精力为0的情况!");
				}
				Panel.JingLi.text = $"{m_SourceModel.OfflineVigor}点";
				Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
			}
		}

		private void ZhenYuanShow(long value)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				Panel.ZhenYuanTitleText.text = "灵气:";
				Panel.ZhenYuanZengZhang.text = string.Format("{0}点", value.ToString("f1"));
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				Panel.ZhenYuanTitleText.text = "真气:";
				Panel.ZhenYuanZengZhang.text = string.Format("{0}点", (value / 100d).ToString("f1"));
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				Panel.ZhenYuanTitleText.text = "真元:";
				Panel.ZhenYuanZengZhang.text = string.Format("{0}点", (value / 10000d).ToString("f1"));
			}
		}

		private void FangQiBtnClick()
		{
			if (Constants.UsesPackagedTrialData) { Singleton<UIManager>.Instance.CloseWnd(this); return; }
			MonoSingleton<GameStart>.Instance.PopUpAttention("确认放弃离线收益？", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.RoleGetOffline, null, (object _obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				}, isShowWaitUI: true);
				int num = Singleton<RoleManager>.Instance.Role_Entity.MaxAge - Singleton<RoleManager>.Instance.Role_Entity.CurrentAge;
				if (num <= 10 && num > 0)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("大限将至,请尽快突破大境界！");
				}
				Singleton<UIManager>.Instance.CloseWnd(this);
			});
		}

		private void LingQuBtnClick()
		{
			RoleGetOfflineReq_1002004 data = new RoleGetOfflineReq_1002004
			{
				IsGetOffline = true
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RoleGetOffline, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RoleGetOfflineResp_1002004 roleGetOfflineResp_ = new RoleGetOfflineResp_1002004();
					roleGetOfflineResp_.MergeFrom(byteString);
					if (roleGetOfflineResp_.IsSuccess)
					{
						RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
						role_Entity.CurrentClever = roleGetOfflineResp_.Clever;
						role_Entity.CurrentVigor = roleGetOfflineResp_.Vigor;
						role_Entity.CurrentPureBlood = roleGetOfflineResp_.CurrentPureBlood;
						role_Entity.CurrentSuperAir = roleGetOfflineResp_.CurrentSuperAir;
						role_Entity.CurrentPhysique = roleGetOfflineResp_.CurrentPhysique;
						role_Entity.CurrentPerception = roleGetOfflineResp_.CurrentPerception;
						role_Entity.CurrentPower = roleGetOfflineResp_.Power;
						role_Entity.SuperAir = roleGetOfflineResp_.SuperAir;
						role_Entity.Physique = roleGetOfflineResp_.Physique;
						role_Entity.Perception = roleGetOfflineResp_.Perception;
						role_Entity.PureBlood = roleGetOfflineResp_.PureBlood;
						role_Entity.CurrentPoisonVal = roleGetOfflineResp_.PoisonVal;
						role_Entity.CurrentAge = roleGetOfflineResp_.Age;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						int num = Singleton<RoleManager>.Instance.Role_Entity.MaxAge - Singleton<RoleManager>.Instance.Role_Entity.CurrentAge;
						if (num <= 10 && num > 0)
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention("大限将至,请尽快突破大境界！");
						}
						MonoSingleton<GameStart>.Instance.PopRollingTextShow("离线收益领取成功！");
						Singleton<UIManager>.Instance.CloseWnd(this);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(roleGetOfflineResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
