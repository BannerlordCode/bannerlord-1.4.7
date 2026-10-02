using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A0 RID: 416
	public abstract class GenericXpModel : MBGameModel<GenericXpModel>
	{
		// Token: 0x06001CAD RID: 7341
		public abstract float GetXpMultiplier(Hero hero);
	}
}
