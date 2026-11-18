using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XiuZhenJie;

public class Window
{
	protected List<Button> m_AllButton = new List<Button>();

	protected List<Toggle> m_AllToggle = new List<Toggle>();

	protected List<InputField> m_AllInput = new List<InputField>();

	public GameObject GameObject { get; set; }

	public Transform Transform { get; set; }

	public bool Resource { get; set; }

	public string Name { get; set; }

	public virtual bool OnMessage(UIMsgID msgID, params object[] paralist)
	{
		return true;
	}

	public virtual void Awake(params object[] paralist)
	{
	}

	public virtual void OnShow(params object[] paralist)
	{
		GameObject.SetActive(value: true);
	}

	public virtual void OnDisable()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnFixUpdate()
	{
	}

	public virtual void OnClose()
	{
		RemoveAllButtonListener();
		RemoveAllToggleListener();
		RemoveAllInputListener();
		m_AllButton.Clear();
		m_AllToggle.Clear();
		m_AllInput.Clear();
	}

	public virtual void ReFlashData()
	{
	}

	public bool ChangeImageSprite(string path, Image image, bool setNativeSize = false)
	{
		if (image == null)
		{
			return false;
		}
		Sprite sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>(path);
		if (sprite != null)
		{
			if (image.sprite != null)
			{
				image.sprite = null;
			}
			image.sprite = sprite;
			if (setNativeSize)
			{
				image.SetNativeSize();
			}
			return true;
		}
		return false;
	}

	public bool ChangeImageSprite(Sprite sprite, Image image, bool setNativeSize = false)
	{
		if (image == null || sprite == null)
		{
			return false;
		}
		if (sprite != null)
		{
			if (image.sprite != null)
			{
				image.sprite = null;
			}
			image.sprite = sprite;
			if (setNativeSize)
			{
				image.SetNativeSize();
			}
			return true;
		}
		return false;
	}

	public void SetIconImg(Sprite sprite, Image image)
	{
		image.sprite = sprite;
		image.gameObject.SetActive(value: true);
	}

	public void ChangImageSpriteAsync(string path, Image image, bool setNativeSize = false)
	{
		if (!(image == null))
		{
			Singleton<ResourceManager>.Instance.AsyncLoadResource(path, OnLoadSpriteFinish, LoadResPriority.RES_MIDDLE, isSprite: true, image, setNativeSize);
		}
	}

	private void OnLoadSpriteFinish(string path, Object obj, object param1 = null, object param2 = null, object param3 = null)
	{
		if (obj != null)
		{
			Sprite sprite = obj as Sprite;
			Image image = param1 as Image;
			bool num = (bool)param2;
			if (image.sprite != null)
			{
				image.sprite = null;
			}
			image.sprite = sprite;
			if (num)
			{
				image.SetNativeSize();
			}
		}
	}

	public void RemoveAllButtonListener()
	{
		foreach (Button item in m_AllButton)
		{
			item.onClick.RemoveAllListeners();
		}
	}

	public void RemoveAllToggleListener()
	{
		foreach (Toggle item in m_AllToggle)
		{
			item.onValueChanged.RemoveAllListeners();
		}
	}

	public void RemoveAllInputListener()
	{
		foreach (InputField item in m_AllInput)
		{
			item.onEndEdit.RemoveAllListeners();
		}
	}

	public void AddInputEndEditListener(InputField input, UnityAction<string> action)
	{
		if (input != null)
		{
			if (!m_AllInput.Contains(input))
			{
				m_AllInput.Add(input);
			}
			input.onEndEdit.RemoveAllListeners();
			input.onEndEdit.AddListener(action);
		}
	}

	public void AddButtonClickListener(Button btn, UnityAction action)
	{
		if (btn != null)
		{
			if (!m_AllButton.Contains(btn))
			{
				m_AllButton.Add(btn);
			}
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener(BtnPlaySound);
			btn.onClick.AddListener(action);
		}
	}

	public void AddButtonClickListener(Button btn, UnityAction<Button> action)
	{
		if (!(btn != null))
		{
			return;
		}
		if (!m_AllButton.Contains(btn))
		{
			m_AllButton.Add(btn);
		}
		btn.onClick.RemoveAllListeners();
		btn.onClick.AddListener(BtnPlaySound);
		btn.onClick.AddListener(() =>
		{
			if (action != null)
			{
				action(btn);
			}
		});
	}

	public void AddToggleClickListener(Toggle toggle, UnityAction<bool> action)
	{
		if (toggle != null)
		{
			if (!m_AllToggle.Contains(toggle))
			{
				m_AllToggle.Add(toggle);
			}
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener(TogglePlaySound);
			toggle.onValueChanged.AddListener(action);
		}
	}

	public virtual void CloseWindow()
	{
		Singleton<UIManager>.Instance.CloseWnd(this);
	}

