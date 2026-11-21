using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XiuZhenJie;

public class OutLog : MonoBehaviour
{
	private static List<string> mLines = new List<string>();

	private static List<string> mWriteTxt = new List<string>();

	public string outpath;

	private string m_AppId = "29cfb85094";

	private void Start()
	{
		outpath = Application.persistentDataPath + "/outLog.txt";
		if (File.Exists(outpath))
		{
			File.Delete(outpath);
		}
		BuglyAgent.InitWithAppId(m_AppId);
		BuglyAgent.EnableExceptionHandler();
		BuglyAgent.RegisterLogCallback(Application_logMessageReceived);
		BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogError);
	}

	public void Application_logMessageReceived(string condition, string stackTrace, UnityEngine.LogType _logtype)
	{
		if (condition.Contains("SSLHandshakeException"))
		{
			mWriteTxt.Add(condition);
			return;
		}
		if (_logtype == UnityEngine.LogType.Error || _logtype == UnityEngine.LogType.Exception)
		{
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("检测到游戏非预期异常，请尽快重启游戏!");
		}
		mWriteTxt.Add(condition);
	}

	private void Update()
	{
		if (mWriteTxt.Count <= 0)
		{
			return;
		}
		string[] array = mWriteTxt.ToArray();
		foreach (string text in array)
		{
			using (StreamWriter streamWriter = new StreamWriter(outpath, append: true, Encoding.UTF8))
			{
				streamWriter.WriteLine(text);
			}
			mWriteTxt.Remove(text);
		}
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
	}
}
