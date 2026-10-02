using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000177 RID: 375
	public class FormationQuerySystem
	{
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x00048904 File Offset: 0x00046B04
		public TeamQuerySystem Team
		{
			get
			{
				return this.Formation.Team.QuerySystem;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x00048916 File Offset: 0x00046B16
		public float FormationPower
		{
			get
			{
				return this._formationPower.Value;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x00048923 File Offset: 0x00046B23
		public float FormationPowerReadOnly
		{
			get
			{
				return this._formationPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x00048930 File Offset: 0x00046B30
		public float FormationMeleeFightingPower
		{
			get
			{
				return this._formationMeleeFightingPower.Value;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x0004893D File Offset: 0x00046B3D
		public float FormationMeleeFightingPowerReadOnly
		{
			get
			{
				return this._formationMeleeFightingPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x0004894A File Offset: 0x00046B4A
		public Vec2 EstimatedDirection
		{
			get
			{
				return this._estimatedDirection.Value;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x00048957 File Offset: 0x00046B57
		public Vec2 EstimatedDirectionReadOnly
		{
			get
			{
				return this._estimatedDirection.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00048964 File Offset: 0x00046B64
		public float EstimatedInterval
		{
			get
			{
				return this._estimatedInterval.Value;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x00048971 File Offset: 0x00046B71
		public float EstimatedIntervalReadOnly
		{
			get
			{
				return this._estimatedInterval.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x0004897E File Offset: 0x00046B7E
		public Vec2 AverageAllyPosition
		{
			get
			{
				return this._averageAllyPosition.Value;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x0004898B File Offset: 0x00046B8B
		public Vec2 AverageAllyPositionReadOnly
		{
			get
			{
				return this._averageAllyPosition.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00048998 File Offset: 0x00046B98
		public float IdealAverageDisplacement
		{
			get
			{
				return this._idealAverageDisplacement.Value;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x000489A5 File Offset: 0x00046BA5
		public float IdealAverageDisplacementReadOnly
		{
			get
			{
				return this._idealAverageDisplacement.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x000489B2 File Offset: 0x00046BB2
		public MBList<Agent> LocalAllyUnits
		{
			get
			{
				return this._localAllyUnits.Value;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x000489BF File Offset: 0x00046BBF
		public MBList<Agent> LocalAllyUnitsReadOnly
		{
			get
			{
				return this._localAllyUnits.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x000489CC File Offset: 0x00046BCC
		public MBList<Agent> LocalEnemyUnits
		{
			get
			{
				return this._localEnemyUnits.Value;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x000489D9 File Offset: 0x00046BD9
		public MBList<Agent> LocalEnemyUnitsReadOnly
		{
			get
			{
				return this._localEnemyUnits.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x000489E6 File Offset: 0x00046BE6
		public FormationClass MainClass
		{
			get
			{
				return this._mainClass.Value;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001393 RID: 5011 RVA: 0x000489F3 File Offset: 0x00046BF3
		public FormationClass MainClassReadOnly
		{
			get
			{
				return this._mainClass.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x00048A00 File Offset: 0x00046C00
		public float InfantryUnitRatio
		{
			get
			{
				return this._infantryUnitRatio.Value;
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x00048A0D File Offset: 0x00046C0D
		public float InfantryUnitRatioReadOnly
		{
			get
			{
				return this._infantryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x00048A1A File Offset: 0x00046C1A
		public float HasShieldUnitRatio
		{
			get
			{
				return this._hasShieldUnitRatio.Value;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x00048A27 File Offset: 0x00046C27
		public float HasShieldUnitRatioReadOnly
		{
			get
			{
				return this._hasShieldUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x00048A34 File Offset: 0x00046C34
		public float HasThrowingUnitRatio
		{
			get
			{
				return this._hasThrowingUnitRatio.Value;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00048A41 File Offset: 0x00046C41
		public float HasThrowingUnitRatioReadOnly
		{
			get
			{
				return this._hasThrowingUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x00048A4E File Offset: 0x00046C4E
		public float RangedUnitRatio
		{
			get
			{
				return this._rangedUnitRatio.Value;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x0600139B RID: 5019 RVA: 0x00048A5B File Offset: 0x00046C5B
		public float RangedUnitRatioReadOnly
		{
			get
			{
				return this._rangedUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x00048A68 File Offset: 0x00046C68
		public int InsideCastleUnitCountIncludingUnpositioned
		{
			get
			{
				return this._insideCastleUnitCountIncludingUnpositioned.Value;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x00048A75 File Offset: 0x00046C75
		public int InsideCastleUnitCountIncludingUnpositionedReadOnly
		{
			get
			{
				return this._insideCastleUnitCountIncludingUnpositioned.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00048A82 File Offset: 0x00046C82
		public int InsideCastleUnitCountPositioned
		{
			get
			{
				return this._insideCastleUnitCountPositioned.Value;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00048A8F File Offset: 0x00046C8F
		public int InsideCastleUnitCountPositionedReadOnly
		{
			get
			{
				return this._insideCastleUnitCountPositioned.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x00048A9C File Offset: 0x00046C9C
		public float CavalryUnitRatio
		{
			get
			{
				return this._cavalryUnitRatio.Value;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00048AA9 File Offset: 0x00046CA9
		public float CavalryUnitRatioReadOnly
		{
			get
			{
				return this._cavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x00048AB6 File Offset: 0x00046CB6
		public float RangedCavalryUnitRatio
		{
			get
			{
				return this._rangedCavalryUnitRatio.Value;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x00048AC3 File Offset: 0x00046CC3
		public float RangedCavalryUnitRatioReadOnly
		{
			get
			{
				return this._rangedCavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060013A4 RID: 5028 RVA: 0x00048AD0 File Offset: 0x00046CD0
		public bool IsMeleeFormation
		{
			get
			{
				return this._isMeleeFormation.Value;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00048ADD File Offset: 0x00046CDD
		public bool IsMeleeFormationReadOnly
		{
			get
			{
				return this._isMeleeFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060013A6 RID: 5030 RVA: 0x00048AEA File Offset: 0x00046CEA
		public bool IsInfantryFormation
		{
			get
			{
				return this._isInfantryFormation.Value;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00048AF7 File Offset: 0x00046CF7
		public bool IsInfantryFormationReadOnly
		{
			get
			{
				return this._isInfantryFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x00048B04 File Offset: 0x00046D04
		public bool HasShield
		{
			get
			{
				return this._hasShield.Value;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00048B11 File Offset: 0x00046D11
		public bool HasShieldReadOnly
		{
			get
			{
				return this._hasShield.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060013AA RID: 5034 RVA: 0x00048B1E File Offset: 0x00046D1E
		public bool HasThrowing
		{
			get
			{
				return this._hasThrowing.Value;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x00048B2B File Offset: 0x00046D2B
		public bool HasThrowingReadOnly
		{
			get
			{
				return this._hasThrowing.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x00048B38 File Offset: 0x00046D38
		public bool IsRangedFormation
		{
			get
			{
				return this._isRangedFormation.Value;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x00048B45 File Offset: 0x00046D45
		public bool IsRangedFormationReadOnly
		{
			get
			{
				return this._isRangedFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x00048B52 File Offset: 0x00046D52
		public bool IsCavalryFormation
		{
			get
			{
				return this._isCavalryFormation.Value;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x00048B5F File Offset: 0x00046D5F
		public bool IsCavalryFormationReadOnly
		{
			get
			{
				return this._isCavalryFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00048B6C File Offset: 0x00046D6C
		public bool IsRangedCavalryFormation
		{
			get
			{
				return this._isRangedCavalryFormation.Value;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00048B79 File Offset: 0x00046D79
		public bool IsRangedCavalryFormationReadOnly
		{
			get
			{
				return this._isRangedCavalryFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00048B86 File Offset: 0x00046D86
		public float MovementSpeedMaximum
		{
			get
			{
				return this._movementSpeedMaximum.Value;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x00048B93 File Offset: 0x00046D93
		public float MovementSpeedMaximumReadOnly
		{
			get
			{
				return this._movementSpeedMaximum.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00048BA0 File Offset: 0x00046DA0
		public float MaximumMissileRange
		{
			get
			{
				return this._maximumMissileRange.Value;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x00048BAD File Offset: 0x00046DAD
		public float MaximumMissileRangeReadOnly
		{
			get
			{
				return this._maximumMissileRange.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x00048BBA File Offset: 0x00046DBA
		public float MissileRangeAdjusted
		{
			get
			{
				return this._missileRangeAdjusted.Value;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00048BC7 File Offset: 0x00046DC7
		public float MissileRangeAdjustedReadOnly
		{
			get
			{
				return this._missileRangeAdjusted.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00048BD4 File Offset: 0x00046DD4
		public float LocalInfantryUnitRatio
		{
			get
			{
				return this._localInfantryUnitRatio.Value;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00048BE1 File Offset: 0x00046DE1
		public float LocalInfantryUnitRatioReadOnly
		{
			get
			{
				return this._localInfantryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x00048BEE File Offset: 0x00046DEE
		public float LocalRangedUnitRatio
		{
			get
			{
				return this._localRangedUnitRatio.Value;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x00048BFB File Offset: 0x00046DFB
		public float LocalRangedUnitRatioReadOnly
		{
			get
			{
				return this._localRangedUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x00048C08 File Offset: 0x00046E08
		public float LocalCavalryUnitRatio
		{
			get
			{
				return this._localCavalryUnitRatio.Value;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x00048C15 File Offset: 0x00046E15
		public float LocalCavalryUnitRatioReadOnly
		{
			get
			{
				return this._localCavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x00048C22 File Offset: 0x00046E22
		public float LocalRangedCavalryUnitRatio
		{
			get
			{
				return this._localRangedCavalryUnitRatio.Value;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x00048C2F File Offset: 0x00046E2F
		public float LocalRangedCavalryUnitRatioReadOnly
		{
			get
			{
				return this._localRangedCavalryUnitRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x00048C3C File Offset: 0x00046E3C
		public float LocalAllyPower
		{
			get
			{
				return this._localAllyPower.Value;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x00048C49 File Offset: 0x00046E49
		public float LocalAllyPowerReadOnly
		{
			get
			{
				return this._localAllyPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060013C2 RID: 5058 RVA: 0x00048C56 File Offset: 0x00046E56
		public float LocalEnemyPower
		{
			get
			{
				return this._localEnemyPower.Value;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x00048C63 File Offset: 0x00046E63
		public float LocalEnemyPowerReadOnly
		{
			get
			{
				return this._localEnemyPower.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00048C70 File Offset: 0x00046E70
		public float LocalPowerRatio
		{
			get
			{
				return this._localPowerRatio.Value;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00048C7D File Offset: 0x00046E7D
		public float LocalPowerRatioReadOnly
		{
			get
			{
				return this._localPowerRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00048C8A File Offset: 0x00046E8A
		public float CasualtyRatio
		{
			get
			{
				return this._casualtyRatio.Value;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x00048C97 File Offset: 0x00046E97
		public float CasualtyRatioReadOnly
		{
			get
			{
				return this._casualtyRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x00048CA4 File Offset: 0x00046EA4
		public bool IsUnderRangedAttack
		{
			get
			{
				return this._isUnderRangedAttack.Value;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00048CB1 File Offset: 0x00046EB1
		public bool IsUnderRangedAttackReadOnly
		{
			get
			{
				return this._isUnderRangedAttack.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x00048CBE File Offset: 0x00046EBE
		public float UnderRangedAttackRatio
		{
			get
			{
				return this._underRangedAttackRatio.Value;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x00048CCB File Offset: 0x00046ECB
		public float UnderRangedAttackRatioReadOnly
		{
			get
			{
				return this._underRangedAttackRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060013CC RID: 5068 RVA: 0x00048CD8 File Offset: 0x00046ED8
		public float MakingRangedAttackRatio
		{
			get
			{
				return this._makingRangedAttackRatio.Value;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x00048CE5 File Offset: 0x00046EE5
		public float MakingRangedAttackRatioReadOnly
		{
			get
			{
				return this._makingRangedAttackRatio.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x00048CF2 File Offset: 0x00046EF2
		public Formation MainFormation
		{
			get
			{
				return this._mainFormation.Value;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x00048CFF File Offset: 0x00046EFF
		public Formation MainFormationReadOnly
		{
			get
			{
				return this._mainFormation.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x00048D0C File Offset: 0x00046F0C
		public float MainFormationReliabilityFactor
		{
			get
			{
				return this._mainFormationReliabilityFactor.Value;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x00048D19 File Offset: 0x00046F19
		public float MainFormationReliabilityFactorReadOnly
		{
			get
			{
				return this._mainFormationReliabilityFactor.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x00048D26 File Offset: 0x00046F26
		public Vec2 WeightedAverageEnemyPosition
		{
			get
			{
				return this._weightedAverageEnemyPosition.Value;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x00048D33 File Offset: 0x00046F33
		public Vec2 WeightedAverageEnemyPositionReadOnly
		{
			get
			{
				return this._weightedAverageEnemyPosition.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x00048D40 File Offset: 0x00046F40
		public Agent ClosestEnemyAgent
		{
			get
			{
				return this._closestEnemyAgent.Value;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00048D4D File Offset: 0x00046F4D
		public Agent ClosestEnemyAgentReadOnly
		{
			get
			{
				return this._closestEnemyAgent.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x00048D5C File Offset: 0x00046F5C
		public FormationQuerySystem ClosestSignificantlyLargeEnemyFormation
		{
			get
			{
				if (this._closestSignificantlyLargeEnemyFormation.Value == null || this._closestSignificantlyLargeEnemyFormation.Value.CountOfUnits == 0)
				{
					this._closestSignificantlyLargeEnemyFormation.Expire();
				}
				Formation value = this._closestSignificantlyLargeEnemyFormation.Value;
				if (value == null)
				{
					return null;
				}
				return value.QuerySystem;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x00048DA9 File Offset: 0x00046FA9
		public FormationQuerySystem ClosestSignificantlyLargeEnemyFormationReadOnly
		{
			get
			{
				Formation cachedValueUnlessTooOld = this._closestSignificantlyLargeEnemyFormation.GetCachedValueUnlessTooOld();
				if (cachedValueUnlessTooOld == null)
				{
					return null;
				}
				return cachedValueUnlessTooOld.QuerySystem;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060013D8 RID: 5080 RVA: 0x00048DC4 File Offset: 0x00046FC4
		public FormationQuerySystem FastestSignificantlyLargeEnemyFormation
		{
			get
			{
				if (this._fastestSignificantlyLargeEnemyFormation.Value == null || this._fastestSignificantlyLargeEnemyFormation.Value.CountOfUnits == 0)
				{
					this._fastestSignificantlyLargeEnemyFormation.Expire();
				}
				Formation value = this._fastestSignificantlyLargeEnemyFormation.Value;
				if (value == null)
				{
					return null;
				}
				return value.QuerySystem;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x00048E11 File Offset: 0x00047011
		public FormationQuerySystem FastestSignificantlyLargeEnemyFormationReadOnly
		{
			get
			{
				Formation cachedValueUnlessTooOld = this._fastestSignificantlyLargeEnemyFormation.GetCachedValueUnlessTooOld();
				if (cachedValueUnlessTooOld == null)
				{
					return null;
				}
				return cachedValueUnlessTooOld.QuerySystem;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00048E29 File Offset: 0x00047029
		public Vec2 HighGroundCloseToForeseenBattleGround
		{
			get
			{
				return this._highGroundCloseToForeseenBattleGround.Value;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x00048E36 File Offset: 0x00047036
		public Vec2 HighGroundCloseToForeseenBattleGroundReadOnly
		{
			get
			{
				return this._highGroundCloseToForeseenBattleGround.GetCachedValueUnlessTooOld();
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00048E43 File Offset: 0x00047043
		public bool IsUnderCavalryChargeFromFront
		{
			get
			{
				return this._isUnderCavalryChargeFromFront.Value;
			}
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00048E50 File Offset: 0x00047050
		public FormationQuerySystem(Formation formation)
		{
			FormationQuerySystem.<>c__DisplayClass231_0 CS$<>8__locals1 = new FormationQuerySystem.<>c__DisplayClass231_0();
			CS$<>8__locals1.formation = formation;
			base..ctor();
			CS$<>8__locals1.<>4__this = this;
			this.Formation = CS$<>8__locals1.formation;
			Mission mission = Mission.Current;
			this._formationPower = new QueryData<float>(new Func<float>(CS$<>8__locals1.formation.GetFormationPower), 2.5f);
			this._formationMeleeFightingPower = new QueryData<float>(new Func<float>(CS$<>8__locals1.formation.GetFormationMeleeFightingPower), 2.5f);
			this._estimatedDirection = new QueryData<Vec2>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnitsWithoutDetachedOnes > 0)
				{
					Vec2 averagePositionOfUnits = CS$<>8__locals1.formation.GetAveragePositionOfUnits(true, true);
					float num = 0f;
					float num2 = 0f;
					Vec2 orderLocalAveragePosition = CS$<>8__locals1.formation.OrderLocalAveragePosition;
					int num3 = 0;
					foreach (IFormationUnit formationUnit in CS$<>8__locals1.formation.UnitsWithoutLooseDetachedOnes)
					{
						Agent agent = (Agent)formationUnit;
						Vec2? localPositionOfUnitOrDefault = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefault(agent);
						if (localPositionOfUnitOrDefault != null)
						{
							Vec2 value = localPositionOfUnitOrDefault.Value;
							Vec2 asVec = agent.Position.AsVec2;
							num += (value.x - orderLocalAveragePosition.x) * (asVec.x - averagePositionOfUnits.x) + (value.y - orderLocalAveragePosition.y) * (asVec.y - averagePositionOfUnits.y);
							num2 += (value.x - orderLocalAveragePosition.x) * (asVec.y - averagePositionOfUnits.y) - (value.y - orderLocalAveragePosition.y) * (asVec.x - averagePositionOfUnits.x);
							num3++;
						}
					}
					if (num3 > 0)
					{
						float num4 = 1f / (float)num3;
						num *= num4;
						num2 *= num4;
						float num5 = MathF.Sqrt(num * num + num2 * num2);
						if (num5 > 0f)
						{
							float num6 = MathF.Acos(MBMath.ClampFloat(num / num5, -1f, 1f));
							Vec2 vec = Vec2.FromRotation(num6);
							Vec2 vec2 = Vec2.FromRotation(-num6);
							float num7 = 0f;
							float num8 = 0f;
							foreach (IFormationUnit formationUnit2 in CS$<>8__locals1.formation.UnitsWithoutLooseDetachedOnes)
							{
								Agent agent2 = (Agent)formationUnit2;
								Vec2? localPositionOfUnitOrDefault2 = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefault(agent2);
								if (localPositionOfUnitOrDefault2 != null)
								{
									Vec2 vec3 = vec.TransformToParentUnitF(localPositionOfUnitOrDefault2.Value - orderLocalAveragePosition);
									Vec2 vec4 = vec2.TransformToParentUnitF(localPositionOfUnitOrDefault2.Value - orderLocalAveragePosition);
									Vec2 asVec2 = agent2.Position.AsVec2;
									num7 += (vec3 - asVec2 + averagePositionOfUnits).LengthSquared;
									num8 += (vec4 - asVec2 + averagePositionOfUnits).LengthSquared;
								}
							}
							if (num7 >= num8)
							{
								return vec2;
							}
							return vec;
						}
					}
				}
				return new Vec2(0f, 1f);
			}, 0.2f);
			this._estimatedInterval = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnitsWithoutDetachedOnes > 0)
				{
					Vec2 estimatedDirection = CS$<>8__locals1.formation.QuerySystem.EstimatedDirection;
					Vec2 currentPosition = CS$<>8__locals1.formation.CurrentPosition;
					float num9 = 0f;
					float num10 = 0f;
					foreach (IFormationUnit formationUnit3 in CS$<>8__locals1.formation.UnitsWithoutLooseDetachedOnes)
					{
						Agent agent3 = (Agent)formationUnit3;
						Vec2? localPositionOfUnitOrDefault3 = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefault(agent3);
						if (localPositionOfUnitOrDefault3 != null)
						{
							Vec2 vec5 = estimatedDirection.TransformToLocalUnitF(agent3.Position.AsVec2 - currentPosition);
							Vec2 vec6 = localPositionOfUnitOrDefault3.Value - vec5;
							Vec2 vec7 = CS$<>8__locals1.formation.Arrangement.GetLocalPositionOfUnitOrDefaultWithAdjustment(agent3, 1f).Value - localPositionOfUnitOrDefault3.Value;
							if (vec7.IsNonZero())
							{
								float num11 = vec7.Normalize();
								float num12 = Vec2.DotProduct(vec6, vec7);
								num9 += num12 * num11;
								num10 += num11 * num11;
							}
						}
					}
					if (num10 != 0f)
					{
						return Math.Max(0f, -num9 / num10 + CS$<>8__locals1.formation.Interval);
					}
				}
				return CS$<>8__locals1.formation.Interval;
			}, 0.2f);
			this._averageAllyPosition = new QueryData<Vec2>(delegate
			{
				int num13 = 0;
				Vec2 vec8 = Vec2.Zero;
				using (List<Team>.Enumerator enumerator3 = mission.Teams.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.IsFriendOf(CS$<>8__locals1.formation.Team))
						{
							foreach (Formation formation2 in CS$<>8__locals1.<>4__this.Formation.Team.FormationsIncludingSpecialAndEmpty)
							{
								if (formation2.CountOfUnits > 0 && formation2 != CS$<>8__locals1.formation)
								{
									num13 += formation2.CountOfUnits;
									vec8 += formation2.GetAveragePositionOfUnits(false, false) * (float)formation2.CountOfUnits;
								}
							}
						}
					}
				}
				if (num13 > 0)
				{
					return vec8 * (1f / (float)num13);
				}
				return CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition;
			}, 5f);
			this._idealAverageDisplacement = new QueryData<float>(() => MathF.Sqrt(CS$<>8__locals1.formation.Width * CS$<>8__locals1.formation.Width * 0.5f * 0.5f + CS$<>8__locals1.formation.Depth * CS$<>8__locals1.formation.Depth * 0.5f * 0.5f) / 2f, 5f);
			this._localAllyUnits = new QueryData<MBList<Agent>>(() => mission.GetNearbyAllyAgents(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, 30f, CS$<>8__locals1.formation.Team, CS$<>8__locals1.<>4__this._localAllyUnits.GetCachedValue()), 5f, new MBList<Agent>());
			this._localEnemyUnits = new QueryData<MBList<Agent>>(() => mission.GetNearbyEnemyAgents(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, 30f, CS$<>8__locals1.formation.Team, CS$<>8__locals1.<>4__this._localEnemyUnits.GetCachedValue()), 5f, new MBList<Agent>());
			this._infantryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.Infantry, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._hasShieldUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsWithCondition(new Func<Agent, bool>(QueryLibrary.HasShield)) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._hasThrowingUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsWithCondition(new Func<Agent, bool>(QueryLibrary.HasThrown)) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._rangedUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.Ranged, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._cavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.Cavalry, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._rangedCavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits > 0)
				{
					return (float)CS$<>8__locals1.formation.GetCountOfUnitsBelongingToPhysicalClass(FormationClass.HorseArcher, false) / (float)CS$<>8__locals1.formation.CountOfUnits;
				}
				return 0f;
			}, 2.5f);
			this._isMeleeFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.InfantryUnitRatio + CS$<>8__locals1.<>4__this.CavalryUnitRatio > CS$<>8__locals1.<>4__this.RangedUnitRatio + CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._isInfantryFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.InfantryUnitRatio >= CS$<>8__locals1.<>4__this.RangedUnitRatio && CS$<>8__locals1.<>4__this.InfantryUnitRatio >= CS$<>8__locals1.<>4__this.CavalryUnitRatio && CS$<>8__locals1.<>4__this.InfantryUnitRatio >= CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._hasShield = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.HasShieldUnitRatio >= 0.4f, 5f);
			this._hasThrowing = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.HasThrowingUnitRatio >= 0.5f, 5f);
			this._isRangedFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.RangedUnitRatio > CS$<>8__locals1.<>4__this.InfantryUnitRatio && CS$<>8__locals1.<>4__this.RangedUnitRatio >= CS$<>8__locals1.<>4__this.CavalryUnitRatio && CS$<>8__locals1.<>4__this.RangedUnitRatio >= CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._isCavalryFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.CavalryUnitRatio > CS$<>8__locals1.<>4__this.InfantryUnitRatio && CS$<>8__locals1.<>4__this.CavalryUnitRatio > CS$<>8__locals1.<>4__this.RangedUnitRatio && CS$<>8__locals1.<>4__this.CavalryUnitRatio >= CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio, 5f);
			this._isRangedCavalryFormation = new QueryData<bool>(() => CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > CS$<>8__locals1.<>4__this.InfantryUnitRatio && CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > CS$<>8__locals1.<>4__this.RangedUnitRatio && CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > CS$<>8__locals1.<>4__this.CavalryUnitRatio, 5f);
			QueryData<float>.SetupSyncGroup(new IQueryData[]
			{
				this._infantryUnitRatio, this._hasShieldUnitRatio, this._rangedUnitRatio, this._cavalryUnitRatio, this._rangedCavalryUnitRatio, this._isMeleeFormation, this._isInfantryFormation, this._hasShield, this._isRangedFormation, this._isCavalryFormation,
				this._isRangedCavalryFormation
			});
			this._movementSpeedMaximum = new QueryData<float>(new Func<float>(CS$<>8__locals1.formation.GetAverageMaximumMovementSpeedOfUnits), 10f);
			this._maximumMissileRange = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits == 0)
				{
					return 0f;
				}
				float maximumRange = 0f;
				CS$<>8__locals1.formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					if (agent.MaximumMissileRange > maximumRange)
					{
						maximumRange = agent.MaximumMissileRange;
					}
				}, null);
				return maximumRange;
			}, 10f);
			this._missileRangeAdjusted = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits == 0)
				{
					return 0f;
				}
				float sum = 0f;
				CS$<>8__locals1.formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					sum += agent.MissileRangeAdjusted;
				}, null);
				return sum / (float)CS$<>8__locals1.formation.CountOfUnits;
			}, 10f);
			this._localInfantryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsInfantry)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			this._localRangedUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsRanged)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			this._localCavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsCavalry)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			this._localRangedCavalryUnitRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.LocalAllyUnits.Count != 0)
				{
					return 1f * (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count<Agent>(new Func<Agent, bool>(QueryLibrary.IsRangedCavalry)) / (float)CS$<>8__locals1.<>4__this.LocalAllyUnits.Count;
				}
				return 0f;
			}, 15f);
			QueryData<float>.SetupSyncGroup(new IQueryData[] { this._localInfantryUnitRatio, this._localRangedUnitRatio, this._localCavalryUnitRatio, this._localRangedCavalryUnitRatio });
			this._localAllyPower = new QueryData<float>(() => CS$<>8__locals1.<>4__this.LocalAllyUnits.Sum<Agent>((Agent lau) => lau.CharacterPowerCached), 5f);
			this._localEnemyPower = new QueryData<float>(() => CS$<>8__locals1.<>4__this.LocalEnemyUnits.Sum<Agent>((Agent leu) => leu.CharacterPowerCached), 5f);
			this._localPowerRatio = new QueryData<float>(() => MBMath.ClampFloat(MathF.Sqrt((CS$<>8__locals1.<>4__this.LocalAllyUnits.Sum<Agent>((Agent lau) => lau.CharacterPowerCached) + 1f) * 1f / (CS$<>8__locals1.<>4__this.LocalEnemyUnits.Sum<Agent>((Agent leu) => leu.CharacterPowerCached) + 1f)), 0.5f, 1.75f), 5f);
			this._casualtyRatio = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.formation.CountOfUnits == 0)
				{
					return 0f;
				}
				CasualtyHandler missionBehavior = mission.GetMissionBehavior<CasualtyHandler>();
				int num14 = ((missionBehavior != null) ? missionBehavior.GetCasualtyCountOfFormation(CS$<>8__locals1.formation) : 0);
				return 1f - (float)num14 * 1f / (float)(num14 + CS$<>8__locals1.formation.CountOfUnits);
			}, 10f);
			this._isUnderRangedAttack = new QueryData<bool>(() => CS$<>8__locals1.formation.GetUnderAttackTypeOfUnits(10f) == Agent.UnderAttackType.UnderRangedAttack, 3f);
			this._underRangedAttackRatio = new QueryData<float>(delegate
			{
				float currentTime = MBCommon.GetTotalMissionTime();
				int countOfUnitsWithCondition = CS$<>8__locals1.formation.GetCountOfUnitsWithCondition((Agent agent) => currentTime - agent.LastRangedHitTime < 10f);
				if (CS$<>8__locals1.formation.CountOfUnits <= 0)
				{
					return 0f;
				}
				return (float)countOfUnitsWithCondition / (float)CS$<>8__locals1.formation.CountOfUnits;
			}, 3f);
			this._makingRangedAttackRatio = new QueryData<float>(delegate
			{
				float currentTime = MBCommon.GetTotalMissionTime();
				int countOfUnitsWithCondition2 = CS$<>8__locals1.formation.GetCountOfUnitsWithCondition((Agent agent) => currentTime - agent.LastRangedAttackTime < 10f);
				if (CS$<>8__locals1.formation.CountOfUnits <= 0)
				{
					return 0f;
				}
				return (float)countOfUnitsWithCondition2 / (float)CS$<>8__locals1.formation.CountOfUnits;
			}, 3f);
			this._closestEnemyAgent = new QueryData<Agent>(delegate
			{
				float num15 = float.MaxValue;
				Agent agent5 = null;
				foreach (Team team in mission.Teams)
				{
					if (team.IsEnemyOf(CS$<>8__locals1.formation.Team))
					{
						foreach (Agent agent6 in team.ActiveAgents)
						{
							float num16 = agent6.Position.DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f));
							if (num16 < num15)
							{
								num15 = num16;
								agent5 = agent6;
							}
						}
					}
				}
				return agent5;
			}, 1.5f);
			this._closestSignificantlyLargeEnemyFormation = new QueryData<Formation>(delegate
			{
				float num17 = float.MaxValue;
				Formation formation3 = null;
				float num18 = float.MaxValue;
				Formation formation4 = null;
				foreach (Team team2 in mission.Teams)
				{
					if (team2.IsEnemyOf(CS$<>8__locals1.formation.Team))
					{
						foreach (Formation formation5 in team2.FormationsIncludingSpecialAndEmpty)
						{
							if (formation5.CountOfUnits > 0)
							{
								if (formation5.QuerySystem.FormationPower / CS$<>8__locals1.<>4__this.FormationPower > 0.2f || formation5.QuerySystem.FormationPower * CS$<>8__locals1.<>4__this.Team.TeamPower / (formation5.Team.QuerySystem.TeamPower * CS$<>8__locals1.<>4__this.FormationPower) > 0.2f)
								{
									float num19 = formation5.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f));
									if (num19 < num17)
									{
										num17 = num19;
										formation3 = formation5;
									}
								}
								else if (formation3 == null)
								{
									float num20 = formation5.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f));
									if (num20 < num18)
									{
										num18 = num20;
										formation4 = formation5;
									}
								}
							}
						}
					}
				}
				return formation3 ?? formation4;
			}, 1.5f);
			this._fastestSignificantlyLargeEnemyFormation = new QueryData<Formation>(delegate
			{
				float num21 = float.MaxValue;
				Formation formation6 = null;
				float num22 = float.MaxValue;
				Formation formation7 = null;
				foreach (Team team3 in mission.Teams)
				{
					if (team3.IsEnemyOf(CS$<>8__locals1.formation.Team))
					{
						foreach (Formation formation8 in team3.FormationsIncludingSpecialAndEmpty)
						{
							if (formation8.CountOfUnits > 0)
							{
								if (formation8.QuerySystem.FormationPower / CS$<>8__locals1.<>4__this.FormationPower > 0.2f || formation8.QuerySystem.FormationPower * CS$<>8__locals1.<>4__this.Team.TeamPower / (formation8.Team.QuerySystem.TeamPower * CS$<>8__locals1.<>4__this.FormationPower) > 0.2f)
								{
									float num23 = formation8.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f)) / (formation8.CachedMovementSpeed * formation8.CachedMovementSpeed);
									if (num23 < num21)
									{
										num21 = num23;
										formation6 = formation8;
									}
								}
								else if (formation6 == null)
								{
									float num24 = formation8.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(new Vec3(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition, CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.GetNavMeshZ(), -1f)) / (formation8.CachedMovementSpeed * formation8.CachedMovementSpeed);
									if (num24 < num22)
									{
										num22 = num24;
										formation7 = formation8;
									}
								}
							}
						}
					}
				}
				return formation6 ?? formation7;
			}, 1.5f);
			this._mainClass = new QueryData<FormationClass>(delegate
			{
				FormationClass formationClass = FormationClass.Infantry;
				float num25 = CS$<>8__locals1.<>4__this.InfantryUnitRatio;
				if (CS$<>8__locals1.<>4__this.RangedUnitRatio > num25)
				{
					formationClass = FormationClass.Ranged;
					num25 = CS$<>8__locals1.<>4__this.RangedUnitRatio;
				}
				if (CS$<>8__locals1.<>4__this.CavalryUnitRatio > num25)
				{
					formationClass = FormationClass.Cavalry;
					num25 = CS$<>8__locals1.<>4__this.CavalryUnitRatio;
				}
				if (CS$<>8__locals1.<>4__this.RangedCavalryUnitRatio > num25)
				{
					formationClass = FormationClass.HorseArcher;
				}
				return formationClass;
			}, 15f);
			this._mainFormation = new QueryData<Formation>(delegate
			{
				IEnumerable<Formation> formationsIncludingSpecialAndEmpty = CS$<>8__locals1.formation.Team.FormationsIncludingSpecialAndEmpty;
				Func<Formation, bool> func;
				if ((func = CS$<>8__locals1.<>9__51) == null)
				{
					func = (CS$<>8__locals1.<>9__51 = (Formation f) => f.CountOfUnits > 0 && f.AI.IsMainFormation && f != CS$<>8__locals1.formation);
				}
				return formationsIncludingSpecialAndEmpty.FirstOrDefault<Formation>(func);
			}, 15f);
			this._mainFormationReliabilityFactor = new QueryData<float>(delegate
			{
				if (CS$<>8__locals1.<>4__this.MainFormation == null)
				{
					return 0f;
				}
				float num26 = ((CS$<>8__locals1.<>4__this.MainFormation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.Charge || CS$<>8__locals1.<>4__this.MainFormation.GetReadonlyMovementOrderReference().OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget || CS$<>8__locals1.<>4__this.MainFormation.GetReadonlyMovementOrderReference() == MovementOrder.MovementOrderRetreat) ? 0.5f : 1f);
				float num27 = ((CS$<>8__locals1.<>4__this.MainFormation.GetUnderAttackTypeOfUnits(10f) == Agent.UnderAttackType.UnderMeleeAttack) ? 0.8f : 1f);
				return num26 * num27;
			}, 5f);
			this._weightedAverageEnemyPosition = new QueryData<Vec2>(() => CS$<>8__locals1.<>4__this.Formation.Team.GetWeightedAverageOfEnemies(CS$<>8__locals1.<>4__this.Formation.CurrentPosition), 0.5f);
			this._highGroundCloseToForeseenBattleGround = new QueryData<Vec2>(delegate
			{
				WorldPosition cachedMedianPosition = CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition);
				WorldPosition medianTargetFormationPosition = CS$<>8__locals1.<>4__this.Team.MedianTargetFormationPosition;
				return mission.FindPositionWithBiggestSlopeTowardsDirectionInSquare(ref cachedMedianPosition, CS$<>8__locals1.<>4__this.Formation.CachedAveragePosition.Distance(CS$<>8__locals1.<>4__this.Team.MedianTargetFormationPosition.AsVec2) * 0.5f, ref medianTargetFormationPosition).AsVec2;
			}, 10f);
			this._insideCastleUnitCountIncludingUnpositioned = new QueryData<int>(() => CS$<>8__locals1.<>4__this.Formation.CountUnitsOnNavMeshIDMod10(1, false), 3f);
			this._insideCastleUnitCountPositioned = new QueryData<int>(() => CS$<>8__locals1.<>4__this.Formation.CountUnitsOnNavMeshIDMod10(1, true), 3f);
			this._isUnderCavalryChargeFromFront = new QueryData<bool>(delegate
			{
				FormationQuerySystem closestSignificantlyLargeEnemyFormation = CS$<>8__locals1.<>4__this.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation;
				if (closestSignificantlyLargeEnemyFormation != null && closestSignificantlyLargeEnemyFormation.IsCavalryFormationReadOnly)
				{
					Vec2 cachedCurrentVelocity = closestSignificantlyLargeEnemyFormation.Formation.CachedCurrentVelocity;
					float num28 = cachedCurrentVelocity.Normalize();
					Vec2 vec9 = CS$<>8__locals1.<>4__this.Formation.CachedMedianPosition.AsVec2 - closestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2;
					float num29 = vec9.Normalize();
					bool flag = cachedCurrentVelocity.DotProduct(vec9) > 0.75f;
					bool flag2 = CS$<>8__locals1.<>4__this.Formation.Arrangement is CircularFormation || CS$<>8__locals1.<>4__this.Formation.Arrangement is SquareFormation || vec9.DotProduct(CS$<>8__locals1.<>4__this.Formation.Direction) < -0.75f;
					if (flag && flag2)
					{
						return num29 / num28 < 15f;
					}
				}
				return false;
			}, 2f);
			this.InitializeTelemetryScopeNames();
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00049528 File Offset: 0x00047728
		public void EvaluateAllPreliminaryQueryData()
		{
			float currentTime = Mission.Current.CurrentTime;
			this._infantryUnitRatio.Evaluate(currentTime);
			this._hasShieldUnitRatio.Evaluate(currentTime);
			this._rangedUnitRatio.Evaluate(currentTime);
			this._cavalryUnitRatio.Evaluate(currentTime);
			this._rangedCavalryUnitRatio.Evaluate(currentTime);
			this._isInfantryFormation.Evaluate(currentTime);
			this._hasShield.Evaluate(currentTime);
			this._isRangedFormation.Evaluate(currentTime);
			this._isCavalryFormation.Evaluate(currentTime);
			this._isRangedCavalryFormation.Evaluate(currentTime);
			this._isMeleeFormation.Evaluate(currentTime);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x000495C4 File Offset: 0x000477C4
		public void ForceExpireCavalryUnitRatio()
		{
			this._cavalryUnitRatio.Expire();
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000495D4 File Offset: 0x000477D4
		public void Expire()
		{
			this._formationPower.Expire();
			this._formationMeleeFightingPower.Expire();
			this._estimatedDirection.Expire();
			this._averageAllyPosition.Expire();
			this._idealAverageDisplacement.Expire();
			this._localAllyUnits.Expire();
			this._localEnemyUnits.Expire();
			this._mainClass.Expire();
			this._infantryUnitRatio.Expire();
			this._hasShieldUnitRatio.Expire();
			this._rangedUnitRatio.Expire();
			this._cavalryUnitRatio.Expire();
			this._rangedCavalryUnitRatio.Expire();
			this._isMeleeFormation.Expire();
			this._isInfantryFormation.Expire();
			this._hasShield.Expire();
			this._isRangedFormation.Expire();
			this._isCavalryFormation.Expire();
			this._isRangedCavalryFormation.Expire();
			this._movementSpeedMaximum.Expire();
			this._maximumMissileRange.Expire();
			this._missileRangeAdjusted.Expire();
			this._localInfantryUnitRatio.Expire();
			this._localRangedUnitRatio.Expire();
			this._localCavalryUnitRatio.Expire();
			this._localRangedCavalryUnitRatio.Expire();
			this._localAllyPower.Expire();
			this._localEnemyPower.Expire();
			this._localPowerRatio.Expire();
			this._casualtyRatio.Expire();
			this._isUnderRangedAttack.Expire();
			this._underRangedAttackRatio.Expire();
			this._makingRangedAttackRatio.Expire();
			this._mainFormation.Expire();
			this._mainFormationReliabilityFactor.Expire();
			this._weightedAverageEnemyPosition.Expire();
			this._closestSignificantlyLargeEnemyFormation.Expire();
			this._fastestSignificantlyLargeEnemyFormation.Expire();
			this._highGroundCloseToForeseenBattleGround.Expire();
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00049790 File Offset: 0x00047990
		public void ExpireAfterUnitAddRemove()
		{
			this._formationPower.Expire();
			float currentTime = Mission.Current.CurrentTime;
			this._infantryUnitRatio.Evaluate(currentTime);
			this._hasShieldUnitRatio.Evaluate(currentTime);
			this._rangedUnitRatio.Evaluate(currentTime);
			this._cavalryUnitRatio.Evaluate(currentTime);
			this._rangedCavalryUnitRatio.Evaluate(currentTime);
			this._isMeleeFormation.Evaluate(currentTime);
			this._isInfantryFormation.Evaluate(currentTime);
			this._hasShield.Evaluate(currentTime);
			this._isRangedFormation.Evaluate(currentTime);
			this._isCavalryFormation.Evaluate(currentTime);
			this._isRangedCavalryFormation.Evaluate(currentTime);
			this._mainClass.Evaluate(currentTime);
			if (this.Formation.CountOfUnits == 0)
			{
				this._infantryUnitRatio.SetValue(0f, currentTime);
				this._hasShieldUnitRatio.SetValue(0f, currentTime);
				this._rangedUnitRatio.SetValue(0f, currentTime);
				this._cavalryUnitRatio.SetValue(0f, currentTime);
				this._rangedCavalryUnitRatio.SetValue(0f, currentTime);
				this._isMeleeFormation.SetValue(false, currentTime);
				this._isInfantryFormation.SetValue(true, currentTime);
				this._hasShield.SetValue(false, currentTime);
				this._isRangedFormation.SetValue(false, currentTime);
				this._isCavalryFormation.SetValue(false, currentTime);
				this._isRangedCavalryFormation.SetValue(false, currentTime);
			}
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000498F6 File Offset: 0x00047AF6
		private void InitializeTelemetryScopeNames()
		{
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000498F8 File Offset: 0x00047AF8
		public float GetClassWeightedFactor(float infantryWeight, float rangedWeight, float cavalryWeight, float rangedCavalryWeight)
		{
			return this.InfantryUnitRatio * infantryWeight + this.RangedUnitRatio * rangedWeight + this.CavalryUnitRatio * cavalryWeight + this.RangedCavalryUnitRatio * rangedCavalryWeight;
		}

		// Token: 0x040004FD RID: 1277
		public readonly Formation Formation;

		// Token: 0x040004FE RID: 1278
		private readonly QueryData<float> _formationPower;

		// Token: 0x040004FF RID: 1279
		private readonly QueryData<float> _formationMeleeFightingPower;

		// Token: 0x04000500 RID: 1280
		private readonly QueryData<Vec2> _estimatedDirection;

		// Token: 0x04000501 RID: 1281
		private readonly QueryData<float> _estimatedInterval;

		// Token: 0x04000502 RID: 1282
		private readonly QueryData<Vec2> _averageAllyPosition;

		// Token: 0x04000503 RID: 1283
		private readonly QueryData<float> _idealAverageDisplacement;

		// Token: 0x04000504 RID: 1284
		private readonly QueryData<MBList<Agent>> _localAllyUnits;

		// Token: 0x04000505 RID: 1285
		private readonly QueryData<MBList<Agent>> _localEnemyUnits;

		// Token: 0x04000506 RID: 1286
		private readonly QueryData<FormationClass> _mainClass;

		// Token: 0x04000507 RID: 1287
		private readonly QueryData<float> _infantryUnitRatio;

		// Token: 0x04000508 RID: 1288
		private readonly QueryData<float> _hasShieldUnitRatio;

		// Token: 0x04000509 RID: 1289
		private readonly QueryData<float> _hasThrowingUnitRatio;

		// Token: 0x0400050A RID: 1290
		private readonly QueryData<float> _rangedUnitRatio;

		// Token: 0x0400050B RID: 1291
		private readonly QueryData<int> _insideCastleUnitCountIncludingUnpositioned;

		// Token: 0x0400050C RID: 1292
		private readonly QueryData<int> _insideCastleUnitCountPositioned;

		// Token: 0x0400050D RID: 1293
		private readonly QueryData<float> _cavalryUnitRatio;

		// Token: 0x0400050E RID: 1294
		private readonly QueryData<float> _rangedCavalryUnitRatio;

		// Token: 0x0400050F RID: 1295
		private readonly QueryData<bool> _isMeleeFormation;

		// Token: 0x04000510 RID: 1296
		private readonly QueryData<bool> _isInfantryFormation;

		// Token: 0x04000511 RID: 1297
		private readonly QueryData<bool> _hasShield;

		// Token: 0x04000512 RID: 1298
		private readonly QueryData<bool> _hasThrowing;

		// Token: 0x04000513 RID: 1299
		private readonly QueryData<bool> _isRangedFormation;

		// Token: 0x04000514 RID: 1300
		private readonly QueryData<bool> _isCavalryFormation;

		// Token: 0x04000515 RID: 1301
		private readonly QueryData<bool> _isRangedCavalryFormation;

		// Token: 0x04000516 RID: 1302
		private readonly QueryData<float> _movementSpeedMaximum;

		// Token: 0x04000517 RID: 1303
		private readonly QueryData<float> _maximumMissileRange;

		// Token: 0x04000518 RID: 1304
		private readonly QueryData<float> _missileRangeAdjusted;

		// Token: 0x04000519 RID: 1305
		private readonly QueryData<float> _localInfantryUnitRatio;

		// Token: 0x0400051A RID: 1306
		private readonly QueryData<float> _localRangedUnitRatio;

		// Token: 0x0400051B RID: 1307
		private readonly QueryData<float> _localCavalryUnitRatio;

		// Token: 0x0400051C RID: 1308
		private readonly QueryData<float> _localRangedCavalryUnitRatio;

		// Token: 0x0400051D RID: 1309
		private readonly QueryData<float> _localAllyPower;

		// Token: 0x0400051E RID: 1310
		private readonly QueryData<float> _localEnemyPower;

		// Token: 0x0400051F RID: 1311
		private readonly QueryData<float> _localPowerRatio;

		// Token: 0x04000520 RID: 1312
		private readonly QueryData<float> _casualtyRatio;

		// Token: 0x04000521 RID: 1313
		private readonly QueryData<bool> _isUnderRangedAttack;

		// Token: 0x04000522 RID: 1314
		private readonly QueryData<float> _underRangedAttackRatio;

		// Token: 0x04000523 RID: 1315
		private readonly QueryData<float> _makingRangedAttackRatio;

		// Token: 0x04000524 RID: 1316
		private readonly QueryData<Formation> _mainFormation;

		// Token: 0x04000525 RID: 1317
		private readonly QueryData<float> _mainFormationReliabilityFactor;

		// Token: 0x04000526 RID: 1318
		private readonly QueryData<Vec2> _weightedAverageEnemyPosition;

		// Token: 0x04000527 RID: 1319
		private readonly QueryData<Agent> _closestEnemyAgent;

		// Token: 0x04000528 RID: 1320
		private readonly QueryData<Formation> _closestSignificantlyLargeEnemyFormation;

		// Token: 0x04000529 RID: 1321
		private readonly QueryData<Formation> _fastestSignificantlyLargeEnemyFormation;

		// Token: 0x0400052A RID: 1322
		private readonly QueryData<Vec2> _highGroundCloseToForeseenBattleGround;

		// Token: 0x0400052B RID: 1323
		private readonly QueryData<bool> _isUnderCavalryChargeFromFront;
	}
}
