using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C3 RID: 1219
	public static class RepairShipAction
	{
		// Token: 0x06004ADE RID: 19166 RVA: 0x0017AF93 File Offset: 0x00179193
		private static void ApplyInternal(Ship ship, float newHitpoints, Settlement repairPort = null)
		{
			SkillLevelingManager.OnShipRepaired(ship, newHitpoints - ship.HitPoints);
			ship.HitPoints = newHitpoints;
			CampaignEventDispatcher.Instance.OnShipRepaired(ship, repairPort);
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0017AFB8 File Offset: 0x001791B8
		public static void Apply(Ship ship, Settlement repairPort)
		{
			PartyBase owner = ship.Owner;
			if (owner.IsMobile && (owner.MobileParty.IsCaravan || owner.MobileParty.IsLordParty))
			{
				int num = (int)Campaign.Current.Models.ShipCostModel.GetShipRepairCost(ship, owner);
				GiveGoldAction.ApplyForPartyToSettlement(owner, repairPort, num, false);
			}
			RepairShipAction.ApplyInternal(ship, ship.MaxHitPoints, repairPort);
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x0017B01C File Offset: 0x0017921C
		public static void ApplyForFree(Ship ship)
		{
			RepairShipAction.ApplyInternal(ship, ship.MaxHitPoints, null);
		}

		// Token: 0x06004AE1 RID: 19169 RVA: 0x0017B02B File Offset: 0x0017922B
		public static void ApplyForBanditShip(Ship ship)
		{
			if (ship.HitPoints < ship.MaxHitPoints * 0.8f)
			{
				RepairShipAction.ApplyInternal(ship, ship.MaxHitPoints * 0.8f, null);
			}
		}
	}
}
