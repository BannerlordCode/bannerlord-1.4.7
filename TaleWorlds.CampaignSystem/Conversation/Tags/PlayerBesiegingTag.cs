using System;
using System.Linq;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026C RID: 620
	public class PlayerBesiegingTag : ConversationTag
	{
		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0009B5D2 File Offset: 0x000997D2
		public override string StringId
		{
			get
			{
				return "PlayerBesiegingTag";
			}
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x0009B5DC File Offset: 0x000997DC
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.SiegeEvent != null)
			{
				return Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any<PartyBase>((PartyBase party) => party.MobileParty == Hero.MainHero.PartyBelongedTo);
			}
			return false;
		}

		// Token: 0x04000AA3 RID: 2723
		public const string Id = "PlayerBesiegingTag";
	}
}
