using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018A RID: 394
	public abstract class TeamAISiegeComponent : TeamAIComponent
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0004E1D4 File Offset: 0x0004C3D4
		// (set) Token: 0x060014EE RID: 5358 RVA: 0x0004E1DB File Offset: 0x0004C3DB
		public static List<SiegeLane> SiegeLanes { get; private set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0004E1E3 File Offset: 0x0004C3E3
		// (set) Token: 0x060014F0 RID: 5360 RVA: 0x0004E1EA File Offset: 0x0004C3EA
		public static SiegeQuerySystem QuerySystem { get; protected set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0004E1F2 File Offset: 0x0004C3F2
		public CastleGate OuterGate { get; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0004E1FA File Offset: 0x0004C3FA
		public List<IPrimarySiegeWeapon> PrimarySiegeWeapons { get; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0004E202 File Offset: 0x0004C402
		public CastleGate InnerGate { get; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0004E20A File Offset: 0x0004C40A
		public MBReadOnlyList<SiegeLadder> Ladders
		{
			get
			{
				return this._ladders;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0004E212 File Offset: 0x0004C412
		// (set) Token: 0x060014F6 RID: 5366 RVA: 0x0004E21A File Offset: 0x0004C41A
		public bool AreLaddersReady { get; private set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x0004E223 File Offset: 0x0004C423
		// (set) Token: 0x060014F8 RID: 5368 RVA: 0x0004E22B File Offset: 0x0004C42B
		public List<int> DifficultNavmeshIDs { get; private set; }

		// Token: 0x060014F9 RID: 5369 RVA: 0x0004E234 File Offset: 0x0004C434
		protected TeamAISiegeComponent(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
			: base(currentMission, currentTeam, thinkTimerTime, applyTimerTime)
		{
			this.CastleGates = currentMission.ActiveMissionObjects.FindAllWithType<CastleGate>().ToList<CastleGate>();
			this.WallSegments = currentMission.ActiveMissionObjects.FindAllWithType<WallSegment>().ToList<WallSegment>();
			this.OuterGate = this.CastleGates.FirstOrDefault<CastleGate>((CastleGate g) => g.GameEntity.HasTag("outer_gate"));
			this.InnerGate = this.CastleGates.FirstOrDefault<CastleGate>((CastleGate g) => g.GameEntity.HasTag("inner_gate"));
			this.SceneSiegeWeapons = Mission.Current.MissionObjects.FindAllWithType<SiegeWeapon>().ToList<SiegeWeapon>();
			this._ladders = this.SceneSiegeWeapons.OfType<SiegeLadder>().ToMBList<SiegeLadder>();
			this.Ram = this.SceneSiegeWeapons.FirstOrDefault<SiegeWeapon>((SiegeWeapon ssw) => ssw is BatteringRam) as BatteringRam;
			this.SiegeTowers = this.SceneSiegeWeapons.OfType<SiegeTower>().ToList<SiegeTower>();
			this.PrimarySiegeWeapons = new List<IPrimarySiegeWeapon>();
			this.PrimarySiegeWeapons.AddRange(this._ladders);
			if (this.Ram != null)
			{
				this.PrimarySiegeWeapons.Add(this.Ram);
			}
			this.PrimarySiegeWeapons.AddRange(this.SiegeTowers);
			this.PrimarySiegeWeaponNavMeshFaceIDs = new HashSet<int>();
			using (List<IPrimarySiegeWeapon>.Enumerator enumerator = this.PrimarySiegeWeapons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IPrimarySiegeWeapon primarySiegeWeapon;
					List<int> list;
					if ((primarySiegeWeapon = enumerator.Current) != null && primarySiegeWeapon.GetNavmeshFaceIds(out list))
					{
						this.PrimarySiegeWeaponNavMeshFaceIDs.UnionWith(list);
					}
				}
			}
			this.CastleKeyPositions = new List<MissionObject>();
			this.CastleKeyPositions.AddRange(this.CastleGates);
			this.CastleKeyPositions.AddRange(this.WallSegments);
			TeamAISiegeComponent.SiegeLanes = new List<SiegeLane>();
			int i;
			int j;
			for (i = 0; i < 3; i = j + 1)
			{
				TeamAISiegeComponent.SiegeLanes.Add(new SiegeLane((FormationAI.BehaviorSide)i, TeamAISiegeComponent.QuerySystem));
				TeamAISiegeComponent.SiegeLanes[i].SetPrimarySiegeWeapons((from psw in this.PrimarySiegeWeapons
					where psw.WeaponSide == (FormationAI.BehaviorSide)i
					select psw into um
					select (um)).ToList<IPrimarySiegeWeapon>());
				TeamAISiegeComponent.SiegeLanes[i].SetDefensePoints((from ckp in this.CastleKeyPositions
					where ((ICastleKeyPosition)ckp).DefenseSide == (FormationAI.BehaviorSide)i
					select ckp into dp
					select (ICastleKeyPosition)dp).ToList<ICastleKeyPosition>());
				TeamAISiegeComponent.SiegeLanes[i].RefreshLane();
				j = i;
			}
			TeamAISiegeComponent.SiegeLanes.ForEach(delegate(SiegeLane sl)
			{
				sl.SetSiegeQuerySystem(TeamAISiegeComponent.QuerySystem);
			});
			this.DifficultNavmeshIDs = new List<int>();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004E570 File Offset: 0x0004C770
		protected internal override void Tick(float dt)
		{
			if (!this._noProperLaneRemains)
			{
				int num = 0;
				SiegeLane siegeLane = null;
				foreach (SiegeLane siegeLane2 in TeamAISiegeComponent.SiegeLanes)
				{
					siegeLane2.RefreshLane();
					siegeLane2.DetermineLaneState();
					if (siegeLane2.IsBreach)
					{
						num++;
					}
					else
					{
						siegeLane = siegeLane2;
					}
				}
				if (siegeLane != null && num >= 2 && !siegeLane.IsOpen && siegeLane.LaneState >= SiegeLane.LaneStateEnum.Used)
				{
					siegeLane.SetLaneState(SiegeLane.LaneStateEnum.Unused);
				}
				if (TeamAISiegeComponent.SiegeLanes.Count != 0)
				{
					goto IL_01D0;
				}
				this._noProperLaneRemains = true;
				using (IEnumerator<FormationAI.BehaviorSide> enumerator2 = (from ckp in this.CastleKeyPositions.Where<MissionObject>(delegate(MissionObject ckp)
					{
						CastleGate castleGate;
						return (castleGate = ckp as CastleGate) != null && castleGate.DefenseSide != FormationAI.BehaviorSide.BehaviorSideNotSet;
					})
					select ((CastleGate)ckp).DefenseSide).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						FormationAI.BehaviorSide difficultLaneSide = enumerator2.Current;
						SiegeLane siegeLane3 = new SiegeLane(difficultLaneSide, TeamAISiegeComponent.QuerySystem);
						siegeLane3.SetPrimarySiegeWeapons(new List<IPrimarySiegeWeapon>());
						siegeLane3.SetDefensePoints((from ckp in this.CastleKeyPositions
							where ((ICastleKeyPosition)ckp).DefenseSide == difficultLaneSide && ckp is CastleGate
							select ckp into dp
							select dp as ICastleKeyPosition).ToList<ICastleKeyPosition>());
						siegeLane3.RefreshLane();
						siegeLane3.DetermineLaneState();
						TeamAISiegeComponent.SiegeLanes.Add(siegeLane3);
					}
					goto IL_01D0;
				}
			}
			foreach (SiegeLane siegeLane4 in TeamAISiegeComponent.SiegeLanes)
			{
				siegeLane4.RefreshLane();
				siegeLane4.DetermineLaneState();
			}
			IL_01D0:
			base.Tick(dt);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004E77C File Offset: 0x0004C97C
		public static void OnMissionFinalize()
		{
			if (TeamAISiegeComponent.SiegeLanes != null)
			{
				TeamAISiegeComponent.SiegeLanes.Clear();
				TeamAISiegeComponent.SiegeLanes = null;
			}
			TeamAISiegeComponent.QuerySystem = null;
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004E79C File Offset: 0x0004C99C
		public bool CalculateIsChargePastWallsApplicable(FormationAI.BehaviorSide side)
		{
			if (Mission.Current.MissionTeamAIType == Mission.MissionTeamAITypeEnum.SallyOut)
			{
				return false;
			}
			if (side == FormationAI.BehaviorSide.BehaviorSideNotSet && this.InnerGate != null && !this.InnerGate.IsGateOpen)
			{
				return false;
			}
			foreach (SiegeLane siegeLane in TeamAISiegeComponent.SiegeLanes)
			{
				if (side == FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					if (!siegeLane.IsOpen)
					{
						return false;
					}
				}
				else if (side == siegeLane.LaneSide)
				{
					return siegeLane.IsOpen && (siegeLane.IsBreach || (siegeLane.HasGate && (this.InnerGate == null || this.InnerGate.IsGateOpen)));
				}
			}
			return true;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004E868 File Offset: 0x0004CA68
		public void SetAreLaddersReady(bool areLaddersReady)
		{
			this.AreLaddersReady = areLaddersReady;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004E871 File Offset: 0x0004CA71
		public bool CalculateIsAnyLaneOpenToGetInside()
		{
			return TeamAISiegeComponent.SiegeLanes.Any<SiegeLane>((SiegeLane sl) => sl.IsOpen);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0004E89C File Offset: 0x0004CA9C
		public bool CalculateIsAnyLaneOpenToGoOutside()
		{
			return TeamAISiegeComponent.SiegeLanes.Any<SiegeLane>(delegate(SiegeLane sl)
			{
				if (!sl.IsOpen)
				{
					return false;
				}
				if (!sl.IsBreach && !sl.HasGate)
				{
					return sl.PrimarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw is SiegeTower);
				}
				return true;
			});
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0004E8C7 File Offset: 0x0004CAC7
		public bool IsPrimarySiegeWeaponNavmeshFaceId(int id)
		{
			return this.PrimarySiegeWeaponNavMeshFaceIDs.Contains(id);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0004E8D8 File Offset: 0x0004CAD8
		public static bool IsFormationGroupInsideCastle(MBList<Formation> formationGroup, bool includeOnlyPositionedUnits, float thresholdPercentage = 0.4f)
		{
			int num = 0;
			foreach (Formation formation in formationGroup)
			{
				num += (includeOnlyPositionedUnits ? formation.Arrangement.PositionedUnitCount : formation.CountOfUnits);
			}
			float num2 = (float)num * thresholdPercentage;
			foreach (Formation formation2 in formationGroup)
			{
				if (formation2.CountOfUnits > 0)
				{
					num2 -= (float)formation2.CountUnitsOnNavMeshIDMod10(1, includeOnlyPositionedUnits);
					if (num2 <= 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0004E9A0 File Offset: 0x0004CBA0
		public static bool IsFormationInsideCastle(Formation formation, bool includeOnlyPositionedUnits, float thresholdPercentage = 0.4f)
		{
			int num = (includeOnlyPositionedUnits ? formation.Arrangement.PositionedUnitCount : formation.CountOfUnits);
			float num2 = (float)num * thresholdPercentage;
			if (num == 0)
			{
				return !(formation.Team.TeamAI is TeamAISiegeAttacker) && !(formation.Team.TeamAI is TeamAISallyOutDefender) && (formation.Team.TeamAI is TeamAISiegeDefender || formation.Team.TeamAI is TeamAISallyOutAttacker);
			}
			if (includeOnlyPositionedUnits)
			{
				return (float)formation.QuerySystem.InsideCastleUnitCountPositioned >= num2;
			}
			return (float)formation.QuerySystem.InsideCastleUnitCountIncludingUnpositioned >= num2;
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0004EA40 File Offset: 0x0004CC40
		public bool IsCastleBreached()
		{
			int num = 0;
			int num2 = 0;
			foreach (Formation formation in this.Mission.AttackerTeam.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					num2++;
					if (TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
					{
						num++;
					}
				}
			}
			if (this.Mission.AttackerAllyTeam != null)
			{
				foreach (Formation formation2 in this.Mission.AttackerAllyTeam.FormationsIncludingSpecialAndEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						num2++;
						if (TeamAISiegeComponent.IsFormationInsideCastle(formation2, true, 0.4f))
						{
							num++;
						}
					}
				}
			}
			return (float)num >= (float)num2 * 0.7f;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0004EB3C File Offset: 0x0004CD3C
		public override void OnDeploymentFinished()
		{
			foreach (SiegeLadder siegeLadder in this._ladders.Where<SiegeLadder>((SiegeLadder l) => !l.IsDisabled))
			{
				this.DifficultNavmeshIDs.Add(siegeLadder.OnWallNavMeshId);
			}
			foreach (SiegeTower siegeTower in this.SiegeTowers)
			{
				this.DifficultNavmeshIDs.AddRange(siegeTower.CollectGetDifficultNavmeshIDs());
			}
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				formation.OnDeploymentFinished();
			}
			foreach (SiegeWeapon siegeWeapon in this.SceneSiegeWeapons)
			{
				if (!siegeWeapon.IsDisabled && siegeWeapon.Side == this.Team.Side)
				{
					siegeWeapon.OnDeploymentFinished();
				}
			}
		}

		// Token: 0x040005AC RID: 1452
		public const int InsideCastleNavMeshID = 1;

		// Token: 0x040005AD RID: 1453
		public const int SiegeTokenForceSize = 15;

		// Token: 0x040005AE RID: 1454
		private const float FormationInsideCastleThresholdPercentage = 0.4f;

		// Token: 0x040005AF RID: 1455
		private const float CastleBreachThresholdPercentage = 0.7f;

		// Token: 0x040005B2 RID: 1458
		public readonly IEnumerable<WallSegment> WallSegments;

		// Token: 0x040005B3 RID: 1459
		public readonly List<SiegeWeapon> SceneSiegeWeapons;

		// Token: 0x040005B4 RID: 1460
		protected readonly IEnumerable<CastleGate> CastleGates;

		// Token: 0x040005B5 RID: 1461
		protected readonly List<SiegeTower> SiegeTowers;

		// Token: 0x040005B6 RID: 1462
		protected readonly HashSet<int> PrimarySiegeWeaponNavMeshFaceIDs;

		// Token: 0x040005B7 RID: 1463
		protected BatteringRam Ram;

		// Token: 0x040005B8 RID: 1464
		protected List<MissionObject> CastleKeyPositions;

		// Token: 0x040005B9 RID: 1465
		private readonly MBList<SiegeLadder> _ladders;

		// Token: 0x040005BA RID: 1466
		private bool _noProperLaneRemains;
	}
}
