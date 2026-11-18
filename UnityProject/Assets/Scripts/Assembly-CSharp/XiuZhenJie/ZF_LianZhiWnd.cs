using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZF_LianZhiWnd : Window
	{
		private ZF_LianZhiPanel m_Panel;

		private ZhenPanChooseItemEntity m_MethodMatrixId;

		public int Shu;

		private int content;

		private bool m_PanDuanX;

		private bool m_PanDuanY;

		private bool m_DQPanDuanX;

		private bool m_DQPanDuanY;

		private int ZhuanDoZhi;

		private bool ZhuanDo;

		private int m_ShengYu;

		private List<string> m_DiYiPaiA;

		private List<string> m_DiErPaiB;

		private List<string> m_DiSanPaiC;

		private Dictionary<int, string> m_YiXuanZhoDicZo;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZF_LianZhiPanel>();
			if (paralist.Length != 0)
			{
				m_MethodMatrixId = paralist[0] as ZhenPanChooseItemEntity;
			}
			m_Panel.ZhuanDo_A.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
			m_Panel.ZhuanDo_A.OnLongButtonDown.AddListener(A_BtnDown);
			m_Panel.ZhuanDo_B.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
			m_Panel.ZhuanDo_B.OnLongButtonDown.AddListener(B_BtnDown);
			m_Panel.ZhuanDo_C.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
			m_YiXuanZhoDicZo = new Dictionary<int, string>();
			ShengCheng();
			m_Panel.ZhuanDo_C.OnLongButtonDown.AddListener(C_BtnDown);
			AddButtonClickListener(m_Panel.Likai, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHao, WenHaoTogClick);
			FuZhi();
			ZF_LianZhiPanel panel = m_Panel;
			panel.ZF_PointMoveHandler = (UnityAction<ZF_LianZhiPanel.SlideVector>)Delegate.Remove(panel.ZF_PointMoveHandler, new UnityAction<ZF_LianZhiPanel.SlideVector>(HuaDo));
			ZF_LianZhiPanel panel2 = m_Panel;
			panel2.ZF_PointMoveHandler = (UnityAction<ZF_LianZhiPanel.SlideVector>)Delegate.Combine(panel2.ZF_PointMoveHandler, new UnityAction<ZF_LianZhiPanel.SlideVector>(HuaDo));
		}

		private void ShengCheng()
		{
			m_DiYiPaiA = new List<string> { "天英", "天辅", "天柱", "天心", "天冲", "天任", "天禽", "天蓬", "天芮" };
			for (int i = 0; i < m_DiYiPaiA.Count; i++)
			{
				m_YiXuanZhoDicZo.Add(i + 10, m_DiYiPaiA[i]);
			}
			m_DiErPaiB = new List<string> { "杜", "景", "休", "开", "惊", "伤", "死", "生" };
			for (int j = 0; j < m_DiErPaiB.Count; j++)
			{
				m_YiXuanZhoDicZo.Add(j + 20, m_DiErPaiB[j]);
			}
			m_DiSanPaiC = new List<string> { "九天", "六合", "太阴", "九地", "玄武", "螣蛇", "白虎", "直符" };
			for (int k = 0; k < m_DiSanPaiC.Count; k++)
			{
				m_YiXuanZhoDicZo.Add(k + 30, m_DiSanPaiC[k]);
			}
		}

		private void A_BtnDown()
		{
			ZhuanDoA(1);
		}

		private void B_BtnDown()
		{
			ZhuanDoA(2);
		}

		private void C_BtnDown()
		{
			ZhuanDoA(3);
		}

		public void ZhuanDoA(int shu)
		{
			if (Shu == 0)
			{
				m_Panel.HuaDo = true;
				Shu = shu;
			}
		}

		private void JinXing(double shu)
		{
			switch (Shu)
			{
			case 1:
				m_Panel.A_1.MeanwhileDirection(shu);
				m_Panel.B_1.MeanwhileDirection(0.0 - shu);
				break;
			case 2:
				m_Panel.A_1.MeanwhileDirection(0.0 - shu);
				m_Panel.B_1.MeanwhileDirection(shu);
				m_Panel.C_1.MeanwhileDirection(0.0 - shu);
				break;
			case 3:
				m_Panel.B_1.MeanwhileDirection(0.0 - shu);
				m_Panel.C_1.MeanwhileDirection(shu);
				break;
			}
		}

		private void JinXingNew(double shu)
		{
			m_Panel.Mask.gameObject.SetActive(value: true);
			switch (Shu)
			{
			case 1:
				m_Panel.A_1.MeanwhileDirectionnew(shu);
				m_Panel.B_1.MeanwhileDirectionnew(0.0 - shu);
				break;
			case 2:
				m_Panel.A_1.MeanwhileDirectionnew(0.0 - shu);
				m_Panel.B_1.MeanwhileDirectionnew(shu);
				m_Panel.C_1.MeanwhileDirectionnew(0.0 - shu);
				break;
			case 3:
				m_Panel.B_1.MeanwhileDirectionnew(0.0 - shu);
				m_Panel.C_1.MeanwhileDirectionnew(shu);
				break;
			}
			Timer.Register(0.5f, () =>
			{
				m_Panel.Mask.gameObject.SetActive(value: false);
			});
		}

		private void HuaDo(ZF_LianZhiPanel.SlideVector slideVector)
		{
			if (slideVector == ZF_LianZhiPanel.SlideVector.NullVector)
			{
				if (ZhuanDoZhi == 0)
				{
					Shu = 0;
					return;
				}
				if (ZhuanDoZhi > 0)
				{
					if (ZhuanDoZhi > 22)
					{
						JinXingNew(45 - ZhuanDoZhi);
						ChuanZhi(1);
					}
					else
					{
						JinXingNew(-ZhuanDoZhi);
					}
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.ZhenFaVoice);
				}
				else
				{
					if (ZhuanDoZhi < -22)
					{
						JinXingNew(-(45 + ZhuanDoZhi));
						ChuanZhi(2);
					}
					else
					{
						JinXingNew(-ZhuanDoZhi);
					}
					Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.ZhenFaVoice);
				}
				Shu = 0;
				ZhuanDoZhi = 0;
			}
			else if (m_Panel.q.z > 0f)
			{
				if (ZhuanDoZhi < 45)
				{
					JinXing(1.0);
					ZhuanDoZhi++;
				}
			}
			else if (ZhuanDoZhi > -45)
			{
				JinXing(-1.0);
				ZhuanDoZhi--;
			}
		}

		private void ChuanZhi(int fangXiang)
		{
			RefiningMatrixReq_1005057 data = new RefiningMatrixReq_1005057
			{
				FangXiang = fangXiang,
				QuanType = Shu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REFININGMATRIX, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					RefiningMatrixResp_1005057 refiningMatrixResp_ = new RefiningMatrixResp_1005057();
					refiningMatrixResp_.MergeFrom(byteString);
					content++;
					if (refiningMatrixResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(refiningMatrixResp_.LiveLevel, refiningMatrixResp_.LiveProficiency, RoleFuZhiType.ZhenTu);
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = refiningMatrixResp_.CurrentVigor;
						string levelByRankName = WorldSetting.GetLevelByRankName(refiningMatrixResp_.Rank);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						if (refiningMatrixResp_.IsRefiningSuccess)
						{
							PopSuccessBg(levelByRankName);
						}
						m_ShengYu--;
						ShuXingBuShu();
					}
					else
					{
						Failure();
					}
				}
			}, isShowWaitUI: true);
		}

		private void Failure()
		{
			AddButtonClickListener(m_Panel.FailOnceAgainBtn, LiKaiBtn);
			m_Panel.FailPanel.gameObject.SetActive(value: true);
			m_Panel.FailWuXingXiaoHao.text = $"你消耗了{content}点精力";
		}

		private void PopSuccessBg(string pinJie)
		{
			AddButtonClickListener(m_Panel.Success_QueDingBtn, LiKaiBtn);
			m_Panel.Success_TitleText.text = "恭喜！成功获取“" + pinJie + "”" + m_MethodMatrixId.Name;
			m_Panel.Success_MiddleText.text = $"你消耗了{content}点精力";
			m_Panel.SuccessBg.gameObject.SetActive(value: true);
		}

		private void FuZhi()
		{
			SearchMatrixRefiningStepListReq_1005060 data = new SearchMatrixRefiningStepListReq_1005060
			{
				MethodMatrixId = m_MethodMatrixId.Id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.SEARCHMATRIXREFININGSTEPLIST, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					SearchMatrixRefiningStepListResp_1005060 searchMatrixRefiningStepListResp_ = new SearchMatrixRefiningStepListResp_1005060();
					searchMatrixRefiningStepListResp_.MergeFrom(byteString);
					if (searchMatrixRefiningStepListResp_.IsSuccess)
					{
						int num = 0;
						foreach (int item in searchMatrixRefiningStepListResp_.WaiQuan)
						{
							m_Panel.A_1.Rechristen(num, m_YiXuanZhoDicZo[item]);
							num++;
						}
						m_Panel.A_1.BianSe(m_YiXuanZhoDicZo[searchMatrixRefiningStepListResp_.WaiQuanSuccess]);
						foreach (int item2 in searchMatrixRefiningStepListResp_.ZhongQuan)
						{
							m_Panel.B_1.Rechristen(num - 8, m_YiXuanZhoDicZo[item2]);
							num++;
						}
						m_Panel.B_1.BianSe(m_YiXuanZhoDicZo[searchMatrixRefiningStepListResp_.ZhongQuanSuccess]);
						foreach (int item3 in searchMatrixRefiningStepListResp_.NeiQuan)
						{
							m_Panel.C_1.Rechristen(num - 16, m_YiXuanZhoDicZo[item3]);
							num++;
						}
						MaxBuShui();
						m_Panel.C_1.BianSe(m_YiXuanZhoDicZo[searchMatrixRefiningStepListResp_.NeiQuanSuccess]);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(searchMatrixRefiningStepListResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void MaxBuShui()
		{
			switch (m_MethodMatrixId.Rank)
			{
			case 1:
			case 2:
			case 3:
			case 4:
				m_ShengYu = 15;
				break;
			case 5:
			case 6:
			case 7:
			case 8:
				m_ShengYu = 12;
				break;
			case 9:
			case 10:
			case 11:
			case 12:
				m_ShengYu = 10;
				break;
			case 13:
			case 14:
			case 15:
			case 16:
				m_ShengYu = 9;
				break;
			}
			ShuXingBuShu();
		}

		private void ShuXingBuShu()
		{
			if (m_ShengYu == 0)
			{
				Failure();
			}
			m_Panel.ShengYu.text = $"剩余移动步数:{m_ShengYu}";
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、旋转阵盘，使其能与最终阵图相符 2、旋转任意一层阵盘，相邻阵盘均会往反方向转动 3、超出转动的次数限制，则炼制阵盘失败 4、转动一格则记录一次次数 5、黄阶最大步数15步，玄阶12步，地阶10步，天阶9步";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "阵盘刻制");
		}
	}
}
