using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShouChao_MonsterItem : MonoBehaviour
	{
		public Image IconImg;

		public Image IconBgImg;

		public Image BgImg;

		private Vector2 m_InitPos;

		private Vector2 m_WaitMovePos;

		private Vector2 m_AttackEffectPos;

		private ShouChaoMonsterStatus m_Status;

		private float m_AttackInterval;

		private float m_AttackTimint;

		public UnityAction MonsterAttackAction;

		public ShouChaoMonsterStatus MonsterStatus
		{
			get
			{
				return m_Status;
			}
			private set
			{
				m_Status = value;
			}
		}

		private void FixedUpdate()
		{
			if (m_Status == ShouChaoMonsterStatus.WaitingAttack)
			{
				m_AttackTimint += Time.fixedDeltaTime;
				if (m_AttackTimint >= m_AttackInterval)
				{
					m_AttackTimint = 0f;
					Attack();
				}
			}
		}

		public void Init()
		{
			m_InitPos = base.transform.localPosition;
			m_WaitMovePos = new Vector2(m_InitPos.x - 20f, m_InitPos.y + 20f);
			m_AttackEffectPos = new Vector2(m_InitPos.x + 5f, m_InitPos.y - 5f);
			m_AttackInterval = UnityEngine.Random.Range(2f, 5f);
			m_Status = ShouChaoMonsterStatus.WaitingMove;
			base.transform.localPosition = m_WaitMovePos;
			Sprite sprite = CatalogIconArt.RandomFromPool("beast_wave");
			if (sprite == null)
			{
				int index = UnityEngine.Random.Range(1, 46);
				sprite = Singleton<FacadeManager>.Instance.SelectImgSprite(0, index, 4);
			}
			base.gameObject.SetActive(value: true);
			IconImg.sprite = sprite;
		}

		public void StartWalking()
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = base.transform.DOLocalMove(m_InitPos, 2f);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(MoveCompelete));
			m_Status = ShouChaoMonsterStatus.Moving;
		}

		private void MoveCompelete()
		{
			if (m_Status == ShouChaoMonsterStatus.Moving)
			{
				m_Status = ShouChaoMonsterStatus.WaitingAttack;
			}
		}

		private void Attack()
		{
			if (m_Status != ShouChaoMonsterStatus.Dead && m_Status != ShouChaoMonsterStatus.Attacking && m_Status != ShouChaoMonsterStatus.Moving && m_Status != ShouChaoMonsterStatus.WaitingMove)
			{
				m_Status = ShouChaoMonsterStatus.Attacking;
				TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = base.transform.DOLocalMove(m_AttackEffectPos, 0.3f);
				tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(AttackCompelete));
			}
		}

		private void AttackCompelete()
		{
			if (m_Status != ShouChaoMonsterStatus.Dead)
			{
				if (MonsterAttackAction != null)
				{
					MonsterAttackAction();
				}
				TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = base.transform.DOLocalMove(m_InitPos, 0.5f);
				tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)(() =>
				{
					m_Status = ShouChaoMonsterStatus.WaitingAttack;
				}));
			}
		}

		public void Dead()
		{
			m_Status = ShouChaoMonsterStatus.Dead;
			DeadMethod();
		}

		private void DeadMethod()
		{
			Color endValue = new Color(1f, 1f, 1f, 0f);
			Color endValue2 = new Color(0.8862746f, 185f / 255f, 0.4666667f, 0f);
			Color endValue3 = new Color(148f / 255f, 0.4784314f, 0.3568628f, 0f);
			IconImg.DOColor(endValue, 1f);
			IconBgImg.DOColor(endValue2, 1f);
			BgImg.DOColor(endValue3, 1f);
			Timer.Register(1f, () =>
			{
				Color color = new Color(1f, 1f, 1f, 1f);
				Color color2 = new Color(0.8862746f, 185f / 255f, 0.4666667f, 1f);
				Color color3 = new Color(148f / 255f, 0.4784314f, 0.3568628f, 1f);
				base.gameObject.SetActive(value: false);
				IconImg.color = color;
				IconBgImg.color = color2;
				BgImg.color = color3;
			});
		}
	}
}
