using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class UnderAttackEffectItem : MonoBehaviour
	{
		private Sprite m_SelfSprite;

		private float m_PlayInterval = 0.05f;

		private float m_PlayTimint;

		private bool m_PlayOnce;

		private int m_SpriteIndex;

		public List<Sprite> aniSpriteList;

		private void Awake()
		{
			m_SelfSprite = base.gameObject.GetComponent<Image>().sprite;
			m_PlayOnce = false;
			m_SpriteIndex = 1;
		}

		private void FixedUpdate()
		{
			if (!m_PlayOnce)
			{
				return;
			}
			m_PlayTimint += Time.fixedDeltaTime;
			if (m_PlayTimint >= m_PlayInterval)
			{
				m_PlayTimint = 0f;
				if (m_SpriteIndex > aniSpriteList.Count - 1)
				{
					m_PlayOnce = false;
					base.gameObject.GetComponent<Image>().sprite = aniSpriteList[aniSpriteList.Count - 1];
					base.gameObject.SetActive(value: false);
				}
				else
				{
					base.gameObject.GetComponent<Image>().sprite = aniSpriteList[m_SpriteIndex];
					m_SpriteIndex++;
				}
			}
		}

		public bool PlayOnce()
		{
			if (m_PlayOnce)
			{
				return false;
			}
			m_SpriteIndex = 1;
			base.gameObject.SetActive(value: true);
			m_PlayOnce = true;
			return true;
		}
	}
}
