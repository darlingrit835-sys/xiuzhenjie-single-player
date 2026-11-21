using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QiZhenGeShaiXuanWnd : Window
	{
		public QiZhenGeShaiXuanPanel _panel;

		public RectTransform _rectTransform;

		private int m_CurrentStage;

		private GoodsTypeEnum m_CurrentType;

		private WuXing m_WuXing;

		private PaiMaiType m_PaiMaiType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<QiZhenGeShaiXuanPanel>();
			_rectTransform = base.GameObject.GetComponent<RectTransform>();
			AddToggleClickListener(_panel.GongFa, GongFaToggleClick);
			AddToggleClickListener(_panel.FaShu, FaShuToggleClick);
			AddToggleClickListener(_panel.DanFang, DanFangToggleClick);
			AddToggleClickListener(_panel.QiTu, QiTuToggleClick);
			AddToggleClickListener(_panel.FuShu, FuShuToggleClick);
			AddToggleClickListener(_panel.ZhenTu, ZhengTuToggleClick);
			AddToggleClickListener(_panel.FaQi, FaQiToggleClick);
			AddToggleClickListener(_panel.FuLu, FuLuToggleClick);
			AddToggleClickListener(_panel.ZhenPan, ZhenPanToggleClick);
			AddToggleClickListener(_panel.GongJu, GongJuToggleClick);
			AddToggleClickListener(_panel.DanYao, DanYaoToggleClick);
			AddToggleClickListener(_panel.DongWu, DongWuToggleClick);
			AddToggleClickListener(_panel.ZhiWu, ZhiWuToggleClick);
			AddToggleClickListener(_panel.KuangWu, KuangWuToggleClick);
			AddToggleClickListener(_panel.MiJing, MiJingToggleClick);
			AddToggleClickListener(_panel.yi, Level_Yi_Click);
			AddToggleClickListener(_panel.er, Level_Er_Click);
			AddToggleClickListener(_panel.san, Level_San_Click);
			AddToggleClickListener(_panel.si, Level_Si_Click);
			AddToggleClickListener(_panel.wu, Level_Wu_Click);
			AddToggleClickListener(_panel.liu, Level_Liu_Click);
			AddToggleClickListener(_panel.qi, Level_Qi_Click);
			AddToggleClickListener(_panel.ba, Level_Ba_Click);
			AddToggleClickListener(_panel.jiu, Level_Jiu_Click);
			AddToggleClickListener(_panel.Jin, WuXing_Jin_Click);
			AddToggleClickListener(_panel.Mu, WuXing_Mu_Click);
			AddToggleClickListener(_panel.Shui, WuXing_Shui_Click);
			AddToggleClickListener(_panel.Huo, WuXing_Huo_Click);
			AddToggleClickListener(_panel.Tu, WuXing_Tu_Click);
			AddToggleClickListener(_panel.AllWuXing, WuXing_All_Click);
			AddToggleClickListener(_panel.YiKouJia, PaiMai_YiKouJia_Click);
			AddToggleClickListener(_panel.JingPai, PaiMai_JingPai_Click);
			AddToggleClickListener(_panel.AllPaiMaiType, PaiMai_All_Click);
			AddButtonClickListener(_panel.queding, QueDingBtnClick);
			AddButtonClickListener(_panel.Mask, LiKaiBtnClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Level_Yi_Click(arg0: true);
			GongFaToggleClick(arg0: true);
			WuXing_All_Click(arg0: true);
			PaiMai_All_Click(arg0: true);
		}

		private void GongFaToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.BOOK_ACHIEVEMENT;
			}
			Text component = _panel.GongFa.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void FaShuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.BOOK_SKILL;
			}
			Text component = _panel.FaShu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void DanFangToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.BOOK_ELIXIR;
			}
			Text component = _panel.DanFang.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void QiTuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.BOOK_EQUIPMENT;
			}
			Text component = _panel.QiTu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void FuShuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.BOOK_SIGN;
			}
			Text component = _panel.FuShu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void ZhengTuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.BOOK_MATRIX;
			}
			Text component = _panel.ZhenTu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void FaQiToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.EQUIPMENT;
			}
			Text component = _panel.FaQi.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void FuLuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.SIGN;
			}
			Text component = _panel.FuLu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void ZhenPanToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.MATRIX;
			}
			Text component = _panel.ZhenPan.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void GongJuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.TOOL;
			}
			Text component = _panel.GongJu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void DanYaoToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.ELIXIR;
			}
			Text component = _panel.DanYao.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void DongWuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.MATERIAL_ANIMAL;
			}
			Text component = _panel.DongWu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void KuangWuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.MATERIAL_MINERAL;
			}
			Text component = _panel.KuangWu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void ZhiWuToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.MATERIAL_PLANT;
			}
			Text component = _panel.ZhiWu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void MiJingToggleClick(bool arg0)
		{
			if (arg0)
			{
				m_CurrentType = GoodsTypeEnum.SECREAT_AREA;
			}
			Text component = _panel.MiJing.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Yi_Click(bool arg0)
		{
			m_CurrentStage = 1;
			Text component = _panel.yi.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Er_Click(bool arg0)
		{
			m_CurrentStage = 2;
			Text component = _panel.er.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_San_Click(bool arg0)
		{
			m_CurrentStage = 3;
			Text component = _panel.san.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Si_Click(bool arg0)
		{
			m_CurrentStage = 4;
			Text component = _panel.si.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Wu_Click(bool arg0)
		{
			m_CurrentStage = 5;
			Text component = _panel.wu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Liu_Click(bool arg0)
		{
			m_CurrentStage = 6;
			Text component = _panel.liu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Qi_Click(bool arg0)
		{
			m_CurrentStage = 7;
			Text component = _panel.qi.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Ba_Click(bool arg0)
		{
			m_CurrentStage = 8;
			Text component = _panel.ba.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void Level_Jiu_Click(bool arg0)
		{
			m_CurrentStage = 9;
			Text component = _panel.jiu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void WuXing_Jin_Click(bool arg0)
		{
			m_WuXing = WuXing.Jin;
			Text component = _panel.Jin.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void WuXing_Mu_Click(bool arg0)
		{
			m_WuXing = WuXing.Mu;
			Text component = _panel.Mu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void WuXing_Shui_Click(bool arg0)
		{
			m_WuXing = WuXing.Shui;
			Text component = _panel.Shui.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void WuXing_Huo_Click(bool arg0)
		{
			m_WuXing = WuXing.Huo;
			Text component = _panel.Huo.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void WuXing_Tu_Click(bool arg0)
		{
			m_WuXing = WuXing.Tu;
			Text component = _panel.Tu.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void WuXing_All_Click(bool arg0)
		{
			m_WuXing = WuXing.All;
			Text component = _panel.AllWuXing.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void PaiMai_YiKouJia_Click(bool arg0)
		{
			m_PaiMaiType = PaiMaiType.YiKouJia;
			Text component = _panel.YiKouJia.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void PaiMai_JingPai_Click(bool arg0)
		{
			m_PaiMaiType = PaiMaiType.JingPai;
			Text component = _panel.JingPai.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void PaiMai_All_Click(bool arg0)
		{
			m_PaiMaiType = PaiMaiType.All;
			Text component = _panel.AllPaiMaiType.transform.Find("Text").GetComponent<Text>();
			ChangeTextColor(component, arg0);
		}

		private void ChangeTextColor(Text _text, bool isChoose)
		{
			if (isChoose)
			{
				_text.color = Color.white;
			}
			else
			{
				_text.color = Color.black;
			}
		}

		private void QueDingBtnClick()
		{
			PopUpPanel(isPop: false);
		}

		private void LiKaiBtnClick()
		{
			PopUpPanel(isPop: false);
		}

		public void PopUpShaiXuanPanel()
		{
			PopUpPanel(isPop: true);
		}

		public void PopUpPanel(bool isPop)
		{
			_ = _rectTransform.sizeDelta;
			if (isPop)
			{
				DOTween.To(() => _rectTransform.anchoredPosition, (Vector2 v) =>
				{
					_rectTransform.anchoredPosition = v;
				}, new Vector2(0f, 0f), 0.5f);
				_panel.Mask.gameObject.SetActive(value: true);
			}
			else
			{
				DOTween.To(() => _rectTransform.anchoredPosition, (Vector2 v) =>
				{
					_rectTransform.anchoredPosition = v;
				}, new Vector2(215f, 0f), 0.5f);
				_panel.Mask.gameObject.SetActive(value: false);
			}
		}

		public void GetConditions(QZG_SearchWuPinListParamModel _condition)
		{
			if (_condition == null)
			{
				_condition = new QZG_SearchWuPinListParamModel();
			}
			if (string.IsNullOrEmpty(_panel.zuidijia.text))
			{
				_condition.minFinalPrice = 0L;
			}
			else
			{
				_condition.minFinalPrice = long.Parse(_panel.zuidijia.text);
			}
			if (string.IsNullOrEmpty(_panel.zuigaojia.text))
			{
				_condition.maxFinalPrice = 0L;
			}
			else
			{
				_condition.maxFinalPrice = long.Parse(_panel.zuigaojia.text);
			}
			_condition.stage = m_CurrentStage;
			_condition.type = (int)m_CurrentType;
			if (m_WuXing == WuXing.All)
			{
				_condition.fiveElement = 0;
			}
			else
			{
				_condition.fiveElement = (short)m_WuXing;
			}
			if (m_PaiMaiType == PaiMaiType.All)
			{
				_condition.auctionType = 0;
			}
			else
			{
				_condition.auctionType = (short)m_PaiMaiType;
			}
		}
	}
}
