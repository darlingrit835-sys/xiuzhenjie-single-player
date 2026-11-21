using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WaiRequestPanel : MonoBehaviour
	{
		public Image img;

		public Image blackImg;

		public List<Sprite> aniSprite;

		private int i;

		private float time;

		private void OnEnable()
		{
			blackImg.gameObject.SetActive(value: false);
			time = 0f;
			i = 0;
		}

		private void Update()
		{
			time += Time.deltaTime;
			base.transform.SetAsLastSibling();
			if (time > 5f)
			{
				if (!blackImg.gameObject.activeSelf)
				{
					blackImg.gameObject.SetActive(value: true);
				}
				img.sprite = aniSprite[i];
				i++;
				if (i >= 49)
				{
					i = 0;
				}
			}
		}

		private void OnDisable()
		{
			blackImg.gameObject.SetActive(value: false);
		}
	}
}
