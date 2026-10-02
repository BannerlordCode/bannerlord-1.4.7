using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004A RID: 74
	public class TutorialDirectionArrowWidget : Widget
	{
		// Token: 0x06000415 RID: 1045 RVA: 0x0000CDEC File Offset: 0x0000AFEC
		public TutorialDirectionArrowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
		private void UpdateArrowState()
		{
			if (this.VerticalArrowWidget != null && this.HorizontalArrowWidget != null && !string.IsNullOrEmpty(this.ArrowState))
			{
				if (this.ArrowState == "Right" || this.ArrowState == "Left")
				{
					this.HorizontalArrowWidget.SetState(this._arrowState);
					this.VerticalArrowWidget.SetState("Default");
					return;
				}
				if (this.ArrowState == "Up" || this.ArrowState == "Down")
				{
					this.HorizontalArrowWidget.SetState("Default");
					this.VerticalArrowWidget.SetState(this._arrowState);
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000CEB6 File Offset: 0x0000B0B6
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0000CEBE File Offset: 0x0000B0BE
		[Editor(false)]
		public string ArrowState
		{
			get
			{
				return this._arrowState;
			}
			set
			{
				if (value != this._arrowState)
				{
					this._arrowState = value;
					base.OnPropertyChanged<string>(value, "ArrowState");
					this.UpdateArrowState();
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0000CEE7 File Offset: 0x0000B0E7
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x0000CEEF File Offset: 0x0000B0EF
		[Editor(false)]
		public BrushWidget HorizontalArrowWidget
		{
			get
			{
				return this._horizontalArrowWidget;
			}
			set
			{
				if (this._horizontalArrowWidget != value)
				{
					this._horizontalArrowWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "HorizontalArrowWidget");
					this.UpdateArrowState();
				}
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000CF13 File Offset: 0x0000B113
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0000CF1B File Offset: 0x0000B11B
		[Editor(false)]
		public BrushWidget VerticalArrowWidget
		{
			get
			{
				return this._verticalArrowWidget;
			}
			set
			{
				if (this._verticalArrowWidget != value)
				{
					this._verticalArrowWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "VerticalArrowWidget");
					this.UpdateArrowState();
				}
			}
		}

		// Token: 0x040001B6 RID: 438
		private string _arrowState;

		// Token: 0x040001B7 RID: 439
		private BrushWidget _horizontalArrowWidget;

		// Token: 0x040001B8 RID: 440
		private BrushWidget _verticalArrowWidget;
	}
}
