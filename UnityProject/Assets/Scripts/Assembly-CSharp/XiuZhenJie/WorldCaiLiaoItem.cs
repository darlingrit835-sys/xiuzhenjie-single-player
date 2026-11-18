using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WorldCaiLiaoItem : MonoBehaviour
	{
		public Toggle ItemToggle;

		public Text ItemName;

		private WorldCaiLiaoModel m_DataSource;

		private LZ_WorldCaiLiaoModel m_DataSources;

		public void SetValue(WorldCaiLiaoModel data, ToggleGroup group, UnityAction<WorldCaiLiaoModel, bool> clickCallBack, bool isChoose = false)
		{
			m_DataSource = data;
			if (m_DataSource == null)
			{
				return;
			}
			ItemName.text = data.c;
			ItemToggle.group = group;
			ItemToggle.onValueChanged.RemoveAllListeners();
			ItemToggle.onValueChanged.AddListener((bool isOn) =>
			{
				if (isOn)
				{
					ItemName.color = Color.white;
				}
				else
				{
					ItemName.color = Color.black;
				}
				if (clickCallBack != null)
				{
					clickCallBack(data, isOn);
				}
			});
			ItemToggle.isOn = isChoose;
		}

		public void SetValues(LZ_WorldCaiLiaoModel data, ToggleGroup group, UnityAction<LZ_WorldCaiLiaoModel, bool> clickCallBack, bool isChoose = false)
		{
			m_DataSources = data;
			if (m_DataSources == null)
			{
				return;
			}
			ItemName.text = data.c;
			ItemToggle.group = group;
			ItemToggle.onValueChanged.RemoveAllListeners();
			ItemToggle.onValueChanged.AddListener((bool isOn) =>
			{
				if (isOn)
				{
					ItemName.color = Color.white;
				}
				else
				{
					ItemName.color = Color.black;
				}
				if (clickCallBack != null)
				{
					clickCallBack(data, isOn);
				}
			});
			ItemToggle.isOn = isChoose;
		}
	}
}
