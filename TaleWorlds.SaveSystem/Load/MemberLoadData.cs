using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003D RID: 61
	internal abstract class MemberLoadData : VariableLoadData
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000BF8C File Offset: 0x0000A18C
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000BF94 File Offset: 0x0000A194
		public ObjectLoadData ObjectLoadData { get; private set; }

		// Token: 0x06000266 RID: 614 RVA: 0x0000BF9D File Offset: 0x0000A19D
		protected MemberLoadData(ObjectLoadData objectLoadData, IReader reader)
			: base(objectLoadData.Context, reader)
		{
			this.ObjectLoadData = objectLoadData;
		}
	}
}
