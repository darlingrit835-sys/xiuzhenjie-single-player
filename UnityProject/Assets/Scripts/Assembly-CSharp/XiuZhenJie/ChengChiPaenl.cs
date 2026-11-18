using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChengChiPaenl : MonoBehaviour
	{
		public Image ChengChi1_1;

		public Image ToXinag1_1;

		public Image ToXinag1_2;

		public Image ChengChi2_1;

		public Image ChengChi1_2;

		public Image ToXinag2_1;

		public Image ToXinag2_2;

		public Image ChengChi2_2;

		public Image ChengChi1_3;

		public Image ToXinag3_1;

		public Image ToXinag3_2;

		public Image ChengChi2_3;

		public Image ChengChi1_4;

		public Image ToXinag4_1;

		public Image ToXinag4_2;

		public Image ChengChi2_4;

		public Image ChengChi1_5;

		public Image ToXinag5_1;

		public Image ToXinag5_2;

		public Image ChengChi2_5;

		public Image ZhanDou;

		public Image ZhanDou1_1;

		public Image ZhanDou2_1;

		public Image ZhanDou1_2;

		public MapEntity dataSource;

		public void ChengShiXingXi(MapEntity _dataSource, bool isHear)
		{
			Sprite currentRoleSprite = Singleton<RoleManager>.Instance.GetCurrentRoleSprite();
			ChengChi1_1.gameObject.SetActive(value: false);
			ChengChi1_2.gameObject.SetActive(value: false);
			ChengChi1_3.gameObject.SetActive(value: false);
			ChengChi1_4.gameObject.SetActive(value: false);
			ChengChi1_5.gameObject.SetActive(value: false);
			ChengChi2_1.gameObject.SetActive(value: false);
			ChengChi2_2.gameObject.SetActive(value: false);
			ChengChi2_3.gameObject.SetActive(value: false);
			ChengChi2_4.gameObject.SetActive(value: false);
			ChengChi2_5.gameObject.SetActive(value: false);
			ZhanDou.gameObject.SetActive(value: false);
			ZhanDou1_1.gameObject.SetActive(value: false);
			ZhanDou1_2.gameObject.SetActive(value: false);
			ToXinag1_1.gameObject.SetActive(value: false);
			ToXinag2_1.gameObject.SetActive(value: false);
			ToXinag3_1.gameObject.SetActive(value: false);
			ToXinag4_1.gameObject.SetActive(value: false);
			ToXinag5_1.gameObject.SetActive(value: false);
			dataSource = _dataSource;
			switch (dataSource.MapType)
			{
			case 4:
				ZhanDou.gameObject.SetActive(value: true);
				ZhanDou1_2.gameObject.SetActive(value: true);
				if (isHear)
				{
					ZhanDou1_1.gameObject.SetActive(value: true);
					if (currentRoleSprite != null)
					{
						ZhanDou2_1.gameObject.GetComponent<Image>().sprite = currentRoleSprite;
					}
					else
					{
						Debug.Log("头像获取失败");
					}
				}
				break;
			case 51:
				ChengChi1_1.gameObject.SetActive(value: true);
				ChengChi2_1.gameObject.SetActive(value: true);
				if (isHear)
				{
					ToXinag1_1.gameObject.SetActive(value: true);
					if (currentRoleSprite != null)
					{
						ToXinag1_2.gameObject.GetComponent<Image>().sprite = currentRoleSprite;
					}
					else
					{
						Debug.Log("头像获取失败");
					}
				}
				break;
			case 52:
				ChengChi1_2.gameObject.SetActive(value: true);
				ChengChi2_2.gameObject.SetActive(value: true);
				if (isHear)
				{
					ToXinag2_1.gameObject.SetActive(value: true);
					if (currentRoleSprite != null)
					{
						ToXinag2_2.gameObject.GetComponent<Image>().sprite = currentRoleSprite;
					}
					else
					{
						Debug.Log("头像获取失败");
					}
				}
				break;
			case 5:
				ChengChi1_3.gameObject.SetActive(value: true);
				ChengChi2_3.gameObject.SetActive(value: true);
				if (isHear)
				{
					ToXinag3_1.gameObject.SetActive(value: true);
					if (currentRoleSprite != null)
					{
						ToXinag3_2.gameObject.GetComponent<Image>().sprite = currentRoleSprite;
					}
					else
					{
						Debug.Log("头像获取失败");
					}
				}
				break;
			case 53:
				ChengChi1_4.gameObject.SetActive(value: true);
				ChengChi2_4.gameObject.SetActive(value: true);
				if (isHear)
				{
					ToXinag4_1.gameObject.SetActive(value: true);
					if (currentRoleSprite != null)
					{
						ToXinag4_2.gameObject.GetComponent<Image>().sprite = currentRoleSprite;
					}
					else
					{
						Debug.Log("头像获取失败");
					}
				}
				break;
			case 54:
				ChengChi1_5.gameObject.SetActive(value: true);
				ChengChi2_5.gameObject.SetActive(value: true);
				if (isHear)
				{
					ToXinag5_1.gameObject.SetActive(value: true);
					if (currentRoleSprite != null)
					{
						ToXinag5_2.gameObject.GetComponent<Image>().sprite = currentRoleSprite;
					}
					else
					{
						Debug.Log("头像获取失败");
					}
				}
				break;
			}
		}
	}
}
