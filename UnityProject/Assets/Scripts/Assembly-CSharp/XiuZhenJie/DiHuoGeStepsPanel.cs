using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeStepsPanel : MonoBehaviour
	{
		public Text CurrentStepName;

		public RectTransform OverContent;

		public RectTransform BeforeContent;

		private GameObject m_InstanceItemObj;

		private List<string> AllSteps;

		private Queue<GameObject> m_BeforeSteps;

		private Queue<GameObject> m_OverSteps;

		private int m_CurrentStepIndex;

		private Window m_DiHuoWnd;

		private void Start()
		{
			m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/StepBgItem.prefab");
		}

		public void InitSteps(List<string> AllStepNames, Window diHuoGeLianZhiWnd)
		{
			m_BeforeSteps = new Queue<GameObject>();
			m_OverSteps = new Queue<GameObject>();
			AllSteps = AllStepNames;
			m_DiHuoWnd = diHuoGeLianZhiWnd;
			m_CurrentStepIndex = 0;
			CurrentStepName.text = AllStepNames[m_CurrentStepIndex];
			for (int i = 1; i <= 3; i++)
			{
				m_BeforeSteps.Enqueue(AddBeforeItem(i));
			}
		}

		private GameObject AddBeforeItem(int index)
		{
			if (index >= AllSteps.Count)
			{
				return null;
			}
			if (m_InstanceItemObj == null)
			{
				m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/StepBgItem.prefab");
			}
			GameObject obj = m_DiHuoWnd.SetListContentParent(BeforeContent, m_InstanceItemObj);
			obj.transform.Find("Text").gameObject.GetComponent<Text>().text = AllSteps[index];
			return obj;
		}

		private GameObject AddOverItem(int index)
		{
			if (index >= AllSteps.Count)
			{
				return null;
			}
			if (m_InstanceItemObj == null)
			{
				m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/StepBgItem.prefab");
			}
			GameObject obj = m_DiHuoWnd.SetListContentParent(OverContent, m_InstanceItemObj);
			obj.transform.Find("Text").gameObject.GetComponent<Text>().text = AllSteps[index];
			return obj;
		}

		public void NextStep()
		{
			m_CurrentStepIndex++;
			BeforeListPopItem();
			OverListPushItem();
			CurrentStepName.text = AllSteps[m_CurrentStepIndex];
		}

		private void BeforeListPopItem()
		{
			Object.Destroy(m_BeforeSteps.Dequeue());
			GameObject gameObject = AddBeforeItem(m_CurrentStepIndex + 3);
			if (!(gameObject == null))
			{
				m_BeforeSteps.Enqueue(gameObject);
			}
		}

		private void OverListPushItem()
		{
			if (m_OverSteps.Count >= 3)
			{
				Object.Destroy(m_OverSteps.Dequeue());
				GameObject gameObject = AddOverItem(m_CurrentStepIndex - 1);
				if (!(gameObject == null))
				{
					m_OverSteps.Enqueue(gameObject);
				}
			}
			else
			{
				GameObject gameObject2 = AddOverItem(m_CurrentStepIndex - 1);
				if (!(gameObject2 == null))
				{
					m_OverSteps.Enqueue(gameObject2);
				}
			}
		}
	}
}
