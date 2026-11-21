using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoItem : MonoBehaviour
	{
		public Image HaveImg;

		public Image NoHaveImg;

		public Image MapHaveImg;

		public Image MapNoHaveImg;

		public Button NumBtn;

		public Text NameText;

		public Text NumText;

		public Text MapNameText;

		public Button MapBtn;

		public Image YouHouXianShiImg;

		private int m_CaiLiaoListIndex;

		private DanFangStepModel m_DanFangStepModel;

		public void SetValue(DanFangStepModel danFangStepModel, Dictionary<string, BaseNaJieGoodEntity> caiLiaoDic, UnityAction<DanFangStepModel, List<BaseNaJieGoodEntity>, UnityAction<int>, int> ClickAction, List<string> caiLiaoList, UnityAction MapBtnClick, int PanDuan)
		{
			if (danFangStepModel == null)
			{
				return;
			}
			m_DanFangStepModel = danFangStepModel;
			NameText.text = danFangStepModel.targetName;
			MapNameText.text = (string.IsNullOrEmpty(danFangStepModel.mapName) ? "虚神界" : danFangStepModel.mapName);
			List<BaseNaJieGoodEntity> naJieGoods = Singleton<RoleManager>.Instance.RoleBoxDic[10].GetSameTemplateGoods(danFangStepModel.targetId);
			m_CaiLiaoListIndex = 0;
			if (naJieGoods.Count <= 0)
			{
				HaveImg.gameObject.SetActive(value: false);
				MapHaveImg.gameObject.SetActive(value: false);
				NumBtn.gameObject.SetActive(value: false);
				if (PanDuan == 1)
				{
					YouHouXianShiImg.gameObject.SetActive(value: true);
				}
				else
				{
					YouHouXianShiImg.gameObject.SetActive(value: false);
				}
			}
			else
			{
				if (caiLiaoList.Contains(naJieGoods[m_CaiLiaoListIndex].GoodsId))
				{
					if (naJieGoods.Count <= 1)
					{
						HaveImg.gameObject.SetActive(value: false);
						MapHaveImg.gameObject.SetActive(value: false);
						NumBtn.gameObject.SetActive(value: false);
						if (PanDuan == 1)
						{
							YouHouXianShiImg.gameObject.SetActive(value: true);
						}
						else
						{
							YouHouXianShiImg.gameObject.SetActive(value: false);
						}
						return;
					}
					m_CaiLiaoListIndex = 1;
				}
				caiLiaoDic.Add(danFangStepModel.id, naJieGoods[m_CaiLiaoListIndex]);
				caiLiaoList.Add(naJieGoods[m_CaiLiaoListIndex].GoodsId);
				NumBtn.onClick.RemoveAllListeners();
				NumBtn.onClick.AddListener(() =>
				{
					if (ClickAction != null)
					{
						ClickAction(m_DanFangStepModel, naJieGoods, GetChooseIndex, m_CaiLiaoListIndex);
					}
				});
				NumBtn.gameObject.SetActive(value: true);
				NumText.text = ((naJieGoods[0].GoodsNum > 99) ? "99+" : naJieGoods[0].GoodsNum.ToString());
				HaveImg.gameObject.SetActive(value: true);
				NoHaveImg.gameObject.SetActive(value: false);
				MapHaveImg.gameObject.SetActive(value: true);
			}
			MapBtn.onClick.RemoveAllListeners();
			MapBtn.onClick.AddListener(MapBtnClick);
		}

		public void SetQiTuValue(QiTuCaiLiaoEntity qiTuModel, Dictionary<string, BaseNaJieGoodEntity> caiLiaoDic, UnityAction<QiTuCaiLiaoEntity, List<BaseNaJieGoodEntity>, UnityAction<int>, int> ClickAction, List<string> caiLiaoList, UnityAction MapBtnClick, int PanDuan)
		{
			if (qiTuModel == null)
			{
				return;
			}
			NameText.text = qiTuModel.TargetName;
			MapNameText.text = (string.IsNullOrEmpty(qiTuModel.MapName) ? "虚神界" : qiTuModel.MapName);
			List<BaseNaJieGoodEntity> naJieGoods = Singleton<RoleManager>.Instance.RoleBoxDic[10].GetSameTemplateGoods(qiTuModel.TargetId);
			m_CaiLiaoListIndex = 0;
			if (naJieGoods.Count <= 0)
			{
				HaveImg.gameObject.SetActive(value: false);
				MapHaveImg.gameObject.SetActive(value: false);
				NumBtn.gameObject.SetActive(value: false);
				if (PanDuan == 1)
				{
					YouHouXianShiImg.gameObject.SetActive(value: true);
				}
				else
				{
					YouHouXianShiImg.gameObject.SetActive(value: false);
				}
			}
			else
			{
				if (caiLiaoList.Contains(naJieGoods[m_CaiLiaoListIndex].GoodsId))
				{
					if (naJieGoods.Count <= 1)
					{
						HaveImg.gameObject.SetActive(value: false);
						MapHaveImg.gameObject.SetActive(value: false);
						NumBtn.gameObject.SetActive(value: false);
						if (PanDuan == 1)
						{
							YouHouXianShiImg.gameObject.SetActive(value: true);
						}
						else
						{
							YouHouXianShiImg.gameObject.SetActive(value: false);
						}
						return;
					}
					m_CaiLiaoListIndex = 1;
				}
				caiLiaoDic.Add(qiTuModel.TargetId, naJieGoods[m_CaiLiaoListIndex]);
				caiLiaoList.Add(naJieGoods[m_CaiLiaoListIndex].GoodsId);
				NumBtn.onClick.RemoveAllListeners();
				NumBtn.onClick.AddListener(() =>
				{
					if (ClickAction != null)
					{
						ClickAction(qiTuModel, naJieGoods, GetChooseIndex, m_CaiLiaoListIndex);
					}
				});
				NumBtn.gameObject.SetActive(value: true);
				NumText.text = ((naJieGoods.Count > 99) ? "99+" : naJieGoods.Count.ToString());
				HaveImg.gameObject.SetActive(value: true);
				NoHaveImg.gameObject.SetActive(value: false);
				MapHaveImg.gameObject.SetActive(value: true);
			}
			MapBtn.onClick.RemoveAllListeners();
			MapBtn.onClick.AddListener(MapBtnClick);
		}

		private void GetChooseIndex(int index)
		{
			m_CaiLiaoListIndex = index;
		}
	}
}
