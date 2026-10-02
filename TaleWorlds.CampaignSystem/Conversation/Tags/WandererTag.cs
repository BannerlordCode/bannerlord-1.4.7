using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000275 RID: 629
	public class WandererTag : ConversationTag
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x0600239C RID: 9116 RVA: 0x0009B7A5 File Offset: 0x000999A5
		public override string StringId
		{
			get
			{
				return "WandererTag";
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0009B7AC File Offset: 0x000999AC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsWanderer;
		}

		// Token: 0x04000AAC RID: 2732
		public const string Id = "WandererTag";
	}
}
