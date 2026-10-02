using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	public interface ITraceWriter
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000667 RID: 1639
		TraceLevel LevelFilter { get; }

		// Token: 0x06000668 RID: 1640
		void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex);
	}
}
