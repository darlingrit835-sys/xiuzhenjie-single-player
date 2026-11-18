using UnityEngine;

namespace XiuZhenJie
{
	public class MiJingItem : MonoBehaviour
	{
		public GameObject MiJing_1;

		public GameObject MiJing_2;

		public GameObject MiJing_3;

		public GameObject MiJing_4;

		public int ObjectIndex;

		public string Name;

		public void Init(int objectIndex, string name)
		{
			ObjectIndex = objectIndex;
			Name = name;
			switch (Random.Range(1, 4))
			{
			case 2:
				MiJing_1.gameObject.SetActive(value: false);
				MiJing_2.gameObject.SetActive(value: true);
				break;
			case 3:
				MiJing_1.gameObject.SetActive(value: false);
				MiJing_3.gameObject.SetActive(value: true);
				break;
			case 4:
				MiJing_1.gameObject.SetActive(value: false);
				MiJing_4.gameObject.SetActive(value: true);
				break;
			}
		}
	}
}
