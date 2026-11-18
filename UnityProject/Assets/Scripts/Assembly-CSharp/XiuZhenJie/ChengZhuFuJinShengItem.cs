using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChengZhuFuJinShengItem : MonoBehaviour
	{
		public GameObject WeiWanCheng;

		public Text ZhaoMuZhiWei;

		public Text JingJieYaoQiu;

		public Text HanGanDu;

		public Text GongFeng;

		public Text ZhiWei;

		public GameObject WanCheng;

		public Text DangQianZhiWei;

		public Text WanChengGongFeng;

		public Text LingQuGongFeng;

		public void SetItem()
		{
			WeiWanCheng.SetActive(value: false);
			WanCheng.SetActive(value: true);
		}

		public void SetWeiWanCheng(string ZhaoMuZhiWei, string JingJieYaoQiu, string HanGanDu, string GongFeng, string ZhiWei)
		{
			this.ZhaoMuZhiWei.text = ZhaoMuZhiWei;
			this.JingJieYaoQiu.text = JingJieYaoQiu;
			this.HanGanDu.text = HanGanDu;
			this.GongFeng.text = GongFeng;
			this.ZhiWei.text = $"【{ZhiWei}】";
			this.ZhiWei.gameObject.AddComponent<Button>().onClick.AddListener(() =>
			{
				UIManager instance = Singleton<UIManager>.Instance;
				object[] paralist = new string[4] { ZhiWei, JingJieYaoQiu, HanGanDu, GongFeng };
				instance.PopUpWnd("ChengZhuFuChengWeiKeQingWnd.prefab", bTop: true, paralist);
			});
		}

		public void SetWanCheng(string DangQianZhiWei, string DangQianGongFeng, int index, Action _action)
		{
			this.DangQianZhiWei.text = $"當前【{DangQianZhiWei}】";
			WanChengGongFeng.text = DangQianGongFeng;
			switch (index)
			{
			case 0:
				LingQuGongFeng.text = "【領取供奉】";
				LingQuGongFeng.gameObject.AddComponent<Button>().onClick.AddListener(() =>
				{
					_action();
				});
				break;
			case 1:
				LingQuGongFeng.text = "【已領取】";
				break;
			}
		}
	}
}
