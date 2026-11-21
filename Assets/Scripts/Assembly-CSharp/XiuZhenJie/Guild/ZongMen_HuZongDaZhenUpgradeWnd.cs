using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class ZongMen_HuZongDaZhenUpgradeWnd : Window
	{
		private ZongMen_HuZongDaZhenUpgradePanel m_Panel;

		private Dictionary<int, ZM_KuangMaiUpdateEntity> m_GongJuUpdateDic;

		private int m_FriendRoleId;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_HuZongDaZhenUpgradePanel>();
			if (paralist.Length != 0)
			{
				m_FriendRoleId = (int)paralist[0];
			}
			m_GongJuUpdateDic = Singleton<FacadeManager>.Instance.SelectKuangMaiUpdateEntities();
			XianShi();
			AddButtonClickListener(m_Panel.GuanBi, Exit);
			AddButtonClickListener(m_Panel.ShengJi, PanDuan);
		}

		private List<int> HuoQu(int shu)
		{
			int num = 1;
			int num2 = 0;
			for (int i = 0; i < shu; i++)
			{
				num *= 10;
				num2 += 5;
			}
			return new List<int> { num, num2 };
		}

		private void XianShi()
		{
			m_Panel.OrderEtcText.text = WorldSetting.GetStageName(m_FriendRoleId);
			m_Panel.BaseDefenseText.text = $"{HuoQu(m_FriendRoleId + 1)[1]}";
			m_Panel.ConsumptionLingShiText.text = $"{m_FriendRoleId * 12}灵石/每分钟";
			m_Panel.ProductOrderText.text = WorldSetting.GetStageName(m_FriendRoleId + 1);
			m_Panel.LingShiText.text = $"{m_GongJuUpdateDic[m_FriendRoleId + 1].MultiplierConsumption}";
			m_Panel.ZhenPanText.text = $"{m_GongJuUpdateDic[m_FriendRoleId + 1].MultiplierConsumption}";
		}

		private void Exit()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void PanDuan()
		{
			MonoSingleton<GameStart>.Instance.PopUpAttention("是否确认升级大阵?", ConfirmShengJi);
		}

		private void ConfirmShengJi()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UpgradeHuZongMatrix, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if ((object)byteString == null || !byteString.Equals(null))
				{
					UpgradeHuZongMatrixResp_1015050 upgradeHuZongMatrixResp_ = new UpgradeHuZongMatrixResp_1015050();
					upgradeHuZongMatrixResp_.MergeFrom(byteString);
					if (upgradeHuZongMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<ZongMen_HuZongDaZhenWnd>("ZongMen_HuZongDaZhenWnd.prefab").JiShi((int)upgradeHuZongMatrixResp_.ShengYuTime);
						Exit();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeHuZongMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
