using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F5 RID: 245
	[Serializable]
	public class BattlePlayerStatsCaptain : BattlePlayerStatsBase
	{
		// Token: 0x060004D4 RID: 1236 RVA: 0x0000574A File Offset: 0x0000394A
		public BattlePlayerStatsCaptain()
		{
			base.GameType = "Captain";
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0000575D File Offset: 0x0000395D
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00005765 File Offset: 0x00003965
		public int CaptainsKilled { get; set; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0000576E File Offset: 0x0000396E
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00005776 File Offset: 0x00003976
		public int MVPs { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000577F File Offset: 0x0000397F
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00005787 File Offset: 0x00003987
		public int Score { get; set; }
	}
}
