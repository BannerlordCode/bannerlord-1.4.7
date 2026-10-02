using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F2 RID: 242
	[JsonConverter(typeof(BattlePlayerStatsBaseJsonConverter))]
	[Serializable]
	public class BattlePlayerStatsBase
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x000055D6 File Offset: 0x000037D6
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x000055DE File Offset: 0x000037DE
		public string GameType { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000055E7 File Offset: 0x000037E7
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x000055EF File Offset: 0x000037EF
		public int Kills { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000055F8 File Offset: 0x000037F8
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00005600 File Offset: 0x00003800
		public int Assists { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00005609 File Offset: 0x00003809
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00005611 File Offset: 0x00003811
		public int Deaths { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0000561A File Offset: 0x0000381A
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00005622 File Offset: 0x00003822
		public int PlayTime { get; set; }
	}
}
