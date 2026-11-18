using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class ZhanFaShiWnd : Window
	{
		private ZhanFaShiPanel m_Panel;

		private int m_XuLiHao;

		private List<PlayerSkillModel> m_JiNengList;

		private string m_Rank;

		private FaShuModel m_ShuLianDu;

		private string m_ShuLianDengji;

		private int m_ShuLianDengjiMax;

		private Dictionary<int, Sprite> m_RankBgSprite;

		private Dictionary<int, Sprite> m_SkillIconSprite;

		private GameObject m_InstanceItemObj;

		private GridLayoutGroup m_GlgroupYi;

		private GridLayoutGroup m_GlgroupEr;

		private GridLayoutGroup m_GlgroupSan;

		public override void Awake(params object[] paralist)
		{
			base.Awake(paralist);
			m_Panel = base.GameObject.GetComponent<ZhanFaShiPanel>();
			AddButtonClickListener(m_Panel.TuiChu, TuiChuBut);
			AddButtonClickListener(m_Panel.WenHao, XianShi);
			AddButtonClickListener(m_Panel.QieHuan, QieHuan);
			ResourceLoad();
			m_InstanceItemObj = Singleton<ResourceManager>.Instance.LoadResource<GameObject>("Assets/GameData/Prefabs/Items/ZhanFaShiItem.prefab");
			m_GlgroupYi = m_Panel.ItemListContentYi.GetComponent<GridLayoutGroup>();
			m_GlgroupEr = m_Panel.ItemListContentEr.GetComponent<GridLayoutGroup>();
			m_GlgroupSan = m_Panel.ItemListContentSan.GetComponent<GridLayoutGroup>();
		}

		private void ResourceLoad()
		{
			m_RankBgSprite = new Dictionary<int, Sprite>();
			m_SkillIconSprite = new Dictionary<int, Sprite>();
			Sprite value = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Huang.png");
			Sprite value2 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Xuan.png");
			Sprite value3 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Di.png");
			Sprite value4 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/Rank_Tian.png");
			m_RankBgSprite.Add(1, value);
			m_RankBgSprite.Add(2, value2);
			m_RankBgSprite.Add(3, value3);
			m_RankBgSprite.Add(4, value4);
			Sprite value5 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_AreaDamage.png");
			Sprite value6 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_ContinueDamage.png");
			Sprite value7 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DefenceSkill.png");
			Sprite value8 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_DownStatus.png");
			Sprite value9 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_NoMove.png");
			Sprite value10 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_OneDamage.png");
			Sprite value11 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_UpStatus.png");
			Sprite value12 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			Sprite value13 = Singleton<ResourceManager>.Instance.LoadResource<Sprite>("Assets/GameData/Textures/Item/FaShu_MoveAdd.png");
			m_SkillIconSprite.Add(14, value5);
			m_SkillIconSprite.Add(12, value6);
			m_SkillIconSprite.Add(20, value7);
			m_SkillIconSprite.Add(40, value8);
			m_SkillIconSprite.Add(41, value9);
			m_SkillIconSprite.Add(10, value10);
			m_SkillIconSprite.Add(30, value11);
			m_SkillIconSprite.Add(31, value11);
			m_SkillIconSprite.Add(50, value12);
			m_SkillIconSprite.Add(51, value12);
			m_SkillIconSprite.Add(60, value13);
		}

		public override void OnShow(params object[] paralist)
		{
			base.OnShow(paralist);
			Singleton<NetManager>.Instance.SendMsg(MsgType.ENTER_BATTLEHOME, null, (object obj) =>
			{
				MonoSingleton<GameStart>.Instance.HideWaitReauestWnd();
				ByteString byteString = obj as ByteString;
				if (!(byteString == null))
				{
					EnterBattleHomeResp_1005038 enterBattleHomeResp_ = new EnterBattleHomeResp_1005038();
					enterBattleHomeResp_.MergeFrom(byteString);
					if (enterBattleHomeResp_.IsSuccess)
					{
						m_JiNengList = new List<PlayerSkillModel>();
						foreach (EnterBattleHomeResp_1005038.Types.SkillResp_1005039 item2 in enterBattleHomeResp_.Skill)
						{
							PlayerSkillModel item = new PlayerSkillModel
							{
								id = item2.SkillId,
								name = item2.Name,
								shuLianDu = item2.Proficiency,
								shuLianDuMax = item2.MaxProficiency,
								level = item2.Level,
								fiveElement = item2.FiveElement,
								shuLianDuLevel = item2.ProficiencyLevel,
								type = item2.Type,
								rank = item2.Rank
							};
							m_JiNengList.Add(item);
						}
						QieHuan();
					}
					else
					{
						string hintMsgByCode = MonoSingleton<GameStart>.Instance.GetHintMsgByCode(enterBattleHomeResp_.Code);
						MonoSingleton<GameStart>.Instance.PopUpAttention(hintMsgByCode);
					}
				}
			}, isShowWaitUI: true);
		}

		public void PanduanQieHuan(bool panduan)
		{
			m_Panel.QieHuan.gameObject.SetActive(panduan);
		}

		private void QieHuan()
		{
			m_XuLiHao++;
			switch (m_XuLiHao)
			{
			case 1:
				foreach (Transform item in m_Panel.ItemListContentYi)
				{
					Object.Destroy(item.gameObject);
				}
				m_Panel.ItemlistYi.gameObject.SetActive(value: true);
				m_Panel.ItemlistSan.gameObject.SetActive(value: false);
				XianShiJiNeng(0);
				break;
			case 2:
				m_Panel.ItemlistYi.gameObject.SetActive(value: false);
				foreach (Transform item2 in m_Panel.ItemListContentEr)
				{
					Object.Destroy(item2.gameObject);
				}
				m_Panel.ItemlistEr.gameObject.SetActive(value: true);
				XianShiJiNeng(3);
				break;
			case 3:
				m_Panel.ItemlistEr.gameObject.SetActive(value: false);
				foreach (Transform item3 in m_Panel.ItemListContentSan)
				{
					Object.Destroy(item3.gameObject);
				}
				m_Panel.ItemlistSan.gameObject.SetActive(value: true);
				XianShiJiNeng(6);
				m_XuLiHao = 0;
				break;
			}
		}

		private void XianShiJiNeng(int jiNengChuShiMin)
		{
			if (m_JiNengList.Count < jiNengChuShiMin)
			{
				return;
			}
			if (m_JiNengList.Count >= jiNengChuShiMin + 3)
			{
				for (int i = jiNengChuShiMin; i < jiNengChuShiMin + 3; i++)
				{
					XuLiaoJiNeng(i);
				}
			}
			else
			{
				for (int j = 0; j < m_JiNengList.Count; j++)
				{
					XuLiaoJiNeng(j);
				}
			}
		}

		private void XuLiaoJiNeng(int xuHao)
		{
			GameObject gameObject = null;
			if (xuHao < 3)
			{
				gameObject = SetListContentParent(m_Panel.ItemListContentYi, m_InstanceItemObj);
			}
			else if (xuHao < 6)
			{
				gameObject = SetListContentParent(m_Panel.ItemListContentEr, m_InstanceItemObj);
			}
			else if (xuHao < 9)
			{
				gameObject = SetListContentParent(m_Panel.ItemListContentSan, m_InstanceItemObj);
			}
			if (!(gameObject == null))
			{
				m_SkillIconSprite.TryGetValue(m_JiNengList[xuHao].type, out var value);
				m_RankBgSprite.TryGetValue(WorldSetting.GetLevelByRank(m_JiNengList[xuHao].rank), out var value2);
				gameObject.GetComponent<ZhanFaShiItem>().SetValue(m_JiNengList[xuHao], value, value2);
			}
		}

		private void TuiChuBut()
		{
			Singleton<UIManager>.Instance.CloseWnd(this);
		}

		private void XianShi()
		{
			string text = "术法大师对手诀的理解远超普通修士，他们可以通过独特的修炼，使自身法术更为强大。每消耗一点精力，可以获得五点熟练度，法术分为生疏、小成、大成、小圆满、大圆满五个阶段。法术小成后，减少施法时间0.25息，大成后可减少施法时间0.5息，小圆满减少施法时间0.75息，大圆满则减少施法时间1息。强大的术师在战斗中使用法术，可以获得非常强大战斗力。";
			Singleton<UIManager>.Instance.PopUpWnd("XiaoWenHaoWnd.prefab", true, text, "战法室");
		}
	}
}
