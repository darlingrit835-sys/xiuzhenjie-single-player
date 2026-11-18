using UnityEngine;

namespace XiuZhenJie
{
	public abstract class BaseItemObj : MonoBehaviour
	{
		private EntityType m_FromType;

		private AttackMoudleType m_AttackMoudle;

		protected GameObject m_TargetObj;

		protected BattleSceneManager m_BattleSceneManager;

		public EntityType FromType
		{
			get
			{
				return m_FromType;
			}
			set
			{
				m_FromType = value;
			}
		}

		public AttackMoudleType AttackMoudle
		{
			get
			{
				return m_AttackMoudle;
			}
			set
			{
				m_AttackMoudle = value;
			}
		}

		public virtual void Init(BattleSceneManager _battleManager, EntityType _targetType, AttackMoudleType _attackMoudle, GameObject _targetObj = null)
		{
			m_BattleSceneManager = _battleManager;
			AttackMoudle = _attackMoudle;
			if (_targetType == EntityType.Monster)
			{
				m_FromType = EntityType.Player;
			}
			else
			{
				m_FromType = EntityType.Monster;
			}
			if (_targetObj != null)
			{
				m_TargetObj = _targetObj;
			}
		}
	}
}
