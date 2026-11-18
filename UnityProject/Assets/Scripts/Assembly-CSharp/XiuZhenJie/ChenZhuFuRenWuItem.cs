using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChenZhuFuRenWuItem : MonoBehaviour
	{
		public Text RenWu;

		public Text RenWuMiaoShu;

		public Text HaoGanDuMiaoShu;

		public Text JiangLiMiaoShu;

		public Text WeiWanCheng;

		public Image AwardImage;

		public Button LingQuBtn;

		public void SetItem(string RenWu, string RenWuMiaoShu, string HaoGanDuMiaoShu, string JiangLiMiaoShu, int index, Action _action)
		{
			this.RenWu.text = RenWu;
			this.RenWuMiaoShu.text = RenWuMiaoShu;
			this.HaoGanDuMiaoShu.text = HaoGanDuMiaoShu;
			this.JiangLiMiaoShu.text = JiangLiMiaoShu;
			switch (index)
			{
			case 0:
				WeiWanCheng.text = "【未完成】";
				AwardImage.enabled = false;
				break;
			case 1:
				WeiWanCheng.text = "【领取奖励】";
				AwardImage.enabled = true;
				LingQuBtn = WeiWanCheng.gameObject.AddComponent<Button>();
				LingQuBtn.onClick.AddListener(() =>
				{
					_action();
				});
				break;
			case 2:
				WeiWanCheng.text = "【已完成】";
				AwardImage.enabled = true;
				break;
			default:
				WeiWanCheng.text = "【未完成】";
				AwardImage.enabled = false;
				break;
			}
		}
	}
}
