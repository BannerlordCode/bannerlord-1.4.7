using System;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003E5 RID: 997
	public struct MissionObjectiveProgressInfo
	{
		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060036F5 RID: 14069 RVA: 0x000E362D File Offset: 0x000E182D
		public bool HasProgress
		{
			get
			{
				return this.RequiredProgressAmount > 0;
			}
		}

		// Token: 0x040017A7 RID: 6055
		public int RequiredProgressAmount;

		// Token: 0x040017A8 RID: 6056
		public int CurrentProgressAmount;
	}
}
