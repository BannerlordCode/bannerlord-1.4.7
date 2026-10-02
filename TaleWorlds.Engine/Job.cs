using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000054 RID: 84
	public class Job
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00006E00 File Offset: 0x00005000
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00006E08 File Offset: 0x00005008
		public bool Finished { get; protected set; }

		// Token: 0x060008A1 RID: 2209 RVA: 0x00006E11 File Offset: 0x00005011
		public virtual void DoJob(float dt)
		{
		}
	}
}
