using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class MineralItem : MonoBehaviour
	{
		private XzjWorldGoodsMaterialsMineralSpaceVO m_DataSource;

		private UnityAction<XzjWorldGoodsMaterialsMineralSpaceVO> m_ClickAction;

		public void Init(XzjWorldGoodsMaterialsMineralSpaceVO _dataSource, UnityAction<XzjWorldGoodsMaterialsMineralSpaceVO> _clickAction)
		{
			m_DataSource = _dataSource;
			m_ClickAction = _clickAction;
			int num = Random.Range(1, 2);
			string path = string.Format("{0}_{1}.png", "Assets/GameData/Textures/Battle/Mineral/Mineral", num);
			Sprite sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>(path);
			base.transform.Find("Icon").gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
		}

		public XzjWorldGoodsMaterialsMineralSpaceVO GetDataSource()
		{
			return m_DataSource;
		}
	}
}
