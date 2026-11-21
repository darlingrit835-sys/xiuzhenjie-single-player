using UnityEngine;

namespace XiuZhenJie
{
	public class ZS_YuanPanButListItem : MonoBehaviour
	{
		public int Shu;

		public ZS_YuanPanButItem Buttn_1;

		public ZS_YuanPanButItem Buttn_2;

		public ZS_YuanPanButItem Buttn_3;

		public ZS_YuanPanButItem Buttn_4;

		public ZS_YuanPanButItem Buttn_5;

		public ZS_YuanPanButItem Buttn_6;

		public ZS_YuanPanButItem Buttn_7;

		public ZS_YuanPanButItem Buttn_8;

		private int m_TimeRemaining;

		private bool m_Direction;

		public void MeanwhileDirection(double direction)
		{
			Buttn_1.Direction(direction);
			Buttn_2.Direction(direction);
			Buttn_3.Direction(direction);
			Buttn_4.Direction(direction);
			Buttn_5.Direction(direction);
			Buttn_6.Direction(direction);
			Buttn_7.Direction(direction);
			Buttn_8.Direction(direction);
		}

		public void MeanwhileDirectionnew(double direction)
		{
			Buttn_1.DireTionNew(direction);
			Buttn_2.DireTionNew(direction);
			Buttn_3.DireTionNew(direction);
			Buttn_4.DireTionNew(direction);
			Buttn_5.DireTionNew(direction);
			Buttn_6.DireTionNew(direction);
			Buttn_7.DireTionNew(direction);
			Buttn_8.DireTionNew(direction);
		}

		public void BianSe(string direction)
		{
			Buttn_1.BianSe(direction);
			Buttn_2.BianSe(direction);
			Buttn_3.BianSe(direction);
			Buttn_4.BianSe(direction);
			Buttn_5.BianSe(direction);
			Buttn_6.BianSe(direction);
			Buttn_7.BianSe(direction);
			Buttn_8.BianSe(direction);
		}

		public void ButImage()
		{
			Buttn_1.ButImage();
			Buttn_2.ButImage();
			Buttn_3.ButImage();
			Buttn_4.ButImage();
			Buttn_5.ButImage();
			Buttn_6.ButImage();
			Buttn_7.ButImage();
			Buttn_8.ButImage();
		}

		public void KaiQi()
		{
			Buttn_1.BinDing();
			Buttn_2.BinDing();
			Buttn_3.BinDing();
			Buttn_4.BinDing();
			Buttn_5.BinDing();
			Buttn_6.BinDing();
			Buttn_7.BinDing();
			Buttn_8.BinDing();
		}

		public void Rechristen(int shu, string nameText)
		{
			switch (shu)
			{
			case 0:
				Buttn_1.YuanPanText.text = nameText;
				Buttn_1.YiYuanPanText.text = nameText;
				break;
			case 1:
				Buttn_2.YuanPanText.text = nameText;
				Buttn_2.YiYuanPanText.text = nameText;
				break;
			case 2:
				Buttn_3.YuanPanText.text = nameText;
				Buttn_3.YiYuanPanText.text = nameText;
				break;
			case 3:
				Buttn_4.YuanPanText.text = nameText;
				Buttn_4.YiYuanPanText.text = nameText;
				break;
			case 4:
				Buttn_5.YuanPanText.text = nameText;
				Buttn_5.YiYuanPanText.text = nameText;
				break;
			case 5:
				Buttn_6.YuanPanText.text = nameText;
				Buttn_6.YiYuanPanText.text = nameText;
				break;
			case 6:
				Buttn_7.YuanPanText.text = nameText;
				Buttn_7.YiYuanPanText.text = nameText;
				break;
			case 7:
				Buttn_8.YuanPanText.text = nameText;
				Buttn_8.YiYuanPanText.text = nameText;
				break;
			}
		}
	}
}
