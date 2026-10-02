using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200002C RID: 44
	public class EngineMethod : Attribute
	{
		// Token: 0x06000161 RID: 353 RVA: 0x00005E05 File Offset: 0x00004005
		public EngineMethod(string engineMethodName, bool activateTelemetryProfiling = false, string[] conditionals = null, bool isMonoInline = false)
		{
			this.EngineMethodName = engineMethodName;
			this.ActivateTelemetryProfiling = activateTelemetryProfiling;
			this.Conditionals = conditionals;
			this.IsMonoInline = isMonoInline;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005E2A File Offset: 0x0000402A
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00005E32 File Offset: 0x00004032
		public string EngineMethodName { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005E3B File Offset: 0x0000403B
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00005E43 File Offset: 0x00004043
		public bool ActivateTelemetryProfiling { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00005E4C File Offset: 0x0000404C
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00005E54 File Offset: 0x00004054
		public string[] Conditionals { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00005E5D File Offset: 0x0000405D
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00005E65 File Offset: 0x00004065
		public bool IsMonoInline { get; private set; }
	}
}
