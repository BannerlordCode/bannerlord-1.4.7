using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003F3 RID: 1011
	public class DefaultSiegeEngineCalculationModel : MissionSiegeEngineCalculationModel
	{
		// Token: 0x06003744 RID: 14148 RVA: 0x000E4C7B File Offset: 0x000E2E7B
		public override float CalculateReloadSpeed(Agent userAgent, float baseSpeed)
		{
			return baseSpeed;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x000E4C7E File Offset: 0x000E2E7E
		public override int CalculateShipSiegeWeaponAmmoCount(IShipOrigin shipOrigin, Agent captain, RangedSiegeWeapon weapon)
		{
			return weapon.AmmoCount;
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x000E4C86 File Offset: 0x000E2E86
		public override int CalculateDamage(Agent attackerAgent, float baseDamage)
		{
			return (int)baseDamage;
		}
	}
}
