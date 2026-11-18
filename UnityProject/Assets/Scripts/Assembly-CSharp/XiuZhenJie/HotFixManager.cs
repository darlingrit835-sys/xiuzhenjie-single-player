using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using XLua;

namespace XiuZhenJie
{
	public class HotFixManager : Singleton<HotFixManager>
	{
		private LuaEnv luaEnv;

		public TextAsset text;

		public void Init()
		{
			if (luaEnv == null)
			{
				luaEnv = new LuaEnv();
				luaEnv.AddLoader(MyLoader);
			}
			luaEnv.DoString("require'XzjHotFix'");
		}

		private byte[] MyLoader(ref string filePath)
		{
			string path = "Assets/GameData/LuaTxt/" + filePath + ".lua.txt";
			return Singleton<ResourceManager>.Instance.LoadResource<TextAsset>(path).bytes;
		}

		private void OnDisable()
		{
			luaEnv.DoString("require'XzjHotFixDispose'");
		}

		private void Destory()
		{
			luaEnv?.Dispose();
		}

		public void OnUpdate()
		{
			luaEnv?.Tick();
		}

		public object ChangeAnyType<T>(T a) where T : BaseModel
		{
			return a;
		}

		public string ObjectToString(object a)
		{
			return a.ToString();
		}

		public string ObscuredDoubleToString(ObscuredDouble value)
		{
			return value.ToString();
		}

		public string ObscuredStringToString(ObscuredString value)
		{
			return value.ToString();
		}

		public string ObscuredIntToString(ObscuredInt value)
		{
			return value.ToString();
		}

		public string ObscuredBoolToString(ObscuredBool value)
		{
			return value.ToString();
		}
	}
}
