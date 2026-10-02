using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000146 RID: 326
	[Serializable]
	public class PlayerLeaderboardData
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0000D29F File Offset: 0x0000B49F
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x0000D2A7 File Offset: 0x0000B4A7
		public PlayerId PlayerId { get; set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
		public string RankId { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0000D2C1 File Offset: 0x0000B4C1
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x0000D2C9 File Offset: 0x0000B4C9
		public int Rating { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0000D2D2 File Offset: 0x0000B4D2
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x0000D2DA File Offset: 0x0000B4DA
		public string Name { get; set; }

		// Token: 0x060008F9 RID: 2297 RVA: 0x0000D2E3 File Offset: 0x0000B4E3
		public PlayerLeaderboardData(PlayerId playerId, string rankId, int rating, string name)
		{
			this.PlayerId = playerId;
			this.RankId = rankId;
			this.Rating = rating;
			this.Name = name;
		}
	}
}
