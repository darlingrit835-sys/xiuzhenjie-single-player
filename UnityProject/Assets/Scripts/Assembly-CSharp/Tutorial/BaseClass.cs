using UnityEngine;
using XLua;

namespace Tutorial
{
	[LuaCallCSharp(GenFlag.No)]
	public class BaseClass
	{
		public static int BSF = 1;

		public int BMF { get; set; }

		public static void BSFunc()
		{
			Debug.Log("Derived Static Func, BSF = " + BSF);
		}

		public void BMFunc()
		{
			Debug.Log("Derived Member Func, BMF = " + BMF);
		}
	}
}
