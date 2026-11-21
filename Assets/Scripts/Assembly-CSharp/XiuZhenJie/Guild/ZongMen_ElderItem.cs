using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_ElderItem : MonoBehaviour
	{
		public string ZhangLaoName;

		public int m_Type;

		public Text ZhangLaoNameText;

		public Text DqZhangLaoMingChenText;

		public Image XuanZhoImage;

		public Button DianJiButton;

		public void ZhangLaoMingCheng(string zhangLaoName)
		{
			ZhangLaoName = zhangLaoName;
			ZhangLaoNameText.text = zhangLaoName;
		}

		public void XuanZho(bool zhangLao)
		{
			XuanZhoImage.gameObject.SetActive(zhangLao);
		}

		public void XianShi(string zhangLaoMingChen, int type)
		{
			if (Singleton<RoleManager>.Instance.Role_Entity.RoleName == zhangLaoMingChen)
			{
				XuanZhoImage.gameObject.SetActive(value: true);
			}
			else
			{
				XuanZhoImage.gameObject.SetActive(value: false);
			}
			m_Type = type;
			DqZhangLaoMingChenText.text = zhangLaoMingChen;
		}
	}
}
