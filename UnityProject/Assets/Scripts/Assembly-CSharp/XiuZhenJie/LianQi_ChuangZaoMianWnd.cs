using Google.Protobuf;

namespace XiuZhenJie
{
	public class LianQi_ChuangZaoMianWnd : Window
	{
		private LianQi_ChuangZaoMianPanel Panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQi_ChuangZaoMianPanel>();
			AddButtonClickListener(Panel.FanHui, LiKaiBtnClick);
			AddButtonClickListener(Panel.LingZhiDuiHuan, LingZhiDuiHuan);
			AddButtonClickListener(Panel.LingZhiZhuCe, LingZhiZhuCe);
			AddButtonClickListener(Panel.MoMiChuangZao, MoMiChuangZao);
			ReFlashData();
			EnterShenFuHui();
			InitJianJie();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			Panel.GongXianDian.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.ExperienceText.text = $"{Singleton<RoleManager>.Instance.GetLianQiShiCurrentExp()}/{Singleton<RoleManager>.Instance.GetLianQiShiMaxExp()}";
			Panel.LingZhiDengJi_text.text = "【" + Singleton<RoleManager>.Instance.GetCurrentLianQiShiLevelName() + "】";
		}

		private void InitJianJie()
		{
			Panel.Des.text = "无数炼器大师为探索铸造法器的最终奥义，成立了炼器协会。炼器协会利用虚神界的特性，可以将真实修真界的所有材料用以模拟创造，去探索材料的柔韧、导灵特性，以铸造更强大的法器。";
		}

		private void EnterShenFuHui()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_EQUIPMENT_ASSOCIATION, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterEquipmentAssociationResp_1008066 enterEquipmentAssociationResp_ = new EnterEquipmentAssociationResp_1008066();
					enterEquipmentAssociationResp_.MergeFrom(byteString);
					if (enterEquipmentAssociationResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterEquipmentAssociationResp_.Contribution;
						ReFlashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterEquipmentAssociationResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void LingZhiDuiHuan()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LianQi_DuiHuanWnd.prefab", true);
		}

		private void LingZhiZhuCe()
		{
			if (!Singleton<RoleManager>.Instance.IsLianQiShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非炼器师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("LianQi_ZhuCeWnd.prefab", true);
			}
		}

		private void MoMiChuangZao()
		{
			if (!Singleton<RoleManager>.Instance.IsLianQiShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非炼器师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("LianQiCreateChooseStageWnd.prefab", true);
			}
		}
	}
}
