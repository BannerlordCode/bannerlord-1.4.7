using System;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x0200000D RID: 13
	public class MBTypeMismatchException : ObjectSystemException
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004EC3 File Offset: 0x000030C3
		internal MBTypeMismatchException(string exceptionString)
			: base("Type Does not match with the expected one. " + exceptionString)
		{
		}
	}
}
