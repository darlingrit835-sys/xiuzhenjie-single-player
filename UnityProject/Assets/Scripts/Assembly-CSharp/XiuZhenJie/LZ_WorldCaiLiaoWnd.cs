using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_WorldCaiLiaoWnd : Window
	{
		public LZ_WorldCaiLiaoPanel Panel;

		private List<LZ_WorldCaiLiaoModel> m_DataSource;

		private LZ_WorldCaiLiaoModel m_CurrentCaiLiao;

		private int m_CurrentShu;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<LZ_WorldCaiLiaoPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as List<LZ_WorldCaiLiaoModel>;
				int.TryParse(paralist[1].ToString(), out m_CurrentShu);
			}
			AddButtonClickListener(Panel.ChooseBtn, ChooseBtnClick);
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
				foreach (LZ_WorldCaiLiaoModel item in m_DataSource)
				{
					WorldCaiLiaoItem component3 = SetListContentParent(Panel.ScrollContent, insObj).gameObject.GetComponent<WorldCaiLiaoItem>();
					if (num == 0)
					{
						component3?.SetValues(item, component2, CaiLiaoItemClick, isChoose: true);
						num++;
					}
					else
					{
						component3?.SetValues(item, component2, CaiLiaoItemClick);
					}
				}
			}
			SetGrticalListContentHight(m_DataSource.Count, Panel.ScrollContent, component);
		}

		private void CaiLiaoItemClick(LZ_WorldCaiLiaoModel model, bool isOn)
		{
			if (isOn)
			{
				m_CurrentCaiLiao = model;
				if (!Panel.Content.gameObject.activeSelf)
				{
					Panel.Content.gameObject.SetActive(value: true);
				}
				Panel.TitleName.text = model.c;
				Panel.JieShao.text = "";
				TemplatePlantDetail(model.a);
				SelectPlantTemp(model);
			}
			else if (model == m_CurrentCaiLiao)
			{
				Panel.Content.gameObject.SetActive(value: false);
			}
		}

		public void TemplatePlantDetail(string templateId)
		{
			TemplateMaterialPlantDetailReq_1011003 data = new TemplateMaterialPlantDetailReq_1011003
			{
				Id = templateId
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.TEMPLATE_MATERIAL_PLANT_DETAIL, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					TemplateMaterialPlantDetailResp_1011003 templateMaterialPlantDetailResp_ = new TemplateMaterialPlantDetailResp_1011003();
					templateMaterialPlantDetailResp_.MergeFrom(byteString);
					if (templateMaterialPlantDetailResp_.IsSuccess)
					{
						m_CurrentCaiLiao.e = templateMaterialPlantDetailResp_.Stage;
						m_CurrentCaiLiao.f = templateMaterialPlantDetailResp_.FiveElement;
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(templateMaterialPlantDetailResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
		}

		private void SelectPlantTemp(LZ_WorldCaiLiaoModel model)
		{
			Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(model.a, (List<string> _list) =>
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
				return;
			}
			Singleton<UIManager>.Instance.FindWndByName<LZ_ChuangZaoWnd>("LZ_ChuangZaoWnd.prefab").ChoosedCaoYao(m_CurrentCaiLiao, m_CurrentShu);
			Singleton<UIManager>.Instance.CloseWnd(this);
			Singleton<UIManager>.Instance.CloseWnd("LZ_CaiLiaoShaiXuanWnd.prefab");
		}
	}
}
