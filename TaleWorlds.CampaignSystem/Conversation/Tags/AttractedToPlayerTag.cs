using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000267 RID: 615
	public class AttractedToPlayerTag : ConversationTag
	{
		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x0009B491 File Offset: 0x00099691
		public override string StringId
		{
			get
			{
				return "AttractedToPlayerTag";
			}
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0009B498 File Offset: 0x00099698
		public override bool IsApplicableTo(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			return heroObject != null && Hero.MainHero.IsFemale != heroObject.IsFemale && !FactionManager.IsAtWarAgainstFaction(heroObject.MapFaction, Hero.MainHero.MapFaction) && Campaign.Current.Models.RomanceModel.GetAttractionValuePercentage(heroObject, Hero.MainHero) > 70 && heroObject.Spouse == null && Hero.MainHero.Spouse == null;
		}

		// Token: 0x04000A9D RID: 2717
		public const string Id = "AttractedToPlayerTag";

		// Token: 0x04000A9E RID: 2718
		private const int MinimumFlirtPercentageForComment = 70;
	}
}
