using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003DF RID: 991
	public class AgentList : AgentReadOnlyList
	{
		// Token: 0x060036AF RID: 13999 RVA: 0x000E2CCE File Offset: 0x000E0ECE
		public AgentList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000E2CD7 File Offset: 0x000E0ED7
		public AgentList(IEnumerable<Agent> collection)
			: base(collection)
		{
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000E2CE0 File Offset: 0x000E0EE0
		public AgentList(List<Agent> collection)
			: base(collection)
		{
		}
	}
}
