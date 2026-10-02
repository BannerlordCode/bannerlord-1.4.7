using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200001B RID: 27
	public class TeamDeathmatchSpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00006F85 File Offset: 0x00005185
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			return base.GetSpawnFrameFromSpawnPoints(this.SpawnPoints.ToList<GameEntity>(), team, hasMount);
		}
	}
}
