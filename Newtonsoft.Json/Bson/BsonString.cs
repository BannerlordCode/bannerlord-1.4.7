using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010A RID: 266
	internal class BsonString : BsonValue
	{
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00035D4F File Offset: 0x00033F4F
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00035D57 File Offset: 0x00033F57
		public int ByteCount { get; set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00035D60 File Offset: 0x00033F60
		public bool IncludeLength { get; }

		// Token: 0x06000D85 RID: 3461 RVA: 0x00035D68 File Offset: 0x00033F68
		public BsonString(object value, bool includeLength)
			: base(value, BsonType.String)
		{
			this.IncludeLength = includeLength;
		}
	}
}
