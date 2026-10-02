using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000115 RID: 277
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class Feature : Attribute
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00007F4A File Offset: 0x0000614A
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00007F52 File Offset: 0x00006152
		public Features FeatureFlag { get; private set; }

		// Token: 0x06000619 RID: 1561 RVA: 0x00007F5B File Offset: 0x0000615B
		public Feature(Features flag)
		{
			this.FeatureFlag = flag;
		}
	}
}
