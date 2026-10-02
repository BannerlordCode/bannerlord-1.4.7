using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000099 RID: 153
	public enum ManagedParametersEnum
	{
		// Token: 0x040004B3 RID: 1203
		EnableCampaignTutorials,
		// Token: 0x040004B4 RID: 1204
		ReducedMouseSensitivityMultiplier,
		// Token: 0x040004B5 RID: 1205
		MeleeAddedElevationForCrosshair,
		// Token: 0x040004B6 RID: 1206
		BipedalRadius,
		// Token: 0x040004B7 RID: 1207
		QuadrupedalRadius,
		// Token: 0x040004B8 RID: 1208
		BipedalCombatSpeedMinMultiplier,
		// Token: 0x040004B9 RID: 1209
		BipedalCombatSpeedMaxMultiplier,
		// Token: 0x040004BA RID: 1210
		BipedalRangedReadySpeedMultiplier,
		// Token: 0x040004BB RID: 1211
		BipedalRangedReloadSpeedMultiplier,
		// Token: 0x040004BC RID: 1212
		DamageInterruptAttackThresholdPierce,
		// Token: 0x040004BD RID: 1213
		DamageInterruptAttackThresholdCut,
		// Token: 0x040004BE RID: 1214
		DamageInterruptAttackThresholdBlunt,
		// Token: 0x040004BF RID: 1215
		MakesRearAttackDamageThreshold,
		// Token: 0x040004C0 RID: 1216
		MissileMinimumDamageToStick,
		// Token: 0x040004C1 RID: 1217
		BreakableProjectileMinimumBreakSpeed,
		// Token: 0x040004C2 RID: 1218
		FistFightDamageMultiplier,
		// Token: 0x040004C3 RID: 1219
		FallDamageMultiplier,
		// Token: 0x040004C4 RID: 1220
		FallDamageAbsorption,
		// Token: 0x040004C5 RID: 1221
		FallSpeedReductionMultiplierForRiderDamage,
		// Token: 0x040004C6 RID: 1222
		SwingHitWithArmDamageMultiplier,
		// Token: 0x040004C7 RID: 1223
		ThrustHitWithArmDamageMultiplier,
		// Token: 0x040004C8 RID: 1224
		NonTipThrustHitDamageMultiplier,
		// Token: 0x040004C9 RID: 1225
		SwingCombatSpeedGraphZeroProgressValue,
		// Token: 0x040004CA RID: 1226
		SwingCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x040004CB RID: 1227
		SwingCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x040004CC RID: 1228
		SwingCombatSpeedGraphOneProgressValue,
		// Token: 0x040004CD RID: 1229
		OverSwingCombatSpeedGraphZeroProgressValue,
		// Token: 0x040004CE RID: 1230
		OverSwingCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x040004CF RID: 1231
		OverSwingCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x040004D0 RID: 1232
		OverSwingCombatSpeedGraphOneProgressValue,
		// Token: 0x040004D1 RID: 1233
		ThrustCombatSpeedGraphZeroProgressValue,
		// Token: 0x040004D2 RID: 1234
		ThrustCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x040004D3 RID: 1235
		ThrustCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x040004D4 RID: 1236
		ThrustCombatSpeedGraphOneProgressValue,
		// Token: 0x040004D5 RID: 1237
		StunPeriodAttackerSwing,
		// Token: 0x040004D6 RID: 1238
		StunPeriodAttackerThrust,
		// Token: 0x040004D7 RID: 1239
		StunDefendWeaponWeightOffsetShield,
		// Token: 0x040004D8 RID: 1240
		StunDefendWeaponWeightMultiplierWeaponWeight,
		// Token: 0x040004D9 RID: 1241
		StunDefendWeaponWeightBonusTwoHanded,
		// Token: 0x040004DA RID: 1242
		StunDefendWeaponWeightBonusPolearm,
		// Token: 0x040004DB RID: 1243
		StunMomentumTransferFactor,
		// Token: 0x040004DC RID: 1244
		StunDefendWeaponWeightParryMultiplier,
		// Token: 0x040004DD RID: 1245
		StunDefendWeaponWeightBonusRightStance,
		// Token: 0x040004DE RID: 1246
		StunDefendWeaponWeightBonusActiveBlocked,
		// Token: 0x040004DF RID: 1247
		StunDefendWeaponWeightBonusChamberBlocked,
		// Token: 0x040004E0 RID: 1248
		StunPeriodAttackerFriendlyFire,
		// Token: 0x040004E1 RID: 1249
		StunPeriodMax,
		// Token: 0x040004E2 RID: 1250
		ProjectileMaxPenetrationSpeed,
		// Token: 0x040004E3 RID: 1251
		ObjectMinPenetration,
		// Token: 0x040004E4 RID: 1252
		ObjectMaxPenetration,
		// Token: 0x040004E5 RID: 1253
		ProjectileMinPenetration,
		// Token: 0x040004E6 RID: 1254
		ProjectileMaxPenetration,
		// Token: 0x040004E7 RID: 1255
		RotatingProjectileMinPenetration,
		// Token: 0x040004E8 RID: 1256
		RotatingProjectileMaxPenetration,
		// Token: 0x040004E9 RID: 1257
		ShieldRightStanceBlockDamageMultiplier,
		// Token: 0x040004EA RID: 1258
		ShieldCorrectSideBlockDamageMultiplier,
		// Token: 0x040004EB RID: 1259
		AgentProjectileNormalWeight,
		// Token: 0x040004EC RID: 1260
		ProjectileNormalWeight,
		// Token: 0x040004ED RID: 1261
		ShieldPenetrationOffset,
		// Token: 0x040004EE RID: 1262
		ShieldPenetrationFactor,
		// Token: 0x040004EF RID: 1263
		AirFrictionJavelin,
		// Token: 0x040004F0 RID: 1264
		AirFrictionArrow,
		// Token: 0x040004F1 RID: 1265
		AirFrictionBallistaBolt,
		// Token: 0x040004F2 RID: 1266
		AirFrictionBullet,
		// Token: 0x040004F3 RID: 1267
		AirFrictionKnife,
		// Token: 0x040004F4 RID: 1268
		AirFrictionAxe,
		// Token: 0x040004F5 RID: 1269
		AirFrictionStone,
		// Token: 0x040004F6 RID: 1270
		AirFrictionBoulder,
		// Token: 0x040004F7 RID: 1271
		AirFrictionBallistaStone,
		// Token: 0x040004F8 RID: 1272
		AirFrictionBallistaBoulder,
		// Token: 0x040004F9 RID: 1273
		HeavyAttackMomentumMultiplier,
		// Token: 0x040004FA RID: 1274
		ActivateHeroTest,
		// Token: 0x040004FB RID: 1275
		Count
	}
}
