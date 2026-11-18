using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ShoTuItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Text DaoYouName;

		public Text RoleLevel;

		public Button HuLueBtn;

		public Button JieShoBtn;

		public bool IsOnLine;

		public string FriendRoleId;

		public int index;

		public void Pos(int listIndex)
		{
			float y = -listIndex * 81;
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, y);
			component.sizeDelta = new Vector2(0f, component.sizeDelta.y);
		}

		public void SetValue(int listIndex, string roleId, bool isOnline, bool sex, string touXiangId, string tuDiname, int level, UnityAction<int, GameObject, string, string> jieShoAction, UnityAction<int, GameObject, string, string> huLueAction)
		{
			index = listIndex;
			FriendRoleId = roleId;
			IsOnLine = isOnline;
			DaoYouName.text = tuDiname;
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId, isOnline);
			TouXiangIcon.sprite = sprite2;
			Pos(listIndex);
			if (huLueAction != null)
			{
				HuLueBtn.onClick.AddListener(() =>
				{
					huLueAction(index, base.gameObject, FriendRoleId, base.name);
				});
			}
			if (jieShoAction != null)
			{
				JieShoBtn.onClick.AddListener(() =>
				{
					jieShoAction(index, base.gameObject, FriendRoleId, base.name);
				});
			}
		}

	}
}
