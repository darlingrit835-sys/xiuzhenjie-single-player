using System.Collections.Generic;
using Google.Protobuf;

namespace XiuZhenJie
{
	public class RoleInfoEntity
	{
		public string Id;

		public string MapId;

		public string MapName;

		public int Sex;

		public int HeadPortraitId;

		public string RoleName;

		public int RoleLevel;

		public int CurrentAge;

		public int MaxAge;

		public long ShinyStone;

		public long ShinyJade;

		public int PureBlood;

		public string FlexibleRoot;

		public long SuperAir;

		public long Perception;

		public long Physique;

		public long CurrentSuperAir;

		public long CurrentPerception;

		public long CurrentPhysique;

		public int CurrentVigor;

		public int Vigor;

		public int CurrentPower;

		public int Power;

		public int CurrentClever;

		public int Clever;

		public int QiYun;

		public int ExtendQiYun;

		public int CurrentPoisonVal;

		public int CurrentPureBlood;

		public int HunLiVal;

		public int LiveRelId;

		public int LiveRe2Id;

		public int LiveRe3Id;

		public int Live1Type;

		public int Live2Type;

		public int Live3Type;

		public int Live4Type;

		public int Live1Level;

		public int Live2Level;

		public int Live3Level;

		public int Live4Level;

		public int Live1Value;

		public int Live2Value;

		public int Live3Value;

		public int Live4Value;

		public int YueKaDays;

		public int UsingTitleId;

		public bool JinLingGen;

		public bool MuLingGen;

		public bool ShuiLingGen;

		public bool HuoLingGen;

		public bool TuLingGen;

		public int PracticeGuideVal;

		public int BattleGuideVal;

		public int AcupointGuideVal;

		public int ShopGuideVal;

		public int SignGuideVal;

		public int ElixirGuideVal;

		public bool IsHaveNewLetter;

		public long RunningTime;

		public bool IsGetSignIn;

		public int SignInDay;

		public string ZongMenId;

		public string ZongMenName;

		public string ZongMenSimpleName;

		public int ZongMenIconIndex;

		public int ZongMenLastNameImgIndex;

		public int TaskId;

		public int TaskStatus;

		public int TianShuBeiStatus;

		public bool TianDaoLing;

		public int GongXianDian;

		public int VipLevel;

		public int TopUpMoney;

		public int ChengHaoFrameId;

		public int HeadFrameId;

		public List<int> OwnerTitleList;

		private Dictionary<int, ChengHaoInfoEntity> m_TypeChengHaoDic;

		private Dictionary<int, ChengHaoInfoEntity> m_ChengHaoInfoDic;

		public Dictionary<int, ChengHaoInfoEntity> TypeChengHaoDic
		{
			get
			{
				return m_TypeChengHaoDic;
			}
			private set
			{
				m_TypeChengHaoDic = value;
			}
		}

		public RoleInfoEntity()
		{
			RunningTime = 0L;
			OwnerTitleList = new List<int>();
		}

