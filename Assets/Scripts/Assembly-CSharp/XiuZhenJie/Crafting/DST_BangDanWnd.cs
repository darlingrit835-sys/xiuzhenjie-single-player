using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_BangDanWnd : Window
	{
		public DST_BangDanPanel _panel;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			_panel = base.GameObject.GetComponent<DST_BangDanPanel>();
			if ((int)paralist[0] == 1)
			{
				_panel.BangDan.text = "道境榜";
			}
			else
			{
				_panel.BangDan.text = "岁月榜";
			}
			AddButtonClickListener(_panel.Close_Btn, Close_BtnClick);
			AddToggleClickListener(_panel.LianQI_btn, (bool isOn) =>
			{
				ClearData();
				CreateData(1);
			});
			AddToggleClickListener(_panel.ZhuJi_btn, ZhuJi_btnClick);
			AddToggleClickListener(_panel.JieDan_btn, JieDan_btnClick);
			AddToggleClickListener(_panel.YuanYin_btn, YuanYin_btnClick);
			AddToggleClickListener(_panel.HuaShen_btn, HuaShen_btnClick);
			CreateData(1);
			_panel.MyRank.text = string.Format(_panel.MyRank.text, 40);
		}

		private void ClearData()
		{
			foreach (RectTransform item in _panel.BangDanScrol.content.gameObject.GetComponent<RectTransform>())
			{
				Object.Destroy(item.gameObject);
			}
		}

		private void CreateData(int type)
		{
			string level = type switch
			{
				1 => "炼器", 
				2 => "筑基", 
				3 => "结丹", 
				4 => "元婴", 
				5 => "化神", 
				_ => "信息错误", 
			};
			GameObject gameObject = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/PaiHangBangItemPanel.prefab");
			float y = gameObject.GetComponent<RectTransform>().sizeDelta.y;
			VerticalLayoutGroup component = _panel.ItemListContent.GetComponent<VerticalLayoutGroup>();
			for (int i = 0; i < 15; i++)
			{
				SetListContentParent(_panel.ItemListContent, gameObject).GetComponent<PaiHangBangItemPanel>().SetValue(i + 1, $"玩家_{i}", level, "无门派", "第九层");
			}
			_panel.ItemListContent.sizeDelta = new Vector2(_panel.ItemListContent.sizeDelta.x, SetVerticalListContentHight(15, y, component));
		}

		private void ZhuJi_btnClick(bool arg0)
		{
			ClearData();
			CreateData(2);
		}

		private void JieDan_btnClick(bool arg0)
		{
			ClearData();
			CreateData(3);
		}

		private void YuanYin_btnClick(bool arg0)
		{
			ClearData();
			CreateData(4);
		}

		private void HuaShen_btnClick(bool arg0)
		{
			ClearData();
			CreateData(5);
		}

		private void Close_BtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd("DST_BangDanWnd.prefab");
		}
	}
}
