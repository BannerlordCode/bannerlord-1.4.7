using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200014E RID: 334
	public enum OrderType
	{
		// Token: 0x04000405 RID: 1029
		None,
		// Token: 0x04000406 RID: 1030
		Move,
		// Token: 0x04000407 RID: 1031
		MoveToLineSegment,
		// Token: 0x04000408 RID: 1032
		MoveToLineSegmentWithHorizontalLayout,
		// Token: 0x04000409 RID: 1033
		Charge,
		// Token: 0x0400040A RID: 1034
		ChargeWithTarget,
		// Token: 0x0400040B RID: 1035
		StandYourGround,
		// Token: 0x0400040C RID: 1036
		FollowMe,
		// Token: 0x0400040D RID: 1037
		FollowEntity,
		// Token: 0x0400040E RID: 1038
		Retreat,
		// Token: 0x0400040F RID: 1039
		AdvanceTenPaces,
		// Token: 0x04000410 RID: 1040
		FallBackTenPaces,
		// Token: 0x04000411 RID: 1041
		Advance,
		// Token: 0x04000412 RID: 1042
		FallBack,
		// Token: 0x04000413 RID: 1043
		LookAtEnemy,
		// Token: 0x04000414 RID: 1044
		LookAtDirection,
		// Token: 0x04000415 RID: 1045
		ArrangementLine,
		// Token: 0x04000416 RID: 1046
		ArrangementCloseOrder,
		// Token: 0x04000417 RID: 1047
		ArrangementLoose,
		// Token: 0x04000418 RID: 1048
		ArrangementCircular,
		// Token: 0x04000419 RID: 1049
		ArrangementSchiltron,
		// Token: 0x0400041A RID: 1050
		ArrangementVee,
		// Token: 0x0400041B RID: 1051
		ArrangementColumn,
		// Token: 0x0400041C RID: 1052
		ArrangementScatter,
		// Token: 0x0400041D RID: 1053
		FormCustom,
		// Token: 0x0400041E RID: 1054
		FormDeep,
		// Token: 0x0400041F RID: 1055
		FormWide,
		// Token: 0x04000420 RID: 1056
		FormWider,
		// Token: 0x04000421 RID: 1057
		CohesionHigh,
		// Token: 0x04000422 RID: 1058
		CohesionMedium,
		// Token: 0x04000423 RID: 1059
		CohesionLow,
		// Token: 0x04000424 RID: 1060
		HoldFire,
		// Token: 0x04000425 RID: 1061
		FireAtWill,
		// Token: 0x04000426 RID: 1062
		RideFree,
		// Token: 0x04000427 RID: 1063
		Mount,
		// Token: 0x04000428 RID: 1064
		Dismount,
		// Token: 0x04000429 RID: 1065
		AIControlOn,
		// Token: 0x0400042A RID: 1066
		AIControlOff,
		// Token: 0x0400042B RID: 1067
		Transfer,
		// Token: 0x0400042C RID: 1068
		Use,
		// Token: 0x0400042D RID: 1069
		AttackEntity,
		// Token: 0x0400042E RID: 1070
		PointDefence,
		// Token: 0x0400042F RID: 1071
		Count
	}
}
