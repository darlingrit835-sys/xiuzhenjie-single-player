using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZS_ZhuCeTipWnd : Window
	{
		public ZS_ZhuCeTipPanel Panel;

		private DST_CommonEntity m_DataSource;

		private GameObject m_ItemObj;

		private int status;

		private UnityAction<string> ZhuCeSuccessAction;

		private List<string> dataList = new List<string>();

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			status = 0;
			Panel = base.GameObject.GetComponent<ZS_ZhuCeTipPanel>();
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
			DeleteMethodMatrixReq_1008089 deleteMethodMatrixReq_1008089 = new DeleteMethodMatrixReq_1008089
			{
				MethodMatrixId = m_DataSource.Id
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodMatrix, deleteMethodMatrixReq_1008089, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodMatrixResp_1008089 deleteMethodMatrixResp_ = new DeleteMethodMatrixResp_1008089();
						deleteMethodMatrixResp_.MergeFrom(byteString);
						if (deleteMethodMatrixResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<ZS_ZhuCeWnd>("ZS_ZhuCeWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodMatrixResp_.Code));
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
				Singleton<RoleManager>.Instance.SelectTemplateMatrixDetail(m_DataSource.Id, m_DataSource.Level, m_DataSource.Rank, m_DataSource.FiveElement, (List<string> detail) =>
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
				if (array.Length == 2)
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入阵图名称！");
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
			string[] obj = new string[1] { "阵" };
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("最后一个字以“阵”结尾");
				return;
			}
			RegisterMethodMatrixReq_1008092 data = new RegisterMethodMatrixReq_1008092
			{
				MethodMatrixId = m_DataSource.Id,
				Rename = zhuCeDanFangParamsModel.name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.REGISTERMETHODMATRIX, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterMethodMatrixResp_1008092 registerMethodMatrixResp_ = new RegisterMethodMatrixResp_1008092();
					registerMethodMatrixResp_.MergeFrom(byteString);
					if (registerMethodMatrixResp_.IsSuccess)
					{
						Singleton<UIManager>.Instance.FindWndByName<ZS_ZhuCeWnd>("ZS_ZhuCeWnd.prefab")?.InitData();
					}
					Singleton<UIManager>.Instance.CloseWnd(this);
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodMatrixResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
