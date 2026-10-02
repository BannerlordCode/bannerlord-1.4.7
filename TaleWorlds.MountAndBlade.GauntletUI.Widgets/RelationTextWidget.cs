using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000039 RID: 57
	public class RelationTextWidget : TextWidget
	{
		// Token: 0x0600034F RID: 847 RVA: 0x0000AA05 File Offset: 0x00008C05
		public RelationTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000AA18 File Offset: 0x00008C18
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isVisualsDirty)
			{
				base.Text = ((this.Amount > 0) ? ("+" + this.Amount.ToString()) : this.Amount.ToString());
				if (this.Amount > 0)
				{
					base.Brush.FontColor = this.PositiveColor;
				}
				else if (this.Amount < 0)
				{
					base.Brush.FontColor = this.NegativeColor;
				}
				else
				{
					base.Brush.FontColor = this.ZeroColor;
				}
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000AABE File Offset: 0x00008CBE
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000AAC6 File Offset: 0x00008CC6
		[Editor(false)]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (this._amount != value)
				{
					this._amount = value;
					base.OnPropertyChanged(value, "Amount");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000AAEB File Offset: 0x00008CEB
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000AAF3 File Offset: 0x00008CF3
		[Editor(false)]
		public Color ZeroColor
		{
			get
			{
				return this._zeroColor;
			}
			set
			{
				if (value != this._zeroColor)
				{
					this._zeroColor = value;
					base.OnPropertyChanged(value, "ZeroColor");
				}
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000AB16 File Offset: 0x00008D16
		// (set) Token: 0x06000356 RID: 854 RVA: 0x0000AB1E File Offset: 0x00008D1E
		[Editor(false)]
		public Color PositiveColor
		{
			get
			{
				return this._positiveColor;
			}
			set
			{
				if (value != this._positiveColor)
				{
					this._positiveColor = value;
					base.OnPropertyChanged(value, "PositiveColor");
				}
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000AB41 File Offset: 0x00008D41
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000AB49 File Offset: 0x00008D49
		[Editor(false)]
		public Color NegativeColor
		{
			get
			{
				return this._negativeColor;
			}
			set
			{
				if (value != this._negativeColor)
				{
					this._negativeColor = value;
					base.OnPropertyChanged(value, "NegativeColor");
				}
			}
		}

		// Token: 0x04000156 RID: 342
		private bool _isVisualsDirty = true;

		// Token: 0x04000157 RID: 343
		private int _amount;

		// Token: 0x04000158 RID: 344
		private Color _zeroColor;

		// Token: 0x04000159 RID: 345
		private Color _positiveColor;

		// Token: 0x0400015A RID: 346
		private Color _negativeColor;
	}
}
