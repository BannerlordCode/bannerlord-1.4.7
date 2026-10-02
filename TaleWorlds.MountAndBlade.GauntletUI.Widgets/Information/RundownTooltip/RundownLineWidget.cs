using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Information.RundownTooltip
{
	// Token: 0x0200014A RID: 330
	public class RundownLineWidget : ListPanel
	{
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00030EF6 File Offset: 0x0002F0F6
		// (set) Token: 0x0600119C RID: 4508 RVA: 0x00030EFE File Offset: 0x0002F0FE
		public TextWidget NameTextWidget { get; set; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x0600119D RID: 4509 RVA: 0x00030F07 File Offset: 0x0002F107
		// (set) Token: 0x0600119E RID: 4510 RVA: 0x00030F0F File Offset: 0x0002F10F
		public TextWidget ValueTextWidget { get; set; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x00030F18 File Offset: 0x0002F118
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x00030F20 File Offset: 0x0002F120
		public float Value { get; set; }

		// Token: 0x060011A1 RID: 4513 RVA: 0x00030F29 File Offset: 0x0002F129
		public RundownLineWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00030F34 File Offset: 0x0002F134
		public void RefreshValueOffset(float columnWidth)
		{
			if (columnWidth >= 0f && this.NameTextWidget.Size.X > 1E-05f && this.ValueTextWidget.Size.X > 1E-05f)
			{
				this.ValueTextWidget.ScaledPositionXOffset = columnWidth - (this.NameTextWidget.Size.X + this.ValueTextWidget.Size.X + base.ScaledMarginLeft + base.ScaledMarginRight);
			}
		}
	}
}
