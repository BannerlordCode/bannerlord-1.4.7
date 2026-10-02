using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x00022D60 File Offset: 0x00020F60
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x00022D68 File Offset: 0x00020F68
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000801 RID: 2049 RVA: 0x00022D71 File Offset: 0x00020F71
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
			this._lock = new object();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00022D98 File Offset: 0x00020F98
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture));
			stringBuilder.Append(" ");
			stringBuilder.Append(level.ToString("g"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			string text = stringBuilder.ToString();
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._traceMessages.Count >= 1000)
				{
					this._traceMessages.Dequeue();
				}
				this._traceMessages.Enqueue(text);
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00022E5C File Offset: 0x0002105C
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00022E64 File Offset: 0x00021064
		public override string ToString()
		{
			object @lock = this._lock;
			string text2;
			lock (@lock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this._traceMessages)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
				text2 = stringBuilder.ToString();
			}
			return text2;
		}

		// Token: 0x040002BE RID: 702
		private readonly Queue<string> _traceMessages;

		// Token: 0x040002BF RID: 703
		private readonly object _lock;
	}
}
