using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000323 RID: 803
	[AttributeUsage(AttributeTargets.Method)]
	public class ConsoleCommandMethod : Attribute
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002D97 RID: 11671 RVA: 0x000B007C File Offset: 0x000AE27C
		// (set) Token: 0x06002D98 RID: 11672 RVA: 0x000B0084 File Offset: 0x000AE284
		public string CommandName { get; private set; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002D99 RID: 11673 RVA: 0x000B008D File Offset: 0x000AE28D
		// (set) Token: 0x06002D9A RID: 11674 RVA: 0x000B0095 File Offset: 0x000AE295
		public string Description { get; private set; }

		// Token: 0x06002D9B RID: 11675 RVA: 0x000B009E File Offset: 0x000AE29E
		public ConsoleCommandMethod(string commandName, string description)
		{
			this.CommandName = commandName;
			this.Description = description;
		}
	}
}
