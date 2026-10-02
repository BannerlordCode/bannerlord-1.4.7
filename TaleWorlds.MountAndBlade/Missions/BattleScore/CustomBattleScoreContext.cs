using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Missions.BattleScore
{
	// Token: 0x020003F0 RID: 1008
	public class CustomBattleScoreContext : BattleScoreContext
	{
		// Token: 0x06003739 RID: 14137 RVA: 0x000E495F File Offset: 0x000E2B5F
		public CustomBattleScoreContext(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x0600373A RID: 14138 RVA: 0x000E496E File Offset: 0x000E2B6E
		public override bool IsPowerComparisonRelevant
		{
			get
			{
				return this._mission.Mode != MissionMode.Deployment;
			}
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000E4981 File Offset: 0x000E2B81
		public override Banner GetAttackerBanner()
		{
			return this.GetSideBannerInfo(BattleSideEnum.Attacker);
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000E498A File Offset: 0x000E2B8A
		public override Banner GetDefenderBanner()
		{
			return this.GetSideBannerInfo(BattleSideEnum.Defender);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000E4994 File Offset: 0x000E2B94
		private Banner GetSideBannerInfo(BattleSideEnum sideEnum)
		{
			MissionCombatantsLogic missionBehavior = this._mission.GetMissionBehavior<MissionCombatantsLogic>();
			if (missionBehavior == null)
			{
				return null;
			}
			return missionBehavior.GetBannerForSide(sideEnum);
		}

		// Token: 0x040017BA RID: 6074
		private readonly Mission _mission;
	}
}
