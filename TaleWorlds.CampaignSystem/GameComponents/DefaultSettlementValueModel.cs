using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000153 RID: 339
	public class DefaultSettlementValueModel : SettlementValueModel
	{
		// Token: 0x06001A6A RID: 6762 RVA: 0x00085BEC File Offset: 0x00083DEC
		private static float GetSettlementScoreForBeingHomeSettlementOfClan(Settlement settlement, Clan clan, float maxDistanceOfSettlementsToHomeSettlement)
		{
			float num = 0f;
			if (clan.IsRebelClan || clan.IsBanditFaction || clan.MapFaction.Settlements.Count == 0)
			{
				if (settlement == clan.InitialHomeSettlement)
				{
					num = float.MaxValue;
				}
				else
				{
					num = float.MinValue;
				}
			}
			else
			{
				if (settlement.SettlementComponent is Hideout || settlement.SettlementComponent is RetirementSettlementComponent)
				{
					return float.MinValue;
				}
				if (settlement.MapFaction.IsAtWarWith(clan.MapFaction))
				{
					num -= 10240f;
				}
				if (settlement.OwnerClan == clan)
				{
					num += 5120f;
				}
				if (settlement.MapFaction == clan.MapFaction)
				{
					num += 2560f;
				}
				if (settlement.IsVillage)
				{
					num += 320f;
				}
				else if (settlement.IsCastle)
				{
					num += 640f;
				}
				else if (settlement.IsTown)
				{
					num += 1280f;
				}
				if (settlement == clan.HomeSettlement)
				{
					num += 4.5f;
				}
				if (settlement == clan.InitialHomeSettlement)
				{
					num += 3.5f;
				}
				if (settlement.Culture == clan.Culture)
				{
					num += 17f;
				}
				Clan ownerClan = settlement.OwnerClan;
				if (((ownerClan != null) ? ownerClan.Culture : null) == clan.Culture)
				{
					num += 12f;
				}
				Settlement factionMidSettlement = clan.MapFaction.FactionMidSettlement;
				if (clan.MapFaction.Settlements.Count > 1 && settlement != factionMidSettlement)
				{
					float num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, false, false, MobileParty.NavigationType.All);
					if (settlement.HasPort)
					{
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, true, false, MobileParty.NavigationType.All);
						if (distance < num2)
						{
							num2 = distance;
						}
					}
					if (factionMidSettlement.HasPort)
					{
						float num3 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, false, true, MobileParty.NavigationType.All);
						if (num3 < num2)
						{
							num2 = num3;
						}
						if (settlement.HasPort)
						{
							num3 = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, true, true, MobileParty.NavigationType.All);
							if (num3 < num2)
							{
								num2 = num3;
							}
						}
					}
					float num4 = 20f - MBMath.Map(num2, 0f, maxDistanceOfSettlementsToHomeSettlement, 0f, 20f);
					num += num4;
				}
				else
				{
					num += 20f;
				}
				int num5 = DefaultSettlementValueModel.CalculateTotalProsperity(settlement);
				float num6 = MBMath.Map(MathF.Sqrt(2500f + (float)num5) / 100f, 0.5f, 1f, 0f, 5f);
				num += num6;
				float num7 = MBMath.Map(SettlementHelper.GetNeighborScoreForConsideringClan(settlement, clan), -2f, 1f, -10f, 10f);
				num += num7;
				int num8 = 0;
				for (;;)
				{
					int num9 = num8;
					Kingdom kingdom = clan.Kingdom;
					int? num10 = ((kingdom != null) ? new int?(kingdom.Clans.Count) : null);
					if (!((num9 < num10.GetValueOrDefault()) & (num10 != null)))
					{
						break;
					}
					Clan clan2 = clan.Kingdom.Clans[num8];
					if (clan2 != clan && settlement == clan2.HomeSettlement)
					{
						num -= 10f;
					}
					num8++;
				}
			}
			return num;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00085EFC File Offset: 0x000840FC
		public override Settlement FindMostSuitableHomeSettlement(Clan clan)
		{
			Settlement settlement = null;
			if (Settlement.All != null && Settlement.All.Count != 0 && !clan.IsRebelClan && !clan.IsBanditFaction && clan.MapFaction.Settlements.Count != 0)
			{
				float minValue = float.MinValue;
				float num = DefaultSettlementValueModel.FindFarthestDistanceBetweenSettlementsInClan(clan);
				DefaultSettlementValueModel.TryToFindHomeSettlementForClan(clan, clan.Fiefs.SelectQ<Town, Settlement>((Town x) => x.Settlement), num, out settlement, ref minValue);
				if (minValue < 5120f && clan.Kingdom != null)
				{
					DefaultSettlementValueModel.TryToFindHomeSettlementForClan(clan, clan.Kingdom.Fiefs.SelectQ<Town, Settlement>((Town x) => x.Settlement), num, out settlement, ref minValue);
				}
				if (minValue < 2560f)
				{
					DefaultSettlementValueModel.TryToFindHomeSettlementForClan(clan, Settlement.All, num, out settlement, ref minValue);
				}
				return settlement;
			}
			if (clan == Clan.PlayerClan && clan.InitialHomeSettlement == null)
			{
				return Settlement.All[0];
			}
			return clan.InitialHomeSettlement;
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00086008 File Offset: 0x00084208
		private static void TryToFindHomeSettlementForClan(Clan clanToConsider, IEnumerable<Settlement> settlementsToConsider, float maxDistance, out Settlement homeSettlement, ref float maxScore)
		{
			homeSettlement = null;
			foreach (Settlement settlement in settlementsToConsider)
			{
				if (settlement.IsFortification || settlement.IsVillage || settlement.IsHideout)
				{
					float settlementScoreForBeingHomeSettlementOfClan = DefaultSettlementValueModel.GetSettlementScoreForBeingHomeSettlementOfClan(settlement, clanToConsider, maxDistance);
					if (settlementScoreForBeingHomeSettlementOfClan > maxScore)
					{
						homeSettlement = settlement;
						maxScore = settlementScoreForBeingHomeSettlementOfClan;
					}
				}
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0008607C File Offset: 0x0008427C
		private static float FindFarthestDistanceBetweenSettlementsInClan(Clan clan)
		{
			float num = float.MinValue;
			foreach (Settlement settlement in clan.MapFaction.Settlements)
			{
				if (settlement != clan.MapFaction.FactionMidSettlement)
				{
					float num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, false, false, MobileParty.NavigationType.All);
					if (num2 > num)
					{
						num = num2;
					}
					if (settlement.HasPort)
					{
						num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, false, true, MobileParty.NavigationType.All);
						if (num2 > num)
						{
							num = num2;
						}
					}
					if (clan.MapFaction.FactionMidSettlement.HasPort)
					{
						num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, true, false, MobileParty.NavigationType.All);
						if (num2 > num)
						{
							num = num2;
						}
						if (settlement.HasPort)
						{
							num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(clan.MapFaction.FactionMidSettlement, settlement, true, true, MobileParty.NavigationType.All);
							if (num2 > num)
							{
								num = num2;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x000861B8 File Offset: 0x000843B8
		private static int CalculateTotalProsperity(Settlement settlement)
		{
			int num = 0;
			if (settlement.IsFortification)
			{
				num = (int)settlement.Town.Prosperity;
				using (List<Village>.Enumerator enumerator = settlement.BoundVillages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Village village = enumerator.Current;
						num += (int)village.Hearth;
					}
					return num;
				}
			}
			if (settlement.IsVillage)
			{
				num = (int)settlement.Village.Hearth;
			}
			return num;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0008623C File Offset: 0x0008443C
		public override float CalculateSettlementBaseValue(Settlement settlement)
		{
			float num = (settlement.IsCastle ? 1.25f : 1f);
			float value = settlement.GetValue(null, true);
			float baseGeographicalAdvantage = DefaultSettlementValueModel.GetBaseGeographicalAdvantage(settlement.IsVillage ? settlement.Village.Bound : settlement);
			return num * value * baseGeographicalAdvantage * 0.33f;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0008628C File Offset: 0x0008448C
		public override float CalculateSettlementValueForFaction(Settlement settlement, IFaction faction)
		{
			float num = (settlement.IsCastle ? 1.25f : 1f);
			float num2 = ((settlement.MapFaction == faction.MapFaction) ? 1.1f : 1f);
			float num3 = ((settlement.Culture == ((faction != null) ? faction.Culture : null)) ? 1.1f : 1f);
			float value = settlement.GetValue(null, true);
			float num4 = DefaultSettlementValueModel.GeographicalAdvantageForFaction(settlement.IsVillage ? settlement.Village.Bound : settlement, faction);
			float num5 = 1f;
			if (settlement.HasPort && settlement.IsFortification)
			{
				num5 = 1.2f;
				if (!faction.Settlements.Any<Settlement>((Settlement x) => x.HasPort))
				{
					num5 *= 1.4f;
				}
			}
			return value * num * num2 * num3 * num4 * num5 * 0.33f;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00086374 File Offset: 0x00084574
		public override float CalculateSettlementValueForEnemyHero(Settlement settlement, Hero hero)
		{
			float num = (settlement.IsCastle ? 1.25f : 1f);
			float num2 = ((settlement.OwnerClan == hero.Clan) ? 1.1f : 1f);
			float num3 = ((settlement.Culture == hero.Culture) ? 1.1f : 1f);
			float value = settlement.GetValue(null, true);
			float num4 = DefaultSettlementValueModel.GeographicalAdvantageForFaction(settlement.IsVillage ? settlement.Village.Bound : settlement, hero.MapFaction);
			float num5 = 1f;
			if (settlement.HasPort && settlement.IsFortification)
			{
				num5 = 1.2f;
				if (!hero.Clan.Settlements.Any<Settlement>((Settlement x) => x.HasPort))
				{
					num5 *= 1.4f;
				}
			}
			return value * num * num3 * num2 * num4 * num5 * 0.33f;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00086460 File Offset: 0x00084660
		private static float GetBaseGeographicalAdvantage(Settlement settlement)
		{
			float num = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement.MapFaction.FactionMidSettlement, settlement, false, false, MobileParty.NavigationType.All) / Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All);
			return 1f / (1f + num);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x000864AC File Offset: 0x000846AC
		private static float GeographicalAdvantageForFaction(Settlement settlement, IFaction faction)
		{
			Settlement factionMidSettlement = faction.FactionMidSettlement;
			float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(settlement, factionMidSettlement, false, false, MobileParty.NavigationType.All);
			if (faction.FactionMidSettlement.MapFaction != faction)
			{
				return MathF.Clamp(Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) / (distance + 0.1f), 0f, 4f);
			}
			float distanceToClosestNonAllyFortification = faction.DistanceToClosestNonAllyFortification;
			if (settlement.MapFaction == faction && distance < distanceToClosestNonAllyFortification)
			{
				return MathF.Clamp(Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All) / (distanceToClosestNonAllyFortification - distance), 1f, 4f);
			}
			float num = (distance - distanceToClosestNonAllyFortification) / Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.All);
			return 1f / (1f + num);
		}

		// Token: 0x040008D7 RID: 2263
		private const float BenefitRatioForFaction = 0.33f;

		// Token: 0x040008D8 RID: 2264
		private const float CastleMultiplier = 1.25f;

		// Token: 0x040008D9 RID: 2265
		private const float SameMapFactionMultiplier = 1.1f;

		// Token: 0x040008DA RID: 2266
		private const float SameCultureMultiplier = 1.1f;

		// Token: 0x040008DB RID: 2267
		private const float BeingOwnerMultiplier = 1.1f;

		// Token: 0x040008DC RID: 2268
		private const float HavingNoCoastalSettlementMultiplier = 1.4f;

		// Token: 0x040008DD RID: 2269
		private const float HavingPortMultiplier = 1.2f;

		// Token: 0x040008DE RID: 2270
		private const int SettlementAtWarWithClan = 10240;

		// Token: 0x040008DF RID: 2271
		private const int HomeSettlementToOtherClanScore = 10;

		// Token: 0x040008E0 RID: 2272
		private const int AlreadyOwnerClanScoreForHomeSettlement = 5120;

		// Token: 0x040008E1 RID: 2273
		private const int SameFactionWithClanScoreForHomeSettlement = 2560;

		// Token: 0x040008E2 RID: 2274
		private const int SettlementTypeScoreForHomeSettlementTown = 1280;

		// Token: 0x040008E3 RID: 2275
		private const int SettlementTypeScoreForHomeSettlementCastle = 640;

		// Token: 0x040008E4 RID: 2276
		private const int SettlementTypeScoreForHomeSettlementVillage = 320;

		// Token: 0x040008E5 RID: 2277
		private const int MidSettlementDistanceScoreForHomeSettlement = 20;

		// Token: 0x040008E6 RID: 2278
		private const int SameCultureWithClanCultureScoreForHomeSettlement = 17;

		// Token: 0x040008E7 RID: 2279
		private const float AlreadyHomeSettlementScoreForHomeSettlement = 4.5f;

		// Token: 0x040008E8 RID: 2280
		private const float InitialHomeSettlementScoreForHomeSettlement = 3.5f;

		// Token: 0x040008E9 RID: 2281
		private const int SettlementOwnerClanCultureSameForHomeSettlement = 12;

		// Token: 0x040008EA RID: 2282
		private const int NeighborScoreForHomeSettlement = 10;

		// Token: 0x040008EB RID: 2283
		private const int ProsperityScoreForHomeSettlement = 5;
	}
}
