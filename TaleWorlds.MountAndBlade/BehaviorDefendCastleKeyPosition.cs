using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000111 RID: 273
	public class BehaviorDefendCastleKeyPosition : BehaviorComponent
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x0001D327 File Offset: 0x0001B527
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0001D32E File Offset: 0x0001B52E
		public BehaviorDefendCastleKeyPosition(Formation formation)
			: base(formation)
		{
			this._teamAISiegeDefender = formation.Team.TeamAI as TeamAISiegeComponent;
			this._behaviorState = BehaviorDefendCastleKeyPosition.BehaviorState.UnSet;
			this._laddersOnThisSide = new List<SiegeLadder>();
			this.ResetOrderPositions();
			this._hasFormedShieldWall = true;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0001D36C File Offset: 0x0001B56C
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyOrder : this._waitOrder);
			this.CurrentFacingOrder = ((base.Formation.CachedClosestEnemyFormation != null && TeamAISiegeComponent.IsFormationInsideCastle(base.Formation.CachedClosestEnemyFormation.Formation, true, 0.4f)) ? FacingOrder.FacingOrderLookAtEnemy : ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyFacingOrder : this._waitFacingOrder));
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0001D3EC File Offset: 0x0001B5EC
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0001D448 File Offset: 0x0001B648
		private void ResetOrderPositions()
		{
			this._behaviorSide = base.Formation.AI.Side;
			this._innerGate = null;
			this._outerGate = null;
			this._laddersOnThisSide.Clear();
			WorldFrame worldFrame;
			WorldFrame worldFrame2;
			if (this._teamAISiegeDefender.OuterGate.DefenseSide == this._behaviorSide)
			{
				CastleGate outerGate = this._teamAISiegeDefender.OuterGate;
				this._innerGate = this._teamAISiegeDefender.InnerGate;
				this._outerGate = this._teamAISiegeDefender.OuterGate;
				worldFrame = outerGate.MiddleFrame;
				worldFrame2 = outerGate.DefenseWaitFrame;
				this._tacticalMiddlePos = outerGate.MiddlePosition;
				this._tacticalWaitPos = outerGate.WaitPosition;
			}
			else
			{
				WallSegment wallSegment = this._teamAISiegeDefender.WallSegments.Where<WallSegment>((WallSegment ws) => ws.DefenseSide == this._behaviorSide && ws.IsBreachedWall).FirstOrDefault<WallSegment>();
				if (wallSegment != null)
				{
					worldFrame = wallSegment.MiddleFrame;
					worldFrame2 = wallSegment.DefenseWaitFrame;
					this._tacticalMiddlePos = wallSegment.MiddlePosition;
					this._tacticalWaitPos = wallSegment.WaitPosition;
				}
				else
				{
					IEnumerable<IPrimarySiegeWeapon> enumerable = this._teamAISiegeDefender.PrimarySiegeWeapons.Where<IPrimarySiegeWeapon>(delegate(IPrimarySiegeWeapon sw)
					{
						SiegeWeapon siegeWeapon;
						return sw.WeaponSide == this._behaviorSide && (((siegeWeapon = sw as SiegeWeapon) != null && !siegeWeapon.IsDestroyed && !siegeWeapon.IsDeactivated) || sw.HasCompletedAction());
					});
					if (!enumerable.Any<IPrimarySiegeWeapon>())
					{
						worldFrame = WorldFrame.Invalid;
						worldFrame2 = WorldFrame.Invalid;
						this._tacticalMiddlePos = null;
						this._tacticalWaitPos = null;
					}
					else
					{
						this._laddersOnThisSide = enumerable.OfType<SiegeLadder>().ToList<SiegeLadder>();
						ICastleKeyPosition castleKeyPosition = enumerable.FirstOrDefault<IPrimarySiegeWeapon>().TargetCastlePosition as ICastleKeyPosition;
						worldFrame = castleKeyPosition.MiddleFrame;
						worldFrame2 = castleKeyPosition.DefenseWaitFrame;
						this._tacticalMiddlePos = castleKeyPosition.MiddlePosition;
						this._tacticalWaitPos = castleKeyPosition.WaitPosition;
					}
				}
			}
			if (this._tacticalMiddlePos != null)
			{
				this._readyOrderPosition = this._tacticalMiddlePos.Position;
				this._readyOrder = MovementOrder.MovementOrderMove(this._readyOrderPosition);
				this._readyFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._tacticalMiddlePos.Direction);
			}
			else if (worldFrame.Origin.IsValid)
			{
				worldFrame.Rotation.f.Normalize();
				this._readyOrderPosition = worldFrame.Origin;
				this._readyOrder = MovementOrder.MovementOrderMove(this._readyOrderPosition);
				this._readyFacingOrder = FacingOrder.FacingOrderLookAtDirection(worldFrame.Rotation.f.AsVec2);
			}
			else
			{
				this._readyOrderPosition = WorldPosition.Invalid;
				this._readyOrder = MovementOrder.MovementOrderStop;
				this._readyFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			if (this._tacticalWaitPos != null)
			{
				this._waitOrder = MovementOrder.MovementOrderMove(this._tacticalWaitPos.Position);
				this._waitFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._tacticalWaitPos.Direction);
			}
			else if (worldFrame2.Origin.IsValid)
			{
				worldFrame2.Rotation.f.Normalize();
				this._waitOrder = MovementOrder.MovementOrderMove(worldFrame2.Origin);
				this._waitFacingOrder = FacingOrder.FacingOrderLookAtDirection(worldFrame2.Rotation.f.AsVec2);
			}
			else
			{
				this._waitOrder = MovementOrder.MovementOrderStop;
				this._waitFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			base.CurrentOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyOrder : this._waitOrder);
			this.CurrentFacingOrder = ((base.Formation.CachedClosestEnemyFormation != null && TeamAISiegeComponent.IsFormationInsideCastle(base.Formation.CachedClosestEnemyFormation.Formation, true, 0.4f)) ? FacingOrder.FacingOrderLookAtEnemy : ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyFacingOrder : this._waitFacingOrder));
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0001D79D File Offset: 0x0001B99D
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			bool flag = false;
			if (this._teamAISiegeDefender != null && !base.Formation.IsDeployment)
			{
				for (int i = 0; i < TeamAISiegeComponent.SiegeLanes.Count; i++)
				{
					SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes[i];
					if (siegeLane.LaneSide == this._behaviorSide)
					{
						if (siegeLane.IsOpen)
						{
							flag = true;
						}
						else
						{
							for (int j = 0; j < siegeLane.PrimarySiegeWeapons.Count; j++)
							{
								IPrimarySiegeWeapon primarySiegeWeapon = siegeLane.PrimarySiegeWeapons[j];
								SiegeLadder siegeLadder;
								if ((siegeLadder = primarySiegeWeapon as SiegeLadder) != null)
								{
									if (siegeLadder.IsUsed)
									{
										flag = true;
										break;
									}
								}
								else if ((primarySiegeWeapon as SiegeWeapon).GetComponent<SiegeWeaponMovementComponent>().HasApproachedTarget)
								{
									flag = true;
									break;
								}
							}
						}
					}
				}
			}
			BehaviorDefendCastleKeyPosition.BehaviorState behaviorState = (flag ? BehaviorDefendCastleKeyPosition.BehaviorState.Ready : BehaviorDefendCastleKeyPosition.BehaviorState.Waiting);
			bool flag2 = false;
			if (behaviorState != this._behaviorState)
			{
				this._behaviorState = behaviorState;
				base.CurrentOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyOrder : this._waitOrder);
				this.CurrentFacingOrder = ((this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready) ? this._readyFacingOrder : this._waitFacingOrder);
				flag2 = true;
			}
			if (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.Siege)
			{
				if (this._outerGate != null && this._outerGate.State == CastleGate.GateState.Open && !this._outerGate.IsDestroyed)
				{
					if (!this._outerGate.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(this._outerGate, false);
					}
				}
				else if (this._innerGate != null && this._innerGate.State == CastleGate.GateState.Open && !this._innerGate.IsDestroyed && !this._innerGate.IsUsedByFormation(base.Formation))
				{
					base.Formation.StartUsingMachine(this._innerGate, false);
				}
				foreach (SiegeLadder siegeLadder2 in this._laddersOnThisSide)
				{
					if (!siegeLadder2.IsDisabledForBattleSide(BattleSideEnum.Defender) && !siegeLadder2.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(siegeLadder2, false);
					}
				}
			}
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready && this._tacticalMiddlePos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalMiddlePos.Width), true);
			}
			else if (this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Waiting && this._tacticalWaitPos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalWaitPos.Width), true);
			}
			if (flag2 || !this._hasFormedShieldWall)
			{
				bool flag3;
				if (this._behaviorState == BehaviorDefendCastleKeyPosition.BehaviorState.Ready && this._readyOrderPosition.IsValid)
				{
					Vec3 navMeshVec = base.Formation.CachedMedianPosition.GetNavMeshVec3();
					flag3 = this._readyOrderPosition.DistanceSquaredWithLimit(in navMeshVec, MathF.Min(base.Formation.Depth, base.Formation.Width) * 1.2f) <= (this._hasFormedShieldWall ? (MathF.Min(base.Formation.Depth, base.Formation.Width) * MathF.Min(base.Formation.Depth, base.Formation.Width)) : (MathF.Min(base.Formation.Depth, base.Formation.Width) * MathF.Min(base.Formation.Depth, base.Formation.Width) * 0.25f));
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (flag4 != this._hasFormedShieldWall)
				{
					this._hasFormedShieldWall = flag4;
					base.Formation.SetArrangementOrder(this._hasFormedShieldWall ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLine);
				}
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0001DB84 File Offset: 0x0001BD84
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this.ResetOrderPositions();
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0001DB92 File Offset: 0x0001BD92
		public override void ResetBehavior()
		{
			base.ResetBehavior();
			this.ResetOrderPositions();
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0001DBA0 File Offset: 0x0001BDA0
		protected override void OnBehaviorActivatedAux()
		{
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			this._hasFormedShieldWall = true;
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0001DC0D File Offset: 0x0001BE0D
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x04000341 RID: 833
		private TeamAISiegeComponent _teamAISiegeDefender;

		// Token: 0x04000342 RID: 834
		private CastleGate _innerGate;

		// Token: 0x04000343 RID: 835
		private CastleGate _outerGate;

		// Token: 0x04000344 RID: 836
		private List<SiegeLadder> _laddersOnThisSide;

		// Token: 0x04000345 RID: 837
		private BehaviorDefendCastleKeyPosition.BehaviorState _behaviorState;

		// Token: 0x04000346 RID: 838
		private MovementOrder _waitOrder;

		// Token: 0x04000347 RID: 839
		private MovementOrder _readyOrder;

		// Token: 0x04000348 RID: 840
		private FacingOrder _waitFacingOrder;

		// Token: 0x04000349 RID: 841
		private FacingOrder _readyFacingOrder;

		// Token: 0x0400034A RID: 842
		private TacticalPosition _tacticalMiddlePos;

		// Token: 0x0400034B RID: 843
		private TacticalPosition _tacticalWaitPos;

		// Token: 0x0400034C RID: 844
		private bool _hasFormedShieldWall;

		// Token: 0x0400034D RID: 845
		private WorldPosition _readyOrderPosition;

		// Token: 0x02000433 RID: 1075
		private enum BehaviorState
		{
			// Token: 0x0400194D RID: 6477
			UnSet,
			// Token: 0x0400194E RID: 6478
			Waiting,
			// Token: 0x0400194F RID: 6479
			Ready
		}
	}
}
