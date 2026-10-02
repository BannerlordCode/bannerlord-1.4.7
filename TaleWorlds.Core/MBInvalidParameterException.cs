using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000A4 RID: 164
	public class MBInvalidParameterException : MBException
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x0001DDA4 File Offset: 0x0001BFA4
		public MBInvalidParameterException(string parameterName)
			: base("The parameter must be valid : " + parameterName)
		{
		}
	}
}
