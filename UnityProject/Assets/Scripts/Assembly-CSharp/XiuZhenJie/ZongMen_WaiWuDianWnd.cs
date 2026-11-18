using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_WaiWuDianWnd : Window
	{
		private ZongMen_WaiWuDianPanel m_Panel;

		private int m_Id1;

		private int m_Id2;

		private int m_Time1;

		private int m_Time2;

		private bool m_PanDuanXiuGai;

		private bool m_YiWanCheng1;

		private bool m_YiWanCheng2;

		private int m_TiShengLevel;

		private IntoWaiWuResp_1015038 m_IntoWaiWuResp1015038 = new IntoWaiWuResp_1015038();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZongMen_WaiWuDianPanel>();
			m_IntoWaiWuResp1015038 = paralist[2] as IntoWaiWuResp_1015038;
			AddButtonClickListener(m_Panel.LiKaiButton, LiKaiBtn);
			AddButtonClickListener(m_Panel.WenHaoButton, WenHaoTogClick);
			AddButtonClickListener(m_Panel.FaBuButton, FaBuBut);
			HuoQuZhangLaoXingXi();
		}

		public void HuoQuZhangLaoXingXi()
		{
			m_YiWanCheng1 = false;
			m_YiWanCheng2 = false;
			if (m_IntoWaiWuResp1015038.ZhangLaoInfo != null)
			{
				m_PanDuanXiuGai = Singleton<RoleManager>.Instance.Role_Entity.Id == m_IntoWaiWuResp1015038.ZhangLaoInfo.RoleId;
				m_Panel.ZhangLaoWuImage.gameObject.SetActive(value: false);
				ZhangLaoFuZhi(m_IntoWaiWuResp1015038.ZhangLaoInfo);
			}
			else
			{
				m_Panel.ZhangLaoWuImage.gameObject.SetActive(value: true);
			}
			CurrentContribution(m_IntoWaiWuResp1015038.CurrentContribution);
			if (m_IntoWaiWuResp1015038.XuanShangInfo.Count > 0)
			{
				m_Time1 = (int)m_IntoWaiWuResp1015038.XuanShangInfo[0].ShengYuTime;
				m_Id1 = m_IntoWaiWuResp1015038.XuanShangInfo[0].Id;
				XuanShangInfo(m_IntoWaiWuResp1015038.XuanShangInfo[0]);
				m_Panel.ForTheTaskImage.gameObject.SetActive(value: true);
			}
			else
			{
				m_Panel.ForTheTaskImage.gameObject.SetActive(value: false);
			}
			if (m_IntoWaiWuResp1015038.XuanShangInfo.Count > 1)
			{
				m_Time2 = (int)m_IntoWaiWuResp1015038.XuanShangInfo[1].ShengYuTime;
				m_Id2 = m_IntoWaiWuResp1015038.XuanShangInfo[1].Id;
				XuanShangInfo_1(m_IntoWaiWuResp1015038.XuanShangInfo[1]);
				m_Panel.ForTheTask_2Image.gameObject.SetActive(value: true);
			}
			else
			{
				m_Panel.ForTheTask_2Image.gameObject.SetActive(value: false);
			}
		}

		private void ZhangLaoFuZhi(IntoWaiWuResp_1015038.Types.ZhangLaoInfoResp_1015039 xingXi)
		{
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(xingXi.Sex, xingXi.RoleImage, isOnline: true);
			m_Panel.PortraitImage.gameObject.GetComponent<Image>().sprite = sprite2;
			m_Panel.NameText.text = xingXi.RoleName;
			m_Panel.ZhanLiText.text = $"{xingXi.Fighting}";
			m_TiShengLevel = (xingXi.Level - 1) / 10 + 1;
			m_Panel.ProductionText.text = $"提升悬赏任务成功率{m_TiShengLevel}%";
			m_Panel.JingJieText.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(xingXi.Level);
		}


		public override void OnFixUpdate()
		{
			base.OnFixUpdate();
			if (m_Time1 > 0)
			{
				m_Time1 -= (int)Time.fixedDeltaTime;
				m_Panel.YuJiTimeText.text = TimeToString(m_Time1);
			}
			else if (!m_YiWanCheng1)
			{
				m_YiWanCheng1 = true;
				JieSuanPanDuan(1);
			}
			if (m_Time2 > 0)
			{
				m_Time2 -= (int)Time.fixedDeltaTime;
				m_Panel.YuJiTime_2Text.text = TimeToString(m_Time2);
			}
			else if (!m_YiWanCheng2)
			{
				m_YiWanCheng2 = true;
				JieSuanPanDuan_1(1);
			}
		}

		private static string TimeToString(int result)
		{
			int num = result / 1000 / 60;
			if (num > 0 && num < 2)
			{
				num = 1;
			}
			return $"{num}分钟";
		}

		private void XuanShangInfo(IntoWaiWuResp_1015038.Types.XuanShangInfoResp_1015040 xuanShangInfo)
		{
			m_Id1 = xuanShangInfo.Id;
			m_Panel.WuPingNameText.text = xuanShangInfo.GoodsName;
			m_Panel.LingShiJiangLiText.text = $"{xuanShangInfo.Stone}";
			m_Panel.ChengGonglvText.text = $"{xuanShangInfo.SuccessRate}%";
			JieSuanPanDuan(xuanShangInfo.Status);
		}

		private void JieSuanPanDuan(int status)
		{
			m_Panel.YuJiTimeImage.gameObject.SetActive(value: false);
			m_Panel.YiWanChengText.gameObject.SetActive(value: false);
			m_Panel.JieSuanButton.gameObject.SetActive(value: false);
			m_Panel.DenDaiButton.gameObject.SetActive(value: false);
			switch (status)
			{
			case 0:
				m_Panel.YuJiTimeImage.gameObject.SetActive(value: true);
				m_Panel.DenDaiButton.gameObject.SetActive(value: true);
				break;
			case 1:
				m_Panel.YiWanChengText.gameObject.SetActive(value: true);
				m_Panel.JieSuanButton.gameObject.SetActive(value: true);
				AddButtonClickListener(m_Panel.JieSuanButton, JiSuan1);
				break;
			}
		}

		private void XuanShangInfo_1(IntoWaiWuResp_1015038.Types.XuanShangInfoResp_1015040 xuanShangInfo)
		{
			m_Id2 = xuanShangInfo.Id;
			m_Panel.WuPingName_2Text.text = xuanShangInfo.GoodsName;
			m_Panel.LingShiJiangLi_2Text.text = $"{xuanShangInfo.Stone}";
			m_Panel.ChengGonglv_2Text.text = $"{xuanShangInfo.SuccessRate}%";
			JieSuanPanDuan_1(xuanShangInfo.Status);
		}

		private void JieSuanPanDuan_1(int status)
		{
			m_Panel.YuJiTime_2Image.gameObject.SetActive(value: false);
			m_Panel.YiWanCheng_2Text.gameObject.SetActive(value: false);
			m_Panel.JieSuan_2Button.gameObject.SetActive(value: false);
			m_Panel.DenDai_2Button.gameObject.SetActive(value: false);
			switch (status)
			{
			case 0:
				m_Panel.YuJiTime_2Image.gameObject.SetActive(value: true);
				m_Panel.DenDai_2Button.gameObject.SetActive(value: true);
				break;
			case 1:
				m_Panel.YiWanCheng_2Text.gameObject.SetActive(value: true);
				m_Panel.JieSuan_2Button.gameObject.SetActive(value: true);
				AddButtonClickListener(m_Panel.JieSuan_2Button, JiSuan2);
				break;
			}
		}

		private void JiSuan1()
		{
			JieSuan(m_Id1);
		}

		private void JiSuan2()
		{
			JieSuan(m_Id2);
		}

		private void JieSuan(int shu)
		{
			WaiWuJieSuanXuanShangReq_1015041 data = new WaiWuJieSuanXuanShangReq_1015041
			{
				Id = shu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.WAIWUJIESUANXUANSHANG, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					WaiWuJieSuanXuanShangResp_1015041 waiWuJieSuanXuanShangResp_ = new WaiWuJieSuanXuanShangResp_1015041();
					waiWuJieSuanXuanShangResp_.MergeFrom(byteString);
					if (waiWuJieSuanXuanShangResp_.IsSuccess)
					{
						HuoQuZhangLaoXingXi();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(waiWuJieSuanXuanShangResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CurrentContribution(long currentContribution)
		{
			m_Panel.DqGongXinaText.text = $"当前贡献：{currentContribution}";
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void FaBuBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_WaiWuDianXiangQingWnd.prefab", true, m_TiShengLevel);
		}

		private void WenHaoTogClick()
		{
			string text = $"1、同时发布两条悬赏，用以寻找稀有物品 2、灵石基于越多，则成功几率越大 3、发布时，点击搜索，必须搜索到本世界的物品，依照名字搜索，无法模糊搜索，必须输入全称 4、同名物品选中某一个后，可以开始发布悬赏 5、悬赏结束后，可以领取，悬赏前会支付报酬";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "内务殿");
		}
	}
}
