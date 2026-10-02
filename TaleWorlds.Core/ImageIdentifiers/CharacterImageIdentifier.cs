using System;

namespace TaleWorlds.Core.ImageIdentifiers
{
	// Token: 0x020000E4 RID: 228
	public class CharacterImageIdentifier : ImageIdentifier
	{
		// Token: 0x06000B8B RID: 2955 RVA: 0x0002559C File Offset: 0x0002379C
		public CharacterImageIdentifier(CharacterCode characterCode)
		{
			base.Id = ((characterCode != null) ? characterCode.Code : null) ?? "";
			base.AdditionalArgs = "";
			base.TextureProviderName = "CharacterImageTextureProvider";
		}
	}
}
