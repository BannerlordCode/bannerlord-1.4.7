using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000077 RID: 119
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorContext
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x0001AFC8 File Offset: 0x000191C8
		internal ErrorContext([Nullable(2)] object originalObject, [Nullable(2)] object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001AFED File Offset: 0x000191ED
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0001AFF5 File Offset: 0x000191F5
		internal bool Traced { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0001AFFE File Offset: 0x000191FE
		public Exception Error { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0001B006 File Offset: 0x00019206
		[Nullable(2)]
		public object OriginalObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0001B00E File Offset: 0x0001920E
		[Nullable(2)]
		public object Member
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0001B016 File Offset: 0x00019216
		public string Path { get; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0001B01E File Offset: 0x0001921E
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0001B026 File Offset: 0x00019226
		public bool Handled { get; set; }
	}
}
