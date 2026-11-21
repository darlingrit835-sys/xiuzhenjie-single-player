using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class JiFenDuiHuanWnd : Window
	{
		public JiFenDuiHuanPanel Panel;

		private long m_CurrentJifen;

		private List<bool> m_StatusList;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<JiFenDuiHuanPanel>();
			AddButtonClickListener(Panel.CloseBtn, CloseWindow);
			if (paralist.Length != 0)
			{
				m_CurrentJifen = long.Parse(paralist[0].ToString());
				m_StatusList = paralist[1] as List<bool>;
			}
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Panel.CurrentJiFen.text = $"当前积分：{m_CurrentJifen}分";
			for (int i = 0; i < m_StatusList.Count; i++)
			{
				Panel.JiangLiItems[i].SetValue(i + 1, m_StatusList[i], DuiHuanBtn);
			}
		}

		private void DuiHuanBtn(int id)
		{
			GetJiFenReq_1017006 data = new GetJiFenReq_1017006
			{
				Id = id
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.GetJiFen, data, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					GetJiFenResp_1017006 getJiFenResp_ = new GetJiFenResp_1017006();
					getJiFenResp_.MergeFrom(byteString);
					if (getJiFenResp_.IsSuccess)
					{
						Panel.JiangLiItems[id - 1].ChangeStatus();
					}
					string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(getJiFenResp_.Code);
					MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
				}
			}, isShowWaitUI: true);
		}
	}
}
