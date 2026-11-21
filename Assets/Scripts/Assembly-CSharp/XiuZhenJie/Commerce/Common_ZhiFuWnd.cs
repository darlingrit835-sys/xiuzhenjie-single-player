using UnityEngine.Events;

namespace XiuZhenJie
{
	public class Common_ZhiFuWnd : Window
	{
		public Common_ZhiFuPanel Panel;

		private UnityAction<int, int> QueDingPayActon;

		private long JiaGeValue;

		private ZhiFuType m_ZhiFuType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<Common_ZhiFuPanel>();
			AddButtonClickListener(Panel.QueRenTouRuBtn, QueRenZhiFu);
			AddButtonClickListener(Panel.QuXiaoBtn, QuXiaoBtn);
			if (paralist.Length > 2)
			{
				JiaGeValue = long.Parse(paralist[0].ToString());
				QueDingPayActon = paralist[1] as UnityAction<int, int>;
				m_ZhiFuType = (ZhiFuType)int.Parse(paralist[2].ToString());
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Init();
		}

		private void Init()
		{
			if (m_ZhiFuType == ZhiFuType.LingShi)
			{
				Panel.DanWeiText.text = "灵石";
				Panel.NumberValue.text = JiaGeValue.ToString("0");
				double num = Singleton<RoleManager>.Instance.Role_Entity.ShinyStone - JiaGeValue;
				Panel.ShengYuLingShiText.text = string.Format("支付后还剩余 {0} 灵石", "<size=14>" + num.ToString("0") + "</size>");
			}
			else if (m_ZhiFuType == ZhiFuType.LingYu)
			{
				Panel.DanWeiText.text = "灵玉";
				Panel.NumberValue.text = JiaGeValue.ToString("0");
				double num2 = Singleton<RoleManager>.Instance.Role_Entity.ShinyJade - JiaGeValue;
				Panel.ShengYuLingShiText.text = string.Format("支付后还剩余 {0} 灵玉", "<size=14>" + num2.ToString("0") + "</size>");
			}
			else if (m_ZhiFuType == ZhiFuType.GongXianDian)
			{
				Panel.DanWeiText.text = "贡献";
				Panel.NumberValue.text = JiaGeValue.ToString("0");
				Panel.ShengYuLingShiText.text = "";
			}
		}

		private void QuXiaoBtn()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenZhiFu()
		{
			if (QueDingPayActon != null)
			{
				QueDingPayActon((int)JiaGeValue, (int)m_ZhiFuType);
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
		}
	}
}
