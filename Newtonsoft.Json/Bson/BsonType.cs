using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200010E RID: 270
	internal enum BsonType : sbyte
	{
		// Token: 0x0400042B RID: 1067
		Number = 1,
		// Token: 0x0400042C RID: 1068
		String,
		// Token: 0x0400042D RID: 1069
		Object,
		// Token: 0x0400042E RID: 1070
		Array,
		// Token: 0x0400042F RID: 1071
		Binary,
		// Token: 0x04000430 RID: 1072
		Undefined,
		// Token: 0x04000431 RID: 1073
		Oid,
		// Token: 0x04000432 RID: 1074
		Boolean,
		// Token: 0x04000433 RID: 1075
		Date,
		// Token: 0x04000434 RID: 1076
		Null,
		// Token: 0x04000435 RID: 1077
		Regex,
		// Token: 0x04000436 RID: 1078
		Reference,
		// Token: 0x04000437 RID: 1079
		Code,
		// Token: 0x04000438 RID: 1080
		Symbol,
		// Token: 0x04000439 RID: 1081
		CodeWScope,
		// Token: 0x0400043A RID: 1082
		Integer,
		// Token: 0x0400043B RID: 1083
		TimeStamp,
		// Token: 0x0400043C RID: 1084
		Long,
		// Token: 0x0400043D RID: 1085
		MinKey = -1,
		// Token: 0x0400043E RID: 1086
		MaxKey = 127
	}
}
