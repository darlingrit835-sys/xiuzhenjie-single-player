using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoShaiXuanMapPanel : MonoBehaviour
	{
		public Button QueRenBtn;

		public RectTransform Content;

		private GameObject itemObj;

		private void Start()
		{
			QueRenBtn.onClick.RemoveAllListeners();
			QueRenBtn.onClick.AddListener(Close);
		}

		public void SetValue(List<MapEntity> mapNameList)
		{
			base.gameObject.SetActive(value: true);
			foreach (Transform item in Content)
			{
				Object.Destroy(item.gameObject);
			}
			if (itemObj == null)
			{
				itemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGe_MapItem.prefab");
			}
			foreach (MapEntity mapName in mapNameList)
			{
				Object.Instantiate(itemObj, Content).transform.Find("Text").GetComponent<Text>().text = mapName.MapName;
			}
		}

		private void Close()
		{
			foreach (Transform item in Content)
			{
				Object.Destroy(item.gameObject);
			}
			base.gameObject.SetActive(value: false);
		}
	}
}
