using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class KeyBoardWnd : Window
	{
		public KeyBoardPanel Panel;

		public Action<int> ChangePageAction;

		private IEnumerator RemainingSecondIEnumerator;

		private int m_CurrentPageNum;

		private int m_MaxPageNum;

		private bool m_FirstInput;

		private int m_RemainingSeconds;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<KeyBoardPanel>();
			m_MaxPageNum = 1;
			m_CurrentPageNum = 1;
			RemainingSecondIEnumerator = RemainingSecond();
			if (paralist.Length >= 2)
			{
				int.TryParse(paralist[0].ToString(), out m_CurrentPageNum);
				int.TryParse(paralist[1].ToString(), out m_MaxPageNum);
				m_CurrentPageNum = ((m_CurrentPageNum > m_MaxPageNum) ? m_MaxPageNum : m_CurrentPageNum);
			}
			AddButtonClickListener(Panel.BgBtn, CloseBg);
			AddButtonClickListener(Panel.InputBtn, OpenBg);
			AddButtonClickListener(Panel.btn_QueRen, QueRenBtnClick);
			AddButtonClickListener(Panel.btn_BackSpace, BackBtnClick);
			AddButtonClickListener(Panel.btn_0, InputKey);
			AddButtonClickListener(Panel.btn_1, InputKey);
			AddButtonClickListener(Panel.btn_2, InputKey);
			AddButtonClickListener(Panel.btn_3, InputKey);
			AddButtonClickListener(Panel.btn_4, InputKey);
			AddButtonClickListener(Panel.btn_5, InputKey);
			AddButtonClickListener(Panel.btn_6, InputKey);
			AddButtonClickListener(Panel.btn_7, InputKey);
			AddButtonClickListener(Panel.btn_8, InputKey);
			AddButtonClickListener(Panel.btn_9, InputKey);
			MonoSingleton<GameStart>.Instance.StartCoroutine(RemainingSecondIEnumerator);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 2)
			{
				int.TryParse(paralist[0].ToString(), out m_CurrentPageNum);
				int.TryParse(paralist[1].ToString(), out m_MaxPageNum);
				m_CurrentPageNum = ((m_CurrentPageNum > m_MaxPageNum) ? m_MaxPageNum : m_CurrentPageNum);
			}
			Panel.InputBtn.interactable = true;
			m_RemainingSeconds = 3;
			Image component = Panel.InputBtn.gameObject.GetComponent<Image>();
			Text pageNumText = Panel.PageNumText;
			component.color = new Color(component.color.r, component.color.g, component.color.b, 1f);
			pageNumText.color = new Color(pageNumText.color.r, pageNumText.color.g, pageNumText.color.b, 1f);
			Panel.PageNumText.text = $"{m_CurrentPageNum}/{m_MaxPageNum}";
			base.GameObject.transform.SetAsLastSibling();
		}

		private IEnumerator RemainingSecond()
		{
			float alpha = 1f;
			while (true)
			{
				m_RemainingSeconds--;
				yield return new WaitForSeconds(1f);
				if (m_RemainingSeconds == 0)
				{
					Image _img = Panel.InputBtn.gameObject.GetComponent<Image>();
					Text _text = Panel.PageNumText;
					while (alpha > 0f)
					{
						alpha -= 0.03f;
						_img.color = new Color(_img.color.r, _img.color.g, _img.color.b, alpha);
						_text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
						yield return new WaitForSeconds(0.01f);
					}
					Panel.InputBtn.interactable = false;
					alpha = 1f;
				}
			}
		}

		private void CloseBg()
		{
			Panel.KeyBoardBg.gameObject.SetActive(value: false);
		}

		private void OpenBg()
		{
		}

		private void InputKey(Button _btn)
		{
			if (!(_btn == null))
			{
				int key = int.Parse(_btn.name);
				InputKey(key);
			}
		}

		private void InputKey(int key)
		{
			if (m_FirstInput)
			{
				if (key == 0)
				{
					key = 1;
				}
				Panel.PageNumText.text = $"{key}/{m_MaxPageNum}";
				m_FirstInput = false;
				return;
			}
			string text = Panel.PageNumText.text;
			string[] array = text.Split('/');
			text = ((array.Length != 2) ? key.ToString() : ((!(array[0] == "0")) ? (array[0] + key) : key.ToString()));
			if (int.Parse(text) > m_MaxPageNum)
			{
				text = m_MaxPageNum.ToString();
			}
			Panel.PageNumText.text = $"{text}/{m_MaxPageNum}";
			int.TryParse(text, out m_CurrentPageNum);
		}

		private void BackBtnClick()
		{
		}

		private void QueRenBtnClick()
		{
			if (ChangePageAction != null)
			{
				ChangePageAction(m_CurrentPageNum);
			}
		}

		public override void OnClose()
		{
			base.OnClose();
			MonoSingleton<GameStart>.Instance.StopCoroutine(RemainingSecondIEnumerator);
		}
	}
}
