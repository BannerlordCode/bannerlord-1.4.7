using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors
{
	// Token: 0x02000469 RID: 1129
	public class ItemBarterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004864 RID: 18532 RVA: 0x0016CAFD File Offset: 0x0016ACFD
		public override void RegisterEvents()
		{
			CampaignEvents.BarterablesRequested.AddNonSerializedListener(this, new Action<BarterData>(this.CheckForBarters));
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0016CB16 File Offset: 0x0016AD16
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0016CB18 File Offset: 0x0016AD18
		public void CheckForBarters(BarterData args)
		{
			CampaignVec2 campaignVec;
			if (args.OffererHero != null)
			{
				campaignVec = args.OffererHero.GetCampaignPosition();
			}
			else if (args.OffererParty != null)
			{
				campaignVec = args.OffererParty.MobileParty.Position;
			}
			else
			{
				campaignVec = args.OtherHero.GetCampaignPosition();
			}
			if (campaignVec.IsValid())
			{
				List<Settlement> closestSettlements = this._distanceCache.GetClosestSettlements(campaignVec.ToVec2());
				if (args.OffererParty != null && args.OtherParty != null)
				{
					for (int i = 0; i < args.OffererParty.ItemRoster.Count; i++)
					{
						ItemRosterElement elementCopyAtIndex = args.OffererParty.ItemRoster.GetElementCopyAtIndex(i);
						if (elementCopyAtIndex.Amount > 0 && elementCopyAtIndex.EquipmentElement.GetBaseValue() > 100)
						{
							int num = this.CalculateAverageItemValueInNearbySettlements(elementCopyAtIndex.EquipmentElement, args.OffererParty, closestSettlements);
							Barterable barterable = new ItemBarterable(args.OffererHero, args.OtherHero, args.OffererParty, args.OtherParty, elementCopyAtIndex, num);
							args.AddBarterable<ItemBarterGroup>(barterable, false);
						}
					}
					for (int j = 0; j < args.OtherParty.ItemRoster.Count; j++)
					{
						ItemRosterElement elementCopyAtIndex2 = args.OtherParty.ItemRoster.GetElementCopyAtIndex(j);
						if (elementCopyAtIndex2.Amount > 0 && elementCopyAtIndex2.EquipmentElement.GetBaseValue() > 100)
						{
							int num2 = this.CalculateAverageItemValueInNearbySettlements(elementCopyAtIndex2.EquipmentElement, args.OtherParty, closestSettlements);
							Barterable barterable2 = new ItemBarterable(args.OtherHero, args.OffererHero, args.OtherParty, args.OffererParty, elementCopyAtIndex2, num2);
							args.AddBarterable<ItemBarterGroup>(barterable2, false);
						}
					}
				}
			}
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x0016CCBC File Offset: 0x0016AEBC
		private int CalculateAverageItemValueInNearbySettlements(EquipmentElement itemRosterElement, PartyBase involvedParty, List<Settlement> nearbySettlements)
		{
			int num = 0;
			if (!nearbySettlements.IsEmpty<Settlement>())
			{
				foreach (Settlement settlement in nearbySettlements)
				{
					num += settlement.Town.GetItemPrice(itemRosterElement, involvedParty.MobileParty, true);
				}
				num /= nearbySettlements.Count;
			}
			return num;
		}

		// Token: 0x04001406 RID: 5126
		private const int ItemValueThreshold = 100;

		// Token: 0x04001407 RID: 5127
		private ItemBarterBehavior.SettlementDistanceCache _distanceCache = new ItemBarterBehavior.SettlementDistanceCache();

		// Token: 0x0200087C RID: 2172
		private class SettlementDistanceCache
		{
			// Token: 0x060068A4 RID: 26788 RVA: 0x001CA9CD File Offset: 0x001C8BCD
			public SettlementDistanceCache()
			{
				this._latestHeroPosition = new Vec2(-1f, -1f);
				this._sortedSettlements = new List<ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair>(64);
				this._closestSettlements = new List<Settlement>(3);
			}

			// Token: 0x060068A5 RID: 26789 RVA: 0x001CAA04 File Offset: 0x001C8C04
			public List<Settlement> GetClosestSettlements(Vec2 position)
			{
				if (!position.NearlyEquals(this._latestHeroPosition, 1E-05f))
				{
					this._latestHeroPosition = position;
					MBReadOnlyList<Town> allTowns = Campaign.Current.AllTowns;
					int count = allTowns.Count;
					for (int i = 0; i < count; i++)
					{
						Settlement settlement = allTowns[i].Settlement;
						this._sortedSettlements.Add(new ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair(position.DistanceSquared(settlement.Position.ToVec2()), settlement));
					}
					this._sortedSettlements.Sort();
					this._closestSettlements.Clear();
					this._closestSettlements.Add(this._sortedSettlements[0].Settlement);
					this._closestSettlements.Add(this._sortedSettlements[1].Settlement);
					this._closestSettlements.Add(this._sortedSettlements[2].Settlement);
					this._sortedSettlements.Clear();
				}
				return this._closestSettlements;
			}

			// Token: 0x0400245C RID: 9308
			private Vec2 _latestHeroPosition;

			// Token: 0x0400245D RID: 9309
			private List<ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair> _sortedSettlements;

			// Token: 0x0400245E RID: 9310
			private List<Settlement> _closestSettlements;

			// Token: 0x02000902 RID: 2306
			private struct SettlementDistancePair : IComparable<ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair>
			{
				// Token: 0x06006A2C RID: 27180 RVA: 0x001CC97F File Offset: 0x001CAB7F
				public SettlementDistancePair(float distance, Settlement settlement)
				{
					this._distance = distance;
					this.Settlement = settlement;
				}

				// Token: 0x06006A2D RID: 27181 RVA: 0x001CC98F File Offset: 0x001CAB8F
				public int CompareTo(ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair other)
				{
					if (this._distance == other._distance)
					{
						return 0;
					}
					if (this._distance > other._distance)
					{
						return 1;
					}
					return -1;
				}

				// Token: 0x040025DA RID: 9690
				private float _distance;

				// Token: 0x040025DB RID: 9691
				public Settlement Settlement;
			}
		}
	}
}
