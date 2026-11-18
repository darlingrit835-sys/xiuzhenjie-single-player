using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class WorldCaiLiaoWnd : Window
	{
		public WorldCaiLiaoPanel Panel;

		private List<WorldCaiLiaoModel> m_DataSource;

		private WorldCaiLiaoModel m_CurrentCaiLiao;

		private CaiLiaoShaiXuanSourceType m_CaiLiaoShaiXuanSourceType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<WorldCaiLiaoPanel>();
			if (paralist.Length != 0)
			{
				m_CaiLiaoShaiXuanSourceType = (CaiLiaoShaiXuanSourceType)int.Parse(paralist[0].ToString());
				m_DataSource = paralist[1] as List<WorldCaiLiaoModel>;
			}
			AddButtonClickListener(Panel.ChooseBtn, ChooseBtnClick);
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			InitData();
		}

		private void InitData()
		{
			GameObject insObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/WorldCaiLiaoItem.prefab");
			GridLayoutGroup component = Panel.ScrollContent.gameObject.GetComponent<GridLayoutGroup>();
			if (m_DataSource != null && m_DataSource.Count != 0)
			{
				ToggleGroup component2 = Panel.ScrollContent.gameObject.GetComponent<ToggleGroup>();
				int num = 0;
				foreach (WorldCaiLiaoModel item in m_DataSource)
				{
					WorldCaiLiaoItem component3 = SetListContentParent(Panel.ScrollContent, insObj).gameObject.GetComponent<WorldCaiLiaoItem>();
					if (num == 0)
					{
						component3?.SetValue(item, component2, CaiLiaoItemClick, isChoose: true);
						num++;
					}
					else
					{
						component3?.SetValue(item, component2, CaiLiaoItemClick);
					}
				}
			}
			SetGrticalListContentHight(m_DataSource.Count, Panel.ScrollContent, component);
		}

		private void CaiLiaoItemClick(WorldCaiLiaoModel model, bool isOn)
		{
			if (isOn)
			{
				m_CurrentCaiLiao = model;
				if (m_CaiLiaoShaiXuanSourceType == CaiLiaoShaiXuanSourceType.LianDan)
				{
					if (!Panel.Content.gameObject.activeSelf)
					{
						Panel.Content.gameObject.SetActive(value: true);
					}
					Panel.TitleName.text = model.c;
					Panel.JieShao.text = "";
				}
				else
				{
					if (!Panel.LianQiContent.gameObject.activeSelf)
					{
						Panel.LianQiContent.gameObject.SetActive(value: true);
					}
					Panel.LQ_TitleName.text = model.c;
				}
				if (model.b == 12 || model.b == 3)
				{
					if (m_CaiLiaoShaiXuanSourceType == CaiLiaoShaiXuanSourceType.LianDan)
					{
						SelectPlantTemp(model);
					}
					else
					{
						SelectPlantTempPartLianQi(model);
					}
				}
				else if (model.b == 7)
				{
					if (m_CaiLiaoShaiXuanSourceType == CaiLiaoShaiXuanSourceType.LianDan)
					{
						SelectAnimalTemp(model);
					}
					else
					{
						SelectAnimalTempPartLianQi(model);
					}
				}
				else if (model.b == 13)
				{
					SelectKuangWuTemp(model);
				}
			}
			else if (model == m_CurrentCaiLiao)
			{
				Panel.Content.gameObject.SetActive(value: false);
			}
		}

		private void SelectKuangWuTemp(WorldCaiLiaoModel model)
		{
			Singleton<RoleManager>.Instance.SelectTemplateMaterialMineralDetail(model.a, (List<string> _list) =>
			{
				Panel.LQ_PinJiText.text = _list[0].Split(',')[1].ToString();
				Panel.LQ_WuXingText.text = _list[1].Split(',')[1].ToString();
				Panel.LQ_ChanDiText.text = _list[2].Split(',')[1].ToString();
				Panel.LQ_JianDuText.text = _list[3].Split(',')[1].ToString();
				Panel.LQ_RenDuText.text = _list[4].Split(',')[1].ToString();
			});
		}

		private void SelectAnimalTemp(WorldCaiLiaoModel model)
		{
			Singleton<RoleManager>.Instance.SelectTemplateMaterialAnimalDetail(model.a, (List<string> _list) =>
			{
				Panel.PinJiText.text = _list[0].Split(',')[1].ToString();
				Panel.WuXingText.text = _list[1].Split(',')[1].ToString();
				Panel.ChanDiText.text = _list[2].Split(',')[1].ToString();
				Panel.SiQiText.text = _list[3].Split(',')[1].ToString();
				Panel.WuWeiText.text = _list[4].Split(',')[1].ToString();
				Panel.BuXieText.text = _list[5].Split(',')[1].ToString();
				Panel.GuiJingText.text = _list[6].Split(',')[1].ToString();
			});
		}

		private void SelectAnimalTempPartLianQi(WorldCaiLiaoModel model)
		{
			Singleton<RoleManager>.Instance.SelectTemplateMaterialAnimalDetail(model.a, (List<string> _list) =>
			{
				Panel.LQ_PinJiText.text = _list[0].Split(',')[1].ToString();
				Panel.LQ_WuXingText.text = _list[1].Split(',')[1].ToString();
				Panel.LQ_ChanDiText.text = _list[2].Split(',')[1].ToString();
				Panel.LQ_JianDuText.text = _list[7].Split(',')[1].ToString();
				Panel.LQ_RenDuText.text = _list[8].Split(',')[1].ToString();
			});
		}

		private void SelectPlantTemp(WorldCaiLiaoModel model)
		{
			Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(model.a, (List<string> _list) =>
			{
				Panel.PinJiText.text = _list[0].Split(',')[1].ToString();
				Panel.WuXingText.text = _list[1].Split(',')[1].ToString();
				if (model.b == 3)
				{
					Panel.ChanDiText.text = "虚神界";
				}
				else
				{
					Panel.ChanDiText.text = _list[2].Split(',')[1].ToString();
				}
				Panel.SiQiText.text = _list[3].Split(',')[1].ToString();
				Panel.WuWeiText.text = _list[4].Split(',')[1].ToString();
				Panel.BuXieText.text = _list[5].Split(',')[1].ToString();
				Panel.GuiJingText.text = _list[6].Split(',')[1].ToString();
			});
		}

		private void SelectPlantTempPartLianQi(WorldCaiLiaoModel model)
		{
			Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(model.a, (List<string> _list) =>
			{
				Panel.LQ_PinJiText.text = _list[0].Split(',')[1].ToString();
				Panel.LQ_WuXingText.text = _list[1].Split(',')[1].ToString();
				if (model.b == 3)
				{
					Panel.LQ_ChanDiText.text = "虚神界";
				}
				else
				{
					Panel.LQ_ChanDiText.text = _list[2].Split(',')[1].ToString();
				}
				Panel.LQ_JianDuText.text = _list[7].Split(',')[1].ToString();
				Panel.LQ_RenDuText.text = _list[8].Split(',')[1].ToString();
			});
		}

		private void SetDengJieText()
		{
			string text = "";
			switch (m_CurrentCaiLiao.e)
			{
			case 1:
				text = "一阶";
				break;
			case 2:
				text = "二阶";
				break;
			case 3:
				text = "三阶";
				break;
			case 4:
				text = "四阶";
				break;
			case 5:
				text = "五阶";
				break;
			case 6:
				text = "六阶";
				break;
			case 7:
				text = "七阶";
				break;
			case 8:
				text = "八阶";
				break;
			case 9:
				text = "九阶";
				break;
			}
			Panel.PinJiText.text = text;
		}

		private void SetWuXingText()
		{
			string text = "";
			switch (m_CurrentCaiLiao.f)
			{
			case 1:
				text = "金";
				break;
			case 2:
				text = "木";
				break;
			case 3:
				text = "水";
				break;
			case 4:
				text = "火";
				break;
			case 5:
				text = "土";
				break;
			}
			Panel.WuXingText.text = text;
		}

		private void SetSiQiText()
		{
			string text = "";
			switch (m_CurrentCaiLiao.g)
			{
			case 1:
				text = "寒";
				break;
			case 2:
				text = "凉";
				break;
			case 3:
				text = "温";
				break;
			case 4:
				text = "热";
				break;
			}
			Panel.SiQiText.text = text;
		}

		private void SetWuWei()
		{
			string text = "";
			switch (m_CurrentCaiLiao.h)
			{
			case 1:
				text = "辛";
				break;
			case 2:
				text = "苦";
				break;
			case 3:
				text = "酸";
				break;
			case 4:
				text = "甘";
				break;
			case 5:
				text = "咸";
				break;
			}
			Panel.WuWeiText.text = text;
		}

		private void SetBuXie()
		{
			string text = "";
			switch (m_CurrentCaiLiao.i)
			{
			case 1:
				text = "补";
				break;
			case 2:
				text = "泻";
				break;
			}
			Panel.BuXieText.text = text;
		}

		private void SetGuiJingText()
		{
			string text = "";
			switch (m_CurrentCaiLiao.j)
			{
			case 1:
				text = "手厥阴";
				break;
			case 2:
				text = "手少阴";
				break;
			case 3:
				text = "足少阳";
				break;
			case 4:
				text = "手少阳";
				break;
			case 5:
				text = "足太阳";
				break;
			case 6:
				text = "足太阴";
				break;
			case 7:
				text = "手太阳";
				break;
			case 8:
				text = "手阳明";
				break;
			case 9:
				text = "手太阴";
				break;
			case 10:
				text = "足阳明";
				break;
			case 11:
				text = "足厥阴";
				break;
			case 12:
				text = "足少阴";
				break;
			}
			Panel.GuiJingText.text = text;
		}

		private void ChooseBtnClick()
		{
			if (m_CurrentCaiLiao == null)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请选择材料！");
			}
			else if (m_CaiLiaoShaiXuanSourceType == CaiLiaoShaiXuanSourceType.LianDan)
			{
				Singleton<UIManager>.Instance.FindWndByName<DiHuoGeLianZhiBuZhouSheZhiWnd>("DiHuoGeLianZhiBuZhouSheZhiWnd.prefab")?.ChoosedCaoYao(m_CurrentCaiLiao);
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("CaiLiaoXuanZeShaiXuanWnd.prefab");
			}
			else
			{
				Singleton<UIManager>.Instance.FindWndByName<LianQiBuZhouSheZhiWnd>("LianQiBuZhouSheZhiWnd.prefab")?.ChoosedCaiLiao(m_CurrentCaiLiao);
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("LianQiCaiLiaoXuanZeShaiXuanWnd.prefab");
			}
		}
	}
}
