using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanDouJiNengItem : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		public Button DianJiJiNeng;

		public Image MaskImage;

		public Image JiNengIcon;

		public Text JiNengName;

		public Text JiNengLevel;

		public Image ShengYuTimesImg;

		public Text ShengYuTimes;

		public Image NameBgImg;

		public GameObject SkillIconAni;

		private UnityAction<object, ZhanDouJiNengItem> MouseDownCallBack;

		private UnityAction<object, ZhanDouJiNengItem> MouseUpCallBack;

		private UnityAction<object, ZhanDouJiNengItem> CloseFaQiCallBack;

		private object dataSource;

		private SkillGroupType currentSkillType;

		private People roleInfo;

		private double ZhenYuan;

		private bool _isActive;

		private bool _MouseHaveDown;

		private SkillGroupType currentGroupType;

		public object DataSource
		{
			get
			{
				return dataSource;
			}
			private set
			{
				dataSource = value;
			}
		}

		public bool IsActive { get; private set; }

		private void Awake()
		{
			IsActive = false;
		}

		private void Update()
		{
			if (dataSource != null && roleInfo != null && ZhenYuan != roleInfo.CurrentSuperAir)
			{
				if (roleInfo != null)
				{
					ZhenYuan = roleInfo.CurrentSuperAir;
				}
				switch (currentSkillType)
				{
				case SkillGroupType.FaShu:
				{
					PlayerSkillModel playerSkillModel = dataSource as PlayerSkillModel;
					if (ZhenYuan < (double)(long)playerSkillModel.usedSuperAir)
					{
						CloseBtn();
					}
					else
					{
						OpenBtn();
					}
					break;
				}
				case SkillGroupType.FaQi:
				{
					PlayerFaQiModel playerFaQiModel = dataSource as PlayerFaQiModel;
					if (ZhenYuan < playerFaQiModel.minSpiritualityValue)
					{
						CloseBtn();
					}
					else
					{
						OpenBtn();
					}
					break;
				}
				}
			}
			if (!Input.GetMouseButtonUp(0) || !_MouseHaveDown)
			{
				return;
			}
			_MouseHaveDown = false;
			if (MouseUpCallBack != null)
			{
				if (!IsActive)
				{
					MouseUpCallBack(dataSource, this);
					return;
				}
				CloseFaQiCallBack(dataSource, this);
				CloseFaQi();
			}
		}

		public void SetItem(object _dataSource, SkillGroupType groupType, UnityAction<object, ZhanDouJiNengItem> _downCallBack, UnityAction<object, ZhanDouJiNengItem> _upCallBack, People _roleInfo, UnityAction<object, ZhanDouJiNengItem> _closeFaQiCallBack = null)
		{
			switch (groupType)
			{
			case SkillGroupType.FaShu:
			{
				currentGroupType = SkillGroupType.FaShu;
				PlayerSkillModel faShu = _dataSource as PlayerSkillModel;
				SetFaShu(faShu);
				break;
			}
			case SkillGroupType.FaQi:
			{
				currentGroupType = SkillGroupType.FaQi;
				PlayerFaQiModel faQi = _dataSource as PlayerFaQiModel;
				SetFaQi(faQi);
				break;
			}
			case SkillGroupType.FuLu:
			{
				currentGroupType = SkillGroupType.FuLu;
				PlayerFuLuModel fuLu = _dataSource as PlayerFuLuModel;
				SetFuLu(fuLu);
				break;
			}
			}
			currentSkillType = groupType;
			ZhenYuan = 0.0;
			roleInfo = _roleInfo;
			DataSource = _dataSource;
			MouseDownCallBack = _downCallBack;
			MouseUpCallBack = _upCallBack;
			CloseFaQiCallBack = _closeFaQiCallBack;
		}

		public void SetItem(object _dataSource, SkillGroupType groupType, UnityAction<object, ZhanDouJiNengItem> _downCallBack, UnityAction<object, ZhanDouJiNengItem> _upCallBack, UnityAction<object, ZhanDouJiNengItem> _closeFaQiCallBack = null)
		{
			switch (groupType)
			{
			case SkillGroupType.FaShu:
			{
				currentGroupType = SkillGroupType.FaShu;
				PlayerSkillModel faShu = _dataSource as PlayerSkillModel;
				SetFaShu(faShu);
				break;
			}
			case SkillGroupType.FaQi:
			{
				currentGroupType = SkillGroupType.FaQi;
				PlayerFaQiModel faQi = _dataSource as PlayerFaQiModel;
				SetFaQi(faQi);
				break;
			}
			case SkillGroupType.FuLu:
			{
				currentGroupType = SkillGroupType.FuLu;
				PlayerFuLuModel fuLu = _dataSource as PlayerFuLuModel;
				SetFuLu(fuLu);
				break;
			}
			}
			currentSkillType = groupType;
			ZhenYuan = 0.0;
			DataSource = _dataSource;
			MouseDownCallBack = _downCallBack;
			MouseUpCallBack = _upCallBack;
			CloseFaQiCallBack = _closeFaQiCallBack;
		}

		private void SetFaShu(PlayerSkillModel data)
		{
			if (data != null)
			{
				JiNengName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				JiNengLevel.text = $"{levelByRankName}·{data.level}";
				Sprite sprite = SelectFiveElement(data.fiveElement);
				if (sprite != null)
				{
					DianJiJiNeng.gameObject.GetComponent<Image>().sprite = sprite;
				}
				Sprite sprite2 = Singleton<FacadeManager>.Instance.SelectImgSprite(data.fiveElement, data.icon, 1);
				if (sprite2 != null)
				{
					JiNengIcon.sprite = sprite2;
				}
				DianJiJiNeng.interactable = true;
				NameBgImg.gameObject.SetActive(value: true);
				JiNengIcon.gameObject.SetActive(value: true);
			}
		}

		private void SetFaQi(PlayerFaQiModel data)
		{
			if (data != null)
			{
				JiNengName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				JiNengLevel.text = $"{levelByRankName}·{data.level}";
				Sprite sprite = SelectFiveElement(data.fiveElements);
				if (sprite != null)
				{
					DianJiJiNeng.gameObject.GetComponent<Image>().sprite = sprite;
				}
				Sprite sprite2 = Singleton<FacadeManager>.Instance.SelectImgSprite(data.fiveElements, data.icon, 2);
				if (sprite2 != null)
				{
					JiNengIcon.sprite = sprite2;
				}
				DianJiJiNeng.interactable = true;
				NameBgImg.gameObject.SetActive(value: true);
				JiNengIcon.gameObject.SetActive(value: true);
			}
		}

		private void SetFuLu(PlayerFuLuModel data)
		{
			if (data != null)
			{
				JiNengName.text = data.name;
				string levelByRankName = WorldSetting.GetLevelByRankName(data.rank);
				JiNengLevel.text = $"{levelByRankName}·{data.level}";
				Sprite sprite = SelectFiveElement(data.fiveElements);
				if (sprite != null)
				{
					DianJiJiNeng.gameObject.GetComponent<Image>().sprite = sprite;
				}
				Sprite sprite2 = Singleton<FacadeManager>.Instance.SelectImgSprite(data.fiveElements, data.icon, 3);
				if (sprite2 != null)
				{
					JiNengIcon.sprite = sprite2;
				}
				DianJiJiNeng.interactable = true;
				NameBgImg.gameObject.SetActive(value: true);
				JiNengIcon.gameObject.SetActive(value: true);
				ShengYuTimesImg.gameObject.SetActive(value: true);
				ShengYuTimes.text = data.count.ToString();
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

		private void CloseBtn()
		{
			DianJiJiNeng.interactable = false;
		}

		private void OpenBtn()
		{
			DianJiJiNeng.interactable = true;
		}

		public void CloseFaQi()
		{
			SkillIconAni.gameObject.SetActive(value: false);
			IsActive = false;
		}

		public void OpenFaQi()
		{
			SkillIconAni.gameObject.SetActive(value: true);
			IsActive = true;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			_MouseHaveDown = true;
			if (MouseDownCallBack != null)
			{
				MouseDownCallBack(dataSource, this);
			}
		}
	}
}
