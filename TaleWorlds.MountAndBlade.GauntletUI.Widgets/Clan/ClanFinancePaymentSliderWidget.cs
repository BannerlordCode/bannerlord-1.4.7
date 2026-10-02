using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000173 RID: 371
	public class ClanFinancePaymentSliderWidget : SliderWidget
	{
		// Token: 0x06001369 RID: 4969 RVA: 0x00034B52 File Offset: 0x00032D52
		public ClanFinancePaymentSliderWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00034B5C File Offset: 0x00032D5C
		protected override void OnLateUpdate(float dt)
		{
			this.CurrentRatioIndicatorWidget.ScaledPositionXOffset = Mathf.Clamp(base.Size.X * ((float)this.CurrentSize / (float)this.SizeLimit) - this.CurrentRatioIndicatorWidget.Size.X / 2f, 0f, base.Size.X);
			this.InitialFillWidget.ScaledPositionXOffset = this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse + this.CurrentRatioIndicatorWidget.Size.X / 2f;
			this.InitialFillWidget.ScaledSuggestedWidth = base.Size.X - this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse - this.CurrentRatioIndicatorWidget.Size.X / 2f;
			if (base.Handle.PositionXOffset > this.CurrentRatioIndicatorWidget.PositionXOffset)
			{
				this.NewIncreaseFillWidget.ScaledPositionXOffset = this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse + this.CurrentRatioIndicatorWidget.Size.X / 2f;
				this.NewIncreaseFillWidget.ScaledSuggestedWidth = Mathf.Clamp((base.Handle.PositionXOffset - this.CurrentRatioIndicatorWidget.PositionXOffset) * base._scaleToUse, 0f, base.Size.X);
				this.NewDecreaseFillWidget.ScaledSuggestedWidth = 0f;
			}
			else if (base.Handle.PositionXOffset < this.CurrentRatioIndicatorWidget.PositionXOffset)
			{
				this.NewDecreaseFillWidget.ScaledPositionXOffset = base.Handle.PositionXOffset * base._scaleToUse + base.Handle.Size.X / 2f;
				this.NewDecreaseFillWidget.ScaledSuggestedWidth = Mathf.Clamp(this.CurrentRatioIndicatorWidget.PositionXOffset * base._scaleToUse + this.CurrentRatioIndicatorWidget.Size.X / 2f - (base.Handle.PositionXOffset * base._scaleToUse + base.Handle.Size.X / 2f), 0f, base.Size.X);
				this.NewIncreaseFillWidget.ScaledSuggestedWidth = 0f;
			}
			else
			{
				this.NewIncreaseFillWidget.ScaledSuggestedWidth = 0f;
				this.NewDecreaseFillWidget.ScaledSuggestedWidth = 0f;
			}
			base.OnLateUpdate(dt);
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x0600136B RID: 4971 RVA: 0x00034DCC File Offset: 0x00032FCC
		// (set) Token: 0x0600136C RID: 4972 RVA: 0x00034DD4 File Offset: 0x00032FD4
		[Editor(false)]
		public Widget InitialFillWidget
		{
			get
			{
				return this._initialFillWidget;
			}
			set
			{
				if (this._initialFillWidget != value)
				{
					this._initialFillWidget = value;
				}
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00034DE6 File Offset: 0x00032FE6
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x00034DEE File Offset: 0x00032FEE
		[Editor(false)]
		public Widget NewIncreaseFillWidget
		{
			get
			{
				return this._newIncreaseFillWidget;
			}
			set
			{
				if (this._newIncreaseFillWidget != value)
				{
					this._newIncreaseFillWidget = value;
				}
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00034E00 File Offset: 0x00033000
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x00034E08 File Offset: 0x00033008
		[Editor(false)]
		public Widget NewDecreaseFillWidget
		{
			get
			{
				return this._newDecreaseFillWidget;
			}
			set
			{
				if (this._newDecreaseFillWidget != value)
				{
					this._newDecreaseFillWidget = value;
				}
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00034E1A File Offset: 0x0003301A
		// (set) Token: 0x06001372 RID: 4978 RVA: 0x00034E22 File Offset: 0x00033022
		[Editor(false)]
		public Widget CurrentRatioIndicatorWidget
		{
			get
			{
				return this._currentRatioIndicatorWidget;
			}
			set
			{
				if (this._currentRatioIndicatorWidget != value)
				{
					this._currentRatioIndicatorWidget = value;
				}
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00034E34 File Offset: 0x00033034
		// (set) Token: 0x06001374 RID: 4980 RVA: 0x00034E3C File Offset: 0x0003303C
		[Editor(false)]
		public int CurrentSize
		{
			get
			{
				return this._currentSize;
			}
			set
			{
				if (this._currentSize != value)
				{
					this._currentSize = value;
				}
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00034E4E File Offset: 0x0003304E
		// (set) Token: 0x06001376 RID: 4982 RVA: 0x00034E56 File Offset: 0x00033056
		[Editor(false)]
		public int TargetSize
		{
			get
			{
				return this._targetSize;
			}
			set
			{
				if (this._targetSize != value)
				{
					this._targetSize = value;
				}
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00034E68 File Offset: 0x00033068
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x00034E70 File Offset: 0x00033070
		[Editor(false)]
		public int SizeLimit
		{
			get
			{
				return this._sizeLimit;
			}
			set
			{
				if (this._sizeLimit != value)
				{
					this._sizeLimit = value;
				}
			}
		}

		// Token: 0x040008CD RID: 2253
		private Widget _initialFillWidget;

		// Token: 0x040008CE RID: 2254
		private Widget _newIncreaseFillWidget;

		// Token: 0x040008CF RID: 2255
		private Widget _newDecreaseFillWidget;

		// Token: 0x040008D0 RID: 2256
		private Widget _currentRatioIndicatorWidget;

		// Token: 0x040008D1 RID: 2257
		private int _currentSize;

		// Token: 0x040008D2 RID: 2258
		private int _targetSize;

		// Token: 0x040008D3 RID: 2259
		private int _sizeLimit;
	}
}
