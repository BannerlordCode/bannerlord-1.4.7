using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E6 RID: 998
	public class DesertersCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x0010E352 File Offset: 0x0010C552
		public static int MergePartiesMaxSize
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x0010E356 File Offset: 0x0010C556
		private float DesertersSpawnRadiusAroundVillages
		{
			get
			{
				return 0.2f * Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0010E36F File Offset: 0x0010C56F
		private Clan DeserterClan
		{
			get
			{
				if (this._deserterClan == null)
				{
					this._deserterClan = Clan.FindFirst((Clan x) => x.StringId == "deserters");
				}
				return this._deserterClan;
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0010E3A9 File Offset: 0x0010C5A9
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0010E3DC File Offset: 0x0010C5DC
		private void HourlyTickParty(MobileParty party)
		{
			if (this.IsDeserterParty(party) && this.CanPartyMerge(party) && party.MemberRoster.TotalRegulars < DesertersCampaignBehavior.MergePartiesMaxSize)
			{
				LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(party.Position.ToVec2(), this.GetMergeDistance(party));
				for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
				{
					if (this.IsDeserterParty(mobileParty) && mobileParty != party && this.CanPartyMerge(mobileParty) && mobileParty.MemberRoster.TotalRegulars + party.MemberRoster.TotalRegulars <= DesertersCampaignBehavior.MergePartiesMaxSize && MBRandom.RandomFloat < 0.05f)
					{
						this.MergeParties(party, mobileParty);
						return;
					}
				}
			}
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x0010E48C File Offset: 0x0010C68C
		private bool CanPartyMerge(MobileParty mobileParty)
		{
			return mobileParty.IsActive && mobileParty.MapEvent == null && !mobileParty.IsCurrentlyUsedByAQuest && !mobileParty.IsCurrentlyEngagingParty && !mobileParty.IsFleeing();
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0010E4BC File Offset: 0x0010C6BC
		private void MergeParties(MobileParty party, MobileParty nearbyParty)
		{
			Debug.Print(string.Format("Deserter parties {0} of {1} and {2} of {3} merged.", new object[]
			{
				party.StringId,
				party.MemberRoster.TotalManCount,
				nearbyParty.StringId,
				nearbyParty.MemberRoster.TotalManCount
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			party.MemberRoster.Add(nearbyParty.MemberRoster);
			foreach (TroopRosterElement troopRosterElement in nearbyParty.PrisonRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.HeroObject != null)
				{
					TransferPrisonerAction.Apply(troopRosterElement.Character, nearbyParty.Party, party.Party);
				}
			}
			if (party.PrisonRoster.Count > 0)
			{
				party.PrisonRoster.Add(nearbyParty.PrisonRoster);
			}
			party.PartyTradeGold += nearbyParty.PartyTradeGold;
			party.ItemRoster.Add(nearbyParty.ItemRoster);
			DestroyPartyAction.Apply(null, nearbyParty);
			PartyBaseHelper.SortRoster(party);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0010E5EC File Offset: 0x0010C7EC
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsNavalMapEvent && (mapEvent.IsFieldBattle || mapEvent.IsSiegeAssault || mapEvent.IsSiegeOutside || mapEvent.IsSallyOut) && mapEvent.HasWinner && this.DeserterClan != null && this.DeserterClan.WarPartyComponents.Count < Campaign.Current.Models.BanditDensityModel.GetMaxSupportedNumberOfLootersForClan(this.DeserterClan))
			{
				MapEventSide mapEventSide = mapEvent.GetMapEventSide(mapEvent.DefeatedSide);
				TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
				foreach (MapEventParty mapEventParty in mapEventSide.Parties)
				{
					if (this.CanPartyGenerateDeserters(mapEventParty))
					{
						troopRoster.Add(mapEventParty.RoutedInBattle);
						troopRoster.Add(mapEventParty.DiedInBattle);
					}
				}
				if (MBRandom.RandomFloat < 0.9f)
				{
					troopRoster.RemoveIf((TroopRosterElement x) => x.Character.IsHero);
					if (troopRoster.TotalManCount >= 15)
					{
						this.TrySpawnDeserters(mapEvent, troopRoster);
					}
				}
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0010E724 File Offset: 0x0010C924
		private bool CanPartyGenerateDeserters(MapEventParty mapEventParty)
		{
			return mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsLordParty && mapEventParty.Party.MobileParty.ActualClan != null && !mapEventParty.Party.MobileParty.ActualClan.IsMinorFaction;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0010E77C File Offset: 0x0010C97C
		private void TrySpawnDeserters(MapEvent mapEvent, TroopRoster routedTroops)
		{
			int maxDeserterPartyCountForMapEvent = this.GetMaxDeserterPartyCountForMapEvent(mapEvent);
			List<TroopRoster> rostersSuitableForDeserters = this.GetRostersSuitableForDeserters(routedTroops, maxDeserterPartyCountForMapEvent);
			List<Settlement> list = this.SelectRandomSettlementsForDeserters(mapEvent, rostersSuitableForDeserters.Count);
			for (int i = 0; i < rostersSuitableForDeserters.Count; i++)
			{
				this.SpawnDesertersParty(mapEvent, rostersSuitableForDeserters[i], list[i]);
			}
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0010E7D0 File Offset: 0x0010C9D0
		private int GetMaxDeserterPartyCountForMapEvent(MapEvent mapEvent)
		{
			bool flag = mapEvent.AttackerSide.Parties.Any<MapEventParty>((MapEventParty x) => this.CanPartyGenerateDeserters(x) && x.Party.MobileParty.Army != null && (x.Party.MobileParty.AttachedTo != null || x.Party.MobileParty.Army.LeaderParty == x.Party.MobileParty));
			bool flag2 = mapEvent.DefenderSide.Parties.Any<MapEventParty>((MapEventParty x) => this.CanPartyGenerateDeserters(x) && x.Party.MobileParty.Army != null && (x.Party.MobileParty.AttachedTo != null || x.Party.MobileParty.Army.LeaderParty == x.Party.MobileParty));
			if (flag && flag2)
			{
				return 5;
			}
			return 3;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0010E820 File Offset: 0x0010CA20
		private List<TroopRoster> GetRostersSuitableForDeserters(TroopRoster routedTroops, int maxPartyCount)
		{
			int totalManCount = routedTroops.TotalManCount;
			int maxSupportedNumberOfLootersForClan = Campaign.Current.Models.BanditDensityModel.GetMaxSupportedNumberOfLootersForClan(this.DeserterClan);
			int num = Math.Min(maxPartyCount, maxSupportedNumberOfLootersForClan - this.DeserterClan.WarPartyComponents.Count);
			int num2 = totalManCount / 15;
			int num3 = Math.Min(num, num2);
			List<TroopRoster> list = new List<TroopRoster>();
			for (int i = 0; i < num3; i++)
			{
				list.Add(routedTroops.RemoveNumberOfNonHeroTroopsRandomly(Math.Min(routedTroops.TotalManCount / (num3 - i), 40)));
			}
			return list;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0010E8AB File Offset: 0x0010CAAB
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x0010E8B0 File Offset: 0x0010CAB0
		private void SpawnDesertersParty(MapEvent mapEvent, TroopRoster troops, Settlement settlement)
		{
			CampaignVec2 deserterSpawnPosition = this.GetDeserterSpawnPosition(settlement);
			MobileParty mobileParty = BanditPartyComponent.CreateLooterParty(this.DeserterClan.StringId + "_1", this.DeserterClan, settlement, false, null, deserterSpawnPosition);
			mobileParty.MemberRoster.Add(troops);
			this.InitializeDeserterParty(mobileParty);
			mobileParty.SetMovePatrolAroundPoint(mobileParty.Position, MobileParty.NavigationType.Default);
			PartyBaseHelper.SortRoster(mobileParty);
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x0010E910 File Offset: 0x0010CB10
		private List<Settlement> SelectRandomSettlementsForDeserters(MapEvent mapEvent, int count)
		{
			CampaignVec2 campaignVec = mapEvent.Position;
			List<Settlement> list = DesertersCampaignBehavior.FindSettlementsAroundPoint(in campaignVec, (Settlement x) => x.IsVillage, MobileParty.NavigationType.Default, this.GetMaxVillageDistance());
			if (list.Count > count)
			{
				list.Shuffle<Settlement>();
				return list.Take<Settlement>(count).ToList<Settlement>();
			}
			if (list.Count == 0)
			{
				List<Settlement> list2 = list;
				campaignVec = mapEvent.Position;
				list2.Add(SettlementHelper.FindNearestSettlementToPoint(in campaignVec, (Settlement x) => x.IsVillage));
			}
			int count2 = list.Count;
			for (int i = 0; i < count - count2; i++)
			{
				list.Add(list[MBRandom.RandomInt(0, count2 - 1)]);
			}
			return list;
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0010E9D4 File Offset: 0x0010CBD4
		private static List<Settlement> FindSettlementsAroundPoint(in CampaignVec2 point, Func<Settlement, bool> condition, MobileParty.NavigationType navCapabilities, float maxDistance)
		{
			List<Settlement> list = new List<Settlement>();
			foreach (Settlement settlement in Settlement.All)
			{
				if ((condition == null || condition(settlement)) && settlement.Position.Distance(point) < maxDistance)
				{
					list.Add(settlement);
				}
			}
			return list;
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0010EA50 File Offset: 0x0010CC50
		private float GetMaxVillageDistance()
		{
			return Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay / 2f;
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0010EA6C File Offset: 0x0010CC6C
		private CampaignVec2 GetDeserterSpawnPosition(Settlement settlement)
		{
			CampaignVec2 campaignVec = NavigationHelper.FindPointAroundPosition(settlement.GatePosition, MobileParty.NavigationType.Default, this.DesertersSpawnRadiusAroundVillages, 0f, true, false);
			float num = MobileParty.MainParty.SeeingRange * MobileParty.MainParty.SeeingRange;
			if (campaignVec.DistanceSquared(MobileParty.MainParty.Position) < num)
			{
				for (int i = 0; i < 15; i++)
				{
					CampaignVec2 campaignVec2 = NavigationHelper.FindReachablePointAroundPosition(campaignVec, MobileParty.NavigationType.Default, this.DesertersSpawnRadiusAroundVillages, 0f, false);
					if (NavigationHelper.IsPositionValidForNavigationType(campaignVec2, MobileParty.NavigationType.Default))
					{
						float num3;
						float num2 = DistanceHelper.FindClosestDistanceFromMobilePartyToPoint(MobileParty.MainParty, campaignVec2, MobileParty.NavigationType.Default, out num3);
						if (num2 * num2 > num)
						{
							campaignVec = campaignVec2;
							break;
						}
					}
				}
			}
			return campaignVec;
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0010EAFF File Offset: 0x0010CCFF
		private void InitializeDeserterParty(MobileParty banditParty)
		{
			banditParty.Party.SetVisualAsDirty();
			banditParty.ActualClan = this.DeserterClan;
			banditParty.Aggressiveness = 1f - 0.2f * MBRandom.RandomFloat;
			DesertersCampaignBehavior.CreatePartyTrade(banditParty);
			this.GiveFoodToBanditParty(banditParty);
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0010EB3C File Offset: 0x0010CD3C
		private static void CreatePartyTrade(MobileParty banditParty)
		{
			int num = (int)(10f * (float)banditParty.Party.MemberRoster.TotalManCount * (0.5f + 1f * MBRandom.RandomFloat));
			banditParty.InitializePartyTrade(num);
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x0010EB7C File Offset: 0x0010CD7C
		private void GiveFoodToBanditParty(MobileParty banditParty)
		{
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.IsFood)
				{
					int num = MBRandom.RoundRandomized((float)banditParty.MemberRoster.TotalManCount * (1f / (float)itemObject.Value) * 8f * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat);
					if (num > 0)
					{
						banditParty.ItemRoster.AddToCounts(itemObject, num);
					}
				}
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x0010EC20 File Offset: 0x0010CE20
		private float GetMergeDistance(MobileParty mobileParty)
		{
			return mobileParty._lastCalculatedSpeed * 2f;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x0010EC2E File Offset: 0x0010CE2E
		private bool IsDeserterParty(MobileParty mobileParty)
		{
			return mobileParty.ActualClan != null && mobileParty.ActualClan == this.DeserterClan;
		}

		// Token: 0x040012BF RID: 4799
		public const int MinimumDeserterPartyCount = 15;

		// Token: 0x040012C0 RID: 4800
		public const int MaximumDeserterPartyCount = 40;

		// Token: 0x040012C1 RID: 4801
		private const int MaxDeserterPartyCountAfterBattle = 3;

		// Token: 0x040012C2 RID: 4802
		private const int MaxDeserterPartyCountAfterArmyBattle = 5;

		// Token: 0x040012C3 RID: 4803
		private Clan _deserterClan;
	}
}
