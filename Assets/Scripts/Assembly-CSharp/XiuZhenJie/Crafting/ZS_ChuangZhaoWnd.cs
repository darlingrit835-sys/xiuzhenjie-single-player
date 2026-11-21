using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_ChuangZhaoWnd : Window
	{
		private ZS_ChuangZhaoPanel m_Panel;

		private int m_Selected;

		private string m_SelectedName;

		private Dictionary<int, string> m_YiXuanZhoDicA;

		private Dictionary<int, string> m_YiXuanZhoDicB;

		private Dictionary<int, string> m_YiXuanZhoDicC;

		private Dictionary<int, string> m_YiXuanZhoDicD;

		private Dictionary<string, int> m_YiXuanZhoDicZo;

		private string m_ZhongXinDian;

		private int m_Classes;

		private List<string> m_DiYiPaiA;

		private List<string> m_DiErPaiB;

		private List<string> m_DiSanPaiC;

		private GameObject m_ItemObj;

		private List<string> m_DataList;

		private ZS_WorldCaiLiaoModel m_WorldCaiLiaoModel;

		private string m_TypeName;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZS_ChuangZhaoPanel>();
			m_YiXuanZhoDicA = new Dictionary<int, string>();
			m_YiXuanZhoDicB = new Dictionary<int, string>();
			m_YiXuanZhoDicC = new Dictionary<int, string>();
			m_YiXuanZhoDicD = new Dictionary<int, string>();
			m_YiXuanZhoDicZo = new Dictionary<string, int>();
			m_WorldCaiLiaoModel = new ZS_WorldCaiLiaoModel();
			AddButtonClickListener(m_Panel.XiaYiBuBtn, XiaYiBuBtnClick);
			AddToggleClickListener(m_Panel.YiJieTog, YiJieTog);
			AddToggleClickListener(m_Panel.ErJieTog, ErJieTog);
			AddToggleClickListener(m_Panel.SanJieTog, SanJieTog);
			AddToggleClickListener(m_Panel.SiJieTog, SiJieTog);
			AddToggleClickListener(m_Panel.WuJieTog, WuJieTog);
			AddToggleClickListener(m_Panel.LiuJieTog, LiuJieTog);
			AddToggleClickListener(m_Panel.QiJieTog, QiJieTog);
			AddToggleClickListener(m_Panel.BaJieTog, BaJieTog);
			AddToggleClickListener(m_Panel.JiuJieTog, JiuJieTog);
			ShengCheng();
			AddButtonClickListener(m_Panel.SuiJi, DianJieSuiJiBut);
			AddButtonClickListener(m_Panel.Likai, FanHuiBtnClick);
			AddButtonClickListener(m_Panel.WenHao, WenHaoTogClick);
			AddButtonClickListener(m_Panel.ChuangZao, ChuangZao);
			m_Panel.A_1.ButImage();
			m_Panel.C_1.ButImage();
			m_Panel.B_1.ButImage();
			m_Classes = 1;
			ChuShuiHua();
		}

		private void FanHuiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WenHaoTogClick()
		{
			string text = "1、阵盘分25个位置，隐藏位为中宫。 2、可以随机将位置放入对应宫位。 3、不同的排列和组合会生成不同的阵法或失败。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "阵图创造");
		}

		private void ShengCheng()
		{
			m_DiYiPaiA = new List<string> { "天英", "天辅", "天柱", "天心", "天冲", "天任", "天禽", "天蓬", "天芮" };
			for (int i = 0; i < m_DiYiPaiA.Count; i++)
			{
				m_YiXuanZhoDicZo.Add(m_DiYiPaiA[i], i + 10);
			}
			m_DiErPaiB = new List<string> { "杜", "景", "休", "开", "惊", "伤", "死", "生" };
			for (int j = 0; j < m_DiErPaiB.Count; j++)
			{
				m_YiXuanZhoDicZo.Add(m_DiErPaiB[j], j + 20);
			}
			m_DiSanPaiC = new List<string> { "九天", "六合", "太阴", "九地", "玄武", "螣蛇", "白虎", "直符" };
			for (int k = 0; k < m_DiSanPaiC.Count; k++)
			{
				m_YiXuanZhoDicZo.Add(m_DiSanPaiC[k], k + 30);
			}
		}

		private void ChuShuiHua()
		{
			m_Panel.A_1.KaiQi();
			m_Panel.B_1.KaiQi();
			m_Panel.C_1.KaiQi();
			m_Panel.D_1.GuanBi();
			m_Panel.E_1.GuanBi();
			m_Panel.F_1.GuanBi();
			m_Panel.D_1.PanDuanXianShi(m_YiXuanZhoDicD);
			m_Panel.E_1.PanDuanXianShi(m_YiXuanZhoDicD);
			m_Panel.F_1.PanDuanXianShi(m_YiXuanZhoDicD);
		}

		public void LunPanDianJi(int shu, string name)
		{
			m_Selected = shu;
			m_SelectedName = name;
			HuanXin();
		}

		private void HuanXin()
		{
			m_Panel.D_1.GuanBi();
			m_Panel.E_1.GuanBi();
			m_Panel.F_1.GuanBi();
			if (m_Selected < 8)
			{
				HuanXin_A();
			}
			else if (m_Selected < 16)
			{
				HuanXin_B();
			}
			else if (m_Selected < 24)
			{
				HuanXin_C();
			}
		}

		private void HuanXin_A()
		{
			m_Panel.D_1.KaiQi(m_SelectedName);
			m_Panel.D_1.PanDuanXianShi(m_YiXuanZhoDicA);
			m_Panel.E_1.PanDuanXianShi(m_YiXuanZhoDicD);
			m_Panel.F_1.PanDuanXianShi(m_YiXuanZhoDicD);
		}

		private void HuanXin_B()
		{
			m_Panel.E_1.KaiQi(m_SelectedName);
			m_Panel.D_1.PanDuanXianShi(m_YiXuanZhoDicD);
			m_Panel.E_1.PanDuanXianShi(m_YiXuanZhoDicB);
			m_Panel.F_1.PanDuanXianShi(m_YiXuanZhoDicD);
		}

		private void HuanXin_C()
		{
			m_Panel.F_1.KaiQi(m_SelectedName);
			m_Panel.D_1.PanDuanXianShi(m_YiXuanZhoDicD);
			m_Panel.E_1.PanDuanXianShi(m_YiXuanZhoDicD);
			m_Panel.F_1.PanDuanXianShi(m_YiXuanZhoDicC);
		}

		public void ButDianJi(string name)
		{
			ShanChu(name);
			QingLiMingChen(m_Selected, name);
			if (m_Selected < 8)
			{
				ButDianJi_A(name);
			}
			else if (m_Selected < 16)
			{
				ButDianJi_B(name);
			}
			else if (m_Selected < 24)
			{
				ButDianJi_C(name);
			}
			HuanXin();
			ChuangZhaoPanDuan();
		}

		private void ButDianJi_A(string name)
		{
			if (m_YiXuanZhoDicA.ContainsKey(m_Selected))
			{
				m_YiXuanZhoDicA[m_Selected] = name;
			}
			else
			{
				m_YiXuanZhoDicA.Add(m_Selected, name);
			}
		}

		private void ButDianJi_B(string name)
		{
			if (m_YiXuanZhoDicB.ContainsKey(m_Selected))
			{
				m_YiXuanZhoDicB[m_Selected] = name;
			}
			else
			{
				m_YiXuanZhoDicB.Add(m_Selected, name);
			}
		}

		private void ButDianJi_C(string name)
		{
			if (m_YiXuanZhoDicC.ContainsKey(m_Selected))
			{
				m_YiXuanZhoDicC[m_Selected] = name;
			}
			else
			{
				m_YiXuanZhoDicC.Add(m_Selected, name);
			}
		}

		private void ShanChu(string name)
		{
			if (m_Selected < 8)
			{
				ShanChu_A(name, romove: false, 0);
			}
			else if (m_Selected < 16)
			{
				ShanChu_B(name, romove: false, 0);
			}
			else if (m_Selected < 24)
			{
				ShanChu_C(name, romove: false, 0);
			}
		}

		private void ShanChu_A(string name, bool romove, int shuint)
		{
			foreach (KeyValuePair<int, string> item in m_YiXuanZhoDicA.Where((KeyValuePair<int, string> variable) => variable.Value == name))
			{
				romove = true;
				shuint = item.Key;
			}
			if (romove)
			{
				m_YiXuanZhoDicA.Remove(shuint);
				QingLiMingChen(shuint, "");
			}
		}

		private void ShanChu_B(string name, bool romove, int shuint)
		{
			foreach (KeyValuePair<int, string> item in m_YiXuanZhoDicB.Where((KeyValuePair<int, string> variable) => variable.Value == name))
			{
				romove = true;
				shuint = item.Key;
			}
			if (romove)
			{
				m_YiXuanZhoDicB.Remove(shuint);
				QingLiMingChen(shuint, "");
			}
		}

		private void ShanChu_C(string name, bool romove, int shuint)
		{
			foreach (KeyValuePair<int, string> item in m_YiXuanZhoDicC.Where((KeyValuePair<int, string> variable) => variable.Value == name))
			{
				romove = true;
				shuint = item.Key;
			}
			if (romove)
			{
				m_YiXuanZhoDicC.Remove(shuint);
				QingLiMingChen(shuint, "");
			}
		}

		private void QingLiMingChen(int shu, string name)
		{
			if (shu < 8)
			{
				m_Panel.A_1.Rechristen(shu, name);
			}
			else if (shu < 16)
			{
				m_Panel.B_1.Rechristen(shu - 8, name);
			}
			else if (shu < 24)
			{
				m_Panel.C_1.Rechristen(shu - 16, name);
			}
		}

		private void ChuangZhaoPanDuan()
		{
			if (m_YiXuanZhoDicA.Count + m_YiXuanZhoDicB.Count + m_YiXuanZhoDicC.Count == 24)
			{
				m_Panel.ChuangZao.interactable = true;
			}
			else
			{
				m_Panel.ChuangZao.interactable = false;
			}
		}

		private void ZhoXinDian()
		{
			foreach (string item in new List<string> { "天英", "天辅", "天柱", "天心", "天冲", "天任", "天禽", "天蓬", "天芮" })
			{
				if (!m_YiXuanZhoDicA.ContainsValue(item))
				{
					m_ZhongXinDian = item;
				}
			}
		}

		private void ChuangZao()
		{
			ZhoXinDian();
			VerifyCreateMethodMatrixReq_1008088 verifyCreateMethodMatrixReq_ = new VerifyCreateMethodMatrixReq_1008088
			{
				Stage = m_Classes
			};
			for (int i = 0; i < 8; i++)
			{
				verifyCreateMethodMatrixReq_.WaiQuan.Add(m_YiXuanZhoDicZo[m_YiXuanZhoDicA[i]]);
			}
			for (int j = 8; j < 16; j++)
			{
				verifyCreateMethodMatrixReq_.ZhongQuan.Add(m_YiXuanZhoDicZo[m_YiXuanZhoDicB[j]]);
			}
			for (int k = 16; k < 24; k++)
			{
				verifyCreateMethodMatrixReq_.NeiQuan.Add(m_YiXuanZhoDicZo[m_YiXuanZhoDicC[k]]);
			}
			verifyCreateMethodMatrixReq_.ZhongDian = m_YiXuanZhoDicZo[m_ZhongXinDian];
			Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFYCREATEMETHODMATRIXREQ, verifyCreateMethodMatrixReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					VerifyCreateMethodMatrixResp_1008088 verifyCreateMethodMatrixResp_ = new VerifyCreateMethodMatrixResp_1008088();
					verifyCreateMethodMatrixResp_.MergeFrom(byteString);
					if (verifyCreateMethodMatrixResp_.IsSuccess)
					{
						m_WorldCaiLiaoModel.b = Singleton<RoleManager>.Instance.Role_Entity.CurrentClever - verifyCreateMethodMatrixResp_.CurrentClever;
						if (verifyCreateMethodMatrixResp_.CreatedIsSuccess)
						{
							m_WorldCaiLiaoModel.a = verifyCreateMethodMatrixResp_.MethodMatrixId;
							m_WorldCaiLiaoModel.e = verifyCreateMethodMatrixResp_.Stage;
							m_WorldCaiLiaoModel.f = verifyCreateMethodMatrixResp_.FiveElement;
							m_WorldCaiLiaoModel.Rank = verifyCreateMethodMatrixResp_.Rank;
							m_WorldCaiLiaoModel.CreatedBy = verifyCreateMethodMatrixResp_.CreatedBy;
							m_WorldCaiLiaoModel.ConversionRate = verifyCreateMethodMatrixResp_.ConversionRate;
							m_WorldCaiLiaoModel.Type = verifyCreateMethodMatrixResp_.Type;
							m_WorldCaiLiaoModel.UnitConsume = verifyCreateMethodMatrixResp_.UnitConsume;
							m_WorldCaiLiaoModel.Duration = verifyCreateMethodMatrixResp_.Duration;
							Successful(m_WorldCaiLiaoModel);
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodMatrixResp_.Code);
							Failure(hintMsgByCode, m_WorldCaiLiaoModel.b);
						}
						Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateMethodMatrixResp_.CurrentClever;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						Singleton<UIManager>.Instance.CloseWnd(this);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateMethodMatrixResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FuZhiList()
		{
			m_DiYiPaiA = new List<string> { "天英", "天辅", "天柱", "天心", "天冲", "天任", "天禽", "天蓬", "天芮" };
			m_DiYiPaiA = ListRandom(m_DiYiPaiA);
			m_DiYiPaiA = ListRandom(m_DiYiPaiA);
			m_DiErPaiB = new List<string> { "杜", "景", "休", "开", "惊", "伤", "死", "生" };
			m_DiErPaiB = ListRandom(m_DiErPaiB);
			m_DiErPaiB = ListRandom(m_DiErPaiB);
			m_DiSanPaiC = new List<string> { "九天", "六合", "太阴", "九地", "玄武", "螣蛇", "白虎", "直符" };
			m_DiSanPaiC = ListRandom(m_DiSanPaiC);
			m_DiSanPaiC = ListRandom(m_DiSanPaiC);
		}

		private void DianJieSuiJiBut()
		{
			ChuShuiHua();
			FuZhiList();
			m_YiXuanZhoDicA.Clear();
			m_YiXuanZhoDicB.Clear();
			m_YiXuanZhoDicC.Clear();
			XieRu(m_DiYiPaiA, 0);
			XieRu(m_DiErPaiB, 8);
			XieRu(m_DiSanPaiC, 16);
			ChuangZhaoPanDuan();
		}

		private void XieRu(List<string> myList, int shu)
		{
			if (shu < 8)
			{
				for (int i = 0; i < 8; i++)
				{
					QingLiMingChen(i, myList[i]);
					m_YiXuanZhoDicA.Add(i, myList[i]);
				}
			}
			else if (shu < 16)
			{
				for (int j = 0; j < myList.Count; j++)
				{
					QingLiMingChen(shu + j, myList[j]);
					m_YiXuanZhoDicB.Add(shu + j, myList[j]);
				}
			}
			else if (shu < 24)
			{
				for (int k = 0; k < myList.Count; k++)
				{
					QingLiMingChen(shu + k, myList[k]);
					m_YiXuanZhoDicC.Add(shu + k, myList[k]);
				}
			}
		}

		private List<string> ListRandom(List<string> myList)
		{
			System.Random random = new System.Random();
			for (int i = 0; i < myList.Count; i++)
			{
				int num = random.Next(0, myList.Count - 1);
				if (num != i)
				{
					string value = myList[i];
					myList[i] = myList[num];
					myList[num] = value;
				}
			}
			return myList;
		}

		private void XiaYiBuBtnClick()
		{
			int currentZhenFaShiLevel = Singleton<RoleManager>.Instance.GetCurrentZhenFaShiLevel();
			if (m_Classes > currentZhenFaShiLevel)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友,您的阵法水平不足,无法创造该等阶阵图！");
			}
			else
			{
				m_Panel.XuanZheBG.gameObject.SetActive(value: false);
			}
		}

		private void YiJieTog(bool isOn)
		{
			Text component = m_Panel.YiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 1;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void ErJieTog(bool isOn)
		{
			Text component = m_Panel.ErJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 2;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void SanJieTog(bool isOn)
		{
			Text component = m_Panel.SanJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 3;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void SiJieTog(bool isOn)
		{
			Text component = m_Panel.SiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 4;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void WuJieTog(bool isOn)
		{
			Text component = m_Panel.WuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 5;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void LiuJieTog(bool isOn)
		{
			Text component = m_Panel.LiuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 6;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void QiJieTog(bool isOn)
		{
			Text component = m_Panel.QiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 7;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void BaJieTog(bool isOn)
		{
			Text component = m_Panel.BaJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 8;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void JiuJieTog(bool isOn)
		{
			Text component = m_Panel.JiuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_Classes = 9;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void Failure(string content, int wuXing)
		{
			AddButtonClickListener(m_Panel.FailLiKaiBtn, FanHuiBtnClick);
			AddButtonClickListener(m_Panel.FailOnceAgainBtn, ShiBeiGuanBiBut);
			m_Panel.FailPanel.gameObject.SetActive(value: true);
			m_Panel.FailChuangZaoShiBaiContentText.text = content;
			m_Panel.FailWuXingXiaoHao.text = $"{wuXing}";
		}

		private void ShiBeiGuanBiBut()
		{
			m_Panel.FailPanel.gameObject.SetActive(value: false);
		}

		private void Successful(ZS_WorldCaiLiaoModel zhenTu)
		{
			AddButtonClickListener(m_Panel.SuccessLiKaiBtn, FanHuiBtnClick);
			AddButtonClickListener(m_Panel.SuccessOnceAgainBtn, GuanBiiBut);
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			m_QuZhuCheBut(zhenTu);
			m_Panel.SuccessPanel.gameObject.SetActive(value: true);
			m_Panel.SuccessWuXingXiaoHao.text = $"悟性消耗{zhenTu.b}点";
		}

		private void m_QuZhuCheBut(ZS_WorldCaiLiaoModel zhenTu)
		{
			TyepPanDuan(zhenTu.Type);
			m_DataList = new List<string>
			{
				"类型," + m_TypeName,
				"等阶," + WorldSetting.GetStageChineseName(zhenTu.e),
				"品阶," + WorldSetting.GetRankAllName(zhenTu.Rank),
				"五行," + WorldSetting.GetWuXingName((WuXing)zhenTu.f),
				$"灵石消耗,{zhenTu.UnitConsume}灵石",
				$"持续时间,{zhenTu.Duration / 1000.0}息",
				$"灵衍值,{zhenTu.ConversionRate}",
				"创建人," + zhenTu.CreatedBy
			};
			XianShi();
		}

		private void TyepPanDuan(int type)
		{
			switch (type)
			{
			case 1:
				m_TypeName = "杀";
				break;
			case 2:
				m_TypeName = "困";
				break;
			case 3:
				m_TypeName = "幻";
				break;
			case 4:
				m_TypeName = "回蓝";
				break;
			case 5:
				m_TypeName = "回血";
				break;
			}
		}

		private void XianShi()
		{
			QingLi();
			foreach (string data in m_DataList)
			{
				string[] array = data.Split(',');
				DST_ZhuCeTipItem component = SetListContentParent(m_Panel.SuccessContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
				if (array.Length == 2)
				{
					component?.SetValue(array[0] + "：", array[1], "");
				}
				else if (array.Length == 3)
				{
					component?.SetValue(array[0] + "：", array[1], array[2]);
				}
			}
		}

		private void QingLi()
		{
			foreach (RectTransform item in m_Panel.SuccessContent.gameObject.GetComponent<RectTransform>())
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		private void GuanBiiBut()
		{
			m_Panel.SuccessPanel.gameObject.SetActive(value: false);
		}
	}
}
