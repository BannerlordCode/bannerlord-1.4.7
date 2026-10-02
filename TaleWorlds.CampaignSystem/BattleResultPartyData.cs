using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002B RID: 43
	public struct BattleResultPartyData
	{
		// Token: 0x060001DB RID: 475 RVA: 0x00013B2F File Offset: 0x00011D2F
		public BattleResultPartyData(PartyBase party)
		{
			this.Party = party;
			this.Characters = new List<CharacterObject>();
		}

		// Token: 0x0400001F RID: 31
		public readonly PartyBase Party;

		// Token: 0x04000020 RID: 32
		public readonly List<CharacterObject> Characters;
	}
}
