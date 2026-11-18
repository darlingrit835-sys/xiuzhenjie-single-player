using System;
using System.Collections.Generic;
using System.Threading;
using XLua.LuaDLL;

namespace XLua
{
	public class DelegateBridge : DelegateBridgeBase
	{
		internal static DelegateBridge[] DelegateBridgeList = new DelegateBridge[0];

		public static bool Gen_Flag = false;

		private int _oldTop;

		private Stack<int> _stack = new Stack<int>();

		public DelegateBridge(int reference, LuaEnv luaenv)
			: base(reference, luaenv)
		{
		}

		public void PCall(IntPtr L, int nArgs, int nResults, int errFunc)
		{
			if (Lua.lua_pcall(L, nArgs, nResults, errFunc) != 0)
			{
				luaEnv.ThrowExceptionFromError(errFunc - 1);
			}
		}

		public void InvokeSessionStart()
		{
			Monitor.Enter(luaEnv.luaEnvLock);
			IntPtr l = luaEnv.L;
			_stack.Push(_oldTop);
			_oldTop = Lua.lua_gettop(l);
			Lua.load_error_func(l, luaEnv.errorFuncRef);
			Lua.lua_getref(l, luaReference);
		}

		public void Invoke(int nRet)
		{
			if (Lua.lua_pcall(luaEnv.L, Lua.lua_gettop(luaEnv.L) - _oldTop - 2, nRet, _oldTop + 1) != 0)
			{
				int oldTop = _oldTop;
				_oldTop = _stack.Pop();
				Monitor.Exit(luaEnv.luaEnvLock);
				luaEnv.ThrowExceptionFromError(oldTop);
			}
		}

		public void InvokeSessionEnd()
		{
			Lua.lua_settop(luaEnv.L, _oldTop);
			_oldTop = _stack.Pop();
			Monitor.Exit(luaEnv.luaEnvLock);
		}

		public TResult InvokeSessionEndWithResult<TResult>()
		{
			if (Lua.lua_gettop(luaEnv.L) < _oldTop + 2)
			{
				InvokeSessionEnd();
				throw new InvalidOperationException("no result!");
			}
			try
			{
				luaEnv.translator.Get(luaEnv.L, _oldTop + 2, out TResult v);
				return v;
			}
			finally
			{
				InvokeSessionEnd();
			}
		}

		public void InParam<T>(T p)
		{
			try
			{
				luaEnv.translator.PushByType(luaEnv.L, p);
			}
			catch (Exception ex)
			{
				InvokeSessionEnd();
				throw ex;
			}
		}

		public void InParams<T>(T[] ps)
		{
			try
			{
				for (int i = 0; i < ps.Length; i++)
				{
					luaEnv.translator.PushByType(luaEnv.L, ps[i]);
				}
			}
			catch (Exception ex)
			{
				InvokeSessionEnd();
				throw ex;
			}
		}

		public void OutParam<TResult>(int pos, out TResult ret)
		{
			if (Lua.lua_gettop(luaEnv.L) < _oldTop + 2 + pos)
			{
				InvokeSessionEnd();
				throw new InvalidOperationException("no result in " + pos);
			}
			try
			{
				luaEnv.translator.Get(luaEnv.L, _oldTop + 2 + pos, out ret);
			}
			catch (Exception ex)
			{
				InvokeSessionEnd();
				throw ex;
			}
		}

		public void Action()
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				if (Lua.lua_pcall(l, 0, 0, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				Lua.lua_settop(l, num);
			}
		}

		public void Action<T1>(T1 p1)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				if (Lua.lua_pcall(l, 1, 0, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				Lua.lua_settop(l, num);
			}
		}

		public void Action<T1, T2>(T1 p1, T2 p2)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				translator.PushByType(l, p2);
				if (Lua.lua_pcall(l, 2, 0, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				Lua.lua_settop(l, num);
			}
		}

		public void Action<T1, T2, T3>(T1 p1, T2 p2, T3 p3)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				translator.PushByType(l, p2);
				translator.PushByType(l, p3);
				if (Lua.lua_pcall(l, 3, 0, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				Lua.lua_settop(l, num);
			}
		}

		public void Action<T1, T2, T3, T4>(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				translator.PushByType(l, p2);
				translator.PushByType(l, p3);
				translator.PushByType(l, p4);
				if (Lua.lua_pcall(l, 4, 0, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				Lua.lua_settop(l, num);
			}
		}

		public TResult Func<TResult>()
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				if (Lua.lua_pcall(l, 0, 1, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				TResult v;
				try
				{
					translator.Get(l, -1, out v);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					Lua.lua_settop(l, num);
				}
				return v;
			}
		}

		public TResult Func<T1, TResult>(T1 p1)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				if (Lua.lua_pcall(l, 1, 1, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				TResult v;
				try
				{
					translator.Get(l, -1, out v);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					Lua.lua_settop(l, num);
				}
				return v;
			}
		}

		public TResult Func<T1, T2, TResult>(T1 p1, T2 p2)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				translator.PushByType(l, p2);
				if (Lua.lua_pcall(l, 2, 1, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				TResult v;
				try
				{
					translator.Get(l, -1, out v);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					Lua.lua_settop(l, num);
				}
				return v;
			}
		}

		public TResult Func<T1, T2, T3, TResult>(T1 p1, T2 p2, T3 p3)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				translator.PushByType(l, p2);
				translator.PushByType(l, p3);
				if (Lua.lua_pcall(l, 3, 1, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				TResult v;
				try
				{
					translator.Get(l, -1, out v);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					Lua.lua_settop(l, num);
				}
				return v;
			}
		}

		public TResult Func<T1, T2, T3, T4, TResult>(T1 p1, T2 p2, T3 p3, T4 p4)
		{
			lock (luaEnv.luaEnvLock)
			{
				IntPtr l = luaEnv.L;
				ObjectTranslator translator = luaEnv.translator;
				int num = Lua.lua_gettop(l);
				int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
				Lua.lua_getref(l, luaReference);
				translator.PushByType(l, p1);
				translator.PushByType(l, p2);
				translator.PushByType(l, p3);
				translator.PushByType(l, p4);
				if (Lua.lua_pcall(l, 4, 1, errfunc) != 0)
				{
					luaEnv.ThrowExceptionFromError(num);
				}
				TResult v;
				try
				{
					translator.Get(l, -1, out v);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				finally
				{
					Lua.lua_settop(l, num);
				}
				return v;
			}
		}
	}
}
