using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000288 RID: 648
	public class MissionSpawnPhase
	{
		// Token: 0x0600240D RID: 9229 RVA: 0x0008266D File Offset: 0x0008086D
		public void OnInitialTroopsSpawned()
		{
			this.InitialSpawnedNumber = this.InitialSpawnNumber;
			this.InitialSpawnNumber = 0;
		}

		// Token: 0x04000DE3 RID: 3555
		public int TotalSpawnNumber;

		// Token: 0x04000DE4 RID: 3556
		public int InitialSpawnedNumber;

		// Token: 0x04000DE5 RID: 3557
		public int InitialSpawnNumber;

		// Token: 0x04000DE6 RID: 3558
		public int RemainingSpawnNumber;

		// Token: 0x04000DE7 RID: 3559
		public int NumberActiveTroops;
	}
}
