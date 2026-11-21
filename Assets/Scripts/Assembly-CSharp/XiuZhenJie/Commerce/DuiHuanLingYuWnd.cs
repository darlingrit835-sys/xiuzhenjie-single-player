using UnityEngine;

namespace XiuZhenJie
{
	public class DuiHuanLingYuWnd : Window
	{
		public DuiHuanLingYuPanel duiHuanXianYuPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			duiHuanXianYuPanel = base.GameObject.GetComponent<DuiHuanLingYuPanel>();
			AddButtonClickListener(duiHuanXianYuPanel.CloseBtn, CloseWnd);
			Init();
		}

		public void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("DuiHuanLingYuWnd.prefab");
		}

		public void Init()
		{
			GameObject original = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DuiHuanXianYuItem.prefab");
			for (int i = 0; i < 6; i++)
			{
				GameObject gameObject = Object.Instantiate(original);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(duiHuanXianYuPanel.Content);
				component.localScale = Vector3.one;
				component.eulerAngles = Vector3.zero;
				DuiHuanLingYuItem component2 = gameObject.GetComponent<DuiHuanLingYuItem>();
				string path = string.Empty;
				string lingyuValue = string.Empty;
				int jiage = 0;
				switch (i)
				{
				case 0:
					path = "Assets/GameData/Textures/LingYuLogo.png";
					jiage = 6;
					lingyuValue = "60";
					break;
				case 1:
					path = "Assets/GameData/Textures/LingYuLogo1.png";
					jiage = 12;
					lingyuValue = "120";
					break;
				case 2:
					path = "Assets/GameData/Textures/LingYuLogo2.png";
					jiage = 30;
					lingyuValue = "300";
					break;
				case 3:
					path = "Assets/GameData/Textures/LingYuLogo3.png";
					jiage = 68;
					lingyuValue = "680";
					break;
				case 4:
					path = "Assets/GameData/Textures/LingYuLogo4.png";
					jiage = 128;
					lingyuValue = "1280";
					break;
				case 5:
					path = "Assets/GameData/Textures/LingYuLogo5.png";
					jiage = 328;
					lingyuValue = "3280";
					break;
				}
				ChangImageSpriteAsync(path, component2.BackGroundImg);
				component2.SetValue(i.ToString(), jiage, lingyuValue, () =>
				{
					Debug.Log("调用商店api购买");
				});
			}
		}
	}
}
