using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMenChengYuanItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Text DaoYouName;

		public Text LastTimeBack;

		public Text RoleLevel;

		public Button ZhuChuZongMenBtn;

		public Text FuZhiYi_Text;

		public Image FuZhiYi_Img;

		public Image FuZhiEr_Img;

		public Text FuZhiEr_Text;

		public bool IsOnLine;

		public string FriendRoleId;

		public int index;

		public void SetValue(int index, bool isZongZhu, string roleId, bool isOnline, bool sex, string touXiangId, string name, int level, long lastBackTime, RoleFuZhiType fuZhi_One, RoleFuZhiType fuzhi_Two, UnityAction<int, string, string, GameObject> ZhuChuZongMenAction)
		{
			this.index = index;
			FriendRoleId = roleId;
			IsOnLine = isOnline;
			DaoYouName.text = name;
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			if (lastBackTime == 0L)
			{
				LastTimeBack.text = "无";
			}
			else
			{
				string text = CommonMethod.GetTimeStampTime((lastBackTime / 1000).ToString()).ToString("yyyy/MM/dd  HH:mm");
				LastTimeBack.text = text;
			}
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId, isOnline);
			TouXiangIcon.sprite = sprite2;
			ZhuChuZongMenBtn.onClick.RemoveAllListeners();
			if (isZongZhu)
			{
				ZhuChuZongMenBtn.interactable = true;
				if (ZhuChuZongMenAction != null)
				{
					ZhuChuZongMenBtn.onClick.AddListener(() =>
					{
						ZhuChuZongMenAction(index, roleId, name, base.gameObject);
					});
				}
			}
			else
			{
				ZhuChuZongMenBtn.interactable = false;
			}
			if (fuZhi_One != RoleFuZhiType.None)
			{
				FuZhiYi_Img.gameObject.SetActive(value: true);
				FuZhiYi_Text.text = GetFuZhiName(fuZhi_One);
			}
			if (fuzhi_Two != RoleFuZhiType.None)
			{
				FuZhiEr_Img.gameObject.SetActive(value: true);
				FuZhiEr_Text.text = GetFuZhiName(fuzhi_Two);
			}
			float y = -index * 81;
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, y);
			component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
		}

		private string GetFuZhiName(RoleFuZhiType fuZhiType)
		{
			return fuZhiType switch
			{
				RoleFuZhiType.LianDan => "炼丹", 
				RoleFuZhiType.ZhiFu => "画符", 
				RoleFuZhiType.FaShu => "修法", 
				RoleFuZhiType.BuDao => "布道", 
				RoleFuZhiType.LingZhi => "种植", 
				_ => "", 
			};
		}

	}
}
