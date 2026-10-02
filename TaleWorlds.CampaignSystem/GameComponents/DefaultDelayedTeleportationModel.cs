using System;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010E RID: 270
	public class DefaultDelayedTeleportationModel : DelayedTeleportationModel
	{
		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x0006E2C3 File Offset: 0x0006C4C3
		private float MaximumDistanceForDelayAsDays
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x0006E2CA File Offset: 0x0006C4CA
		public override float DefaultTeleportationSpeed
		{
			get
			{
				return 0.24f;
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0006E2D4 File Offset: 0x0006C4D4
		public override ExplainedNumber GetTeleportationDelayAsHours(Hero teleportingHero, PartyBase target)
		{
			float num = this.MaximumDistanceForDelayAsDays * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			float num2 = 0f;
			IMapPoint mapPoint = teleportingHero.GetMapPoint();
			if (mapPoint != null)
			{
				MobileParty.NavigationType navigationType = (teleportingHero.Clan.HasNavalNavigationCapability ? MobileParty.NavigationType.All : MobileParty.NavigationType.Default);
				if (target.IsSettlement)
				{
					if (teleportingHero.CurrentSettlement != null && teleportingHero.CurrentSettlement == target.Settlement)
					{
						num2 = 0f;
					}
					else
					{
						float num3;
						num2 = DistanceHelper.FindClosestDistanceFromMapPointToSettlement(mapPoint, target.Settlement, navigationType, out num3);
					}
				}
				else if (target.IsMobile)
				{
					Settlement settlement;
					MobileParty mobileParty;
					if ((settlement = mapPoint as Settlement) != null)
					{
						num2 = DistanceHelper.FindClosestDistanceFromMobilePartyToSettlement(target.MobileParty, settlement, navigationType);
					}
					else if ((mobileParty = mapPoint as MobileParty) != null)
					{
						float num4 = DistanceHelper.FindClosestDistanceFromMobilePartyToMobileParty(target.MobileParty, mobileParty, navigationType);
						if (num4 < num)
						{
							num2 = num4;
						}
					}
				}
			}
			num2 = MathF.Clamp(num2, 0f, num);
			return new ExplainedNumber(num2 * this.DefaultTeleportationSpeed, false, null);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0006E3BA File Offset: 0x0006C5BA
		public override bool CanPerformImmediateTeleport(Hero hero, MobileParty targetMobileParty, Settlement targetSettlement)
		{
			return (targetSettlement != null && !targetSettlement.IsUnderSiege && !targetSettlement.IsUnderRaid) || (targetMobileParty != null && targetMobileParty.MapEvent == null && !targetMobileParty.IsCurrentlyEngagingParty && (!targetMobileParty.IsCurrentlyAtSea || targetMobileParty.CurrentSettlement != null));
		}
	}
}
