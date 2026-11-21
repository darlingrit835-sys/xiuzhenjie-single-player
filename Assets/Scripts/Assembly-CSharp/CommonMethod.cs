using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CommonMethod
{
	public static string HuoBiLongToString(long value, int trim)
	{
		string text = value.ToString();
		if (trim == 0)
		{
			return text;
		}
		string text2 = string.Empty;
		while (!string.IsNullOrEmpty(text))
		{
			int length = text.Length;
			string empty = string.Empty;
			if (text.Length > trim)
			{
				empty = text.Substring(length - trim, trim);
				text = text.Substring(0, length - trim);
			}
			else
			{
				empty = text.Substring(0, text.Length);
				text = string.Empty;
			}
			text2 = ((!string.IsNullOrEmpty(text2)) ? $"{empty},{text2}" : empty);
		}
		return text2;
	}

	public static long HuoBiStringToLong(string value)
	{
		string[] array = value.Split(',');
		string text = string.Empty;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			text += text2;
		}
		return long.Parse(text);
	}

	public static byte[] intToBytes(int value)
	{
		byte[] array = new byte[4];
		array[3] = (byte)((value >> 24) & 0xFF);
		array[2] = (byte)((value >> 16) & 0xFF);
		array[1] = (byte)((value >> 8) & 0xFF);
		array[0] = (byte)(value & 0xFF);
		return array;
	}

	public static int bytesToInt(byte[] src, int offset)
	{
		return (src[offset] & 0xFF) | ((src[offset + 1] & 0xFF) << 8) | ((src[offset + 2] & 0xFF) << 16) | ((src[offset + 3] & 0xFF) << 24);
	}

	public static string parseTimeSeconds(int t, int type = 1)
	{
		string text = "";
		if (t >= 86400)
		{
			int num = Convert.ToInt16(t / 86400);
			int num2 = Convert.ToInt16(t % 86400 / 3600);
			int num3 = Convert.ToInt16(t % 86400 % 3600 / 60);
			int num4 = Convert.ToInt16(t % 86400 % 3600 % 60);
			if (type == 0)
			{
				return num + "天" + num2 + "小时" + num3 + "分" + num4 + "秒";
			}
			return num + "天" + num2 + "小时" + num3 + "分";
		}
		if (t >= 3600)
		{
			int num2 = Convert.ToInt16(t / 3600);
			int num3 = Convert.ToInt16(t % 3600 / 60);
			int num4 = Convert.ToInt16(t % 3600 % 60);
			if (type == 0)
			{
				return num2 + "小时" + num3 + "分" + num4 + "秒";
			}
			return num2 + "小时" + num3 + "分";
		}
		if (t >= 60)
		{
			int num3 = Convert.ToInt16(t / 60);
			int num4 = Convert.ToInt16(t % 60);
			return num3 + "分" + num4 + "秒";
		}
		return (int)Convert.ToInt16(t) + "秒";
	}

	public static string xDateSeconds()
	{
		long num = 0L;
		DateTime utcNow = DateTime.UtcNow;
		long num2 = 60L;
		long num3 = 3600L;
		long num4 = 86400L;
		long num5 = 1970L;
		long[] array = new long[12]
		{
			0L, 31L, 59L, 90L, 120L, 151L, 181L, 212L, 243L, 273L,
			304L, 334L
		};
		long[] array2 = new long[2] { 365L, 366L };
		long num6 = 0L;
		num += utcNow.Second;
		num += utcNow.Minute * num2;
		num += utcNow.Hour * num3;
		num += (utcNow.Day - 1) * num4;
		num = ((!DateTime.IsLeapYear(utcNow.Year)) ? (num + array[utcNow.Month - 1] * num4) : (num + (array[utcNow.Month - 1] + 1) * num4));
		long num7 = utcNow.Year - num5;
		for (int i = 0; i < num7; i++)
		{
			if (DateTime.IsLeapYear((int)num5 + i))
			{
				num6++;
			}
		}
		return (num + ((num7 - num6) * array2[0] + num6 * array2[1]) * num4).ToString();
	}

	public static DateTime GetGMTDateTime(int Value)
	{
		long num = 2000L;
		long num2 = 1L;
		long num3 = 1L;
		long num4 = 0L;
		long num5 = 0L;
		long num6 = 0L;
		long num7 = 0L;
		long num8 = 0L;
		long num9 = 0L;
		long num10 = 0L;
		long num11 = 0L;
		num7 = Value / 31536000;
		num += num7;
		num8 = Value % 31536000 / 86400;
		int num12 = 0;
		for (int i = 2000; i < num; i++)
		{
			if (i % 4 == 0)
			{
				num12++;
			}
		}
		num9 = Value % 31536000 % 86400 / 3600;
		num4 += num9;
		num10 = Value % 31536000 % 86400 % 3600 / 60;
		num5 += num10;
		num11 = Value % 31536000 % 86400 % 3600 % 60;
		num6 += num11;
		return new DateTime((int)num, (int)num2, (int)num3, (int)num4, (int)num5, (int)num6).AddDays(num8 - num12);
	}

	public static string GetIP(ADDRESSFAM Addfam)
	{
		try
		{
			string result = "";
			List<string> allIPs = GetAllIPs(Addfam, includeDetails: false);
			if (allIPs.Count > 0)
			{
				result = allIPs[allIPs.Count - 1];
			}
			return result;
		}
		catch (Exception ex)
		{
			Debug.Log("获取本机ip出现异常:" + ex.Message);
			return "192.168.0.24";
		}
	}

	private static List<string> GetAllIPs(ADDRESSFAM Addfam, bool includeDetails)
	{
		if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
		{
			return null;
		}
		List<string> list = new List<string>();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
			{
				switch (Addfam)
				{
				case ADDRESSFAM.IPv4:
					if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						string text = unicastAddress.Address.ToString();
						if (includeDetails)
						{
							text = text + "  " + networkInterface.Description.PadLeft(6) + networkInterface.NetworkInterfaceType.ToString().PadLeft(10);
						}
						list.Add(text);
					}
					break;
				case ADDRESSFAM.IPv6:
					if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetworkV6)
					{
						list.Add(unicastAddress.Address.ToString());
					}
					break;
				}
			}
		}
		return list;
	}

	public static long GetCreatetime()
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0);
		return Convert.ToInt64((DateTime.Now - dateTime).TotalMilliseconds);
	}

	public static DateTime GetTimeStampTime(string timeStamp)
	{
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long ticks = long.Parse(timeStamp + "0000000");
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value);
	}

	public static string SecondToMinute(int time)
	{
		int num = 0;
		int num2 = 0;
		num2 = time;
		if (num2 > 60)
		{
			num = num2 / 60;
			num2 %= 60;
		}
		string obj = ((num < 10) ? ("0" + num) : num.ToString());
		string text = ((num < 10) ? ("0" + num2) : num2.ToString());
		return obj + ":" + text;
	}

	public static string SecondToMinute(long time)
	{
		long num = 0L;
		long num2 = 0L;
		num2 = time;
		if (num2 > 60)
		{
			num = num2 / 60;
			num2 %= 60;
		}
		string obj = ((num < 10) ? ("0" + num) : num.ToString());
		string text = ((num < 10) ? ("0" + num2) : num2.ToString());
		return obj + ":" + text;
	}

	public static string SecondToHour(int time)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		num3 = Convert.ToInt32(time);
		if (num3 > 60)
		{
			num2 = num3 / 60;
			num3 %= 60;
		}
		if (num2 > 60)
		{
			num = num2 / 60;
			num2 %= 60;
		}
		string arg = ((num2 < 10) ? ("0" + num2) : num2.ToString());
		string arg2 = ((num2 < 10) ? ("0" + num3) : num3.ToString());
		return $"{num}:{arg}:{arg2}";
	}

	public static string GetMD5WithString(string input)
	{
		byte[] array = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static string GetMD5WithFilePath(string filePath)
	{
		FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(inputStream)).Replace("-", "");
	}

	public static bool getTimeSpan(string timeStr, string startTime, string endTime)
	{
		TimeSpan timeOfDay = DateTime.Parse(startTime).TimeOfDay;
		TimeSpan timeOfDay2 = DateTime.Parse(endTime).TimeOfDay;
		TimeSpan timeOfDay3 = Convert.ToDateTime(timeStr).TimeOfDay;
		if (timeOfDay3 > timeOfDay && timeOfDay3 < timeOfDay2)
		{
			return true;
		}
		return false;
	}

	public static bool getTimeSpan(DateTime timeStr, string startTime, string endTime)
	{
		TimeSpan timeOfDay = DateTime.Parse(startTime).TimeOfDay;
		TimeSpan timeOfDay2 = DateTime.Parse(endTime).TimeOfDay;
		DateTime dateTime = timeStr;
		TimeSpan timeOfDay3 = dateTime.TimeOfDay;
		if (timeOfDay3 > timeOfDay && timeOfDay3 < timeOfDay2)
		{
			return true;
		}
		return false;
	}

	public static T DeepCopyByReflection<T>(T obj)
	{
		if (obj is string || obj.GetType().IsValueType)
		{
			return obj;
		}
		object obj2 = Activator.CreateInstance(obj.GetType());
		FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			try
			{
				fieldInfo.SetValue(obj2, DeepCopyByReflection(fieldInfo.GetValue(obj)));
			}
			catch
			{
			}
		}
		return (T)obj2;
	}
}
