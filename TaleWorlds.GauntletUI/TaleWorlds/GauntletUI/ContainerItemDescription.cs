using System;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200001E RID: 30
	public class ContainerItemDescription
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000C0A5 File Offset: 0x0000A2A5
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000C0AD File Offset: 0x0000A2AD
		public string WidgetId { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000C0B6 File Offset: 0x0000A2B6
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000C0BE File Offset: 0x0000A2BE
		public int WidgetIndex { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000C0C7 File Offset: 0x0000A2C7
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000C0CF File Offset: 0x0000A2CF
		public float WidthStretchRatio { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000C0E0 File Offset: 0x0000A2E0
		public float HeightStretchRatio { get; set; }

		// Token: 0x06000256 RID: 598 RVA: 0x0000C0E9 File Offset: 0x0000A2E9
		public ContainerItemDescription()
		{
			this.WidgetId = "";
			this.WidgetIndex = -1;
			this.WidthStretchRatio = 1f;
			this.HeightStretchRatio = 1f;
		}
	}
}
