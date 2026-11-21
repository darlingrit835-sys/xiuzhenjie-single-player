using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShangPinXiangQingWnd : Window
	{
		private ShangPuItemModel modelSource;

		private string mapId;

		public ShangPinXiangQingPanel _panel;

		private UnityAction<ShangPuItemModel, string> BuySuccessAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<ShangPinXiangQingPanel>();
			AddButtonClickListener(_panel.QueRen, QueRenClick);
			AddButtonClickListener(_panel.Close, CloseClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length == 3)
			{
				modelSource = paralist[0] as ShangPuItemModel;
				mapId = paralist[1].ToString();
				BuySuccessAction = paralist[2] as UnityAction<ShangPuItemModel, string>;
			}
			if (modelSource != null)
			{
				ReflashData();
			}
		}

		public override void OnDisable()
		{
			base.OnDisable();
		}

		public void ReflashData()
		{
			_panel.Name.text = modelSource.name;
			_panel.ItemPriceText.text = modelSource.systemPrice.ToString();
			_panel.Des.text = modelSource.describe;
			string arg = (((double)Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - modelSource.systemPrice > 0.0) ? ((long)((double)Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - modelSource.systemPrice)).ToString() : "0");
			_panel.ShenYuLingShi.text = $"剩余灵石:{arg}";
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ShangPuXiangQingItem.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = _panel.ItemListContent.GetComponent<VerticalLayoutGroup>();
			if (modelSource.wuPinProperty != null && modelSource.wuPinProperty.Count > 0)
			{
				for (int i = 0; i < modelSource.wuPinProperty.Count; i++)
				{
					string[] array = modelSource.wuPinProperty[i].Split(',');
					SetListContentParent(_panel.ItemListContent, gameObject).GetComponent<ShangPuXiangQingItem>().SetValue(array[0], array[1]);
				}
			}
			_panel.ItemListContent.sizeDelta = new Vector2(_panel.ItemListContent.sizeDelta.x, SetVerticalListContentHight(modelSource.wuPinProperty.Count, y, component));
		}

		private void QueRenClick()
		{
			if (BuySuccessAction != null)
			{
				BuySuccessAction(modelSource, mapId);
			}
			Singleton<UIManager>.Instance.CloseWnd("ShangPuWuPinXiangQingWnd.prefab");
		}

		private void CloseClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("ShangPuWuPinXiangQingWnd.prefab");
		}
	}
}
