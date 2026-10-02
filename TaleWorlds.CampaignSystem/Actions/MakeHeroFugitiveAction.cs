using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004BC RID: 1212
	public static class MakeHeroFugitiveAction
	{
		// Token: 0x06004AC8 RID: 19144 RVA: 0x0017A7D8 File Offset: 0x001789D8
		private static void ApplyInternal(Hero fugitive, bool showNotification)
		{
			if (fugitive.IsAlive)
			{
				if (fugitive.PartyBelongedTo != null)
				{
					if (fugitive.PartyBelongedTo.LeaderHero == fugitive)
					{
						DestroyPartyAction.Apply(null, fugitive.PartyBelongedTo);
					}
					else
					{
						fugitive.PartyBelongedTo.MemberRoster.RemoveTroop(fugitive.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
				}
				if (fugitive.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(fugitive);
				}
				fugitive.ChangeState(Hero.CharacterStates.Fugitive);
				CampaignEventDispatcher.Instance.OnCharacterBecameFugitive(fugitive, showNotification);
			}
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x0017A853 File Offset: 0x00178A53
		public static void Apply(Hero fugitive, bool showNotification = false)
		{
			MakeHeroFugitiveAction.ApplyInternal(fugitive, showNotification);
		}
	}
}
