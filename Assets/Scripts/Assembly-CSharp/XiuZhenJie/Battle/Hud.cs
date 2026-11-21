using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Hud : MonoBehaviour
	{
		private float speed = 0.7f;

		private float timer;

		private float time = 4f;

		private void Update()
		{
			Scroll();
		}

		private void Scroll()
		{
			base.transform.Translate(Vector3.up * speed * Time.deltaTime);
			timer += Time.deltaTime;
			GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f - timer);
			Object.Destroy(base.gameObject, time);
		}
	}
}
