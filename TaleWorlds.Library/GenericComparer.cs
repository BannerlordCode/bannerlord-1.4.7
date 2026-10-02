using System;
using System.Collections.Generic;

namespace TaleWorlds.Library
{
	// Token: 0x02000035 RID: 53
	public class GenericComparer<T> : Comparer<T> where T : IComparable<T>
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00007068 File Offset: 0x00005268
		public override int Compare(T x, T y)
		{
			if (x != null)
			{
				if (y != null)
				{
					return x.CompareTo(y);
				}
				return 1;
			}
			else
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007096 File Offset: 0x00005296
		public override bool Equals(object obj)
		{
			return obj is GenericComparer<T>;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000070A1 File Offset: 0x000052A1
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
