using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000180 RID: 384
	public class SiegeLane
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x0004BFE8 File Offset: 0x0004A1E8
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x0004BFF0 File Offset: 0x0004A1F0
		public SiegeLane.LaneStateEnum LaneState { get; private set; }

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0004BFF9 File Offset: 0x0004A1F9
		public FormationAI.BehaviorSide LaneSide { get; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0004C001 File Offset: 0x0004A201
		// (set) Token: 0x06001488 RID: 5256 RVA: 0x0004C009 File Offset: 0x0004A209
		public List<IPrimarySiegeWeapon> PrimarySiegeWeapons { get; private set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0004C012 File Offset: 0x0004A212
		// (set) Token: 0x0600148A RID: 5258 RVA: 0x0004C01A File Offset: 0x0004A21A
		public bool IsOpen { get; private set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0004C023 File Offset: 0x0004A223
		// (set) Token: 0x0600148C RID: 5260 RVA: 0x0004C02B File Offset: 0x0004A22B
		public bool IsBreach { get; private set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0004C034 File Offset: 0x0004A234
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x0004C03C File Offset: 0x0004A23C
		public bool HasGate { get; private set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0004C045 File Offset: 0x0004A245
		// (set) Token: 0x06001490 RID: 5264 RVA: 0x0004C04D File Offset: 0x0004A24D
		public List<ICastleKeyPosition> DefensePoints { get; private set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0004C056 File Offset: 0x0004A256
		// (set) Token: 0x06001492 RID: 5266 RVA: 0x0004C05E File Offset: 0x0004A25E
		public WorldPosition DefenderOrigin { get; private set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0004C067 File Offset: 0x0004A267
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x0004C06F File Offset: 0x0004A26F
		public WorldPosition AttackerOrigin { get; private set; }

		// Token: 0x06001495 RID: 5269 RVA: 0x0004C078 File Offset: 0x0004A278
		public SiegeLane(FormationAI.BehaviorSide laneSide, SiegeQuerySystem siegeQuerySystem)
		{
			this.LaneSide = laneSide;
			this.IsOpen = false;
			this.PrimarySiegeWeapons = new List<IPrimarySiegeWeapon>();
			this.DefensePoints = new List<ICastleKeyPosition>();
			this.IsBreach = false;
			this._siegeQuerySystem = siegeQuerySystem;
			this._lastAssignedFormations = new Formation[Mission.Current.Teams.Count];
			this.HasGate = false;
			this.LaneState = SiegeLane.LaneStateEnum.Active;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0004C0E8 File Offset: 0x0004A2E8
		public bool CalculateIsLaneUnusable()
		{
			if (this.IsOpen)
			{
				return false;
			}
			if (this.HasGate)
			{
				for (int i = 0; i < this.DefensePoints.Count; i++)
				{
					CastleGate castleGate;
					if ((castleGate = this.DefensePoints[i] as CastleGate) != null && castleGate.IsGateOpen && castleGate.GameEntity.HasTag("outer_gate"))
					{
						return false;
					}
				}
			}
			for (int j = 0; j < this.PrimarySiegeWeapons.Count; j++)
			{
				IPrimarySiegeWeapon primarySiegeWeapon = this.PrimarySiegeWeapons[j];
				UsableMachine usableMachine;
				SiegeTower siegeTower;
				BatteringRam batteringRam;
				if (((usableMachine = primarySiegeWeapon as UsableMachine) == null || usableMachine.GameEntity.IsValid) && ((siegeTower = primarySiegeWeapon as SiegeTower) == null || !siegeTower.IsDestroyed) && (primarySiegeWeapon.HasCompletedAction() || (batteringRam = primarySiegeWeapon as BatteringRam) == null || !batteringRam.IsDestroyed))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0004C1C8 File Offset: 0x0004A3C8
		public Formation GetLastAssignedFormation(int teamIndex)
		{
			if (teamIndex >= 0)
			{
				return this._lastAssignedFormations[teamIndex];
			}
			return null;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0004C1D8 File Offset: 0x0004A3D8
		public void SetLaneState(SiegeLane.LaneStateEnum newLaneState)
		{
			this.LaneState = newLaneState;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0004C1E1 File Offset: 0x0004A3E1
		public void SetLastAssignedFormation(int teamIndex, Formation formation)
		{
			if (teamIndex >= 0)
			{
				this._lastAssignedFormations[teamIndex] = formation;
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0004C1F0 File Offset: 0x0004A3F0
		public void SetSiegeQuerySystem(SiegeQuerySystem siegeQuerySystem)
		{
			this._siegeQuerySystem = siegeQuerySystem;
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0004C1FC File Offset: 0x0004A3FC
		public float CalculateLaneCapacity()
		{
			bool flag = false;
			for (int i = 0; i < this.DefensePoints.Count; i++)
			{
				WallSegment wallSegment;
				if ((wallSegment = this.DefensePoints[i] as WallSegment) != null && wallSegment.IsBreachedWall)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return 60f;
			}
			if (this.HasGate)
			{
				bool flag2 = true;
				for (int j = 0; j < this.DefensePoints.Count; j++)
				{
					CastleGate castleGate;
					if ((castleGate = this.DefensePoints[j] as CastleGate) != null && !castleGate.IsGateOpen)
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					return 60f;
				}
			}
			float num = 0f;
			for (int k = 0; k < this.PrimarySiegeWeapons.Count; k++)
			{
				SiegeWeapon siegeWeapon = this.PrimarySiegeWeapons[k] as SiegeWeapon;
				if ((this.PrimarySiegeWeapons[k].HasCompletedAction() || !siegeWeapon.IsDeactivated) && !siegeWeapon.IsDestroyed)
				{
					num += this.PrimarySiegeWeapons[k].SiegeWeaponPriority;
				}
			}
			return num;
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0004C310 File Offset: 0x0004A510
		public SiegeLane.LaneDefenseStates GetDefenseState()
		{
			switch (this.LaneState)
			{
			case SiegeLane.LaneStateEnum.Safe:
			case SiegeLane.LaneStateEnum.Unused:
				return SiegeLane.LaneDefenseStates.Empty;
			case SiegeLane.LaneStateEnum.Used:
			case SiegeLane.LaneStateEnum.Abandoned:
				return SiegeLane.LaneDefenseStates.Token;
			case SiegeLane.LaneStateEnum.Active:
			case SiegeLane.LaneStateEnum.Contested:
			case SiegeLane.LaneStateEnum.Conceited:
				return SiegeLane.LaneDefenseStates.Full;
			default:
				return SiegeLane.LaneDefenseStates.Full;
			}
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0004C350 File Offset: 0x0004A550
		private bool IsPowerBehindLane()
		{
			switch (this.LaneSide)
			{
			case FormationAI.BehaviorSide.Left:
				return this._siegeQuerySystem.LeftRegionMemberCount >= 30;
			case FormationAI.BehaviorSide.Middle:
				return this._siegeQuerySystem.MiddleRegionMemberCount >= 30;
			case FormationAI.BehaviorSide.Right:
				return this._siegeQuerySystem.RightRegionMemberCount >= 30;
			default:
				MBDebug.ShowWarning("Lane without side");
				return false;
			}
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0004C3BC File Offset: 0x0004A5BC
		public bool IsUnderAttack()
		{
			switch (this.LaneSide)
			{
			case FormationAI.BehaviorSide.Left:
				return this._siegeQuerySystem.LeftCloseAttackerCount >= 15;
			case FormationAI.BehaviorSide.Middle:
				return this._siegeQuerySystem.MiddleCloseAttackerCount >= 15;
			case FormationAI.BehaviorSide.Right:
				return this._siegeQuerySystem.RightCloseAttackerCount >= 15;
			default:
				MBDebug.ShowWarning("Lane without side");
				return false;
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0004C428 File Offset: 0x0004A628
		public bool IsDefended()
		{
			switch (this.LaneSide)
			{
			case FormationAI.BehaviorSide.Left:
				return this._siegeQuerySystem.LeftDefenderCount >= 15;
			case FormationAI.BehaviorSide.Middle:
				return this._siegeQuerySystem.MiddleDefenderCount >= 15;
			case FormationAI.BehaviorSide.Right:
				return this._siegeQuerySystem.RightDefenderCount >= 15;
			default:
				MBDebug.ShowWarning("Lane without side");
				return false;
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004C494 File Offset: 0x0004A694
		public void DetermineLaneState()
		{
			if (this.LaneState != SiegeLane.LaneStateEnum.Conceited || this.IsDefended())
			{
				if (this.CalculateIsLaneUnusable())
				{
					this.LaneState = SiegeLane.LaneStateEnum.Safe;
				}
				else if (Mission.Current.IsTeleportingAgents)
				{
					this.LaneState = SiegeLane.LaneStateEnum.Active;
				}
				else if (!this.IsOpen)
				{
					bool flag = true;
					foreach (IPrimarySiegeWeapon primarySiegeWeapon in this.PrimarySiegeWeapons)
					{
						if (!(primarySiegeWeapon is IMoveableSiegeWeapon) || primarySiegeWeapon.HasCompletedAction() || ((SiegeWeapon)primarySiegeWeapon).IsUsed)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						this.LaneState = SiegeLane.LaneStateEnum.Unused;
					}
					else
					{
						this.LaneState = ((!this.IsPowerBehindLane()) ? SiegeLane.LaneStateEnum.Used : SiegeLane.LaneStateEnum.Active);
					}
				}
				else if (!this.IsPowerBehindLane())
				{
					this.LaneState = SiegeLane.LaneStateEnum.Abandoned;
				}
				else
				{
					this.LaneState = ((!this.IsUnderAttack() || this.IsDefended()) ? SiegeLane.LaneStateEnum.Contested : SiegeLane.LaneStateEnum.Conceited);
				}
				if (this.HasGate && this.LaneState < SiegeLane.LaneStateEnum.Active && TeamAISiegeComponent.QuerySystem.InsideAttackerCount >= 15)
				{
					this.LaneState = SiegeLane.LaneStateEnum.Active;
				}
			}
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0004C5BC File Offset: 0x0004A7BC
		public WorldPosition GetCurrentAttackerPosition()
		{
			if (this.IsBreach)
			{
				return this.DefenderOrigin;
			}
			if (this._attackerMovableWeapon != null)
			{
				return this._attackerMovableWeapon.WaitFrame.origin.ToWorldPosition();
			}
			return this.AttackerOrigin;
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0004C5F4 File Offset: 0x0004A7F4
		public void DetermineOrigins()
		{
			this._attackerMovableWeapon = null;
			if (this.IsBreach)
			{
				WallSegment wallSegment = this.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall) as WallSegment;
				this.DefenderOrigin = wallSegment.MiddleFrame.Origin;
				this.AttackerOrigin = wallSegment.AttackerWaitFrame.Origin;
				return;
			}
			this.HasGate = this.DefensePoints.Any<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is CastleGate);
			IEnumerable<IPrimarySiegeWeapon> enumerable;
			if (this.PrimarySiegeWeapons.Count != 0)
			{
				IEnumerable<IPrimarySiegeWeapon> primarySiegeWeapons = this.PrimarySiegeWeapons;
				enumerable = primarySiegeWeapons;
			}
			else
			{
				enumerable = from sw in Mission.Current.MissionObjects.FindAllWithType<SiegeWeapon>().Where<SiegeWeapon>(delegate(SiegeWeapon sw)
					{
						IPrimarySiegeWeapon primarySiegeWeapon;
						return (primarySiegeWeapon = sw as IPrimarySiegeWeapon) != null && primarySiegeWeapon.WeaponSide == this.LaneSide;
					})
					select sw as IPrimarySiegeWeapon;
			}
			IEnumerable<IPrimarySiegeWeapon> enumerable2 = enumerable;
			IMoveableSiegeWeapon moveableSiegeWeapon;
			if ((moveableSiegeWeapon = enumerable2.FirstOrDefault<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw is IMoveableSiegeWeapon) as IMoveableSiegeWeapon) != null)
			{
				this._attackerMovableWeapon = moveableSiegeWeapon as SiegeWeapon;
				this.DefenderOrigin = ((moveableSiegeWeapon as IPrimarySiegeWeapon).TargetCastlePosition as ICastleKeyPosition).MiddleFrame.Origin;
				this.AttackerOrigin = moveableSiegeWeapon.GetInitialFrame().origin.ToWorldPosition();
				return;
			}
			SiegeLadder siegeLadder = enumerable2.FirstOrDefault<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw is SiegeLadder) as SiegeLadder;
			this.DefenderOrigin = (siegeLadder.TargetCastlePosition as ICastleKeyPosition).MiddleFrame.Origin;
			this.AttackerOrigin = siegeLadder.InitialWaitPosition.GetGlobalFrame().origin.ToWorldPosition();
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x0004C7C4 File Offset: 0x0004A9C4
		public void RefreshLane()
		{
			for (int i = this.PrimarySiegeWeapons.Count - 1; i >= 0; i--)
			{
				SiegeWeapon siegeWeapon;
				if ((siegeWeapon = this.PrimarySiegeWeapons[i] as SiegeWeapon) != null && siegeWeapon.IsDisabled)
				{
					this.PrimarySiegeWeapons.RemoveAt(i);
				}
			}
			bool flag = false;
			for (int j = 0; j < this.DefensePoints.Count; j++)
			{
				WallSegment wallSegment;
				if ((wallSegment = this.DefensePoints[j] as WallSegment) != null && wallSegment.IsBreachedWall)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.IsOpen = true;
				this.IsBreach = true;
				return;
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = true;
			for (int k = 0; k < this.DefensePoints.Count; k++)
			{
				ICastleKeyPosition castleKeyPosition = this.DefensePoints[k];
				CastleGate castleGate;
				if (flag4 && (castleGate = castleKeyPosition as CastleGate) != null)
				{
					flag2 = true;
					flag3 = true;
					if (!castleGate.IsDestroyed && castleGate.State != CastleGate.GateState.Open)
					{
						flag4 = false;
						break;
					}
				}
				else if (!flag3 && !(castleKeyPosition is WallSegment))
				{
					flag3 = true;
				}
			}
			bool flag5 = false;
			if (!flag3)
			{
				for (int l = 0; l < this.PrimarySiegeWeapons.Count; l++)
				{
					IPrimarySiegeWeapon primarySiegeWeapon = this.PrimarySiegeWeapons[l];
					if (primarySiegeWeapon.HasCompletedAction() && !(primarySiegeWeapon as UsableMachine).IsDestroyed)
					{
						flag5 = true;
						break;
					}
				}
			}
			this.IsOpen = (flag2 && flag4) || flag5;
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0004C928 File Offset: 0x0004AB28
		public void SetPrimarySiegeWeapons(List<IPrimarySiegeWeapon> primarySiegeWeapons)
		{
			this.PrimarySiegeWeapons = primarySiegeWeapons;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0004C934 File Offset: 0x0004AB34
		public void SetDefensePoints(List<ICastleKeyPosition> defensePoints)
		{
			this.DefensePoints = defensePoints;
			foreach (ICastleKeyPosition castleKeyPosition in this.PrimarySiegeWeapons.Select<IPrimarySiegeWeapon, ICastleKeyPosition>((IPrimarySiegeWeapon psw) => psw.TargetCastlePosition as ICastleKeyPosition))
			{
				if (castleKeyPosition != null && !this.DefensePoints.Contains(castleKeyPosition))
				{
					this.DefensePoints.Add(castleKeyPosition);
				}
			}
		}

		// Token: 0x04000578 RID: 1400
		private readonly Formation[] _lastAssignedFormations;

		// Token: 0x04000579 RID: 1401
		private SiegeQuerySystem _siegeQuerySystem;

		// Token: 0x0400057A RID: 1402
		private SiegeWeapon _attackerMovableWeapon;

		// Token: 0x020004D3 RID: 1235
		public enum LaneStateEnum
		{
			// Token: 0x04001C0D RID: 7181
			Safe,
			// Token: 0x04001C0E RID: 7182
			Unused,
			// Token: 0x04001C0F RID: 7183
			Used,
			// Token: 0x04001C10 RID: 7184
			Active,
			// Token: 0x04001C11 RID: 7185
			Abandoned,
			// Token: 0x04001C12 RID: 7186
			Contested,
			// Token: 0x04001C13 RID: 7187
			Conceited
		}

		// Token: 0x020004D4 RID: 1236
		public enum LaneDefenseStates
		{
			// Token: 0x04001C15 RID: 7189
			Empty,
			// Token: 0x04001C16 RID: 7190
			Token,
			// Token: 0x04001C17 RID: 7191
			Full
		}
	}
}
