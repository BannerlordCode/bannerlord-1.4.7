using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A6 RID: 166
	public class MBNotNullParameterException : MBException
	{
		// Token: 0x06000916 RID: 2326 RVA: 0x0001DDCA File Offset: 0x0001BFCA
		public MBNotNullParameterException(string parameterName)
			: base("The parameter must be null : " + parameterName)
		{
		}
	}
}
