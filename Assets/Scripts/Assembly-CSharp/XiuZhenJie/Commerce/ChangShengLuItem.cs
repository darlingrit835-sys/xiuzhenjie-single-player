using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChangShengLuItem : MonoBehaviour
	{
		public Image ChangShengLuDanImage;

		public Image ChangShengLuShengImage;

		public Text JingJieText;

		public Text WuPingZhoLeiText;

		public Image WuPingZhoLeiImage;

		public Image WuPingZhoLeiImage2;

		public Image WuPingZhoLeiImage3;

		public Image WuPingZhoLeiImage4;

		public Button YiLingQuButton;

		public Button LingQuButton;

		public Button JieSuoButton;

		public Button LingQuHuiButton;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private int Level;

		public void PanDuanXianShi(bool beiJing, bool jieSuo, bool lingQu, string jingJie, string wuPingZhoLei, int wuPingLeiXing, int level, UnityAction<bool> huLueAction, int listIndex)
		{
			float y = -listIndex * 48 - 24;
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, y);
			component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
			Level = level;
			ResourceLoad();
			JingJieText.text = jingJie;
			WuPingZhoLeiText.text = wuPingZhoLei;
			BeiJing(beiJing);
			JieSuo(jieSuo, lingQu);
			WuPingLeiXing(wuPingLeiXing);
			if (huLueAction != null)
			{
				JieSuoButton.onClick.AddListener(() =>
				{
					huLueAction(base.gameObject);
				});
			}
		}

		private void BeiJing(bool beiJing)
		{
			if (beiJing)
			{
				ChangShengLuDanImage.gameObject.SetActive(value: true);
				ChangShengLuShengImage.gameObject.SetActive(value: false);
			}
			else
			{
				ChangShengLuDanImage.gameObject.SetActive(value: false);
				ChangShengLuShengImage.gameObject.SetActive(value: true);
			}
		}

		private void JieSuo(bool jieSuo, bool lingQu)
		{
			if (jieSuo)
			{
				if (lingQu)
				{
					JieSuoButton.gameObject.SetActive(value: false);
					YiLingQuButton.gameObject.SetActive(value: true);
					LingQuButton.gameObject.SetActive(value: false);
					LingQuHuiButton.gameObject.SetActive(value: false);
					return;
				}
				JieSuoButton.gameObject.SetActive(value: false);
				YiLingQuButton.gameObject.SetActive(value: false);
				if (Singleton<RoleManager>.Instance.Role_Entity.RoleLevel >= Level)
				{
					LingQuButton.gameObject.SetActive(value: true);
					LingQuHuiButton.gameObject.SetActive(value: false);
					LingQuButton.onClick.RemoveListener(LingQu);
					LingQuButton.onClick.AddListener(LingQu);
				}
				else
				{
					LingQuButton.gameObject.SetActive(value: false);
					LingQuHuiButton.gameObject.SetActive(value: true);
				}
			}
			else
			{
				JieSuoButton.gameObject.SetActive(value: true);
				YiLingQuButton.gameObject.SetActive(value: false);
				LingQuButton.gameObject.SetActive(value: false);
				LingQuHuiButton.gameObject.SetActive(value: false);
			}
		}

		private void LingQu()
		{
			GetCSLGoodsReq_1002028 data = new GetCSLGoodsReq_1002028
			{
				Level = Level
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GETCSL_GOODS, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GetCSLGoodsResp_1002028 getCSLGoodsResp_ = new GetCSLGoodsResp_1002028();
					getCSLGoodsResp_.MergeFrom(byteString);
					if (getCSLGoodsResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyJade = (int)getCSLGoodsResp_.ShinyJade;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						JieSuo(jieSuo: true, lingQu: true);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(getCSLGoodsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JieSuo()
		{
		}

		private void WuPingLeiXing(int wuPingLeiXing)
		{
			YingChang();
			switch (wuPingLeiXing)
			{
			case 1:
				WuPingZhoLeiImage.gameObject.SetActive(value: true);
				break;
			case 4:
				WuPingZhoLeiImage4.gameObject.SetActive(value: true);
				break;
			case 7:
				WuPingZhoLeiImage2.gameObject.SetActive(value: true);
				break;
			case 10:
				WuPingZhoLeiImage3.gameObject.SetActive(value: true);
				break;
			}
		}

		private void YingChang()
		{
			WuPingZhoLeiImage.gameObject.SetActive(value: false);
			WuPingZhoLeiImage2.gameObject.SetActive(value: false);
			WuPingZhoLeiImage3.gameObject.SetActive(value: false);
			WuPingZhoLeiImage4.gameObject.SetActive(value: false);
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
		}
	}
}
