using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZongMen_ShiKongMiJingWnd : Window
	{
		private ZongMen_ShiKongMiJingPanel m_Panel;

		private OfflineEntity m_SourceModel;

		private int m_NeedGongxian;

		private bool m_TodayCanUse;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_ShiKongMiJingPanel>();
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.ShiKongMiJingButton, ShiKongMiJingBut);
			EnterShiKongMiJing();
		}

		private void EnterShiKongMiJing()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.EnterShiKongMiJing, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterShiKongMiJingResp_1015154 enterShiKongMiJingResp_ = new EnterShiKongMiJingResp_1015154();
					enterShiKongMiJingResp_.MergeFrom(byteString);
					if (enterShiKongMiJingResp_.IsSuccess)
					{
						m_NeedGongxian = enterShiKongMiJingResp_.NeedGongxian;
						m_TodayCanUse = enterShiKongMiJingResp_.TodayCanUse;
						m_Panel.ShiKongMiJingButton.interactable = m_TodayCanUse;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterShiKongMiJingResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ShiKongMiJingBut()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention($"请确定是否消耗{m_NeedGongxian}点宗门贡献点，开启时空秘境", () =>
			{
				AddButtonClickListener(m_Panel.QueRenBtnClick, LiKaiBtnDq);
				Singleton<NetManager>.Instance.SendMsg(MsgType.GetShiKongMiJing, null, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						GetShiKongMiJingResp_1002032 getShiKongMiJingResp_ = new GetShiKongMiJingResp_1002032();
						getShiKongMiJingResp_.MergeFrom(byteString);
						if (getShiKongMiJingResp_.IsSuccess)
						{
							m_Panel.ShenShiZengZhang.text = $"{getShiKongMiJingResp_.RefiningPerception}点";
							m_Panel.TiPoZengZhang.text = $"{getShiKongMiJingResp_.RefiningtPhysique}点";
							ZhenYuanShow((int)getShiKongMiJingResp_.RefiningSuperAir);
							RoleInfoEntity role_Entity = Singleton<RoleManager>.Instance.Role_Entity;
							role_Entity.CurrentPureBlood = getShiKongMiJingResp_.CurrentPureBlood;
							role_Entity.CurrentSuperAir = getShiKongMiJingResp_.CurrentSuperAir;
							role_Entity.CurrentPhysique = getShiKongMiJingResp_.CurrentPhysique;
							role_Entity.CurrentPerception = getShiKongMiJingResp_.CurrentPerception;
							role_Entity.SuperAir = getShiKongMiJingResp_.SuperAir;
							role_Entity.Physique = getShiKongMiJingResp_.Physique;
							role_Entity.Perception = getShiKongMiJingResp_.Perception;
							role_Entity.PureBlood = getShiKongMiJingResp_.PureBlood;
							role_Entity.CurrentAge = getShiKongMiJingResp_.Age;
							m_Panel.ShiKongMiJingButton.interactable = false;
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							m_Panel.LiXianImage.gameObject.SetActive(value: true);
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(getShiKongMiJingResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private void ZhenYuanShow(int value)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 31)
			{
				m_Panel.ZhenYuanTitleText.text = "灵气增加:";
				m_Panel.ZhenYuanZengZhang.text = $"{value:f1}点";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 31 && Singleton<RoleManager>.Instance.Role_Entity.RoleLevel < 61)
			{
				m_Panel.ZhenYuanTitleText.text = "真气增加:";
				m_Panel.ZhenYuanZengZhang.text = $"{(float)value / 100f:f1}点";
			}
			else if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= 61)
			{
				m_Panel.ZhenYuanTitleText.text = "真元增加:";
				m_Panel.ZhenYuanZengZhang.text = $"{(float)value / 100f / 100f:f1}点";
			}
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void LiKaiBtnDq()
		{
			m_Panel.LiXianImage.gameObject.SetActive(value: false);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、时空秘境每日可使用一次，每日0点重置 2、三阶宗门加速时间为2小时，每高一阶增加30分钟，宗门等阶越高则加速时间越长 3、每次时空加速需要消耗100点贡献点，不随宗门等阶增长而增加";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "时空秘境");
		}
	}
}
