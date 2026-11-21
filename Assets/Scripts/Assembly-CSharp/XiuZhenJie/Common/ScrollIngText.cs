using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ScrollIngText : MonoBehaviour
	{
		private float speed = 1f;

		private float timer;

		private float time = 5f;

		private void Update()
		{
			Scroll();
		}

		private void Scroll()
		{
			base.transform.Translate(Vector3.up * speed * Time.deltaTime);
			timer += Time.deltaTime;
			GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f - timer / 3f);
			GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, 1f - timer / 3f);
			Object.Destroy(base.gameObject, time);
		}
	}
}
