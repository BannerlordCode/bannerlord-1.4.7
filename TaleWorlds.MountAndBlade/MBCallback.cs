using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D9 RID: 729
	public class MBCallback : ManagedFromNativeCallback
	{
		// Token: 0x06002A89 RID: 10889 RVA: 0x000A36D1 File Offset: 0x000A18D1
		public MBCallback(string[] conditionals = null, bool isMultiThreadCallable = false)
			: base(conditionals, isMultiThreadCallable)
		{
		}
	}
}
