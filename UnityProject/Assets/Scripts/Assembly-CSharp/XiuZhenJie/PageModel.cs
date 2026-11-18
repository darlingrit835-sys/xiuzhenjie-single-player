using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class PageModel<T>
	{
		public int pageNum;

		public int pageSize;

		public int size;

		public string orderBy;

		public int total;

		public int pages;

		public List<T> list;

		public PageModel()
		{
			list = new List<T>();
		}
	}
}
