using System.Collections.Generic;
using UnityEngine;

namespace XiuZhenJie
{
	public class PeopleMonster_HighOrder_Entity : PeopleMonster_Normal_Entity
	{
		private Dictionary<string, DefenceWuQi_Item> m_DefenceWuQiDic = new Dictionary<string, DefenceWuQi_Item>();

		public override void SetCtrl(Controller ctrl, string id, Vector3 locationVector3)
		{
			base.SetCtrl(ctrl, id, locationVector3);
			BaseController.transform.localScale = new Vector3(2f, 2f, 2f);
		}

		public override void FaQiInit(BaseSkillModel _playerFaQiModel)
		{
			if (FlyWuQiDic.ContainsKey(_playerFaQiModel.id))
			{
				FlyWuQiDic[_playerFaQiModel.id].Cancle(_isDestory: true);
				FlyWuQiDic.Remove(_playerFaQiModel.id);
			}
			else if (m_DefenceWuQiDic.ContainsKey(_playerFaQiModel.id))
			{
				m_DefenceWuQiDic.Remove(_playerFaQiModel.id);
				if (m_DefenceWuQiDic.Count <= 0)
				{
					BaseController.CancleShowDefenceEquipmentEffect();
				}
			}
			else if (_playerFaQiModel.skillType % 1000 == 15)
			{
				AttackFaQiInit(_playerFaQiModel);
			}
			else if (_playerFaQiModel.skillType % 1000 == 26)
			{
				m_DefenceWuQiDic.Add(_playerFaQiModel.id, new DefenceWuQi_Item
				{
					Weapon_Id = _playerFaQiModel.id
				});
				BaseController.ShowDefenceEquipmentEffect();
			}
		}

		private void AttackFaQiInit(BaseSkillModel _playerFaQiModel)
		{
			m_GongJiFaQi_InsObj = BattleMgr.m_BattleSceneResourceManager.FaQiInsObj;
			GameObject gameObject = Object.Instantiate(m_GongJiFaQi_InsObj);
			WuQi_Item component = gameObject.GetComponent<WuQi_Item>();
			float num = 2 * (FlyWuQiDic.Count + 1) + 2;
			gameObject.transform.position = new Vector3(BaseController.gameObject.transform.position.x + num, BaseController.gameObject.transform.position.y, BaseController.gameObject.transform.position.z);
			component.Init(_playerFaQiModel.SkillIndex, this, m_GongFaPower * 3f, _playerFaQiModel.id, BaseController, _playerFaQiModel.baseDamage, (WuXing)_playerFaQiModel.fiveElement, 4f, FlyWuQiDic.Count + 1, BattleMgr.m_BattleSceneResourceManager.FaQiDic);
			component.InitDamageAdd(1.0, 1.0, 1.0, 1.0, 1.0);
			if (!FlyWuQiDic.ContainsKey(_playerFaQiModel.id))
			{
				FlyWuQiDic.Add(_playerFaQiModel.id, component);
			}
			if (m_Target != null)
			{
				component.SetTarget(m_Target);
			}
		}

		private double GetDamageAfterDefenceFaQi(double _damage, int _fiveElement)
		{
			double num = 0.0;
			double num2 = 0.0;
			using (Dictionary<string, DefenceWuQi_Item>.Enumerator enumerator = m_DefenceWuQiDic.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<string, DefenceWuQi_Item> current = enumerator.Current;
					_damage -= _damage * num2;
					num = current.Value.GetDefenceDamage(_damage, _fiveElement);
					if ((double)m_BattleModel.CurrentSuperAir >= num)
					{
						BattleLifeModel battleModel = m_BattleModel;
						battleModel.CurrentSuperAir = (double)battleModel.CurrentSuperAir - num;
						return 0.0;
					}
					_ = current.Value;
					_damage *= (num - (double)m_BattleModel.CurrentSuperAir) / num;
					m_BattleModel.CurrentSuperAir = 0.0;
					BattleLifeModel battleModel2 = m_BattleModel;
					battleModel2.CurrentPerception = (double)battleModel2.CurrentPerception + (double)(int)current.Value.DataSource.usedPerception;
					m_BattleModel.CurrentPerception = (((double)m_BattleModel.CurrentPerception > (double)m_BattleModel.MaxPerception) ? m_BattleModel.MaxPerception : m_BattleModel.CurrentPerception);
				}
			}
			m_DefenceWuQiDic.Clear();
			BaseController.CancleShowDefenceEquipmentEffect();
			return _damage;
		}

		public override double UnderAttack(int skillIndex, EntityBase _entityBase, double _damage, int _fiveElement)
		{
			double num = _damage;
			_damage = GetDamageAfterDefenceFaQi(_damage, _fiveElement);
			if (_damage <= 0.0)
			{
				if (num > 0.0 && _entityBase.Entity_Type == EntityType.Player)
				{
					UnderAttackSendMsg(skillIndex, num, _entityBase.BattleModel.ObjectIndex);
				}
				BaseController.ShowStateText("吸收");
				return 0.0;
			}
			return base.UnderAttack(skillIndex, _entityBase, _damage, _fiveElement);
		}
	}
}
