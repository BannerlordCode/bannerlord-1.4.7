using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000375 RID: 885
	public interface IUsable
	{
		// Token: 0x0600325D RID: 12893
		void OnUse(Agent userAgent, sbyte agentBoneIndex);

		// Token: 0x0600325E RID: 12894
		void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex);
	}
}
