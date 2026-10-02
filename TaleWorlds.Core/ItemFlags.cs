using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000094 RID: 148
	[Flags]
	public enum ItemFlags : uint
	{
		// Token: 0x04000466 RID: 1126
		ForceAttachOffHandPrimaryItemBone = 256U,
		// Token: 0x04000467 RID: 1127
		ForceAttachOffHandSecondaryItemBone = 512U,
		// Token: 0x04000468 RID: 1128
		AttachmentMask = 768U,
		// Token: 0x04000469 RID: 1129
		NotUsableByFemale = 1024U,
		// Token: 0x0400046A RID: 1130
		NotUsableByMale = 2048U,
		// Token: 0x0400046B RID: 1131
		DropOnWeaponChange = 4096U,
		// Token: 0x0400046C RID: 1132
		DropOnAnyAction = 8192U,
		// Token: 0x0400046D RID: 1133
		CannotBePickedUp = 16384U,
		// Token: 0x0400046E RID: 1134
		CanBePickedUpFromCorpse = 32768U,
		// Token: 0x0400046F RID: 1135
		QuickFadeOut = 65536U,
		// Token: 0x04000470 RID: 1136
		WoodenAttack = 131072U,
		// Token: 0x04000471 RID: 1137
		WoodenParry = 262144U,
		// Token: 0x04000472 RID: 1138
		HeldInOffHand = 524288U,
		// Token: 0x04000473 RID: 1139
		HasToBeHeldUp = 1048576U,
		// Token: 0x04000474 RID: 1140
		UseTeamColor = 2097152U,
		// Token: 0x04000475 RID: 1141
		Civilian = 4194304U,
		// Token: 0x04000476 RID: 1142
		DoNotScaleBodyAccordingToWeaponLength = 8388608U,
		// Token: 0x04000477 RID: 1143
		DoesNotHideChest = 16777216U,
		// Token: 0x04000478 RID: 1144
		NotStackable = 33554432U,
		// Token: 0x04000479 RID: 1145
		Stealth = 67108864U,
		// Token: 0x0400047A RID: 1146
		DoesNotSpawnWhenDropped = 134217728U
	}
}
