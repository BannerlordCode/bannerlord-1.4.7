using System;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000027 RID: 39
	public class OverrideView : Attribute
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000810C File Offset: 0x0000630C
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00008114 File Offset: 0x00006314
		public Type BaseType { get; private set; }

		// Token: 0x06000117 RID: 279 RVA: 0x0000811D File Offset: 0x0000631D
		public OverrideView(Type baseType)
		{
			this.BaseType = baseType;
		}
	}
}
