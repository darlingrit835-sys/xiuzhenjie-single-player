using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CanWuWnd : Window
	{
		public CanWuPanel Panel;

		internal Button PracticeBook { get; private set; }
		internal bool PracticeListLoaded { get; private set; }
		internal bool PracticeBookUnderstood { get; private set; }

		private GameObject CanWuObj;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private GameObject sectionPrefab;
		private readonly List<CanWuEntity> books = new List<CanWuEntity>();
		private SystemMethodShelf shelf;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			ResourceLoad();
			Panel = base.GameObject.GetComponent<CanWuPanel>();
			InkInterfaceArt.Set(Panel.GetComponent<Image>(), "InkLandscapePaper", false);
			Panel.SmallTitleText.text = string.Empty;
			Panel.SmallTitleText.color = InkInterfaceArt.SoftInk;
			InkInterfaceArt.Set(Panel.SmallTitleText.transform.parent.GetComponent<Image>(), "LightPlaque", false);
			InkDialogArt.SetWash(Panel.TiShiPanel);
			InkDialogArt.SetPaper(Panel.TiShiPanel.Find("TiShiPanel")?.GetComponent<Image>(), "InkLedgerPaper");
			Transform noticeTitle = Panel.TiShiPanel.Find("TiShiPanel/Title");
			InkInterfaceArt.Set(noticeTitle?.GetComponent<Image>(), "LightPlaque", false);
			Text noticeTitleText = noticeTitle?.GetComponentInChildren<Text>(true);
			if (noticeTitleText != null)
				noticeTitleText.color = InkInterfaceArt.Ink;
			Panel.TiShiContentText.color = InkInterfaceArt.Ink;
			InkDialogArt.StyleButton(Panel.TiShiLiKaiBtn, "LightPlaque", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(Panel.TiShiGoSettingBtn, "LightAction", InkDialogArt.Ivory);
			InkCultivationPages.StudyShelf(Panel);
			shelf = new SystemMethodShelf(Panel, RefreshShelf);
			sectionPrefab = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/CanWuScrollItem.prefab");
			AddButtonClickListener(Panel.ShaiXuanBtn, () => MonoSingleton<GameStart>.Instance.PopUpAttention(
				"普通功法先在纳戒读书，天道传承可直接参悟。\n\n功法五行须与任一灵根相合，达到所需境界即可参悟。《启灵经》人人可修。\n\n学成后可在练功房更换功法。", title: "参悟规则"));
			AddButtonClickListener(Panel.TiShiLiKaiBtn, () => Panel.TiShiPanel.gameObject.SetActive(false));
			Panel.TiShiLiKaiBtnText.text = "回书目";
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			shelf.Reset();
			Panel.TiShiPanel.gameObject.SetActive(false);
			InkCultivationPages.RefreshStudyRole(Panel);
			EnterCanWu();
		}

		private void ResourceLoad()
		{
			m_GoodsIconSprite = new Dictionary<int, Sprite>();
			m_RankBgSprite = new Dictionary<int, Sprite>();
			CanWuObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_CanJuan.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FuShu.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_DanShu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_FaSuShu.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_QiTu.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Goods_ZhenTu.png");
			m_GoodsIconSprite.Add(2, value);
			m_GoodsIconSprite.Add(3, value3);
			m_GoodsIconSprite.Add(6, value2);
			m_GoodsIconSprite.Add(1, value4);
			m_GoodsIconSprite.Add(4, value5);
			m_GoodsIconSprite.Add(5, value6);
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value7);
			m_RankBgSprite.Add(2, value8);
			m_RankBgSprite.Add(3, value9);
			m_RankBgSprite.Add(4, value10);
		}

		public void EnterCanWu()
		{
			PracticeBook = null;
			PracticeListLoaded = false;
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_CANWU_LIST, null, response =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				if (!(response is ByteString bytes) || GameObject == null) return;
				var list = OpenUnderstandPanelResp_1001011.Parser.ParseFrom(bytes);
				if (!list.IsSuccess)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(list.Code));
					return;
				}
				books.Clear();
				foreach (var entry in list.Object)
				{
					if (!m_GoodsIconSprite.ContainsKey(entry.Type)) continue;
					books.Add(new CanWuEntity
					{
						Type = entry.Type, Name = entry.Name, FiveElement = entry.FiveElement, Id = entry.Id,
						IsCanWu = entry.IsUnderstand, Level = entry.Level, Rank = entry.Rank
					});
				}
				PracticeBookUnderstood = books.Any(book => book.Id == PracticeGuideFlow.StarterId && book.IsCanWu);
				if (Singleton<GuideManager>.Instance.Practice?.Stage == 2 && !PracticeBookUnderstood) shelf.Reset();
				RefreshShelf();
				PracticeListLoaded = true;
			}, isShowWaitUI: true);
		}

		private void RefreshShelf()
		{
			PracticeBook = null;
			foreach (Transform child in Panel.ScrollContent)
			{
				child.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			var page = shelf.Books(books);
			if (page.Count > 0)
			{
				var section = SetListContentParent(Panel.ScrollContent, sectionPrefab).GetComponent<CanWuScrollItem>();
				section.SetValue(string.Empty, this);
				section.HelpBtn.gameObject.SetActive(false);
				foreach (var book in page)
				{
					var card = SetListContentParent(section.ContentRect, CanWuObj).GetComponent<UI_GoodsItem>();
					m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(book.Rank), out var rank);
					card.SetItem(book, book.Name, book.FiveElement, book.Level, rank, m_GoodsIconSprite[book.Type],
						(_, __) => OpenBook(book), book.IsCanWu);
					InkCultivationPages.Book(card, book);
					section.AddItem(card);
					if (book.Id == PracticeGuideFlow.StarterId && !book.IsCanWu) PracticeBook = card.ClickBtn;
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(Panel.ScrollContent);
			Panel.ScrollView.StopMovement();
			Panel.ScrollView.verticalNormalizedPosition = 1f;
		}

		private void OpenBook(CanWuEntity book)
		{
			if (SystemMethodShelf.IsSystem(book.Id))
			{
				shelf.Details(book, () => CanWuAction(book), GoToSettingGongFa);
				return;
			}
			var roles = Singleton<RoleManager>.Instance;
			UnityAction<List<string>> show = details => ShowBookDetails(book, details);
			switch ((CanWuTitleType)book.Type)
			{
				case CanWuTitleType.GongFa: roles.SelectTemplateAchievementDetail(book.Id, book.Level, book.Rank, book.FiveElement, show); break;
				case CanWuTitleType.FaShu: roles.SelectTemplateSkillDetail(book.Id, book.Level, book.Rank, book.FiveElement, show); break;
				case CanWuTitleType.DanFang: roles.SelectTemplateElixirDetail(book.Id, book.Level, book.Rank, book.FiveElement, show); break;
				case CanWuTitleType.FuShu: roles.SelectTemplateSignDetail(book.Id, book.Level, book.Rank, book.FiveElement, show); break;
				case CanWuTitleType.QiTu: roles.SelectTemplateQiTuDetail(book.Id, book.Level, book.Rank, book.FiveElement, show); break;
				case CanWuTitleType.ZhenFa: roles.SelectTemplateMatrixDetail(book.Id, book.Level, book.Rank, book.FiveElement, show); break;
			}
		}

		private void ShowBookDetails(CanWuEntity book, List<string> details)
		{
			if (GameObject == null || !GameObject.activeInHierarchy || details == null || details.Count == 0) return;
			var state = SystemMethodShelf.State(book);
			string gate = WorldSetting.GetRoleLevelNameByRoleLevel(book.Level);
			if (book.Type == (int)CanWuTitleType.GongFa)
			{
				details.RemoveAll(row => row.StartsWith("等阶,", System.StringComparison.Ordinal));
				details.Insert(0, "所需境界," + gate);
				if (book.Id == PracticeGuideFlow.StarterId)
					details.Insert(0, "入门心法,各灵根均可修习");
				else if (book.IsCanWu)
					details.Insert(0, "修习记录," + SystemMethodShelf.EquipHint(book));
			}
			var data = new Common_TipWndParamEntity
			{
				TitleName = book.Name, ContentList = details, DataSource = book, ID = book.Id, Rank = book.Rank
			};
			bool learnedMethod = book.IsCanWu && book.Type == (int)CanWuTitleType.GongFa;
			if (learnedMethod)
			{
				data.BtnName = "去练功房";
				data.SureBtnClick = _ => GoToSettingGongFa();
				Singleton<UIManager>.Instance.PopUpWnd("CanWuGongFaWnd.prefab", true, data, book);
				return;
			}
			if (state == SystemMethodShelf.Shelf.Ready)
			{
				data.BtnName = "参悟";
				data.SureBtnClick = _ => CanWuAction(book);
			}
			else data.BtnName = book.IsCanWu ? "收起" : gate + "可悟";
			var detail = Singleton<UIManager>.Instance.PopUpWnd("Common_TipWnd.prefab", true, data) as Common_TipWnd;
			if (detail?.Panel == null) return;
			InkCultivationPages.BookHeading(detail.Panel.TitleText, book);
			detail.Panel.QueRenBtn.interactable = book.IsCanWu || state == SystemMethodShelf.Shelf.Ready;
		}

		private void CanWuAction(CanWuEntity book)
		{
			if (SystemMethodShelf.State(book) != SystemMethodShelf.Shelf.Ready) return;
			string id = book.Id;
			int type = book.Type;
			var practice = Singleton<GuideManager>.Instance.Practice;
			UnderstandMethodReq_1001005 data = new UnderstandMethodReq_1001005
			{
				Id = id,
				Type = type
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.ROLE_UNDERSTAND, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					ActionBoolResp_1001001 actionBoolResp_ = new ActionBoolResp_1001001();
					actionBoolResp_.MergeFrom(byteString);
					if (actionBoolResp_.IsSuccess)
					{
						EnterCanWu();
						TiShi(type, book.Name);
						if (type == (int)CanWuTitleType.GongFa && id == PracticeGuideFlow.StarterId)
							practice?.RecordMilestone(2);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(actionBoolResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void TiShi(int type, string bookName)
		{
			Panel.TiShiPanel.gameObject.SetActive(value: true);
			string text = "去设置";
			UnityAction call = null;
			switch ((CanWuTitleType)type)
			{
			case CanWuTitleType.ZhenFa:
				text = "去刻制";
				call = ToKeZhi;
				break;
			case CanWuTitleType.FaShu:
				text = "去设置";
				call = ToSettingFaShu;
				break;
			case CanWuTitleType.DanFang:
				text = "去炼丹";
				call = ToLianDan;
				break;
			case CanWuTitleType.QiTu:
				text = "去炼器";
				call = ToLianQi;
				break;
			case CanWuTitleType.FuShu:
				text = "去画符";
				call = ToHuaFu;
				break;
			case CanWuTitleType.GongFa:
				text = "去练功房";
				call = GoToSettingGongFa;
				break;
			}
			Panel.TiShiGoSettingBtnText.text = text;
			Panel.TiShiContentText.text = "已参悟《" + bookName + "》。";
			Panel.TiShiGoSettingBtn.onClick.RemoveAllListeners();
			Panel.TiShiGoSettingBtn.onClick.AddListener(call);
		}

		private void ToHuaFu()
		{
			if (!Singleton<RoleManager>.Instance.IsFuShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟符室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd?.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(3)?.Invoke();
		}

		private void ToLianQi()
		{
			if (!Singleton<RoleManager>.Instance.IsLianQiShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟炼器室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd?.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(2)?.Invoke();
		}

		private void ToSettingFaShu()
		{
			Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab").ZhanDouBtnClick();
			Singleton<UIManager>.Instance.FindWndByName<RoleZhuangBeiWnd>("RoleZhuangBeiWnd.prefab").Panel.JiNengTog.isOn = true;
		}

		private void GoToSettingGongFa()
		{
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				if (Singleton<GameMapManager>.Instance.CurrentMapName.Equals("DongFuScene"))
				{
					mainTitleWnd.DongFuBtnClickNoChangeScene();
				}
				else
				{
					mainTitleWnd.DongFuBtnClick();
				}
				Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab")?.LianGongFangBtn();
			}
			else
			{
				dongFuRoomWnd?.LianGongFangBtn();
			}
		}

		private void ToLianDan()
		{
			if (!Singleton<RoleManager>.Instance.IsDanShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟炼丹室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(1)?.Invoke();
		}

		private void ToKeZhi()
		{
			if (!Singleton<RoleManager>.Instance.IsZhenFaShi())
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请先开辟阵法室");
				return;
			}
			DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
			MainTitleWnd mainTitleWnd = Singleton<UIManager>.Instance.FindWndByName<MainTitleWnd>("MainTitleWnd.prefab");
			if (dongFuRoomWnd == null)
			{
				mainTitleWnd.DongFuBtnClick();
			}
			dongFuRoomWnd?.GetRoomClick(5)?.Invoke();
		}

		public void CanWuBtnClick()
		{
			Singleton<UIManager>.Instance.HideWnd("CanWuWnd.prefab");
			Singleton<UIManager>.Instance.HideWnd("DongFuWnd.prefab");
			Singleton<UIManager>.Instance.PopUpWnd("CanWuZhongWnd.prefab", true);
		}
	}
}
