using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014A RID: 330
	[Serializable]
	public class PlayerStatsCaptain : PlayerStatsRanked
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0000D5E4 File Offset: 0x0000B7E4
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0000D5EC File Offset: 0x0000B7EC
		public int CaptainsKilled { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0000D5F5 File Offset: 0x0000B7F5
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0000D5FD File Offset: 0x0000B7FD
		public int MVPs { get; set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0000D606 File Offset: 0x0000B806
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x0000D60E File Offset: 0x0000B80E
		public int Score { get; set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0000D617 File Offset: 0x0000B817
		[JsonIgnore]
		public int AverageScore
		{
			get
			{
				if (this.Score / (base.WinCount + base.LoseCount) == 0)
				{
					return 1;
				}
				return base.WinCount + base.LoseCount;
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0000D63E File Offset: 0x0000B83E
		public PlayerStatsCaptain()
		{
			base.GameType = "Captain";
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0000D654 File Offset: 0x0000B854
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int rating, int ratingDeviation, string rank, bool evaluating, int evaluationMatchesPlayedCount, int captainsKilled, int mvps, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount, rating, ratingDeviation, rank, evaluating, evaluationMatchesPlayedCount);
			this.CaptainsKilled = captainsKilled;
			this.MVPs = mvps;
			this.Score = score;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0000D694 File Offset: 0x0000B894
		public void FillWithNewPlayer(PlayerId playerId, int defaultRating, int defaultRatingDeviation)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, defaultRating, defaultRatingDeviation, "", true, 0, 0, 0, 0);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0000D6BC File Offset: 0x0000B8BC
		public void Update(BattlePlayerStatsCaptain stats, bool won)
		{
			base.Update(stats, won);
			this.CaptainsKilled += stats.CaptainsKilled;
			this.MVPs += stats.MVPs;
			this.Score += stats.Score;
		}
	}
}
