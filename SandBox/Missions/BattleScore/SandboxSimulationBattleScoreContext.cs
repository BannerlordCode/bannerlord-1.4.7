using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions.BattleScore;

namespace SandBox.Missions.BattleScore
{
	// Token: 0x020000A0 RID: 160
	public class SandboxSimulationBattleScoreContext : BattleScoreContext
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x0002CB7F File Offset: 0x0002AD7F
		public SandboxSimulationBattleScoreContext(BattleSimulation battleSimulation)
		{
			this._battleSimulation = battleSimulation;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0002CB8E File Offset: 0x0002AD8E
		public override bool IsPowerComparisonRelevant
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002CB91 File Offset: 0x0002AD91
		public override Banner GetAttackerBanner()
		{
			return this._battleSimulation.MapEvent.AttackerSide.LeaderParty.Banner;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002CBAD File Offset: 0x0002ADAD
		public override Banner GetDefenderBanner()
		{
			return this._battleSimulation.MapEvent.DefenderSide.LeaderParty.Banner;
		}

		// Token: 0x04000392 RID: 914
		private readonly BattleSimulation _battleSimulation;
	}
}
