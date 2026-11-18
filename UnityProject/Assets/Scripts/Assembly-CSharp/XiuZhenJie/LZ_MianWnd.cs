using Google.Protobuf;

namespace XiuZhenJie
{
	public class LZ_MianWnd : Window
	{
		public LT_MianPanel Panel;

		private string m_LevelInfo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LT_MianPanel>();
			AddButtonClickListener(Panel.FanHui, LiKaiBtnClick);
			AddButtonClickListener(Panel.LingZhiDuiHuan, LingZhiDuiHuan);
			AddButtonClickListener(Panel.LingZhiZhuCe, LingZhiZhuCe);
			AddButtonClickListener(Panel.MoMiChuangZao, MoMiChuangZao);
			EnterShenFuHui();
			InitJianJie();
			ReFlashData();
		}

		public override void ReFlashData()
		{
			base.ReFlashData();
			m_LevelInfo = "【" + Singleton<RoleManager>.Instance.GetCurrentLingZhiLevelName() + "】";
			Panel.GongXianDian.text = $"{Singleton<RoleManager>.Instance.Role_Entity.GongXianDian} 点";
			Panel.ExperienceText.text = Singleton<RoleManager>.Instance.GetLingZhiCurrentExp() + "/" + Singleton<RoleManager>.Instance.GetLingZhiMaxExp();
			Panel.LingZhiDengJi_text.text = m_LevelInfo;
		}

		private void InitJianJie()
		{
			Panel.Des.text = "灵植是各种丹药的基础材料，了解植物特性可以让灵植得以被种植。灵植协会中的灵植师们，可以通过对植物的了解，进行植物培植嫁接，然后培育出各式各样的灵植。这些灵植用于丹药的炼制、法器的炼制中。灵植协会收集修真界所有的灵植种子，并且保护灵植培植者最基本的利益，促进修真界的物产繁荣。";
		}

		private void EnterShenFuHui()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_PLANT_ASSOCIATION_RESP, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterPlantAssociationResp_1008054 enterPlantAssociationResp_ = new EnterPlantAssociationResp_1008054();
					enterPlantAssociationResp_.MergeFrom(byteString);
					if (enterPlantAssociationResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.GongXianDian = enterPlantAssociationResp_.Contribution;
						ReFlashData();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterPlantAssociationResp_.Code);
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
			Singleton<UIManager>.Instance.PopUpWnd("LZ_LingZhiDuiHuanWnd.prefab", true);
		}

		private void LingZhiZhuCe()
		{
			if (!Singleton<RoleManager>.Instance.IsLingZhi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非灵植师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("LZ_ZhuCeWnd.prefab", true);
			}
		}

		private void MoMiChuangZao()
		{
			if (!Singleton<RoleManager>.Instance.IsLingZhi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友并非灵植师,无法入内!");
			}
			else
			{
				Singleton<UIManager>.Instance.PopUpWnd("LZ_ChuangZaoWnd.prefab", true);
			}
		}
	}
}
