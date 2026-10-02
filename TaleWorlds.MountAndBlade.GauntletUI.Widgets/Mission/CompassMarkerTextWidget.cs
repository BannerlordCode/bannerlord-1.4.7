using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D9 RID: 217
	public class CompassMarkerTextWidget : TextWidget
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x0001EF51 File Offset: 0x0001D151
		public CompassMarkerTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0001EF5A File Offset: 0x0001D15A
		private void UpdateBrush()
		{
			if (this.PrimaryBrush != null && this.SecondaryBrush != null)
			{
				base.Brush = (this.IsPrimary ? this.PrimaryBrush : this.SecondaryBrush);
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0001EF88 File Offset: 0x0001D188
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x0001EF90 File Offset: 0x0001D190
		public bool IsPrimary
		{
			get
			{
				return this._isPrimary;
			}
			set
			{
				if (this._isPrimary != value)
				{
					this._isPrimary = value;
					base.OnPropertyChanged(value, "IsPrimary");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x0001EFBC File Offset: 0x0001D1BC
		public float Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (Math.Abs(this._position - value) > 1E-45f)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0001EFE5 File Offset: 0x0001D1E5
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x0001EFED File Offset: 0x0001D1ED
		public Brush PrimaryBrush
		{
			get
			{
				return this._primaryBrush;
			}
			set
			{
				if (this._primaryBrush != value)
				{
					this._primaryBrush = value;
					base.OnPropertyChanged<Brush>(value, "PrimaryBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0001F011 File Offset: 0x0001D211
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0001F019 File Offset: 0x0001D219
		public Brush SecondaryBrush
		{
			get
			{
				return this._secondaryBrush;
			}
			set
			{
				if (this._secondaryBrush != value)
				{
					this._secondaryBrush = value;
					base.OnPropertyChanged<Brush>(value, "SecondaryBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x04000501 RID: 1281
		private bool _isPrimary;

		// Token: 0x04000502 RID: 1282
		private float _position;

		// Token: 0x04000503 RID: 1283
		private Brush _primaryBrush;

		// Token: 0x04000504 RID: 1284
		private Brush _secondaryBrush;
	}
}
