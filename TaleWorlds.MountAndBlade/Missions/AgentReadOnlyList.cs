using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Missions
{
	// Token: 0x020003E0 RID: 992
	public class AgentReadOnlyList : MBReadOnlyList<Agent>
	{
		// Token: 0x060036B2 RID: 14002 RVA: 0x000E2CE9 File Offset: 0x000E0EE9
		public AgentReadOnlyList(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000E2CF2 File Offset: 0x000E0EF2
		public AgentReadOnlyList(IEnumerable<Agent> collection)
			: base(collection)
		{
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000E2CFB File Offset: 0x000E0EFB
		public AgentReadOnlyList(List<Agent> collection)
			: base(collection)
		{
		}
	}
}
