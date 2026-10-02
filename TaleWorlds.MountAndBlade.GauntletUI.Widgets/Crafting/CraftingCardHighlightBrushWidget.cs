using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000164 RID: 356
	public class CraftingCardHighlightBrushWidget : BrushWidget
	{
		// Token: 0x060012D7 RID: 4823 RVA: 0x000339F8 File Offset: 0x00031BF8
		public CraftingCardHighlightBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00033A08 File Offset: 0x00031C08
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			if (this._firstFrame && base.IsVisible)
			{
				this._firstFrame = false;
				return;
			}
			if (!this._playingAnimation && !this._firstFrame)
			{
				base.BrushRenderer.RestartAnimation();
				this._playingAnimation = true;
			}
		}

		// Token: 0x0400088D RID: 2189
		private bool _playingAnimation;

		// Token: 0x0400088E RID: 2190
		private bool _firstFrame = true;
	}
}
