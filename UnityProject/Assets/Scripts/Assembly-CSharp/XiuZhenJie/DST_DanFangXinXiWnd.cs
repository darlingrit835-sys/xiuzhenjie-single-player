using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_DanFangXinXiWnd : Window
	{
		public DST_DanFangXinXiPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<DST_DanFangXinXiPanel>();
			AddButtonClickListener(_panel.DuiHuanBtn, DuiHuanClickHandler);
			AddButtonClickListener(_panel.ZhuCeBtn, ZhuCeClickHandler);
			AddButtonClickListener(_panel.CloseBtn, CloseClickHandler);
			InitData(paralist);
			GreateData(paralist);
		}

		private void GreateData(object[] paralist)
		{
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DanFang_SuoXuYaoCaiItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = _panel.ItemListContent.GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < 8; i++)
			{
				SetListContentParent(_panel.ItemListContent, gameObject).GetComponent<DanFang_YaoCaiItem>().SetValue("药材:" + i);
			}
			_panel.ItemListContent.sizeDelta = new Vector2(_panel.ItemListContent.sizeDelta.x, SetVerticalListContentHight(8, y, component));
		}

		private void InitData(object[] paralist)
		{
			if ((int)paralist[0] == 1)
			{
				_panel.ItemName.gameObject.SetActive(value: false);
				_panel.ZhuCeBtn.gameObject.SetActive(value: false);
				_panel.NameText.gameObject.SetActive(value: true);
				_panel.DuiHuanBtn.gameObject.SetActive(value: true);
			}
			else
			{
				_panel.NameText.gameObject.SetActive(value: false);
				_panel.DuiHuanBtn.gameObject.SetActive(value: false);
				_panel.ItemName.gameObject.SetActive(value: true);
				_panel.ZhuCeBtn.gameObject.SetActive(value: true);
			}
		}

		private void ZhuCeClickHandler()
		{
		}

		private void DuiHuanClickHandler()
		{
		}

		private void CloseClickHandler()
		{
			Singleton<UIManager>.Instance.CloseWnd("DanFangXinXiWnd.prefab");
		}
	}
}
