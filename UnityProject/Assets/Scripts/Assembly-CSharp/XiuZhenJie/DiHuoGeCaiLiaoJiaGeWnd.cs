using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

namespace XiuZhenJie
{
	public class DiHuoGeCaiLiaoJiaGeWnd : Window
	{
		public DiHuoGeCaiLiaoJiaGePanel Panel;

		private GameObject m_caiLiaoJiaGeItem;

		private List<CaiLiaoJiaGe> CaiLiaoList;

		private Dictionary<string, DiHuoCaiLiaoJiaGeItem> m_CaiLiaoDic;

		private DanFangModel m_DataSource;

		private QiTuEntity m_LianQiDataSource;

		private int CaiLiaoZoJiaGe;

		private DiHuoGeLianZhiType m_CurrentLianZhiType;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			Panel = base.GameObject.GetComponent<DiHuoGeCaiLiaoJiaGePanel>();
			AddButtonClickListener(Panel.TuChu, CloseWnd);
			AddButtonClickListener(Panel.QueRenBtn, QueRen);
			m_caiLiaoJiaGeItem = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/DiHuoCaiLiaoJiaGeItem.prefab");
			m_CaiLiaoDic = new Dictionary<string, DiHuoCaiLiaoJiaGeItem>();
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			if (paralist.Length >= 3)
			{
				CaiLiaoList = paralist[1] as List<CaiLiaoJiaGe>;
				m_CurrentLianZhiType = DiHuoGeLianZhiType.Self;
				int currentLianZhiType = int.Parse(paralist[2].ToString());
				m_CurrentLianZhiType = (DiHuoGeLianZhiType)currentLianZhiType;
				if (m_CurrentLianZhiType == DiHuoGeLianZhiType.Other || m_CurrentLianZhiType == DiHuoGeLianZhiType.Self)
				{
					m_DataSource = paralist[0] as DanFangModel;
				}
				else
				{
					m_LianQiDataSource = paralist[0] as QiTuEntity;
				}
			}
			InitData();
		}

		private void InitData()
		{
			if (m_CurrentLianZhiType == DiHuoGeLianZhiType.Other || m_CurrentLianZhiType == DiHuoGeLianZhiType.Self)
			{
				if (m_DataSource == null)
				{
					return;
				}
			}
			else if (m_LianQiDataSource == null)
			{
				return;
			}
			CreateDataAsny();
		}

		private void CreateDataAsny()
		{
			CaiLiaoZoJiaGe = 0;
			if (CaiLiaoList != null)
			{
				for (int i = 0; i < CaiLiaoList.Count; i++)
				{
					DiHuoCaiLiaoJiaGeItem component = SetListContentParent(Panel.ScrollContent, m_caiLiaoJiaGeItem).GetComponent<DiHuoCaiLiaoJiaGeItem>();
					component.SetValue(CaiLiaoList[i].c, (int)CaiLiaoList[i].b);
					int num = (int)CaiLiaoList[i].b;
					CaiLiaoZoJiaGe += num;
					if (!m_CaiLiaoDic.ContainsKey(CaiLiaoList[i].a))
					{
						m_CaiLiaoDic.Add(CaiLiaoList[i].a, component);
					}
				}
			}
			else
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("当前材料暂无！");
				Singleton<UIManager>.Instance.CloseWnd(this);
			}
			Panel.JiaGe.text = $"合计:<color=#A00200>{CaiLiaoZoJiaGe}</color>灵石";
		}

		private void QueRen()
		{
			if (CaiLiaoList != null && CaiLiaoList.Count > 0)
			{
				GoMaiBtn();
				return;
			}
			MonoSingleton<GameStart>.Instance.PopUpAttention("当前无材料！");
			CloseWnd();
		}

		private void GoMaiBtn()
		{
			if (Singleton<RoleManager>.Instance.ShengYuLingShi(CaiLiaoZoJiaGe) < 0.0)
			{
				MonoSingleton<GameStart>.Instance.PopUpAttention("灵石不足");
				return;
			}
			OneKeyBuyMaterialsReq_1004030 oneKeyBuyMaterialsReq_ = new OneKeyBuyMaterialsReq_1004030();
			foreach (CaiLiaoJiaGe caiLiao in CaiLiaoList)
			{
				oneKeyBuyMaterialsReq_.MaterialId.Add(caiLiao.a);
			}
			Singleton<NetManager>.Instance.SendMsg(MsgType.ONEKEY_BUY_MATERIALS, oneKeyBuyMaterialsReq_, (object _obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = _obj as ByteString;
				if (!(byteString == null))
				{
					OneKeyBuyMaterialsResp_1004030 oneKeyBuyMaterialsResp_ = new OneKeyBuyMaterialsResp_1004030();
					oneKeyBuyMaterialsResp_.MergeFrom(byteString);
					if (oneKeyBuyMaterialsResp_.IsSuccess)
					{
						Singleton<RoleManager>.Instance.Role_Entity.ShinyStone = oneKeyBuyMaterialsResp_.ShinyStone;
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
						CloseWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(oneKeyBuyMaterialsResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		private void CloseWnd()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
			switch (m_CurrentLianZhiType)
			{
			case DiHuoGeLianZhiType.Self:
				Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoShaiXuanWnd.prefab", true, m_DataSource);
				break;
			case DiHuoGeLianZhiType.Other:
				Singleton<UIManager>.Instance.PopUpWnd("DiHuoGeCaiLiaoShaiXuan_LK_Wnd.prefab", true, m_DataSource);
				break;
			case DiHuoGeLianZhiType.LianQiSelf:
				Singleton<UIManager>.Instance.PopUpWnd("LianQiCaiLiaoShaiXuanWnd.prefab", true, m_LianQiDataSource);
				break;
			case DiHuoGeLianZhiType.LianQiOther:
				Singleton<UIManager>.Instance.PopUpWnd("LianQiCaiLiaoShaiXuan_LK_Wnd.prefab", true, m_LianQiDataSource);
				break;
			}
		}
	}
}
