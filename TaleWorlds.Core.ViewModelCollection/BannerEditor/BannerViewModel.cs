using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.BannerEditor
{
	// Token: 0x0200002E RID: 46
	public class BannerViewModel : ViewModel
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00006064 File Offset: 0x00004264
		public Banner Banner { get; }

		// Token: 0x060001EC RID: 492 RVA: 0x0000606C File Offset: 0x0000426C
		public BannerViewModel(Banner banner)
		{
			this.Banner = banner;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000607B File Offset: 0x0000427B
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00006088 File Offset: 0x00004288
		[DataSourceProperty]
		public string BannerCode
		{
			get
			{
				return this.Banner.BannerCode;
			}
			set
			{
				if (value != this.Banner.BannerCode)
				{
					this.Banner.Deserialize(value);
					base.OnPropertyChangedWithValue<string>(value, "BannerCode");
				}
			}
		}
	}
}
