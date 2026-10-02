using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000143 RID: 323
	public interface IFormationUnit
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001035 RID: 4149
		IFormationArrangement Formation { get; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001036 RID: 4150
		// (set) Token: 0x06001037 RID: 4151
		int FormationFileIndex { get; set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001038 RID: 4152
		// (set) Token: 0x06001039 RID: 4153
		int FormationRankIndex { get; set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600103A RID: 4154
		IFormationUnit FollowedUnit { get; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600103B RID: 4155
		bool IsShieldUsageEncouraged { get; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600103C RID: 4156
		bool IsPlayerUnit { get; }
	}
}
