using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000069 RID: 105
	public class MBList<T> : MBReadOnlyList<T>
	{
		// Token: 0x06000355 RID: 853 RVA: 0x0000C1FF File Offset: 0x0000A3FF
		public MBList()
		{
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000C207 File Offset: 0x0000A407
		public MBList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000C210 File Offset: 0x0000A410
		public MBList(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000C219 File Offset: 0x0000A419
		public MBList(List<T> collection)
			: base(collection)
		{
		}
	}
}
