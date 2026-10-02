using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006F RID: 111
	public class MBReadOnlyQueue<T> : Queue<T>
	{
		// Token: 0x060003E8 RID: 1000 RVA: 0x0000DDDE File Offset: 0x0000BFDE
		public MBReadOnlyQueue()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000DDE6 File Offset: 0x0000BFE6
		public MBReadOnlyQueue(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000DDEF File Offset: 0x0000BFEF
		public MBReadOnlyQueue(Queue<T> queue)
			: base(queue)
		{
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
		public MBReadOnlyQueue(IEnumerable<T> collection)
			: base(collection)
		{
		}
	}
}
