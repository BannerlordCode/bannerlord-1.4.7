using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017D RID: 381
	public class ArcherPosition
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0004B188 File Offset: 0x00049388
		public GameEntity Entity { get; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0004B190 File Offset: 0x00049390
		public TacticalPosition TacticalArcherPosition { get; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0004B198 File Offset: 0x00049398
		// (set) Token: 0x0600144F RID: 5199 RVA: 0x0004B1A0 File Offset: 0x000493A0
		public int ConnectedSides
		{
			get
			{
				return this._connectedSides;
			}
			private set
			{
				this._connectedSides = value;
			}
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004B1A9 File Offset: 0x000493A9
		public Formation GetLastAssignedFormation(int teamIndex)
		{
			if (teamIndex >= 0)
			{
				return this._lastAssignedFormations[teamIndex];
			}
			return null;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004B1BC File Offset: 0x000493BC
		public ArcherPosition(GameEntity _entity, SiegeQuerySystem siegeQuerySystem, BattleSideEnum battleSide)
		{
			this.Entity = _entity;
			this.TacticalArcherPosition = this.Entity.GetFirstScriptOfType<TacticalPosition>();
			this._siegeQuerySystem = siegeQuerySystem;
			this.DetermineArcherPositionSide(battleSide);
			this._lastAssignedFormations = new Formation[Mission.Current.Teams.Count];
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004B20F File Offset: 0x0004940F
		private static int ConvertToBinaryPow(int pow)
		{
			return 1 << pow;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0004B217 File Offset: 0x00049417
		public bool IsArcherPositionRelatedToSide(FormationAI.BehaviorSide side)
		{
			return (ArcherPosition.ConvertToBinaryPow((int)side) & this.ConnectedSides) != 0;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0004B229 File Offset: 0x00049429
		public FormationAI.BehaviorSide GetArcherPositionClosestSide()
		{
			return this._closestSide;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0004B231 File Offset: 0x00049431
		public void OnDeploymentFinished(SiegeQuerySystem siegeQuerySystem, BattleSideEnum battleSide)
		{
			this._siegeQuerySystem = siegeQuerySystem;
			this.DetermineArcherPositionSide(battleSide);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0004B244 File Offset: 0x00049444
		private void DetermineArcherPositionSide(BattleSideEnum battleSide)
		{
			this.ConnectedSides = 0;
			if (this.TacticalArcherPosition != null)
			{
				int tacticalPositionSide = (int)this.TacticalArcherPosition.TacticalPositionSide;
				if (tacticalPositionSide < 3)
				{
					this._closestSide = this.TacticalArcherPosition.TacticalPositionSide;
					this.ConnectedSides = ArcherPosition.ConvertToBinaryPow(tacticalPositionSide);
				}
			}
			if (this.ConnectedSides == 0)
			{
				if (battleSide == BattleSideEnum.Defender)
				{
					ArcherPosition.CalculateArcherPositionSideUsingDefenderLanes(this._siegeQuerySystem, this.Entity.GlobalPosition, out this._closestSide, out this._connectedSides);
					return;
				}
				ArcherPosition.CalculateArcherPositionSideUsingAttackerRegions(this._siegeQuerySystem, this.Entity.GlobalPosition, out this._closestSide, out this._connectedSides);
			}
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0004B2E0 File Offset: 0x000494E0
		private static void CalculateArcherPositionSideUsingAttackerRegions(SiegeQuerySystem siegeQuerySystem, Vec3 position, out FormationAI.BehaviorSide _closestSide, out int ConnectedSides)
		{
			float num = position.DistanceSquared(siegeQuerySystem.LeftAttackerOrigin);
			float num2 = position.DistanceSquared(siegeQuerySystem.MiddleAttackerOrigin);
			float num3 = position.DistanceSquared(siegeQuerySystem.RightAttackerOrigin);
			FormationAI.BehaviorSide behaviorSide;
			if (num < num2 && num < num3)
			{
				behaviorSide = FormationAI.BehaviorSide.Left;
			}
			else if (num3 < num2)
			{
				behaviorSide = FormationAI.BehaviorSide.Right;
			}
			else
			{
				behaviorSide = FormationAI.BehaviorSide.Middle;
			}
			_closestSide = behaviorSide;
			ConnectedSides = ArcherPosition.ConvertToBinaryPow((int)behaviorSide);
			Vec2 vec = position.AsVec2 - siegeQuerySystem.LeftDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.LeftToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.LeftToMidDir.RightVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(0);
			}
			else
			{
				vec = position.AsVec2 - siegeQuerySystem.MidDefenderOrigin.AsVec2;
				if (vec.DotProduct(siegeQuerySystem.MidToLeftDir) >= 0f && vec.DotProduct(siegeQuerySystem.MidToLeftDir.RightVec()) >= 0f)
				{
					ConnectedSides |= ArcherPosition.ConvertToBinaryPow(0);
				}
			}
			vec = position.AsVec2 - siegeQuerySystem.MidDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.LeftToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.LeftToMidDir.LeftVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(1);
			}
			else
			{
				vec = position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2;
				if (vec.DotProduct(siegeQuerySystem.RightToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.RightToMidDir.RightVec()) >= 0f)
				{
					ConnectedSides |= ArcherPosition.ConvertToBinaryPow(1);
				}
			}
			vec = position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.MidToRightDir) >= 0f && vec.DotProduct(siegeQuerySystem.MidToRightDir.LeftVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(2);
				return;
			}
			vec = position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2;
			if (vec.DotProduct(siegeQuerySystem.RightToMidDir) >= 0f && vec.DotProduct(siegeQuerySystem.RightToMidDir.LeftVec()) >= 0f)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow(2);
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0004B558 File Offset: 0x00049758
		private static void CalculateArcherPositionSideUsingDefenderLanes(SiegeQuerySystem siegeQuerySystem, Vec3 position, out FormationAI.BehaviorSide _closestSide, out int ConnectedSides)
		{
			float num = position.DistanceSquared(siegeQuerySystem.LeftDefenderOrigin);
			float num2 = position.DistanceSquared(siegeQuerySystem.MidDefenderOrigin);
			float num3 = position.DistanceSquared(siegeQuerySystem.RightDefenderOrigin);
			FormationAI.BehaviorSide behaviorSide;
			if (num < num2 && num < num3)
			{
				behaviorSide = FormationAI.BehaviorSide.Left;
			}
			else if (num3 < num2)
			{
				behaviorSide = FormationAI.BehaviorSide.Right;
			}
			else
			{
				behaviorSide = FormationAI.BehaviorSide.Middle;
			}
			FormationAI.BehaviorSide behaviorSide2 = FormationAI.BehaviorSide.BehaviorSideNotSet;
			switch (behaviorSide)
			{
			case FormationAI.BehaviorSide.Left:
				if ((position.AsVec2 - siegeQuerySystem.LeftDefenderOrigin.AsVec2).Normalized().DotProduct(siegeQuerySystem.DefenderLeftToDefenderMidDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			case FormationAI.BehaviorSide.Middle:
				if ((position.AsVec2 - siegeQuerySystem.MidDefenderOrigin.AsVec2).Normalized().DotProduct(siegeQuerySystem.DefenderMidToDefenderRightDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Right;
				}
				else
				{
					behaviorSide2 = FormationAI.BehaviorSide.Left;
				}
				break;
			case FormationAI.BehaviorSide.Right:
				if ((position.AsVec2 - siegeQuerySystem.RightDefenderOrigin.AsVec2).Normalized().DotProduct(siegeQuerySystem.DefenderMidToDefenderRightDir) < 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			}
			_closestSide = behaviorSide;
			ConnectedSides = ArcherPosition.ConvertToBinaryPow((int)behaviorSide);
			if (behaviorSide2 != FormationAI.BehaviorSide.BehaviorSideNotSet)
			{
				ConnectedSides |= ArcherPosition.ConvertToBinaryPow((int)behaviorSide2);
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0004B6A3 File Offset: 0x000498A3
		public void SetLastAssignedFormation(int teamIndex, Formation formation)
		{
			if (teamIndex >= 0)
			{
				this._lastAssignedFormations[teamIndex] = formation;
			}
		}

		// Token: 0x0400056C RID: 1388
		private FormationAI.BehaviorSide _closestSide;

		// Token: 0x0400056D RID: 1389
		private int _connectedSides;

		// Token: 0x0400056E RID: 1390
		private SiegeQuerySystem _siegeQuerySystem;

		// Token: 0x0400056F RID: 1391
		private readonly Formation[] _lastAssignedFormations;
	}
}
