using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000142 RID: 322
	public interface IFormationArrangement
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000FF3 RID: 4083
		// (set) Token: 0x06000FF4 RID: 4084
		float Width { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000FF5 RID: 4085
		float Depth { get; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000FF6 RID: 4086
		// (set) Token: 0x06000FF7 RID: 4087
		float FlankWidth { get; set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000FF8 RID: 4088
		float RankDepth { get; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000FF9 RID: 4089
		float MinimumWidth { get; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000FFA RID: 4090
		float MaximumWidth { get; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000FFB RID: 4091
		float MinimumFlankWidth { get; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000FFC RID: 4092
		bool? IsLoose { get; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000FFD RID: 4093
		float IntervalMultiplier { get; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000FFE RID: 4094
		float DistanceMultiplier { get; }

		// Token: 0x06000FFF RID: 4095
		IFormationUnit GetPlayerUnit();

		// Token: 0x06001000 RID: 4096
		MBReadOnlyList<IFormationUnit> GetAllUnits();

		// Token: 0x06001001 RID: 4097
		void GetAllUnits(in MBList<IFormationUnit> allUnitsListToBeFilledIn);

		// Token: 0x06001002 RID: 4098
		MBList<IFormationUnit> GetUnpositionedUnits();

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001003 RID: 4099
		int UnitCount { get; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001004 RID: 4100
		int RankCount { get; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001005 RID: 4101
		int PositionedUnitCount { get; }

		// Token: 0x06001006 RID: 4102
		bool AddUnit(IFormationUnit unit);

		// Token: 0x06001007 RID: 4103
		void RemoveUnit(IFormationUnit unit);

		// Token: 0x06001008 RID: 4104
		IFormationUnit GetUnit(int fileIndex, int rankIndex);

		// Token: 0x06001009 RID: 4105
		void OnBatchRemoveStart();

		// Token: 0x0600100A RID: 4106
		void OnBatchRemoveEnd();

		// Token: 0x0600100B RID: 4107
		Vec2? GetLocalPositionOfUnitOrDefault(int unitIndex);

		// Token: 0x0600100C RID: 4108
		Vec2? GetLocalPositionOfUnitOrDefault(IFormationUnit unit);

		// Token: 0x0600100D RID: 4109
		Vec2? GetLocalPositionOfUnitOrDefaultWithAdjustment(IFormationUnit unit, float distanceBetweenAgentsAdjustment);

		// Token: 0x0600100E RID: 4110
		Vec2? GetLocalDirectionOfUnitOrDefault(int unitIndex);

		// Token: 0x0600100F RID: 4111
		Vec2? GetLocalDirectionOfUnitOrDefault(IFormationUnit unit);

		// Token: 0x06001010 RID: 4112
		WorldPosition? GetWorldPositionOfUnitOrDefault(int unitIndex);

		// Token: 0x06001011 RID: 4113
		WorldPosition? GetWorldPositionOfUnitOrDefault(IFormationUnit unit);

		// Token: 0x06001012 RID: 4114
		List<IFormationUnit> GetUnitsToPop(int count);

		// Token: 0x06001013 RID: 4115
		List<IFormationUnit> GetUnitsToPop(int count, Vec3 targetPosition);

		// Token: 0x06001014 RID: 4116
		IEnumerable<IFormationUnit> GetUnitsToPopWithCondition(int count, Func<IFormationUnit, bool> conditionFunction);

		// Token: 0x06001015 RID: 4117
		void SwitchUnitLocations(IFormationUnit firstUnit, IFormationUnit secondUnit);

		// Token: 0x06001016 RID: 4118
		void SwitchUnitLocationsWithUnpositionedUnit(IFormationUnit firstUnit, IFormationUnit secondUnit);

		// Token: 0x06001017 RID: 4119
		void SwitchUnitLocationsWithBackMostUnit(IFormationUnit unit);

		// Token: 0x06001018 RID: 4120
		IFormationUnit GetNeighborUnitOfLeftSide(IFormationUnit unit);

		// Token: 0x06001019 RID: 4121
		IFormationUnit GetNeighborUnitOfRightSide(IFormationUnit unit);

		// Token: 0x0600101A RID: 4122
		Vec2? GetLocalWallDirectionOfRelativeFormationLocation(IFormationUnit unit);

		// Token: 0x0600101B RID: 4123
		IEnumerable<Vec2> GetUnavailableUnitPositions();

		// Token: 0x0600101C RID: 4124
		float GetOccupationWidth(int unitCount);

		// Token: 0x0600101D RID: 4125
		Vec2? CreateNewPosition(int unitIndex);

		// Token: 0x0600101E RID: 4126
		void BeforeFormationFrameChange();

		// Token: 0x0600101F RID: 4127
		void OnFormationFrameChanged(bool updateCachedOrderedLocalPositions = false);

		// Token: 0x06001020 RID: 4128
		bool IsTurnBackwardsNecessary(Vec2 previousPosition, WorldPosition? newPosition, Vec2 previousDirection, bool hasNewDirection, Vec2? newDirection);

		// Token: 0x06001021 RID: 4129
		void TurnBackwards();

		// Token: 0x06001022 RID: 4130
		void OnFormationDispersed();

		// Token: 0x06001023 RID: 4131
		void Reset();

		// Token: 0x06001024 RID: 4132
		IFormationArrangement Clone(IFormation formation);

		// Token: 0x06001025 RID: 4133
		void DeepCopyFrom(IFormationArrangement arrangement);

		// Token: 0x06001026 RID: 4134
		void RearrangeTo(IFormationArrangement arrangement);

		// Token: 0x06001027 RID: 4135
		void RearrangeFrom(IFormationArrangement arrangement);

		// Token: 0x06001028 RID: 4136
		void RearrangeTransferUnits(IFormationArrangement arrangement);

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001029 RID: 4137
		// (remove) Token: 0x0600102A RID: 4138
		event Action OnWidthChanged;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600102B RID: 4139
		// (remove) Token: 0x0600102C RID: 4140
		event Action OnShapeChanged;

		// Token: 0x0600102D RID: 4141
		void ReserveMiddleFrontUnitPosition(IFormationUnit vanguard);

		// Token: 0x0600102E RID: 4142
		void ReleaseMiddleFrontUnitPosition();

		// Token: 0x0600102F RID: 4143
		Vec2 GetLocalPositionOfReservedUnitPosition();

		// Token: 0x06001030 RID: 4144
		void OnUnitLostMount(IFormationUnit unit);

		// Token: 0x06001031 RID: 4145
		float GetDirectionChangeTendencyOfUnit(IFormationUnit unit);

		// Token: 0x06001032 RID: 4146
		void UpdateLocalPositionErrors(bool recalculateErrors = true);

		// Token: 0x06001033 RID: 4147
		void OnTickOccasionally();

		// Token: 0x170003A3 RID: 931
		// (set) Token: 0x06001034 RID: 4148
		bool AreLocalPositionsDirty { set; }
	}
}
