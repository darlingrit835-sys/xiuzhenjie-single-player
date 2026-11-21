using System.Collections.Generic;
using UnityEngine;

public class ConfigerManager : Singleton<ConfigerManager>
{
	protected Dictionary<string, ExcelBase> m_AllExcelData = new Dictionary<string, ExcelBase>();

	public T LoadData<T>(string path) where T : ExcelBase
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		if (m_AllExcelData.ContainsKey(path))
		{
			Debug.LogError("重复加载相同配置文件" + path);
			return m_AllExcelData[path] as T;
		}
		T val = BinarySerializeXmlOpt.BinaryDeserilize<T>(path);
		val?.Init();
		m_AllExcelData.Add(path, val);
		return val;
	}

	public T FindData<T>(string path) where T : ExcelBase
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		ExcelBase value = null;
		if (m_AllExcelData.TryGetValue(path, out value))
		{
			return value as T;
		}
		value = LoadData<T>(path);
		return (T)value;
	}
}
