using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000076 RID: 118
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001AEBC File Offset: 0x000190BC
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0001AEC4 File Offset: 0x000190C4
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x0600064C RID: 1612 RVA: 0x0001AECD File Offset: 0x000190CD
		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001AEFC File Offset: 0x000190FC
		[NullableContext(1)]
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache traceEventCache = new TraceEventCache();
			TraceEventType traceEventType = this.GetTraceEventType(level);
			foreach (object obj in global::System.Diagnostics.Trace.Listeners)
			{
				TraceListener traceListener = (TraceListener)obj;
				if (!traceListener.IsThreadSafe)
				{
					TraceListener traceListener2 = traceListener;
					lock (traceListener2)
					{
						traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
						goto IL_006E;
					}
					goto IL_005F;
				}
				goto IL_005F;
				IL_006E:
				if (global::System.Diagnostics.Trace.AutoFlush)
				{
					traceListener.Flush();
					continue;
				}
				continue;
				IL_005F:
				traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
				goto IL_006E;
			}
		}
	}
}
