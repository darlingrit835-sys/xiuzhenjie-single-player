using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ChuangZhao_LianZhiDengJieWnd : Window
	{
		public ChuangZhao_LianZhiDengJiePanel Panel;

		private int m_CurrentChooseIndex;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ChuangZhao_LianZhiDengJiePanel>();
			AddButtonClickListener(Panel.LiKaiBtn, LiKaiBtnClick);
			AddButtonClickListener(Panel.XiaYiBuBtn, XiaYiBuBtnClick);
			AddToggleClickListener(Panel.YiJieTog, YiJieTog);
			AddToggleClickListener(Panel.ErJieTog, ErJieTog);
			AddToggleClickListener(Panel.SanJieTog, SanJieTog);
			AddToggleClickListener(Panel.SiJieTog, SiJieTog);
			AddToggleClickListener(Panel.WuJieTog, WuJieTog);
			AddToggleClickListener(Panel.LiuJieTog, LiuJieTog);
			AddToggleClickListener(Panel.QiJieTog, QiJieTog);
			AddToggleClickListener(Panel.BaJieTog, BaJieTog);
			AddToggleClickListener(Panel.JiuJieTog, JiuJieTog);
			AddButtonClickListener(Panel.WeiHao, WenHaoTogClick);
			YiJieTog(isOn: true);
		}

		private void LiKaiBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
		}

		private void XiaYiBuBtnClick()
		{
			int currentDanShiLevel = Singleton<RoleManager>.Instance.GetCurrentDanShiLevel();
			if (m_CurrentChooseIndex > currentDanShiLevel)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("道友,您的炼丹水平不足,无法创造该等阶丹方！");
				return;
			}
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab", true, m_CurrentChooseIndex);
		}

		private void YiJieTog(bool isOn)
		{
			Text component = Panel.YiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 1;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void ErJieTog(bool isOn)
		{
			Text component = Panel.ErJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 2;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void SanJieTog(bool isOn)
		{
			Text component = Panel.SanJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 3;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void SiJieTog(bool isOn)
		{
			Text component = Panel.SiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 4;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void WuJieTog(bool isOn)
		{
			Text component = Panel.WuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 5;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void LiuJieTog(bool isOn)
		{
			Text component = Panel.LiuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 6;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void QiJieTog(bool isOn)
		{
			Text component = Panel.QiJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 7;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void BaJieTog(bool isOn)
		{
			Text component = Panel.BaJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 8;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void JiuJieTog(bool isOn)
		{
			Text component = Panel.JiuJieTog.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
			if (isOn)
			{
				m_CurrentChooseIndex = 9;
				component.color = Color.white;
			}
			else
			{
				component.color = Color.black;
			}
		}

		private void WenHaoTogClick()
		{
			string text = $"选择合适的药材，进行配比后可以尝试创造丹方。若此丹方已有他人注册，则无法创造成功。丹方内蕴天地法则，通过材料的四气、五味、补泻、归经等属性搭配，中和药效而成丹。一阶丹方仅需5种材料即可，每高一阶的丹方，都需要增加一味材料，越高级的材料能产生的药效越强，当然如果配伍不当，也可能更弱。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "创造丹方");
		}
	}
}
