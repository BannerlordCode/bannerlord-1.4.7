using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020000FE RID: 254
	public class AgentController
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00016FAD File Offset: 0x000151AD
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00016FB5 File Offset: 0x000151B5
		public Agent Owner { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00016FBE File Offset: 0x000151BE
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00016FC6 File Offset: 0x000151C6
		public Mission Mission { get; set; }

		// Token: 0x06000C34 RID: 3124 RVA: 0x00016FCF File Offset: 0x000151CF
		public virtual void OnInitialize()
		{
		}
	}
}
