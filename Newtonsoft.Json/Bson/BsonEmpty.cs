using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000107 RID: 263
	internal class BsonEmpty : BsonToken
	{
		// Token: 0x06000D7A RID: 3450 RVA: 0x00035CD2 File Offset: 0x00033ED2
		private BsonEmpty(BsonType type)
		{
			this.Type = type;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00035CE1 File Offset: 0x00033EE1
		public override BsonType Type { get; }

		// Token: 0x0400041C RID: 1052
		public static readonly BsonToken Null = new BsonEmpty(BsonType.Null);

		// Token: 0x0400041D RID: 1053
		public static readonly BsonToken Undefined = new BsonEmpty(BsonType.Undefined);
	}
}
