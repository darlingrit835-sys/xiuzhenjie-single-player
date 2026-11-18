using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class KongLingFaZhenWnd : Window
	{
		public KongLingFaZhenPanel kongLingFaZhenPanel;

		public bool JinOpen = true;

		public bool MuOpen = true;

		public bool ShuiOpen = true;

		public bool HuoOpen = true;

		public bool TuOpen = true;

		private FaZhenLevel m_CurrentLevel;

		private List<WuXing> m_WuXingCloseList = new List<WuXing>();

		private List<WuXing> m_WuXingOpenList = new List<WuXing>();

		private Dictionary<string, KongLingLevelModel> m_KongLingModelDic;

		private KongLingZhenModel m_DataSource;

		private int HuangLevelRunningPrice = 200;

		private int XuanLevelRunningPrice = 1000;

		private int DiLevelRunningPrice = 10000;

		private int TianLevelRunningPrice = 100000;

		private int AddTimes;

		private DongFuWnd m_DongFuWnd;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			kongLingFaZhenPanel = base.GameObject.GetComponent<KongLingFaZhenPanel>();
			AddButtonClickListener(kongLingFaZhenPanel.JinBtn, JinBtnClick);
			AddButtonClickListener(kongLingFaZhenPanel.MuBtn, MuBtnClick);
			AddButtonClickListener(kongLingFaZhenPanel.ShuiBtn, ShuiBtnClick);
			AddButtonClickListener(kongLingFaZhenPanel.HuoBtn, HuoBtnClick);
			AddButtonClickListener(kongLingFaZhenPanel.TuBtn, TuBtnClick);
			AddButtonClickListener(kongLingFaZhenPanel.GengHuanDengJieBtn, GengHuanDengJiBtnClick);
			AddButtonClickListener(kongLingFaZhenPanel.CloseBtn, CloseWnd);
			AddButtonClickListener(kongLingFaZhenPanel.QueRenTouRu, QueDingTouRuBtn);
			AddButtonClickListener(kongLingFaZhenPanel.AddBtn, AddLingShiBtn);
			AddButtonClickListener(kongLingFaZhenPanel.RemoveBtn, RemoveLingShiBtn);
			AddButtonClickListener(kongLingFaZhenPanel.WeiHao, WenHaoTogClick);
			m_DongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_WuXingCloseList = new List<WuXing>();
			m_WuXingOpenList = new List<WuXing>
			{
				WuXing.Jin,
				WuXing.Mu,
				WuXing.Tu,
				WuXing.Shui,
				WuXing.Huo
			};
			m_KongLingModelDic = Singleton<FacadeManager>.Instance.SelectKongLingLevelInfo();
			if (m_KongLingModelDic != null)
			{
				m_DataSource = paralist[0] as KongLingZhenModel;
				ReFlash();
			}
		}

		public void ReFlash()
		{
			if (m_DataSource != null)
			{
				kongLingFaZhenPanel.TitleText.text = m_DataSource.i;
			}
			int num = (int)(m_CurrentLevel = (FaZhenLevel)m_DataSource.j);
			int num2 = 0;
			switch (num)
			{
			case 0:
				num2 = 0;
				break;
			case 1:
				num2 = HuangLevelRunningPrice;
				break;
			case 2:
				num2 = XuanLevelRunningPrice;
				break;
			case 3:
				num2 = DiLevelRunningPrice;
				break;
			case 4:
				num2 = TianLevelRunningPrice;
				break;
			}
			kongLingFaZhenPanel.LingShiTouRuText.text = (AddTimes * num2).ToString();
			if (!m_DataSource.c)
			{
				JinBtnClick();
			}
			if (!m_DataSource.e)
			{
				ShuiBtnClick();
			}
			if (!m_DataSource.g)
			{
				TuBtnClick();
			}
			if (!m_DataSource.f)
			{
				HuoBtnClick();
			}
			if (!m_DataSource.d)
			{
				MuBtnClick();
			}
		}

		public void JinBtnClick()
		{
			JinOpen = !JinOpen;
			if (JinOpen)
			{
				m_WuXingOpenList.Add(WuXing.Jin);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", kongLingFaZhenPanel.JinBtn.gameObject.GetComponent<Image>());
			}
			else
			{
				m_WuXingOpenList.Remove(WuXing.Jin);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", kongLingFaZhenPanel.JinBtn.gameObject.GetComponent<Image>());
			}
			CloseFaZhen(JinOpen, WuXing.Jin);
		}

		public void MuBtnClick()
		{
			MuOpen = !MuOpen;
			if (MuOpen)
			{
				m_WuXingOpenList.Add(WuXing.Mu);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", kongLingFaZhenPanel.MuBtn.gameObject.GetComponent<Image>());
			}
			else
			{
				m_WuXingOpenList.Remove(WuXing.Mu);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", kongLingFaZhenPanel.MuBtn.gameObject.GetComponent<Image>());
			}
			CloseFaZhen(MuOpen, WuXing.Mu);
		}

		public void ShuiBtnClick()
		{
			ShuiOpen = !ShuiOpen;
			if (ShuiOpen)
			{
				m_WuXingOpenList.Add(WuXing.Shui);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", kongLingFaZhenPanel.ShuiBtn.gameObject.GetComponent<Image>());
			}
			else
			{
				m_WuXingOpenList.Remove(WuXing.Shui);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", kongLingFaZhenPanel.ShuiBtn.gameObject.GetComponent<Image>());
			}
			CloseFaZhen(ShuiOpen, WuXing.Shui);
		}

		public void HuoBtnClick()
		{
			HuoOpen = !HuoOpen;
			if (HuoOpen)
			{
				m_WuXingOpenList.Add(WuXing.Huo);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", kongLingFaZhenPanel.HuoBtn.gameObject.GetComponent<Image>());
			}
			else
			{
				m_WuXingOpenList.Remove(WuXing.Huo);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", kongLingFaZhenPanel.HuoBtn.gameObject.GetComponent<Image>());
			}
			CloseFaZhen(HuoOpen, WuXing.Huo);
		}

		public void TuBtnClick()
		{
			TuOpen = !TuOpen;
			if (TuOpen)
			{
				m_WuXingOpenList.Add(WuXing.Tu);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingChoose.png", kongLingFaZhenPanel.TuBtn.gameObject.GetComponent<Image>());
			}
			else
			{
				m_WuXingOpenList.Remove(WuXing.Tu);
				ChangeImageSprite("Assets/GameData/Textures/DongFu/WuXingNoChoose.png", kongLingFaZhenPanel.TuBtn.gameObject.GetComponent<Image>());
			}
			CloseFaZhen(TuOpen, WuXing.Tu);
		}

		private void CloseFaZhen(bool isOpen, WuXing wuXing)
		{
			int num = 0;
			switch (m_CurrentLevel)
			{
			case FaZhenLevel.Huang:
				num = 1;
				break;
			case FaZhenLevel.Xuan:
				num = 2;
				break;
			case FaZhenLevel.Di:
				num = 3;
				break;
			case FaZhenLevel.Tian:
				num = 4;
				break;
			}
			if (!isOpen)
			{
				m_WuXingCloseList.Add(wuXing);
				if (m_WuXingCloseList.Count > num)
				{
					WuXing wuXing2 = m_WuXingCloseList[0];
					m_WuXingCloseList.RemoveAt(0);
					switch (wuXing2)
					{
					case WuXing.Jin:
						JinBtnClick();
						break;
					case WuXing.Mu:
						MuBtnClick();
						break;
					case WuXing.Shui:
						ShuiBtnClick();
						break;
					case WuXing.Huo:
						HuoBtnClick();
						break;
					case WuXing.Tu:
						TuBtnClick();
						break;
					}
				}
			}
			else if (m_WuXingCloseList.Contains(wuXing))
			{
				m_WuXingCloseList.Remove(wuXing);
			}
			ComputeKongLingValue();
		}

		private void ComputeKongLingValue()
		{
			float num = 0f;
			num = ((m_WuXingOpenList.Count == 0) ? 0f : (1f / (float)m_WuXingOpenList.Count));
			foreach (WuXing wuXingOpen in m_WuXingOpenList)
			{
				switch (wuXingOpen)
				{
				case WuXing.Jin:
					kongLingFaZhenPanel.JinValueText.text = string.Format("{0}%", (num * 100f).ToString("0"));
					break;
				case WuXing.Mu:
					kongLingFaZhenPanel.MuValueText.text = string.Format("{0}%", (num * 100f).ToString("0"));
					break;
				case WuXing.Shui:
					kongLingFaZhenPanel.ShuiValueText.text = string.Format("{0}%", (num * 100f).ToString("0"));
					break;
				case WuXing.Huo:
					kongLingFaZhenPanel.HuoValueText.text = string.Format("{0}%", (num * 100f).ToString("0"));
					break;
				case WuXing.Tu:
					kongLingFaZhenPanel.TuValueText.text = string.Format("{0}%", (num * 100f).ToString("0"));
					break;
				}
			}
			foreach (WuXing wuXingClose in m_WuXingCloseList)
			{
				switch (wuXingClose)
				{
				case WuXing.Jin:
					kongLingFaZhenPanel.JinValueText.text = $"{0}%";
					break;
				case WuXing.Mu:
					kongLingFaZhenPanel.MuValueText.text = $"{0}%";
					break;
				case WuXing.Shui:
					kongLingFaZhenPanel.ShuiValueText.text = $"{0}%";
					break;
				case WuXing.Huo:
					kongLingFaZhenPanel.HuoValueText.text = $"{0}%";
					break;
				case WuXing.Tu:
					kongLingFaZhenPanel.TuValueText.text = $"{0}%";
					break;
				}
			}
		}

		public void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			KongLingOpenOrCloseModel kongLingOpenOrCloseModel = new KongLingOpenOrCloseModel();
			float num = 0f;
			num = ((m_WuXingOpenList.Count == 0) ? 0f : (1f / (float)m_WuXingOpenList.Count));
			foreach (WuXing wuXingOpen in m_WuXingOpenList)
			{
				switch (wuXingOpen)
				{
				case WuXing.Jin:
					kongLingOpenOrCloseModel.gold = num;
					break;
				case WuXing.Mu:
					kongLingOpenOrCloseModel.wood = num;
					break;
				case WuXing.Shui:
					kongLingOpenOrCloseModel.water = num;
					break;
				case WuXing.Huo:
					kongLingOpenOrCloseModel.fire = num;
					break;
				case WuXing.Tu:
					kongLingOpenOrCloseModel.earth = num;
					break;
				}
			}
			if (m_DataSource.c == (kongLingOpenOrCloseModel.gold != 0f) && m_DataSource.f == (kongLingOpenOrCloseModel.fire != 0f) && m_DataSource.e == (kongLingOpenOrCloseModel.water != 0f) && m_DataSource.g == (kongLingOpenOrCloseModel.earth != 0f) && m_DataSource.d == (kongLingOpenOrCloseModel.wood != 0f))
			{
				return;
			}
			ChangeControllerMatrixReq_1001009 data = new ChangeControllerMatrixReq_1001009
			{
				EarthIsOpen = (kongLingOpenOrCloseModel.earth > 0f),
				FireIsOpen = (kongLingOpenOrCloseModel.fire > 0f),
				GoldIsOpen = (kongLingOpenOrCloseModel.gold > 0f),
				WaterIsOpen = (kongLingOpenOrCloseModel.water > 0f),
				WoodIsOpen = (kongLingOpenOrCloseModel.wood > 0f)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CHANGE_CONTROLLER_MATRIX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ChangeControllerMatrixResp_1001009 changeControllerMatrixResp_ = new ChangeControllerMatrixResp_1001009();
					changeControllerMatrixResp_.MergeFrom(byteString);
					if (changeControllerMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab").OnShow();
					}
				}
			}, isShowWaitUI: true);
		}

		public void GengHuanDengJiBtnClick()
		{
			double num = 0.0;
			switch (m_DataSource.j)
			{
			case 0:
				num = m_KongLingModelDic["1"].price;
				break;
			case 1:
				num = m_KongLingModelDic["2"].price;
				break;
			case 2:
				num = m_KongLingModelDic["3"].price;
				break;
			case 3:
				num = m_KongLingModelDic["4"].price;
				break;
			default:
				num = m_KongLingModelDic["1"].price;
				break;
			case 4:
				break;
			}
			Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", false, true, num, new UnityAction<int, int>(ZhiFuCompelete), 1);
		}

		private void ZhiFuCompelete(int price, int type)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADE_CONTROLLER_MATRIX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeControllerMatrixResp_1005015 upgradeControllerMatrixResp_ = new UpgradeControllerMatrixResp_1005015();
					upgradeControllerMatrixResp_.MergeFrom(byteString);
					if (upgradeControllerMatrixResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = upgradeControllerMatrixResp_.ShinyStone;
						DongFuWnd dongFuWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab");
						int j = m_DataSource.j;
						KongLingLevelModel value = new KongLingLevelModel();
						if (m_KongLingModelDic.TryGetValue((j + 1).ToString(), out value))
						{
							m_DataSource.a = value.id;
							m_DataSource.i = WorldSetting.GetLevelByRankName(value.rank) + "·控灵阵";
							m_DataSource.j = j + 1;
							m_DataSource.desc = value.desc;
						}
						ReFlash();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						dongFuWnd.OnShow();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeControllerMatrixResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void AddLingShiBtn()
		{
			int result = 0;
			int.TryParse(kongLingFaZhenPanel.LingShiTouRuText.text, out result);
			int j = m_DataSource.j;
			int num = 0;
			switch (j)
			{
			case 0:
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先升级控灵阵！");
				return;
			case 1:
				result += HuangLevelRunningPrice;
				num = result / 200;
				break;
			case 2:
				result += XuanLevelRunningPrice;
				num = result / 1000;
				break;
			case 3:
				result += DiLevelRunningPrice;
				num = result / 10000;
				break;
			case 4:
				result += TianLevelRunningPrice;
				num = result / 100000;
				break;
			}
			num *= 6;
			kongLingFaZhenPanel.LingShiTouRuText.text = result.ToString("0");
			kongLingFaZhenPanel.ShengYuShiJianText.text = $"投入后可增加运转时间:  <size=18>{num}</size>  小时";
			AddTimes++;
		}

		private void WenHaoTogClick()
		{
			string text = $"控灵阵可以改变周遭灵气的五行比例，移除某些无法使用的五行灵气。天灵根仅能吸纳某一种五行灵气，所以若排除了其余四种五行灵气，修行速度提升巨大。同理，灵根属性和控灵属性一致，能最大化提升修行效率。五行全灵根则无需使用控灵阵。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "控灵阵");
		}

		private void RemoveLingShiBtn()
		{
			int result = 0;
			int.TryParse(kongLingFaZhenPanel.LingShiTouRuText.text, out result);
			if (result > 0)
			{
				int j = m_DataSource.j;
				int num = 0;
				switch (j)
				{
				case 1:
					result -= HuangLevelRunningPrice;
					num = result / 200;
					break;
				case 2:
					result -= XuanLevelRunningPrice;
					num = result / 1000;
					break;
				case 3:
					result -= DiLevelRunningPrice;
					num = result / 10000;
					break;
				case 4:
					result -= TianLevelRunningPrice;
					num = result / 100000;
					break;
				}
				num *= 6;
				kongLingFaZhenPanel.LingShiTouRuText.text = result.ToString("0");
				kongLingFaZhenPanel.ShengYuShiJianText.text = $"投入后可增加运转时间:  <size=18>{num}</size>  小时";
				AddTimes--;
			}
		}

		private void QueDingTouRuBtn()
		{
			int result = 0;
			int.TryParse(kongLingFaZhenPanel.LingShiTouRuText.text, out result);
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
			AddTimeControllerMatrixReq_1005024 data = new AddTimeControllerMatrixReq_1005024
			{
				Hours = AddTimes * 6
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ADD_TIME_CONTROLLER_MATRIX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					AddTimeControllerMatrixResp_1005024 addTimeControllerMatrixResp_ = new AddTimeControllerMatrixResp_1005024();
					addTimeControllerMatrixResp_.MergeFrom(byteString);
					if (addTimeControllerMatrixResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = addTimeControllerMatrixResp_.ShinyStone;
						m_DongFuWnd.AddControllerMatrixTime(AddTimes * 6);
						Singleton<UIManager>.Instance.CloseWnd(this);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(addTimeControllerMatrixResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
