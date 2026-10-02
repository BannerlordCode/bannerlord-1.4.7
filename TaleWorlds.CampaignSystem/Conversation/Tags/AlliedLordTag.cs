using System;
using Helpers;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000248 RID: 584
	public class AlliedLordTag : ConversationTag
	{
		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x0009AC99 File Offset: 0x00098E99
		public override string StringId
		{
			get
			{
				return "PlayerIsAlliedTag";
			}
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0009ACA0 File Offset: 0x00098EA0
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && DiplomacyHelper.IsSameFactionAndNotEliminated(character.HeroObject.MapFaction, Hero.MainHero.MapFaction);
		}

		// Token: 0x04000A7E RID: 2686
		public const string Id = "PlayerIsAlliedTag";
	}
}
