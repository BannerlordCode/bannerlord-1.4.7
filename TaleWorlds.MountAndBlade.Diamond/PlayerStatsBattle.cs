using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000149 RID: 329
	[Serializable]
	public class PlayerStatsBattle : PlayerStatsBase
	{
		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0000D53D File Offset: 0x0000B73D
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x0000D545 File Offset: 0x0000B745
		public int RoundsWon { get; private set; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0000D54E File Offset: 0x0000B74E
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x0000D556 File Offset: 0x0000B756
		public int RoundsLost { get; private set; }

		// Token: 0x06000917 RID: 2327 RVA: 0x0000D55F File Offset: 0x0000B75F
		public PlayerStatsBattle()
		{
			base.GameType = "Battle";
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000D572 File Offset: 0x0000B772
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount, int roundsWon, int roundsLost)
		{
			base.FillWith(playerId, killCount, deathCount, assistCount, winCount, loseCount, forfeitCount);
			this.RoundsWon = roundsWon;
			this.RoundsLost = roundsLost;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0000D598 File Offset: 0x0000B798
		public void FillWithNewPlayer(PlayerId playerId)
		{
			this.FillWith(playerId, 0, 0, 0, 0, 0, 0, 0, 0);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0000D5B4 File Offset: 0x0000B7B4
		public void Update(BattlePlayerStatsBattle stats, bool won)
		{
			base.Update(stats, won);
			this.RoundsWon += stats.RoundsWon;
			this.RoundsLost += stats.RoundsLost;
		}
	}
}
