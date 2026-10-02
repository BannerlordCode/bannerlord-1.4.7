using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C1 RID: 1217
	public class RaftStateChangeAction
	{
		// Token: 0x06004AD5 RID: 19157 RVA: 0x0017AD68 File Offset: 0x00178F68
		private static void ApplyInternal(MobileParty mobileParty, bool isRaftState)
		{
			mobileParty.IsInRaftState = isRaftState;
			if (mobileParty.Army != null)
			{
				mobileParty.Army = null;
			}
			if (isRaftState)
			{
				mobileParty.MovePartyToTheClosestLand();
				mobileParty.Ai.DisableAi();
				if (mobileParty.Party.PrisonRoster.TotalManCount > 0)
				{
					if (mobileParty.Party.PrisonRoster.TotalHeroes > 0)
					{
						foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero)
							{
								EndCaptivityAction.ApplyByEscape(troopRosterElement.Character.HeroObject, null, true);
							}
						}
					}
					mobileParty.PrisonRoster.Clear();
				}
			}
			else
			{
				mobileParty.Ai.EnableAi();
				mobileParty.RecalculateShortTermBehavior();
				mobileParty.Ai.DefaultBehaviorNeedsUpdate = true;
				mobileParty.Ai.RethinkAtNextHourlyTick = true;
			}
			CampaignEventDispatcher.Instance.OnMobilePartyRaftStateChanged(mobileParty);
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0017AE70 File Offset: 0x00179070
		public static void ActivateRaftStateForParty(MobileParty mobileParty)
		{
			RaftStateChangeAction.ApplyInternal(mobileParty, true);
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x0017AE79 File Offset: 0x00179079
		public static void DeactivateRaftStateForParty(MobileParty mobileParty)
		{
			RaftStateChangeAction.ApplyInternal(mobileParty, false);
		}
	}
}
