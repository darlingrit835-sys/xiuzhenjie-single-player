using System;

namespace XiuZhenJie
{
    // The game server returns one guild snapshot after a read or a successful order.
    [Serializable] internal sealed class SectCourtState
    {
        public string guildName, tierName, regionId, regionKind, regionName, regionPath;
        public string plan, foe, waveKind, mineName;
        public int stage, tier, office, workers, miners, guards, formation, members;
        public long guildId, revision, deadline, serverTime, slowUntil, damage, target, ore, crystal, fang, merit;
        public long formationOre, formationCrystal, promoteOre, promoteFang;
        public int buildFormation, nextStage, nextWorkers, formationCap, nextFormationCap;
        public long buildOre, buildCrystal, stipend, nextStipend, walletStones;
        public float nextOreRate, nextCrystalRate;
        public string nextRegionName, nextRegionKind, stipendDay;
        public bool stipendClaimed;
        public long winOre, winFang, winMerit, lossOre, lossMerit;
        public float oreRate, crystalRate;
        public bool canAssign, canForge, canPromote, joined, joinedElsewhere, @protected;
        public Role[] roles;
        public Echo[] echoes;
        public Report[] reports;
        public Treasure[] treasures;

        [Serializable] internal sealed class Role { public string name; public int office; }
        [Serializable] internal sealed class Echo { public string name, tactic; public long damage, at; }
        [Serializable] internal sealed class Report
        {
            public string foe, waveKind;
            public long at, damage, target, ore, fang, merit, participants, slowUntil;
            public bool victory, idle, @protected;
            public Echo[] echoes;
        }
        [Serializable] internal sealed class Treasure
        {
            public string id, name, category, description;
            public int cost, owned;
        }
        [Serializable] internal sealed class Order { public string action, value; public long guildId, revision; }
        [Serializable] internal sealed class Failure { public string error; }
    }
}
