using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoChooseWnd : Window
	{
		public DiHuoGeCaiLiaoChoosePanel Panel;

		private List<BaseNaJieGoodEntity> m_DataSource;

		private GameObject m_caiLiaoPrefabItem;

		private UnityAction<int, BaseNaJieGoodEntity> CompeleteChooseAction;

		private int m_EnterIndex;

		private BaseNaJieGoodEntity m_EnterGood;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DiHuoGeCaiLiaoChoosePanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			m_caiLiaoPrefabItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoGeCaiLiaoChooseItem.prefab");
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_EnterIndex = 0;
			if (paralist.Length == 5)
			{
				string text = paralist[0].ToString();
				string text2 = paralist[1].ToString();
				Panel.TitleText.text = text;
				Panel.CaiLiaoNameText.text = text2;
				m_DataSource = paralist[2] as List<BaseNaJieGoodEntity>;
				CompeleteChooseAction = paralist[3] as UnityAction<int, BaseNaJieGoodEntity>;
				int.TryParse(paralist[4].ToString(), out m_EnterIndex);
			}
			InitData();
		}

		private void InitData()
		{
			if (m_DataSource != null && m_DataSource.Count != 0)
			{
				MonoSingleton<GameStart>.Instance.m_mono.StartCoroutine(CreateDataAsny());
			}
		}

		private IEnumerator CreateDataAsny()
		{
			ToggleGroup group = Panel.ScrollViewContent.gameObject.GetComponent<ToggleGroup>();
			GridLayoutGroup component = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(m_DataSource.Count, Panel.ScrollViewContent, component);
			m_EnterGood = m_DataSource[m_EnterIndex];
			for (int i = 0; i < m_DataSource.Count; i++)
			{
				GameObject gameObject = SetListContentParent(Panel.ScrollViewContent, m_caiLiaoPrefabItem);
				Toggle component2 = gameObject.gameObject.GetComponent<Toggle>();
				if (component2 != null)
				{
					component2.group = group;
				}
				gameObject.GetComponent<DiHuoGeCaiLiaoChooseItem>().SetValue(isOn: (i == m_EnterIndex) ? true : false, baseData: m_DataSource[i], ToggleChangeAction: ToggleValueChange, index: i);
				yield return new WaitForEndOfFrame();
			}
		}

		private void ToggleValueChange(bool isOn, BaseNaJieGoodEntity good, int index)
		{
			if (isOn)
			{
				m_EnterIndex = index;
				m_EnterGood = good;
			}
		}

		private void QueRenBtnClick()
		{
			if (CompeleteChooseAction != null)
			{
				CompeleteChooseAction(m_EnterIndex, m_EnterGood);
			}
			CloseBtnClick();
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
