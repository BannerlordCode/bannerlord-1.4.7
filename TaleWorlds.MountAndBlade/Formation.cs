using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021F RID: 543
	public sealed class Formation : IFormation
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06001F74 RID: 8052 RVA: 0x0006CDB4 File Offset: 0x0006AFB4
		// (remove) Token: 0x06001F75 RID: 8053 RVA: 0x0006CDEC File Offset: 0x0006AFEC
		public event Action<Formation, Agent> OnUnitAdded;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06001F76 RID: 8054 RVA: 0x0006CE24 File Offset: 0x0006B024
		// (remove) Token: 0x06001F77 RID: 8055 RVA: 0x0006CE5C File Offset: 0x0006B05C
		public event Action<Formation, Agent> OnUnitRemoved;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06001F78 RID: 8056 RVA: 0x0006CE94 File Offset: 0x0006B094
		// (remove) Token: 0x06001F79 RID: 8057 RVA: 0x0006CECC File Offset: 0x0006B0CC
		public event Action<Formation, Agent> OnUnitAttached;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06001F7A RID: 8058 RVA: 0x0006CF04 File Offset: 0x0006B104
		// (remove) Token: 0x06001F7B RID: 8059 RVA: 0x0006CF3C File Offset: 0x0006B13C
		public event Action<Formation> OnUnitCountChanged;

		// Token: 0x14000029 RID: 41
		// (add) Token: 0x06001F7C RID: 8060 RVA: 0x0006CF74 File Offset: 0x0006B174
		// (remove) Token: 0x06001F7D RID: 8061 RVA: 0x0006CFAC File Offset: 0x0006B1AC
		public event Action<Formation> OnUnitSpacingChanged;

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06001F7E RID: 8062 RVA: 0x0006CFE4 File Offset: 0x0006B1E4
		// (remove) Token: 0x06001F7F RID: 8063 RVA: 0x0006D01C File Offset: 0x0006B21C
		public event Action<Formation> OnTick;

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06001F80 RID: 8064 RVA: 0x0006D054 File Offset: 0x0006B254
		// (remove) Token: 0x06001F81 RID: 8065 RVA: 0x0006D08C File Offset: 0x0006B28C
		public event Action<Formation> OnWidthChanged;

		// Token: 0x1400002C RID: 44
		// (add) Token: 0x06001F82 RID: 8066 RVA: 0x0006D0C4 File Offset: 0x0006B2C4
		// (remove) Token: 0x06001F83 RID: 8067 RVA: 0x0006D0FC File Offset: 0x0006B2FC
		public event Action<Formation, MovementOrder.MovementOrderEnum> OnBeforeMovementOrderApplied;

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06001F84 RID: 8068 RVA: 0x0006D134 File Offset: 0x0006B334
		// (remove) Token: 0x06001F85 RID: 8069 RVA: 0x0006D16C File Offset: 0x0006B36C
		public event Action<Formation, ArrangementOrder.ArrangementOrderEnum> OnAfterArrangementOrderApplied;

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x0006D1A1 File Offset: 0x0006B3A1
		// (set) Token: 0x06001F87 RID: 8071 RVA: 0x0006D1A9 File Offset: 0x0006B3A9
		public Formation.RetreatPositionCacheSystem RetreatPositionCache { get; private set; } = new Formation.RetreatPositionCacheSystem(2);

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x0006D1B2 File Offset: 0x0006B3B2
		// (set) Token: 0x06001F89 RID: 8073 RVA: 0x0006D1BA File Offset: 0x0006B3BA
		public FormationClass RepresentativeClass { get; private set; } = FormationClass.NumberOfAllFormations;

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x0006D1C3 File Offset: 0x0006B3C3
		// (set) Token: 0x06001F8B RID: 8075 RVA: 0x0006D1CB File Offset: 0x0006B3CB
		public bool IsAIControlled { get; private set; } = true;

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x0006D1D4 File Offset: 0x0006B3D4
		// (set) Token: 0x06001F8D RID: 8077 RVA: 0x0006D1DC File Offset: 0x0006B3DC
		public Vec2 Direction { get; private set; }

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x0006D1E5 File Offset: 0x0006B3E5
		// (set) Token: 0x06001F8F RID: 8079 RVA: 0x0006D1ED File Offset: 0x0006B3ED
		public int UnitSpacing { get; private set; }

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0006D1F6 File Offset: 0x0006B3F6
		// (set) Token: 0x06001F91 RID: 8081 RVA: 0x0006D1FE File Offset: 0x0006B3FE
		public object OrderPositionLock { get; private set; } = new object();

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001F92 RID: 8082 RVA: 0x0006D207 File Offset: 0x0006B407
		// (set) Token: 0x06001F93 RID: 8083 RVA: 0x0006D20F File Offset: 0x0006B40F
		public object SimulationFormationLock { get; private set; } = new object();

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x0006D218 File Offset: 0x0006B418
		public int CountOfUnits
		{
			get
			{
				return this.Arrangement.UnitCount + this._detachedUnits.Count;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x0006D231 File Offset: 0x0006B431
		public int CountOfDetachedUnits
		{
			get
			{
				return this._detachedUnits.Count;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001F96 RID: 8086 RVA: 0x0006D23E File Offset: 0x0006B43E
		public int CountOfUndetachableNonPlayerUnits
		{
			get
			{
				return this._undetachableNonPlayerUnitCount;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x0006D246 File Offset: 0x0006B446
		public int CountOfUnitsWithoutDetachedOnes
		{
			get
			{
				return this.Arrangement.UnitCount + this._looseDetachedUnits.Count;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001F98 RID: 8088 RVA: 0x0006D25F File Offset: 0x0006B45F
		public MBReadOnlyList<IFormationUnit> UnitsWithoutLooseDetachedOnes
		{
			get
			{
				return this.Arrangement.GetAllUnits();
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0006D26C File Offset: 0x0006B46C
		public int CountOfUnitsWithoutLooseDetachedOnes
		{
			get
			{
				return this.Arrangement.UnitCount;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x0006D279 File Offset: 0x0006B479
		public int CountOfDetachableNonPlayerUnits
		{
			get
			{
				return this.Arrangement.UnitCount - ((this.IsPlayerTroopInFormation || this.HasPlayerControlledTroop) ? 1 : 0) - this.CountOfUndetachableNonPlayerUnits;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0006D2A2 File Offset: 0x0006B4A2
		public Vec2 OrderPosition
		{
			get
			{
				return this._orderPosition.AsVec2;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x0006D2AF File Offset: 0x0006B4AF
		public Vec3 OrderGroundPosition
		{
			get
			{
				return this._orderPosition.GetGroundVec3();
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0006D2BC File Offset: 0x0006B4BC
		public bool OrderPositionIsValid
		{
			get
			{
				return this._orderPosition.IsValid;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x0006D2C9 File Offset: 0x0006B4C9
		public float Depth
		{
			get
			{
				return this.Arrangement.Depth;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x0006D2D6 File Offset: 0x0006B4D6
		public float MinimumWidth
		{
			get
			{
				return this.Arrangement.MinimumWidth;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x0006D2E3 File Offset: 0x0006B4E3
		public float MaximumWidth
		{
			get
			{
				return this.Arrangement.MaximumWidth;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0006D2F0 File Offset: 0x0006B4F0
		public float UnitDiameter
		{
			get
			{
				return Formation.GetDefaultUnitDiameter(this.CalculateHasSignificantNumberOfMounted && !(this.RidingOrder == RidingOrder.RidingOrderDismount));
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0006D318 File Offset: 0x0006B518
		public Vec2 CurrentDirection
		{
			get
			{
				return (this.QuerySystem.EstimatedDirection * 0.8f + this.Direction * 0.2f).Normalized();
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0006D357 File Offset: 0x0006B557
		public Vec2 SmoothedAverageUnitPosition
		{
			get
			{
				return this._smoothedAverageUnitPosition;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x0006D35F File Offset: 0x0006B55F
		public MBReadOnlyList<Agent> LooseDetachedUnits
		{
			get
			{
				return this._looseDetachedUnits;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x0006D367 File Offset: 0x0006B567
		public MBReadOnlyList<Agent> DetachedUnits
		{
			get
			{
				return this._detachedUnits;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x0006D36F File Offset: 0x0006B56F
		// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x0006D377 File Offset: 0x0006B577
		public AttackEntityOrderSecondaryDetachment AttackEntityOrderSecondaryDetachment { get; private set; }

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0006D380 File Offset: 0x0006B580
		// (set) Token: 0x06001FA9 RID: 8105 RVA: 0x0006D388 File Offset: 0x0006B588
		public FormationAI AI { get; private set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x0006D391 File Offset: 0x0006B591
		// (set) Token: 0x06001FAB RID: 8107 RVA: 0x0006D39C File Offset: 0x0006B59C
		public Formation TargetFormation
		{
			get
			{
				return this._targetFormation;
			}
			private set
			{
				if (this._targetFormation != value)
				{
					this._targetFormation = value;
					this.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						Formation value2 = value;
						agent.SetTargetFormationIndex((value2 != null) ? value2.Index : (-1));
					}, null);
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001FAC RID: 8108 RVA: 0x0006D3E3 File Offset: 0x0006B5E3
		// (set) Token: 0x06001FAD RID: 8109 RVA: 0x0006D3EB File Offset: 0x0006B5EB
		public FormationQuerySystem QuerySystem { get; private set; }

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x0006D3F4 File Offset: 0x0006B5F4
		// (set) Token: 0x06001FAF RID: 8111 RVA: 0x0006D3FC File Offset: 0x0006B5FC
		public Formation.FormationIntegrityDataGroup CachedFormationIntegrityData { get; private set; }

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x0006D405 File Offset: 0x0006B605
		// (set) Token: 0x06001FB1 RID: 8113 RVA: 0x0006D40D File Offset: 0x0006B60D
		public Vec2 CachedAveragePosition { get; private set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x0006D416 File Offset: 0x0006B616
		// (set) Token: 0x06001FB3 RID: 8115 RVA: 0x0006D41E File Offset: 0x0006B61E
		public WorldPosition CachedMedianPosition { get; private set; }

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0006D427 File Offset: 0x0006B627
		// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x0006D42F File Offset: 0x0006B62F
		public Vec2 CachedCurrentVelocity { get; private set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x0006D438 File Offset: 0x0006B638
		// (set) Token: 0x06001FB7 RID: 8119 RVA: 0x0006D440 File Offset: 0x0006B640
		public float CachedMovementSpeed { get; private set; } = 1f;

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x0006D449 File Offset: 0x0006B649
		// (set) Token: 0x06001FB9 RID: 8121 RVA: 0x0006D451 File Offset: 0x0006B651
		public float CachedClosestEnemyFormationDistanceSquared { get; private set; }

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001FBA RID: 8122 RVA: 0x0006D45A File Offset: 0x0006B65A
		public FormationQuerySystem CachedClosestEnemyFormation
		{
			get
			{
				Formation cachedClosestEnemyFormation = this._cachedClosestEnemyFormation;
				if (cachedClosestEnemyFormation == null)
				{
					return null;
				}
				return cachedClosestEnemyFormation.QuerySystem;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x0006D46D File Offset: 0x0006B66D
		public MBReadOnlyList<IDetachment> Detachments
		{
			get
			{
				return this._detachments;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x0006D475 File Offset: 0x0006B675
		// (set) Token: 0x06001FBD RID: 8125 RVA: 0x0006D47D File Offset: 0x0006B67D
		public int? OverridenUnitCount { get; private set; }

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001FBE RID: 8126 RVA: 0x0006D486 File Offset: 0x0006B686
		// (set) Token: 0x06001FBF RID: 8127 RVA: 0x0006D48E File Offset: 0x0006B68E
		public bool IsSpawning { get; private set; }

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x0006D497 File Offset: 0x0006B697
		// (set) Token: 0x06001FC1 RID: 8129 RVA: 0x0006D49F File Offset: 0x0006B69F
		public bool IsAITickedAfterSplit { get; set; }

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x0006D4A8 File Offset: 0x0006B6A8
		// (set) Token: 0x06001FC3 RID: 8131 RVA: 0x0006D4B0 File Offset: 0x0006B6B0
		public bool HasPlayerControlledTroop { get; private set; }

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x0006D4B9 File Offset: 0x0006B6B9
		// (set) Token: 0x06001FC5 RID: 8133 RVA: 0x0006D4C1 File Offset: 0x0006B6C1
		public bool IsPlayerTroopInFormation { get; private set; }

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001FC6 RID: 8134 RVA: 0x0006D4CA File Offset: 0x0006B6CA
		// (set) Token: 0x06001FC7 RID: 8135 RVA: 0x0006D4D2 File Offset: 0x0006B6D2
		public bool ContainsAgentVisuals { get; set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x0006D4DB File Offset: 0x0006B6DB
		// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x0006D4E3 File Offset: 0x0006B6E3
		public Agent PlayerOwner
		{
			get
			{
				return this._playerOwner;
			}
			set
			{
				this._playerOwner = value;
				this.SetControlledByAI(value == null, false);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x0006D52F File Offset: 0x0006B72F
		// (set) Token: 0x06001FCA RID: 8138 RVA: 0x0006D4F7 File Offset: 0x0006B6F7
		public string BannerCode
		{
			get
			{
				return this._bannerCode;
			}
			set
			{
				this._bannerCode = value;
				if (GameNetwork.IsServer)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new InitializeFormation(this, this.Team.TeamIndex, this._bannerCode));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x0006D537 File Offset: 0x0006B737
		public bool IsSplittableByAI
		{
			get
			{
				return this.IsAIOwned && this.IsConvenientForTransfer;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x0006D54C File Offset: 0x0006B74C
		public bool IsAIOwned
		{
			get
			{
				return !this._enforceNotSplittableByAI && (this.IsAIControlled || (!this.Team.IsPlayerGeneral && (!this.Team.IsPlayerSergeant || this.PlayerOwner != Agent.Main)));
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x0006D59B File Offset: 0x0006B79B
		public bool IsConvenientForTransfer
		{
			get
			{
				return Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege || this.Team.Side != BattleSideEnum.Attacker || this.QuerySystem.InsideCastleUnitCountIncludingUnpositioned == 0;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0006D5C8 File Offset: 0x0006B7C8
		public Vec2 OrderLocalAveragePosition
		{
			get
			{
				if (this._orderLocalAveragePositionIsDirty)
				{
					this._orderLocalAveragePositionIsDirty = false;
					this._orderLocalAveragePosition = default(Vec2);
					if (this.UnitsWithoutLooseDetachedOnes.Count > 0)
					{
						int num = 0;
						foreach (IFormationUnit formationUnit in this.UnitsWithoutLooseDetachedOnes)
						{
							Vec2? localPositionOfUnitOrDefault = this.Arrangement.GetLocalPositionOfUnitOrDefault(formationUnit);
							if (localPositionOfUnitOrDefault != null)
							{
								this._orderLocalAveragePosition += localPositionOfUnitOrDefault.Value;
								num++;
							}
						}
						if (num > 0)
						{
							this._orderLocalAveragePosition *= 1f / (float)num;
						}
					}
				}
				return this._orderLocalAveragePosition;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x0006D69C File Offset: 0x0006B89C
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x0006D6A4 File Offset: 0x0006B8A4
		public FacingOrder FacingOrder { get; private set; }

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0006D6AD File Offset: 0x0006B8AD
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x0006D6B5 File Offset: 0x0006B8B5
		public ArrangementOrder ArrangementOrder { get; private set; }

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x0006D6BE File Offset: 0x0006B8BE
		// (set) Token: 0x06001FD5 RID: 8149 RVA: 0x0006D6C6 File Offset: 0x0006B8C6
		public FormOrder FormOrder { get; private set; }

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x0006D6CF File Offset: 0x0006B8CF
		// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x0006D6D7 File Offset: 0x0006B8D7
		public RidingOrder RidingOrder { get; private set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x0006D6E0 File Offset: 0x0006B8E0
		// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x0006D6E8 File Offset: 0x0006B8E8
		public FiringOrder FiringOrder { get; private set; }

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x0006D6F1 File Offset: 0x0006B8F1
		private bool IsSimulationFormation
		{
			get
			{
				return this.Team == null;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x0006D6FC File Offset: 0x0006B8FC
		public bool HasAnyMountedUnit
		{
			get
			{
				if (this._overridenHasAnyMountedUnit != null)
				{
					return this._overridenHasAnyMountedUnit.Value;
				}
				int num = (int)(this.QuerySystem.RangedCavalryUnitRatioReadOnly * (float)this.CountOfUnits + 1E-05f);
				int num2 = (int)(this.QuerySystem.CavalryUnitRatioReadOnly * (float)this.CountOfUnits + 1E-05f);
				return num + num2 > 0;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x0006D75C File Offset: 0x0006B95C
		// (set) Token: 0x06001FDD RID: 8157 RVA: 0x0006D769 File Offset: 0x0006B969
		public float Width
		{
			get
			{
				return this.Arrangement.Width;
			}
			private set
			{
				this.Arrangement.Width = value;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x0006D777 File Offset: 0x0006B977
		public bool IsDeployment
		{
			get
			{
				return Mission.Current.Mode == MissionMode.Deployment;
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x0006D786 File Offset: 0x0006B986
		public FormationClass LogicalClass
		{
			get
			{
				return this._logicalClass;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x0006D78E File Offset: 0x0006B98E
		public IEnumerable<FormationClass> SecondaryLogicalClasses
		{
			get
			{
				FormationClass primaryLogicalClass = this.LogicalClass;
				if (primaryLogicalClass == FormationClass.NumberOfAllFormations)
				{
					yield break;
				}
				List<ValueTuple<FormationClass, int>> list = new List<ValueTuple<FormationClass, int>>();
				for (int i = 0; i < this._logicalClassCounts.Length; i++)
				{
					if (this._logicalClassCounts[i] > 0)
					{
						list.Add(new ValueTuple<FormationClass, int>((FormationClass)i, this._logicalClassCounts[i]));
					}
				}
				if (list.Count > 0)
				{
					list.Sort(Comparer<ValueTuple<FormationClass, int>>.Create(delegate([TupleElementNames(new string[] { "fClass", "count" })] ValueTuple<FormationClass, int> x, [TupleElementNames(new string[] { "fClass", "count" })] ValueTuple<FormationClass, int> y)
					{
						if (x.Item2 < y.Item2)
						{
							return 1;
						}
						if (x.Item2 <= y.Item2)
						{
							return 0;
						}
						return -1;
					}));
					foreach (ValueTuple<FormationClass, int> valueTuple in list)
					{
						if (valueTuple.Item1 != primaryLogicalClass)
						{
							yield return valueTuple.Item1;
						}
					}
					List<ValueTuple<FormationClass, int>>.Enumerator enumerator = default(List<ValueTuple<FormationClass, int>>.Enumerator);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x0006D79E File Offset: 0x0006B99E
		// (set) Token: 0x06001FE2 RID: 8162 RVA: 0x0006D7A8 File Offset: 0x0006B9A8
		public IFormationArrangement Arrangement
		{
			get
			{
				return this._arrangement;
			}
			set
			{
				if (this._arrangement != null)
				{
					this._arrangement.OnWidthChanged -= this.Arrangement_OnWidthChanged;
					this._arrangement.OnShapeChanged -= this.Arrangement_OnShapeChanged;
				}
				this._arrangement = value;
				if (this._arrangement != null)
				{
					this._arrangement.OnWidthChanged += this.Arrangement_OnWidthChanged;
					this._arrangement.OnShapeChanged += this.Arrangement_OnShapeChanged;
				}
				this.Arrangement_OnWidthChanged();
				this.Arrangement_OnShapeChanged();
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x0006D834 File Offset: 0x0006BA34
		public FormationClass PhysicalClass
		{
			get
			{
				return this.QuerySystem.MainClass;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06001FE4 RID: 8164 RVA: 0x0006D841 File Offset: 0x0006BA41
		public IEnumerable<FormationClass> SecondaryPhysicalClasses
		{
			get
			{
				FormationClass primaryPhysicalClass = this.PhysicalClass;
				if (primaryPhysicalClass != FormationClass.Infantry && this.QuerySystem.InfantryUnitRatio > 0f)
				{
					yield return FormationClass.Infantry;
				}
				if (primaryPhysicalClass != FormationClass.Ranged && this.QuerySystem.RangedUnitRatio > 0f)
				{
					yield return FormationClass.Ranged;
				}
				if (primaryPhysicalClass != FormationClass.Cavalry && this.QuerySystem.CavalryUnitRatio > 0f)
				{
					yield return FormationClass.Cavalry;
				}
				if (primaryPhysicalClass != FormationClass.HorseArcher && this.QuerySystem.RangedCavalryUnitRatio > 0f)
				{
					yield return FormationClass.HorseArcher;
				}
				yield break;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x0006D854 File Offset: 0x0006BA54
		public float Interval
		{
			get
			{
				if (this.CalculateHasSignificantNumberOfMounted && !(this.RidingOrder == RidingOrder.RidingOrderDismount))
				{
					return Formation.CavalryInterval(this.UnitSpacing) * this.Arrangement.IntervalMultiplier;
				}
				return Formation.InfantryInterval(this.UnitSpacing) * this.Arrangement.IntervalMultiplier;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06001FE6 RID: 8166 RVA: 0x0006D8AA File Offset: 0x0006BAAA
		public bool CalculateHasSignificantNumberOfMounted
		{
			get
			{
				if (this._overridenHasAnyMountedUnit != null)
				{
					return this._overridenHasAnyMountedUnit.Value;
				}
				return this.QuerySystem.CavalryUnitRatio + this.QuerySystem.RangedCavalryUnitRatio >= 0.1f;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x0006D8E8 File Offset: 0x0006BAE8
		public float Distance
		{
			get
			{
				if (this.CalculateHasSignificantNumberOfMounted && !(this.RidingOrder == RidingOrder.RidingOrderDismount))
				{
					return Formation.CavalryDistance(this.UnitSpacing) * this.Arrangement.DistanceMultiplier;
				}
				return Formation.InfantryDistance(this.UnitSpacing) * this.Arrangement.DistanceMultiplier;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x0006D940 File Offset: 0x0006BB40
		public Vec2 CurrentPosition
		{
			get
			{
				ColumnFormation columnFormation;
				if ((columnFormation = this.Arrangement as ColumnFormation) != null)
				{
					if (this.CountOfUnitsWithoutDetachedOnes <= 0)
					{
						return this.OrderPosition;
					}
					Agent agent = (columnFormation.GetUnit(columnFormation.VanguardFileIndex, 0) ?? columnFormation.Vanguard) as Agent;
					if (agent != null)
					{
						return agent.Position.AsVec2;
					}
				}
				return this.CachedAveragePosition + this.CurrentDirection.TransformToParentUnitF(-this.OrderLocalAveragePosition);
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x0006D9BF File Offset: 0x0006BBBF
		// (set) Token: 0x06001FEA RID: 8170 RVA: 0x0006D9C7 File Offset: 0x0006BBC7
		public Agent Captain
		{
			get
			{
				return this._captain;
			}
			set
			{
				if (this._captain != value)
				{
					this._captain = value;
					this.OnCaptainChanged();
				}
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001FEB RID: 8171 RVA: 0x0006D9DF File Offset: 0x0006BBDF
		public float MinimumDistance
		{
			get
			{
				return Formation.GetDefaultMinimumUnitDistance(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount));
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001FEC RID: 8172 RVA: 0x0006DA04 File Offset: 0x0006BC04
		public bool IsLoose
		{
			get
			{
				return ArrangementOrder.GetUnitLooseness(this.ArrangementOrder.OrderEnum);
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001FED RID: 8173 RVA: 0x0006DA16 File Offset: 0x0006BC16
		public float MinimumInterval
		{
			get
			{
				return Formation.GetDefaultMinimumUnitInterval(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount));
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001FEE RID: 8174 RVA: 0x0006DA3B File Offset: 0x0006BC3B
		public float MaximumInterval
		{
			get
			{
				return Formation.GetDefaultUnitInterval(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount), ArrangementOrder.GetUnitSpacingOf(this.ArrangementOrder.OrderEnum));
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x0006DA70 File Offset: 0x0006BC70
		public float MaximumDistance
		{
			get
			{
				return Formation.GetDefaultUnitDistance(this.HasAnyMountedUnit && !(this.RidingOrder == RidingOrder.RidingOrderDismount), ArrangementOrder.GetUnitSpacingOf(this.ArrangementOrder.OrderEnum));
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x0006DAA5 File Offset: 0x0006BCA5
		// (set) Token: 0x06001FF1 RID: 8177 RVA: 0x0006DAAD File Offset: 0x0006BCAD
		internal bool PostponeCostlyOperations { get; private set; }

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0006DAB8 File Offset: 0x0006BCB8
		public Formation(Team team, int index)
		{
			this.Team = team;
			this.Index = index;
			this.FormationIndex = (FormationClass)index;
			this.IsSpawning = false;
			this.Reset();
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0006DB60 File Offset: 0x0006BD60
		~Formation()
		{
			if (!this.IsSimulationFormation)
			{
				Formation._simulationFormationTemp = null;
			}
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0006DB94 File Offset: 0x0006BD94
		bool IFormation.GetIsLocalPositionAvailable(Vec2 localPosition, Vec2? nearestAvailableUnitPositionLocal)
		{
			Vec2 vec = this.Direction.TransformToParentUnitF(localPosition);
			WorldPosition worldPosition = this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.NavMeshVec3);
			worldPosition.SetVec2(this.OrderPosition + vec);
			WorldPosition worldPosition2 = WorldPosition.Invalid;
			if (nearestAvailableUnitPositionLocal != null)
			{
				vec = this.Direction.TransformToParentUnitF(nearestAvailableUnitPositionLocal.Value);
				worldPosition2 = this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.NavMeshVec3);
				worldPosition2.SetVec2(this.OrderPosition + vec);
			}
			float num = MathF.Abs(localPosition.x) + MathF.Abs(localPosition.y) + (this.Interval + this.Distance) * 2f;
			return Mission.Current.IsFormationUnitPositionAvailableMT(ref this._orderPosition, ref worldPosition, ref worldPosition2, num, this.Team);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0006DC58 File Offset: 0x0006BE58
		IFormationUnit IFormation.GetClosestUnitTo(Vec2 localPosition, MBList<IFormationUnit> unitsWithSpaces, float? maxDistance)
		{
			Vec2 vec = this.Direction.TransformToParentUnitF(localPosition);
			Vec2 vec2 = this.OrderPosition + vec;
			return this.GetClosestUnitToAux(vec2, unitsWithSpaces, maxDistance);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0006DC8C File Offset: 0x0006BE8C
		IFormationUnit IFormation.GetClosestUnitTo(IFormationUnit targetUnit, MBList<IFormationUnit> unitsWithSpaces, float? maxDistance)
		{
			return this.GetClosestUnitToAux(((Agent)targetUnit).Position.AsVec2, unitsWithSpaces, maxDistance);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0006DCB4 File Offset: 0x0006BEB4
		void IFormation.SetUnitToFollow(IFormationUnit unit, IFormationUnit toFollow, Vec2 vector)
		{
			Agent agent = unit as Agent;
			Agent agent2 = toFollow as Agent;
			agent.SetColumnwiseFollowAgent(agent2, ref vector);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x0006DCD8 File Offset: 0x0006BED8
		bool IFormation.BatchUnitPositions(MBArrayList<Vec2i> orderedPositionIndices, MBArrayList<Vec2> orderedLocalPositions, MBList2D<int> availabilityTable, MBList2D<WorldPosition> globalPositionTable, int fileCount, int rankCount)
		{
			if (this._orderPosition.IsValid && this._orderPosition.GetNavMesh() != UIntPtr.Zero)
			{
				Mission.Current.BatchFormationUnitPositions(orderedPositionIndices, orderedLocalPositions, availabilityTable, globalPositionTable, this._orderPosition, this.Direction, fileCount, rankCount, Mission.Current.IsSiegeBattle);
				return true;
			}
			return false;
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0006DD38 File Offset: 0x0006BF38
		public WorldPosition CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			if (!this.OrderPositionIsValid)
			{
				Debug.Print(string.Concat(new object[]
				{
					"Formation order position is not valid. Team: ",
					this.Team.TeamIndex,
					", Formation: ",
					(int)this.FormationIndex
				}), 0, Debug.DebugColor.Yellow, 17592186044416UL);
			}
			if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
			{
				if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
				{
					this._orderPosition.GetGroundVec3();
				}
			}
			else
			{
				this._orderPosition.GetNavMeshVec3();
			}
			return this._orderPosition;
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x0006DDC4 File Offset: 0x0006BFC4
		public void SetMovementOrder(MovementOrder input)
		{
			Action<Formation, MovementOrder.MovementOrderEnum> onBeforeMovementOrderApplied = this.OnBeforeMovementOrderApplied;
			if (onBeforeMovementOrderApplied != null)
			{
				onBeforeMovementOrderApplied(this, input.OrderEnum);
			}
			if (input.OrderEnum == MovementOrder.MovementOrderEnum.Invalid)
			{
				input = MovementOrder.MovementOrderStop;
			}
			bool flag = !this._movementOrder.AreOrdersPracticallySame(this._movementOrder, input, this.IsAIControlled);
			if (flag)
			{
				this._movementOrder.OnCancel(this);
			}
			if (flag)
			{
				if (MovementOrder.GetMovementOrderDefensivenessChange(this._movementOrder.OrderEnum, input.OrderEnum) != 0)
				{
					if (MovementOrder.GetMovementOrderDefensiveness(input.OrderEnum) == 0)
					{
						this._formationOrderDefensivenessFactor = 0;
					}
					else
					{
						this._formationOrderDefensivenessFactor = MovementOrder.GetMovementOrderDefensiveness(input.OrderEnum) + ArrangementOrder.GetArrangementOrderDefensiveness(this.ArrangementOrder.OrderEnum);
					}
					this.UpdateAgentDrivenPropertiesBasedOnOrderDefensiveness();
				}
				this._movementOrder = input;
				this._movementOrder.OnApply(this);
			}
			this.SetTargetFormation(null);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0006DE94 File Offset: 0x0006C094
		public void SetFacingOrder(FacingOrder order)
		{
			this.FacingOrder = order;
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0006DEA0 File Offset: 0x0006C0A0
		public void SetArrangementOrder(ArrangementOrder order)
		{
			if (order.OrderType != this.ArrangementOrder.OrderType)
			{
				this.ArrangementOrder.OnCancel(this);
				int arrangementOrderDefensivenessChange = ArrangementOrder.GetArrangementOrderDefensivenessChange(this.ArrangementOrder.OrderEnum, order.OrderEnum);
				if (arrangementOrderDefensivenessChange != 0 && MovementOrder.GetMovementOrderDefensiveness(this._movementOrder.OrderEnum) != 0)
				{
					this._formationOrderDefensivenessFactor += arrangementOrderDefensivenessChange;
					this.UpdateAgentDrivenPropertiesBasedOnOrderDefensiveness();
				}
				this.ArrangementOrder = order;
				this.ArrangementOrder.OnApply(this);
				Action<Formation, ArrangementOrder.ArrangementOrderEnum> onAfterArrangementOrderApplied = this.OnAfterArrangementOrderApplied;
				if (onAfterArrangementOrderApplied != null)
				{
					onAfterArrangementOrderApplied(this, this.ArrangementOrder.OrderEnum);
				}
				if (this.FormOrder.OrderEnum == FormOrder.FormOrderEnum.Custom && order.OrderEnum != ArrangementOrder.ArrangementOrderEnum.Column)
				{
					this.SetFormOrder(FormOrder.FormOrderCustom(this.CalculateDesiredWidth()), false);
				}
				this.QuerySystem.Expire();
				if (this.CountOfUnitsWithoutLooseDetachedOnes > 0)
				{
					this.ForceCalculateCaches();
					return;
				}
			}
			else
			{
				this.ArrangementOrder.SoftUpdate(this);
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0006DF9C File Offset: 0x0006C19C
		public void SetFormOrder(FormOrder order, bool updateDesiredFileCount = true)
		{
			if (order.OrderEnum == FormOrder.FormOrderEnum.Custom && updateDesiredFileCount)
			{
				this._desiredFileCount = (int)((order.CustomFlankWidth - this.UnitDiameter) / (this.UnitDiameter + this.Interval)) + 1;
			}
			this.FormOrder = order;
			this.FormOrder.OnApply(this);
			this.QuerySystem.Expire();
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x0006DFFC File Offset: 0x0006C1FC
		public void SetRidingOrder(RidingOrder order)
		{
			if (this.RidingOrder != order)
			{
				this.RidingOrder = order;
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.SetRidingOrder(order.OrderEnum);
				}, null);
				this.Arrangement_OnShapeChanged();
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0006E050 File Offset: 0x0006C250
		public void SetFiringOrder(FiringOrder order)
		{
			if (this.FiringOrder != order)
			{
				this.FiringOrder = order;
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.SetFiringOrder(order.OrderEnum);
				}, null);
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0006E09C File Offset: 0x0006C29C
		public void SetControlledByAI(bool isControlledByAI, bool enforceNotSplittableByAI = false)
		{
			if (this.IsAIControlled != isControlledByAI)
			{
				this.IsAIControlled = isControlledByAI;
				if (this.IsAIControlled)
				{
					if (this.AI.ActiveBehavior != null && this.CountOfUnits > 0)
					{
						bool forceTickOccasionally = Mission.Current.ForceTickOccasionally;
						Mission.Current.ForceTickOccasionally = true;
						BehaviorComponent activeBehavior = this.AI.ActiveBehavior;
						this.AI.Tick();
						Mission.Current.ForceTickOccasionally = forceTickOccasionally;
						if (activeBehavior == this.AI.ActiveBehavior)
						{
							this.AI.ActiveBehavior.OnBehaviorActivated();
						}
						this.SetMovementOrder(this.AI.ActiveBehavior.CurrentOrder);
					}
					this._enforceNotSplittableByAI = enforceNotSplittableByAI;
					return;
				}
				this._enforceNotSplittableByAI = false;
				FormationAI ai = this.AI;
				if (ai == null)
				{
					return;
				}
				BehaviorComponent activeBehavior2 = ai.ActiveBehavior;
				if (activeBehavior2 == null)
				{
					return;
				}
				activeBehavior2.OnLostAIControl();
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0006E170 File Offset: 0x0006C370
		public void SetTargetFormation(Formation targetFormation)
		{
			this.TargetFormation = targetFormation;
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0006E179 File Offset: 0x0006C379
		public void OnDeploymentFinished()
		{
			FormationAI ai = this.AI;
			if (ai != null)
			{
				ai.OnDeploymentFinished();
			}
			OrderController.TryCancelStopOrder(this);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0006E192 File Offset: 0x0006C392
		public void ResetArrangementOrderTickTimer()
		{
			this._arrangementOrderTickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0006E1B0 File Offset: 0x0006C3B0
		public void SetPositioning(WorldPosition? position = null, Vec2? direction = null, int? unitSpacing = null)
		{
			Vec2 orderPosition = this.OrderPosition;
			Vec2 direction2 = this.Direction;
			WorldPosition? worldPosition = null;
			bool flag = false;
			bool flag2 = false;
			if (position != null && position.Value.IsValid)
			{
				if (!this.HasBeenPositioned && !this.IsSimulationFormation)
				{
					this.HasBeenPositioned = true;
				}
				if (!position.Value.AsVec2.NearlyEquals(this.OrderPosition, 0.001f))
				{
					if (!Mission.Current.IsPositionInsideBoundaries(position.Value.AsVec2))
					{
						Vec2 closestBoundaryPosition = Mission.Current.GetClosestBoundaryPosition(position.Value.AsVec2);
						if (this.OrderPosition != closestBoundaryPosition)
						{
							WorldPosition value = position.Value;
							value.SetVec2(closestBoundaryPosition);
							worldPosition = new WorldPosition?(value);
						}
					}
					else
					{
						worldPosition = position;
					}
					if (!this.IsSimulationFormation && position.Value.AsVec2.DistanceSquared(this.OrderPosition) > this.UnitDiameter * this.UnitDiameter * 25f)
					{
						this.Arrangement.UpdateLocalPositionErrors(true);
					}
				}
			}
			if (direction != null && !this.Direction.NearlyEquals(direction.Value, 0.01f))
			{
				flag = true;
			}
			if (unitSpacing != null && this.UnitSpacing != unitSpacing.Value)
			{
				flag2 = true;
				if (!this.IsSimulationFormation)
				{
					this.Arrangement.UpdateLocalPositionErrors(false);
				}
			}
			if (worldPosition != null || flag || flag2)
			{
				this.Arrangement.BeforeFormationFrameChange();
				if (worldPosition != null)
				{
					this._orderPosition = worldPosition.Value;
				}
				if (flag)
				{
					this.Direction = direction.Value;
				}
				if (flag2)
				{
					this.UnitSpacing = unitSpacing.Value;
					Action<Formation> onUnitSpacingChanged = this.OnUnitSpacingChanged;
					if (onUnitSpacingChanged != null)
					{
						onUnitSpacingChanged(this);
					}
					this.Arrangement_OnShapeChanged();
					this.Arrangement.AreLocalPositionsDirty = true;
				}
				if (!this.IsSimulationFormation && this.Arrangement.IsTurnBackwardsNecessary(orderPosition, worldPosition, direction2, flag, direction))
				{
					this.Arrangement.TurnBackwards();
				}
				this.Arrangement.OnFormationFrameChanged(false);
				if (worldPosition != null)
				{
					this.ArrangementOrder.OnOrderPositionChanged(this, orderPosition);
				}
			}
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x0006E400 File Offset: 0x0006C600
		public int GetCountOfUnitsWithCondition(Func<Agent, bool> function)
		{
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (function((Agent)formationUnit))
				{
					num++;
				}
			}
			foreach (Agent agent in this._detachedUnits)
			{
				if (function(agent))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0006E4B0 File Offset: 0x0006C6B0
		public readonly ref MovementOrder GetReadonlyMovementOrderReference()
		{
			return ref this._movementOrder;
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0006E4B8 File Offset: 0x0006C6B8
		public Agent GetFirstUnit()
		{
			return this.GetUnitWithIndex(0);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0006E4C1 File Offset: 0x0006C6C1
		public int GetCountOfUnitsBelongingToLogicalClass(FormationClass logicalClass)
		{
			return this._logicalClassCounts[(int)logicalClass];
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0006E4CC File Offset: 0x0006C6CC
		public int GetCountOfUnitsBelongingToPhysicalClass(FormationClass physicalClass, bool excludeBannerBearers)
		{
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				bool flag = false;
				switch (physicalClass)
				{
				case FormationClass.Infantry:
					flag = (excludeBannerBearers ? QueryLibrary.IsInfantryWithoutBanner((Agent)formationUnit) : QueryLibrary.IsInfantry((Agent)formationUnit));
					break;
				case FormationClass.Ranged:
					flag = (excludeBannerBearers ? QueryLibrary.IsRangedWithoutBanner((Agent)formationUnit) : QueryLibrary.IsRanged((Agent)formationUnit));
					break;
				case FormationClass.Cavalry:
					flag = (excludeBannerBearers ? QueryLibrary.IsCavalryWithoutBanner((Agent)formationUnit) : QueryLibrary.IsCavalry((Agent)formationUnit));
					break;
				case FormationClass.HorseArcher:
					flag = (excludeBannerBearers ? QueryLibrary.IsRangedCavalryWithoutBanner((Agent)formationUnit) : QueryLibrary.IsRangedCavalry((Agent)formationUnit));
					break;
				}
				if (flag)
				{
					num++;
				}
			}
			foreach (Agent agent in this._detachedUnits)
			{
				bool flag2 = false;
				switch (physicalClass)
				{
				case FormationClass.Infantry:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsInfantryWithoutBanner(agent) : QueryLibrary.IsInfantry(agent));
					break;
				case FormationClass.Ranged:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsRangedWithoutBanner(agent) : QueryLibrary.IsRanged(agent));
					break;
				case FormationClass.Cavalry:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsCavalryWithoutBanner(agent) : QueryLibrary.IsCavalry(agent));
					break;
				case FormationClass.HorseArcher:
					flag2 = (excludeBannerBearers ? QueryLibrary.IsRangedCavalryWithoutBanner(agent) : QueryLibrary.IsRangedCavalry(agent));
					break;
				}
				if (flag2)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0006E680 File Offset: 0x0006C880
		public void SetSpawnIndex(int value = 0)
		{
			this._currentSpawnIndex = value;
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0006E689 File Offset: 0x0006C889
		public int GetNextSpawnIndex()
		{
			int currentSpawnIndex = this._currentSpawnIndex;
			this._currentSpawnIndex++;
			return currentSpawnIndex;
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x0006E6A0 File Offset: 0x0006C8A0
		public Agent GetUnitWithIndex(int unitIndex)
		{
			if (this.Arrangement.GetAllUnits().Count > unitIndex)
			{
				return (Agent)this.Arrangement.GetAllUnits()[unitIndex];
			}
			unitIndex -= this.Arrangement.GetAllUnits().Count;
			if (this._detachedUnits.Count > unitIndex)
			{
				return this._detachedUnits[unitIndex];
			}
			return null;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0006E708 File Offset: 0x0006C908
		public Vec2 GetAveragePositionOfUnits(bool excludeDetachedUnits, bool excludePlayer)
		{
			int num = (excludeDetachedUnits ? this.CountOfUnitsWithoutDetachedOnes : this.CountOfUnits);
			if (num > 0)
			{
				Vec2 vec = Vec2.Zero;
				foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
				{
					Agent agent = (Agent)formationUnit;
					if (!excludePlayer || !agent.IsMainAgent)
					{
						vec += agent.Position.AsVec2;
					}
					else
					{
						num--;
					}
				}
				if (excludeDetachedUnits)
				{
					using (List<Agent>.Enumerator enumerator2 = this._looseDetachedUnits.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Agent agent2 = enumerator2.Current;
							vec += agent2.Position.AsVec2;
						}
						goto IL_0112;
					}
				}
				foreach (Agent agent3 in this._detachedUnits)
				{
					vec += agent3.Position.AsVec2;
				}
				IL_0112:
				if (num > 0)
				{
					return vec * (1f / (float)num);
				}
			}
			return Vec2.Invalid;
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0006E868 File Offset: 0x0006CA68
		public Agent GetMedianAgent(bool excludeDetachedUnits, bool excludePlayer, Vec2 averagePosition)
		{
			excludeDetachedUnits = excludeDetachedUnits && this.CountOfUnitsWithoutDetachedOnes > 0;
			excludePlayer = excludePlayer && (this.CountOfUndetachableNonPlayerUnits > 0 || this.CountOfDetachableNonPlayerUnits > 0);
			float num = float.MaxValue;
			Agent agent = null;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent2 = (Agent)formationUnit;
				if (!excludePlayer || !agent2.IsMainAgent)
				{
					float num2 = agent2.Position.AsVec2.DistanceSquared(averagePosition);
					if (num2 <= num)
					{
						agent = agent2;
						num = num2;
					}
				}
			}
			if (excludeDetachedUnits)
			{
				using (List<Agent>.Enumerator enumerator2 = this._looseDetachedUnits.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Agent agent3 = enumerator2.Current;
						float num3 = agent3.Position.AsVec2.DistanceSquared(averagePosition);
						if (num3 <= num)
						{
							agent = agent3;
							num = num3;
						}
					}
					return agent;
				}
			}
			foreach (Agent agent4 in this._detachedUnits)
			{
				float num4 = agent4.Position.AsVec2.DistanceSquared(averagePosition);
				if (num4 <= num)
				{
					agent = agent4;
					num = num4;
				}
			}
			return agent;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0006E9F0 File Offset: 0x0006CBF0
		public Agent.UnderAttackType GetUnderAttackTypeOfUnits(float timeLimit = 3f)
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			timeLimit = MBCommon.GetTotalMissionTime() - timeLimit;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				num = MathF.Max(num, ((Agent)formationUnit).LastMeleeHitTime);
				num2 = MathF.Max(num2, ((Agent)formationUnit).LastRangedHitTime);
				if (num2 >= 0f && num2 > timeLimit)
				{
					return Agent.UnderAttackType.UnderRangedAttack;
				}
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				num = MathF.Max(num, this._detachedUnits[i].LastMeleeHitTime);
				num2 = MathF.Max(num2, this._detachedUnits[i].LastRangedHitTime);
				if (num2 >= 0f && num2 > timeLimit)
				{
					return Agent.UnderAttackType.UnderRangedAttack;
				}
			}
			if (num >= 0f && num > timeLimit)
			{
				return Agent.UnderAttackType.UnderMeleeAttack;
			}
			return Agent.UnderAttackType.NotUnderAttack;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0006EAFC File Offset: 0x0006CCFC
		public Agent.MovementBehaviorType GetMovementTypeOfUnits()
		{
			float curMissionTime = MBCommon.GetTotalMissionTime();
			int retreatingCount = 0;
			int attackingCount = 0;
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				if (agent.IsAIControlled && (agent.IsRetreating() || (agent.Formation != null && agent.Formation._movementOrder.OrderType == OrderType.Retreat)))
				{
					int num = retreatingCount;
					retreatingCount = num + 1;
				}
				if (curMissionTime - agent.LastMeleeAttackTime < 3f)
				{
					int num = attackingCount;
					attackingCount = num + 1;
				}
			}, null);
			if (this.CountOfUnits > 0 && (float)retreatingCount / (float)this.CountOfUnits > 0.3f)
			{
				return Agent.MovementBehaviorType.Flee;
			}
			if (attackingCount > 0)
			{
				return Agent.MovementBehaviorType.Engaged;
			}
			return Agent.MovementBehaviorType.Idle;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0006EB68 File Offset: 0x0006CD68
		public IEnumerable<Agent> GetUnitsWithoutDetachedOnes()
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				yield return formationUnit as Agent;
			}
			List<IFormationUnit>.Enumerator enumerator = default(List<IFormationUnit>.Enumerator);
			int num;
			for (int i = 0; i < this._looseDetachedUnits.Count; i = num + 1)
			{
				yield return this._looseDetachedUnits[i];
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0006EB78 File Offset: 0x0006CD78
		public Vec2 GetWallDirectionOfRelativeFormationLocation(Agent unit)
		{
			if (unit.IsDetachedFromFormation)
			{
				return Vec2.Invalid;
			}
			Vec2? localWallDirectionOfRelativeFormationLocation = this.Arrangement.GetLocalWallDirectionOfRelativeFormationLocation(unit);
			if (localWallDirectionOfRelativeFormationLocation != null)
			{
				return this.Direction.TransformToParentUnitF(localWallDirectionOfRelativeFormationLocation.Value);
			}
			return Vec2.Invalid;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0006EBC4 File Offset: 0x0006CDC4
		public Vec2 GetDirectionOfUnit(Agent unit)
		{
			if (unit.IsDetachedFromFormation)
			{
				return unit.GetMovementDirection();
			}
			Vec2? localDirectionOfUnitOrDefault = this.Arrangement.GetLocalDirectionOfUnitOrDefault(unit);
			if (localDirectionOfUnitOrDefault != null)
			{
				return this.Direction.TransformToParentUnitF(localDirectionOfUnitOrDefault.Value);
			}
			return unit.GetMovementDirection();
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0006EC14 File Offset: 0x0006CE14
		private WorldPosition GetOrderPositionOfUnitAux(Agent unit)
		{
			WorldPosition? worldPositionOfUnitOrDefault = this.Arrangement.GetWorldPositionOfUnitOrDefault(unit);
			if (worldPositionOfUnitOrDefault != null)
			{
				return worldPositionOfUnitOrDefault.Value;
			}
			if (!this.OrderPositionIsValid)
			{
				WorldPosition worldPosition = unit.GetWorldPosition();
				Debug.Print(string.Concat(new object[]
				{
					"Formation order position is not valid. Team: ",
					this.Team.TeamIndex,
					", Formation: ",
					(int)this.FormationIndex,
					"Unit Pos: ",
					worldPosition.GetGroundVec3(),
					"Mission Mode: ",
					Mission.Current.Mode
				}), 0, Debug.DebugColor.Yellow, 17592186044416UL);
			}
			WorldPosition worldPosition2 = this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3);
			if (unit.Mission.IsFormationUnitPositionAvailable(ref worldPosition2, this.Team))
			{
				return worldPosition2;
			}
			return unit.GetWorldPosition();
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0006ECFA File Offset: 0x0006CEFA
		public MovementOrder.MovementStateEnum GetMovementState()
		{
			return this._movementOrder.MovementState;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0006ED08 File Offset: 0x0006CF08
		public WorldPosition GetOrderPositionOfUnit(Agent unit)
		{
			if (this._movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.Follow && this._movementOrder._targetAgent != null && this._movementOrder._targetAgent == unit)
			{
				return unit.GetWorldPosition();
			}
			if (unit.IsDetachedFromFormation && (this._movementOrder.MovementState != MovementOrder.MovementStateEnum.Charge || !unit.Detachment.IsLoose || unit.Mission.Mode == MissionMode.Deployment || this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.None).IsValid))
			{
				WorldFrame? detachmentFrame = this.GetDetachmentFrame(unit);
				if (detachmentFrame == null)
				{
					return WorldPosition.Invalid;
				}
				return detachmentFrame.GetValueOrDefault().Origin;
			}
			else
			{
				if (unit.Mission.IsDeploymentFinished && unit.GetAgentFlags().HasAnyFlag(AgentFlag.UnreachableViaNavMesh))
				{
					return WorldPosition.Invalid;
				}
				switch (this._movementOrder.MovementState)
				{
				case MovementOrder.MovementStateEnum.Charge:
					if (unit.Mission.Mode == MissionMode.Deployment)
					{
						return this.GetOrderPositionOfUnitAux(unit);
					}
					if (!this.OrderPositionIsValid)
					{
						WorldPosition worldPosition = unit.GetWorldPosition();
						Debug.Print(string.Concat(new object[]
						{
							"Formation order position is not valid. Team: ",
							this.Team.TeamIndex,
							", Formation: ",
							(int)this.FormationIndex,
							"Unit Pos: ",
							worldPosition.GetGroundVec3()
						}), 0, Debug.DebugColor.Yellow, 17592186044416UL);
					}
					return this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.None);
				case MovementOrder.MovementStateEnum.Hold:
					return this.GetOrderPositionOfUnitAux(unit);
				case MovementOrder.MovementStateEnum.Retreat:
					return WorldPosition.Invalid;
				case MovementOrder.MovementStateEnum.StandGround:
					return unit.GetWorldPosition();
				default:
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Formation.cs", "GetOrderPositionOfUnit", 1607);
					return WorldPosition.Invalid;
				}
			}
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0006EEC8 File Offset: 0x0006D0C8
		public Vec2 GetCurrentGlobalPositionOfUnit(Agent unit, bool blendWithOrderDirection)
		{
			if (unit.IsDetachedFromFormation)
			{
				return unit.Position.AsVec2;
			}
			Vec2? localPositionOfUnitOrDefaultWithAdjustment = this.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(unit, blendWithOrderDirection ? ((this.QuerySystem.EstimatedInterval - this.Interval) * 0.9f) : 0f);
			if (localPositionOfUnitOrDefaultWithAdjustment != null)
			{
				return (blendWithOrderDirection ? this.CurrentDirection : this.QuerySystem.EstimatedDirection).TransformToParentUnitF(localPositionOfUnitOrDefaultWithAdjustment.Value) + this.CurrentPosition;
			}
			return unit.Position.AsVec2;
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0006EF64 File Offset: 0x0006D164
		public float GetAverageMaximumMovementSpeedOfUnits()
		{
			if (this.CountOfUnitsWithoutDetachedOnes == 0)
			{
				return 0.1f;
			}
			float num = 0f;
			foreach (Agent agent in this.GetUnitsWithoutDetachedOnes())
			{
				num += agent.GetMaximumForwardUnlimitedSpeed();
			}
			return num / (float)this.CountOfUnitsWithoutDetachedOnes;
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0006EFD0 File Offset: 0x0006D1D0
		private void CacheMovementSpeedOfUnits()
		{
			float? num;
			float? num2;
			this.ArrangementOrder.GetMovementSpeedRestriction(out num, out num2);
			if (num == null && num2 == null)
			{
				num = new float?(1f);
			}
			if (num2 != null)
			{
				if (this.CountOfUnits == 0)
				{
					this.CachedMovementSpeed = 0.1f;
					return;
				}
				IEnumerable<Agent> enumerable;
				if (this.CountOfUnitsWithoutDetachedOnes != 0)
				{
					enumerable = this.GetUnitsWithoutDetachedOnes();
				}
				else
				{
					IEnumerable<Agent> enumerable2 = this._detachedUnits;
					enumerable = enumerable2;
				}
				float num3 = enumerable.Min<Agent>((Agent u) => u.WalkSpeedCached);
				this.CachedMovementSpeed = num3 * num2.Value;
				return;
			}
			else
			{
				if (this.CountOfUnits == 0)
				{
					this.CachedMovementSpeed = 0.1f;
					return;
				}
				IEnumerable<Agent> enumerable3;
				if (this.CountOfUnitsWithoutDetachedOnes != 0)
				{
					enumerable3 = this.GetUnitsWithoutDetachedOnes();
				}
				else
				{
					IEnumerable<Agent> enumerable2 = this._detachedUnits;
					enumerable3 = enumerable2;
				}
				float num4 = enumerable3.Average<Agent>((Agent u) => u.GetMaximumForwardUnlimitedSpeed());
				Formation.FormationIntegrityDataGroup cachedFormationIntegrityData = this.CachedFormationIntegrityData;
				if (cachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents < cachedFormationIntegrityData.AverageMaxUnlimitedSpeedExcludeFarAgents * 0.5f)
				{
					this.CachedMovementSpeed = num4 * num.Value;
					return;
				}
				this.CachedMovementSpeed = num4;
				return;
			}
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0006F104 File Offset: 0x0006D304
		private void CacheClosestEnemyFormation()
		{
			float num = float.MaxValue;
			this._cachedClosestEnemyFormation = null;
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.IsEnemyOf(this.Team))
				{
					foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							float num2 = formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(this.CachedAveragePosition, this.CachedMedianPosition.GetNavMeshZ(), -1f));
							if (num2 < num)
							{
								num = num2;
								this._cachedClosestEnemyFormation = formation;
							}
						}
					}
				}
			}
			this.CachedClosestEnemyFormationDistanceSquared = num;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0006F210 File Offset: 0x0006D410
		private void CacheFormationIntegrityData()
		{
			bool flag = false;
			if (this.CountOfUnitsWithoutLooseDetachedOnes > 0)
			{
				float num = 0f;
				MBReadOnlyList<IFormationUnit> allUnits = this.Arrangement.GetAllUnits();
				int num2 = 0;
				float num3 = this.QuerySystem.EstimatedInterval - this.Interval;
				foreach (IFormationUnit formationUnit in allUnits)
				{
					Agent agent = (Agent)formationUnit;
					Vec2? localPositionOfUnitOrDefaultWithAdjustment = this.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(agent, num3);
					if (localPositionOfUnitOrDefaultWithAdjustment != null)
					{
						Vec2 vec = this.QuerySystem.EstimatedDirection.TransformToParentUnitF(localPositionOfUnitOrDefaultWithAdjustment.Value) + this.CurrentPosition;
						num2++;
						num += (vec - agent.Position.AsVec2).LengthSquared;
					}
				}
				if (num2 > 0)
				{
					float num4 = num / (float)num2 * 4f;
					float num5 = 0f;
					float num6 = 0f;
					Vec2 vec2 = Vec2.Zero;
					float num7 = 0f;
					num2 = 0;
					foreach (IFormationUnit formationUnit2 in allUnits)
					{
						Agent agent2 = (Agent)formationUnit2;
						Vec2? localPositionOfUnitOrDefaultWithAdjustment2 = this.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(agent2, num3);
						if (localPositionOfUnitOrDefaultWithAdjustment2 != null)
						{
							float lengthSquared = (this.QuerySystem.EstimatedDirection.TransformToParentUnitF(localPositionOfUnitOrDefaultWithAdjustment2.Value) + this.CurrentPosition - agent2.Position.AsVec2).LengthSquared;
							if (lengthSquared < num4)
							{
								if (lengthSquared > num6)
								{
									num6 = lengthSquared;
								}
								num5 += lengthSquared;
								vec2 += agent2.AverageVelocity.AsVec2;
								num7 += agent2.GetMaximumForwardUnlimitedSpeed();
								num2++;
							}
						}
					}
					if (num2 > 0)
					{
						vec2 *= 1f / (float)num2;
						num5 /= (float)num2;
						num7 /= (float)num2;
						this.CachedFormationIntegrityData = new Formation.FormationIntegrityDataGroup(vec2, MathF.Sqrt(num5), MathF.Sqrt(num6), num7);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.CachedFormationIntegrityData = new Formation.FormationIntegrityDataGroup(Vec2.Zero, 0f, 0f, 0f);
			}
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0006F478 File Offset: 0x0006D678
		private void CacheAverageAndMedianPositionAndVelocity()
		{
			Vec2 vec = ((this.CountOfUnitsWithoutDetachedOnes > 1) ? this.GetAveragePositionOfUnits(true, true) : ((this.CountOfUnitsWithoutDetachedOnes > 0) ? this.GetAveragePositionOfUnits(true, false) : this.OrderPosition));
			float currentTime = Mission.Current.CurrentTime;
			float num = currentTime - this._lastAveragePositionCacheTime;
			if (num > 0f)
			{
				this.CachedCurrentVelocity = (vec - this.CachedAveragePosition) * (1f / num);
			}
			this._lastAveragePositionCacheTime = currentTime;
			this.CachedAveragePosition = vec;
			this.CachedMedianPosition = ((this.CountOfUnitsWithoutDetachedOnes == 0) ? ((this.CountOfUnits == 0) ? this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None) : ((this.CountOfUnits == 1) ? this.GetFirstUnit().GetWorldPosition() : this.GetMedianAgent(false, true, this.CachedAveragePosition).GetWorldPosition())) : ((this.CountOfUnitsWithoutDetachedOnes == 1) ? this.GetMedianAgent(true, false, this.CachedAveragePosition).GetWorldPosition() : this.GetMedianAgent(true, true, this.CachedAveragePosition).GetWorldPosition()));
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0006F578 File Offset: 0x0006D778
		public float GetFormationPower()
		{
			float sum = 0f;
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				sum += agent.CharacterPowerCached;
			}, null);
			return sum;
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0006F5B0 File Offset: 0x0006D7B0
		public float GetFormationMeleeFightingPower()
		{
			float sum = 0f;
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				sum += agent.CharacterPowerCached * ((this.FormationIndex == FormationClass.Ranged || this.FormationIndex == FormationClass.HorseArcher) ? 0.4f : 1f);
			}, null);
			return sum;
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0006F5F0 File Offset: 0x0006D7F0
		internal IDetachment GetDetachmentForDebug(Agent agent)
		{
			return this.Detachments.FirstOrDefault<IDetachment>((IDetachment d) => d.IsAgentUsingOrInterested(agent));
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0006F621 File Offset: 0x0006D821
		public WorldFrame? GetDetachmentFrame(Agent agent)
		{
			return agent.Detachment.GetAgentFrame(agent);
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0006F630 File Offset: 0x0006D830
		public Vec2 GetMiddleFrontUnitPositionOffset()
		{
			Vec2 localPositionOfReservedUnitPosition = this.Arrangement.GetLocalPositionOfReservedUnitPosition();
			return this.Direction.TransformToParentUnitF(localPositionOfReservedUnitPosition);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0006F658 File Offset: 0x0006D858
		public List<IFormationUnit> GetUnitsToPopWithReferencePosition(int count, Vec3 targetPosition)
		{
			int num = MathF.Min(count, this.Arrangement.UnitCount);
			List<IFormationUnit> list = ((num == 0) ? new List<IFormationUnit>() : this.Arrangement.GetUnitsToPop(num, targetPosition));
			int num2 = count - list.Count;
			if (num2 > 0)
			{
				List<Agent> list2 = this._looseDetachedUnits.Take<Agent>(num2).ToList<Agent>();
				num2 -= list2.Count;
				list.AddRange(list2);
			}
			if (num2 > 0)
			{
				IEnumerable<Agent> enumerable = this._detachedUnits.Take<Agent>(num2);
				num2 -= enumerable.Count<Agent>();
				list.AddRange(enumerable);
			}
			return list;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0006F6E4 File Offset: 0x0006D8E4
		public List<IFormationUnit> GetUnitsToPop(int count)
		{
			int num = MathF.Min(count, this.Arrangement.UnitCount);
			List<IFormationUnit> list = ((num == 0) ? new List<IFormationUnit>() : this.Arrangement.GetUnitsToPop(num));
			int num2 = count - list.Count;
			if (num2 > 0)
			{
				List<Agent> list2 = this._looseDetachedUnits.Take<Agent>(num2).ToList<Agent>();
				num2 -= list2.Count;
				list.AddRange(list2);
			}
			if (num2 > 0)
			{
				IEnumerable<Agent> enumerable = this._detachedUnits.Take<Agent>(num2);
				num2 -= enumerable.Count<Agent>();
				list.AddRange(enumerable);
			}
			return list;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0006F76E File Offset: 0x0006D96E
		public IEnumerable<ValueTuple<WorldPosition, Vec2>> GetUnavailableUnitPositionsAccordingToNewOrder(Formation simulationFormation, in WorldPosition position, in Vec2 direction, float width, int unitSpacing)
		{
			return Formation.GetUnavailableUnitPositionsAccordingToNewOrder(this, simulationFormation, position, direction, this.Arrangement, width, unitSpacing);
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0006F790 File Offset: 0x0006D990
		public void GetUnitSpawnFrameWithIndex(int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitCount, int unitSpacing, bool isMountedFormation, out WorldPosition? unitSpawnPosition, out Vec2? unitSpawnDirection)
		{
			float num;
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(null, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, unitCount, isMountedFormation, this.Index, out unitSpawnPosition, out unitSpawnDirection, out num);
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0006F7C0 File Offset: 0x0006D9C0
		public void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitSpacing, out WorldPosition? unitSpawnPosition, out Vec2? unitSpawnDirection)
		{
			float num;
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, this.Arrangement.UnitCount, this.HasAnyMountedUnit, this.Index, out unitSpawnPosition, out unitSpawnDirection, out num);
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0006F800 File Offset: 0x0006DA00
		public void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitSpacing, int overridenUnitCount, out WorldPosition? unitPosition, out Vec2? unitDirection)
		{
			float num;
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, overridenUnitCount, this.HasAnyMountedUnit, this.Index, out unitPosition, out unitDirection, out num);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0006F838 File Offset: 0x0006DA38
		public void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, float width, int unitSpacing, out WorldPosition? unitSpawnPosition, out Vec2? unitSpawnDirection, out float actualWidth)
		{
			Formation.GetUnitPositionWithIndexAccordingToNewOrder(simulationFormation, unitIndex, in formationPosition, in formationDirection, this.Arrangement, width, unitSpacing, this.Arrangement.UnitCount, this.HasAnyMountedUnit, this.Index, out unitSpawnPosition, out unitSpawnDirection, out actualWidth);
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0006F878 File Offset: 0x0006DA78
		public bool HasUnitsWithCondition(Func<Agent, bool> function)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (function((Agent)formationUnit))
				{
					return true;
				}
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				if (function(this._detachedUnits[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0006F90C File Offset: 0x0006DB0C
		public bool HasUnitsWithCondition(Func<Agent, bool> function, out Agent result)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (function((Agent)formationUnit))
				{
					result = (Agent)formationUnit;
					return true;
				}
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				if (function(this._detachedUnits[i]))
				{
					result = this._detachedUnits[i];
					return true;
				}
			}
			result = null;
			return false;
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x0006F9B8 File Offset: 0x0006DBB8
		public bool HasAnyEnemyFormationsThatIsNotEmpty()
		{
			foreach (Team team in Mission.Current.Teams)
			{
				if (team.IsEnemyOf(this.Team))
				{
					using (List<Formation>.Enumerator enumerator2 = team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.CountOfUnits > 0)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0006FA60 File Offset: 0x0006DC60
		public bool HasUnitWithConditionLimitedRandom(Func<Agent, bool> function, int startingIndex, int willBeCheckedUnitCount, out Agent resultAgent)
		{
			int unitCount = this.Arrangement.UnitCount;
			int count = this._detachedUnits.Count;
			if (unitCount + count <= willBeCheckedUnitCount)
			{
				return this.HasUnitsWithCondition(function, out resultAgent);
			}
			for (int i = 0; i < willBeCheckedUnitCount; i++)
			{
				if (startingIndex < unitCount)
				{
					int num = MBRandom.RandomInt(unitCount);
					if (function((Agent)this.Arrangement.GetAllUnits()[num]))
					{
						resultAgent = (Agent)this.Arrangement.GetAllUnits()[num];
						return true;
					}
				}
				else if (count > 0)
				{
					int num = MBRandom.RandomInt(count);
					if (function(this._detachedUnits[num]))
					{
						resultAgent = this._detachedUnits[num];
						return true;
					}
				}
			}
			resultAgent = null;
			return false;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0006FB1C File Offset: 0x0006DD1C
		public int[] CollectUnitIndices()
		{
			if (this._agentIndicesCache == null || this._agentIndicesCache.Length != this.CountOfUnits)
			{
				this._agentIndicesCache = new int[this.CountOfUnits];
			}
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				this._agentIndicesCache[num] = ((Agent)formationUnit).Index;
				num++;
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				this._agentIndicesCache[num] = this._detachedUnits[i].Index;
				num++;
			}
			return this._agentIndicesCache;
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0006FBE8 File Offset: 0x0006DDE8
		public void ApplyActionOnEachUnit(Action<Agent> action, Agent ignoreAgent = null)
		{
			if (ignoreAgent == null)
			{
				foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
				{
					Agent agent = (Agent)formationUnit;
					action(agent);
				}
				for (int i = 0; i < this._detachedUnits.Count; i++)
				{
					action(this._detachedUnits[i]);
				}
				return;
			}
			foreach (IFormationUnit formationUnit2 in this.Arrangement.GetAllUnits())
			{
				Agent agent2 = (Agent)formationUnit2;
				if (agent2 != ignoreAgent)
				{
					action(agent2);
				}
			}
			for (int j = 0; j < this._detachedUnits.Count; j++)
			{
				Agent agent3 = this._detachedUnits[j];
				if (agent3 != ignoreAgent)
				{
					action(agent3);
				}
			}
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x0006FCF8 File Offset: 0x0006DEF8
		public void ApplyActionOnEachAttachedUnit(Action<Agent> action)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				Agent agent = (Agent)formationUnit;
				action(agent);
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x0006FD58 File Offset: 0x0006DF58
		public void ApplyActionOnEachDetachedUnit(Action<Agent> action)
		{
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				action(this._detachedUnits[i]);
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0006FD90 File Offset: 0x0006DF90
		public void ApplyActionOnEachUnitViaBackupList(Action<Agent> action)
		{
			if (this.Arrangement.GetAllUnits().Count > 0)
			{
				foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits().ToArray())
				{
					action((Agent)formationUnit);
				}
			}
			if (this._detachedUnits.Count > 0)
			{
				foreach (Agent agent in this._detachedUnits.ToArray())
				{
					action(agent);
				}
			}
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0006FE14 File Offset: 0x0006E014
		public void ApplyActionOnEachUnit(Action<Agent, List<WorldPosition>> action, List<WorldPosition> list)
		{
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				action((Agent)formationUnit, list);
			}
			for (int i = 0; i < this._detachedUnits.Count; i++)
			{
				action(this._detachedUnits[i], list);
			}
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0006FE9C File Offset: 0x0006E09C
		public int CountUnitsOnNavMeshIDMod10(int navMeshID, bool includeOnlyPositionedUnits)
		{
			int num = 0;
			foreach (IFormationUnit formationUnit in this.Arrangement.GetAllUnits())
			{
				if (((Agent)formationUnit).GetCurrentNavigationFaceId() % 10 == navMeshID && (!includeOnlyPositionedUnits || this.Arrangement.GetUnpositionedUnits() == null || this.Arrangement.GetUnpositionedUnits().IndexOf(formationUnit) < 0))
				{
					num++;
				}
			}
			if (!includeOnlyPositionedUnits)
			{
				using (List<Agent>.Enumerator enumerator2 = this._detachedUnits.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetCurrentNavigationFaceId() % 10 == navMeshID)
						{
							num++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x0006FF78 File Offset: 0x0006E178
		public void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
			AgentControllerType controller = agent.Controller;
			if (oldController != AgentControllerType.Player && controller == AgentControllerType.Player)
			{
				this.HasPlayerControlledTroop = true;
				if (!GameNetwork.IsMultiplayer)
				{
					this.TryRelocatePlayerUnit();
				}
				if (!agent.IsDetachableFromFormation)
				{
					this.OnUndetachableNonPlayerUnitRemoved(agent);
					return;
				}
			}
			else if (oldController == AgentControllerType.Player && controller != AgentControllerType.Player)
			{
				this.HasPlayerControlledTroop = false;
				if (!agent.IsDetachableFromFormation)
				{
					this.OnUndetachableNonPlayerUnitAdded(agent);
				}
			}
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x0006FFD6 File Offset: 0x0006E1D6
		public void OnMassUnitTransferStart()
		{
			this.PostponeCostlyOperations = true;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x0006FFE0 File Offset: 0x0006E1E0
		public void OnMassUnitTransferEnd()
		{
			this.ReapplyFormOrder();
			this.QuerySystem.Expire();
			this.Team.QuerySystem.ExpireAfterUnitAddRemove();
			if (this._logicalClassNeedsUpdate)
			{
				this.CalculateLogicalClass();
			}
			if (this.CountOfUnits == 0)
			{
				this.RepresentativeClass = FormationClass.NumberOfAllFormations;
			}
			if (Mission.Current.IsTeleportingAgents)
			{
				this.SetPositioning(new WorldPosition?(this._orderPosition), null, null);
				this.Arrangement.OnFormationFrameChanged(false);
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				this.SetHasPendingUnitPositions(false);
			}
			this.PostponeCostlyOperations = false;
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x0007009B File Offset: 0x0006E29B
		public void OnBatchUnitRemovalStart()
		{
			this.PostponeCostlyOperations = true;
			this.Arrangement.OnBatchRemoveStart();
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000700B0 File Offset: 0x0006E2B0
		public void OnBatchUnitRemovalEnd()
		{
			this.Arrangement.OnBatchRemoveEnd();
			if (this.PostponeCostlyOperations)
			{
				this.ReapplyFormOrder();
				this.QuerySystem.ExpireAfterUnitAddRemove();
				this.Team.QuerySystem.ExpireAfterUnitAddRemove();
				if (this._logicalClassNeedsUpdate)
				{
					this.CalculateLogicalClass();
				}
				this.PostponeCostlyOperations = false;
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x00070108 File Offset: 0x0006E308
		public void OnUnitAddedOrRemoved()
		{
			if (!this.PostponeCostlyOperations)
			{
				this.ReapplyFormOrder();
				this.QuerySystem.ExpireAfterUnitAddRemove();
				Team team = this.Team;
				if (team != null)
				{
					team.QuerySystem.ExpireAfterUnitAddRemove();
				}
			}
			this.CacheAverageAndMedianPositionAndVelocity();
			this.CacheMovementSpeedOfUnits();
			this.CacheFormationIntegrityData();
			float currentTime = Mission.Current.CurrentTime;
			this._cachedMovementSpeedUpdateTimer.Reset(currentTime);
			this._cachedFormationIntegrityDataUpdateTimer.Reset(currentTime);
			this._cachedPositionAndVelocityUpdateTimer.Reset(currentTime);
			Action<Formation> onUnitCountChanged = this.OnUnitCountChanged;
			if (onUnitCountChanged == null)
			{
				return;
			}
			onUnitCountChanged(this);
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00070196 File Offset: 0x0006E396
		public void OnAgentLostMount(Agent agent)
		{
			if (!agent.IsDetachedFromFormation)
			{
				this._arrangement.OnUnitLostMount(agent);
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000701AC File Offset: 0x0006E3AC
		public void OnFormationDispersed()
		{
			this.Arrangement.OnFormationDispersed();
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.ForceUpdateCachedAndFormationValues(true, false);
			}, null);
			this.SetHasPendingUnitPositions(false);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000701E6 File Offset: 0x0006E3E6
		public void OnUnitDetachmentChanged(Agent unit, bool isOldDetachmentLoose, bool isNewDetachmentLoose)
		{
			if (isOldDetachmentLoose && !isNewDetachmentLoose)
			{
				this._looseDetachedUnits.Remove(unit);
				return;
			}
			if (!isOldDetachmentLoose && isNewDetachmentLoose)
			{
				this._looseDetachedUnits.Add(unit);
			}
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00070210 File Offset: 0x0006E410
		public void OnUndetachableNonPlayerUnitAdded(Agent unit)
		{
			if (unit.Formation == this && !unit.IsPlayerControlled)
			{
				this._undetachableNonPlayerUnitCount++;
			}
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x00070237 File Offset: 0x0006E437
		public void OnUndetachableNonPlayerUnitRemoved(Agent unit)
		{
			if (unit.Formation == this && !unit.IsPlayerControlled)
			{
				this._undetachableNonPlayerUnitCount--;
			}
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0007025E File Offset: 0x0006E45E
		public void ResetMovementOrderPositionCache()
		{
			this._movementOrder.ResetPositionCache();
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0007026C File Offset: 0x0006E46C
		public void Reset()
		{
			this.Arrangement = new LineFormation(this, true);
			this._arrangementOrderTickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._arrangementTickOccasionallyTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._cachedFormationIntegrityDataUpdateTimer = new Timer(Mission.Current.CurrentTime, 0.9f + MBRandom.RandomFloat * 0.2f, true);
			this._cachedPositionAndVelocityUpdateTimer = new Timer(Mission.Current.CurrentTime, 0.075f + MBRandom.RandomFloat * 0.05f, true);
			this._cachedMovementSpeedUpdateTimer = new Timer(Mission.Current.CurrentTime, 1.9f + MBRandom.RandomFloat * 0.2f, true);
			this._cachedClosestEnemyFormationUpdateTimer = new Timer(Mission.Current.CurrentTime, 1.4f + MBRandom.RandomFloat * 0.2f, true);
			this.ResetAux();
			this.FacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			this._enforceNotSplittableByAI = false;
			this.ContainsAgentVisuals = false;
			this.PlayerOwner = null;
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00070380 File Offset: 0x0006E580
		public IEnumerable<Formation> Split(int count = 2)
		{
			foreach (Formation formation in this.Team.FormationsIncludingEmpty)
			{
				formation.PostponeCostlyOperations = true;
			}
			IEnumerable<Formation> enumerable = this.Team.MasterOrderController.SplitFormation(this, count);
			if (enumerable.Count<Formation>() > 1 && this.Team != null)
			{
				foreach (Formation formation2 in enumerable)
				{
					formation2.QuerySystem.Expire();
				}
			}
			foreach (Formation formation3 in this.Team.FormationsIncludingEmpty)
			{
				formation3.CalculateLogicalClass();
				formation3.PostponeCostlyOperations = false;
			}
			if (this.CountOfUnits == 0)
			{
				this.RepresentativeClass = FormationClass.NumberOfAllFormations;
			}
			return enumerable;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00070490 File Offset: 0x0006E690
		public void TransferUnits(Formation target, int unitCount)
		{
			this.PostponeCostlyOperations = true;
			target.PostponeCostlyOperations = true;
			this.Team.MasterOrderController.TransferUnits(this, target, unitCount);
			this.CalculateLogicalClass();
			target.CalculateLogicalClass();
			if (this.CountOfUnits == 0)
			{
				this.RepresentativeClass = FormationClass.NumberOfAllFormations;
			}
			this.PostponeCostlyOperations = false;
			target.PostponeCostlyOperations = false;
			this.QuerySystem.Expire();
			target.QuerySystem.Expire();
			this.Team.QuerySystem.ExpireAfterUnitAddRemove();
			target.Team.QuerySystem.ExpireAfterUnitAddRemove();
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x00070520 File Offset: 0x0006E720
		public void TransferUnitsAux(Formation target, int unitCount, bool isPlayerOrder, bool useSelectivePop)
		{
			if (!isPlayerOrder && !this.IsSplittableByAI)
			{
				return;
			}
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print(string.Concat(new object[]
			{
				this.Team.Side,
				" ",
				this.FormationIndex.GetName(),
				" transfers ",
				unitCount,
				" units to ",
				target.FormationIndex.GetName()
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			if (unitCount == 0)
			{
				return;
			}
			if (target.CountOfUnits == 0)
			{
				target.CopyOrdersFrom(this);
				target.SetPositioning(new WorldPosition?(this._orderPosition), new Vec2?(this.Direction), new int?(this.UnitSpacing));
			}
			BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
			List<IFormationUnit> list;
			if (battleBannerBearersModel.GetFormationBanner(this) == null)
			{
				list = (useSelectivePop ? this.GetUnitsToPopWithReferencePosition(unitCount, target.OrderPositionIsValid ? target.OrderPosition.ToVec3(0f) : target.CachedMedianPosition.GetGroundVec3()) : this.GetUnitsToPop(unitCount).ToList<IFormationUnit>());
			}
			else
			{
				List<Agent> formationBannerBearers = battleBannerBearersModel.GetFormationBannerBearers(this);
				int num = Math.Min(this.CountOfUnits, unitCount + formationBannerBearers.Count);
				list = (useSelectivePop ? this.GetUnitsToPopWithReferencePosition(num, target.OrderPositionIsValid ? target.OrderPosition.ToVec3(0f) : target.CachedMedianPosition.GetGroundVec3()) : this.GetUnitsToPop(num).ToList<IFormationUnit>());
				foreach (Agent agent in formationBannerBearers)
				{
					if (list.Count <= unitCount)
					{
						break;
					}
					list.Remove(agent);
				}
				if (list.Count > unitCount)
				{
					int num2 = list.Count - unitCount;
					list.RemoveRange(list.Count - num2, num2);
				}
			}
			if (battleBannerBearersModel.GetFormationBanner(target) != null)
			{
				foreach (Agent agent2 in battleBannerBearersModel.GetFormationBannerBearers(target))
				{
					if (agent2.Formation == this && !list.Contains(agent2))
					{
						int num3 = list.FindIndex(delegate(IFormationUnit unit)
						{
							Agent agent3;
							return (agent3 = unit as Agent) != null && agent3.Banner == null;
						});
						if (num3 < 0)
						{
							break;
						}
						list[num3] = agent2;
					}
				}
			}
			foreach (IFormationUnit formationUnit in list)
			{
				((Agent)formationUnit).Formation = target;
			}
			this.Team.TriggerOnFormationsChanged(this);
			this.Team.TriggerOnFormationsChanged(target);
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000708E8 File Offset: 0x0006EAE8
		[Conditional("DEBUG")]
		public void DebugArrangements()
		{
			foreach (Team team in Mission.Current.Teams)
			{
				foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.AgentVisuals.SetContourColor(null, true);
						}, null);
					}
				}
			}
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.AgentVisuals.SetContourColor(new uint?(4294901760U), true);
			}, null);
			Vec3 vec = this.Direction.ToVec3(0f);
			vec.RotateAboutZ(1.5707964f);
			bool isSimulationFormation = this.IsSimulationFormation;
			vec * this.Width * 0.5f;
			this.Direction.ToVec3(0f) * this.Depth * 0.5f;
			bool orderPositionIsValid = this.OrderPositionIsValid;
			this.CachedMedianPosition.SetVec2(this.CurrentPosition);
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				WorldPosition orderPositionOfUnit = this.GetOrderPositionOfUnit(agent);
				if (orderPositionOfUnit.IsValid)
				{
					Vec2 vec2 = this.GetDirectionOfUnit(agent);
					vec2.Normalize();
					vec2 *= 0.1f;
					orderPositionOfUnit.GetGroundVec3() + vec2.ToVec3(0f);
					orderPositionOfUnit.GetGroundVec3() - vec2.LeftVec().ToVec3(0f);
					orderPositionOfUnit.GetGroundVec3() + vec2.LeftVec().ToVec3(0f);
					string.Concat(new object[]
					{
						"(",
						((IFormationUnit)agent).FormationFileIndex,
						",",
						((IFormationUnit)agent).FormationRankIndex,
						")"
					});
				}
			}, null);
			bool orderPositionIsValid2 = this.OrderPositionIsValid;
			foreach (IDetachment detachment in this.Detachments)
			{
				UsableMachine usableMachine = detachment as UsableMachine;
				RangedSiegeWeapon rangedSiegeWeapon = detachment as RangedSiegeWeapon;
			}
			if (this.Arrangement is ColumnFormation)
			{
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.GetFollowedUnit();
					string.Concat(new object[]
					{
						"(",
						((IFormationUnit)agent).FormationFileIndex,
						",",
						((IFormationUnit)agent).FormationRankIndex,
						")"
					});
				}, null);
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00070AE4 File Offset: 0x0006ECE4
		public void AddUnit(Agent unit)
		{
			bool countOfUnits = this.CountOfUnits != 0;
			if (this.Arrangement.AddUnit(unit) && Mission.Current.HasMissionBehavior<AmmoSupplyLogic>() && Mission.Current.GetMissionBehavior<AmmoSupplyLogic>().IsAgentEligibleForAmmoSupply(unit))
			{
				unit.SetScriptedCombatFlags(unit.GetScriptedCombatFlags() | Agent.AISpecialCombatModeFlags.IgnoreAmmoLimitForRangeCalculation);
				unit.ResetAiWaitBeforeShootFactor();
				unit.UpdateAgentStats();
			}
			if (unit.IsPlayerControlled)
			{
				this.HasPlayerControlledTroop = true;
			}
			if (unit.IsPlayerTroop)
			{
				this.IsPlayerTroopInFormation = true;
			}
			if (!unit.IsDetachableFromFormation && !unit.IsPlayerControlled)
			{
				this.OnUndetachableNonPlayerUnitAdded(unit);
			}
			if (unit.Character != null)
			{
				FormationClass formationClass = this.Team.Mission.GetAgentTroopClass(this.Team.Side, unit.Character).DefaultClass();
				this._logicalClassCounts[(int)formationClass]++;
				if (this._logicalClass != formationClass)
				{
					if (this.PostponeCostlyOperations)
					{
						this._logicalClassNeedsUpdate = true;
					}
					else
					{
						this.CalculateLogicalClass();
						this._logicalClassNeedsUpdate = false;
					}
				}
			}
			this._movementOrder.OnUnitJoinOrLeave(this, unit, true);
			Formation targetFormation = this.TargetFormation;
			unit.SetTargetFormationIndex((targetFormation != null) ? targetFormation.Index : (-1));
			unit.SetFiringOrder(this.FiringOrder.OrderEnum);
			unit.SetRidingOrder(this.RidingOrder.OrderEnum);
			this.OnUnitAddedOrRemoved();
			Action<Formation, Agent> onUnitAdded = this.OnUnitAdded;
			if (onUnitAdded != null)
			{
				onUnitAdded(this, unit);
			}
			if (!countOfUnits && this.CountOfUnits > 0)
			{
				TeamAIComponent teamAI = this.Team.TeamAI;
				if (teamAI == null)
				{
					return;
				}
				teamAI.OnUnitAddedToFormationForTheFirstTime(this);
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00070C60 File Offset: 0x0006EE60
		public void RemoveUnit(Agent unit)
		{
			if (unit.IsDetachedFromFormation)
			{
				unit.Detachment.RemoveAgent(unit);
				this._detachedUnits.Remove(unit);
				this._looseDetachedUnits.Remove(unit);
				unit.Detachment = null;
				unit.SetDetachmentWeight(-1f);
			}
			else
			{
				this.Arrangement.RemoveUnit(unit);
			}
			if (unit.Character != null)
			{
				FormationClass formationClass = this.Team.Mission.GetAgentTroopClass(this.Team.Side, unit.Character).DefaultClass();
				this._logicalClassCounts[(int)formationClass]--;
				if (this._logicalClass == formationClass)
				{
					if (this.PostponeCostlyOperations)
					{
						this._logicalClassNeedsUpdate = true;
					}
					else
					{
						this.CalculateLogicalClass();
						this._logicalClassNeedsUpdate = false;
					}
				}
			}
			if (unit.IsPlayerTroop)
			{
				this.IsPlayerTroopInFormation = false;
			}
			if (unit.IsPlayerControlled)
			{
				this.HasPlayerControlledTroop = false;
			}
			if (unit == this.Captain && !unit.CanLeadFormationsRemotely)
			{
				this.Captain = null;
			}
			if (!unit.IsDetachableFromFormation && !unit.IsPlayerControlled)
			{
				this.OnUndetachableNonPlayerUnitRemoved(unit);
			}
			if (Mission.Current.Mode != MissionMode.Deployment && !this.IsAIControlled && this.CountOfUnits == 0)
			{
				this.SetControlledByAI(true, false);
			}
			this._movementOrder.OnUnitJoinOrLeave(this, unit, false);
			unit.SetTargetFormationIndex(-1);
			unit.SetFiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill);
			unit.SetRidingOrder(RidingOrder.RidingOrderEnum.Free);
			this.OnUnitAddedOrRemoved();
			Action<Formation, Agent> onUnitRemoved = this.OnUnitRemoved;
			if (onUnitRemoved == null)
			{
				return;
			}
			onUnitRemoved(this, unit);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00070DCF File Offset: 0x0006EFCF
		public void DetachUnit(Agent unit, bool isLoose)
		{
			this.Arrangement.RemoveUnit(unit);
			this._detachedUnits.Add(unit);
			if (isLoose)
			{
				this._looseDetachedUnits.Add(unit);
			}
			unit.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefaultDetached);
			this.OnUnitAttachedOrDetached();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00070E08 File Offset: 0x0006F008
		public void AttachUnit(Agent unit)
		{
			this._detachedUnits.Remove(unit);
			this._looseDetachedUnits.Remove(unit);
			this.Arrangement.AddUnit(unit);
			unit.Detachment = null;
			unit.SetDetachmentWeight(-1f);
			this._movementOrder.OnUnitJoinOrLeave(this, unit, true);
			this.OnUnitAttachedOrDetached();
			Action<Formation, Agent> onUnitAttached = this.OnUnitAttached;
			if (onUnitAttached == null)
			{
				return;
			}
			onUnitAttached(this, unit);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00070E74 File Offset: 0x0006F074
		public void SwitchUnitLocations(Agent firstUnit, Agent secondUnit)
		{
			if (!firstUnit.IsDetachedFromFormation && !secondUnit.IsDetachedFromFormation && (((IFormationUnit)firstUnit).FormationFileIndex != -1 || ((IFormationUnit)secondUnit).FormationFileIndex != -1))
			{
				if (((IFormationUnit)firstUnit).FormationFileIndex == -1)
				{
					this.Arrangement.SwitchUnitLocationsWithUnpositionedUnit(secondUnit, firstUnit);
					return;
				}
				if (((IFormationUnit)secondUnit).FormationFileIndex == -1)
				{
					this.Arrangement.SwitchUnitLocationsWithUnpositionedUnit(firstUnit, secondUnit);
					return;
				}
				this.Arrangement.SwitchUnitLocations(firstUnit, secondUnit);
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x00070EDE File Offset: 0x0006F0DE
		public void ForceCalculateCaches()
		{
			this.CacheAverageAndMedianPositionAndVelocity();
			this.CacheClosestEnemyFormation();
			this.CacheFormationIntegrityData();
			this.CacheMovementSpeedOfUnits();
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00070EF8 File Offset: 0x0006F0F8
		public void Tick(float dt)
		{
			float currentTime = Mission.Current.CurrentTime;
			if (this._cachedPositionAndVelocityUpdateTimer.Check(currentTime))
			{
				this.CacheAverageAndMedianPositionAndVelocity();
			}
			if (this._cachedClosestEnemyFormationUpdateTimer.Check(currentTime) || this._cachedClosestEnemyFormation == null || this._cachedClosestEnemyFormation.CountOfUnits == 0)
			{
				this.CacheClosestEnemyFormation();
			}
			if (this._cachedFormationIntegrityDataUpdateTimer.Check(currentTime))
			{
				this.CacheFormationIntegrityData();
			}
			if (this._cachedMovementSpeedUpdateTimer.Check(currentTime))
			{
				this.CacheMovementSpeedOfUnits();
			}
			if (this.Team.HasTeamAi && (this.IsAIControlled || this.Team.IsPlayerSergeant))
			{
				this.AI.Tick();
			}
			else
			{
				this.IsAITickedAfterSplit = true;
			}
			int num = 0;
			while (!this._movementOrder.IsApplicable(this) && num++ < 10)
			{
				this.SetMovementOrder(this._movementOrder.GetSubstituteOrder(this));
			}
			Formation targetFormation = this.TargetFormation;
			if (targetFormation != null && targetFormation.CountOfUnits <= 0)
			{
				this.TargetFormation = null;
			}
			if (this._arrangementOrderTickOccasionallyTimer.Check(currentTime))
			{
				this.ArrangementOrder.TickOccasionally(this);
			}
			if (this._arrangementTickOccasionallyTimer.Check(currentTime))
			{
				this.Arrangement.OnTickOccasionally();
			}
			this._movementOrder.Tick(this);
			WorldPosition worldPosition = this._movementOrder.CreateNewOrderWorldPositionMT(this, WorldPosition.WorldPositionEnforcedCache.None);
			Vec2 direction = this.FacingOrder.GetDirection(this, this._movementOrder._targetAgent);
			if (worldPosition.IsValid || direction.IsValid)
			{
				this.SetPositioning(new WorldPosition?(worldPosition), new Vec2?(direction), null);
			}
			this.TickDetachments(dt);
			Action<Formation> onTick = this.OnTick;
			if (onTick != null)
			{
				onTick(this);
			}
			if (this._hasPendingUnitPositions)
			{
				this.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				this.SetHasPendingUnitPositions(false);
			}
			this.SmoothAverageUnitPosition(dt);
			if (this._isArrangementShapeChanged)
			{
				this._isArrangementShapeChanged = false;
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000710F9 File Offset: 0x0006F2F9
		public void SetHasPendingUnitPositions(bool hasPendingUnitPositions)
		{
			this._hasPendingUnitPositions = hasPendingUnitPositions;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00071104 File Offset: 0x0006F304
		public void JoinDetachment(IDetachment detachment)
		{
			if (!this.Team.DetachmentManager.ContainsDetachment(detachment))
			{
				this.Team.DetachmentManager.MakeDetachment(detachment);
			}
			this._detachments.Add(detachment);
			this.Team.DetachmentManager.OnFormationJoinDetachment(this, detachment);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00071153 File Offset: 0x0006F353
		public void FormAttackEntityDetachment(GameEntity targetEntity)
		{
			this.AttackEntityOrderSecondaryDetachment = new AttackEntityOrderSecondaryDetachment(targetEntity);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00071161 File Offset: 0x0006F361
		public void LeaveDetachment(IDetachment detachment)
		{
			detachment.OnFormationLeave(this);
			this._detachments.Remove(detachment);
			this.Team.DetachmentManager.OnFormationLeaveDetachment(this, detachment);
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00071189 File Offset: 0x0006F389
		public void DisbandAttackEntityDetachment()
		{
			if (this.AttackEntityOrderSecondaryDetachment != null)
			{
				this.AttackEntityOrderSecondaryDetachment.Disband(this);
				this.AttackEntityOrderSecondaryDetachment = null;
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000711A8 File Offset: 0x0006F3A8
		public void Rearrange(IFormationArrangement arrangement)
		{
			if (this.Arrangement.GetType() == arrangement.GetType())
			{
				return;
			}
			IFormationArrangement arrangement2 = this.Arrangement;
			this.Arrangement = arrangement;
			arrangement2.RearrangeTo(arrangement);
			arrangement.RearrangeFrom(arrangement2);
			arrangement2.RearrangeTransferUnits(arrangement);
			this.ReapplyFormOrder();
			this._movementOrder.OnArrangementChanged(this);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x00071208 File Offset: 0x0006F408
		public void TickForColumnArrangementInitialPositioning(Formation formation)
		{
			if (!this.IsDeployment && (this.CachedFormationIntegrityData.MaxDeviationOfPositionExcludeFarAgents < this.Interval * 0.75f || this.OrderPosition.DistanceSquared(this.CurrentPosition) > this.Arrangement.RankDepth * this.Arrangement.RankDepth))
			{
				this.ArrangementOrder.RearrangeAux(this, true);
			}
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x00071274 File Offset: 0x0006F474
		public float CalculateFormationDirectionEnforcingFactorForRank(int rankIndex)
		{
			if (rankIndex == -1)
			{
				return 0f;
			}
			return this.ArrangementOrder.CalculateFormationDirectionEnforcingFactorForRank(rankIndex, this.Arrangement.RankCount);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000712A5 File Offset: 0x0006F4A5
		public void BeginSpawn(int unitCount, bool isMounted)
		{
			this.IsSpawning = true;
			this.OverridenUnitCount = new int?(unitCount);
			this._overridenHasAnyMountedUnit = new bool?(isMounted);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000712C8 File Offset: 0x0006F4C8
		public void EndSpawn()
		{
			this.IsSpawning = false;
			this.OverridenUnitCount = null;
			this._overridenHasAnyMountedUnit = null;
			this.Arrangement.UpdateLocalPositionErrors(true);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x00071303 File Offset: 0x0006F503
		public override int GetHashCode()
		{
			return (int)(this.Team.TeamIndex * 10 + this.FormationIndex);
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x0007131A File Offset: 0x0006F51A
		internal bool IsUnitDetachedForDebug(Agent unit)
		{
			return this._detachedUnits.Contains(unit);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x00071328 File Offset: 0x0006F528
		internal IEnumerable<IFormationUnit> GetUnitsToPopWithPriorityFunction(int count, Func<Agent, int> priorityFunction, List<Agent> excludedHeroes, bool excludeBannerman)
		{
			Formation.<>c__DisplayClass387_0 CS$<>8__locals1 = new Formation.<>c__DisplayClass387_0();
			CS$<>8__locals1.excludedHeroes = excludedHeroes;
			CS$<>8__locals1.excludeBannerman = excludeBannerman;
			CS$<>8__locals1.priorityFunction = priorityFunction;
			List<IFormationUnit> list = new List<IFormationUnit>();
			if (count <= 0)
			{
				return list;
			}
			CS$<>8__locals1.selectCondition = (Agent agent) => !CS$<>8__locals1.excludedHeroes.Contains(agent3) && (!CS$<>8__locals1.excludeBannerman || agent3.Banner == null);
			List<Agent> list2 = (from unit in this._arrangement.GetAllUnits().Concat<IFormationUnit>(this._detachedUnits).Where<IFormationUnit>(delegate(IFormationUnit unit)
				{
					Agent agent3;
					return (agent3 = unit as Agent) != null && CS$<>8__locals1.selectCondition(agent3);
				})
				select unit as Agent).ToList<Agent>();
			if (list2.IsEmpty<Agent>())
			{
				return list;
			}
			int num = count;
			CS$<>8__locals1.bestFit = int.MaxValue;
			while (num > 0 && CS$<>8__locals1.bestFit > 0 && list2.Count > 0)
			{
				Formation.<>c__DisplayClass387_1 CS$<>8__locals2 = new Formation.<>c__DisplayClass387_1();
				Formation.<>c__DisplayClass387_0 CS$<>8__locals3 = CS$<>8__locals1;
				IEnumerable<Agent> enumerable = list2;
				Func<Agent, int> func;
				if ((func = CS$<>8__locals1.<>9__3) == null)
				{
					func = (CS$<>8__locals1.<>9__3 = (Agent unit) => CS$<>8__locals1.priorityFunction(unit));
				}
				CS$<>8__locals3.bestFit = enumerable.Max<Agent>(func);
				Formation.<>c__DisplayClass387_1 CS$<>8__locals4 = CS$<>8__locals2;
				Func<IFormationUnit, bool> func2;
				if ((func2 = CS$<>8__locals1.<>9__4) == null)
				{
					func2 = (CS$<>8__locals1.<>9__4 = delegate(IFormationUnit unit)
					{
						Agent agent2;
						return (agent2 = unit as Agent) != null && CS$<>8__locals1.selectCondition(agent2) && CS$<>8__locals1.priorityFunction(agent2) == CS$<>8__locals1.bestFit;
					});
				}
				CS$<>8__locals4.bestFitCondition = func2;
				int num2 = Math.Min(num, this._arrangement.GetAllUnits().Count<IFormationUnit>((IFormationUnit unit) => CS$<>8__locals2.bestFitCondition(unit)));
				if (num2 > 0)
				{
					IEnumerable<IFormationUnit> toPop2 = this._arrangement.GetUnitsToPopWithCondition(num2, CS$<>8__locals2.bestFitCondition);
					if (!toPop2.IsEmpty<IFormationUnit>())
					{
						list.AddRange(toPop2);
						num -= toPop2.Count<IFormationUnit>();
						list2.RemoveAll((Agent unit) => toPop2.Contains(unit));
					}
				}
				if (num > 0)
				{
					IEnumerable<Agent> toPop3 = this._looseDetachedUnits.Where<Agent>((Agent agent) => CS$<>8__locals2.bestFitCondition(agent)).Take<Agent>(num);
					if (!toPop3.IsEmpty<Agent>())
					{
						list.AddRange(toPop3);
						num -= toPop3.Count<Agent>();
						list2.RemoveAll((Agent unit) => toPop3.Contains(unit));
					}
				}
				if (num > 0)
				{
					IEnumerable<Agent> toPop = this._detachedUnits.Where<Agent>((Agent agent) => CS$<>8__locals2.bestFitCondition(agent)).Take<Agent>(num);
					if (!toPop.IsEmpty<Agent>())
					{
						list.AddRange(toPop);
						num -= toPop.Count<Agent>();
						list2.RemoveAll((Agent unit) => toPop.Contains(unit));
					}
				}
			}
			return list;
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000715B8 File Offset: 0x0006F7B8
		internal void TransferUnitsWithPriorityFunction(Formation target, int unitCount, Func<Agent, int> priorityFunction, bool excludeBannerman, List<Agent> excludedAgents)
		{
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			MBDebug.Print(string.Concat(new object[]
			{
				this.Team.Side.ToString(),
				" ",
				this.FormationIndex.GetName(),
				" transfers ",
				unitCount,
				" units to ",
				target.FormationIndex.GetName()
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			if (unitCount == 0)
			{
				return;
			}
			if (target.CountOfUnits == 0)
			{
				target.CopyOrdersFrom(this);
				target.SetPositioning(new WorldPosition?(this._orderPosition), new Vec2?(this.Direction), new int?(this.UnitSpacing));
			}
			foreach (IFormationUnit formationUnit in new List<IFormationUnit>(this.GetUnitsToPopWithPriorityFunction(unitCount, priorityFunction, excludedAgents, excludeBannerman)))
			{
				((Agent)formationUnit).Formation = target;
			}
			this.Team.TriggerOnFormationsChanged(this);
			this.Team.TriggerOnFormationsChanged(target);
			MBDebug.Print(string.Concat(new object[]
			{
				this.FormationIndex.GetName(),
				" has ",
				this.CountOfUnits,
				" units, ",
				target.FormationIndex.GetName(),
				" has ",
				target.CountOfUnits,
				" units"
			}), 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000717C8 File Offset: 0x0006F9C8
		private IFormationUnit GetClosestUnitToAux(Vec2 position, MBReadOnlyList<IFormationUnit> unitsWithSpaces, float? maxDistance)
		{
			if (unitsWithSpaces == null)
			{
				unitsWithSpaces = this.Arrangement.GetAllUnits();
			}
			IFormationUnit formationUnit = null;
			float num = ((maxDistance != null) ? (maxDistance.Value * maxDistance.Value) : float.MaxValue);
			for (int i = 0; i < unitsWithSpaces.Count; i++)
			{
				IFormationUnit formationUnit2 = unitsWithSpaces[i];
				if (formationUnit2 != null)
				{
					float num2 = ((Agent)formationUnit2).Position.AsVec2.DistanceSquared(position);
					if (num > num2)
					{
						num = num2;
						formationUnit = formationUnit2;
					}
				}
			}
			return formationUnit;
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x00071850 File Offset: 0x0006FA50
		private void CopyOrdersFrom(Formation target)
		{
			this.SetMovementOrder(target._movementOrder);
			this.SetFormOrder(target.FormOrder, true);
			this.SetPositioning(null, null, new int?(target.UnitSpacing));
			this.SetRidingOrder(target.RidingOrder);
			this.SetFiringOrder(target.FiringOrder);
			this.SetControlledByAI(target.IsAIControlled || !target.Team.IsPlayerGeneral, false);
			if (target.AI.Side != FormationAI.BehaviorSide.BehaviorSideNotSet)
			{
				this.AI.Side = target.AI.Side;
			}
			this.SetMovementOrder(target._movementOrder);
			this.TargetFormation = target.TargetFormation;
			this.FacingOrder = target.FacingOrder;
			this.SetArrangementOrder(target.ArrangementOrder);
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x00071928 File Offset: 0x0006FB28
		private void TickDetachments(float dt)
		{
			if (!this.IsDeployment)
			{
				for (int i = this._detachments.Count - 1; i >= 0; i--)
				{
					IDetachment detachment = this._detachments[i];
					UsableMachine usableMachine = detachment as UsableMachine;
					if (((usableMachine != null) ? usableMachine.Ai : null) != null)
					{
						usableMachine.Ai.Tick(null, (usableMachine.UserFormations.Count > 1) ? this : null, this.Team, dt);
						if (usableMachine.Ai.HasActionCompleted || (usableMachine.IsDisabledForBattleSideAI(this.Team.Side) && usableMachine.ShouldAutoLeaveDetachmentWhenDisabled(this.Team.Side)))
						{
							this.LeaveDetachment(detachment);
						}
					}
				}
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000719E0 File Offset: 0x0006FBE0
		[Conditional("DEBUG")]
		private void TickOrderDebug()
		{
			WorldPosition cachedMedianPosition = this.CachedMedianPosition;
			WorldPosition worldPosition = this.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
			cachedMedianPosition.SetVec2(this.CachedAveragePosition);
			if (worldPosition.IsValid)
			{
				if (!this._movementOrder.GetPosition(this).IsValid)
				{
					if (this.AI != null)
					{
						BehaviorComponent activeBehavior = this.AI.ActiveBehavior;
						return;
					}
				}
				else if (this.AI != null)
				{
					BehaviorComponent activeBehavior2 = this.AI.ActiveBehavior;
					return;
				}
			}
			else if (this.AI != null)
			{
				BehaviorComponent activeBehavior3 = this.AI.ActiveBehavior;
			}
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x00071A66 File Offset: 0x0006FC66
		[Conditional("DEBUG")]
		private void TickDebug(float dt)
		{
			if (!MBDebug.IsDisplayingHighLevelAI)
			{
				return;
			}
			if (!this.IsSimulationFormation && this._movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.FollowEntity)
			{
				string name = this._movementOrder.TargetEntity.Name;
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x00071A97 File Offset: 0x0006FC97
		private void OnUnitAttachedOrDetached()
		{
			this.ReapplyFormOrder();
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x00071A9F File Offset: 0x0006FC9F
		[Conditional("DEBUG")]
		private void DebugAssertDetachments()
		{
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00071AA1 File Offset: 0x0006FCA1
		private void SetOrderPosition(WorldPosition pos)
		{
			this._orderPosition = pos;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x00071AAA File Offset: 0x0006FCAA
		private int GetHeroPointForCaptainSelection(Agent agent)
		{
			return agent.Character.Level + 100 * agent.Character.GetSkillValue(DefaultSkills.Charm);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00071ACB File Offset: 0x0006FCCB
		private void OnCaptainChanged()
		{
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.UpdateAgentProperties();
			}, null);
			Mission.Current.OnFormationCaptainChanged(this);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x00071AFE File Offset: 0x0006FCFE
		private void UpdateAgentDrivenPropertiesBasedOnOrderDefensiveness()
		{
			this.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.Defensiveness = (float)this._formationOrderDefensivenessFactor;
			}, null);
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00071B14 File Offset: 0x0006FD14
		private void ResetAux()
		{
			if (this._detachments != null)
			{
				for (int i = this._detachments.Count - 1; i >= 0; i--)
				{
					this.LeaveDetachment(this._detachments[i]);
				}
			}
			else
			{
				this._detachments = new MBList<IDetachment>();
			}
			this._detachedUnits = new MBList<Agent>();
			this._looseDetachedUnits = new MBList<Agent>();
			this.AttackEntityOrderSecondaryDetachment = null;
			this.AI = new FormationAI(this);
			this.QuerySystem = new FormationQuerySystem(this);
			this.SetPositioning(null, new Vec2?(Vec2.Forward), new int?(1));
			this.SetMovementOrder(MovementOrder.MovementOrderStop);
			if (this._overridenHasAnyMountedUnit != null)
			{
				bool? overridenHasAnyMountedUnit = this._overridenHasAnyMountedUnit;
				bool flag = true;
				if ((overridenHasAnyMountedUnit.GetValueOrDefault() == flag) & (overridenHasAnyMountedUnit != null))
				{
					this.SetArrangementOrder(ArrangementOrder.ArrangementOrderSkein);
					goto IL_00EC;
				}
			}
			this.SetFormOrder(FormOrder.FormOrderWide, true);
			this.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			IL_00EC:
			this.SetRidingOrder(RidingOrder.RidingOrderFree);
			this.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			this.Width = 0f * (this.Interval + this.UnitDiameter) + this.UnitDiameter;
			this.HasBeenPositioned = false;
			this._currentSpawnIndex = 0;
			this.IsPlayerTroopInFormation = false;
			this.HasPlayerControlledTroop = false;
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x00071C5F File Offset: 0x0006FE5F
		private void ResetForSimulation()
		{
			this.Arrangement.Reset();
			this.ResetAux();
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00071C74 File Offset: 0x0006FE74
		private void TryRelocatePlayerUnit()
		{
			if (this.HasPlayerControlledTroop || this.IsPlayerTroopInFormation)
			{
				IFormationUnit playerUnit = this.Arrangement.GetPlayerUnit();
				if (playerUnit != null && playerUnit.FormationFileIndex >= 0 && playerUnit.FormationRankIndex >= 0)
				{
					this.Arrangement.SwitchUnitLocationsWithBackMostUnit(playerUnit);
				}
			}
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00071CC0 File Offset: 0x0006FEC0
		private void ReapplyFormOrder()
		{
			FormOrder formOrder = this.FormOrder;
			if (this.FormOrder.OrderEnum == FormOrder.FormOrderEnum.Custom && this.ArrangementOrder.OrderEnum != ArrangementOrder.ArrangementOrderEnum.Circle)
			{
				formOrder.CustomFlankWidth = this.Arrangement.FlankWidth;
			}
			this.SetFormOrder(formOrder, false);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x00071D09 File Offset: 0x0006FF09
		private float CalculateDesiredWidth()
		{
			return MathF.Max(0f, (float)(this._desiredFileCount - 1) * (this.UnitDiameter + this.Interval) + this.UnitDiameter);
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x00071D34 File Offset: 0x0006FF34
		private void CalculateLogicalClass()
		{
			int num = 0;
			FormationClass formationClass = FormationClass.NumberOfAllFormations;
			for (int i = 0; i < this._logicalClassCounts.Length; i++)
			{
				FormationClass formationClass2 = (FormationClass)i;
				int num2 = this._logicalClassCounts[i];
				if (num2 > num)
				{
					num = num2;
					formationClass = formationClass2;
				}
			}
			this._logicalClass = formationClass;
			if (this._logicalClass != FormationClass.NumberOfAllFormations)
			{
				this.RepresentativeClass = this._logicalClass;
			}
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x00071D8C File Offset: 0x0006FF8C
		private void SmoothAverageUnitPosition(float dt)
		{
			this._smoothedAverageUnitPosition = ((!this._smoothedAverageUnitPosition.IsValid) ? this.CachedAveragePosition : Vec2.Lerp(this._smoothedAverageUnitPosition, this.CachedAveragePosition, dt * 3f));
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00071DC1 File Offset: 0x0006FFC1
		private void Arrangement_OnWidthChanged()
		{
			Action<Formation> onWidthChanged = this.OnWidthChanged;
			if (onWidthChanged == null)
			{
				return;
			}
			onWidthChanged(this);
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x00071DD4 File Offset: 0x0006FFD4
		private void Arrangement_OnShapeChanged()
		{
			this._orderLocalAveragePositionIsDirty = true;
			this._isArrangementShapeChanged = true;
			if (!GameNetwork.IsMultiplayer)
			{
				this.TryRelocatePlayerUnit();
			}
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x00071DF4 File Offset: 0x0006FFF4
		public static float GetLastSimulatedFormationsOccupationWidthIfLesserThanActualWidth(Formation simulationFormation)
		{
			float occupationWidth = simulationFormation.Arrangement.GetOccupationWidth(simulationFormation.OverridenUnitCount.GetValueOrDefault());
			if (simulationFormation.Width > occupationWidth)
			{
				return occupationWidth;
			}
			return -1f;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x00071E2C File Offset: 0x0007002C
		public static List<WorldFrame> GetFormationFramesForBeforeFormationCreation(float width, int manCount, bool areMounted, WorldPosition spawnOrigin, Mat3 spawnRotation)
		{
			List<Formation.AgentArrangementData> list = new List<Formation.AgentArrangementData>();
			Formation formation = new Formation(null, -1);
			formation.SetOrderPosition(spawnOrigin);
			formation.Direction = spawnRotation.f.AsVec2.Normalized();
			LineFormation lineFormation = new LineFormation(formation, true);
			lineFormation.Width = width;
			for (int i = 0; i < manCount; i++)
			{
				list.Add(new Formation.AgentArrangementData(i, lineFormation));
			}
			lineFormation.OnFormationFrameChanged(false);
			foreach (Formation.AgentArrangementData agentArrangementData in list)
			{
				lineFormation.AddUnit(agentArrangementData);
			}
			List<WorldFrame> list2 = new List<WorldFrame>();
			int cachedOrderedAndAvailableUnitPositionIndicesCount = lineFormation.GetCachedOrderedAndAvailableUnitPositionIndicesCount();
			for (int j = 0; j < cachedOrderedAndAvailableUnitPositionIndicesCount; j++)
			{
				Vec2i cachedOrderedAndAvailableUnitPositionIndexAt = lineFormation.GetCachedOrderedAndAvailableUnitPositionIndexAt(j);
				WorldPosition globalPositionAtIndex = lineFormation.GetGlobalPositionAtIndex(cachedOrderedAndAvailableUnitPositionIndexAt.X, cachedOrderedAndAvailableUnitPositionIndexAt.Y);
				list2.Add(new WorldFrame(spawnRotation, globalPositionAtIndex));
			}
			return list2;
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00071F30 File Offset: 0x00070130
		public static float GetDefaultUnitDiameter(bool isMounted)
		{
			if (isMounted)
			{
				return ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.QuadrupedalRadius) * 2f;
			}
			return ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.BipedalRadius) * 2f;
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00071F58 File Offset: 0x00070158
		public static float GetDefaultMinimumUnitInterval(bool isMounted)
		{
			if (!isMounted)
			{
				return Formation.InfantryInterval(0);
			}
			return Formation.CavalryInterval(0);
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x00071F6A File Offset: 0x0007016A
		public static float GetDefaultUnitInterval(bool isMounted, int unitSpacing)
		{
			if (!isMounted)
			{
				return Formation.InfantryInterval(unitSpacing);
			}
			return Formation.CavalryInterval(unitSpacing);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00071F7C File Offset: 0x0007017C
		public static float GetDefaultMinimumUnitDistance(bool isMounted)
		{
			if (!isMounted)
			{
				return Formation.InfantryDistance(0);
			}
			return Formation.CavalryDistance(0);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00071F8E File Offset: 0x0007018E
		public static float GetDefaultUnitDistance(bool isMounted, int unitSpacing)
		{
			if (!isMounted)
			{
				return Formation.InfantryDistance(unitSpacing);
			}
			return Formation.CavalryDistance(unitSpacing);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x00071FA0 File Offset: 0x000701A0
		public static float GetDefaultFileWidth(int fileUnitCount, int unitSpacing, bool isMounted)
		{
			float defaultUnitInterval = Formation.GetDefaultUnitInterval(isMounted, unitSpacing);
			float defaultUnitDiameter = Formation.GetDefaultUnitDiameter(isMounted);
			return (float)(fileUnitCount - 1) * (defaultUnitInterval + defaultUnitDiameter);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00071FC4 File Offset: 0x000701C4
		public static float GetDefaultRankDepth(int rankUnitCount, int unitSpacing, bool isMounted)
		{
			float defaultUnitDistance = Formation.GetDefaultUnitDistance(isMounted, unitSpacing);
			float defaultUnitDiameter = Formation.GetDefaultUnitDiameter(isMounted);
			return (float)(rankUnitCount - 1) * (defaultUnitDistance + defaultUnitDiameter);
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x00071FE8 File Offset: 0x000701E8
		public static float InfantryInterval(int unitSpacing)
		{
			return 0.38f * (float)unitSpacing;
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x00071FF2 File Offset: 0x000701F2
		public static float CavalryInterval(int unitSpacing)
		{
			return 0.18f + 0.32f * (float)unitSpacing;
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00072002 File Offset: 0x00070202
		public static float InfantryDistance(int unitSpacing)
		{
			return 0.4f * (float)unitSpacing;
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x0007200C File Offset: 0x0007020C
		public static float CavalryDistance(int unitSpacing)
		{
			return 1.7f + 0.3f * (float)unitSpacing;
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x0007201C File Offset: 0x0007021C
		public static bool IsDefenseRelatedAIDrivenComponent(DrivenProperty drivenProperty)
		{
			return drivenProperty == DrivenProperty.AIDecideOnAttackChance || drivenProperty == DrivenProperty.AIAttackOnDecideChance || drivenProperty == DrivenProperty.AIAttackOnParryChance || drivenProperty == DrivenProperty.AiUseShieldAgainstEnemyMissileProbability || drivenProperty == DrivenProperty.AiDefendWithShieldDecisionChanceValue;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00072038 File Offset: 0x00070238
		private static void GetUnitPositionWithIndexAccordingToNewOrder(Formation simulationFormation, int unitIndex, in WorldPosition formationPosition, in Vec2 formationDirection, IFormationArrangement arrangement, float width, int unitSpacing, int unitCount, bool isMounted, int index, out WorldPosition? unitPosition, out Vec2? unitDirection, out float actualWidth)
		{
			unitPosition = null;
			unitDirection = null;
			if (simulationFormation == null)
			{
				if (Formation._simulationFormationTemp == null || Formation._simulationFormationUniqueIdentifier != index)
				{
					Formation._simulationFormationTemp = new Formation(null, -1);
				}
				simulationFormation = Formation._simulationFormationTemp;
			}
			object simulationFormationLock = simulationFormation.SimulationFormationLock;
			lock (simulationFormationLock)
			{
				if (simulationFormation.UnitSpacing == unitSpacing && (MathF.Abs(simulationFormation.Width - width + 1E-05f) < simulationFormation.Interval + simulationFormation.UnitDiameter - 1E-05f || (width < simulationFormation.MinimumWidth && MathF.Abs(simulationFormation.Width - simulationFormation.MinimumWidth) < 1E-05f)) && simulationFormation.OrderPositionIsValid)
				{
					Vec3 orderGroundPosition = simulationFormation.OrderGroundPosition;
					WorldPosition worldPosition = formationPosition;
					Vec3 vec = worldPosition.GetGroundVec3();
					if (orderGroundPosition.NearlyEquals(in vec, 0.1f) && simulationFormation.Direction.NearlyEquals(formationDirection, 0.1f) && !(simulationFormation.Arrangement.GetType() != arrangement.GetType()))
					{
						goto IL_0273;
					}
				}
				simulationFormation._overridenHasAnyMountedUnit = new bool?(isMounted);
				simulationFormation.ResetForSimulation();
				simulationFormation.SetPositioning(null, null, new int?(unitSpacing));
				simulationFormation.OverridenUnitCount = new int?(unitCount);
				simulationFormation.SetPositioning(new WorldPosition?(formationPosition), new Vec2?(formationDirection), null);
				simulationFormation.Rearrange(arrangement.Clone(simulationFormation));
				simulationFormation.Arrangement.DeepCopyFrom(arrangement);
				simulationFormation.Width = width;
				Formation._simulationFormationUniqueIdentifier = index;
				ColumnFormation columnFormation;
				if ((columnFormation = arrangement as ColumnFormation) != null && arrangement.RankCount > 1)
				{
					Vec3 vec = ((columnFormation.Vanguard ?? arrangement.GetUnit(columnFormation.VanguardFileIndex, 0)) as Agent).Position;
					Vec2 asVec = vec.AsVec2;
					vec = (arrangement.GetUnit(columnFormation.VanguardFileIndex, 1) as Agent).Position;
					Vec2 asVec2 = vec.AsVec2;
					Vec2 vec2 = (asVec - asVec2).Normalized();
					WorldPosition worldPosition = formationPosition;
					Vec2 vec3 = (worldPosition.AsVec2 - asVec).Normalized();
					if (arrangement.IsTurnBackwardsNecessary(asVec, new WorldPosition?(formationPosition), vec2, true, new Vec2?(vec3)))
					{
						(simulationFormation.Arrangement as ColumnFormation).UnitPositionsOnVanguardFileIndex.Reverse();
					}
				}
				IL_0273:
				actualWidth = simulationFormation.Width;
				if (width >= actualWidth)
				{
					Vec2? vec4 = simulationFormation.Arrangement.GetLocalPositionOfUnitOrDefault(unitIndex);
					if (vec4 == null)
					{
						vec4 = simulationFormation.Arrangement.CreateNewPosition(unitIndex);
					}
					if (vec4 != null)
					{
						Vec2 vec5 = simulationFormation.Direction.TransformToParentUnitF(vec4.Value);
						WorldPosition worldPosition2 = simulationFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
						worldPosition2.SetVec2(worldPosition2.AsVec2 + vec5);
						unitPosition = new WorldPosition?(worldPosition2);
						unitDirection = new Vec2?(formationDirection);
					}
				}
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00072378 File Offset: 0x00070578
		private static IEnumerable<ValueTuple<WorldPosition, Vec2>> GetUnavailableUnitPositionsAccordingToNewOrder(Formation formation, Formation simulationFormation, WorldPosition position, Vec2 direction, IFormationArrangement arrangement, float width, int unitSpacing)
		{
			if (simulationFormation == null)
			{
				if (Formation._simulationFormationTemp == null || Formation._simulationFormationUniqueIdentifier != formation.Index)
				{
					Formation._simulationFormationTemp = new Formation(null, -1);
				}
				simulationFormation = Formation._simulationFormationTemp;
			}
			object obj = simulationFormation.SimulationFormationLock;
			lock (obj)
			{
				if (simulationFormation.UnitSpacing == unitSpacing && MathF.Abs(simulationFormation.Width - width) < simulationFormation.Interval + simulationFormation.UnitDiameter && simulationFormation.OrderPositionIsValid)
				{
					Vec3 orderGroundPosition = simulationFormation.OrderGroundPosition;
					Vec3 groundVec = position.GetGroundVec3();
					if (orderGroundPosition.NearlyEquals(in groundVec, 0.1f) && simulationFormation.Direction.NearlyEquals(direction, 0.1f) && !(simulationFormation.Arrangement.GetType() != arrangement.GetType()))
					{
						goto IL_0233;
					}
				}
				simulationFormation._overridenHasAnyMountedUnit = new bool?(formation.HasAnyMountedUnit);
				simulationFormation.ResetForSimulation();
				simulationFormation.SetPositioning(null, null, new int?(unitSpacing));
				simulationFormation.OverridenUnitCount = new int?(formation.CountOfUnitsWithoutDetachedOnes);
				simulationFormation.SetPositioning(new WorldPosition?(position), new Vec2?(direction), null);
				simulationFormation.Rearrange(arrangement.Clone(simulationFormation));
				simulationFormation.Arrangement.DeepCopyFrom(arrangement);
				simulationFormation.Width = width;
				Formation._simulationFormationUniqueIdentifier = formation.Index;
				IL_0233:
				IEnumerable<Vec2> unavailableUnitPositions = simulationFormation.Arrangement.GetUnavailableUnitPositions();
				foreach (Vec2 vec in unavailableUnitPositions)
				{
					Vec2 vec2 = simulationFormation.Direction.TransformToParentUnitF(vec);
					WorldPosition worldPosition = simulationFormation.CreateNewOrderWorldPosition(WorldPosition.WorldPositionEnforcedCache.None);
					worldPosition.SetVec2(worldPosition.AsVec2 + vec2);
					yield return new ValueTuple<WorldPosition, Vec2>(worldPosition, direction);
				}
				IEnumerator<Vec2> enumerator = null;
			}
			obj = null;
			yield break;
			yield break;
		}

		// Token: 0x04000ADC RID: 2780
		public const float AveragePositionCalculatePeriod = 0.1f;

		// Token: 0x04000ADD RID: 2781
		public const int MinimumUnitSpacing = 0;

		// Token: 0x04000ADE RID: 2782
		public const int RetreatPositionDistanceCacheCount = 2;

		// Token: 0x04000ADF RID: 2783
		public const float RetreatPositionCacheUseDistanceSquared = 400f;

		// Token: 0x04000AE0 RID: 2784
		private static Formation _simulationFormationTemp;

		// Token: 0x04000AE1 RID: 2785
		private static int _simulationFormationUniqueIdentifier;

		// Token: 0x04000AEC RID: 2796
		public readonly Team Team;

		// Token: 0x04000AED RID: 2797
		public readonly int Index;

		// Token: 0x04000AEE RID: 2798
		public readonly FormationClass FormationIndex;

		// Token: 0x04000AEF RID: 2799
		public Banner Banner;

		// Token: 0x04000AF0 RID: 2800
		public bool HasBeenPositioned;

		// Token: 0x04000AF1 RID: 2801
		public Vec2? ReferencePosition;

		// Token: 0x04000AF3 RID: 2803
		private bool _logicalClassNeedsUpdate;

		// Token: 0x04000AF4 RID: 2804
		private FormationClass _logicalClass = FormationClass.NumberOfAllFormations;

		// Token: 0x04000AF5 RID: 2805
		private readonly int[] _logicalClassCounts = new int[4];

		// Token: 0x04000AF6 RID: 2806
		private Agent _playerOwner;

		// Token: 0x04000AF7 RID: 2807
		private string _bannerCode;

		// Token: 0x04000AF9 RID: 2809
		private bool _enforceNotSplittableByAI = true;

		// Token: 0x04000AFA RID: 2810
		private WorldPosition _orderPosition;

		// Token: 0x04000AFD RID: 2813
		private Vec2 _orderLocalAveragePosition;

		// Token: 0x04000AFE RID: 2814
		private bool _orderLocalAveragePositionIsDirty = true;

		// Token: 0x04000AFF RID: 2815
		private int _formationOrderDefensivenessFactor = 2;

		// Token: 0x04000B00 RID: 2816
		private MovementOrder _movementOrder;

		// Token: 0x04000B01 RID: 2817
		private Timer _arrangementOrderTickOccasionallyTimer;

		// Token: 0x04000B02 RID: 2818
		private Timer _arrangementTickOccasionallyTimer;

		// Token: 0x04000B03 RID: 2819
		private Agent _captain;

		// Token: 0x04000B04 RID: 2820
		private Vec2 _smoothedAverageUnitPosition = Vec2.Invalid;

		// Token: 0x04000B05 RID: 2821
		private MBList<IDetachment> _detachments;

		// Token: 0x04000B06 RID: 2822
		private IFormationArrangement _arrangement;

		// Token: 0x04000B07 RID: 2823
		private int[] _agentIndicesCache;

		// Token: 0x04000B08 RID: 2824
		private MBList<Agent> _detachedUnits;

		// Token: 0x04000B09 RID: 2825
		private int _undetachableNonPlayerUnitCount;

		// Token: 0x04000B0A RID: 2826
		private MBList<Agent> _looseDetachedUnits;

		// Token: 0x04000B0B RID: 2827
		private bool? _overridenHasAnyMountedUnit;

		// Token: 0x04000B0C RID: 2828
		private bool _isArrangementShapeChanged;

		// Token: 0x04000B0D RID: 2829
		private bool _hasPendingUnitPositions;

		// Token: 0x04000B0E RID: 2830
		private int _currentSpawnIndex;

		// Token: 0x04000B0F RID: 2831
		private int _desiredFileCount;

		// Token: 0x04000B14 RID: 2836
		private Formation _targetFormation;

		// Token: 0x04000B17 RID: 2839
		private Timer _cachedFormationIntegrityDataUpdateTimer;

		// Token: 0x04000B1B RID: 2843
		private Timer _cachedPositionAndVelocityUpdateTimer;

		// Token: 0x04000B1C RID: 2844
		private float _lastAveragePositionCacheTime;

		// Token: 0x04000B1E RID: 2846
		private Timer _cachedMovementSpeedUpdateTimer;

		// Token: 0x04000B20 RID: 2848
		private Formation _cachedClosestEnemyFormation;

		// Token: 0x04000B21 RID: 2849
		private Timer _cachedClosestEnemyFormationUpdateTimer;

		// Token: 0x02000520 RID: 1312
		private class AgentArrangementData : IFormationUnit
		{
			// Token: 0x17000A46 RID: 2630
			// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x000EFB6D File Offset: 0x000EDD6D
			// (set) Token: 0x06003BE7 RID: 15335 RVA: 0x000EFB75 File Offset: 0x000EDD75
			public IFormationArrangement Formation { get; private set; }

			// Token: 0x17000A47 RID: 2631
			// (get) Token: 0x06003BE8 RID: 15336 RVA: 0x000EFB7E File Offset: 0x000EDD7E
			// (set) Token: 0x06003BE9 RID: 15337 RVA: 0x000EFB86 File Offset: 0x000EDD86
			public int FormationFileIndex { get; set; } = -1;

			// Token: 0x17000A48 RID: 2632
			// (get) Token: 0x06003BEA RID: 15338 RVA: 0x000EFB8F File Offset: 0x000EDD8F
			// (set) Token: 0x06003BEB RID: 15339 RVA: 0x000EFB97 File Offset: 0x000EDD97
			public int FormationRankIndex { get; set; } = -1;

			// Token: 0x17000A49 RID: 2633
			// (get) Token: 0x06003BEC RID: 15340 RVA: 0x000EFBA0 File Offset: 0x000EDDA0
			public IFormationUnit FollowedUnit { get; }

			// Token: 0x17000A4A RID: 2634
			// (get) Token: 0x06003BED RID: 15341 RVA: 0x000EFBA8 File Offset: 0x000EDDA8
			public bool IsShieldUsageEncouraged
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000A4B RID: 2635
			// (get) Token: 0x06003BEE RID: 15342 RVA: 0x000EFBAB File Offset: 0x000EDDAB
			public bool IsPlayerUnit
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06003BEF RID: 15343 RVA: 0x000EFBAE File Offset: 0x000EDDAE
			public AgentArrangementData(int index, IFormationArrangement arrangement)
			{
				this.Formation = arrangement;
			}
		}

		// Token: 0x02000521 RID: 1313
		public struct FormationIntegrityDataGroup
		{
			// Token: 0x06003BF0 RID: 15344 RVA: 0x000EFBCB File Offset: 0x000EDDCB
			public FormationIntegrityDataGroup(Vec2 averageVelocityExcludeFarAgents, float deviationOfPositionsExcludeFarAgents, float maxDeviationOfPositionExcludeFarAgents, float averageMaxUnlimitedSpeedExcludeFarAgents)
			{
				this.AverageVelocityExcludeFarAgents = averageVelocityExcludeFarAgents;
				this.DeviationOfPositionsExcludeFarAgents = deviationOfPositionsExcludeFarAgents;
				this.MaxDeviationOfPositionExcludeFarAgents = maxDeviationOfPositionExcludeFarAgents;
				this.AverageMaxUnlimitedSpeedExcludeFarAgents = averageMaxUnlimitedSpeedExcludeFarAgents;
			}

			// Token: 0x04001D1D RID: 7453
			public Vec2 AverageVelocityExcludeFarAgents;

			// Token: 0x04001D1E RID: 7454
			public float DeviationOfPositionsExcludeFarAgents;

			// Token: 0x04001D1F RID: 7455
			public float MaxDeviationOfPositionExcludeFarAgents;

			// Token: 0x04001D20 RID: 7456
			public float AverageMaxUnlimitedSpeedExcludeFarAgents;
		}

		// Token: 0x02000522 RID: 1314
		public class RetreatPositionCacheSystem
		{
			// Token: 0x06003BF1 RID: 15345 RVA: 0x000EFBEA File Offset: 0x000EDDEA
			public RetreatPositionCacheSystem(int cacheCount)
			{
				this._retreatPositionDistance = new List<ValueTuple<Vec2, WorldPosition>>(2);
			}

			// Token: 0x06003BF2 RID: 15346 RVA: 0x000EFC00 File Offset: 0x000EDE00
			public WorldPosition GetRetreatPositionFromCache(Vec2 agentPosition)
			{
				for (int i = this._retreatPositionDistance.Count - 1; i >= 0; i--)
				{
					if (this._retreatPositionDistance[i].Item1.DistanceSquared(agentPosition) < 400f)
					{
						return this._retreatPositionDistance[i].Item2;
					}
				}
				return WorldPosition.Invalid;
			}

			// Token: 0x06003BF3 RID: 15347 RVA: 0x000EFC5D File Offset: 0x000EDE5D
			public void AddNewPositionToCache(Vec2 agentPostion, WorldPosition retreatingPosition)
			{
				if (this._retreatPositionDistance.Count >= 2)
				{
					this._retreatPositionDistance.RemoveAt(0);
				}
				this._retreatPositionDistance.Add(new ValueTuple<Vec2, WorldPosition>(agentPostion, retreatingPosition));
			}

			// Token: 0x04001D21 RID: 7457
			private List<ValueTuple<Vec2, WorldPosition>> _retreatPositionDistance;
		}
	}
}
