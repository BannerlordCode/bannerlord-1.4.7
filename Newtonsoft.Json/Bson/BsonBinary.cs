using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010B RID: 267
	internal class BsonBinary : BsonValue
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00035D79 File Offset: 0x00033F79
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x00035D81 File Offset: 0x00033F81
		public BsonBinaryType BinaryType { get; set; }

		// Token: 0x06000D88 RID: 3464 RVA: 0x00035D8A File Offset: 0x00033F8A
		public BsonBinary(byte[] value, BsonBinaryType binaryType)
			: base(value, BsonType.Binary)
		{
			this.BinaryType = binaryType;
		}
	}
}
