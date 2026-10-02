using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000394 RID: 916
	[EngineStruct("Weapon_stats_data", false, null)]
	public struct WeaponStatsData
	{
		// Token: 0x04001640 RID: 5696
		public MatrixFrame WeaponFrame;

		// Token: 0x04001641 RID: 5697
		public Vec3 RotationSpeed;

		// Token: 0x04001642 RID: 5698
		public ulong WeaponFlags;

		// Token: 0x04001643 RID: 5699
		public uint Properties;

		// Token: 0x04001644 RID: 5700
		public int WeaponClass;

		// Token: 0x04001645 RID: 5701
		public int AmmoClass;

		// Token: 0x04001646 RID: 5702
		public int ItemUsageIndex;

		// Token: 0x04001647 RID: 5703
		public int ThrustSpeed;

		// Token: 0x04001648 RID: 5704
		public int SwingSpeed;

		// Token: 0x04001649 RID: 5705
		public int MissileSpeed;

		// Token: 0x0400164A RID: 5706
		public int ShieldArmor;

		// Token: 0x0400164B RID: 5707
		public int ThrustDamage;

		// Token: 0x0400164C RID: 5708
		public int SwingDamage;

		// Token: 0x0400164D RID: 5709
		public int DefendSpeed;

		// Token: 0x0400164E RID: 5710
		public int Accuracy;

		// Token: 0x0400164F RID: 5711
		public int WeaponLength;

		// Token: 0x04001650 RID: 5712
		public float WeaponBalance;

		// Token: 0x04001651 RID: 5713
		public float SweetSpot;

		// Token: 0x04001652 RID: 5714
		public short MaxDataValue;

		// Token: 0x04001653 RID: 5715
		public short ReloadPhaseCount;

		// Token: 0x04001654 RID: 5716
		public int ThrustDamageType;

		// Token: 0x04001655 RID: 5717
		public int SwingDamageType;
	}
}
