using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000114 RID: 276
	public class DisconnectInfo
	{
		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x00007F29 File Offset: 0x00006129
		// (set) Token: 0x06000615 RID: 1557 RVA: 0x00007F31 File Offset: 0x00006131
		public DisconnectType Type { get; set; }

		// Token: 0x06000616 RID: 1558 RVA: 0x00007F3A File Offset: 0x0000613A
		public DisconnectInfo()
		{
			this.Type = DisconnectType.Unknown;
		}
	}
}
