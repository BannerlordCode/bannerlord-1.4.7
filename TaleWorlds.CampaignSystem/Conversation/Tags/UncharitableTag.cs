using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025E RID: 606
	public class UncharitableTag : ConversationTag
	{
		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x0009B240 File Offset: 0x00099440
		public override string StringId
		{
			get
			{
				return "UncharitableTag";
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0009B247 File Offset: 0x00099447
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Generosity) + character.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000A94 RID: 2708
		public const string Id = "UncharitableTag";
	}
}
