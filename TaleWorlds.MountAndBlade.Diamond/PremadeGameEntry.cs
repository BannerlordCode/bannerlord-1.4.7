using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000151 RID: 337
	[Serializable]
	public class PremadeGameEntry
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x0000DC11 File Offset: 0x0000BE11
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x0000DC19 File Offset: 0x0000BE19
		[JsonProperty]
		public Guid Id { get; private set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0000DC22 File Offset: 0x0000BE22
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x0000DC2A File Offset: 0x0000BE2A
		[JsonProperty]
		public string Name { get; private set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0000DC33 File Offset: 0x0000BE33
		// (set) Token: 0x06000968 RID: 2408 RVA: 0x0000DC3B File Offset: 0x0000BE3B
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x0000DC44 File Offset: 0x0000BE44
		// (set) Token: 0x0600096A RID: 2410 RVA: 0x0000DC4C File Offset: 0x0000BE4C
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0000DC55 File Offset: 0x0000BE55
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0000DC5D File Offset: 0x0000BE5D
		[JsonProperty]
		public string MapName { get; private set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0000DC66 File Offset: 0x0000BE66
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0000DC6E File Offset: 0x0000BE6E
		[JsonProperty]
		public string FactionA { get; private set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0000DC77 File Offset: 0x0000BE77
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0000DC7F File Offset: 0x0000BE7F
		[JsonProperty]
		public string FactionB { get; private set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0000DC88 File Offset: 0x0000BE88
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0000DC90 File Offset: 0x0000BE90
		[JsonProperty]
		public bool IsPasswordProtected { get; private set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0000DC99 File Offset: 0x0000BE99
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0000DCA1 File Offset: 0x0000BEA1
		[JsonProperty]
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x06000975 RID: 2421 RVA: 0x0000DCAA File Offset: 0x0000BEAA
		public PremadeGameEntry()
		{
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		public PremadeGameEntry(Guid id, string name, string region, string gameType, string mapName, string factionA, string factionB, bool isPasswordProtected, PremadeGameType premadeGameType)
		{
			this.Id = id;
			this.Name = name;
			this.Region = region;
			this.GameType = gameType;
			this.MapName = mapName;
			this.FactionA = factionA;
			this.FactionB = factionB;
			this.IsPasswordProtected = isPasswordProtected;
			this.PremadeGameType = premadeGameType;
		}
	}
}
