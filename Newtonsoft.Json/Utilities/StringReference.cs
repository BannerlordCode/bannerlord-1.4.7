using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StringReference
	{
		// Token: 0x170000D2 RID: 210
		public char this[int i]
		{
			get
			{
				return this._chars[i];
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x000183E3 File Offset: 0x000165E3
		public char[] Chars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x000183EB File Offset: 0x000165EB
		public int StartIndex
		{
			get
			{
				return this._startIndex;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x000183F3 File Offset: 0x000165F3
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000183FB File Offset: 0x000165FB
		public StringReference(char[] chars, int startIndex, int length)
		{
			this._chars = chars;
			this._startIndex = startIndex;
			this._length = length;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00018412 File Offset: 0x00016612
		public override string ToString()
		{
			return new string(this._chars, this._startIndex, this._length);
		}

		// Token: 0x04000202 RID: 514
		private readonly char[] _chars;

		// Token: 0x04000203 RID: 515
		private readonly int _startIndex;

		// Token: 0x04000204 RID: 516
		private readonly int _length;
	}
}
