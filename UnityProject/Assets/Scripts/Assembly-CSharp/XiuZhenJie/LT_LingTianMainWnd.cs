using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class LT_LingTianMainWnd : Window
	{
		public LT_LingTianMainPanel Panel;

		private int m_MaxLingTianShu;

		private GameObject m_InstanceItemObj;

		private int m_LingTianShu;

		private List<LT_LingTianItem> m_BaiQiFang = new List<LT_LingTianItem>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LT_LingTianMainPanel>();
			AddButtonClickListener(Panel.TuiChu, TuiChu);
			AddButtonClickListener(Panel.ZhoZi, ZhoZi);
			AddButtonClickListener(Panel.WenHao, XianShi);
			AddButtonClickListener(Panel.LingTu, LingTu);
			AddButtonClickListener(Panel.LingYe, LingYe);
			AddButtonClickListener(Panel.YiJianButton, KaiKen);
			m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/LT_ZhoZhiItem.prefab");
			if (paralist.Length != 0)
			{
				m_MaxLingTianShu = (int)paralist[0] * 4;
			}
			HQMaxLingTianShu();
			EnterPlantResp();
		}

		private void LingTu()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_LingTuWnd.prefab", true);
		}

		private void LingYe()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_LingYeWnd.prefab", true);
		}

		private void HQMaxLingTianShu()
		{
			float y = m_MaxLingTianShu * 55;
			Panel.ScrollContent.sizeDelta = new Vector2(Panel.ScrollContent.sizeDelta.x, y);
		}

		private void EnterPlantResp()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_PLANT_RESP, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterPlantResp_1013001 enterPlantResp_ = new EnterPlantResp_1013001();
					enterPlantResp_.MergeFrom(byteString);
					if (enterPlantResp_.IsSuccess)
					{
						if (enterPlantResp_.Land == null)
						{
							return;
						}
						m_LingTianShu = enterPlantResp_.Land.Count;
						int num = 0;
						for (int i = 1; i <= m_MaxLingTianShu; i++)
						{
							LT_LingTianItem component = SetListContentParent(Panel.ScrollContent, m_InstanceItemObj).GetComponent<LT_LingTianItem>();
							if (enterPlantResp_.Land.Count > 0 && enterPlantResp_.Land.Count > num)
							{
								if (enterPlantResp_.Land[num].Id == i)
								{
									component.ZhiWuID = enterPlantResp_.Land[num].PlantId;
									component.ZhiWuName.text = enterPlantResp_.Land[num].PlantName ?? " ";
									component.m_ID = enterPlantResp_.Land[num].Id;
									component.m_Status = enterPlantResp_.Land[num].Status;
									component.m_TimeRemaining = enterPlantResp_.Land[num].TimeRemaining;
									component.m_TotalWasteTime = enterPlantResp_.Land[num].TotalWasteTime;
									num++;
								}
								else
								{
									component.m_ID = i;
									component.m_Status = 0;
								}
							}
							else
							{
								component.m_ID = i;
								component.m_Status = 0;
							}
							m_BaiQiFang.Add(component);
							component.ShuXingShuJu();
						}
						Panel.YiJianButton.gameObject.SetActive(enterPlantResp_.IsOneKeyKaiKen);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterPlantResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					LingTianNumberNew();
				}
			});
		}

		private void KaiKen()
		{
			foreach (LT_LingTianItem variable in m_BaiQiFang)
			{
				if (variable.m_Status != 0)
				{
					continue;
				}
				KaiKenLandReq_1013003 data = new KaiKenLandReq_1013003
				{
					LandIndex = variable.m_ID
				};
				Singleton<NetManager>.Instance.SendMsg(MsgType.KAI_KEN_LAND_REQ, data, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						KaiKenLandResp_1013003 kaiKenLandResp_ = new KaiKenLandResp_1013003();
						kaiKenLandResp_.MergeFrom(byteString);
						if (kaiKenLandResp_.IsSuccess)
						{
							variable.m_Status = kaiKenLandResp_.LandStatus;
							variable.m_TimeRemaining = kaiKenLandResp_.TimeRemaining;
							variable.m_TotalWasteTime = kaiKenLandResp_.TotalWasteTime;
							Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = kaiKenLandResp_.CurrentVigor;
							Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
							variable.ShuXingShuJu();
						}
						else
						{
							string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(kaiKenLandResp_.Code);
							MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
						}
					}
				});
			}
		}

		private void TuiChu()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void ZhoZi()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_ZhongZiWnd.prefab", true);
		}

		private void XianShi()
		{
			string text = "1、黄阶洞府4块灵田，玄阶8块，地阶12块，天阶16块。 2、田块每次种植完需要重新开垦。 3、开垦灵田后放置已调配好的土壤，蕴灵值越高的土壤肥力越强，则植物生长越快。 4、灵植种子在纳戒中通过采集获得，种植的灵土与灵植种子五行匹配则生长更为迅速。 5、生长过程中的灵植可以通过浇灌灵液加速生长，灵液的加速效率和五行匹配以及蕴灵值有关。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "灵田");
		}

		private void LingTianNumberNew()
		{
			Panel.LingTianNumber.text = $"当前开垦：{m_LingTianShu}块  最大可用：{m_MaxLingTianShu}块";
		}
	}
}
