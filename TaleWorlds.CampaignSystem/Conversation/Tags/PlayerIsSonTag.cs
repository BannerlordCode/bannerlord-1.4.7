using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024C RID: 588
	public class PlayerIsSonTag : ConversationTag
	{
		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x0009AD68 File Offset: 0x00098F68
		public override string StringId
		{
			get
			{
				return "PlayerIsSonTag";
			}
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x0009AD6F File Offset: 0x00098F6F
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && !Hero.MainHero.IsFemale && (Hero.MainHero.Father == character.HeroObject || Hero.MainHero.Mother == character.HeroObject);
		}

		// Token: 0x04000A82 RID: 2690
		public const string Id = "PlayerIsSonTag";
	}
}
