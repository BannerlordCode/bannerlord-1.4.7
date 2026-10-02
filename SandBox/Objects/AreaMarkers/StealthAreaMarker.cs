using System;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects.AreaMarkers
{
	// Token: 0x02000046 RID: 70
	public class StealthAreaMarker : AreaMarker
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000F6AA File Offset: 0x0000D8AA
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x0000F6B2 File Offset: 0x0000D8B2
		public GameEntity ReinforcementAllyGroupSpawnPoint { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000F6BB File Offset: 0x0000D8BB
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000F6C3 File Offset: 0x0000D8C3
		public GameEntity WaitPoint { get; private set; }

		// Token: 0x060002A4 RID: 676 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		public override void AfterMissionStart()
		{
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				if (weakGameEntity.HasTag("reinforcement_ally_group_spawn_point_tag"))
				{
					this.ReinforcementAllyGroupSpawnPoint = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
				}
				if (weakGameEntity.HasTag("wait_point_tag"))
				{
					this.WaitPoint = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
				}
			}
		}

		// Token: 0x04000128 RID: 296
		private const string ReinforcementAllyGroupSpawnPointTag = "reinforcement_ally_group_spawn_point_tag";

		// Token: 0x04000129 RID: 297
		private const string WaitPointTag = "wait_point_tag";

		// Token: 0x0400012A RID: 298
		public string ReinforcementAllyGroupId;
	}
}
