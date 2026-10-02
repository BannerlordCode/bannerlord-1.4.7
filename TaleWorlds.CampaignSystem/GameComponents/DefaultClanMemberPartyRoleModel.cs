using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000104 RID: 260
	public class DefaultClanMemberPartyRoleModel : ClanMemberPartyRoleModel
	{
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x0006BEB3 File Offset: 0x0006A0B3
		public override int MaximumPartyRoleAssignmentCount
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0006BEB6 File Offset: 0x0006A0B6
		public override IEnumerable<PartyRole> GetAssignablePartyRoles()
		{
			yield return PartyRole.Quartermaster;
			yield return PartyRole.Scout;
			yield return PartyRole.Surgeon;
			yield return PartyRole.Engineer;
			yield break;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0006BEC0 File Offset: 0x0006A0C0
		public override SkillObject GetRelevantSkillForPartyRole(PartyRole role)
		{
			if (role == PartyRole.Engineer)
			{
				return DefaultSkills.Engineering;
			}
			if (role == PartyRole.Quartermaster)
			{
				return DefaultSkills.Steward;
			}
			if (role == PartyRole.Scout)
			{
				return DefaultSkills.Scouting;
			}
			if (role == PartyRole.Surgeon)
			{
				return DefaultSkills.Medicine;
			}
			Debug.FailedAssert(string.Format("Undefined clan role relevant skill {0}", role), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultClanMemberRoleModel.cs", "GetRelevantSkillForPartyRole", 43);
			return null;
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x0006BF19 File Offset: 0x0006A119
		public override bool IsHeroAssignableForPartyRole(Hero hero, PartyRole role, MobileParty party)
		{
			return Campaign.Current.Models.ClanMemberPartyRoleModel.DoesHeroHaveEnoughSkillForPartyRole(hero, role, party) && hero.CanBeGovernorOrHavePartyRole();
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x0006BF3C File Offset: 0x0006A13C
		public override bool DoesHeroHaveEnoughSkillForPartyRole(Hero hero, PartyRole role, MobileParty party)
		{
			if (party.GetHeroPartyRoles(hero).Contains(role))
			{
				return true;
			}
			if (role == PartyRole.Engineer || role == PartyRole.Quartermaster || role == PartyRole.Scout || role == PartyRole.Surgeon)
			{
				return Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRoleInParty(role, hero, party);
			}
			if (role == PartyRole.None)
			{
				return true;
			}
			Debug.FailedAssert(string.Format("Undefined clan role is asked if assignable {0}", role), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultClanMemberRoleModel.cs", "DoesHeroHaveEnoughSkillForPartyRole", 73);
			return false;
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x0006BFAB File Offset: 0x0006A1AB
		public override bool IsHeroAssignableForPartyRoleInParty(PartyRole role, Hero hero, MobileParty party)
		{
			return hero.PartyBelongedTo == party && hero != party.GetRoleHolder(role) && hero.GetSkillValue(Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(role)) >= 0;
		}
	}
}
