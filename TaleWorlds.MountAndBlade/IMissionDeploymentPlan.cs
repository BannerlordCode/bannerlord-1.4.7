using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000216 RID: 534
	public interface IMissionDeploymentPlan
	{
		// Token: 0x06001F09 RID: 7945
		void Initialize();

		// Token: 0x06001F0A RID: 7946
		void ClearAll();

		// Token: 0x06001F0B RID: 7947
		void MakeDefaultDeploymentPlans();

		// Token: 0x06001F0C RID: 7948
		void MakeDeploymentPlan(Team team, float spawnPathOffset = 0f, float targetOffset = 0f);

		// Token: 0x06001F0D RID: 7949
		bool RemakeDeploymentPlan(Team team);

		// Token: 0x06001F0E RID: 7950
		void ClearDeploymentPlan(Team team);

		// Token: 0x06001F0F RID: 7951
		bool IsPlanMade(Team team);

		// Token: 0x06001F10 RID: 7952
		bool IsPlanMade(Team team, out bool isFirstPlan);

		// Token: 0x06001F11 RID: 7953
		bool IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position);

		// Token: 0x06001F12 RID: 7954
		bool HasDeploymentBoundaries(Team team);

		// Token: 0x06001F13 RID: 7955
		[return: TupleElementNames(new string[] { "id", "points" })]
		MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries(Team team);

		// Token: 0x06001F14 RID: 7956
		bool SupportsReinforcements();

		// Token: 0x06001F15 RID: 7957
		void UpdateReinforcementPlan(Team team);

		// Token: 0x06001F16 RID: 7958
		bool SupportsNavmesh(Team team);

		// Token: 0x06001F17 RID: 7959
		bool HasPlayerSpawnFrame(BattleSideEnum battleSide);

		// Token: 0x06001F18 RID: 7960
		bool GetPlayerSpawnFrame(BattleSideEnum battleSide, out WorldPosition position, out Vec2 direction);

		// Token: 0x06001F19 RID: 7961
		Vec2 GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position);

		// Token: 0x06001F1A RID: 7962
		void ProjectPositionToDeploymentBoundaries(Team team, ref WorldPosition position);

		// Token: 0x06001F1B RID: 7963
		bool GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition intersection);

		// Token: 0x06001F1C RID: 7964
		MatrixFrame GetDeploymentFrame(Team team);

		// Token: 0x06001F1D RID: 7965
		IFormationDeploymentPlan GetFormationPlan(Team team, FormationClass fClass, bool isReinforcement = false);

		// Token: 0x06001F1E RID: 7966
		float GetSpawnPathOffset(Team team);

		// Token: 0x06001F1F RID: 7967
		MatrixFrame GetZoomFocusFrame(Team team);

		// Token: 0x06001F20 RID: 7968
		float GetZoomOffset(Team team, float fovAngle);
	}
}
