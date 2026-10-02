using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000289 RID: 649
	public struct MissionFormationSpawnData
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x0008268A File Offset: 0x0008088A
		public int NumTroops
		{
			get
			{
				return this.FootTroopCount + this.MountedTroopCount;
			}
		}

		// Token: 0x04000DE8 RID: 3560
		public int FootTroopCount;

		// Token: 0x04000DE9 RID: 3561
		public int MountedTroopCount;
	}
}
