using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000009 RID: 9
	public class BrightnessDemoWidget : TextureWidget
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002879 File Offset: 0x00000A79
		public BrightnessDemoWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "BrightnessDemoTextureProvider";
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002894 File Offset: 0x00000A94
		// (set) Token: 0x0600003E RID: 62 RVA: 0x0000289C File Offset: 0x00000A9C
		[Editor(false)]
		public BrightnessDemoWidget.DemoTypes DemoType
		{
			get
			{
				return this._demoType;
			}
			set
			{
				if (this._demoType != value)
				{
					this._demoType = value;
					base.OnPropertyChanged<string>(Enum.GetName(typeof(BrightnessDemoWidget.DemoTypes), value), "DemoType");
					base.SetTextureProviderProperty("DemoType", (int)value);
				}
			}
		}

		// Token: 0x04000016 RID: 22
		private BrightnessDemoWidget.DemoTypes _demoType = BrightnessDemoWidget.DemoTypes.None;

		// Token: 0x02000194 RID: 404
		public enum DemoTypes
		{
			// Token: 0x0400097F RID: 2431
			None = -1,
			// Token: 0x04000980 RID: 2432
			BrightnessWide,
			// Token: 0x04000981 RID: 2433
			ExposureTexture1,
			// Token: 0x04000982 RID: 2434
			ExposureTexture2,
			// Token: 0x04000983 RID: 2435
			ExposureTexture3,
			// Token: 0x04000984 RID: 2436
			ExposureTexture4,
			// Token: 0x04000985 RID: 2437
			ExposureTexture5,
			// Token: 0x04000986 RID: 2438
			ExposureTexture6
		}
	}
}
