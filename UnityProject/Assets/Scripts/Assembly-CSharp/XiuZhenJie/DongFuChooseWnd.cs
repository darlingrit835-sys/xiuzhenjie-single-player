using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DongFuChooseWnd : Window
	{
		public DongFuChoosePanel _panel;

		public Dictionary<string, WorldHomeModel> MoBanData;

		private GameObject _objInstance;

		private int m_CurrentLevel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<DongFuChoosePanel>();
			AddButtonClickListener(_panel.BackBtn, () =>
			{
				Singleton<UIManager>.Instance.FindWndByName<DongFuWnd>("DongFuWnd.prefab")?.reflash();
				Singleton<UIManager>.Instance.CloseWnd("DongFuChooseWnd.prefab");
			});
			int.TryParse(paralist[0].ToString(), out m_CurrentLevel);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			MoBanData = Singleton<FacadeManager>.Instance.SelectDongFuLevelInfo();
			if (MoBanData != null && MoBanData.Count != 0)
			{
				_objInstance = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DongFuItem.prefab");
				ReflashData();
			}
		}

		private void ReflashData()
		{
			int index = int.Parse(MoBanData[m_CurrentLevel.ToString()].id);
			_panel.TitleText.text = MoBanData[m_CurrentLevel.ToString()].name;
			foreach (Transform item in _panel.DongFuChooseContent)
			{
				Object.Destroy(item.gameObject);
			}
			CreateObj(index);
		}

		private void CreateObj(int index)
		{
			for (int i = index; i < 4; i++)
			{
				switch (i)
				{
				case 0:
				{
					Sprite sprite4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/DongFu/HuangJieDongFu.png");
					instanceDongFuItem(sprite4, i);
					break;
				}
				case 1:
				{
					Sprite sprite3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/DongFu/XuanJieDongFu.png");
					instanceDongFuItem(sprite3, i);
					break;
				}
				case 2:
				{
					Sprite sprite2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/DongFu/DiJieDongFu.png");
					instanceDongFuItem(sprite2, i);
					break;
				}
				case 3:
				{
					Sprite sprite = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/DongFu/TianJieDongFu.png");
					instanceDongFuItem(sprite, i);
					break;
				}
				}
			}
			VerticalLayoutGroup component = _panel.DongFuChooseContent.gameObject.GetComponent<VerticalLayoutGroup>();
			float y = SetVerticalListContentHight(4 - index, 155f, component);
			_panel.DongFuChooseContent.sizeDelta = new Vector2(_panel.DongFuChooseContent.sizeDelta.x, y);
		}

		private void instanceDongFuItem(Sprite sprite, int index)
		{
			GameObject _item = Object.Instantiate(_objInstance);
			RectTransform component = _item.GetComponent<RectTransform>();
			component.SetParent(_panel.DongFuChooseContent);
			component.localScale = Vector3.one;
			component.eulerAngles = Vector3.zero;
			DongFuItem component2 = _item.GetComponent<DongFuItem>();
			WorldHomeModel _data = MoBanData[(index + 1).ToString()];
			component2.SetValue(sprite, _data.level.ToString(), _data.name, $"{(double)_data.conversionRate * 100.0}%", "无限期", (long)(double)_data.price, _data.describe, () =>
			{
				buyDongFu(_data.price, _data.id, _item.gameObject);
			});
		}

		private void buyDongFu(double price, string id, GameObject item)
		{
			string key = m_CurrentLevel.ToString();
			if (int.Parse(id) - int.Parse(MoBanData[key].id) != 1)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention(string.Format("请先升级低阶洞府!", MoBanData[id].name));
				return;
			}
			if (Singleton<RoleManager>.Instance.GetRoleStage() < int.Parse(id))
			{
				switch (int.Parse(id))
				{
				case 2:
					MonoSingleton<GameStart>.Instance.PopUpAttention("需要等阶:筑基！");
					break;
				case 3:
					MonoSingleton<GameStart>.Instance.PopUpAttention("需要等阶:结丹！");
					break;
				case 4:
					MonoSingleton<GameStart>.Instance.PopUpAttention("需要等阶:元婴！");
					break;
				}
				return;
			}
			if (Singleton<RoleManager>.Instance.Role_Entity.VipLevel >= 4)
			{
				price = (int)((float)price * 0.9f);
			}
			if (Singleton<RoleManager>.Instance.ShengYuLingShi((long)price) <= 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("您的灵石不足!");
				return;
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.UPGRADE_DONGFU, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					UpgradeHomeResp_1005002 upgradeHomeResp_ = new UpgradeHomeResp_1005002();
					upgradeHomeResp_.MergeFrom(byteString);
					if (upgradeHomeResp_.IsSuccess)
					{
						Object.Destroy(item.gameObject);
						_panel.TitleText.text = upgradeHomeResp_.Name;
						m_CurrentLevel = upgradeHomeResp_.Rank / 4;
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = upgradeHomeResp_.ShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						ReflashData();
						DongFuRoomWnd dongFuRoomWnd = Singleton<UIManager>.Instance.FindWndByName<DongFuRoomWnd>("DongFuRoomWnd.prefab");
						dongFuRoomWnd?.PanDuanLingTian(1);
						dongFuRoomWnd?.UpdateDongFuLevel();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(upgradeHomeResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
