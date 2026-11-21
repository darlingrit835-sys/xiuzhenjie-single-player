using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class JingShiWnd : Window
	{
		public JingShiPanel Panel;

		private IEnumerator m_ChuWuAni;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<JingShiPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtn);
			AddButtonClickListener(Panel.HuaFuBtn, HuaFuBtn);
			AddButtonClickListener(Panel.ChuWuXiangBtn, ChuWuXiang);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			m_ChuWuAni = ChuWuAnimation();
			MonoSingleton<GameStart>.Instance.StartCoroutine(m_ChuWuAni);
		}

		private void LiKaiBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void HuaFuBtn()
		{
			Singleton<UIManager>.Instance.PopUpWnd("HuaFuChooseWnd.prefab", true);
		}

		private void ChuWuXiang()
		{
			Singleton<UIManager>.Instance.PopUpWnd("ChuWuXiangWnd.prefab", true);
		}

		private IEnumerator ChuWuAnimation()
		{
			Image image = Panel.ChuWuXiangIcon;
			_ = image.color;
			float value = 0f;
			float colorValue = 1f;
			while (true)
			{
				if (colorValue >= 1f)
				{
					value = -0.01f;
				}
				else if (colorValue <= 0.5f)
				{
					value = 0.01f;
				}
				image.color = new Color(colorValue + value, colorValue + value, colorValue + value, 1f);
				colorValue += value;
				yield return new WaitForSeconds(0.02f);
			}
		}

		public override void OnClose()
		{
			base.OnClose();
			MonoSingleton<GameStart>.Instance.StopCoroutine(m_ChuWuAni);
		}
	}
}
