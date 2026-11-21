using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class JuLingFaZhenWnd : Window
	{
		public JuLingFaZhenPanel Panel;

		private JuLingFaZhenModel m_DataSource;

		private Dictionary<string, JuLingLevelModel> m_JuLingModelDic;

		private int HuangLevelRunningPrice = 100;

		private int XuanLevelRunningPrice = 500;

		private int DiLevelRunningPrice = 5000;

		private int TianLevelRunningPrice = 50000;

		private int AddTimes;

		private long ShengYuSecond;

		private DongFuWnd m_DongFuWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<JuLingFaZhenPanel>();
			InkCultivationPages.PaperDialog(Panel.transform, "JuLingFaZhenBg", Panel.QueRenTouRuBtn, Panel.GengHuanDengJieBtn);
			AddButtonClickListener(Panel.GengHuanDengJieBtn, GengHuanDengJieBtnClick);
			AddButtonClickListener(Panel.CloseBtn, CloseWndClick);
			AddButtonClickListener(Panel.QueRenTouRuBtn, QueRenTouRu);
			AddButtonClickListener(Panel.AddLingShiBtn, AddLingShi);
			AddButtonClickListener(Panel.RemoveLingShiBtn, RemoveLingShi);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			AddTimes = 0;
			m_DongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			m_JuLingModelDic = new Dictionary<string, JuLingLevelModel>();
			base.OnShow(paralist);
			m_JuLingModelDic = Singleton<FacadeManager>.Instance.SelectJuLingLevelInfo();
			m_DataSource = paralist[0] as JuLingFaZhenModel;
			switch (m_DataSource.d)
			{
			case 1:
				m_DataSource.c = m_JuLingModelDic["1"].conversionRate;
				break;
			case 2:
				m_DataSource.c = m_JuLingModelDic["2"].conversionRate;
				break;
			case 3:
				m_DataSource.c = m_JuLingModelDic["3"].conversionRate;
				break;
			case 4:
				m_DataSource.c = m_JuLingModelDic["4"].conversionRate;
				break;
			default:
				m_DataSource.c = m_JuLingModelDic["0"].conversionRate;
				break;
			}
			long.TryParse(paralist[1].ToString(), out ShengYuSecond);
			RefalshData();
		}

		public void RefalshData()
		{
			if (m_DataSource == null)
			{
				return;
			}
			string text = "提升洞府灵气浓郁程度{0}%";
			int num = 0;
			if (m_JuLingModelDic != null)
			{
				switch (m_DataSource.d)
				{
				case 0:
					text = string.Format(text, 100);
					num = 0;
					break;
				case 1:
					text = string.Format(text, m_JuLingModelDic["1"].conversionRate * 100.0);
					num = HuangLevelRunningPrice;
					break;
				case 2:
					text = string.Format(text, m_JuLingModelDic["2"].conversionRate * 100.0);
					num = XuanLevelRunningPrice;
					break;
				case 3:
					text = string.Format(text, m_JuLingModelDic["3"].conversionRate * 100.0);
					num = DiLevelRunningPrice;
					break;
				case 4:
					text = string.Format(text, m_JuLingModelDic["4"].conversionRate * 100.0);
					num = TianLevelRunningPrice;
					break;
				}
				Panel.TitleText.text = m_DataSource.b;
				Panel.NongYuChengDuText.text = text;
				Panel.CurrentAddLingShiText.text = (AddTimes * num).ToString();
			}
		}

		private void CloseWndClick()
		{
			Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void GengHuanDengJieBtnClick()
		{
			double num = 0.0;
			switch (m_DataSource.d)
			{
			case 0:
				num = m_JuLingModelDic["1"].price;
				break;
			case 1:
				num = m_JuLingModelDic["2"].price;
				break;
			case 2:
				num = m_JuLingModelDic["3"].price;
				break;
			case 3:
				num = m_JuLingModelDic["4"].price;
				break;
			}
			Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", false, true, num, new UnityAction<int, int>(ZhiFuCompelete), 1);
		}

		private void ZhiFuCompelete(int price, int type)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADE_GATHER_MATRIX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeGatherMatrixResp_1005013 upgradeGatherMatrixResp_ = new UpgradeGatherMatrixResp_1005013();
					upgradeGatherMatrixResp_.MergeFrom(byteString);
					if (upgradeGatherMatrixResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = upgradeGatherMatrixResp_.ShinyStone;
						DongFuWnd dongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
						int num = ((m_DataSource.d != 0) ? WorldSetting.GetLevelByRank(m_DataSource.d) : 0);
						JuLingLevelModel value = new JuLingLevelModel();
						if (m_JuLingModelDic.TryGetValue((num + 1).ToString(), out value))
						{
							m_DataSource.a = value.id;
							m_DataSource.b = WorldSetting.GetLevelByRankName(value.rank) + "·聚灵阵";
							m_DataSource.d = num + 1;
							m_DataSource.c = value.conversionRate;
							m_DataSource.e = value.desc;
						}
						dongFuWnd.OnShow();
						RefalshData();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeGatherMatrixResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void QueRenTouRu()
		{
			int result = 0;
			int.TryParse(Panel.CurrentAddLingShiText.text, out result);
			if (result <= 0 || AddTimes <= 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请投入灵石！");
				return;
			}
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(result) < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足！");
				return;
			}
			AddTimeGatherMatrixReq_1005025 data = new AddTimeGatherMatrixReq_1005025
			{
				Hours = AddTimes * 6
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ADD_TIME_GATHER_MATRIX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AddTimeGatherMatrixResp_1005025 addTimeGatherMatrixResp_ = new AddTimeGatherMatrixResp_1005025();
					addTimeGatherMatrixResp_.MergeFrom(byteString);
					if (addTimeGatherMatrixResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = addTimeGatherMatrixResp_.ShinyStone;
						Singleton<UIManager>.Instance.CloseWnd(this);
						m_DongFuWnd?.AddGatherMatrixTime(AddTimes * 6);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addTimeGatherMatrixResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void AddLingShi()
		{
			int num = AddTimes * 100;
			int d = m_DataSource.d;
			int num2 = 0;
			switch (d)
			{
			case 0:
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先升级控灵阵！");
				return;
			case 1:
				num = HuangLevelRunningPrice;
				num2 = num / 100;
				break;
			case 2:
				num = XuanLevelRunningPrice;
				num2 = num / 500;
				break;
			case 3:
				num = DiLevelRunningPrice;
				num2 = num / 5000;
				break;
			case 4:
				num = TianLevelRunningPrice;
				num2 = num / 50000;
				break;
			}
			AddTimes++;
			num2 = num2 * 6 * AddTimes;
			if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 2)
			{
				float num3 = (float)(AddTimes * num) * 0.9f;
				Panel.CurrentAddLingShiText.text = num3.ToString("0");
			}
			else
			{
				float num4 = AddTimes * num;
				Panel.CurrentAddLingShiText.text = num4.ToString("0");
			}
			Panel.ShengYuXiaoShiText.text = $"投入后可增加运转时间:  <size=18>{num2}</size>  小时";
		}

		private void RemoveLingShi()
		{
			int num = AddTimes * 100;
			if (num > 0)
			{
				int d = m_DataSource.d;
				int num2 = 0;
				switch (d)
				{
				case 1:
					num = HuangLevelRunningPrice;
					num2 = num / 100;
					break;
				case 2:
					num = XuanLevelRunningPrice;
					num2 = num / 500;
					break;
				case 3:
					num = DiLevelRunningPrice;
					num2 = num / 5000;
					break;
				case 4:
					num = TianLevelRunningPrice;
					num2 = num / 50000;
					break;
				}
				AddTimes--;
				num2 = num2 * 6 * AddTimes;
				if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 2)
				{
					float num3 = (float)(AddTimes * num) * 0.9f;
					Panel.CurrentAddLingShiText.text = num3.ToString("0");
				}
				else
				{
					float num4 = AddTimes * num;
					Panel.CurrentAddLingShiText.text = num4.ToString("0");
				}
				Panel.ShengYuXiaoShiText.text = $"投入后可增加运转时间:  <size=18>{num2}</size>  小时";
			}
		}

		private void WenHaoTogClick()
		{
			string text = $"聚灵阵可以汇聚周遭灵气，大幅度提升修行效率，聚灵阵需要投入灵石方可启用";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "聚灵阵");
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}
	}
}
