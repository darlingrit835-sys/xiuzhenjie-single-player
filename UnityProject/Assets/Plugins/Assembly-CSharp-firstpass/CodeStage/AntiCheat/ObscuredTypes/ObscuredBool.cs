using System;
using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredBool : IEquatable<ObscuredBool>, IComparable<ObscuredBool>, IComparable<bool>, IComparable
	{
		private static byte cryptoKey = 215;

		[SerializeField]
		private byte currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private bool fakeValue;

		[SerializeField]
		[FormerlySerializedAs("fakeValueChanged")]
		private bool fakeValueActive;

		private ObscuredBool(bool value)
		{
			currentCryptoKey = cryptoKey;
			hiddenValue = Encrypt(value);
			bool existsAndIsRunning = ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = existsAndIsRunning && value;
			fakeValueActive = existsAndIsRunning;
			inited = true;
		}

		public static void SetNewCryptoKey(byte newKey)
		{
			cryptoKey = newKey;
		}

		public static int Encrypt(bool value)
		{
			return Encrypt(value, 0);
		}

		public static int Encrypt(bool value, byte key)
		{
			if (key == 0)
			{
				key = cryptoKey;
			}
			return (value ? 213 : 181) ^ key;
		}

		public static bool Decrypt(int value)
		{
			return Decrypt(value, 0);
		}

		public static bool Decrypt(int value, byte key)
		{
			if (key == 0)
			{
				key = cryptoKey;
			}
			value ^= key;
			return value != 181;
		}

		public static ObscuredBool FromEncrypted(int encrypted)
		{
			ObscuredBool result = default;
			result.SetEncrypted(encrypted);
			return result;
		}

		public void ApplyNewCryptoKey()
		{
			if (currentCryptoKey != cryptoKey)
			{
				hiddenValue = Encrypt(InternalDecrypt(), cryptoKey);
				currentCryptoKey = cryptoKey;
			}
		}

		public void RandomizeCryptoKey()
		{
			bool value = InternalDecrypt();
			currentCryptoKey = (byte)ThreadSafeRandom.Next(150);
			hiddenValue = Encrypt(value, currentCryptoKey);
		}

		public int GetEncrypted()
		{
			ApplyNewCryptoKey();
			return hiddenValue;
		}

		public void SetEncrypted(int encrypted)
		{
			inited = true;
			hiddenValue = encrypted;
			if (currentCryptoKey == 0)
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

		public bool GetDecrypted()
		{
			return InternalDecrypt();
		}

		private bool InternalDecrypt()
		{
			if (!inited)
			{
				currentCryptoKey = cryptoKey;
				hiddenValue = Encrypt(value: false);
				fakeValue = false;
				fakeValueActive = false;
				inited = true;
				return false;
			}
			bool flag = (hiddenValue ^ currentCryptoKey) != 181;
			if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && flag != fakeValue)
			{
				ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}
			return flag;
		}

		public static implicit operator ObscuredBool(bool value)
		{
			return new ObscuredBool(value);
		}

		public static implicit operator bool(ObscuredBool value)
		{
			return value.InternalDecrypt();
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ObscuredBool))
			{
				return false;
			}
			return Equals((ObscuredBool)obj);
		}

		public bool Equals(ObscuredBool obj)
		{
			if (currentCryptoKey == obj.currentCryptoKey)
			{
				return hiddenValue == obj.hiddenValue;
			}
			return Decrypt(hiddenValue, currentCryptoKey) == Decrypt(obj.hiddenValue, obj.currentCryptoKey);
		}

		public int CompareTo(ObscuredBool other)
		{
			return InternalDecrypt().CompareTo(other.InternalDecrypt());
		}

		public int CompareTo(bool other)
		{
			return InternalDecrypt().CompareTo(other);
		}

		public int CompareTo(object obj)
		{
			return InternalDecrypt().CompareTo(obj);
		}
	}
}
