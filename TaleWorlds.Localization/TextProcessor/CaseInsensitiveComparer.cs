using System;
using System.Collections.Generic;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x0200002D RID: 45
	internal class CaseInsensitiveComparer : IEqualityComparer<string>
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00006FE6 File Offset: 0x000051E6
		public bool Equals(string x, string y)
		{
			return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006FF0 File Offset: 0x000051F0
		public int GetHashCode(string x)
		{
			return x.ToLowerInvariant().GetHashCode();
		}
	}
}
