using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x0200006C RID: 108
	public class MBQueue<T> : MBReadOnlyQueue<T>, IMBCollection
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x0000DB8A File Offset: 0x0000BD8A
		public MBQueue()
		{
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000DB92 File Offset: 0x0000BD92
		public MBQueue(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000DB9B File Offset: 0x0000BD9B
		public MBQueue(Queue<T> queue)
			: base(queue)
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000DBA4 File Offset: 0x0000BDA4
		public MBQueue(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000DBB0 File Offset: 0x0000BDB0
		public bool Remove(T item)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			int count = base.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				T t = base.Dequeue();
				if (!flag && @default.Equals(t, item))
				{
					flag = true;
				}
				else
				{
					base.Enqueue(t);
				}
			}
			return flag;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000DBFB File Offset: 0x0000BDFB
		void IMBCollection.Clear()
		{
			base.Clear();
		}
	}
}
