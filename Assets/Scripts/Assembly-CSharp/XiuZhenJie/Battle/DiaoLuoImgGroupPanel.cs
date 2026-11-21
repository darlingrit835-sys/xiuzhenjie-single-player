using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiaoLuoImgGroupPanel : MonoBehaviour
	{
		public Image PuTongBg;

		public Text PuTongNumText;

		public Image YouXiuBg;

		public Text YouXiuNumText;

		public Image ChangJianBg;

		public Text ChangJianNumText;

		public Image XiYouBg;

		public Text XiYouNumText;

		public Image QiZhenBg;

		public Text QiZhenText;

		public Image ChuanShuoBg;

		public Text ChuanShuoNumText;

		private Dictionary<int, List<BaseNaJieGoods>> wuPinDic;

		public Dictionary<int, List<BaseNaJieGoods>> WuPinDic
		{
			get
			{
				return wuPinDic;
			}
			private set
			{
				wuPinDic = value;
			}
		}

		private void Awake()
		{
			WuPinDic = new Dictionary<int, List<BaseNaJieGoods>>();
			for (int i = 0; i < 6; i++)
			{
				WuPinDic.Add(i + 1, new List<BaseNaJieGoods>());
			}
		}

		public void DiaoLuoAdd(Dictionary<int, List<BaseNaJieGoods>> dataSource)
		{
			foreach (KeyValuePair<int, List<BaseNaJieGoods>> item in dataSource)
			{
				switch (item.Key)
				{
				case 1:
				case 2:
				case 3:
				case 4:
					DealWithData(1, item.Value);
					NumAdd(PuTongNumText, item.Value.Count);
					break;
				case 5:
				case 6:
				case 7:
				case 8:
					DealWithData(2, item.Value);
					NumAdd(YouXiuNumText, item.Value.Count);
					break;
				case 9:
				case 10:
				case 11:
				case 12:
					DealWithData(3, item.Value);
					NumAdd(XiYouNumText, item.Value.Count);
					break;
				case 13:
				case 14:
				case 15:
				case 16:
					DealWithData(4, item.Value);
					NumAdd(QiZhenText, item.Value.Count);
					break;
				}
			}
		}

		private void DealWithData(int index, List<BaseNaJieGoods> _data)
		{
			foreach (BaseNaJieGoods _datum in _data)
			{
				WuPinDic[index].Add(_datum);
			}
		}

		private void NumAdd(Text numText, int addValue)
		{
			Vector3 endValue = new Vector3(4f, 4f, 4f);
			Vector3 normalV3 = Vector3.one;
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = numText.transform.DOScale(endValue, 0.4f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
			{
				int num = int.Parse(numText.text);
				num += addValue;
				numText.text = num.ToString();
				numText.transform.DOScale(normalV3, 0.4f);
			}));
		}
	}
}
