using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D6 RID: 214
	public abstract class InventoryListener
	{
		// Token: 0x0600147D RID: 5245
		public abstract int GetGold();

		// Token: 0x0600147E RID: 5246
		public abstract TextObject GetTraderName();

		// Token: 0x0600147F RID: 5247
		public abstract void SetGold(int gold);

		// Token: 0x06001480 RID: 5248
		public abstract PartyBase GetOppositeParty();

		// Token: 0x06001481 RID: 5249
		public abstract void OnTransaction();
	}
}
