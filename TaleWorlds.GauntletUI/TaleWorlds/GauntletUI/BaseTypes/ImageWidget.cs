using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x0200005B RID: 91
	public class ImageWidget : BrushWidget
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001AC4F File Offset: 0x00018E4F
		// (set) Token: 0x0600063A RID: 1594 RVA: 0x0001AC57 File Offset: 0x00018E57
		public bool OverrideDefaultStateSwitchingEnabled { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x0001AC60 File Offset: 0x00018E60
		// (set) Token: 0x0600063C RID: 1596 RVA: 0x0001AC6B File Offset: 0x00018E6B
		public bool OverrideDefaultStateSwitchingDisabled
		{
			get
			{
				return !this.OverrideDefaultStateSwitchingEnabled;
			}
			set
			{
				if (value != !this.OverrideDefaultStateSwitchingEnabled)
				{
					this.OverrideDefaultStateSwitchingEnabled = !value;
				}
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001AC83 File Offset: 0x00018E83
		public ImageWidget(UIContext context)
			: base(context)
		{
			base.AddState("Pressed");
			base.AddState("Hovered");
			base.AddState("Disabled");
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001ACB0 File Offset: 0x00018EB0
		protected override void RefreshState()
		{
			if (!this.OverrideDefaultStateSwitchingEnabled)
			{
				if (base.IsDisabled)
				{
					this.SetState("Disabled");
				}
				else if (base.IsPressed)
				{
					this.SetState("Pressed");
				}
				else if (base.IsHovered)
				{
					this.SetState("Hovered");
				}
				else
				{
					this.SetState("Default");
				}
			}
			base.RefreshState();
		}
	}
}
