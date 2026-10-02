using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000241 RID: 577
	public class WaryTag : ConversationTag
	{
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x0009AA9F File Offset: 0x00098C9F
		public override string StringId
		{
			get
			{
				return "WaryTag";
			}
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0009AAA8 File Offset: 0x00098CA8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.MapFaction != Hero.MainHero.MapFaction && (Settlement.CurrentSettlement == null || Settlement.CurrentSettlement.SiegeEvent != null) && (Campaign.Current.ConversationManager.CurrentConversationIsFirst || FactionManager.IsAtWarAgainstFaction(character.HeroObject.MapFaction, Hero.MainHero.MapFaction));
		}

		// Token: 0x04000A77 RID: 2679
		public const string Id = "WaryTag";
	}
}
