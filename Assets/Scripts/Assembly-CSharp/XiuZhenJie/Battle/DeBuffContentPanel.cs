using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class DeBuffContentPanel : MonoBehaviour
	{
		private GameObject iconObj;

		public Transform monsterDeBuffContent;

		public Transform playerDeBuffContent;

		private Dictionary<string, DeBuffIconItem> MonsterIconDic = new Dictionary<string, DeBuffIconItem>();

		private Dictionary<string, DeBuffIconItem> PeopleIconDic = new Dictionary<string, DeBuffIconItem>();

		private void Start()
		{
			iconObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DeBuffIconItem.prefab");
		}

		public void SetNewIcon(Sprite icon, LifeType lifeType, BaseSkillModel skillModel, double DamageValue)
		{
			switch (lifeType)
			{
			case LifeType.People:
				if (PeopleIconDic.ContainsKey(skillModel.id))
				{
					UnityEngine.Object.Destroy(PeopleIconDic[skillModel.id].gameObject);
					PeopleIconDic.Remove(skillModel.id);
				}
				SetPeopleDeBuff(icon, skillModel, DamageValue);
				break;
			case LifeType.Monster:
				if (MonsterIconDic.ContainsKey(skillModel.id))
				{
					UnityEngine.Object.Destroy(MonsterIconDic[skillModel.id].gameObject);
					MonsterIconDic.Remove(skillModel.id);
				}
				SetMonsterDeBuff(icon, skillModel, DamageValue);
				break;
			}
		}

		private void SetMonsterDeBuff(Sprite icon, BaseSkillModel baseSkill, double DamageValue)
		{
			DeBuffIconItem component = UnityEngine.Object.Instantiate(iconObj, monsterDeBuffContent).GetComponent<DeBuffIconItem>();
			InkInterfaceArt.Apply(component.gameObject, windowName: "BattleWnd", isListItem: true);
			component.CompeleteAction = (UnityAction)Delegate.Combine(component.CompeleteAction, (UnityAction)(() =>
			{
				if (MonsterIconDic.ContainsKey(baseSkill.id))
				{
					MonsterIconDic.Remove(baseSkill.id);
				}
			}));
			component.StartAttack(icon, baseSkill, DamageValue, LifeType.Monster);
			MonsterIconDic.Add(baseSkill.id, component);
		}

		private void SetPeopleDeBuff(Sprite icon, BaseSkillModel baseSkill, double DamageValue)
		{
			DeBuffIconItem component = UnityEngine.Object.Instantiate(iconObj, playerDeBuffContent).GetComponent<DeBuffIconItem>();
			InkInterfaceArt.Apply(component.gameObject, windowName: "BattleWnd", isListItem: true);
			component.CompeleteAction = (UnityAction)Delegate.Combine(component.CompeleteAction, (UnityAction)(() =>
			{
				if (PeopleIconDic.ContainsKey(baseSkill.id))
				{
					PeopleIconDic.Remove(baseSkill.id);
				}
			}));
			component.StartAttack(icon, baseSkill, DamageValue, LifeType.People);
			PeopleIconDic.Add(baseSkill.id, component);
		}
	}
}
