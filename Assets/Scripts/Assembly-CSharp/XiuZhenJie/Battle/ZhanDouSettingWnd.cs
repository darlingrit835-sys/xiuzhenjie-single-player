using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	[Obsolete("该类已经废弃", true)]
	public class ZhanDouSettingWnd : Window
	{
		public ZhanMenPanel Panel;

		private RoleInfoModel m_RoleInfo;

		private Dictionary<int, Sprite> m_WuXingBgSprite;

		private List<RoleBattleSettingModel> m_FaShuList;

		private List<RoleBattleSettingModel> m_FaQiList;

		private List<RoleBattleSettingModel> m_FuLuList;

		private List<RoleBattleSettingModel> m_ZhenFaList;

		private DouMenXuanZeJiNengItem m_ChooseItem;

		private RoleBattleSettingModel m_CurrentOpenedTargetDataSource;

		private const int FASHULIMIT_ONE = 500;

		private const int FASHULIMIT_TWO = 1000;

		private const int FASHULIMIT_THREE = 1475;

		private const int FASHULIMIT_FOUR = 3600;

		private const int FAQILIMIT_ONE = 500;

		private const int FAQILIMIT_TWO = 1100;

		private const int FAQILIMIT_THREE = 1900;

		private const int FAQILIMIT_FOUR = 7000;

		private const int FULULIMIT_ONE = 500;

		private const int FULULIMIT_TWO = 1000;

		private const int FULULIMIT_THREE = 1800;

		private const int FULULIMIT_FOUR = 3600;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZhanMenPanel>();
			ResourceLoad();
			AddButtonClickListener(Panel.ZhiDaoLeButton, ShowSheZhiGameObject);
			AddButtonClickListener(Panel.CLoseJieSuoButton, ShowSheZhiGameObject);
			AddButtonClickListener(Panel.ZheZhaoBtn, () =>
			{
				Panel.XuanZeGameObject.gameObject.transform.DOLocalMoveY(-687f, 0.5f);
				Panel.ZheZhaoBtn.gameObject.SetActive(value: false);
			});
			AddButtonClickListener(Panel.FaShu_1, FaShu_1_Click);
			AddButtonClickListener(Panel.FaShu_2, FaShu_2_Click);
			AddButtonClickListener(Panel.FaShu_3, FaShu_3_Click);
			AddButtonClickListener(Panel.FaShu_4, FaShu_4_Click);
			AddButtonClickListener(Panel.FaQi_1, FaQi_1_Click);
			AddButtonClickListener(Panel.FaQi_2, FaQi_2_Click);
			AddButtonClickListener(Panel.FaQi_3, FaQi_3_Click);
			AddButtonClickListener(Panel.FaQi_4, FaQi_4_Click);
			AddButtonClickListener(Panel.FuLu_1, FuLu_1_Click);
			AddButtonClickListener(Panel.FuLu_2, FuLu_2_Click);
			AddButtonClickListener(Panel.FuLu_3, FuLu_3_Click);
			AddButtonClickListener(Panel.FuLu_4, FuLu_4_Click);
			AddButtonClickListener(Panel.BackBtn, BackBtn);
		}

		private void BackBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			InitData();
		}

		private void ResourceLoad()
		{
			m_WuXingBgSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingAll.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingJin.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingMu.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingShui.png");
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingHuo.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/ZhanDouSettingTu.png");
			m_WuXingBgSprite.Add(0, value);
			m_WuXingBgSprite.Add(1, value2);
			m_WuXingBgSprite.Add(2, value3);
			m_WuXingBgSprite.Add(3, value4);
			m_WuXingBgSprite.Add(4, value5);
			m_WuXingBgSprite.Add(5, value6);
		}

		private void InitData()
		{
			m_FaShuList = new List<RoleBattleSettingModel>();
			m_FuLuList = new List<RoleBattleSettingModel>();
			m_FaQiList = new List<RoleBattleSettingModel>();
			List<RoleBattleSettingModel> list = null;
			if (list == null)
			{
				return;
			}
			foreach (RoleBattleSettingModel item in list)
			{
				if (item.d)
				{
					switch (item.g)
					{
					case 1:
						m_FaShuList.Add(item);
						break;
					case 2:
						m_FuLuList.Add(item);
						break;
					case 3:
						m_FaQiList.Add(item);
						break;
					}
				}
			}
		}

		private void ShowSheZhiGameObject()
		{
			Panel.JieSuoGameObject.SetActive(value: false);
		}

		private void ShowJieSuoGameObject(string shenShi)
		{
			Panel.SheZhiGameObject.SetActive(value: true);
			Panel.JieSuoGameObject.SetActive(value: true);
			Panel.XuanZeGameObject.SetActive(value: false);
			SetJieSuoGameObject(shenShi);
		}

		private void SetJieSuoGameObject(string ShenShi)
		{
			Panel.JieSuoMiaoShu.text = $"神识达到{ShenShi}即可自动解锁";
		}

		private void SetXuanZeGameObject(RoleBattleSettingModel currentItem, List<RoleBattleSettingItemModel> dataSource, int type, SettingItemType settingType, Action callBack)
		{
			ReflashXuanZeData(currentItem, dataSource, type, settingType, callBack);
			Panel.XuanZeGameObject.gameObject.transform.DOLocalMoveY(-61.73f, 0.5f);
		}

		private void ReflashXuanZeData(RoleBattleSettingModel currentItem, List<RoleBattleSettingItemModel> dataSource, int type, SettingItemType settingType, Action callBack)
		{
			m_CurrentOpenedTargetDataSource = currentItem;
			List<string> list = new List<string>();
			List<RoleBattleSettingItemModel> list2 = new List<RoleBattleSettingItemModel>();
			if (dataSource != null)
			{
				foreach (RoleBattleSettingItemModel item in dataSource)
				{
					if (list.Contains(item.id))
					{
						list2.Add(item);
					}
				}
				foreach (RoleBattleSettingItemModel item2 in list2)
				{
					dataSource.Remove(item2);
				}
			}
			switch (type)
			{
			case 1:
				Panel.CurrentTitleText.text = "当前法术";
				Panel.KeYongListTitleText.text = "可用法术";
				if (dataSource == null)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("无已经参悟的法术,请去洞府参悟法术");
					return;
				}
				break;
			case 2:
				Panel.CurrentTitleText.text = "当前法器";
				Panel.KeYongListTitleText.text = "可用法器";
				if (dataSource == null)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("无可用法器");
					return;
				}
				break;
			case 3:
				Panel.CurrentTitleText.text = "当前符箓";
				Panel.KeYongListTitleText.text = "可用符箓";
				if (dataSource == null)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("无可用符箓");
					return;
				}
				break;
			}
			if (currentItem != null && !string.IsNullOrEmpty(currentItem.c))
			{
				Panel.DangQianXuanZeJiNengBg.gameObject.SetActive(value: true);
				Panel.DangQianXuanZeJiNengText.text = currentItem.a;
				string levelByRankName = WorldSetting.GetLevelByRankName(currentItem.i);
				Panel.DangQianXuanZeJiNengLevelText.text = $"{levelByRankName}·{currentItem.h}";
				Sprite sprite = Singleton<FacadeManager>.Instance.SelectImgSprite(currentItem.e, currentItem.f, type);
				Panel.DangQianXuanZeJiNengImag.sprite = sprite;
				Sprite value = null;
				m_WuXingBgSprite.TryGetValue(currentItem.e, out value);
				if (value != null)
				{
					Panel.DangQianXuanZeJiNengBg.sprite = value;
				}
			}
			else
			{
				Panel.DangQianXuanZeJiNengBg.gameObject.SetActive(value: false);
			}
			foreach (Transform item3 in Panel.XuanZeJiNengListContent)
			{
				UnityEngine.Object.Destroy(item3.gameObject);
			}
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DouMenXuanXeFaShuItem.prefab");
			_ = gameObject.GetComponent<RectTransform>().sizeDelta;
			GridLayoutGroup component = Panel.XuanZeJiNengListContent.GetComponent<GridLayoutGroup>();
			foreach (RoleBattleSettingItemModel item4 in dataSource)
			{
				SetListContentParent(Panel.XuanZeJiNengListContent, gameObject).GetComponent<DouMenXuanZeJiNengItem>();
				Singleton<FacadeManager>.Instance.SelectImgSprite(item4.fiveElements, item4.icon, type);
			}
			SetGrticalListContentHight(dataSource.Count, Panel.XuanZeJiNengListContent, component);
			Panel.XuanDingButton.onClick.RemoveAllListeners();
			Panel.ZheZhaoBtn.gameObject.SetActive(value: true);
			Panel.RemoveSetted.onClick.RemoveAllListeners();
			Panel.RemoveSetted.onClick.AddListener(() =>
			{
				RemoveSetted(currentItem, dataSource, type, settingType, callBack);
			});
			AddButtonClickListener(Panel.XuanDingButton, () =>
			{
				if (callBack != null)
				{
					callBack();
				}
			});
		}

		private void RemoveSetted(RoleBattleSettingModel currentItem, List<RoleBattleSettingItemModel> dataSource, int type, SettingItemType settingType, Action callBack)
		{
			if (currentItem == null)
			{
				currentItem = new RoleBattleSettingModel();
			}
			switch (currentItem.b)
			{
			case "2":
				Panel.FaShu_1.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "3":
				Panel.FaShu_2.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "4":
				Panel.FaShu_3.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "5":
				Panel.FaShu_4.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "6":
				Panel.FuLu_1.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "7":
				Panel.FuLu_2.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "8":
				Panel.FuLu_3.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "9":
				Panel.FuLu_4.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "10":
				Panel.FaQi_1.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "11":
				Panel.FaQi_2.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "12":
				Panel.FaQi_3.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			case "13":
				Panel.FaQi_4.gameObject.GetComponent<DouMenSheZhiJiNengItem>()?.ClearValue();
				break;
			}
			if (!string.IsNullOrEmpty(currentItem.c))
			{
				RoleBattleSettingItemModel item = new RoleBattleSettingItemModel
				{
					id = currentItem.c,
					fiveElements = currentItem.e,
					icon = currentItem.f,
					level = currentItem.h,
					name = currentItem.a,
					rank = currentItem.i,
					usedPerception = currentItem.j
				};
				if (dataSource == null)
				{
					dataSource = new List<RoleBattleSettingItemModel>();
				}
				dataSource.Add(item);
			}
			ReflashXuanZeData(null, dataSource, type, settingType, callBack);
			Panel.DangQianXuanZeJiNengBg.gameObject.SetActive(value: false);
			ReFlashData();
		}

		private void SetDangQianXuanZe(DouMenXuanZeJiNengItem DouMenXuanZeJiNengItem)
		{
		}

		private void ChooseItemCompelete(ref DouMenSheZhiJiNengItem item, SettingItemType itemType)
		{
			int num = -1;
			int type = -1;
			switch (itemType)
			{
			case SettingItemType.Escape:
				num = 0;
				type = 0;
				break;
			case SettingItemType.FaShu1:
			case SettingItemType.FaShu2:
			case SettingItemType.FaShu3:
			case SettingItemType.FaShu4:
				num = 1;
				type = 1;
				break;
			case SettingItemType.FuLu1:
			case SettingItemType.FuLu2:
			case SettingItemType.FuLu3:
			case SettingItemType.FuLu4:
				num = 2;
				type = 3;
				break;
			case SettingItemType.FaQi1:
			case SettingItemType.FaQi2:
			case SettingItemType.FaQi3:
			case SettingItemType.FaQi4:
				num = 3;
				type = 2;
				break;
			}
			if (m_CurrentOpenedTargetDataSource != null)
			{
				if (num == -1)
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("设置失败");
					return;
				}
				RoleBattleSettingModel obj = new RoleBattleSettingModel
				{
					g = num,
					a = m_CurrentOpenedTargetDataSource.a,
					e = m_CurrentOpenedTargetDataSource.e,
					c = m_CurrentOpenedTargetDataSource.c,
					f = m_CurrentOpenedTargetDataSource.f,
					d = true
				};
				int num2 = (int)itemType;
				obj.b = num2.ToString();
				obj.j = m_CurrentOpenedTargetDataSource.j;
				obj.i = m_CurrentOpenedTargetDataSource.i;
				obj.h = m_CurrentOpenedTargetDataSource.h;
				Singleton<FacadeManager>.Instance.SelectImgSprite(m_CurrentOpenedTargetDataSource.e, m_CurrentOpenedTargetDataSource.f, type);
			}
			Panel.XuanZeGameObject.gameObject.transform.DOLocalMoveY(-687f, 0.5f);
			Panel.ZheZhaoBtn.gameObject.SetActive(value: false);
		}

		private void ChooseCallBack(DouMenSheZhiJiNengItem item, string index, int type, SettingItemType settingType)
		{
		}

		private void FaShu_1_Click()
		{
		}

		private void FaShu_2_Click()
		{
		}

		private void FaShu_3_Click()
		{
		}

		private void FaShu_4_Click()
		{
		}

		private void FaQi_1_Click()
		{
		}

		private void FaQi_2_Click()
		{
		}

		private void FaQi_3_Click()
		{
		}

		private void FaQi_4_Click()
		{
		}

		private void FuLu_1_Click()
		{
		}

		private void FuLu_2_Click()
		{
		}

		private void FuLu_3_Click()
		{
		}

		private void FuLu_4_Click()
		{
		}
	}
}
