using System;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003E7 RID: 999
	public abstract class MissionObjectiveTarget<T> : MissionObjectiveTarget
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x000E3640 File Offset: 0x000E1840
		public T Target { get; }

		// Token: 0x060036FB RID: 14075 RVA: 0x000E3648 File Offset: 0x000E1848
		public MissionObjectiveTarget(T target)
		{
			this.Target = target;
		}
	}
}
