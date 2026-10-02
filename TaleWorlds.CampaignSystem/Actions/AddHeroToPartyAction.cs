using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000491 RID: 1169
	public static class AddHeroToPartyAction
	{
		// Token: 0x06004A09 RID: 18953 RVA: 0x00176488 File Offset: 0x00174688
		private static void ApplyInternal(Hero hero, MobileParty newParty, bool showNotification = true)
		{
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			if (partyBelongedTo != null)
			{
				partyBelongedTo.MemberRoster.AddToCounts(hero.CharacterObject, -1, false, 0, 0, true, -1);
			}
			hero.StayingInSettlement = null;
			bool isNotable = hero.IsNotable;
			if (hero.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
			newParty.AddElementToMemberRoster(hero.CharacterObject, 1, false);
			CampaignEventDispatcher.Instance.OnHeroJoinedParty(hero, newParty);
			if (showNotification && newParty == MobileParty.MainParty && hero.IsPlayerCompanion)
			{
				TextObject textObject = GameTexts.FindText("str_companion_added", null);
				StringHelpers.SetCharacterProperties("COMPANION", hero.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, null, "");
			}
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x0017652F File Offset: 0x0017472F
		public static void Apply(Hero hero, MobileParty party, bool showNotification = true)
		{
			AddHeroToPartyAction.ApplyInternal(hero, party, showNotification);
		}
	}
}
