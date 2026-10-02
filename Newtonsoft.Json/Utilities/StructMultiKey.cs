using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006B RID: 107
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StructMultiKey<[Nullable(2)] T1, [Nullable(2)] T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		// Token: 0x060005D7 RID: 1495 RVA: 0x00018949 File Offset: 0x00016B49
		public StructMultiKey(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001895C File Offset: 0x00016B5C
		public override int GetHashCode()
		{
			T1 value = this.Value1;
			int num = ((value != null) ? value.GetHashCode() : 0);
			T2 value2 = this.Value2;
			return num ^ ((value2 != null) ? value2.GetHashCode() : 0);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000189B4 File Offset: 0x00016BB4
		public override bool Equals(object obj)
		{
			if (obj is StructMultiKey<T1, T2>)
			{
				StructMultiKey<T1, T2> structMultiKey = (StructMultiKey<T1, T2>)obj;
				return this.Equals(structMultiKey);
			}
			return false;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000189DB File Offset: 0x00016BDB
		public bool Equals([Nullable(new byte[] { 0, 1, 1 })] StructMultiKey<T1, T2> other)
		{
			return object.Equals(this.Value1, other.Value1) && object.Equals(this.Value2, other.Value2);
		}

		// Token: 0x0400020A RID: 522
		public readonly T1 Value1;

		// Token: 0x0400020B RID: 523
		public readonly T2 Value2;
	}
}
