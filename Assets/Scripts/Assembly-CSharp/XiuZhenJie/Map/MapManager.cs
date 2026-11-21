using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class MapManager : MonoBehaviour
	{
		private BattleSceneManager m_BattleSceneManager;

		private MapEntity m_MapDataSource;

		public GameObject[] ItemJin_Tu_Huo;

		public GameObject[] ItemMu;

		public GameObject[] ItemShui;

		public GameObject[] GuaiWuList;

		public GameObject[] RenGuaiWuList;

		public GameObject[] QiYuList;

		public GameObject[] ZhiWuKuangWu;

		private int PuToDiTuFanWei = 280;

		private int MiJingDiTuFanWei = 340;

		private int TowerDiTuFanWei = 80;

		private List<Vector3> BiJin = new List<Vector3>();

		private List<Vector3> PuToFanWeiWaiQuan = new List<Vector3>();

		private List<Vector3> PuToFanWeiZhoQuan = new List<Vector3>();

		private List<Vector3> PuToFanWeiNeiQuan = new List<Vector3>();

		private List<Vector3> MiJingFanWeiWaiQuan = new List<Vector3>();

		private List<Vector3> MiJingFanWeiZhoQuan = new List<Vector3>();

		private List<Vector3> MiJingFanWeiNeiQuan = new List<Vector3>();

		public Queue<Vector3> ZhiWuKuangWuZuoBiao = new Queue<Vector3>();

		public Queue<Vector3> RenGuaiWuZuoBiao = new Queue<Vector3>();

		public Queue<Vector3> GuaiWuZuoBiao = new Queue<Vector3>();

		public Queue<Vector3> QiYuZuoBiao = new Queue<Vector3>();

		private List<Vector3> ZiYuanZongZuoBiao = new List<Vector3>();

		public void Init(BattleSceneManager _battleSceneManager, List<MonsterEntity> _MosterList, WuXing wuXing, int MapSzie)
		{
			m_BattleSceneManager = _battleSceneManager;
			ItemJin_Tu_Huo = Singleton<BattleManager>.Instance.Jin_Tu_HuoMapObjList.ToArray();
			ItemMu = Singleton<BattleManager>.Instance.MuMapObjList.ToArray();
			ItemShui = Singleton<BattleManager>.Instance.ShuiMapObjList.ToArray();
			PanDuanDiTu(MapSzie, wuXing);
		}

		public void Init(BattleSceneManager _battleSceneManager, BattleMapType MapType, WuXing wuXing)
		{
			m_BattleSceneManager = _battleSceneManager;
			ItemJin_Tu_Huo = Singleton<BattleManager>.Instance.Jin_Tu_HuoMapObjList.ToArray();
			ItemMu = Singleton<BattleManager>.Instance.MuMapObjList.ToArray();
			ItemShui = Singleton<BattleManager>.Instance.ShuiMapObjList.ToArray();
			switch (MapType)
			{
			case BattleMapType.NormalMap:
				PanDuanDiTu(1, wuXing);
				break;
			case BattleMapType.ChuanChengMiJing:
				PanDuanDiTu(2, wuXing);
				break;
			case BattleMapType.CangBaoMiJing:
				PanDuanDiTu(3, wuXing);
				break;
			case BattleMapType.TongTianTa:
				wuXing = (WuXing)Random.Range(1, 5);
				PanDuanDiTu(4, wuXing);
				break;
			}
		}

		private int TiQuX(Vector3 i)
		{
			return (int)i.x;
		}

		private int TiQuY(Vector3 i)
		{
			return (int)i.y;
		}

		public void PanDuanDiTu(int diTuLeiXing, WuXing wuXing)
		{
			switch (diTuLeiXing)
			{
			case 1:
				DiShi(PuToDiTuFanWei);
				PanDuShuXing(wuXing);
				break;
			case 2:
				DiShi(MiJingDiTuFanWei);
				PanDuShuXing(wuXing);
				break;
			case 3:
				DiShi(MiJingDiTuFanWei);
				PanDuShuXing(wuXing);
				break;
			case 4:
			{
				DiShi(TowerDiTuFanWei);
				PanDuShuXing(wuXing);
				MaxMin(20);
				Vector3 item = new Vector3(0f, 9f);
				GuaiWuZuoBiao.Enqueue(item);
				Vector3 item2 = new Vector3(-9f, 9f);
				GuaiWuZuoBiao.Enqueue(item2);
				Vector3 item3 = new Vector3(9f, 9f);
				GuaiWuZuoBiao.Enqueue(item3);
				break;
			}
			}
		}

		private void MaxMin(int shu)
		{
			_ = shu / 2;
			_ = shu / 2;
		}

		private void ShanChu(List<Vector3> x, int q, int w, int e, int r, int t, int y, int u, int i)
		{
			x.RemoveAt(q);
			x.RemoveAt(w);
			x.RemoveAt(e);
			x.RemoveAt(r);
			x.RemoveAt(t);
			x.RemoveAt(y);
			x.RemoveAt(u);
			x.RemoveAt(i);
		}

		private void SuiJi(List<Vector3> nei, List<Vector3> Wai, float fanwei)
		{
			int num = Random.Range(1, 5);
			if (num == 1)
			{
				WeiYi(nei, fanwei);
			}
			if (num > 1)
			{
				WeiYi(Wai, fanwei);
			}
		}

		private void DiShi(int wuPingFanWei)
		{
			float num = (float)wuPingFanWei / 19.2f;
			float num2 = (float)wuPingFanWei / 9.68f;
			Vector3 item = new Vector3(wuPingFanWei / 2, wuPingFanWei / 2, 1f);
			BiJin.Add(item);
			for (int i = 0; (float)i < num; i++)
			{
				item.x -= 19.2f;
				BiJin.Add(item);
			}
			float num3 = 9.68f;
			int j = 0;
			for (int k = 0; (float)k < num2 + 1f; k++)
			{
				for (; (float)j < num + 1f; j++)
				{
					Vector3 item2 = BiJin[j];
					item2.y -= num3;
					BiJin.Add(item2);
				}
				num3 += 9.68f;
				j = 0;
			}
		}

		private void PanDuShuXing(WuXing wuXing)
		{
			switch (wuXing)
			{
			case WuXing.Jin:
			case WuXing.Huo:
			case WuXing.Tu:
				Shecen(ItemJin_Tu_Huo, 0, 1, 2, 11);
				break;
			case WuXing.Mu:
				Shecen(ItemMu, 0, 4, 5, 33);
				break;
			case WuXing.Shui:
				Shecen(ItemShui, 0, 3, 4, 19);
				break;
			case WuXing.All:
				break;
			}
		}

		private void Shecen(GameObject[] item1, int x1, int x2, int x3, int x4)
		{
			for (int i = 0; i < BiJin.Count; i++)
			{
				Vector3 vector = BiJin[i];
				int num = Random.Range(x1, x2);
				Object.Instantiate(item1[num], new Vector3(vector.x, vector.y, vector.z), Quaternion.identity);
				int num2 = Random.Range(x3, x4);
				Object.Instantiate(item1[num2], new Vector3(BiJin[i].x - 9.6f, BiJin[i].y - 4.84f, BiJin[i].z + 1f), Quaternion.identity);
			}
		}

		private void DiTuFanWei(List<Vector3> neiQuan, List<Vector3> zhoQuan, List<Vector3> waiQuan, int nei, int zho, float wai)
		{
			for (int i = 0; i < BiJin.Count; i++)
			{
				Vector3 item = BiJin[i];
				if ((item.x <= (float)nei) & (item.x >= (float)(-nei)))
				{
					if ((item.y <= (float)nei) & (item.y >= (float)(-nei)))
					{
						neiQuan.Add(item);
					}
				}
				else if ((item.x <= (float)zho) & (item.x >= (float)(-zho)))
				{
					if ((item.y <= (float)zho) & (item.y >= (float)(-zho)))
					{
						zhoQuan.Add(item);
					}
				}
				else if (((item.x <= wai) & (item.x >= 0f - wai)) && ((item.y <= wai) & (item.y >= 0f - wai)))
				{
					waiQuan.Add(item);
				}
			}
		}

		private void WuPingNeiFanWei(int zhiWuKuangWuShuLiang, int Waiwu, int Ren, List<Vector3> quan, Queue<Vector3> zhiWuKuangWuZoBia, Queue<Vector3> guaiWuZoBia, Queue<Vector3> renGuaiWuZoBia, float fanwei, int gaiWuJuLi, int zhiGaiJuLi)
		{
			for (int i = 0; i < zhiWuKuangWuShuLiang; i++)
			{
				Vector3 vector = CanShengSuiJiShu(quan, ZiYuanZongZuoBiao, fanwei);
				zhiWuKuangWuZoBia.Enqueue(vector);
				ZiYuanZongZuoBiao.Add(vector);
				if (i < Waiwu)
				{
					int w = TiQuX(vector);
					int q = TiQuY(vector);
					Vector3 item = CanShengSuiJiShuNew(w, q, ZiYuanZongZuoBiao, zhiGaiJuLi, gaiWuJuLi);
					ZiYuanZongZuoBiao.Add(item);
					guaiWuZoBia.Enqueue(item);
				}
			}
			for (int j = 0; j < Waiwu - zhiWuKuangWuShuLiang; j++)
			{
				Vector3 item2 = CanShengZoBiao(quan, fanwei, ZiYuanZongZuoBiao);
				guaiWuZoBia.Enqueue(item2);
				ZiYuanZongZuoBiao.Add(item2);
			}
			for (int k = 0; k < Ren; k++)
			{
				Vector3 item3 = CanShengZoBiao(quan, fanwei, ZiYuanZongZuoBiao);
				renGuaiWuZoBia.Enqueue(item3);
				ZiYuanZongZuoBiao.Add(item3);
			}
		}

		private void WeiYi(List<Vector3> quan, float fanwei)
		{
			Vector3 item = CanShengZoBiao(quan, fanwei, ZiYuanZongZuoBiao);
			QiYuZuoBiao.Enqueue(item);
			ZiYuanZongZuoBiao.Add(item);
		}

		private Vector3 CanShengSuiJiShu(List<Vector3> PuToFanWei, List<Vector3> zoZoBiao, float fanwei)
		{
			Vector3 vector;
			do
			{
				int index = Random.Range(0, PuToFanWei.Count);
				int num = TiQuX(PuToFanWei[index]);
				int num2 = TiQuY(PuToFanWei[index]);
				vector = new Vector3(num, num2);
			}
			while (PanDuanWeiZhi(vector, zoZoBiao, fanwei));
			return vector;
		}

		private Vector3 CanShengSuiJiShuNew(int w, int q, List<Vector3> zoZoBiao, float zofanWei, float fanwei)
		{
			Vector3 vector;
			do
			{
				vector = new Vector3(Random.Range((float)w - fanwei, (float)w + fanwei), Random.Range((float)q + fanwei, (float)q - fanwei));
			}
			while (PanDuanWeiZhi(vector, zoZoBiao, zofanWei));
			return vector;
		}

		private Vector3 CanShengZoBiao(List<Vector3> PuToFanWei, float fanwie, List<Vector3> zoZoBiao)
		{
			Vector3 vector;
			do
			{
				int index = Random.Range(0, PuToFanWei.Count);
				int num = TiQuX(PuToFanWei[index]);
				int num2 = TiQuY(PuToFanWei[index]);
				vector = new Vector3(Random.Range((float)num - fanwie, (float)num + fanwie), Random.Range((float)num2 + fanwie, (float)num2 - fanwie));
			}
			while (PanDuanWeiZhi(vector, zoZoBiao, fanwie));
			return vector;
		}

		private void BaoCo(Vector3 t)
		{
			if (t == default(Vector3))
			{
				Debug.Log("地图生成失败");
			}
		}

		private bool Panduan(Vector3 createPosition, float x)
		{
			if ((createPosition.x < x) & (createPosition.x > 0f - x))
			{
				_ = (createPosition.y < x) & (createPosition.y > 0f - x);
				return false;
			}
			return true;
		}

		private bool PanDuanWeiZhi(Vector3 createPosition, List<Vector3> zoZoBiao, float fanWei)
		{
			float x = createPosition.x;
			float y = createPosition.y;
			for (int i = 0; i < zoZoBiao.Count; i++)
			{
				float x2 = zoZoBiao[i].x;
				float y2 = zoZoBiao[i].y;
				if (((x2 - fanWei < x) & (x2 + fanWei > x)) && ((y2 - fanWei < y) & (y2 + fanWei > y)))
				{
					return true;
				}
			}
			return false;
		}

		public void ReSetMonsterPosQue()
		{
			GuaiWuZuoBiao.Clear();
			Vector3 item = new Vector3(0f, 9f);
			GuaiWuZuoBiao.Enqueue(item);
			Vector3 item2 = new Vector3(-9f, 9f);
			GuaiWuZuoBiao.Enqueue(item2);
			Vector3 item3 = new Vector3(9f, 9f);
			GuaiWuZuoBiao.Enqueue(item3);
		}
	}
}
