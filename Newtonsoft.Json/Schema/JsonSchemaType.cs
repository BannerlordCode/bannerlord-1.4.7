using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AD RID: 173
	[Flags]
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum JsonSchemaType
	{
		// Token: 0x04000343 RID: 835
		None = 0,
		// Token: 0x04000344 RID: 836
		String = 1,
		// Token: 0x04000345 RID: 837
		Float = 2,
		// Token: 0x04000346 RID: 838
		Integer = 4,
		// Token: 0x04000347 RID: 839
		Boolean = 8,
		// Token: 0x04000348 RID: 840
		Object = 16,
		// Token: 0x04000349 RID: 841
		Array = 32,
		// Token: 0x0400034A RID: 842
		Null = 64,
		// Token: 0x0400034B RID: 843
		Any = 127
	}
}
