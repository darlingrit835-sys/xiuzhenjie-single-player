using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class DST_ZhuCeTipWnd : Window
	{
		public DST_ZhuCeTipPanel Panel;

		private DST_DanFangModel m_DataSource;

		private GameObject m_ItemObj;

		private UnityAction<string> ZhuCeSuccessAction;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DST_ZhuCeTipPanel>();
			if (paralist.Length != 0)
			{
				m_DataSource = paralist[0] as DST_DanFangModel;
				int result = 0;
				if (paralist[1] != null)
				{
					int.TryParse(paralist[1].ToString(), out result);
				}
				if (result == 1 || result == 2)
				{
					Panel.DanNameText.text = m_DataSource.b;
					Panel.DanNameText.interactable = false;
					Panel.QueRenBtn.interactable = false;
					Panel.ShanChuBtn.interactable = false;
				}
			}
			m_ItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DstZhuCeItem.prefab");
			AddButtonClickListener(Panel.CloseBtn, CloseBtnClick);
			AddButtonClickListener(Panel.QueRenBtn, QueRenBtnClick);
			AddButtonClickListener(Panel.WenHaoBtn, WenHaoBtnClick);
			AddButtonClickListener(Panel.ShanChuBtn, ShanChuBtnClick);
			InitDetail();
		}

		private void WenHaoBtnClick()
		{
			string text = "命名规则:\n1、至少3个字\n2、至多5个字\n3、最后一个字以“散,药,丹,丸”结尾\n\n\n";
			Singleton<UIManager>.Instance.PopUpWnd("PopUpWnd.prefab", true, text, null, "命名规则", 6);
		}

		private void ShanChuBtnClick()
		{
			DeleteMethodElixirReq_1008094 deleteMethodElixirReq_1008094 = new DeleteMethodElixirReq_1008094
			{
				MethodElixirId = m_DataSource.a
			};
			MonoSingleton<GameStart>.Instance.PopUpAttention("请确认是否删除", () =>
			{
				Singleton<NetManager>.Instance.SendMsg(MsgType.DeleteMethodElixir, deleteMethodElixirReq_1008094, (object obj) =>
				{
					MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
					ByteString byteString = obj as ByteString;
					if (!(byteString == null))
					{
						DeleteMethodElixirResp_1008094 deleteMethodElixirResp_ = new DeleteMethodElixirResp_1008094();
						deleteMethodElixirResp_.MergeFrom(byteString);
						if (deleteMethodElixirResp_.IsSuccess)
						{
							Singleton<UIManager>.Instance.FindWndByName<DST_ZhuCeWnd>("DST_ZhuCeWnd.prefab")?.InitData();
							Singleton<UIManager>.Instance.CloseWnd(this);
						}
						else
						{
							MonoSingleton<GameStart>.Instance.PopUpAttention(MonoSingleton<GameStart>.Instance.GetHintMsgByCode(deleteMethodElixirResp_.Code));
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
			Singleton<RoleManager>.Instance.SelectTemplateElixirDetail(m_DataSource.a, m_DataSource.e, m_DataSource.c, m_DataSource.d, (List<string> detail) =>
			{
				GridLayoutGroup component = Panel.ScrollViewContent.gameObject.GetComponent<GridLayoutGroup>();
				foreach (string item in detail)
				{
					string[] array = item.Split(',');
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
				SetGrticalListContentHight(detail.Count, Panel.ScrollViewContent, component);
			});
		}

		private void CloseBtnClick()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void QueRenBtnClick()
		{
			ZhuCeDanFangParamsModel zhuCeDanFangParamsModel = new ZhuCeDanFangParamsModel();
			zhuCeDanFangParamsModel.registerId = m_DataSource.a;
			zhuCeDanFangParamsModel.name = Panel.DanNameText.text;
			if (string.IsNullOrEmpty(zhuCeDanFangParamsModel.name) || zhuCeDanFangParamsModel.name.Length == 0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("请输入丹方名称！");
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
			string[] obj = new string[4] { "散", "药", "丹", "丸" };
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
				MonoSingleton<GameStart>.Instance.PopUpAttention("最后一个字以“散,药,丹,丸”结尾");
				return;
			}
			RegisterMethodElixirReq_1008017 data = new RegisterMethodElixirReq_1008017
			{
				MethodElixirId = m_DataSource.a,
				Rename = zhuCeDanFangParamsModel.name
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.XSJ_DST_ZHUCEDANFANG, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					RegisterMethodElixirResp_1008017 registerMethodElixirResp_ = new RegisterMethodElixirResp_1008017();
					registerMethodElixirResp_.MergeFrom(byteString);
					if (registerMethodElixirResp_.IsSuccess)
					{
						ChangeScene(registerMethodElixirResp_);
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
					else
					{
						string hintMsgByCode2 = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodElixirResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode2, () =>
						{
							Singleton<UIManager>.Instance.CloseWnd(this);
						});
					}
				}
			}, isShowWaitUI: true);
		}

		private void ChangeScene(RegisterMethodElixirResp_1008017 registerMethodElixirResp_1008017)
		{
			if (Singleton<GameMapManager>.Instance.CurrentMapName == "MainScene")
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.FindWndByName<DST_ZhuCeWnd>("DST_ZhuCeWnd.prefab")?.InitData();
				return;
			}
			string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(registerMethodElixirResp_1008017.Code);
			MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianDanJieSuanWnd.prefab");
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			}, () =>
			{
				Singleton<UIManager>.Instance.CloseWnd(this);
				Singleton<UIManager>.Instance.CloseWnd("DiHuoGeLianDanJieSuanWnd.prefab");
				Singleton<GameMapManager>.Instance.LoadScene("LoadingWnd.prefab", "MainScene");
			});
		}
	}
}
