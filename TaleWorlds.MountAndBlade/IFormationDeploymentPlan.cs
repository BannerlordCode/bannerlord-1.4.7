using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000215 RID: 533
	public interface IFormationDeploymentPlan
	{
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001EFE RID: 7934
		FormationClass Class { get; }

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001EFF RID: 7935
		FormationClass SpawnClass { get; }

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001F00 RID: 7936
		float PlannedWidth { get; }

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001F01 RID: 7937
		float PlannedDepth { get; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001F02 RID: 7938
		int PlannedTroopCount { get; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001F03 RID: 7939
		bool HasDimensions { get; }

		// Token: 0x06001F04 RID: 7940
		bool HasFrame();

		// Token: 0x06001F05 RID: 7941
		MatrixFrame GetFrame();

		// Token: 0x06001F06 RID: 7942
		Vec3 GetPosition();

		// Token: 0x06001F07 RID: 7943
		Vec2 GetDirection();

		// Token: 0x06001F08 RID: 7944
		WorldPosition CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache);
	}
}
