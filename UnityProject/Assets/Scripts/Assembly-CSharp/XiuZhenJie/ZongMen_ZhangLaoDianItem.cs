using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_ZhangLaoDianItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Text DaoYouName;

		public Text LastTimeBack;

		public Text RoleLevel;

		public Button ZhuChuZongMenBtn;

		public Button ZhuChuZongMensBtn;

		public Text FuZhiYi_Text;

		public Image FuZhiYi_Img;

		public Image FuZhiEr_Img;

		public Text FuZhiEr_Text;

		public bool IsOnLine;

		public string FriendRoleId;

		public int index;

		public void SetValue(int indexs, bool isZongZhu, string roleId, bool isOnline, bool sex, string touXiangId, string names, int level, long lastBackTime, int fuZhiOne, UnityAction<int, string, string, GameObject> zhuChuZongMenAction, UnityAction<ZongMenZhangLaoRoleEntity, string, string, GameObject> zhuChuZongMenActions, ZongMenZhangLaoRoleEntity item)
		{
			index = indexs;
			FriendRoleId = roleId;
			IsOnLine = isOnline;
			DaoYouName.text = names;
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			if (lastBackTime == 0L)
			{
				LastTimeBack.text = "无";
			}
			else
			{
				LastTimeBack.text = $"{lastBackTime}";
			}
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId, isOnline);
			TouXiangIcon.sprite = sprite2;
			ZhuChuZongMenBtn.onClick.RemoveAllListeners();
			if (isZongZhu)
			{
				ZhuChuZongMenBtn.interactable = true;
				if (zhuChuZongMenAction != null)
				{
					ZhuChuZongMenBtn.onClick.AddListener(() =>
					{
						zhuChuZongMenAction(index, roleId, names, base.gameObject);
					});
				}
				if (ZhuChuZongMensBtn != null)
				{
					ZhuChuZongMensBtn.onClick.AddListener(() =>
					{
						zhuChuZongMenActions(item, roleId, names, base.gameObject);
					});
				}
			}
			else
			{
				ZhuChuZongMenBtn.interactable = false;
			}
			FuZhiYi_Img.gameObject.SetActive(value: true);
			FuZhiYi_Text.text = GetFuZhiName(fuZhiOne) ?? "";
			float y = -index * 91 + 85;
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, y);
			component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
		}

		private string GetFuZhiName(int fuZhiType)
		{
			return (ZhangLaoRenMing)fuZhiType switch
			{
				ZhangLaoRenMing.执法长老 => "执法长老", 
				ZhangLaoRenMing.内务长老 => "内务长老", 
				ZhangLaoRenMing.外务长老 => "外务长老", 
				ZhangLaoRenMing.矿脉长老 => "矿脉长老", 
				ZhangLaoRenMing.护宗长老 => "护宗长老", 
				ZhangLaoRenMing.灵田长老 => "灵田长老", 
				ZhangLaoRenMing.内门长老 => "内门长老", 
				ZhangLaoRenMing.冶炼长老 => "冶炼长老", 
				ZhangLaoRenMing.炼丹长老 => "炼丹长老", 
				ZhangLaoRenMing.炼器长老 => "炼器长老", 
				ZhangLaoRenMing.御兽长老 => "御兽长老", 
				ZhangLaoRenMing.阵法长老 => "阵法长老", 
				ZhangLaoRenMing.制符长老 => "制符长老", 
				ZhangLaoRenMing.工坊长老 => "工坊长老", 
				ZhangLaoRenMing.宗主 => "宗主", 
				ZhangLaoRenMing.无 => "无", 
				_ => "", 
			};
		}

	}
}
