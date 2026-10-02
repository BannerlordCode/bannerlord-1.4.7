using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E3 RID: 227
	public class ReloadBarHeightAdjustmentWidget : Widget
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x000208FB File Offset: 0x0001EAFB
		public ReloadBarHeightAdjustmentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00020904 File Offset: 0x0001EB04
		private void Refresh()
		{
			if (this.FillWidget != null)
			{
				base.ScaledSuggestedHeight = 50f * this.RelativeDurationToMaxDuration * base._scaleToUse;
				this.FillWidget.ScaledSuggestedHeight = base.ScaledSuggestedHeight - (this.FillWidget.MarginBottom + this.FillWidget.MarginTop) * base._scaleToUse;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00020962 File Offset: 0x0001EB62
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0002096A File Offset: 0x0001EB6A
		public float RelativeDurationToMaxDuration
		{
			get
			{
				return this._relativeDurationToMaxDuration;
			}
			set
			{
				if (value != this._relativeDurationToMaxDuration)
				{
					this._relativeDurationToMaxDuration = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x00020982 File Offset: 0x0001EB82
		// (set) Token: 0x06000BBF RID: 3007 RVA: 0x0002098A File Offset: 0x0001EB8A
		public Widget FillWidget
		{
			get
			{
				return this._fillWidget;
			}
			set
			{
				if (value != this._fillWidget)
				{
					this._fillWidget = value;
					this.Refresh();
				}
			}
		}

		// Token: 0x0400054C RID: 1356
		private const float _baseHeight = 50f;

		// Token: 0x0400054D RID: 1357
		private float _relativeDurationToMaxDuration;

		// Token: 0x0400054E RID: 1358
		private Widget _fillWidget;
	}
}
