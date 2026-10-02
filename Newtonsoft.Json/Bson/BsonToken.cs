using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000104 RID: 260
	internal abstract class BsonToken
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000D6A RID: 3434
		public abstract BsonType Type { get; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00035C06 File Offset: 0x00033E06
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x00035C0E File Offset: 0x00033E0E
		public BsonToken Parent { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x00035C17 File Offset: 0x00033E17
		// (set) Token: 0x06000D6E RID: 3438 RVA: 0x00035C1F File Offset: 0x00033E1F
		public int CalculatedSize { get; set; }
	}
}
