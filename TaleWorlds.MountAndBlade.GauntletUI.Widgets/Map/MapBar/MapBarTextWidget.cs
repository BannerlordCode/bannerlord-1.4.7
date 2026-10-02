using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012A RID: 298
	public class MapBarTextWidget : TextWidget
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x0002B150 File Offset: 0x00029350
		public MapBarTextWidget(UIContext context)
			: base(context)
		{
			base.intPropertyChanged += this.TextPropertyChanged;
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0002B17C File Offset: 0x0002937C
		private void TextPropertyChanged(PropertyOwnerObject widget, string propertyName, int propertyValue)
		{
			if (propertyName == "IntText")
			{
				if (this._prevValue != -99)
				{
					if (propertyValue - this._prevValue > 0)
					{
						if (base.CurrentState == "Positive")
						{
							base.BrushRenderer.RestartAnimation();
						}
						else
						{
							this.SetState("Positive");
						}
					}
					else if (propertyValue - this._prevValue < 0)
					{
						if (base.CurrentState == "Negative")
						{
							base.BrushRenderer.RestartAnimation();
						}
						else
						{
							this.SetState("Negative");
						}
					}
				}
				this._prevValue = propertyValue;
			}
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002B218 File Offset: 0x00029418
		private void RefreshFontColor()
		{
			Color color;
			if (this.IsWarning)
			{
				color = this.WarningColor;
			}
			else
			{
				color = this.NormalColor;
			}
			foreach (Style style in base.Brush.Styles)
			{
				style.FontColor = color;
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0002B288 File Offset: 0x00029488
		// (set) Token: 0x06000FA1 RID: 4001 RVA: 0x0002B290 File Offset: 0x00029490
		[Editor(false)]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChanged(value, "IsWarning");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0002B2B4 File Offset: 0x000294B4
		// (set) Token: 0x06000FA3 RID: 4003 RVA: 0x0002B2BC File Offset: 0x000294BC
		[Editor(false)]
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					base.OnPropertyChanged(value, "NormalColor");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0002B2E5 File Offset: 0x000294E5
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x0002B2ED File Offset: 0x000294ED
		[Editor(false)]
		public Color WarningColor
		{
			get
			{
				return this._warningColor;
			}
			set
			{
				if (value != this._warningColor)
				{
					this._warningColor = value;
					base.OnPropertyChanged(value, "WarningColor");
					this.RefreshFontColor();
				}
			}
		}

		// Token: 0x04000718 RID: 1816
		private int _prevValue = -99;

		// Token: 0x04000719 RID: 1817
		private bool _isWarning;

		// Token: 0x0400071A RID: 1818
		private Color _normalColor;

		// Token: 0x0400071B RID: 1819
		private Color _warningColor;
	}
}
