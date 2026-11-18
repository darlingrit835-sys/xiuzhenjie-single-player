using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XiuZhenJie
{
	public class GuideWnd : Window
	{
		private GuidePanel m_Panel;

		private GuideMaskItem m_CurrentGuideItemPanel;

		private Dictionary<int, GuideMaskItem> m_LearnItemDic;

		private Dictionary<int, GuideMaskItem> m_CanWuItemDic;

		private Dictionary<int, GuideMaskItem> m_BattleItemDic;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<GuidePanel>();
			AddButtonClickListener(m_Panel.TiaoGuoBtn, TiaoGuoMethod);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_CurrentGuideItemPanel = null;
			m_LearnItemDic = new Dictionary<int, GuideMaskItem>();
			m_CanWuItemDic = new Dictionary<int, GuideMaskItem>();
			m_BattleItemDic = new Dictionary<int, GuideMaskItem>();
			Init();
		}

		private void Init()
		{
			if (m_Panel == null)
			{
				return;
			}
			foreach (Transform item in m_Panel.CubeMask)
			{
				GuideMaskItem component = item.gameObject.GetComponent<GuideMaskItem>();
				if (!(component != null))
				{
					continue;
				}
				switch (component.GuideMaskType)
				{
				case GuideType.Learn:
					if (!m_LearnItemDic.ContainsKey(component.GuideIndex))
					{
						m_LearnItemDic.Add(component.GuideIndex, component);
					}
					break;
				case GuideType.Battle:
					if (!m_BattleItemDic.ContainsKey(component.GuideIndex))
					{
						m_BattleItemDic.Add(component.GuideIndex, component);
					}
					break;
				case GuideType.DongFuCanWu:
					if (!m_CanWuItemDic.ContainsKey(component.GuideIndex))
					{
						m_CanWuItemDic.Add(component.GuideIndex, component);
					}
					break;
				}
			}
		}

		public void ShowGuide(GuideType type, int index, UnityAction clickCallBack = null)
		{
			GuideMaskItem value = null;
			switch (type)
			{
			case GuideType.Learn:
				m_LearnItemDic.TryGetValue(index, out value);
				break;
			case GuideType.Battle:
				m_BattleItemDic.TryGetValue(index, out value);
				break;
			case GuideType.DongFuCanWu:
				m_CanWuItemDic.TryGetValue(index, out value);
				break;
			}
			if (!(value == null))
			{
				if (m_CurrentGuideItemPanel != null)
				{
					m_CurrentGuideItemPanel.gameObject.SetActive(value: false);
				}
				value.gameObject.SetActive(value: true);
				value.BeginShow(clickCallBack);
				m_CurrentGuideItemPanel = value;
			}
		}

		public bool IsGuideEnd(GuideType type, int index)
		{
			switch (type)
			{
			case GuideType.Learn:
				if (index == m_LearnItemDic.Count)
				{
					return true;
				}
				return false;
			case GuideType.Battle:
				if (index == m_BattleItemDic.Count)
				{
					return true;
				}
				return false;
			case GuideType.LianDan:
				return false;
			case GuideType.DongFuCanWu:
				if (index == m_CanWuItemDic.Count)
				{
					return true;
				}
				return false;
			default:
				return false;
			}
		}

		private void TiaoGuoMethod()
		{
			Singleton<GuideManager>.Instance.NoShowGuide(m_CurrentGuideItemPanel.GuideMaskType);
			Singleton<UIManager>.Instance.CloseWnd("GuideWnd.prefab");
		}
	}
}
