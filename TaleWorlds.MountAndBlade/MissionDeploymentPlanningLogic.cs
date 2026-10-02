using System;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200028E RID: 654
	public abstract class MissionDeploymentPlanningLogic : MissionLogic, IMissionDeploymentPlan
	{
		// Token: 0x06002459 RID: 9305 RVA: 0x0008480C File Offset: 0x00082A0C
		public virtual void Initialize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x00084813 File Offset: 0x00082A13
		public virtual void ClearAll()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0008481A File Offset: 0x00082A1A
		public virtual void MakeDefaultDeploymentPlans()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x00084821 File Offset: 0x00082A21
		public virtual void MakeDeploymentPlan(Team team, float spawnPathOffset = 0f, float targetPathOffset = 0f)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x00084828 File Offset: 0x00082A28
		public virtual bool RemakeDeploymentPlan(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0008482F File Offset: 0x00082A2F
		public virtual void ClearDeploymentPlan(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x00084836 File Offset: 0x00082A36
		public virtual bool IsPlanMade(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0008483D File Offset: 0x00082A3D
		public virtual bool IsPlanMade(Team team, out bool isFirstPlan)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x00084844 File Offset: 0x00082A44
		public virtual bool IsPositionInsideDeploymentBoundaries(Team team, in Vec2 position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0008484B File Offset: 0x00082A4B
		public virtual bool HasDeploymentBoundaries(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x00084852 File Offset: 0x00082A52
		[return: TupleElementNames(new string[] { "id", "points" })]
		public virtual MBReadOnlyList<ValueTuple<string, MBList<Vec2>>> GetDeploymentBoundaries(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x00084859 File Offset: 0x00082A59
		public virtual bool SupportsReinforcements()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x00084860 File Offset: 0x00082A60
		public virtual void UpdateReinforcementPlan(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x00084867 File Offset: 0x00082A67
		public virtual bool SupportsNavmesh(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0008486E File Offset: 0x00082A6E
		public virtual bool HasPlayerSpawnFrame(BattleSideEnum battleSide)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00084875 File Offset: 0x00082A75
		public virtual bool GetPlayerSpawnFrame(BattleSideEnum battleSide, out WorldPosition position, out Vec2 direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0008487C File Offset: 0x00082A7C
		public virtual Vec2 GetClosestDeploymentBoundaryPosition(Team team, in Vec2 position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x00084883 File Offset: 0x00082A83
		public virtual void ProjectPositionToDeploymentBoundaries(Team team, ref WorldPosition position)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0008488A File Offset: 0x00082A8A
		public virtual bool GetPathDeploymentBoundaryIntersection(Team team, in WorldPosition startPosition, in WorldPosition endPosition, out WorldPosition foundPosition)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x00084891 File Offset: 0x00082A91
		public virtual MatrixFrame GetDeploymentFrame(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x00084898 File Offset: 0x00082A98
		public virtual IFormationDeploymentPlan GetFormationPlan(Team team, FormationClass fClass, bool isReinforcement = false)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0008489F File Offset: 0x00082A9F
		public virtual float GetSpawnPathOffset(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000848A6 File Offset: 0x00082AA6
		public virtual MatrixFrame GetZoomFocusFrame(Team team)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000848AD File Offset: 0x00082AAD
		public virtual float GetZoomOffset(Team team, float fovAngle)
		{
			throw new NotImplementedException();
		}
	}
}
