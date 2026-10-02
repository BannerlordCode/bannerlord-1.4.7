using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x02000128 RID: 296
	public class MapBarCustomValueTextWidget : TextWidget
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x0002AEAD File Offset: 0x000290AD
		public MapBarCustomValueTextWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0002AEC0 File Offset: 0x000290C0
		private void RefreshTextAnimation(int valueDifference)
		{
			if (valueDifference <= 0)
			{
				if (valueDifference < 0)
				{
					if (base.CurrentState == "Negative")
					{
						base.BrushRenderer.RestartAnimation();
						return;
					}
					this.SetState("Negative");
				}
				return;
			}
			if (base.CurrentState == "Positive")
			{
				base.BrushRenderer.RestartAnimation();
				return;
			}
			this.SetState("Positive");
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002AF28 File Offset: 0x00029128
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

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0002AF98 File Offset: 0x00029198
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x0002AFA0 File Offset: 0x000291A0
		[Editor(false)]
		public int ValueAsInt
		{
			get
			{
				return this._valueAsInt;
			}
			set
			{
				if (value != this._valueAsInt)
				{
					this.RefreshTextAnimation(value - this._valueAsInt);
					this._valueAsInt = value;
					base.OnPropertyChanged(value, "ValueAsInt");
				}
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0002AFCC File Offset: 0x000291CC
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0002AFD4 File Offset: 0x000291D4
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

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0002AFF8 File Offset: 0x000291F8
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0002B000 File Offset: 0x00029200
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

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0002B029 File Offset: 0x00029229
		// (set) Token: 0x06000F92 RID: 3986 RVA: 0x0002B031 File Offset: 0x00029231
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

		// Token: 0x0400070F RID: 1807
		private bool _isWarning;

		// Token: 0x04000710 RID: 1808
		private Color _normalColor;

		// Token: 0x04000711 RID: 1809
		private Color _warningColor;

		// Token: 0x04000712 RID: 1810
		private int _valueAsInt;
	}
}
