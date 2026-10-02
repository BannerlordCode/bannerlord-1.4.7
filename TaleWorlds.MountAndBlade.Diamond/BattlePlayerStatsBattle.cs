using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F4 RID: 244
	[Serializable]
	public class BattlePlayerStatsBattle : BattlePlayerStatsBase
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x00005715 File Offset: 0x00003915
		public BattlePlayerStatsBattle()
		{
			base.GameType = "Battle";
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00005728 File Offset: 0x00003928
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x00005730 File Offset: 0x00003930
		public int RoundsWon { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x00005739 File Offset: 0x00003939
		// (set) Token: 0x060004D3 RID: 1235 RVA: 0x00005741 File Offset: 0x00003941
		public int RoundsLost { get; set; }
	}
}
