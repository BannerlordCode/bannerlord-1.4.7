using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000264 RID: 612
	public interface IBattlePowerCalculationLogic : IMissionBehavior
	{
		// Token: 0x0600226B RID: 8811
		float GetTotalTeamPower(Team team);
	}
}
