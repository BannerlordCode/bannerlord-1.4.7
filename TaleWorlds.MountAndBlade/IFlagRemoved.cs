using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000261 RID: 609
	public interface IFlagRemoved : IMissionBehavior
	{
		// Token: 0x0600225F RID: 8799
		void OnFlagsRemoved(int remainingFlagIndex);
	}
}
