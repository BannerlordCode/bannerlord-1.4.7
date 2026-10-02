using System;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x020000AF RID: 175
	[Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
	public enum UndefinedSchemaIdHandling
	{
		// Token: 0x0400034F RID: 847
		None,
		// Token: 0x04000350 RID: 848
		UseTypeName,
		// Token: 0x04000351 RID: 849
		UseAssemblyQualifiedName
	}
}
