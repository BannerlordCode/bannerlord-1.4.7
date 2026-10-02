using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000026 RID: 38
	public class IconOffsetButtonWidget : IconBrushWidget
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x000075E5 File Offset: 0x000057E5
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x000075ED File Offset: 0x000057ED
		public int NormalXOffset { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001FA RID: 506 RVA: 0x000075F6 File Offset: 0x000057F6
		// (set) Token: 0x060001FB RID: 507 RVA: 0x000075FE File Offset: 0x000057FE
		public int NormalYOffset { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00007607 File Offset: 0x00005807
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000760F File Offset: 0x0000580F
		public int PressedXOffset { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00007618 File Offset: 0x00005818
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00007620 File Offset: 0x00005820
		public int PressedYOffset { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00007629 File Offset: 0x00005829
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00007631 File Offset: 0x00005831
		public Widget ButtonIcon { get; set; }

		// Token: 0x06000202 RID: 514 RVA: 0x0000763A File Offset: 0x0000583A
		public IconOffsetButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007644 File Offset: 0x00005844
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			Brush iconBrush = base.IconBrush;
			BrushLayer brushLayer = ((iconBrush != null) ? iconBrush.GetLayer(base.IconID) : null);
			if (((brushLayer != null) ? brushLayer.Sprite : null) != null)
			{
				base.SuggestedWidth = (float)brushLayer.Sprite.Width;
				base.SuggestedHeight = (float)brushLayer.Sprite.Height;
			}
			if (this.ButtonIcon != null)
			{
				if (base.IsPressed || base.IsSelected)
				{
					this.ButtonIcon.PositionYOffset = (float)this.PressedYOffset;
					this.ButtonIcon.PositionXOffset = (float)this.PressedXOffset;
					return;
				}
				this.ButtonIcon.PositionYOffset = (float)this.NormalYOffset;
				this.ButtonIcon.PositionXOffset = (float)this.NormalXOffset;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007704 File Offset: 0x00005904
		protected override void RefreshState()
		{
			if (base.IsSelected)
			{
				this.SetState("Selected");
				return;
			}
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			if (base.IsPressed)
			{
				this.SetState("Pressed");
				return;
			}
			if (base.IsHovered)
			{
				this.SetState("Hovered");
				return;
			}
			this.SetState("Default");
		}
	}
}
