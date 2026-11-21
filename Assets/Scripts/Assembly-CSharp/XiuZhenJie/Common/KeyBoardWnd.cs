using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class KeyBoardWnd : Window
	{
		public KeyBoardPanel Panel;
		public Action<int> ChangePageAction;

		private int currentPage = 1;
		private int maxPage = 1;
		private int enteredPage;
		private bool replaceDigits;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = GameObject.GetComponent<KeyBoardPanel>();
			// The recovered prefab assigns InputBtn to digit 1. Bind the visible page counter itself.
			var pageCounter = Panel.PageNumText.transform.parent.GetComponent<Image>();
			Panel.InputBtn = pageCounter.GetComponent<Button>() ?? pageCounter.gameObject.AddComponent<Button>();
			Panel.InputBtn.targetGraphic = pageCounter;
			pageCounter.raycastTarget = true;
			pageCounter.rectTransform.sizeDelta = new Vector2(94f, 32f);
			Panel.PageNumText.fontSize = 15;
			InkDialogArt.StyleButton(Panel.InputBtn, "JadeButton", Color.white);
			InkInterfaceArt.Set(Panel.KeyBoardBg, "Parchment", false);
			AddButtonClickListener(Panel.BgBtn, CloseBg);
			AddButtonClickListener(Panel.InputBtn, OpenBg);
			AddButtonClickListener(Panel.btn_QueRen, Confirm);
			AddButtonClickListener(Panel.btn_BackSpace, Backspace);
			InkDialogArt.StyleButton(Panel.btn_QueRen, "JadeButton", Color.white);
			Button[] digits = { Panel.btn_0, Panel.btn_1, Panel.btn_2, Panel.btn_3, Panel.btn_4,
				Panel.btn_5, Panel.btn_6, Panel.btn_7, Panel.btn_8, Panel.btn_9 };
			foreach (Button button in digits)
			{
				int digit = int.Parse(button.name);
				AddButtonClickListener(button, () => InputDigit(digit));
				InkDialogArt.StyleButton(button, "LightPlaque", InkInterfaceArt.Ink);
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 2)
			{
				int.TryParse(paralist[1]?.ToString(), out maxPage);
				int.TryParse(paralist[0]?.ToString(), out currentPage);
			}
			maxPage = Mathf.Max(1, maxPage);
			currentPage = Mathf.Clamp(currentPage, 1, maxPage);
			Panel.InputBtn.interactable = maxPage > 1;
			CloseBg();
			GameObject.transform.SetAsLastSibling();
		}

		private void OpenBg()
		{
			enteredPage = currentPage;
			replaceDigits = true;
			SetPadVisible(true);
		}

		private void CloseBg()
		{
			enteredPage = currentPage;
			SetPadVisible(false);
		}

		private void SetPadVisible(bool visible)
		{
			Panel.KeyBoardBg.gameObject.SetActive(visible);
			Panel.BgBtn.image.raycastTarget = visible;
			Panel.BgBtn.image.color = visible ? new Color(0.08f, 0.14f, 0.12f, 0.44f) : Color.clear;
			ShowPage();
		}

		private void ShowPage()
		{
			Panel.PageNumText.text = $"{enteredPage} / {maxPage}";
			Panel.btn_QueRen.interactable = enteredPage > 0;
		}

		private void InputDigit(int digit)
		{
			long previous = replaceDigits ? 0 : enteredPage;
			enteredPage = (int)Math.Min(previous * 10 + digit, maxPage);
			replaceDigits = false;
			ShowPage();
		}

		private void Backspace()
		{
			enteredPage /= 10;
			replaceDigits = false;
			ShowPage();
		}

		private void Confirm()
		{
			if (enteredPage < 1)
				return;
			currentPage = enteredPage;
			CloseBg();
			ChangePageAction?.Invoke(currentPage);
		}
	}
}
