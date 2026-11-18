using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class LZ_ZhuCeTipWnd : Window
	{
		public LZ_ZhuCeTipPanel Panel;

		private DST_CommonEntity m_DataSource;

		private GameObject m_ItemObj;

		private int status;

		private UnityAction<string> ZhuCeSuccessAction;

		private List<string> dataList = new List<string>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			status = 0;
			Panel = base.GameObject.GetComponent<LZ_ZhuCeTipPanel>();
			if (paralist.Length != 0)
			{
				if (paralist.Length > 1)
				{
					dataList = paralist[1] as List<string>;
					status = (int)paralist[2];
				}
				m_DataSource = paralist[0] as DST_CommonEntity;
			}
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			AddButtonClickListener(Panel.ShanChuBtn, ShanChuBtnClick);
			InitDetail();
		}

		private void ShanChuBtnClick()
		{
			DeleteMethodPlantReq_1008096 deleteMethodPlantReq_1008096 = new DeleteMethodPlantReq_1008096
			{
				MethodPlantId = m_DataSource.Id
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodPlant, deleteMethodPlantReq_1008096, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodPlantResp_1008096 deleteMethodPlantResp_ = new DeleteMethodPlantResp_1008096();
						deleteMethodPlantResp_.MergeFrom(byteString);
						if (deleteMethodPlantResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<LZ_ZhuCeWnd>("LT_ZhongZiWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodPlantResp_.Code));
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
				Singleton<RoleManager>.Instance.SelectTemplatePlantDetail(m_DataSource.Id, (List<string> detail) =>
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入灵植名称！");
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
			string[] obj = new string[14]
			{
				"竹", "芽", "叶", "兰", "果", "芝", "参", "根", "藤", "花",
				"枝", "木", "草", "子"
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("最后一个字以“竹,芽,叶,兰,果,芝,参,根,藤,花,枝,木,草,子”结尾");
				return;
			}
			RegisterTemplatePlantReq_1008061 data = new RegisterTemplatePlantReq_1008061
			{
				TemplatePlantId = m_DataSource.Id,
				Rename = zhuCeDanFangParamsModel.name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REGISTER_TEMPLATE_PLANT_RESP, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterTemplatePlantResp_1008061 registerTemplatePlantResp_ = new RegisterTemplatePlantResp_1008061();
					registerTemplatePlantResp_.MergeFrom(byteString);
					if (registerTemplatePlantResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<LZ_ZhuCeWnd>("LT_ZhongZiWnd.prefab")?.InitData();
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerTemplatePlantResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
