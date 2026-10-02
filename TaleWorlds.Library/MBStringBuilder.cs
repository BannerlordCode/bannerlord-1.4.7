using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace TaleWorlds.Library
{
	// Token: 0x0200001F RID: 31
	public struct MBStringBuilder
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00003DF7 File Offset: 0x00001FF7
		public void Initialize(int capacity = 16, [CallerMemberName] string callerMemberName = "")
		{
			this._cachedStringBuilder = MBStringBuilder.CachedStringBuilder.Acquire(capacity);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003E05 File Offset: 0x00002005
		public string ToStringAndRelease()
		{
			string text = this._cachedStringBuilder.ToString();
			this.Release();
			return text;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003E18 File Offset: 0x00002018
		public void Release()
		{
			MBStringBuilder.CachedStringBuilder.Release(this._cachedStringBuilder);
			this._cachedStringBuilder = null;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003E2C File Offset: 0x0000202C
		public MBStringBuilder Append(char value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003E41 File Offset: 0x00002041
		public MBStringBuilder Append(int value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003E56 File Offset: 0x00002056
		public MBStringBuilder Append(uint value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003E6B File Offset: 0x0000206B
		public MBStringBuilder Append(float value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003E80 File Offset: 0x00002080
		public MBStringBuilder Append(double value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003E95 File Offset: 0x00002095
		public MBStringBuilder Append<T>(T value)
		{
			this._cachedStringBuilder.Append(value);
			return this;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003EAF File Offset: 0x000020AF
		public MBStringBuilder AppendLine()
		{
			this._cachedStringBuilder.AppendLine();
			return this;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003EC3 File Offset: 0x000020C3
		public MBStringBuilder AppendLine<T>(T value)
		{
			this.Append<T>(value);
			this.AppendLine();
			return this;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003EDA File Offset: 0x000020DA
		public int Length
		{
			get
			{
				return this._cachedStringBuilder.Length;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003EE7 File Offset: 0x000020E7
		public override string ToString()
		{
			Debug.FailedAssert("Don't use this. Use ToStringAndRelease instead!", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\CachedStringBuilder.cs", "ToString", 190);
			return null;
		}

		// Token: 0x04000065 RID: 101
		private StringBuilder _cachedStringBuilder;

		// Token: 0x020000C9 RID: 201
		private static class CachedStringBuilder
		{
			// Token: 0x06000751 RID: 1873 RVA: 0x00018507 File Offset: 0x00016707
			public static StringBuilder Acquire(int capacity = 16)
			{
				if (capacity <= 4096 && MBStringBuilder.CachedStringBuilder._cachedStringBuilder != null)
				{
					StringBuilder cachedStringBuilder = MBStringBuilder.CachedStringBuilder._cachedStringBuilder;
					MBStringBuilder.CachedStringBuilder._cachedStringBuilder = null;
					cachedStringBuilder.EnsureCapacity(capacity);
					return cachedStringBuilder;
				}
				return new StringBuilder(capacity);
			}

			// Token: 0x06000752 RID: 1874 RVA: 0x00018532 File Offset: 0x00016732
			public static void Release(StringBuilder sb)
			{
				if (sb.Capacity <= 4096)
				{
					MBStringBuilder.CachedStringBuilder._cachedStringBuilder = sb;
					MBStringBuilder.CachedStringBuilder._cachedStringBuilder.Clear();
				}
			}

			// Token: 0x06000753 RID: 1875 RVA: 0x00018552 File Offset: 0x00016752
			public static string GetStringAndReleaseBuilder(StringBuilder sb)
			{
				string text = sb.ToString();
				MBStringBuilder.CachedStringBuilder.Release(sb);
				return text;
			}

			// Token: 0x04000257 RID: 599
			private const int MaxBuilderSize = 4096;

			// Token: 0x04000258 RID: 600
			[ThreadStatic]
			private static StringBuilder _cachedStringBuilder;
		}
	}
}
