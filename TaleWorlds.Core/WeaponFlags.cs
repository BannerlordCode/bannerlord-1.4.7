using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000095 RID: 149
	[Flags]
	public enum WeaponFlags : ulong
	{
		// Token: 0x0400047C RID: 1148
		MeleeWeapon = 1UL,
		// Token: 0x0400047D RID: 1149
		RangedWeapon = 2UL,
		// Token: 0x0400047E RID: 1150
		WeaponMask = 3UL,
		// Token: 0x0400047F RID: 1151
		FirearmAmmo = 4UL,
		// Token: 0x04000480 RID: 1152
		NotUsableWithOneHand = 16UL,
		// Token: 0x04000481 RID: 1153
		NotUsableWithTwoHand = 32UL,
		// Token: 0x04000482 RID: 1154
		HandUsageMask = 48UL,
		// Token: 0x04000483 RID: 1155
		WideGrip = 64UL,
		// Token: 0x04000484 RID: 1156
		AttachAmmoToVisual = 128UL,
		// Token: 0x04000485 RID: 1157
		Consumable = 256UL,
		// Token: 0x04000486 RID: 1158
		HasHitPoints = 512UL,
		// Token: 0x04000487 RID: 1159
		DataValueMask = 768UL,
		// Token: 0x04000488 RID: 1160
		HasString = 1024UL,
		// Token: 0x04000489 RID: 1161
		StringHeldByHand = 3072UL,
		// Token: 0x0400048A RID: 1162
		UnloadWhenSheathed = 4096UL,
		// Token: 0x0400048B RID: 1163
		AffectsArea = 8192UL,
		// Token: 0x0400048C RID: 1164
		AffectsAreaBig = 16384UL,
		// Token: 0x0400048D RID: 1165
		Burning = 32768UL,
		// Token: 0x0400048E RID: 1166
		BonusAgainstShield = 65536UL,
		// Token: 0x0400048F RID: 1167
		CanPenetrateShield = 131072UL,
		// Token: 0x04000490 RID: 1168
		CantReloadOnHorseback = 262144UL,
		// Token: 0x04000491 RID: 1169
		AutoReload = 524288UL,
		// Token: 0x04000492 RID: 1170
		CanBeUsedWhileCrouched = 1048576UL,
		// Token: 0x04000493 RID: 1171
		TwoHandIdleOnMount = 2097152UL,
		// Token: 0x04000494 RID: 1172
		NoBlood = 4194304UL,
		// Token: 0x04000495 RID: 1173
		PenaltyWithShield = 8388608UL,
		// Token: 0x04000496 RID: 1174
		CanDismount = 16777216UL,
		// Token: 0x04000497 RID: 1175
		CanHook = 33554432UL,
		// Token: 0x04000498 RID: 1176
		CanKnockDown = 67108864UL,
		// Token: 0x04000499 RID: 1177
		CanCrushThrough = 134217728UL,
		// Token: 0x0400049A RID: 1178
		CanBlockRanged = 268435456UL,
		// Token: 0x0400049B RID: 1179
		MissileWithPhysics = 536870912UL,
		// Token: 0x0400049C RID: 1180
		MultiplePenetration = 1073741824UL,
		// Token: 0x0400049D RID: 1181
		LeavesTrail = 2147483648UL,
		// Token: 0x0400049E RID: 1182
		UseHandAsThrowBase = 4294967296UL,
		// Token: 0x0400049F RID: 1183
		HeldBackwards = 8589934592UL,
		// Token: 0x040004A0 RID: 1184
		CanKillEvenIfBlunt = 17179869184UL,
		// Token: 0x040004A1 RID: 1185
		AmmoBreaksOnBounceBack = 68719476736UL,
		// Token: 0x040004A2 RID: 1186
		AmmoCanBreakOnBounceBack = 137438953472UL,
		// Token: 0x040004A3 RID: 1187
		AmmoBreakOnBounceBackMask = 206158430208UL,
		// Token: 0x040004A4 RID: 1188
		AmmoSticksWhenShot = 274877906944UL
	}
}
