using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000100 RID: 256
	internal enum BsonBinaryType : byte
	{
		// Token: 0x040003FD RID: 1021
		Binary,
		// Token: 0x040003FE RID: 1022
		Function,
		// Token: 0x040003FF RID: 1023
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld,
		// Token: 0x04000400 RID: 1024
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld,
		// Token: 0x04000401 RID: 1025
		Uuid,
		// Token: 0x04000402 RID: 1026
		Md5,
		// Token: 0x04000403 RID: 1027
		UserDefined = 128
	}
}
