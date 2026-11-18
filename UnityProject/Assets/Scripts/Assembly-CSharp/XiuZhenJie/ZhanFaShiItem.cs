using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanFaShiItem : MonoBehaviour
	{
		public Image BenShen;

		public Text DiYiJiNengShuLianLevel;

		public Text DiYiJiNengShuLianDu;

		public Image DiYiJiNengShiFaTiao;

		public Button XiuXingBtn;

		public Button XiuXingBtnNew;

		private string Rank;

		private string ShuLianDengji;

		private string JianSHaoXiaoGuo;

		public PlayerSkillModel FashuDanGe;

		public UI_GoodsItem UI_GoodsItem;

		private int m_ShuLianDengjiMax;

		private bool m_YuXing;

		private bool m_YingChang;

		private float m_Translation;

		private Sprite m_RankSpr;

		private Sprite m_IconSpr;

		private void Update()
		{
			if (!m_YuXing)
			{
				return;
			}
			m_Translation = Time.deltaTime * 0.34f;
			if (DiYiJiNengShiFaTiao.fillAmount < 1f)
			{
				DiYiJiNengShiFaTiao.fillAmount += m_Translation;
				m_YingChang = true;
				PanDuan(panduan: false);
				return;
			}
			if (DiYiJiNengShuLianLevel.text == "生疏" && ShuLianDengji == "小成")
			{
				XiuXingBtn.gameObject.SetActive(value: true);
				XiuXingBtnNew.gameObject.SetActive(value: false);
				UI_GoodsItem.SetItem(FashuDanGe, FashuDanGe.name, FashuDanGe.fiveElement, FashuDanGe.level, m_RankSpr, m_IconSpr, null);
			}
			DiYiJiNengShuLianLevel.text = ShuLianDengji;
			DiYiJiNengShuLianDu.text = ((ShuLianDengji == "大圆满") ? "已满" : $"{FashuDanGe.shuLianDu}/{FashuDanGe.shuLianDuMax}");
			DiYiJiNengShiFaTiao.fillAmount = 0f;
			MonoSingleton<GameStart>.Instance.PopRollingTextShow("修炼成功!");
			m_YuXing = false;
			PanDuan(panduan: true);
			m_YingChang = false;
		}

		private void PanDuan(bool panduan)
		{
			if (m_YingChang)
			{
				Singleton<UIManager>.Instance.FindWndByName<ZhanFaShiWnd>("ZhanFaShiWnd.prefab")?.PanduanQieHuan(panduan);
			}
		}

		public void SetValue(PlayerSkillModel fashu, Sprite iconSpr, Sprite rankSpr)
		{
			FashuDanGe = fashu;
			BtnOnClick();
			HuoQuShuLianDengji(fashu.shuLianDuLevel);
			DiYiJiNengShuLianLevel.text = ShuLianDengji;
			DiYiJiNengShuLianDu.text = ((ShuLianDengji == "大圆满") ? "已满" : $"{FashuDanGe.shuLianDu}/{FashuDanGe.shuLianDuMax}");
			DiYiJiNengShiFaTiao.fillAmount = 0f;
			m_RankSpr = rankSpr;
			m_IconSpr = iconSpr;
			if (fashu.shuLianDuLevel == 1)
			{
				XiuXingBtnNew.gameObject.SetActive(value: true);
				XiuXingBtn.gameObject.SetActive(value: false);
				UI_GoodsItem.SetItem(fashu, fashu.name, fashu.fiveElement, fashu.level, rankSpr, iconSpr, FirstXiuLian);
			}
			else
			{
				XiuXingBtnNew.gameObject.SetActive(value: false);
				XiuXingBtn.gameObject.SetActive(value: true);
				UI_GoodsItem.SetItem(fashu, fashu.name, fashu.fiveElement, fashu.level, rankSpr, iconSpr, null);
			}
		}

		private void FirstXiuLian(object dataSource, GameObject itemObj)
		{
			if (dataSource is PlayerSkillModel playerSkillModel)
			{
				DianJiShiJian(playerSkillModel.id, 1);
			}
		}

		private void HuoQu(BaseSkillModel fashu)
		{
			switch (WorldSetting.GetLevelByRank(fashu.rank))
			{
			case 1:
				Rank = "黄";
				break;
			case 2:
				Rank = "玄";
				break;
			case 3:
				Rank = "地";
				break;
			case 4:
				Rank = "天";
				break;
			}
		}

		private void HuoQuShuLianDengji(int level)
		{
			switch (level)
			{
			case 1:
				ShuLianDengji = "生疏";
				JianSHaoXiaoGuo = "0息";
				break;
			case 2:
				ShuLianDengji = "小成";
				JianSHaoXiaoGuo = "0.25息";
				break;
			case 3:
				ShuLianDengji = "大成";
				JianSHaoXiaoGuo = "0.5息";
				break;
			case 4:
				ShuLianDengji = "小圆满";
				JianSHaoXiaoGuo = "0.75息";
				break;
			case 5:
				ShuLianDengji = "大圆满";
				JianSHaoXiaoGuo = "1息";
				break;
			}
		}

		public void DianJiShiJian(string jiNengId, int dinaJiChiShu)
		{
			if (m_YuXing)
			{
				return;
			}
			PracticeSkillReq_1005040 data = new PracticeSkillReq_1005040
			{
				MethodSkillId = jiNengId,
				PracticeNum = dinaJiChiShu
			};
			Singleton<NetManager>.Instance.SendMsg(MsgType.PRACTICE_SKILL, data, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					PracticeSkillResp_1005040 practiceSkillResp_ = new PracticeSkillResp_1005040();
					practiceSkillResp_.MergeFrom(byteString);
					if (practiceSkillResp_.IsSuccess)
					{
						HuoQuShuLianDengji(practiceSkillResp_.Level);
						FashuDanGe.shuLianDu = practiceSkillResp_.Proficiency;
						FashuDanGe.shuLianDuMax = practiceSkillResp_.MaxProficiency;
						Singleton<RoleManager>.Instance.Role_Entity.CurrentVigor = practiceSkillResp_.CurrentVigor;
						Singleton<RoleManager>.Instance.SetFuZhiYeCurrentExp(practiceSkillResp_.LiveLevel, practiceSkillResp_.LiveProficiency, RoleFuZhiType.FaShu);
						Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(practiceSkillResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			});
			m_YuXing = true;
		}

		private void XiangQing()
		{
			if (ShuLianDengji == "大圆满")
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("当前法术已大圆满，无法继续修炼");
			}
			else if (!m_YuXing)
			{
				ZhanFaShiXianQingGoMai zhanFaShiXianQingGoMai = new ZhanFaShiXianQingGoMai
				{
					Name = FashuDanGe.name,
					ShuLianDu = FashuDanGe.shuLianDu.ToString(),
					ShuLianLevel = ShuLianDengji,
					ShiFaTime = JianSHaoXiaoGuo
				};
				Singleton<UIManager>.Instance.PopUpWnd("ZhanFaShiXiangQingWnd.prefab", true, zhanFaShiXianQingGoMai, this);
			}
		}

		private void BtnOnClick()
		{
			XiuXingBtn.onClick.RemoveAllListeners();
			XiuXingBtn.onClick.AddListener(XiangQing);
		}
	}
}
