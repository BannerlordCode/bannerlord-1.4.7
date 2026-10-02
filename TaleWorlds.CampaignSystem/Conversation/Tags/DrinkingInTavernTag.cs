using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000242 RID: 578
	public class DrinkingInTavernTag : ConversationTag
	{
		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x0009AB1E File Offset: 0x00098D1E
		public override string StringId
		{
			get
			{
				return "DrinkingInTavernTag";
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0009AB28 File Offset: 0x00098D28
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (LocationComplex.Current != null && character.IsHero)
			{
				Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(character.HeroObject);
				Location locationWithId = LocationComplex.Current.GetLocationWithId("tavern");
				if (character.HeroObject.IsWanderer && Settlement.CurrentSettlement != null && locationWithId == locationOfCharacter)
				{
					return true;
				}
			}
			else if (character.HeroObject == null && LocationComplex.Current != null && Settlement.CurrentSettlement != null && LocationComplex.Current.GetLocationWithId("tavern") == CampaignMission.Current.Location)
			{
				return true;
			}
			return false;
		}

		// Token: 0x04000A78 RID: 2680
		public const string Id = "DrinkingInTavernTag";
	}
}
