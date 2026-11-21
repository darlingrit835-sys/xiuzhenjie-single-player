using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class RoleMailInfoModel : BaseModel
	{
		public string i;

		public string title;

		public string sid;

		public string c;

		public bool s;

		public long st;

		public long rt;

		public bool h;

		public bool isGetAll;

		public List<XzjRoleLetterAwardVO> l;
	}
}
