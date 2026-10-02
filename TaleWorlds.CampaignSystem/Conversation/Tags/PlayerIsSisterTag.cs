using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000250 RID: 592
	public class PlayerIsSisterTag : ConversationTag
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x0009AE4B File Offset: 0x0009904B
		public override string StringId
		{
			get
			{
				return "PlayerIsSisterTag";
			}
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0009AE52 File Offset: 0x00099052
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.IsFemale && character.IsHero && character.HeroObject.Siblings.Contains(Hero.MainHero);
		}

		// Token: 0x04000A86 RID: 2694
		public const string Id = "PlayerIsSisterTag";
	}
}
