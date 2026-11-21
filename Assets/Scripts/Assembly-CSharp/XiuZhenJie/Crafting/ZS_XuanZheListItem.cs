using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class ZS_XuanZheListItem : MonoBehaviour
	{
		public int ListShu;

		public ZS_XuanZheItem Buttn_1;

		public ZS_XuanZheItem Buttn_2;

		public ZS_XuanZheItem Buttn_3;

		public ZS_XuanZheItem Buttn_4;

		public ZS_XuanZheItem Buttn_5;

		public ZS_XuanZheItem Buttn_6;

		public ZS_XuanZheItem Buttn_7;

		public ZS_XuanZheItem Buttn_8;

		public ZS_XuanZheItem Buttn_9;

		public void GuanBi()
		{
			Buttn_1.XianshiNew(xianshi: false);
			Buttn_2.XianshiNew(xianshi: false);
			Buttn_3.XianshiNew(xianshi: false);
			Buttn_4.XianshiNew(xianshi: false);
			Buttn_5.XianshiNew(xianshi: false);
			Buttn_6.XianshiNew(xianshi: false);
			Buttn_7.XianshiNew(xianshi: false);
			Buttn_8.XianshiNew(xianshi: false);
			if (ListShu == 9)
			{
				Buttn_9.XianshiNew(xianshi: false);
			}
		}

		public void KaiQi(string namestr)
		{
			Buttn_1.XianshiNew(xianshi: true, namestr);
			Buttn_2.XianshiNew(xianshi: true, namestr);
			Buttn_3.XianshiNew(xianshi: true, namestr);
			Buttn_4.XianshiNew(xianshi: true, namestr);
			Buttn_5.XianshiNew(xianshi: true, namestr);
			Buttn_6.XianshiNew(xianshi: true, namestr);
			Buttn_7.XianshiNew(xianshi: true, namestr);
			Buttn_8.XianshiNew(xianshi: true, namestr);
			if (ListShu == 9)
			{
				Buttn_9.XianshiNew(xianshi: true, namestr);
			}
		}

		public void PanDuanXianShi(Dictionary<int, string> namestr)
		{
			for (int i = 0; i < ListShu; i++)
			{
				switch (i)
				{
				case 0:
				{
					bool panDuan = namestr.ContainsValue(Buttn_1.Name);
					Buttn_1.ImgGaiBian(panDuan);
					break;
				}
				case 1:
				{
					bool panDuan = namestr.ContainsValue(Buttn_2.Name);
					Buttn_2.ImgGaiBian(panDuan);
					break;
				}
				case 2:
				{
					bool panDuan = namestr.ContainsValue(Buttn_3.Name);
					Buttn_3.ImgGaiBian(panDuan);
					break;
				}
				case 3:
				{
					bool panDuan = namestr.ContainsValue(Buttn_4.Name);
					Buttn_4.ImgGaiBian(panDuan);
					break;
				}
				case 4:
				{
					bool panDuan = namestr.ContainsValue(Buttn_5.Name);
					Buttn_5.ImgGaiBian(panDuan);
					break;
				}
				case 5:
				{
					bool panDuan = namestr.ContainsValue(Buttn_6.Name);
					Buttn_6.ImgGaiBian(panDuan);
					break;
				}
				case 6:
				{
					bool panDuan = namestr.ContainsValue(Buttn_7.Name);
					Buttn_7.ImgGaiBian(panDuan);
					break;
				}
				case 7:
				{
					bool panDuan = namestr.ContainsValue(Buttn_8.Name);
					Buttn_8.ImgGaiBian(panDuan);
					break;
				}
				case 8:
				{
					bool panDuan = namestr.ContainsValue(Buttn_9.Name);
					Buttn_9.ImgGaiBian(panDuan);
					break;
				}
				}
			}
		}
	}
}
