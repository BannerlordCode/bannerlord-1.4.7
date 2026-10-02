using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000276 RID: 630
	public class InHomeSettlementTag : ConversationTag
	{
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x0600239F RID: 9119 RVA: 0x0009B7CB File Offset: 0x000999CB
		public override string StringId
		{
			get
			{
				return "InHomeSettlementTag";
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0009B7D4 File Offset: 0x000999D4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return (character.IsHero && Settlement.CurrentSettlement != null && character.HeroObject.HomeSettlement == Settlement.CurrentSettlement) || (character.IsHero && Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.OwnerClan.Leader == character.HeroObject);
		}

		// Token: 0x04000AAD RID: 2733
		public const string Id = "InHomeSettlementTag";
	}
}
