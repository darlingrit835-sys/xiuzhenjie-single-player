using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_ChuangZaoWnd : Window
	{
		public LZ_ChuangZaoPanel Panel;

		private int m_Classes;

		public DST_CommonEntity m_LingZhiEntily;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_ItemObj;

		public Dictionary<int, LZ_WorldCaiLiaoModel> m_LingZhiShu;

		private List<string> dataList;

		private LZ_WorldCaiLiaoModel m_WorldCaiLiaoModel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LZ_ChuangZaoPanel>();
			m_LingZhiShu = new Dictionary<int, LZ_WorldCaiLiaoModel>();
			m_Classes = 1;
			ResourceLoad();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtn);
			AddButtonClickListener(Panel.WeiHao, WeiHao);
			AddButtonClickListener(Panel.XiaYiBuBtn, XiaYiBuBtnClick);
			AddToggleClickListener(Panel.YiJieTog, YiJieTog);
			AddToggleClickListener(Panel.ErJieTog, ErJieTog);
			AddToggleClickListener(Panel.SanJieTog, SanJieTog);
			AddToggleClickListener(Panel.SiJieTog, SiJieTog);
			AddToggleClickListener(Panel.WuJieTog, WuJieTog);
			AddToggleClickListener(Panel.LiuJieTog, LiuJieTog);
			AddToggleClickListener(Panel.QiJieTog, QiJieTog);
			AddToggleClickListener(Panel.BaJieTog, BaJieTog);
			AddToggleClickListener(Panel.JiuJieTog, JiuJieTog);
			YiJieTog(isOn: true);
			Panel.XuanZheBG.gameObject.SetActive(value: true);
		}

		private void XiaYiBuBtnClick()
		{
			int currentLingZhiLevel = Singleton<RoleManager>.Instance.GetCurrentLingZhiLevel();
			if (m_Classes > currentLingZhiLevel)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友,您的灵植水平不足,无法创造该等阶植物！");
				return;
			}
			Registration();
			Panel.XuanZheBG.gameObject.SetActive(value: false);
			Panel.BeiYuBeiJing.gameObject.SetActive(value: true);
		}

		private void YiJieTog(bool isOn)
		{
			Text component = Panel.YiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.ErJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.SanJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.SiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.WuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.LiuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.QiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.BaJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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
			Text component = Panel.JiuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
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

		private void Registration()
		{
			AddButtonClickListener(Panel.Simulation, Simulation);
			AddButtonClickListener(Panel.ZhiWUBut_1, ZhiWUBut_1);
			AddButtonClickListener(Panel.ZhiWUBut_2, ZhiWUBut_2);
			AddButtonClickListener(Panel.ZhiWUBut_3, ZhiWUBut_3);
			AddButtonClickListener(Panel.ZhiWUBut_4, ZhiWUBut_4);
			AddButtonClickListener(Panel.ZhiWUBut_5, ZhiWUBut_5);
			AddButtonClickListener(Panel.ZhiWUBut_6, ZhiWUBut_6);
			AddButtonClickListener(Panel.ZhiWUBut_7, ZhiWUBut_7);
			AddButtonClickListener(Panel.ZhiWUBut_8, ZhiWUBut_8);
			AddButtonClickListener(Panel.ZhiWUBut_9, ZhiWUBut_9);
			AddButtonClickListener(Panel.ZhiWUBut_10, ZhiWUBut_10);
			AddButtonClickListener(Panel.ZhiWUBut_11, ZhiWUBut_11);
			AddButtonClickListener(Panel.ZhiWUBut_12, ZhiWUBut_12);
			AddButtonClickListener(Panel.ZhiWUBut_13, ZhiWUBut_13);
		}

		private void Simulation()
		{
			if (m_LingZhiShu.ContainsKey(12))
			{
				if (m_LingZhiShu.Count > 4)
				{
					VerifyCreateTemplatePlantReq_1008058 verifyCreateTemplatePlantReq_ = new VerifyCreateTemplatePlantReq_1008058
					{
						Stage = m_Classes
					};
					new List<string>();
					foreach (KeyValuePair<int, LZ_WorldCaiLiaoModel> item in m_LingZhiShu)
					{
						VerifyCreateTemplatePlantReq_1008058.Types.TemplatePlantReq_1008065 templatePlantReq_ = new VerifyCreateTemplatePlantReq_1008058.Types.TemplatePlantReq_1008065();
						templatePlantReq_.Location = item.Key;
						templatePlantReq_.TemplatePlantId = item.Value.a;
						verifyCreateTemplatePlantReq_.Plant.Add(templatePlantReq_);
					}
					Singleton<NetManager>.Instance.SendMsg(MsgType.VERIFY_CREATE_TEMPLATE_PLANT_RESP, verifyCreateTemplatePlantReq_, (object _obj) =>
					{
						MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
						ByteString byteString = _obj as ByteString;
						if (!(byteString == null))
						{
							VerifyCreateTemplatePlantResp_1008058 verifyCreateTemplatePlantResp_ = new VerifyCreateTemplatePlantResp_1008058();
							verifyCreateTemplatePlantResp_.MergeFrom(byteString);
							m_WorldCaiLiaoModel = new LZ_WorldCaiLiaoModel();
							m_LingZhiEntily = new DST_CommonEntity();
							if (verifyCreateTemplatePlantResp_.IsSuccess)
							{
								m_WorldCaiLiaoModel.b = Singleton<RoleManager>.Instance.Role_Entity.CurrentClever - verifyCreateTemplatePlantResp_.CurrentClever;
								if (verifyCreateTemplatePlantResp_.CreatedIsSuccess)
								{
									m_WorldCaiLiaoModel.a = verifyCreateTemplatePlantResp_.TemplatePlantId;
									m_WorldCaiLiaoModel.h = (int)verifyCreateTemplatePlantResp_.FiveFlavour;
									m_WorldCaiLiaoModel.e = verifyCreateTemplatePlantResp_.Stage;
									m_WorldCaiLiaoModel.f = verifyCreateTemplatePlantResp_.FiveElement;
									m_WorldCaiLiaoModel.g = (int)verifyCreateTemplatePlantResp_.FourGas;
									m_WorldCaiLiaoModel.i = (int)verifyCreateTemplatePlantResp_.ReinforcingDiarrhoea;
									m_WorldCaiLiaoModel.j = verifyCreateTemplatePlantResp_.AffiliationAcupoint;
									m_WorldCaiLiaoModel.ConversionRate = (int)verifyCreateTemplatePlantResp_.ConversionRate;
									m_WorldCaiLiaoModel.Rank = verifyCreateTemplatePlantResp_.Rank;
									m_WorldCaiLiaoModel.CreatedBy = verifyCreateTemplatePlantResp_.CreatedBy;
									m_LingZhiEntily.Level = verifyCreateTemplatePlantResp_.Stage;
									m_LingZhiEntily.Id = verifyCreateTemplatePlantResp_.TemplatePlantId;
									successful(m_WorldCaiLiaoModel);
								}
								else
								{
									string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateTemplatePlantResp_.Code);
									Failure(hintMsgByCode);
								}
								Singleton<RoleManager>.Instance.Role_Entity.CurrentClever = verifyCreateTemplatePlantResp_.CurrentClever;
								Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							}
							else
							{
								string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(verifyCreateTemplatePlantResp_.Code);
								MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2);
							}
						}
					}, isShowWaitUI: true);
				}
				else
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("最低选择5个材料!");
				}
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请选择一株灵植作为根茎!");
			}
		}

		private void ZhiWUBut_1()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 1);
		}

		private void ZhiWUBut_2()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 2);
		}

		private void ZhiWUBut_3()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 3);
		}

		private void ZhiWUBut_4()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 4);
		}

		private void ZhiWUBut_5()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 5);
		}

		private void ZhiWUBut_6()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 6);
		}

		private void ZhiWUBut_7()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 7);
		}

		private void ZhiWUBut_8()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 8);
		}

		private void ZhiWUBut_9()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 9);
		}

		private void ZhiWUBut_10()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 10);
		}

		private void ZhiWUBut_11()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 11);
		}

		private void ZhiWUBut_12()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 12);
		}

		private void ZhiWUBut_13()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LZ_CaiLiaoShaiXuanWnd.prefab", true, m_Classes, 13);
		}

		public void ChoosedCaoYao(LZ_WorldCaiLiaoModel ZhiWu, int Shu)
		{
			switch (Shu)
			{
			case 1:
				Panel.XuanZhoZhiWUBut_1.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_1, ZhiWu, Shu);
				Panel.ZhiWUImg_1.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_1.gameObject.SetActive(value: false);
				break;
			case 2:
				Panel.XuanZhoZhiWUBut_2.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_2, ZhiWu, Shu);
				Panel.ZhiWUImg_2.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_2.gameObject.SetActive(value: false);
				break;
			case 3:
				Panel.XuanZhoZhiWUBut_3.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_3, ZhiWu, Shu);
				Panel.ZhiWUImg_3.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_3.gameObject.SetActive(value: false);
				break;
			case 4:
				Panel.XuanZhoZhiWUBut_4.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_4, ZhiWu, Shu);
				Panel.ZhiWUImg_4.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_4.gameObject.SetActive(value: false);
				break;
			case 5:
				Panel.XuanZhoZhiWUBut_5.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_5, ZhiWu, Shu);
				Panel.ZhiWUImg_5.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_5.gameObject.SetActive(value: false);
				break;
			case 6:
				Panel.XuanZhoZhiWUBut_6.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_6, ZhiWu, Shu);
				Panel.ZhiWUImg_6.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_6.gameObject.SetActive(value: false);
				break;
			case 7:
				Panel.XuanZhoZhiWUBut_7.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_7, ZhiWu, Shu);
				Panel.ZhiWUImg_7.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_7.gameObject.SetActive(value: false);
				break;
			case 8:
				Panel.XuanZhoZhiWUBut_8.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_8, ZhiWu, Shu);
				Panel.ZhiWUImg_8.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_8.gameObject.SetActive(value: false);
				break;
			case 9:
				Panel.XuanZhoZhiWUBut_9.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_9, ZhiWu, Shu);
				Panel.ZhiWUImg_9.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_9.gameObject.SetActive(value: false);
				break;
			case 10:
				Panel.XuanZhoZhiWUBut_10.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_10, ZhiWu, Shu);
				Panel.ZhiWUImg_10.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_10.gameObject.SetActive(value: false);
				break;
			case 11:
				Panel.XuanZhoZhiWUBut_11.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_11, ZhiWu, Shu);
				Panel.ZhiWUImg_11.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_11.gameObject.SetActive(value: false);
				break;
			case 12:
				Panel.XuanZhoZhiWUBut_12.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_12, ZhiWu, Shu);
				Panel.ZhiWUImg_12.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_12.gameObject.SetActive(value: false);
				break;
			case 13:
				Panel.XuanZhoZhiWUBut_13.gameObject.SetActive(value: true);
				FuZhi(Panel.XuanZhoZhiWUBut_13, ZhiWu, Shu);
				Panel.ZhiWUImg_13.gameObject.SetActive(value: true);
				Panel.XuanZhoZhiWUImg_13.gameObject.SetActive(value: false);
				break;
			}
			if (m_LingZhiShu.ContainsKey(Shu))
			{
				m_LingZhiShu[Shu] = ZhiWu;
			}
			else
			{
				m_LingZhiShu.Add(Shu, ZhiWu);
			}
		}

		private void FuZhi(UI_GoodsItem NaJieKongJian, LZ_WorldCaiLiaoModel ZhiWu, int Shu)
		{
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(1, out value);
			int levelByRank = WorldSetting.GetLevelByRank(ZhiWu.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			switch (Shu)
			{
			case 1:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_1();
				});
				break;
			case 2:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_2();
				});
				break;
			case 3:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_3();
				});
				break;
			case 4:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_4();
				});
				break;
			case 5:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_5();
				});
				break;
			case 6:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_6();
				});
				break;
			case 7:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_7();
				});
				break;
			case 8:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_8();
				});
				break;
			case 9:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_9();
				});
				break;
			case 10:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_10();
				});
				break;
			case 11:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_11();
				});
				break;
			case 12:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_12();
				});
				break;
			case 13:
				NaJieKongJian.SetItem(ZhiWu, ZhiWu.c, ZhiWu.f, ZhiWu.e, value2, value, (object _obj, GameObject _gameObj) =>
				{
					ZhiWUBut_13();
				});
				break;
			}
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhiWu.png");
			m_GoodsIconSprite.Add(1, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void successful(LZ_WorldCaiLiaoModel ZhiWu)
		{
			AddButtonClickListener(Panel.SuccessLiKaiBtn, LiKaiBtn);
			AddButtonClickListener(Panel.SuccessOnceAgainBtn, GuanBiiBut);
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			m_QuZhuCheBut(ZhiWu);
			Panel.SuccessPanel.gameObject.SetActive(value: true);
			Panel.SuccessWuXingXiaoHao.text = $"悟性消耗{ZhiWu.b}点";
		}

		private void m_QuZhuCheBut(LZ_WorldCaiLiaoModel ZhiWu)
		{
			dataList = new List<string>();
			dataList.Add(string.Format("{0},{1}", "品阶", WorldSetting.GetLevelByRankName(ZhiWu.Rank)));
			dataList.Add(string.Format("{0},{1}", "等阶", WorldSetting.GetStageChineseName(ZhiWu.e)));
			dataList.Add(string.Format("{0},{1}", "产地", "虚神界"));
			dataList.Add(string.Format("{0},{1}", "五行", WorldSetting.GetWuXingName((WuXing)ZhiWu.f)));
			dataList.Add(string.Format("{0},{1}", "四气", WorldSetting.SiQiName(ZhiWu.g)));
			dataList.Add(string.Format("{0},{1}", "五味", WorldSetting.WuWeiName(ZhiWu.h)));
			dataList.Add(string.Format("{0},{1}", "补泻", WorldSetting.BuXieName(ZhiWu.i)));
			dataList.Add(string.Format("{0},{1}", "归经", WorldSetting.GuiJingName(ZhiWu.j)));
			XianShi();
		}

		private void XianShi()
		{
			QingLi();
			foreach (string data in dataList)
			{
				string[] array = data.Split(',');
				DST_ZhuCeTipItem component = SetListContentParent(Panel.SuccessContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
				if (array != null && array.Length == 2)
				{
					component?.SetValue(array[0] + "：", array[1], "");
				}
				else if (array != null && array.Length == 3)
				{
					component?.SetValue(array[0] + "：", array[1], array[2]);
				}
			}
		}

		private void QingLi()
		{
			foreach (RectTransform item in Panel.SuccessContent.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void GuanBiiBut()
		{
			Panel.SuccessPanel.gameObject.SetActive(value: false);
		}

		private void Failure(string content)
		{
			AddButtonClickListener(Panel.FailLiKaiBtn, LiKaiBtn);
			AddButtonClickListener(Panel.FailOnceAgainBtn, ShiBeiGuanBiBut);
			Panel.FailPanel.gameObject.SetActive(value: true);
			Panel.FailChuangZaoShiBaiContentText.text = content;
			Panel.FailWuXingXiaoHao.text = m_WorldCaiLiaoModel.b + "点";
		}

		private void ShiBeiGuanBiBut()
		{
			Panel.FailPanel.gameObject.SetActive(value: false);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void WeiHao()
		{
			string text = "灵植创造规则：\n1.选择一株灵植作为根茎\n2.在灵植的不同生长状态中嫁接其他灵植\n3.模拟培育灵植，可得知灵植是否嫁接成活\n灵植创造要求：\n1.无法创造超过自身境界的灵植\n2.无法创造超过自身灵植师等阶的灵植\n3.练气期（一品灵植师）可以选择5种植物，每提升一个境界添加1种植物\n4.创造成功扣除所选灵植个数与灵植品阶乘积的悟性点，悟性可能出现负数\n5.创造失败扣除等同于所选灵植个数的悟性点";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "创造灵植");
		}
	}
}
