using System;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000147 RID: 327
	[JsonConverter(typeof(PlayerStatsBaseJsonConverter))]
	[Serializable]
	public class PlayerStatsBase
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0000D308 File Offset: 0x0000B508
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x0000D310 File Offset: 0x0000B510
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0000D319 File Offset: 0x0000B519
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x0000D321 File Offset: 0x0000B521
		[JsonProperty]
		public int KillCount { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0000D32A File Offset: 0x0000B52A
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x0000D332 File Offset: 0x0000B532
		[JsonProperty]
		public int DeathCount { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0000D33B File Offset: 0x0000B53B
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x0000D343 File Offset: 0x0000B543
		[JsonProperty]
		public int AssistCount { get; set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000902 RID: 2306 RVA: 0x0000D34C File Offset: 0x0000B54C
		// (set) Token: 0x06000903 RID: 2307 RVA: 0x0000D354 File Offset: 0x0000B554
		[JsonProperty]
		public int WinCount { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0000D35D File Offset: 0x0000B55D
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x0000D365 File Offset: 0x0000B565
		[JsonProperty]
		public int LoseCount { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0000D36E File Offset: 0x0000B56E
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x0000D376 File Offset: 0x0000B576
		[JsonProperty]
		public int ForfeitCount { get; set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0000D37F File Offset: 0x0000B57F
		[JsonIgnore]
		public float AverageKillPerDeath
		{
			get
			{
				return (float)this.KillCount / (float)((this.DeathCount != 0) ? this.DeathCount : 1);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x0000D39B File Offset: 0x0000B59B
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x0000D3A3 File Offset: 0x0000B5A3
		[JsonProperty]
		public string GameType { get; set; }

		// Token: 0x0600090C RID: 2316 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		public void FillWith(PlayerId playerId, int killCount, int deathCount, int assistCount, int winCount, int loseCount, int forfeitCount)
		{
			this.PlayerId = playerId;
			this.KillCount = killCount;
			this.DeathCount = deathCount;
			this.AssistCount = assistCount;
			this.WinCount = winCount;
			this.LoseCount = loseCount;
			this.ForfeitCount = forfeitCount;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0000D3EC File Offset: 0x0000B5EC
		public virtual void Update(BattlePlayerStatsBase battleStats, bool won)
		{
			this.KillCount += battleStats.Kills;
			this.DeathCount += battleStats.Deaths;
			this.AssistCount += battleStats.Assists;
			int num;
			if (won)
			{
				num = this.WinCount;
				this.WinCount = num + 1;
				return;
			}
			num = this.LoseCount;
			this.LoseCount = num + 1;
		}
	}
}
