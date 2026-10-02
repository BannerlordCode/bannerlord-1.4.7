using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	internal class EnumInfo
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x00014947 File Offset: 0x00012B47
		public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
		{
			this.IsFlags = isFlags;
			this.Values = values;
			this.Names = names;
			this.ResolvedNames = resolvedNames;
		}

		// Token: 0x040001C1 RID: 449
		public readonly bool IsFlags;

		// Token: 0x040001C2 RID: 450
		public readonly ulong[] Values;

		// Token: 0x040001C3 RID: 451
		public readonly string[] Names;

		// Token: 0x040001C4 RID: 452
		public readonly string[] ResolvedNames;
	}
}
