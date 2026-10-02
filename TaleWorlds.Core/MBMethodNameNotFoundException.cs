using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A3 RID: 163
	public class MBMethodNameNotFoundException : MBException
	{
		// Token: 0x06000913 RID: 2323 RVA: 0x0001DD91 File Offset: 0x0001BF91
		public MBMethodNameNotFoundException(string methodName)
			: base("Unable to find method " + methodName)
		{
		}
	}
}
