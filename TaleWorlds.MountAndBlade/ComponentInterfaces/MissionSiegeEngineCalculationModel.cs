using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003F6 RID: 1014
	public abstract class MissionSiegeEngineCalculationModel : MBGameModel<MissionSiegeEngineCalculationModel>
	{
		// Token: 0x06003756 RID: 14166
		public abstract float CalculateReloadSpeed(Agent userAgent, float baseSpeed);

		// Token: 0x06003757 RID: 14167
		public abstract int CalculateShipSiegeWeaponAmmoCount(IShipOrigin shipOrigin, Agent captain, RangedSiegeWeapon weapon);

		// Token: 0x06003758 RID: 14168
		public abstract int CalculateDamage(Agent attackerAgent, float baseDamage);
	}
}
