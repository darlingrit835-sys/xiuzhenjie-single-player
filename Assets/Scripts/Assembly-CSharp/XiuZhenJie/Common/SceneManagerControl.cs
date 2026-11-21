using UnityEngine;

namespace XiuZhenJie
{
	public class SceneManagerControl : MonoBehaviour
	{
		private float m_DevHeight = 9.6f;

		private float m_DevWidth = 6.4f;

		private void Start()
		{
			_ = Screen.height;
			float orthographicSize = GetComponent<Camera>().orthographicSize;
			float num = (float)Screen.width * 1f / (float)Screen.height;
			if (orthographicSize * 2f * num < m_DevWidth)
			{
				orthographicSize = m_DevWidth / (2f * num);
				GetComponent<Camera>().orthographicSize = orthographicSize;
			}
		}
	}
}
