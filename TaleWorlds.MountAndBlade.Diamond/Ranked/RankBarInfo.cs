using System;

namespace TaleWorlds.MountAndBlade.Diamond.Ranked
{
	// Token: 0x0200015F RID: 351
	[Serializable]
	public class RankBarInfo
	{
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0000F00E File Offset: 0x0000D20E
		// (set) Token: 0x060009BB RID: 2491 RVA: 0x0000F016 File Offset: 0x0000D216
		public string RankId { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0000F01F File Offset: 0x0000D21F
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0000F027 File Offset: 0x0000D227
		public string PreviousRankId { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0000F030 File Offset: 0x0000D230
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0000F038 File Offset: 0x0000D238
		public string NextRankId { get; set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0000F041 File Offset: 0x0000D241
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0000F049 File Offset: 0x0000D249
		public float ProgressPercentage { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0000F052 File Offset: 0x0000D252
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0000F05A File Offset: 0x0000D25A
		public int Rating { get; set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0000F063 File Offset: 0x0000D263
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0000F06B File Offset: 0x0000D26B
		public int RatingToNextRank { get; set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0000F074 File Offset: 0x0000D274
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0000F07C File Offset: 0x0000D27C
		public bool IsEvaluating { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0000F085 File Offset: 0x0000D285
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0000F08D File Offset: 0x0000D28D
		public int EvaluationMatchesPlayed { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0000F096 File Offset: 0x0000D296
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0000F09E File Offset: 0x0000D29E
		public int TotalEvaluationMatchesRequired { get; set; }

		// Token: 0x060009CC RID: 2508 RVA: 0x0000F0A7 File Offset: 0x0000D2A7
		public RankBarInfo()
		{
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0000F0B0 File Offset: 0x0000D2B0
		public RankBarInfo(string rankId, string previousRankId, string nextRankId, float progressPercentage, int rating, int ratingToNextRank, bool isEvaluating, int evaluationMatchesPlayed, int totalEvaluationMatchesRequired)
		{
			this.RankId = rankId;
			this.PreviousRankId = previousRankId;
			this.NextRankId = nextRankId;
			this.ProgressPercentage = progressPercentage;
			this.Rating = rating;
			this.RatingToNextRank = ratingToNextRank;
			this.IsEvaluating = isEvaluating;
			this.EvaluationMatchesPlayed = evaluationMatchesPlayed;
			this.TotalEvaluationMatchesRequired = totalEvaluationMatchesRequired;
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0000F108 File Offset: 0x0000D308
		public static RankBarInfo CreateBarInfo(string rankId, string previousRankId, string nextRankId, float progressPercentage, int rating, int ratingToNextRank)
		{
			return new RankBarInfo(rankId, previousRankId, nextRankId, progressPercentage, rating, ratingToNextRank, false, 0, 0);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0000F128 File Offset: 0x0000D328
		public static RankBarInfo CreateUnrankedInfo(int matchesPlayed, int totalMatchesRequired)
		{
			return new RankBarInfo("", "", "", 0f, 0, 0, true, matchesPlayed, totalMatchesRequired);
		}
	}
}
