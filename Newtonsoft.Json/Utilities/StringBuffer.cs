using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000067 RID: 103
	[NullableContext(2)]
	[Nullable(0)]
	internal struct StringBuffer
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00018279 File Offset: 0x00016479
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00018281 File Offset: 0x00016481
		public int Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001828A File Offset: 0x0001648A
		public bool IsEmpty
		{
			get
			{
				return this._buffer == null;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00018295 File Offset: 0x00016495
		public StringBuffer(IArrayPool<char> bufferPool, int initalSize)
		{
			this = new StringBuffer(BufferUtils.RentBuffer(bufferPool, initalSize));
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000182A4 File Offset: 0x000164A4
		[NullableContext(1)]
		private StringBuffer(char[] buffer)
		{
			this._buffer = buffer;
			this._position = 0;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000182B4 File Offset: 0x000164B4
		public void Append(IArrayPool<char> bufferPool, char value)
		{
			if (this._position == this._buffer.Length)
			{
				this.EnsureSize(bufferPool, 1);
			}
			char[] buffer = this._buffer;
			int position = this._position;
			this._position = position + 1;
			buffer[position] = value;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000182F4 File Offset: 0x000164F4
		[NullableContext(1)]
		public void Append([Nullable(2)] IArrayPool<char> bufferPool, char[] buffer, int startIndex, int count)
		{
			if (this._position + count >= this._buffer.Length)
			{
				this.EnsureSize(bufferPool, count);
			}
			Array.Copy(buffer, startIndex, this._buffer, this._position, count);
			this._position += count;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00018341 File Offset: 0x00016541
		public void Clear(IArrayPool<char> bufferPool)
		{
			if (this._buffer != null)
			{
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
				this._buffer = null;
			}
			this._position = 0;
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00018368 File Offset: 0x00016568
		private void EnsureSize(IArrayPool<char> bufferPool, int appendLength)
		{
			char[] array = BufferUtils.RentBuffer(bufferPool, (this._position + appendLength) * 2);
			if (this._buffer != null)
			{
				Array.Copy(this._buffer, array, this._position);
				BufferUtils.ReturnBuffer(bufferPool, this._buffer);
			}
			this._buffer = array;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000183B3 File Offset: 0x000165B3
		[NullableContext(1)]
		public override string ToString()
		{
			return this.ToString(0, this._position);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000183C2 File Offset: 0x000165C2
		[NullableContext(1)]
		public string ToString(int start, int length)
		{
			return new string(this._buffer, start, length);
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000183D1 File Offset: 0x000165D1
		public char[] InternalBuffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x04000200 RID: 512
		private char[] _buffer;

		// Token: 0x04000201 RID: 513
		private int _position;
	}
}
