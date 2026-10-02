using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000282 RID: 642
	public class HonorTag : ConversationTag
	{
		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x060023C3 RID: 9155 RVA: 0x0009BA0B File Offset: 0x00099C0B
		public override string StringId
		{
			get
			{
				return "HonorTag";
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x0009BA12 File Offset: 0x00099C12
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0;
		}

		// Token: 0x04000AB9 RID: 2745
		public const string Id = "HonorTag";
	}
}
