using System;
using System.Runtime.InteropServices;
using CodeStage.AntiCheat.Common;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredDouble : IFormattable, IEquatable<ObscuredDouble>, IComparable<ObscuredDouble>, IComparable<double>, IComparable
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct DoubleLongBytesUnion
		{
			[FieldOffset(0)]
			public double d;

			[FieldOffset(0)]
			public long l;

			[FieldOffset(0)]
			public ACTkByte8 b8;
		}

		private static long cryptoKey = 210987L;

		[SerializeField]
		private long currentCryptoKey;

		[SerializeField]
		private long hiddenValue;

		[SerializeField]
		[FormerlySerializedAs("hiddenValue")]
		private ACTkByte8 hiddenValueOldByte8;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private double fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredDouble(double value)
		{
			currentCryptoKey = cryptoKey;
			hiddenValue = InternalEncrypt(value, 0L);
			hiddenValueOldByte8 = default;
			bool existsAndIsRunning = ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = (existsAndIsRunning ? value : 0.0);
			fakeValueActive = existsAndIsRunning;
			inited = true;
		}

		public static void SetNewCryptoKey(long newKey)
		{
			cryptoKey = newKey;
		}

		public static long Encrypt(double value)
		{
			return Encrypt(value, cryptoKey);
		}

		public static long Encrypt(double value, long key)
		{
			DoubleLongBytesUnion doubleLongBytesUnion = new DoubleLongBytesUnion
			{
				d = value
			};
			doubleLongBytesUnion.l ^= key;
			doubleLongBytesUnion.b8.Shuffle();
			return doubleLongBytesUnion.l;
		}

		private static long InternalEncrypt(double value, long key = 0L)
		{
			long num = key;
			if (num == 0L)
			{
				num = cryptoKey;
			}
			return Encrypt(value, num);
		}

		public static double Decrypt(long value)
		{
			return Decrypt(value, cryptoKey);
		}

		public static double Decrypt(long value, long key)
		{
			DoubleLongBytesUnion doubleLongBytesUnion = new DoubleLongBytesUnion
			{
				l = value
			};
			doubleLongBytesUnion.b8.UnShuffle();
			doubleLongBytesUnion.l ^= key;
			return doubleLongBytesUnion.d;
		}

		public static long MigrateEncrypted(long encrypted, byte fromVersion = 0, byte toVersion = 2)
		{
			DoubleLongBytesUnion doubleLongBytesUnion = new DoubleLongBytesUnion
			{
				l = encrypted
			};
			if (fromVersion < 2 && toVersion == 2)
			{
				doubleLongBytesUnion.b8.Shuffle();
			}
			return doubleLongBytesUnion.l;
		}

		public static ObscuredDouble FromEncrypted(long encrypted)
		{
			ObscuredDouble result = default;
			result.SetEncrypted(encrypted);
			return result;
		}

		public void ApplyNewCryptoKey()
		{
			if (currentCryptoKey != cryptoKey)
			{
				hiddenValue = InternalEncrypt(InternalDecrypt(), cryptoKey);
				currentCryptoKey = cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			double value = InternalDecrypt();
			currentCryptoKey = ThreadSafeRandom.Next(100000, 999999);
			hiddenValue = InternalEncrypt(value, currentCryptoKey);
		}

		public long GetEncrypted()
		{
			ApplyNewCryptoKey();
			return hiddenValue;
		}

		public void SetEncrypted(long encrypted)
		{
			inited = true;
			hiddenValue = encrypted;
			if (currentCryptoKey == 0L)
			{
				currentCryptoKey = cryptoKey;
			}
			if (ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				fakeValueActive = false;
				fakeValue = InternalDecrypt();
				fakeValueActive = true;
			}
			else
			{
				fakeValueActive = false;
			}
		}

		public double GetDecrypted()
		{
			return InternalDecrypt();
		}

		private double InternalDecrypt()
		{
			if (!inited)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = InternalEncrypt(0.0, 0L);
				fakeValue = 0.0;
				fakeValueActive = false;
				inited = true;
				return 0.0;
			}
			double num = Decrypt(hiddenValue, currentCryptoKey);
			if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && Math.Abs(num - fakeValue) > ObscuredCheatingDetector.Instance.doubleEpsilon)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return num;
		}

		public static implicit operator ObscuredDouble(double value)
		{
			return new ObscuredDouble(value);
		}

		public static implicit operator double(ObscuredDouble value)
		{
			return value.InternalDecrypt();
		}

		public static explicit operator ObscuredDouble(ObscuredFloat f)
		{
			return (float)f;
		}

		public static ObscuredDouble operator ++(ObscuredDouble input)
		{
			double value = input.InternalDecrypt() + 1.0;
			input.hiddenValue = InternalEncrypt(value, input.currentCryptoKey);
			if (ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				input.fakeValue = value;
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}
			return input;
		}

		public static ObscuredDouble operator --(ObscuredDouble input)
		{
			double value = input.InternalDecrypt() - 1.0;
			input.hiddenValue = InternalEncrypt(value, input.currentCryptoKey);
			if (ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				input.fakeValue = value;
				input.fakeValueActive = true;
			}
			else
			{
				input.fakeValueActive = false;
			}
			return input;
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InternalDecrypt().ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return InternalDecrypt().ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return InternalDecrypt().ToString(format, provider);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredDouble))
			{
				return false;
			}
			return Equals((ObscuredDouble)obj);
		}

		public bool Equals(ObscuredDouble obj)
		{
			return obj.InternalDecrypt().Equals(InternalDecrypt());
		}

		public int CompareTo(ObscuredDouble other)
		{
			return InternalDecrypt().CompareTo(other.InternalDecrypt());
		}

		public int CompareTo(double other)
		{
			return InternalDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			return InternalDecrypt().CompareTo(obj);
		}
	}
}
