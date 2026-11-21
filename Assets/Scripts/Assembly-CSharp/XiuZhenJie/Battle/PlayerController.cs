using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class PlayerController : Controller
	{
		public SpriteRenderer ShenShiCircle;

		public Animator DefenceFaQiAnimator;

		private PlayerEntity m_PlayerEntity;

		public override void Init(string Id, EntityBase entityBase, UnityAction<Collider2D> _lockEnterAction, UnityAction<Collider2D> _lockExitAction, UnityAction<Collider2D> _foundEnterAction, UnityAction<Collider2D> _foundExitAction)
		{
			base.Init(Id, entityBase, _lockEnterAction, _lockExitAction, _foundEnterAction, _foundExitAction);
			DefenceFaQiAnimator = DefenceFaQiItemObj.GetComponent<Animator>();
			TouXiangSprite.sprite = Singleton<RoleManager>.Instance.GetCurrentRoleSprite();
			m_PlayerEntity = entityBase as PlayerEntity;
		}

		protected override void CtrlUpdate()
		{
			base.CtrlUpdate();
		}

		public override void ShowDefenceEquipmentEffect()
		{
			base.ShowDefenceEquipmentEffect();
			if (!DefenceFaQiItemObj.activeSelf)
			{
				DefenceFaQiItemObj.SetActive(value: true);
			}
		}

		public override void CancleShowDefenceEquipmentEffect()
		{
			base.CancleShowDefenceEquipmentEffect();
			if (DefenceFaQiAnimator == null || (DefenceFaQiItemObj != null && !DefenceFaQiItemObj.activeSelf))
			{
				return;
			}
			DefenceFaQiAnimator.SetBool("running", value: false);
			DefenceFaQiAnimator.SetBool("end", value: true);
			DefenceFaQiAnimator.SetBool("over", value: false);
			Timer.Register(1.5f, () =>
			{
				if (DefenceFaQiItemObj != null)
				{
					if (!DefenceFaQiAnimator.GetBool("running"))
					{
						DefenceFaQiAnimator.SetBool("running", value: true);
					}
					if (DefenceFaQiAnimator.GetBool("end"))
					{
						DefenceFaQiAnimator.SetBool("end", value: false);
					}
					if (DefenceFaQiAnimator.GetBool("over"))
					{
						DefenceFaQiAnimator.SetBool("over", value: false);
					}
					DefenceFaQiItemObj.SetActive(value: false);
				}
			});
		}

		public void SetCollider2D(bool IsShow)
		{
			CircleCollider2D component = TouXiangSprite.gameObject.GetComponent<CircleCollider2D>();
			if (!(component == null))
			{
				component.enabled = IsShow;
			}
		}

		public override void SetShenShiCircleScale(float scale)
		{
			base.SetShenShiCircleScale(scale);
		}

		protected override void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundEnterTriggerCheck(_collider2d);
			if (!(_collider2d.gameObject.tag != "monster") && FoundEnterAction != null)
			{
				FoundEnterAction(_collider2d);
			}
		}

		protected override void ShenShiFoundExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundExitTriggerCheck(_collider2d);
			if (!(_collider2d.gameObject.tag != "monster") && FoundExitAction != null)
			{
				FoundExitAction(_collider2d);
			}
		}

		protected override void ShenShiLockEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockEnterTriggerCheck(_collider2d);
			if (!(_collider2d.gameObject.tag != "monster") && LockEnterAction != null)
			{
				LockEnterAction(_collider2d);
			}
		}

		protected override void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockExitTriggerCheck(_collider2d);
			if (!(_collider2d.gameObject.tag != "monster") && LockExitAction != null)
			{
				LockExitAction(_collider2d);
			}
		}

		protected override void PlayerSelfEnterTriggerCheck(Collider2D _collider2d)
		{
			base.PlayerSelfEnterTriggerCheck(_collider2d);
			switch (_collider2d.gameObject.tag)
			{
			case "Plant":
			case "Mineral":
				goto case "BattleDiaoLuoGoods";
			case "MiJing":
			case "BattleDiaoLuoGoods":
			{
				if (m_PlayerEntity?.BattleMgr.Interaction != null && !_collider2d.CompareTag("MiJing")) return;
				DiaoLuoItem component2 = _collider2d.GetComponentInParent<DiaoLuoItem>();
				if (!component2) return;
				int objectIndex2 = component2.ObjectIndex;
				m_PlayerEntity?.PickUpGoods(component2.GoodsId, objectIndex2, component2.Name, component2.gameObject);
				break;
			}
			case "BattleDiaoLuoLingShi":
			{
				if (m_PlayerEntity?.BattleMgr.Interaction != null) return;
				DiaoLuoItem component = _collider2d.GetComponentInParent<DiaoLuoItem>();
				if (!component) return;
				int objectIndex = component.ObjectIndex;
				m_PlayerEntity?.PickUpGoods(component.GoodsId, objectIndex, component.Name, component.gameObject, component.Count);
				break;
			}
			}
		}
	}
}
