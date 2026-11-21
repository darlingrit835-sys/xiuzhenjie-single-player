using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class MingCiItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Image QinMiDuIcon;

		public Text DaoYouName;

		public Text QinMiDuText;

		public Text RoleLevel;

		public Button JieShiBtn;

		public Button JuJueBtn;

		public Text FuZhiYi_Text;

		public Text FuZhiEr_Text;

		public Image FuZhiYi_Img;

		public Image FuZhiEr_Img;

		public bool IsOnLine;

		public string FriendRoleId;

		public int index;

		public void SetValue(int listIndex, string roleId, bool isOnline, bool sex, string touXiangId, string name, int level, UnityAction<int, GameObject, string, string> JieShiAction, UnityAction<int, GameObject, string, string> JuJueAction, RoleFuZhiType fuZhi_One, RoleFuZhiType fuzhi_Two)
		{
			index = listIndex;
			FriendRoleId = roleId;
			IsOnLine = isOnline;
			DaoYouName.text = name;
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId.ToString(), isOnline);
			TouXiangIcon.sprite = sprite2;
			JieShiBtn.onClick.RemoveAllListeners();
			JuJueBtn.onClick.RemoveAllListeners();
			if (JieShiAction != null)
			{
				JieShiBtn.onClick.AddListener(() =>
				{
					JieShiAction(index, base.gameObject, FriendRoleId, name);
				});
			}
			if (JuJueAction != null)
			{
				JuJueBtn.onClick.AddListener(() =>
				{
					JuJueAction(index, base.gameObject, FriendRoleId, name);
				});
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
			float y = -listIndex * 81;
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
