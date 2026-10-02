using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000137 RID: 311
	public class DefaultPartyShipLimitModel : PartyShipLimitModel
	{
		// Token: 0x06001947 RID: 6471 RVA: 0x0007D83E File Offset: 0x0007BA3E
		public override int GetIdealShipNumber(MobileParty mobileParty)
		{
			return 0;
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0007D841 File Offset: 0x0007BA41
		public override int GetIdealShipNumber(Clan clan)
		{
			return 0;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0007D844 File Offset: 0x0007BA44
		public override float GetShipPriority(MobileParty mobileParty, Ship ship, bool isSelling)
		{
			return 0f;
		}
	}
}
