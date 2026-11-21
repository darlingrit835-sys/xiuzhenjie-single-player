using System;
using System.Collections.Generic;

namespace XiuZhenJie
{
	[Serializable]
	public class RoleBattleInfoModel : BaseModel
	{
		public List<PlayerSkillModel> skillList;

		public List<PlayerFuLuModel> signList;

		public List<PlayerFaQiModel> equipmentList;

		public List<RoleZhuangBeiModel> defenseEquipmentList;

		public PlayerSkillModel speed;
	}
}
