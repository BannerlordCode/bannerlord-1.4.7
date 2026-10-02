using System;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D2 RID: 210
	internal enum QueryOperator
	{
		// Token: 0x040003B6 RID: 950
		None,
		// Token: 0x040003B7 RID: 951
		Equals,
		// Token: 0x040003B8 RID: 952
		NotEquals,
		// Token: 0x040003B9 RID: 953
		Exists,
		// Token: 0x040003BA RID: 954
		LessThan,
		// Token: 0x040003BB RID: 955
		LessThanOrEquals,
		// Token: 0x040003BC RID: 956
		GreaterThan,
		// Token: 0x040003BD RID: 957
		GreaterThanOrEquals,
		// Token: 0x040003BE RID: 958
		And,
		// Token: 0x040003BF RID: 959
		Or,
		// Token: 0x040003C0 RID: 960
		RegexEquals,
		// Token: 0x040003C1 RID: 961
		StrictEquals,
		// Token: 0x040003C2 RID: 962
		StrictNotEquals
	}
}
