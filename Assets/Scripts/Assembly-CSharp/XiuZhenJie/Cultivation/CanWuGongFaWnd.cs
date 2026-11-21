using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CanWuGongFaWnd : Window
	{
		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private CanWuGongFaPanel m_Panel;

		private Common_TipWndParamEntity m_DataSource;

		private GameObject m_InsObj;

		private XzjBoxEntity m_NaJieBox;

		private CanWuEntity m_CanWuEntity;

		private int m_DqCanJuan;

		private int m_DqMaxCanJuan;

		private string m_GongFaChoShu;

		private bool m_PartBool;

		private string m_PartId = "1";

		private int m_PartCount;

		private bool m_DaoJingBool;

		private string m_DaoJingId = "1";

		private int m_DaoJingCount;

		private string m_LingYanZhi;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<CanWuGongFaPanel>();
			InkDialogArt.StyleDetails(m_Panel.transform, m_Panel.TitleText, m_Panel.CloseBtn, m_Panel.ScrollRect);
			InkDialogArt.StyleButton(m_Panel.QueRenBtn, "SealButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(m_Panel.BuQuanBtn, "IvoryPlaque", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(m_Panel.TiSheng, "IvoryPlaque", InkInterfaceArt.Ink);
			InkDialogArt.SetWash(m_Panel.BgBuQiImage.transform);
			InkDialogArt.SetPaper(m_Panel.GuanBi.transform.parent.GetComponent<Image>());
			InkDialogArt.StyleButton(m_Panel.GuanBi, "Close", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(m_Panel.BuQiBtn, "SealButton", InkDialogArt.Ivory);
			m_Panel.XianShiOneTexy.color = InkInterfaceArt.Ink;
			m_Panel.XianShiTwoText.color = InkInterfaceArt.SoftInk;
			AddButtonClickListener(m_Panel.CloseBtn, CloseWindow);
			AddButtonClickListener(m_Panel.QueRenBtn, SureBtnClick);
			AddButtonClickListener(m_Panel.BuQuanBtn, BuQuan);
			AddButtonClickListener(m_Panel.GuanBi, GuanBi);
			AddButtonClickListener(m_Panel.BuQiBtn, Complemented);
			AddButtonClickListener(m_Panel.TiSheng, TiShengPingJibut);
			Singleton<RoleManager>.Instance.RoleBoxDic.TryGetValue(10, out m_NaJieBox);
			m_InsObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as Common_TipWndParamEntity;
				m_CanWuEntity = paralist[1] as CanWuEntity;
				if (m_DataSource == null)
				{
					return;
				}
				CreateContentList();
				InkCultivationPages.BookHeading(m_Panel.TitleText, m_CanWuEntity);
				m_Panel.TitleText.text = (string.IsNullOrEmpty(m_DataSource.TitleName) ? "标题" : m_DataSource.TitleName);
				m_Panel.ButtonText.text = (string.IsNullOrEmpty(m_DataSource.BtnName) ? "确定" : m_DataSource.BtnName);
			}
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(ResourceLoad);
		}

		private void TiShengPingJibut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("TiShengPingJieWnd.prefab", true, m_DataSource.ID, GoodsTypeEnum.DE_JING, m_CanWuEntity.Level, m_LingYanZhi, m_DataSource.Rank);
		}

		private void CreateContentList()
		{
			if (m_DataSource.ContentList != null)
			{
				for (int i = 0; i < m_DataSource.ContentList.Count; i++)
				{
					string[] row = m_DataSource.ContentList[i].Split(new[] { ',' }, 3);
					if (row.Length < 2)
						continue;
					if (row[0] == "灵衍值")
						m_LingYanZhi = row[1];
					if (row[0] == "功法重数")
						m_GongFaChoShu = row[1];
					NaJieTipItem item = Object.Instantiate(m_InsObj, m_Panel.ScrollRectContent).GetComponent<NaJieTipItem>();
					item.SetDetailItem(row[0], row[1], row.Length == 3 ? row[2] : string.Empty, i);
				}
			}
			InkDialogArt.FitDetailRows(m_Panel.ScrollRect);
		}

		public override void CloseWindow()
		{
			m_DataSource.CloseBtnClick?.Invoke(m_DataSource.DataSource);
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void SureBtnClick()
		{
			m_DataSource.SureBtnClick?.Invoke(m_DataSource.DataSource);
			CloseWindow();
		}

		private void BuQuan()
		{
			PanDuanJingDu();
			New();
			m_Panel.BgBuQiImage.gameObject.SetActive(value: true);
		}

		private void GuanBi()
		{
			m_Panel.BgBuQiImage.gameObject.SetActive(value: false);
		}

		private void Assignment()
		{
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (goods.TemplateId == m_CanWuEntity.Id)
				{
					m_Panel.XianShiOne.gameObject.SetActive(value: true);
					UI_GoodsTogItem component = m_Panel.XianShiOne.GetComponent<UI_GoodsTogItem>();
					m_GoodsIconSprite.TryGetValue(goods.Type, out var value);
					int levelByRank = WorldSetting.GetLevelByRank(goods.Rank);
					m_RankBgSprite.TryGetValue(levelByRank, out var value2);
					component.SetValue(goods, goods.Name, goods.FiveElement, goods.Level, value2, value, null, Part, m_PartBool, goods.GoodsNum);
					m_Panel.BuQiBtn_Wu.gameObject.SetActive(value: false);
				}
				else if (goods.GetGoodsStage() == m_CanWuEntity.GetStage() && goods.Type == 33)
				{
					m_Panel.XianShiTwo.gameObject.SetActive(value: true);
					UI_GoodsTogItem component2 = m_Panel.XianShiTwo.GetComponent<UI_GoodsTogItem>();
					m_GoodsIconSprite.TryGetValue(goods.Type, out var value3);
					int levelByRank2 = WorldSetting.GetLevelByRank(goods.Rank);
					m_RankBgSprite.TryGetValue(levelByRank2, out var value4);
					component2.SetValue(goods, goods.Name, goods.FiveElement, goods.Level, value4, value3, null, DaoJing, m_DaoJingBool, goods.GoodsNum);
					m_Panel.GuanBi_Wu.gameObject.SetActive(value: false);
				}
			}
		}

		private void Part(bool isOn, object data, GameObject gameObj)
		{
			if (data is BaseNaJieGoodEntity baseNaJieGoodEntity)
			{
				m_PartId = baseNaJieGoodEntity.TemplateId;
				if (!m_PartBool)
				{
					m_PartCount += baseNaJieGoodEntity.GoodsNum;
					m_PartBool = true;
				}
				else
				{
					m_PartCount = 0;
					m_PartBool = false;
				}
				PanDuan();
			}
		}

		private void DaoJing(bool isOn, object data, GameObject gameObj)
		{
			if (data is BaseNaJieGoodEntity baseNaJieGoodEntity)
			{
				if (!m_DaoJingBool)
				{
					m_DaoJingCount += baseNaJieGoodEntity.GoodsNum;
					m_DaoJingBool = true;
				}
				else
				{
					m_DaoJingCount = 0;
					m_DaoJingBool = false;
				}
				PanDuan();
			}
		}

		private void PanDuan()
		{
			New();
			if (m_PartCount + m_DaoJingCount >= m_DqMaxCanJuan && m_GongFaChoShu != "第三重")
			{
				m_Panel.BuQiBtn.interactable = true;
			}
			else
			{
				m_Panel.BuQiBtn.interactable = false;
			}
		}

		private void PanDuanJingDu()
		{
			switch (WorldSetting.GetLevelByRank(m_CanWuEntity.Rank))
			{
			case 1:
				PdHuangJie();
				break;
			case 2:
				PdXuanJie();
				break;
			case 3:
				PdDiJie();
				break;
			case 4:
				PdTianJie();
				break;
			}
			Show();
		}

		private void PdHuangJie()
		{
			string gongFaChoShu = m_GongFaChoShu;
			if (!(gongFaChoShu == "第一重"))
			{
				if (gongFaChoShu == "第二重")
				{
					m_DqMaxCanJuan = 4;
				}
			}
			else
			{
				m_DqMaxCanJuan = 2;
			}
		}

		private void PdXuanJie()
		{
			string gongFaChoShu = m_GongFaChoShu;
			if (!(gongFaChoShu == "第一重"))
			{
				if (gongFaChoShu == "第二重")
				{
					m_DqMaxCanJuan = 9;
				}
			}
			else
			{
				m_DqMaxCanJuan = 3;
			}
		}

		private void PdDiJie()
		{
			string gongFaChoShu = m_GongFaChoShu;
			if (!(gongFaChoShu == "第一重"))
			{
				if (gongFaChoShu == "第二重")
				{
					m_DqMaxCanJuan = 36;
				}
			}
			else
			{
				m_DqMaxCanJuan = 6;
			}
		}

		private void PdTianJie()
		{
			string gongFaChoShu = m_GongFaChoShu;
			if (!(gongFaChoShu == "第一重"))
			{
				if (gongFaChoShu == "第二重")
				{
					m_DqMaxCanJuan = 81;
				}
			}
			else
			{
				m_DqMaxCanJuan = 9;
			}
		}

		private void Show()
		{
			string arg = "";
			string arg2 = "";
			switch (m_GongFaChoShu)
			{
			case "第一重":
				arg = "上";
				arg2 = "中";
				break;
			case "第二重":
				arg = "中";
				arg2 = "下";
				break;
			case "第三重":
				m_Panel.XianShiOneTexy.text = "当前功法已经圆满！";
				return;
			}
			m_Panel.XianShiOneTexy.text = $"当前仅有{arg}篇：还需要{m_DqMaxCanJuan}张残卷可以补全{arg2}篇。";
		}

		private void New()
		{
			if (m_GongFaChoShu == "第三重")
			{
				m_Panel.XianShiTwoText.text = "0/0";
			}
			else
			{
				m_Panel.XianShiTwoText.text = $"{m_PartCount + m_DaoJingCount}/{m_DqMaxCanJuan}";
			}
		}

		private void Complemented()
		{
			UsedAchievementPartReq_1001006 usedAchievementPartReq_ = new UsedAchievementPartReq_1001006
			{
				Id = m_CanWuEntity.Id
			};
			if (m_PartCount >= m_DqMaxCanJuan)
			{
				usedAchievementPartReq_.PartCount = m_DqMaxCanJuan;
				usedAchievementPartReq_.DaoJingCount = 0;
			}
			else if (m_DaoJingCount + m_PartCount >= m_DqMaxCanJuan)
			{
				usedAchievementPartReq_.PartCount = m_PartCount;
				usedAchievementPartReq_.DaoJingCount = m_DqMaxCanJuan - m_PartCount;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.USED_ACHIEVEMENT_PART, usedAchievementPartReq_, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					UsedAchievementPartResp_1001006 usedAchievementPartResp_ = new UsedAchievementPartResp_1001006();
					usedAchievementPartResp_.MergeFrom(byteString);
					if (usedAchievementPartResp_.IsSuccess)
					{
						if (m_PartId != "1")
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].ResetGoodsNum(m_PartId, usedAchievementPartResp_.PartCount);
						}
						if (m_DaoJingId != "1")
						{
							Singleton<RoleManager>.Instance.RoleBoxDic[10].ResetGoodsNum(m_DaoJingId, usedAchievementPartResp_.DaoJingCount);
						}
					}
					CloseWindow();
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(usedAchievementPartResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			});
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_GongFaShu.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_CanJuan.png");
			m_GoodsIconSprite.Add(33, value2);
			m_GoodsIconSprite.Add(1, value);
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value3);
			m_RankBgSprite.Add(2, value4);
			m_RankBgSprite.Add(3, value5);
			m_RankBgSprite.Add(4, value6);
			Assignment();
		}
	}
}
