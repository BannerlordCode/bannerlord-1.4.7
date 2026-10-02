using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000214 RID: 532
	public struct FormationSceneSpawnEntry
	{
		// Token: 0x06001EFD RID: 7933 RVA: 0x0006BB52 File Offset: 0x00069D52
		public FormationSceneSpawnEntry(FormationClass formationClass, GameEntity spawnEntity, GameEntity reinforcementSpawnEntity)
		{
			this.FormationClass = formationClass;
			this.SpawnEntity = spawnEntity;
			this.ReinforcementSpawnEntity = reinforcementSpawnEntity;
		}

		// Token: 0x04000A9D RID: 2717
		public readonly FormationClass FormationClass;

		// Token: 0x04000A9E RID: 2718
		public readonly GameEntity SpawnEntity;

		// Token: 0x04000A9F RID: 2719
		public readonly GameEntity ReinforcementSpawnEntity;
	}
}
