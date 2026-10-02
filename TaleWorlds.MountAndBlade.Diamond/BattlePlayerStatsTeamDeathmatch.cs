using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F9 RID: 249
	[Serializable]
	public class BattlePlayerStatsTeamDeathmatch : BattlePlayerStatsBase
	{
		// Token: 0x060004F4 RID: 1268 RVA: 0x00005884 File Offset: 0x00003A84
		public BattlePlayerStatsTeamDeathmatch()
		{
			base.GameType = "TeamDeathmatch";
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00005897 File Offset: 0x00003A97
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0000589F File Offset: 0x00003A9F
		public int Score { get; set; }
	}
}
