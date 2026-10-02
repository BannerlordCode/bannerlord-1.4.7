using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F8 RID: 248
	[Serializable]
	public class BattlePlayerStatsSkirmish : BattlePlayerStatsBase
	{
		// Token: 0x060004EF RID: 1263 RVA: 0x0000584F File Offset: 0x00003A4F
		public BattlePlayerStatsSkirmish()
		{
			base.GameType = "Skirmish";
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00005862 File Offset: 0x00003A62
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x0000586A File Offset: 0x00003A6A
		public int MVPs { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00005873 File Offset: 0x00003A73
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x0000587B File Offset: 0x00003A7B
		public int Score { get; set; }
	}
}
