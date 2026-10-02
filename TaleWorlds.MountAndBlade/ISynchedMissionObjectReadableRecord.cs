using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000364 RID: 868
	public interface ISynchedMissionObjectReadableRecord
	{
		// Token: 0x060031D8 RID: 12760
		bool ReadFromNetwork(ref bool bufferReadValid);
	}
}
