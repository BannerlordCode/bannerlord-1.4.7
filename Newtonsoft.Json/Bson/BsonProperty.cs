using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010D RID: 269
	internal class BsonProperty
	{
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00035DE3 File Offset: 0x00033FE3
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x00035DEB File Offset: 0x00033FEB
		public BsonString Name { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00035DF4 File Offset: 0x00033FF4
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x00035DFC File Offset: 0x00033FFC
		public BsonToken Value { get; set; }
	}
}
