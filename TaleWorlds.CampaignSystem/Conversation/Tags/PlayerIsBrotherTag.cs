using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024F RID: 591
	public class PlayerIsBrotherTag : ConversationTag
	{
		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x0600232A RID: 9002 RVA: 0x0009AE0F File Offset: 0x0009900F
		public override string StringId
		{
			get
			{
				return "PlayerIsBrotherTag";
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x0009AE16 File Offset: 0x00099016
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Hero.MainHero.IsFemale && character.IsHero && character.HeroObject.Siblings.Contains(Hero.MainHero);
		}

		// Token: 0x04000A85 RID: 2693
		public const string Id = "PlayerIsBrotherTag";
	}
}
