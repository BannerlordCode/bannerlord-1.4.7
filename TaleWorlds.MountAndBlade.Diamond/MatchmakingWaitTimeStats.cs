using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000134 RID: 308
	[Serializable]
	public class MatchmakingWaitTimeStats
	{
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0000C275 File Offset: 0x0000A475
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0000C27C File Offset: 0x0000A47C
		public static MatchmakingWaitTimeStats Empty { get; private set; } = new MatchmakingWaitTimeStats();

		// Token: 0x06000862 RID: 2146 RVA: 0x0000C290 File Offset: 0x0000A490
		public MatchmakingWaitTimeStats()
		{
			this._regionStats = new List<MatchmakingWaitTimeRegionStats>();
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0000C2A3 File Offset: 0x0000A4A3
		public void AddRegionStats(MatchmakingWaitTimeRegionStats regionStats)
		{
			this._regionStats.Add(regionStats);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public MatchmakingWaitTimeRegionStats GetRegionStats(string region)
		{
			foreach (MatchmakingWaitTimeRegionStats matchmakingWaitTimeRegionStats in this._regionStats)
			{
				if (matchmakingWaitTimeRegionStats.Region.ToLower() == region.ToLower())
				{
					return matchmakingWaitTimeRegionStats;
				}
			}
			return null;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0000C320 File Offset: 0x0000A520
		public int GetWaitTime(string region, string gameType, WaitTimeStatType statType)
		{
			int num = 0;
			if (!string.IsNullOrEmpty(region) && !string.IsNullOrEmpty(gameType))
			{
				MatchmakingWaitTimeRegionStats regionStats = this.GetRegionStats(region);
				if (regionStats != null)
				{
					num = regionStats.GetWaitTime(gameType, statType);
				}
			}
			return num;
		}

		// Token: 0x0400035F RID: 863
		private List<MatchmakingWaitTimeRegionStats> _regionStats;
	}
}
