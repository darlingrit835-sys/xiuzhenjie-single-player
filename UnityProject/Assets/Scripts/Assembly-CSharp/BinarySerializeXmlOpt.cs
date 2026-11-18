using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class BinarySerializeXmlOpt
{
	public static bool Xmlserialize(string path, object obj)
	{
		try
		{
			using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
				new XmlSerializer(obj.GetType()).Serialize(textWriter, obj);
			}
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogError("此类无法转换成xml " + obj.GetType()?.ToString() + "," + ex);
		}
		return false;
	}

	public static T XmlDeserialize<T>(string path) where T : class
	{
		T result = null;
		try
		{
			using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
			result = (T)new XmlSerializer(typeof(T)).Deserialize(stream);
			return result;
		}
		catch (Exception ex)
		{
			Debug.LogError("此xml无法转成二进制: " + path + "," + ex);
		}
		return result;
	}

	public static object XmlDeserialize(string path, Type type)
	{
		object result = null;
		try
		{
			using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
			result = new XmlSerializer(type).Deserialize(stream);
		}
		catch (Exception ex)
		{
			Debug.LogError("此xml无法转成二进制: " + path + "," + ex);
		}
		return result;
	}

	public static T XmlDeserializeRun<T>(string path) where T : class
	{
		T result = null;
		TextAsset textAsset = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>(path);
		if (textAsset == null)
		{
			Debug.LogError("cant load TextAsset: " + path);
			return null;
		}
		try
		{
			using (MemoryStream stream = new MemoryStream(textAsset.bytes))
			{
				result = (T)new XmlSerializer(typeof(T)).Deserialize(stream);
			}
			Singleton<ResourceManager>.Instance.ReleaseResouce(path, destoryObj: true);
		}
		catch (Exception ex)
		{
			Debug.LogError("load TextAsset exception: " + path + "," + ex);
		}
		return result;
	}

	public static bool BinarySerilize(string path, object obj)
	{
		try
		{
			using (FileStream serializationStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				new BinaryFormatter().Serialize(serializationStream, obj);
			}
			return true;
		}
		catch (Exception ex)
		{
			Debug.LogError("此类无法转换成二进制 " + obj.GetType()?.ToString() + "," + ex);
		}
		return false;
	}

	public static T BinaryDeserilize<T>(string path) where T : class
	{
		T result = null;
		TextAsset textAsset = Singleton<ResourceManager>.Instance.LoadResource<TextAsset>(path);
		if (textAsset == null)
		{
			Debug.LogError("cant load TextAsset: " + path);
			return null;
		}
		try
		{
			using (MemoryStream serializationStream = new MemoryStream(textAsset.bytes))
			{
				result = (T)new BinaryFormatter().Deserialize(serializationStream);
			}
			Singleton<ResourceManager>.Instance.ReleaseResouce(path, destoryObj: true);
		}
		catch (Exception ex)
		{
			Debug.LogError("load TextAsset exception: " + path + "," + ex);
		}
		return result;
	}
}
