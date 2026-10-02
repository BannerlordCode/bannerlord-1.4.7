using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F6 RID: 246
	[Serializable]
	public class BattlePlayerStatsDuel : BattlePlayerStatsBase
	{
		// Token: 0x060004DB RID: 1243 RVA: 0x00005790 File Offset: 0x00003990
		public BattlePlayerStatsDuel()
		{
			base.GameType = "Duel";
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x000057A3 File Offset: 0x000039A3
		// (set) Token: 0x060004DD RID: 1245 RVA: 0x000057AB File Offset: 0x000039AB
		public int DuelsWon { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x000057B4 File Offset: 0x000039B4
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x000057BC File Offset: 0x000039BC
		public int InfantryWins { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x000057C5 File Offset: 0x000039C5
		// (set) Token: 0x060004E1 RID: 1249 RVA: 0x000057CD File Offset: 0x000039CD
		public int ArcherWins { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x000057D6 File Offset: 0x000039D6
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x000057DE File Offset: 0x000039DE
		public int CavalryWins { get; set; }
	}
}
