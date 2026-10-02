using System;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000150 RID: 336
	[Serializable]
	public class PremadeGameList
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x0000DBD7 File Offset: 0x0000BDD7
		public static PremadeGameList Empty { get; private set; } = new PremadeGameList(new PremadeGameEntry[0]);

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0000DBF1 File Offset: 0x0000BDF1
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0000DBF9 File Offset: 0x0000BDF9
		[JsonProperty]
		public PremadeGameEntry[] PremadeGameEntries { get; private set; }

		// Token: 0x06000962 RID: 2402 RVA: 0x0000DC02 File Offset: 0x0000BE02
		public PremadeGameList(PremadeGameEntry[] entries)
		{
			this.PremadeGameEntries = entries;
		}
	}
}
