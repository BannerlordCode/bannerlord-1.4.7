using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq.JsonPath
{
	// Token: 0x020000D8 RID: 216
	[NullableContext(1)]
	[Nullable(0)]
	internal class RootFilter : PathFilter
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x0002FDCF File Offset: 0x0002DFCF
		private RootFilter()
		{
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0002FDD7 File Offset: 0x0002DFD7
		public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current, [Nullable(2)] JsonSelectSettings settings)
		{
			return new JToken[] { root };
		}

		// Token: 0x040003C9 RID: 969
		public static readonly RootFilter Instance = new RootFilter();
	}
}
