using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Core
{
	// Token: 0x0200009F RID: 159
	public class MBException : ApplicationException
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x0001DD26 File Offset: 0x0001BF26
		public MBException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001DD30 File Offset: 0x0001BF30
		public MBException(string message)
			: base(message)
		{
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001DD39 File Offset: 0x0001BF39
		public MBException()
		{
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001DD41 File Offset: 0x0001BF41
		public MBException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
