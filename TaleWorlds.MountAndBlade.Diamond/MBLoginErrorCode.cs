using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000137 RID: 311
	public enum MBLoginErrorCode
	{
		// Token: 0x04000367 RID: 871
		None,
		// Token: 0x04000368 RID: 872
		CouldNotLogin,
		// Token: 0x04000369 RID: 873
		VersionMismatch,
		// Token: 0x0400036A RID: 874
		IncorrectPassword,
		// Token: 0x0400036B RID: 875
		FamilyShareNotAllowed,
		// Token: 0x0400036C RID: 876
		BannedFromGame,
		// Token: 0x0400036D RID: 877
		NoAuthenticationToken,
		// Token: 0x0400036E RID: 878
		AuthTokenExpired,
		// Token: 0x0400036F RID: 879
		BannedFromHostingServers,
		// Token: 0x04000370 RID: 880
		CustomBattleServerIncompatibleVersion,
		// Token: 0x04000371 RID: 881
		ReachedMaxNumberofCustomBattleServers,
		// Token: 0x04000372 RID: 882
		CouldNotDestroyOldSession,
		// Token: 0x04000373 RID: 883
		LoggingInDisabled
	}
}
