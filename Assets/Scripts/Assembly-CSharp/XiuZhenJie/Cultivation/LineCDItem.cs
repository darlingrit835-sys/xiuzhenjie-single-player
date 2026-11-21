using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class LineCDItem : MonoBehaviour
	{
		public List<GameObject> m_AniObjs;

		private int i;

		private float currentTime;

		private void Update()
		{
			if (m_AniObjs == null)
			{
				return;
			}
			currentTime += Time.deltaTime;
			if (currentTime >= 0.05f)
			{
				currentTime = 0f;
				if (i == 14)
				{
					m_AniObjs[13].gameObject.SetActive(value: false);
					i = 0;
				}
				if (i - 1 >= 0)
				{
					m_AniObjs[i - 1].gameObject.SetActive(value: false);
				}
				m_AniObjs[i].gameObject.SetActive(value: true);
				i++;
			}
		}
	}
}