	private void BtnPlaySound()
	{
		Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.BtnClick);
	}

	private void TogglePlaySound(bool isOn)
	{
		if (isOn)
		{
			Singleton<VoiceManager>.Instance.PlayVoice(VoiceType.BtnClick);
		}
	}

	public GameObject SetListContentParent(RectTransform ParentContent, GameObject insObj)
	{
		GameObject gameObject = Object.Instantiate(insObj);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(ParentContent);
		component.localScale = Vector3.one;
		component.eulerAngles = Vector3.zero;
		return gameObject;
	}

	public float SetVerticalListContentHight(int itemCount, float itemHight, VerticalLayoutGroup vlgroup)
	{
		if (vlgroup == null)
		{
			return 0f;
		}
		float num = vlgroup.padding.top;
		float spacing = vlgroup.spacing;
		float num2 = (float)itemCount * itemHight + (float)(itemCount - 1) * spacing + 2f * num;
		if (!(num2 < 0f))
		{
			return num2;
		}
		return 0f;
	}

	public float SetVerticalListContentHight(int itemCount, float itemHight, float topPading, float verticalSpacing)
	{
		float num = (float)itemCount * itemHight + (float)(itemCount - 1) * verticalSpacing + 2f * topPading;
		if (!(num < 0f))
		{
			return num;
		}
		return 0f;
	}

	public float SetHorizontalListContentHight(int itemCount, float itemWight, HorizontalLayoutGroup hlgroup)
	{
		float num = hlgroup.padding.left;
		float spacing = hlgroup.spacing;
		return (float)itemCount * itemWight + (float)(itemCount - 1) * spacing + 2f * num;
	}

	public float SetGridLayOutContentWight(int count, GridLayoutGroup glroup)
	{
		float x = glroup.spacing.x;
		float num = glroup.padding.left;
		float x2 = glroup.cellSize.x;
		return (float)count * x2 + (float)(count - 1) * x + 2f * num;
	}

	public float SetHorizontalLayoutContentWight(int count, float itemHight, HorizontalLayoutGroup hlroup)
	{
		_ = hlroup.padding.left;
		float spacing = hlroup.spacing;
		return (float)count * itemHight + (float)(count - 1) * spacing;
	}

	public float SetGrticalListContentHight(int itemCount, GridLayoutGroup glroup)
	{
		float y = glroup.spacing.y;
		float num = glroup.padding.top;
		float y2 = glroup.cellSize.y;
		float num2 = (float)itemCount * y2 + (float)(itemCount - 1) * y + 2f * num;
		if (!(num2 < 0f))
		{
			return num2;
		}
		return 0f;
	}

	public void SetGrticalListContentHight(int itemCount, RectTransform rect, GridLayoutGroup glroup)
	{
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);
		_ = glroup.spacing;
		float y = glroup.spacing.y;
		float num = glroup.padding.top;
		_ = glroup.padding.left;
		float y2 = glroup.cellSize.y;
		float x = glroup.cellSize.x;
		float x2 = rect.rect.size.x;
		if (x2 != 0f)
		{
			int num2 = 0;
			num2 = ((glroup.constraint != GridLayoutGroup.Constraint.FixedColumnCount) ? GetOneRowNum(rect, glroup) : glroup.constraintCount);
			int num3 = ((num2 != 0) ? (itemCount % num2) : 0);
			int num4 = 1;
			if (num3 == 0 && num2 > 0)
			{
				num4 = itemCount / num2;
			}
			else if (num2 > 0)
			{
				num4 = itemCount / num2 + 1;
			}
			float num5 = (x2 - (float)num2 * x) / (float)(num2 + 1);
			glroup.spacing = new Vector2(num5, glroup.spacing.y);
			glroup.padding.left = (int)num5;
			float num6 = (float)num4 * y2 + (float)(num4 - 1) * y + 2f * num;
			num6 = ((num6 < 0f) ? 0f : num6);
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, num6);
		}
	}

	public int GetOneRowNum(RectTransform rect, GridLayoutGroup glroup)
	{
		float x = glroup.spacing.x;
		float x2 = glroup.cellSize.x;
		float num = rect.rect.size.x;
		int num2 = 0;
		while (true)
		{
			if (x2 == 0f)
			{
				return num2;
			}
			if (!(num >= x2))
			{
				break;
			}
			num -= x2;
			num2++;
			if (num > x)
			{
				num -= x;
				continue;
			}
			return num2;
		}
		return num2;
	}

	public void GradientShowOrHideImg(Image targetImg, float times, bool isShow, UnityAction compeleteCallBack)
	{
		if (isShow)
		{
			targetImg.color = new Color(targetImg.color.r, targetImg.color.g, targetImg.color.b, 0f);
			targetImg.gameObject.SetActive(value: true);
			targetImg.material.DOColor(new Color(targetImg.color.r, targetImg.color.g, targetImg.color.b, times), 1f).onComplete = () =>
			{
				if (compeleteCallBack != null)
				{
					compeleteCallBack();
				}
			};
			return;
		}
		targetImg.material.DOColor(new Color(targetImg.color.r, targetImg.color.g, targetImg.color.b, 0f), times).onComplete = () =>
		{
			targetImg.color = new Color(targetImg.color.r, targetImg.color.g, targetImg.color.b, 1f);
			targetImg.gameObject.SetActive(value: false);
			if (compeleteCallBack != null)
			{
				compeleteCallBack();
			}
		};
	}
}
