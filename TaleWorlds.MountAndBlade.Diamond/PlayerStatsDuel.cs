using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014B RID: 331
	[Serializable]
	public class PlayerStatsDuel : PlayerStatsBase
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000926 RID: 2342 RVA: 0x0000D70A File Offset: 0x0000B90A
		// (set) Token: 0x06000927 RID: 2343 RVA: 0x0000D712 File Offset: 0x0000B912
		public int DuelsWon { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0000D71B File Offset: 0x0000B91B
		// (set) Token: 0x06000929 RID: 2345 RVA: 0x0000D723 File Offset: 0x0000B923
		public int InfantryWins { get; set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0000D72C File Offset: 0x0000B92C
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0000D734 File Offset: 0x0000B934
		public int ArcherWins { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0000D73D File Offset: 0x0000B93D
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0000D745 File Offset: 0x0000B945
		public int CavalryWins { get; set; }

		// Token: 0x0600092E RID: 2350 RVA: 0x0000D74E File Offset: 0x0000B94E
		public PlayerStatsDuel()
		{
			base.GameType = "Duel";
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000D761 File Offset: 0x0000B961
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int duelsWon, int infantryWins, int archerWins, int cavalryWins)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.DuelsWon = duelsWon;
			this.InfantryWins = infantryWins;
			this.ArcherWins = archerWins;
			this.CavalryWins = cavalryWins;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0000D794 File Offset: 0x0000B994
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		public void Update(BattlePlayerStatsDuel stats, bool won)
		{
			base.Update(stats, won);
			this.DuelsWon += stats.DuelsWon;
			this.InfantryWins += stats.InfantryWins;
			this.ArcherWins += stats.ArcherWins;
			this.CavalryWins += stats.CavalryWins;
		}
	}
}
