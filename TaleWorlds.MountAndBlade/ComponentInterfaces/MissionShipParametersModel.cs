using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003F5 RID: 1013
	public abstract class MissionShipParametersModel : MBGameModel<MissionShipParametersModel>
	{
		// Token: 0x06003752 RID: 14162
		public abstract int CalculateMainDeckCrewSize(IShipOrigin shipOrigin, Agent formationUnit);

		// Token: 0x06003753 RID: 14163
		public abstract float CalculateWindBonus(IShipOrigin shipOrigin, Agent captain, float baseSailForceMagnitude);

		// Token: 0x06003754 RID: 14164
		public abstract float CalculateOarForceMultiplier(Agent pilotAgent, float baseOarForce);
	}
}
