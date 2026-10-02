using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E6 RID: 230
	public class EmptyImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B8D RID: 2957 RVA: 0x00025614 File Offset: 0x00023814
		public EmptyImageIdentifier()
		{
			base.Id = string.Empty;
			base.AdditionalArgs = string.Empty;
			base.TextureProviderName = string.Empty;
		}
	}
}
