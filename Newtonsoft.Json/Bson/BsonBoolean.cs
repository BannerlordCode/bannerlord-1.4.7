using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000109 RID: 265
	internal class BsonBoolean : BsonValue
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x00035D28 File Offset: 0x00033F28
		private BsonBoolean(bool value)
			: base(value, BsonType.Boolean)
		{
		}

		// Token: 0x04000421 RID: 1057
		public static readonly BsonBoolean False = new BsonBoolean(false);

		// Token: 0x04000422 RID: 1058
		public static readonly BsonBoolean True = new BsonBoolean(true);
	}
}
