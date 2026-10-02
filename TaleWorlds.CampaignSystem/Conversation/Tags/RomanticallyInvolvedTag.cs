using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000266 RID: 614
	public class RomanticallyInvolvedTag : ConversationTag
	{
		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x0009B45B File Offset: 0x0009965B
		public override string StringId
		{
			get
			{
				return "RomanticallyInvolvedTag";
			}
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0009B462 File Offset: 0x00099662
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Romance.GetRomanticLevel(character.HeroObject, CharacterObject.PlayerCharacter.HeroObject) >= Romance.RomanceLevelEnum.CourtshipStarted;
		}

		// Token: 0x04000A9C RID: 2716
		public const string Id = "RomanticallyInvolvedTag";
	}
}
