using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Battle_BuffItem : MonoBehaviour
	{
		public Image IconSprite;

		public Image IconMaskImg;

		private float m_Time;

		private float m_CurrentTime;

		private bool m_IsBegin;

		public string SkillId;

		public int skillType;

		private UnityAction<Battle_BuffItem> m_CompeleteAction;

		public void SetItem(string id, int skillType, int fiveElement, Sprite iconSprite, float time, UnityAction<Battle_BuffItem> CompeleteAction)
		{
			this.skillType = skillType;
			SkillId = id;
			m_Time = time;
			m_CurrentTime = time;
			WorldSetting.WuXingColor((WuXing)fiveElement);
			IconSprite.sprite = iconSprite;
			if (time == -1f)
			{
				m_IsBegin = false;
			}
			else
			{
				IconMaskImg.gameObject.SetActive(value: true);
				m_IsBegin = true;
			}
			m_CompeleteAction = CompeleteAction;
		}

		public void ReflashItem()
		{
			m_CurrentTime = m_Time;
		}

		private void Update()
		{
			if (!m_IsBegin)
			{
				return;
			}
			m_CurrentTime -= Time.deltaTime;
			IconMaskImg.fillAmount = m_CurrentTime / m_Time;
			if (m_CurrentTime <= 0f)
			{
				m_IsBegin = false;
				if (m_CompeleteAction == null)
				{
					Object.Destroy(base.gameObject);
				}
				else
				{
					m_CompeleteAction(this);
				}
			}
		}
	}
}
