using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200025D RID: 605
	public interface IMissionAgentSpawnLogic : IMissionBehavior
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x0600224E RID: 8782
		BattleSideEnum PlayerSide { get; }

		// Token: 0x0600224F RID: 8783
		void StartSpawner(BattleSideEnum side);

		// Token: 0x06002250 RID: 8784
		void StopSpawner(BattleSideEnum side);

		// Token: 0x06002251 RID: 8785
		bool IsSideSpawnEnabled(BattleSideEnum side);

		// Token: 0x06002252 RID: 8786
		bool IsSideDepleted(BattleSideEnum side);

		// Token: 0x06002253 RID: 8787
		float GetReinforcementInterval(BattleSideEnum side = BattleSideEnum.None);

		// Token: 0x06002254 RID: 8788
		IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side);

		// Token: 0x06002255 RID: 8789
		bool GetSpawnHorses(BattleSideEnum side);

		// Token: 0x06002256 RID: 8790
		int GetNumberOfPlayerControllableTroops();
	}
}
