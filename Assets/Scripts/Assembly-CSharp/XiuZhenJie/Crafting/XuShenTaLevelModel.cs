using System.Collections.Generic;

namespace XiuZhenJie
{
	public class XuShenTaLevelModel : BaseModel
	{
		public List<XuShenTaBangDanRankItemModel> list;

		public int number;

		public double shinyJade;

		public long drawDate;

		public XuShenTaLevelModel()
		{
			list = new List<XuShenTaBangDanRankItemModel>();
		}

		public XuShenTaLevelModel(EnterRushTowerTierNumRankedResp_1008002 data)
		{
			list = new List<XuShenTaBangDanRankItemModel>();
			foreach (EnterRushTowerTierNumRankedResp_1008002.Types.RushTowerRankedObjectResp_1008003 item2 in data.Object)
			{
				XuShenTaBangDanRankItemModel item = new XuShenTaBangDanRankItemModel
				{
					factionName = item2.Faction,
					roleName = item2.Name,
					floorNumber = item2.TierNum,
					roleLevel = item2.Level
				};
				list.Add(item);
			}
			number = data.MyRanked;
		}

		public XuShenTaLevelModel(EnterRushTowerAgeRankedResp_1008004 data)
		{
			list = new List<XuShenTaBangDanRankItemModel>();
			foreach (EnterRushTowerAgeRankedResp_1008004.Types.RushTowerRankedObjectResp_1008005 item2 in data.Object)
			{
				XuShenTaBangDanRankItemModel item = new XuShenTaBangDanRankItemModel
				{
					factionName = item2.Faction,
					roleName = item2.Name,
					floorNumber = item2.TierNum,
					roleAge = item2.Age
				};
				list.Add(item);
			}
			number = data.MyRanked;
		}
	}
}
