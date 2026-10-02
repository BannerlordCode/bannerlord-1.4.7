using System;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002CD RID: 717
	public class FFASpawnFrameBehavior : SpawnFrameBehaviorBase
	{
		// Token: 0x06002977 RID: 10615 RVA: 0x0009BCCD File Offset: 0x00099ECD
		public override MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn)
		{
			return base.GetSpawnFrameFromSpawnPoints(this.SpawnPoints.ToList<GameEntity>(), null, hasMount);
		}
	}
}
