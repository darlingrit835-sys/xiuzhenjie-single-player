using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NaJieTipWnd : Window
	{
		public NaJieTipPanel NaJieTipPanel;

		public NaJieTipModel naJieTipModel;

		public BaseNaJieGoodEntity m_DataSource;

		private List<string> m_DataList;

		private int detailRequest;

		private Image artwork;

		public int OpenType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_DataList = new List<string>();
			NaJieTipPanel = base.GameObject.GetComponent<NaJieTipPanel>();
			InkDialogArt.StyleDetails(NaJieTipPanel.transform.Find("Bg"), NaJieTipPanel.ItemName,
				NaJieTipPanel.TipCloseButton, NaJieTipPanel.ScrollView);
			InkInventoryArt.StyleLedger(NaJieTipPanel.ScrollView.transform);
			InkDialogArt.StyleButton(NaJieTipPanel.MiddleBtn, "SealButton", InkDialogArt.Ivory);
			InkDialogArt.StyleButton(NaJieTipPanel.LeftBtn, "IvoryPlaque", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(NaJieTipPanel.RightBtn, "IvoryPlaque", InkInterfaceArt.Ink);
			InkDialogArt.StyleButton(NaJieTipPanel.TiSheng, "IvoryPlaque", InkInterfaceArt.Ink);
			AddButtonClickListener(NaJieTipPanel.TipCloseButton, CloseWindow);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 1)
			{
				naJieTipModel = paralist[0] as NaJieTipModel;
			}
			if (naJieTipModel == null)
			{
				Debug.Log("装备详情无法显示");
				return;
			}
			m_DataSource = naJieTipModel.dataSource as BaseNaJieGoodEntity;
			if (naJieTipModel.FromRing && m_DataSource != null)
			{
				var heading = NaJieTipPanel.ScrollView.transform.parent.Find("Title_image");
				if (artwork == null)
				{
					var node = new GameObject("RingArtwork", typeof(RectTransform), typeof(Image));
					node.layer = heading.gameObject.layer;
					node.transform.SetParent(heading, false);
					artwork = node.GetComponent<Image>();
					artwork.raycastTarget = false;
					artwork.preserveAspect = true;
					var rect = artwork.rectTransform;
					rect.anchorMin = rect.anchorMax = new Vector2(0f, 0.5f);
					rect.pivot = new Vector2(0, .5f);
					rect.anchoredPosition = new Vector2(6, 0);
					rect.sizeDelta = new Vector2(28, 28);
				}
				artwork.sprite = RingLedger.Artwork(m_DataSource);
				artwork.color = RingGoodsCatalog.ArtTint(artwork.sprite);
				artwork.gameObject.SetActive(true);
			}
			else if (artwork != null) artwork.gameObject.SetActive(false);
			if (m_DataSource != null && (m_DataSource.Type == 26 || m_DataSource.Type == 29 || m_DataSource.Type == 14 || m_DataSource.Type == 15 || m_DataSource.Type == 16))
			{
				NaJieTipPanel.TiSheng.gameObject.SetActive(value: true);
				AddButtonClickListener(NaJieTipPanel.TiSheng, TiShengPingJibut);
			}
			else
			{
				NaJieTipPanel.TiSheng.gameObject.SetActive(value: false);
			}
			GetGoodsInfo();
		}

		private void TiShengPingJibut()
		{
			Singleton<UIManager>.Instance.PopUpWnd("TiShengPingJieWnd.prefab", true, m_DataSource.GoodsId, GoodsTypeEnum.XIAN_JIN, m_DataSource.Level, $"{m_DataSource.ConversionRate:F}", m_DataSource.Rank);
		}

		private void GetGoodsInfo()
		{
			int generation = ++detailRequest;
			m_DataList.Clear();
			if (naJieTipModel.FromRing && RingGoodsCatalog.AddInstanceMaterialDetails(m_DataSource, m_DataList))
			{
				ReflashValue();
				return;
			}
			if (naJieTipModel.FromRing) ReflashValue();
			if (TalismanStudio.IsStudioGood(m_DataSource))
			{
				TalismanStudio.DescribeGood(m_DataSource.GoodsId, detail => {
					if (generation != detailRequest || NaJieTipPanel == null) return;
					m_DataList.Add("符效," + detail);
					m_DataList.Add("启用,在成符匣中启符。");
					ReflashValue();
				});
				return;
			}
			if (m_DataSource.ContentList == null || m_DataSource.ContentList.Count == 0)
			{
				Singleton<RoleManager>.Instance.RoleBoxDic[10].GetGoodsInfomation(m_DataSource, (List<string> strList) =>
				{
					if (generation != detailRequest || NaJieTipPanel == null) return;
					if (strList != null)
					{
						foreach (string str in strList)
						{
							m_DataList.Add(str);
						}
					}
					ReflashValue();
				});
			}
			else
			{
				if (21 == m_DataSource.Type)
				{
					Singleton<RoleManager>.Instance.RoleBoxDic[10].GetFuLuTimesXiangQing(m_DataSource.GoodsId, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> strList) =>
					{
						if (generation != detailRequest || NaJieTipPanel == null) return;
						foreach (string content in m_DataSource.ContentList)
						{
							if (content.Contains("剩余使用") && strList != null && strList.Count >= 1)
							{
								m_DataList.Add(strList[0]);
							}
							else
							{
								m_DataList.Add(content);
							}
						}
						ReflashValue();
					});
					return;
				}
				if (19 == m_DataSource.Type)
				{
					Singleton<RoleManager>.Instance.RoleBoxDic[10].GetDanYaoTimesXiangQing(m_DataSource.GoodsId, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> strList) =>
					{
						if (generation != detailRequest || NaJieTipPanel == null) return;
						foreach (string content2 in m_DataSource.ContentList)
						{
							if (content2.Contains("药效值") && !content2.Contains("（原）") && strList != null && strList.Count >= 2)
							{
								m_DataList.Add(strList[0]);
							}
							else if (content2.Contains("已服用次数") && strList != null && strList.Count >= 2)
							{
								m_DataList.Add(strList[1]);
							}
							else
							{
								m_DataList.Add(content2);
							}
						}
						ReflashValue();
					});
					return;
				}
				foreach (string content3 in m_DataSource.ContentList)
				{
					m_DataList.Add(content3);
				}
				ReflashValue();
			}
		}

		public void ReflashValue()
		{
			try
			{
				NaJieTipPanel.ItemName.text = naJieTipModel.Name;
				NaJieTipPanel.MiddleBtn.gameObject.SetActive(value: true);
				if (!string.IsNullOrEmpty(naJieTipModel.MiddleBtnText))
				{
					NaJieTipPanel.MiddleBtn.gameObject.SetActive(value: true);
					NaJieTipPanel.MiddleBtnText.text = naJieTipModel.MiddleBtnText;
				}
				else
				{
					NaJieTipPanel.MiddleBtn.gameObject.SetActive(value: false);
				}
				if (!string.IsNullOrEmpty(naJieTipModel.LeftBtnText))
				{
					NaJieTipPanel.LeftBtn.gameObject.SetActive(value: true);
					NaJieTipPanel.LeftBtnText.text = naJieTipModel.LeftBtnText;
				}
				else
				{
					NaJieTipPanel.LeftBtn.gameObject.SetActive(value: false);
				}
				NaJieTipPanel.LeftBtn.interactable = !naJieTipModel.IsLock && naJieTipModel.LeftBtnClickEvent != null;
				NaJieTipPanel.MiddleBtn.interactable = naJieTipModel.MiddleBtnClickEvent != null;
				NaJieTipPanel.RightBtn.interactable = naJieTipModel.RightBtnClickEvent != null;
				NaJieTipPanel.RightBtnText.text = naJieTipModel.IsLock ? "解锁" : "锁定";
				NaJieTipPanel.LeftBtn.onClick.RemoveAllListeners();
				NaJieTipPanel.MiddleBtn.onClick.RemoveAllListeners();
				NaJieTipPanel.RightBtn.onClick.RemoveAllListeners();
				AddButtonClickListener(NaJieTipPanel.MiddleBtn, () =>
				{
					if (naJieTipModel.MiddleBtnClickEvent != null)
					{
						naJieTipModel.MiddleBtnClickEvent(naJieTipModel.type, naJieTipModel.dataSource);
					}
					Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
				});
				AddButtonClickListener(NaJieTipPanel.LeftBtn, () =>
				{
					if (naJieTipModel.LeftBtnClickEvent != null)
					{
						naJieTipModel.LeftBtnClickEvent(naJieTipModel.type, naJieTipModel.dataSource);
					}
					Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
				});
				AddButtonClickListener(NaJieTipPanel.RightBtn, () =>
				{
					if (naJieTipModel.RightBtnClickEvent != null)
					{
						naJieTipModel.RightBtnClickEvent(naJieTipModel.type, naJieTipModel.dataSource);
					}
					Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
				});
				LoadTipItem();
			}
			catch (Exception ex)
			{
				Debug.Log("najieTip  reflash失败:" + ex.Message);
			}
		}

		public void LoadTipItem()
		{
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/NaJieTipItem.prefab");
			if (gameObject == null)
			{
				Debug.Log("納戒详细信息无法显示");
				return;
			}
			RectTransform content = NaJieTipPanel.Content;
			foreach (Transform item in content)
			{
				item.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(item.gameObject);
			}
			var lines = m_DataList;
			if (naJieTipModel.FromRing)
			{
				var g = m_DataSource;
				string status = RingLedger.Status(g);
				lines = new List<string> {
					$"物品,{RingLedger.Kind(g)} · {WorldSetting.GetRankAllName(g.Rank)} · {g.Level} 级",
					$"持有,{g.GoodsNum} 件" + (status.Length == 0 ? "" : " · " + status),
					$"回收估价,{g.RecyclePrice} 灵石／件 · 合计 {(decimal)g.RecyclePrice * g.GoodsNum} 灵石"
				};
				foreach (string detail in m_DataList)
				{
					if (detail.StartsWith("品阶,", StringComparison.Ordinal) ||
						detail.StartsWith("回收价格,", StringComparison.Ordinal) ||
						detail.StartsWith("体积,", StringComparison.Ordinal) ||
						detail.StartsWith("体积（立方尺）,", StringComparison.Ordinal)) continue;
					lines.Add(detail);
				}
				if (g.GoodsVolume > 0)
					lines.Add($"单件体积,{g.GoodsVolume:0.###} 立方尺,纳戒按物品格计数，洞府储物箱按体积结算。");
			}
			for (int i = 0; i < lines.Count; i++)
			{
				string[] row = lines[i].Split(new[] { ',' }, 3);
				if (row.Length < 2)
					continue;
				NaJieTipItem item = SetListContentParent(content, gameObject).GetComponent<NaJieTipItem>();
				item.SetDetailItem(row[0], row[1], row.Length == 3 ? row[2] : string.Empty, i);
			}
			InkDialogArt.FitDetailRows(NaJieTipPanel.ScrollView);
		}

		public new void CloseWindow()
		{
			Singleton<UIManager>.Instance.CloseWnd("NaJieTipWnd.prefab");
		}

		public void XiaoHuiClick()
		{
			Singleton<UIManager>.Instance.PopUpWnd("NaJieTipWnd.prefab", true, 2);
		}

		public override void OnDisable()
		{
			detailRequest++;
			base.OnDisable();
		}
	}
}
