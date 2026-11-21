using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class TriggerCheck : MonoBehaviour
	{
		public UnityAction<Collider2D> TriggerEnterAction;

		public UnityAction<Collider2D> TriggerStayAction;

		public UnityAction<Collider2D> TriggerExitAction;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (TriggerEnterAction != null)
			{
				TriggerEnterAction(collision);
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (TriggerStayAction != null)
			{
				TriggerStayAction(collision);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (TriggerExitAction != null)
			{
				TriggerExitAction(collision);
			}
		}
	}
}
