using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000265 RID: 613
	public interface IPlayerInputEffector : IMissionBehavior
	{
		// Token: 0x0600226C RID: 8812
		Agent.EventControlFlag OnCollectPlayerEventControlFlags();
	}
}
