using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DaoYouItem : MonoBehaviour
	{
		public Image TouXiangIcon;

		public Image QinMiDuIcon;

		public Image ReceiveMsgNumIcon;

		public Text DaoYouName;

		public Text QinMiDuText;

		public Text RoleLevel;

		public Text NoReadMsgNum;

		public Button ChuanYinBtn;

		public Button ChuanWuBtn;

		public Text FuZhiYi_Text;

		public Image FuZhiYi_Img;

		public Image FuZhiEr_Img;

		public Text FuZhiEr_Text;

		public bool IsOnLine;

		public string FriendRoleId;

		public int NoReadCount;

		public int index;

		public void SetValue(int listIndex, string roleId, bool isOnline, int unReadMsgNum, bool sex, string touXiangId, string name, int level, int Intimacy, UnityAction<DaoYouFirendEntity> chuanYinAction, UnityAction<DaoYouFirendEntity> chuanWuAction, RoleFuZhiType fuZhi_One, RoleFuZhiType fuzhi_Two)
		{
			index = listIndex;
			FriendRoleId = roleId;
			NoReadCount = unReadMsgNum;
			IsOnLine = isOnline;
			DaoYouName.text = name;
			NoReadMsgNum.text = unReadMsgNum.ToString();
			if (unReadMsgNum > 0)
			{
				ReceiveMsgNumIcon.gameObject.SetActive(value: true);
			}
			RoleLevel.text = "境界:" + WorldSetting.GetRoleLevelNameByRoleLevel(level);
			Sprite sprite2 = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, touXiangId, isOnline);
			TouXiangIcon.sprite = sprite2;
			DaoYouFirendEntity daoYouFirendEntity = new DaoYouFirendEntity
			{
				DaoYouItemObj = base.gameObject,
				RoleHeadId = int.Parse(touXiangId),
				RoleId = roleId,
				RoleName = name,
				RoleSex = (sex ? 1 : 0)
			};
			ChuanYinBtn.onClick.RemoveAllListeners();
			ChuanWuBtn.onClick.RemoveAllListeners();
			if (chuanYinAction != null)
			{
				ChuanYinBtn.onClick.AddListener(() =>
				{
					EnterChatRoom();
					chuanYinAction(daoYouFirendEntity);
				});
			}
			if (chuanWuAction != null)
			{
				ChuanWuBtn.onClick.AddListener(() =>
				{
					chuanWuAction(daoYouFirendEntity);
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
				RoleFuZhiType.LianQi => "炼器", 
				RoleFuZhiType.ZhenTu => "阵法", 
				_ => "", 
			};
		}


		public void EnterChatRoom()
		{
			NoReadCount = 0;
			NoReadMsgNum.text = NoReadCount.ToString();
			ReceiveMsgNumIcon.gameObject.SetActive(value: false);
		}

		public void AddOneNoReadMsg()
		{
			NoReadCount++;
			NoReadMsgNum.text = NoReadCount.ToString();
			ReceiveMsgNumIcon.gameObject.SetActive(value: true);
		}
	}
}
