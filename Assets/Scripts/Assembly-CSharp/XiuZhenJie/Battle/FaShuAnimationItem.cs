using UnityEngine;

namespace XiuZhenJie
{
	public class FaShuAnimationItem : MonoBehaviour
	{
		private Animator animator;

		private AnimatorStateInfo info;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			info = animator.GetCurrentAnimatorStateInfo(0);
			if (info.normalizedTime >= 1f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
