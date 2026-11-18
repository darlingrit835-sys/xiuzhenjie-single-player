using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_ZhangLaoItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Text DaoYouName;

		public Text ZhanLiText;

		public Text ZhiWeiText;

		public Text RoleLevel;

		public Button RenMingButton;

		private string FriendRoleId;

		public void SetValues(string roleId, bool sex, string touXiangId, bool isOnline, string daoYouName, string lastBackTime, string zhiWei, int level, UnityAction<string, GameObject> renMingAction)
		{
			FriendRoleId = roleId;
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId, isOnline);
			TouXiangIcon.sprite = sprite2;
			DaoYouName.text = daoYouName;
			ZhanLiText.text = lastBackTime;
			ZhiWeiText.text = zhiWei;
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			if (RenMingButton != null)
			{
				RenMingButton.onClick.AddListener(() =>
				{
					renMingAction(FriendRoleId, base.gameObject);
				});
			}
		}

	}
}
