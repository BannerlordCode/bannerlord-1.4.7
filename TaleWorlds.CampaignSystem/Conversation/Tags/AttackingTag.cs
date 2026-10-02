using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026A RID: 618
	public class AttackingTag : ConversationTag
	{
		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x0009B571 File Offset: 0x00099771
		public override string StringId
		{
			get
			{
				return "AttackingTag";
			}
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0009B578 File Offset: 0x00099778
		public override bool IsApplicableTo(CharacterObject character)
		{
			return HeroHelper.WillLordAttack() || (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.SiegeEvent != null && Settlement.CurrentSettlement.Parties.Contains(Hero.MainHero.PartyBelongedTo));
		}

		// Token: 0x04000AA1 RID: 2721
		public const string Id = "AttackingTag";
	}
}
