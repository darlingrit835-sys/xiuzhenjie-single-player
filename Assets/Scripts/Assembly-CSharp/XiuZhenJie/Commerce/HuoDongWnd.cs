using UnityEngine;

namespace XiuZhenJie
{
	public class HuoDongWnd : Window
	{
		public HuoDongWndPanel huoDongWndPanel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			huoDongWndPanel = base.GameObject.GetComponent<HuoDongWndPanel>();
			AddButtonClickListener(huoDongWndPanel.closeBtn, CloseThisWnd);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Init();
		}

		public void CloseThisWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd("HuoDongWnd.prefab");
		}

		public void Init()
		{
			GameObject original = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/HuoDongItem.prefab");
			for (int i = 0; i < 5; i++)
			{
				GameObject gameObject = Object.Instantiate(original);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(huoDongWndPanel.content.GetComponent<RectTransform>());
				component.localScale = Vector3.one;
				component.eulerAngles = Vector3.zero;
				gameObject.GetComponent<HuoDongWndItem>().SetValue(i.ToString(), 45, "超值月卡", "1950灵玉", "首次充值获得450灵玉,\n每日可领取50灵玉，可领取30天，\n合计1950灵玉", () =>
				{
					Debug.Log("调用商店api购买");
				});
			}
			float y = 1274f;
			huoDongWndPanel.content.sizeDelta = new Vector2(huoDongWndPanel.content.sizeDelta.x, y);
		}
	}
}
