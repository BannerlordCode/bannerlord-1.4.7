using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace SandBox.Missions.MissionLogics.Hideout.Objectives
{
	// Token: 0x02000095 RID: 149
	public class ClearTheMainCampObjective : MissionObjective
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0002ADD1 File Offset: 0x00028FD1
		public override string UniqueId
		{
			get
			{
				return "hideout_mission_clear_the_main_camp_objective";
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0002ADD8 File Offset: 0x00028FD8
		public override TextObject Name
		{
			get
			{
				return new TextObject("{=OLWkIYxa}Clear the Main Camp", null);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0002ADE5 File Offset: 0x00028FE5
		public override TextObject Description
		{
			get
			{
				return new TextObject("{=lGZLiIey}Clear the main camp with your troops.", null);
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0002ADF2 File Offset: 0x00028FF2
		public ClearTheMainCampObjective(Mission mission, List<Agent> agents)
			: base(mission)
		{
			this._agents = agents;
			this._requiredProgressAmount = agents.Count;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0002AE10 File Offset: 0x00029010
		public override MissionObjectiveProgressInfo GetCurrentProgress()
		{
			return new MissionObjectiveProgressInfo
			{
				CurrentProgressAmount = this._requiredProgressAmount - this._agents.Count,
				RequiredProgressAmount = this._requiredProgressAmount
			};
		}

		// Token: 0x04000366 RID: 870
		private readonly List<Agent> _agents;

		// Token: 0x04000367 RID: 871
		private readonly int _requiredProgressAmount;
	}
}
