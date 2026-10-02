using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014D RID: 333
	[Serializable]
	public class PlayerStatsSiege : PlayerStatsBase
	{
		// Token: 0x170002EC RID: 748
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x0000D8B8 File Offset: 0x0000BAB8
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x0000D8C0 File Offset: 0x0000BAC0
		public int WallsBreached { get; set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0000D8C9 File Offset: 0x0000BAC9
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0000D8D1 File Offset: 0x0000BAD1
		public int SiegeEngineKills { get; set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0000D8DA File Offset: 0x0000BADA
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x0000D8E2 File Offset: 0x0000BAE2
		public int SiegeEnginesDestroyed { get; set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0000D8EB File Offset: 0x0000BAEB
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x0000D8F3 File Offset: 0x0000BAF3
		public int ObjectiveGoldGained { get; set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0000D8FC File Offset: 0x0000BAFC
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x0000D904 File Offset: 0x0000BB04
		public int Score { get; set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0000D90D File Offset: 0x0000BB0D
		public int AverageScore
		{
			get
			{
				return this.Score / ((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0000D935 File Offset: 0x0000BB35
		public int AverageKillCount
		{
			get
			{
				return base.KillCount / ((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0000D95D File Offset: 0x0000BB5D
		public PlayerStatsSiege()
		{
			base.GameType = "Siege";
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0000D970 File Offset: 0x0000BB70
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int wallsBreached, int siegeEngineKills, int siegeEnginesDestroyed, int objectiveGoldGained, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.WallsBreached = wallsBreached;
			this.SiegeEngineKills = siegeEngineKills;
			this.SiegeEnginesDestroyed = siegeEnginesDestroyed;
			this.ObjectiveGoldGained = objectiveGoldGained;
			this.Score = score;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0000D9AC File Offset: 0x0000BBAC
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		public void Update(BattlePlayerStatsSiege stats, bool won)
		{
			base.Update(stats, won);
			this.WallsBreached += stats.WallsBreached;
			this.SiegeEngineKills += stats.SiegeEngineKills;
			this.SiegeEnginesDestroyed += stats.SiegeEnginesDestroyed;
			this.ObjectiveGoldGained += stats.ObjectiveGoldGained;
			this.Score += stats.Score;
		}
	}
}
