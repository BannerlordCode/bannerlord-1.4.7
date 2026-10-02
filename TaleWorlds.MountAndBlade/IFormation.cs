using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000141 RID: 321
	public interface IFormation
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000FE5 RID: 4069
		float Interval { get; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000FE6 RID: 4070
		float Distance { get; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000FE7 RID: 4071
		float UnitDiameter { get; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000FE8 RID: 4072
		float MinimumInterval { get; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000FE9 RID: 4073
		float MaximumInterval { get; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000FEA RID: 4074
		float MinimumDistance { get; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000FEB RID: 4075
		float MaximumDistance { get; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000FEC RID: 4076
		int? OverridenUnitCount { get; }

		// Token: 0x06000FED RID: 4077
		bool GetIsLocalPositionAvailable(Vec2 localPosition, Vec2? nearestAvailableUnitPositionLocal);

		// Token: 0x06000FEE RID: 4078
		bool BatchUnitPositions(MBArrayList<Vec2i> orderedPositionIndices, MBArrayList<Vec2> orderedLocalPositions, MBList2D<int> availabilityTable, MBList2D<WorldPosition> globalPositionTable, int fileCount, int rankCount);

		// Token: 0x06000FEF RID: 4079
		IFormationUnit GetClosestUnitTo(Vec2 localPosition, MBList<IFormationUnit> unitsWithSpaces = null, float? maxDistance = null);

		// Token: 0x06000FF0 RID: 4080
		IFormationUnit GetClosestUnitTo(IFormationUnit targetUnit, MBList<IFormationUnit> unitsWithSpaces = null, float? maxDistance = null);

		// Token: 0x06000FF1 RID: 4081
		void OnUnitAddedOrRemoved();

		// Token: 0x06000FF2 RID: 4082
		void SetUnitToFollow(IFormationUnit unit, IFormationUnit toFollow, Vec2 vector);
	}
}
