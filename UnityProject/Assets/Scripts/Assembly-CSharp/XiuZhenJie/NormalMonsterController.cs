using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class NormalMonsterController : Controller
	{
		public GameObject LockedSprite;

		public SpriteRenderer IconSpriteBg;

		public Image JianRenImg;

		public Image BloodImg;

		public Text NameText;

		public override void Init(string Id, EntityBase entityBase, UnityAction<Collider2D> _lockEnterAction, UnityAction<Collider2D> _lockExitAction, UnityAction<Collider2D> _foundEnterAction, UnityAction<Collider2D> _foundExitAction)
		{
			base.Init(Id, entityBase, _lockEnterAction, _lockExitAction, _foundEnterAction, _foundExitAction);
			if (entityBase != null)
			{
				NameText.text = $"{entityBase.BattleModel.Level}·{WorldSetting.GetWuXingName((WuXing)entityBase.BattleModel.AttackFiveElement)}·{entityBase.BattleModel.Name}";
			}
		}

		public override void SetTouXiangColor()
		{
			Color color = Color.black;
			switch (EntityDataSource.GetBattleProps().Type)
			{
			case 3:
				color = new Color(127f / 255f, 210f / 255f, 49f / 255f, 1f);
				break;
			case 4:
				color = new Color(163f / 255f, 53f / 255f, 237f / 255f, 1f);
				break;
			case 5:
				color = new Color(1f, 128f / 255f, 1f / 255f, 1f);
				break;
			}
			IconSpriteBg.color = color;
		}

		public void Locked()
		{
			if (LockedSprite != null)
			{
				LockedSprite.gameObject.SetActive(value: true);
			}
		}

		public void CancleLocked()
		{
			if (LockedSprite != null)
			{
				LockedSprite.gameObject.SetActive(value: false);
			}
		}

		public override void UnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, int _fiveElement)
		{
			base.UnderAttack(skillIndex, _damageFromEntity, _damage, _fiveElement);
			JianRenImg.fillAmount = (float)(double)EntityDataSource.BattleModel.CurrentJianRen / (float)(double)EntityDataSource.BattleModel.Max_JianRen;
			BloodImg.fillAmount = (float)(double)EntityDataSource.BattleModel.CurrentBlood / (float)(double)EntityDataSource.BattleModel.Max_Blood;
		}

		public override void SetShenShiCircleScale(float scale)
		{
			base.SetShenShiCircleScale(scale);
			ShenShiFoundRadius = ShenShiLockRadius * 1.5f;
			ShenShiFoundTrigger.gameObject.GetComponent<CircleCollider2D>().radius = ShenShiFoundRadius;
		}

		public override void Dead()
		{
			base.Dead();
			base.gameObject.GetComponent<Animation>().Play();
			Timer.Register(2f, () =>
			{
				if (base.gameObject != null)
				{
					Object.Destroy(base.gameObject);
				}
			});
		}

		protected override void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundEnterTriggerCheck(_collider2d);
			if ((!(_collider2d != null) || !(_collider2d.gameObject.tag != "Player")) && FoundEnterAction != null)
			{
				FoundEnterAction(_collider2d);
			}
		}

		protected override void ShenShiFoundExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiFoundExitTriggerCheck(_collider2d);
			if ((!(_collider2d != null) || !(_collider2d.gameObject.tag != "Player")) && FoundExitAction != null)
			{
				FoundExitAction(_collider2d);
			}
		}

		protected override void ShenShiLockEnterTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockEnterTriggerCheck(_collider2d);
			if ((!(_collider2d != null) || !(_collider2d.gameObject.tag != "Player")) && LockEnterAction != null)
			{
				LockEnterAction(_collider2d);
			}
		}

		protected override void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
			base.ShenShiLockExitTriggerCheck(_collider2d);
			if ((!(_collider2d != null) || !(_collider2d.gameObject.tag != "Player")) && LockExitAction != null)
			{
				LockExitAction(_collider2d);
			}
		}
	}
}
