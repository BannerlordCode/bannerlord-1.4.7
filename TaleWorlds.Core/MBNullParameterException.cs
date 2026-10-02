using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A5 RID: 165
	public class MBNullParameterException : MBException
	{
		// Token: 0x06000915 RID: 2325 RVA: 0x0001DDB7 File Offset: 0x0001BFB7
		public MBNullParameterException(string parameterName)
			: base("The parameter cannot be null : " + parameterName)
		{
		}
	}
}
