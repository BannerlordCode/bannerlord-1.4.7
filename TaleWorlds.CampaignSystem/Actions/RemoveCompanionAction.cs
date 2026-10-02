using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C2 RID: 1218
	public static class RemoveCompanionAction
	{
		// Token: 0x06004AD9 RID: 19161 RVA: 0x0017AE8C File Offset: 0x0017908C
		private static void ApplyInternal(Clan clan, Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			MobileParty partyBelongedTo = companion.PartyBelongedTo;
			PartyBase partyBase = ((partyBelongedTo != null) ? partyBelongedTo.Party : null);
			companion.CompanionOf = null;
			if (partyBase != null && partyBase.IsMobile && detail != RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord)
			{
				bool flag = partyBase.LeaderHero == companion;
				partyBase.MemberRoster.AddToCounts(companion.CharacterObject, -1, false, 0, 0, true, -1);
				if (flag)
				{
					partyBase.MobileParty.SetMoveModeHold();
					partyBase.MobileParty.Ai.RethinkAtNextHourlyTick = true;
					if (partyBase.MemberRoster.Count == 0)
					{
						DestroyPartyAction.Apply(null, partyBase.MobileParty);
					}
					else
					{
						DisbandPartyAction.StartDisband(partyBase.MobileParty);
					}
				}
			}
			if (detail == RemoveCompanionAction.RemoveCompanionDetail.Fire)
			{
				if (companion.PartyBelongedToAsPrisoner != null)
				{
					EndCaptivityAction.ApplyByEscape(companion, null, true);
				}
				else
				{
					MakeHeroFugitiveAction.Apply(companion, false);
				}
				if (companion.IsWanderer)
				{
					companion.ResetEquipments();
				}
			}
			if (companion.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(companion);
			}
			CampaignEventDispatcher.Instance.OnCompanionRemoved(companion, detail);
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x0017AF6B File Offset: 0x0017916B
		public static void ApplyByFire(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.Fire);
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x0017AF75 File Offset: 0x00179175
		public static void ApplyAfterQuest(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.AfterQuest);
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x0017AF7F File Offset: 0x0017917F
		public static void ApplyByDeath(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.Death);
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x0017AF89 File Offset: 0x00179189
		public static void ApplyByByTurningToLord(Clan clan, Hero companion)
		{
			RemoveCompanionAction.ApplyInternal(clan, companion, RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord);
		}

		// Token: 0x020008A0 RID: 2208
		public enum RemoveCompanionDetail
		{
			// Token: 0x040024DE RID: 9438
			Fire,
			// Token: 0x040024DF RID: 9439
			Death,
			// Token: 0x040024E0 RID: 9440
			AfterQuest,
			// Token: 0x040024E1 RID: 9441
			ByTurningToLord
		}
	}
}
