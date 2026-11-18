using System;
using XLua.LuaDLL;

namespace XLua
{
	public static class CopyByValue
	{
		public static bool Pack(IntPtr buff, int offset, byte field)
		{
			return Lua.xlua_pack_int8_t(buff, offset, field);
		}

		public static bool UnPack(IntPtr buff, int offset, out byte field)
		{
			return Lua.xlua_unpack_int8_t(buff, offset, out field);
		}

		public static bool Pack(IntPtr buff, int offset, sbyte field)
		{
			return Lua.xlua_pack_int8_t(buff, offset, (byte)field);
		}

		public static bool UnPack(IntPtr buff, int offset, out sbyte field)
		{
			bool result = Lua.xlua_unpack_int8_t(buff, offset, out var field2);
			field = (sbyte)field2;
			return result;
		}

		public static bool Pack(IntPtr buff, int offset, short field)
		{
			return Lua.xlua_pack_int16_t(buff, offset, field);
		}

		public static bool UnPack(IntPtr buff, int offset, out short field)
		{
			return Lua.xlua_unpack_int16_t(buff, offset, out field);
		}

		public static bool Pack(IntPtr buff, int offset, ushort field)
		{
			return Lua.xlua_pack_int16_t(buff, offset, (short)field);
		}

		public static bool UnPack(IntPtr buff, int offset, out ushort field)
		{
			bool result = Lua.xlua_unpack_int16_t(buff, offset, out var field2);
			field = (ushort)field2;
			return result;
		}

		public static bool Pack(IntPtr buff, int offset, int field)
		{
			return Lua.xlua_pack_int32_t(buff, offset, field);
		}

		public static bool UnPack(IntPtr buff, int offset, out int field)
		{
			return Lua.xlua_unpack_int32_t(buff, offset, out field);
		}

		public static bool Pack(IntPtr buff, int offset, uint field)
		{
			return Lua.xlua_pack_int32_t(buff, offset, (int)field);
		}

		public static bool UnPack(IntPtr buff, int offset, out uint field)
		{
			bool result = Lua.xlua_unpack_int32_t(buff, offset, out var field2);
			field = (uint)field2;
			return result;
		}

		public static bool Pack(IntPtr buff, int offset, long field)
		{
			return Lua.xlua_pack_int64_t(buff, offset, field);
		}

		public static bool UnPack(IntPtr buff, int offset, out long field)
		{
			return Lua.xlua_unpack_int64_t(buff, offset, out field);
		}

		public static bool Pack(IntPtr buff, int offset, ulong field)
		{
			return Lua.xlua_pack_int64_t(buff, offset, (long)field);
		}

		public static bool UnPack(IntPtr buff, int offset, out ulong field)
		{
			bool result = Lua.xlua_unpack_int64_t(buff, offset, out var field2);
			field = (ulong)field2;
			return result;
		}

		public static bool Pack(IntPtr buff, int offset, float field)
		{
			return Lua.xlua_pack_float(buff, offset, field);
		}

		public static bool UnPack(IntPtr buff, int offset, out float field)
		{
			return Lua.xlua_unpack_float(buff, offset, out field);
		}

		public static bool Pack(IntPtr buff, int offset, double field)
		{
			return Lua.xlua_pack_double(buff, offset, field);
		}

		public static bool UnPack(IntPtr buff, int offset, out double field)
		{
			return Lua.xlua_unpack_double(buff, offset, out field);
		}

		public static bool Pack(IntPtr buff, int offset, decimal field)
		{
			return Lua.xlua_pack_decimal(buff, offset, ref field);
		}

		public static bool UnPack(IntPtr buff, int offset, out decimal field)
		{
			if (!Lua.xlua_unpack_decimal(buff, offset, out var scale, out var sign, out var hi, out var lo))
			{
				field = 0m;
				return false;
			}
			field = new decimal((int)(lo & 0xFFFFFFFFu), (int)(lo >> 32), hi, (sign & 0x80) != 0, scale);
			return true;
		}

		public static bool IsStruct(Type type)
		{
			if (type.IsValueType() && !type.IsEnum())
			{
				return !type.IsPrimitive();
			}
			return false;
		}
	}
}
