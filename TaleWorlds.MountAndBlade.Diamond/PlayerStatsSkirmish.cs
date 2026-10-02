using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014E RID: 334
	[Serializable]
	public class PlayerStatsSkirmish : PlayerStatsRanked
	{
		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x0000DA40 File Offset: 0x0000BC40
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0000DA48 File Offset: 0x0000BC48
		public int MVPs { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0000DA51 File Offset: 0x0000BC51
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0000DA59 File Offset: 0x0000BC59
		public int Score { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0000DA62 File Offset: 0x0000BC62
		[JsonIgnore]
		public int AverageScore
		{
			get
			{
				return this.Score / ((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0000DA8A File Offset: 0x0000BC8A
		public PlayerStatsSkirmish()
		{
			base.GameType = "Skirmish";
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int rating, int ratingDeviation, string rank, bool evaluating, int evaluationMatchesPlayedCount, int mvps, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount, rating, ratingDeviation, rank, evaluating, evaluationMatchesPlayedCount);
			this.MVPs = mvps;
			this.Score = score;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0000DAD8 File Offset: 0x0000BCD8
		public void FillWithNewPlayer(PlayerId playerId, int defaultRating, int defaultRatingDeviation)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, defaultRating, defaultRatingDeviation, "", true, 0, 0, 0);
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0000DAFD File Offset: 0x0000BCFD
		public void Update(BattlePlayerStatsSkirmish stats, bool won)
		{
			base.Update(stats, won);
			this.MVPs += stats.MVPs;
			this.Score += stats.Score;
		}
	}
}
