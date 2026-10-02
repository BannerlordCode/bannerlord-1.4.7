using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003CF RID: 975
	public class BattleSpawnLogic : MissionLogic
	{
		// Token: 0x06003652 RID: 13906 RVA: 0x000E06CA File Offset: 0x000DE8CA
		public BattleSpawnLogic(string selectedSpawnPointSetTag)
		{
			this._selectedSpawnPointSetTag = selectedSpawnPointSetTag;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000E06DC File Offset: 0x000DE8DC
		public override void OnPreMissionTick(float dt)
		{
			if (this._isScenePrepared)
			{
				return;
			}
			WeakGameEntity weakGameEntity = base.Mission.Scene.FindWeakEntityWithTag(this._selectedSpawnPointSetTag);
			if (weakGameEntity != null)
			{
				List<WeakGameEntity> list = base.Mission.Scene.FindWeakEntitiesWithTag("spawnpoint_set").ToList<WeakGameEntity>();
				list.Remove(weakGameEntity);
				foreach (WeakGameEntity weakGameEntity2 in list)
				{
					weakGameEntity2.Remove(76);
				}
			}
			this._isScenePrepared = true;
		}

		// Token: 0x04001754 RID: 5972
		public const string BattleTag = "battle_set";

		// Token: 0x04001755 RID: 5973
		public const string SallyOutTag = "sally_out_set";

		// Token: 0x04001756 RID: 5974
		public const string ReliefForceAttackTag = "relief_force_attack_set";

		// Token: 0x04001757 RID: 5975
		private const string SpawnPointSetCommonTag = "spawnpoint_set";

		// Token: 0x04001758 RID: 5976
		private readonly string _selectedSpawnPointSetTag;

		// Token: 0x04001759 RID: 5977
		private bool _isScenePrepared;
	}
}
