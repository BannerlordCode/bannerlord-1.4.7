using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C1 RID: 193
	public class JsonSelectSettings
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0002A0EC File Offset: 0x000282EC
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x0002A0F4 File Offset: 0x000282F4
		public TimeSpan? RegexMatchTimeout { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0002A0FD File Offset: 0x000282FD
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x0002A105 File Offset: 0x00028305
		public bool ErrorWhenNoMatch { get; set; }
	}
}
