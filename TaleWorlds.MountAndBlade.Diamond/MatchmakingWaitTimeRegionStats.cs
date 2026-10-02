using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000136 RID: 310
	[Serializable]
	public class MatchmakingWaitTimeRegionStats
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0000C354 File Offset: 0x0000A554
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x0000C35C File Offset: 0x0000A55C
		public string Region { get; private set; }

		// Token: 0x06000868 RID: 2152 RVA: 0x0000C365 File Offset: 0x0000A565
		public MatchmakingWaitTimeRegionStats(string region)
		{
			this.Region = region;
			this._gameTypeAverageWaitTimes = new Dictionary<string, Dictionary<WaitTimeStatType, int>>();
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0000C380 File Offset: 0x0000A580
		public void SetGameTypeAverage(string gameType, WaitTimeStatType statType, int average)
		{
			Dictionary<WaitTimeStatType, int> dictionary;
			if (!this._gameTypeAverageWaitTimes.TryGetValue(gameType, out dictionary))
			{
				dictionary = new Dictionary<WaitTimeStatType, int>();
				this._gameTypeAverageWaitTimes.Add(gameType, dictionary);
			}
			this._gameTypeAverageWaitTimes[gameType][statType] = average;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0000C3C3 File Offset: 0x0000A5C3
		public bool HasStatsForGameType(string gameType)
		{
			return gameType != null && this._gameTypeAverageWaitTimes.ContainsKey(gameType);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
		public int GetWaitTime(string gameType, WaitTimeStatType statType)
		{
			Dictionary<WaitTimeStatType, int> dictionary;
			int num;
			if (this._gameTypeAverageWaitTimes.TryGetValue(gameType, out dictionary) && dictionary.TryGetValue(statType, out num))
			{
				return num;
			}
			return int.MaxValue;
		}

		// Token: 0x04000365 RID: 869
		private Dictionary<string, Dictionary<WaitTimeStatType, int>> _gameTypeAverageWaitTimes;
	}
}
