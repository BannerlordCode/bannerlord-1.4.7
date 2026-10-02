using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000131 RID: 305
	[Serializable]
	public class MatchmakingQueueStats
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x0000BE01 File Offset: 0x0000A001
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x0000BE08 File Offset: 0x0000A008
		public static MatchmakingQueueStats Empty { get; private set; } = new MatchmakingQueueStats();

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0000BE10 File Offset: 0x0000A010
		[JsonIgnore]
		public int TotalCount
		{
			get
			{
				int num = 0;
				foreach (MatchmakingQueueRegionStats matchmakingQueueRegionStats in this.RegionStats)
				{
					num += matchmakingQueueRegionStats.TotalCount;
				}
				return num;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0000BE68 File Offset: 0x0000A068
		[JsonIgnore]
		public int AverageWaitTime
		{
			get
			{
				int num = 0;
				int num2 = 0;
				if (this.RegionStats.Count > 0)
				{
					foreach (MatchmakingQueueRegionStats matchmakingQueueRegionStats in this.RegionStats)
					{
						num2 += matchmakingQueueRegionStats.AverageWaitTime;
					}
					num = num2 / this.RegionStats.Count;
				}
				return num;
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0000BEEC File Offset: 0x0000A0EC
		public MatchmakingQueueStats()
		{
			this.RegionStats = new List<MatchmakingQueueRegionStats>();
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0000BEFF File Offset: 0x0000A0FF
		public void AddRegionStats(MatchmakingQueueRegionStats matchmakingQueueRegionStats)
		{
			this.RegionStats.Add(matchmakingQueueRegionStats);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0000BF10 File Offset: 0x0000A110
		public MatchmakingQueueRegionStats GetRegionStats(string region)
		{
			foreach (MatchmakingQueueRegionStats matchmakingQueueRegionStats in this.RegionStats)
			{
				if (matchmakingQueueRegionStats.Region.ToLower() == region.ToLower())
				{
					return matchmakingQueueRegionStats;
				}
			}
			return null;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0000BF7C File Offset: 0x0000A17C
		public int GetQueueCountOf(string region, string[] gameTypes)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(region) && gameTypes != null)
			{
				MatchmakingQueueRegionStats regionStats = this.GetRegionStats(region);
				if (regionStats != null)
				{
					num = regionStats.GetQueueCountOf(gameTypes);
				}
			}
			return num;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0000BFAC File Offset: 0x0000A1AC
		public string[] GetRegionNames()
		{
			string[] array = new string[this.RegionStats.Count];
			for (int i = 0; i < this.RegionStats.Count; i++)
			{
				array[i] = this.RegionStats[i].Region;
			}
			return array;
		}

		// Token: 0x04000354 RID: 852
		[JsonProperty]
		public List<MatchmakingQueueRegionStats> RegionStats;
	}
}
