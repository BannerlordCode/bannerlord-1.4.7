using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E6 RID: 742
	public class CommunityGameJoinData
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000A4957 File Offset: 0x000A2B57
		// (set) Token: 0x06002AD2 RID: 10962 RVA: 0x000A495F File Offset: 0x000A2B5F
		public string Name { get; set; }

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x000A4968 File Offset: 0x000A2B68
		// (set) Token: 0x06002AD4 RID: 10964 RVA: 0x000A4970 File Offset: 0x000A2B70
		public PlayerId PlayerId { get; set; }
	}
}
