using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class ChuangZhaoGongFaSceneManager : MonoSingleton<ChuangZhaoGongFaSceneManager>
	{
		public GameObject BallIntanceObj;

		public List<Vector3> m_DataSource;

		public List<ChuangZhaoGongFaLightItem> m_ChuangZhaoFaList;

		public void SetPoint(List<Vector3> PointList)
		{
			m_DataSource = PointList;
			m_ChuangZhaoFaList = new List<ChuangZhaoGongFaLightItem>();
			for (int i = 0; i < PointList.Count; i++)
			{
				ChuangZhaoGongFaLightItem component = Object.Instantiate(BallIntanceObj, PointList[i], new Quaternion(0f, 0f, 0f, 0f)).GetComponent<ChuangZhaoGongFaLightItem>();
				if (PointList.Count > i + 1)
				{
					component.SetLine(i, PointList[i + 1], StepCompelete);
				}
				else
				{
					component.SetLine(i, PointList[i], StepCompelete, isEnd: true);
				}
				m_ChuangZhaoFaList.Add(component);
			}
		}

		public void ClearData()
		{
			m_DataSource.Clear();
			foreach (ChuangZhaoGongFaLightItem chuangZhaoFa in m_ChuangZhaoFaList)
			{
				Object.Destroy(chuangZhaoFa.gameObject);
			}
			m_ChuangZhaoFaList.Clear();
		}

		public void StartLine()
		{
			if (m_ChuangZhaoFaList.Count > 0)
			{
				m_ChuangZhaoFaList[0].gameObject.SetActive(value: true);
				m_ChuangZhaoFaList[0].StartLine();
			}
		}

		private void StepCompelete(int index)
		{
			if (m_ChuangZhaoFaList.Count > index + 2)
			{
				m_ChuangZhaoFaList[index + 1].gameObject.SetActive(value: true);
				m_ChuangZhaoFaList[index + 1].StartLine();
			}
			else
			{
				m_ChuangZhaoFaList[index + 1].gameObject.SetActive(value: true);
				Singleton<UIManager>.Instance.FindWndByName<ChuangZhaoGongFaWnd>("ChuangZhaoGongFaWnd.prefab")?.ChuangZhaoCompelete();
			}
		}
	}
}
