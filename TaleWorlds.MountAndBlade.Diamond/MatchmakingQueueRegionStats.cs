using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000132 RID: 306
	[Serializable]
	public class MatchmakingQueueRegionStats
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0000BFF5 File Offset: 0x0000A1F5
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x0000BFFD File Offset: 0x0000A1FD
		[JsonProperty]
		public string Region { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0000C008 File Offset: 0x0000A208
		[JsonIgnore]
		public int TotalCount
		{
			get
			{
				int num = 0;
				foreach (MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats in this.GameTypeStats)
				{
					num += matchmakingQueueGameTypeStats.Count;
				}
				return num;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0000C060 File Offset: 0x0000A260
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0000C068 File Offset: 0x0000A268
		[JsonProperty]
		public int MaxWaitTime { get; set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0000C071 File Offset: 0x0000A271
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0000C079 File Offset: 0x0000A279
		[JsonProperty]
		public int MinWaitTime { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0000C082 File Offset: 0x0000A282
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0000C08A File Offset: 0x0000A28A
		[JsonProperty]
		public int MedianWaitTime { get; set; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0000C093 File Offset: 0x0000A293
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0000C09B File Offset: 0x0000A29B
		[JsonProperty]
		public int AverageWaitTime { get; set; }

		// Token: 0x0600084F RID: 2127 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		public MatchmakingQueueRegionStats(string region)
		{
			this.Region = region;
			this.GameTypeStats = new List<MatchmakingQueueGameTypeStats>();
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		public MatchmakingQueueGameTypeStats GetQueueCountObjectOf(string[] gameTypes)
		{
			if (gameTypes != null)
			{
				foreach (MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats in this.GameTypeStats)
				{
					if (matchmakingQueueGameTypeStats.EqualWith(gameTypes))
					{
						return matchmakingQueueGameTypeStats;
					}
				}
			}
			return null;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0000C120 File Offset: 0x0000A320
		public void AddStats(MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats)
		{
			this.GameTypeStats.Add(matchmakingQueueGameTypeStats);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0000C130 File Offset: 0x0000A330
		public int GetQueueCountOf(string[] gameTypes)
		{
			int num = 0;
			if (gameTypes != null)
			{
				foreach (MatchmakingQueueGameTypeStats matchmakingQueueGameTypeStats in this.GameTypeStats)
				{
					if (matchmakingQueueGameTypeStats.HasAnyGameType(gameTypes))
					{
						num += matchmakingQueueGameTypeStats.Count;
					}
				}
			}
			return num;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0000C194 File Offset: 0x0000A394
		public void SetWaitTimeStats(int averageWaitTime, int maxWaitTime, int minWaitTime, int medianWaitTime)
		{
			this.AverageWaitTime = averageWaitTime;
			this.MaxWaitTime = maxWaitTime;
			this.MinWaitTime = minWaitTime;
			this.MedianWaitTime = medianWaitTime;
		}

		// Token: 0x04000356 RID: 854
		[JsonProperty]
		public List<MatchmakingQueueGameTypeStats> GameTypeStats;
	}
}
