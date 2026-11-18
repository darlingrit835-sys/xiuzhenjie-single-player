using UnityEngine;

namespace XiuZhenJie
{
	public class SkillManager : MonoBehaviour
	{
		private ResourceManager m_ResMgr;

		public void Init()
		{
			m_ResMgr = Singleton<ResourceManager>.Instance;
		}

		public void SkillAttack(EntityBase entityBase, string skillId)
		{
		}

		public void AttackDamage()
		{
		}

		public void SkillAction()
		{
		}

		public void AttackEffect()
		{
		}
	}
}
