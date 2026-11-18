using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianQiCreateChooseStageWnd : Window
	{
		public LianQiCreateChooseStagePanel Panel;

		private int m_CurrentStage;

		private int m_CreateGoodsType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LianQiCreateChooseStagePanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.XiaYiBuBtn, XiaYiBuBtnClick);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			InitGroup_1();
			InitGroup_2();
		}

		private void XiaYiBuBtnClick()
		{
			int currentLianQiShiLevel = Singleton<RoleManager>.Instance.GetCurrentLianQiShiLevel();
			if (m_CurrentStage > currentLianQiShiLevel)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友,您的炼器水平不足,无法创造该等阶器图！");
				return;
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.PopUpWnd("LianQiBuZhouSheZhiWnd.prefab", true, m_CurrentStage, m_CreateGoodsType);
		}

		private void InitGroup_1()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeChooseSmallItem.prefab");
			GridLayoutGroup component = Panel.ToggleGroup_1.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.ToggleGroup_1.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 9; i++)
			{
				GameObject gameObject = SetListContentParent(Panel.ToggleGroup_1, insObj);
				gameObject.GetComponent<Toggle>().group = component2;
				DiHuoGeChooseSmallItem component3 = gameObject.GetComponent<DiHuoGeChooseSmallItem>();
				switch (i)
				{
				case 0:
					component3.SetValue(null, "练气", i, component2, Group_1_Click, isChoose: true);
					break;
				case 1:
					component3.SetValue(null, "筑基", i, component2, Group_1_Click);
					break;
				case 2:
					component3.SetValue(null, "结丹", i, component2, Group_1_Click);
					break;
				case 3:
					component3.SetValue(null, "元婴", i, component2, Group_1_Click);
					break;
				case 4:
					component3.SetValue(null, "分神", i, component2, Group_1_Click);
					break;
				case 5:
					component3.SetValue(null, "炼虚", i, component2, Group_1_Click);
					break;
				case 6:
					component3.SetValue(null, "合体", i, component2, Group_1_Click);
					break;
				case 7:
					component3.SetValue(null, "渡劫", i, component2, Group_1_Click);
					break;
				case 8:
					component3.SetValue(null, "大乘", i, component2, Group_1_Click);
					break;
				}
			}
			SetGrticalListContentHight(9, Panel.ToggleGroup_1, component);
		}

		private void InitGroup_2()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeChooseSmallItem.prefab");
			GridLayoutGroup component = Panel.ToggleGroup_2.gameObject.GetComponent<GridLayoutGroup>();
			ToggleGroup component2 = Panel.ToggleGroup_2.gameObject.GetComponent<ToggleGroup>();
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = SetListContentParent(Panel.ToggleGroup_2, insObj);
				gameObject.GetComponent<Toggle>().group = component2;
				DiHuoGeChooseSmallItem component3 = gameObject.GetComponent<DiHuoGeChooseSmallItem>();
				switch (i)
				{
				case 0:
					component3.SetValue(null, "可配置攻击", i, component2, Group_2_Click, isChoose: true);
					break;
				case 1:
					component3.SetValue(null, "可配置防御", i, component2, Group_2_Click);
					break;
				case 2:
					component3.SetValue(null, "可穿戴防御", i, component2, Group_2_Click);
					break;
				case 3:
					component3.SetValue(null, "鞋", i, component2, Group_2_Click);
					break;
				}
			}
			SetGrticalListContentHight(4, Panel.ToggleGroup_2, component);
		}

		private void Group_1_Click(string name, int index, object data)
		{
			switch (name)
			{
			case "练气":
				m_CurrentStage = 1;
				break;
			case "筑基":
				m_CurrentStage = 2;
				break;
			case "结丹":
				m_CurrentStage = 3;
				break;
			case "元婴":
				m_CurrentStage = 4;
				break;
			case "分神":
				m_CurrentStage = 5;
				break;
			case "炼虚":
				m_CurrentStage = 6;
				break;
			case "合体":
				m_CurrentStage = 7;
				break;
			case "渡劫":
				m_CurrentStage = 8;
				break;
			case "大乘":
				m_CurrentStage = 9;
				break;
			}
		}

		private void Group_2_Click(string name, int index, object data)
		{
			switch (name)
			{
			case "可配置攻击":
				m_CreateGoodsType = 15;
				break;
			case "可配置防御":
				m_CreateGoodsType = 26;
				break;
			case "可穿戴防御":
				m_CreateGoodsType = 16;
				break;
			case "鞋":
				m_CreateGoodsType = 29;
				break;
			}
		}

		private void WenHaoTogClick()
		{
			string text = $"1、每个等阶的炼器大师仅可创造不超过自身品阶和境界的器图。 2、选择器图类型后，具体器具图标将随机生成。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "器图创造");
		}
	}
}
