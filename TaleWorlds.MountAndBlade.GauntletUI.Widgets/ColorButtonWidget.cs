using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200000F RID: 15
	public class ColorButtonWidget : ButtonWidget
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x00003B60 File Offset: 0x00001D60
		public ColorButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003B6C File Offset: 0x00001D6C
		private void ApplyStringColorToBrush(string color)
		{
			Color color2 = Color.ConvertStringToColor(color);
			foreach (Style style in base.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = color2;
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003BE0 File Offset: 0x00001DE0
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003BE8 File Offset: 0x00001DE8
		[Editor(false)]
		public string ColorToApply
		{
			get
			{
				return this._colorToApply;
			}
			set
			{
				if (this._colorToApply != value)
				{
					this._colorToApply = value;
					base.OnPropertyChanged<string>(value, "ColorToApply");
					if (!string.IsNullOrEmpty(value))
					{
						this.ApplyStringColorToBrush(value);
					}
				}
			}
		}

		// Token: 0x04000057 RID: 87
		private string _colorToApply;
	}
}
