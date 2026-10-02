using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018E RID: 398
	public abstract class PartyNavigationModel : MBGameModel<PartyNavigationModel>
	{
		// Token: 0x06001C2D RID: 7213
		public abstract bool CanPlayerNavigateToPosition(CampaignVec2 vec2, out MobileParty.NavigationType navigationType);

		// Token: 0x06001C2E RID: 7214
		public abstract float GetEmbarkDisembarkThresholdDistance();

		// Token: 0x06001C2F RID: 7215
		public abstract bool IsTerrainTypeValidForNavigationType(TerrainType terrainType, MobileParty.NavigationType navigationType);

		// Token: 0x06001C30 RID: 7216
		public abstract int[] GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType navigationType);

		// Token: 0x06001C31 RID: 7217
		public abstract bool HasNavalNavigationCapability(MobileParty mobileParty);
	}
}
