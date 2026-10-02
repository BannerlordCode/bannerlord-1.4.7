using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014F RID: 335
	[Serializable]
	public class PlayerStatsTeamDeathmatch : PlayerStatsBase
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0000DB2D File Offset: 0x0000BD2D
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0000DB35 File Offset: 0x0000BD35
		public int Score { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0000DB3E File Offset: 0x0000BD3E
		public float AverageScore
		{
			get
			{
				return (float)this.Score / (float)((base.WinCount + base.LoseCount != 0) ? (base.WinCount + base.LoseCount) : 1);
			}
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0000DB68 File Offset: 0x0000BD68
		public PlayerStatsTeamDeathmatch()
		{
			base.GameType = "TeamDeathmatch";
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0000DB7B File Offset: 0x0000BD7B
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int score)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.Score = score;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0000DB98 File Offset: 0x0000BD98
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000DBB3 File Offset: 0x0000BDB3
		public void Update(BattlePlayerStatsTeamDeathmatch stats, bool won)
		{
			base.Update(stats, won);
			this.Score += stats.Score;
		}
	}
}
