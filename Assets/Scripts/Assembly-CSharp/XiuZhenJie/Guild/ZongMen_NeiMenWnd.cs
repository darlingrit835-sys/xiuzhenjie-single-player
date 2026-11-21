using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZongMen_NeiMenWnd : Window
	{
		public ZongMen_NeiMenPanel Panel;

		private string m_HillName;

		private bool m_IsZongZhu;

		private GameObject m_NaJieObj;

		private XzjBoxEntity m_NaJieBox;

		private Dictionary<string, BaseNaJieGoodEntity> m_NaJieGoodsDic;

		private Dictionary<string, BaseNaJieGoodEntity> m_ChooseGoodsDic;

		private Dictionary<string, UI_GoodsItem> m_NaJieGoodsItemDic;

		private Dictionary<string, UI_GoodsItem> m_ChooseGoodsItemDic;

		private Dictionary<int, Sprite> m_GoodsIconSprite;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private int m_GongFaSendTimeRemaining;

		private int m_FaQiSendTimeRemaining;

		private int m_FaShuSendTimeRemaining;

		private float m_Time;

		private Dictionary<int, ZM_NeiMenUpdateEntity> m_NeiMenUpdateDic;

		private Dictionary<int, ZM_LianDanUpdateEntity> m_UpdateDic;

		private int m_CurrentZongMenStage = 3;

		private int m_GongFaCurrentStage = 3;

		private int m_FaQiCurrentStage = 3;

		private int m_FaShuCurrentStage = 3;

		private int m_CurrentDiZiNum;

		private int m_CurrentKeYongDiZiNum;

		private IntoNeiMenResp_1015130 m_DataSource;

		private int m_CurrentJuanZengIndex;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<ZongMen_NeiMenPanel>();
			AddButtonClickListener(Panel.LiKaiBtn, CloseWindow);
			AddButtonClickListener(Panel.AddBtn, AddBtnClick);
			AddButtonClickListener(Panel.JianBtn, JianBtnClick);
			AddButtonClickListener(Panel.QueRenDiZiNumBtn, QueRenDiZiNumBtnBtnClick);
			AddButtonClickListener(Panel.GongFa_JuanZeng_Btn, GongFaJuanZengBtnClick);
			AddButtonClickListener(Panel.FaQi_JuanZeng_Btn, FaQiJuanZengBtnClick);
			AddButtonClickListener(Panel.FaShu_JuanZeng_Btn, FaShuJuanZengBtnClick);
			AddButtonClickListener(Panel.PeiFaFaQiBtn, UpdateFaQiBtnClick);
			AddButtonClickListener(Panel.PeiFaFaShuBtn, UpdateFaShuBtnClick);
			AddButtonClickListener(Panel.PeiFaGongFaBtn, UpdateGongFaBtnClick);
			AddButtonClickListener(Panel.JuanZengCloseBtn, CloseJuanZengPanel);
			AddButtonClickListener(Panel.JuanZengBtn, JuanZengBtnClick);
			Panel.DiZiShuValueInput.onEndEdit.AddListener(InputNumChanged);
			m_UpdateDic = Singleton<FacadeManager>.Instance.SelectLianDanUpdateEntity();
			m_NeiMenUpdateDic = Singleton<FacadeManager>.Instance.SelectNeiMenUpdateEntity();
			m_NaJieGoodsDic = new Dictionary<string, BaseNaJieGoodEntity>();
			m_ChooseGoodsDic = new Dictionary<string, BaseNaJieGoodEntity>();
			m_NaJieGoodsItemDic = new Dictionary<string, UI_GoodsItem>();
			m_ChooseGoodsItemDic = new Dictionary<string, UI_GoodsItem>();
			m_NaJieObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/UI_GoodsItem.prefab");
			m_GoodsIconSprite = Singleton<FacadeManager>.Instance.SelectGoodsSprite();
			m_RankBgSprite = Singleton<FacadeManager>.Instance.SelectGoodsRankBg();
			m_NaJieBox = Singleton<RoleManager>.Instance.RoleBoxDic[10];
			m_NaJieBox.GetAllGoods(() =>
			{
				foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
				{
					if (!m_NaJieGoodsDic.ContainsKey(goods.GoodsId))
					{
						m_NaJieGoodsDic.Add(goods.GoodsId, goods.Copy());
					}
				}
			}, isShowWaiting: false);
			m_HillName = paralist[0].ToString();
			m_DataSource = paralist[1] as IntoNeiMenResp_1015130;
			m_IsZongZhu = (bool)paralist[2];
			Panel.ShanFengNameText.text = m_HillName;
			m_Time = 0f;
			SetDataSource();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			m_Time += Time.deltaTime;
			if (m_Time >= 1f)
			{
				m_Time = 0f;
				GongFaUpdating();
				FaShuUpdating();
				FaQiUpdating();
			}
		}

		private void ReflashPanel()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_IntoNeiMen, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					IntoNeiMenResp_1015130 intoNeiMenResp_ = new IntoNeiMenResp_1015130();
					intoNeiMenResp_.MergeFrom(byteString);
					if (intoNeiMenResp_.IsSuccess)
					{
						m_DataSource = intoNeiMenResp_;
						SetDataSource();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(intoNeiMenResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void SetDataSource()
		{
			if (m_DataSource != null)
			{
				m_GongFaCurrentStage = m_DataSource.GongFaStage;
				m_FaQiCurrentStage = m_DataSource.FaQiStage;
				m_FaShuCurrentStage = m_DataSource.FaShuStage;
				if (m_DataSource.ZhangLaoInfo != null)
				{
					SetLeaderInfo(m_DataSource.ZhangLaoInfo.Sex ? 1 : 0, m_DataSource.ZhangLaoInfo.RoleImage, m_DataSource.ZhangLaoInfo.RoleName, m_DataSource.ZhangLaoInfo.Fighting.ToString(), m_DataSource.ZhangLaoInfo.Level, m_DataSource.ZhangLaoInfo.TeXiaoVal);
				}
				else
				{
					Panel.WuZhangLaoPanel.gameObject.SetActive(value: true);
				}
				m_CurrentDiZiNum = m_DataSource.DiZiNum;
				m_CurrentKeYongDiZiNum = m_DataSource.KeYongDiZiNum;
				ReflashDiZiNum();
				int num = OnceZhanDouLi(m_GongFaCurrentStage, m_FaQiCurrentStage, m_FaShuCurrentStage);
				Panel.RenJunZhanLiText.text = ((m_DataSource.DiZiNum > 0) ? num.ToString() : "0");
				Panel.TotalZhanLiText.text = (m_DataSource.DiZiNum * num).ToString() ?? "";
				Panel.GongFaValue_1_Text.text = $"{WorldSetting.GetStageName(m_GongFaCurrentStage)}五行功法";
				Panel.FaQiValue_1_Text.text = $"{WorldSetting.GetStageName(m_FaQiCurrentStage)}法器套装";
				Panel.FaShuValue_1_Text.text = $"{WorldSetting.GetStageName(m_FaShuCurrentStage)}五行法术";
				Panel.GongFaValue_2_Text.text = $"每人战力最高提升至{OncePartZhanDouLi(m_GongFaCurrentStage)}";
				Panel.FaQiValue_2_Text.text = $"每人战力最高提升至{OncePartZhanDouLi(m_FaQiCurrentStage)}";
				Panel.FaShuValue_2_Text.text = $"每人战力最高提升至{OncePartZhanDouLi(m_FaShuCurrentStage)}";
				ZM_NeiMenUpdateEntity zM_NeiMenUpdateEntity = m_NeiMenUpdateDic[m_GongFaCurrentStage + 1];
				_ = m_NeiMenUpdateDic[m_FaQiCurrentStage + 1];
				_ = m_NeiMenUpdateDic[m_FaShuCurrentStage + 1];
				int num2 = ((zM_NeiMenUpdateEntity.GongFa - m_DataSource.GongFaStock > 0) ? (zM_NeiMenUpdateEntity.GongFa - m_DataSource.GongFaStock) : 0);
				int num3 = ((zM_NeiMenUpdateEntity.FaQi - m_DataSource.FaQiStock > 0) ? (zM_NeiMenUpdateEntity.FaQi - m_DataSource.FaQiStock) : 0);
				int num4 = ((zM_NeiMenUpdateEntity.FaShu - m_DataSource.FaShuStock > 0) ? (zM_NeiMenUpdateEntity.FaShu - m_DataSource.FaShuStock) : 0);
				Panel.PeiFaGongFaShengYuText.text = $"还需{WorldSetting.GetStageName(m_GongFaCurrentStage + 1)}功法{num2}本";
				Panel.PeiFaFaQiShengYuText.text = $"还需{WorldSetting.GetStageName(m_FaQiCurrentStage + 1)}法器{num3}件";
				Panel.PeiFaFaShuShengYuText.text = $"还需{WorldSetting.GetStageName(m_FaShuCurrentStage + 1)}法术{num4}本";
				m_FaQiSendTimeRemaining = (int)(m_DataSource.PeiFaFaQiDanShengYuTime / 1000);
				m_FaShuSendTimeRemaining = (int)(m_DataSource.PeiFaFaShuShengYuTime / 1000);
				m_GongFaSendTimeRemaining = (int)(m_DataSource.PeiFaGongFaShengYuTime / 1000);
				GongFaUpdating();
				FaShuUpdating();
				FaQiUpdating();
			}
		}

		private void ReflashDiZiNum()
		{
			Panel.DiZiShuValueInput.text = m_CurrentDiZiNum.ToString();
			Panel.ShengYuDiZiShuText.text = $"剩余可用弟子人数{m_CurrentKeYongDiZiNum}";
		}

		private void SetLeaderInfo(int sex, string headId, string name, string battleValue, int level, int fuZhiValue)
		{
			Sprite touXiangSprite = Singleton<RoleManager>.Instance.GetTouXiangSprite(sex, headId);
			Panel.TouXianImg.sprite = touXiangSprite;
			Panel.ZhangLaoNameText.text = name;
			Panel.ZhangLaoZhanLiText.text = battleValue;
			Panel.ZhangLaoEffectText.text = $"提升内门战斗力：{fuZhiValue}%";
			Panel.ZhangLaoJingJieText.text = WorldSetting.GetRoleLevelNameByRoleLevel(level);
		}

		private void AddBtnClick()
		{
			if (m_CurrentKeYongDiZiNum < 100)
			{
				m_CurrentDiZiNum += m_CurrentKeYongDiZiNum;
				m_CurrentKeYongDiZiNum = 0;
			}
			else
			{
				m_CurrentDiZiNum += 100;
				m_CurrentKeYongDiZiNum -= 100;
			}
			ReflashDiZiNum();
		}

		private void JianBtnClick()
		{
			if (m_CurrentDiZiNum < 100)
			{
				m_CurrentKeYongDiZiNum += m_CurrentDiZiNum;
				m_CurrentDiZiNum = 0;
			}
			else
			{
				m_CurrentDiZiNum -= 100;
				m_CurrentKeYongDiZiNum += 100;
			}
			ReflashDiZiNum();
		}

		private void InputNumChanged(string value)
		{
			int num = int.Parse(value);
			if (num > m_CurrentDiZiNum)
			{
				if (num - m_CurrentDiZiNum > m_CurrentKeYongDiZiNum)
				{
					m_CurrentDiZiNum += m_CurrentKeYongDiZiNum;
					m_CurrentKeYongDiZiNum = 0;
				}
				else
				{
					m_CurrentKeYongDiZiNum -= num - m_CurrentDiZiNum;
					m_CurrentDiZiNum = num;
				}
			}
			else if (num <= m_CurrentDiZiNum && num > 0)
			{
				m_CurrentKeYongDiZiNum += m_CurrentDiZiNum - num;
				m_CurrentDiZiNum = num;
			}
			else if (num < 0)
			{
				m_CurrentKeYongDiZiNum += m_CurrentDiZiNum;
				m_CurrentDiZiNum = 0;
			}
			ReflashDiZiNum();
		}

		private void QueRenDiZiNumBtnBtnClick()
		{
			NeiMenDiZiNumReq_1015132 data = new NeiMenDiZiNumReq_1015132
			{
				DiZiNum = ((m_CurrentDiZiNum == 0) ? (-1) : m_CurrentDiZiNum)
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_NeiMenDiZiNum, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					NeiMenDiZiNumResp_1015132 neiMenDiZiNumResp_ = new NeiMenDiZiNumResp_1015132();
					neiMenDiZiNumResp_.MergeFrom(byteString);
					if (neiMenDiZiNumResp_.IsSuccess)
					{
						ReflashPanel();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(neiMenDiZiNumResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GongFaJuanZengBtnClick()
		{
			OpenNaJieGoods(1);
		}

		private void FaQiJuanZengBtnClick()
		{
			OpenNaJieGoods(2);
		}

		private void FaShuJuanZengBtnClick()
		{
			OpenNaJieGoods(3);
		}

		private void OpenNaJieGoods(int index)
		{
			m_CurrentJuanZengIndex = index;
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			switch (index)
			{
			case 1:
				list = GetGongFaData();
				Panel.JuanZengPanelTitle.text = "捐赠功法";
				break;
			case 2:
				list = GetFaQiData();
				Panel.JuanZengPanelTitle.text = "捐赠法器";
				break;
			case 3:
				list = GetFaShuData();
				Panel.JuanZengPanelTitle.text = "捐赠法术";
				break;
			}
			CreateNaJieData(list);
			if (list.Count <= 0)
			{
				Panel.JuanZengNullImg.gameObject.SetActive(value: true);
			}
			else
			{
				Panel.JuanZengNullImg.gameObject.SetActive(value: false);
			}
			Panel.JuanZengPanel.gameObject.SetActive(value: true);
		}

		private List<BaseNaJieGoodEntity> GetGongFaData()
		{
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			foreach (BaseNaJieGoodEntity value in m_NaJieGoodsDic.Values)
			{
				if (!value.IsBinding && !value.IsUsing && !value.IsLock && value.Type == 1 && value.GetGoodsStage() == m_GongFaCurrentStage + 1)
				{
					list.Add(value);
				}
			}
			return list;
		}

		private List<BaseNaJieGoodEntity> GetFaShuData()
		{
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			foreach (BaseNaJieGoodEntity value in m_NaJieGoodsDic.Values)
			{
				if (!value.IsBinding && !value.IsUsing && !value.IsLock && value.Type == 6 && value.GetGoodsStage() == m_FaShuCurrentStage + 1)
				{
					list.Add(value);
				}
			}
			return list;
		}

		private List<BaseNaJieGoodEntity> GetFaQiData()
		{
			List<BaseNaJieGoodEntity> list = new List<BaseNaJieGoodEntity>();
			foreach (BaseNaJieGoodEntity value in m_NaJieGoodsDic.Values)
			{
				if (!value.IsBinding && !value.IsUsing && !value.IsLock && (value.Type == 14 || value.Type == 15 || value.Type == 16 || value.Type == 26 || value.Type == 29) && value.GetGoodsStage() == m_FaQiCurrentStage + 1)
				{
					list.Add(value);
				}
			}
			return list;
		}

		private void SetContentHight()
		{
			GridLayoutGroup component = Panel.JuanZengNaJieContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(Panel.JuanZengNaJieContent.childCount, Panel.JuanZengNaJieContent, component);
			GridLayoutGroup component2 = Panel.JuanZengChooseContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(Panel.JuanZengChooseContent.childCount, Panel.JuanZengChooseContent, component2);
		}

		private void RemoveNaJieDic(string id, int count)
		{
			if (!m_NaJieGoodsDic.ContainsKey(id))
			{
				return;
			}
			if (m_NaJieGoodsDic[id].GoodsNum > count)
			{
				m_NaJieGoodsDic[id].GoodsNum -= count;
				if (m_NaJieGoodsItemDic.ContainsKey(id))
				{
					m_NaJieGoodsItemDic[id].ReduceNum(count);
				}
			}
			else
			{
				m_NaJieGoodsDic.Remove(id);
				m_NaJieGoodsItemDic.Remove(id);
			}
		}

		private void RemoveChooseDic(string id, int count)
		{
			if (!m_ChooseGoodsDic.ContainsKey(id))
			{
				return;
			}
			if (m_ChooseGoodsDic[id].GoodsNum > count)
			{
				m_ChooseGoodsDic[id].GoodsNum -= count;
				if (m_ChooseGoodsItemDic.ContainsKey(id))
				{
					m_ChooseGoodsItemDic[id].ReduceNum(count);
				}
			}
			else
			{
				m_ChooseGoodsDic.Remove(id);
				m_ChooseGoodsItemDic.Remove(id);
			}
		}

		private void CreateNaJieData(List<BaseNaJieGoodEntity> baseNaJieGoods)
		{
			foreach (BaseNaJieGoodEntity baseNaJieGood in baseNaJieGoods)
			{
				if (!m_NaJieGoodsItemDic.ContainsKey(baseNaJieGood.GoodsId))
				{
					m_NaJieGoodsItemDic.Add(baseNaJieGood.GoodsId, CreateObjItem(baseNaJieGood, Panel.JuanZengNaJieContent, NaJieGoodsClick));
				}
			}
			GridLayoutGroup component = Panel.JuanZengNaJieContent.gameObject.GetComponent<GridLayoutGroup>();
			SetGrticalListContentHight(baseNaJieGoods.Count, Panel.JuanZengNaJieContent, component);
		}

		private UI_GoodsItem CreateObjItem(BaseNaJieGoodEntity _data, RectTransform rect, UnityAction<object, GameObject, BaseNaJieGoodEntity> clickAction)
		{
			GameObject gameObject = SetListContentParent(rect, m_NaJieObj);
			gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			UI_GoodsItem component = gameObject.GetComponent<UI_GoodsItem>();
			Sprite value = null;
			m_GoodsIconSprite.TryGetValue(_data.Type, out value);
			int levelByRank = WorldSetting.GetLevelByRank(_data.Rank);
			Sprite value2 = null;
			m_RankBgSprite.TryGetValue(levelByRank, out value2);
			component.SetItem(_data, _data.Name, _data.FiveElement, _data.Level, value2, value, (object obj, GameObject gameObj) =>
			{
				if (clickAction != null)
				{
					clickAction(obj, gameObj, _data);
				}
			}, _data.IsUsing, _data.GoodsNum);
			return component;
		}

		private void NaJieGoodsClick(object obj, GameObject gameObj, BaseNaJieGoodEntity data)
		{
			BaseNaJieGoodEntity goods = data;
			if (data.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = goods,
					QueRenCallBack = (int num) =>
					{
						NaJieGoodsChoosed(num, gameObj, data);
					},
					TitleContnet = "捐赠"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				NaJieGoodsChoosed(1, gameObj, data);
			}
		}

		private void NaJieGoodsChoosed(int num, GameObject gameObj, BaseNaJieGoodEntity data)
		{
			if (num > data.GoodsNum)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("数量不足.");
				return;
			}
			gameObj.GetComponent<UI_GoodsItem>();
			if (data.GoodsNum - num <= 0)
			{
				num = data.GoodsNum;
				Object.Destroy(gameObj);
			}
			if (m_ChooseGoodsDic.ContainsKey(data.GoodsId) && m_ChooseGoodsItemDic.ContainsKey(data.GoodsId))
			{
				m_ChooseGoodsDic[data.GoodsId].GoodsNum += num;
				m_ChooseGoodsItemDic[data.GoodsId].ResetNum(m_ChooseGoodsDic[data.GoodsId].GoodsNum);
			}
			else
			{
				BaseNaJieGoodEntity baseNaJieGoodEntity = data.Copy();
				baseNaJieGoodEntity.GoodsNum = num;
				UI_GoodsItem value = CreateObjItem(baseNaJieGoodEntity, Panel.JuanZengChooseContent, ChooseContentGoodsClick);
				m_ChooseGoodsDic.Add(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity);
				m_ChooseGoodsItemDic.Add(baseNaJieGoodEntity.GoodsId, value);
			}
			RemoveNaJieDic(data.GoodsId, num);
			SetContentHight();
		}

		private void ChooseContentGoodsClick(object obj, GameObject gameObj, BaseNaJieGoodEntity data)
		{
			BaseNaJieGoodEntity goods = data;
			if (data.GoodsNum > 1)
			{
				Common_ChooseNumEntity common_ChooseNumEntity = new Common_ChooseNumEntity
				{
					Goods = goods,
					QueRenCallBack = (int num) =>
					{
						ChooseContentGoodsChoosed(num, gameObj, data);
					},
					TitleContnet = "取回"
				};
				Singleton<UIManager>.Instance.PopUpWnd("Common_ChooseGoodsNumWnd.prefab", true, common_ChooseNumEntity);
			}
			else
			{
				ChooseContentGoodsChoosed(1, gameObj, data);
			}
		}

		private void ChooseContentGoodsChoosed(int num, GameObject gameObj, BaseNaJieGoodEntity data)
		{
			if (num > data.GoodsNum)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("数量不足.");
				return;
			}
			gameObj.GetComponent<UI_GoodsItem>();
			if (data.GoodsNum - num <= 0)
			{
				num = data.GoodsNum;
				Object.Destroy(gameObj);
			}
			if (m_NaJieGoodsDic.ContainsKey(data.GoodsId) && m_NaJieGoodsItemDic.ContainsKey(data.GoodsId))
			{
				m_NaJieGoodsDic[data.GoodsId].GoodsNum += num;
				m_NaJieGoodsItemDic[data.GoodsId].ResetNum(m_NaJieGoodsDic[data.GoodsId].GoodsNum);
			}
			else
			{
				BaseNaJieGoodEntity baseNaJieGoodEntity = data.Copy();
				baseNaJieGoodEntity.GoodsNum = num;
				UI_GoodsItem value = CreateObjItem(baseNaJieGoodEntity, Panel.JuanZengNaJieContent, NaJieGoodsClick);
				m_NaJieGoodsDic.Add(baseNaJieGoodEntity.GoodsId, baseNaJieGoodEntity);
				m_NaJieGoodsItemDic.Add(baseNaJieGoodEntity.GoodsId, value);
			}
			RemoveChooseDic(data.GoodsId, num);
			SetContentHight();
		}

		private void JuanZengBtnClick()
		{
			switch (m_CurrentJuanZengIndex)
			{
			case 1:
				JuanZengGongFa();
				break;
			case 2:
				JuanZengFaQi();
				break;
			case 3:
				JuanZengFaShu();
				break;
			}
		}

		private void JuanZengGongFa()
		{
			if (m_ChooseGoodsDic.Count <= 0)
			{
				return;
			}
			JuanZengGongFaReq_1015133 juanZengGongFaReq_ = new JuanZengGongFaReq_1015133();
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> item in m_ChooseGoodsDic)
			{
				juanZengGongFaReq_.Goods.Add(new JuanZengGongFaReq_1015133.Types.GoodsReq_1015150
				{
					Count = item.Value.GoodsNum,
					GoodsId = item.Key
				});
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JuanZengGongFa, juanZengGongFaReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					JuanZengGongFaResp_1015133 juanZengGongFaResp_ = new JuanZengGongFaResp_1015133();
					juanZengGongFaResp_.MergeFrom(byteString);
					if (juanZengGongFaResp_.IsSuccess)
					{
						m_DataSource.GongFaStock = juanZengGongFaResp_.KuCun;
						CloseJuanZengPanel();
						SetDataSource();
						MonoSingleton<GameStart>.Instance.PopUpAttention($"成功捐献功法{juanZengGongFaResp_.SuccessCount}本,捐献失败{juanZengGongFaResp_.FailedCount}本");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(juanZengGongFaResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuanZengFaQi()
		{
			if (m_ChooseGoodsDic.Count <= 0)
			{
				return;
			}
			JuanZengFaQiReq_1015134 juanZengFaQiReq_ = new JuanZengFaQiReq_1015134();
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> item in m_ChooseGoodsDic)
			{
				juanZengFaQiReq_.Goods.Add(new JuanZengFaQiReq_1015134.Types.GoodsReq_1015151
				{
					Count = item.Value.GoodsNum,
					GoodsId = item.Key
				});
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JuanZengFaQi, juanZengFaQiReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					JuanZengFaQiResp_1015134 juanZengFaQiResp_ = new JuanZengFaQiResp_1015134();
					juanZengFaQiResp_.MergeFrom(byteString);
					if (juanZengFaQiResp_.IsSuccess)
					{
						m_DataSource.FaQiStock = juanZengFaQiResp_.KuCun;
						CloseJuanZengPanel();
						SetDataSource();
						MonoSingleton<GameStart>.Instance.PopUpAttention($"成功捐献法器{juanZengFaQiResp_.SuccessCount}件,捐献失败{juanZengFaQiResp_.FailedCount}件");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(juanZengFaQiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void JuanZengFaShu()
		{
			if (m_ChooseGoodsDic.Count <= 0)
			{
				return;
			}
			JuanZengFaShuReq_1015135 juanZengFaShuReq_ = new JuanZengFaShuReq_1015135();
			foreach (KeyValuePair<string, BaseNaJieGoodEntity> item in m_ChooseGoodsDic)
			{
				juanZengFaShuReq_.Goods.Add(new JuanZengFaShuReq_1015135.Types.GoodsReq_1015152
				{
					Count = item.Value.GoodsNum,
					GoodsId = item.Key
				});
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_JuanZengFaShu, juanZengFaShuReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					JuanZengFaShuResp_1015135 juanZengFaShuResp_ = new JuanZengFaShuResp_1015135();
					juanZengFaShuResp_.MergeFrom(byteString);
					if (juanZengFaShuResp_.IsSuccess)
					{
						m_DataSource.FaShuStock = juanZengFaShuResp_.KuCun;
						CloseJuanZengPanel();
						SetDataSource();
						MonoSingleton<GameStart>.Instance.PopUpAttention($"成功捐献法术{juanZengFaShuResp_.SuccessCount}本,捐献失败{juanZengFaShuResp_.FailedCount}本");
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(juanZengFaShuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CloseJuanZengPanel()
		{
			m_NaJieGoodsDic.Clear();
			foreach (BaseNaJieGoodEntity goods in m_NaJieBox.GoodsList)
			{
				if (!m_NaJieGoodsDic.ContainsKey(goods.GoodsId))
				{
					m_NaJieGoodsDic.Add(goods.GoodsId, goods.Copy());
				}
			}
			foreach (Transform item in Panel.JuanZengChooseContent)
			{
				Object.Destroy(item.gameObject);
			}
			foreach (Transform item2 in Panel.JuanZengNaJieContent)
			{
				Object.Destroy(item2.gameObject);
			}
			m_NaJieGoodsItemDic.Clear();
			m_ChooseGoodsItemDic.Clear();
			m_ChooseGoodsDic.Clear();
			Panel.JuanZengPanel.gameObject.SetActive(value: false);
		}

		private string GetZongMenChineseName(int stage)
		{
			return stage switch
			{
				3 => "三阶宗门", 
				4 => "四阶宗门", 
				5 => "五阶宗门", 
				6 => "六阶宗门", 
				7 => "七阶宗门", 
				8 => "八阶宗门", 
				9 => "九阶宗门", 
				_ => "三阶宗门", 
			};
		}

		private int OnceZhanDouLi(int gongFaStage, int faQiStage, int faShuStage)
		{
			return Mathf.FloorToInt(Mathf.Pow(2f, 1f + (float)(gongFaStage + faQiStage + faShuStage) / 3f) * (float)m_CurrentZongMenStage);
		}

		private int OncePartZhanDouLi(int stage)
		{
			return Mathf.FloorToInt(Mathf.Pow(2f, 1 + stage) * (float)m_CurrentZongMenStage);
		}

		private void UpdateGongFaBtnClick()
		{
			ZM_NeiMenUpdateEntity value = null;
			List<string> list = new List<string>();
			if (m_NeiMenUpdateDic.TryGetValue(m_GongFaCurrentStage + 1, out value))
			{
				list.Add($"{WorldSetting.GetStageName(m_GongFaCurrentStage)}功法");
				list.Add(OnceZhanDouLi(m_GongFaCurrentStage, m_FaQiCurrentStage, m_FaShuCurrentStage).ToString());
				list.Add(OnceZhanDouLi(m_GongFaCurrentStage + 1, m_FaQiCurrentStage, m_FaShuCurrentStage).ToString());
				list.Add(m_DataSource.GongFaStock.ToString());
				list.Add(value.GongFa.ToString());
				list.Add(m_UpdateDic[m_GongFaCurrentStage + 1].TimeCast / 60 + "分钟");
				list.Add(GetZongMenChineseName(m_GongFaCurrentStage + 1));
				Panel.UpdateGongFaPanel.gameObject.SetActive(value: true);
				Panel.UpdateGongFaPanel.SetValue(list, SureUpdateGongFaBtnClick);
			}
		}

		private void SureUpdateGongFaBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_PeiFaGongFa, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PeiFaGongFaResp_1015136 peiFaGongFaResp_ = new PeiFaGongFaResp_1015136();
					peiFaGongFaResp_.MergeFrom(byteString);
					if (peiFaGongFaResp_.IsSuccess)
					{
						GongFaFuZhiStartUpgrade((int)(peiFaGongFaResp_.ShengYuTime / 1000));
						Panel.UpdateGongFaPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(peiFaGongFaResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void GongFaFuZhiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpPeiFaGongFaBtn.gameObject.SetActive(value: true);
				m_GongFaSendTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpPeiFaGongFaBtn.gameObject.SetActive(value: false);
			}
		}

		private void UpdateFaQiBtnClick()
		{
			ZM_NeiMenUpdateEntity value = null;
			List<string> list = new List<string>();
			if (m_NeiMenUpdateDic.TryGetValue(m_FaQiCurrentStage + 1, out value))
			{
				list.Add($"{WorldSetting.GetStageName(m_FaQiCurrentStage)}法器");
				list.Add(OnceZhanDouLi(m_GongFaCurrentStage, m_FaQiCurrentStage, m_FaShuCurrentStage).ToString());
				list.Add(OnceZhanDouLi(m_GongFaCurrentStage, m_FaQiCurrentStage + 1, m_FaShuCurrentStage).ToString());
				list.Add(m_DataSource.FaQiStock.ToString());
				list.Add(value.FaQi.ToString());
				list.Add(m_UpdateDic[m_FaQiCurrentStage + 1].TimeCast / 60 + "分钟");
				list.Add(GetZongMenChineseName(m_FaQiCurrentStage + 1));
				Panel.UpdateFaQiPanel.gameObject.SetActive(value: true);
				Panel.UpdateFaQiPanel.SetValue(list, SureUpdateFaQiBtnClick);
			}
		}

		private void SureUpdateFaQiBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_PeiFaFaQi, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PeiFaFaQiResp_1015138 peiFaFaQiResp_ = new PeiFaFaQiResp_1015138();
					peiFaFaQiResp_.MergeFrom(byteString);
					if (peiFaFaQiResp_.IsSuccess)
					{
						FaQiFuZhiStartUpgrade((int)(peiFaFaQiResp_.ShengYuTime / 1000));
						Panel.UpdateFaQiPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(peiFaFaQiResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FaQiFuZhiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpPeiFaFaQiBtn.gameObject.SetActive(value: true);
				m_FaQiSendTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpPeiFaFaQiBtn.gameObject.SetActive(value: false);
			}
		}

		private void UpdateFaShuBtnClick()
		{
			ZM_NeiMenUpdateEntity value = null;
			List<string> list = new List<string>();
			if (m_NeiMenUpdateDic.TryGetValue(m_FaShuCurrentStage + 1, out value))
			{
				list.Add($"{WorldSetting.GetStageName(m_FaShuCurrentStage)}法术");
				list.Add(OnceZhanDouLi(m_GongFaCurrentStage, m_FaQiCurrentStage, m_FaShuCurrentStage).ToString());
				list.Add(OnceZhanDouLi(m_GongFaCurrentStage, m_FaQiCurrentStage, m_FaShuCurrentStage + 1).ToString());
				list.Add(m_DataSource.FaShuStock.ToString());
				list.Add(value.FaShu.ToString());
				list.Add(m_UpdateDic[m_FaShuCurrentStage + 1].TimeCast / 60 + "分钟");
				list.Add(GetZongMenChineseName(m_FaShuCurrentStage + 1));
				Panel.UpdateFaShuPanel.gameObject.SetActive(value: true);
				Panel.UpdateFaShuPanel.SetValue(list, SureUpdateFaShuBtnClick);
			}
		}

		private void SureUpdateFaShuBtnClick()
		{
			Singleton<NetManager>.Instance.SendMsg(MsgType.GUILD_PeiFaFaShu, null, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					PeiFaFaShuResp_1015140 peiFaFaShuResp_ = new PeiFaFaShuResp_1015140();
					peiFaFaShuResp_.MergeFrom(byteString);
					if (peiFaFaShuResp_.IsSuccess)
					{
						FaShuFuZhiStartUpgrade((int)(peiFaFaShuResp_.ShengYuTime / 1000));
						Panel.UpdateFaShuPanel.gameObject.SetActive(value: false);
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(peiFaFaShuResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void FaShuFuZhiStartUpgrade(int UpgradeSecond)
		{
			if (UpgradeSecond > 0)
			{
				Panel.SpeedUpPeiFaFaShuBtn.gameObject.SetActive(value: true);
				m_FaShuSendTimeRemaining = UpgradeSecond;
			}
			else
			{
				Panel.SpeedUpPeiFaFaShuBtn.gameObject.SetActive(value: false);
			}
		}

		private void GongFaUpdating()
		{
			if (m_GongFaSendTimeRemaining > 0)
			{
				m_GongFaSendTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_GongFaSendTimeRemaining);
				Panel.PeiFaFaQiOtherTimeText.text = text;
				if (m_GongFaSendTimeRemaining <= 0)
				{
					m_GongFaSendTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void FaQiUpdating()
		{
			if (m_FaQiSendTimeRemaining > 0)
			{
				m_FaQiSendTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_FaQiSendTimeRemaining);
				Panel.PeiFaFaQiOtherTimeText.text = text;
				if (m_FaQiSendTimeRemaining <= 0)
				{
					m_FaQiSendTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		private void FaShuUpdating()
		{
			if (m_FaShuSendTimeRemaining > 0)
			{
				m_FaShuSendTimeRemaining--;
				string text = CommonMethod.SecondToHour(m_FaShuSendTimeRemaining);
				Panel.PeiFaFaShuOtherTimeText.text = text;
				if (m_FaShuSendTimeRemaining <= 0)
				{
					m_FaShuSendTimeRemaining = 0;
					ReflashPanel();
				}
			}
		}

		public override void OnClose()
		{
			base.OnClose();
			Panel.DiZiShuValueInput.onEndEdit.RemoveAllListeners();
		}
	}
}
