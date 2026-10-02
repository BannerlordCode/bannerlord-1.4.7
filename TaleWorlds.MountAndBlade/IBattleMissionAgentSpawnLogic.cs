using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025C RID: 604
	public interface IBattleMissionAgentSpawnLogic : IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002247 RID: 8775
		int TotalSpawnNumber { get; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002248 RID: 8776
		int BattleSize { get; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002249 RID: 8777
		int NumberOfAgents { get; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600224A RID: 8778
		MissionSpawnPhase DefenderActivePhase { get; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x0600224B RID: 8779
		MissionSpawnPhase AttackerActivePhase { get; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x0600224C RID: 8780
		readonly ref MissionSpawnSettings SpawnSettings { get; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x0600224D RID: 8781
		IMissionDeploymentPlan DeploymentPlan { get; }
	}
}
