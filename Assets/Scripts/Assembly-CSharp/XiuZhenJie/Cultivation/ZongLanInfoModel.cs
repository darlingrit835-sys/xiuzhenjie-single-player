namespace XiuZhenJie
{
	public class ZongLanInfoModel : BaseModel
	{
		public int a;

		public int b;

		public int c;

		public int d;

		public int e;

		public int f;

		public int g;

		public int h;

		public int i;

		public int j;

		public int k;

		public int l;

		public int m;

		public int n;

		public int o;

		public int p;

		public int q;

		public int r;

		public int s;

		public int t;

		public int u;

		public int v;

		public int w;

		public int x;

		public ZongLanInfoModel()
		{
			i = 14;
			j = 13;
			k = 12;
			l = 1;
			m = 11;
			n = 8;
			o = 9;
			p = 8;
		}

		public ZongLanInfoModel(AcupointDetailResp_1005020 data)
		{
			a = data.ThreadType2Count;
			b = data.ThreadType1Count;
			c = data.ThreadType3Count;
			d = data.ThreadType4Count;
			e = data.ThreadType6Count;
			f = data.ThreadType5Count;
			g = data.ThreadType8Count;
			h = data.ThreadType7Count;
			i = 14;
			j = 13;
			k = 12;
			l = 1;
			m = 11;
			n = 10;
			o = 9;
			p = 8;
			q = data.ThreadType2Level;
			r = data.ThreadType1Level;
			s = data.ThreadType3Level;
			t = data.ThreadType4Level;
			u = data.ThreadType6Level;
			v = data.ThreadType5Level;
			w = data.ThreadType8Level;
			x = data.ThreadType7Level;
		}
	}
}
