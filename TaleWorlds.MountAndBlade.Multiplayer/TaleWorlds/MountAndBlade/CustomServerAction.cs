using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000011 RID: 17
	public class CustomServerAction
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000050B9 File Offset: 0x000032B9
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000050C1 File Offset: 0x000032C1
		public Action Execute { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000050CA File Offset: 0x000032CA
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000050D2 File Offset: 0x000032D2
		public GameServerEntry GameServerEntry { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000050DB File Offset: 0x000032DB
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000050E3 File Offset: 0x000032E3
		public string Name { get; private set; }

		// Token: 0x06000105 RID: 261 RVA: 0x000050EC File Offset: 0x000032EC
		public CustomServerAction(Action execute, GameServerEntry gameServerEntry, string name)
		{
			this.Execute = execute;
			this.GameServerEntry = gameServerEntry;
			this.Name = name;
		}
	}
}
