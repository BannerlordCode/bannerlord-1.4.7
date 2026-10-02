using System;
using System.Linq;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000133 RID: 307
	[Serializable]
	public class MatchmakingQueueGameTypeStats
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0000C1B3 File Offset: 0x0000A3B3
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x0000C1BB File Offset: 0x0000A3BB
		[JsonProperty]
		public string[] GameTypes { get; set; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0000C1CC File Offset: 0x0000A3CC
		[JsonProperty]
		public int Count { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0000C1D5 File Offset: 0x0000A3D5
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0000C1DD File Offset: 0x0000A3DD
		[JsonProperty]
		public int TotalWaitTime { get; set; }

		// Token: 0x0600085A RID: 2138 RVA: 0x0000C1E6 File Offset: 0x0000A3E6
		public MatchmakingQueueGameTypeStats()
		{
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0000C1EE File Offset: 0x0000A3EE
		public MatchmakingQueueGameTypeStats(string[] gameTypes)
		{
			this.GameTypes = gameTypes;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0000C1FD File Offset: 0x0000A3FD
		public bool HasGameType(string gameType)
		{
			return this.GameTypes.Contains(gameType);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0000C20C File Offset: 0x0000A40C
		public bool EqualWith(string[] gameTypes)
		{
			if (this.GameTypes.Length == gameTypes.Length)
			{
				foreach (string text in gameTypes)
				{
					if (!this.HasGameType(text))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0000C248 File Offset: 0x0000A448
		internal bool HasAnyGameType(string[] gameTypes)
		{
			foreach (string text in gameTypes)
			{
				if (this.HasGameType(text))
				{
					return true;
				}
			}
			return false;
		}
	}
}
