using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003E1 RID: 993
	public interface IMissionSiegeWeaponsController
	{
		// Token: 0x060036B5 RID: 14005
		int GetMaxDeployableWeaponCount(Type t);

		// Token: 0x060036B6 RID: 14006
		IEnumerable<IMissionSiegeWeapon> GetSiegeWeapons();

		// Token: 0x060036B7 RID: 14007
		void OnWeaponDeployed(SiegeWeapon missionWeapon);

		// Token: 0x060036B8 RID: 14008
		void OnWeaponUndeployed(SiegeWeapon missionWeapon);
	}
}
