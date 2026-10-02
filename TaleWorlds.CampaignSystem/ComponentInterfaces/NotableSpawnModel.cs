using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B4 RID: 436
	public abstract class NotableSpawnModel : MBGameModel<NotableSpawnModel>
	{
		// Token: 0x06001D75 RID: 7541
		public abstract int GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation);
	}
}
