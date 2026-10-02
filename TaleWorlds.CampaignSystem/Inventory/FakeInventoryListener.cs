using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D7 RID: 215
	public class FakeInventoryListener : InventoryListener
	{
		// Token: 0x06001483 RID: 5251 RVA: 0x0005F113 File Offset: 0x0005D313
		public override int GetGold()
		{
			return 0;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0005F116 File Offset: 0x0005D316
		public override TextObject GetTraderName()
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0005F11D File Offset: 0x0005D31D
		public override void SetGold(int gold)
		{
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0005F11F File Offset: 0x0005D31F
		public override void OnTransaction()
		{
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0005F121 File Offset: 0x0005D321
		public override PartyBase GetOppositeParty()
		{
			return null;
		}
	}
}
