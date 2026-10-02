using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000202 RID: 514
	public abstract class ClanMemberPartyRoleModel : MBGameModel<ClanMemberPartyRoleModel>
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06001FBD RID: 8125
		public abstract int MaximumPartyRoleAssignmentCount { get; }

		// Token: 0x06001FBE RID: 8126
		public abstract IEnumerable<PartyRole> GetAssignablePartyRoles();

		// Token: 0x06001FBF RID: 8127
		public abstract SkillObject GetRelevantSkillForPartyRole(PartyRole role);

		// Token: 0x06001FC0 RID: 8128
		public abstract bool IsHeroAssignableForPartyRole(Hero hero, PartyRole role, MobileParty party);

		// Token: 0x06001FC1 RID: 8129
		public abstract bool DoesHeroHaveEnoughSkillForPartyRole(Hero hero, PartyRole role, MobileParty party);

		// Token: 0x06001FC2 RID: 8130
		public abstract bool IsHeroAssignableForPartyRoleInParty(PartyRole role, Hero hero, MobileParty party);
	}
}
