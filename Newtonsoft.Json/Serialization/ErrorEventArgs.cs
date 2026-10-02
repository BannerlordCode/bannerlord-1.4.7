using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001B02F File Offset: 0x0001922F
		[Nullable(2)]
		public object CurrentObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001B037 File Offset: 0x00019237
		public ErrorContext ErrorContext { get; }

		// Token: 0x0600065A RID: 1626 RVA: 0x0001B03F File Offset: 0x0001923F
		public ErrorEventArgs([Nullable(2)] object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
