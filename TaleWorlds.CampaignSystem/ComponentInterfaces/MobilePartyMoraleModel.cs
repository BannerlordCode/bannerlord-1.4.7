using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D0 RID: 464
	public abstract class MobilePartyMoraleModel : MBGameModel<MobilePartyMoraleModel>
	{
		// Token: 0x06001E5F RID: 7775
		public abstract float CalculateMoraleChange(MobileParty party);

		// Token: 0x06001E60 RID: 7776
		public abstract TextObject GetMoraleTooltipText(MobileParty party);
	}
}
