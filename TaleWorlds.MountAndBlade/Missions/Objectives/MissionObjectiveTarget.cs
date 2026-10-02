using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Objectives
{
	// Token: 0x020003E6 RID: 998
	public abstract class MissionObjectiveTarget
	{
		// Token: 0x060036F6 RID: 14070
		public abstract bool IsActive();

		// Token: 0x060036F7 RID: 14071
		public abstract TextObject GetName();

		// Token: 0x060036F8 RID: 14072
		public abstract Vec3 GetGlobalPosition();
	}
}
