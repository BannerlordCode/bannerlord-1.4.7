using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027E RID: 638
	public class MercyTag : ConversationTag
	{
		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x0009B953 File Offset: 0x00099B53
		public override string StringId
		{
			get
			{
				return "MercyTag";
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0009B95A File Offset: 0x00099B5A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) > 0;
		}

		// Token: 0x04000AB5 RID: 2741
		public const string Id = "MercyTag";
	}
}
