using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class AH_PaiPingXiangQingWnd : Window
	{
		private AH_PaiPingXiangQingPanel m_Panel;

		public PaiMaiPing m_PaiMaiPing;

		private GameObject m_ItemObj;

		private long m_CurrentBid;

		private GameObject m_XiangQingInstanceObj;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<AH_PaiPingXiangQingPanel>();
			m_XiangQingInstanceObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/RoleZhuangBeiXiangQingItem.prefab");
			m_Panel.QiPaiJia.onEndEdit.AddListener(LingShiShu);
			AddButtonClickListener(m_Panel.JiaBut, JiaHaoXiaoGuo);
			AddButtonClickListener(m_Panel.JianBut, JianHaoXiaoGuo);
			AddButtonClickListener(m_Panel.GuanBiBut, GuanBi);
			AddButtonClickListener(m_Panel.ChuJiaBut, ChuJiaBut);
			m_PaiMaiPing = paralist[0] as PaiMaiPing;
			if (m_PaiMaiPing != null)
			{
				NewShuJu(m_PaiMaiPing);
			}
			else
			{
				string textValue = "网络连接失败！";
				MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
				GuanBi();
			}
			LingShi();
		}

		public void NewShuJu(PaiMaiPing paiMaiPing)
		{
			m_PaiMaiPing = paiMaiPing;
			m_CurrentBid = (long)((double)m_PaiMaiPing.CurrentPrice + (double)m_PaiMaiPing.StartPrice * 0.1);
			MingCheng(m_PaiMaiPing.GoodsName);
			ChuJia(m_PaiMaiPing.CurrentPrice);
			PanDuan();
			HuoQu();
		}

		private void PanDuan()
		{
			m_Panel.ChuJiaBut.interactable = m_PaiMaiPing.DetermineWhetherItCanBeAuctioned;
		}

		private void LingShiShu(string count)
		{
			string text = m_Panel.QiPaiJia.text;
			m_CurrentBid = long.Parse(text);
			PanDu();
		}

		private void LingShi()
		{
			m_Panel.QiPaiJia.text = $"{m_CurrentBid}";
		}

		private void PanDu()
		{
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(m_CurrentBid) < 0.0)
			{
				m_CurrentBid = (long)((double)m_PaiMaiPing.CurrentPrice + (double)m_PaiMaiPing.StartPrice * 0.1);
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前灵石不足");
			}
			else if ((double)m_PaiMaiPing.CurrentPrice + (double)m_PaiMaiPing.StartPrice * 0.1 > (double)m_CurrentBid)
			{
				m_CurrentBid = (long)((double)m_PaiMaiPing.CurrentPrice + (double)m_PaiMaiPing.StartPrice * 0.1);
				MonoSingleton<GameStart>.Instance.PopUpAttention("低于当前最低加价，请重新输入！");
			}
			LingShi();
		}

		private void JiaHaoXiaoGuo()
		{
			m_CurrentBid += (long)((double)m_PaiMaiPing.StartPrice * 0.1);
			PanDu();
		}

		private void JianHaoXiaoGuo()
		{
			m_CurrentBid -= (long)((double)m_PaiMaiPing.StartPrice * 0.1);
			PanDu();
		}

		private void MingCheng(string name)
		{
			m_Panel.NameText.text = name;
		}

		private void ChuJia(long jiaGe)
		{
			m_Panel.BidText.text = $"{jiaGe}灵石";
			m_Panel.ZuiDiBidText.text = $"最低加价+ {(double)m_PaiMaiPing.StartPrice * 0.1} 灵石";
		}

		private void ChuJiaBut()
		{
			TreasureBuyReq_1006020 data = new TreasureBuyReq_1006020
			{
				TreasureId = m_PaiMaiPing.Id,
				Price = m_CurrentBid
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TREASURE_BUY_REQ, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				TreasureBuyResp_1006020 treasureBuyResp_ = new TreasureBuyResp_1006020();
				treasureBuyResp_.MergeFrom(byteString);
				if (!(byteString == null))
				{
					if (treasureBuyResp_.IsSuccess)
					{
						string textValue = "加价成功!";
						MonoSingleton<GameStart>.Instance.PopUpAttention(textValue);
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = treasureBuyResp_.ShinyStone;
						LingShi();
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(treasureBuyResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void HuoQu()
		{
			Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(m_PaiMaiPing.GoodsId, m_PaiMaiPing.GoodsType, m_PaiMaiPing.Level, m_PaiMaiPing.Rank, m_PaiMaiPing.FiveElement, 0, DetailsDisplay);
		}

		private void DetailsDisplay(List<string> detail)
		{
			int num = 0;
			if (m_PaiMaiPing.GoodsId == null || detail == null)
			{
				return;
			}
			float y = m_XiangQingInstanceObj.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = m_Panel.XiangQingContent.gameObject.GetComponent<VerticalLayoutGroup>();
			QingLi();
			foreach (string item in detail)
			{
				if (!item.Contains("回收价格") && !item.Contains("名称"))
				{
					RoleZhuangBeiXiangQingItem component2 = Object.Instantiate(m_XiangQingInstanceObj, m_Panel.XiangQingContent).GetComponent<RoleZhuangBeiXiangQingItem>();
					string[] array = item.Split(',');
					string content = array[0] + "：" + array[1];
					if (array.Length >= 2)
					{
						component2.SetValue(num, content);
					}
					num++;
				}
			}
			float y2 = SetVerticalListContentHight(detail.Count, y, component);
			m_Panel.XiangQingContent.sizeDelta = new Vector2(m_Panel.XiangQingContent.sizeDelta.x, y2);
		}

		private void QingLi()
		{
			foreach (Transform item in m_Panel.XiangQingContent.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void GuanBi()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
