using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000191 RID: 401
	public abstract class ShipStatModel : MBGameModel<ShipStatModel>
	{
		// Token: 0x06001C3C RID: 7228
		public abstract float GetShipFlagshipScore(Ship ship);
	}
}
