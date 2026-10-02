using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007E RID: 126
	[Flags]
	public enum CharacterRestrictionFlags : uint
	{
		// Token: 0x040004D9 RID: 1241
		None = 0U,
		// Token: 0x040004DA RID: 1242
		NotTransferableInPartyScreen = 1U,
		// Token: 0x040004DB RID: 1243
		CanNotGoInHideout = 2U
	}
}
