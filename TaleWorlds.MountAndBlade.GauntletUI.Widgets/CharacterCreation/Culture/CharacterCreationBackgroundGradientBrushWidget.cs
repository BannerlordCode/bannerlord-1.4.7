using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Culture
{
	// Token: 0x0200018A RID: 394
	public class CharacterCreationBackgroundGradientBrushWidget : BrushWidget
	{
		// Token: 0x0600146D RID: 5229 RVA: 0x000379AD File Offset: 0x00035BAD
		public CharacterCreationBackgroundGradientBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000379B8 File Offset: 0x00035BB8
		private void SetCultureBackground(Color cultureColor1)
		{
			foreach (Style style in base.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Color = cultureColor1;
				}
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x00037A28 File Offset: 0x00035C28
		// (set) Token: 0x06001470 RID: 5232 RVA: 0x00037A30 File Offset: 0x00035C30
		[Editor(false)]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (this._cultureColor1 != value)
				{
					this._cultureColor1 = value;
					base.OnPropertyChanged(value, "CultureColor1");
					this.SetCultureBackground(value);
				}
			}
		}

		// Token: 0x04000943 RID: 2371
		private Color _cultureColor1;
	}
}
