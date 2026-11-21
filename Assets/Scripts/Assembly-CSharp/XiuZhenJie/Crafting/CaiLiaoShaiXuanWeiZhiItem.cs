using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class CaiLiaoShaiXuanWeiZhiItem : MonoBehaviour
	{
		public Text Name;

		public Toggle toggle;

		private UnityAction<MapEntity> Action;

		private MapEntity m_MapModel;

		public void SetItem(MapEntity mapModel, UnityAction<MapEntity> action, ToggleGroup group)
		{
			if (mapModel != null)
			{
				m_MapModel = mapModel;
				Name.text = m_MapModel.MapName;
				toggle.group = group;
				Action = action;
				toggle.onValueChanged.AddListener(Select);
			}
		}

		private void Select(bool val)
		{
			if (val)
			{
				Action?.Invoke(m_MapModel);
				Name.color = Color.white;
			}
			else
			{
				Name.color = Color.black;
			}
		}
	}
}
