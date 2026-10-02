using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200018E RID: 398
	[EngineStruct("Animation_system_bone_data_biped", false, null)]
	[Serializable]
	public struct AnimationSystemBoneDataBiped
	{
		// Token: 0x040005D7 RID: 1495
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public sbyte[] RagdollStationaryCheckBoneIndices;

		// Token: 0x040005D8 RID: 1496
		public sbyte RagdollStationaryCheckBoneCount;

		// Token: 0x040005D9 RID: 1497
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public sbyte[] MoveAdderBoneIndices;

		// Token: 0x040005DA RID: 1498
		public sbyte MoveAdderBoneCount;

		// Token: 0x040005DB RID: 1499
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public sbyte[] SplashDecalBoneIndices;

		// Token: 0x040005DC RID: 1500
		public sbyte SplashDecalBoneCount;

		// Token: 0x040005DD RID: 1501
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public sbyte[] BloodBurstBoneIndices;

		// Token: 0x040005DE RID: 1502
		public sbyte BloodBurstBoneCount;

		// Token: 0x040005DF RID: 1503
		public sbyte MainHandBoneIndex;

		// Token: 0x040005E0 RID: 1504
		public sbyte OffHandBoneIndex;

		// Token: 0x040005E1 RID: 1505
		public sbyte MainHandItemBoneIndex;

		// Token: 0x040005E2 RID: 1506
		public sbyte OffHandItemBoneIndex;

		// Token: 0x040005E3 RID: 1507
		public sbyte MainHandItemSecondaryBoneIndex;

		// Token: 0x040005E4 RID: 1508
		public sbyte OffHandItemSecondaryBoneIndex;

		// Token: 0x040005E5 RID: 1509
		public sbyte OffHandShoulderBoneIndex;

		// Token: 0x040005E6 RID: 1510
		public sbyte HandNumBonesForIk;

		// Token: 0x040005E7 RID: 1511
		public sbyte PrimaryFootBoneIndex;

		// Token: 0x040005E8 RID: 1512
		public sbyte SecondaryFootBoneIndex;

		// Token: 0x040005E9 RID: 1513
		public sbyte RightFootIkEndEffectorBoneIndex;

		// Token: 0x040005EA RID: 1514
		public sbyte LeftFootIkEndEffectorBoneIndex;

		// Token: 0x040005EB RID: 1515
		public sbyte RightFootIkTipBoneIndex;

		// Token: 0x040005EC RID: 1516
		public sbyte LeftFootIkTipBoneIndex;

		// Token: 0x040005ED RID: 1517
		public sbyte FootNumBonesForIk;
	}
}
