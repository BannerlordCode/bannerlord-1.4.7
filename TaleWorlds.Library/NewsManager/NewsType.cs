using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TaleWorlds.Library.NewsManager
{
	// Token: 0x020000AC RID: 172
	public struct NewsType
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00016BFA File Offset: 0x00014DFA
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x00016C02 File Offset: 0x00014E02
		[JsonConverter(typeof(StringEnumConverter))]
		public NewsItem.NewsTypes Type { get; set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00016C0B File Offset: 0x00014E0B
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x00016C13 File Offset: 0x00014E13
		public int Index { get; set; }
	}
}
