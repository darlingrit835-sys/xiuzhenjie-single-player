using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public abstract class Controller : MonoBehaviour
	{
		public float ShenShiLockRadius = 10f;

		public float ShenShiFoundRadius = 15f;

		protected float m_ShiFaTime;

		protected float m_MaxShiFaTime;

		public EntityBase EntityDataSource;

		public Transform JianTouSprite;

		public SpriteRenderer TouXiangSprite;

		public RectTransform ShiFaProgressRect;

		public Image ShiFangProgressImg;

		private GameObject m_HuDunAniObj;

		public RectTransform MonsterCanvasRect;

		public TriggerCheck PlayerTrigger;

		public TriggerCheck ShenShiLockTrigger;

		public TriggerCheck ShenShiFoundTrigger;

		public UnityAction<Collider2D> LockEnterAction;

		public UnityAction<Collider2D> LockExitAction;

		public UnityAction<Collider2D> FoundEnterAction;

		public UnityAction<Collider2D> FoundExitAction;

		public GameObject DefenceFaQiItemObj;

		protected bool isMove;

		private Vector2 dir = Vector2.zero;

		protected Transform camTrans;

		protected bool skillMove;

		protected float skillMoveSpeed;

		protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

		protected Dictionary<string, GameObject> FoundAreaObjDic = new Dictionary<string, GameObject>();

		protected Dictionary<string, GameObject> LockAreaObjDic = new Dictionary<string, GameObject>();

		protected Vector3 m_Dir;

		public bool IsDie;

		public CircleCollider2D SelfCollider2D;

		public string Id;

		protected Timer m_ShiFaTimer;

		public Vector2 Dir
		{
			get
			{
				return dir;
			}
			set
			{
				if (value == Vector2.zero)
				{
					isMove = false;
				}
				else
				{
					isMove = true;
				}
				dir = value;
			}
		}

		private void Awake()
		{
			TriggerCheck shenShiLockTrigger = ShenShiLockTrigger;
			shenShiLockTrigger.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Combine(shenShiLockTrigger.TriggerEnterAction, new UnityAction<Collider2D>(ShenShiLockEnterTriggerCheck));
			TriggerCheck shenShiLockTrigger2 = ShenShiLockTrigger;
			shenShiLockTrigger2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Combine(shenShiLockTrigger2.TriggerExitAction, new UnityAction<Collider2D>(ShenShiLockExitTriggerCheck));
			TriggerCheck shenShiFoundTrigger = ShenShiFoundTrigger;
			shenShiFoundTrigger.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Combine(shenShiFoundTrigger.TriggerEnterAction, new UnityAction<Collider2D>(ShenShiFoundEnterTriggerCheck));
			TriggerCheck shenShiFoundTrigger2 = ShenShiFoundTrigger;
			shenShiFoundTrigger2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Combine(shenShiFoundTrigger2.TriggerExitAction, new UnityAction<Collider2D>(ShenShiFoundExitTriggerCheck));
			TriggerCheck playerTrigger = PlayerTrigger;
			playerTrigger.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Combine(playerTrigger.TriggerEnterAction, new UnityAction<Collider2D>(PlayerSelfEnterTriggerCheck));
			TriggerCheck playerTrigger2 = PlayerTrigger;
			playerTrigger2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Combine(playerTrigger2.TriggerExitAction, new UnityAction<Collider2D>(PlayerSelfExitTriggerCheck));
			SelfCollider2D = base.transform.Find("PlayerIcon").gameObject.GetComponent<CircleCollider2D>();
		}

		private void OnDestroy()
		{
			TriggerCheck shenShiLockTrigger = ShenShiLockTrigger;
			shenShiLockTrigger.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Remove(shenShiLockTrigger.TriggerEnterAction, new UnityAction<Collider2D>(ShenShiLockEnterTriggerCheck));
			TriggerCheck shenShiFoundTrigger = ShenShiFoundTrigger;
			shenShiFoundTrigger.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Remove(shenShiFoundTrigger.TriggerEnterAction, new UnityAction<Collider2D>(ShenShiFoundEnterTriggerCheck));
			TriggerCheck shenShiLockTrigger2 = ShenShiLockTrigger;
			shenShiLockTrigger2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Remove(shenShiLockTrigger2.TriggerExitAction, new UnityAction<Collider2D>(ShenShiLockExitTriggerCheck));
			TriggerCheck shenShiFoundTrigger2 = ShenShiFoundTrigger;
			shenShiFoundTrigger2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Remove(shenShiFoundTrigger2.TriggerExitAction, new UnityAction<Collider2D>(ShenShiFoundExitTriggerCheck));
			TriggerCheck playerTrigger = PlayerTrigger;
			playerTrigger.TriggerEnterAction = (UnityAction<Collider2D>)Delegate.Remove(playerTrigger.TriggerEnterAction, new UnityAction<Collider2D>(PlayerSelfEnterTriggerCheck));
			TriggerCheck playerTrigger2 = PlayerTrigger;
			playerTrigger2.TriggerExitAction = (UnityAction<Collider2D>)Delegate.Remove(playerTrigger2.TriggerExitAction, new UnityAction<Collider2D>(PlayerSelfExitTriggerCheck));
		}

		private void Update()
		{
			CtrlUpdate();
		}

		public virtual void Init(string Id, EntityBase entityBase, UnityAction<Collider2D> _lockEnterAction, UnityAction<Collider2D> _lockExitAction, UnityAction<Collider2D> _foundEnterAction, UnityAction<Collider2D> _foundExitAction)
		{
			this.Id = Id;
			EntityDataSource = entityBase;
			LockEnterAction = _lockEnterAction;
			LockExitAction = _lockExitAction;
			FoundEnterAction = _foundEnterAction;
			FoundExitAction = _foundExitAction;
			base.gameObject.SetActive(value: true);
		}

		public virtual void SetTouXiangSprite(Sprite sprite)
		{
			TouXiangSprite.sprite = sprite;
		}

		public virtual void SetTouXiangColor()
		{
		}

		protected virtual void CtrlUpdate()
		{
			ShiFaProgress();
		}

		public virtual void Dead()
		{
			IsDie = true;
		}

		public virtual void UnderAttack(int skillIndex, EntityBase _damageFromEntity, double _damage, int _fiveElement)
		{
			EntityDataSource.UnderAttack(skillIndex, _damageFromEntity, _damage, _fiveElement);
			if (!IsDie && (double)EntityDataSource.GetBattleProps().CurrentBlood <= 0.0)
			{
				Dead();
			}
		}

		public virtual void SetDirection(Vector2 vector2)
		{
			if (!(base.gameObject == null) && !(JianTouSprite == null))
			{
				m_Dir = vector2;
				float num = Vector2.Angle(new Vector2(0f, 1f), vector2);
				if (vector2.x <= 0f)
				{
					JianTouSprite.eulerAngles = new Vector3(0f, 0f, num);
				}
				else
				{
					JianTouSprite.eulerAngles = new Vector3(0f, 0f, 0f - num);
				}
			}
		}

		protected float GetAngle(Vector3 MoveNormalized)
		{
			if (MoveNormalized.y < 0f)
			{
				return 0f - Vector3.Angle(Vector3.right, MoveNormalized);
			}
			return Vector3.Angle(Vector3.right, MoveNormalized);
		}

		public virtual void ShowDamageNum(ulong _damageNum, DamageNumType _damageNumType)
		{
			var scene = EntityDataSource?.BattleMgr;
			if (scene != null && scene.Flourish != null)
			{
				Color color = _damageNumType == DamageNumType.Red ? new Color(1f,.52f,.39f)
					: _damageNumType == DamageNumType.Yellow ? new Color(1f,.88f,.58f) : new Color(.85f,.88f,.84f);
				if (_damageNum > 0) scene.Flourish.ShowReadout(this, WorldSetting.simplifyNumber(_damageNum), color);
				return;
			}
			if (_damageNum == 0)
			{
				_damageNum = 1uL;
			}
			Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
			GameObject gameObject = null;
			switch (_damageNumType)
			{
			case DamageNumType.Red:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Red/BattleRedDamageContentItem.prefab");
				dictionary = EntityDataSource.BattleMgr.m_BattleSceneResourceManager.RedNumDic;
				break;
			case DamageNumType.Yellow:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleYellowDamageContentItem.prefab");
				dictionary = EntityDataSource.BattleMgr.m_BattleSceneResourceManager.YellowNumDic;
				break;
			case DamageNumType.Gray:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Gray/BattleGrayDamageContentItem.prefab");
				dictionary = EntityDataSource.BattleMgr.m_BattleSceneResourceManager.GrayNumDic;
				break;
			default:
				gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleYellowDamageContentItem.prefab");
				dictionary = EntityDataSource.BattleMgr.m_BattleSceneResourceManager.YellowNumDic;
				break;
			}
			GameObject content = UnityEngine.Object.Instantiate(gameObject, MonsterCanvasRect.transform);
			if (content == null)
			{
				return;
			}
			RectTransform component = content.GetComponent<RectTransform>();
			string text = WorldSetting.simplifyNumber(_damageNum);
			foreach (char c in text)
			{
				GameObject value = null;
				if (dictionary.TryGetValue(c.ToString(), out value))
				{
					UnityEngine.Object.Instantiate(value, component);
				}
			}
			content.GetComponent<Animation>().enabled = true;
			Timer.Register(2f, () =>
			{
				if (!IsDie)
				{
					UnityEngine.Object.Destroy(content);
				}
			});
		}

		public virtual void ShowStateText(string _text)
		{
			var scene = EntityDataSource?.BattleMgr;
			if (scene != null && scene.Flourish != null)
			{
				scene.Flourish.ShowReadout(this, _text, new Color(.72f,.97f,.88f));
				return;
			}
			if (MonsterCanvasRect == null)
			{
				return;
			}
			new Dictionary<string, GameObject>();
			GameObject gameObject = null;
			gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/Battle/UINum/Yellow/BattleYellowDamageContentItem.prefab");
			if (gameObject == null)
			{
				return;
			}
			Dictionary<string, GameObject> yellowNumDic = EntityDataSource.BattleMgr.m_BattleSceneResourceManager.YellowNumDic;
			GameObject content = UnityEngine.Object.Instantiate(gameObject, MonsterCanvasRect.transform);
			RectTransform component = content.GetComponent<RectTransform>();
			GameObject value = null;
			if (yellowNumDic.TryGetValue(_text, out value))
			{
				UnityEngine.Object.Instantiate(value, component);
			}
			content.GetComponent<Animation>().enabled = true;
			Timer.Register(2f, () =>
			{
				if (!IsDie)
				{
					UnityEngine.Object.Destroy(content);
				}
			});
		}

		public virtual void SetShenShiCircleScale(float scale)
		{
			ShenShiLockRadius = scale * 5f;
			ShenShiFoundRadius = ShenShiLockRadius * 2f;
			ShenShiFoundTrigger.gameObject.GetComponent<CircleCollider2D>().radius = ShenShiFoundRadius;
			float x = ShenShiLockTrigger.gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
			float num = ShenShiLockRadius / x;
			ShenShiLockTrigger.transform.localScale = new Vector3(num, num, num);
		}

		protected virtual void ShenShiLockEnterTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void ShenShiFoundEnterTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void ShenShiLockExitTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void ShenShiFoundExitTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void PlayerSelfEnterTriggerCheck(Collider2D _collider2d)
		{
		}

		protected virtual void PlayerSelfExitTriggerCheck(Collider2D _collider2d)
		{
		}

		public void SetHuDun(GameObject huDunObj)
		{
			CancleHuDun();
			m_HuDunAniObj = huDunObj;
		}

		public void CancleHuDun()
		{
			if (m_HuDunAniObj != null)
			{
				UnityEngine.Object.Destroy(m_HuDunAniObj);
				m_HuDunAniObj = null;
			}
		}

		public virtual void ShiFangSkill(float shiFaTime)
		{
			m_ShiFaTime = shiFaTime;
			m_MaxShiFaTime = shiFaTime;
			ShiFaProgressRect.gameObject.SetActive(value: true);
			m_ShiFaTimer = Timer.Register(shiFaTime, () =>
			{
				if (!IsDie)
				{
					m_ShiFaTime = 0f;
					ShiFaProgressRect.gameObject.SetActive(value: false);
				}
			});
		}

		public virtual void CancleShiFangSkill()
		{
			if (m_ShiFaTimer != null)
			{
				m_ShiFaTimer.Cancel();
			}
			m_ShiFaTime = 0f;
			ShiFaProgressRect.gameObject.SetActive(value: false);
		}

		public virtual void ShiFaProgress()
		{
			if (m_ShiFaTime > 0f && m_MaxShiFaTime > 0f)
			{
				m_ShiFaTime -= Time.deltaTime;
				ShiFangProgressImg.fillAmount = (m_MaxShiFaTime - m_ShiFaTime) / m_MaxShiFaTime;
			}
			else if (m_ShiFaTime < 0f)
			{
				m_ShiFaTime = 0f;
			}
		}

		public virtual void EnterZhenFa(BattleZhenFaType battleZhenFaType, BaseSkillModel baseSkillModel, ZhenFa_Item zhenFa_Item)
		{
			EntityDataSource.EnterZhenFa(battleZhenFaType, baseSkillModel, zhenFa_Item);
		}

		public virtual void ExitZhenFa(BattleZhenFaType battleZhenFaType, string skillId)
		{
			EntityDataSource.ExitZhenFa(battleZhenFaType, skillId);
		}

		public virtual void ShowDefenceEquipmentEffect()
		{
		}

		public virtual void CancleShowDefenceEquipmentEffect()
		{
		}
	}
}
