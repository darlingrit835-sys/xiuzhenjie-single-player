using System;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LianZhiBuZhouSheZhiItem : MonoBehaviour
	{
		public Button ShanChuButton;

		public Text ShunXuText;

		public Text MiaoShuText;

		public Text TouZhiText;

		public Text CaoYaoNameText;

		public Toggle toggle;

		public string index;

		public int _index;

		public void SetValue(int index, string MiaoShu, ToggleGroup group, Action<bool, int, LianZhiBuZhouSheZhiItem> togClick, Action<LianZhiBuZhouSheZhiItem, string> action)
		{
			if (this.index.Length <= 0)
			{
				_index = index;
				this.index = index.ToString();
				ShunXuText.text = this.index;
			}
			MiaoShuText.text = MiaoShu;
			toggle.group = group;
			ShanChuButton.onClick.AddListener(() =>
			{
				if (action != null)
				{
					action(this, this.index);
				}
			});
			toggle.onValueChanged.AddListener((bool isOn) =>
			{
				ToggleClick(isOn);
				if (togClick != null)
				{
					togClick(isOn, _index, this);
				}
			});
			ShanChuButton.gameObject.SetActive(value: false);
		}

		public void SetValue(int index, string MiaoShu, string CaoYaoName, ToggleGroup group, Action<bool, int, LianZhiBuZhouSheZhiItem> togClick, Action<LianZhiBuZhouSheZhiItem, string> action)
		{
			if (this.index.Length <= 0)
			{
				_index = index;
				this.index = index.ToString();
				ShunXuText.text = this.index;
			}
			MiaoShuText.gameObject.SetActive(value: false);
			TouZhiText.gameObject.SetActive(value: true);
			CaoYaoNameText.gameObject.SetActive(value: true);
			CaoYaoNameText.text = string.Format(CaoYaoNameText.text, CaoYaoName);
			toggle.group = group;
			ShanChuButton.onClick.AddListener(() =>
			{
				if (action != null)
				{
					action(this, this.index);
				}
			});
			toggle.onValueChanged.AddListener((bool isOn) =>
			{
				ToggleClick(isOn);
				if (togClick != null)
				{
					togClick(isOn, _index, this);
				}
			});
			ShanChuButton.gameObject.SetActive(value: false);
		}

		public void ResetValue(string MiaoShu)
		{
			MiaoShuText.gameObject.SetActive(value: true);
			TouZhiText.gameObject.SetActive(value: false);
			CaoYaoNameText.gameObject.SetActive(value: false);
			MiaoShuText.text = MiaoShu;
		}

		public void ResetValue(string MiaoShu, string CaoYaoName)
		{
			MiaoShuText.gameObject.SetActive(value: false);
			TouZhiText.gameObject.SetActive(value: true);
			CaoYaoNameText.gameObject.SetActive(value: true);
			CaoYaoNameText.text = string.Format(CaoYaoNameText.text, CaoYaoName);
		}

		private void ToggleClick(bool val)
		{
			if (val)
			{
				MiaoShuText.color = new Color(1f, 1f, 1f, 0.8f);
				TouZhiText.color = new Color(1f, 1f, 1f, 0.8f);
				CaoYaoNameText.color = new Color(1f, 1f, 1f, 0.5f);
				ShunXuText.color = new Color(1f, 1f, 1f, 0.5f);
				ShanChuButton.gameObject.SetActive(value: true);
			}
			else
			{
				MiaoShuText.color = new Color(0f, 0f, 0f, 1f);
				TouZhiText.color = new Color(0f, 0f, 0f, 1f);
				CaoYaoNameText.color = new Color(0f, 0f, 0f, 0.5f);
				ShunXuText.color = new Color(0f, 0f, 0f, 0.5f);
				ShanChuButton.gameObject.SetActive(value: false);
			}
		}
	}
}
