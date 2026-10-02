using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C6 RID: 454
	public abstract class SettlementMenuOverlayModel : MBGameModel<SettlementMenuOverlayModel>
	{
		// Token: 0x06001E04 RID: 7684
		public abstract Dictionary<Hero, bool> GetOverlayHeroes();
	}
}
