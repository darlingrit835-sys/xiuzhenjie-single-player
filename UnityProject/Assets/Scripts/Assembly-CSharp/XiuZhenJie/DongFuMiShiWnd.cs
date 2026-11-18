using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DongFuMiShiWnd : Window
	{
		public DongFuMiShiPanel Panel;

		private int m_CurrentCangKuLevel;

		private List<DongFuMiShiModel> m_DataSource;

		private int shu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuMiShiPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiClick);
			AddButtonClickListener(Panel.CangKu_1_Btn, CangKu_1_Open);
			AddButtonClickListener(Panel.CangKu_2_Btn, CangKu_2_Open);
			AddButtonClickListener(Panel.CangKu_3_Btn, CangKu_3_Open);
			AddButtonClickListener(Panel.CangKu_4_Btn, CangKu_4_Open);
			AddButtonClickListener(Panel.CangKu_5_Btn, CangKu_5_Open);
			AddButtonClickListener(Panel.ChongFuMingQuXiaoZhiFuBtn, QueRenZhiFuBtn);
			AddButtonClickListener(Panel.ChongFuMingQueRenZhiFuBtn, CancleZhiFuBtn);
			AddButtonClickListener(Panel.CangKu_1_ChongMingMingBtn, CangKu_1);
			AddButtonClickListener(Panel.CangKu_2_ChongMingMingBtn, CangKu_2);
			AddButtonClickListener(Panel.CangKu_3_ChongMingMingBtn, CangKu_3);
			AddButtonClickListener(Panel.CangKu_4_ChongMingMingBtn, CangKu_4);
			AddButtonClickListener(Panel.CangKu_5_ChongMingMingBtn, CangKu_5);
			AddButtonClickListener(Panel.GouMaiBtn, GouMaiCangKu);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			m_DataSource = new List<DongFuMiShiModel>();
			m_CurrentCangKuLevel = 1;
			shu = -1;
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_SPACE_BOX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					EnterSpaceBoxResp_1005005 enterSpaceBoxResp_ = new EnterSpaceBoxResp_1005005();
					enterSpaceBoxResp_.MergeFrom(byteString);
					if (enterSpaceBoxResp_.IsSuccess)
					{
						for (int i = 0; i < enterSpaceBoxResp_.Name.Count; i++)
						{
							m_DataSource.Add(new DongFuMiShiModel
							{
								l = i + 1,
								i = (i + 1).ToString(),
								n = enterSpaceBoxResp_.Name[i]
							});
						}
						m_CurrentCangKuLevel = enterSpaceBoxResp_.Name.Count;
						Init();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterSpaceBoxResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void Init()
		{
			m_DataSource.Sort((DongFuMiShiModel a, DongFuMiShiModel b) => a.l.CompareTo(b.l));
			if (m_DataSource == null || m_DataSource.Count <= 0)
			{
				return;
			}
			for (int num = 0; num < m_DataSource.Count; num++)
			{
				switch (num)
				{
				case 0:
					Panel.CangKu_1.gameObject.SetActive(value: true);
					Panel.CangKu_1_Name.text = m_DataSource[num].n;
					break;
				case 1:
					Panel.CangKu_2.gameObject.SetActive(value: true);
					Panel.CangKu_2_Name.text = m_DataSource[num].n;
					break;
				case 2:
					Panel.CangKu_3.gameObject.SetActive(value: true);
					Panel.CangKu_3_Name.text = m_DataSource[num].n;
					break;
				case 3:
					Panel.CangKu_4.gameObject.SetActive(value: true);
					Panel.CangKu_4_Name.text = m_DataSource[num].n;
					break;
				case 4:
					Panel.CangKu_5.gameObject.SetActive(value: true);
					Panel.CangKu_5_Name.text = m_DataSource[num].n;
					break;
				}
			}
		}

		private void CangKu_1_Open()
		{
			if (m_DataSource.Count > 0)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true, m_DataSource[0].i);
			}
		}

		private void CangKu_2_Open()
		{
			if (m_DataSource.Count > 1)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true, m_DataSource[1].i);
			}
		}

		private void CangKu_3_Open()
		{
			if (m_DataSource.Count > 2)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true, m_DataSource[2].i);
			}
		}

		private void CangKu_4_Open()
		{
			if (m_DataSource.Count > 3)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true, m_DataSource[3].i);
			}
		}

		private void CangKu_5_Open()
		{
			if (m_DataSource.Count > 4)
			{
				Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true, m_DataSource[4].i);
			}
		}

		private void GouMaiCangKu()
		{
			switch (m_CurrentCangKuLevel)
			{
			case 1:
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, 10000, new UnityAction<int, int>(CangKuGouMaiSuccess), 1);
				break;
			case 2:
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, 100000, new UnityAction<int, int>(CangKuGouMaiSuccess), 1);
				break;
			case 3:
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, 1000, new UnityAction<int, int>(CangKuGouMaiSuccess), 2);
				break;
			case 4:
				Singleton<UIManager>.Instance.PopUpWnd("Common_ZhiFuWnd.prefab", true, 10000, new UnityAction<int, int>(CangKuGouMaiSuccess), 2);
				break;
			default:
				MonoSingleton<GameStart>.Instance.PopUpAttention("仓库已满！");
				break;
			}
		}

		private void WenHaoTogClick()
		{
			string text = "密室是修士存放贵重物品之场所，最多可购买5个芥子储物箱，每个储物箱容量为200立方尺，芥子储物箱购买价格会逐个增加。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "密室");
		}

		private void QueRenZhiFuBtn()
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.ShinyJade < 5)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵玉不足!");
			}
			else
			{
				QueRenChongMingMingBtn();
			}
		}

		private void CancleZhiFuBtn()
		{
			Panel.ChongFuMingRect.gameObject.SetActive(value: false);
		}

		private void CangKu_1()
		{
			shu = 1;
			Panel.ChongFuMingRect.gameObject.SetActive(value: true);
		}

		private void CangKu_2()
		{
			shu = 2;
			Panel.ChongFuMingRect.gameObject.SetActive(value: true);
		}

		private void CangKu_3()
		{
			shu = 3;
			Panel.ChongFuMingRect.gameObject.SetActive(value: true);
		}

		private void CangKu_4()
		{
			shu = 4;
			Panel.ChongFuMingRect.gameObject.SetActive(value: true);
		}

		private void CangKu_5()
		{
			shu = 5;
			Panel.ChongFuMingRect.gameObject.SetActive(value: true);
		}

		private void QueRenChongMingMingBtn()
		{
			string name = Panel.ChongFuMingInput.text;
			if (string.IsNullOrEmpty(name))
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入名称!");
				return;
			}
			RenameBoxReq_1005007 data = new RenameBoxReq_1005007
			{
				NewName = name,
				Location = shu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.RENAME_BOX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RenameBoxResp_1005007 renameBoxResp_ = new RenameBoxResp_1005007();
					renameBoxResp_.MergeFrom(byteString);
					if (renameBoxResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)renameBoxResp_.ShinyJade;
						switch (shu)
						{
						case 1:
							Panel.CangKu_1_Name.text = name;
							break;
						case 2:
							Panel.CangKu_2_Name.text = name;
							break;
						case 3:
							Panel.CangKu_3_Name.text = name;
							break;
						case 4:
							Panel.CangKu_4_Name.text = name;
							break;
						case 5:
							Panel.CangKu_5_Name.text = name;
							break;
						}
						Panel.ChongFuMingRect.gameObject.SetActive(value: false);
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(renameBoxResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void CangKuGouMaiSuccess(int price, int type)
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.BUYSPACEBOX, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					BuySpaceBoxResp_1005021 buySpaceBoxResp_ = new BuySpaceBoxResp_1005021();
					buySpaceBoxResp_.MergeFrom(byteString);
					if (buySpaceBoxResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = buySpaceBoxResp_.ShinyJade;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = buySpaceBoxResp_.ShinyStone;
						if (m_DataSource == null)
						{
							m_DataSource = new List<DongFuMiShiModel>();
						}
						m_CurrentCangKuLevel++;
						DongFuMiShiModel item = new DongFuMiShiModel
						{
							i = m_CurrentCangKuLevel.ToString(),
							n = buySpaceBoxResp_.Name,
							l = m_CurrentCangKuLevel
						};
						m_DataSource.Add(item);
						switch (m_CurrentCangKuLevel)
						{
						case 2:
							Panel.CangKu_2.gameObject.SetActive(value: true);
							Panel.CangKu_2_Name.text = buySpaceBoxResp_.Name;
							break;
						case 3:
							Panel.CangKu_3.gameObject.SetActive(value: true);
							Panel.CangKu_3_Name.text = buySpaceBoxResp_.Name;
							break;
						case 4:
							Panel.CangKu_4.gameObject.SetActive(value: true);
							Panel.CangKu_4_Name.text = buySpaceBoxResp_.Name;
							break;
						case 5:
							Panel.CangKu_5.gameObject.SetActive(value: true);
							Panel.CangKu_5_Name.text = buySpaceBoxResp_.Name;
							break;
						}
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(buySpaceBoxResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		public override void OnDisable()
		{
			base.OnDisable();
			Panel.CangKu_1.gameObject.SetActive(value: false);
			Panel.CangKu_2.gameObject.SetActive(value: false);
			Panel.CangKu_3.gameObject.SetActive(value: false);
			Panel.CangKu_4.gameObject.SetActive(value: false);
			Panel.CangKu_5.gameObject.SetActive(value: false);
		}

		private void LiKaiClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
