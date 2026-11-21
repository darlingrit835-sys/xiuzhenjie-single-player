using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LT_LingTianItem : MonoBehaviour
	{
		public Image KaiKenImage;

		public Button KaiKenButton;

		public Image KaiPiImage;

		public Image JingDuImage;

		public Text JingDuText;

		public Text JinDuTimeText;

		public Image ZhiZwImage;

		public Button TuRangButton;

		public Button ZhoZhiButton;

		public Button JiaoGuanButton;

		public Button ShoHuoButton;

		public Text ZhiWuJinDuTimeText;

		public Image ZhiWuJinDuTiaoImage;

		public Image ZhoZhiImage;

		public Image JiaoGuanImage;

		public Image JiaoGuan_2Image;

		public Image ShouHuoImage;

		public Image KeZhoZhi;

		public Button ZhiWuXiangQing;

		public Text ZhiWuName;

		public string ZhiWuID;

		public int m_ID;

		public int m_Status;

		public long m_TimeRemaining;

		public long m_TotalWasteTime;

		private bool JinDu;

		internal System.Action StateChanged;

		private void Awake()
		{
			InkInterfaceArt.Set(GetComponent<Image>(), "Parchment", false);
			InkInterfaceArt.Set(KaiKenImage, "LightPaper", false);
			InkInterfaceArt.Set(KaiPiImage, "LightPaper", false);
			InkInterfaceArt.Set(ZhiZwImage, "Parchment", false);
			InkInterfaceArt.Set(ZhoZhiImage.transform.parent.GetComponent<Image>(), "LightPaper", false);
			ZhiWuName.color = InkInterfaceArt.Ink;
			ZhiWuJinDuTimeText.color = InkInterfaceArt.Ink;
			JinDuTimeText.color = InkInterfaceArt.Ink;
			KaiPiImage.transform.Find("Text").GetComponent<Text>().color = InkInterfaceArt.Ink;
			InkDialogArt.StyleButton(KaiKenButton, "ParchmentHeading", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(TuRangButton, "ParchmentHeading", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(ZhoZhiButton, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(JiaoGuanButton, "JadeButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(ShoHuoButton, "JadeButton", InkDialogArt.Ivory);
		}

		private void ZiYuanJiaZai()
		{
			KaiKenImage.gameObject.SetActive(value: false);
			KaiKenButton.gameObject.SetActive(value: false);
			ZhiWuXiangQing.gameObject.SetActive(value: false);
			KaiPiImage.gameObject.SetActive(value: false);
			JingDuImage.gameObject.SetActive(value: false);
			JingDuText.gameObject.SetActive(value: false);
			JinDuTimeText.gameObject.SetActive(value: false);
			ZhiZwImage.gameObject.SetActive(value: false);
			TuRangButton.gameObject.SetActive(value: false);
			ZhoZhiButton.gameObject.SetActive(value: false);
			JiaoGuanButton.gameObject.SetActive(value: false);
			ShoHuoButton.gameObject.SetActive(value: false);
			ZhiWuJinDuTimeText.gameObject.SetActive(value: false);
			ZhiWuJinDuTiaoImage.gameObject.SetActive(value: false);
			ZhoZhiImage.gameObject.SetActive(value: false);
			JiaoGuanImage.gameObject.SetActive(value: false);
			JiaoGuan_2Image.gameObject.SetActive(value: false);
			ShouHuoImage.gameObject.SetActive(value: false);
			KeZhoZhi.gameObject.SetActive(value: false);
		}

		public void ShuXingShuJu()
		{
			if (!base.gameObject.Equals(null))
			{
				ZiYuanJiaZai();
				BtoOnClick();
				switch (m_Status)
				{
				case 0:
					KaiKenImage.gameObject.SetActive(value: true);
					KaiKenButton.gameObject.SetActive(value: true);
					break;
				case 1:
					KaiPiImage.gameObject.SetActive(value: true);
					JingDuImage.gameObject.SetActive(value: true);
					JingDuText.gameObject.SetActive(value: true);
					JinDuTimeText.gameObject.SetActive(value: true);
					break;
				case 2:
					TuRangButton.interactable = true;
					ZhiZwImage.gameObject.SetActive(value: true);
					TuRangButton.gameObject.SetActive(value: true);
					ZhoZhiButton.gameObject.SetActive(value: true);
					ZhoZhiButton.interactable = false;
					ZhoZhiImage.gameObject.SetActive(value: true);
					break;
				case 3:
					ZhiZwImage.gameObject.SetActive(value: true);
					KeZhoZhi.gameObject.SetActive(value: true);
					TuRangButton.gameObject.SetActive(value: true);
					ZhoZhiButton.gameObject.SetActive(value: true);
					ZhoZhiButton.interactable = true;
					TuRangButton.interactable = true;
					ZhoZhiImage.gameObject.SetActive(value: false);
					break;
				case 4:
					JiaoGuanButton.interactable = true;
					PanDuanImg();
					JiaoGuanButton.gameObject.SetActive(value: true);
					break;
				case 5:
					PanDuanImg();
					JiaoGuanButton.gameObject.SetActive(value: true);
					JiaoGuanButton.interactable = false;
					break;
				case 6:
					PanDuanImg();
					break;
				}
				StateChanged?.Invoke();
			}
		}

		private void PanDuanImg()
		{
			ZhiWuXiangQing.gameObject.SetActive(value: true);
			ZhiZwImage.gameObject.SetActive(value: true);
			TuRangButton.gameObject.SetActive(value: true);
			TuRangButton.interactable = false;
			ZhiWuJinDuTimeText.gameObject.SetActive(value: true);
			ZhiWuJinDuTiaoImage.gameObject.SetActive(value: true);
			if (m_TimeRemaining <= 0)
			{
				ShouHuoImage.gameObject.SetActive(value: true);
				ShoHuoButton.gameObject.SetActive(value: true);
			}
			if (m_TotalWasteTime / 2 < m_TimeRemaining)
			{
				JinDu = false;
				JiaoGuanImage.gameObject.SetActive(value: true);
			}
			else
			{
				JiaoGuan_2Image.gameObject.SetActive(value: true);
				JinDu = true;
			}
		}

		private void FixedUpdate()
		{
			if (m_TimeRemaining > 0)
			{
				m_TimeRemaining -= (int)(Time.fixedDeltaTime * 1000f);
				switch (m_Status)
				{
				case 1:
				{
					JinDuTimeText.text = ((m_TimeRemaining > 60000) ? ("剩余: " + m_TimeRemaining / 60000 + " 分钟") : ("剩余: " + m_TimeRemaining / 1000 + " 秒"));
					float num = 1f - (float)m_TimeRemaining * 1f / (float)m_TotalWasteTime;
					JingDuText.text = $"{num * 100f:F2}%";
					JingDuImage.fillAmount = num;
					break;
				}
				case 4:
				case 5:
				{
					ZhiWuJinDuTimeText.text = string.Format(TimeToString((int)m_TimeRemaining));
					float fillAmount = 1f - (float)m_TimeRemaining * 1f / (float)m_TotalWasteTime;
					ZhiWuJinDuTiaoImage.fillAmount = fillAmount;
					break;
				}
				}
			}
			if (m_TimeRemaining > 0)
			{
				return;
			}
			switch (m_Status)
			{
			case 1:
				JingDuImage.fillAmount = 1f;
				m_Status++;
				ShuXingShuJu();
				break;
			case 4:
			case 5:
				JinDu = true;
				ZhiWuJinDuTimeText.text = TimeToString(0);
				ZhiWuJinDuTiaoImage.fillAmount = 1f;
				m_Status = 6;
				ShuXingShuJu();
				break;
			case 6:
				if (JinDu)
				{
					ZhiWuJinDuTimeText.gameObject.SetActive(value: false);
					ZhiWuJinDuTiaoImage.fillAmount = 1f;
				}
				break;
			case 2:
			case 3:
				break;
			}
		}

		private static string TimeToString(int result)
		{
			int num = result / 1000 / 60 / 60;
			int num2 = (result - num * 3600000) / 60000;
			int num3 = (result - num * 3600000 - num2 * 60000) / 1000;
			return $"{num:D2}:{num2:D2}:{num3:D2}";
		}

		private void BtoOnClick()
		{
			KaiKenButton.onClick.RemoveListener(KaiKenBut);
			KaiKenButton.onClick.AddListener(KaiKenBut);
			TuRangButton.onClick.RemoveListener(TuRangBut);
			TuRangButton.onClick.AddListener(TuRangBut);
			ZhoZhiButton.onClick.RemoveListener(ZhoZhiBut);
			ZhoZhiButton.onClick.AddListener(ZhoZhiBut);
			JiaoGuanButton.onClick.RemoveListener(JiaoGuanBut);
			JiaoGuanButton.onClick.AddListener(JiaoGuanBut);
			ShoHuoButton.onClick.RemoveListener(ShoHuoBut);
			ShoHuoButton.onClick.AddListener(ShoHuoBut);
			ZhiWuXiangQing.onClick.RemoveListener(XiangQing);
			ZhiWuXiangQing.onClick.AddListener(XiangQing);
		}

		private void XiangQing()
		{
			Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(ZhiWuID, (List<string> list) =>
			{
				PopUpXiangQingPanel(ZhiWuName.text, "关闭", null, list);
			});
		}

		public void KaiKenBut()
		{
			string xiangQingData = $"开垦第{m_ID}地块，大约需要10分钟才能完成，本次将消耗10点精力";
			PopUpXiangQingPanel($"开垦第{m_ID}块地", "确认", UnityAction, xiangQingData);
			void UnityAction(object obj)
			{
				LianWangKaiKenBut();
			}
		}

		private void LianWangKaiKenBut()
		{
			KaiKenLandReq_1013003 data = new KaiKenLandReq_1013003
			{
				LandIndex = m_ID
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
						m_Status = kaiKenLandResp_.LandStatus;
						m_TimeRemaining = kaiKenLandResp_.TimeRemaining;
						m_TotalWasteTime = kaiKenLandResp_.TotalWasteTime;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = kaiKenLandResp_.CurrentVigor;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						ShuXingShuJu();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(kaiKenLandResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void TuRangBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_TuRangGenHuanWnd.prefab", false, true, this);
		}

		private void ZhoZhiBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_ZhongZiWnd.prefab", false, true, this);
		}

		private void JiaoGuanBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_JiaoGuanWnd.prefab", false, true, this);
		}

		private void ShoHuoBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("LT_ShoHuoWnd.prefab", false, true, this);
		}

		private static void PopUpXiangQingPanel(string canWuActionModel, string btnName, UnityAction<object> sureBtnCallBack, string xiangQingData)
		{
			Common_QiRenParamEntity common_QiRenParamEntity = new Common_QiRenParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = canWuActionModel,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_QiRenWnd.prefab", true, common_QiRenParamEntity);
		}

		private static void PopUpXiangQingPanel(string name, string btnName, UnityAction<object> sureBtnCallBack, List<string> xiangQingData)
		{
			Common_TipWndParamEntity common_TipWndParamEntity = new Common_TipWndParamEntity
			{
				BtnName = btnName,
				CloseBtnClick = null,
				ContentList = xiangQingData,
				DataSource = null,
				TitleName = name,
				SureBtnClick = sureBtnCallBack
			};
			Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, common_TipWndParamEntity);
		}
	}
}
