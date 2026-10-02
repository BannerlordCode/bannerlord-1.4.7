using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace Helpers
{
	// Token: 0x02000027 RID: 39
	public static class ShipHelper
	{
		// Token: 0x06000178 RID: 376 RVA: 0x000110E0 File Offset: 0x0000F2E0
		public static Banner GetShipBanner(IShipOrigin shipOrigin, IAgent captain = null)
		{
			CharacterObject characterObject;
			if ((characterObject = ((captain != null) ? captain.Character : null) as CharacterObject) != null && characterObject.IsHero)
			{
				return characterObject.HeroObject.ClanBanner;
			}
			Ship ship;
			if ((ship = shipOrigin as Ship) == null || ship.Owner == null)
			{
				return Banner.CreateOneColoredEmptyBanner(92);
			}
			if (ship.Owner.IsMobile && ship.Owner.MobileParty.Army != null)
			{
				return ship.Owner.MobileParty.Army.LeaderParty.MapFaction.Banner;
			}
			return ship.Owner.Banner;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0001117C File Offset: 0x0000F37C
		[return: TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
		public static ValueTuple<uint, uint> GetSailColors(IShipOrigin shipOrigin, IAgent captain = null)
		{
			ValueTuple<uint, uint> valueTuple = new ValueTuple<uint, uint>(4291609515U, 4291609515U);
			CharacterObject characterObject;
			Ship ship;
			if ((characterObject = ((captain != null) ? captain.Character : null) as CharacterObject) != null && characterObject.IsHero)
			{
				valueTuple.Item1 = characterObject.HeroObject.MapFaction.Color;
				valueTuple.Item2 = characterObject.HeroObject.MapFaction.Color2;
			}
			else if ((ship = shipOrigin as Ship) != null && ship.Owner != null)
			{
				if (ship.Owner.IsMobile && ship.Owner.MobileParty.Army != null)
				{
					valueTuple.Item1 = ship.Owner.MobileParty.Army.LeaderParty.MapFaction.Color;
					valueTuple.Item2 = ship.Owner.MobileParty.Army.LeaderParty.MapFaction.Color2;
				}
				else
				{
					valueTuple.Item1 = ship.Owner.MapFaction.Color;
					valueTuple.Item2 = ship.Owner.MapFaction.Color2;
				}
			}
			return valueTuple;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000112A0 File Offset: 0x0000F4A0
		public static Banner GetShipBanner(PartyBase party = null)
		{
			if (party == null)
			{
				return Banner.CreateOneColoredEmptyBanner(92);
			}
			if (party.IsMobile && party.MobileParty.Army != null)
			{
				return party.MobileParty.Army.LeaderParty.MapFaction.Banner;
			}
			return party.Banner;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000112F0 File Offset: 0x0000F4F0
		[return: TupleElementNames(new string[] { "sailColor1", "sailColor2" })]
		public static ValueTuple<uint, uint> GetSailColors(PartyBase party = null)
		{
			ValueTuple<uint, uint> valueTuple = new ValueTuple<uint, uint>(4291609515U, 4291609515U);
			if (party != null)
			{
				if (party.IsMobile && party.MobileParty.Army != null)
				{
					valueTuple.Item1 = party.MobileParty.Army.LeaderParty.MapFaction.Color;
					valueTuple.Item2 = party.MobileParty.Army.LeaderParty.MapFaction.Color2;
				}
				else
				{
					valueTuple.Item1 = party.Owner.MapFaction.Color;
					valueTuple.Item2 = party.Owner.MapFaction.Color2;
				}
			}
			return valueTuple;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0001139C File Offset: 0x0000F59C
		public static List<Ship> GetOrderedNavalRaidShipsOfPlayerParty()
		{
			List<Ship> list = new List<Ship>();
			foreach (Ship ship in MobileParty.MainParty.Ships)
			{
				if (ship.ShipHull.CanNavigateShallowWater)
				{
					list.Add(ship);
				}
			}
			return list.OrderByDescending<Ship, int>((Ship x) => x.ShipHull.MainDeckCrewCapacity).Take<Ship>(3).ToList<Ship>();
		}

		// Token: 0x04000007 RID: 7
		public const int NavalRaidMissionShipLimit = 3;
	}
}