		public RoleInfoEntity(RoleLoginResp_1002002 other)
		{
			OwnerTitleList = new List<int>();
			HunLiVal = other.HunLiVal;
			Id = other.Id;
			MapId = other.MapId;
			MapName = other.MapName;
			Sex = other.Sex;
			HeadPortraitId = other.HeadPortraitId;
			RoleName = other.Name;
			RoleLevel = other.Level;
			CurrentAge = other.CurrentAge;
			MaxAge = other.MaxAge;
			ShinyStone = other.ShinyStone;
			ShinyJade = other.ShinyJade;
			PureBlood = other.PureBlood;
			FlexibleRoot = other.FlexibleRoot;
			SuperAir = other.SuperAir;
			Perception = other.Perception;
			Physique = other.Physique;
			CurrentSuperAir = other.CurrentSuperAir;
			CurrentPerception = other.CurrentPerception;
			CurrentPhysique = other.CurrentPhysique;
			CurrentVigor = other.CurrentVigor;
			CurrentClever = other.CurrentClever;
			CurrentPoisonVal = other.CurrentPoisonVal;
			CurrentPureBlood = other.CurrentPureBlood;
			LiveRelId = other.LiveRelId;
			Live1Type = other.Live1Type;
			Live2Type = other.Live2Type;
			Live1Level = other.Live1Level;
			Live2Level = other.Live2Level;
			Live1Value = other.Live1Value;
			Live2Value = other.Live2Value;
			YueKaDays = other.YueKaDays;
			UsingTitleId = other.Title;
			BattleGuideVal = other.BattleGuideVal;
			PracticeGuideVal = other.PracticeGuideVal;
			IsHaveNewLetter = other.IsNewLetter;
			HuoLingGen = other.HasFire;
			ShuiLingGen = other.HasWater;
			TuLingGen = other.HasEarth;
			MuLingGen = other.HasWood;
			JinLingGen = other.HasGold;
			RunningTime = 0L;
			AcupointGuideVal = other.AcupointGuideVal;
			ShopGuideVal = other.ShopGuideVal;
			SignGuideVal = other.SignGuideVal;
			ElixirGuideVal = other.ElixirGuideVal;
			IsGetSignIn = other.IsGetSignIn;
			SignInDay = other.SignInDay;
			ZongMenId = other.GuildId.ToString();
			ZongMenName = other.GuildName;
			ZongMenSimpleName = other.GuildSimpleName;
			ZongMenIconIndex = other.TokenIndex;
			ZongMenLastNameImgIndex = other.LastNameIndex;
			TaskId = other.TaskId;
			TaskStatus = other.TaskStatus;
			CurrentPower = other.CurrentPower;
			Power = other.Power;
			Vigor = other.Vigor;
			Clever = other.Clever;
			LiveRe3Id = 8;
			Live3Type = other.Live3Type;
			Live3Level = other.Live3Level;
			Live3Value = other.Live3Value;
			Live4Type = other.Live4Type;
			Live4Level = other.Live4Level;
			Live4Value = other.Live4Value;
			TianShuBeiStatus = other.TianShuBeiStatus;
			TianDaoLing = other.IsTianDaoLing;
			QiYun = other.QiYun;
			ExtendQiYun = (other.IsHaoYunFu ? 2000 : 0);
			VipLevel = other.VipLevel;
			TopUpMoney = other.LeiChong;
			HeadFrameId = other.HeadFrame;
			ChengHaoFrameId = other.TitleFrame;
			if (other.TitleId != null)
			{
				foreach (int item in other.TitleId)
				{
					OwnerTitleList.Add(item);
				}
			}
			InitTitleInfo();
		}

		private void InitTitleInfo()
		{
			m_ChengHaoInfoDic = Singleton<FacadeManager>.Instance.SelectChengHaoInfo();
			if (m_TypeChengHaoDic == null)
			{
				m_TypeChengHaoDic = new Dictionary<int, ChengHaoInfoEntity>();
			}
			else
			{
				m_TypeChengHaoDic.Clear();
			}
			if (OwnerTitleList == null)
			{
				return;
			}
			foreach (int ownerTitle in OwnerTitleList)
			{
				ChengHaoInfoEntity value = null;
				m_ChengHaoInfoDic.TryGetValue(ownerTitle, out value);
				if (value == null)
				{
					continue;
				}
				if (m_TypeChengHaoDic.ContainsKey(value.Type))
				{
					if (m_TypeChengHaoDic[value.Type].EffectValue < value.EffectValue)
					{
						m_TypeChengHaoDic[value.Type] = value;
					}
				}
				else
				{
					m_TypeChengHaoDic.Add(value.Type, value);
				}
			}
		}

		public void AddNewRoleTitle(ByteString byteStr)
		{
			if (byteStr == null)
			{
				return;
			}
			InformTitleResp_1002006 informTitleResp_ = new InformTitleResp_1002006();
			informTitleResp_.MergeFrom(byteStr);
			if (informTitleResp_.IsSuccess)
			{
				OwnerTitleList.Add(informTitleResp_.Title);
				if (m_ChengHaoInfoDic.ContainsKey(informTitleResp_.Title))
				{
					MonoSingleton<GameStart>.Instance.PopRollingTextShow($"获取新的称号：{m_ChengHaoInfoDic[informTitleResp_.Title].Name}");
					AddQiYun(informTitleResp_.Title);
				}
			}
			InitTitleInfo();
			Singleton<UIManager>.Instance.ReFlashAllOpenedWnd();
		}

		private void AddQiYun(int id)
		{
			int num = 0;
			switch (id)
			{
			case 23:
				num = 200;
				break;
			case 24:
				num = 500;
				break;
			case 25:
				num = 1000;
				break;
			case 902:
				num = 500;
				break;
			case 903:
				num = 300;
				break;
			case 904:
				num = 200;
				break;
			case 65:
				num = 100;
				break;
			case 704:
				num = 500;
				break;
			case 705:
				num = 1000;
				break;
			}
			if (num != 0)
			{
				MonoSingleton<GameStart>.Instance.PopRollingTextShow("气运增长" + num);
			}
			QiYun += num;
		}
	}
}
