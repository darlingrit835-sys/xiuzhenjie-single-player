using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class Battle_SkillItem : MonoBehaviour
	{
		public Image SkillIcon;

		public Image CircleCD_Img;

		public Image CDMask_Img;

		public Animation WeaponRunningAni;

		public Text LevelAndRank;

		public Text SkillName;

		public Button InputBtn;

		private object m_DateSource;

		private SkillGroupType m_ThisSkillType;

		internal SkillGroupType Kind => m_ThisSkillType;

		private UnityAction<object, SkillGroupType> ClickItemCallBack;

		private float m_CurrentCD;

		private float m_CD;

		private Text m_CooldownText;
		private int m_ShownSeconds;
		private Image m_Flash;
		private float m_FlashTime;
		private Color m_FlashColor;

		private void Awake()
		{
			((RectTransform)transform).sizeDelta = new Vector2(57,76);
			InkInterfaceArt.Set(InputBtn.image,"JadeButton",false);
			transform.Find("Image").GetComponent<Image>().enabled = false;
			SkillName.color = InkDialogArt.Ivory;
			LevelAndRank.color = InkBattleHud.Gold;
			var top = new Vector2(.5f,1f);
			InkBattleHud.Place(SkillIcon.rectTransform,top,0,-5,36,36);
			InkBattleHud.Place(CircleCD_Img.rectTransform,top,0,-2,42,42);
			InkBattleHud.Place(CDMask_Img.rectTransform,top,0,-2,42,42);
			var rim = transform.Find("GrayBg") as RectTransform;
			if (rim != null) InkBattleHud.Place(rim,top,0,-4,38,38);
			InkBattleHud.Band(SkillName.rectTransform,1,1,42,17);
			InkBattleHud.Band(LevelAndRank.rectTransform,1,1,60,13);
			SkillName.resizeTextForBestFit = LevelAndRank.resizeTextForBestFit = true;
			SkillName.resizeTextMinSize = LevelAndRank.resizeTextMinSize = 7;
			SkillName.fontSize = SkillName.resizeTextMaxSize = 14;
			LevelAndRank.fontSize = LevelAndRank.resizeTextMaxSize = 11;
			var flash = new GameObject("QiRing",typeof(RectTransform),typeof(Image));
			flash.layer = gameObject.layer;
			flash.transform.SetParent(CircleCD_Img.transform.parent,false);
			m_Flash = flash.GetComponent<Image>();
			m_Flash.sprite = CircleCD_Img.sprite;
			m_Flash.raycastTarget = false;
			m_Flash.enabled = false;
			InkBattleHud.Place(m_Flash.rectTransform,top,0,-2,42,42);
			var label = new GameObject("CooldownSeconds", typeof(RectTransform), typeof(Text));
			label.layer = gameObject.layer;
			label.transform.SetParent(CDMask_Img.transform, false);
			m_CooldownText = label.GetComponent<Text>();
			InkDialogArt.Inset(m_CooldownText.rectTransform, 1f, 1f, 1f, 1f);
			m_CooldownText.font = SkillName.font;
			m_CooldownText.fontSize = 18;
			m_CooldownText.fontStyle = FontStyle.Bold;
			m_CooldownText.color = InkDialogArt.Ivory;
			m_CooldownText.alignment = TextAnchor.MiddleCenter;
			m_CooldownText.raycastTarget = false;
			var outline = label.AddComponent<Outline>();
			outline.effectColor = InkInterfaceArt.Ink;
			outline.effectDistance = new Vector2(1f, -1f);
			CDMask_Img.raycastTarget = false;
		}

		private void Update()
		{
			if (m_FlashTime > 0f)
			{
				m_FlashTime = Mathf.Max(0f,m_FlashTime-Time.unscaledDeltaTime);
				float light = m_FlashTime / .32f;
				m_FlashColor.a = light;
				m_Flash.color = m_FlashColor;
				m_Flash.rectTransform.localScale = Vector3.one * Mathf.Lerp(1.36f,.83f,light);
				m_Flash.rectTransform.anchoredPosition = new Vector2(0,-23 + 21*m_Flash.rectTransform.localScale.y);
				m_Flash.enabled = m_FlashTime > 0f;
			}
			if (m_CurrentCD <= 0f) return;
			m_CurrentCD = Mathf.Max(0f, m_CurrentCD - Time.deltaTime);
			CDMask_Img.fillAmount = m_CurrentCD / m_CD;
			if (m_CurrentCD == 0f)
			{
				CDMask_Img.gameObject.SetActive(false);
				Pulse(new Color32(149,238,208,255));
			}
			else
				DisplayCooldown();
		}

		private void Pulse(Color tint)
		{
			m_FlashColor = tint;
			m_FlashTime = .32f;
			m_Flash.enabled = true;
		}

		private void DisplayCooldown()
		{
			int seconds = Mathf.CeilToInt(m_CurrentCD);
			if (seconds != m_ShownSeconds)
			{
				m_ShownSeconds = seconds;
				m_CooldownText.text = seconds.ToString();
			}
		}

		public void SetItem(object _dataSource, Dictionary<int, Sprite> _iconDic, SkillGroupType _ThisSkillType, UnityAction<object, SkillGroupType> _clickItemCallBack)
		{
			m_DateSource = _dataSource;
			m_ThisSkillType = _ThisSkillType;
			InputBtn.onClick.RemoveAllListeners();
			InputBtn.onClick.AddListener(() =>
			{
				if (!(m_CurrentCD > 0f) && _clickItemCallBack != null)
				{
					Pulse(InkBattleHud.Gold);
					_clickItemCallBack(m_DateSource, m_ThisSkillType);
				}
			});
			SetInfo(_iconDic);
		}

		private void SetInfo(Dictionary<int, Sprite> _iconDic)
		{
			BaseSkillModel data = m_DateSource as BaseSkillModel;
			switch (m_ThisSkillType)
			{
			case SkillGroupType.FaShu:
				SetFaShu(data, _iconDic);
				break;
			case SkillGroupType.FaQi:
				SetFaQi(data, _iconDic);
				break;
			case SkillGroupType.FuLu:
				SetFuLu(data, _iconDic);
				break;
			case SkillGroupType.ZhenFa:
				SetZhenFa(data, _iconDic);
				break;
			}
		}

		public void SetCD(double _time)
		{
			m_CurrentCD = Mathf.Max(0f, (float)_time / 1000f);
			m_CD = m_CurrentCD;
			CDMask_Img.fillAmount = 1f;
			CDMask_Img.gameObject.SetActive(m_CurrentCD > 0f);
			if (m_CurrentCD > 0f)
				DisplayCooldown();
		}

		private void SetFaShu(BaseSkillModel data, Dictionary<int, Sprite> _iconDic)
		{
			if (data != null)
			{
				SkillName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				string signleWuXingName = WorldSetting.GetSignleWuXingName((WuXing)data.fiveElement);
				LevelAndRank.text = $"{signleWuXingName}·{levelByRankName}·{data.level}";
				Sprite value = null;
				if (_iconDic.TryGetValue(data.skillType, out value))
				{
					SkillIcon.sprite = value;
				}
			}
		}

		private void SetFaQi(BaseSkillModel data, Dictionary<int, Sprite> _iconDic)
		{
			if (data != null)
			{
				SkillName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				string signleWuXingName = WorldSetting.GetSignleWuXingName((WuXing)data.fiveElement);
				LevelAndRank.text = $"{signleWuXingName}·{levelByRankName}·{data.level}";
				Sprite value = null;
				if (_iconDic.TryGetValue(data.skillType, out value))
				{
					SkillIcon.sprite = value;
				}
			}
		}

		public void SetFaQiRunningAni(bool isShowAni)
		{
			if (!(WeaponRunningAni == null))
			{
				if (!isShowAni)
				{
					WeaponRunningAni.transform.localEulerAngles = Vector3.zero;
					WeaponRunningAni.Stop();
				}
				else
				{
					WeaponRunningAni.Play();
				}
				WeaponRunningAni.enabled = isShowAni;
			}
		}

		private void SetFuLu(BaseSkillModel data, Dictionary<int, Sprite> _iconDic)
		{
			if (data != null)
			{
				SkillName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				string signleWuXingName = WorldSetting.GetSignleWuXingName((WuXing)data.fiveElement);
				LevelAndRank.text = $"{signleWuXingName}·{levelByRankName}·{data.level}";
				Sprite value = null;
				if (_iconDic.TryGetValue(21, out value))
				{
					SkillIcon.sprite = value;
				}
			}
		}

		private void SetZhenFa(BaseSkillModel data, Dictionary<int, Sprite> _iconDic)
		{
			if (data != null)
			{
				SkillName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				string signleWuXingName = WorldSetting.GetSignleWuXingName((WuXing)data.fiveElement);
				LevelAndRank.text = $"{signleWuXingName}·{levelByRankName}·{data.level}";
				Sprite value = null;
				if (_iconDic.TryGetValue(20, out value))
				{
					SkillIcon.sprite = value;
				}
			}
		}

		private Sprite SelectFiveElement(int index)
		{
			Sprite result = null;
			switch (index)
			{
			case 1:
				result = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingJin.png");
				break;
			case 2:
				result = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingMu.png");
				break;
			case 3:
				result = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingShui.png");
				break;
			case 4:
				result = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingHuo.png");
				break;
			case 5:
				result = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingTu.png");
				break;
			}
			return result;
		}
	}
}
