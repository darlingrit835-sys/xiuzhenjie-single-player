using System;
using UnityEngine;
using XLua;

namespace Tutorial
{
	[LuaCallCSharp(GenFlag.No)]
	public class DerivedClass : BaseClass
	{
		[LuaCallCSharp(GenFlag.No)]
		public enum TestEnumInner
		{
			E3 = 0,
			E4 = 1
		}

		private class InnerCalc : ICalc
		{
			public int id = 100;

			public int add(int a, int b)
			{
				return a + b;
			}
		}

		public Action<string> TestDelegate = (string param) =>
		{
			Debug.Log("TestDelegate in c#:" + param);
		};

		public int DMF { get; set; }

		public event Action TestEvent;

		public void DMFunc()
		{
			Debug.Log("Derived Member Func, DMF = " + DMF);
		}

		public double ComplexFunc(Param1 p1, ref int p2, out string p3, Action luafunc, out Action csfunc)
		{
			Debug.Log("P1 = {x=" + p1.x + ",y=" + p1.y + "},p2 = " + p2);
			luafunc();
			p2 *= p1.x;
			p3 = "hello " + p1.y;
			csfunc = () =>
			{
				Debug.Log("csharp callback invoked!");
			};
			return 1.23;
		}

		public void TestFunc(int i)
		{
			Debug.Log("TestFunc(int i)");
		}

		public void TestFunc(string i)
		{
			Debug.Log("TestFunc(string i)");
		}

		public static DerivedClass operator +(DerivedClass a, DerivedClass b)
		{
			return new DerivedClass
			{
				DMF = a.DMF + b.DMF
			};
		}

		public void DefaultValueFunc(int a = 100, string b = "cccc", string c = null)
		{
			Debug.Log("DefaultValueFunc: a=" + a + ",b=" + b + ",c=" + c);
		}

		public void VariableParamsFunc(int a, params string[] strs)
		{
			Debug.Log("VariableParamsFunc: a =" + a);
			foreach (string text in strs)
			{
				Debug.Log("str:" + text);
			}
		}

		public TestEnum EnumTestFunc(TestEnum e)
		{
			Debug.Log("EnumTestFunc: e=" + e);
			return TestEnum.E2;
		}

		public void CallEvent()
		{
			TestEvent();
		}

		public ulong TestLong(long n)
		{
			return (ulong)(n + 1);
		}

		public ICalc GetCalc()
		{
			return new InnerCalc();
		}

		public void GenericMethod<T>()
		{
			Debug.Log("GenericMethod<" + typeof(T)?.ToString() + ">");
		}
	}
}
