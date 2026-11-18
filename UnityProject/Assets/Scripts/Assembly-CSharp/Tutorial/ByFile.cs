using UnityEngine;
using XLua;

namespace Tutorial
{
	public class ByFile : MonoBehaviour
	{
		private LuaEnv luaenv;

		private void Start()
		{
			luaenv = new LuaEnv();
			luaenv.DoString("require 'byfile'");
		}

		private void Update()
		{
			if (luaenv != null)
			{
				luaenv.Tick();
			}
		}

		private void OnDestroy()
		{
			luaenv.Dispose();
		}
	}
}
