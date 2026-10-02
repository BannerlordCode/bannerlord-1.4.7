using System;
using System.Runtime.Serialization;

namespace TaleWorlds.Library
{
	// Token: 0x02000097 RID: 151
	public class TWException : ApplicationException
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00013602 File Offset: 0x00011802
		public TWException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001360C File Offset: 0x0001180C
		public TWException(string message)
			: base(message)
		{
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00013615 File Offset: 0x00011815
		public TWException()
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001361D File Offset: 0x0001181D
		public TWException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
