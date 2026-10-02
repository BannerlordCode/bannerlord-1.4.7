using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CA RID: 202
	public class ClassLoadoutTroopTupleCultureColorBrushWidget : BrushWidget
	{
		// Token: 0x06000A92 RID: 2706 RVA: 0x0001D9C7 File Offset: 0x0001BBC7
		public ClassLoadoutTroopTupleCultureColorBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001D9D0 File Offset: 0x0001BBD0
		private void UpdateColor()
		{
			foreach (Style style in base.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = this.CultureColor;
				}
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0001DA44 File Offset: 0x0001BC44
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0001DA4C File Offset: 0x0001BC4C
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChanged(value, "CultureColor");
					this.UpdateColor();
				}
			}
		}

		// Token: 0x040004D4 RID: 1236
		private Color _cultureColor;
	}
}
