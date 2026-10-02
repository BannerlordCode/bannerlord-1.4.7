using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Credits
{
	// Token: 0x02000160 RID: 352
	public class CreditsTextWidget : RichTextWidget
	{
		// Token: 0x0600129D RID: 4765 RVA: 0x000332E2 File Offset: 0x000314E2
		public CreditsTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x000332EC File Offset: 0x000314EC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.overrideFont != null)
			{
				this._richText.StyleFontContainer.ClearFonts();
				foreach (Style style in base.ReadOnlyBrush.Styles)
				{
					this._richText.StyleFontContainer.Add(style.Name, this.overrideFont, (float)style.FontSize * base._scaleToUse);
				}
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00033388 File Offset: 0x00031588
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x00033390 File Offset: 0x00031590
		[Editor(false)]
		public string OverrideFont
		{
			get
			{
				return this._overrideFont;
			}
			set
			{
				if (this._overrideFont != value)
				{
					this._overrideFont = value;
					base.OnPropertyChanged<string>(value, "OverrideFont");
					this.overrideFont = base.Context.FontFactory.GetFont(this.OverrideFont);
				}
			}
		}

		// Token: 0x04000873 RID: 2163
		private Font overrideFont;

		// Token: 0x04000874 RID: 2164
		private string _overrideFont;
	}
}
