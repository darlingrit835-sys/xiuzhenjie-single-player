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

		private UnityAction<object, SkillGroupType> ClickItemCallBack;

		private float m_CurrentCD;

		private float m_CD;

		private void Update()
		{
			if (!(m_CurrentCD > 0f))
			{
				return;
			}
			m_CurrentCD -= Time.deltaTime;
			float num = 1f / m_CD * Time.deltaTime;
			CDMask_Img.fillAmount -= num;
			if (m_CurrentCD <= 0f)
			{
				m_CurrentCD = 0f;
				m_CD = 0f;
				if (CDMask_Img.gameObject.activeSelf)
				{
					CDMask_Img.gameObject.SetActive(value: false);
					CDMask_Img.fillAmount = 1f;
				}
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
			CDMask_Img.gameObject.SetActive(value: true);
			m_CurrentCD = (float)_time / 1000f;
			m_CD = m_CurrentCD;
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
