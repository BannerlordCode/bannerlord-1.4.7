using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023E RID: 574
	public class NpcIsLiegeTag : ConversationTag
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x0009AA2A File Offset: 0x00098C2A
		public override string StringId
		{
			get
			{
				return "NpcIsLiegeTag";
			}
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0009AA31 File Offset: 0x00098C31
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsKingdomLeader;
		}

		// Token: 0x04000A74 RID: 2676
		public const string Id = "NpcIsLiegeTag";
	}
}
