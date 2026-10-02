using System;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Hideout.Objectives
{
	// Token: 0x02000096 RID: 150
	internal class DefeatHideoutBossObjective : MissionObjective
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x0002AE4C File Offset: 0x0002904C
		public override string UniqueId
		{
			get
			{
				return "hideout_mission_defeat_hideout_boss_objective";
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0002AE53 File Offset: 0x00029053
		public override TextObject Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x0002AE5B File Offset: 0x0002905B
		public override TextObject Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0002AE64 File Offset: 0x00029064
		public DefeatHideoutBossObjective(Mission mission, bool isDuel)
			: base(mission)
		{
			this._name = (isDuel ? new TextObject("{=QEynMlwL}Win the Duel", null) : new TextObject("{=0sPTRh6L}Win the Fight", null));
			this._description = (isDuel ? new TextObject("{=t13oVKkw}Win the duel against the bandit boss.", null) : new TextObject("{=7vqW1CsE}Eliminate the bandit boss and his troops.", null));
		}

		// Token: 0x04000368 RID: 872
		private readonly TextObject _name;

		// Token: 0x04000369 RID: 873
		private readonly TextObject _description;
	}
}
