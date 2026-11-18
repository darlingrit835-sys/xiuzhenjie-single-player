using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class QianDaoItem : MonoBehaviour
	{
		public Image GoodsIcon;

		public Image GoodsRank;

		public Image MaskImg;

		public Image QianDaoIm;

		public Animation QianDaoAni;

		public Text GoodsName;

		public Text[] GoodsNames;

		private int IndexDay;

		public void SetValue(int indexDay, bool haveQianDao, List<QianDaoItemGoodEntity> data)
		{
			if (data != null)
			{
				IndexDay = indexDay;
				if (IndexDay == 7)
				{
					Set7thDay(data);
				}
				else
				{
					SetNormalDay((data.Count > 0) ? data[0] : null);
				}
				if (haveQianDao)
				{
					MaskImg.gameObject.SetActive(value: true);
				}
			}
		}

		private void SetNormalDay(QianDaoItemGoodEntity data)
		{
			if (data != null)
			{
				if (data.GoodsType == 99 || data.GoodsType == 100)
				{
					GoodsIcon.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
				}
				int num = ((Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 6) ? (data.GoodsCount * 2) : data.GoodsCount);
				GoodsName.text = $"{data.GoodsName}*{num}";
				GoodsIcon.sprite = data.GoodsIcon;
				GoodsRank.sprite = data.GoodsRankSprite;
			}
		}

		private void Set7thDay(List<QianDaoItemGoodEntity> data)
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (GoodsNames.Length > i)
				{
					int num = ((Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 6) ? (data[i].GoodsCount * 2) : data[i].GoodsCount);
					GoodsNames[i].gameObject.SetActive(value: true);
					GoodsNames[i].text = $"{data[i].GoodsName}*{num}";
				}
			}
		}

		public void StartAnimation()
		{
			base.transform.SetAsLastSibling();
			MaskImg.gameObject.SetActive(value: true);
			QianDaoAni.Play();
		}
	}
}
