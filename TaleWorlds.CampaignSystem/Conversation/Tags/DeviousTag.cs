using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000283 RID: 643
	public class DeviousTag : ConversationTag
	{
		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x0009BA39 File Offset: 0x00099C39
		public override string StringId
		{
			get
			{
				return "DeviousTag";
			}
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0009BA40 File Offset: 0x00099C40
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) < 0;
		}

		// Token: 0x04000ABA RID: 2746
		public const string Id = "DeviousTag";
	}
}
