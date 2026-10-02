using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000186 RID: 390
	public class SkillProgressFillBarWidget : FillBarWidget
	{
		// Token: 0x0600143A RID: 5178 RVA: 0x000371B0 File Offset: 0x000353B0
		public SkillProgressFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x000371BC File Offset: 0x000353BC
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (this.PercentageIndicatorWidget != null)
			{
				base.ScaledPositionXOffset = Mathf.Clamp((this.PercentageIndicatorWidget.ScaledPositionXOffset - base.Size.X / 2f) * base._scaleToUse, 0f, 600f * base._scaleToUse);
			}
			base.OnRender(twoDimensionContext, drawContext);
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00037219 File Offset: 0x00035419
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x00037221 File Offset: 0x00035421
		public Widget PercentageIndicatorWidget
		{
			get
			{
				return this._percentageIndicatorWidget;
			}
			set
			{
				if (this._percentageIndicatorWidget != value)
				{
					this._percentageIndicatorWidget = value;
					base.OnPropertyChanged<Widget>(value, "PercentageIndicatorWidget");
				}
			}
		}

		// Token: 0x0400092E RID: 2350
		private Widget _percentageIndicatorWidget;
	}
}
