using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E3 RID: 227
	public class BannerImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B8A RID: 2954 RVA: 0x0002555E File Offset: 0x0002375E
		public BannerImageIdentifier(Banner banner, bool nineGrid = false)
		{
			base.Id = ((banner != null) ? banner.BannerCode : "");
			base.AdditionalArgs = (nineGrid ? "ninegrid" : "");
			base.TextureProviderName = "BannerImageTextureProvider";
		}
	}
}
