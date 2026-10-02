using System;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x0200000B RID: 11
	public class MBOutOfRangeException : ObjectSystemException
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004EA3 File Offset: 0x000030A3
		internal MBOutOfRangeException(string parameterName)
			: base("The given value is out of range : " + parameterName)
		{
		}
	}
}
