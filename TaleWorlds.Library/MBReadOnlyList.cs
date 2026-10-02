using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006E RID: 110
	public class MBReadOnlyList<T> : List<T>
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		public MBReadOnlyList()
		{
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		public MBReadOnlyList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000DDD5 File Offset: 0x0000BFD5
		public MBReadOnlyList(IEnumerable<T> collection)
			: base(collection)
		{
		}
	}
}
