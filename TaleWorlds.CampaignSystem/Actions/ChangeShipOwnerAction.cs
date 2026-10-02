using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004A3 RID: 1187
	public static class ChangeShipOwnerAction
	{
		// Token: 0x06004A4E RID: 19022 RVA: 0x00178398 File Offset: 0x00176598
		private static void ApplyInternal(PartyBase newOwner, Ship ship, ChangeShipOwnerAction.ShipOwnerChangeDetail changeDetail)
		{
			PartyBase owner = ship.Owner;
			if (changeDetail == ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTrade)
			{
				float shipTradeValue = Campaign.Current.Models.ShipCostModel.GetShipTradeValue(ship, owner, newOwner);
				if (owner.IsSettlement)
				{
					if (newOwner.MobileParty.IsCaravan || newOwner.MobileParty.IsVillager)
					{
						GiveGoldAction.ApplyForPartyToCharacter(newOwner, null, (int)shipTradeValue, false);
					}
					else
					{
						Clan actualClan = newOwner.MobileParty.ActualClan;
						if (((actualClan != null) ? actualClan.Leader : null) != null)
						{
							GiveGoldAction.ApplyBetweenCharacters(newOwner.MobileParty.ActualClan.Leader, null, (int)shipTradeValue, false);
						}
						else if (newOwner.MobileParty.LeaderHero != null)
						{
							GiveGoldAction.ApplyBetweenCharacters(newOwner.MobileParty.LeaderHero, null, (int)shipTradeValue, false);
						}
						else
						{
							Debug.FailedAssert("Unhandled case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\ChangeShipOwnerAction.cs", "ApplyInternal", 46);
							GiveGoldAction.ApplyForPartyToCharacter(newOwner, null, (int)shipTradeValue, false);
						}
					}
					if (newOwner.Ships.Any<Ship>() && !newOwner.MobileParty.Anchor.IsValid)
					{
						newOwner.MobileParty.Anchor.SetSettlement(ship.Owner.Settlement);
					}
				}
				else if (owner.MobileParty.IsCaravan || owner.MobileParty.IsVillager)
				{
					GiveGoldAction.ApplyForCharacterToParty(null, owner, (int)shipTradeValue, false);
				}
				else
				{
					Clan actualClan2 = owner.MobileParty.ActualClan;
					if (((actualClan2 != null) ? actualClan2.Leader : null) != null)
					{
						GiveGoldAction.ApplyBetweenCharacters(null, owner.MobileParty.ActualClan.Leader, (int)shipTradeValue, false);
					}
					else if (owner.LeaderHero != null)
					{
						GiveGoldAction.ApplyBetweenCharacters(null, owner.LeaderHero, (int)shipTradeValue, false);
					}
					else
					{
						Debug.FailedAssert("Unhandled case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\ChangeShipOwnerAction.cs", "ApplyInternal", 71);
						GiveGoldAction.ApplyForCharacterToParty(null, owner, (int)shipTradeValue, false);
					}
				}
			}
			ship.Owner = newOwner;
			if (owner != null)
			{
				MobileParty mobileParty = owner.MobileParty;
				if (mobileParty != null)
				{
					mobileParty.SetNavalVisualAsDirty();
				}
			}
			if (newOwner != null)
			{
				MobileParty mobileParty2 = newOwner.MobileParty;
				if (mobileParty2 != null)
				{
					mobileParty2.SetNavalVisualAsDirty();
				}
			}
			CampaignEventDispatcher.Instance.OnShipOwnerChanged(ship, owner, changeDetail);
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x00178586 File Offset: 0x00176786
		public static void ApplyByTransferring(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTransferring);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x00178590 File Offset: 0x00176790
		public static void ApplyByTrade(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByTrade);
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x0017859A File Offset: 0x0017679A
		public static void ApplyByLooting(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByLooting);
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x001785A4 File Offset: 0x001767A4
		public static void ApplyByProduction(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByProduction);
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x001785AE File Offset: 0x001767AE
		public static void ApplyByMobilePartyCreation(PartyBase newOwner, Ship ship)
		{
			ChangeShipOwnerAction.ApplyInternal(newOwner, ship, ChangeShipOwnerAction.ShipOwnerChangeDetail.ApplyByMobilePartyCreation);
		}

		// Token: 0x02000890 RID: 2192
		public enum ShipOwnerChangeDetail
		{
			// Token: 0x04002495 RID: 9365
			ApplyByTrade,
			// Token: 0x04002496 RID: 9366
			ApplyByTransferring,
			// Token: 0x04002497 RID: 9367
			ApplyByLooting,
			// Token: 0x04002498 RID: 9368
			ApplyByMobilePartyCreation,
			// Token: 0x04002499 RID: 9369
			ApplyByProduction
		}
	}
}
