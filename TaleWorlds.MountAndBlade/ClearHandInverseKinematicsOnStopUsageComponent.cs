using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200036B RID: 875
	public class ClearHandInverseKinematicsOnStopUsageComponent : UsableMissionObjectComponent
	{
		// Token: 0x0600323F RID: 12863 RVA: 0x000CD1D9 File Offset: 0x000CB3D9
		protected internal override void OnUseStopped(Agent userAgent, bool isSuccessful = true)
		{
			userAgent.ClearHandInverseKinematics();
		}
	}
}
