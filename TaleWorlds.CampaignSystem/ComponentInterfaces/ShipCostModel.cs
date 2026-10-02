using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000190 RID: 400
	public abstract class ShipCostModel : MBGameModel<ShipCostModel>
	{
		// Token: 0x06001C37 RID: 7223
		public abstract float GetShipTradeValue(Ship ship, PartyBase seller, PartyBase buyer);

		// Token: 0x06001C38 RID: 7224
		public abstract float GetShipRepairCost(Ship ship, PartyBase owner);

		// Token: 0x06001C39 RID: 7225
		public abstract int GetShipUpgradePieceCost(Ship ship, ShipUpgradePiece piece, PartyBase owner);

		// Token: 0x06001C3A RID: 7226
		public abstract float GetShipSellingPenalty();
	}
}
