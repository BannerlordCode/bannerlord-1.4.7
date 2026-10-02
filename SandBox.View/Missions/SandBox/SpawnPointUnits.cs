using System;

namespace SandBox.View.Missions.SandBox
{
	// Token: 0x0200002C RID: 44
	public class SpawnPointUnits
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000D4B6 File Offset: 0x0000B6B6
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000D4BE File Offset: 0x0000B6BE
		public string SpName { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000D4C7 File Offset: 0x0000B6C7
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000D4CF File Offset: 0x0000B6CF
		public SpawnPointUnits.SceneType Place { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000D4D8 File Offset: 0x0000B6D8
		// (set) Token: 0x0600012E RID: 302 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		public int MinCount { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000D4E9 File Offset: 0x0000B6E9
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000D4F1 File Offset: 0x0000B6F1
		public int MaxCount { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000D4FA File Offset: 0x0000B6FA
		// (set) Token: 0x06000132 RID: 306 RVA: 0x0000D502 File Offset: 0x0000B702
		public string Type { get; private set; }

		// Token: 0x06000133 RID: 307 RVA: 0x0000D50B File Offset: 0x0000B70B
		public SpawnPointUnits(string sp_name, SpawnPointUnits.SceneType place, int minCount, int maxCount)
		{
			this.SpName = sp_name;
			this.Place = place;
			this.MinCount = minCount;
			this.MaxCount = maxCount;
			this.CurrentCount = 0;
			this.SpawnedAgentCount = 0;
			this.Type = "other";
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000D549 File Offset: 0x0000B749
		public SpawnPointUnits(string sp_name, SpawnPointUnits.SceneType place, string type, int minCount, int maxCount)
		{
			this.SpName = sp_name;
			this.Place = place;
			this.Type = type;
			this.MinCount = minCount;
			this.MaxCount = maxCount;
			this.CurrentCount = 0;
			this.SpawnedAgentCount = 0;
		}

		// Token: 0x0400008A RID: 138
		public int CurrentCount;

		// Token: 0x0400008C RID: 140
		public int SpawnedAgentCount;

		// Token: 0x02000090 RID: 144
		public enum SceneType
		{
			// Token: 0x040002D5 RID: 725
			Center,
			// Token: 0x040002D6 RID: 726
			Shipyard,
			// Token: 0x040002D7 RID: 727
			Tavern,
			// Token: 0x040002D8 RID: 728
			VillageCenter,
			// Token: 0x040002D9 RID: 729
			Arena,
			// Token: 0x040002DA RID: 730
			LordsHall,
			// Token: 0x040002DB RID: 731
			Castle,
			// Token: 0x040002DC RID: 732
			Dungeon,
			// Token: 0x040002DD RID: 733
			EmptyShop,
			// Token: 0x040002DE RID: 734
			All,
			// Token: 0x040002DF RID: 735
			NotDetermined
		}
	}
}
