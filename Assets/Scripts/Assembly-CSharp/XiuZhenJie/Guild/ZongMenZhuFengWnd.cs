using System;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenZhuFengWnd : Window
	{
		public ZongMenZhuFengPanel Panel;

		private bool m_ZongZhu;

		private IntoDaDianResp_1015020 m_IntoDaDianResp1015020;

		private string m_LingYuKuCun;

		private int m_ZhuFenShenJiTime;

		private float m_Time;

		private XzjBoxEntity m_NaJieBox;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject m_InsObj;

		private BaseNaJieGoodEntity m_DataNew;

		private string m_Mid;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMenZhuFengPanel>();
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(ResourceLoad);
			m_IntoDaDianResp1015020 = paralist[0] as IntoDaDianResp_1015020;
			Panel.NameText.text = (string)paralist[1];
			m_ZhuFenShenJiTime = (int)((long)paralist[2] / 1000);
			if (m_ZhuFenShenJiTime > 0)
			{
				m_Time++;
			}
			m_ZongZhu = (bool)paralist[3];
			Panel.QuRenGengHuangButton.gameObject.SetActive(value: true);
			Panel.QuRenGengHuangButton.interactable = m_ZongZhu;
			Panel.ShenJiJiaShuButton.interactable = m_ZongZhu;
			AddButtonClickListener(Panel.WaiWuDianBtn, WaiWuDian);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsTogItem.prefab");
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.QuRenGengHuangButton, QueRenShenJi);
			AddButtonClickListener(Panel.ShenJiJiaShuButton, ShengJiCangKuTime);
			AddButtonClickListener(Panel.ZongMenDaDianBtn, ZongMenDaDian);
			AddButtonClickListener(Panel.ZhangLaoDianBtn, ZhangLaoDian);
			AddButtonClickListener(Panel.ZhangLaoDianBtn, ZhangLaoDian);
			AddButtonClickListener(Panel.ZhiFaDianBtn, ZhiFaDian);
			AddButtonClickListener(Panel.YanWuChangBtn, YanWuChang);
			AddButtonClickListener(Panel.NeiWuDianBtn, NeiWuDian);
			AddButtonClickListener(Panel.WaiWuDianBtn, WaiWuDian);
			AddButtonClickListener(Panel.CangBaoGeBtn, CangBaoGe);
			AddButtonClickListener(Panel.ShiKomgMiJingButton, ShiKomgMiJingBut);
			AddButtonClickListener(Panel.NameButton, GaiMing);
			AddButtonClickListener(Panel.WenHao, WenHaoTogClick);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
				ShengJibut();
			}
		}

		private void ShengJibut()
		{
			if (m_IntoDaDianResp1015020.Stage == 9)
			{
				Panel.QuRenGengHuangButton.gameObject.SetActive(value: false);
				Panel.ShenJiJiaShuButton.gameObject.SetActive(value: false);
			}
			else if (m_ZhuFenShenJiTime > 0)
			{
				m_ZhuFenShenJiTime--;
				string text = SecondToHour(m_ZhuFenShenJiTime);
				Panel.ShenJiJiaShuButton.gameObject.SetActive(value: true);
				Panel.ShenJiJiaShuText.text = text;
			}
			else
			{
				Panel.ShenJiJiaShuButton.gameObject.SetActive(value: false);
			}
		}

		private static string SecondToHour(int time)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			num3 = Convert.ToInt32(time);
			if (num3 > 60)
			{
				num2 = num3 / 60;
				num3 %= 60;
			}
			if (num2 > 60)
			{
				num = num2 / 60;
				num2 %= 60;
			}
			string arg = ((num2 < 10) ? ("0" + num2) : num2.ToString());
			string arg2 = ((num2 < 10) ? ("0" + num3) : num3.ToString());
			return $"{num}:{arg}:{arg2}";
		}

		private void ShengJiCangKuTime()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_SpeedUpWnd.prefab", true, m_ZhuFenShenJiTime / 60 + 1, new UnityAction<int>(SpeedUpdate));
		}

		private void SpeedUpdate(int castLingYu)
		{
			GuildUpgradeJadeBuyTimeReq_1015147 data = new GuildUpgradeJadeBuyTimeReq_1015147
			{
				Jade = castLingYu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildUpgradeJadeBuyTime, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeKuangMaiSpaceJadeBuyTimeResp_1015053 upgradeKuangMaiSpaceJadeBuyTimeResp_ = new UpgradeKuangMaiSpaceJadeBuyTimeResp_1015053();
					upgradeKuangMaiSpaceJadeBuyTimeResp_.MergeFrom(byteString);
					if (upgradeKuangMaiSpaceJadeBuyTimeResp_.IsSuccess)
					{
						m_ZhuFenShenJiTime = (int)(upgradeKuangMaiSpaceJadeBuyTimeResp_.ShengYuTime / 1000);
						if (m_ZhuFenShenJiTime <= 0)
						{
							m_ZhuFenShenJiTime = 0;
							ShengJibut();
						}
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeKuangMaiSpaceJadeBuyTimeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void QueRenShenJi()
		{
			if (HasSectCourt) { CourtPage(SectCourt.Page.Region); return; }
			AddButtonClickListener(Panel.JingJieClose, ShenJiClossBut);
			AddButtonClickListener(Panel.XuanZhoGuanBiButton, XuanZhoGuanBi);
			AddButtonClickListener(Panel.QueRenJingJie, QueRenJingJieBut);
			AddButtonClickListener(Panel.ShenJiXuanZheButton, WeiXuanZho);
			Panel.JingJieLevel.text = WorldSetting.GetStageName(m_IntoDaDianResp1015020.Stage) + "宗门";
			ShenJiXiaoHao();
			Panel.JingJieBeiJingImage.gameObject.SetActive(value: true);
			Panel.QueRenJingJie.interactable = false;
		}

		private void ShenJiClossBut()
		{
			Panel.GuanBiButXuanZhoBGImage.gameObject.SetActive(value: false);
			Panel.JingJieBeiJingImage.gameObject.SetActive(value: false);
		}

		private void XuanZhoGuanBi()
		{
			Panel.QueRenJingJie.interactable = false;
			Panel.GuanBiButXuanZhoBGImage.gameObject.SetActive(value: false);
			Panel.XuanZhoBGImage.gameObject.SetActive(value: false);
		}

		private void QueRenJingJieBut()
		{
			GuildUpgradeReq_1015029 data = new GuildUpgradeReq_1015029
			{
				GoodsId = m_Mid
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildUpgrade, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuildUpgradeResp_1015029 guildUpgradeResp_ = new GuildUpgradeResp_1015029();
					guildUpgradeResp_.MergeFrom(byteString);
					if (guildUpgradeResp_.IsSuccess)
					{
						m_Time = 1f;
						m_ZhuFenShenJiTime = (int)guildUpgradeResp_.ShengYuTime / 1000;
						ShenJiClossBut();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildUpgradeResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void WeiXuanZho()
		{
			Panel.XuanZhoBGImage.gameObject.SetActive(value: true);
			ShenJiXuanZhe();
		}

		private void ShenJiXuanZhe()
		{
			foreach (Transform item in Panel.XuanZHeRectTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			int num = 0;
			GridLayoutGroup component = Panel.XuanZHeRectTransform.GetComponent<GridLayoutGroup>();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.Type == 32)
				{
					num++;
					CreatBoxItem(goods);
				}
			}
			SetGrticalListContentHight(num, Panel.XuanZHeRectTransform, component);
		}

		private void CreatBoxItem(BaseNaJieGoodEntity data)
		{
			GameObject gameObject = SetListContentParent(Panel.XuanZHeRectTransform, m_InsObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsTogItem component = gameObject.GetComponent<UI_GoodsTogItem>();
			m_GoodsIconSprite.TryGetValue(data.Type, out var value);
			int levelByRank = WorldSetting.GetLevelByRank(data.Rank);
			m_RankBgSprite.TryGetValue(levelByRank, out var value2);
			component.SetValue(data, data.Name, data.FiveElement, data.Level, value2, value, null, ChoosedItem);
		}

		private void ChoosedItem(bool isOn, object data, GameObject gameObj)
		{
			m_DataNew = data as BaseNaJieGoodEntity;
			if (m_DataNew != null)
			{
				m_GoodsIconSprite.TryGetValue(m_DataNew.Type, out var value);
				int levelByRank = WorldSetting.GetLevelByRank(m_DataNew.Rank);
				m_RankBgSprite.TryGetValue(levelByRank, out var value2);
				Panel.GoodsItem.SetValue(m_DataNew, m_DataNew.Name, m_DataNew.FiveElement, m_DataNew.Level, value2, value, null, WeiXuanZho);
				Panel.XuanZhoBGImage.gameObject.SetActive(value: false);
				Panel.GuanBiButXuanZhoBGImage.gameObject.SetActive(value: true);
				Panel.QueRenJingJie.interactable = true;
				m_Mid = m_DataNew.GoodsId;
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("数据异常！");
			}
		}

		private void WeiXuanZho(bool isOn, object data, GameObject gameObj)
		{
			Panel.XuanZhoBGImage.gameObject.SetActive(value: true);
			ShenJiXuanZhe();
		}

		private void ShenJiXiaoHao()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			switch (m_IntoDaDianResp1015020.Stage)
			{
			case 3:
				text = "5000000";
				text2 = "50000";
				text3 = "50000";
				text4 = "24小时";
				break;
			case 4:
				text = "50000000";
				text2 = "500000";
				text3 = "500000";
				text4 = "48小时";
				break;
			case 5:
				text = "300000000";
				text2 = "5000000";
				text3 = "5000000";
				text4 = "96小时";
				break;
			case 6:
				text = "1000000000";
				text2 = "50000000";
				text3 = "50000000";
				text4 = "192小时";
				break;
			case 7:
				text = "5000000000";
				text2 = "500000000";
				text3 = "500000000";
				text4 = "384小时";
				break;
			case 8:
				text = "25000000000";
				text2 = "5000000000";
				text3 = "5000000000";
				text4 = "768小时";
				break;
			}
			Panel.ShenJiXiaoHaoLingShiText.text = text + "灵石";
			Panel.ShenJiXiaoHaoYuanCaiText.text = text2 + "灵矿";
			Panel.ShenJiXiaoHaoLingKuangMaiText.text = text3 + "灵植";
			Panel.JingJiTiemText.text = text4;
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_MiJing.png");
			m_GoodsIconSprite.Add(32, value);
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value2);
			m_RankBgSprite.Add(2, value3);
			m_RankBgSprite.Add(3, value4);
			m_RankBgSprite.Add(4, value5);
		}

		private void GaiMing()
		{
			AddButtonClickListener(Panel.Close, ClossBut);
			AddButtonClickListener(Panel.QueDingBut, QueDing);
			Panel.LingYuXiaoHaoText.text = $"500/{(int)m_IntoDaDianResp1015020.JadeKuCun}灵玉";
			Panel.LingShiXiaoHaoText.text = $"50000/{(int)m_IntoDaDianResp1015020.StoneKuCun}灵石";
			Panel.LingKuangXiaoHaoText.text = $"50000/{(int)m_IntoDaDianResp1015020.KuangShiKuCun}灵矿";
			Panel.LingZhiXiaoHaoText.text = $"50000/{(int)m_IntoDaDianResp1015020.YaoCaiKuCun}灵植";
			Panel.BeiJingImage.gameObject.SetActive(value: true);
		}

		private void QueDing()
		{
			if (!m_ZongZhu)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("宗主才能更改主峰名称！");
				return;
			}
			if (string.IsNullOrEmpty(Panel.NewNameInputField.text) && Panel.NewNameInputField.name.Length < 3)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("至少需要三个字");
				return;
			}
			GuildHillUpdateNameReq_1015148 data = new GuildHillUpdateNameReq_1015148
			{
				Name = Panel.NewNameInputField.text,
				Type = 10
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GuildHillUpdateName, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					GuildHillUpdateNameResp_1015148 guildHillUpdateNameResp_ = new GuildHillUpdateNameResp_1015148();
					guildHillUpdateNameResp_.MergeFrom(byteString);
					if (guildHillUpdateNameResp_.IsSuccess)
					{
						Panel.NameText.text = Panel.NewNameInputField.text;
						ClossBut();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(guildHillUpdateNameResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}

		private void ClossBut()
		{
			Panel.BeiJingImage.gameObject.SetActive(value: false);
		}

		private void ShiKomgMiJingBut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_ShiKongMiJingWnd.prefab", true);
		}

		private void ZongMenDaDian()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.INTODADIAN, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoDaDianResp_1015020 intoDaDianResp_ = new IntoDaDianResp_1015020();
					intoDaDianResp_.MergeFrom(byteString);
					if (intoDaDianResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_DaDianWnd.prefab", true, intoDaDianResp_, m_ZongZhu);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoDaDianResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void ZhangLaoDian()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ZongMen_ZhangLaoWnd.prefab", true, m_IntoDaDianResp1015020, m_ZongZhu);
		}

		private void ZhiFaDian()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.INTOZHIFA, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoZhiFaResp_1015026 intoZhiFaResp_ = new IntoZhiFaResp_1015026();
					intoZhiFaResp_.MergeFrom(byteString);
					if (intoZhiFaResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_ZhiFaDianWnd.prefab", true, m_IntoDaDianResp1015020, m_ZongZhu, intoZhiFaResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoZhiFaResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void YanWuChang()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.INTOYANWU, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoYanWuResp_1015030 intoYanWuResp_ = new IntoYanWuResp_1015030();
					intoYanWuResp_.MergeFrom(byteString);
					if (intoYanWuResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_YanWuTaiWnd.prefab", true, m_IntoDaDianResp1015020, m_ZongZhu, intoYanWuResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoYanWuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void NeiWuDian()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.INTONEIWU, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoNeiWuResp_1015034 intoNeiWuResp_ = new IntoNeiWuResp_1015034();
					intoNeiWuResp_.MergeFrom(byteString);
					if (intoNeiWuResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_NeiWuDianWnd.prefab", true, m_IntoDaDianResp1015020, m_ZongZhu, intoNeiWuResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoNeiWuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void WaiWuDian()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.INTOWAIWU, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					IntoWaiWuResp_1015038 intoWaiWuResp_ = new IntoWaiWuResp_1015038();
					intoWaiWuResp_.MergeFrom(byteString);
					if (intoWaiWuResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_WaiWuDianWnd.prefab", true, m_IntoDaDianResp1015020, m_ZongZhu, intoWaiWuResp_);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoWaiWuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private bool HasSectCourt => Singleton<UIManager>.Instance.FindWndByName<ZongMenWnd>("ZongMenWnd.prefab")?.HasSectCourt == true;

		private void CourtPage(SectCourt.Page page)
		{
			var existing = Panel.transform.Find("SectCourt")?.GetComponent<SectCourt>();
			if (existing != null) { existing.Show(page); return; }
			SectCourt.Open(Panel.transform, Panel.NameText.font, Panel.NameText.text, m_IntoDaDianResp1015020.Stage,
				page, null, legacyShop: EnterCangBaoGe);
		}

		private void CangBaoGe()
		{
			if (HasSectCourt) CourtPage(SectCourt.Page.Treasure);
			else EnterCangBaoGe();
		}

		private void WenHaoTogClick()
		{
			if (HasSectCourt)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("宗主可在驻域册消耗矿砂、兽牙晋封。四阶纳洲、七阶统陆。藏宝阁的战备用兽潮功勋购置，原有贡献点另在贡献点兑换处使用。");
				return;
			}
			string text = $"1、升级宗门需要下一阶的洞天福地以及大量灵宝灵材，收集资源后升级即可 2、升级宗门后，其余山峰方可升级 3、获取宗门贡献，可以在藏宝阁兑换物品或进入时空秘境加速修行";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "主峰");
		}

		internal static void EnterCangBaoGe()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.EnterCangBaoGe, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterCangBaoGeResp_1015145 enterCangBaoGeResp_ = new EnterCangBaoGeResp_1015145();
					enterCangBaoGeResp_.MergeFrom(byteString);
					if (enterCangBaoGeResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.PopUpWnd("ZongMen_CangBaoGeWnd.prefab", true, enterCangBaoGeResp_.GongXianDian);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterCangBaoGeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}
	}
}
