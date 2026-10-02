using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014C RID: 332
	[Serializable]
	public class PlayerStatsRanked : PlayerStatsBase
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0000D815 File Offset: 0x0000BA15
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0000D81D File Offset: 0x0000BA1D
		public int Rating { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0000D826 File Offset: 0x0000BA26
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x0000D82E File Offset: 0x0000BA2E
		public string Rank { get; set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0000D837 File Offset: 0x0000BA37
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0000D83F File Offset: 0x0000BA3F
		public bool Evaluating { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0000D848 File Offset: 0x0000BA48
		// (set) Token: 0x06000939 RID: 2361 RVA: 0x0000D850 File Offset: 0x0000BA50
		public int EvaluationMatchesPlayedCount { get; set; }

		// Token: 0x0600093A RID: 2362 RVA: 0x0000D859 File Offset: 0x0000BA59
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int rating, int ratingDeviation, string rank, bool evaluating, int evaluationMatchesPlayedCount)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.Rating = rating;
			this.Rank = rank;
			this.Evaluating = evaluating;
			this.EvaluationMatchesPlayedCount = evaluationMatchesPlayedCount;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0000D88C File Offset: 0x0000BA8C
		public virtual void FillWithNewPlayer(PlayerId playerId, string gameType, int defaultRating, int defaultRatingDeviation)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, defaultRating, defaultRatingDeviation, "", true, 0);
		}
	}
}
