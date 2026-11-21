using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DongFuYongTuChooseWnd : Window
	{
		public DongFuYongTuChoosePanel Panel;

		private RoleFuZhiType m_CurrentChooseType;

		private UnityAction<RoleFuZhiType> m_SureAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DongFuYongTuChoosePanel>();
			InkCultivationPages.PaperDialog(Panel.transform, "TiShiBg", Panel.QueRenBtn, null);
			if (paralist.Length > 1)
			{
				List<RoleFuZhiType> type = paralist[0] as List<RoleFuZhiType>;
				m_SureAction = paralist[1] as UnityAction<RoleFuZhiType>;
				Init(type);
				var options = new[] { Panel.LianDanTog, Panel.HuaFuTog, Panel.ZhenFaTog, Panel.XiuFaTog, Panel.WuDaoTog, Panel.LianQiTog };
				int available = options.Count(item => item.isActiveAndEnabled && item.IsCanClick && item.GetComponent<Toggle>().IsInteractable());
				var caption = Panel.transform.Find("TiShiBg/ContentBg/TitleImg");
				InkInterfaceArt.Set(caption.GetComponent<Image>(), "ParchmentHeading", false);
				var text = caption.GetComponentInChildren<Text>();
				text.text = $"可选洞府用途　{available} 项";
				text.color = InkInterfaceArt.Ink;
			}
			AddButtonClickListener(Panel.CloseBtn, CloseWnd);
			AddButtonClickListener(Panel.QueRenBtn, QueRenWnd);
		}

		private void Init(List<RoleFuZhiType> type)
		{
			m_CurrentChooseType = RoleFuZhiType.None;
			Panel.LianDanTog.SetItem(LianDan, isEnable: true);
			Panel.HuaFuTog.SetItem(HuaFu, isEnable: true);
			Panel.ZhenFaTog.SetItem(BuZhen, isEnable: true);
			Panel.XiuFaTog.SetItem(XiuFa, isEnable: true);
			Panel.WuDaoTog.SetItem(WuDao, isEnable: true);
			Panel.LianQiTog.SetItem(LianQi, isEnable: true);
			if (type == null || type.Count <= 0)
			{
				return;
			}
			foreach (RoleFuZhiType item in type)
			{
				switch (item)
				{
				case RoleFuZhiType.LianDan:
					Panel.LianDanTog.SetItem(LianDan, isEnable: false);
					break;
				case RoleFuZhiType.ZhiFu:
					Panel.HuaFuTog.SetItem(HuaFu, isEnable: false);
					break;
				case RoleFuZhiType.FaShu:
					Panel.XiuFaTog.SetItem(XiuFa, isEnable: false);
					break;
				case RoleFuZhiType.BuDao:
					Panel.WuDaoTog.SetItem(WuDao, isEnable: false);
					break;
				case RoleFuZhiType.ZhenTu:
					Panel.ZhenFaTog.SetItem(BuZhen, isEnable: false);
					break;
				case RoleFuZhiType.LianQi:
					Panel.LianQiTog.SetItem(LianQi, isEnable: false);
					break;
				}
			}
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenWnd()
		{
			if (m_CurrentChooseType != RoleFuZhiType.None)
			{
				if (m_SureAction != null)
				{
					m_SureAction(m_CurrentChooseType);
				}
				Singleton<UIManager>.Instance.CloseWnd(this);
			}
		}

		private void LianDan(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.LianDan;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void LianQi(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.LianQi;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void HuaFu(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.ZhiFu;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void BuZhen(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.ZhenTu;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void ZhongZhi(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.LingZhi;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void QiQiao(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.QiaoJiang;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void YuShou(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.YuShou;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void XiuFa(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.FaShu;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}

		private void WuDao(bool isOn)
		{
			if (isOn)
			{
				m_CurrentChooseType = RoleFuZhiType.BuDao;
			}
			else
			{
				m_CurrentChooseType = RoleFuZhiType.None;
			}
		}
	}
}
