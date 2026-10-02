using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F2 RID: 1010
	public class GarrisonRecruitmentCampaignBehavior : CampaignBehaviorBase, IGarrisonRecruitmentBehavior
	{
		// Token: 0x06003F71 RID: 16241 RVA: 0x0011EF15 File Offset: 0x0011D115
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnDailySettlementTick));
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x0011EF2E File Offset: 0x0011D12E
		private static CharacterObject GetBasicTroopForTown(Town town)
		{
			return town.MapFaction.BasicTroop;
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x0011EF3C File Offset: 0x0011D13C
		private void OnDailySettlementTick(Settlement settlement)
		{
			if (settlement.IsFortification)
			{
				Town town = settlement.Town;
				if (this.SettlementCheckGarrisonChangeCommonCondition(settlement))
				{
					this.TickGarrisonChangeForTown(town);
				}
				if (this.CanSettlementAutoRecruit(settlement))
				{
					this.TickAutoRecruitmentGarrisonChange(town);
				}
				if (town.GarrisonParty != null)
				{
					this.HandleGarrisonXpChange(town);
				}
			}
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x0011EF88 File Offset: 0x0011D188
		private void TickAutoRecruitmentGarrisonChange(Town town)
		{
			float resultNumber = this.GetAutoRecruitmentGarrisonChangeExplainedNumber(town).ResultNumber;
			if (resultNumber > 0f)
			{
				if (town.GarrisonParty == null)
				{
					town.Owner.Settlement.AddGarrisonParty();
				}
				int num = 0;
				while ((float)num < resultNumber)
				{
					GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop = this._volunteerListCache.ElementAt<GarrisonRecruitmentCampaignBehavior.VolunteerTroop>(num);
					Hero ownerNotable = volunteerTroop.OwnerNotable;
					int notableVolunteerArrayIndex = volunteerTroop.NotableVolunteerArrayIndex;
					town.GarrisonParty.MemberRoster.AddToCounts(ownerNotable.VolunteerTypes[notableVolunteerArrayIndex], 1, false, 0, 0, true, -1);
					town.Settlement.OwnerClan.AutoRecruitmentExpenses += Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(ownerNotable.VolunteerTypes[notableVolunteerArrayIndex], town.Settlement.OwnerClan.Leader, false).RoundedResultNumber;
					ownerNotable.VolunteerTypes[notableVolunteerArrayIndex] = null;
					num++;
				}
			}
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x0011F06C File Offset: 0x0011D26C
		private void TickGarrisonChangeForTown(Town town)
		{
			int num = (int)this.GetBaseGarrisonChangeExplainedNumber(town, false).ResultNumber;
			if (num > 0)
			{
				if (town.GarrisonParty == null)
				{
					town.Owner.Settlement.AddGarrisonParty();
				}
				town.GarrisonParty.MemberRoster.AddToCounts(GarrisonRecruitmentCampaignBehavior.GetBasicTroopForTown(town), num, false, 0, 0, true, -1);
			}
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x0011F0C4 File Offset: 0x0011D2C4
		private void HandleGarrisonXpChange(Town town)
		{
			int num = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateDailyTroopXpBonus(town);
			float num2 = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateGarrisonXpBonusMultiplier(town);
			if (num > 0)
			{
				foreach (TroopRosterElement troopRosterElement in town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					town.GarrisonParty.MemberRoster.AddXpToTroop(troopRosterElement.Character, MathF.Round((float)num * num2 * (float)troopRosterElement.Number));
				}
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x0011F174 File Offset: 0x0011D374
		private void RepopulateVolunteerListCache(Town town)
		{
			this._volunteerListCache.Clear();
			foreach (Hero hero in town.Settlement.Notables)
			{
				if (hero.IsAlive)
				{
					int num = Campaign.Current.Models.VolunteerModel.MaximumIndexGarrisonCanRecruitFromHero(town.Settlement, hero);
					for (int i = 0; i < num; i++)
					{
						if (hero.VolunteerTypes[i] != null)
						{
							GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop = new GarrisonRecruitmentCampaignBehavior.VolunteerTroop(hero, i);
							this._volunteerListCache.Add(volunteerTroop);
						}
					}
				}
			}
			foreach (Village village in town.Settlement.BoundVillages)
			{
				if (village.VillageState == Village.VillageStates.Normal)
				{
					foreach (Hero hero2 in village.Settlement.Notables)
					{
						if (hero2.IsAlive)
						{
							int num2 = Campaign.Current.Models.VolunteerModel.MaximumIndexGarrisonCanRecruitFromHero(town.Settlement, hero2);
							for (int j = 0; j < num2; j++)
							{
								if (hero2.VolunteerTypes[j] != null)
								{
									GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop2 = new GarrisonRecruitmentCampaignBehavior.VolunteerTroop(hero2, j);
									this._volunteerListCache.Add(volunteerTroop2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0011F314 File Offset: 0x0011D514
		private ExplainedNumber GetAutoRecruitmentGarrisonChangeExplainedNumber(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, true, null);
			this.RepopulateVolunteerListCache(town);
			MobileParty garrisonParty = town.GarrisonParty;
			int num = ((garrisonParty != null) ? garrisonParty.GetAvailableWageBudget() : town.Settlement.GarrisonWagePaymentLimit);
			if (num > 0)
			{
				int num2 = 0;
				int num3 = 0;
				int count = this._volunteerListCache.Count;
				explainedNumber.Add((float)count, new TextObject("{=Uzsnek6O}Auto Recruitment", null), null);
				foreach (GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop in this._volunteerListCache)
				{
					num2 += volunteerTroop.Wage;
					if (num2 >= num)
					{
						break;
					}
					num3++;
				}
				if ((float)num3 < explainedNumber.LimitMaxValue)
				{
					explainedNumber.LimitMax((float)num3, new TextObject("{=7GJOWuUO}Wage Limit", null));
				}
				int num4 = ((town.GarrisonParty == null) ? ((int)Campaign.Current.Models.PartySizeLimitModel.CalculateGarrisonPartySizeLimit(town.Settlement, false).ResultNumber) : (town.GarrisonParty.Party.PartySizeLimit - town.GarrisonParty.Party.NumberOfAllMembers));
				if ((float)num4 < explainedNumber.LimitMaxValue)
				{
					explainedNumber.LimitMax((float)num4, new TextObject("{=mp68RYnD}Party Size Limit", null));
				}
				int maximumDailyAutoRecruitmentCount = Campaign.Current.Models.SettlementGarrisonModel.GetMaximumDailyAutoRecruitmentCount(town);
				if ((float)maximumDailyAutoRecruitmentCount < explainedNumber.LimitMaxValue)
				{
					explainedNumber.LimitMax((float)maximumDailyAutoRecruitmentCount, new TextObject("{=91fnSU2A}Maximum Auto Recruitment", null));
				}
			}
			return explainedNumber;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0011F4A4 File Offset: 0x0011D6A4
		private ExplainedNumber GetBaseGarrisonChangeExplainedNumber(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber explainedNumber = Campaign.Current.Models.SettlementGarrisonModel.CalculateBaseGarrisonChange(town.Settlement, includeDescriptions);
			int num = ((town.GarrisonParty == null) ? ((int)Campaign.Current.Models.PartySizeLimitModel.CalculateGarrisonPartySizeLimit(town.Settlement, false).ResultNumber) : (town.GarrisonParty.Party.PartySizeLimit - town.GarrisonParty.Party.NumberOfAllMembers));
			if (explainedNumber.LimitMaxValue > (float)num)
			{
				explainedNumber.LimitMax((float)num, new TextObject("{=mp68RYnD}Party Size Limit", null));
			}
			int characterWage = Campaign.Current.Models.PartyWageModel.GetCharacterWage(GarrisonRecruitmentCampaignBehavior.GetBasicTroopForTown(town));
			MobileParty garrisonParty = town.GarrisonParty;
			int num2 = ((garrisonParty != null) ? garrisonParty.GetAvailableWageBudget() : town.Settlement.GarrisonWagePaymentLimit) / characterWage;
			if (explainedNumber.LimitMaxValue > (float)num2)
			{
				explainedNumber.LimitMax((float)num2, new TextObject("{=7GJOWuUO}Wage Limit", null));
			}
			return explainedNumber;
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0011F598 File Offset: 0x0011D798
		public ExplainedNumber GetGarrisonChangeExplainedNumber(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, true, null);
			ExplainedNumber baseGarrisonChangeExplainedNumber = this.GetBaseGarrisonChangeExplainedNumber(town, true);
			explainedNumber.AddFromExplainedNumber(baseGarrisonChangeExplainedNumber, new TextObject("{=basevalue}Base", null));
			if (this.CanSettlementAutoRecruit(town.Settlement))
			{
				ExplainedNumber autoRecruitmentGarrisonChangeExplainedNumber = this.GetAutoRecruitmentGarrisonChangeExplainedNumber(town);
				explainedNumber.AddFromExplainedNumber(autoRecruitmentGarrisonChangeExplainedNumber, new TextObject("{=Uzsnek6O}Auto Recruitment", null));
			}
			return explainedNumber;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0011F5F9 File Offset: 0x0011D7F9
		private bool CanSettlementAutoRecruit(Settlement settlement)
		{
			return settlement.Town.GarrisonAutoRecruitmentIsEnabled && settlement.Town.FoodChange > 0f && this.SettlementCheckGarrisonChangeCommonCondition(settlement);
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0011F623 File Offset: 0x0011D823
		private bool SettlementCheckGarrisonChangeCommonCondition(Settlement settlement)
		{
			return settlement.Party.MapEvent == null && settlement.Party.SiegeEvent == null;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0011F642 File Offset: 0x0011D842
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x040012F0 RID: 4848
		private SortedSet<GarrisonRecruitmentCampaignBehavior.VolunteerTroop> _volunteerListCache = new SortedSet<GarrisonRecruitmentCampaignBehavior.VolunteerTroop>();

		// Token: 0x02000801 RID: 2049
		public struct VolunteerTroop : IComparable
		{
			// Token: 0x06006478 RID: 25720 RVA: 0x001C4065 File Offset: 0x001C2265
			public VolunteerTroop(Hero ownerNotable, int notableVolunteerArrayIndex)
			{
				this.OwnerNotable = ownerNotable;
				this.NotableVolunteerArrayIndex = notableVolunteerArrayIndex;
				this.Wage = Campaign.Current.Models.PartyWageModel.GetCharacterWage(ownerNotable.VolunteerTypes[notableVolunteerArrayIndex]);
			}

			// Token: 0x06006479 RID: 25721 RVA: 0x001C4098 File Offset: 0x001C2298
			public int CompareTo(object obj)
			{
				GarrisonRecruitmentCampaignBehavior.VolunteerTroop volunteerTroop = (GarrisonRecruitmentCampaignBehavior.VolunteerTroop)obj;
				int num = this.Wage.CompareTo(volunteerTroop.Wage);
				if (num == 0)
				{
					num = volunteerTroop.NotableVolunteerArrayIndex.CompareTo(this.NotableVolunteerArrayIndex);
				}
				if (num == 0)
				{
					num = volunteerTroop.OwnerNotable.Id.CompareTo(this.OwnerNotable.Id);
				}
				return num;
			}

			// Token: 0x04002051 RID: 8273
			public Hero OwnerNotable;

			// Token: 0x04002052 RID: 8274
			public int NotableVolunteerArrayIndex;

			// Token: 0x04002053 RID: 8275
			public int Wage;
		}
	}
}
