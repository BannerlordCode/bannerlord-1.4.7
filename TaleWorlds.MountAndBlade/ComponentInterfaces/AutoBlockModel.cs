using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x02000401 RID: 1025
	public abstract class AutoBlockModel : MBGameModel<AutoBlockModel>
	{
		// Token: 0x060037B8 RID: 14264
		public abstract Agent.UsageDirection GetBlockDirection(Mission mission);
	}
}
