using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000037 RID: 55
	internal class ElementLoadData : VariableLoadData
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000B22B File Offset: 0x0000942B
		// (set) Token: 0x06000238 RID: 568 RVA: 0x0000B233 File Offset: 0x00009433
		public ContainerLoadData ContainerLoadData { get; private set; }

		// Token: 0x06000239 RID: 569 RVA: 0x0000B23C File Offset: 0x0000943C
		internal ElementLoadData(ContainerLoadData containerLoadData, IReader reader)
			: base(containerLoadData.Context, reader)
		{
			this.ContainerLoadData = containerLoadData;
		}
	}
}
