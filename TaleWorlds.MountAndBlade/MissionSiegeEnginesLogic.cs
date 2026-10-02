using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000292 RID: 658
	public class MissionSiegeEnginesLogic : MissionLogic
	{
		// Token: 0x06002481 RID: 9345 RVA: 0x0008491F File Offset: 0x00082B1F
		public MissionSiegeEnginesLogic(List<MissionSiegeWeapon> defenderSiegeWeapons, List<MissionSiegeWeapon> attackerSiegeWeapons)
		{
			this._defenderSiegeWeaponsController = new MissionSiegeWeaponsController(BattleSideEnum.Defender, defenderSiegeWeapons);
			this._attackerSiegeWeaponsController = new MissionSiegeWeaponsController(BattleSideEnum.Attacker, attackerSiegeWeapons);
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00084941 File Offset: 0x00082B41
		public IMissionSiegeWeaponsController GetSiegeWeaponsController(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Defender)
			{
				return this._defenderSiegeWeaponsController;
			}
			if (side == BattleSideEnum.Attacker)
			{
				return this._attackerSiegeWeaponsController;
			}
			return null;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x00084959 File Offset: 0x00082B59
		public void GetMissionSiegeWeapons(out IEnumerable<IMissionSiegeWeapon> defenderSiegeWeapons, out IEnumerable<IMissionSiegeWeapon> attackerSiegeWeapons)
		{
			defenderSiegeWeapons = this._defenderSiegeWeaponsController.GetSiegeWeapons();
			attackerSiegeWeapons = this._attackerSiegeWeaponsController.GetSiegeWeapons();
		}

		// Token: 0x04000E0C RID: 3596
		private readonly MissionSiegeWeaponsController _defenderSiegeWeaponsController;

		// Token: 0x04000E0D RID: 3597
		private readonly MissionSiegeWeaponsController _attackerSiegeWeaponsController;
	}
}
