using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LQ_ZhuCeTipWnd : Window
	{
		public LQ_ZhuCeTipPanel Panel;

		private LianQi_CommonEntity m_DataSource;

		private GameObject m_ItemObj;

		private int status;

		private UnityAction<string> ZhuCeSuccessAction;

		private List<string> dataList = new List<string>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			status = 0;
			Panel = base.GameObject.GetComponent<LQ_ZhuCeTipPanel>();
			if (paralist.Length != 0)
			{
				if (paralist.Length > 1)
				{
					dataList = paralist[1] as List<string>;
					status = (int)paralist[2];
				}
				m_DataSource = paralist[0] as LianQi_CommonEntity;
			}
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			AddButtonClickListener(Panel.ShanChuBtn, ShanChuBtnClick);
			InitDetail();
		}

		private void ShanChuBtnClick()
		{
			DeleteMethodEquipmentReq_1008095 deleteMethodEquipmentReq_1008095 = new DeleteMethodEquipmentReq_1008095
			{
				MethodEquipmentId = m_DataSource.Id
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodEquipment, deleteMethodEquipmentReq_1008095, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodEquipmentResp_1008095 deleteMethodEquipmentResp_ = new DeleteMethodEquipmentResp_1008095();
						deleteMethodEquipmentResp_.MergeFrom(byteString);
						if (deleteMethodEquipmentResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<LianQi_ZhuCeWnd>("LianQi_ZhuCeWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodEquipmentResp_.Code));
						}
					}
				}, isShowWaitUI: true);
			});
		}

		private void InitDetail()
		{
			if (m_DataSource == null)
			{
				return;
			}
			if (status != -1)
			{
				Singleton<RoleManager>.Instance.SelectTemplateQiTuDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> detail) =>
				{
					GridLayoutGroup component3 = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
					foreach (string item in detail)
					{
						string[] array2 = item.Split(',');
						if (array2[0] != "产地")
						{
							DST_ZhuCeTipItem component4 = SetListContentParent(Panel.ScrollViewContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
							if (array2 != null && array2.Length == 2)
							{
								component4?.SetValue(array2[0] + "：", array2[1], "");
							}
							else if (array2 != null && array2.Length == 3)
							{
								component4?.SetValue(array2[0] + "：", array2[1], array2[2]);
							}
						}
						else
						{
							DST_ZhuCeTipItem component5 = SetListContentParent(Panel.ScrollViewContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
							if (array2 != null && array2.Length == 2)
							{
								component5?.SetValue(array2[0] + "：", "虚神界", "");
							}
							else if (array2 != null && array2.Length == 3)
							{
								component5?.SetValue(array2[0] + "：", "虚神界", array2[2]);
							}
						}
					}
					SetGrticalListContentHight(detail.Count, Panel.ScrollViewContent, component3);
				});
				return;
			}
			GridLayoutGroup component = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
			foreach (string data in dataList)
			{
				string[] array = data.Split(',');
				DST_ZhuCeTipItem component2 = SetListContentParent(Panel.ScrollViewContent, m_ItemObj).gameObject.GetComponent<DST_ZhuCeTipItem>();
				if (array != null && array.Length == 2)
				{
					component2?.SetValue(array[0] + "：", array[1], "");
				}
				else if (array != null && array.Length == 3)
				{
					component2?.SetValue(array[0] + "：", array[1], array[2]);
				}
			}
			SetGrticalListContentHight(dataList.Count, Panel.ScrollViewContent, component);
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenBtnClick()
		{
			ZhuCeDanFangParamsModel zhuCeDanFangParamsModel = new ZhuCeDanFangParamsModel();
			zhuCeDanFangParamsModel.registerId = m_DataSource.Id;
			zhuCeDanFangParamsModel.name = Panel.DanNameText.text;
			if (string.IsNullOrEmpty(zhuCeDanFangParamsModel.name) || zhuCeDanFangParamsModel.name.Length == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入器图名称！");
				return;
			}
			for (int i = 0; i < zhuCeDanFangParamsModel.name.Length; i++)
			{
				if (zhuCeDanFangParamsModel.name[i] <= '\u007f')
				{
					MonoSingleton<GameStart>.Instance.PopUpAttention("请输入汉字！");
					return;
				}
			}
			if (zhuCeDanFangParamsModel.name.Length < 3)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("至少需要3个字");
				return;
			}
			if (zhuCeDanFangParamsModel.name.Length > 5)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最多只能5个字");
				return;
			}
			string[] obj = new string[30]
			{
				"爪", "斧", "弓", "剑", "靴", "钺", "衣", "裳", "戟", "琴",
				"印", "镯", "刀", "枪", "锤", "棒", "盾", "旗", "索", "矛",
				"珠", "幡", "氅", "绫", "鼎", "针", "钟", "铃", "尺", "甲"
			};
			string text = zhuCeDanFangParamsModel.name.Substring(zhuCeDanFangParamsModel.name.Length - 1, 1);
			bool flag = false;
			string[] array = obj;
			foreach (string value in array)
			{
				if (text.Equals(value))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("最后一个字以“爪,斧,弓,剑,靴,钺,衣,裳,戟,琴,印,镯,刀,枪,锤,棒,盾,旗,索,矛,珠,幡,氅,绫,鼎,针,钟,铃,尺,甲”结尾");
				return;
			}
			RegisterMethodEquipmentReq_1008072 data = new RegisterMethodEquipmentReq_1008072
			{
				MethodEquipmentId = m_DataSource.Id,
				Rename = zhuCeDanFangParamsModel.name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REGISTER_METHOD_EQUIPMENT, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterMethodEquipmentResp_1008072 registerMethodEquipmentResp_ = new RegisterMethodEquipmentResp_1008072();
					registerMethodEquipmentResp_.MergeFrom(byteString);
					if (registerMethodEquipmentResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<LianQi_ZhuCeWnd>("LianQi_ZhuCeWnd.prefab")?.InitData();
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodEquipmentResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
