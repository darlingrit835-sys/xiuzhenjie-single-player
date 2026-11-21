using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DeBuffIconItem : MonoBehaviour
	{
		public string skillId;

		public Text SecondsText;

		public Image IconImg;

		public Image ZheZhaoImg;

		private Monster monster;

		private People player;

		public UnityAction CompeleteAction;

		private int currentSecond;

		private int currentTime;

		public void StartAttack(Sprite icon, BaseSkillModel baseSkill, double DamageValue, LifeType targetType)
		{
			skillId = baseSkill.id;
			Singleton<UIManager>.Instance.FindWndByName<ZhanDouWnd>("ZhanDou.prefab");
			IconImg.sprite = icon;
			currentSecond = 0;
			currentTime = 0;
			int num = (int)((double)baseSkill.continued / 1000.0);
			StartCoroutine(EveryAttack(num, 10, DamageValue, (WuXing)baseSkill.fiveElement, targetType));
			StartCoroutine(EverySeconds(num));
		}

		private IEnumerator EverySeconds(int totalTimes)
		{
			if (currentSecond == 0)
			{
				totalTimes = 30;
			}
			float addValue = 1f / (float)totalTimes;
			while (currentSecond < totalTimes)
			{
				SecondsText.text = (totalTimes - currentSecond).ToString();
				ZheZhaoImg.fillAmount += addValue;
				yield return new WaitForSeconds(1f);
				currentSecond++;
			}
		}

		private IEnumerator EveryAttack(int seconds, int times, double damage, WuXing wuXing, LifeType targetType)
		{
			if (seconds == 0)
			{
				seconds = 30;
			}
			float JianGe = seconds / times;
			double everyTimeDamage = damage / (double)times;
			while (currentTime < times)
			{
				switch (targetType)
				{
				case LifeType.People:
					player.UnderAttacked(everyTimeDamage, wuXing, SkillType.DOT, SkillGroupType.FaShu);
					break;
				case LifeType.Monster:
					monster.UnderAttacked(everyTimeDamage, wuXing, SkillType.DOT, SkillGroupType.FaShu);
					break;
				}
				yield return new WaitForSeconds(JianGe);
				currentTime++;
			}
			if (CompeleteAction != null)
			{
				CompeleteAction();
			}
			Object.Destroy(base.gameObject);
		}
	}
}
