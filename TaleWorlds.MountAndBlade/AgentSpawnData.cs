using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000104 RID: 260
	[EngineStruct("Agent_spawn_data", false, null)]
	public struct AgentSpawnData
	{
		// Token: 0x040002D8 RID: 728
		public int HitPoints;

		// Token: 0x040002D9 RID: 729
		public int MonsterUsageIndex;

		// Token: 0x040002DA RID: 730
		public int Weight;

		// Token: 0x040002DB RID: 731
		public float StandingChestHeight;

		// Token: 0x040002DC RID: 732
		public float StandingPelvisHeight;

		// Token: 0x040002DD RID: 733
		public float StandingEyeHeight;

		// Token: 0x040002DE RID: 734
		public float CrouchEyeHeight;

		// Token: 0x040002DF RID: 735
		public float MountedEyeHeight;

		// Token: 0x040002E0 RID: 736
		public float RiderEyeHeightAdder;

		// Token: 0x040002E1 RID: 737
		public float JumpAcceleration;

		// Token: 0x040002E2 RID: 738
		public Vec3 EyeOffsetWrtHead;

		// Token: 0x040002E3 RID: 739
		public Vec3 FirstPersonCameraOffsetWrtHead;

		// Token: 0x040002E4 RID: 740
		public float RiderCameraHeightAdder;

		// Token: 0x040002E5 RID: 741
		public float RiderBodyCapsuleHeightAdder;

		// Token: 0x040002E6 RID: 742
		public float RiderBodyCapsuleForwardAdder;

		// Token: 0x040002E7 RID: 743
		public float ArmLength;

		// Token: 0x040002E8 RID: 744
		public float ArmWeight;

		// Token: 0x040002E9 RID: 745
		public float JumpSpeedLimit;

		// Token: 0x040002EA RID: 746
		public float RelativeSpeedLimitForCharge;
	}
}
